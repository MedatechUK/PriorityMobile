Imports System.Xml

Public Class groupPartVoucher : Inherits partVoucher

    Public Sub New(ByVal data As xmlnode)
        MyBase.new(data)

    End Sub

    Private _applicableSpend As Double
    Protected Property ApplicableSpend() As Double
        Get
            Return _applicableSpend
        End Get
        Set(ByVal value As Double)
            _applicableSpend = value
        End Set
    End Property

    Public Overrides Property Discount() As Double
        Get
            If ApplicableSpend >= MyBase.Spend Then
                Return MyBase.Discount
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal value As Double)
            MyBase.Discount = value
        End Set
    End Property

    Protected Overrides Sub enact()
        ApplicableSpend = 0
        For Each item As CartItem In ts.cart.CartItems.Values
            If BuyPart.Contains(item.PARTNAME) Then
                ApplicableSpend += item.LINETOTAL
            End If
        Next
        ts.cart.Discount += Discount
    End Sub
End Class
