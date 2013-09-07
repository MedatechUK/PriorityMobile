'Public Class frm_Conditions

'    Private Sub frm_Conditions_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
'        For Each cond As cargo3.Condition In myStates.myConditions.Values
'            lst_Condition.Items.Add(cond.Name)
'        Next
'    End Sub

'    Private Sub lst_Condition_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lst_Condition.DoubleClick
'        Dim fCond As New frm_Condition(lst_Condition.SelectedItems(0).Text)
'        fCond.ShowDialog()
'    End Sub

'    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
'        Dim NewName As String = InputBox("Coordinate Name")
'        If NewName.Length > 0 Then
'            Dim fCond As New frm_Condition(NewName)
'            fCond.ShowDialog()
'            lst_Condition.Items.Clear()
'            For Each cond As cargo3.Condition In myStates.myConditions.Values
'                lst_Condition.Items.Add(cond.Name)
'            Next
'        End If
'    End Sub

'End Class