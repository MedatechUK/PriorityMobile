Imports PriorityMobile

Public Class ctrl_Balances
    Inherits iView

    Public Overrides Sub Bind()
        With Me
            Try
                .paymentterms.DataBindings.Add("Text", thisForm.TableData, "paymentterms")
                .overduepayment.DataBindings.Add("Text", thisForm.TableData, "overduepayment")
                .dueamount.DataBindings.Add("Text", thisForm.TableData, "dueamount")
                .todaysinvoicetotals.DataBindings.Add("Text", thisForm.TableData, "todaysinvoicetotals")
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        End With
    End Sub

#Region "Direct Activations"

    'Public Overrides Sub DirectActivations(ByRef ToolBar As daToolbar)
    '    ToolBar.Add(AddressOf hPlaceCall, "PHONE.BMP", thisForm.CurrentRow("phone").ToString.Length > 0)
    'End Sub

    'Private Sub hPlaceCall()

    '    Dim ph As New Microsoft.WindowsMobile.Telephony.Phone
    '    Try
    '        ph.Talk(thisForm.CurrentRow("phone"))
    '    Catch ex As Exception
    '        MsgBox(String.Format("Call failed to: {0}.", thisForm.CurrentRow("phone")))
    '    End Try

    'End Sub

#End Region

End Class
