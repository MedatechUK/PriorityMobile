Imports PriorityMobile

Public Class ctrl_Customer
    Inherits iView

    Public Overrides Sub Bind()
        With Me
            Try
                .custnumber.DataBindings.Add("Text", thisForm.TableData, "custnumber")
                .custname.DataBindings.Add("Text", thisForm.TableData, "custname")
                .contact.DataBindings.Add("Text", thisForm.TableData, "contact")
                .phone.DataBindings.Add("Text", thisForm.TableData, "phone")
                .postcode.DataBindings.Add("Text", thisForm.TableData, "postcode")
            Catch 
            End Try
        End With
    End Sub

#Region "Direct Activations"

    Public Overrides Sub DirectActivations(ByRef ToolBar As daToolbar)
        ToolBar.Add(AddressOf hPlaceCall, "PHONE.BMP", thisForm.CurrentRow("phone").ToString.Length > 0)
    End Sub

    Private Sub hPlaceCall()

        'Dim ph As New Microsoft.WindowsMobile.Telephony.Phone
        'Try
        '    ph.Talk(thisForm.CurrentRow("phone"))
        'Catch ex As Exception
        '    MsgBox(String.Format("Call failed to: {0}.", thisForm.CurrentRow("phone")))
        'End Try

    End Sub

#End Region

End Class
