Public Class subcmd : Inherits Subscriber

#Region "Initialisation"

    Public Sub New(ByRef sArg As StartArgs)
        Instantiate(sArg)
    End Sub

#End Region

#Region "Overridden Properties"

    Public Overrides ReadOnly Property tcpListener() As Boolean
        Get
            Return False
        End Get
    End Property

    Public Overrides Sub NewTCPMessage(ByRef Request As svcMsgXML, ByRef Response() As Byte)

    End Sub

    Public Overrides ReadOnly Property ServiceType() As String
        Get
            Return "subcmd"
        End Get
    End Property

    Public Overrides ReadOnly Property MyFilter() As msgFilter
        Get
            Return New msgFilter()
        End Get
    End Property

#End Region

#Region "Overridden Methods"

    Public Overrides Sub NewLogEntry(ByRef logEntry As msgLogRequest)
        Try
            With logEntry
                Select Case .EntryType
                    Case LogEntryType.Err, LogEntryType.FailureAudit
                        Console.BackgroundColor = ConsoleColor.Red
                    Case LogEntryType.Warning
                        Console.BackgroundColor = ConsoleColor.Yellow
                    Case LogEntryType.SuccessAudit, LogEntryType.Information
                        Console.BackgroundColor = ConsoleColor.Green
                End Select

                Select Case .Verbosity
                    Case EvtLogVerbosity.Normal
                        Console.ForegroundColor = ConsoleColor.Black
                    Case EvtLogVerbosity.Verbose
                        Console.ForegroundColor = ConsoleColor.DarkBlue
                    Case EvtLogVerbosity.VeryVerbose
                        Console.ForegroundColor = ConsoleColor.DarkGray
                    Case EvtLogVerbosity.Arcane
                        Console.ForegroundColor = ConsoleColor.Gray

                End Select

                Console.WriteLine("{0}", Pad( _
                    String.Format( _
                        "{0}{1}{2}{3}", _
                        Pad(.TimeStamp, 22), _
                        Pad(.LogSource, 12), _
                        Pad(.Source.ToUpper, 25), _
                        Pad(.svcType.ToUpper, 16) _
                    ), _
                        Console.WindowWidth - 1 _
                    ) _
                )

                Console.BackgroundColor = ConsoleColor.Black
                Console.ForegroundColor = ConsoleColor.Green

                Console.Write( _
                    "{0}", _
                    .LogData.ToString _
                )

            End With

        Catch ex As Exception
            Console.ForegroundColor = ConsoleColor.Red
            Console.WriteLine(ex.Message)
        End Try

    End Sub

    Private Function Pad(ByVal Str As String, ByVal Width As Integer) As String
        Return String.Format("{0}{1}", Str, New String(" ", Width)).Substring(0, Width)
    End Function

    Private Function Pad(ByVal logSource As EvtLogSource, ByVal width As Integer)
        Select Case logSource
            Case EvtLogSource.APPLICATION
                Return String.Format("{0}{1}", "APPLICATION", New String(" ", width)).Substring(0, width)
            Case Else
                Return String.Format("{0}{1}", "SYSTEM", New String(" ", width)).Substring(0, width)
        End Select
    End Function

#End Region

End Class
