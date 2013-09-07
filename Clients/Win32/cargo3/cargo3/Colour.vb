Public Class Colour

    Public Sub New(ByVal name As String, ByVal r As Integer, ByVal g As Integer, ByVal b As Integer)
        _Name = Name
        _r = r
        _g = g
        _b = b
    End Sub

    Private _Name As String = ""
    Public ReadOnly Property Name() As String
        Get
            Return _Name
        End Get
    End Property

    Public Function Match(ByVal r As Integer, ByVal g As Integer, ByVal b As Integer, Optional ByVal t As Integer = 5) As Boolean
        Dim ret As Boolean = True
        If Not ColourTolerance(_r, r, t) Then ret = False
        If Not ColourTolerance(_g, g, t) Then ret = False
        If Not ColourTolerance(_b, b, t) Then ret = False
        Return ret
    End Function

    Private Function ColourTolerance(ByVal Colour As Integer, ByVal Match As Integer, ByVal Tol As Integer) As Boolean
        Dim t As Double = (2.55 * Tol)
        Return (Match > (Colour - t)) And (Match < (Colour + t))
    End Function

    Private _r As Integer
    Public Property Red() As Integer
        Get
            Return _r
        End Get
        Set(ByVal value As Integer)
            _r = value
        End Set
    End Property

    Private _g As Integer
    Public Property Green() As Integer
        Get
            Return _g
        End Get
        Set(ByVal value As Integer)
            _g = value
        End Set
    End Property

    Private _b As Integer
    Public Property Blue() As Integer
        Get
            Return _b
        End Get
        Set(ByVal value As Integer)
            _b = value
        End Set
    End Property

End Class
