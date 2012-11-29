Public Class dlgAddPlanned

    Inherits PriorityMobile.UserDialog

    Public Overrides ReadOnly Property MyControls() As ControlCollection
        Get
            Return Me.Controls
        End Get
    End Property

    Private Sub chkBroken_CheckStateChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkBroken.CheckStateChanged
        txtLocation.Enabled = Not (chkBroken.Checked)
    End Sub

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

    Private Sub hLostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSerialNumber.LostFocus, chkBroken.LostFocus, txtLocation.LostFocus
        btnOK.Enabled = False
        If txtSerialNumber.Text.Length > 0 Then
            If txtLocation.Text.Length > 0 Or chkBroken.Checked Then
                btnOK.Enabled = True
            End If
        End If
    End Sub

End Class
