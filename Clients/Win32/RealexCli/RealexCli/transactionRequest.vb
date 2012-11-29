Imports System.Collections.Generic
Imports System.Collections
Imports System.Text

Imports System.Security.Cryptography
Imports System.Xml
Imports System.Net
Imports System.IO

Namespace RealexPayments

    Namespace RealAuth

        Public Class TransactionRequest


#Region "Private variables"

            ' used in every transaction
            Private m_normalPassword As [String]
            Private m_rebatePassword As [String]
            Private m_refundPassword As [String]

            ' used by all transaction types
            Private m_transType As [String]
            Private m_transTimestamp As [String]
            Private m_transMerchantName As [String]
            Private m_transAccountName As [String]
            Private m_transOrderID As [String]

            Private m_transSHA1Hash As [String]
            Private m_transComments As ArrayList
            Private m_transBillingAddressCode As [String]
            Private m_transBillingAddressCountry As [String]
            Private m_transShippingAddressCode As [String]
            Private m_transShippingAddressCountry As [String]
            Private m_transCustomerNumber As [String]
            Private m_transVariableReference As [String]
            Private m_transProductID As [String]

            ' used by *some* transaction types
            Private m_transAmount As UInteger
            Private m_transCurrency As [String]
            Private m_transCard As CreditCard
            Private m_transAutoSettle As Integer
            Private m_transPASRef As [String]
            Private m_transAuthCode As [String]

            'TODO: add any additional instance variables you would like to send to Realex here
            'private String m_transMyInterestingVariableName;

#End Region

