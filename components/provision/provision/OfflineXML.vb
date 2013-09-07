Imports System.Xml
Imports System.Xml.Linq
Imports System.IO
Imports System.Threading

Public Enum eSyncEventType
    NewNode = 0
    EditNode = 1
    DeleteNode = 2
    BeginDownload = 3
    EndDownload = 4
    EndSync = 5
End Enum

Public Class OfflineXML

    Private SyncObjectList As New List(Of SyncObject)
    Private WithEvents PostTimer As New System.Windows.Forms.Timer
    Private posting As Boolean = False

#Region "Public Properties"

    Private _UrlGetParams As New Dictionary(Of String, String)
    Public Property UrlGetParams() As Dictionary(Of String, String)
        Get
            Return _UrlGetParams
        End Get
        Set(ByVal value As Dictionary(Of String, String))
            _UrlGetParams = value
        End Set
    End Property

    Private _UserEnv As UserEnv
    Public ReadOnly Property ThisUserEnv() As UserEnv
        Get
            Return _UserEnv
        End Get
    End Property

    Private _Loaded As Boolean = False
    Public Property Loaded() As Boolean
        Get
            Return _Loaded
        End Get
        Set(ByVal value As Boolean)
            _Loaded = value
        End Set
    End Property

    Private _LocalFile As String
    Public Property LocalFile() As String
        Get
            Return _LocalFile
        End Get
        Set(ByVal value As String)
            _LocalFile = value
        End Set
    End Property

    Private _FileURL As String
    Public Property FileURL() As String
        Get
            Dim fn As String = _FileURL
            For Each k As String In UrlGetParams.Keys
                fn += String.Format("&{0}={1}", k, UrlGetParams(k))
            Next
            Return fn
        End Get
        Set(ByVal value As String)
            _FileURL = value
        End Set
    End Property

    Private _Document As New XmlDocument
    Public Property Document() As XmlDocument
        Get
            Return _Document
        End Get
        Set(ByVal value As XmlDocument)
            _Document = value
        End Set
    End Property

    Private _syncHandler As System.EventHandler = Nothing
    Public Property syncHandler() As System.EventHandler
        Get
            Return _syncHandler
        End Get
        Set(ByVal value As System.EventHandler)
            _syncHandler = value
        End Set
    End Property

    Private _CurrentPath As String = ""
    Public ReadOnly Property CurrentType() As String
        Get
            If IsNothing(_CurrentPath) Then
                Return Nothing
            Else
                Return Document.SelectSingleNode(_CurrentPath).Name
            End If
        End Get
    End Property

    Private _SyncEventType As eSyncEventType
    Public ReadOnly Property SyncEventType() As eSyncEventType
        Get
            Return _SyncEventType
        End Get
    End Property

    Private _SyncItemCount As Integer = 0
    Public ReadOnly Property SyncItemCount() As Integer
        Get
            Return _SyncItemCount
        End Get
    End Property

    Private _SyncCurrentItem As Integer = 0
    Public ReadOnly Property SyncCurrentItem() As Integer
        Get
            Return _SyncCurrentItem
        End Get
    End Property

#End Region

