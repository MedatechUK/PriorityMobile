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
                .todaysinvoicetotals.DataBindings.Add("Text", thisForm.TableData, "todaysinvoicetotals")
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

        Dim headerFont As New PrinterFont(50, 5, 2) 'variable width. 
        Dim largeFont As New PrinterFont(30, 0, 3)
        Dim smallFont As New PrinterFont(35, 0, 2)

        Using lblReceipt As New Label(thisForm.Printer, eLabelStyle.receipt)

            Dim custDetails As New ReceiptFormatter(64, _
                                        New FormattedColumn(13, 0, eAlignment.Right), _
                                        New FormattedColumn(48, 16, eAlignment.Left))
            custDetails.AddRow("Customer:", "C123456")
            custDetails.AddRow("", "Some customer details here")
            custDetails.AddRow("", "TR16 5BU")

            Dim paymentDetails As New ReceiptFormatter(63, _
                                        New FormattedColumn(21, 0, eAlignment.Center), _
                                        New FormattedColumn(21, 21, eAlignment.Center), _
                                        New FormattedColumn(21, 42, eAlignment.Right))
            paymentDetails.AddRow("04/02/2013 09:07", "Cheque", "#" & "99")
            paymentDetails.AddRow("02/01/2013 16:21", "Cash", "#" & "0.85")

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
                         New Point(thisForm.Printer.Dimensions.Width - 10, thisForm.Printer.Dimensions.Height + 10), 5)
                'header
                .AddText("Bank Details", New Point(thisForm.Printer.Dimensions.Width / 2 - 96, thisForm.Printer.Dimensions.Height + 10), largeFont)


                .AddMultiLine("HSBC" & vbCrLf & "Branch Location" & vbCrLf & "Account Number" & vbCrLf & "Sort Code", _
              New Point(10, thisForm.Printer.Dimensions.Height + 10), smallFont, 30)

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
    End Sub

#End Region

    Private Sub payment_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cash.LostFocus, cheque.LostFocus
        If cash.Text.Length = 0 Then cash.Text = "0.00"
        If cheque.Text.Length = 0 Then cheque.Text = "0.00"
        thisForm.RefreshDirectActivations()
    End Sub

End Class
