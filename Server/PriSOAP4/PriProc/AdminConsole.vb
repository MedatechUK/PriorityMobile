Imports priority

Module AdminConsole

    Public WithEvents svr As MyServer
    Private cli As New Dictionary(Of String, Cli)

#Region "Supported Commands"

    Private ReadOnly Property commands() As Dictionary(Of String, String)
        Get

            ' TODO: Add new commands here with a short description
            '       Long descriptions should be added to the hBuilder 

            Dim ret As New Dictionary(Of String, String)
            With ret
                .Add("user", "user {username}: Begins a login request.")
                .Add("pass", "pass {password}: Provide a password for the login request.")
                .Add("resetpassword", "Resets the username and password to factory defaults.")
                .Add("startargs", "Lists command line start-up arguments for the service.")
                .Add("chuser", "chuser {new username}: Change the Priority user name.")
                .Add("chpass", "chpass {new password}: Change the Priority password.")
                .Add("config", "Begin system auto-configuration.")
                .Add("mappath", "mappath {drive}: \\{server}\{share}: Maps a drive to Priority.")
                .Add("sysinfo", "Display the system information.")
                .Add("selftest", "Run the self test tool.")
                .Add("log", "Enter / exit logging mode. Windows events are displayed in logging mode.")
                .Add("testpost", "testpost {db_name}: Send a test loading to the specified db, or to default db if ommitted.")
                .Add("pause", "Pause the service, causing bubbles to stack in the bubble queue.")
                .Add("resume", "Resumes the service, processing all bubbles stored in the queue.")
                .Add("uptime", "Displays how long since the system last restarted.")
                .Add("quit", "Disconnect your session.")
            End With
            Return ret
        End Get
    End Property

#End Region

#Region "Command line interface event handlers"

    Private Sub hCliResponse(ByVal ConnectionID As String, ByVal Data As String)
        svr.Send(ConnectionID, Data, False)
    End Sub

    Private Sub hCliExit(ByVal ConnectionID As String)
        cli.Remove(ConnectionID)
        svr.KillConnection(ConnectionID)
    End Sub

#End Region