#Region "Public Properties"

            ' public properties reprenting the protected vars above
            Public Property TransType() As [String]
                Get
                    Return (m_transType)
                End Get

                Set(ByVal value As [String])
                    value = value.ToLower()
                    Select Case value

                        Case ("auth"), ("void"), ("settle"), ("credit"), ("rebate"), ("tss"), _
                         ("offline")
                            m_transType = value
                            Exit Select

                        Case ("magicnewtransactiontype")
                            Throw New DataValidationException("Transaction type " & value & "not yet implemented.")
                        Case Else

                            Throw New DataValidationException("Unknown transaction type requested.")
                    End Select
                End Set
            End Property

            Public Property TransTimestamp() As [String]
                Get
                    Return (m_transTimestamp)
                End Get

                Set(ByVal value As [String])
                    Throw New ReadOnlyException("This property is read-only.")
                End Set
            End Property

            Public Property TransMerchantName() As [String]
                Get
                    Return (m_transMerchantName)
                End Get

                Set(ByVal value As [String])
                    m_transMerchantName = value
                End Set
            End Property

            Public Property TransAccountName() As [String]
                Get
                    Return (m_transAccountName)
                End Get

                Set(ByVal value As [String])
                    m_transAccountName = value
                End Set
            End Property

            Public Property TransOrderID() As [String]
                Get
                    Return (m_transOrderID)
                End Get

                Set(ByVal value As [String])
                    m_transOrderID = value
                End Set
            End Property

            Public Property TransComments() As ArrayList
                Get
                    Return (m_transComments)
                End Get

                'FIXME: how should this be done?
                Set(ByVal value As ArrayList)
                End Set
            End Property

            Public Property TransBillingAddressCode() As [String]
                Get
                    Return (m_transBillingAddressCode)
                End Get

                Set(ByVal value As [String])
                    m_transBillingAddressCode = value
                End Set
            End Property

            Public Property TransBillingAddressCountry() As [String]
                Get
                    Return (m_transBillingAddressCountry)
                End Get

                Set(ByVal value As [String])
                    value = value.ToUpper()
                    Validator.assertAlphaStrict("Billing Country", value)
                    Validator.assertLength("Billing Country", value, 2)
                    m_transBillingAddressCountry = value
                End Set
            End Property

            Public Property TransShippingAddressCode() As [String]
                Get
                    Return (m_transShippingAddressCode)
                End Get

                Set(ByVal value As [String])
                    m_transShippingAddressCode = value
                End Set
            End Property

            Public Property TransShippingAddressCountry() As [String]
                Get
                    Return (m_transShippingAddressCountry)
                End Get

                Set(ByVal value As [String])
                    value = value.ToUpper()
                    Validator.assertAlphaStrict("Shipping Country", value)
                    Validator.assertLength("Shipping Country", value, 2)
                    m_transShippingAddressCountry = value
                End Set
            End Property

            Public Property TransCustomerNumber() As [String]
                Get
                    Return (m_transCustomerNumber)
                End Get

                Set(ByVal value As [String])
                    Validator.assertAlphaNumericLoose("Customer Number", value)
                    m_transCustomerNumber = value
                End Set
            End Property

            Public Property TransVariableReference() As [String]
                Get
                    Return (m_transVariableReference)
                End Get

                Set(ByVal value As [String])
                    Validator.assertAlphaNumericLoose("Variable Reference", value)
                    m_transVariableReference = value
                End Set
            End Property

            Public Property TransProductID() As [String]
                Get
                    Return (m_transProductID)
                End Get

                Set(ByVal value As [String])
                    Validator.assertAlphaNumericLoose("Product ID", value)
                    m_transProductID = value
                End Set
            End Property


            ' used by *some* transaction types
            Public Property TransAmount() As UInteger
                Get
                    Return (m_transAmount)
                End Get

                Set(ByVal value As UInteger)
                    m_transAmount = value
                End Set
            End Property

            Public Property TransCurrency() As [String]
                Get
                    Return (m_transCurrency)
                End Get

                Set(ByVal value As [String])
                    m_transCurrency = value
                End Set
            End Property

            Public Property TransCard() As CreditCard
                Get
                    Return (m_transCard)
                End Get

                Set(ByVal value As CreditCard)
                    m_transCard = value
                End Set
            End Property

            Public Property TransAutoSettle() As Integer
                Get
                    Return (m_transAutoSettle)
                End Get

                Set(ByVal value As Integer)
                    m_transAutoSettle = value
                End Set
            End Property

            Public Property TransPASRef() As [String]
                Get
                    Return (m_transPASRef)
                End Get

                Set(ByVal value As [String])
                    m_transPASRef = value
                End Set
            End Property

            Public Property TransAuthCode() As [String]
                Get
                    Return (m_transAuthCode)
                End Get

                Set(ByVal value As [String])
                    m_transAuthCode = value
                End Set
            End Property

            'TODO: Add your property handler(s) for your own variables here
            '
            '            public String TransMyInterestingVariableName {
            '                get {
            '                    return (m_transMyInterestingVariableName);
            '                }
            '                set {
            '                    m_transMyInterestingVariableName = value;
            '                }
            '            }
            '            

#End Region

#Region "Initialisation and finalisation"

            ' Constructor(s)
            Public Sub New(ByVal merchantName As String, ByVal normalPassword As [String], ByVal rebatePassword As [String], ByVal refundPassword As [String])

                m_transComments = New ArrayList()

                m_transMerchantName = merchantName
                m_normalPassword = normalPassword
                m_rebatePassword = rebatePassword
                m_refundPassword = refundPassword
                m_transAutoSettle = 1
            End Sub

#End Region

