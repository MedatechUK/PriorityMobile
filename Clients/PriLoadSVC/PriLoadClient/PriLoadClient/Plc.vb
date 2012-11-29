'-q "M:\Clients\PriLoadSVC\PriLoadClient\PriLoadClient\bin\Debug\MAIL/OUTBOX/d2a2588d-4a24-4d42-9709-c50d5d63a1ef.txt" -w -v++
'-d "m:\components\win32ntSox\ntSox_Client\ntSox_Client\bin\Debug\data.txt" -i "m:\components\win32ntSox\ntSox_Client\ntSox_Client\bin\Debug\ZPDA_PART_LOAD.ini" -w -v++
Imports System.Reflection
Imports System.Threading
Imports System.io
Imports ntEvtlog

Module PLC

#Region "Emumerations"

    Private Enum RunMode
        Normal
        ReQ ' Requeue a file
        EventQ
        ReadQ
        Vector
        qEvent ' q an event
    End Enum

    Private Enum helpFile
        none
        syntax
        ini
        server
    End Enum

#End Region

#Region "Local Variables"

    Private MyRunMode As RunMode = RunMode.Normal
    Private inifile As String = Nothing
    Private datafile As String = Nothing
    Private evtData As String = Nothing
    Private quit As Boolean = False

    Dim WithEvents c As MyClient
    Dim ev As New ntEvtlog.evt

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

    Private _Wait As Boolean = False
    Public Property Wait() As Boolean
        Get
            Return _Wait
        End Get
        Set(ByVal value As Boolean)
            _Wait = value
        End Set
    End Property

    Private _Connected As Boolean = False
    Public Property Connected() As Boolean
        Get
            Return _Connected
        End Get
        Set(ByVal value As Boolean)
            _Connected = value
        End Set
    End Property

    Private _SendStr As String = Nothing
    Public Property SendStr() As String
        Get
            Return _SendStr
        End Get
        Set(ByVal value As String)
            _SendStr = value
        End Set
    End Property

    Private _DeleteonQd As Boolean = False
    Public Property DelonQd() As Boolean
        Get
            Return _DeleteonQd
        End Get
        Set(ByVal value As Boolean)
            _DeleteonQd = value
        End Set
    End Property

    Private Sub mkNEDir(ByVal dir As String)
        Dim f As New IO.DirectoryInfo(BasePath & dir & "\")
        If Not f.Exists Then
            FileSystem.MkDir(BasePath & dir)
        End If
    End Sub

#End Region

#Region "Parse Arguments"

    Private Sub GetArgs(ByVal Command As String)

        Dim h As helpFile = helpFile.none
        Dim exStr As String = Nothing
        Dim state As String = ""
        Dim registering As Boolean = False
        If Command.Length = 0 Then Command = "/?"
        For Each arg As String In MakeArgs(Command)
            Select Case Left(arg, 1)
                Case "/", "-"
                    Select Case LCase(Right(arg, arg.Length - 1))
                        Case "vec", "vector"
                            state = ""
                            MyRunMode = RunMode.Vector
                        Case "e", "event"
                            state = ""
                            If MyRunMode = RunMode.ReQ Then
                                MyRunMode = RunMode.qEvent
                            Else
                                MyRunMode = RunMode.EventQ
                            End If
                        Case "del", "delete"
                            DelonQd = True
                            state = ""
                        Case "w", "wait"
                            state = ""
                            Wait = True
                        Case "h", "help", "?"
                            state = LCase(Right(arg, arg.Length - 1))
                            h = helpFile.syntax
                        Case "i", "ini"
                            state = LCase(Right(arg, arg.Length - 1))
                        Case "d", "data"
                            state = LCase(Right(arg, arg.Length - 1))
                        Case "v", "verbose"
                            state = ""
                            ev.LogVerbosity = ntEvtlog.EvtLogVerbosity.Verbose
                        Case "v+", "veryverbose"
                            state = ""
                            ev.LogVerbosity = ntEvtlog.EvtLogVerbosity.VeryVerbose
                        Case "v++", "arcane"
                            state = ""
                            ev.LogVerbosity = ntEvtlog.EvtLogVerbosity.Arcane
                        Case "r", "register"
                            registering = True
                            state = ""
                            quit = True
                        Case "q", "enq"
                            If MyRunMode = RunMode.EventQ Then
                                MyRunMode = RunMode.qEvent
                            Else
                                MyRunMode = RunMode.ReQ
                            End If
                        Case Else
                            exStr = String.Format("Unknown argument: {0}. Please seek /help.", arg)
                    End Select
                Case Else
                    Select Case state
                        Case "i", "ini"
                            inifile = arg
                        Case "d", "data"
                            datafile = arg
                        Case "h", "help", "?"
                            Select Case LCase(arg)
                                Case "syntax"
                                    h = helpFile.syntax
                                Case "ini"
                                    h = helpFile.ini
                                Case "server"
                                    h = helpFile.server
                                Case Else
                                    exStr = "Invalid syntax. Please seek /help."
                            End Select
                        Case Else
                            exStr = "Invalid syntax. Please seek /help."
                    End Select
            End Select
        Next
        If Not h = helpFile.none Then
            doHelp(h)
            quit = True
        End If
        If registering Then
            ev.RegisterLog(AppName)
        End If
        If MyRunMode = RunMode.EventQ Then
            If IsNothing(inifile) And IsNothing(datafile) Then
                MyRunMode = RunMode.ReadQ
            End If
        End If
        If Not IsNothing(exStr) Then
            Throw New Exception(exStr)
        End If
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

#End Region

#Region "Console Messages"

    Sub doWelcome()
        With Assembly.GetExecutingAssembly().GetName()
            AppName = .Name
            Console.WriteLine("")
            Console.WriteLine(String.Format("(c){0} eMerge I.T.", Year(Now).ToString))
            Console.WriteLine(String.Format("{0}", AppName))
            Console.WriteLine("By Simon Barnett.")
            Console.WriteLine(String.Format("Build: {0}.{1}.{2}.{3}", _
                .Version.Major, _
                .Version.Minor, _
                .Version.Build, _
                .Version.Revision))
            Console.WriteLine("")
        End With
    End Sub

    Private Sub doHelp(ByVal h As helpFile)

        Dim fn As String = ""

        Select Case h
            Case helpFile.syntax
                fn = "syntax.txt"
            Case helpFile.ini
                fn = "ini.txt"
            Case helpFile.server
                fn = "server.txt"
        End Select

        If Not File.Exists(BasePath & "help\" & fn) Then
            Console.WriteLine( _
                String.Format( _
                    "Help file [help\{0}] was not found.", _
                    fn _
                ) _
            )
        Else
            Using sr As New StreamReader(BasePath & "help\" & fn)
                For Each str As String In Split(sr.ReadToEnd, vbCrLf)
                    Console.WriteLine(str)
                Next
            End Using
        End If

    End Sub

#End Region

#Region "Subs"

    Sub Main()

        Try
            'Directory.SetCurrentDirectory(BasePath)

            doWelcome()
            With ev
                .AppName = AppName
                .LogMode = ntEvtlog.EvtLogMode.EventLog
                .LogVerbosity = ntEvtlog.EvtLogVerbosity.Normal
            End With
            checkPaths()
            GetArgs(Command)

        Catch ex As Exception
            SafeLog( _
                String.Format("{0}", ex.Message), _
                EventLogEntryType.Error, _
                ntEvtlog.EvtLogVerbosity.Normal _
            )
            quit = True
        End Try

        If Not quit Then

            ' Process commands
            Try
                If Not quit Then
                    Select Case MyRunMode
                        Case RunMode.Normal, RunMode.EventQ
                            enQ(SendStr)
                        Case RunMode.ReQ, RunMode.qEvent
                            reQ(SendStr)
                        Case RunMode.Vector
                            Vec(SendStr)
                        Case RunMode.ReadQ
                            Readq(SendStr)
                    End Select
                End If
            Catch ex As Exception
                quit = True
                SafeLog( _
                    String.Format("{0}", ex.Message), _
                    EventLogEntryType.Information, _
                    ntEvtlog.EvtLogVerbosity.VeryVerbose _
                )
            End Try

            quit = IsNothing(SendStr)
            If Not quit Then
                ' Try Connect
                c = New MyClient("127.0.0.1", 8022)
                SafeLog( _
                    String.Format("Opening [{0}:{1}]...", c.IP, c.Port), _
                    EventLogEntryType.Information, _
                    ntEvtlog.EvtLogVerbosity.VeryVerbose _
                )
                c.Connect()
            End If

            ' Wait for connection
            While Not quit And Not Connected
                Thread.Sleep(100)
            End While

            ' Process commands
            If Not quit Then
                SafeLog( _
                    String.Format("Sending load data to [{0}:{1}].", c.IP, c.Port), _
                     EventLogEntryType.Information, _
                      ntEvtlog.EvtLogVerbosity.VeryVerbose _
                  )
                c.Send(SendStr)
            End If

        End If

        ' Wait for disconnection
        While Not quit
            Thread.Sleep(100)
        End While

        If Wait Then
            Console.WriteLine("")
            Console.WriteLine("Press any key to continue.")
            Dim strInput As String = Console.ReadKey(False).ToString
            While (strInput = "")
                Thread.Sleep(100)
            End While
        End If

    End Sub

    Private Sub checkPaths()

        SafeLog( _
            String.Format("Running in: {0}", BasePath), _
             EventLogEntryType.SuccessAudit, _
              ntEvtlog.EvtLogVerbosity.VeryVerbose _
          )
        Try
            ' Check Paths Exist
            mkNEDir("MAIL")
            mkNEDir("MAIL/SENT")
            mkNEDir("MAIL/OUTBOX")
            mkNEDir("DATA")

        Catch ex As Exception
            quit = True
            SafeLog( _
                String.Format("Subfolders do not exist in [{0}] and could not create them because [{1}].", BasePath, ex.Message), _
                 EventLogEntryType.Error, _
                  ntEvtlog.EvtLogVerbosity.Normal _
              )
        End Try

    End Sub

    Private Sub enQ(ByRef SendStr As String)

        SendStr = Nothing

        Dim t1colcount As Integer = -1
        Dim t2colcount As Integer = -1
        Dim t1() As String = Nothing
        Dim lastt1() As String = Nothing
        Dim t2() As String = Nothing
        Dim cmd As String = ""
        Dim myGUID As String = System.Guid.NewGuid().ToString()

        Select Case MyRunMode
            Case RunMode.Normal
                cmd = "LoadData"
            Case Else
                cmd = "LoadDataWait"
        End Select

        ' Bad INI file - checking if the file exists in the basepath
        If File.Exists(BasePath & inifile) Then
            inifile = BasePath & inifile
        End If

        ' Bad Data file - checking if the file exists in the basepath
        If File.Exists(BasePath & datafile) Then
            datafile = BasePath & datafile
        End If

        Try
            If File.Exists(inifile) Then
                SafeLog( _
                    String.Format("Using INI file [{0}]...file ok.", _
                    Replace(inifile, BasePath, "")), _
                     EventLogEntryType.Information, _
                      ntEvtlog.EvtLogVerbosity.VeryVerbose _
                  )
                If File.Exists(datafile) Then
                    SafeLog( _
                        String.Format("Using Data file [{0}]...file ok.", _
                        Replace(datafile, BasePath, "")), _
                         EventLogEntryType.Information, _
                          ntEvtlog.EvtLogVerbosity.VeryVerbose _
                      )
                    Dim pri As New Priority.Loading
                    With pri
                        '********.LoadingConstants.Constants("%Constant%") = "test"
                        .FromFile(inifile)
                        t1colcount = UBound(Split(.RecordType1, ","))
                        t2colcount = UBound(Split(.RecordType2, ","))

                        Using sr As New StreamReader(datafile)
                            While Not sr.EndOfStream
                                Dim col() As String = Split(sr.ReadLine, Chr(9))
                                t1 = Nothing : t2 = Nothing
                                If UBound(col) = t1colcount + t2colcount + 1 Then

                                    For i As Integer = 0 To t1colcount
                                        AddItem(t1, col(i))
                                    Next

                                    ' Surpress duplicate Type 1 records
                                    If IsNothing(lastt1) Or Not SameArray(lastt1, t1) Then
                                        .AddRecord(1) = t1
                                        lastt1 = t1
                                    End If

                                    For i As Integer = t1colcount + 1 To t1colcount + 1 + t2colcount
                                        AddItem(t2, col(i))
                                    Next
                                    .AddRecord(2) = t2

                                Else
                                    ' Data mismatch
                                    Throw New Exception( _
                                        "The data contained in the file: " & vbCrLf & _
                                        "[" & datafile & "]" & _
                                        "does not match the parameter specification of the initialisation file: " & vbCrLf & _
                                        "[" & inifile & "]" _
                                    )
                                End If

                            End While
                        End Using

                        Dim fn As String = _
                        BasePath & "MAIL/OUTBOX/" & _
                            myGUID & ".txt"

                        SafeLog( _
                            String.Format("Saving new loading to [{0}].", Replace(fn, BasePath, "")), _
                             EventLogEntryType.SuccessAudit, _
                              ntEvtlog.EvtLogVerbosity.VeryVerbose _
                          )

                        .ToFile(fn)

                        SendStr = _
                            "enq" & Chr(32) & _
                            myGUID & Chr(32) & _
                            Chr(34) & fn & Chr(34) & Chr(32) & _
                            cmd

                    End With
                Else
                    'no data file
                    Throw New Exception( _
                        "The specified data file does not exist: " & vbCrLf & _
                        "[" & datafile & "]" _
                    )
                End If
            Else
                'no ini file
                Throw New Exception( _
                    "The specified initialisation file does not exist: " & vbCrLf & _
                    "[" & inifile & "]" _
                )
            End If

        Catch ex As Exception
            SafeLog( _
                "Item was not queued. The reason was: " & vbCrLf & _
                ex.Message, _
                EventLogEntryType.FailureAudit, _
                ntEvtlog.EvtLogVerbosity.Normal _
            )
            c.Disconnect()
        End Try

    End Sub

    Private Function SameArray(ByVal Ar1() As String, ByVal Ar2() As String) As Boolean
        If IsNothing(Ar1) Then
            If Not IsNothing(Ar2) Then
                Return False
            End If
        End If
        If IsNothing(Ar2) Then
            If Not IsNothing(Ar1) Then
                Return False
            End If
        End If
        If IsNothing(Ar1) And IsNothing(Ar2) Then
            Return True
        End If
        If Not (UBound(Ar1) = UBound(Ar2)) Then
            Return False
        End If
        For i As Integer = 0 To UBound(Ar1)
            If Not Strings.StrComp(Ar1(i), Ar2(i), CompareMethod.Text) = 0 Then
                Return False
            End If
        Next
        Return True
    End Function

    Private Sub AddItem(ByRef Ar() As String, ByVal NewItem As String)
        Try
            ReDim Preserve Ar(UBound(Ar) + 1)
        Catch
            ReDim Ar(0)
        Finally
            Ar(UBound(Ar)) = NewItem
        End Try
    End Sub

    Private Sub reQ(ByRef SendStr As String)

        SendStr = Nothing
        Dim cmd As String = ""
        Select Case MyRunMode
            Case RunMode.ReQ
                cmd = "LoadData"
            Case RunMode.qEvent
                cmd = "LoadDataWait"
        End Select

        ' Bad datafile file - checking if the file exists in the basepath
        If File.Exists(BasePath & datafile) Then
            datafile = BasePath & datafile
        End If

        If File.Exists(datafile) Then
            SafeLog( _
                String.Format("Re-queuing file [{0}]...file ok.", _
                Replace(datafile, BasePath, "")), _
                 EventLogEntryType.Information, _
                  ntEvtlog.EvtLogVerbosity.VeryVerbose _
              )
            SendStr = _
                "enq" & Chr(32) & _
                System.Guid.NewGuid().ToString() & Chr(32) & _
                Chr(34) & datafile & Chr(34) & Chr(32) & _
                cmd
        Else
            Throw New Exception( _
                "The specified data file does not exist: " & vbCrLf & _
                "[" & datafile & "]" _
            )
        End If

    End Sub

    Private Sub Vec(ByRef SendStr As String)

        SendStr = Nothing

        ' Bad datafile file - checking if the file exists in the basepath
        If File.Exists(BasePath & datafile) Then
            datafile = BasePath & datafile
        End If

        If File.Exists(datafile) Then
            SafeLog( _
                String.Format("Vector Image [{0}]...file ok.", _
                Replace(datafile, BasePath, "")), _
                 EventLogEntryType.Information, _
                  ntEvtlog.EvtLogVerbosity.VeryVerbose _
              )
            SendStr = _
                "enq" & Chr(32) & _
                System.Guid.NewGuid().ToString() & Chr(32) & _
                Chr(34) & datafile & Chr(34) & Chr(32) & _
                "SaveSignature"
        Else
            Throw New Exception( _
                "The specified data file does not exist: " & vbCrLf & _
                "[" & datafile & "]" _
            )
        End If

    End Sub

    Private Sub Readq(ByRef SendStr As String)

        SendStr = Nothing

        SafeLog( _
            String.Format("Reading the server event queue.", _
            ""), _
             EventLogEntryType.Information, _
              ntEvtlog.EvtLogVerbosity.VeryVerbose _
          )

        SendStr = _
            "evt"

    End Sub

    Private Sub SafeLog(ByVal Entry As String, ByVal EventType As LogEntryType, ByVal Verbosity As ntEvtlog.EvtLogVerbosity)
        Try
            ev.Log( _
                Entry, _
                EventType, _
                Verbosity _
              )
        Catch exep As Exception
            Console.WriteLine( _
                "Failed to write to the [{0} {1}] log.{4}" & _
                "The error reported was: {4}{2}{4}" & _
                "The event could not be written to the log because: {4}{3}{4}", _
                ev.LogName, ev.AppName, Entry, exep.Message, vbCrLf _
            )
        End Try
    End Sub

#End Region

#Region "Client Event Handlers"

    Private Sub honClientConnection() Handles c.onClientConnection

        SafeLog( _
            String.Format("Connected to [{0}:{1}]", c.IP, c.Port), _
             EventLogEntryType.Information, _
              ntEvtlog.EvtLogVerbosity.Arcane _
          )
        Connected = True

        SafeLog( _
            String.Format("Sending helo {0}...", AppName), _
             EventLogEntryType.Information, _
              ntEvtlog.EvtLogVerbosity.Arcane _
          )
        c.Send("app " & AppName)

    End Sub

    Private Sub honClientCommand(ByVal CmdStr As String, ByVal Args() As String) Handles c.onClientCommand

        SafeLog( _
            String.Format("Received command:{0}{1}{0}From [{2}:{3}]", _
            vbCrLf, CmdStr, c.IP, c.Port), _
             EventLogEntryType.Information, _
              ntEvtlog.EvtLogVerbosity.Arcane _
          )

        Select Case LCase(Args(0))
            Case "+qd"
                SafeLog( _
                    String.Format("Queued item: {0}", _
                    Args(1)), _
                    EventLogEntryType.SuccessAudit, _
                    ntEvtlog.EvtLogVerbosity.Verbose _
                )

                ' Delete the datafile if required
                If DelonQd Then
                    SafeLog( _
                        String.Format("Deleting the queued data file at:{0}{1}", _
                        vbCrLf, datafile), _
                         EventLogEntryType.Information, _
                          ntEvtlog.EvtLogVerbosity.Verbose _
                      )
                    While File.Exists(datafile)
                        Try
                            File.Delete(datafile)
                        Catch
                            Thread.Sleep(1000)
                        End Try
                    End While
                End If

                ' Disconnect
                c.Disconnect()

            Case "-qd"
                SafeLog( _
                    String.Format("Item was not queued.{0}The reason was:{0}{1}", _
                    vbCrLf, Args(1)), _
                    EventLogEntryType.FailureAudit, _
                    ntEvtlog.EvtLogVerbosity.Normal _
                )
                c.Disconnect()

            Case "+evt"
                If Not IsNothing(evtData) Then
                    Dim sd As New Priority.SerialData
                    With sd
                        sd.FromStr(evtData)
                        For y As Integer = 0 To UBound(sd.Data, 2)
                            Console.WriteLine( _
                                String.Format( _
                                    "{1} for BubbleID [{0}] returned {2}.{4}The Data was [{3}].", _
                                    .Data(0, y), _
                                    .Data(1, y), _
                                    .Data(2, y), _
                                    .Data(3, y), _
                                    vbCrLf _
                                ) _
                            )
                        Next
                    End With
                    c.Disconnect()

                Else
                    Console.WriteLine("No event data returned.")
                    c.Disconnect()
                End If

            Case Else
                If MyRunMode = RunMode.ReadQ Then
                    If Left(CmdStr, 2) = ">>" Then
                        evtData += Replace(Right(CmdStr, CmdStr.Length - 2), vbCrLf, "")
                    End If
                End If

        End Select

    End Sub

    Private Sub honClientConnectionFail(ByVal ex As String) Handles c.onClientConnectionFail
        SafeLog( _
            String.Format("A connection error occured:{0}{1}{0}The command was:{0}{2} ", vbCrLf, ex, SendStr), _
            EventLogEntryType.Error, _
            ntEvtlog.EvtLogVerbosity.Normal _
        )
        quit = True
    End Sub

    Private Sub honClientDisconnection() Handles c.onClientDisconnection
        SafeLog( _
            String.Format("Disconnection from [{0}:{1}]...ok", c.IP, c.Port), _
             EventLogEntryType.Information, _
              ntEvtlog.EvtLogVerbosity.Arcane _
          )
        quit = True
    End Sub

#End Region

End Module
