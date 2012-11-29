'
'Pay and Shop Limited (payandshop.com) - Licence Agreement.
'© Copyright and zero Warranty Notice.
'
'
'Merchants and their internet, call centre, and wireless application
'developers (either in-house or externally appointed partners and
'commercial organisations) may access payandshop.com technical
'references, application programming interfaces (APIs) and other sample
'code and software ("Programs") either free of charge from
'www.payandshop.com or by emailing info@payandshop.com. 
'
'payandshop.com provides the programs "as is" without any warranty of
'any kind, either expressed or implied, including, but not limited to,
'the implied warranties of merchantability and fitness for a particular
'purpose. The entire risk as to the quality and performance of the
'programs is with the merchant and/or the application development
'company involved. Should the programs prove defective, the merchant
'and/or the application development company assumes the cost of all
'necessary servicing, repair or correction.
'
'Copyright remains with payandshop.com, and as such any copyright
'notices in the code are not to be removed. The software is provided as
'sample code to assist internet, wireless and call center application
'development companies integrate with the payandshop.com service.
'
'Any Programs licensed by Pay and Shop to merchants or developers are
'licensed on a non-exclusive basis solely for the purpose of availing
'of the Pay and Shop payment solution service in accordance with the
'written instructions of an authorised representative of Pay and Shop
'Limited. Any other use is strictly prohibited.
'


Imports System.Collections.Generic
Imports System.Text

Imports System.Xml

Namespace RealexPayments

    Namespace RealAuth

        Public Class CreditCard

            Public Const CVN_PRESENT As Integer = 1
            Public Const CVN_ILLEGIBLE As Integer = 2
            Public Const CVN_NOT_REQUESTED_BY_MERCHANT As Integer = 3
            Public Const CVN_NOT_ON_CARD As Integer = 4

            Public Property CardNumber() As [String]
                Get
                    If IsNothing(m_number) Then
                        Return ""
                    Else
                        Return (m_number)
                    End If
                End Get

                Set(ByVal value As [String])
                    If (value.Length >= 12) AndAlso (value.Length <= 19) Then
                        m_number = value
                    Else
                        Throw New DataValidationException("Card number fails Luhn check.")
                    End If
                End Set
            End Property

            Public Property CardType() As [String]
                Get
                    Return (m_cctype)
                End Get

                Set(ByVal value As [String])
                    If value.Equals("") Then
                        Throw New DataValidationException("Card type must not be blank.")
                    End If

                    value = value.ToUpper()

                    Select Case value
                        Case ("VISA"), ("MC"), ("AMEX"), ("LASER"), ("DINERS"), ("SWITCH"), _
                         ("SOLO"), ("JCB")
                            m_cctype = value
                            Exit Select
                        Case Else
                            Throw New DataValidationException("Invalid credit card type specified.")
                    End Select
                End Set
            End Property

            Public Property IssueNumber() As Integer
                Get
                    Return (m_issueNumber)
                End Get

                Set(ByVal value As Integer)
                    If Not m_cctype.Equals("SWITCH") Then
                        Throw New DataValidationException("Issue numbers are only used by Switch cards.")
                    End If

                    m_issueNumber = value
                End Set
            End Property

            Public Property ExpiryDate() As [String]
                Get
                    Return (m_expiryDate)
                End Get

                Set(ByVal value As [String])
                    If value.Length <> 4 Then
                        Throw New DataValidationException("Card expiry length is incorrect (must be exactly 4 digits).")
                    End If

                    Dim sMonth As [String] = value.Substring(0, 2)
                    Dim sYear As [String] = value.Substring(2, 2)
                    Dim iMonth As Integer = Int16.Parse(sMonth)
                    Dim iYear As Integer = Int16.Parse(sYear)
                    Dim currentYear As Integer = New DateTime().Year Mod 100

                    If Not ((iMonth >= 1) AndAlso (iMonth <= 12)) Then
                        Throw New DataValidationException("Card expiry month must be between 01 and 12 inclusive.")
                    End If

                    If iYear < (currentYear - 1) Then
                        ' refunds can be made to expired cards
                        Throw New DataValidationException("Card expiry year is too far into the past.")
                    End If

                    If iYear > (currentYear + 20) Then
                        Throw New DataValidationException("Card expiry year is too far into the future.")
                    End If

                    m_expiryDate = value
                End Set
            End Property

            Public Property CardholderName() As [String]
                Get
                    Return (m_cardholderName)
                End Get

                Set(ByVal value As [String])
                    If value.Equals("") Then
                        Throw New DataValidationException("Cardholder name must not be empty.")
                    End If

                    m_cardholderName = value
                End Set
            End Property

            Public Property CVN() As [String]
                Get
                    Return (m_cvn)
                End Get

                Set(ByVal value As [String])
                    If (value.Length <> 3) AndAlso (value.Length <> 4) Then
                        Throw New DataValidationException("CVN must be 3 or 4 digits in length.")
                    End If

                    m_cvn = value
                End Set
            End Property

            Public Property CVNPresent() As Integer
                Get
                    Return (m_cvnPresent)
                End Get

                Set(ByVal value As Integer)
                    If Not (value >= 1) AndAlso (value <= 4) Then
                        Throw New DataValidationException("Invalid CVN status. Please use the defined constants.")
                    End If

                    m_cvnPresent = value
                End Set
            End Property

            Private m_cctype As [String]
            Private m_number As [String]
            Private m_issueNumber As Integer
            ' Switch cards only
            Private m_expiryDate As [String]
            Private m_cardholderName As [String]
            Private m_cvn As [String]
            Private m_cvnPresent As Integer


            Public Sub New(ByVal cctype As [String], ByVal number As [String], ByVal expiryDate__1 As [String], ByVal cardholderName__2 As [String], ByVal cvn__3 As [String], ByVal cvnPresent__4 As Integer)
                CardType = cctype
                CardNumber = number
                ExpiryDate = expiryDate__1
                CardholderName = cardholderName__2
                CVN = cvn__3
                CVNPresent = cvnPresent__4
            End Sub

            Public Sub New(ByVal cctype As [String], ByVal number As [String], ByVal expiryDate__1 As [String], ByVal cardholderName__2 As [String], ByVal cvn__3 As [String], ByVal cvnPresent__4 As Integer, _
             ByVal issueNumber__5 As Integer)
                CardType = cctype
                CardNumber = number
                ExpiryDate = expiryDate__1
                CardholderName = cardholderName__2
                CVN = cvn__3
                CVNPresent = cvnPresent__4
                IssueNumber = issueNumber__5
            End Sub

            Public Sub WriteXML(ByVal xml As XmlWriter)

                xml.WriteStartElement("card")
                If True Then
                    xml.WriteElementString("number", m_number)
                    xml.WriteElementString("expdate", m_expiryDate)
                    xml.WriteElementString("type", m_cctype)
                    xml.WriteElementString("chname", m_cardholderName)
                    If m_cctype.Equals("SWITCH") Then
                        xml.WriteElementString("issueno", m_issueNumber.ToString())
                    End If
                    xml.WriteStartElement("cvn")
                    If True Then
                        xml.WriteElementString("number", m_cvn)
                        xml.WriteElementString("presind", m_cvnPresent.ToString())
                    End If
                    xml.WriteEndElement()
                End If
                xml.WriteEndElement()
            End Sub

        End Class

    End Namespace

End Namespace
