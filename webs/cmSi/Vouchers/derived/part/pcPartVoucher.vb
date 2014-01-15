Imports System.Xml

Public Class pcPartVoucher
    Inherits partVoucher

    Sub New(ByVal data As XmlNode)
        MyBase.New(data)
    End Sub

    Protected Overrides Sub Enact()
        For Each item As CartItem In ts.cart.CartItems.Values
            If BuyPart.Contains(item.PARTNAME) Then
                item.Discount += CDbl(item.PARTPRICE / 100) * Discount * CInt(item.QTY)
            End If
        Next
    End Sub

End Class
