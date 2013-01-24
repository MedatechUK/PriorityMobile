Public Class PrinterFont

    Private _Fontface As Integer
    Public Property Fontface() As Integer
        Get
            Return _Fontface
        End Get
        Set(ByVal value As Integer)
            _Fontface = value
        End Set
    End Property

    Private _Fontsize As Integer
    Public Property Fontsize() As Integer
        Get
            Return _Fontsize
        End Get
        Set(ByVal value As Integer)
            _Fontsize = value
        End Set
    End Property

    Private _VerticalOffset As Integer
    Public Property VerticalOffset() As Integer
        Get
            Return _VerticalOffset
        End Get
        Set(ByVal value As Integer)
            _VerticalOffset = value
        End Set
    End Property

    Public Sub New(ByVal VerticalOffset As Integer, Optional ByVal Fontface As Integer = 0, Optional ByVal Fontsize As Integer = 0)
        _VerticalOffset = VerticalOffset
        _Fontface = Fontface
        _Fontsize = Fontsize
    End Sub

End Class