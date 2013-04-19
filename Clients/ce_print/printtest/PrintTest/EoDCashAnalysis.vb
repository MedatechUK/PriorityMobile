

Imports CPCL
Imports btZebra

Public Class EoDCashAnalysis

    Private WithEvents prn As New btZebra.LabelPrinter( _
        New Point(300, 300), _
        New Size(576, 0), _
        "My Documents\prnimg\" _
    )

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim macaddress As String = "0022583cdd7e"

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
        Dim headerFont As New PrinterFont(50, 5, 2) 'variable width. 
        Dim largeFont As New PrinterFont(30, 0, 3) '16 
        Dim smallFont As New PrinterFont(35, 0, 2) '8 

        Using lblCashAnalysis As New Label(prn, eLabelStyle.receipt)

            Dim rcptHead As New ReceiptFormatter(64, _
                                                New FormattedColumn(32, 0, eAlignment.Left), _
                                                New FormattedColumn(32, 32, eAlignment.Left))
            rcptHead.AddRow("", "")
            rcptHead.AddRow("Date:", "05/04/2013")
            rcptHead.AddRow("Route Number:", "12")

            Dim rcptPayments As New ReceiptFormatter(64, _
                                                 New FormattedColumn(16, 0, eAlignment.Center), _
                                                 New FormattedColumn(16, 16, eAlignment.Center), _
                                                 New FormattedColumn(16, 32, eAlignment.Center), _
                                                 New FormattedColumn(16, 48, eAlignment.Center))

            rcptPayments.AddRow("Cust Code:", "Cust Name:", "Cash Amount:", "Cheque Amount:")
            'iterate through payments - nifty xpath needed.
            rcptPayments.AddRow("ABC123", "ABC & Company", "#23.18", "#0.48")
            rcptPayments.AddRow("DEF456", "DEF Ltd.", "#99.00", "#0.00")


            Dim rcptTotals As New ReceiptFormatter(64, _
                                               New FormattedColumn(22, 0, eAlignment.Right), _
                                               New FormattedColumn(21, 22, eAlignment.Right), _
                                               New FormattedColumn(21, 43, eAlignment.Right))
            rcptTotals.AddRow("", "Total Cash Amount:", "Total Cheque Amount:")
            rcptTotals.AddRow("", "#122.18", "#0.48")
            'calculated from the above data I suppose 
            rcptTotals.AddRow("", "", "Total Payments:")
            rcptTotals.AddRow("", "", "#122.66")

            Dim rcptCashAnalysis As New ReceiptFormatter(64, _
                                                     New FormattedColumn(32, 0, eAlignment.Left), _
                                                     New FormattedColumn(16, 32, eAlignment.Left), _
                                                     New FormattedColumn(16, 48, eAlignment.Left))
            'lots of rows - this is a written box. 

            rcptCashAnalysis.AddRow("", "Pounds:", "Pence:")
            rcptCashAnalysis.AddRow("50 Pound Notes", "", "")
            rcptCashAnalysis.AddRow("20 Pound Notes", "", "")
            rcptCashAnalysis.AddRow("10 Pound Notes", "", "")
            rcptCashAnalysis.AddRow("5 Pound Notes", "", "")
            rcptCashAnalysis.AddRow("2 Pound Coins", "", "")
            rcptCashAnalysis.AddRow("1 Pound Coins", "", "")
            rcptCashAnalysis.AddRow("50 Pence Coins", "", "")
            rcptCashAnalysis.AddRow("20 Pence Coins", "", "")
            rcptCashAnalysis.AddRow("Silver", "", "")
            rcptCashAnalysis.AddRow("Bronze", "", "")
            rcptCashAnalysis.AddRow("", "", "")
            rcptCashAnalysis.AddRow("", "Total Cash:", "#122.18") 'is this a calculated column? 
            rcptCashAnalysis.AddRow("", "", "")
            rcptCashAnalysis.AddRow("Cheque:", "", "")
            rcptCashAnalysis.AddRow("Other:", "", "")
            'are these calculated columns? 
            rcptCashAnalysis.AddRow("", "Total Cheques:", "#0.48")
            rcptCashAnalysis.AddRow("", "Grand Total:", "#122.66")



            With lblCashAnalysis
                .CharSet(eCountry.UK)
                'logo
                .AddImage("roddas.pcx", New Point(186, prn.Dimensions.Height + 10), 147)

                'line
                .AddLine(New Point(10, prn.Dimensions.Height + 10), _
                         New Point(prn.Dimensions.Width - 10, prn.Dimensions.Height + 10), 10, 15)

                'header = 222.5px wide
                .AddText("PAYMENTS RECEIVED", New Point((prn.Dimensions.Width / 2) - 223, prn.Dimensions.Height + 10), _
                         headerFont)

                .AddLine(New Point(10, prn.Dimensions.Height + 10), _
                         New Point(prn.Dimensions.Width - 10, prn.Dimensions.Height + 10), 10, 15)
                'date & routenumber

                For Each StrVal In rcptHead.FormattedText
                    .AddText(StrVal, New Point(22, prn.Dimensions.Height), smallFont)
                Next

                'line
                .AddLine(New Point(10, prn.Dimensions.Height + 10), _
                         New Point(prn.Dimensions.Width - 10, prn.Dimensions.Height + 10), 2)

                'payments rcpt
                For Each StrVal In rcptPayments.FormattedText
                    .AddText(StrVal, New Point(22, prn.Dimensions.Height), smallFont)
                Next

                'totals
                .AddLine(New Point(10, prn.Dimensions.Height + 10), _
                                         New Point(prn.Dimensions.Width - 10, prn.Dimensions.Height + 10), 1)


                For i As Integer = 0 To 1
                    .AddText(rcptTotals.FormattedText(i), New Point(22, prn.Dimensions.Height), smallFont)
                Next
                .AddLine(New Point(225, prn.Dimensions.Height + 10), _
                                        New Point(prn.Dimensions.Width - 10, prn.Dimensions.Height + 10), 1)

                For i As Integer = 2 To 3
                    .AddText(rcptTotals.FormattedText(i), New Point(22, prn.Dimensions.Height), smallFont)
                Next

                .AddLine(New Point(420, prn.Dimensions.Height + 10), _
                                        New Point(prn.Dimensions.Width - 10, prn.Dimensions.Height + 10), 1)
                
                'line

                .AddLine(New Point(10, prn.Dimensions.Height + 10), _
                         New Point(prn.Dimensions.Width - 10, prn.Dimensions.Height + 10), 10)

                .AddText("CASH ANALYSIS", New Point((prn.Dimensions.Width / 2) - 166, prn.Dimensions.Height + 10), _
                         headerFont)

                .AddLine(New Point(10, prn.Dimensions.Height + 10), _
                         New Point(prn.Dimensions.Width - 10, prn.Dimensions.Height + 10), 10)
                'cash

                .AddText(rcptCashAnalysis.FormattedText(0), New Point(22, prn.Dimensions.Height), smallFont)

                .AddLine(New Point(10, prn.Dimensions.Height + 10), _
                         New Point(prn.Dimensions.Width - 10, prn.Dimensions.Height + 10), 1)

                For i As Integer = 1 To 10
                    .AddText(rcptCashAnalysis.FormattedText(i), New Point(22, prn.Dimensions.Height), smallFont)
                Next

                .AddLine(New Point(10, prn.Dimensions.Height + 10), _
                         New Point(prn.Dimensions.Width - 10, prn.Dimensions.Height + 10), 1)

                .AddText(rcptCashAnalysis.FormattedText(12), New Point(22, prn.Dimensions.Height), smallFont)

                .AddLine(New Point(280, prn.Dimensions.Height + 10), _
                         New Point(prn.Dimensions.Width - 10, prn.Dimensions.Height + 10), 1)

                For i As Integer = 14 To 15
                    .AddText(rcptCashAnalysis.FormattedText(i), New Point(22, prn.Dimensions.Height), smallFont)
                Next

                .AddLine(New Point(10, prn.Dimensions.Height + 10), _
                         New Point(prn.Dimensions.Width - 10, prn.Dimensions.Height + 10), 1)


                .AddText(rcptCashAnalysis.FormattedText(16), New Point(22, prn.Dimensions.Height), smallFont)


                .AddLine(New Point(280, prn.Dimensions.Height + 10), _
                        New Point(prn.Dimensions.Width - 10, prn.Dimensions.Height + 10), 1)

                .AddText(rcptCashAnalysis.FormattedText(17), New Point(22, prn.Dimensions.Height), smallFont)

                .AddLine(New Point(280, prn.Dimensions.Height + 10), _
                        New Point(prn.Dimensions.Width - 10, prn.Dimensions.Height + 10), 1)

                .AddLine(New Point(10, prn.Dimensions.Height + 10), _
                         New Point(prn.Dimensions.Width - 10, prn.Dimensions.Height + 10), 10)

                'tear 'n' print!
                .AddTearArea(New Point(0, prn.Dimensions.Height))
                prn.Print(.toByte)


            End With





            'Line
            'RcptFormatter 4x~ 16-16-16-16
            'RcptFormatter 3x3 pad-1/3-1/3
            'Line 
            'Cash Analysis
            'line
            'RcptFormatter 3x11 - borders? 
            'gap
            'RcptFormatter 3x1 pad-15-10
            'gap
            'RcptFormatter 3x2 21-21-21
            'gap
            'RcptFormatter 3x3 2/3-1/6-1/6

        End Using

    End Sub

End Class