Imports System.Xml

Public Class multiBuyVoucher
    Inherits partVoucher

    Sub New(ByVal data As XmlNode)
        MyBase.New(data)
        BuyQty = CInt(VoucherData.SelectSingleNode("buy").Attributes("qty").Value)
        GetQty = CInt(VoucherData.SelectSingleNode("get").Attributes("qty").Value)
    End Sub

    Private _buyQty As Integer
    Public Property BuyQty() As Integer
        Get
            Return _buyQty
        End Get
        Set(ByVal value As Integer)
            _buyQty = value
        End Set
    End Property

    Private _getQty As Integer
    Public Property GetQty() As Integer
        Get
            Return _getQty
        End Get
        Set(ByVal value As Integer)
            _getQty = value
        End Set
    End Property

    Protected Overrides Sub Enact()
        For Each item As CartItem In ts.cart.CartItems.Values
            If BuyPart.Contains(item.PARTNAME) And (item.QTY \ (BuyQty + GetQty) > 0) Then
                item.Discount += Discount * GetQty * (item.QTY \ (BuyQty + GetQty))
            End If
        Next


    End Sub

End Class
