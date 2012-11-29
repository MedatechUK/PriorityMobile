Public Class ListSort

#Region "Local Variables"

    Private _Columns As New List(Of ListSortColumn)

#End Region

#Region "Public Events"

    Public Event Bind()
    Public Event ItemSelect()
    Public Event SelectedIndexChanged(ByVal itemText As String)

#End Region

#Region "Public Properties"

    Public ReadOnly Property Items() As System.Windows.Forms.ListView.ListViewItemCollection
        Get
            Return View.Items
        End Get
    End Property

    Private _Sort As String = ""
    Public Property Sort() As String
        Get
            Return _Sort
        End Get
        Set(ByVal value As String)
            _Sort = value            
        End Set
    End Property

    Public ReadOnly Property Selected() As String
        Get
            With View
                If .SelectedIndices.Count = 0 Then Return Nothing
                Return .Items(.SelectedIndices(0)).Text
            End With
        End Get
    End Property

#End Region

#Region "Public Methods"

    Public Sub AddColumn(ByVal BoundColumn As String, Optional ByVal ColumnTitle As String = Nothing, Optional ByVal ColumnWidth As Integer = 100)
        If IsNothing(ColumnTitle) Then ColumnTitle = String.Format("Column {0}", (_Columns.Count + 1).ToString)
        _Columns.Add(New ListSortColumn(BoundColumn, ColumnTitle, ColumnWidth))
    End Sub

    Public Sub Clear()
        View.Clear()
        With View
            For Each c As ListSortColumn In _Columns
                Dim ch As New ColumnHeader()                
                ch.Text = c.ColumnTitle
                .Columns.Add(ch)
                With .Columns
                    With .Item(.Count - 1)
                        .Width = c.ColumnWidth                        
                    End With
                End With
            Next
        End With
    End Sub

    Public Sub AddRow(ByVal Row As System.Data.DataRow)
        With View
            .Items.Add(New ListViewItem)
            With .Items(.Items.Count - 1)
                Dim first As Boolean = True
                For Each c As ListSortColumn In _Columns
                    If first Then
                        .Text = Row.Item(c.BoundColumn)
                        first = False
                    Else
                        With .SubItems
                            .Add(New System.Windows.Forms.ListViewItem.ListViewSubItem)
                            .Item(.Count - 1).Text = Row.Item(c.BoundColumn)
                        End With
                    End If
                Next
            End With
        End With
    End Sub

#End Region

#Region "Local Event Handlers"

    Private Sub ListView_ColumnClick(ByVal sender As Object, ByVal e As System.Windows.Forms.ColumnClickEventArgs) Handles View.ColumnClick
        If Not String.Compare(Sort, _Columns(e.Column).BoundColumn, True) = 0 Then
            Sort = _Columns(e.Column).BoundColumn
        Else
            Sort = _Columns(e.Column).BoundColumn & " DESC"
        End If
        RaiseEvent Bind()
    End Sub

    Private Sub ListView_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles View.DoubleClick
        RaiseEvent ItemSelect()
    End Sub

    Private Sub ListView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles View.KeyPress
        Select Case e.KeyChar
            Case Chr(13)
                RaiseEvent ItemSelect()
        End Select
    End Sub

    Private Sub ListView_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) _
        Handles View.SelectedIndexChanged, View.LostFocus
        With View
            If .SelectedIndices.Count = 0 Then Exit Sub
            RaiseEvent SelectedIndexChanged(.Items(.SelectedIndices(0)).Text)
        End With
    End Sub

#End Region

End Class
