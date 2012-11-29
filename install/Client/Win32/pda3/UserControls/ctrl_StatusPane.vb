Imports System.Xml

Public Class ctrl_StatusPane
    Inherits ViewControl.iView

#Region "Overrides base class"

    Public Overrides Sub Bind()
        TextBox1.DataBindings.Add("Text", thisForm.TableData, "callnumber")
        ComboBox1.DataBindings.Add("SelectedValue", thisForm.TableData, "callstatus")
    End Sub

    Public Overrides Sub CurrentChanged()
        ListBind(ComboBox1, "Status")
    End Sub

    Public Overrides Function ValidColumn(ByVal ColumnName As String, ByVal ProposedValue As String) As Boolean
        Dim dr() As DataRow = Nothing

        Select Case ColumnName.ToUpper
            Case "CALLSTATUS"
                If Not thisForm.LookUp.AllowedValues("Status").Contains(ProposedValue) Then Return False
                Select Case ProposedValue.ToUpper
                    Case "ON-SITE", "EN-ROUTE"
                        dr = thisForm.TableData.DataSource.Select("callnumber <> '" & Me.TextBox1.Text & "' and callstatus in ('On-Site','En-Route')")
                        If dr.Count > 0 Then
                            MsgBox( _
                                String.Format( _
                                    "You are already {0} for call {1}.", _
                                    dr(0).Item("callstatus"), _
                                    dr(0).Item("callnumber") _
                                ) _
                            )
                        End If
                        Return dr.Count = 0

                    Case "COMPLETE", "INCOMPLETE"
                        Dim ret As Boolean = True
                        Dim n As XmlNode = thisForm.FormData.SelectSingleNode(thisForm.thisxPath)
                        If n.SelectSingleNode("report/malfunction").InnerText.Length = 0 Then ret = False
                        If n.SelectSingleNode("report/resolution").InnerText.Length = 0 Then ret = False
                        If n.SelectSingleNode("report/repair").InnerText.Length = 0 Then ret = False
                        If n.SelectSingleNode("signature/image").InnerText.Length = 0 Then ret = False
                        If n.SelectSingleNode("signature/print").InnerText.Length = 0 Then ret = False
                        If Not ret Then
                            MsgBox( _
                                String.Format( _
                                    "Cannot {0} incomplete form.", _
                                    ProposedValue _
                                ) _
                            )
                        End If
                        Return ret

                    Case Else
                        Return MyBase.ValidColumn(ColumnName, ProposedValue)
                End Select
            Case Else
                Return MyBase.ValidColumn(ColumnName, ProposedValue)
        End Select

    End Function

    Public Overrides Function SubFormVisible(ByVal Name As String) As Boolean
        With Me
            If IsNothing(.thisForm.TableData.Current) Then Return False
            Select Case Name.ToUpper
                Case "REPAIR", "PARTS", "SIGN"
                    Select Case .thisForm.TableData.Current("callstatus").ToString.ToUpper
                        Case "ISSUED", "EN-ROUTE"
                            Return False
                        Case Else
                            Return True
                    End Select
                Case Else
                    Return True
            End Select
        End With
    End Function

    Public Overrides Sub RowUpdated(ByVal Column As String, ByVal NewValue As String)
        Select Case Column.ToUpper
            Case "CALLSTATUS"

        End Select
    End Sub

#End Region

#Region "Direct Activations"

    Public Overrides Sub DirectActivations(ByRef ToolBar As System.Windows.Forms.ToolStrip)
        ToolBar.Items.Add("", Image.FromFile("icons\PHONE.BMP"), AddressOf hPlaceCall)
    End Sub

    Private Sub hPlaceCall()
        With thisForm
            With .FormData
                Dim n As XmlNode = thisForm.FormData.SelectSingleNode(thisForm.thisxPath)
                If IsNothing(n.SelectSingleNode("times")) Then
                    n.AppendChild(.CreateElement("times"))
                End If
                n = n.SelectSingleNode("times")
                n.AppendChild(.CreateElement("CALLED"))
                n.LastChild.InnerText = DateDiff(DateInterval.Minute, #1/1/1988#, Now)
            End With
            .Save()
        End With
        MsgBox(String.Format("Calling {0}.", thisForm.TableData.Current("phone")))
    End Sub

#End Region

End Class
