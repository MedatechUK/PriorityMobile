Imports Microsoft.VisualBasic
Imports System.Collections.Generic
Imports System.Xml
Imports System.IO
Imports System.Web
Imports System.Web.UI.WebControls

Public Class Cart

#Region "Public Variables"

    Public CartItems As New Dictionary(Of String, CartItem)

#End Region

#Region "Initialisation"

    Public Sub New()
        MyBase.New()
    End Sub

#End Region

#Region "Public Properties"

    Private _PostGuid As System.Guid = Nothing
    Public Property PostGuid() As System.Guid
        Get
            Return _PostGuid
        End Get
        Set(ByVal value As System.Guid)
            _PostGuid = value
        End Set
    End Property
    Private _CURRENCY As String = "GBP"
    Public Property CURRENCY() As String
        Get
            Return _CURRENCY
        End Get
        Set(ByVal value As String)
            _CURRENCY = value
        End Set
    End Property

    Private _Value As String = "0.00"
    Public ReadOnly Property Value() As String
        Get
            Dim t As Double = 0
            For Each i As CartItem In Me.CartItems.Values
                t += CDbl(i.LINETOTAL)
            Next
            Return FormatDouble(CStr(t))
        End Get
    End Property

    Private _TotalTax As String = "0.00"
    Public ReadOnly Property TotalTax() As String
        Get
            Dim t As Double = 0
            For Each i As CartItem In Me.CartItems.Values
                t += i.QTY * (CDbl(i.SALESTAX / 100) * CDbl(i.PARTPRICE))
            Next
            Return FormatDouble(CStr(t))
        End Get
    End Property

    Private _Total As String = "0.00"
    Public ReadOnly Property Total() As String
        Get
            Dim t As Double = 0
            For Each i As CartItem In Me.CartItems.Values
                t += i.QTY * ((1 + CDbl(i.SALESTAX / 100)) * CDbl(i.PARTPRICE))
            Next
            Return FormatDouble(CStr(t))
        End Get
    End Property

    Public ReadOnly Property PACKFAMILY() As List(Of Integer)
        Get
            Dim t As New List(Of Integer)
            For Each i As CartItem In Me.CartItems.Values
                't += (CInt(i.QTY) * CInt(i.PackSize))
                If Not t.Contains(i.PackFamily) Then
                    t.Add(i.PackFamily)
                End If
            Next
            Return t
        End Get
    End Property

    Private _DELIVERY As New DeliveryItem
    Public Property DELIVERY() As DeliveryItem
        Get
            Return _DELIVERY
        End Get
        Set(ByVal value As DeliveryItem)
            _DELIVERY = value
        End Set
    End Property

    Private _Payment As CardPayment
    Public Property Payment() As CardPayment
        Get
            Return _Payment
        End Get
        Set(ByVal value As CardPayment)
            _Payment = value
        End Set
    End Property

    Public ReadOnly Property chkFreeDel(ByVal FreeDelMin As String) As String
        Get
            If Me.DELIVERY.DELIVERYPART = cmsData.Settings.Item("FreeDelPart") Then
                If Me.Value > cmsData.Settings.Item("FreeDelMin") Then
                    Return 0
                End If
            End If
            Return FreeDelMin
        End Get
    End Property

#End Region

#Region "Private Functions"

    Private Function FormatDouble(ByVal str As String) As String
        If InStr(str, ".") Then
            Return Split(str, ".")(0) & "." & Left(Split(str, ".")(1) & "00", 2)
        Else
            Return str & ".00"
        End If
    End Function

#End Region

