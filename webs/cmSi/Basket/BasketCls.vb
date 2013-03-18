Imports Microsoft.VisualBasic
Imports System.Collections.Generic
Imports System.Xml
Imports System.IO
Imports System.Web
Imports System.Web.UI.WebControls

Public Class BasketCls

#Region "initialisations"

    Public Sub New()
        '********************************* This sets up the basket        
        CURRENCY = cmsData.Settings.Get("DefaultCurrency")
        TAXCODE = "001"
        DEFAULTDELIVERY = xmlFunc.DefaultDelivery(cmsData.part, CURRENCY)
        'UpdateDeliveryOptions()
    End Sub

#End Region

    Public Sub UpdateDeliveryOptions()

    End Sub

#Region "Public Variables"

    Public BasketItems As New Dictionary(Of String, BasketItem)
    Public DeliveryItems As New Dictionary(Of String, DeliveryItem)

#End Region

#Region "Private Variables"

    Private ReadOnly Property ts() As Session
        Get
            Return cmsSessions.CurrentSession(HttpContext.Current)
        End Get
    End Property

#End Region

#Region "public properties"

    Private _CURRENCY As String = "GBP"
    Public Property CURRENCY() As String
        Get
            Return _CURRENCY
        End Get
        Set(ByVal value As String)
            _CURRENCY = value
        End Set
    End Property

    Private _TAXCODE As String = "001"
    Public Property TAXCODE() As String
        Get
            Return _TAXCODE
        End Get
        Set(ByVal value As String)
            _TAXCODE = value
        End Set
    End Property

    Public ReadOnly Property BasketURL() As String
        Get
            Return cmsData.Settings.Get("BasketPage")
        End Get
    End Property

    Private tmp As New DropDownList
    Public ReadOnly Property DelOptList() As DropDownList
        Get
            With tmp
                .ID = "DELOPT"

                With .Items

                    'List the delivery options for this currency
                    Dim nodes As XmlNodeList = AllDeliveryParts(cmsData.part) 'DeliveryParts(cmsData.Part, CURRENCY) '

                    If Not IsNothing(nodes) Then
                        For Each n As XmlNode In nodes
                            Dim li As New ListItem
                            With li
                                .Value = n.SelectSingleNode("PARTNAME").InnerText
                                .Text = n.SelectSingleNode("PARTDES").InnerText
                                '.Selected = CBool(String.Compare(li.Value, ts.cart.DELIVERY.DELIVERYPART, True) = 0)
                                '.Enabled = Not IsNothing(CurrentDelivery(cmsData.Part, ts.cart.CURRENCY, .Value))
                            End With
                            If Not .Contains(li) Then
                                .Add(li)
                            End If
                            'If Not (tmp.Items(tmp.Items.IndexOf(li)).Enabled = Not IsNothing(CurrentDelivery(cmsData.Part, ts.cart.CURRENCY, n.SelectSingleNode("PARTNAME").InnerText))) Then
                            '    tmp.Items(tmp.Items.IndexOf(li)).Enabled = Not IsNothing(CurrentDelivery(cmsData.Part, ts.cart.CURRENCY, n.SelectSingleNode("PARTNAME").InnerText))
                            'End If
                        Next
                    End If
                End With
            End With

            Return tmp
        End Get
    End Property

    Private _DEFAULTDELIVERY As String = ""
    Public Property DEFAULTDELIVERY() As String
        Get
            Return _DEFAULTDELIVERY
        End Get
        Set(ByVal value As String)
            _DEFAULTDELIVERY = value
        End Set
    End Property

#End Region

