Imports System.Data.SqlClient
Imports Microsoft.VisualBasic
Imports System.Diagnostics
Imports system.threading
Imports ntEvtlog

Public Class PROCclient

    Dim logging As Boolean = False

    Dim WithEvents c As MyClient
    Dim ev As New ntEvtlog.evt

    Enum eRunMode
        synchronous = 0
        asynchronous = 1
    End Enum

#Region "private properties"

    Private _Log As String = ""
    Public ReadOnly Property Log() As String
        Get
            Return _Log
        End Get
    End Property

    Private _RunMode As eRunMode = eRunMode.asynchronous
    Public Property RunMode() As eRunMode
        Get
            Return _RunMode
        End Get
        Set(ByVal value As eRunMode)
            _RunMode = value
        End Set
    End Property

    Private _LogID As String = Nothing
    Public Property LogID() As String
        Get
            Return _LogID
        End Get
        Set(ByVal value As String)
            _LogID = value
        End Set
    End Property

    Private _ThisCommand As String = ""
    Public Property ThisCommand() As String
        Get
            Return _ThisCommand
        End Get
        Set(ByVal value As String)
            _ThisCommand = value
        End Set
    End Property

    Private _ThisCommandDescription As String = ""
    Public Property ThisCommandDescription() As String
        Get
            Return _ThisCommandDescription
        End Get
        Set(ByVal value As String)
            _ThisCommandDescription = value
        End Set
    End Property

    Private _ThisArguments As String = ""
    Public Property ThisArguments() As String
        Get
            Return _ThisArguments
        End Get
        Set(ByVal value As String)
            _ThisArguments = value
        End Set
    End Property

    Private _AppName As String = ""
    Public Property AppName() As String
        Get
            Return _AppName
        End Get
        Set(ByVal value As String)
            _AppName = value
        End Set
    End Property

    Private _Environment As String = ""
    Public Property Environment() As String
        Get
            Return _Environment
        End Get
        Set(ByVal value As String)
            _Environment = value
        End Set
    End Property

    Private _LogVerbosity As Integer = 99
    Public Property LogVerbosity() As Integer
        Get
            Return _LogVerbosity
        End Get
        Set(ByVal value As Integer)
            _LogVerbosity = value
        End Set
    End Property

    Private _RemoteIP As String = ""
    Public Property RemoteIP() As String
        Get
            Return _RemoteIP
        End Get
        Set(ByVal value As String)
            _RemoteIP = value
        End Set
    End Property

    Private _ConnectionString As String = ""
    Public Property ConnectionString() As String
        Get
            Return _ConnectionString
        End Get
        Set(ByVal value As String)
            _ConnectionString = value
        End Set
    End Property

    Private _quit As Boolean = False
    Private Property quit() As Boolean
        Get
            Return _quit
        End Get
        Set(ByVal value As Boolean)
            _quit = value
        End Set
    End Property

    Private _Connected As Boolean = False
    Private Property Connected() As Boolean
        Get
            Return _Connected
        End Get
        Set(ByVal value As Boolean)
            _Connected = value
        End Set
    End Property

    Dim _result As Boolean = False
    Public ReadOnly Property Result() As Boolean
        Get
            Return _result
        End Get
    End Property

    Dim _error As String = Nothing
    Public ReadOnly Property ErrorMessage() As String
        Get
            Return _error
        End Get
    End Property
#End Region

#Region "Public Subs"

    Public Sub ServerOutput()

        With ev
            .LogMode = ntEvtlog.EvtLogMode.EventLog
            .LogVerbosity = LogVerbosity
            .AppName = AppName
            .RegisterLog()
        End With

        quit = False
        Connected = False

        ' Try Connect
        c = New MyClient("127.0.0.1", 8021)
        SafeLog( _
            String.Format("Opening [{0}:{1}]...", c.IP, c.Port), _
            EventLogEntryType.Information, _
            ntEvtlog.EvtLogVerbosity.VeryVerbose _
        )
        c.Connect()

        ' Wait for connection
        While Not quit And Not Connected
            Thread.Sleep(100)
        End While

        ' Process commands
        If Not quit Then

            If RunMode = eRunMode.synchronous And Not IsNothing(LogID) Then
                SafeLog( _
                    String.Format("Sending log ID {0} to [{1}:{2}]", LogID, c.IP, c.Port), _
                     EventLogEntryType.Information, _
                      ntEvtlog.EvtLogVerbosity.Arcane _
                  )
                c.Send("log " & Chr(34) & LogID & Chr(34))
            End If

            SafeLog( _
                String.Format("Sending {2} to [{0}:{1}].", _
                    c.IP, c.Port, ThisCommandDescription), _
                 EventLogEntryType.Information, _
                  ntEvtlog.EvtLogVerbosity.VeryVerbose _
              )
            With Me
                If ThisArguments.Length > 0 Then
                    c.Send(ThisCommand & " " & ThisArguments)
                Else
                    c.Send(ThisCommand)
                End If
            End With
        End If

        ' Wait for connection
        While Not quit And Connected
            Thread.Sleep(100)
        End While

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
        c.Send("app " & Chr(34) & AppName & Chr(34))

    End Sub

    Private Sub honClientCommand(ByVal CmdStr As String, ByVal Args() As String) Handles c.onClientCommand

        SafeLog( _
            String.Format("Received command:{0}{1}{0}From [{2}:{3}]", _
            vbCrLf, CmdStr, c.IP, c.Port), _
             EventLogEntryType.Information, _
              ntEvtlog.EvtLogVerbosity.Arcane _
          )

        Select Case Left(Args(0), 1)
            Case "+", "-"
                Dim cmd() As String = Split(LCase(Right(Args(0), Args(0).Length - 1)), ":")
                Select Case cmd(0)
                    Case ThisCommand
                        Select Case Left(LCase(Args(0)), 1)
                            Case "+"
                                _result = True
                                SafeLog( _
                                    String.Format("{0} completed ok.", _
                                    ThisCommandDescription), _
                                    EventLogEntryType.SuccessAudit, _
                                    ntEvtlog.EvtLogVerbosity.Verbose _
                                )
                            Case "-"
                                _result = False
                                _error = cmd(1)
                                SafeLog( _
                                    String.Format("{0} failed.", _
                                    ThisCommandDescription), _
                                    EventLogEntryType.FailureAudit, _
                                    ntEvtlog.EvtLogVerbosity.Normal _
                                )
                        End Select
                        ' Disconnect
                        c.Disconnect()
                End Select
        End Select

        Select Case Left(Args(0), 2)
            Case ">>"
                logging = True
            Case "<<"
                logging = False
            Case Else
                If logging And RunMode = eRunMode.synchronous Then
                    _Log += Replace(CmdStr, vbCrLf, "\n")
                End If
        End Select

    End Sub

    Private Sub honClientConnectionFail(ByVal ex As String) Handles c.onClientConnectionFail
        SafeLog( _
            String.Format("A connection error occured:{0}{1}", vbCrLf, ex), _
            EventLogEntryType.Error, _
            ntEvtlog.EvtLogVerbosity.Normal _
        )
        _Log += "No system. Please restart the PriProc service.$$||"
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

#Region "Debugging subs"

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

End Class
