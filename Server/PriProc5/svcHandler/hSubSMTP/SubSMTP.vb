Public Class subsmtp : Inherits Subscriber

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
            Return "subsmtp"
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
                Console.Write(.LogData.ToString)
            End With

        Catch ex As Exception
            Console.ForegroundColor = ConsoleColor.Red
            Console.WriteLine(ex.Message)
        End Try

    End Sub

#End Region

End Class
