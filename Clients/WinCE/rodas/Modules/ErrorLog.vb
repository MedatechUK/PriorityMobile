Public Class ErrorLog
    Private ErrType As String
    Private ItemDescription As String
    Private ItemAmount As Integer
    Public Property EType() As String
        Get
            Return ErrType
        End Get
        Set(ByVal value As String)
            ErrType = value
        End Set
    End Property
    Public Property Desc() As String
        Get
            Return ItemDescription
        End Get
        Set(ByVal value As String)
            ItemDescription = value
        End Set
    End Property
    Public Property Amount() As Integer
        Get
            Return ItemAmount
        End Get
        Set(ByVal value As Integer)
            ItemAmount = value
        End Set
    End Property
    Public Sub New(ByVal t As String, ByVal d As String, ByVal a As Integer)
        EType = t
        Amount = a
        Desc = d
    End Sub
End Class
