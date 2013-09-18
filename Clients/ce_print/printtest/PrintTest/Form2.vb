Imports CPCL
Imports btZebra

Public Class Form2

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
        Using qrLabel As New Label(prn, eLabelStyle.receipt)
            With qrLabel
                .AddBarcode("123 test", New Point((prn.Dimensions.Width / 2) - 223, prn.Dimensions.Height + 10), 150, Symbology.QRCODE)
                .AddTearArea(New Point(0, prn.Dimensions.Height))
                prn.Print(.toByte)
            End With
        End Using
    End Sub

End Class
