Imports System.Xml
Imports System.IO

Public Class UserEnv

#Region "Initialisation and finalisation"

    Public Sub New(ByVal User As String, ByVal Server As Uri)
        _User = User
        _Server = Server
    End Sub

#End Region

#Region "Public Properties"

    Private _User As String
    Public Property User() As String
        Get
            Return _User
        End Get
        Set(ByVal value As String)
            _User = value
        End Set
    End Property
    Private _Server As Uri
    Public Property Server() As Uri
        Get
            Return _Server
        End Get
        Set(ByVal value As Uri)
            _Server = value
        End Set
    End Property

#End Region

#Region "Public Methods"

    Public Function LocalFolder() As String

        Dim dir As New DirectoryInfo( _
            String.Format( _
                "{0}\PriorityMobile\{1}\{2}", _
                 Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), _
                 _Server.Host, _
                _User _
                ) _
            )
        With dir
            Debug.Print("Local Environment = " & dir.FullName)
            If Not .Exists Then .Create()
            Return dir.FullName
        End With

    End Function

#End Region

End Class

Public Class OfflineXML

#Region "Initialisation and Finalisation"

    Public Sub New(ByRef UE As UserEnv, ByVal Filename As String, ByVal FileURL As String)
        _LocalFile = String.Format("{0}\{1}", UE.LocalFolder, Filename)
        _FileURL = String.Format("{0}{1}", UE.Server.AbsoluteUri.ToString, FileURL)
        If Not File.Exists(_LocalFile) Then
            Try
                File.Copy(Filename, _LocalFile)
            Catch EX As Exception
            End Try
        End If
        _Document.Load(_LocalFile)
    End Sub

#End Region

#Region "Public Properties"

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
            Return _FileURL
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

#End Region

#Region "Public Methods"

    Public Sub Sync()
        '#TODO
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

#End Region

End Class

Public Class TopLevelForm

#Region "Private Variables"

    Private _openForms As New List(Of xForm)

#End Region

#Region "initialisation and finalisation"

    Public Sub New(ByRef ThisForm As xForm)
        _TopForm = ThisForm
        _openForms.Add(_TopForm)
    End Sub

#End Region

#Region "Public Properties"

    Private _TopForm As xForm
    Public Property TopForm() As xForm
        Get
            Return _TopForm
        End Get
        Set(ByVal value As xForm)
            _TopForm = value
        End Set
    End Property

    Public ReadOnly Property CurrentForm() As xForm
        Get
            Return _openForms(_openForms.Count - 1)
        End Get
    End Property

#End Region

#Region "Public Methods"

    Public Sub OpenForm(ByVal FormName As String)
        If CurrentForm.SubForms.Keys.Contains(FormName) Then
            _openForms.Add(CurrentForm.SubForms(FormName))
            CurrentForm.Bind()
        Else
            MsgBox( _
                String.Format( _
                    "Form {0} does not contain a subform called {1}", _
                    CurrentForm.FormName, _
                    FormName _
                ), _
            MsgBoxStyle.OkOnly)
        End If
    End Sub

    Public Sub CloseForm()
        If Not IsNothing(CurrentForm.Parent) Then
            _openForms.RemoveAt(_openForms.Count - 1)
            'CurrentForm.Bind()
        End If
    End Sub

#End Region

End Class

Public Class xmlForms

#Region "Public Shared Members"

    Public Shared FormDesign As OfflineXML
    Public Shared FormData As OfflineXML
    
#End Region

#Region "initialisation and Finalisation"

    Public Sub New(ByVal _FormDesign As OfflineXML, ByVal _FormData As OfflineXML)

        FormDesign = _FormDesign
        FormData = _FormData

        For Each f As XmlNode In FormDesign.Document.SelectNodes("forms/form")
            tf.Add(New TopLevelForm(New xForm(f, f.Attributes("xpath").Value, Nothing)))
        Next
        For Each t As TopLevelForm In tf
            t.TopForm.Bind()
        Next

    End Sub

#End Region

#Region "Private Properties"

    Private tf As New List(Of TopLevelForm)

#End Region

#Region "Public Properties"

    Public ReadOnly Property TopForms() As List(Of TopLevelForm)
        Get
            Return tf
        End Get
    End Property

    Private _Activeform As Integer = 0
    Public Property ActiveForm() As Integer
        Get
            Return _Activeform
        End Get
        Set(ByVal value As Integer)
            If value > tf.Count - 1 Then
                _Activeform = 0
            Else
                _Activeform = value
            End If
        End Set
    End Property

#End Region

End Class

Public Class xForm

#Region "Private Variables"
    Private DataT As DataTable
#End Region