#Region "Initialisation and Finalisation"

    Public Sub New(ByRef UE As UserEnv, _
                   ByVal Filename As String, _
                   ByVal URL As String, _
                   ByVal Flags As StartFlags, _
                   Optional ByVal Handler As System.EventHandler = Nothing, _
                   Optional ByVal PostIntervalSec As Integer = 0 _
    )

        Dim retry As Boolean = False
        Dim download As Boolean = True
        Dim sourceError As Exception = Nothing

        _SyncCurrentItem = 0
        _SyncItemCount = 0
        _UserEnv = UE

        With PostTimer
            If PostIntervalSec > 0 Then
                If PostIntervalSec < 60 Then PostIntervalSec = 60
                .Interval = PostIntervalSec * 1000
                AddHandler .Tick, AddressOf hPost
                .Enabled = True
            Else
                .Enabled = False
            End If
        End With

        LocalFile = String.Format("{0}\{1}", UE.LocalFolder, Filename)
        FileURL = String.Format("{0}{1}?user={2}", UE.Server.AbsoluteUri.ToString, URL, UE.User)

        If Not IsNothing(Handler) Then syncHandler = Handler
        If Flags.WipeData Then DeleteLocalCache()

        If Not File.Exists(LocalFile) Then

            Dim x As New Xml.XmlDocument()
            Do
                Try
                    With x
                        If Not IsNothing(syncHandler) Then
                            _CurrentPath = Nothing
                            _SyncEventType = eSyncEventType.BeginDownload
                            syncHandler.Invoke(Me, New System.EventArgs)
                        End If
                        .Load(FileURL)
                        sourceError = HasErrors(x)
                        If IsNothing(sourceError) Then
                            .Save(LocalFile)
                        End If
                    End With
                    download = True
                Catch ex As Exception
                    download = False
                    retry = MsgBox( _
                        String.Format("Cannot download {0}. {1}", FileURL, ex.Message) _
                        , MsgBoxStyle.RetryCancel, "Connection" _
                    ) = MsgBoxResult.Retry
                Finally
                    If Not IsNothing(syncHandler) Then
                        _CurrentPath = Nothing
                        _SyncEventType = eSyncEventType.EndDownload
                        syncHandler.Invoke(Me, New System.EventArgs)
                    End If
                    x = Nothing
                End Try
            Loop Until download Or (Not retry)

            If Not download Then Throw New Exception(String.Format("Cannot download {0}.", FileURL))
            If Not IsNothing(sourceError) Then Throw sourceError
            Document.Load(LocalFile)
            If Not IsNothing(syncHandler) Then SyncNewFile()

        Else

            If Flags.ClearCache Then
                Sync()
            Else
                Document.Load(LocalFile)
                If Not IsNothing(syncHandler) Then
                    _CurrentPath = Nothing
                    _SyncEventType = eSyncEventType.EndSync
                    syncHandler.Invoke(Me, New System.EventArgs)
                End If
            End If

        End If

        Loaded = True

    End Sub

#End Region

#Region "Public Methods"

    Public Sub Sync()

        Dim retry As Boolean = False
        Dim download As Boolean = True
        Dim sourceError As Exception = Nothing

        _SyncCurrentItem = 0
        _SyncItemCount = 0

        Try
            Dim x As New Xml.XmlDocument()
            Do
                Try
                    If Not IsNothing(syncHandler) Then
                        _CurrentPath = Nothing
                        _SyncEventType = eSyncEventType.BeginDownload
                        syncHandler.Invoke(Me, New System.EventArgs)
                    End If

                    x.Load(FileURL)
                    sourceError = HasErrors(x)
                    download = True
                Catch
                    download = False
                    retry = MsgBox( _
                        String.Format("Cannot download {0}.", FileURL) _
                        , MsgBoxStyle.RetryCancel, "Connection" _
                    ) = MsgBoxResult.Retry
                Finally
                    If Not IsNothing(syncHandler) Then
                        _CurrentPath = Nothing
                        _SyncEventType = eSyncEventType.EndDownload
                        syncHandler.Invoke(Me, New System.EventArgs)
                    End If
                End Try
            Loop Until download Or (Not retry)
            If Not IsNothing(sourceError) Then Throw sourceError

            With Document
                If download Then
                    Dim syncObjectNodes As XmlNodeList = x.SelectNodes("//sync/object")
                    Select Case syncObjectNodes.Count
                        Case 0
                            DeleteLocalCache()
                            x.Save(LocalFile)
                            .Load(LocalFile)

                        Case Else

                            If Not Loaded Then .Load(LocalFile)

                            SyncObjectList.Clear()
                            For Each s As XmlNode In syncObjectNodes
                                Dim f As Boolean = False
                                For Each so As SyncObject In SyncObjectList
                                    If so.IsChild(s) Then
                                        f = True
                                        Exit For
                                    End If
                                Next
                                If Not f Then SyncObjectList.Add(New SyncObject(s))
                            Next

                            For Each so As SyncObject In SyncObjectList
                                _SyncItemCount += x.SelectNodes(so.Path).Count
                            Next

                            For Each so As SyncObject In SyncObjectList
                                doSyncObject(x, so)
                            Next

                            .Save(LocalFile)

                    End Select
                Else
                    .Load(LocalFile)
                End If
            End With

        Catch SyncException As Exception
            MsgBox(SyncException.Message, MsgBoxStyle.Critical)

        Finally
            If Not IsNothing(syncHandler) Then
                _CurrentPath = Nothing
                _SyncEventType = eSyncEventType.EndSync
                syncHandler.Invoke(Me, New System.EventArgs)
            End If

        End Try

    End Sub

    Public Sub DeleteLocalCache()
        If File.Exists(LocalFile) Then
            Try
                File.Delete(LocalFile)
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        End If
    End Sub

    Public Sub MakePaths(ByVal path() As String)
        For Each p As String In path
            Dim thisNode As XmlNode = Document.SelectSingleNode(_CurrentPath)
            For Each el As String In Split(p, "/")
                With thisNode
                    If IsNothing(.SelectSingleNode(el)) Then
                        .AppendChild(.OwnerDocument.CreateElement(el))
                    End If
                    thisNode = .SelectSingleNode(el)
                End With
            Next
        Next
    End Sub