#Region "client event handlers"

    Private Sub hOnConnect(ByVal ConnectionID As String) _
        Handles svr.OnConnect

        Try
            svr.SessionData(ConnectionID, "loggedon") = "false"
            svr.SessionData(ConnectionID, "logmode") = "false"
            svr.Send( _
                ConnectionID, _
                Sysinfo _
            )

            ev.LogVerbosity = ApplicationSetting("LogVerbosity")
            ev.Log( _
                     String.Format("New inbound connection: [{0}]", _
                     ConnectionID), _
                     EventLogEntryType.Information, _
                     ntEvtlog.EvtLogVerbosity.Arcane _
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

        Dim hBuilder As New Builder
        Dim help As Boolean = False
        Dim DOScmd As Boolean = False

        Try
            Try
                If Not NoIIS Then
                    ev.LogVerbosity = ApplicationSetting("LogVerbosity")
                Else
                    ev.LogVerbosity = ntEvtlog.EvtLogVerbosity.VeryVerbose
                End If
            Catch
                ev.LogVerbosity = ntEvtlog.EvtLogVerbosity.VeryVerbose
            End Try

            ev.Log( _
                    String.Format("Rcvd command:{0}[{1}]{0}from Connection [{2}]", _
                    vbCrLf, CmdStr, ConnectionID), _
                    EventLogEntryType.Information, _
                    ntEvtlog.EvtLogVerbosity.Arcane _
                )

            Dim ord As Integer = 0
            Select Case Args(0).ToUpper
                Case "HELP", "?"
                    If Not CBool(svr.SessionData(ConnectionID, "loggedon")) Then
                        svr.SendFormat(ConnectionID, "-{0}: You must be logged in to do that.", Args(0).ToLower)
                        Exit Select
                    End If
                    If UBound(Args) > 0 Then
                        If commands.Keys.Contains(Args(1)) Then
                            hBuilder.AppendFormat("+Help: {0}", Args(1).ToUpper).AppendLine()
                            hBuilder.AppendFormat("{0}", commands(Args(1)))
                            help = True
                            ord = 1
                        End If
                    End If
            End Select

            Select Case LCase(Args(ord))

                Case "startargs"
                    If Not CBool(svr.SessionData(ConnectionID, "loggedon")) Then
                        Exit Select
                    End If

                    If UBound(Args) > 0 Then
                        ' Unsupported command
                        DOScmd = True
                        cli(ConnectionID).Send(String.Format("HELP {0}", Args(1)))
                        Exit Select
                    End If

                    hBuilder.AppendFormat("+startup: Start-up parameters: ", "").AppendLine.AppendLine()
                    For Each c As String In Arguments.Keys
                        hBuilder.AppendFormat("-{0} {1}", (c.ToUpper & Space(20)).Substring(0, 14), Arguments(c)).AppendLine()
                    Next
                    help = True

                Case "help", "?"
                    If Not CBool(svr.SessionData(ConnectionID, "loggedon")) Then
                        Exit Select
                    End If

                    If UBound(Args) > 0 Then
                        ' Unsupported command
                        DOScmd = True
                        cli(ConnectionID).Send(String.Format("HELP {0}", Args(1)))
                        Exit Select
                    End If

                    hBuilder.AppendFormat("+Help: Supported Commands: ", "").AppendLine.AppendLine()
                    For Each c As String In commands.Keys
                        hBuilder.AppendFormat("{0} {1}", (c.ToUpper & Space(20)).Substring(0, 15), commands(c)).AppendLine()
                    Next
                    hBuilder.AppendLine()
                    hBuilder.AppendFormat("Try: help [command] for more information. ", "")
                    help = True

                Case "resetpassword"
                    If help Then
                        'hBuilder.Append("")
                        Exit Select
                    End If
                    svr.Send(ConnectionID, "+resetpassword: ok")
                    With My.Settings
                        .PRIORITYUSER = "tabula"
                        .PRIORITYPWD = "Tabula!"
                        .Save()
                    End With

                Case "user"
                    If help Then
                        'hBuilder.Append("")
                        Exit Select
                    End If
                    svr.SessionData(ConnectionID, "user") = Args(1)
                    svr.Send(ConnectionID, "+user: ok Send password.")

                Case "pass"
                    If help Then
                        'hBuilder.Append("")
                        Exit Select
                    End If
                    With My.Settings
                        If Strings.StrComp(svr.SessionData(ConnectionID, "user"), .PRIORITYUSER, CompareMethod.Text) = 0 _
                            And Strings.StrComp(Args(1), .PRIORITYPWD, CompareMethod.Binary) = 0 Then
                            With svr
                                .SessionData(ConnectionID, "loggedon") = "true"
                                .Send(ConnectionID, "+pass: Welcome.")
                                .Send(ConnectionID, "")
                                Threading.Thread.Sleep(100)
                                svr.Send(ConnectionID, Sysinfo)
                                Try
                                    ' IIS location test
                                    .SendFormat(ConnectionID, "Connected to IIS folder at [{0}]", iisFolder)
                                    ' Database/UNC Connection tests
                                    With My.Settings
                                        Connection = New GenericConnection(.PROVIDER, .DATASOURCE, .PRIORITYUSER, .PRIORITYPWD)
                                        If .DATASOURCE.Length = 0 Then Throw New Exception("No datasource specified.")
                                        Dim dr As NetworkDrive = GetNetworkDrive(.PRIUNC)
                                        If .PRIUNC.Length = 0 Then Throw New Exception("No UNC Fileshare specified.")
                                        If IsNothing(dr) Then Throw New Exception(String.Format("Priority Fileshare [{0}] is unmapped.", .PRIUNC))
                                        If Not String.Compare(dr.DriveLetter, .PRIORITYDIR, True) = 0 Then Throw New Exception(String.Format("Drive [{0}] is not mapped to [{1}].", .PRIORITYDIR, .PRIUNC))
                                        If Not dr.info.IsReady Then Throw New Exception(String.Format("Priority mapped drive [{0}] is not ready.", dr.DriveLetter))

                                        svr.SendFormat(ConnectionID, "PriorityDrive = {0}", .PRIORITYDIR)
                                        svr.SendFormat(ConnectionID, "PriorityShare = {0}", .PRIUNC)
                                        svr.SendFormat(ConnectionID, "Datasource = {0}", .DATASOURCE)
                                        Select Case .PROVIDER
                                            Case eProviderType.MSSQL
                                                svr.SendFormat(ConnectionID, "Data Provider = {0}", "MSSQL")
                                            Case eProviderType.ORACLE
                                                svr.SendFormat(ConnectionID, "Data Provider = {0}", "Oracle")
                                        End Select

                                        ' Attempt connection to database
                                        svr.Send(ConnectionID, String.Format("Connecting to database [{0}]...", .DATASOURCE), False)
                                        If Not Connection.State = ConnectionState.Open Then
                                            Connection.Open()
                                        End If
                                        svr.Send(ConnectionID, "OK!")
                                        svr.Send(ConnectionID, "System initialised.")
                                        svr.Send(ConnectionID, "")

                                        ApplicationSetting("DSN") = Connection.ConnectionString
                                        ApplicationSetting("PROVIDER") = Connection.Provider

                                    End With


                                Catch ConnectionException As Exception
                                    .Send(ConnectionID, "Initialisation exception encountered:")
                                    .SendFormat(ConnectionID, "{0}", ConnectionException.Message)

                                Finally
                                    Dim qState As String = "The bubble queue is NOT RUNNING!"
                                    If Not IsNothing(lEv) Then
                                        If lEv.qStarted Then
                                            qState = "The bubble queue is RUNNING."
                                        End If
                                    End If
                                    .SendFormat(ConnectionID, "{0}", qState)

                                    svr.SendFormat(ConnectionID, "Starting shell session [{0}]...", ConnectionID)
                                    cli.Add(ConnectionID, New Cli(ConnectionID))
                                    AddHandler cli(ConnectionID).Response, AddressOf hCliResponse
                                    AddHandler cli(ConnectionID).Exited, AddressOf hCliExit
                                    DOScmd = True

                                End Try
                            End With

                        Else
                            svr.Send(ConnectionID, "-pass: Bad username/password")
                        End If
                    End With

                Case "chuser"
                    If help Then
                        'hBuilder.Append("")
                        Exit Select
                    End If
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
                    If help Then
                        'hBuilder.Append("")
                        Exit Select
                    End If
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
                    If help Then
                        'hBuilder.Append("")
                        Exit Select
                    End If
                    DOScmd = True
                    If cli.Keys.Contains(ConnectionID) Then
                        svr.Send(ConnectionID, String.Format("+quit: terminating shell session [{0}]...", ConnectionID))
                        cli(ConnectionID).Quit()
                    Else
                        svr.Send(ConnectionID, "+quit: Bye!")
                        svr.KillConnection(ConnectionID)
                    End If

                Case "beep"
                    If Not CBool(svr.SessionData(ConnectionID, "loggedon")) Then
                        svr.SendFormat(ConnectionID, "-{0}: You must be logged in to do that.", Args(0).ToLower)
                        Exit Select
                    End If
                    If help Then
                        'hBuilder.Append("")
                        Exit Select
                    End If
                    Beep()

                Case "config"
                    If Not CBool(svr.SessionData(ConnectionID, "loggedon")) Then
                        svr.SendFormat(ConnectionID, "-{0}: You must be logged in to do that.", Args(0).ToLower)
                        Exit Select
                    End If
                    If help Then
                        'hBuilder.Append("")
                        Exit Select
                    End If
                    If Connection.Provider = eProviderType.ORACLE Then
                        svr.SendFormat(ConnectionID, "-{0}: Auto configuration not supported on Oracle.", Args(0).ToLower)
                        Exit Select
                    Else
                        If CBool(svr.SessionData(ConnectionID, "loggedon")) Then
                            Dim ToConsole As New Builder(ConnectionID)
                            Dim et As ntEvtlog.LogEntryType = ntEvtlog.LogEntryType.SuccessAudit
                            Dim verb As ntEvtlog.EvtLogVerbosity = ntEvtlog.EvtLogVerbosity.Verbose
                            ToConsole.Append("Starting configurator from console...").AppendLine()
                            Try
                                Configure(ToConsole)

                            Catch ex As Exception
                                ToConsole.AppendFormat("{0}", ex.Message).AppendLine()
                                et = ntEvtlog.LogEntryType.Err
                                verb = ntEvtlog.EvtLogVerbosity.Normal
                            Finally
                                ev.Log(ToConsole.ToString, et, verb)
                            End Try

                        Else
                            svr.Send(ConnectionID, "-config: You must be logged in to do that.")
                        End If
                    End If

                Case "mappath"
                    If Not CBool(svr.SessionData(ConnectionID, "loggedon")) Then
                        svr.SendFormat(ConnectionID, "-{0}: You must be logged in to do that.", Args(0).ToLower)
                        Exit Select
                    End If
                    If help Then
                        'hBuilder.Append("")
                        Exit Select
                    End If
                    Try
                        If UBound(Args) = 0 Then
                            DOScmd = True
                            svr.SendFormat(ConnectionID, "+mappath: {0} is mapped to {1}.", My.Settings.PRIORITYDIR, My.Settings.PRIUNC)
                            cli(ConnectionID).Send("net use")
                        Else
                            If Not (UBound(Args) = 2) Then Throw New Exception("Invalid syntax. Should be MAPPATH {drive}: ""\\{Server}\{Share}""")
                            If Not (Args(1).Length = 2 And Args(1).Substring(1, 1) = ":") Then Throw New Exception("Invalid syntax. Not a valid Drive Letter.")
                            If Not (Args(2).Substring(0, 2) = "\\" And UBound(Args(2).Split("\")) = 3) Then Throw New Exception("Invalid syntax. Not a valid UNC Share name.")

                            Dim ToConsole As New Builder(ConnectionID)
                            Dim et As ntEvtlog.LogEntryType = ntEvtlog.LogEntryType.SuccessAudit
                            Dim verb As ntEvtlog.EvtLogVerbosity = ntEvtlog.EvtLogVerbosity.Verbose
                            ToConsole.Append("Mapping a drive from the console...").AppendLine()
                            Try
                                MapDriveLetter(Args(1), Args(2), ToConsole)
                                With My.Settings
                                    .PRIORITYDIR = Args(1)
                                    .PRIUNC = Args(2)
                                    .Save()
                                End With

                            Catch ex As Exception
                                ToConsole.AppendFormat("{0}", ex.Message).AppendLine()
                                et = ntEvtlog.LogEntryType.Err
                                verb = ntEvtlog.EvtLogVerbosity.Normal
                            Finally
                                ev.Log(ToConsole.ToString, et, verb)
                            End Try
                        End If
                    Catch EX As Exception
                        svr.SendFormat(ConnectionID, "-mappath: {0}", EX.Message)
                    End Try

                Case "sysinfo"
                    If help Then
                        'hBuilder.Append("")
                        Exit Select
                    End If
                    svr.Send( _
                        ConnectionID, _
                        Sysinfo _
                    )

                Case "selftest"
                    If Not CBool(svr.SessionData(ConnectionID, "loggedon")) Then
                        svr.SendFormat(ConnectionID, "-{0}: You must be logged in to do that.", Args(0).ToLower)
                        Exit Select
                    End If
                    If help Then
                        'hBuilder.Append("")
                        Exit Select
                    End If
                    With svr
                        .Send(ConnectionID, Sysinfo)
                        Try
                            ' iis Connectivity test
                            .SendFormat(ConnectionID, "Connected to IIS folder at [{0}]", iisFolder)
                            ' Database/UNC Connection tests
                            With My.Settings
                                Connection = New GenericConnection(.PROVIDER, .DATASOURCE, .PRIORITYUSER, .PRIORITYPWD)
                                If .DATASOURCE.Length = 0 Then Throw New Exception("No datasource specified.")
                                Dim dr As NetworkDrive = GetNetworkDrive(.PRIUNC)
                                If .PRIUNC.Length = 0 Then Throw New Exception("No UNC Fileshare specified.")
                                If IsNothing(dr) Then Throw New Exception(String.Format("Priority Fileshare [{0}] is unmapped.", .PRIUNC))
                                If Not String.Compare(dr.DriveLetter, .PRIORITYDIR, True) = 0 Then Throw New Exception(String.Format("Drive [{0}] is not mapped to [{1}].", .PRIORITYDIR, .PRIUNC))
                                If Not dr.info.IsReady Then Throw New Exception(String.Format("Priority mapped drive [{0}] is not ready.", dr.DriveLetter))

                                svr.SendFormat(ConnectionID, "PriorityDrive = {0}", .PRIORITYDIR)
                                svr.SendFormat(ConnectionID, "PriorityShare = {0}", .PRIUNC)
                                svr.SendFormat(ConnectionID, "Datasource = {0}", .DATASOURCE)
                                Select Case .PROVIDER
                                    Case eProviderType.MSSQL
                                        svr.SendFormat(ConnectionID, "Data Provider = {0}", "MSSQL")
                                    Case eProviderType.ORACLE
                                        svr.SendFormat(ConnectionID, "Data Provider = {0}", "Oracle")
                                End Select

                                ' Attempt connection to database
                                svr.Send(ConnectionID, String.Format("Connecting to database [{0}]...", .DATASOURCE), False)
                                If Not Connection.State = ConnectionState.Open Then
                                    Connection.Open()
                                    ApplicationSetting("DSN") = Connection.ConnectionString
                                    ApplicationSetting("PROVIDER") = Connection.Provider
                                End If
                                svr.Send(ConnectionID, "OK!")
                                svr.Send(ConnectionID, "System initialised.")
                                svr.Send(ConnectionID, "")
                            End With

                        Catch ConnectionException As Exception
                            .Send(ConnectionID, "Initialisation exception encountered:")
                            .SendFormat(ConnectionID, "{0}", ConnectionException.Message)
                        Finally
                            Dim qState As String = "The bubble queue is NOT RUNNING!"
                            If Not IsNothing(lEv) Then
                                If lEv.qStarted Then
                                    qState = "The bubble queue is RUNNING."
                                End If
                            End If
                            .SendFormat(ConnectionID, "{0}", qState)
                        End Try

                    End With

                Case "testpost"
                    If Not CBool(svr.SessionData(ConnectionID, "loggedon")) Then
                        svr.SendFormat(ConnectionID, "-{0}: You must be logged in to do that.", Args(0).ToLower)
                        Exit Select
                    End If
                    If help Then
                        'hBuilder.Append("")
                        Exit Select
                    End If
                    Using xl As New priority.Loading
                        With xl
                            Try
                                .Table = "ZSFDC_TABLE"
                                .Procedure = "ZSFDC_TEST"
                                svr.SendFormat(ConnectionID, "Sending test bubble to table {0} for procedure {1}...", .Table, .Procedure)
                                If UBound(Args) > 0 Then
                                    .Environment = Args(1)
                                    svr.SendFormat(ConnectionID, "Using environment [{0}]", Args(1))
                                Else
                                    svr.Send(ConnectionID, "Using system default environment.")
                                End If

                                .AddColumn(1) = New LoadColumn("USERNAME", tColumnType.typeCHAR)
                                .AddColumn(1) = New LoadColumn("WARHS", tColumnType.typeCHAR)
                                .AddColumn(1) = New LoadColumn("BIN", tColumnType.typeCHAR)
                                .AddColumn(1) = New LoadColumn("CURDATE", tColumnType.typeDATE)
                                .AddColumn(2) = New LoadColumn("PART", tColumnType.typeCHAR)
                                .AddColumn(2) = New LoadColumn("STATUS", tColumnType.typeCHAR)
                                .AddColumn(2) = New LoadColumn("CQUANT", tColumnType.typeINT)

                                .AddRecordType(1) = New LoadRow("user", "Main", "0", Now.ToString)
                                .AddRecordType(2) = New LoadRow("PART123", "Goods", "1")
                                .AddRecordType(2) = New LoadRow("PART321", "Goods", "1")

                                Dim exp As New Exception
                                If Not .Post("http://localhost:8080", exp) Then Throw exp

                                svr.Send(ConnectionID, "+testpost: Bubble sent.")
                            Catch ex As Exception
                                svr.SendFormat(ConnectionID, "-testpost: {0}", ex.Message)
                            End Try

                        End With
                    End Using

                Case "pause"
                    If Not CBool(svr.SessionData(ConnectionID, "loggedon")) Then
                        svr.SendFormat(ConnectionID, "-{0}: You must be logged in to do that.", Args(0).ToLower)
                        Exit Select
                    End If
                    If help Then
                        'hBuilder.Append("")
                        Exit Select
                    End If
                    Dim SVCCtrl As New System.ServiceProcess.ServiceController(AppName)
                    Select Case SVCCtrl.Status
                        Case ServiceProcess.ServiceControllerStatus.Running
                            svr.SendFormat(ConnectionID, "Sending PAUSE to service...", "")
                            SVCCtrl.Pause()
                            While Not SVCCtrl.Status = ServiceProcess.ServiceControllerStatus.Paused
                                Threading.Thread.Sleep(100)
                                SVCCtrl = New System.ServiceProcess.ServiceController(AppName)
                            End While
                            svr.SendFormat(ConnectionID, "Service PAUSEd.", "")
                        Case ServiceProcess.ServiceControllerStatus.PausePending
                            svr.SendFormat(ConnectionID, "I'm doing it already!", "")
                        Case ServiceProcess.ServiceControllerStatus.Paused
                            svr.SendFormat(ConnectionID, "Service is already PAUSEd.", "")
                        Case ServiceProcess.ServiceControllerStatus.ContinuePending, ServiceProcess.ServiceControllerStatus.StartPending, ServiceProcess.ServiceControllerStatus.StopPending
                            svr.SendFormat(ConnectionID, "Service is PENDING another operation.", "")
                        Case Else
                            svr.SendFormat(ConnectionID, "wtf?", "")
                    End Select

                Case "resume"
                    If Not CBool(svr.SessionData(ConnectionID, "loggedon")) Then
                        svr.SendFormat(ConnectionID, "-{0}: You must be logged in to do that.", Args(0).ToLower)
                        Exit Select
                    End If
                    If help Then
                        'hBuilder.Append("")
                        Exit Select
                    End If
                    Dim SVCCtrl As New System.ServiceProcess.ServiceController(AppName)
                    Select Case SVCCtrl.Status
                        Case ServiceProcess.ServiceControllerStatus.Paused
                            svr.SendFormat(ConnectionID, "Sending CONTINUE to service...", "")
                            SVCCtrl.Continue()
                            While Not SVCCtrl.Status = ServiceProcess.ServiceControllerStatus.Running
                                Threading.Thread.Sleep(100)
                                SVCCtrl = New System.ServiceProcess.ServiceController(AppName)
                            End While
                            svr.SendFormat(ConnectionID, "Service CONTINUEd.", "")
                        Case ServiceProcess.ServiceControllerStatus.ContinuePending
                            svr.SendFormat(ConnectionID, "I'm doing it already!", "")
                        Case ServiceProcess.ServiceControllerStatus.Running
                            svr.SendFormat(ConnectionID, "Service is already CONTINUEd.", "")
                        Case ServiceProcess.ServiceControllerStatus.PausePending, ServiceProcess.ServiceControllerStatus.StartPending, ServiceProcess.ServiceControllerStatus.StopPending
                            svr.SendFormat(ConnectionID, "Service is PENDING another operation.", "")
                        Case Else
                            svr.SendFormat(ConnectionID, "wtf?", "")
                    End Select

                Case "uptime"
                    Dim uptime As DateTime = DateAdd(DateInterval.Minute, DateDiff(DateInterval.Minute, CDate(My.Settings.SYSSTART), Now), New DateTime(0))
                    Dim gt_hr As String = ""
                    If DateDiff(DateInterval.Second, New DateTime(0), uptime) < 60 Then
                        svr.SendFormat(ConnectionID, "+uptime: {1} has been running for {0}.", "< 1 minute", AppName)
                    Else
                        If DateDiff(DateInterval.Hour, New DateTime(0), uptime) > 0 Then gt_hr = " and "
                        svr.SendFormat(ConnectionID, "+uptime: {5} has been running for {0}{1}{2}{3}{4}.", _
                            UpTimePart(DatePart(DateInterval.Year, uptime) - 1, "Year", , ","), _
                            UpTimePart(DatePart(DateInterval.Month, uptime) - 1, "Month", , ","), _
                            UpTimePart(DatePart(DateInterval.Day, uptime) - 1, "Day", , ","), _
                            UpTimePart(DatePart(DateInterval.Hour, uptime), "Hour", , ""), _
                            UpTimePart(DatePart(DateInterval.Minute, uptime), "Minute", gt_hr, ""), _
                            AppName _
                        )
                    End If

                Case "time"
                    svr.SendFormat(ConnectionID, "+time: {0}", Date.Now.ToString)

                Case "log"
                    svr.SessionData(ConnectionID, "logmode") = CStr(Not CBool(svr.SessionData(ConnectionID, "logmode")))
                    Select Case CBool(svr.SessionData(ConnectionID, "logmode"))
                        Case True
                            svr.Send(ConnectionID, "+log: Listening for log events...")
                        Case Else
                            svr.Send(ConnectionID, "+log: ear muffs on.")
                    End Select

                Case "start"
                    svr.Send(ConnectionID, "-start: not supported.")

                Case Else
                    If CBool(svr.SessionData(ConnectionID, "loggedon")) Then
                        DOScmd = True
                        Dim ret As String = ""
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
                        cli(ConnectionID).Send(ret)

                        '    Dim mappath As Boolean = False
                        '    Dim out As String = ""
                        '    Dim er As String = ""
                        '    Dim ret As String = ""

                        '    ' Are we mapping a path squire?
                        '    If LCase(Args(0)) = "net" Then
                        '        If UBound(Args) >= 3 Then
                        '            If LCase(Args(1)) = "use" Then
                        '                If LCase(Args(2)) = LCase(My.Settings.PRIORITYDIR) Then
                        '                    ' oh, you are. Well done! Good luck, hope it works out for you.
                        '                    mappath = True
                        '                End If
                        '            End If
                        '        End If
                        '    End If

                        '    For i As Integer = 0 To UBound(Args)
                        '        If InStr(Args(i), Chr(32)) > 0 Then
                        '            ret += Chr(34) & Args(i) & Chr(34)
                        '        Else
                        '            ret += Args(i)
                        '        End If
                        '        If i < UBound(Args) Then
                        '            ret += " "
                        '        End If
                        '    Next

                        '    RunCmd(ret, out, er)
                        '    If out.Length > 0 Then svr.Send(ConnectionID, out)
                        '    If er.Length > 0 Then
                        '        ' Ah well, better luck next time
                        '        svr.Send(ConnectionID, "-" & Args(0) & ": " & er)
                        '    Else
                        '        svr.Send(ConnectionID, "+" & Args(0) & ": " & er)
                        '        If mappath Then
                        '            ' It worked! Let's save that value for next time.
                        '            With My.Settings
                        '                .PRIUNC = Args(3).ToUpper
                        '                .Save()
                        '                ev.Log( _
                        '                    String.Format("Mapped drive {0} to UNC share at {1}.", .PRIORITYDIR, .PRIUNC), _
                        '                    EventLogEntryType.SuccessAudit, _
                        '                    ntEvtlog.EvtLogVerbosity.Normal _
                        '                )
                        '            End With
                        '        End If
                        '    End If

                    Else
                        svr.Send(ConnectionID, String.Format("-Command unknown [{0}]. Try logging on.", CmdStr))
                    End If

            End Select
            If help Then svr.Send(ConnectionID, hBuilder.ToString)
            If cli.Keys.Contains(ConnectionID) And Not DOScmd Then cli(ConnectionID).Send("rem")

        Catch ex As Exception
            ev.Log( _
                ex.Message, _
                EventLogEntryType.Error, _
                ntEvtlog.EvtLogVerbosity.Normal _
            )
            svr.Send(ConnectionID, ex.Message)
        End Try

    End Sub

    Private Function UpTimePart(ByVal Value As Integer, ByVal Description As String, Optional ByVal Prefix As String = "", Optional ByVal Suffix As String = "") As String
        If Value > 0 Then
            Dim plural As String = ""
            Dim count As Integer = 0
            If Value > 1 Then
                plural = "s"
            End If
            Return String.Format("{3}{0} {1}{2}{4}", Value.ToString, Description, plural, Prefix, Suffix)
        Else
            Return ""
        End If
    End Function

#End Region

#Region "Service Shutdown"

    Public Sub EndConsole(ByRef LogBuilder As Builder)

        ' Iterate threads
        For Each Cn As String In svr.Connections.Keys
            If svr.Connections(Cn).IsConnected Then ' Thread is connected?
                svr.SendFormat(Cn, "{0}+service terminated.", vbCrLf)
                svr.SendFormat(Cn, "ending shell session [{0}]", Cn)
                svr.SendFormat(Cn, "+bye!", Cn)
            End If
            If cli.Keys.Contains(Cn) Then ' Thread has a console?
                LogBuilder.AppendFormat("terminating console for thread ID [{0}].", Cn).AppendLine()
                If svr.Connections(Cn).IsConnected Then ' Thread is connected?                    
                    cli(Cn).Quit()
                Else
                    cli(Cn).EndProcess()
                End If
            End If
        Next

        ' End Orphaned console processes
        For Each cn As String In cli.Keys
            If Not svr.Connections.Keys.Contains(cn) Then
                cli(cn).EndProcess()
            End If
        Next

        ' Give running threads 5 seconds to close the console
        For i As Integer = 0 To 50
            If cli.Count = 0 Then Exit For
            Threading.Thread.Sleep(100)
        Next

        ' Forcably disconnect 'stuck' threads
        If Not cli.Count = 0 Then
            LogBuilder.Append("Not all console threads exited in a timely manner...Forcing closure.").AppendLine()
            For Each cn As String In cli.Keys
                cli(cn).EndProcess()
            Next
        End If

    End Sub

#End Region

#Region "Console logging"

    Public Sub AdminConsoleLog(ByVal EntryType As ntEvtlog.LogEntryType, ByVal Data As String)
        Dim cleanup As New List(Of String)
        Dim EntryDes As String = ""        
        For Each cn As String In cli.Keys
            Try
                If CBool(svr.SessionData(cn, "logmode")) Then
                    Select Case EntryType
                        Case ntEvtlog.LogEntryType.Err
                            EntryDes = "error"
                        Case ntEvtlog.LogEntryType.FailureAudit
                            EntryDes = "FailureAudit"
                        Case ntEvtlog.LogEntryType.Information
                            EntryDes = "Information"
                        Case ntEvtlog.LogEntryType.SuccessAudit
                            EntryDes = "SuccessAudit"
                        Case ntEvtlog.LogEntryType.Warning
                            EntryDes = "Warning"
                    End Select
                    svr.Send(cn, "")
                    svr.SendFormat(cn, "{0}: {1}", Now.ToString, EntryDes.ToUpper)
                    svr.SendFormat(cn, "{0}", Data.Replace(Chr(10), vbCrLf))
                End If
            Catch
                cleanup.Add(cn)
            End Try
        Next

        For Each cn As String In cleanup
            cli(cn).EndProcess()
            cli.Remove(cn)
        Next

    End Sub

#End Region

End Module