#Region "Initialisation and finalisation"

    Public Sub New(ByRef thisnode As XmlNode, ByVal xPath As String, ByVal Parent As xForm)
        Try
            With Me
                .thisNode = thisnode
                .Parent = Parent
                .xPath = xPath
                .FormName = thisnode.Attributes("name").Value
            End With

            If thisnode.SelectNodes("form").Count > 0 Then
                For Each sf As XmlNode In thisnode.SelectNodes("form")
                    SubForms.Add(sf.Attributes("name").Value, New xForm(sf, sf.Attributes("xpath").Value, Me))
                Next
            End If

            If thisnode.SelectNodes("views/view").Count > 0 Then
                For Each vw As XmlNode In thisnode.SelectNodes("views/view")
                    Select Case vw.Attributes("control").Value.ToUpper
                        Case ""
                            Me.Views.Add(New UserControl1)
                    End Select
                Next
            Else
                Throw New Exception(String.Format("Form {0} has no views", thisnode.Attributes("name").Value))
            End If

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

#End Region

#Region "Public Properties"

    Private _thisNode As XmlNode
    Public Property thisNode() As XmlNode
        Get
            Return _thisNode
        End Get
        Set(ByVal value As XmlNode)
            _thisNode = value
        End Set
    End Property

    Private _FormName As String
    Public Property FormName() As String
        Get
            Return _FormName
        End Get
        Set(ByVal value As String)
            _FormName = value
        End Set
    End Property

    Private _xpath As String
    Public Property xPath() As String
        Get
            Dim path As String = _xpath
            Dim p As xForm = Parent
            Dim k As String
            Do Until IsNothing(p)
                If Not IsNothing(thisNode.ParentNode.Attributes("key")) Then
                    k = String.Format( _
                        "[{1}={0}{2}{0}]", _
                        Chr(34), _
                        thisNode.ParentNode.Attributes("key").Value, _
                        p.Views(p.CurrentView).Selected _
                        )
                Else
                    k = ""
                End If
                path = String.Format("{0}{1}{2}", p.xPath, k, path)
                p = p.Parent
            Loop
            Return path
        End Get
        Set(ByVal value As String)
            _xpath = value
        End Set
    End Property

    Private _parent As xForm
    Public Property Parent() As xForm
        Get
            Return _parent
        End Get
        Set(ByVal value As xForm)
            _parent = value
        End Set
    End Property

    Private _subforms As New Dictionary(Of String, xForm)
    Public Property SubForms() As Dictionary(Of String, xForm)
        Get
            Return _subforms
        End Get
        Set(ByVal value As Dictionary(Of String, xForm))
            _subforms = value
        End Set
    End Property

    Private _Views As New List(Of ViewControl.iView)
    Public Property Views() As List(Of ViewControl.iView)
        Get
            Return _Views
        End Get
        Set(ByVal value As List(Of ViewControl.iView))
            _Views = value
        End Set
    End Property

    Private _CurrentView As Integer = 0
    Public Property CurrentView() As Integer
        Get
            Return _CurrentView
        End Get
        Set(ByVal value As Integer)
            If value > Me.Views.Count - 1 Then
                _CurrentView = 0
            Else
                _CurrentView = value
            End If
        End Set
    End Property

#End Region

#Region "Public Methods"

    Public Sub Bind()

        Try

            Dim ds As New DataSet
            Dim node As XmlNode = xmlForms.FormData.Document.SelectSingleNode(Me.xPath)

            If Not IsNothing(node) Then

                Dim retstr As String = String.Format("<{0}>", node.ParentNode.Name)
                For Each n As XmlNode In xmlForms.FormData.Document.SelectNodes(Me.xPath)
                    retstr += Replace(n.OuterXml, String.Format(" changed={0}1{0}", Chr(34)), "", , , CompareMethod.Text)
                Next
                retstr += String.Format("</{0}>", node.ParentNode.Name)

                Dim MemoryStream As New System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(retstr))
                MemoryStream.Seek(0, System.IO.SeekOrigin.Begin)
                ds.ReadXml(XmlReader.Create(MemoryStream))
                DataT = ds.Tables(0)
                AddHandler DataT.ColumnChanged, AddressOf hColumnChanged

                For Each V As ViewControl.iView In Me.Views
                    V.SetTable(DataT)
                    V.Bind()
                Next

            Else
                Throw New Exception("xPath not found")
            End If

        Catch ex As Exception
            MsgBox(String.Format("Error {0} retrieving data with xpath {1}", ex.Message, _xpath))
        End Try

    End Sub

#End Region

#Region "Event Handlers"

    Private Sub hColumnChanged(ByVal sender As Object, ByVal e As System.Data.DataColumnChangeEventArgs)
        With xmlForms.FormData
            Dim changedAttribute As XmlAttribute
            changedAttribute = .Document.CreateAttribute("changed")
            changedAttribute.Value = "1"
            Dim n As XmlNode = .Document.SelectSingleNode(Me.xPath).SelectSingleNode(e.Column.ColumnName)
            With n
                n.InnerText = e.ProposedValue
                n.Attributes.Append(changedAttribute)
            End With
            .Document.Save(.LocalFile)
        End With
    End Sub

#End Region

End Class
