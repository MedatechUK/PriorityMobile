﻿Imports System.Xml
Imports PriorityMobile

Module SharedXMLFunc

#Region "add Order"

    Private Function StandardPricelist(ByRef thisform As xForm) As XmlNode
        Return thisform.FormData.SelectSingleNode("pdadata/stdpricelist")
    End Function

    Private Sub GetCustomerPrice(ByRef thisform As xForm, ByRef Price As Double, ByVal PriceList As XmlNode, ByVal name As String, ByVal qty As Double)
        With thisform
            For Each custPrice As XmlNode In PriceList.SelectNodes(String.Format("parts/part[name='{0}']", name))
                If qty >= CDbl(custPrice.SelectSingleNode("tquant").InnerText) Then
                    If CDbl(custPrice.SelectSingleNode("price").InnerText) < Price Then
                        Price = CDbl(custPrice.SelectSingleNode("price").InnerText)
                    End If
                End If
            Next
        End With
    End Sub

    Public Sub AddOrderItem(ByRef thisform As xForm, ByRef Order As XmlNode, ByVal name As String, ByVal barcode As String, ByVal des As String, ByVal qty As String)

        With thisform
            Dim parts As XmlNode = Order.SelectSingleNode("parts")
            Dim OrderPart As XmlNode = parts.SelectSingleNode(String.Format("part[name='{0}']", name))
            Dim Price As Double = CDbl(StandardPricelist(thisform).SelectSingleNode(String.Format(".//part[name='{0}']/price", name)).InnerText)
            If IsNothing(OrderPart) Then
                GetCustomerPrice(thisform, Price, Order.ParentNode.ParentNode.SelectSingleNode("customerpricelist"), name, CDbl(qty))
                Dim part As XmlNode = .CreateNode(parts, "part")
                .CreateNode(part, "name", name)
                .CreateNode(part, "barcode", barcode)
                .CreateNode(part, "des", des)
                .CreateNode(part, "qty", qty)
                .CreateNode(part, "unitprice", Price)
            Else
                GetCustomerPrice(thisform, Price, Order.ParentNode.ParentNode.SelectSingleNode("customerpricelist"), name, CDbl(OrderPart.SelectSingleNode("qty").InnerText) + CDbl(qty))
                OrderPart.SelectSingleNode("qty").InnerText = CDbl(OrderPart.SelectSingleNode("qty").InnerText) + CDbl(qty)
                OrderPart.SelectSingleNode("unitprice").InnerText = Price
            End If

            Dim total As Double = 0
            For Each part As XmlNode In Order.SelectNodes(".//part")
                total += CDbl(part.SelectSingleNode("qty").InnerText) * CDbl(part.SelectSingleNode("unitprice").InnerText)
            Next
            Order.SelectSingleNode("value").InnerText = total.ToString
            .Save()
        End With

    End Sub

    Public Sub AddOrder(ByRef thisform As xForm, ByRef Orders As XmlNode, ByVal DeliveryDate As String, ByVal PONum As String)
        With thisform
            Dim Order As XmlNode = .CreateNode(Orders, "order")
            .CreateNode(Order, "deliverydate", DeliveryDate)
            .CreateNode(Order, "ponum", PONum)
            .CreateNode(Order, "value", "0.00")
            Dim Parts As XmlNode = .CreateNode(Order, "parts")
            Dim Part As XmlNode = .CreateNode(Parts, "part")
            .CreateNode(Part, "name", "0")
            .CreateNode(Part, "barcode", "0")
            .CreateNode(Part, "des", "0")
            .CreateNode(Part, "qty", "0")
            .CreateNode(Part, "unitprice", "0")
            .Save()
        End With
    End Sub

#End Region

