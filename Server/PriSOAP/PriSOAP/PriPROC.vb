Imports ntSox
Imports System.Reflection
Imports System.Threading
Imports System.io
Imports System.Data.SqlClient

Public Class PriPROC

    Private Log As New Dictionary(Of String, Dictionary(Of Integer, LogItem))

#Region "Private Variables"

    Private WithEvents svr As New MyServer("127.0.0.1", 8021)
    Private ev As New ntEvtlog.evt
    Private ini As Priority.tabulaini
    Private StartTime As Date

#End Region

#Region "public properties"

    Private _AppName As String = Nothing
    Public Property AppName() As String
        Get
            Return _AppName
        End Get
        Set(ByVal value As String)
            _AppName = value
        End Set
    End Property

    Private _StartMessage As String
    Public Property StartMessage() As String
        Get
            Return _StartMessage
        End Get
        Set(ByVal value As String)
            _StartMessage = value
        End Set
    End Property

    Private _BasePath As String = Nothing
    Public ReadOnly Property BasePath() As String
        Get
            If IsNothing(_BasePath) Then
                Dim fullPath As String = System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase
                If InStr(fullPath, "file:///", CompareMethod.Text) > 0 Then
                    fullPath = Replace(fullPath, "file:///", "")
                End If
                If InStr(fullPath, "/", CompareMethod.Text) > 0 Then
                    fullPath = Replace(fullPath, "/", "\")
                End If
                _BasePath = fullPath.Substring(0, fullPath.LastIndexOf("\"))
                If Strings.Right(_BasePath, 1) <> "\" Then _BasePath += "\"

            End If
            Return _BasePath
        End Get
    End Property

    Private _Connstr As String
    Public ReadOnly Property ConnectionString() As String
        Get
            With My.Settings
                Return String.Format( _
                    "Data Source={0};Initial Catalog=system;User ID={1};Password={2}", _
                    .DATASOURCE, .PRIORITYUSER, .PRIORITYPWD _
                )
            End With
        End Get
    End Property

    Private _EnvSQL As String = Nothing
    Public ReadOnly Property EnvSQL() As String
        Get
            Return Strings.Format("select DNAME from system.dbo.ENVIRONMENT where DNAME <> ''", "")
        End Get
    End Property

#End Region

#Region "server Start / Stop"

    Protected Overrides Sub OnStart(ByVal args() As String)

    End Sub
    Protected Overrides Sub on(ByVal args() As String)

        StartTime = Now

        ' Add code here to start your service. This method should set things
        ' in motion so your service can do its work.        
        Try
            With Assembly.GetExecutingAssembly().GetName()
                AppName = .Name
                StartMessage = String.Format("+{0} @{5}:{11} Build: {1}.{4} Running: {6} ({7}.{8}.{9})", _
                    .Name, _
                    .Version.Major, _
                    .Version.Minor, _
                    .Version.Build, _
                    .Version.Revision, _
                    Environment.MachineName, _
                    Environment.OSVersion.Platform.ToString, _
                    Environment.OSVersion.Version.Major, _
                    Environment.OSVersion.Version.Minor, _
                    Environment.OSVersion.Version.Build, _
                    svr.IP, _
                    svr.Port _
                )
            End With

            With ev
                .AppName = AppName
                .LogMode = ntEvtlog.EvtLogMode.EventLog
                .RegisterLog()
                doArgs(args)
                .LogVerbosity = My.Settings.LogVerbosity
            End With

            ev.Log( _
                    String.Format( _
                        "Starting Priority Procedure Service...{0}" & _
                        "Running on [{1}:{2}].{0}" & _
                        "Logging level set to [{3}]{0}", _
                        vbCrLf, svr.IP, svr.Port, _
                        ev.DescribeVerbosity(ev.LogVerbosity) _
                    ), _
                    EventLogEntryType.Information, _
                    ntEvtlog.EvtLogVerbosity.Normal _
                )

            ' Get the Priority Directory from the tabula.ini
            With My.Settings

                ini = New Priority.tabulaini(ev)
                If ini.Loaded Then
                    Try
                        Dim tmp As String = ""
                        Dim sp() As String = Nothing

                        tmp = ini.iniValue("Environment", "Priority Directory")
                        tmp = Replace(tmp, "\system\prep", "")
                        tmp = Replace(tmp, "\\", "\")
                        .PRIORITYDIR = tmp
                        ev.Log( _
                            String.Format("Priority Directory is {0}", .PRIORITYDIR), _
                            EventLogEntryType.Information, _
                            ntEvtlog.EvtLogVerbosity.VeryVerbose _
                        )

                        ' Save the datasource
                        .DATASOURCE = ini.iniValue("Environment", "Tabula Host")

                        ' Check to see if there is a UNC path setup
                        If .PRIUNC.Length = 0 Then
                            ' No UNC path so let's guess it based on the ini...
                            tmp = ini.iniValue("Environment", "Priority RemoteRoot")
                            sp = Split(tmp, ("\"))
                            Dim root As String = sp(UBound(sp))
                            tmp = ini.iniValue("Environment", "Tabula Host")
                            sp = Split(tmp, ("\"))
                            Dim host As String = sp(0)
                            .PRIUNC = String.Format("\\{0}\{1}", host, root).ToUpper
                            ev.Log( _
                                String.Format("No Priority UNC share is defined so based your your tabula.ini file I guess it's here: [{0}]", .PRIUNC), _
                                EventLogEntryType.Information, _
                                ntEvtlog.EvtLogVerbosity.VeryVerbose _
                            )
                        End If
                        .Save()

                    Catch ex As Exception
                        ev.Log( _
                            String.Format("{0}", ex.Message), _
                            EventLogEntryType.Error, _
                            ntEvtlog.EvtLogVerbosity.Normal _
                        )
                    End Try
                End If

                Try
                    ' Check the Priority Directory is accessible
                    Dim f As New IO.DirectoryInfo(.PRIORITYDIR)
                    If Not f.Exists Then
                        Throw New Exception(String.Format("Priority directory {0} does not exist. Attempting to map...", .PRIORITYDIR))
                    End If
                    For Each di As DirectoryInfo In f.GetDirectories()
                        ' inspect each subfolder to ensure our mapped drive is live
                    Next

                Catch ex As Exception ' Our Priority drive is missing or unmapped.

                    ' Can't open the Priority Directory
                    ev.Log(Strings.Format("{0}", _
                        ex.Message), _
                        EventLogEntryType.FailureAudit, _
                        ntEvtlog.EvtLogVerbosity.VeryVerbose _
                    )

                    ' Let's see if we can map the drive                    
                    Dim out As String
                    Dim er As String

                    out = "" : er = ""
                    RunCmd(String.Format("net use {0} /DELETE", .PRIORITYDIR), out, er)
                    If er.Length > 0 Then
                        ev.Log( _
                            String.Format("Unmap of {0} failed because {1}.", .PRIORITYDIR, er), _
                            EventLogEntryType.FailureAudit, _
                            ntEvtlog.EvtLogVerbosity.VeryVerbose _
                        )
                    End If

                    out = "" : er = ""
                    RunCmd(String.Format("net use {0} {1}", .PRIORITYDIR, .PRIUNC), out, er)
                    If er.Length > 0 Then
                        ev.Log( _
                            String.Format("Mapping of {0} to {1} failed because {2}.{3}The command was [{4}].", .PRIORITYDIR, .PRIUNC, er, _
                            vbCrLf, String.Format("net use {0} {1}", .PRIORITYDIR, .PRIUNC)), _
                            EventLogEntryType.Error, _
                            ntEvtlog.EvtLogVerbosity.Normal _
                        )
                        ' Clear the saved path because it doesn't work
                        With My.Settings
                            .PRIUNC = ""
                            .Save()
                        End With
                    Else
                        ev.Log( _
                            String.Format("Mapped drive {0} to UNC share at {1}.", .PRIORITYDIR, .PRIUNC), _
                            EventLogEntryType.SuccessAudit, _
                            ntEvtlog.EvtLogVerbosity.Normal _
                        )
                    End If

                End Try

                ' Set default username ans password
                If .PRIORITYUSER.Length = 0 Then
                    .PRIORITYUSER = "tabula"
                End If

                If .PRIORITYPWD.Length = 0 Then
                    .PRIORITYPWD = "Tabula!"
                End If

            End With

            ' Start the service
            svr.StartSvc()

        Catch ex As Exception
            ev.Log( _
                String.Format("{0}", ex.Message), _
                EventLogEntryType.Error, _
                ntEvtlog.EvtLogVerbosity.Normal _
            )
        End Try

    End Sub

    Protected Overrides Sub OnStop()
        ' Add code here to perform any tear-down necessary to stop your service.
        Try
            ev.Log( _
                    String.Format("Stopping Priority Loading Service on [{0}:{1}] ", svr.IP, svr.Port), _
                    EventLogEntryType.Information, _
                    ntEvtlog.EvtLogVerbosity.Normal _
                )
            svr.StopSvc()

        Catch ex As Exception
            ev.Log( _
                String.Format("{0}", ex.Message), _
                EventLogEntryType.Error, _
                ntEvtlog.EvtLogVerbosity.Normal _
            )
        End Try
    End Sub

#End Region

#Region "client event handlers"

    Private Sub hOnConnect(ByVal ConnectionID As String) _
        Handles svr.OnConnect

        Try
            svr.SessionData(ConnectionID, "loggedon") = "false"
            svr.Send( _
                ConnectionID, _
                StartMessage _
            )

            ev.Log( _
                     String.Format("New inbound connection: [{0}]", _
                     ConnectionID), _
                     EventLogEntryType.Information, _
                     ntEvtlog.EvtLogVerbosity.Verbose _
            )

        Catch ex As Exception
            ev.Log( _
                ex.Message, _
                EventLogEntryType.Error, _
                ntEvtlog.EvtLogVerbosity.Normal _
            )
        End Try

    End Sub

    Private Sub hOnCommand(ByVal ConnectionID As String, ByVal CmdStr As String, ByVal Args() As String) _
        Handles svr.OnCommand

        Try
            ev.Log( _
                    String.Format("Rcvd command:{0}[{1}]{0}from Connection [{2}]", _
                    vbCrLf, CmdStr, ConnectionID), _
                    EventLogEntryType.Information, _
                    ntEvtlog.EvtLogVerbosity.VeryVerbose _
                )

            Select Case LCase(Args(0))
                Case "app"
                    svr.SessionData(ConnectionID, "app") = Args(1)
                    ev.Log( _
                            String.Format("Connection [{0}] identified itself as: [{1}].", _
                            ConnectionID, Args(1)), _
                            EventLogEntryType.Information, _
                            ntEvtlog.EvtLogVerbosity.VeryVerbose _
                        )

                Case "inetpub"
                    hConsoleMessage(ConnectionID, "Requesting environment update...")
                    ev.Log( _
                            String.Format("Connection [{0}] requested an environment update.", _
                            ConnectionID, Args(1)), _
                            EventLogEntryType.Information, _
                            ntEvtlog.EvtLogVerbosity.VeryVerbose _
                        )
                    Try
                        If UBound(Args) = 1 Then
                            MakeConfig(ConnectionID, Args(1))
                        Else
                            Throw New Exception("Invalid Syntax. Please specify Path.")
                        End If
                        hConsoleMessage(ConnectionID, "+inetpub: Updated config")
                    Catch ex As Exception
                        hConsoleMessage(ConnectionID, "-inetpub: " & ex.Message)
                    End Try
                    Me.EndLog(ConnectionID)

                Case "setenv"
                    hConsoleMessage(ConnectionID, "Setting environment...")
                    ev.Log( _
                            String.Format("Connection [{0}] requests update of environment settings.", _
                            ConnectionID, Args(1)), _
                            EventLogEntryType.Information, _
                            ntEvtlog.EvtLogVerbosity.VeryVerbose _
                        )
                    Try
                        If UBound(Args) = 3 Then
                            SetEnv(ConnectionID, Args(1), Args(2), Args(3))
                        Else
                            Throw New Exception("Invalid Syntax. Please specify Path, environment and verbosity.")
                        End If
                        hConsoleMessage(ConnectionID, "+setenv: Updated config")
                    Catch ex As Exception
                        hConsoleMessage(ConnectionID, "-setenv: " & ex.Message)
                    End Try
                    Me.EndLog(ConnectionID)

                Case "logs"
                    For Each l As String In Log.Keys
                        svr.Send(ConnectionID, l)
                    Next
                    svr.Send(ConnectionID, "+logs: Completed sucsessfully.")

                Case "log"
                    If UBound(Args) = 1 Then
                        svr.SessionData(ConnectionID, "log") = Args(1)
                        svr.Send(ConnectionID, "+log: Logging set for ID [" & Args(1) & "]")
                    Else
                        svr.Send(ConnectionID, "-log: Invalid Syntax - Missing ID.")
                    End If

                Case "readlog"
                    If UBound(Args) = 1 Then
                        If Log.ContainsKey(Args(1)) Then
                            svr.Send(ConnectionID, ">>")

                            Dim min As Integer = -1 : Dim max As Integer = -1
                            For Each rt As Integer In Log(Args(1)).Keys
                                If max = -1 Or rt > max Then max = rt
                                If min = -1 Or rt < min Then min = rt
                            Next

                            If max > -1 And min > -1 Then
                                For i As Integer = min To max
                                    svr.Send(ConnectionID, Log(Args(1))(i).Message)
                                Next
                                For i As Integer = min To max
                                    Log(Args(1)).Remove(i)
                                Next
                                svr.SessionData(ConnectionID, "RunTime") = Nothing

                            End If

                            svr.Send(ConnectionID, "<<")
                            svr.Send(ConnectionID, "+readlog: Ok.")
                        Else
                            svr.Send(ConnectionID, "-readlog: Invalid ID.")
                        End If
                    Else
                        svr.Send(ConnectionID, "-readlog: Invalid Syntax - Missing ID.")
                    End If


                Case "selftest"
                    Dim wap As New Priority.WINACTIVP(Me.ev)
                    With wap
                        .TimeOut = My.Settings.LOADTIMEOUT
                        .PRIORITYENV = "demo"
                        If UBound(Args) > 0 Then
                            .PRIORITYENV = Args(1)
                        End If

                        .PROC = "ZSFDC_TEST"
                        .ConnectionID = ConnectionID
                        If UBound(Args) > 1 Then
                            .PROC = Args(2)
                        End If

                        .PRIORITYDIR = My.Settings.PRIORITYDIR
                        .PRIORITYUSER = My.Settings.PRIORITYUSER
                        .PRIORITYPWD = My.Settings.PRIORITYPWD
                        AddHandler .ConsoleMessage, AddressOf hConsoleMessage

                        svr.Send(ConnectionID, String.Format("Starting test procedure [{0}] in environment [{1}]...", .PROC, .PRIORITYENV))
                        .Start()
                        If .result Then
                            svr.Send(ConnectionID, String.Format("+selftest:Completed Ok.", ""))
                        Else
                            svr.Send(ConnectionID, String.Format("-selftest:Test failed.", ""))
                        End If
                    End With
                    EndLog(ConnectionID)

                Case "proc"
                    If UBound(Args) = 2 Then
                        Dim wap As New Priority.WINACTIVP(Me.ev)
                        With wap
                            .TimeOut = My.Settings.LOADTIMEOUT
                            .PRIORITYENV = Args(1)
                            .PROC = Args(2)
                            .PRIORITYDIR = My.Settings.PRIORITYDIR
                            .PRIORITYUSER = My.Settings.PRIORITYUSER
                            .PRIORITYPWD = My.Settings.PRIORITYPWD
                            svr.Send(ConnectionID, String.Format("+proc: Ok Starting procedure [{0}] in environment [{1}]...", .PROC, .PRIORITYENV))
                        End With

                        Dim myThread As Thread
                        myThread = New Thread(New ThreadStart(AddressOf wap.Start))
                        myThread.Name = "WinActivP"
                        myThread.Start()
                    Else
                        svr.Send(ConnectionID, String.Format("-proc: invalid syntax. Please send [proc [environment] [procedure]]."))
                    End If

                Case "procwait"
                    If UBound(Args) = 2 Then
                        Dim wap As New Priority.WINACTIVP(Me.ev)
                        With wap
                            .TimeOut = My.Settings.LOADTIMEOUT
                            .PRIORITYENV = Args(1)
                            .PROC = Args(2)
                            .PRIORITYDIR = My.Settings.PRIORITYDIR
                            .PRIORITYUSER = My.Settings.PRIORITYUSER
                            .PRIORITYPWD = My.Settings.PRIORITYPWD
                            .Start()
                            If .result Then
                                svr.Send(ConnectionID, String.Format("+procwait: Procedure [{0}] in environment [{1}] was executed.", .PROC, .PRIORITYENV))
                            Else
                                svr.Send(ConnectionID, String.Format("-procwait: Procedure [{0}] in environment [{1}] failed to execute.", .PROC, .PRIORITYENV))
                            End If
                        End With

                    Else
                        svr.Send(ConnectionID, String.Format("-procwait: invalid syntax. Please send [proc [environment] [procedure]]."))
                    End If

                Case "resetpassword"
                    svr.Send(ConnectionID, "+resetpassword: ok")
                    With My.Settings
                        .PRIORITYUSER = "tabula"
                        .PRIORITYPWD = "Tabula!"
                        .Save()
                    End With

                Case "user"
                    svr.SessionData(ConnectionID, "user") = Args(1)
                    svr.Send(ConnectionID, "+user: ok Send password.")

                Case "pass"
                    With My.Settings
                        If Strings.StrComp(svr.SessionData(ConnectionID, "user"), .PRIORITYUSER, CompareMethod.Text) = 0 _
                            And Strings.StrComp(Args(1), .PRIORITYPWD, CompareMethod.Binary) = 0 Then
                            svr.SessionData(ConnectionID, "loggedon") = "true"
                            svr.Send(ConnectionID, "+pass: Welcome.")
                        Else
                            svr.Send(ConnectionID, "-pass: Bad username/password")
                        End If
                    End With

                Case "chuser"
                    If CBool(svr.SessionData(ConnectionID, "loggedon")) Then
                        With My.Settings
                            .PRIORITYUSER = Args(1)
                            .Save()
                            svr.Send(ConnectionID, "+chuser: Username changed.")
                        End With
                    Else
                        svr.Send(ConnectionID, "-chuser: You must be logged in to do that.")
                    End If

                Case "chpass"
                    If CBool(svr.SessionData(ConnectionID, "loggedon")) Then
                        With My.Settings
                            .PRIORITYPWD = Args(1)
                            .Save()
                            svr.Send(ConnectionID, "+chpass: Password changed.")
                        End With
                    Else
                        svr.Send(ConnectionID, "-chpass: You must be logged in to do that.")
                    End If

                Case "quit"
                    svr.KillConnection(ConnectionID)

                Case "timeout"
                    If CBool(svr.SessionData(ConnectionID, "loggedon")) Then
                        With My.Settings
                            If UBound(Args) = 0 Then
                                svr.Send(ConnectionID, String.Format("+timeout: {0} seconds.", .LOADTIMEOUT))
                            Else
                                .LOADTIMEOUT = CInt(Args(1))
                                .Save()
                                svr.Send(ConnectionID, String.Format("+timeout: Set to {0} seconds.", Args(1)))
                            End If
                        End With
                    Else
                        svr.Send(ConnectionID, "-timeout: You must be logged in to do that.")
                    End If

                Case "sysinfo"
                    hConsoleMessage(ConnectionID, Replace(Me.StartMessage, "+priproc", "+sysinfo", , , CompareMethod.Text))
                    EndLog(ConnectionID)

                Case "uptime"
                    hConsoleMessage(ConnectionID, _
                        myUptime(DateDiff(DateInterval.Second, StartTime, Now)) _
                    )
                    EndLog(ConnectionID)

                Case Else
                    If CBool(svr.SessionData(ConnectionID, "loggedon")) Then

                        Dim mappath As Boolean = False
                        Dim out As String = ""
                        Dim er As String = ""
                        Dim ret As String = ""

                        ' Are we mapping a path squire?
                        If LCase(Args(0)) = "net" Then
                            If UBound(Args) >= 3 Then
                                If LCase(Args(1)) = "use" Then
                                    If LCase(Args(2)) = LCase(My.Settings.PRIORITYDIR) Then
                                        ' oh, you are. Well done! Good luck, hope it works out for you.
                                        mappath = True
                                    End If
                                End If
                            End If
                        End If

                        For i As Integer = 0 To UBound(Args)
                            If InStr(Args(i), Chr(32)) > 0 Then
                                ret += Chr(34) & Args(i) & Chr(34)
                            Else
                                ret += Args(i)
                            End If
                            If i < UBound(Args) Then
                                ret += " "
                            End If
                        Next

                        RunCmd(ret, out, er)
                        If out.Length > 0 Then svr.Send(ConnectionID, out)
                        If er.Length > 0 Then
                            ' Ah well, better luck next time
                            svr.Send(ConnectionID, "-" & Args(0) & ": " & er)
                        Else
                            svr.Send(ConnectionID, "+" & Args(0) & ": " & er)
                            If mappath Then
                                ' It worked! Let's save that value for next time.
                                With My.Settings
                                    .PRIUNC = Args(3).ToUpper
                                    .Save()
                                    ev.Log( _
                                        String.Format("Mapped drive {0} to UNC share at {1}.", .PRIORITYDIR, .PRIUNC), _
                                        EventLogEntryType.SuccessAudit, _
                                        ntEvtlog.EvtLogVerbosity.Normal _
                                    )
                                End With
                            End If
                        End If

                    Else
                        svr.Send(ConnectionID, String.Format("-Command unknown [{0}]. Try logging on.", CmdStr))
                    End If

            End Select

        Catch ex As Exception
            ev.Log( _
                ex.Message, _
                EventLogEntryType.Error, _
                ntEvtlog.EvtLogVerbosity.Normal _
            )
            svr.Send(ConnectionID, ex.Message)
            EndLog(ConnectionID)
        End Try

    End Sub


#End Region

#Region "Private Functions"

    Private Sub hConsoleMessage(ByVal ConnectionID As String, ByVal str As String)
        svr.Send(ConnectionID, str)

        ' Are we a logging console squire?
        If Not IsNothing(svr.SessionData(ConnectionID, "log")) Then

            ' Create the log if non exists
            If Not Log.ContainsKey(svr.SessionData(ConnectionID, "log")) Then
                Log.Add(svr.SessionData(ConnectionID, "log"), New Dictionary(Of Integer, LogItem))
            End If

            If IsNothing(svr.SessionData(ConnectionID, "RunTime")) Then
                svr.SessionData(ConnectionID, "RunTime") = "0"
            End If
            ' Increment RunTime Counter 
            svr.SessionData(ConnectionID, "RunTime") = CStr(CInt(svr.SessionData(ConnectionID, "RunTime")) + 1)

            Dim l As New LogItem(svr.SessionData(ConnectionID, "RunTime"), str & "$$")
            Log(svr.SessionData(ConnectionID, "log")).Add(CInt(svr.SessionData(ConnectionID, "RunTime")), l)

        End If

    End Sub

    Private Sub EndLog(ByVal ConnectionID As String)

        ' Is there anybody thur?
        If Not IsNothing(svr.SessionData(ConnectionID, "log")) Then

            ' Increment RunTime Counter 
            svr.SessionData(ConnectionID, "RunTime") = CStr(CInt(svr.SessionData(ConnectionID, "RunTime")) + 1)

            Dim l As New LogItem(svr.SessionData(ConnectionID, "RunTime"), "||")
            Log(svr.SessionData(ConnectionID, "log")).Add(CInt(svr.SessionData(ConnectionID, "RunTime")), l)

        End If

    End Sub

    Private Sub doArgs(ByVal Args() As String)
        Dim state As String = ""
        Try
            For Each arg As String In Args
                With My.Settings
                    Select Case Left(arg, 1)
                        Case "/", "-"
                            Select Case LCase(Right(arg, arg.Length - 1))
                                Case "s", "soap", "service"
                                    state = "s"
                                Case "v-", "quiet"
                                    state = ""
                                    .Item("LogVerbosity") = _
                                        CInt(ntEvtlog.EvtLogVerbosity.Normal)
                                    .Save()
                                Case "v", "verbose"
                                    state = ""
                                    .Item("LogVerbosity") = _
                                        CInt(ntEvtlog.EvtLogVerbosity.Verbose)
                                    .Save()
                                Case "v+", "veryverbose"
                                    state = ""
                                    .Item("LogVerbosity") = _
                                        CInt(ntEvtlog.EvtLogVerbosity.VeryVerbose)
                                    .Save()
                                Case "v++", "arcane"
                                    state = ""
                                    .Item("LogVerbosity") = _
                                        CInt(ntEvtlog.EvtLogVerbosity.Arcane)
                                    .Save()
                                Case Else
                                    Throw New Exception(String.Format("Unknown Switch [{0}].", _
                                        LCase(Right(arg, arg.Length - 1))))
                            End Select
                        Case Else
                            Select Case state
                                Case "s", "soap", "service"
                                    .Item("PriLoadSVC_PriWebSvc_Service") = _
                                        arg
                                    .Save()
                                Case Else
                                    Throw New Exception( _
                                        String.Format("Unknown command [{0}].", _
                                            arg _
                                        ) _
                                    )
                            End Select
                    End Select
                End With
            Next
        Catch ex As Exception
            ev.Log( _
                ex.Message, _
                EventLogEntryType.Error, _
                ntEvtlog.EvtLogVerbosity.Normal _
            )
        End Try
    End Sub

    Private Sub RunCmd(ByVal cmd As String, ByRef out As String, ByRef er As String)

        Dim myProcess As Process = New Process()

        myProcess.StartInfo.FileName = "cmd.exe"
        myProcess.StartInfo.UseShellExecute = False
        myProcess.StartInfo.CreateNoWindow = True
        myProcess.StartInfo.RedirectStandardInput = True
        myProcess.StartInfo.RedirectStandardOutput = True
        myProcess.StartInfo.RedirectStandardError = True
        myProcess.Start()
        Dim sIn As StreamWriter = myProcess.StandardInput
        sIn.AutoFlush = True

        Dim sOut As StreamReader = myProcess.StandardOutput
        Dim sErr As StreamReader = myProcess.StandardError
        sIn.Write(cmd & _
           System.Environment.NewLine)
        sIn.Write("exit" & System.Environment.NewLine)
        out = sOut.ReadToEnd()
        er = sErr.ReadToEnd
        If Not myProcess.HasExited Then
            myProcess.Kill()
        End If

        sIn.Close()
        sOut.Close()
        sErr.Close()
        myProcess.Close()

        out = Split(Split(out, cmd & vbCrLf)(1), "exit")(0)
        Dim pt As String = Split(out, vbCrLf)(UBound(Split(out, vbCrLf)))

        out = Replace(out, pt, "")
        Do While InStr(out, vbCrLf & vbCrLf) > 0
            out = Replace(out, vbCrLf & vbCrLf, vbCrLf)
        Loop
        er = Replace(er, pt, "")
        Do While InStr(er, vbCrLf & vbCrLf) > 0
            er = Replace(er, vbCrLf & vbCrLf, vbCrLf)
        Loop

        If Right(out, 2) = vbCrLf Then out = Left(out, out.Length - 2)
        If Right(er, 2) = vbCrLf Then er = Left(er, er.Length - 2)

        If IsNothing(er) Then er = ""
        If IsNothing(out) Then out = ""

    End Sub

    Private Sub MakeConfig(ByVal ConnectionID As String, ByVal InetpubDir As String)

        Dim command As SqlCommand = Nothing
        Dim dataReader As SqlDataReader
        Dim env() As String = Nothing
        Dim Connection As SqlConnection

        With My.Settings
            If Not File.Exists(InetpubDir) Then
                Throw New Exception(String.Format("Configuration file [{0}] not found.", _
                    InetpubDir))
            End If
            Try
                Connection = New SqlConnection(ConnectionString)
                Connection.Open()
            Catch ex As Exception
                Throw New Exception(String.Format("Could not open connection to [{0}]. " & _
                    "The error was [{1}].", .DATASOURCE, ex.Message))
            End Try

            command = Connection.CreateCommand()
            command.CommandText = EnvSQL

            Try
                dataReader = command.ExecuteReader()
            Catch ex As Exception
                Connection.Close()
                Throw New Exception(String.Format("Could not get environment data from [{0}]." & _
                    "The error was [{1}].", .DATASOURCE, ex.Message))
            End Try

            If Not dataReader.HasRows Then
                Connection.Close()
                Throw New Exception(String.Format("No Priority Environments are listed from [{0}].", .DATASOURCE))
            End If

        End With

        While dataReader.Read
            Try
                ReDim Preserve env(UBound(env) + 1)
            Catch
                ReDim env(0)
            Finally
                env(UBound(env)) = dataReader.Item(0)
            End Try

        End While
        Connection.Close()

        Dim str As String = ""
        Dim strLeft As String = ""
        Dim strRight As String = ""

        Using sr As New StreamReader(InetpubDir)
            With sr
                str = .ReadToEnd
            End With
        End Using
        If Not (InStr(str, "<connectionStrings>", CompareMethod.Text) > 0 And InStr(str, "</connectionStrings>", CompareMethod.Text) > 0) Then
            Throw New Exception(String.Format("Invalid Configuration file [{0}].", InetpubDir))
        End If

        strLeft = Split(str, "<connectionStrings>")(0)
        strRight = Split(str, "</connectionStrings>")(1)

        While Right(strLeft, 2) = vbCrLf
            strLeft = Left(strLeft, strLeft.Length - 2)
        End While

        While Left(strRight, 2) = vbCrLf
            strRight = Right(strRight, strRight.Length - 2)
        End While

        Using sw As New StreamWriter(InetpubDir)
            With sw
                .WriteLine(strLeft)
                .WriteLine("<connectionStrings>")
                For i As Integer = 0 To UBound(env)
                    With My.Settings
                        sw.WriteLine(Chr(9) & String.Format( _
                            "<add name={0}{2}{0} connectionString={0}Data Source={1};Initial Catalog={2};User ID={3};Password={4}{0} providerName={0}System.Data.SqlClient{0}/>", _
                            Chr(34), .DATASOURCE, env(i), .PRIORITYUSER, .PRIORITYPWD))
                        hConsoleMessage(ConnectionID, String.Format("Adding environment [{2}] to the config file with connection string: <Data Source={1};Initial Catalog={2};User ID={3};Password={4}>.", _
                            Chr(34), .DATASOURCE, env(i), .PRIORITYUSER, .PRIORITYPWD))
                    End With
                Next
                .WriteLine("</connectionStrings>")
                .WriteLine(strRight)
            End With
        End Using

    End Sub

    Private Sub SetEnv(ByVal ConnectionID As String, ByVal InetpubDir As String, ByVal Env As String, ByVal verbosity As String)

        Dim str As String = ""
        Dim strLeft As String = ""
        Dim strRight As String = ""

        If Not File.Exists(InetpubDir) Then
            Throw New Exception(String.Format("Configuration file [{0}] not found.", _
                InetpubDir))
        End If

        Using sr As New StreamReader(InetpubDir)
            With sr
                str = .ReadToEnd
            End With
        End Using
        If Not (InStr(str, "<appSettings>", CompareMethod.Text) > 0 And InStr(str, "</appSettings>", CompareMethod.Text) > 0) Then
            Throw New Exception(String.Format("Invalid Configuration file [{0}].", InetpubDir))
        End If

        strLeft = Split(str, "<appSettings>")(0)
        strRight = Split(str, "</appSettings>")(1)

        While Right(strLeft, 2) = vbCrLf
            strLeft = Left(strLeft, strLeft.Length - 2)
        End While

        While Left(strRight, 2) = vbCrLf
            strRight = Right(strRight, strRight.Length - 2)
        End While

        Dim sett() As String = Split(Split(Split(str, "<appSettings>")(1), "</appSettings>")(0), "/>")

        Using sw As New StreamWriter(InetpubDir)
            With sw
                .WriteLine(strLeft)
                .WriteLine("<appSettings>")
                For Each s As String In sett
                    If InStr(s, String.Format("key={0}Environment{0}", Chr(34))) > 0 Then
                        sw.WriteLine(String.Format( _
                            "{2}<add key={0}Environment{0} value={0}{1}{0} />", _
                            Chr(34), Env, Chr(9)))
                    ElseIf InStr(s, String.Format("key={0}LogVerbosity{0}", Chr(34))) > 0 Then
                        sw.WriteLine(String.Format( _
                            "{2}<add key={0}LogVerbosity{0} value={0}{1}{0} />", _
                            Chr(34), verbosity, Chr(9)))
                    Else
                        If InStr(s, "<add") > 0 Then
                            sw.WriteLine(Chr(9) & Replace(Replace(s, Chr(9), ""), vbCrLf, "") & "/>")
                        Else
                            If Len(Replace(Replace(s, Chr(9), ""), vbCrLf, "")) > 0 Then
                                sw.WriteLine(Chr(9) & Replace(Replace(s, Chr(9), ""), vbCrLf, ""))
                            End If
                        End If
                    End If
                Next
                .WriteLine("</appSettings>")
                .WriteLine(strRight)
            End With
        End Using

    End Sub

#Region "Uptime functions"

    Private Function myUptime(ByVal sec As Integer) As String

        Dim tstr As String = ""
        Dim _secMin As Integer = 60
        Dim _secHour As Integer = 60 * _secMin
        Dim _secDay As Integer = 24 * _secHour
        Dim _secWeek As Integer = 7 * _secDay
        Dim _secMonth As Integer = 4 * _secWeek
        Dim _secyear As Integer = 12 * _secMonth

        Dim num(6) As String
        num(0) = formatDatePart(sec, "year", _secyear)
        num(1) = formatDatePart(sec, "month", _secMonth)
        num(2) = formatDatePart(sec, "week", _secWeek)
        num(3) = formatDatePart(sec, "day", _secDay)
        num(4) = formatDatePart(sec, "hour", _secHour)
        num(5) = formatDatePart(sec, "minute", _secMin)
        num(6) = formatDatePart(sec, "second", 1)

        Dim td = textdate(num)
        For i As Integer = 0 To UBound(num)
            If num(i).Length = 0 Then
                num(i) = 0
            End If
        Next

        Return String.Format( _
            "+Uptime: " & td, _
            num(0).ToString, _
            num(1).ToString, _
            num(2).ToString, _
            num(3).ToString, _
            num(4).ToString, _
            num(5).ToString, _
            num(6).ToString _
        )

    End Function

    Private Function formatDatePart(ByRef sec As Integer, ByVal description As String, ByVal SecVar As Integer) As String
        Dim ret As String = ""
        If sec >= SecVar Then
            ret += Int(sec / SecVar).ToString & " " & description
            If sec > (SecVar * 2) Then ret += "s"
            sec = sec - (Int(sec / SecVar) * SecVar)
        Else
            ret += "0 " & description & "s"
        End If
        Return ret
    End Function

    Private Function textdate(ByVal num() As String) As String
        Dim first As Boolean = False
        Dim ret As String = ""
        For i As Integer = 0 To UBound(num) - 1
            If first Or Left(num(i), 1) <> "0" Then
                first = True
                ret += "{" & i.ToString & "}"
                If i < UBound(num) - 1 Then
                    ret += ", "
                Else
                    ret += " "
                End If
            End If
        Next

        For i As Integer = 0 To UBound(num) - 1
            If Left(num(i), 1) <> "0" Then
                ret += "and "
                Exit For
            End If
        Next

        ret += "{" & UBound(num).ToString & "}. "
        Return ret

    End Function

#End Region

#End Region

End Class
Public Class LogItem

    Public Sub New(ByVal Runtime As Integer, ByVal Message As String)
        _runtime = Runtime
        _Message = Message
    End Sub

    Private _runtime As Integer
    Public Property RunTime() As Integer
        Get
            Return _runtime
        End Get
        Set(ByVal value As Integer)
            _runtime = value
        End Set
    End Property

    Private _Message As String
    Public Property Message() As String
        Get
            Return _Message
        End Get
        Set(ByVal value As String)
            _Message = value
        End Set
    End Property

End Class