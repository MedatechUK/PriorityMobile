Public Class frmDisplay
    Private editCell As DataGridCell
    Private inEditMode As Boolean = False
    Private inUpdateMode As Boolean = False

    Private Sub frmDisplay_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim p As System.Drawing.Point
        p.X = (Screen.PrimaryScreen.WorkingArea.Width - Me.Width) / 2
        p.Y = (Screen.PrimaryScreen.WorkingArea.Height - Me.Height) / 2
        Me.Location = p
    End Sub


    Private Sub DataGrid1_CurrentCellChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DataGrid1.CurrentCellChanged
        If Not inUpdateMode Then
            If inEditMode AndAlso Not DataGrid1.CurrentCell.Equals(editCell) Then
                ' Update edited cell
                inUpdateMode = True
                DataGrid1.Visible = False
                Dim currentCell As DataGridCell = DataGrid1.CurrentCell
                DataGrid1(editCell.RowNumber, editCell.ColumnNumber) = txtEdit.Text
                DataGrid1.CurrentCell = currentCell
                DataGrid1.Visible = True
                inUpdateMode = False
                txtEdit.Visible = False
                inEditMode = False
            End If

            ' Enter edit mode
            editCell = DataGrid1.CurrentCell
            txtEdit.Text = DirectCast(DataGrid1(editCell.RowNumber, editCell.ColumnNumber), String)
            Dim cellPos As Rectangle = DataGrid1.GetCellBounds(editCell.RowNumber, editCell.ColumnNumber)
            txtEdit.Left = cellPos.Left - 1
            txtEdit.Top = cellPos.Top + DataGrid1.Top - 1
            txtEdit.Width = cellPos.Width + 2
            txtEdit.Height = cellPos.Height + 2
            txtEdit.Visible = True
            inEditMode = True
        End If
    End Sub
End Class