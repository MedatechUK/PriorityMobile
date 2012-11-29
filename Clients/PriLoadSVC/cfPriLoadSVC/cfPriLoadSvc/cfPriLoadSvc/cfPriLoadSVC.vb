Imports ntSox
Imports ntDictionaryLib
Imports System.Reflection
Imports ntEvtLog

Module cfPriLoadSVC

    Private WithEvents svr As New MyServer("127.0.0.1", 8022)
    Private ev As New ntEvtLog.evt
    Private WithEvents q As q
    Private evq As EVq
    Private ws As New PriWebSvc.Service
    Private Settings As settings

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

    Sub Main(ByVal args() As String)

        Dim sp As New Splash
        sp.Show()

        Dim cmdArg As String = ""
        For i As Integer = 0 To UBound(args)
            cmdArg += args(i)
            If i < UBound(args) Then cmdArg += " "
        Next

        Try
            With Assembly.GetExecutingAssembly().GetName()
                AppName = .Name
                Settings = New settings(AppName)
                StartMessage = String.Format("+{0} @{5}:{11} Build: {1}.{4} Running: {6} ({7}.{8}.{9})", _
                    .Name, _
                    .Version.Major, _
                    .Version.Minor, _
                    .Version.Build, _
                    .Version.Revision, _
                    "PocketPC", _
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
                .LogMode = ntEvtLog.EvtLogMode.File                                
                If cmdArg.Length > 0 Then doArgs(MakeArgs(cmdArg))
                .LogVerbosity = Settings.LogVerbosity
            End With

            ev.Log( _
                    String.Format( _
                        "Starting Priority Loading Service...{0}" & _
                        "Running on [{1}:{2}].{0}" & _
                        "Logging level set to [{3}]{0}" & _
                        "Using Priority SOAP Service at:{0}http://{4}:8080", _
                        vbCrLf, svr.IP, svr.Port, _
                        ev.DescribeVerbosity(ev.LogVerbosity), _
                        System.Net.Dns.GetHostEntry("soapsvc").AddressList(0).ToString _
                    ), _
                    LogEntryType.Information, _
                    ntEvtLog.EvtLogVerbosity.Normal _
                )

            evq = New EVq(ev)
            q = New q(ev)

            ' Start the service
            svr.StartSvc()


            q.Add("test", New qItem(System.Guid.NewGuid.ToString, BasePath & "MAIL\OUTBOX\load_sig.txt", "test", "LoadData"))


        Catch ex As Exception
            ev.Log( _
                String.Format("{0}", ex.Message), _
                LogEntryType.Err, _
                ntEvtLog.EvtLogVerbosity.Normal _
            )
        End Try
    End Sub

    Private Function MakeArgs(ByVal Value As String) As String()
        Dim ret() As String = Nothing
        Dim sp() As String = Split(Value, ChrW(34))
        For i As Integer = 0 To UBound(sp)
            If EvenNumber(i + 1) Then
                NewArg(ret, sp(i))
            Else
                Dim tmp() As String = Split(sp(i), ChrW(32))
                For Each str As String In tmp
                    NewArg(ret, str)
                Next
            End If
        Next
        Return ret
    End Function

    Private Function EvenNumber(ByVal Value As Integer) As Boolean
        Return Value Mod 2 = 0
    End Function

    Private Sub NewArg(ByRef ArgArray() As String, ByVal NewValue As String)
        If NewValue.Length > 0 Then
            Try
                If Not IsNothing(ArgArray) Then
                    ReDim Preserve ArgArray(UBound(ArgArray) + 1)
                Else
                    ReDim ArgArray(0)
                End If
            Catch ex As Exception
                ReDim ArgArray(0)
            Finally
                ArgArray(UBound(ArgArray)) = NewValue
            End Try
        End If
    End Sub

    Private Sub doArgs(ByVal Args() As String)
        Dim state As String = ""
        Try
            For Each arg As String In Args
                With Settings
                    Select Case Left(arg, 1)
                        Case "/", "-"
                            Select Case LCase(Right(arg, arg.Length - 1))
                                Case "s", "soap", "service"
                                    state = "s"
                                Case "v-", "quiet"
                                    state = ""
                                    .LogVerbosity = _
                                        CInt(ntEvtLog.EvtLogVerbosity.Normal)
                                Case "v", "verbose"
                                    state = ""
                                    .LogVerbosity = _
                                        CInt(ntEvtLog.EvtLogVerbosity.Verbose)
                                Case "v+", "veryverbose"
                                    state = ""
                                    .LogVerbosity = _
                                        CInt(ntEvtLog.EvtLogVerbosity.VeryVerbose)
                                Case "v++", "arcane"
                                    state = ""
                                    .LogVerbosity = _
                                        CInt(ntEvtLog.EvtLogVerbosity.Arcane)
                                Case Else
                                    Throw New Exception(String.Format("Unknown Switch [{0}].", _
                                        LCase(Right(arg, arg.Length - 1))))
                            End Select
                        Case Else
                            Select Case state
                                Case "s", "soap", "service"
                                    Settings.SoapSVC = arg
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
                LogEntryType.Err, _
                ntEvtLog.EvtLogVerbosity.Normal _
            )
        End Try
    End Sub

    '    Protected Overrides Sub OnStop()
    '        ' Add code here to perform any tear-down necessary to stop your service.
    '        Try
    '            ev.Log( _
    '                    String.Format("Stopping Priority Loading Service on [{0}:{1}] ", svr.IP, svr.Port), _
    '                    LogEntryType.Information, _
    '                    ntEvtlog.EvtLogVerbosity.Normal _
    '                )
    '            svr.StopSvc()

    '        Catch ex As Exception
    '            ev.Log( _
    '                String.Format("{0}", ex.Message), _
    '                LogEntryType.Err, _
    '                ntEvtlog.EvtLogVerbosity.Normal _
    '            )
    '        End Try
    '    End Sub

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
                     LogEntryType.Information, _
                     ntEvtlog.EvtLogVerbosity.Verbose _
            )

        Catch ex As Exception
            ev.Log( _
                ex.Message, _
                LogEntryType.Err, _
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
                    LogEntryType.Information, _
                    ntEvtlog.EvtLogVerbosity.VeryVerbose _
                )

            Select Case LCase(Args(0))
                Case "app"
                    svr.SessionData(ConnectionID, "app") = Args(1)
                    ev.Log( _
                            String.Format("Connection [{0}] identified itself as: [{1}].", _
                            ConnectionID, Args(1)), _
                            LogEntryType.Information, _
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
                                LogEntryType.Information, _
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
                                LogEntryType.Information, _
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
                LogEntryType.Err, _
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
                    Case "loaddatawait"
                        Dim txtResult As String = ws.LoadDataWait(.BubbleID, SerialData)
                        hResult = True
                        evq.Add(.BubbleID, New eventItem(.BubbleID, .Caller, .SOAPProc, Left(txtResult, 1) <> "!", txtResult))
                    Case Else
                        hResult = ws.LoadData(SerialData)
                End Select

            Catch ex As Exception
                hResult = False
                ev.Log( _
                        String.Format("Failed to send bubble [{1}].{0}{2}{0}Re-trying.", _
                        vbCrLf, .BubbleID, ex.Message), _
                        LogEntryType.FailureAudit, _
                        ntEvtlog.EvtLogVerbosity.Verbose _
                    )
            End Try

            If hResult Then
                ev.Log( _
                        String.Format( _
                            "Bubble [{1}] was burst!{0}Bubble was from Application [{2}]{0}Bubble has been sent to the SOAP service at:{0}{3}.", _
                            vbCrLf, _
                            .BubbleID, _
                            .Caller, _
                            My.WebServices.Service.Url _
                        ), _
                        LogEntryType.SuccessAudit, _
                        ntEvtLog.EvtLogVerbosity.Verbose _
                    )
            End If
        End With

    End Sub

    'End Class

End Module