#End Region

#Region "Private Methods"

#Region "Check downloaded XML for errors"

    Private Function HasErrors(ByVal xmldoc As XmlDocument) As Exception

        Dim erdes As String = ""
        Dim pstr As String = ""
        Dim er As XmlNode = xmldoc.SelectSingleNode("//ERROR")

        If Not IsNothing(er) Then
            erdes = er.InnerText
            er = er.ParentNode
            Do While Not IsNothing(er.ParentNode)
                pstr = er.Name & "/" & pstr
                er = er.ParentNode
            Loop
            Return New Exception(String.Format("Error in XML path {0}/{1}: {2}", Me.FileURL, pstr, erdes))
        Else
            Return Nothing
        End If
    End Function

#End Region

#Region "Syncronisation"

#Region "Syncronisation on existing file"

    Private Sub doSyncObject(ByRef SourceXML As XmlDocument, ByVal SyncOb As SyncObject, Optional ByVal ParentPath As String = "")

        Dim soPath As String = Nothing
        Dim obParent As XmlNode

        With SyncOb
            Dim thispath As String = ParentPath & .RelativePath
            For Each SourceNode As XmlNode In SourceXML.SelectNodes(thispath)
                If ParentPath.Length = 0 Then _SyncCurrentItem += 1
                soPath = .xPathQuery(thispath, SourceNode)

                Dim TargetNode As XmlNode = Document.SelectSingleNode(soPath)
                If IsNothing(TargetNode) Then
                    obParent = Document.SelectSingleNode(thispath.Substring(0, thispath.LastIndexOf("/")))
                    obParent.AppendChild(obParent.OwnerDocument.ImportNode(SourceNode, True))

                    If Not IsNothing(syncHandler) Then
                        _CurrentPath = soPath
                        _SyncEventType = eSyncEventType.NewNode
                        syncHandler.Invoke(Me, New System.EventArgs)
                    End If

                Else
                    If Not IsNothing(syncHandler) Then
                        _CurrentPath = soPath
                        _SyncEventType = eSyncEventType.EditNode
                        syncHandler.Invoke(Me, New System.EventArgs)
                    End If
                    If IsNothing(TargetNode.Attributes("changed")) Then
                        If .sObject.Count > 0 Then
                            For Each so As SyncObject In .sObject
                                doSyncObject(SourceXML, so, soPath)
                            Next
                        Else
                            TargetNode.InnerXml = SourceNode.InnerXml
                            'If Not String.Compare(SourceNode.InnerXml, Left(TargetNode.InnerXml, SourceNode.InnerXml.Length)) = 0 Then
                            'RecurseCompare(SourceNode, TargetNode, soPath, .ListObjects)
                        End If

                    End If

                End If
            Next

            For Each TargetNode As XmlNode In Document.SelectNodes(thispath)
                soPath = .xPathQuery(thispath, TargetNode)
                If IsNothing(SourceXML.SelectSingleNode(soPath)) Then
                    If Not IsNothing(syncHandler) Then
                        _CurrentPath = soPath
                        _SyncEventType = eSyncEventType.DeleteNode
                        syncHandler.Invoke(Me, New System.EventArgs)
                    End If
                    TargetNode.ParentNode.RemoveChild(TargetNode)
                End If
            Next

        End With

    End Sub

    Private Sub RecurseCompare(ByVal SourceNode As XmlNode, ByVal TargetNode As XmlNode, ByVal syncOb As String, Optional ByVal NoFollow As List(Of String) = Nothing)

        If IsNothing(NoFollow) Then NoFollow = New List(Of String)
        For Each SourceChild As XmlNode In SourceNode.ChildNodes
            Dim TargetChild As XmlNode = Nothing
            Try
                TargetChild = Document.SelectSingleNode(String.Format("{0}/{1}", syncOb, SourceChild.Name))
            Catch
                Beep()
            End Try
            If IsNothing(TargetChild) Then
                TargetNode.AppendChild(TargetNode.OwnerDocument.ImportNode(SourceChild, True))
            Else
                If Not String.Compare(SourceChild.InnerXml, TargetChild.InnerXml) = 0 Then
                    If Not NoFollow.Contains(PathNoParam(String.Format("{0}/{1}", syncOb, SourceChild.Name))) Then
                        If IsNothing(TargetChild.Attributes("changed")) Then
                            If String.Compare(SourceChild.InnerXml, SourceChild.InnerText) = 0 Then
                                TargetChild.InnerText = SourceChild.InnerText
                            Else
                                RecurseCompare(SourceChild, TargetChild, String.Format("{0}/{1}", syncOb, SourceChild.Name))
                            End If
                        End If
                    End If
                End If
            End If
        Next

    End Sub

    Private Function PathNoParam(ByVal thisPath As String) As String
        Dim bstr As String = ""
        While thisPath.IndexOf("[") > 0
            bstr += thisPath.Substring(0, thisPath.IndexOf("["))
            Dim r As Integer = thisPath.IndexOf("]")
            thisPath = thisPath.Substring(r + 1, thisPath.Length - (r + 1))
        End While
        Return bstr & thisPath
    End Function

