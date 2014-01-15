Imports System.Web
Imports System.Xml

Public Class pcSpendVoucher : Inherits spendVoucher
    'this voucher type will take a spend, optionally zero and offer a discount
    ''' <summary>
    ''' Inherits spendVoucher type. Instantiates base voucher with 
    ''' percentage discount (overrides discount property)
    ''' on entire order, with optional minimum spend.
    ''' </summary>
    ''' <remarks></remarks>
#Region "Constructor"

    Public Sub New(ByVal data As XmlNode)
        MyBase.new(data)
    End Sub

    Public Overrides Property discount() As Double
        'property overriden here rather than constructor because discount is proportional
        'to cart total, which is likely to change after instantiation. 
        Get
            If CDbl(ts.cart.Total) >= Spend Then
                Return (CDbl(ts.cart.Total) / 100) * MyBase.Discount
            Else
                Return 0.0
            End If
        End Get

        Set(ByVal value As Double)
            MyBase.Discount = value
        End Set
    End Property

#End Region


End Class
