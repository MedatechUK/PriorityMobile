Imports PriPROC
Imports System.Reflection

Public MustInherit Class ServiceMustInherit : Implements IDisposable

#Region "Must Override"

    Public MustOverride ReadOnly Property ServicePort() As Integer
    Public MustOverride ReadOnly Property ServiceType() As String
    Public MustOverride ReadOnly Property udpListener() As Boolean
    Public MustOverride ReadOnly Property tcpListener() As Boolean

    Public MustOverride Sub NewTCPMessage(ByRef Request As svcMsgXML, ByRef Response As Byte())
    Public MustOverride Sub NewUDPMessage(ByRef Request As svcMsgXML)
    Public MustOverride Function BeginConfig(ByRef Request As svcMsgXML) As ServiceMessage
    Public MustOverride Sub Start(ByRef sArg As StartArgs)

#End Region

#Region "May Override"

    Public Overridable Sub onTerminate()

    End Sub

    Public Overridable ReadOnly Property BroadcastDelay() As Integer
        Get
            Return 30
        End Get
    End Property

    Public Overridable ReadOnly Property BroadcastPort() As Integer
        Get
            Return 8090
        End Get
    End Property

#End Region

#Region "Public Readonly Properties"

    Private _Config As Boolean = False
    Public Property Config() As Boolean
        Get
            Return _Config
        End Get
        Set(ByVal value As Boolean)
            _Config = value
        End Set
    End Property

    Private _Closing As Boolean = False
    Public ReadOnly Property Closing() As Boolean
        Get
            Return _Closing
        End Get
    End Property

    Private _InteractiveShutdown As Boolean = False
    Public ReadOnly Property InteractiveShutdown() As Boolean
        Get
            Return _InteractiveShutdown
        End Get
    End Property

    Public ReadOnly Property NetBiosName() As String
        Get
            Return Environment.MachineName
        End Get
    End Property

#End Region

#Region "Public Properties"

    Private _thisiListener As iListener = Nothing
    Public Property thisiListener() As iListener
        Get
            Return _thisiListener
        End Get
        Set(ByVal value As iListener)
            _thisiListener = value
        End Set
    End Property

    Private _thisuListener As uListener = Nothing
    Public Property thisuListener() As uListener
        Get
            Return _thisuListener
        End Get
        Set(ByVal value As uListener)
            _thisuListener = value
        End Set
    End Property

    Private _LogPort As Integer = eServicePorts.log
    Public Property logPort() As Integer
        Get
            Return _LogPort
        End Get
        Set(ByVal value As Integer)
            _LogPort = value
        End Set
    End Property

    Private _LogServer As String = NetBiosName
    Public Property LogServer() As String
        Get
            Return _LogServer
        End Get
        Set(ByVal value As String)
            _LogServer = value
        End Set
    End Property

    Private _LogQueue As Queue(Of msgLogRequest)
    Public Property LogQueue() As Queue(Of msgLogRequest)
        Get
            Return _LogQueue
        End Get
        Set(ByVal value As Queue(Of msgLogRequest))
            _LogQueue = value
        End Set
    End Property

    Private _LogClose As Boolean
    Public Property LogClose() As Boolean
        Get
            Return _LogClose
        End Get
        Set(ByVal value As Boolean)
            _LogClose = value
        End Set
    End Property

#End Region

