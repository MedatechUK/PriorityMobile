Module AdminConsole
    Public WithEvents svr As New MyServer("127.0.0.1", 8021)
    Private cli As New Dictionary(Of String, Cli)

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
            svr.Send( _
                ConnectionID, _
                Sysinfo _
            )

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

        Dim DOScmd As Boolean = False
        Try
            ev.Log( _
                    String.Format("Rcvd command:{0}[{1}]{0}from Connection [{2}]", _
                    vbCrLf, CmdStr, ConnectionID), _
                    EventLogEntryType.Information, _
                    ntEvtlog.EvtLogVerbosity.Arcane _
                )

            Select Case LCase(Args(0))

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
                            With svr
                                .SessionData(ConnectionID, "loggedon") = "true"
                                .Send(ConnectionID, "+pass: Welcome.")
                                .Send(ConnectionID, "")
                                svr.Send(ConnectionID, Sysinfo)
                                .SendFormat(ConnectionID, "Connected to IIS folder at [{0}]", iisFolder)
                                Try
                                    ' Database/UNC Connection tests
                                    With My.Settings
                                        Connection = NewConnection(.DATASOURCE, .PRIORITYUSER, .PRIORITYPWD)
                                        If .DATASOURCE.Length = 0 Then Throw New Exception("No datasource specified.")
                                        Dim dr As NetworkDrive = GetNetworkDrive(.PRIUNC)
                                        If .PRIUNC.Length = 0 Then Throw New Exception("No UNC Fileshare specified.")
                                        If IsNothing(dr) Then Throw New Exception(String.Format("Priority Fileshare [{0}] is unmapped.", .PRIUNC))
                                        If Not String.Compare(dr.DriveLetter, .PRIORITYDIR, True) = 0 Then Throw New Exception(String.Format("Drive [{0}] is not mapped to [{1}].", .PRIORITYDIR, .PRIUNC))
                                        If Not dr.info.IsReady Then Throw New Exception(String.Format("Priority mapped drive [{0}] is not ready.", dr.DriveLetter))

                                        svr.SendFormat(ConnectionID, "PriorityDrive = {0}", .PRIORITYDIR)
                                        svr.SendFormat(ConnectionID, "PriorityShare = {0}", .PRIUNC)
                                        svr.SendFormat(ConnectionID, "Datasource = {0}", .DATASOURCE)

                                        ' Attempt connection to database
                                        svr.Send(ConnectionID, String.Format("Connecting to database [{0}]...", .DATASOURCE), False)
                                        If Not Connection.State = ConnectionState.Open Then
                                            Connection.Open()                                        
                                        End If
                                        svr.Send(ConnectionID, "OK!")
                                        svr.Send(ConnectionID, "System initialised.")
                                        svr.Send(ConnectionID, "")
                                    End With


                                Catch ConnectionException As Exception
                                    .Send(ConnectionID, "Initialisation exception encountered:")
                                    .SendFormat(ConnectionID, "{0}", ConnectionException.Message)

                                Finally
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
                    DOScmd = True
                    If cli.Keys.Contains(ConnectionID) Then
                        svr.Send(ConnectionID, String.Format("+terminating shell session [{0}]...", ConnectionID))
                        cli(ConnectionID).Quit()
                    Else
                        svr.Send(ConnectionID, "+Bye")
                        svr.KillConnection(ConnectionID)
                    End If

                Case "beep"
                    Beep()

                Case "config"
                    If CBool(svr.SessionData(ConnectionID, "loggedon")) Then
                        Dim ToConsole As New Builder(ConnectionID)
                        Dim et As ntEvtlog.LogEntryType = ntEvtlog.LogEntryType.SuccessAudit
                        Dim verb As ntEvtlog.EvtLogVerbosity = ntEvtlog.EvtLogVerbosity.Verbose
                        ToConsole.Append("Starting configurator from console...").AppendLine()
                        Try
                            Configure(ToConsole)

                            If Not WatchingConfig Then
                                ' Watch the sites web.config for changes            
                                ToConsole.AppendFormat( _
                                        "Starting filewatcher on [{0}web.config]...", _
                                        iisFolder).AppendLine()
                                BeginWatchWebConfig()
                            End If

                            If IsNothing(lEv) Then
                                ' Initialise the Bubble Queue
                                ToConsole.AppendFormat( _
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

                Case "mappath"
                    Try
                        If UBound(Args) = 0 Then
                            svr.SendFormat(ConnectionID, "+mappath: {0} is mapped to {1}.", My.Settings.PRIORITYDIR, My.Settings.PRIUNC)
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
                    svr.Send( _
                        ConnectionID, _
                        Sysinfo _
                    )

                Case "selftest"
                    With svr
                        .Send(ConnectionID, Sysinfo)
                        .SendFormat(ConnectionID, "Connected to IIS folder at [{0}]", iisFolder)
                        Try
                            ' Database/UNC Connection tests
                            With My.Settings
                                Connection = NewConnection(.DATASOURCE, .PRIORITYUSER, .PRIORITYPWD)
                                If .DATASOURCE.Length = 0 Then Throw New Exception("No datasource specified.")
                                Dim dr As NetworkDrive = GetNetworkDrive(.PRIUNC)
                                If .PRIUNC.Length = 0 Then Throw New Exception("No UNC Fileshare specified.")
                                If IsNothing(dr) Then Throw New Exception(String.Format("Priority Fileshare [{0}] is unmapped.", .PRIUNC))
                                If Not String.Compare(dr.DriveLetter, .PRIORITYDIR, True) = 0 Then Throw New Exception(String.Format("Drive [{0}] is not mapped to [{1}].", .PRIORITYDIR, .PRIUNC))
                                If Not dr.info.IsReady Then Throw New Exception(String.Format("Priority mapped drive [{0}] is not ready.", dr.DriveLetter))

                                svr.SendFormat(ConnectionID, "PriorityDrive = {0}", .PRIORITYDIR)
                                svr.SendFormat(ConnectionID, "PriorityShare = {0}", .PRIUNC)
                                svr.SendFormat(ConnectionID, "Datasource = {0}", .DATASOURCE)

                                ' Attempt connection to database
                                svr.Send(ConnectionID, String.Format("Connecting to database [{0}]...", .DATASOURCE), False)
                                If Not Connection.State = ConnectionState.Open Then
                                    Connection.Open()
                                End If
                                svr.Send(ConnectionID, "OK!")
                                svr.Send(ConnectionID, "System initialised.")
                                svr.Send(ConnectionID, "")
                            End With

                        Catch ConnectionException As Exception
                            .Send(ConnectionID, "Initialisation exception encountered:")
                            .SendFormat(ConnectionID, "{0}", ConnectionException.Message)
                        End Try

                    End With

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


#End Region

End Module