#Region "Update Basket"

    Public Sub AddBasketItem(ByVal Item As BasketItem)
        With BasketItems
            If Not .ContainsKey(Item.PARTNAME) Then
                .Add(Item.PARTNAME, Item)
            Else
                Dim newQTY As String = CStr(CInt(.Item(Item.PARTNAME).QTY) + CInt(Item.QTY))
                .Item(Item.PARTNAME).QTY = newQTY
            End If
        End With
    End Sub

    Public Sub RemoveBasketItem(ByVal PARTNAME As String)
        With BasketItems
            If .ContainsKey(PARTNAME) Then
                .Remove(PARTNAME)
            End If
        End With

        With cmsSessions.CurrentSession(HttpContext.Current).cart.CartItems
            If .ContainsKey(PARTNAME) Then
                .Remove(PARTNAME)
            End If
        End With
    End Sub

    Public Sub SetBasketItemQTY(ByVal PARTNAME As String, ByVal QTY As String)
        With BasketItems
            If .ContainsKey(PARTNAME) Then
                .Item(PARTNAME).QTY = QTY
            End If
        End With
    End Sub

#End Region

#Region "Public Subs (Cart Bindings)"

    Public Sub Clear()
        BasketItems.Clear()
    End Sub

    Public Sub BindBasket(ByRef Grid As System.Web.UI.WebControls.GridView)

        ts.cart.CartItems.Clear()
        For Each key As String In BasketItems.Keys
            Dim part As XmlNode = xmlFunc.Part(cmsData.part, BasketItems(key).PARTNAME)
            If Not IsNothing(part) Then

                Dim cur As XmlNode
                If HttpContext.Current.User.Identity.IsAuthenticated Then
                    cur = PartCurrency(part, ts.Basket.CURRENCY, HttpContext.Current.Profile("CUSTNAME"))
                Else
                    cur = PartCurrency(part, ts.Basket.CURRENCY, "")
                End If

                With ts.cart.CartItems
                    .Add(part.SelectSingleNode("PARTNAME").InnerText, _
                        New CartItem(part.SelectSingleNode("PARTNAME").InnerText, _
                        part.SelectSingleNode("PARTDES").InnerText, _
                        CStr(QTYPrice(cur, BasketItems(key).QTY)), _
                        BasketItems(key).QTY, _
                        part.SelectSingleNode("PACKFAMILY").InnerText, _
                         cur.Attributes("TAXRATE").InnerText, _
                        BasketItems(key).REFERER _
                        ) _
                    )
                End With
            End If
        Next

        'Check the current delivery part
        With ts.cart.DELIVERY
            If IsNothing(CurrentDelivery(cmsData.part, CURRENCY, .DELIVERYPART)) Then
                ' Not a valid delivery part so get default for currency
                .DELIVERYPART = xmlFunc.DefaultDelivery(cmsData.part, CURRENCY)
            End If
        End With

        With ts.cart
            Dim del As XmlNode = CurrentDelivery(cmsData.part, CURRENCY, .DELIVERY.DELIVERYPART)
            Dim cur As XmlNode = DeliveryCurrency(del, CURRENCY)
            .CartItems.Add(del.SelectSingleNode("PARTNAME").InnerText, _
                New CartItem(del.SelectSingleNode("PARTNAME").InnerText, _
                del.SelectSingleNode("PARTDES").InnerText, _
                CStr(DeliveryPrice(cmsData.part, ts.cart, CURRENCY, .DELIVERY.DELIVERYPART)), _
                "1", _
                0, _
                cur.Attributes("TAXRATE").InnerText, _
                "" _
                ) _
            )
        End With

        With Grid
            .DataSource = ts.cart.CartItems.Values
            .DataBind()

            For Each col As GridViewRow In .Rows
                Dim PARTNAME As String = SelectedPart(Grid, col.RowIndex)
                If Not IsNothing(CurrentDelivery(cmsData.part, ts.cart.CURRENCY, PARTNAME)) Then
                    For Each ct As Object In col.Cells(6).Controls
                        Try
                            If ct.commandname = "Delete" Then ct.visible = False
                        Catch
                        End Try
                    Next
                End If
            Next
        End With

    End Sub

    Public Sub AddHandlers(ByRef Grid As System.Web.UI.WebControls.GridView)
        ts.cart.CURRENCY = CURRENCY
        With Grid
            AddHandler .RowDataBound, AddressOf hRowDataBound
            AddHandler .RowDeleting, AddressOf hRowDeleting
            AddHandler .RowEditing, AddressOf hRowEditing
            AddHandler .RowCancelingEdit, AddressOf hRowCancelingEdit
            AddHandler .RowUpdating, AddressOf hRowUpdating
        End With
    End Sub

