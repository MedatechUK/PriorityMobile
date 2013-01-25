Public Class PriProc

    Private LicenceProcess As Long
    Private StartConfig As System.Timers.Timer        

#Region "Initialisation and finalisation"

    Protected Overrides Sub OnStart(ByVal args() As String)

        Dim et As ntEvtlog.LogEntryType = ntEvtlog.LogEntryType.SuccessAudit
        LogBuilder = New Builder()

        Try
            ' Configure logging options
            ev = New ntEvtlog.evt(ntEvtlog.EvtLogMode.EventLog, ntEvtlog.EvtLogVerbosity.Arcane, AppName)
            ev.RegisterLog(AppName)

            LogBuilder.AppendFormat("Starting {0}...", AppName).AppendLine()
            Try
                If args.Length > 0 Then
                    LogBuilder.Append("Found arguments").AppendLine()
                    For Each arg As String In args
                        LogBuilder.AppendFormat("{0} ", arg)
                    Next
                    LogBuilder.AppendLine()

                    ' Process start arguments                    
                    doStartArgs(args, LogBuilder)
                Else
                    LogBuilder.Append("No start arguments found.").AppendLine()
                End If

            Catch ex As Exception
                et = ntEvtlog.LogEntryType.Warning
                LogBuilder.AppendFormat("Invalid Start Argument: {0}", ex.Message).AppendLine()
            Finally

                Try
                    With My.Settings
                        LogBuilder.AppendFormat("Starting telnet service @{0} on port {1}...", .SERVICEHOST, .SERVICEPORT)
                        svr = New MyServer(.SERVICEHOST, .SERVICEPORT)
                        svr.StartSvc()
                        LogBuilder.Append("OK").AppendLine()
                    End With
                Catch ex As Exception
                    et = ntEvtlog.LogEntryType.Warning
                    LogBuilder.Append("Failed.").AppendLine()
                    LogBuilder.AppendFormat("The exception thown was: {0}", ex.Message).AppendLine()
                Finally
                    ev.Log(LogBuilder.ToString, et, ntEvtlog.EvtLogVerbosity.Normal)
                    SystemStart()
                End Try

            End Try

        Catch ex As Exception
            Console.WriteLine(String.Format("Failed to initialise log. {0}", ex.Message))
            Console.WriteLine("I gave up. Sorry.")
            Beep() : Beep()
        End Try

    End Sub

    Protected Overrides Sub OnStop()

        Dim et As ntEvtlog.LogEntryType = ntEvtlog.LogEntryType.SuccessAudit
        LogBuilder = New Builder()

        Try
            ' Configure logging options
            ev = New ntEvtlog.evt(ntEvtlog.EvtLogMode.EventLog, ntEvtlog.EvtLogVerbosity.Arcane, AppName)
            LogBuilder.AppendFormat("Closing {0}...", AppName)

            EndConsole(LogBuilder)
            svr.StopSvc()

            LogBuilder.Append("OK").AppendLine()

        Catch ex As Exception
            et = ntEvtlog.LogEntryType.Warning
            LogBuilder.AppendLine.AppendFormat("{0}", ex.Message).AppendLine()
        Finally
            ev.Log(LogBuilder.ToString, et, ntEvtlog.EvtLogVerbosity.Normal)
        End Try

    End Sub

#End Region

#Region "Pause / resume the bubble service"

    Protected Overrides Sub OnPause()

        Dim logbuilder As New Builder
        Try
            logbuilder.AppendFormat( _
                    "Pausing Bubble queue at [{0}{1}\]...", _
                    iisFolder, _
                    BubbleFolder(tBubbleFolder.QueueFolder) _
            ).AppendLine()

            If Not IsNothing(lEv) Then RemoveHandler lEv.NewBubble, AddressOf hNewBubble

        Catch ex As Exception
            logbuilder.AppendFormat("{0}", ex.Message).AppendLine()

        Finally
            ev.Log( _
                logbuilder.ToString, _
                ntEvtlog.LogEntryType.Information, _
                ntEvtlog.EvtLogVerbosity.Normal _
            )
        End Try

    End Sub

    Protected Overrides Sub OnContinue()

        Dim logbuilder As New Builder
        Try
            logbuilder.AppendFormat( _
                    "Resuming Bubble queue at [{0}{1}\]...", _
                    iisFolder, _
                    BubbleFolder(tBubbleFolder.QueueFolder) _
            ).AppendLine()

            If IsNothing(lEv) Then
                lEv = New PriLoadEvents( _
                    New System.IO.DirectoryInfo( _
                        String.Format( _
                                "{0}{1}\", _
                                iisFolder, _
                                BubbleFolder(tBubbleFolder.QueueFolder) _
                            ) _
                        ) _
                    )
            Else
                lEv.RestartQ()
            End If

            AddHandler lEv.NewBubble, AddressOf hNewBubble

        Catch ex As Exception
            logbuilder.AppendFormat("{0}", ex.Message).AppendLine()

        Finally
            ev.Log( _
                logbuilder.ToString, _
                ntEvtlog.LogEntryType.Information, _
                ntEvtlog.EvtLogVerbosity.Normal _
            )
        End Try

    End Sub

#End Region

