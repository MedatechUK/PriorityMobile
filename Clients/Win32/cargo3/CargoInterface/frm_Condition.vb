'Imports cargo3

'Public Class frm_Condition

'    Public Sub New(ByVal ConditionName As String)
'        InitializeComponent()

'        Me.lbl_Condition.Text = ConditionName
'        For Each coord As Coordinate In myStates.myCoordinates.Values
'            lst_Coord.Items.Add(coord.Name)
'        Next
'        If myStates.myConditions.Keys.Contains(ConditionName) Then
'            With myStates.myConditions(ConditionName)
'                lst_Coord.SelectedItem = .thisCoordinate.Name
'                Check_Not.Checked = .BoolNot
'                For Each col As Colour In .Colours
'                    lst_Colour.Items.Add(col.Name)
'                    lst_Colour.Items(lst_Colour.Items.Count - 1).BackColor = Color.FromArgb(col.Red, col.Green, col.Blue)
'                Next
'            End With
'        End If
'        For Each allcol In myStates.myColours.Values
'            lst_AllColours.Items.Add(allcol.Name)
'            lst_AllColours.Items(lst_AllColours.Items.Count - 1).BackColor = Color.FromArgb(allcol.Red, allcol.Green, allcol.Blue)
'        Next
'    End Sub

'    Private Sub frm_Condition_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
'        If Not (myStates.myConditions.Keys.Contains(Me.lbl_Condition.Text)) Then
'            myStates.myConditions.Add(Me.lbl_Condition.Text, New Condition(Me.lbl_Condition.Text, myStates.myCoordinates(Me.lst_Coord.SelectedItem), Me.Check_Not.Checked))
'        Else
'            With myStates.myConditions(Me.lbl_Condition.Text)
'                .thisCoordinate = myStates.myCoordinates(Me.lst_Coord.SelectedItem)
'                .BoolNot = Me.Check_Not.Checked
'            End With
'        End If

'        With myStates.myConditions(Me.lbl_Condition.Text).Colours
'            .Clear()
'            For Each i As ListViewItem In lst_Colour.Items
'                .Add(myStates.myColours(i.Text))
'            Next
'        End With

'    End Sub

'    Private Sub lst_AllColours_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lst_AllColours.DoubleClick
'        If lst_AllColours.SelectedItems.Count > 0 Then
'            lst_Colour.Items.Add(lst_AllColours.SelectedItems(0).Clone)
'        End If
'    End Sub

'    Private Sub lst_Colour_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles lst_Colour.KeyPress
'        If Asc(e.KeyChar) = 8 Then
'            If lst_Colour.SelectedItems.Count > 0 Then
'                lst_Colour.Items.Remove(lst_Colour.SelectedItems(0))
'            End If
'        End If
'    End Sub

'End Class