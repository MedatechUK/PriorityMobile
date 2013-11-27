Public Class RegisteredService

    Public Sub New(ByVal ServiceType As eServicePorts, ByVal NetBiosName As String)
        _ServiceType = ServiceType
        _NetBiosName = NetBiosName
    End Sub

    Public Sub New(ByVal ServiceName As String, ByVal NetBiosName As String)
        _ServiceType = ServiceFromDescriptor(ServiceName)
        _NetBiosName = NetBiosName
    End Sub

    Private _ServiceType As eServicePorts
    Public ReadOnly Property ServiceType() As eServicePorts
        Get
            Return _ServiceType
        End Get
    End Property

    Private _NetBiosName As String
    Public Property NetBiosName() As String
        Get
            Return _NetBiosName
        End Get
        Set(ByVal value As String)
            _NetBiosName = value
        End Set
    End Property

    Public ReadOnly Property ServiceDescriptor() As String
        Get
            Return ServiceName(_ServiceType)
        End Get
    End Property

    Public ReadOnly Property Port() As Integer
        Get
            Return _ServiceType
        End Get        
    End Property

End Class
