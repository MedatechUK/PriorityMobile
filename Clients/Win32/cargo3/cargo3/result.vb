Public Class Result

    Private _NextState As State = Nothing
    Public Property NextState() As State
        Get
            Return _NextState
        End Get
        Set(ByVal value As State)
            _NextState = value
        End Set
    End Property

    Private _NextAction As Action = Nothing
    Public Property NextAction() As Action
        Get
            Return _NextAction
        End Get
        Set(ByVal value As Action)
            _NextAction = value
        End Set
    End Property

    Private _Script As KeyScript = Nothing
    Public Property Script() As KeyScript
        Get
            Return _Script
        End Get
        Set(ByVal value As KeyScript)
            _Script = value
        End Set
    End Property

End Class
