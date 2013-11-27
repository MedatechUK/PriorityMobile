Public MustInherit Class loaderMustInherit : Inherits ServiceMustInherit

    Private ReadOnly Property thisSvc() As eServicePorts
        Get
            Return eServicePorts.loader
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

End Class
