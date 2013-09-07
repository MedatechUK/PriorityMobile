Imports System.Windows.Forms
Public Class cTableItem
    Inherits Dictionary(Of String, String)

    Public Sub New(ByRef DefaultItem As cTableItem, Optional ByRef EditItem As ListViewItem = Nothing)

        _EditItem = EditItem        
        For Each k As String In DefaultItem.Keys
            Me.Add(k, "")
        Next

    End Sub

    Public Sub New(ByRef Columns As cColumns)        
        For Each col As cColumn In Columns.Values
            Me.Add(String.Format(":$.{0}", col.Name), "")
        Next
    End Sub

    Private _EditItem As ListViewItem
    Public Property EditItem() As ListViewItem
        Get
            Return _EditItem
        End Get
        Set(ByVal value As ListViewItem)
            _EditItem = value
        End Set
    End Property

    Public Property Column(ByVal ColumnName As String) As String
        Get
            If Keys.Contains(ColumnName) Then
                Return Item(ColumnName)
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal value As String)
            If Keys.Contains(ColumnName) Then
                Item(ColumnName) = value
            End If
        End Set
    End Property

    Public Function thisItem() As ListViewItem
        Dim ret As New ListViewItem
        Dim i As Integer = 0
        For Each k As String In Keys
            If i = 0 Then
                ret.Text = Item(k)
            Else
                Dim lvsi As New ListViewItem.ListViewSubItem
                lvsi.Text = Item(k)
                ret.SubItems.Add(lvsi)
            End If
            i += 1
        Next
        Return ret
    End Function

End Class
