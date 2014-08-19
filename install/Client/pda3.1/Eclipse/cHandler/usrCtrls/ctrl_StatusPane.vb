Imports System.Xml
Imports System.Data
Imports PriorityMobile
Imports PriorityMobile.funcDate

Public Class ctrl_StatusPane
    Inherits PriorityMobile.iView

    Private dlg As dlgOnCall
    Private dlgTimer As System.Windows.Forms.Timer

#Region "Overrides base class"

    Public Overrides ReadOnly Property ButtomImage() As String
        Get
            Return "OpenCall.bmp"
        End Get
    End Property

    Public Overrides Sub Bind()
        With Me
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

                If dr.Count >= 0 Then 'Already on call

                    '    dlg = New dlgOnCall
                    '    dlg.Name = "ONCALL"
                    '    Dim l As Label = dlg.FindControl("lblOnCall")
                    '    Dim t As TextBox = dlg.FindControl("txtcallnumber")
                    '    l.Text = String.Format( _
                    '                "Call [{1}] is already in status [{0}]. Do you wish to open call [{1}] now?", _
                    '                dr(0).Item("callstatus"), _
                    '                dr(0).Item("callnumber") _
                    '            )
                    '    t.Text = dr(0).Item("callnumber")
                    '    thisForm.Dialog(dlg)

                    '    'dlgTimer = New System.Windows.Forms.Timer
                    '    'With dlgTimer
                    '    '    .Interval = 1000
                    '    '    AddHandler .Tick, AddressOf hdlgTimer
                    '    '    .Enabled = True
                    '    'End With

                    '    Return False

                    'Else

                    If xmlForms.StatusRule(ProposedValue, eStatusRule.post) Then
                        Dim ret As Boolean = True
                        Dim n As XmlNode = thisForm.FormData.SelectSingleNode(thisForm.thisxPath)
                        If n.SelectSingleNode("report/detail/malfunction").InnerText.Length = 0 Then ret = False
                        If n.SelectSingleNode("report/detail/resolution").InnerText.Length = 0 Then ret = False

                        If Not (xmlForms.StatusRule(ProposedValue, eStatusRule.postincomplete)) Then
                            If n.SelectSingleNode("report/detail/repair").InnerText.Length = 0 Then ret = False
                            If n.SelectSingleNode("report/signature/image").InnerText.Length = 0 Then ret = False
                            If n.SelectSingleNode("report/signature/print").InnerText.Length = 0 Then ret = False
                        End If

                        If Not ret Then
                            MsgBox( _
                                String.Format( _
                                    "Cannot set incomplete form to '{0}'.", _
                                    ProposedValue _
                                ) _
                            )
                        Else
                            Return MsgBox(String.Format("Set the call to '{0}'?", ProposedValue), MsgBoxStyle.OkCancel) = MsgBoxResult.Ok
                        End If

                    Else

                        ' Change of status to non post type
                        xmlForms.FormData.PostData("status", _
                            String.Format( _
                                           "<status><callnumber>{0}</callnumber><callstatus>{1}</callstatus></status>", _
                                           thisForm.CurrentRow("callnumber"), _
                                           ProposedValue _
                                         ), _
                        New Exception)
                        Return True
                    End If

                End If


            Case Else
                Return MyBase.ValidColumn(ColumnName, ProposedValue)
        End Select

    End Function

    Private Sub hdlgTimer(ByVal sender As Object, ByVal e As System.EventArgs)
        thisForm.Dialog(dlg)
        dlgTimer.Enabled = False
        dlg = Nothing
    End Sub

    Public Overrides Sub CloseDialog(ByVal frmDialog As PriorityMobile.UserDialog)
        With thisForm
            If frmDialog.Result = DialogResult.OK Then
                Dim t As TextBox = frmDialog.FindControl("txtcallnumber")
                .TableData.Position = .TableData.Find("CALLNUMBER", t.Text)
                Dim DT As DateTimePicker = .Views(0).FindControl("DateTimePicker")
                If Not IsNothing(DT) Then DT.Value = DateFromInt(CInt(.CurrentRow("calldate")))
            End If
            .RefreshForm()
            .RefreshSubForms()
        End With
    End Sub

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
                xmlForms.Log("Set CALLSTATUS to {0}.", NewValue)
                With thisForm
                    If xmlForms.StatusRule(NewValue, eStatusRule.post) Then
                        With .FormData
                            Dim n As XmlNode = thisForm.FormData.SelectSingleNode(thisForm.thisxPath & "/report/times")
                            n.AppendChild(.CreateElement(Replace("endcall", Chr(32), "_")))
                            n.LastChild.InnerText = funcDate.TimeToMin()

                            n = thisForm.FormData.SelectSingleNode(thisForm.thisxPath)
                            Dim appendstring As String
                            'appendstring = n.SelectSingleNode("detail").InnerText
                            appendstring &= ":[!--lastreport--]:" & n.SelectSingleNode("report/detail/repair").InnerText & ":[hr]:"
                            appendstring &= String.Format( _
                                    ":[br]:TIME STAMP: {0}", _
                                     Now.ToString _
                                )
                            n.SelectSingleNode("report/detail/repair").InnerText = appendstring
                            ' Append a timestamp to the repair detail
                            'n.SelectSingleNode("report/detail/repair").InnerText = _
                            '    Split(n.SelectSingleNode("detail").InnerText, ":[!--lastreport--]:")(1) & _
                            '    ":[hr]:" & _
                            '    n.SelectSingleNode("report/detail/repair").InnerText & _
                            '    String.Format( _
                            '        ":[br]:TIME STAMP: {0}", _
                            '         Now.ToString _
                            '    )

                            n.Attributes.Append(xmlForms.postAttribute)
                            thisForm.Save()

                            thisForm.Log(xmlForms.FormData.Document.SelectSingleNode(thisForm.thisxPath).OuterXml)

                            If IsNothing(xmlForms.FormData.Document.SelectSingleNode(thisForm.thisxPath).Attributes("post")) Then
                                Throw New Exception("Post flag was not set on the node.")
                            End If

                        End With
                        .Save()
                    End If
                    If xmlForms.StatusRule(NewValue, eStatusRule.beginreport) Then
                        With .FormData
                            Dim n As XmlNode = thisForm.FormData.SelectSingleNode(thisForm.thisxPath & "/report/times")
                            n.AppendChild(.CreateElement(Replace("begintravel", Chr(32), "_")))
                            n.LastChild.InnerText = funcDate.TimeToMin()

                            ' Update the call to changed so edits no longer overwrite
                            xmlForms.SetNodeChanged(thisForm.FormData.SelectSingleNode(thisForm.thisxPath))
                            'thisForm.FormData.SelectSingleNode(thisForm.thisxPath).Attributes.Append(xmlForms.changedAttribute)
                            thisForm.Log(xmlForms.FormData.Document.SelectSingleNode(thisForm.thisxPath).OuterXml)

                        End With
                        .Save()
                    End If
                    If xmlForms.StatusRule(NewValue, eStatusRule.beginwork) Then
                        With .FormData
                            Dim n As XmlNode = thisForm.FormData.SelectSingleNode(thisForm.thisxPath & "/report/times")
                            n.AppendChild(.CreateElement(Replace("beginwork", Chr(32), "_")))
                            n.LastChild.InnerText = funcDate.TimeToMin()

                            n.AppendChild(.CreateElement(Replace("traveltime", Chr(32), "_")))
                            n.LastChild.InnerText = funcDate.TimeToMin() - CInt(n.SelectSingleNode("begintravel").InnerText)

                        End With
                        .Save()
                    End If
                End With
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

#End Region

End Class
