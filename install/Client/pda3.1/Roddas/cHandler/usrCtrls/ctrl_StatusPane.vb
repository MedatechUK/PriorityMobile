Imports System.Xml
Imports System.Data
Imports PriorityMobile

Public Class ctrl_StatusPane
    Inherits PriorityMobile.iView

#Region "Overrides base class"

    Public Overrides Sub Bind()


        With Me
            '.TextBox1.DataBindings.Clear()
            '.ComboBox1.DataBindings.Clear()
            '.txt_CallType.DataBindings.Clear()
            '.txt_Contact.DataBindings.Clear()
            '.txt_Customer.DataBindings.Clear()
            '.txt_Phone.DataBindings.Clear()
            '.txt_Serial.DataBindings.Clear()
            '.txt_location.DataBindings.Clear()
            Try
                .TextBox1.DataBindings.Add("Text", thisForm.TableData, "callnumber")
                .ComboBox1.DataBindings.Add("SelectedValue", thisForm.TableData, "callstatus")
                .txt_CallType.DataBindings.Add("Text", thisForm.TableData, "servtype")
                .txt_Contact.DataBindings.Add("Text", thisForm.TableData, "contact")
                .txt_Customer.DataBindings.Add("Text", thisForm.TableData, "custname")
                .txt_Phone.DataBindings.Add("Text", thisForm.TableData, "phone")
                .txt_Serial.DataBindings.Add("Text", thisForm.TableData, "serial")
                .txt_location.DataBindings.Add("Text", thisForm.TableData, "location")
            Catch
            End Try
        End With
    End Sub

    Public Overrides Sub CurrentChanged()
        ListBind(ComboBox1, "Status")
    End Sub

    Public Overrides Function ValidColumn(ByVal ColumnName As String, ByVal ProposedValue As String) As Boolean

        Select Case ColumnName.ToUpper
            Case "CALLSTATUS"

                If Not thisForm.LookUp.AllowedValues("Status").Contains(ProposedValue) Then Return False

                Dim i As Integer = 1
                Dim activeStatus As List(Of String) = xmlForms.StatusList(eStatusRule.active)
                Dim query As String = ""
                For Each act In activeStatus
                    query += String.Format("'{0}'", act)
                    If i < activeStatus.Count Then query += ","
                    i += 1
                Next

                Dim dr() As DataRow = Nothing
                dr = thisForm.Datasource.Select(String.Format("callnumber <> '{0}' and callstatus in ({1})", Me.TextBox1.Text, query))
                If dr.Count > 0 Then
                    MsgBox( _
                        String.Format( _
                            "You are already {0} for call {1}.", _
                            dr(0).Item("callstatus"), _
                            dr(0).Item("callnumber") _
                        ) _
                    )
                End If
                If dr.Count > 0 Then Return False

                If xmlForms.StatusRule(ProposedValue, eStatusRule.post) Then
                    Dim ret As Boolean = True
                    Dim n As XmlNode = thisForm.FormData.SelectSingleNode(thisForm.thisxPath)
                    If n.SelectSingleNode("report/detail/malfunction").InnerText.Length = 0 Then ret = False
                    If n.SelectSingleNode("report/detail/resolution").InnerText.Length = 0 Then ret = False
                    If n.SelectSingleNode("report/detail/repair").InnerText.Length = 0 Then ret = False
                    If n.SelectSingleNode("report/signature/image").InnerText.Length = 0 Then ret = False
                    If n.SelectSingleNode("report/signature/print").InnerText.Length = 0 Then ret = False
                    If Not ret Then
                        MsgBox( _
                            String.Format( _
                                "Cannot {0} incomplete form.", _
                                ProposedValue _
                            ) _
                        )
                    End If
                    Return ret

                Else
                    Return MyBase.ValidColumn(ColumnName, ProposedValue)
                End If

            Case Else
                Return MyBase.ValidColumn(ColumnName, ProposedValue)
        End Select

    End Function

    Public Overrides Function SubFormVisible(ByVal Name As String) As Boolean
        With Me
            If IsNothing(.Selected) Then Return False
            If IsNothing(.thisForm.TableData.Current) Then Return False            
            Select Case Name.ToUpper
                Case "REPAIR", "PARTS", "SIGN"
                    Return Not xmlForms.StatusRule(.thisForm.CurrentRow("callstatus").ToString, eStatusRule.prereport) And Not xmlForms.StatusRule(.thisForm.CurrentRow("callstatus").ToString, eStatusRule.post)
                Case Else
                    Return True
            End Select
        End With
    End Function

    Public Overrides Sub RowUpdated(ByVal Column As String, ByVal NewValue As String)
        Select Case Column.ToUpper
            Case "CALLSTATUS"
                Select Case NewValue.ToUpper
                    Case Else
                        With thisForm
                            With .FormData
                                Dim n As XmlNode = thisForm.FormData.SelectSingleNode(thisForm.thisxPath & "/report/times")
                                n.AppendChild(.CreateElement(Replace(NewValue.ToUpper, Chr(32), "_")))
                                n.LastChild.InnerText = funcDate.DateToMin()
                            End With
                            .Save()
                        End With
                End Select
            Case Else

        End Select
    End Sub

#End Region

#Region "Direct Activations"

    Public Overrides Sub DirectActivations(ByRef ToolBar As daToolbar)
        ToolBar.Add(AddressOf hPlaceCall, "PHONE.BMP", thisForm.CurrentRow("phone").ToString.Length > 0)
    End Sub

    Private Sub hPlaceCall()
        With thisForm
            With .FormData
                Dim n As XmlNode = thisForm.FormData.SelectSingleNode(thisForm.thisxPath)
                If IsNothing(n.SelectSingleNode("report/times/CALLED")) Then
                    n = n.SelectSingleNode("report/times")
                    n.AppendChild(.CreateElement("CALLED"))
                    n.LastChild.InnerText = DateDiff(DateInterval.Minute, #1/1/1988#, Now)
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

#End Region

End Class
