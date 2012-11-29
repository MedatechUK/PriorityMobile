Public Class dlgBarcode
    Inherits PriorityMobile.UserDialog

    Private scanValue As String = ""

    Public Overrides ReadOnly Property MyControls() As ControlCollection
        Get
            Return Me.Controls
        End Get
    End Property

    Private Sub btn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click, btnOK.Click
        Dim btn As Button = sender
        If btn.DialogResult = DialogResult.Cancel Then
            Result = DialogResult.Cancel
            EndDialog()
        Else
            If txtSerialNumber.Text.Length > 0 Then
                Result = btn.DialogResult
                EndDialog()
            Else
                MsgBox("Please enter a serial number.")
            End If
        End If
    End Sub

    Private Sub txtSerialNumber_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtSerialNumber.KeyUp
        e.Handled = True
        If e.KeyCode = Keys.Enter Then
            Me.txtSerialNumber.Text = scanValue
            Dim btn As New Button
            btn.DialogResult = DialogResult.OK
            btn_Click(btn, New System.EventArgs)
        Else
            scanValue += Chr(e.KeyCode)
        End If
        Me.txtSerialNumber.Text = scanValue
    End Sub

    Private Sub dlgAddPlanned_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Resize
        With Me
            .btnCancel.Width = .Width / 2
            .btnOK.Width = .Width / 2
        End With
    End Sub

End Class
