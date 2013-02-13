Imports PriorityMobile

Public Class ctrl_Payment
    Inherits iView

    Public Overrides Sub Bind()
        With Me
            Try
                .cash.DataBindings.Add("Text", thisForm.TableData, "cash")
                .cheque.DataBindings.Add("Text", thisForm.TableData, "cheque")
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        End With
    End Sub

#Region "Direct Activations"

    Public Overrides Sub DirectActivations(ByRef ToolBar As daToolbar)
        With Me
            ToolBar.Add(AddressOf hPrint, "print.BMP", (CDbl(.cash.Text) + CDbl(.cheque.Text)) > 0)
        End With
    End Sub


    Private Sub hPrint()
        MsgBox("Printing...", MsgBoxStyle.OkOnly)
    End Sub

#End Region

End Class
