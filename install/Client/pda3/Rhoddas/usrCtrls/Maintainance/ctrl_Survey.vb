Imports System.Xml
Imports PriorityMobile

Public Class ctrl_Survey
    Inherits iView

    Public Overrides Sub FormClosing()
        thisForm.Save()
    End Sub

    Public Overrides Sub Bind()
        With thisForm
            Survey.FormLabel = .FormName
            Survey.LoadSurvey(.FormData.SelectSingleNode(.boundxPath))
        End With
    End Sub

    Private Sub Survey_NewResponse(ByVal QuestionNumber As Integer, ByVal Value As String, ByVal Text As String) Handles Survey.NewResponse
        With thisForm
            With .FormData
                If Not IsNothing(Text) Then
                    .SelectSingleNode(String.Format("{0}/question[number='{1}']/response/text", thisForm.boundxPath, QuestionNumber.ToString)).InnerText = Text
                End If
                If Not IsNothing(Value) Then
                    .SelectSingleNode(String.Format("{0}/question[number='{1}']/response/value", thisForm.boundxPath, QuestionNumber.ToString)).InnerText = Value
                End If
                Dim chNode As XmlNode = .SelectSingleNode("pdadata/maintainance")
                chNode.Attributes.Append(xmlForms.changedAttribute)
            End With
        End With
    End Sub

End Class
