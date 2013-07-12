Public Class frmPassCode

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If TextBox1.Text <> "Router" Then
            Exit Sub
        Else
            Me.DialogResult = Windows.Forms.DialogResult.OK
        End If
    End Sub
End Class