#Region "Private methods"

            Private Sub generateTimestamp()
                m_transTimestamp = DateTime.Now.ToString("yyyyMMddHHmmss")
            End Sub

            Private Function hexEncode(ByVal data As Byte()) As [String]

                Dim result As [String] = ""
                For Each b As Byte In data
                    result += b.ToString("X2")
                Next
                result = result.ToLower()

                Return (result)
            End Function

            Private Sub generateSHA1Hash()

                Dim sha As SHA1 = New SHA1Managed()

                Dim hashInput As [String] = ""
                hashInput += m_transTimestamp
                hashInput += "."
                hashInput += m_transMerchantName
                hashInput += "."
                hashInput += m_transOrderID
                hashInput += "."
                hashInput += CStr(m_transAmount)
                hashInput += "."
                hashInput += m_transCurrency
                hashInput += "."
                If Not IsNothing(m_transCard) Then
                    hashInput += Convert.ToString(m_transCard.CardNumber)
                Else
                    hashInput += ""
                End If
                If debug Then
                    Console.WriteLine("")
                    Console.WriteLine("Hashing ...")
                    Console.WriteLine("Hash Input: {0}", hashInput)
                End If
                Dim hashStage1 As [String] = hexEncode(sha.ComputeHash(Encoding.UTF8.GetBytes(hashInput))) & "." & m_normalPassword
                If debug Then Console.WriteLine("hashStage1: {0}", hashStage1)

                Dim hashStage2 As [String] = hexEncode(sha.ComputeHash(Encoding.UTF8.GetBytes(hashStage1)))
                If debug Then Console.WriteLine("hashStage2: {0}", hashStage2)
                If debug Then Console.WriteLine("")
                m_transSHA1Hash = hashStage2
            End Sub

            Protected Function ToXML() As [String]

                generateTimestamp()
                ' timestamp the request as it's generated
                generateSHA1Hash()
                ' ... and ensure that we have a correct hash
                ' NOTE: element variable names are named in the XML case, not in camel case, so as to
                ' avoid confusion in mapping between the two.

                Dim xmlSettings As New XmlWriterSettings()
                xmlSettings.Indent = True
                xmlSettings.NewLineOnAttributes = False
                xmlSettings.NewLineChars = vbCr & vbLf
                xmlSettings.CloseOutput = True

                Dim strBuilder As New StringBuilder()

                Dim xml As XmlWriter = XmlWriter.Create(strBuilder, xmlSettings)

                xml.WriteStartDocument()

                xml.WriteStartElement("request")
                If True Then
                    xml.WriteAttributeString("timestamp", m_transTimestamp)
                    xml.WriteAttributeString("type", m_transType)

                    xml.WriteElementString("merchantid", m_transMerchantName)
                    xml.WriteElementString("account", m_transAccountName)
                    xml.WriteElementString("orderid", m_transOrderID)

                    Select Case m_transType
                        Case ("auth"), ("credit"), ("offline"), ("rebate"), ("tss")

                            xml.WriteStartElement("amount")
                            xml.WriteAttributeString("currency", m_transCurrency)
                            xml.WriteString(m_transAmount.ToString())
                            xml.WriteEndElement()

                            m_transCard.WriteXML(xml)

                            xml.WriteStartElement("autosettle")
                            xml.WriteAttributeString("flag", m_transAutoSettle.ToString())
                            xml.WriteEndElement()
                            Exit Select
                    End Select

                    Select Case m_transType
                        Case ("credit"), ("rebate"), ("settle"), ("void")
                            xml.WriteElementString("pasref", m_transPASRef)
                            Exit Select
                    End Select

                    Select Case m_transType
                        Case ("credit"), ("offline"), ("rebate"), ("settle"), ("void")
                            xml.WriteElementString("authcode", m_transAuthCode)
                            Exit Select
                    End Select

                    xml.WriteElementString("sha1hash", m_transSHA1Hash)

                    ' if this is a transaction requiring an additional hash, include it here
                    Dim sha As SHA1 = New SHA1Managed()
                    Select Case m_transType
                        Case ("credit")
                            Dim refundHash As [String] = hexEncode(sha.ComputeHash(Encoding.UTF8.GetBytes(m_refundPassword)))
                            xml.WriteElementString("refundhash", refundHash)
                            Exit Select
                        Case ("rebate")
                            Dim rebateHash As [String] = hexEncode(sha.ComputeHash(Encoding.UTF8.GetBytes(m_rebatePassword)))
                            xml.WriteElementString("refundhash", rebateHash)
                            ' this is still sent as "refundhash", not "rebatehash"
                            Exit Select
                    End Select

                    xml.WriteStartElement("comments")
                    If True Then
                        Dim iComment As Integer = 1
                        ' this must start from 1, not 0.
                        For Each comment As [String] In m_transComments
                            xml.WriteStartElement("comment")
                            xml.WriteAttributeString("id", iComment.ToString())
                            xml.WriteString(comment)
                            xml.WriteEndElement()

                            iComment += 1
                        Next
                    End If
                    xml.WriteEndElement()

                    xml.WriteStartElement("tssinfo")
                    If True Then
                        If True Then
                            xml.WriteStartElement("address")
                            xml.WriteAttributeString("type", "billing")
                            xml.WriteElementString("code", m_transBillingAddressCode)
                            xml.WriteElementString("country", m_transBillingAddressCountry)
                            xml.WriteEndElement()
                        End If

                        If True Then
                            xml.WriteStartElement("address")
                            xml.WriteAttributeString("type", "shipping")
                            xml.WriteElementString("code", m_transShippingAddressCode)
                            xml.WriteElementString("country", m_transShippingAddressCountry)
                            xml.WriteEndElement()
                        End If

                        If True Then
                            xml.WriteElementString("custnum", m_transCustomerNumber)
                            xml.WriteElementString("varref", m_transVariableReference)
                            xml.WriteElementString("prodid", m_transProductID)
                        End If
                    End If
                    xml.WriteEndElement()
                End If

                'TODO: if you wish to send Realex any additional variables, include them here
                'xml.WriteElementString("MyInterestingVariable", m_myInterestingVariableName);

                xml.WriteEndElement()

                xml.Flush()
                xml.Close()

                Return (strBuilder.ToString())
            End Function

