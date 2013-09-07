Public Class Coordinate

    Public Sub New(ByVal X As Integer, ByVal Y As Integer)
        _x = X
        _y = Y
    End Sub

    Private _x As Integer
    Public Property x() As Integer
        Get
            Return _x
        End Get
        Set(ByVal value As Integer)
            _x = value
        End Set
    End Property

    Private _y As Integer
    Public Property y() As Integer
        Get
            Return _y
        End Get
        Set(ByVal value As Integer)
            _y = value
        End Set
    End Property

End Class