#End Region

#Region "Syncronisation on new file"

    Private Sub SyncNewFile()

        With Document
            Dim syncObjectNodes As XmlNodeList = Document.SelectNodes("//sync/object")
            Select Case syncObjectNodes.Count
                Case 0
                    If Not IsNothing(syncHandler) Then
                        _CurrentPath = Nothing
                        _SyncEventType = eSyncEventType.EndSync
                        syncHandler.Invoke(Me, New System.EventArgs)
                    End If
                    Exit Sub

                Case Else

                    For Each s As XmlNode In syncObjectNodes
                        Dim f As Boolean = False
                        For Each so As SyncObject In SyncObjectList
                            If so.IsChild(s) Then
                                f = True
                                Exit For
                            End If
                        Next
                        If Not f Then SyncObjectList.Add(New SyncObject(s))
                    Next

                    For Each so As SyncObject In SyncObjectList
                        _SyncItemCount += Document.SelectNodes(so.Path).Count
                    Next

                    For Each so As SyncObject In SyncObjectList
                        doSyncNewObject(Document, so)
                    Next

                    If Not IsNothing(syncHandler) Then
                        _CurrentPath = Nothing
                        _SyncEventType = eSyncEventType.EndSync
                        syncHandler.Invoke(Me, New System.EventArgs)
                    End If

            End Select

            .Save(LocalFile)

        End With

    End Sub

    Private Sub doSyncNewObject(ByRef SourceXML As XmlDocument, ByVal SyncOb As SyncObject, Optional ByVal ParentPath As String = "")

        Dim soPath As String = Nothing
        With SyncOb
            Dim thispath As String = ParentPath & .RelativePath
            For Each SourceNode As XmlNode In SourceXML.SelectNodes(thispath)
                If ParentPath.Length = 0 Then _SyncCurrentItem += 1
                soPath = .xPathQuery(thispath, SourceNode)
                _CurrentPath = soPath
                _SyncEventType = eSyncEventType.NewNode
                syncHandler.Invoke(Me, New System.EventArgs)
                For Each so As SyncObject In .sObject
                    doSyncNewObject(SourceXML, so, soPath)
                Next
            Next
        End With

    End Sub

#End Region

#End Region

