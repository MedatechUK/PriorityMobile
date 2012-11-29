
Public Class BooleanPropertyEditor
    Inherits PropertyEditorBase

    Private WithEvents myListBox As New Windows.Forms.ListBox 'this is the control to be used in design time DropDown editor

    Protected Overrides Function GetEditControl(ByVal PropertyName As String, ByVal CurrentValue As Object) As System.Windows.Forms.Control
        With myListBox
            .BorderStyle = System.Windows.Forms.BorderStyle.None
            'Creating ListBox items... 
            'Note that as this is executed in design mode, performance is not important and there is no need to cache these items if they can change each time.
            .Items.Clear() 'clear previous items if any
            .Items.Add("True")
            .Items.Add("False")
            .SelectedIndex = myListBox.FindString(CurrentValue) 'Select current item based on CurrentValue of the property
            .Height = myListBox.PreferredHeight
        End With
        Return myListBox
    End Function

    Protected Overrides Function GetEditedValue(ByVal EditControl As System.Windows.Forms.Control, ByVal PropertyName As String, ByVal OldValue As Object) As Object
        Return CBool(myListBox.Text) '.Substring(0, 2) 'return new value for property
    End Function

    Private Sub myTreeView_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles myListBox.Click
        Me.CloseDropDownWindow() 'when user clicks on an item, the edit process is done.
    End Sub

End Class