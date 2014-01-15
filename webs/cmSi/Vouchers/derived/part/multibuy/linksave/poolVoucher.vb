Imports System.Xml
Imports System.Linq

Public Class poolVoucher
    Inherits multiBuyVoucher

    Sub New(ByVal data As XmlNode)
        MyBase.New(data)
    End Sub

    Protected Overrides Sub Enact()

        Dim matchedParts As IOrderedEnumerable(Of CartItem) = _
                                    From ci In ts.cart.CartItems.Values _
                                    Where (BuyPart.Contains(ci.PARTNAME)) _
                                    AndAlso ci.PARTPRICE > Discount _
                                    Order By CDbl(ci.PARTPRICE) Ascending

        Dim matchedQty As Integer = Aggregate ci In matchedParts _
                                    Into Sum(CInt(ci.QTY))

        Dim Line As Integer = 0
        Dim Remaining As Integer = matchedQty \ (BuyQty + GetQty)

        While Remaining > 0
            Dim Qty As Integer = matchedParts(Line).QTY
            While Qty > 0 And Remaining > 0
                If CDbl(matchedParts(Line).LINETOTAL) = 0 Then
                    Exit While
                End If
                matchedParts(Line).Discount += Discount
                Qty -= 1
                Remaining -= 1
            End While
            Line += 1
        End While


    End Sub
End Class