#Region "Post Data"

    Public Function Post(ByRef postException As Exception) As Boolean

        Dim ret As Boolean = False

        If Not posting Then
            posting = True
            Dim postnodes As XmlNodeList

            Using sw As New StreamWriter(_UserEnv.LocalFolder & "\log.txt", True)
                sw.WriteLine("{0}: Checking for postable nodes.", Now.ToString)
                postnodes = Document.SelectNodes("//*[@post ='1']")
                While postnodes.Count > 0 And IsNothing(postException)
                    sw.WriteLine("Found {0} postable node(s).", postnodes.Count)
                    If PostData(postnodes(0), postException) Then
                        sw.WriteLine("Posted node ok.")
                        postnodes(0).Attributes.RemoveNamedItem("post")
                        Document.Save(LocalFile)
                        ret = True
                    Else
                        sw.WriteLine("Post Failed: {0}", postException.Message)
                        ret = False
                    End If
                    postnodes = Document.SelectNodes("//*[@post ='1']")
                End While
            End Using
            posting = False
        End If
        Return ret

    End Function

    Private Sub hPost(ByVal sender As Object, ByVal e As System.EventArgs)
        If Not posting Then
            posting = True
            Dim thPost As New Thread(AddressOf PostThread)
            With thPost
                .IsBackground = True
                .Start()
            End With
        End If
    End Sub

    Private Sub PostThread()

        Dim postnodes As XmlNodeList
        Dim postException As Exception
        Using sw As New StreamWriter(_UserEnv.LocalFolder & "\log.txt", True)
            sw.WriteLine("{0}: Checking for postable nodes.", Now.ToString)
            postnodes = Document.SelectNodes("//*[@post ='1']")
            While postnodes.Count > 0 And IsNothing(postException)
                sw.WriteLine("Found {0} postable node(s).", postnodes.Count)
                If PostData(postnodes(0), postException) Then
                    sw.WriteLine("Posted node ok.")
                    postnodes(0).Attributes.RemoveNamedItem("post")
                    Document.Save(LocalFile)
                Else
                    sw.WriteLine("Post Failed: {0}", postException.Message)
                End If
                postnodes = Document.SelectNodes("//*[@post ='1']")
            End While
        End Using
        posting = False

    End Sub

    Public Function PostData(ByVal NodeName As String, ByVal OuterXML As String, ByRef Ex As Exception) As Boolean
        Return PostBytes(SendWrapper(NodeName, OuterXML), Ex)
    End Function

    Public Function PostData(ByVal node As XmlNode, ByRef Ex As Exception) As Boolean
        Return PostBytes(SendWrapper(node.Name, node.OuterXml), Ex)
    End Function

    Private Function PostBytes(ByVal XMLData As Byte(), ByRef Ex As Exception) As Boolean

        Dim posted As Boolean = False
        Dim requestStream As Stream = Nothing
        Dim uploadResponse As Net.HttpWebResponse = Nothing
        Dim ms As MemoryStream = New MemoryStream(XMLData)

        Try

            Dim uploadRequest As Net.HttpWebRequest = CType(Net.HttpWebRequest.Create(String.Format("{0}{1}", ThisUserEnv.Server.AbsoluteUri, "posthandler.ashx")), Net.HttpWebRequest)
            uploadRequest.Method = "POST"
            uploadRequest.Proxy = Nothing
            uploadRequest.SendChunked = True
            requestStream = uploadRequest.GetRequestStream()

            ' Upload the XML
            Dim buffer(1024) As Byte
            Dim bytesRead As Integer
            While True
                bytesRead = ms.Read(buffer, 0, buffer.Length)
                If bytesRead = 0 Then
                    Exit While
                End If
                requestStream.Write(buffer, 0, bytesRead)
            End While

            ' The request stream must be closed before getting the response.
            requestStream.Close()

            uploadResponse = uploadRequest.GetResponse()

            Dim thisRequest As New XmlDocument
            Dim reader As New StreamReader(uploadResponse.GetResponseStream)
            With thisRequest
                .LoadXml(reader.ReadToEnd)
                Dim n As XmlNode = .SelectSingleNode("response")
                Dim er As Boolean = False
                For Each attrib As XmlAttribute In n.Attributes
                    If attrib.Name = "status" Then
                        If Not attrib.Value = "200" Then er = True
                    End If
                    If attrib.Name = "message" Then
                        If er Then
                            Throw New PostException(attrib.Value)
                        End If
                    End If
                Next
            End With

            posted = True

        Catch exep As UriFormatException
            Ex = New Exception("Invalid server URL.")
        Catch exep As Net.WebException
            Ex = New Exception("Could not connect to server. Please check Wi-Fi.")
        Catch exep As PostException
            Ex = New Exception("Posting fails. Server said: " & exep.Message)
        Catch exep As Exception
            Ex = New Exception("Unknown error while posting.")
        Finally
            If uploadResponse IsNot Nothing Then
                uploadResponse.Close()
            End If
            If requestStream IsNot Nothing Then
                requestStream.Close()
            End If
        End Try

        Return posted
    End Function

    Private Function SendWrapper(ByVal NodeName As String, ByVal OuterXML As String) As Byte()
        Dim myEncoder As New System.Text.ASCIIEncoding
        Dim str As New System.Text.StringBuilder
        Dim xw As XmlWriter = XmlWriter.Create(str)
        With xw
            .WriteStartDocument()
            .WriteStartElement("post")
            .WriteStartElement("meta")
            .WriteElementString("user", ThisUserEnv.User)
            .WriteElementString("date", Date.Now.ToString)
            .WriteElementString("nodetype", NodeName)
            .WriteEndElement()
            .WriteRaw(OuterXML)
            .WriteEndElement()
            .WriteEndDocument()
            .Flush()
        End With
        Return myEncoder.GetBytes(str.ToString)
    End Function

#End Region

#End Region

End Class

Public Class PostException : Inherits System.Exception

    Public Sub New()

    End Sub

    Public Sub New(ByVal message As String)
        MyBase.New(message)
    End Sub

End Class