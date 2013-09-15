Imports PrioritySFDC
Imports Priority
Imports CPCL

Public Class myHandler : Inherits iHandler

    Public Overrides Sub DisabledButtons(ByRef Add As Boolean, ByRef Edit As Boolean, ByRef Copy As Boolean, ByRef Delete As Boolean, ByRef Print As Boolean)
        'Print = True
    End Sub

    Public Overrides Sub btn_PrintPress(ByRef thisForm As PrioritySFDC.iForm)
        'MyBase.btn_PrintPress(thisForm)
        'thisForm.Printer
        'thisForm.ViewMain.TableView.ViewTable.thisTable.SelectedIndexCollection()
        With thisForm.Printer
            If Not .Connected Then
                .BeginConnect(thisForm.ue.MACAddress)
                Do While .WaitConnect
                    Threading.Thread.Sleep(100)
                Loop
            Else
                PrintForm(thisForm)
            End If            
        End With
    End Sub

    Public Overrides Sub PrintForm(ByRef thisForm As PrioritySFDC.iForm)
        Dim headerFont As New PrinterFont(50, 5, 2) 'variable width. 

        Using TestPrint As New CPCL.Label(thisForm.Printer, eLabelStyle.receipt)
            With TestPrint
                'logo
                .AddImage("roddas.pcx", New Point(186, thisForm.Printer.Dimensions.Height + 10), 147)

                'line
                .AddLine(New Point(10, thisForm.Printer.Dimensions.Height + 10), _
                         New Point(thisForm.Printer.Dimensions.Width - 10, thisForm.Printer.Dimensions.Height + 10), 10, 15)

                'header = 334px wide
                .AddText("TEST PRINT", New Point((thisForm.Printer.Dimensions.Width / 2) - 86, thisForm.Printer.Dimensions.Height + 10), _
                         headerFont)

                'line
                .AddLine(New Point(10, thisForm.Printer.Dimensions.Height + 10), _
                         New Point(thisForm.Printer.Dimensions.Width - 10, thisForm.Printer.Dimensions.Height + 10), 10)

                'tear 'n' print!
                .AddTearArea(New Point(0, thisForm.Printer.Dimensions.Height))
                thisForm.Printer.Print(.toByte)

            End With
        End Using

    End Sub

    Public Overrides Sub btn_AddPress(ByRef thisForm As PrioritySFDC.iForm)
        'thisForm.Dialog(New dlgTestDialog, "add")
        With thisForm.ViewMain.TableView
            .TableView = TableView.eTableView.vForm
        End With
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
                thisForm.ViewMain.FormView.ViewForm.FocusedControl.thisColumn.isReadOnly = True
                'thisForm.ViewMain.FormView.Focus()

            Case "add"
                If frmDialog.Result = DialogResult.OK Then
                    MyBase.btn_AddPress(thisForm)
                    'thisForm.ViewMain.FormView.ViewForm.Columns("READONLY").Value = "Test"
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

    Public Overrides Function InitCalc(ByRef uiCol As PrioritySFDC.uiColumn) As PrioritySFDC.calcSetting
        Select Case uiCol.thisColumn.Name.ToLower
            Case "tquant"
                Dim cs As calcSetting = MyBase.InitCalc(uiCol)
                cs.Max = 10
                cs.Min = -10
                Return cs
            Case Else
                Return MyBase.InitCalc(uiCol)
        End Select

    End Function

End Class
