Imports System.Threading
Imports System.Windows.Forms
Imports System.IO
Imports System.Environment
Imports System.Web
Imports System.Reflection

Public Class PriorityHTTPixie

    Private Declare Function FindWindow Lib "user32" _
        (ByVal lpClassName As String, _
         ByVal lpWindowName As String) As IntPtr


    Private Declare Function GetWindowThreadProcessId Lib "user32" _
        (ByVal hwnd As Long, _
        ByVal lpdwProcessId As Long) As Long

    Private WithEvents svr As New MyServer("127.0.0.1", 2000)

#Region "Initialisation and finalisation"

    Private Sub Form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.Hide()
        With NotifyIcon1
            .Visible = True
            .Text = Environment.MachineName & ":" & svr.Port
        End With

        Try
            ' Start the service
            svr.StartSvc()
        Catch ex As Exception
            MsgBox("The service failed to start. Please ensure there is not another instance of the service already running", MsgBoxStyle.OkOnly + MsgBoxStyle.Critical, "Service not started.")
        End Try
    End Sub

    Private Sub Form1_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed
        svr.StopSvc()
    End Sub

#End Region

#Region "Private Properties"

    Private ReadOnly Property NoPage() As String
        Get
            Dim PAGE As String = "<!DOCTYPE html PUBLIC {0}-//W3C//DTD XHTML 1.0 Strict//EN{0} {0}http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd{0}>" & _
                    "<html xmlns={0}http://www.w3.org/1999/xhtml{0}>" & _
                    "<head>" & _
                    "<meta http-equiv={0}Content-Type{0} content={0}text/html; charset=iso-8859-1{0}/>" & _
                    "<title>404 - A response file was not found.</title>" & _
                    "<style type={0}text/css{0}>" & _
                    "<!--" & _
                    "body{margin:0;font-size:.7em;font-family:Verdana, Arial, Helvetica, sans-serif;background:#EEEEEE;}" & _
                    "fieldset{padding:0 15px 10px 15px;}" & _
                    "h1{font-size:2.4em;margin:0;color:#FFF;}" & _
                    "h2{font-size:1.7em;margin:0;color:#CC0000;}" & _
                    "h3{font-size:1.2em;margin:10px 0 0 0;color:#000000;}" & _
                    "#header{width:96%;margin:0 0 0 0;padding:6px 2% 6px 2%;font-family:{0}trebuchet MS{0}, Verdana, sans-serif;color:#FFF;" & _
                    "background-color:#555555;}" & _
                    "#content{margin:0 0 0 2%;position:relative;}" & _
                    ".content-container{background:#FFF;width:96%;margin-top:8px;padding:10px;position:relative;}" & _
                    "-->" & _
                    "</style>" & _
                    "</head>" & _
                    "<body>" & _
                    "<div id={0}header{0}><h1>Server Error</h1></div>" & _
                    "<div id={0}content{0}>" & _
                    " <div class={0}content-container{0}><fieldset>" & _
                    "  <h2>The $ERR$.html response file was not found on the local machine.</h2>" & _
                    "  <h3>Response files should be stored in the wpfLocalHTTP application installation folder.<br>" & _
                    "The requested response file was $PATH$$ERR$.html<br><BR>" & _
                    "Please contact your administrator to replace these files.</h3>" & _
                    " </fieldset></div>" & _
                    "</div>" & _
                    "</body>" & _
                    "</html>"
            Return Replace(PAGE, "{0}", Chr(34))
        End Get
    End Property

    Private ReadOnly Property dtostr() As String
        Get
            Return Format(Now, "ddd, dd MMM yyyy HH:mm:ss")
        End Get
    End Property

    Private ReadOnly Property FullPath()
        Get
            Dim fp As String = System.Reflection.Assembly.GetExecutingAssembly().Location
            While Not fp.EndsWith("\")
                fp = fp.Remove(fp.Length - 1, 1)
            End While
            Return fp
        End Get
    End Property

#End Region

#Region "Handlers"

    Private Sub hOnConnect(ByVal ConnectionID As String) _
        Handles svr.OnConnect

    End Sub

    Private Sub hOnCommand(ByVal ConnectionID As String, ByVal CmdStr As String, ByVal Args() As String) _
        Handles svr.OnCommand

        With svr

            .SessionData(ConnectionID, "exe") = ""
            .SessionData(ConnectionID, "param") = ""
            .SessionData(ConnectionID, "path") = FullPath
            .SessionData(ConnectionID, "dir") = Environment.GetFolderPath(SpecialFolder.System)
            .SessionData(ConnectionID, "err") = ""
            .SessionData(ConnectionID, "erdes") = ""

            If CmdStr.StartsWith("GET ") Then
                If InStr(Args(1), ".exe", CompareMethod.Text) > 0 Then
                    Dim arg = Args(1).Remove(0, 1)
                    For i As Integer = 2 To UBound(Args) - 1
                        arg += Args(i)
                    Next
                    Decode(arg)

                    If InStr(Args(1), "?") > 0 Then
                        .SessionData(ConnectionID, "exe") = Microsoft.VisualBasic.Strings.Left(arg, InStr(arg, "?") - 1)
                        .SessionData(ConnectionID, "param") = Strings.Right(arg, arg.Length - InStr(arg, "?"))
                    Else
                        .SessionData(ConnectionID, "exe") = arg
                        .SessionData(ConnectionID, "param") = ""
                    End If

                    If InStr(.SessionData(ConnectionID, "exe"), "/") > 0 Then
                        Dim e() As String = Split(.SessionData(ConnectionID, "exe"), "/")
                        Dim bstr As String = ""
                        .SessionData(ConnectionID, "exe") = e(UBound(e))
                        For i As Integer = 0 To UBound(e) - 1
                            bstr += e(i) & "\"
                        Next
                        .SessionData(ConnectionID, "dir") = bstr
                    End If

                    If Microsoft.VisualBasic.Right(.SessionData(ConnectionID, "dir"), 1) <> "\" Then
                        .SessionData(ConnectionID, "dir") = .SessionData(ConnectionID, "dir") & "\"
                    End If

                    Dim erdes As String = RunApp(ConnectionID)

                    If IsNothing(erdes) Then
                        .SessionData(ConnectionID, "err") = "200"
                        .SessionData(ConnectionID, "erdes") = ""
                    Else
                        .SessionData(ConnectionID, "err") = "500"
                        .SessionData(ConnectionID, "erdes") = erdes
                    End If

                ElseIf InStr(Args(1), "/favicon.ico", CompareMethod.Text) > 0 Then
                    .SessionData(ConnectionID, "err") = "404"
                    .SessionData(ConnectionID, "erdes") = "Page not found."

                ElseIf InStr(Args(1), "/help", CompareMethod.Text) > 0 Then
                    .SessionData(ConnectionID, "err") = "401"
                    .SessionData(ConnectionID, "erdes") = ""

                ElseIf InStr(Args(1), "/about", CompareMethod.Text) > 0 Then
                    .SessionData(ConnectionID, "err") = "402"
                    .SessionData(ConnectionID, "erdes") = ""

                Else
                    .SessionData(ConnectionID, "err") = "402"
                    .SessionData(ConnectionID, "erdes") = ""

                End If

                Respond(ConnectionID)

            End If

        End With

    End Sub
#End Region

#Region "Context Menu Handlers"

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        Me.Dispose()
    End Sub

    Private Sub AboutToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AboutToolStripMenuItem.Click
        Try
            Dim myProcess As Process = New Process()
            With myProcess
                With .StartInfo
                    .FileName = "iexplore.exe"
                    .Arguments = "http://localhost:" & svr.Port & "/about"
                    .UseShellExecute = True
                    .CreateNoWindow = False
                End With
                .Start()
            End With

        Catch EX As Exception

        End Try
    End Sub

#End Region

#Region "Private Functions"

    Private Function RunApp(ByVal ConnectionID As String) As String
        Try
            Dim sOutput As String = ""
            Dim sErrs As String = ""
            Dim myProcess As Process = New Process()

            With myProcess
                With .StartInfo
                    .FileName = svr.SessionData(ConnectionID, "exe")
                    .Arguments = svr.SessionData(ConnectionID, "param")
                    .WorkingDirectory = svr.SessionData(ConnectionID, "dir")
                    .UseShellExecute = True
                    .CreateNoWindow = False
                End With
                .Start()
            End With

            Return Nothing

        Catch EX As Exception
            Return EX.Message
        End Try

    End Function

    Private Sub Respond(ByVal ConnectionID As String)

        Dim response As String = ""
        With svr
            Dim fn As String = String.Format("{0}{1}.html", FullPath, .SessionData(ConnectionID, "err"))
            If Not File.Exists(fn) Then
                response = NoPage
                .Send(ConnectionID, "HTTP/1.1 404 Not Found")
            Else
                Dim sr As New System.IO.StreamReader(fn)
                With sr
                    response = .ReadToEnd
                    .Dispose()
                End With

                Select Case .SessionData(ConnectionID, "err")
                    Case "200", "401", "402"
                        .Send(ConnectionID, "HTTP/1.1 200 OK")
                    Case "404"
                        .Send(ConnectionID, "HTTP/1.1 404 Not Found")
                    Case "500"
                        .Send(ConnectionID, "HTTP/1.1 500 Internal Error")
                End Select

            End If

            ParseResponse(response, ConnectionID)

            .Send(ConnectionID, "Content-Type: text/html")
            .Send(ConnectionID, "Cache-Control: no-cache")
            .Send(ConnectionID, String.Format("Last-Modified: {0} GMT", dtostr))
            .Send(ConnectionID, "Accept-Ranges: bytes")
            .Send(ConnectionID, "Server: wpfDesktopHTTP")
            .Send(ConnectionID, "X-Powered-By: eMerge-IT")
            .Send(ConnectionID, String.Format("Date: {0} GMT", dtostr))
            .Send(ConnectionID, "Content-Length: " & response.Length)
            .Send(ConnectionID, "")
            .Send(ConnectionID, response)
            .KillConnection(ConnectionID)

            'For Each p As Process In Process.GetProcesses
            '    If Strings.Left(p.MainWindowTitle, Len("Priority HTTPixie - Running ")) = "Priority HTTPixie - Running " Then
            '        p.Kill()
            '    End If
            'Next

        End With

    End Sub

    Private Sub ParseResponse(ByRef Response As String, ByVal ConnectionID As String)

        Response = Response.Replace("$ERR$", svr.SessionData(ConnectionID, "err").ToUpper)
        Response = Response.Replace("$ERDES$", svr.SessionData(ConnectionID, "erdes"))
        Response = Response.Replace("$EXE$", svr.SessionData(ConnectionID, "exe").ToUpper)
        Response = Response.Replace("$PAR$", svr.SessionData(ConnectionID, "param").ToUpper)
        Response = Response.Replace("$PATH$", svr.SessionData(ConnectionID, "path").ToUpper)
        Response = Response.Replace("$DIR$", svr.SessionData(ConnectionID, "dir").ToUpper)
        Response = Response.Replace("$TIME$", dtostr)
        Response = Response.Replace("$PORT$", svr.Port)

        With Assembly.GetExecutingAssembly().GetName()
            Response = Response.Replace("$APP$", .Name)
            Response = Response.Replace("$VER_X###$", .Version.Major)
            Response = Response.Replace("$VER_#X##$", .Version.Minor)
            Response = Response.Replace("$VER_##X#$", .Version.Build)
            Response = Response.Replace("$VER_###X$", .Version.Revision)
            Response = Response.Replace("$MACH$", Environment.MachineName)
            Response = Response.Replace("$OS$", Environment.OSVersion.Platform.ToString)
            Response = Response.Replace("$OSVER_X##$", Environment.OSVersion.Version.Major)
            Response = Response.Replace("$OSVER_#X#$", Environment.OSVersion.Version.Minor)
            Response = Response.Replace("$OSVER_##X$", Environment.OSVersion.Version.Build)
        End With

    End Sub

    Private Sub Decode(ByRef arg As String)
        If InStr(arg, "%") > 0 Then
            Dim hv() As String = Split(arg, "%")
            For i As Integer = 1 To UBound(hv)
                Dim h As String = Microsoft.VisualBasic.Left(hv(i), 2)
                Dim nv As String = Integer.Parse(h, Globalization.NumberStyles.HexNumber)
                arg = Replace(arg, "%" & h, Chr(nv))
            Next
        End If
    End Sub

#End Region

End Class
