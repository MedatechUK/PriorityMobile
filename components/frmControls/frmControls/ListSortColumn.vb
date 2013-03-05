Public Enum eColumnFormat
    fmt_None = 0
    fmt_Date = 1
    fmt_Money = 2
End Enum

Public Class ListSortColumn
    Private _ColumnFormat As eColumnFormat = eColumnFormat.fmt_None
    Public Property ColumnFormat() As eColumnFormat
        Get
            Return _ColumnFormat
        End Get
        Set(ByVal value As eColumnFormat)
            _ColumnFormat = value
        End Set
    End Property
    Private _BoundColumn As String = ""
    Public Property BoundColumn() As String
        Get
            Return _BoundColumn
        End Get
        Set(ByVal value As String)
            _BoundColumn = value
        End Set
    End Property
    Private _ColumnTitle As String = ""
    Public Property ColumnTitle() As String
        Get
            Return _ColumnTitle
        End Get
        Set(ByVal value As String)
            _ColumnTitle = value
        End Set
    End Property
    Private _ColumnWidth As Integer = 100
    Public Property ColumnWidth() As Integer
        Get
            Return _ColumnWidth
        End Get
        Set(ByVal value As Integer)
            _ColumnWidth = value
        End Set
    End Property
    Public Sub New(ByVal BoundColumn As String, ByVal ColumnTitle As String, ByVal ColumnWidth As Integer, ByVal ColumnFormat As eColumnFormat)
        _ColumnFormat = ColumnFormat
        _BoundColumn = BoundColumn
        _ColumnTitle = ColumnTitle
        _ColumnWidth = ColumnWidth
    End Sub

End Class
