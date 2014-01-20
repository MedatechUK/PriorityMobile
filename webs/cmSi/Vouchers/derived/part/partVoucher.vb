Imports System.Xml
'''<summary> 
''' inherits base voucher type. If cart item is in buyPart list, apply discount.
''' </summary>
Public Class partVoucher

    Inherits spendVoucher
    Private _buyPart As List(Of String)
    Public Property BuyPart() As List(Of String)
        Get
            Return _buyPart
        End Get
        Set(ByVal value As List(Of String))
            _buyPart = value
        End Set
    End Property

    Sub New(ByVal data As XmlNode)
        MyBase.New(data)
        BuyPart = New List(Of String)
        For Each Part As XmlNode In VoucherData.SelectNodes("buy/part")
            BuyPart.Add(Part.Attributes("name").Value)
        Next
    End Sub

    Protected Overrides Sub Enact()
        For Each item As CartItem In ts.cart.CartItems.Values
            If BuyPart.Contains(item.PARTNAME) Then
                item.Discount += Discount * CInt(item.QTY)
            End If
        Next
    End Sub
End Class
