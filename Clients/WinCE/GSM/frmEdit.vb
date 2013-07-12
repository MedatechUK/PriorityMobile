Imports System.Data
Public Class frmEdit
    'Now we need to set up the variables for the editable Datagrids - I will number them to correspond to the grid to which they belong
    Private oldval1 As Integer = 0
    Private editCell1 As DataGridCell
    Private inEditMode1 As Boolean = False
    Private inUpdateMode1 As Boolean = False

    Private oldval2 As Integer = 0
    Private editCell2 As DataGridCell
    Private inEditMode2 As Boolean = False
    Private inUpdateMode2 As Boolean = False

    Private oldval3 As Integer = 0
    Private editCell3 As DataGridCell
    Private inEditMode3 As Boolean = False
    Private inUpdateMode3 As Boolean = False
    'Set up the datatables we need
    Public pars As New DataTable
    Public Rej As New DataTable
    Public usr As New DataTable

    Private Sub frmEdit_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub

    Private Sub DataGrid1_CurrentCellChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DataGrid1.CurrentCellChanged
        If Not inUpdateMode1 Then
            If inEditMode1 AndAlso Not DataGrid1.CurrentCell.Equals(editCell1) Then
                ' Update edited cell
                'as we are only able to update the quantity we must ensure it is numeric
                If IsNumeric(txtEdit1.Text) = True Then


                    inUpdateMode1 = True
                    DataGrid1.Visible = False
                    Dim currentCell As DataGridCell = DataGrid1.CurrentCell
                    DataGrid1(editCell1.RowNumber, editCell1.ColumnNumber) = txtEdit1.Text
                    'If oldval1 <> Convert.ToInt16(txtEdit1.Text) Then
                    '    rejected1 += oldval
                    '    rejected1 -= Convert.ToInt16(txtEdit1.Text)
                    '    Me.Text = "Rejected Items - " & rejected
                    '    NumericUpDown1.Maximum = rejected
                    '    NumericUpDown1.Value = 1
                    'End If
                    DataGrid1.CurrentCell = currentCell
                    DataGrid1.Visible = True
                    inUpdateMode1 = False
                    txtEdit1.Visible = False
                    inEditMode1 = False
                End If
            End If

            ' Enter edit mode
            editCell1 = DataGrid1.CurrentCell
            If editCell1.ColumnNumber = 1 Then
                Exit Sub
            Else
                oldval1 = DataGrid1(editCell1.RowNumber, editCell1.ColumnNumber).ToString
            End If
            txtEdit1.Text = DataGrid1(editCell1.RowNumber, editCell1.ColumnNumber).ToString
            Dim cellPos As Rectangle = DataGrid1.GetCellBounds(editCell1.RowNumber, editCell1.ColumnNumber)
            txtEdit1.Left = cellPos.Left - 1
            txtEdit1.Top = cellPos.Top + DataGrid1.Top - 1
            txtEdit1.Width = cellPos.Width + 2
            txtEdit1.Height = cellPos.Height + 2
            txtEdit1.Visible = True
            inEditMode1 = True
        End If
        DataGrid1.Select(DataGrid1.CurrentRowIndex)
        DataGrid1.Visible = True
    End Sub

    Private Sub DataGrid2_CurrentCellChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DataGrid2.CurrentCellChanged
        If Not inUpdateMode2 Then
            If inEditMode2 AndAlso Not DataGrid1.CurrentCell.Equals(editCell2) Then
                ' Update edited cell
                'as we are only able to update the quantity we must ensure it is numeric
                If IsNumeric(txtEdit2.Text) = True Then


                    inUpdateMode2 = True
                    DataGrid2.Visible = False
                    Dim currentCell As DataGridCell = DataGrid2.CurrentCell
                    DataGrid2(editCell2.RowNumber, editCell2.ColumnNumber) = txtEdit2.Text
                    'If oldval <> Convert.ToInt16(txtEdit2.Text) Then
                    '    rejected += oldval
                    '    rejected -= Convert.ToInt16(txtEdit.Text)
                    '    Me.Text = "Rejected Items - " & rejected
                    '    NumericUpDown1.Maximum = rejected
                    '    NumericUpDown1.Value = 1
                    'End If
                    DataGrid2.CurrentCell = currentCell
                    DataGrid2.Visible = True
                    inUpdateMode2 = False
                    txtEdit2.Visible = False
                    inEditMode2 = False
                End If
            End If

            ' Enter edit mode
            editCell2 = DataGrid2.CurrentCell
            If editCell2.ColumnNumber = 1 Then
                Exit Sub
            Else
                oldval2 = DataGrid2(editCell2.RowNumber, editCell2.ColumnNumber).ToString
            End If
            txtEdit2.Text = DataGrid2(editCell2.RowNumber, editCell2.ColumnNumber).ToString
            Dim cellPos As Rectangle = DataGrid2.GetCellBounds(editCell2.RowNumber, editCell2.ColumnNumber)
            txtEdit2.Left = cellPos.Left - 1
            txtEdit2.Top = cellPos.Top + DataGrid1.Top - 1
            txtEdit2.Width = cellPos.Width + 2
            txtEdit2.Height = cellPos.Height + 2
            txtEdit2.Visible = True
            inEditMode2 = True
        End If
        DataGrid2.Select(DataGrid2.CurrentRowIndex)
        DataGrid2.Visible = True
    End Sub

    Private Sub DataGrid3_CurrentCellChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DataGrid3.CurrentCellChanged
        If Not inUpdateMode3 Then
            If inEditMode3 AndAlso Not DataGrid3.CurrentCell.Equals(editCell3) Then
                ' Update edited cell
                'as we are only able to update the quantity we must ensure it is numeric
                If IsNumeric(txtEdit3.Text) = True Then


                    inUpdateMode3 = True
                    DataGrid3.Visible = False
                    Dim currentCell As DataGridCell = DataGrid3.CurrentCell
                    DataGrid3(editCell3.RowNumber, editCell3.ColumnNumber) = txtEdit3.Text
                    'If oldval <> Convert.ToInt16(txtEdit.Text) Then
                    '    rejected += oldval
                    '    rejected -= Convert.ToInt16(txtEdit.Text)
                    '    Me.Text = "Rejected Items - " & rejected
                    '    NumericUpDown1.Maximum = rejected
                    '    NumericUpDown1.Value = 1
                    'End If
                    DataGrid3.CurrentCell = currentCell
                    DataGrid3.Visible = True
                    inUpdateMode3 = False
                    txtEdit3.Visible = False
                    inEditMode3 = False
                End If
            End If

            ' Enter edit mode
            editCell3 = DataGrid3.CurrentCell
            'If editCell3.ColumnNumber = 1 Then
            '    Exit Sub
            'Else
            '    oldval = DataGrid1(editCell3.RowNumber, editCell3.ColumnNumber).ToString
            'End If
            txtEdit3.Text = DataGrid3(editCell3.RowNumber, editCell3.ColumnNumber).ToString
            Dim cellPos As Rectangle = DataGrid3.GetCellBounds(editCell3.RowNumber, editCell3.ColumnNumber)
            txtEdit3.Left = cellPos.Left - 1
            txtEdit3.Top = cellPos.Top + DataGrid3.Top - 1
            txtEdit3.Width = cellPos.Width + 2
            txtEdit3.Height = cellPos.Height + 2
            txtEdit3.Visible = True
            inEditMode3 = True
        End If
        DataGrid3.Select(DataGrid3.CurrentRowIndex)
        DataGrid3.Visible = True
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Dim rowIndex As Integer = DataGrid1.CurrentRowIndex

        pars.Rows(rowIndex).Delete()
        DataGrid1.DataSource = pars
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        Dim rowIndex As Integer = DataGrid2.CurrentRowIndex

        Rej.Rows(rowIndex).Delete()
        DataGrid2.DataSource = Rej
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        Dim rowIndex As Integer = DataGrid3.CurrentRowIndex

        usr.Rows(rowIndex).Delete()
        DataGrid3.DataSource = usr
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Me.DialogResult = Windows.Forms.DialogResult.OK
    End Sub
End Class