#Region "Save Cart"

    Public Function SaveCart() As System.Guid

        _PostGuid = System.Guid.NewGuid

        Dim fn As String = String.Format("{0}orders\{1}.xml", HttpContext.Current.Server.MapPath("\"), _PostGuid.ToString)
        Dim objX As New XmlTextWriter(fn, Nothing)
        With objX
            .WriteStartDocument()
            .WriteStartElement("order_post")
            If CInt(cmsData.Settings.Get("LiveOrders")) = 0 Then .WriteAttributeString("test", "1")
            .WriteElementString("order_id", _PostGuid.ToString)
            .WriteElementString("customer_id", HttpContext.Current.Profile("CUSTNAME"))
            If Not IsNothing(cmsSessions.CurrentSession(HttpContext.Current).cart.Payment) Then
                .WriteStartElement("payment")
                .WriteElementString("trans", cmsSessions.CurrentSession(HttpContext.Current).cart.Payment.trans)
                .WriteElementString("authcode", cmsSessions.CurrentSession(HttpContext.Current).cart.Payment.authcode)
                .WriteElementString("amount", cmsSessions.CurrentSession(HttpContext.Current).cart.Payment.amount)
                .WriteEndElement() 'End Settings 
            End If
            .WriteElementString("delivery_address_1", HttpContext.Current.Profile.GetProfileGroup("Address").Item("Address1"))
            .WriteElementString("delivery_address_2", HttpContext.Current.Profile.GetProfileGroup("Address").Item("Address2"))
            .WriteElementString("delivery_address_3", HttpContext.Current.Profile.GetProfileGroup("Address").Item("Address3"))
            .WriteElementString("delivery_address_4", HttpContext.Current.Profile.GetProfileGroup("Address").Item("Address4"))
            .WriteElementString("delivery_address_5", HttpContext.Current.Profile.GetProfileGroup("Address").Item("Address5"))
            .WriteElementString("delivery_address_postcode", HttpContext.Current.Profile.GetProfileGroup("Address").Item("Postcode"))
            .WriteStartElement("lines")
            For Each ci As CartItem In cmsSessions.CurrentSession(HttpContext.Current).cart.CartItems.Values
                .WriteStartElement("line")
                .WriteElementString("sku", ci.PARTNAME)
                .WriteElementString("qty", ci.QTY)
                .WriteElementString("unit_price", ci.PARTPRICE)
                .WriteElementString("delivery_date", String.Format("{0}-{1}-{2}", Year(Now).ToString, Month(Now).ToString, Day(Now).ToString))
                .WriteEndElement() 'End Settings 
            Next
            .WriteEndElement() 'End Settings 
            .WriteEndElement() 'End Settings 
            .WriteEndDocument()
            .Flush()
            .Close()
        End With
        Return _PostGuid
    End Function

    Public Function PostCart(ByVal Order As System.Guid) As Exception

        Dim result As Exception = Nothing

        Dim xmldata As String
        Using sr As New StreamReader(String.Format("{0}orders\{1}.xml", HttpContext.Current.Server.MapPath("\"), Order.ToString))
            xmldata = sr.ReadToEnd
        End Using

        Dim requestStream As Stream = Nothing
        Dim uploadResponse As Net.HttpWebResponse = Nothing
        Dim myEncoder As New System.Text.ASCIIEncoding
        Dim bytes As Byte() = myEncoder.GetBytes(xmldata)
        Dim ms As MemoryStream = New MemoryStream(bytes)

        Try
            Dim RequestURL As String = cmsData.Settings.Get("OrdersURL")
            Dim uploadRequest As Net.HttpWebRequest = CType(Net.HttpWebRequest.Create(RequestURL), Net.HttpWebRequest)
            uploadRequest.Method = Net.WebRequestMethods.Http.Post
            uploadRequest.Proxy = Nothing
            requestStream = uploadRequest.GetRequestStream()

            ' Upload the XML
            Dim buffer(1024) As Byte
            Dim bytesRead As Integer
            While True
                bytesRead = ms.Read(buffer, 0, buffer.Length)
                If bytesRead = 0 Then
                    Exit While
                End If
                requestStream.Write(buffer, 0, bytesRead)
            End While

            ' The request stream must be closed before getting the response.
            requestStream.Close()

            uploadResponse = uploadRequest.GetResponse()

            Dim thisRequest As New XmlDocument
            With HttpContext.Current
                Dim reader As New StreamReader(uploadResponse.GetResponseStream)
                With thisRequest
                    .LoadXml(reader.ReadToEnd)
                    Dim n As XmlNode = .SelectSingleNode("response")
                    If Not CInt(n.Attributes("status").Value) = 200 Then
                        result = New Exception(n.Attributes("message").Value)
                    End If
                End With
            End With

        Catch ex As UriFormatException
            result = ex
        Catch ex As Net.WebException
            result = ex
        Finally
            If uploadResponse IsNot Nothing Then
                uploadResponse.Close()
            End If
            If requestStream IsNot Nothing Then
                requestStream.Close()
            End If
        End Try

        Return result

    End Function

#End Region

End Class