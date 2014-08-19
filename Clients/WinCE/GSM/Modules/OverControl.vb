Public Class OverControl
    Public Shared Sub msgboxa(ByVal msg As String)
        Dim f As New frmMsgBox
        f.Label1.Text = msg
        f.ShowDialog()
        If f.ShowDialog = DialogResult.OK Then
            f.Dispose()
        End If
    End Sub
End Class
