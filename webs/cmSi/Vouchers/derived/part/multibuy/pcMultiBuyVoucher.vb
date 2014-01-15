Imports System.Xml
Public Class pcMultiBuyVoucher
    Inherits multiBuyVoucher

    Sub New(ByVal data As xmlnode)
        MyBase.New(data)
    End Sub

    Protected Overrides Sub Enact()
        For Each item As CartItem In ts.cart.CartItems.Values
            If BuyPart.Contains(item.PARTNAME) And (item.QTY \ (BuyQty + GetQty) > 0) Then
                item.Discount += CDbl(item.PARTPRICE / 100) * Discount * GetQty * (item.QTY \ (BuyQty + GetQty))
            End If
        Next
    End Sub

End Class
