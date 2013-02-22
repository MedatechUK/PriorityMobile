Public Class dlgAddCredit

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

    Private Sub CreditQty_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CreditQty.ValueChanged
        With rcvQty
            If .Value > CreditQty.Value Then
                .Value = CreditQty.Value
            End If
            .Maximum = CreditQty.Value
        End With
    End Sub

    Private Sub CreditReason_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CreditReason.SelectedIndexChanged
        Select Case CreditReason.SelectedIndex
            Case 0
                Me.btnOK.Enabled = False
            Case Else
                Me.btnOK.Enabled = True
        End Select
    End Sub

End Class
