Imports Microsoft.VisualBasic
Imports System.IO
Imports System.Collections.Generic
Imports System.Xml
Imports System.Web

Module xmlFunc

    Private ReadOnly Property ts() As Session
        Get
            Return cmsSessions.CurrentSession(HttpContext.Current)
        End Get
    End Property

    Public Function AllParts(ByRef Doc As XmlDocument) As Dictionary(Of String, String)
        Try
            Dim ret As New Dictionary(Of String, String)
            For Each node As XmlNode In Doc.SelectNodes(String.Format( _
                "/*[position()=1]/PARTS/PART[not(@DELIVERY)]", _
                Chr(34)))
                ret.Add(node.SelectSingleNode("PARTNAME").InnerText, node.SelectSingleNode("PARTDES").InnerText)
            Next
            Return ret
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Public Function Part(ByRef Doc As XmlDocument, ByVal PartName As String) As XmlNode
        Try
            Return Doc.SelectSingleNode(String.Format( _
                "/*[position()=1]/PARTS/PART[PARTNAME={0}{1}{0}]", _
                Chr(34), PartName))
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Public Function AllDeliveryParts(ByRef Doc As XmlDocument) As XmlNodeList
        Try
            Return Doc.SelectNodes(String.Format( _
                "/*[position()=1]/PARTS/PART[@DELIVERY]", _
                Chr(34)))
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Public Function DeliveryParts(ByRef Doc As XmlDocument, ByVal Currency As String) As XmlNodeList
        Try
            Return Doc.SelectNodes(String.Format( _
                "/*[position()=1]/PARTS/PART[@DELIVERY]/PRICE/CURRENCY[@CURSTR={0}{1}{0}]/ancestor::PART[1]", _
                Chr(34), Currency))
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Public Function PartCurrency(ByRef PartNode As XmlNode, ByVal CURSTR As String, ByRef CUSTNAME As String) As XmlNode
        Try
            If CUSTNAME.Length > 0 Then
                Dim N As XmlNode = PartNode.SelectSingleNode(String.Format("PRICE[@CUSTNAME={0}{2}{0}]/CURRENCY[@CURSTR={0}{1}{0}]", Chr(34), CURSTR, CUSTNAME))
                If IsNothing(N) Then
                    Return PartNode.SelectSingleNode(String.Format("PRICE[@DEFAULT={0}1{0}]/CURRENCY[@CURSTR={0}{1}{0}]", Chr(34), CURSTR))
                Else
                    Return N
                End If
            Else
                Return PartNode.SelectSingleNode(String.Format("PRICE[@DEFAULT={0}1{0}]/CURRENCY[@CURSTR={0}{1}{0}]", Chr(34), CURSTR))
            End If

        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Public Function DeliveryCurrency(ByRef PartNode As XmlNode, ByVal CURSTR As String) As XmlNode
        Try

            Return PartNode.SelectSingleNode(String.Format("PRICE/CURRENCY[@CURSTR={0}{1}{0}]", Chr(34), CURSTR))

        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Public Function DefaultDelivery(ByRef Doc As XmlDocument, ByVal CUR As String) As String
        Try
            Dim n As XmlNode = Doc.SelectSingleNode(String.Format( _
                   "/*[position()=1]/CURRENCY/CODE[@CODE = {0}{1}{0}]", Chr(34), CUR))

            'Dim n As XmlNode = Doc.SelectSingleNode(String.Format( _
            '       "/*[position()=1]/PARTS/PART[@DELIVERY and @DEFAULT= {0}{1}{0}]/PARTNAME", Chr(34), CUR))
            Return n.Attributes("DEFDEL").Value
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Public Function WebCurrencies(ByRef Doc As XmlDocument) As XmlNodeList
        Try
            Return Doc.SelectNodes(String.Format( _
                   "/*[position()=1]/CURRENCY/CODE", Chr(34)))

        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Public Function CurrentDelivery(ByRef Doc As XmlDocument, ByVal CUR As String, ByVal DeliveryPart As String) As XmlNode
        Try
            Return Doc.SelectSingleNode(String.Format( _
                "/*[position()=1]/PARTS/PART[@DELIVERY]/PRICE/CURRENCY[@CURSTR={0}{1}{0}]/ancestor::PART[position()=1 and PARTNAME={0}{2}{0}]", _
                Chr(34), CUR, DeliveryPart))

        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Public Function QTYPrice(ByRef Cur As XmlNode, ByVal qty As Integer) As Double
        Dim ret As Double
        For Each Pricebreak As XmlNode In Cur.SelectNodes("BREAK")
            If qty >= CInt(Pricebreak.Attributes("QTY").InnerText) Then
                ret = CDbl(Pricebreak.Attributes("PRICE").InnerText)
            End If
        Next
        Return ret
    End Function

    Public Function DeliveryPrice(ByRef doc As XmlDocument, ByRef cart As Cart, ByVal CURRENCY As String, ByVal DELIVERYPART As String) As Double

        Dim price As Double = 0
        Dim fam As New Dictionary(Of Integer, DelFam)
        Dim usedFam As New List(Of Integer)
        Dim famprice As New List(Of Double)
        Dim xfam As XmlNodeList = doc.SelectNodes(String.Format( _
                "/*[position()=1]/PARTS/PART[@DELIVERY and PARTNAME={0}{1}{0}]/PRICE/CURRENCY[@CURSTR={0}{2}{0}]/FAMILY", _
            Chr(34), DELIVERYPART, CURRENCY _
                ) _
        )

        For Each f As XmlNode In xfam
            If Not fam.Keys.Contains(f.Attributes("FAMILY").Value) Then
                fam.Add(f.Attributes("FAMILY").Value, New DelFam(CDbl(f.Attributes("PRICE").Value), CDbl(f.Attributes("INCREMENT").Value)))
            Else
                With fam(f.Attributes("FAMILY").Value)
                    .Price = CDbl(f.Attributes("PRICE").Value)
                    .Increment = CDbl(f.Attributes("INCREMENT").Value)
                End With
            End If
        Next

        For Each c As CartItem In cart.CartItems.Values
            price += (c.QTY - 1) * fam(CInt(c.PackFamily)).Increment
            If Not usedFam.Contains(c.PackFamily) Then
                usedFam.Add(c.PackFamily)
                famprice.Add(fam(CInt(c.PackFamily)).Price)
            End If
        Next

        If famprice.Count = 0 Then
            Return 0
        ElseIf DELIVERYPART = cmsData.Settings.Item("FreeDelPart") Then
            If CDbl(cart.Value) >= CDbl(cmsData.Settings.Item("FreeDelMin")) Then
                Return 0
            Else
                Return CDbl(price + famprice.Max)
            End If
        Else
            Return CDbl(price + famprice.Max)
        End If

    End Function

    Public Function PartPage(ByRef doc As XmlDocument, ByVal part As String) As XmlNode
        Dim nodes As XmlNodeList = doc.SelectNodes(String.Format("//page[@part = {0}{1}{0}]", Chr(34), part))
        If nodes.Count = 0 Then
            Return Nothing
        Else
            Return nodes(0)
        End If
    End Function

    Public Function BoxCount(ByVal PartName As String) As Integer
        Dim ret As Integer = 1
        Try
            Dim PagePart As XmlNode = cmsData.doc.SelectSingleNode(String.Format("//page[@part='{0}']", PartName))
            If Not IsNothing(PagePart) Then
                If PagePart.Attributes("boxcount").Value.Length > 0 Then
                    ret = CInt(PagePart.Attributes("boxcount").Value)
                End If
            End If
        Catch
            ret = 1
        End Try
        Return ret

    End Function

End Module

Class DelFam

    Public Sub New(ByVal Price As Double, ByVal Increment As Double)
        _Price = Price
        _Increment = Increment
    End Sub
    Private _Price As Double
    Public Property Price() As Double
        Get
            Return _Price
        End Get
        Set(ByVal value As Double)
            _Price = value
        End Set
    End Property
    Private _Increment As Double
    Public Property Increment() As Double
        Get
            Return _Increment
        End Get
        Set(ByVal value As Double)
            _Increment = value
        End Set
    End Property
End Class