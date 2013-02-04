Imports CPCL
Imports btZebra


Public Class Receipt
    Private WithEvents prn As New btZebra.LabelPrinter( _
        New Point(300, 300), _
        New Size(576, 0), _
        "\prnimg\" _
    )

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim macaddress As String = "00225831c92a"

        If Not prn.Connected Then
            prn.BeginConnect(macaddress, , True)
        Else
            Print()
        End If

    End Sub

    Private Sub hConnectionEstablished() Handles prn.connectionEstablished
        Print()
    End Sub

    Private Sub Print()
        Dim headerFont As New PrinterFont(40, 5, 2) 'variable width. 
        Dim largeFont As New PrinterFont(30, 0, 3)
        Dim smallFont As New PrinterFont(25, 0, 2)

        Using lblReceipt As New Label(prn, eLabelStyle.receipt)

            Dim custDetails As New ReceiptFormatter(64, _
                                        New FormattedColumn(16, 0, eAlignment.Right), _
                                        New FormattedColumn(48, 16, eAlignment.Left))
            custDetails.AddRow("Customer:", "C123456")
            custDetails.AddRow("", "Some customer details here")
            custDetails.AddRow("", "TR16 5BU")

            Dim paymentDetails As New ReceiptFormatter(66, _
                                        New FormattedColumn(22, 0, eAlignment.Center), _
                                        New FormattedColumn(22, 0, eAlignment.Center), _
                                        New FormattedColumn(22, 0, eAlignment.Right))

            paymentDetails.AddRow("04/02/2013 09:07", "Cheque", "£99")
            paymentDetails.AddRow("02/01/2013 16:21", "Cash", "£0.85")

            With lblReceipt
                'logo
                .AddImage("roddas.pcx", New Point(186, prn.Dimensions.Height + 10), 147)

                'line
                .AddLine(New Point(10, prn.Dimensions.Height + 10), _
                         New Point(prn.Dimensions.Width - 10, prn.Dimensions.Height + 10), 4)

                'header = 334px wide
                .AddText("RECEIPT", New Point((prn.Dimensions.Width / 2) - 86, prn.Dimensions.Height + 10), _
                         headerFont)

                'line
                .AddLine(New Point(10, prn.Dimensions.Height + 10), _
                         New Point(prn.Dimensions.Width - 10, prn.Dimensions.Height + 10), 2)

                'address
                .AddMultiLine("A.E. Rodda & Son Ltd." & vbCrLf & "The Creamery" & vbCrLf & "Scorrier" _
                                & vbCrLf & "Redruth" & vbCrLf & "Cornwall" & vbCrLf & "TR165BU", _
                                             New Point(10, prn.Dimensions.Height + 10), largeFont, 30)
                'line
                .AddLine(New Point(10, prn.Dimensions.Height + 10), _
                         New Point(prn.Dimensions.Width - 10, prn.Dimensions.Height + 10), 2)

                'customer details 
                For Each StrVal In custDetails.FormattedText
                    .AddText(StrVal, New Point(22, prn.Dimensions.Height), smallFont)
                Next

                'line
                .AddLine(New Point(10, prn.Dimensions.Height + 10), _
                         New Point(prn.Dimensions.Width - 10, prn.Dimensions.Height + 10), 2)

                For Each StrVal In paymentDetails.FormattedText
                    .AddText(StrVal, New Point(44, prn.Dimensions.Height), smallFont)
                Next

                'line
                .AddLine(New Point(10, prn.Dimensions.Height + 10), _
                         New Point(prn.Dimensions.Width - 10, prn.Dimensions.Height + 10), 4)

                'vat number 
                Dim vat As String = "V.A.T. No.  131 7759 63"
                .AddText(vat, New Point((prn.Dimensions.Width / 2 - (vat.Length / 2) * 16), _
                                           prn.Dimensions.Height + 10), largeFont)

                .AddMultiLine("For any remittance queries please contact" & vbCrLf & "accounts@roddas.co.uk".PadLeft(32, " "), _
                              New Point(prn.Dimensions.Width / 2 - 168, prn.Dimensions.Height + 10), smallFont, 30)
                'line
                .AddLine(New Point(10, prn.Dimensions.Height + 10), _
                         New Point(prn.Dimensions.Width - 10, prn.Dimensions.Height + 10), 2)
                'header
                .AddText("Bank Details", New Point(prn.Dimensions.Width / 2 - 96, prn.Dimensions.Height + 10), largeFont)
                .AddMultiLine("HSBC" & vbCrLf & "Branch Location" & vbCrLf & "Account Number" & vbCrLf & "Sort Code", _
                              New Point(10, prn.Dimensions.Height + 10), smallFont, 30)
                'line
                .AddLine(New Point(10, prn.Dimensions.Height + 10), _
                         New Point(prn.Dimensions.Width - 10, prn.Dimensions.Height + 10), 4)
                'please quote
                .AddText("Please quote account number in all correspondence.", New Point(prn.Dimensions.Width / 2 - 200, _
                                                                                         prn.Dimensions.Height + 10), _
                         smallFont)

                'tear 'n' print!
                .AddTearArea(New Point(0, prn.Dimensions.Height))
                prn.Print(.toByte)


            End With
        End Using
    End Sub

End Class