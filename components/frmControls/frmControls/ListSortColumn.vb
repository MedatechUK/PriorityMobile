Public Class ListSortColumn

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
    Public Sub New(ByVal BoundColumn As String, ByVal ColumnTitle As String, ByVal ColumnWidth As Integer)
        _BoundColumn = BoundColumn
        _ColumnTitle = ColumnTitle
        _ColumnWidth = ColumnWidth
    End Sub

End Class
