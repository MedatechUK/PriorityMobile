Public MustInherit Class consoleMustInherit : Inherits ServiceMustInherit

    Private ReadOnly Property thisSvc() As eServicePorts
        Get
            Return eServicePorts.console
        End Get
    End Property

    Public Overrides ReadOnly Property ServiceType() As String
        Get
            Return ServiceName(thisSvc)
        End Get
    End Property

    Public Overrides ReadOnly Property ServicePort() As Integer
        Get
            Return thisSvc
        End Get
    End Property

    Public Overrides ReadOnly Property udpListener() As Boolean
        Get
            Return False
        End Get
    End Property

    Public Overrides ReadOnly Property tcpListener() As Boolean
        Get
            Return True
        End Get
    End Property

    Public Overrides Sub NewUDPMessage(ByRef Request As svcMsgXML)

    End Sub

    Public Overrides ReadOnly Property BroadcastPort() As Integer
        Get
            Return eServicePorts.discovery
        End Get
    End Property

End Class
