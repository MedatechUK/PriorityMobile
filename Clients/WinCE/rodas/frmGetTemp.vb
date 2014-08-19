Public Class frmGetTemp

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If IsNumeric(TextBox1.Text) = True Then
            Me.DialogResult = DialogResult.OK
        End If
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        If IsNumeric(TextBox1.Text) = False Then
            MsgBox("Please enter numeric data only!")
            TextBox1.Text = ""
            Exit Sub
        End If
        If TextBox1.Text = "" Or TextBox1.Text = "0" Then Exit Sub
        If IsNumeric(TextBox1.Text) = True And Convert.ToDecimal((TextBox1.Text)) <> 0 Then
            TextBox1.Text *= -1
        End If
    End Sub
End Class