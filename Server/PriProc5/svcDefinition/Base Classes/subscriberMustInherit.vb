Public MustInherit Class subscriberMustInherit : Inherits ServiceMustInherit

    Public Overrides ReadOnly Property ServicePort() As Integer
        Get
            Return eServicePorts.subscriber
        End Get
    End Property

    Public Overrides ReadOnly Property udpListener() As Boolean
        Get
            Return True
        End Get
    End Property

End Class
