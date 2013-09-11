<%@ WebHandler Language="VB" Class="postxml" %>

Imports System
Imports System.Web
Imports System.Xml
Imports System.io
Imports priority.v3Loading
Imports priority.SerialData

Public Class postxml : Implements IHttpHandler
    
    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest

        Dim StatusCode As Integer = 200
        Dim StatusMessage As String = "Ok"
        Dim thisRequest As New XmlDocument
        Dim ws As New PriWebSVC.Service
        Dim erl As New priority.v3Loading
        
        With context
            
            Dim reader As New StreamReader(.Request.InputStream)
            Try
                With thisRequest
                    .LoadXml(reader.ReadToEnd)
                    Dim nodes As Xml.XmlNodeList = .SelectNodes("//order_post")
                    For Each node As XmlNode In nodes

                        With erl
  
                            .Clear()
                            .Table = "ZTRX_LOS_ORDERS"
                            .Procedure = "ZTRX_LOAD_LOS_NAD"
                            .Environment = "tru"
                            
                            Try
                                If Not IsNothing(node.Attributes("test")) Then
                                    .Environment = "trucpy1"
                                    .Procedure = "ZSFDC_TEST"
                                Else
                                    Throw New Exception("save")
                                End If
                            Catch
                                Using xmwr As New StreamWriter(context.Server.MapPath("\") & "orders\" & Trim(node.SelectSingleNode("order_id").InnerText()) & ".xml", Nothing)
                                    xmwr.Write(node.OuterXml)
                                End Using
                            End Try                        
                                                       
                            .RecordType1 = "CUSTOMER_ID,ORDER_ID,ORDER_DATE,TYPECODE,ZTRX_SINGLE_SHIP,REFERENCE,ORDERSOURCE,PHONE,EMAIL"
                            .RecordType2 = "SKU,QTY,UNIT_PRICE,DELIVERY_DATE,PARTDES," & _
                                "ADDRESS_1,ADDRESS_2,ADDRESS_3,ADDRESS_4,ADDRESS_5,ADDRESS_POSTCODE,ACPACK," & _
                                "TRANS,AUTHCODE,AMOUNT," & _
                                "LEANAME," & _
                                "RT," & _
                                "BILLADD_1,BILLADD_2,BILLADD_3,BILLADD_4,BILLADD_5,BILLADD_POSTCODE,BILLPHONE,BILLEMAIL"
                            .RecordTypes = "TEXT,TEXT,,TEXT,TEXT,TEXT,TEXT,TEXT,TEXT," & _
                                "TEXT,,,TEXT,TEXT," & _
                                "TEXT,TEXT,TEXT,TEXT,TEXT,TEXT,TEXT," & _
                                "TEXT,TEXT,," & _
                                "TEXT," & _
                                "TEXT," & _
                                "TEXT,TEXT,TEXT,TEXT,TEXT,TEXT,TEXT,TEXT"
                            
                            Dim ZTRX_SINGLE_SHIP As String = PresentValue(node, "SINGLE_SHIP")
                            Dim ZTRX_TYPECODE As String = PresentValue(node, "TYPECODE", "WWW")
                            Dim ZTRX_REFERENCE As String = PresentValue(node, "REFERENCE")
                            Dim ZTRX_ORDERSOURCE As String = PresentValue(node, "ORDERSOURCE")
                            Dim ACPACK As String = PresentValue(node, "ACPACK")
                            Dim customer_id As String = PresentValue(node, "customer_id")
                            Dim PHONE As String = PresentValue(node, "PHONE")
                            Dim EMAIL As String = PresentValue(node, "EMAIL")
                            Dim LEANAME As String = PresentValue(node, "LEANAME")
                            
                            Dim t1() As String = { _
                                    customer_id, _
                                    node.SelectSingleNode("order_id").InnerText(), _
                                    DateDiff(DateInterval.Minute, #1/1/1988#, Now).ToString(), _
                                    ZTRX_TYPECODE, _
                                    ZTRX_SINGLE_SHIP, _
                                    ZTRX_REFERENCE, _
                                    ZTRX_ORDERSOURCE, _
                                    PHONE, _
                                    EMAIL _
                                    }
                            .AddRecord(1) = t1

                            Dim lines As Xml.XmlNodeList = node.SelectSingleNode("lines").SelectNodes("//line")
                            For Each line As XmlNode In lines
                                
                                Dim PARTDES As String = PresentValue(line, "partdes")
                                
                                Dim t2() As String = { _
                                    line.SelectSingleNode("sku").InnerText(), _
                                    CStr(CInt(line.SelectSingleNode("qty").InnerText()) * 1000), _
                                    line.SelectSingleNode("unit_price").InnerText(), _
                                    ConvertDate(line.SelectSingleNode("delivery_date").InnerText()), _
                                    PARTDES, _
                                    "", _
                                    "", _
                                    "", _
                                    "", _
                                    "", _
                                    "", _
                                    "", _
                                    "", _
                                    "", _
                                    "0", _
                                    "", _
                                    "2", _
                                    "", _
                                    "", _
                                    "", _
                                    "", _
                                    "", _
                                    "", _
                                    "", _
                                    "" _
                                }
                                .AddRecord(2) = t2
                            Next
                            
                            Dim postcode As String = PresentValue(node, "delivery_address_postcode")

                            Dim t3() As String = { _
                                    "", _
                                    "0", _
                                    "0", _
                                    "", _
                                    "", _
                                    node.SelectSingleNode("delivery_address_1").InnerText(), _
                                    node.SelectSingleNode("delivery_address_2").InnerText(), _
                                    node.SelectSingleNode("delivery_address_3").InnerText(), _
                                    node.SelectSingleNode("delivery_address_4").InnerText(), _
                                    node.SelectSingleNode("delivery_address_5").InnerText(), _
                                    postcode, _
                                    ACPACK, _
                                    "", _
                                    "", _
                                    "0", _
                                    "", _
                                    "3", _
                                    "", _
                                    "", _
                                    "", _
                                    "", _
                                    "", _
                                    "", _
                                    "", _
                                    "" _
                            }
                            .AddRecord(2) = t3
                            
                            Dim payment As XmlNode = node.SelectSingleNode("payment")
                            If Not IsNothing(payment) Then
                                Dim t4() As String = { _
                                        "", _
                                        "0", _
                                        "0", _
                                        "", _
                                        "", _
                                        "", _
                                        "", _
                                        "", _
                                        "", _
                                        "", _
                                        "", _
                                        "", _
                                        payment.SelectSingleNode("trans").InnerText, _
                                        payment.SelectSingleNode("authcode").InnerText, _
                                        payment.SelectSingleNode("amount").InnerText, _
                                        "", _
                                        "4", _
                                        "", _
                                        "", _
                                        "", _
                                        "", _
                                        "", _
                                        "", _
                                        "", _
                                        "" _
                                }
                                .AddRecord(2) = t4
                            End If
                            
                            If LEANAME.Length > 0 Then
                                Dim t5() As String = { _
                                    "", _
                                    "0", _
                                    "0", _
                                    "", _
                                    "", _
                                    "", _
                                    "", _
                                    "", _
                                    "", _
                                    "", _
                                    "", _
                                    "", _
                                    "", _
                                    "", _
                                    "0", _
                                    LEANAME, _
                                    "5", _
                                    "", _
                                    "", _
                                    "", _
                                    "", _
                                    "", _
                                    "", _
                                    "", _
                                    "" _
                                }
                                .AddRecord(2) = t5
                            End If
                            
                            Dim bill1 As String
                            Dim bill2 As String
                            Dim bill3 As String
                            Dim bill4 As String
                            Dim bill5 As String
                            Dim bill_postcode As String
                            Dim bill_phone As String
                            Dim bill_email As String
                            
                            Dim billNode As XmlNode = node.SelectSingleNode("billing_address")
                            If Not IsNothing(billNode) Then
                                bill1 = PresentValue(billNode, "address_1")
                                bill2 = PresentValue(billNode, "address_2")
                                bill3 = PresentValue(billNode, "address_3")
                                bill4 = PresentValue(billNode, "address_4")
                                bill5 = PresentValue(billNode, "address_5")
                                bill_postcode = PresentValue(billNode, "postcode")
                                bill_phone = PresentValue(billNode, "phone")
                                bill_email = PresentValue(billNode, "email")
                            Else
                                bill1 = PresentValue(node, "delivery_address_1")
                                bill2 = PresentValue(node, "delivery_address_2")
                                bill3 = PresentValue(node, "delivery_address_3")
                                bill4 = PresentValue(node, "delivery_address_4")
                                bill5 = PresentValue(node, "delivery_address_5")
                                bill_postcode = PresentValue(node, "delivery_address_postcode")
                                bill_phone = PresentValue(node, "PHONE")
                                bill_email = PresentValue(node, "EMAIL")
                            End If
                            
                            Dim t6() As String = { _
                                "", _
                                "0", _
                                "0", _
                                "", _
                                "", _
                                "", _
                                "", _
                                "", _
                                "", _
                                "", _
                                "", _
                                "", _
                                "", _
                                "", _
                                "0", _
                                "", _
                                "6", _
                                bill1, _
                                bill2, _
                                bill3, _
                                bill4, _
                                bill5, _
                                bill_postcode, _
                                bill_phone, _
                                bill_email _
                            }
                            .AddRecord(2) = t6                            
                                                             
                            If Not ws.LoadData(.ToSerial) Then _
                                Throw New Exception("Data loading failed. Please see the Windows event log for more information.")

                        End With
                        
                    Next
                End With
            Catch ex As Exception
                StatusMessage = ex.Message
                StatusCode = 400
            Finally
                With reader
                    .Close()
                    .Dispose()
                End With
            End Try
            
            With .Response
                .Clear()
                .ContentType = "text/xml"
                .ContentEncoding = Encoding.UTF8
                Dim objX As New XmlTextWriter(context.Response.OutputStream, Nothing)
                With objX
                    .WriteStartDocument()
                    .WriteStartElement("response")
                    .WriteAttributeString("status", CStr(StatusCode))
                    .WriteAttributeString("message", StatusMessage)
                    .WriteEndElement() 'End Settings 
                    .WriteEndDocument()
                    .Flush()
                    .Close()
                End With
                .End()
            End With        
                
        End With
        
    End Sub
    
    Private Function ConvertDate(ByVal s As String) As String        
        Dim dt As New Date(CInt(Split(s, "-")(0)), CInt(Split(s, "-")(1)), CInt(Split(s, "-")(2)))
        Return dt.ToString("dd/MM/yy")
    End Function
 
    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property
    
    Private Function PresentValue(ByRef Node As XmlNode, ByVal Nodename As String, Optional ByVal DefaultStr As String = Nothing) As String
        Dim ret As String = ""
        Try
            ret = Node.SelectSingleNode(Nodename).InnerText()
        Catch
            If Not IsNothing(DefaultStr) Then ret = DefaultStr
        End Try
        Return ret
    End Function

End Class