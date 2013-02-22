Public Class dlgScanDeliver

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

    Private Sub lotnumber_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles lotnumber.KeyPress
        Select Case e.KeyChar
            Case vbCrLf
                Me.btnOK.Enabled = True
                Result = DialogResult.OK
                EndDialog()
            Case Else
                If Me.lotnumber.Text.Length > 0 Then
                    Result = DialogResult.OK
                End If
        End Select
    End Sub

End Class
