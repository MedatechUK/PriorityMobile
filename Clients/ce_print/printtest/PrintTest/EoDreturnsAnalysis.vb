

Imports CPCL
Imports btZebra

Public Class EoDReturnsAnalysis

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

        Using lblReturnsAnalysis As New Label(prn, eLabelStyle.receipt)

            Dim rcptReStock As New ReceiptFormatter(64, _
                                                New FormattedColumn(10, 0, eAlignment.Left), _
                                                New FormattedColumn(22, 10, eAlignment.Left), _
                                                New FormattedColumn(6, 32, eAlignment.Left), _
                                                New FormattedColumn(26, 38, eAlignment.Left))

            rcptReStock.AddRow("Part:", "Part Desc:", "Qty:", "Customer Reason:")
            rcptReStock.AddRow("012232", "This is a part", "99", "Too many parts!")
            rcptReStock.AddRow("1c1233", "Here's another", "12", "Incorrect type of part")

            Dim rcptQuarantine As New ReceiptFormatter(64, _
                                                New FormattedColumn(10, 0, eAlignment.Left), _
                                                New FormattedColumn(22, 10, eAlignment.Left), _
                                                New FormattedColumn(6, 32, eAlignment.Left), _
                                                New FormattedColumn(26, 38, eAlignment.Left))

            rcptQuarantine.AddRow("Part:", "Part Desc:", "Qty:", "Customer Reason:")
            rcptQuarantine.AddRow("11232", "First part", "3", "Quarantine-able problem")
            rcptQuarantine.AddRow("91112", "Second part", "1", "Another similar problem")

            With lblReturnsAnalysis
                .CharSet(eCountry.UK)
                'logo
                .AddImage("roddas.pcx", New Point(186, prn.Dimensions.Height + 10), 147)

                'line
                .AddLine(New Point(10, prn.Dimensions.Height + 10), _
                         New Point(prn.Dimensions.Width - 10, prn.Dimensions.Height + 10), 10, 15)

                'header = 204.5px wide
                .AddText("RETURNS ANALYSIS", New Point((prn.Dimensions.Width / 2) - 205, prn.Dimensions.Height + 10), _
                         headerFont)

                .AddLine(New Point(10, prn.Dimensions.Height + 10), _
                         New Point(prn.Dimensions.Width - 10, prn.Dimensions.Height + 10), 10, 15)


                .AddText("Returned Goods for Re-Stock", New Point(20, prn.Dimensions.Height + 30), _
                         largeFont)

                .AddTearArea(New Point(0, prn.Dimensions.Height), 15)

                For Each strval In rcptReStock.FormattedText
                    .AddText(strval, New Point(22, prn.Dimensions.Height), smallFont)

                Next

                .AddLine(New Point(10, prn.Dimensions.Height + 10), _
                         New Point(prn.Dimensions.Width - 10, prn.Dimensions.Height + 10), 2, 15)

                .AddText("Returned Goods for Quarantine", New Point(20, prn.Dimensions.Height + 20), _
                             largeFont)

                .AddTearArea(New Point(0, prn.Dimensions.Height), 15)

                For Each strval In rcptQuarantine.FormattedText
                    .AddText(strval, New Point(22, prn.Dimensions.Height), smallFont)
                Next


                .AddLine(New Point(10, prn.Dimensions.Height + 10), _
                         New Point(prn.Dimensions.Width - 10, prn.Dimensions.Height + 10), 10, 15)


                'tear 'n' print!
                .AddTearArea(New Point(0, prn.Dimensions.Height))
                prn.Print(.toByte)

            End With
        End Using


    End Sub
end class