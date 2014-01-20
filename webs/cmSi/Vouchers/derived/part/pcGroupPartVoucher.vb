Imports System.Xml
Public Class pcGroupPartVoucher : Inherits groupPartVoucher

    Public Sub New(ByVal data As xmlnode)
        MyBase.New(data)
    End Sub

    Protected Overrides Sub enact()
        ApplicableSpend = 0
        For Each item As CartItem In ts.cart.CartItems.Values
            If BuyPart.Contains(item.PARTNAME) Then
                ApplicableSpend += item.LINETOTAL
            End If
        Next
        ts.cart.Discount += CDbl(ApplicableSpend / 100) * Discount
    End Sub

End Class