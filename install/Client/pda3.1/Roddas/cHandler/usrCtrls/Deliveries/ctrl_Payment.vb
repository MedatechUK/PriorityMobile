Imports System.Xml
Imports CPCL
Imports PriorityMobile

Public Class ctrl_Payment
    Inherits iView

    Private _WaitPrint As Boolean = False

    Private Loading As Boolean = True
    Private dbl_Overdue As Double = 0
    Private dbl_Due As Double = 0
    Private dbl_Credit As Double = 0
    Private dbl_Today As Double = 0
    Private dbl_UnAllocated As Double = 0

    Public Overrides Sub FormClosing()
        With thisForm
            If MsgBox("Save Payment?", MsgBoxStyle.OkCancel) = MsgBoxResult.Ok Then
                With .FormData.SelectSingleNode(thisForm.thisxPath)
                    .SelectSingleNode("cheque").InnerText = CDbl(cheque.Text).ToString
                    .SelectSingleNode("cash").InnerText = CDbl(cash.Text).ToString
                    If IsNumeric(Unallocated.Text) Then
                        .SelectSingleNode("unallocated").InnerText = CDbl(Unallocated.Text).ToString
                    End If
                    .SelectSingleNode("credit").InnerText = CDbl(Credit.Text).ToString
                    If radio_Cheque.Checked Then
                        .SelectSingleNode("bank").InnerText = Banks.SelectedItem
                        .SelectSingleNode("chequenum").InnerText = ChqNum.Text
                    End If
                    .SelectSingleNode("flags").InnerText = Me.Flags
                End With
                .Save()
                If MsgBox("Print receipt?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                    With thisForm.Printer
                        If Not .Connected Then
                            .BeginConnect(thisForm.MACAddress)
                            Do While .WaitConnect
                                Threading.Thread.Sleep(100)
                            Loop
                        End If
                        If .Connected Then PrintForm()
                    End With
                End If
            End If
        End With

        MyBase.FormClosing()

    End Sub

    Public Overrides Sub Bind()

        Loading = True

        With thisForm.FormData.SelectSingleNode(thisForm.thisxPath)
            dbl_Overdue = CDbl(.SelectSingleNode("overduepayment").InnerText)
            With Me.inc_Overdue
                .Enabled = CBool(dbl_Overdue > 0)
                .Checked = CBool(dbl_Overdue > 0)
            End With
            overduepayment.Text = dbl_Overdue.ToString("C")

            dbl_Due = CDbl(.SelectSingleNode("dueamount").InnerText)
            With Me.inc_Due
                .Enabled = CBool(dbl_Due > 0)
                .Checked = CBool(dbl_Due > 0)
            End With
            dueamount.Text = dbl_Due.ToString("C")

            dbl_Today = DeliveryValue(thisForm, thisForm.FormData.SelectSingleNode(thisForm.Parent.thisxPath)) + PostalValue(thisForm, thisForm.FormData.SelectSingleNode(thisForm.Parent.thisxPath))
            With Me.inc_Today
                .Enabled = CBool(dbl_Today > 0)
                .Checked = CBool(dbl_Today > 0)
            End With
            todaysinvoicetotals.Text = dbl_Today.ToString("C")

            dbl_Credit = CreditValue(thisForm, thisForm.FormData.SelectSingleNode(thisForm.Parent.thisxPath))
            With Me.inc_Credit
                .Enabled = CBool(dbl_Credit <> 0)
                .Checked = False
            End With
            Credit.Text = dbl_Credit.ToString("C")

            With inc_Unallocated
                .Enabled = True
                .Checked = False
            End With

        End With

        Me.ChqNum.Text = ""

        With Me.Banks.Items
            .Clear()
            For Each bank As XmlNode In thisForm.FormData.SelectNodes("pdadata/bankcodes/bankcode")
                .Add(bank.SelectSingleNode("bankname").InnerText)
            Next
        End With

        If CDbl(thisForm.FormData.SelectSingleNode(thisForm.thisxPath).SelectSingleNode("cash").InnerText) <> 0 Then
            radio_Cash.Checked = True
            radio_Cheque.Checked = False
            Unallocated.Text = CDbl(thisForm.FormData.SelectSingleNode(thisForm.thisxPath).SelectSingleNode("unallocated").InnerText).ToString
            SetFlags(thisForm.FormData.SelectSingleNode(thisForm.thisxPath).SelectSingleNode("flags").InnerText)
        ElseIf CDbl(thisForm.FormData.SelectSingleNode(thisForm.thisxPath).SelectSingleNode("cheque").InnerText) Then
            Banks.SelectedItem = thisForm.FormData.SelectSingleNode(thisForm.thisxPath).SelectSingleNode("bank").InnerText
            ChqNum.Text = thisForm.FormData.SelectSingleNode(thisForm.thisxPath).SelectSingleNode("chequenum").InnerText
            radio_Cash.Checked = False
            radio_Cheque.Checked = True
            Unallocated.Text = CDbl(thisForm.FormData.SelectSingleNode(thisForm.thisxPath).SelectSingleNode("unallocated").InnerText).ToString
            SetFlags(thisForm.FormData.SelectSingleNode(thisForm.thisxPath).SelectSingleNode("flags").InnerText)
        Else
            radio_Cash.Checked = False
            radio_Cheque.Checked = True
        End If

        With Me
            Try
                .paymentterms.DataBindings.Clear()
                .cheque.Text = CDbl(thisForm.FormData.SelectSingleNode(thisForm.thisxPath).SelectSingleNode("cheque").InnerText).ToString("C")
                .cash.Text = CDbl(thisForm.FormData.SelectSingleNode(thisForm.thisxPath).SelectSingleNode("cash").InnerText).ToString("C")
                .Unallocated.Text = CDbl(thisForm.FormData.SelectSingleNode(thisForm.thisxPath).SelectSingleNode("unallocated").InnerText).ToString("C")
            Catch
            Finally
                .paymentterms.DataBindings.Add("Text", thisForm.TableData, "paymentterms")
            End Try
        End With

        Loading = False
        h_CheckStateChanged(Me, New System.EventArgs)

    End Sub

#Region "Direct Activations"

    'Public Overrides Sub DirectActivations(ByRef ToolBar As daToolbar)
    '    With Me
    '        If cash.Text.Length = 0 Then cash.Text = "0.00"
    '        If cheque.Text.Length = 0 Then cheque.Text = "0.00"
    '        ToolBar.Add(AddressOf hPrint, "print.BMP", (CDbl(.cash.Text) + CDbl(.cheque.Text)) > 0)
    '    End With
    'End Sub

    Public Overrides Sub PrintForm()
        With thisForm
            Dim rc As XmlNode = .FormData.SelectSingleNode(.boundxPath)


            Dim headerFont As New PrinterFont(50, 5, 2) 'variable width. 
            Dim largeFont As New PrinterFont(30, 0, 4)
            Dim smallFont As New PrinterFont(35, 0, 3)

            Using lblReceipt As New Label(thisForm.Printer, eLabelStyle.receipt)


                Dim van As String = rc.ParentNode.ParentNode.ParentNode.SelectSingleNode("vehiclereg").InnerText.ToUpper
                Dim rcDelivery As String = String.Format("{0}-{1}-{2}", rc.ParentNode.ParentNode.ParentNode.SelectSingleNode("curdate").InnerText, _
                                         rc.ParentNode.ParentNode.ParentNode.SelectSingleNode("routenumber").InnerText, _
                                         rc.ParentNode.SelectSingleNode("ordinal").InnerText)
                Dim rcDate As String = Now.ToString("dd/MM/yyyy")
                Dim rcTime As String = Now.ToString("HH:mm")


                Dim docHead As New ReceiptFormatter(80, _
                                                    New FormattedColumn(16, 0, eAlignment.Center), _
                                                    New FormattedColumn(10, 18, eAlignment.Center), _
                                                    New FormattedColumn(7, 30, eAlignment.Center), _
                                                    New FormattedColumn(10, 39, eAlignment.Center))



                docHead.AddRow("Number", "Date", "Time", "Van")
                docHead.AddRow(rcDelivery, rcDate, rcTime, van)



                '#### cust header#### 
                Dim custNumber As String = rc.ParentNode.SelectSingleNode("customer/custnumber").InnerText.ToUpper
                Dim custName As String = rc.ParentNode.SelectSingleNode("customer/custname").InnerText
                Dim custPostCode As String = rc.ParentNode.SelectSingleNode("customer/postcode").InnerText.ToUpper

                Dim custDetails As New ReceiptFormatter(64, _
                                            New FormattedColumn(13, 0, eAlignment.Right), _
                                            New FormattedColumn(48, 16, eAlignment.Left))

                custDetails.AddRow("Customer:", custNumber)
                custDetails.AddRow("", custName)
                custDetails.AddRow("", custPostCode)

                '#### payment details#### 


                Dim rcCash As Double = CDbl(rc.SelectSingleNode("cash").InnerText)
                Dim rcCheque As Double = CDbl(rc.SelectSingleNode("cheque").InnerText)



                Dim paymentDetails As New ReceiptFormatter(63, _
                                            New FormattedColumn(16, 0, eAlignment.Center), _
                                            New FormattedColumn(3, 21, eAlignment.Center), _
                                            New FormattedColumn(21, 25, eAlignment.Right))

                paymentDetails.AddRow("Cheque:", "", rcCheque.ToString("c").Replace("£", "#"))

                paymentDetails.AddRow("Cash:", "", rcCash.ToString("c").Replace("£", "#"))



                With lblReceipt
                    .CharSet(eCountry.UK)
                    'logo
                    .AddImage("roddas.pcx", New Point(10, thisForm.Printer.Dimensions.Height + 10), 147)

                    'line
                    .AddLine(New Point(10, thisForm.Printer.Dimensions.Height + 10), _
                             New Point(thisForm.Printer.Dimensions.Width - 10, thisForm.Printer.Dimensions.Height + 10), 10, 15)

                    'header = 334px wide
                    .AddText("RECEIPT", New Point((thisForm.Printer.Dimensions.Width / 2) - 86, thisForm.Printer.Dimensions.Height + 10), _
                             headerFont)

                    'line
                    .AddLine(New Point(10, thisForm.Printer.Dimensions.Height + 10), _
                             New Point(thisForm.Printer.Dimensions.Width - 10, thisForm.Printer.Dimensions.Height + 10), 10)

                    'address
                    .AddMultiLine("A.E. Rodda & Son Ltd." & vbCrLf & "The Creamery" & vbCrLf & "Scorrier" _
                                    & vbCrLf & "Redruth" & vbCrLf & "Cornwall" & vbCrLf & "TR165BU", _
                                                 New Point(10, thisForm.Printer.Dimensions.Height + 10), largeFont, 30)
                    'line
                    .AddLine(New Point(10, thisForm.Printer.Dimensions.Height + 10), _
                             New Point(thisForm.Printer.Dimensions.Width - 10, thisForm.Printer.Dimensions.Height + 10), 2)
                    'doc header
                    For Each StrVal In docHead.FormattedText
                        .AddText(StrVal, New Point(22, thisForm.Printer.Dimensions.Height), smallFont)
                    Next
                    'line
                    .AddLine(New Point(10, thisForm.Printer.Dimensions.Height + 10), _
                             New Point(thisForm.Printer.Dimensions.Width - 10, thisForm.Printer.Dimensions.Height + 10), 2)

                    'customer details 
                    For Each StrVal In custDetails.FormattedText
                        .AddText(StrVal, New Point(22, thisForm.Printer.Dimensions.Height), smallFont)
                    Next

                    'line
                    .AddLine(New Point(10, thisForm.Printer.Dimensions.Height + 10), _
                             New Point(thisForm.Printer.Dimensions.Width - 10, thisForm.Printer.Dimensions.Height + 10), 2)

                    For Each StrVal In paymentDetails.FormattedText
                        .AddText(StrVal, New Point(44, thisForm.Printer.Dimensions.Height), smallFont)
                    Next

                    'line
                    .AddLine(New Point(10, thisForm.Printer.Dimensions.Height + 10), _
                             New Point(thisForm.Printer.Dimensions.Width - 10, thisForm.Printer.Dimensions.Height + 10), 5)

                    'vat number 
                    Dim vat As String = "V.A.T. No.  131 7759 63"
                    .AddText(vat, New Point((thisForm.Printer.Dimensions.Width + 20), _
                                               thisForm.Printer.Dimensions.Height + 10), largeFont)

                    .AddMultiLine("For any remittance queries please contact" & vbCrLf & "accounts@roddas.co.uk".PadLeft(32, " "), _
                                  New Point(thisForm.Printer.Dimensions.Width + 50, thisForm.Printer.Dimensions.Height + 10), smallFont, 30)

                    'line
                    .AddLine(New Point(10, thisForm.Printer.Dimensions.Height + 10), _
                             New Point(thisForm.Printer.Dimensions.Width - 10, thisForm.Printer.Dimensions.Height + 10), 10)

                    'please quote
                    .AddText("Please quote account number in all correspondence.", New Point(thisForm.Printer.Dimensions.Width + 10, _
                                                                                             thisForm.Printer.Dimensions.Height + 10), _
                             smallFont)

                    'tear 'n' print!
                    '.AddTearArea(New Point(0, thisForm.Printer.Dimensions.Height))
                    thisForm.Printer.Print(.toByte)


                End With
            End Using
        End With
    End Sub

#End Region

    Private Sub Unallocated_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Unallocated.LostFocus
        If Unallocated.Text.Length = 0 Or Not (IsNumeric(Unallocated.Text)) Then
            Unallocated.Text = CDbl("0.00").ToString("C")
        Else
            Unallocated.Text = CDbl(Unallocated.Text).ToString("C")
        End If
        h_CheckStateChanged(Me, New System.EventArgs)
    End Sub

    Private Sub ctrl_Payment_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Resize
        With Me
            .overduepayment.Width = .inc_Overdue.Left - .lbl_Overdue.Width
            .dueamount.Width = .inc_Due.Left - .lbl_Due.Width
            .todaysinvoicetotals.Width = .inc_Today.Left - .lbl_Today.Width
            .Credit.Width = .inc_Credit.Left - .lbl_Credit.Width
            .Unallocated.Width = .inc_Unallocated.Left - .lbl_Unallocated.Width

            .cheque.Width = .radio_Cheque.Left - .lbl_Check.Width
            .cash.Width = .radio_Cash.Left - .lbl_Cash.Width

            .ChqNum.Width = pnl_ChqNum.Width - (Banks.Left + Banks.Width)

        End With
    End Sub

    Private Sub h_CheckStateChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles inc_Overdue.CheckStateChanged, inc_Due.CheckStateChanged, inc_Credit.CheckStateChanged, inc_Today.CheckStateChanged, inc_Unallocated.CheckStateChanged
        If Not Loading Then
            With thisForm
                Dim zero As Double = 0
                Select Case radio_Cash.Checked
                    Case True
                        cash.Text = Total.ToString("C")
                        cheque.Text = zero.ToString("C")
                    Case Else
                        cash.Text = zero.ToString("C")
                        cheque.Text = Total.ToString("C")
                End Select

            End With
            thisForm.RefreshDirectActivations()
        End If
    End Sub

    Private ReadOnly Property Total() As Double
        Get
            Dim ret As Double = 0
            If inc_Overdue.Checked Then ret += CDbl(overduepayment.Text)
            If inc_Due.Checked Then ret += CDbl(dueamount.Text)
            If inc_Today.Checked Then ret += CDbl(todaysinvoicetotals.Text)
            If inc_Credit.Checked Then ret += CDbl(Credit.Text)
            If inc_Unallocated.Checked Then
                If IsNumeric(Unallocated.Text) Then
                    ret += CDbl(Unallocated.Text)
                End If
            End If
            Return ret
        End Get
    End Property

#Region "Flags"

    Private Sub SetFlags(ByVal flags As String)

        inc_Overdue.Checked = False
        inc_Due.Checked = False
        inc_Today.Checked = False
        inc_Credit.Checked = False
        inc_Unallocated.Checked = False

        For i As Integer = 0 To flags.Length - 1
            Select Case flags.Substring(i, 1).ToUpper
                Case "O"
                    inc_Overdue.Checked = True
                Case "D"
                    inc_Due.Checked = True
                Case "T"
                    inc_Today.Checked = True
                Case "C"
                    inc_Credit.Checked = True
                Case "U"
                    inc_Unallocated.Checked = True
            End Select
        Next
    End Sub

    Private ReadOnly Property Flags() As String
        Get
            Dim ret As String = ""
            If inc_Overdue.Checked Then ret += "O"
            If inc_Due.Checked Then ret += "D"
            If inc_Today.Checked Then ret += "T"
            If inc_Credit.Checked Then ret += "C"
            If inc_Unallocated.Checked Then ret += "U"
            Return ret
        End Get
    End Property

#End Region

    Private Sub h_CHEQUE_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles radio_Cheque.CheckedChanged
        Select Case radio_Cheque.Checked
            Case True
                Banks.Enabled = True
                ChqNum.Enabled = True
                radio_Cash.Checked = False
            Case Else
                Banks.Enabled = False
                ChqNum.Enabled = False
        End Select
        h_CheckStateChanged(sender, e)
    End Sub

    Private Sub h_CASH_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles radio_Cash.CheckedChanged
        Select Case radio_Cash.Checked
            Case True
                Banks.Enabled = False
                ChqNum.Enabled = False
                radio_Cheque.Checked = False
            Case Else
                Banks.Enabled = True
                ChqNum.Enabled = True
        End Select
        h_CheckStateChanged(sender, e)
    End Sub

End Class
