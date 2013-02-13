Imports System.Data

Public Class ListSort

#Region "Local Variables"

    Private _Columns As New List(Of ListSortColumn)
    Private _Keys As New List(Of String)

#End Region

#Region "Public Events"

    Public Event Bind()
    Public Event ItemSelect()
    Public Event SelectedIndexChanged(ByVal item As Integer)

#End Region

#Region "Public Properties"

    Public ReadOnly Property Keys() As List(Of String)
        Get
            Return _Keys
        End Get
    End Property

    Public ReadOnly Property Columns() As List(Of ListSortColumn)
        Get
            Return _Columns
        End Get
    End Property

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
                If .SelectedIndices.Count = 0 Then
                    Return Nothing
                Else
                    Return .Items(.SelectedIndices(0)).Text
                End If
            End With
        End Get
    End Property

    Public ReadOnly Property SelectedIndex() As Integer
        Get
            With View
                If .SelectedIndices.Count = 0 Then
                    Return -1
                Else
                    Return .SelectedIndices(0)
                End If
            End With
        End Get
    End Property

    Public Property Value(ByVal ColumnName As String, ByVal Row As Integer) As String
        Get
            Dim i As Integer = 0
            Dim ret As String = Nothing
            For Each C As ListSortColumn In _Columns
                If String.Compare(C.BoundColumn, ColumnName, True) = 0 Then
                    Try
                        ret = View.Items(Row).SubItems(i).Text
                        Exit For
                    Catch
                    End Try
                End If
                i += 1
            Next
            Return ret
        End Get
        Set(ByVal value As String)
            Dim i As Integer = 0
            For Each C As ListSortColumn In _Columns
                If String.Compare(C.BoundColumn, ColumnName, True) = 0 Then
                    View.Items(Row).SubItems(i).Text = value
                    Exit For
                End If
                i += 1
            Next
        End Set
    End Property

    Public Function RowSelected(ByVal Row As DataRow, ByVal View As DataRowView) As Boolean
        For Each k As String In _Keys
            If Not String.Compare(Row(k), View(k), True) = 0 Then
                Return False
            End If
        Next
        Return True
    End Function

#End Region

#Region "Public Methods"

    Public Sub AddColumn(ByVal BoundColumn As String, Optional ByVal ColumnTitle As String = Nothing, Optional ByVal ColumnWidth As Integer = 100, Optional ByVal IsKey As Boolean = False)
        If IsNothing(ColumnTitle) Then ColumnTitle = String.Format("Column {0}", (_Columns.Count + 1).ToString)
        _Columns.Add(New ListSortColumn(BoundColumn, ColumnTitle, ColumnWidth))
        If IsKey Then
            _Keys.Add(BoundColumn)
        End If
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
        Handles View.SelectedIndexChanged, View.LostFocus, View.ItemActivate
        With View
            If .SelectedIndices.Count = 0 Then
                RaiseEvent SelectedIndexChanged(Nothing)
            Else
                RaiseEvent SelectedIndexChanged(.SelectedIndices(0))
            End If
        End With
    End Sub

    'Private Sub View_ItemActivate(ByVal sender As Object, ByVal e As System.EventArgs) Handles View.ItemActivate
    '    With View
    '        If .SelectedIndices.Count = 0 Then Exit Sub
    '        RaiseEvent SelectedIndexChanged(.SelectedIndices(0))
    '    End With
    'End Sub

#End Region

End Class