#Region "Private Methods"

    Private Sub hStartTimer(ByVal sender As Object, ByVal e As System.EventArgs)

        ' Attempt system auto configuration

        With StartConfig
            .Enabled = False
            .Dispose()
        End With

        Dim et As ntEvtlog.LogEntryType = ntEvtlog.LogEntryType.SuccessAudit
        LogBuilder = New Builder()

        Try
            Configure(LogBuilder)
        Catch ex As Exception
            et = ntEvtlog.LogEntryType.Err
        End Try

        ev.Log( _
                LogBuilder.ToString(), _
                et, _
                ntEvtlog.EvtLogVerbosity.Normal _
            )
    End Sub

    Private Sub SystemStart()

        '--- Begin the service

        Dim et As ntEvtlog.LogEntryType = ntEvtlog.LogEntryType.SuccessAudit
        LogBuilder = New Builder()

        Try

            ' Set default user info if missing
            With My.Settings
                .SYSSTART = Now
                If .PRIORITYUSER.Length = 0 Then .PRIORITYUSER = "tabula"
                If .PRIORITYPWD.Length = 0 Then .PRIORITYPWD = "Tabula!"
                .Save()
            End With

            ' Connect to local IIS (http://localhost:8080/config.asmx)
            Dim testConnection As String = iisFolder
            LogBuilder.AppendFormat("Discovered IIS folder at [{0}]", iisFolder).AppendLine()

            ' Set web.config values
            LogBuilder.AppendFormat("Writing system variables to [{0}web.config]...", iisFolder).AppendLine()

            ApplicationSetting("start") = My.Settings.SYSSTART.ToString
            ApplicationSetting("sysinfo") = Sysinfo
            ApplicationSetting("appName") = AppName

            Try
                ' Database/UNC Connection tests
                With My.Settings
                    Connection = New GenericConnection(.PROVIDER, .DATASOURCE, .PRIORITYUSER, .PRIORITYPWD)
                    If .DATASOURCE.Length = 0 Then Throw New Exception("No datasource specified.")
                    Dim dr As NetworkDrive = GetNetworkDrive(.PRIUNC)
                    If .PRIUNC.Length = 0 Then Throw New Exception("No UNC Fileshare specified.")
                    If IsNothing(dr) Then Throw New Exception(String.Format("Priority Fileshare [{0}] is unmapped.", .PRIUNC))
                    If Not String.Compare(dr.DriveLetter, .PRIORITYDIR, True) = 0 Then Throw New Exception(String.Format("Drive [{0}] is not mapped to [{1}].", .PRIORITYDIR, .PRIUNC))
                    If Not dr.info.IsReady Then Throw New Exception(String.Format("Priority mapped drive [{0}] is not ready.", dr.DriveLetter))

                    ' Attempt connection to database
                    LogBuilder.Append("Initialisation checks OK.").AppendLine()
                    LogBuilder.AppendFormat("Connecting to database [{0}]...", .DATASOURCE)
                    Connection.Open()
                    LogBuilder.AppendFormat("OK!", .DATASOURCE).AppendLine()
                    ApplicationSetting("DSN") = Connection.ConnectionString
                    ApplicationSetting("PROVIDER") = Connection.Provider
                End With

                ' Occupy a Priority licence for the session
                Dim myProcess As Process = New Process()
                With myProcess
                    With .StartInfo
                        .FileName = String.Format("{0}\BIN.95\WINRUN.exe", My.Settings.PRIORITYDIR)
                        .Arguments = String.Format( _
                                        "{5}{5} {1} {2} {0}\system\prep {3} WINFORM {4}", _
                                            My.Settings.PRIORITYDIR, _
                                            My.Settings.PRIORITYUSER, _
                                            My.Settings.PRIORITYPWD, _
                                            ApplicationSetting("Environment"), _
                                            "ZPDA_LICENSE_LOCK", _
                                            "'" _
                                    )
                        .UseShellExecute = False
                        .CreateNoWindow = True
                        .RedirectStandardInput = True

                        LogBuilder.Append("Starting Priority Licence process...").AppendLine()
                        LogBuilder.AppendFormat("{0} {1}", .FileName, .Arguments).AppendLine()

                    End With
                    .Start()
                End With

            Catch ConnectionException As Exception
                LogBuilder.Append("Initialisation exception encountered:").AppendLine()
                LogBuilder.AppendFormat("{0}", ConnectionException.Message).AppendLine()
            Finally
                Dim qState As String = "The bubble queue is NOT RUNNING!"
                If Not IsNothing(lEv) Then
                    If lEv.qStarted Then
                        qState = "The bubble queue is RUNNING."
                    End If
                End If
                LogBuilder.AppendFormat("{0}", qState).AppendLine()
            End Try

            ' Verify Connection            
            If Not Connection.State = ConnectionState.Open Then

                ' Begin auto config
                StartConfig = New System.Timers.Timer
                With StartConfig
                    .Interval = 1000
                    AddHandler .Elapsed, AddressOf hStartTimer
                    .Enabled = True
                End With            

            Else

                ' Watch the sites web.config for changes            
                LogBuilder.AppendFormat( _
                        "Starting filewatcher on [{0}web.config]...", _
                        iisFolder).AppendLine()
                BeginWatchWebConfig()

                ' Initialise the Bubble Queue
                LogBuilder.AppendFormat( _
                        "Starting Bubble queue at [{0}{1}\]...", _
                        iisFolder, _
                        BubbleFolder(tBubbleFolder.QueueFolder) _
                ).AppendLine()

                lEv = New PriLoadEvents( _
                    New System.IO.DirectoryInfo( _
                        String.Format( _
                                "{0}{1}\", _
                                iisFolder, _
                                BubbleFolder(tBubbleFolder.QueueFolder) _
                            ) _
                        ) _
                    )
                AddHandler lEv.NewBubble, AddressOf hNewBubble

            End If

        Catch EX As Exception
            et = ntEvtlog.LogEntryType.Err
            LogBuilder.AppendFormat( _
                    "Fatal Exception during initialisation. {0}", _
                    EX.Message _
            ).AppendLine()

        Finally
            ev.Log( _
                LogBuilder.ToString(), _
                et, _
                ntEvtlog.EvtLogVerbosity.Normal _
            )

        End Try

    End Sub

#End Region

End Class