#End Region

#Region "public methods"

            ' Methods and other gack
            Public Sub ContinueTransaction(ByVal transactionResponse As TransactionResponse)

                ' nuke values that definitely won't be in the next transaction
                m_transTimestamp = Nothing
                m_transType = Nothing
                m_transSHA1Hash = Nothing

                TransPASRef = transactionResponse.ResultPASRef
                TransAuthCode = transactionResponse.ResultAuthCode
                TransOrderID = transactionResponse.ResultOrderID
            End Sub

            Public Function Authorize(ByVal transAccount As [String], ByVal transOrderID__1 As [String], ByVal transCurrency__2 As [String], ByVal transAmount__3 As UInteger, ByVal transCard__4 As CreditCard) As TransactionResponse

                TransType = "auth"
                TransAccountName = transAccount
                TransOrderID = transOrderID__1
                TransCurrency = transCurrency__2
                TransAmount = transAmount__3
                TransCard = transCard__4
                TransAutoSettle = 1

                Return (SubmitTransaction())
            End Function

            Public Function Rebate(ByVal transAccount As [String], ByVal transOrderID__1 As [String], ByVal transCurrency__2 As [String], ByVal transAmount__3 As UInteger, ByVal transCard__4 As CreditCard) As TransactionResponse

                TransType = "rebate"
                TransAccountName = transAccount
                TransOrderID = transOrderID__1
                TransCurrency = transCurrency__2
                TransAmount = transAmount__3
                TransCard = transCard__4
                TransAutoSettle = 1

                Return (SubmitTransaction())
            End Function

            Public Function Credit(ByVal transAccount As [String], ByVal transOrderID__1 As [String], ByVal transCurrency__2 As [String], ByVal transAmount__3 As UInteger, ByVal transCard__4 As CreditCard) As TransactionResponse

                TransType = "credit"
                TransAccountName = transAccount
                TransOrderID = transOrderID__1
                TransCurrency = transCurrency__2
                TransAmount = transAmount__3
                TransCard = transCard__4
                TransAutoSettle = 1

                Return (SubmitTransaction())
            End Function

            'Public Function Void(ByVal transAccount As [String], ByVal transOrderID__1 As [String], ByVal transCurrency__2 As [String], ByVal transAmount__3 As UInteger, ByVal transCard__4 As CreditCard) As TransactionResponse

            '    TransType = "void"
            '    TransAccountName = transAccount
            '    TransOrderID = transOrderID__1
            '    TransCurrency = transCurrency__2
            '    TransAmount = transAmount__3
            '    TransCard = transCard__4

            '    Return (SubmitTransaction())
            'End Function

            Public Function Void(ByVal transAccount As [String], ByVal transOrderID__1 As [String], ByVal PASref As String, ByVal Auth As String) As TransactionResponse
                With Me
                    .TransType = "void"
                    .TransAccountName = transAccount
                    .TransOrderID = transOrderID__1
                    .TransPASRef = PASref
                    .TransAuthCode = Auth
                End With

                Return (SubmitTransaction())
            End Function

            Public Function TSS(ByVal transAccount As [String], ByVal transOrderID__1 As [String], ByVal transCurrency__2 As [String], ByVal transAmount__3 As UInteger, ByVal transCard__4 As CreditCard) As TransactionResponse

                TransType = "tss"
                TransAccountName = transAccount
                TransOrderID = transOrderID__1
                TransCurrency = transCurrency__2
                TransAmount = transAmount__3
                TransCard = transCard__4

                Return (SubmitTransaction())
            End Function

            Public Function Offline(ByVal transAccount As [String], ByVal transOrderID__1 As [String], ByVal transCurrency__2 As [String], ByVal transAmount__3 As UInteger, ByVal transCard__4 As CreditCard) As TransactionResponse

                TransType = "offline"
                TransAccountName = transAccount
                TransOrderID = transOrderID__1
                TransCurrency = transCurrency__2
                TransAmount = transAmount__3
                TransCard = transCard__4
                TransAutoSettle = 0

                Return (SubmitTransaction())
            End Function

            Public Function SubmitTransaction() As TransactionResponse

                Dim requestXML As [String] = Me.ToXML()
                If debug Then
                    Console.WriteLine("")
                    Console.WriteLine("Sending XML...")
                    Console.Write(requestXML)
                    Console.WriteLine("")
                End If

                Dim wReq As HttpWebRequest = DirectCast(WebRequest.Create("https://epage.payandshop.com/epage-remote.cgi"), HttpWebRequest)
                wReq.ContentType = "text/xml"
                wReq.UserAgent = "eMerge Realex XML Client"
                wReq.Timeout = 45 * 1000
                ' milliseconds
                wReq.AllowAutoRedirect = False
                wReq.ContentLength = requestXML.Length
                wReq.Method = "POST"

                Try
                    Dim sReq As New StreamWriter(wReq.GetRequestStream())
                    sReq.Write(requestXML)
                    sReq.Flush()
                    sReq.Close()

                    ' dump i/o to files for debugging purposes
                    'TODO: if you have trouble with your requests, uncomment the line below to save a copy.
                    ' PLEASE remember to remove the line again before you go live; otherwise you will be keeping
                    ' your customers' credit card data in cleartext on your server.
                    'File.WriteAllText("request.xml", requestXML);

                    Dim wResp As HttpWebResponse = DirectCast(wReq.GetResponse(), HttpWebResponse)
                    Dim sResp As New StreamReader(wResp.GetResponseStream())

                    Dim responseXML As [String] = sResp.ReadToEnd()
                    sResp.Close()

                    ' dump i/o to files for debugging purposes
                    'TODO: if you have trouble with your requests, uncomment the line below to save a copy.
                    ' PLEASE remember to remove the line again before you go live; otherwise you will be keeping
                    ' your customers' credit card data in cleartext on your server.
                    'File.WriteAllText("response.xml", responseXML);

                    If debug Then
                        Console.WriteLine("")
                        Console.WriteLine("Server said...")
                        Console.WriteLine(IndentXMLString(responseXML))
                        Console.WriteLine("")
                    End If
                    Return (New TransactionResponse(responseXML))
                Catch e As WebException
                    Throw New TransactionFailedException("Web request failed or timed out: " & e.Message)
                End Try
            End Function

            Private Function IndentXMLString(ByVal XMLStr As String) As String ' Format XML for reading
                Dim doc As New XmlDocument
                doc.LoadXml(XMLStr)
                Dim ms As New MemoryStream
                Dim xtw As New XmlTextWriter(ms, Encoding.Unicode)
                xtw.Formatting = Formatting.Indented
                doc.WriteContentTo(xtw)
                xtw.Flush()
                ms.Seek(0, SeekOrigin.Begin)
                Using SR As New StreamReader(ms)
                    Return SR.ReadToEnd
                End Using
            End Function
#End Region

        End Class

    End Namespace

End Namespace

