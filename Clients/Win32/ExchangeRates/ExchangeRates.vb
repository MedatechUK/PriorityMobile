Imports System.Xml
Imports System.IO
Imports System.Net

Module ExchangeRates

    Sub Main()
        Dim args As String()
        args = System.Environment.GetCommandLineArgs()
        Dim arg As String
        If UBound(args) = 0 Then
            arg = "C:\load.xml"
        Else
            arg = args(1)
        End If
        Dim url As String = urlBuilder()
        If urlCheck(url) Then
            xmlParse(url, arg)
        Else
            End
        End If
        Exit Sub
    End Sub


    Function urlCheck(ByVal url As String)
        'checks that the feed is online (and the url valid) 
        Try
            Dim request As WebRequest = WebRequest.Create(url)
            Dim response As WebResponse = request.GetResponse()

        Catch ex As Exception
            Return False
        End Try
        Return True
    End Function


    Sub xmlParse(ByVal url As String, ByVal arg As String)
        Dim aPath As String = arg
        Dim xmldoc As XmlDocument = New XmlDocument
        xmldoc.Load(url)
        Dim dctCode As New Dictionary(Of String, String)

        With dctCode
            'currency codes. Currency language 2 in priority
            .Add("XUDLADS", "AUD")
            .Add("XUDLBK22", "CYP")
            .Add("XUDLBK25", "CSK")
            .Add("XUDLBK33", "HUF")
            .Add("XUDLBK36", "LTL")
            .Add("XUDLBK39", "LVL")
            .Add("XUDLBK44", "MTL")
            .Add("XUDLBK47", "PLZ")
            .Add("XUDLBK78", "ILS")
            .Add("XUDLBK83", "MYR")
            .Add("XUDLBK85", "RUB")
            .Add("XUDLBK87", "THB")
            .Add("XUDLBK89", "CNY")
            .Add("XUDLBK93", "KRW")
            .Add("XUDLBK95", "TRL")
            .Add("XUDLBK97", "INR")
            .Add("XUDLCDS", "CAD")
            .Add("XUDLDKS", "DKK")
            .Add("XUDLERS", "EUR")
            .Add("XUDLHDS", "HKD")
            .Add("XUDLJYS", "JPY")
            .Add("XUDLNDS", "NZD")
            .Add("XUDLNKS", "NOK")
            .Add("XUDLSFS", "CHF")
            .Add("XUDLSGS", "SGD")
            .Add("XUDLSKS", "SEK")
            .Add("XUDLSRS", "SAR")
            .Add("XUDLTWS", "TWD")
            .Add("XUDLUSS", "USD")
            .Add("XUDLZRS", "ZAR")
            .Add("NULL", "NULL")
        End With

        Using writeload As XmlWriter = XmlWriter.Create(aPath)
            With writeload
                .WriteStartDocument()
                .WriteStartElement("Load")


                'iterates through xml document's elements
                Dim aSwitch As Boolean = False
                For Each Element As XmlElement In xmldoc.SelectNodes("//*")


                    'Fills array with appropriate values
                    Dim Values() As String = getXmlData(Element)
                    If Not Values Is Nothing Then
                        If aSwitch = False Then
                            'write upper
                            .WriteStartElement("UpperLevelRow")
                            .WriteAttributeString("DATE", Values(2))
                        End If
                        'write lower
                        .WriteStartElement("LowerLevelRow")
                        .WriteAttributeString("CURRENCY", dctCode(Values(0)))
                        .WriteAttributeString("EXCHANGE", Math.Round(1 / CDbl(Values(1)), 4))
                        .WriteAttributeString("CROSSEXCHANGE", Values(1))
                        .WriteEndElement()

                        aSwitch = True
                    End If

                Next
                .WriteEndElement()
                .WriteEndElement()
                .Flush()
                .Close()
            End With
        End Using

    End Sub

    Function getXmlData(ByVal element As XmlElement)
        'handles odd BOE xml element structure, taking SCODE value, and child element's TIME & OBS_VALUE attributes. 
        Dim Values() As String = {"", "", ""}
        If element.HasAttribute("SCODE") Then

            If element.HasChildNodes Then
                Values(0) = element.Attributes.ItemOf("SCODE").Value
                Dim Child As XmlElement = element.FirstChild
                If Child.HasAttribute("TIME") Then

                    Values(1) = Child.Attributes.ItemOf("OBS_VALUE").Value
                    Values(2) = ( _
              CDate(Child.Attributes.ItemOf("TIME").Value)) _
                                               .ToString("dd/MM/yyyy") 'formats date
                End If
            End If
            Return Values
        Else : Return Nothing 'no update condition
        End If
    End Function


    Function urlBuilder()
        Dim months() As String = {"jan", "feb", "mar", "apr", "may", "jun", _
                                  "jul", "aug", "sep", "oct", "nov", "dec"}
        Dim url As String = "http://www.bankofengland.co.uk/boeapps/iadb/fromshowcolumns.asp?CodeVer=new&xml.x=yes"
        'sorts the date out so the feed retrieves updates no matter when the program is run.
        Dim day As Integer = DateTime.Now.DayOfWeek

        Select Case day
            Case 1
                day = DateTime.Now.Day - 2
            Case 7
                day = DateTime.Now.Day - 1
            Case Else
                day = DateTime.Now.Day
        End Select

        If DateTime.Now.Hour < 10 Then
            day = DateTime.Now.Day - 1
        End If

        url = url & String.Format("&datefrom={0}/{1}/{2}", day - 1, months(DateTime.Now.Month - 1), DateTime.Now.Year)
        url = url & String.Format("&dateto={0}/{1}/{2}", day, months(DateTime.Now.Month - 1), DateTime.Now.Year)
        'data series codes:
        url = url & "&SeriesCodes=XUDLADS,XUDLBK22,XUDLBK25,XUDLBK33,XUDLBK36,XUDLBK39,XUDLBK44,XUDLBK47,XUDLBK78,"
        'retrieves currency codes. For new ones, add to dictionary.
        url = url & "XUDLBK83,XUDLBK85,XUDLBK87,XUDLBK89,XUDLBK93,XUDLBK95,XUDLBK97,XUDLCDS,XUDLDKS,XUDLERS,"
        url = url & "XUDLHDS,XUDLJYS,XUDLNDS,XUDLNKS,XUDLSFS,XUDLSGS,XUDLSKS,XUDLSRS,XUDLTWS,XUDLUSS,XUDLZRS"

        'omits irrelevant garbage: 
        url = url & "&Omit=-A-B-C-D-E-F"
        Return url
    End Function

End Module
