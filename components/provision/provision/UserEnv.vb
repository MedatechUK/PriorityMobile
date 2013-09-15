Imports System.Xml
Imports System.IO
Imports System.Net
Imports System.Drawing
Imports System.Reflection

Public Class UserEnv
    Inherits Dictionary(Of String, OfflineXML)

#Region "Initialisation and finalisation"

    Private ProvisionFile As String
    Private _CustomDLL As String
    Private _AppDataFolder As String

    Public Sub New(ByVal AppDataFolder As String, Optional ByVal CustomDLL As String = Nothing)

        _CustomDLL = CustomDLL
        _AppDataFolder = AppDataFolder

        ProvisionFile = Me.AppPath & "\provision.txt"

        Dim prov As New Xml.XmlDocument
        Dim download As Boolean = True
        Dim retry As Integer = 0
        Dim pnode As XmlNode = Nothing
        Dim ps As String
        Dim StoredPS As String = ProvisionString

        Do
            Try
                prov.Load("http://soti.emerge-it.co.uk/client/provision.xml")
                download = True
            Catch
                download = False
            Finally
                retry += 1
            End Try
        Loop Until download Or (retry > 2)

        Dim ce As FileInfo() = CreatedEnvironments()

        If Not download Then
            If ce.Count > 0 Then
                Dim fpart() As String = ce.Last.DirectoryName.Split("\")
                _User = fpart(UBound(fpart))
                _Server = New System.Uri(String.Format("http://{0}:8080", fpart(UBound(fpart) - 1)))
            Else
                MsgBox("No provisioned environment exists on the device. Please connect to WiFi to provision.")
                Throw New Exception("Device has no stored provisioning info.")
            End If

        Else
            If Not IsNothing(StoredPS) Then
                pnode = prov.SelectSingleNode(String.Format("devices/user[@ProvisionString={0}{1}{0}]", Chr(34), StoredPS))
                If Not IsNothing(pnode) Then
                    _User = pnode.SelectSingleNode("username").InnerText
                    _Server = New Uri(pnode.SelectSingleNode("url").InnerText)
                End If
            Else
                If Not IsNothing(ce) Then
                    If ce.Count > 0 Then
                        Dim fpart() As String = ce.Last.DirectoryName.Split("\")
                        _User = fpart(UBound(fpart))
                        _Server = New System.Uri(String.Format("http://{0}:8080", fpart(UBound(fpart) - 1)))
                    End If
                End If
            End If

            'Dim _ip As String = Dns.GetHostEntry(_Server.Host).ToString

            Do
                retry = False
                Dim dia As New frmProvisionDialog(StoredPS)
                Dim logo As Image = LogoFile
                With dia
                    If Not IsNothing(User) Then
                        .txtServerName.Text = Replace(Server.Host, "http://", "", , , CompareMethod.Text)
                        .txtUsername.Text = User
                        .txtProvisionString.Text = StoredPS
                        .panel_reprovision.Visible = True
                        .Panel_Text.Visible = False
                    Else
                        .panel_reprovision.Visible = False
                        .Panel_Text.Visible = True
                    End If

                    If Not IsNothing(logo) Then .logo.Image = logo
                    .HideSIPButton()
                    .ShowDialog()
                    ps = .ProvisionString
                End With

                pnode = prov.SelectSingleNode(String.Format("devices/user[@ProvisionString={0}{1}{0}]", Chr(34), ps))
                If IsNothing(pnode) Then retry = MsgBox( _
                                                            String.Format("Invalid provisioning code.") _
                                                            , MsgBoxStyle.RetryCancel, "Connection" _
                                                        ) = MsgBoxResult.Retry

            Loop Until Not IsNothing(pnode) Or Not retry

            If IsNothing(pnode) Then
                Throw New Exception("Provisioning info not found.")
            End If

            _User = pnode.SelectSingleNode("username").InnerText
            _Server = New Uri(pnode.SelectSingleNode("url").InnerText)

            With Arguments
                .Add("USERNAME", _User)
            End With

            If Not String.Compare(StoredPS, ps) = 0 Then
                Dim l As System.Drawing.Image = LogoFile()
                l = Nothing
                ProvisionString = ps
            End If

        End If

        If Not IsNothing(_CustomDLL) Then
            If String.Compare(_CustomDLL.Substring(0, 1), "\") = 0 Then
                _CustomHandler = Assembly.LoadFrom(_CustomDLL)
            Else
                UpdateHandler()
                If File.Exists(LocalFolder() & "\custHandler.dll") Then
                    Try
                        _CustomHandler = Assembly.LoadFrom(LocalFolder() & "\custHandler.dll")
                    Catch
                        _CustomHandler = Nothing
                    End Try
                Else
                    _CustomHandler = Nothing
                End If
            End If
        End If

        If File.Exists(MACFile) Then
            Using SR As New StreamReader(MACFile)
                prnmac = SR.ReadToEnd
            End Using
        End If

        ClearLog()

    End Sub

#End Region

#Region "Public Properties"

    Private _exeVersion As New Version(0, 0, 0, 0)
    Public Property exeVersion() As Version
        Get
            Return _exeVersion
        End Get
        Set(ByVal value As Version)
            _exeVersion = value
        End Set
    End Property

    Private _Arguments As New Dictionary(Of String, String)
    Public Property Arguments() As Dictionary(Of String, String)
        Get
            Return _Arguments
        End Get
        Set(ByVal value As Dictionary(Of String, String))
            _Arguments = value
        End Set
    End Property

    Private _SaveProvision As Boolean = True
    Public Property SaveProvision() As Boolean
        Get
            Return _SaveProvision
        End Get
        Set(ByVal value As Boolean)
            _SaveProvision = value
            If Not _SaveProvision Then
                While File.Exists(ProvisionFile)
                    File.Delete(ProvisionFile)
                End While
            End If
        End Set
    End Property

    Private _User As String
    Public Property User() As String
        Get
            Return _User
        End Get
        Set(ByVal value As String)
            _User = value
        End Set
    End Property

    Private _Server As Uri = Nothing
    Public Property Server() As Uri
        Get
            Return _Server
        End Get
        Set(ByVal value As Uri)
            _Server = value
        End Set
    End Property

    Private _ProvisionString As String = Nothing
    Public Property ProvisionString() As String
        Get
            Dim ret As String = Nothing
            Try
                If File.Exists(ProvisionFile) Then
                    Using ps As New StreamReader(ProvisionFile)
                        ret = ps.ReadToEnd
                    End Using
                End If
                Return ret
            Catch ex As Exception
                Return Nothing
            End Try
        End Get
        Set(ByVal value As String)
            While File.Exists(ProvisionFile)
                File.Delete(ProvisionFile)
            End While
            If SaveProvision Then
                Using ps As New StreamWriter(ProvisionFile)
                    ps.Write(value)
                End Using
            End If
        End Set
    End Property

    Public ReadOnly Property LogoFile() As System.Drawing.Image
        Get
            If Not IsNothing(Server) Then ' Get the logo for this environment
                If Not File.Exists(LocalFolder() & "\logo.bmp") Then
                    Try
                        GetFile(Server, "logo.bmp", LocalFolder)
                    Catch
                        Return Nothing
                    End Try
                End If
                Try
                    Dim ptrBitmap As Bitmap = New Bitmap(LocalFolder() & "\logo.bmp")
                    Dim hbmp As IntPtr = ptrBitmap.GetHbitmap
                    Return Image.FromHbitmap(hbmp)
                Catch
                    Return Nothing
                End Try

            Else ' Get the Logo for the application

                If Not File.Exists(AppPath() & "\logo.bmp") Then
                    Try
                        GetFile(New Uri("http://soti.emerge-it.co.uk/client/"), "logo.bmp", AppPath)
                    Catch
                        Return Nothing
                    End Try
                End If
                Try
                    Dim ptrBitmap As Bitmap = New Bitmap(AppPath() & "\logo.bmp")
                    Dim hbmp As IntPtr = ptrBitmap.GetHbitmap
                    Return Image.FromHbitmap(hbmp)
                Catch
                    Return Nothing
                End Try

            End If
        End Get
    End Property

    Private _CustomHandler As System.Reflection.Assembly = Nothing
    Public ReadOnly Property CustomHandler() As System.Reflection.Assembly
        Get
            Return _CustomHandler
        End Get
    End Property

    Private prnmac As String = String.Empty
    Public Property MACAddress() As String
        Get
            Return prnmac
        End Get
        Set(ByVal value As String)
            prnmac = value
            While File.Exists(MACFile)
                File.Delete(MACFile)
                System.Threading.Thread.Sleep(100)
            End While
            Using sr As New StreamWriter(MACFile)
                sr.Write(value)
            End Using
        End Set
    End Property

    Private ReadOnly Property MACFile() As String
        Get
            Return String.Format("{0}\prnmac.txt", AppPath())
        End Get
    End Property

#End Region

#Region "Public Methods"

    Public Function HandlerVersion(ByRef excep As Exception) As String
        excep = Nothing
        Dim ret As String = "0.0.0.0"
        If Not IsNothing(_CustomHandler) Then
            With _CustomHandler.GetName().Version
                ret = String.Format("{0}.{1}.{2}.{3}", _
                    .Major, _
                    .Minor, _
                    .Build, _
                    .Revision _
                )
            End With
        End If
        Return ret
    End Function

    Public Function LocalFolder() As String

        Dim dir As New DirectoryInfo( _
            String.Format( _
                "{0}\{3}\{1}\{2}", _
                 Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), _
                 _Server.Host, _
                _User, _
                _AppDataFolder _
                ) _
            )
        With dir
            If Not .Exists Then .Create()
            Return dir.FullName
        End With

    End Function

    Public Function AppPath() As String

        Dim fullPath As String = System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase
        Return fullPath.Substring(0, fullPath.LastIndexOf("\"))

    End Function

    Public Sub GetFile(ByVal Server As Uri, ByVal filename As String, ByVal toFolder As String)
        Try

            Dim httpRequest As HttpWebRequest = DirectCast(WebRequest.Create(Server.AbsoluteUri & filename.Replace("\", "/")), HttpWebRequest)
            httpRequest.Credentials = CredentialCache.DefaultCredentials

            Dim httpResponse As HttpWebResponse = DirectCast(httpRequest.GetResponse(), HttpWebResponse)

            Dim dataStream As System.IO.Stream = httpResponse.GetResponseStream()
            Dim inBuf As Byte() = New Byte(10000000) {}
            Dim bytesToRead As Integer = Convert.ToInt32(inBuf.Length)
            Dim bytesRead As Integer = 0
            While bytesToRead > 0
                Dim n As Integer = dataStream.Read(inBuf, bytesRead, bytesToRead)
                If n = 0 Then
                    Exit While
                End If
                bytesRead += n
                bytesToRead -= n
            End While
            Dim fstr As New FileStream(toFolder & "\" & filename.Replace("/", "\"), FileMode.OpenOrCreate, FileAccess.Write)
            fstr.Write(inBuf, 0, bytesRead)
            dataStream.Close()
            fstr.Close()
        Catch ex As Exception

        End Try

    End Sub

    Public Sub Log(ByVal FormatString As String, ByVal ParamArray Values() As String)
        Dim thisTry As Integer = 0
        Dim write As Boolean = False
        Do
            Try
                Using sw As New StreamWriter(LocalFolder() & "\log.txt", True)
                    sw.WriteLine(FormatString, Values)
                End Using
                write = True
            Catch
                System.Threading.Thread.Sleep(500)
                thisTry += 1
            End Try
        Loop Until thisTry > 2 Or write
    End Sub

    Public Sub Log(ByVal FormatString As String)
        Dim thisTry As Integer = 0
        Dim write As Boolean = False
        Do
            Try
                Using sw As New StreamWriter(LocalFolder() & "\log.txt", True)
                    sw.WriteLine(FormatString)
                End Using
            Catch
                System.Threading.Thread.Sleep(500)
                thisTry += 1
            End Try
        Loop Until thisTry > 2 Or write
    End Sub

#End Region

#Region "Private Methods"

    Private Sub ClearLog()
        Dim fi As New FileInfo(LocalFolder() & "\log.txt")
        If fi.Exists Then
            If fi.Length > 2048 Then
                fi.Delete()
            End If
        End If
    End Sub

    Private Function CreatedEnvironments() As FileInfo()
        Dim ce() As FileInfo = Nothing
        Dim dir As New DirectoryInfo( _
            String.Format( _
                "{0}\{1}\", _
                 Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), _
                 _AppDataFolder _
            ) _
        )
        If dir.Exists Then
            For Each server As DirectoryInfo In dir.GetDirectories
                For Each User As DirectoryInfo In server.GetDirectories
                    For Each File As FileInfo In User.GetFiles
                        Try
                            ReDim Preserve ce(UBound(ce) + 1)
                        Catch ex As Exception
                            ReDim ce(0)
                        Finally
                            ce(UBound(ce)) = File
                        End Try
                    Next
                Next
            Next
            If Not IsNothing(ce) Then Array.Sort(ce, New FileLastWriteCompare)
        End If
        Return ce
    End Function

    Public Sub UpdateHandler()

        GetFile(Server, _CustomDLL, LocalFolder)
        If File.Exists(String.Format("{0}\{1}", LocalFolder, _CustomDLL)) Then
            Dim r As Integer = 0
            While File.Exists(String.Format("{0}\custHandler.dll", LocalFolder))
                File.Delete(String.Format("{0}\custHandler.dll", LocalFolder))
                r += 1
                System.Threading.Thread.Sleep(100)
                If r > 10 Then Exit While
            End While
            If Not File.Exists(String.Format("{0}\custHandler.dll", LocalFolder)) Then
                File.Move(String.Format("{0}\{1}", LocalFolder, _CustomDLL), String.Format("{0}\custHandler.dll", LocalFolder))
            End If
        End If

    End Sub

#End Region

End Class

Public Class FileLastWriteCompare : Implements IComparer

    Public Function Compare(ByVal o1 As Object, ByVal o2 As Object) As Integer Implements IComparer.Compare
        Dim file1 As System.IO.FileInfo = o1
        Dim file2 As System.IO.FileInfo = o2
        Return DateTime.Compare(file1.LastWriteTime, file2.LastWriteTime)
    End Function

End Class