Imports PriorityMobile

Public Class ctrl_Clean
    Inherits iView

    Public Overrides Sub FormClosing()
        thisForm.Save()
    End Sub

    Public Overrides Sub Bind()
        Survey.FormLabel = "Vehicle Cleanliness"
        Survey.LoadSurvey(thisForm.FormData.SelectSingleNode(thisForm.thisxPath))
    End Sub

    Private Sub Survey_NewResponse(ByVal QuestionNumber As Integer, ByVal Value As String, ByVal Text As String) Handles Survey.NewResponse
        With thisForm
            With .FormData
                If Not IsNothing(Text) Then
                    .SelectSingleNode(String.Format("{0}/question[number='{1}']/response/text", thisForm.thisxPath, QuestionNumber.ToString)).InnerText = Text
                End If
                If Not IsNothing(Value) Then
                    .SelectSingleNode(String.Format("{0}/question[number='{1}']/response/value", thisForm.thisxPath, QuestionNumber.ToString)).InnerText = Value
                End If
                .SelectSingleNode("pdadata/maintainance").Attributes.Append(xmlForms.changedAttribute)
            End With
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
