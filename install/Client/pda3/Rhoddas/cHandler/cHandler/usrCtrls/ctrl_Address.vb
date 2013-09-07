Imports PriorityMobile

Public Class ctrl_Address
    Inherits iView

    Public Overrides Sub Bind()

    End Sub

    Public Overrides Sub CurrentChanged()
        Dim build As String = ""
        With thisForm.Parent
            Gtext.AddNamePair("Customer", .CurrentRow("custname"))            
        End With
        With thisForm
            Gtext.AddNamePair("Payment Terms", .CurrentRow("paymentterm"))
            Gtext.AddNamePair("Contact Name", .CurrentRow("contact"))
            Gtext.AddNamePair("Phone Number", .CurrentRow("phone"))
            Gtext.AddNamePair("Address", _
                CRLFifData(.CurrentRow("address1")) & _
                CRLFifData(.CurrentRow("address2")) & _
                CRLFifData(.CurrentRow("address3")) & _
                CRLFifData(.CurrentRow("address4")) & _
                CRLFifData(.CurrentRow("address5")) & _
                CRLFifData(.CurrentRow("postcode")) _
            )
        End With
        Gtext.PARSE()
    End Sub

    Public Function CRLFifData(ByVal str As String) As String
        If Len(Trim(str)) > 0 Then
            Return str & vbCrLf
        Else
            Return ""
        End If
    End Function

#Region "Direct Activations"

    Public Overrides Sub DirectActivations(ByRef ToolBar As daToolbar)
        ToolBar.Add(AddressOf hPlaceCall, "PHONE.BMP", thisForm.CurrentRow("phone").ToString.Length > 0)
    End Sub

    Private Sub hPlaceCall()

        Dim ph As New Microsoft.WindowsMobile.Telephony.Phone
        Try
            ph.Talk(thisForm.CurrentRow("phone"))
        Catch ex As Exception
            MsgBox(String.Format("Call failed to: {0}.", thisForm.CurrentRow("phone")))
        End Try

    End Sub

#End Region
End Class
