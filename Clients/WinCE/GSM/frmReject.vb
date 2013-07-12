Imports System.Data

Public Class frmReject
    Public operation As String
    Public rejected As Integer
    Public Rejs As New DataTable
    Private oldval As Integer = 0

    Private editCell As DataGridCell
    Private inEditMode As Boolean = False
    Private inUpdateMode As Boolean = False

    Private Sub frmReject_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'first up we need to set the max and min choosable amounts and start a new list of rejects for the data table
        NumericUpDown1.Minimum = 1
        NumericUpDown1.Maximum = rejected

        'Create a datatable to hold the rejects


        'create the columns for the table
        Dim c1 As New DataColumn
        Dim c2 As New DataColumn

        'Col 1 is the quantity
        c1.DataType = System.Type.GetType("System.Int32")
        c1.ColumnName = "Quant"
        c1.Caption = "Quant"
        c1.AutoIncrement = False
        rejs.Columns.Add(c1)

        'Col 2 is the reason
        c2.DataType = System.Type.GetType("System.String")
        c2.ColumnName = "Reason"
        c2.Caption = "Reason"
        c2.AutoIncrement = False
        rejs.Columns.Add(c2)

        DataGrid1.DataSource = rejs
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        'Dim l As New Rejects(NumericUpDown1.Value, operation, ComboBox1.Text, " ")
        Dim l As DataRow
        l = Rejs.NewRow
        l("Quant") = NumericUpDown1.Value
        l("Reason") = ComboBox1.Text
        Rejs.Rows.Add(l)
        DataGrid1.DataSource = Nothing
        DataGrid1.DataSource = rejs

        rejected -= NumericUpDown1.Value
        If rejected = 0 Then
            NumericUpDown1.Minimum = 0
            NumericUpDown1.Maximum = 0
            NumericUpDown1.Value = 0
        Else
            NumericUpDown1.Maximum = rejected
            NumericUpDown1.Value = 1
        End If



    End Sub

    Private Sub DataGrid1_CurrentCellChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DataGrid1.CurrentCellChanged
        If Not inUpdateMode Then
            If inEditMode AndAlso Not DataGrid1.CurrentCell.Equals(editCell) Then
                ' Update edited cell
                'as we are only able to update the quantity we must ensure it is numeric
                If IsNumeric(txtEdit.Text) = True Then


                    inUpdateMode = True
                    DataGrid1.Visible = False
                    Dim currentCell As DataGridCell = DataGrid1.CurrentCell
                    DataGrid1(editCell.RowNumber, editCell.ColumnNumber) = txtEdit.Text
                    If oldval <> Convert.ToInt16(txtEdit.Text) Then
                        rejected += oldval
                        rejected -= Convert.ToInt16(txtEdit.Text)
                        Me.Text = "Rejected Items - " & rejected
                        NumericUpDown1.Maximum = rejected
                        NumericUpDown1.Value = 1
                    End If
                    DataGrid1.CurrentCell = currentCell
                    DataGrid1.Visible = True
                    inUpdateMode = False
                    txtEdit.Visible = False
                    inEditMode = False
                End If
            End If

            ' Enter edit mode
            editCell = DataGrid1.CurrentCell
            If editCell.ColumnNumber = 1 Then
                Exit Sub
            Else
                oldval = DataGrid1(editCell.RowNumber, editCell.ColumnNumber).ToString
            End If
            txtEdit.Text = DataGrid1(editCell.RowNumber, editCell.ColumnNumber).ToString
            Dim cellPos As Rectangle = DataGrid1.GetCellBounds(editCell.RowNumber, editCell.ColumnNumber)
            txtEdit.Left = cellPos.Left - 1
            txtEdit.Top = cellPos.Top + DataGrid1.Top - 1
            txtEdit.Width = cellPos.Width + 2
            txtEdit.Height = cellPos.Height + 2
            txtEdit.Visible = True
            inEditMode = True
        End If
        DataGrid1.Select(DataGrid1.CurrentRowIndex)
        DataGrid1.Visible = True
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        If rejected = 0 Then
            DialogResult = Windows.Forms.DialogResult.OK
        Else
            MsgBox("There are still undocumented rejects, please provide a reason for them before trying to close the form")
        End If
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        Dim rowIndex As Integer = DataGrid1.CurrentRowIndex

        Rejs.Rows(rowIndex).Delete()
        DataGrid1.DataSource = Rejs

    End Sub
End Class