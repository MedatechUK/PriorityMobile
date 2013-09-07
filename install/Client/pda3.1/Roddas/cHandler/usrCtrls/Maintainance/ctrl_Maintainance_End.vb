Imports PriorityMobile
Imports System.Xml

Public Class ctrl_Maintainance_End
    Inherits iView

    Public Overrides Sub Bind()
        With gtext
            .thisHTML = thisForm.FormData.SelectSingleNode("pdadata/maintainance/dayend/text").InnerText
            .PARSE()
        End With
    End Sub

    Public Overrides Function SubFormVisible(ByVal Name As String) As Boolean

        Dim ret As Boolean = True
        Select Case Name.ToUpper
            Case "ROUTE END"
                Dim maintainance As XmlNode = thisForm.FormData.SelectSingleNode("pdadata/maintainance/dayend")
                For Each response As XmlNode In maintainance.SelectNodes(String.Format(".//question", ""))
                    With response
                        If Not IsNothing(.SelectSingleNode("mandatory")) Then
                            If .SelectSingleNode("mandatory").InnerText = "Y" Then
                                If .SelectSingleNode("response/text").InnerText.Length = 0 And .SelectSingleNode("response/value").InnerText.Length = 0 Then
                                    Return False
                                End If
                            End If
                        End If
                    End With
                Next
                Return True
        End Select

        Return ret

    End Function

End Class
