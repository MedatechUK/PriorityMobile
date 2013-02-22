Public Class dlgCloseDelivery

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

    Private Sub delivered_CheckStateChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Delivered.CheckStateChanged
        With Reason
            If Delivered.Checked Then
                .SelectedIndex = 0
                .Enabled = False
                btnOK.Enabled = True
            Else
                .Enabled = True
                btnOK.Enabled = False
            End If
        End With
    End Sub

    Private Sub Reason_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Reason.SelectedIndexChanged
        If Reason.SelectedIndex > 0 Then
            btnOK.Enabled = True
        Else
            btnOK.Enabled = False
        End If
    End Sub
End Class
