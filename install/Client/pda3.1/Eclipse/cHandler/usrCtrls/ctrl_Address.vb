Imports PriorityMobile
Imports System.Xml

Public Class ctrl_address
    Inherits iView

    Public Overrides Sub Bind()

    End Sub

    Public Overrides Sub CurrentChanged()
        Dim build As String = ""
        With thisForm.Parent
            Gtext.AddNamePair("Customer", .CurrentRow("custname"))
            Gtext.AddNamePair("Phone Number", .CurrentRow("phone"))
        End With
        With thisForm
            Gtext.AddNamePair("Contact Name", .CurrentRow("contact"))
            Gtext.AddNamePair("Contact Phone", .CurrentRow("contphone"))
            Gtext.AddNamePair("Address", _
                              CRLFifData(.CurrentRow("sitedes")) & _
                CRLFifData(.CurrentRow("address1")) & _
                CRLFifData(.CurrentRow("address2")) & _
                CRLFifData(.CurrentRow("address3")) & _
                CRLFifData(.CurrentRow("address4")) & _
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

    Public Overrides Sub DirectActivations(ByRef ToolBar As daToolbar)
        With ToolBar
            ToolBar.Add(AddressOf hPlaceCall, "PHONE.BMP", thisForm.CurrentRow("phone").ToString.Length > 0)
        End With
    End Sub

    Private Sub hPlaceCall()
        With thisForm
            With .FormData
                Dim n As XmlNode = thisForm.FormData.SelectSingleNode(thisForm.thisxPath).ParentNode
                If IsNothing(n.SelectSingleNode("report/times/called")) Then
                    n = n.SelectSingleNode("report/times")
                    n.AppendChild(.CreateElement("called"))
                    n.LastChild.InnerText = funcDate.TimeToMin()
                End If

            End With
            .Save()
        End With

        Dim ph As New Microsoft.WindowsMobile.Telephony.Phone
        Try
            ph.Talk(thisForm.CurrentRow("phone"))
        Catch ex As Exception
            MsgBox(String.Format("Call failed to: {0}.", thisForm.CurrentRow("phone")))
        End Try

    End Sub
End Class
