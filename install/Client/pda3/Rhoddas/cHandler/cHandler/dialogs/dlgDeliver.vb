Public Class dlgDeliver

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

    Private Sub LotView_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LotView.SelectedIndexChanged
        Me.btnOK.Enabled = Not (LotView.SelectedIndices.Count = 0)
    End Sub

End Class
