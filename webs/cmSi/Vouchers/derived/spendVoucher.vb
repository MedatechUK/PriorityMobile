Imports System.Web
Imports System.Xml

Public Class spendVoucher : Inherits Voucher
    ''' <summary>
    ''' Inherits Voucher type. Instantiates voucher with discount on entire order, with optional minimum spend.
    ''' </summary>
    ''' <remarks></remarks>
#Region "Public Properties"

    Private _spend As Double
    Public Property Spend() As Double
        Get
            Return _spend
        End Get
        Set(ByVal value As Double)
            _spend = value
        End Set
    End Property
#End Region

#Region "Overriden Properties"
    Public Overrides Property Discount() As Double
        Get
            If CDbl(ts.cart.Total) >= Spend Then
                Return MyBase.Discount
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal value As Double)
            MyBase.Discount = value
        End Set
    End Property
#End Region

#Region "Constructor"
    Public Sub New(ByVal data As XmlNode)
        MyBase.New(data)
        Spend = CDbl(VoucherData.SelectSingleNode("buy/spend").Attributes("amount").Value)
        Discount = CDbl(VoucherData.SelectSingleNode("get/discount").Attributes("amount").Value)
    End Sub
#End Region
#Region "Method Overrides"

    Protected Overrides Sub Enact()
        ts.cart.Discount += Discount
    End Sub
#End Region
End Class

