Imports System.Xml
Imports CPCL
Imports PriorityMobile

Public Class ctrl_Payment
    Inherits iView

    Public Overrides Sub Bind()
        With Me
            Try
                .paymentterms.DataBindings.Add("Text", thisForm.TableData, "paymentterms")
                .overduepayment.DataBindings.Add("Text", thisForm.TableData, "overduepayment")
                .dueamount.DataBindings.Add("Text", thisForm.TableData, "dueamount")
                .todaysinvoicetotals.DataBindings.Add("Text", thisForm.TableData, "todaysinvoicetotal")
                .cash.DataBindings.Add("Text", thisForm.TableData, "cash")
                .cheque.DataBindings.Add("Text", thisForm.TableData, "cheque")
            Catch 
            End Try
        End With
    End Sub

#Region "Direct Activations"

    Public Overrides Sub DirectActivations(ByRef ToolBar As daToolbar)
        With Me
            If cash.Text.Length = 0 Then cash.Text = "0.00"
            If cheque.Text.Length = 0 Then cheque.Text = "0.00"
            ToolBar.Add(AddressOf hPrint, "print.BMP", (CDbl(.cash.Text) + CDbl(.cheque.Text)) > 0)
        End With
    End Sub


    Private Sub hPrint()
        With thisForm.Printer
            If Not .Connected Then
                .BeginConnect(thisForm.MACAddress)
            Else
                PrintForm()
            End If
        End With
    End Sub

    Public Overrides Sub PrintForm()
        With thisForm
            Dim rc As XmlNode = .FormData.SelectSingleNode(.boundxPath)


            Dim headerFont As New PrinterFont(50, 5, 2) 'variable width. 
            Dim largeFont As New PrinterFont(30, 0, 3)
            Dim smallFont As New PrinterFont(35, 0, 2)

            Using lblReceipt As New Label(thisForm.Printer, eLabelStyle.receipt)


                Dim van As String = rc.ParentNode.ParentNode.ParentNode.SelectSingleNode("vehiclereg").InnerText.ToUpper
                Dim rcDelivery As String = String.Format("{0}-{1}-{2}", rc.ParentNode.ParentNode.ParentNode.SelectSingleNode("curdate").InnerText, _
                                         rc.ParentNode.ParentNode.ParentNode.SelectSingleNode("routenumber").InnerText, _
                                         rc.ParentNode.SelectSingleNode("ordinal").InnerText)
                Dim rcDate As String = Now.ToString("dd/MM/yyyy")
                Dim rcTime As String = Now.ToString("HH:mm")


                Dim docHead As New ReceiptFormatter(64, _
                                                    New FormattedColumn(16, 0, eAlignment.Center), _
                                                    New FormattedColumn(16, 16, eAlignment.Center), _
                                                    New FormattedColumn(16, 32, eAlignment.Center), _
                                                    New FormattedColumn(16, 48, eAlignment.Center))



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
                                            New FormattedColumn(21, 0, eAlignment.Center), _
                                            New FormattedColumn(21, 21, eAlignment.Center), _
                                            New FormattedColumn(21, 42, eAlignment.Right))
                paymentDetails.AddRow("Cheque:", "", rcCheque.ToString("c").Replace("£", "#"))
                paymentDetails.AddRow("Cash:", "", rcCash.ToString("c").Replace("£", "#"))



                With lblReceipt
                    'logo
                    .AddImage("roddas.pcx", New Point(186, thisForm.Printer.Dimensions.Height + 10), 147)

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
                    .AddText(vat, New Point((thisForm.Printer.Dimensions.Width / 2 - (vat.Length / 2) * 16), _
                                               thisForm.Printer.Dimensions.Height + 10), largeFont)

                    .AddMultiLine("For any remittance queries please contact" & vbCrLf & "accounts@roddas.co.uk".PadLeft(32, " "), _
                                  New Point(thisForm.Printer.Dimensions.Width / 2 - 168, thisForm.Printer.Dimensions.Height + 10), smallFont, 30)

                    'line
                    .AddLine(New Point(10, thisForm.Printer.Dimensions.Height + 10), _
                             New Point(thisForm.Printer.Dimensions.Width - 10, thisForm.Printer.Dimensions.Height + 10), 10)

                    'please quote
                    .AddText("Please quote account number in all correspondence.", New Point(thisForm.Printer.Dimensions.Width / 2 - 200, _
                                                                                             thisForm.Printer.Dimensions.Height + 10), _
                             smallFont)

                    'tear 'n' print!
                    .AddTearArea(New Point(0, thisForm.Printer.Dimensions.Height))
                    thisForm.Printer.Print(.toByte)


                End With
            End Using
        End With
    End Sub

#End Region

    Private Sub payment_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cash.LostFocus, cheque.LostFocus
        If cash.Text.Length = 0 Then cash.Text = "0.00"
        If cheque.Text.Length = 0 Then cheque.Text = "0.00"
        thisForm.RefreshDirectActivations()
    End Sub

End Class
