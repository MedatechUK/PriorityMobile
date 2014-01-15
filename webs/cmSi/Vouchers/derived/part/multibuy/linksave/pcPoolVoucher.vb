Imports System.Xml
Imports System.Linq
Public Class pcPoolVoucher
    'percentage!
    Inherits poolVoucher

    Sub New(ByVal data As XmlNode)
        MyBase.new(data)
    End Sub

    Protected Overrides Sub Enact()

        Dim matchedParts As IOrderedEnumerable(Of CartItem) = _
                                    From ci In ts.cart.CartItems.Values _
                                    Where (BuyPart.Contains(ci.PARTNAME)) _
                                    AndAlso ci.LINETOTAL > 0 _
                                    Order By CDbl(ci.PARTPRICE) Ascending

        Dim matchedQty As Integer = Aggregate ci In matchedParts _
                                    Into Sum(CInt(ci.QTY))

        'iterates over line's qty, then lines, discounting once per part
        'starting with cheapest part.
        Dim Line As Integer = 0
        Dim Remaining As Integer = matchedQty \ (BuyQty + GetQty)

        While Remaining > 0
            Dim Qty As Integer = matchedParts(Line).QTY
            While Qty > 0 And Remaining > 0
                matchedParts(Line).Discount += _
                                    CDbl(matchedParts(Line).PARTPRICE) / 100 _
                                    * Discount
                Qty -= 1
                Remaining -= 1
            End While
            Line += 1
        End While
    End Sub
End Class
