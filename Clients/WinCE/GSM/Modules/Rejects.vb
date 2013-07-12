Public Class Rejects
    Private RQuant As Integer
    Private ROperation As String
    Private RReason As String
    Private RCode As String
    Public Property quan() As Integer
        Get
            Return RQuant
        End Get
        Set(ByVal value As Integer)
            RQuant = value
        End Set
    End Property
    Public Property op() As String
        Get
            Return ROperation
        End Get
        Set(ByVal value As String)
            ROperation = value
        End Set
    End Property
    Public Property reas() As String
        Get
            Return RReason
        End Get
        Set(ByVal value As String)
            RReason = value
        End Set
    End Property
    Public Property code() As String
        Get
            Return RCode
        End Get
        Set(ByVal value As String)
            RCode = value
        End Set
    End Property
    Public Sub New(ByVal q As Integer, ByVal o As String, ByVal r As String, ByVal c As String)
        quan = q
        op = o
        reas = r
        code = c
    End Sub
End Class