#Region "Credit Note"

    Public Sub AddCreditNote(ByVal thisform As xForm, ByVal CreditNote As XmlNode, ByVal ordi As String, ByVal ivnum As String, ByVal name As String, ByVal des As String, ByVal qty As String, ByVal unitprice As String, ByVal rcvdqty As String, ByVal reason As String)

        With thisform
            If IsNothing(CreditNote.SelectSingleNode(String.Format(".//part[ordi='{0}' and reason='{1}']", ordi, reason))) Then
                Dim part As XmlNode = .CreateNode(CreditNote.SelectSingleNode("parts"), "part")
                .CreateNode(part, "ivnum", ivnum)
                .CreateNode(part, "ordi", ordi)
                .CreateNode(part, "name", name)
                .CreateNode(part, "des", des)
                .CreateNode(part, "qty", qty)
                .CreateNode(part, "unitprice", unitprice)
                .CreateNode(part, "rcvdqty", rcvdqty)
                .CreateNode(part, "reason", reason)
            Else
                Dim part As XmlNode = CreditNote.SelectSingleNode(String.Format(".//part[ordi='{0}' and reason='{1}']", ordi, reason))
                part.SelectSingleNode("qty").InnerText = (CDbl(part.SelectSingleNode("qty").InnerText) + CDbl(qty)).ToString
                part.SelectSingleNode("rcvdqty").InnerText = (CDbl(part.SelectSingleNode("rcvdqty").InnerText) + rcvdqty).ToString
            End If
            .Save()
        End With

    End Sub

#End Region

#Region "delivery"

    Public Sub DeliverItem(ByVal thisform As xForm, ByVal Delivery As XmlNode, ByVal ordi As String, ByVal name As String, ByVal des As String, ByVal parttype As String, ByVal barcode As String, ByVal lotnumber As String, ByVal cquant As String)
        With thisform
            Dim warehouse As XmlNode = .FormData.SelectSingleNode("pdadata/warehouse")
            Dim lot As XmlNode = warehouse.SelectSingleNode(String.Format(".//lot[name='{0}']", lotnumber))
            Dim deliveryParts As XmlNode = Delivery.SelectSingleNode("parts")
            Dim deliveryPart As XmlNode = deliveryParts.SelectSingleNode(String.Format(".//part[ordi = '{0}' and cquant = '0']", ordi))
            Dim deliveredPart As XmlNode = deliveryParts.SelectSingleNode(String.Format(".//part[ordi = '{0}' and lotnumber = '{1}' and cquant != '0']", ordi, lotnumber))
            If IsNothing(deliveredPart) Then
                Dim part As XmlNode = .CreateNode(deliveryParts, "part")
                .CreateNode(part, "ordi", ordi & "|" & lotnumber)
                .CreateNode(part, "name", name)
                .CreateNode(part, "des", des)
                .CreateNode(part, "parttype", parttype)
                .CreateNode(part, "barcode", barcode)
                .CreateNode(part, "lotnumber", lotnumber)
                .CreateNode(part, "tquant", "0")
                .CreateNode(part, "cquant", cquant)
            Else
                deliveredPart.SelectSingleNode("cquant").InnerText = CDbl(deliveredPart.SelectSingleNode("cquant").InnerText) + CDbl(cquant)
            End If
            deliveryPart.SelectSingleNode("tquant").InnerText = CDbl(deliveryPart.SelectSingleNode("tquant").InnerText) - CDbl(cquant)
            lot.SelectSingleNode("qty").InnerText = CDbl(lot.SelectSingleNode("qty").InnerText) - CDbl(cquant)
            .Save()
        End With
    End Sub

    Public Function preClose(ByRef thisform As xForm, ByRef delivery As XmlNode) As Boolean
        Dim ret As Boolean = True
        If delivery.SelectNodes("parts/part[cquant > 0]").Count > 0 Then
            Dim sig As XmlNode = delivery.SelectSingleNode("customersignature")
            If CBool(sig.SelectSingleNode("mandatory").InnerText = "Y") Then
                If Not (sig.SelectSingleNode("image").InnerText.Length > 0 And sig.SelectSingleNode("print").InnerText.Length > 0) Then
                    MsgBox("Customer signature is mandatory.")
                    ret = False
                End If
            End If
            If delivery.SelectNodes("parts/part[tquant > 0]").Count > 0 Then
                ret = MsgBox("Not all parts were delivered. Continue?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes
            End If
        End If
        Return ret
    End Function

    Public Function postclose(ByRef thisform As xForm, ByRef delivery As XmlNode, ByVal Delivered As Boolean) As Boolean
        Dim ret As Boolean = True
        If Delivered Then
            If delivery.SelectNodes("parts/part[cquant > 0]").Count = 0 Then
                MsgBox("Nothing has been delivered.")
                ret = False
            End If
        Else
            If delivery.SelectNodes("parts/part[cquant > 0]").Count > 0 Then
                MsgBox("Parts have been marked as delivered.")
                ret = False
            End If
        End If
        Return ret
    End Function

#End Region

End Module
