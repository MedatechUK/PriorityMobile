Imports System.Windows.Forms

Public Class table

    Sub loadarray(ByVal this(,))

        Dim i As ListViewItem
        Dim s As ListViewItem.ListViewSubItem

        tbl.Items.Clear()

        tbl.Columns.Add("", 200, HorizontalAlignment.Left)
        For c As Integer = 0 To UBound(this, 1)
            tbl.Columns.Add("Col" & CStr(c), 75, HorizontalAlignment.Left)
        Next

        For y As Integer = 0 To UBound(this, 2)
            i = tbl.Items.Add("Row" & CStr(y))
            For x As Integer = 0 To UBound(this, 1)
                s = New ListViewItem.ListViewSubItem
                s.Text = this(x, y)
                i.SubItems.Add(s)
            Next
        Next

        For c As Integer = 0 To tbl.Columns.Count - 1
            tbl.Columns(c).AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent)
        Next

    End Sub

    Function NotNothing(ByVal a)
        If IsNothing(a) Then Return "" Else Return a
    End Function

End Class