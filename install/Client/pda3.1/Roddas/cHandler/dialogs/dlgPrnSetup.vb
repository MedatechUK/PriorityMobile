Public Class dlgPRNSetup

    Inherits PriorityMobile.UserDialog

    Public Overrides ReadOnly Property MyControls() As ControlCollection
        Get
            Return Me.Controls
        End Get
    End Property

    Private Sub btn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click, btnOK.Click
        Dim btn As Button = sender
        Select Case btn.Name.ToLower
            Case "btnok"
                Result = DialogResult.OK
            Case "btncancel"
                Result = DialogResult.Cancel
        End Select
        EndDialog()
    End Sub

    Private Sub MACAddress_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles MACAddress.GotFocus
        Me.btnOK.Enabled = CBool(MACAddress.Text.Length = 12)
    End Sub

    Private Sub MACAddress_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MACAddress.TextChanged
        Me.btnOK.Enabled = CBool(MACAddress.Text.Length = 12)
    End Sub
End Class