#End Region

#Region "Basket event Handlers"

    Private Sub hRowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)

        Dim PARTNAME As String = SelectedPart(sender, e.Row.RowIndex)
        With ts.cart
            If e.Row.RowType = DataControlRowType.Footer Then
                With e.Row.Cells(1)
                    .CssClass = "empty"
                    .ColumnSpan = 3
                End With                

                For n As Integer = 2 To 3
                    e.Row.Cells(n).Visible = False
                Next
                e.Row.Cells(4).Text = _
                    "<b>Sub Total:</b><br/>" & _
                    "<b>VAT:</b><br/>" & _
                    "<b>Total:</b>"
                e.Row.Cells(5).HorizontalAlign = HorizontalAlign.Left
                e.Row.Cells(5).Text = _
                    .Value & "<br/>" & _
                    .TotalTax & "<br/>" & _
                    .Total
                e.Row.Cells(6).VerticalAlign = VerticalAlign.Bottom
                e.Row.Cells(6).Text = _
                    .CURRENCY
            End If
        End With

    End Sub

    Private Sub hRowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs)

        Dim PARTNAME As String = SelectedPart(sender, e.NewEditIndex)
        Dim Cell As TableCellCollection

        With sender
            'Set the edit inde
            .EditIndex = e.NewEditIndex
        End With

        With ts
            'Bind data to the GridView control.        
            .Basket.BindBasket(sender)
            Try
                Cell = sender.Rows(e.NewEditIndex).Cells()
            Catch
                Exit Sub
            End Try

            If Not (String.Compare(PARTNAME, .cart.DELIVERY.DELIVERYPART) = 0) Then
                Cell(4).Controls.Clear()

                Dim bCount As Integer
                Try
                    bCount = ts.Basket.BasketItems(PARTNAME).BoxQTY
                Catch ex As Exception
                    bCount = 1
                End Try

                Select Case bCount
                    Case 1
                        Dim ctrl As New TextBox()
                        ctrl.Text = .Basket.BasketItems(PARTNAME).QTY
                        ctrl.ID = "txtQTY"
                        ctrl.Width = "25"
                        ctrl.MaxLength = 4
                        Cell(4).Controls.Add(ctrl)
                    Case Else
                        Dim bx As New System.Web.UI.WebControls.DropDownList
                        With bx
                            .ID = "txtQTY"
                            For bq As Integer = 0 To 10
                                .Items.Add(New ListItem(String.Format("{0} box{2} ({1})", bq.ToString, bq * bCount.ToString, Plural(CInt(bq.ToString))), bq * bCount))
                                Try
                                    .Items(.Items.Count - 1).Selected = CBool(CInt(bq * bCount) = CInt(ts.Basket.BasketItems(PARTNAME).QTY))
                                Catch
                                End Try
                            Next
                        End With
                        Cell(4).Controls.Add(bx)

                End Select
            Else
                Cell(2).Controls.Clear()
                DelOptList.SelectedValue = .cart.DELIVERY.DELIVERYPART
                'ctrl.AutoPostBack = True
                'AddHandler ctrl.SelectedIndexChanged, AddressOf hSelectedIndexChanged
                Cell(2).Controls.Add(DelOptList)
            End If


        End With
    End Sub

    Private Function Plural(ByVal number As Integer) As String
        If number <> 1 Then
            Return "es"
        Else
            Return ""
        End If
    End Function

    Private Sub hSelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim SelDel As DeliveryItem = DeliveryItems(sender.Items(sender.SelectedIndex).Value)
        With cmsSessions.CurrentSession(HttpContext.Current).cart.DELIVERY
            .DELIVERYPART = SelDel.DELIVERYPART
            .SALESTAX = SelDel.SALESTAX
            .PARTDES = SelDel.PARTDES
        End With

    End Sub

    Private Sub hRowCancelingEdit(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCancelEditEventArgs)

        Dim PARTNAME As String = SelectedPart(sender, e.RowIndex)
        With sender
            Try
                .Rows(.EditIndex).Cells(4).FindControl("txtQTY").text = BasketItems(PARTNAME).QTY
            Catch ex As Exception

            End Try
        End With


        'Reset the edit inde
        sender.EditIndex = -1
        'Bind data to the GridView control.
        With cmsSessions.CurrentSession(HttpContext.Current).Basket
            .BindBasket(sender)
        End With
    End Sub

    Private Sub hRowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs)
        Dim ctl As System.Web.UI.WebControls.GridView = sender
        Dim PARTNAME As String = SelectedPart(sender, e.RowIndex)
        If Not IsNothing(PARTNAME) Then
            With cmsSessions.CurrentSession(HttpContext.Current).Basket
                .RemoveBasketItem(PARTNAME)
                .BindBasket(sender)
            End With
        End If
        With ctl.Page
            Dim BasketCount As System.Web.UI.WebControls.Literal = .Master.FindControl("BasketCount")
            If Not IsNothing(BasketCount) Then BasketCount.Text = ts.Basket.BasketItems.Count
            Dim BasketCount2 As System.Web.UI.WebControls.Literal = .Master.FindControl("BasketCount2")
            If Not IsNothing(BasketCount2) Then BasketCount2.Text = ts.Basket.BasketItems.Count
            Dim BasketCount3 As System.Web.UI.WebControls.Literal = .Master.FindControl("BasketCount3")
            If Not IsNothing(BasketCount3) Then BasketCount3.Text = ts.Basket.BasketItems.Count
            Dim BasketCount4 As System.Web.UI.WebControls.Literal = .Master.FindControl("BasketCount4")
            If Not IsNothing(BasketCount4) Then BasketCount4.Text = ts.Basket.BasketItems.Count
            Dim BasketCount5 As System.Web.UI.WebControls.Literal = .Master.FindControl("BasketCount5")
            If Not IsNothing(BasketCount5) Then BasketCount5.Text = ts.Basket.BasketItems.Count
        End With
    End Sub

    Protected Sub hRowUpdating(ByVal sender As Object, ByVal e As GridViewUpdateEventArgs)

        Dim RID As String = Right("00" & CStr(e.RowIndex + 2), 2)
        Dim PARTNAME As String = SelectedPart(sender, e.RowIndex)

        If Not IsNothing(CurrentDelivery(cmsData.part, ts.Basket.CURRENCY, PARTNAME)) Then
            Dim DELPART As String = cmsSessions.CurrentSession(HttpContext.Current).cfd("delopt", Nothing)
            If Not IsNothing(DELPART) Then
                ts.cart.DELIVERY.DELIVERYPART = DELPART
            End If
        Else
            Dim newQTY As String = ts.cfd("BasketGrid$ctl" & RID & "$txtQTY", Nothing)
            If Not IsNothing(newQTY) Then
                With ts.Basket
                    If Not IsNothing(PARTNAME) And Not IsNothing(newQTY) Then
                        .SetBasketItemQTY(PARTNAME, newQTY)
                    End If
                End With
            End If
        End If

        'Reset the edit inde
        sender.EditIndex = -1
        'Bind data to the GridView control.
        With cmsSessions.CurrentSession(HttpContext.Current).Basket
            .BindBasket(sender)
        End With
    End Sub

#End Region

#Region "Private Functions"

    Private Function SelectedPart(ByVal sender As System.Web.UI.WebControls.GridView, ByVal row As Integer) As String
        Dim k As Object = Nothing
        Try
            k = sender.DataKeys(row)
        Catch
            Return Nothing
        End Try
        If Not IsNothing(k) Then
            If k.value.Length Then
                Return k.value
            Else
                Return Nothing
            End If
        Else
            Return Nothing
        End If
    End Function

#End Region

End Class