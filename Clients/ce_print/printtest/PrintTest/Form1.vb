Imports CPCL
Imports btZebra

Public Class Form1

    Private WithEvents prn As New btZebra.LabelPrinter( _
        New Point(300, 300), _
        New Size(576, 0), _
        "\my documents\prnimg\" _
    )

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click

        If Not prn.Connected Then
            prn.BeginConnect("00225831c92a",,TRUE)
        Else
            Print()
        End If

    End Sub

    Private Sub hConnectionEstablished() Handles prn.connectionEstablished

        Print()

    End Sub

    Private Sub Print()

        Dim largeFont As New PrinterFont(30, 0, 3)
        Dim smallFont As New PrinterFont(25, 0, 2)

        Dim rcpt As New ReceiptFormatter(60, _
            New FormattedColumn(50, 1, eAlignment.Left), _
            New FormattedColumn(9, 51, eAlignment.Right) _
        )
        With rcpt
            .AddRow("1 * Part One", "9.99")
            .AddRow("2 * Part the second", "19.99")
            .AddRow("3 * Third and finally", "199.99")
        End With

        Using lbl As New Label( _
            prn, _
            eLabelStyle.receipt _
        )
            With lbl
                .AddImage("roddas.pcx", New Point(186, prn.Dimensions.Height + 10), 150)
                .AddText("Hello Roddas!", New Point(30, prn.Dimensions.Height + 30), largeFont)
                For Each StrVal In rcpt.FormattedText
                    .AddText(StrVal, New Point(40, prn.Dimensions.Height), smallFont)
                Next
                .AddText("This is a test.", New Point(30, prn.Dimensions.Height + 20), largeFont)
                .AddBarcode("123456", New Point(30, prn.Dimensions.Height + 10), 25, Symbology.CODE39)
                .AddBarcode("AA==123&&BB==456", New Point(30, prn.Dimensions.Height), 150, Symbology.QRCODE)
                .AddBox(New Point(10, 0), New Point(prn.Dimensions.Width - 10, prn.Dimensions.Height + 20))
                .AddTearArea(New Point(0, prn.Dimensions.Height))
                prn.Print(.toByte)
            End With

        End Using
    End Sub

End Class

