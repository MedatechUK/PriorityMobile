Public Class ColumnDef
    Private _Column As New Dictionary(Of String, Integer)
    Default Public ReadOnly Property col(ByVal ColumnName As String) As Integer
        Get
            If Column.ContainsKey(ColumnName) Then
                Return Column(ColumnName)
            Else
                Return Nothing
            End If
        End Get
    End Property
    Default Public ReadOnly Property col(ByVal Ord As Integer) As String
        Get
            For Each k As String In Column.Keys
                If Ord = Column(k) Then
                    Return k
                End If
            Next
            Return Nothing
        End Get
    End Property
    Public Property Column() As Dictionary(Of String, Integer)
        Get
            Return _Column
        End Get
        Set(ByVal value As Dictionary(Of String, Integer))
            _Column = value
        End Set
    End Property
    Public Sub New(ByVal SQL As String)
        Dim str As String = Split(SQL, "select", , CompareMethod.Text)(1)
        Dim str2 As String = Split(str, "from", , CompareMethod.Text)(0)
        Dim i As Integer = 0
        For Each c As String In Split(str2, ",")
            If InStr(c, " as ", CompareMethod.Text) > 0 Then
                c = Split(c, " as ", , CompareMethod.Text)(1)
            End If
            Column.Add(Trim(c), i)
            i += 1
        Next
    End Sub
End Class
