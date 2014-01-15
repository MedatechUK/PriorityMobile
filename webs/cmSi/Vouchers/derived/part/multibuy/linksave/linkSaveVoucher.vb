Imports System.Xml
Imports System.Linq
Public Class linkSaveVoucher
    Inherits multiBuyVoucher
#Region "Public Properties"
    Private _linkparts As Dictionary(Of String, List(Of String))
    Public Property LinkParts() As Dictionary(Of String, List(Of String))
        Get
            Return _linkparts
        End Get
        Set(ByVal value As Dictionary(Of String, List(Of String)))
            _linkparts = value
        End Set
    End Property


    Private _getparts As System.Collections.Generic.IEnumerable(Of String)
    Protected ReadOnly Property GetParts() As System.Collections.Generic.IEnumerable(Of String)
        Get
            Return _getparts
        End Get
    End Property

    Private _matchedparts As IOrderedEnumerable(Of CartItem)
    Protected ReadOnly Property MatchedParts() As IOrderedEnumerable(Of CartItem)
        Get
            Return _matchedparts
        End Get
    End Property

    Private _matchedbuyqty As Integer
    Protected ReadOnly Property MatchedBuyQty() As Integer
        Get
            Return _matchedbuyqty
        End Get
    End Property

    Private _matchedgetqty As Integer
    Protected ReadOnly Property MatchedGetQty() As Integer
        Get
            Return _matchedgetqty
        End Get
    End Property
#End Region

    Sub New(ByVal data As XmlNode)
        MyBase.New(data)
        'init dictionary of buypart, list of get parts
        LinkParts = New Dictionary(Of String, List(Of String))

        For Each bp As String In BuyPart
            Dim getParts As New List(Of String)
            For Each gp As XmlNode In data.SelectNodes( _
                            String.Format("buy/part[@name = '{0}']//gpart", bp))
                getParts.Add(gp.Attributes("name").Value)
            Next
            LinkParts.Add(bp, getParts)
        Next
    End Sub

    Protected Overrides Sub Enact()
        'initialise properties from linq, call method to actually enact - 

        _getparts = From gpList In _
                            (From bp In LinkParts.Keys _
                            Join ci In ts.cart.CartItems.Values _
                            On ci.PARTNAME Equals bp _
                            Where CDbl(ci.QTY) >= BuyQty _
                            AndAlso CDbl(ci.LINETOTAL) > 0 _
                            Select LinkParts(bp)), _
                       item In gpList Select item

        _matchedparts = From ci In ts.cart.CartItems.Values _
                           Join part In GetParts _
                           On part Equals ci.PARTNAME _
                           Where CDbl(ci.QTY) >= GetQty _
                           AndAlso CDbl(ci.LINETOTAL) > 0 _
                           Select ci _
                           Order By CDbl(ci.PARTPRICE)

        _matchedbuyqty = (Aggregate ci In ts.cart.CartItems.Values _
                                      Join part In BuyPart _
                                      On part Equals ci.PARTNAME _
                                      Into Sum(CInt(ci.QTY))) \ BuyQty

        _matchedgetqty = (Aggregate ci In MatchedParts _
                                   Into Sum(CInt(ci.QTY))) \ GetQty

        EnactEffects()
    End Sub

    Protected Overridable Sub EnactEffects()
        Dim Line As Integer = 0
        Dim Remaining As Integer = Math.Min(MatchedBuyQty, MatchedGetQty)

        While Remaining > 0
            Dim Qty As Integer = MatchedParts(Line).QTY
            While Qty > 0 And Remaining > 0
                If CDbl(MatchedParts(Line).LINETOTAL) = 0 Then
                    Exit While
                End If
                MatchedParts(Line).Discount += Discount
                Qty -= 1
                Remaining -= 1
            End While
            Line += 1
        End While
    End Sub
End Class
