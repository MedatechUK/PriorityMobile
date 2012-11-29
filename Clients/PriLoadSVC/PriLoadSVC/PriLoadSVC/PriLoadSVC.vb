Imports ntSox
Imports ntDictionaryLib
Imports System.Reflection

Public Class PriLoadSVC

    Private WithEvents svr As New MyServer("127.0.0.1", 8022)
    Private ev As New ntEvtlog.evt
    Private WithEvents q As New q(ev)
    Private evq As New EVq(ev)
    Private ws As New PriWebSvc.Service

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

#End Region

    Protected Overrides Sub OnStart(ByVal args() As String)
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
                        "Starting Priority Loading Service...{0}" & _
                        "Running on [{1}:{2}].{0}" & _
                        "Logging level set to [{3}]{0}" & _
                        "Using Priority SOAP Service at:{0}{4}", _
                        vbCrLf, svr.IP, svr.Port, _
                        ev.DescribeVerbosity(ev.LogVerbosity), _
                        My.Settings.PriLoadSVC_PriWebSvc_Service _
                    ), _
                    EventLogEntryType.Information, _
                    ntEvtlog.EvtLogVerbosity.Normal _
                )

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

    Private Sub hOnConnect(ByVal ConnectionID As String) _
        Handles svr.OnConnect

        Try
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

                Case "enq"
                    If Not q.ContainsKey(Args(2)) Then
                        q.Add(Args(2), _
                            New qItem( _
                                Args(1), _
                                Args(2), _
                                svr.SessionData(ConnectionID, "app"), _
                                Args(3) _
                            ) _
                        )

                        ev.Log( _
                                String.Format("BubbleID [{0}] queued.", Args(1)), _
                                EventLogEntryType.Information, _
                                ntEvtlog.EvtLogVerbosity.Verbose _
                            )

                        svr.Send( _
                            ConnectionID, _
                            "+qd " & Args(1) _
                        )

                    Else
                        ev.Log( _
                                String.Format("File [{0}] already exists in the queue.", _
                                Args(2)), _
                                EventLogEntryType.Information, _
                                ntEvtlog.EvtLogVerbosity.Normal _
                            )
                        svr.Send( _
                            ConnectionID, _
                            "-qd " & _
                            Chr(34) & _
                            "File [" & Replace(Args(2), BasePath, "") & "] already exists in the queue." & _
                            Chr(34) _
                        )
                    End If

                Case "evt"
                    If svr.SessionData(ConnectionID, "app") = "test" Then
                        For i As Integer = 0 To 2
                            Dim gu As String = System.Guid.NewGuid.ToString
                            evq.Add(gu, New eventItem(gu, svr.SessionData(ConnectionID, "app"), "LoadDataWait", "True", "1"))
                        Next
                    End If
                    Dim bub() As String = Nothing
                    For Each ei As eventItem In evq.Values
                        With ei
                            If .CALLER = svr.SessionData(ConnectionID, "app") Then
                                svr.Send( _
                                    ConnectionID, _
                                    String.Format(">>{2}{0}{3}{0}{4}{0}{5}{1}", _
                                    "\t", _
                                    "\n", _
                                    .BUBBLEID, _
                                    .SOAPPROC, _
                                    .RESULT, _
                                    .DATA) _
                            )
                            End If
                            Try
                                ReDim Preserve bub(UBound(bub) + 1)
                            Catch ex As Exception
                                ReDim bub(0)
                            Finally
                                bub(UBound(bub)) = .BUBBLEID
                            End Try
                        End With
                    Next
                    If Not IsNothing(bub) Then
                        For Each key As String In bub
                            evq.Remove(key)
                        Next
                    End If

                    svr.Send( _
                        ConnectionID, _
                        "+evt")
            End Select

        Catch ex As Exception
            ev.Log( _
                ex.Message, _
                EventLogEntryType.Error, _
                ntEvtlog.EvtLogVerbosity.Normal _
            )
        End Try
    End Sub

    Public Sub Send( _
            ByVal qi As qItem, _
            ByVal SerialData As String, _
            ByRef hResult As Boolean _
        ) _
        Handles q.hSend

        With qi
            Try
                Select Case LCase(qi.SOAPProc)
                    Case "savesignature"
                        Dim img As String = ws.SaveSignature(SerialData)
                        hResult = True
                        evq.Add(.BubbleID, New eventItem(.BubbleID, .Caller, .SOAPProc, CStr(img.Length > 0), img))
                        evq.Save(tSource.File)
                    Case "loaddatawait"
                        Dim txtResult As String = ws.LoadDataWait(.BubbleID, SerialData)
                        hResult = True
                        evq.Add(.BubbleID, New eventItem(.BubbleID, .Caller, .SOAPProc, Left(txtResult, 1) <> "!", txtResult))
                        evq.Save(tSource.File)
                    Case Else
                        hResult = ws.LoadData(SerialData)
                End Select

            Catch ex As Exception
                hResult = False
                ev.Log( _
                        String.Format("Failed to send bubble [{1}].{0}{2}{0}Re-trying.", _
                        vbCrLf, .BubbleID, ex.Message), _
                        EventLogEntryType.FailureAudit, _
                        ntEvtlog.EvtLogVerbosity.Verbose _
                    )
            End Try

            If hResult Then
                ev.Log( _
                        String.Format( _
                            "Bubble [{1}] was burst!{0}Bubble was from Application [{2}]{0}" & _
                                "Bubble has been sent to the SOAP service at:{0}{3}.", _
                            vbCrLf, _
                            .BubbleID, _
                            .Caller, _
                            My.Settings.PriLoadSVC_PriWebSvc_Service.ToString _
                        ), _
                        EventLogEntryType.SuccessAudit, _
                        ntEvtlog.EvtLogVerbosity.Verbose _
                    )
            End If
        End With

    End Sub

End Class
