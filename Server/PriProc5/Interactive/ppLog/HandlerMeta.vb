Public Module HandlerMeta

    Private _HandlerCreated As Boolean = False
    Public Property HandlerCreated() As Boolean
        Get
            Return _HandlerCreated
        End Get
        Set(ByVal value As Boolean)
            _HandlerCreated = value
        End Set
    End Property

    Private _HandlerError As Exception
    Public Property HandlerError() As Exception
        Get
            Return _HandlerError
        End Get
        Set(ByVal value As Exception)
            _HandlerError = value
        End Set
    End Property

End Module
