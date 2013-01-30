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
        Dim headerFont As New PrinterFont(40, 5, 1) 'variable width. 
        Dim fieldFont As New PrinterFont(30, 0, 3)
        Dim smallFont As New PrinterFont(25, 0, 3)

        Using lblDeliveryNote As New Label(prn, eLabelStyle.receipt)

            'first receipt formatter


            With lblDeliveryNote
                'logo
                .AddImage("roddas.pcx", New Point(186, prn.Dimensions.Height + 10), 147)
                'line
                .AddLine(New Point(10, prn.Dimensions.Height + 10), _
                         New Point(prn.Dimensions.Width - 10, prn.Dimensions.Height + 10), 4)
                'header = 334px wide
                .AddText("DELIVERY NOTE", New Point((prn.Dimensions.Width / 2) - 167, prn.Dimensions.Height + 10), headerFont)
                'line
                .AddLine(New Point(10, prn.Dimensions.Height + 10), _
                         New Point(prn.Dimensions.Width - 10, prn.Dimensions.Height + 10), 2)
                'address
                .AddMultiLine("A.E. Rodda & Son Ltd." & vbCrLf & "The Creamery" & vbCrLf & "Scorrier" _
                                & vbCrLf & "Redruth" & vbCrLf & "Cornwall" & vbCrLf & "TR165BU", _
                                             New Point(10, prn.Dimensions.Height + 10), fieldFont, 30)
                'line
                .AddLine(New Point(10, prn.Dimensions.Height + 10), _
                         New Point(prn.Dimensions.Width - 10, prn.Dimensions.Height + 10), 2)
                'tear 'n' print!
                .AddTearArea(New Point(0, prn.Dimensions.Height))
                prn.Print(.toByte)

            End With
        End Using
    End Sub

End Class