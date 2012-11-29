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

        Public Class TransactionResponse

            Private m_resultCode As Integer
            Private m_resultMessage As [String]
            Private m_resultAuthCode As [String]
            Private m_resultPASRef As [String]
            Private m_resultOrderID As [String]
            Private m_resultSuitabilityScore As Integer
            Private m_resultSuitabilityScoreCheck As Dictionary(Of Integer, Integer)

            'TODO: if you have sent Realex additional variables and would like to retrieve them:
            'private String m_resultMyInterestingVariableName;

            Public ReadOnly Property ResultCode() As Integer
                Get
                    Return (m_resultCode)
                End Get
            End Property

            Public ReadOnly Property ResultMessage() As [String]
                Get
                    Return (m_resultMessage)
                End Get
            End Property

            Public ReadOnly Property ResultAuthCode() As [String]
                Get
                    Return (m_resultAuthCode)
                End Get
            End Property

            Public ReadOnly Property ResultPASRef() As [String]
                Get
                    Return (m_resultPASRef)
                End Get
            End Property

            Public ReadOnly Property ResultOrderID() As [String]
                Get
                    Return (m_resultOrderID)
                End Get
            End Property

            Public ReadOnly Property ResultSuitabilityScore() As Integer
                Get
                    Return (m_resultSuitabilityScore)
                End Get
            End Property

            Public Function ResultSuitabilityScoreCheck(ByVal checkID As Integer) As Integer
                Return (m_resultSuitabilityScoreCheck(checkID))
            End Function

            'TODO: if you have sent Realex additional variables and would like to retrieve them:
            '
            '            public String ResultMyInterestingVariableName {
            '                get {
            '                    return (m_resultMyInterestingVariableName);
            '                }
            '                set {
            '                    m_resultMyInterestingVariableName = value;
            '                }
            '            }
            '            


            Public Sub New(ByVal responseXML As [String])

                m_resultSuitabilityScoreCheck = New Dictionary(Of Integer, Integer)()

                Dim xml As New XmlDocument()
                xml.LoadXml(responseXML)

                Try

                    ' these *must* exist
                    m_resultCode = Convert.ToInt32(xml.GetElementsByTagName("result")(0).InnerText)
                    m_resultMessage = xml.GetElementsByTagName("message")(0).InnerText

                    ' these should exist, but don't throw exceptions if they don't.
                    Dim el As XmlNode
                    el = xml.GetElementsByTagName("pasref")(0)
                    m_resultPASRef = If((el IsNot Nothing), el.InnerText, "")

                    el = xml.GetElementsByTagName("authcode")(0)
                    m_resultAuthCode = If((el IsNot Nothing), el.InnerText, "")

                    el = xml.GetElementsByTagName("orderid")(0)
                    m_resultOrderID = If((el IsNot Nothing), el.InnerText, "")

                    el = xml.GetElementsByTagName("tss")(0)
                    If el IsNot Nothing Then
                        For Each node As XmlNode In el.ChildNodes
                            Select Case node.Name
                                Case ("result")
                                    m_resultSuitabilityScore = Convert.ToInt32(node.InnerText)
                                    Exit Select
                                Case ("check")
                                    For Each attr As XmlAttribute In node.Attributes
                                        If attr.Name = "id" Then
                                            m_resultSuitabilityScoreCheck.Add(Convert.ToInt32(attr.InnerText), Convert.ToInt32(node.InnerText))
                                        End If
                                    Next
                                    Exit Select
                            End Select
                        Next

                        'TODO: if you have sent Realex additional variables and would like to retrieve them:
                        '
                        '                    el = xml.GetElementsByTagName("MyInterestingVariable")[0];
                        '                    if (el != null) {
                        '                        m_resultMyInterestingVariableName = el.InnerText;
                        '                    }
                        '                    

                    End If
                Catch e As NullReferenceException
                    Throw New TransactionFailedException("Error parsing XML response: mandatory fields not present. " & e.Message)
                End Try
            End Sub

        End Class
    End Namespace
End Namespace
