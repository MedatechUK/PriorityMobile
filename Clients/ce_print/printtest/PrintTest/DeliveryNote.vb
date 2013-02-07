Imports CPCL
Imports btZebra

Public Class DeliveryNote

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

        Using lblDeliveryNote As New Label(prn, eLabelStyle.receipt)

            'first receipt formatter
            'taking margins into account the receipt is 556px wide. At size 2, char width is 8px. 

            Dim docHead As New ReceiptFormatter(64, _
                                                New FormattedColumn(16, 0, eAlignment.Center), _
                                                New FormattedColumn(16, 16, eAlignment.Center), _
                                                New FormattedColumn(16, 32, eAlignment.Center), _
                                                New FormattedColumn(16, 64, eAlignment.Center))
            docHead.AddRow(" ", " ", " ", " ")
            dochead.AddRow("Number", "Date", "Time", "Van")
            dochead.AddRow("593151", "29/01/13", "11:51:22", "WK11 BHW")

            Dim custDetails As New ReceiptFormatter(64, _
                                                    New FormattedColumn(16, 0, eAlignment.Right), _
                                                    New FormattedColumn(48, 16, eAlignment.Left))
            custDetails.AddRow(" ", " ", " ")
            custDetails.AddRow("Customer:", "G00012")
            custDetails.AddRow("", "Goods returned Restock Van50")
            custDetails.AddRow("", "TR16 5BU")

            Dim partsList As New ReceiptFormatter(64, _
                                                  New FormattedColumn(3, 0, eAlignment.Right), _
                                                  New FormattedColumn(49, 4, eAlignment.Left), _
                                                  New FormattedColumn(8, 54, eAlignment.Left))
            partsList.AddRow(" ", " ", " ")
            partsList.AddRow("No", "Description:", "Code:")
            partsList.AddRow("6", "Green 2lt Semi-Skim Milk", "03324")


            With lblDeliveryNote
                'logo
                .AddImage("roddas.pcx", New Point(186, prn.Dimensions.Height + 10), 147)

                'line
                .AddLine(New Point(10, prn.Dimensions.Height + 10), _
                         New Point(prn.Dimensions.Width - 10, prn.Dimensions.Height + 10), 5)

                'header = 334px wide
                .AddText("DELIVERY NOTE", New Point((prn.Dimensions.Width / 2) - 167, prn.Dimensions.Height + 10), _
                         headerFont)

                'line
                .AddLine(New Point(10, prn.Dimensions.Height + 10), _
                         New Point(prn.Dimensions.Width - 10, prn.Dimensions.Height + 10), 5)

                'address
                .AddMultiLine("A.E. Rodda & Son Ltd." & vbCrLf & "The Creamery" & vbCrLf & "Scorrier" _
                                & vbCrLf & "Redruth" & vbCrLf & "Cornwall" & vbCrLf & "TR165BU", _
                                             New Point(10, prn.Dimensions.Height + 10), largeFont, 30)
                'line
                .AddLine(New Point(10, prn.Dimensions.Height + 10), _
                         New Point(prn.Dimensions.Width - 10, prn.Dimensions.Height + 10), 1)

                'document header 
                For Each StrVal In docHead.FormattedText
                    .AddText(StrVal, New Point(22, prn.Dimensions.Height), smallFont)
                Next

                'line
                .AddLine(New Point(10, prn.Dimensions.Height + 10), _
                         New Point(prn.Dimensions.Width - 10, prn.Dimensions.Height + 10), 1)

                'customer details 
                For Each StrVal In custDetails.FormattedText
                    .AddText(StrVal, New Point(22, prn.Dimensions.Height), smallFont)
                Next

                'line
                .AddLine(New Point(10, prn.Dimensions.Height + 10), _
                         New Point(prn.Dimensions.Width - 10, prn.Dimensions.Height + 10), 1)

                'itemised parts list. 
                For Each StrVal In partsList.FormattedText
                    .AddText(StrVal, New Point(22, prn.Dimensions.Height), smallFont)
                Next

                'vat waffle
                .AddText("Items subject to VAT are marked +", New Point(26, prn.Dimensions.Height + 10), largeFont)

                'line
                .AddLine(New Point(10, prn.Dimensions.Height + 10), _
                         New Point(prn.Dimensions.Width - 10, prn.Dimensions.Height + 10), 1)

                'itemisation
                Dim totals As String = " ( 1 lines 6 units ) " 'this will, of course, be calculated.
                .AddText(totals, New Point((prn.Dimensions.Width / 2 - (totals.Length / 2) * 16), _
                                           prn.Dimensions.Height + 10), largeFont)

                'line
                .AddLine(New Point(10, prn.Dimensions.Height + 10), _
                         New Point(prn.Dimensions.Width - 10, prn.Dimensions.Height + 10), 5)

                'vat number 
                Dim vat As String = "V.A.T. No.  131 7759 63"
                .AddText(vat, New Point((prn.Dimensions.Width / 2 - (vat.Length / 2) * 16), _
                                           prn.Dimensions.Height + 10), largeFont)

                .AddMultiLine("For any remittance queries please contact" & vbCrLf & "accounts@roddas.co.uk".PadLeft(32, " "), _
                              New Point(prn.Dimensions.Width / 2 - 168, prn.Dimensions.Height + 10), smallFont, 30)
                'line
                .AddLine(New Point(10, prn.Dimensions.Height + 10), _
                         New Point(prn.Dimensions.Width - 10, prn.Dimensions.Height + 10), 1)
                'header
                .AddText("Bank Details", New Point(prn.Dimensions.Width / 2 - 96, prn.Dimensions.Height + 10), largeFont)
                .AddMultiLine("HSBC" & vbCrLf & "Branch Location" & vbCrLf & "Account Number" & vbCrLf & "Sort Code", _
                              New Point(10, prn.Dimensions.Height + 10), smallFont, 30)
                'line
                .AddLine(New Point(10, prn.Dimensions.Height + 10), _
                         New Point(prn.Dimensions.Width - 10, prn.Dimensions.Height + 10), 5)
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