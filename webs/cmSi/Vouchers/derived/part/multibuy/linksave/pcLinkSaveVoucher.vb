Imports System.Xml

Public Class pcLinkSaveVoucher
    Inherits linkSaveVoucher

    Sub New(ByVal data As XmlNode)
        MyBase.New(data)
    End Sub

    Protected Overrides Sub Enact()
        MyBase.Enact()
    End Sub

    Protected Overrides Sub EnactEffects()
        Dim Line As Integer = 0
        Dim Remaining As Integer = Math.Min(MatchedBuyQty, MatchedGetQty)

        While Remaining > 0
            Dim Qty As Integer = MatchedParts(Line).QTY
            While Qty > 0 And Remaining > 0
                If CDbl(MatchedParts(Line).LINETOTAL) = 0 Then
                    Exit While
                End If
                MatchedParts(Line).Discount += _
                                    CDbl(MatchedParts(Line).PARTPRICE) / 100 _
                                    * Discount

                Qty -= 1
                Remaining -= 1
            End While
            Line += 1
        End While
    End Sub
End Class
