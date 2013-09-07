Imports PrioritySFDC

Public Class Handler : Inherits iHandler

    Public Overrides Sub DisabledButtons(ByRef Add As Boolean, ByRef Edit As Boolean, ByRef Copy As Boolean, ByRef Delete As Boolean, ByRef Print As Boolean)
        Print = True
    End Sub

    Public Overrides Sub btn_AddPress(ByRef thisForm As PrioritySFDC.iForm)
        thisForm.Dialog(New dlgTestDialog, "add")
    End Sub

    Public Overrides Sub btn_PostPress(ByRef thisForm As PrioritySFDC.iForm)
        thisForm.ViewMain.FormView.ViewForm.SelectColumn("supname")
        thisForm.ViewMain.Focus()
        'MyBase.btn_PostPress(thisForm)
    End Sub

    Public Overrides Sub CloseDialog(ByRef thisForm As PrioritySFDC.iForm, ByRef frmDialog As PrioritySFDC.UserDialog)
        Select Case frmDialog.frmName
            Case "alt"
                thisForm.ViewMain.FormView.ViewForm.SelectColumn("supname")
                'thisForm.ViewMain.FormView.Focus()
            Case "add"
                If frmDialog.Result = DialogResult.OK Then
                    MyBase.btn_AddPress(thisForm)
                    thisForm.ViewMain.FormView.ViewForm.Columns("READONLY").Value = "Test"
                    'Dim data As Data.DataTable = thisForm.DataService.ExecuteReader("")
                End If

        End Select

    End Sub

    Public Overrides Sub AltEntry(ByRef uiCol As PrioritySFDC.uiColumn)
        Select Case uiCol.thisColumn.Name.ToLower
            Case "supname"

                uiCol.ParentForm.Dialog(New dlgTestDialog, "alt")

            Case Else
                MyBase.AltEntry(uiCol)

        End Select

    End Sub

End Class
