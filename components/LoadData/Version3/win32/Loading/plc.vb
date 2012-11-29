Imports System.Threading
Imports System.io
Imports ntSox

Public Class plc

    Private quit As Boolean = False
    Private MyRunMode As RunMode
    Private datafile As String = Nothing
    Private evtData As String = Nothing
    Private myGUID As String = Nothing
    Dim WithEvents c As MyClient
    Dim _ev As ntEvtlog.evt

    Public Event hEvent(ByVal BUBBLEID As String, _
        ByVal SOAPPROC As String, _
        ByVal RESULT As Boolean, _
        ByVal DATA As String)

#Region "Public Subs"

    Public Sub New(ByVal AppName As String, Optional ByRef Evt As ntEvtlog.evt = Nothing)
        _AppName = AppName
        If Not IsNothing(Evt) Then _ev = Evt
    End Sub

    Public Function SendBubble(ByVal fn As String, ByVal Mode As RunMode) As String

        myGUID = Nothing
        datafile = fn
        MyRunMode = Mode
        evtData = Nothing

        Select Case Mode
            Case RunMode.ReQ, RunMode.qEvent
                reQ(SendStr)
            Case RunMode.Vector
                Vec(SendStr)
            Case RunMode.ReadQ
                Readq(SendStr)
        End Select

        quit = IsNothing(SendStr)

        If Not quit Then
            ' Try Connect
            c = New MyClient("127.0.0.1", 8022)
            SafeLog( _
                String.Format("Opening [{0}:{1}]...", c.IP, c.Port), _
                ntEvtlog.LogEntryType.Information, _
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
                 ntEvtlog.LogEntryType.Information, _
                  ntEvtlog.EvtLogVerbosity.VeryVerbose _
              )
            c.Send(SendStr)
        Else
            Throw New Exception("Could not connect to local queue.")
        End If

        ' Wait for disconnection
        While Not quit
            Thread.Sleep(100)
        End While

        Return myGUID

    End Function

#End Region

#Region "public properties"

    Public ReadOnly Property ShortFilename()
        Get
            Dim par() As String = Split(datafile, "\")
            Return par(UBound(par))
        End Get
    End Property

    Private _AppName As String = Nothing
    Public ReadOnly Property AppName() As String
        Get
            Return _AppName
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

#End Region

#Region "Client Event Handlers"

    Private Sub honClientConnection() Handles c.onClientConnection

        SafeLog( _
            String.Format("Connected to [{0}:{1}]", c.IP, c.Port), _
             ntEvtlog.LogEntryType.Information, _
              ntEvtlog.EvtLogVerbosity.Arcane _
          )
        Connected = True

        SafeLog( _
            String.Format("Sending helo {0}...", AppName), _
             ntEvtlog.LogEntryType.Information, _
              ntEvtlog.EvtLogVerbosity.Arcane _
          )
        c.Send("app " & AppName)

    End Sub

    Private Sub honClientCommand(ByVal CmdStr As String, ByVal Args() As String) Handles c.onClientCommand

        SafeLog( _
            String.Format("Received command:{0}{1}{0}From [{2}:{3}]", _
            vbCrLf, CmdStr, c.IP, c.Port), _
             ntEvtlog.LogEntryType.Information, _
              ntEvtlog.EvtLogVerbosity.Arcane _
          )

        Select Case LCase(Args(0))
            Case "+qd"
                SafeLog( _
                    String.Format("Queued item: {0}", _
                    Args(1)), _
                    ntEvtlog.LogEntryType.SuccessAudit, _
                    ntEvtlog.EvtLogVerbosity.Verbose _
                )
                myGUID = Args(1)
                ' Disconnect
                c.Disconnect()

            Case "-qd"
                SafeLog( _
                    String.Format("Item was not queued.{0}The reason was:{0}{1}", _
                    vbCrLf, Args(1)), _
                    ntEvtlog.LogEntryType.FailureAudit, _
                    ntEvtlog.EvtLogVerbosity.Normal _
                )
                c.Disconnect()

            Case "+evt"
                If Not IsNothing(evtData) Then
                    Dim sd As New SerialData
                    With sd
                        sd.FromStr(evtData)
                        For y As Integer = 0 To UBound(sd.Data, 2)
                            RaiseEvent hEvent(.Data(0, y), _
                                    .Data(1, y), _
                                    .Data(2, y), _
                                    .Data(3, y))

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
            ntEvtlog.LogEntryType.Err, _
            ntEvtlog.EvtLogVerbosity.Normal _
        )
        quit = True
    End Sub

    Private Sub honClientDisconnection() Handles c.onClientDisconnection
        SafeLog( _
            String.Format("Disconnection from [{0}:{1}]...ok", c.IP, c.Port), _
             ntEvtlog.LogEntryType.Information, _
              ntEvtlog.EvtLogVerbosity.Arcane _
          )
        quit = True
    End Sub

#End Region

#Region "Private Subs"

    Private Sub reQ(ByRef SendStr As String)

        SendStr = Nothing
        Dim cmd As String = ""
        Select Case MyRunMode
            Case RunMode.ReQ
                cmd = "LoadData"
            Case RunMode.qEvent
                cmd = "LoadDataWait"
        End Select

        If File.Exists(datafile) Then
            SafeLog( _
                String.Format("Re-queuing file [{0}]...file ok.", _
                ShortFilename), _
                 ntEvtlog.LogEntryType.Information, _
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

        If File.Exists(datafile) Then
            SafeLog( _
                String.Format("Vector Image [{0}]...file ok.", _
                shortfilename), _
                 ntEvtlog.LogEntryType.Information, _
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
             ntEvtlog.LogEntryType.Information, _
              ntEvtlog.EvtLogVerbosity.VeryVerbose _
          )

        SendStr = _
            "evt"

    End Sub

    Private Sub SafeLog(ByVal Entry As String, ByVal EventType As ntEvtlog.LogEntryType, ByVal Verbosity As ntEvtlog.EvtLogVerbosity)
        Try
            If Not IsNothing(_ev) Then
                _ev.Log( _
                    Entry, _
                    EventType, _
                    Verbosity _
                  )
            End If
        Catch exep As Exception
            Console.WriteLine( _
                "Failed to write to the [{0} {1}] log.{4}" & _
                "The error reported was: {4}{2}{4}" & _
                "The event could not be written to the log because: {4}{3}{4}", _
                _ev.LogName, _ev.AppName, Entry, exep.Message, vbCrLf _
            )
        End Try
    End Sub

#End Region

End Class
