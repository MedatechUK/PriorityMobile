Public Class frm_States

    Private Sub frm_Conditions_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        For Each cond As cargo3.State In myStates.Values
            lst_States.Items.Add(cond.Name)
        Next
    End Sub

    Private Sub lst_Condition_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lst_States.DoubleClick
        Dim fstate As New frm_State(lst_States.SelectedItems(0).Text)
        fstate.ShowDialog()
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        Dim NewName As String = InputBox("State Name")
        If NewName.Length > 0 Then
            Dim fstate As New frm_State(NewName)
            fstate.ShowDialog()
        End If
    End Sub

End Class