#Region "Initialisation and Finalisation"

    Public Sub Instantiate(ByRef sArg As StartArgs)

        Dim retry As Integer

        _LogQueue = New Queue(Of msgLogRequest)
        _LogClose = False

        With sArg.StartLog.LogData
            .AppendFormat("Starting {0} in ", ServiceType)

            Select Case sArg.RunMode
                Case eRunMode.Interactive
                    .AppendFormat("{0} mode.", "INTERACTIVE").AppendLine()
                    trd = New System.Threading.Thread(AddressOf WaitKeypress)
                    With trd
                        .Name = String.Format("{0}_WaitKeypress", ServiceType)
                        .IsBackground = True
                        .Start()
                    End With

                Case eRunMode.Service
                    .AppendFormat("{0} mode.", "SERVICE").AppendLine()

            End Select

            If tcpListener Then
                retry = 0
                Do Until Not IsNothing(thisiListener) Or retry > 4
                    Try
                        thisiListener = New iListener( _
                            ServicePort, _
                            AddressOf hNewData _
                        )
                        .AppendFormat("Started TCP Listener on {0}:{1}.", NetBiosName, ServicePort).AppendLine()

                    Catch ex As Exception
                        retry += 1
                        Threading.Thread.Sleep(3000)

                    End Try
                Loop

                If IsNothing(thisiListener) Then
                    Throw New SystemFail( _
                        eSysFailCode.FAIL_TCP_LISTENER, _
                        String.Format( _
                            "Could not start TCP Listener on {0}. " & _
                            "Another instance may still be running.", _
                            ServicePort _
                        ) _
                    )
                End If
            End If

            If udpListener Then
                retry = 0
                Do Until Not IsNothing(thisuListener) Or retry > 4
                    Try
                        thisuListener = New uListener( _
                            ServicePort, _
                            AddressOf hNewData _
                        )
                        .AppendFormat( _
                            "Started UDP Listener on {0}:{1}.", _
                            NetBiosName, _
                            ServicePort _
                        ).AppendLine()

                    Catch ex As Exception
                        retry += 1
                        Threading.Thread.Sleep(3000)

                    End Try

                Loop
                If IsNothing(thisuListener) Then
                    Throw New SystemFail( _
                        eSysFailCode.FAIL_UDP_LISTENER, _
                        String.Format( _
                            "Could not start UDP Listener on {0}. " & _
                            "Another instance may still be running.", _
                            ServicePort _
                        ) _
                    )
                End If
            End If

            Start(sArg)

            trdLog = New System.Threading.Thread(AddressOf doLog)
            With trdLog
                .Name = String.Format("{0}_LogQ", ServiceType)
                .IsBackground = True
                .Start()
            End With
            LogEvent(sArg.StartLog)

        End With

    End Sub

#End Region

#Region "Interactive Mode"

    Private trd As System.Threading.Thread
    Private Sub WaitKeypress()
        Console.ReadKey()
        _InteractiveShutdown = True
    End Sub

#End Region

#Region "Public Methods"

    Public Sub hNewData(ByVal sender As Object, ByVal e As PriPROC.byteMsg)
        Try
            Dim responseBytes As Byte() = Nothing
            With TryCast(sender, socketListener)
                Select Case .ProtocolType
                    Case eProtocolType.tcp
                        NewTCPMessage(New svcMsgXML(.ProtocolType, e.Message), responseBytes)
                        With TryCast(sender, PriPROC.iListener)
                            .dResponse.Add(e.msgID, responseBytes)
                        End With
                    Case Else
                        NewUDPMessage(New svcMsgXML(.ProtocolType, e.Message))
                End Select

            End With
        Catch
        End Try
    End Sub

    Public Sub Broadcast(ByVal message As Byte(), Optional ByVal BroadcastType As eBroadcastType = eBroadcastType.bcPublic)
        Using cli As New uClient(BroadcastPort, BroadcastType)
            cli.Send(message)
        End Using
    End Sub

    Public Sub Broadcast(ByVal Port As Integer, ByVal message As Byte(), Optional ByVal BroadcastType As eBroadcastType = eBroadcastType.bcPublic)
        Using cli As New uClient(Port, BroadcastType)
            cli.Send(message)
        End Using
    End Sub

    Public Sub Shutdown()
        If Not Closing Then
            onTerminate()
            _Closing = True

            While Not _LogClose
                Threading.Thread.Sleep(100)
            End While
        End If
    End Sub

#End Region

#Region "Log Message Queue"

    Public Sub LogEvent(ByRef LogMessage As msgLogRequest)
        LogQueue.Enqueue(LogMessage)
    End Sub

    Private trdLog As System.Threading.Thread
    Public Overridable Sub doLog()
        Dim retry As Integer = 0
        With _LogQueue
            Do
                Try
                    If .Count > 0 Then
                        Using cor As New iClient( _
                            LogServer, _
                            logPort _
                        )
                            Dim ret As New svcMsgXML( _
                                    eProtocolType.tcp, _
                                    cor.Send(.Peek.toByte) _
                                )

                            If String.Compare(ret.msgNode.SelectSingleNode("error/code").InnerText, "200") = 0 Then
                                .Dequeue()
                            End If

                        End Using
                    End If
                    retry = 0

                Catch
                    Threading.Thread.Sleep(100)
                    retry += 1

                Finally
                    Threading.Thread.Sleep(100)

                End Try

            Loop Until (Closing And .Count = 0) Or (Closing And retry > 3)
            _LogClose = True

        End With
    End Sub

#End Region

#Region " IDisposable Support "

    ' IDisposable
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' TODO: free other state (managed objects).
                If Not IsNothing(_thisuListener) Then _thisuListener.Dispose()
                If Not IsNothing(_thisiListener) Then _thisiListener.Dispose()
            End If
        End If
        Me.disposedValue = True
    End Sub

    Private disposedValue As Boolean = False        ' To detect redundant calls
    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub

#End Region

End Class
