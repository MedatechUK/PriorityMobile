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
        If btn.DialogResult = DialogResult.Cancel Then
            Result = DialogResult.Cancel
            EndDialog()
        Else
            If txtSerialNumber.Text.Length > 0 Then
                If txtLocation.Text.Length > 0 Or chkBroken.Checked Then
                    Result = btn.DialogResult
                    EndDialog()
                Else
                    MsgBox("Please enter a location or unit removed.")
                End If
            Else
                MsgBox("Please enter a serial number.")
            End If
        End If
    End Sub

    Private Sub dlgAddPlanned_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Resize
        With Me
            .btnCancel.Width = .Width / 2
            .btnOK.Width = .Width / 2
        End With
    End Sub

End Class
