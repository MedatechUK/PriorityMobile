Imports Microsoft.VisualBasic
Imports priority
Imports System.Xml
Imports System.IO
Imports System.Collections.Generic

Public Class PDALoadings

    Private ws As New PriWebSVC.Service

#Region "Initialisation"

    Public Sub New(ByRef context As HttpContext, ByVal SaveFolder As String)

        Dim reader As StreamReader = Nothing

        With context
            Dim FirstSave As String = String.Format("{0}{1}\{2}.xml", .Server.MapPath("\"), SaveFolder, System.Guid.NewGuid.ToString)
            Try
                reader = New StreamReader(.Request.InputStream)
                _thisRequest.LoadXml(reader.ReadToEnd)
                _thisRequest.Save(FirstSave)
            Catch ex As Exception
                Throw New Exception(String.Format("Failed loading from stream: {0}", ex.Message))
            Finally
                With reader
                    .Close()
                    .Dispose()
                End With
            End Try

            Select Case NodeType
                Case "pdadata"

                    Dim savePath As String = SaveFolder & saveFilename
                    While File.Exists(savePath)
                        File.Delete(savePath)
                    End While
                    File.Move(FirstSave, savePath)

                    LoadSurvey("STA", ThisRequest.SelectSingleNode("post/pdadata/maintainance/daystart"))
                    LoadSurvey("END", ThisRequest.SelectSingleNode("post/pdadata/maintainance/dayend"))
                    LoadSalesInvoices()
                    LoadDisposal()
                    LoadOrder()
                    LoadReturn()

                Case Else
                    Throw New Exception("Unsupported nodetype.")

            End Select
        End With

    End Sub

#End Region

#Region "public Properties"

    Private _thisRequest As XmlDocument
    Public ReadOnly Property ThisRequest() As XmlDocument
        Get
            Return _thisRequest
        End Get
    End Property

#End Region

#Region "Private Properties"

    Private ReadOnly Property saveFilename() As String
        Get
            Return String.Format("{0}_{1}.xml", _
                Vehicle, _
                DateAdd(DateInterval.Minute, CURDATE, #1/1/1988#).ToString("yyyMMdd") _
            )
        End Get
    End Property

    Private ReadOnly Property NodeType() As String
        Get
            Try
                Return _thisRequest.SelectSingleNode("post/meta/nodetype").InnerText.ToLower
            Catch ex As Exception
                Throw New Exception("Malformed XML getting NodeType.")
            End Try
        End Get
    End Property

    Private ReadOnly Property CURDATE() As Integer
        Get
            Return CInt(_thisRequest.SelectSingleNode("post/pdadata/home/curdate").InnerText)
        End Get
    End Property

    Private ReadOnly Property user() As String
        Get
            Try
                Return _thisRequest.SelectSingleNode("post/meta/user").InnerText
            Catch ex As Exception
                Throw New Exception("Malformed XML getting user.")
            End Try
        End Get
    End Property

    Private ReadOnly Property Vehicle() As String
        Get
            Try
                Return _thisRequest.SelectSingleNode("post/pdadata/home/vehiclereg").InnerText
            Catch ex As Exception
                Throw New Exception("Malformed XML getting user.")
            End Try
        End Get
    End Property

    Private Sub Signature(ByVal Node As XmlNode, ByRef SignatureFile As String, ByRef PrintName As String)

        Try
            If Node.SelectSingleNode("image").InnerText.Length > 0 Then
                SignatureFile = ws.SaveSignature(Node.SelectSingleNode("image").InnerText)
            Else
                SignatureFile = ""
            End If
        Catch ex As Exception
            Throw New Exception(String.Format("Error Saving Signature: {0}.", ex.Message))
        End Try

        If Node.SelectSingleNode("print").InnerText.Length > 0 Then
            PrintName = Node.SelectSingleNode("print").InnerText
        Else
            PrintName = ""
        End If

    End Sub

    Private ReadOnly Property PriorityText(ByVal node As XmlNode) As List(Of String)
        Get
            Dim result As New List(Of String)
            Try
                Dim Build As String = ""
                Dim ln() As String = Split(node.InnerText.Replace(Chr(10), "").Replace(":[", "<").Replace("]:", ">"), Chr(13) & Chr(13))
                result.Add("<style> p,div,li ")
                result.Add("{margin:0cm;font-size:10.0pt;font-family:'Arial';}</style>")
                Dim rText As String = ""

                For Each l As String In ln
                    rText = rText & " <p> " & Replace(l, Chr(13), " <br> ") & " </p> "
                Next

                Dim words() = Split(rText, " ")
                For i As Integer = 0 To UBound(words)
                    If Len(Build & " " & words(i)) > 68 Then
                        result.Add(Build)
                        While words(i).length > 68
                            result.Add(words(i).ToString.Substring(0, 67))
                            words(i) = words(i).ToString.Substring(67)
                        End While
                        Build = "" & words(i) & " "
                    Else
                        Build = Build & words(i) & " "
                    End If
                Next
                result.Add(Build)

                Return result

            Catch ex As Exception
                Throw New Exception("Malformed XML getting Priority Text.")
            End Try

        End Get
    End Property

#End Region

#Region "Loading Definitions"

    ' Survey Loading Definition
    Private Sub ldDef_Survey(ByVal erl As priority.Loading)
        With erl
            .Clear()
            .Table = "ZSFDC_LOADQUESTDOC"
            .Procedure = "ZSFDC_LOADQUESTDOC"

            .AddColumn(1) = New LoadColumn("CURDATE", tColumnType.typeDATE, 8)
            '.AddColumn(1) = New LoadColumn("DOCNO", tColumnType.typeCHAR, 16)
            .AddColumn(1) = New LoadColumn("QUESTFCODE", tColumnType.typeCHAR, 3)
            .AddColumn(1) = New LoadColumn("CUSTNAME", tColumnType.typeCHAR, 16)
            .AddColumn(1) = New LoadColumn("NAME", tColumnType.typeCHAR, 24)
            .AddColumn(1) = New LoadColumn("ANSWERBY", tColumnType.typeCHAR, 24)
            '.AddColumn(1) = New LoadColumn("FUNCDES", tColumnType.typeCHAR, 32)
            '.AddColumn(1) = New LoadColumn("SERVCALLDOCNO", tColumnType.typeCHAR, 16)
            '.AddColumn(1) = New LoadColumn("EXTFILENAME", tColumnType.typeCHAR, 80)
            '.AddColumn(2) = New LoadColumn("TEXT", tColumnType.typeCHAR, 68)                
            .AddColumn(2) = New LoadColumn("QUESTNUM", tColumnType.typeREAL, 3)
            .AddColumn(2) = New LoadColumn("ANSNUM", tColumnType.typeREAL, 3)
            .AddColumn(3) = New LoadColumn("ITEMTEXT", tColumnType.typeCHAR, 68)
        End With
    End Sub

    ' Delivery Loading Definition
    Private Sub ldDef_SalesInvoice(ByRef erl As priority.Loading)
        With erl
            .Clear()
            .Table = "ZSFDC_LOADAINVOICES"
            .Procedure = "ZSFDC_LOADAINVOICES"

            '.AddColumn(1) = New LoadColumn("CUSTNAME", tColumnType.typeCHAR, 16)
            '.AddColumn(1) = New LoadColumn("CDES", tColumnType.typeCHAR, 48)
            '.AddColumn(1) = New LoadColumn("NAME", tColumnType.typeCHAR, 24)
            '.AddColumn(1) = New LoadColumn("IVDATE", tColumnType.typeDATE, 8)
            '.AddColumn(1) = New LoadColumn("STATDES", tColumnType.typeCHAR, 12)
            '.AddColumn(1) = New LoadColumn("OWNERLOGIN", tColumnType.typeCHAR, 20)
            .AddColumn(1) = New LoadColumn("ORDNAME", tColumnType.typeCHAR, 16)
            '.AddColumn(1) = New LoadColumn("REFERENCE", tColumnType.typeCHAR, 15)
            '.AddColumn(1) = New LoadColumn("BOOKNUM", tColumnType.typeCHAR, 16)
            '.AddColumn(1) = New LoadColumn("PROJDOCNO", tColumnType.typeCHAR, 16)
            '.AddColumn(1) = New LoadColumn("PLNAME", tColumnType.typeCHAR, 6)
            '.AddColumn(1) = New LoadColumn("PERCENT", tColumnType.typeREAL, 8)
            '.AddColumn(1) = New LoadColumn("DISCOUNT", tColumnType.typeREAL, 16)
            '.AddColumn(1) = New LoadColumn("TOTPRICE", tColumnType.typeREAL, 16)
            '.AddColumn(1) = New LoadColumn("CODE", tColumnType.typeCHAR, 3)
            '.AddColumn(1) = New LoadColumn("IVREFA", tColumnType.typeCHAR, 16)
            '.AddColumn(1) = New LoadColumn("DETAILS", tColumnType.typeCHAR, 24)
            '.AddColumn(1) = New LoadColumn("AGENTCODE", tColumnType.typeCHAR, 16)
            '.AddColumn(1) = New LoadColumn("DCODE", tColumnType.typeCHAR, 4)
            '.AddColumn(1) = New LoadColumn("BRANCHNAME", tColumnType.typeCHAR, 6)
            '.AddColumn(1) = New LoadColumn("WARHSNAME", tColumnType.typeCHAR, 4)
            '.AddColumn(1) = New LoadColumn("LOCNAME", tColumnType.typeCHAR, 14)
            '.AddColumn(1) = New LoadColumn("TOWARHSNAME", tColumnType.typeCHAR, 4)
            '.AddColumn(1) = New LoadColumn("TOLOCNAME", tColumnType.typeCHAR, 14)
            '.AddColumn(1) = New LoadColumn("STCODE", tColumnType.typeCHAR, 2)
            '.AddColumn(1) = New LoadColumn("SHIPPERNAME", tColumnType.typeCHAR, 8)
            '.AddColumn(1) = New LoadColumn("LORRYNUM", tColumnType.typeCHAR, 12)
            '.AddColumn(1) = New LoadColumn("DISTRDATE", tColumnType.typeDATE, 8)
            '.AddColumn(1) = New LoadColumn("BOXNUM", tColumnType.typeCHAR, 16)
            '.AddColumn(1) = New LoadColumn("TAXCODE", tColumnType.typeCHAR, 3)
            '.AddColumn(1) = New LoadColumn("PACKCODE", tColumnType.typeCHAR, 2)
            '.AddColumn(1) = New LoadColumn("MWEIGHT", tColumnType.typeREAL, 16)
            '.AddColumn(1) = New LoadColumn("WEIGHT", tColumnType.typeREAL, 16)
            '.AddColumn(1) = New LoadColumn("AIRWAYBILL", tColumnType.typeCHAR, 20)
            '.AddColumn(1) = New LoadColumn("PACKNUM", tColumnType.typeINT, 6)
            .AddColumn(1) = New LoadColumn("SIGNATURE", tColumnType.typeCHAR, 64)
            .AddColumn(1) = New LoadColumn("PRINT", tColumnType.typeCHAR, 32)

            '.AddColumn(2) = New LoadColumn("FLAG", tColumnType.typeCHAR, 1)

            .AddColumn(3) = New LoadColumn("ORDI", tColumnType.typeINT, 20)
            '.AddColumn(3) = New LoadColumn("PARTNAME", tColumnType.typeCHAR, 15)
            '.AddColumn(3) = New LoadColumn("SETFLAG", tColumnType.typeCHAR, 1)
            .AddColumn(3) = New LoadColumn("TQUANT", tColumnType.typeINT, 17)
            '.AddColumn(3) = New LoadColumn("PALLETNAME", tColumnType.typeCHAR, 16)
            '.AddColumn(3) = New LoadColumn("CUSTPARTNAME", tColumnType.typeCHAR, 15)
            '.AddColumn(3) = New LoadColumn("CUSTPARTBARCODE", tColumnType.typeCHAR, 16)
            '.AddColumn(3) = New LoadColumn("REVNAME", tColumnType.typeCHAR, 10)
            '.AddColumn(3) = New LoadColumn("SERIALNAME", tColumnType.typeCHAR, 22)
            '.AddColumn(3) = New LoadColumn("BARCODE", tColumnType.typeCHAR, 16)
            '.AddColumn(3) = New LoadColumn("NUMPACK", tColumnType.typeINT, 6)
            '.AddColumn(3) = New LoadColumn("PACKCODE2", tColumnType.typeCHAR, 2)
            '.AddColumn(3) = New LoadColumn("CARTONNUM", tColumnType.typeCHAR, 8)
            '.AddColumn(3) = New LoadColumn("BUDCODE", tColumnType.typeCHAR, 24)
            '.AddColumn(3) = New LoadColumn("COSTCNAME", tColumnType.typeCHAR, 8)
            '.AddColumn(3) = New LoadColumn("COSTCNAME2", tColumnType.typeCHAR, 8)
            '.AddColumn(3) = New LoadColumn("COSTCNAME3", tColumnType.typeCHAR, 8)
            '.AddColumn(3) = New LoadColumn("COSTCNAME4", tColumnType.typeCHAR, 8)
            '.AddColumn(3) = New LoadColumn("COSTCNAME5", tColumnType.typeCHAR, 8)
            '.AddColumn(3) = New LoadColumn("ACCNAME", tColumnType.typeCHAR, 16)
            '.AddColumn(3) = New LoadColumn("PDACCNAME", tColumnType.typeCHAR, 16)
            '.AddColumn(3) = New LoadColumn("FNCICODE", tColumnType.typeCHAR, 8)
            '.AddColumn(3) = New LoadColumn("TRANSREFERENCE", tColumnType.typeCHAR, 16)
            '.AddColumn(3) = New LoadColumn("TAXCODE", tColumnType.typeCHAR, 3)
            '.AddColumn(3) = New LoadColumn("TAXCODE3", tColumnType.typeCHAR, 3)
            '.AddColumn(3) = New LoadColumn("MPARTNAME", tColumnType.typeCHAR, 15)
            '.AddColumn(3) = New LoadColumn("MMPARTNAME", tColumnType.typeCHAR, 15)
            '.AddColumn(3) = New LoadColumn("CUSTNAME3", tColumnType.typeCHAR, 16)
            '.AddColumn(3) = New LoadColumn("WARHSNAME3", tColumnType.typeCHAR, 4)
            '.AddColumn(3) = New LoadColumn("LOCNAME3", tColumnType.typeCHAR, 14)   
        End With
    End Sub

    ' Returns Loading definition
    Private Sub ldDef_Returns(ByRef erl As priority.Loading)
        With erl
            .Clear()
            .Table = "ZSFDC_LOADRETURNS"
            .Procedure = "ZSFDC_LOADRETURNS"

            .AddColumn(1) = New LoadColumn("CUSTNAME", tColumnType.typeCHAR, 16)
            '.AddColumn(1) = New LoadColumn("CDES", tColumnType.typeCHAR, 48)
            .AddColumn(1) = New LoadColumn("CURDATE", tColumnType.typeDATE, 8)
            '.AddColumn(1) = New LoadColumn("BOOKNUM", tColumnType.typeCHAR, 16)
            .AddColumn(1) = New LoadColumn("FLAG", tColumnType.typeCHAR, 1)
            '.AddColumn(1) = New LoadColumn("STATDES", tColumnType.typeCHAR, 12)
            .AddColumn(1) = New LoadColumn("OWNERLOGIN", tColumnType.typeCHAR, 20)
            .AddColumn(1) = New LoadColumn("ORDNAME", tColumnType.typeCHAR, 16)
            '.AddColumn(1) = New LoadColumn("REFERENCE", tColumnType.typeCHAR, 15)
            .AddColumn(1) = New LoadColumn("DETAILS", tColumnType.typeCHAR, 24)
            .AddColumn(1) = New LoadColumn("ODOCNO", tColumnType.typeCHAR, 16)
            .AddColumn(1) = New LoadColumn("QDOCNO", tColumnType.typeCHAR, 16)
            .AddColumn(1) = New LoadColumn("IVNUM", tColumnType.typeCHAR, 16)
            '.AddColumn(1) = New LoadColumn("PROJDOCNO", tColumnType.typeCHAR, 16)
            .AddColumn(1) = New LoadColumn("TOWARHSNAME", tColumnType.typeCHAR, 4)
            .AddColumn(1) = New LoadColumn("TOLOCNAME", tColumnType.typeCHAR, 14)
            '.AddColumn(1) = New LoadColumn("WARHSNAME", tColumnType.typeCHAR, 4)
            '.AddColumn(1) = New LoadColumn("LOCNAME", tColumnType.typeCHAR, 14)
            '.AddColumn(1) = New LoadColumn("RMADOCNO", tColumnType.typeCHAR, 16)
            '.AddColumn(1) = New LoadColumn("AGENTCODE", tColumnType.typeCHAR, 16)
            '.AddColumn(1) = New LoadColumn("PARENTSERNUM", tColumnType.typeCHAR, 20)
            .AddColumn(1) = New LoadColumn("PARTNAME", tColumnType.typeCHAR, 15)
            .AddColumn(1) = New LoadColumn("DOCREF", tColumnType.typeCHAR, 10)
            .AddColumn(1) = New LoadColumn("DCODE", tColumnType.typeCHAR, 4)
            '.AddColumn(1) = New LoadColumn("BRANCHNAME", tColumnType.typeCHAR, 6)
            '.AddColumn(1) = New LoadColumn("PLNAME", tColumnType.typeCHAR, 6)

            .AddColumn(2) = New LoadColumn("PARTNAME2", tColumnType.typeCHAR, 15)
            '.AddColumn(2) = New LoadColumn("CUSTPARTNAME", tColumnType.typeCHAR, 15)
            '.AddColumn(2) = New LoadColumn("CUSTPARTBARCODE", tColumnType.typeCHAR, 16)
            '.AddColumn(2) = New LoadColumn("REVNAME", tColumnType.typeCHAR, 10)
            '.AddColumn(2) = New LoadColumn("SETFLAG", tColumnType.typeCHAR, 1)
            .AddColumn(2) = New LoadColumn("TQUANT", tColumnType.typeINT, 17)
            .AddColumn(2) = New LoadColumn("ORDI", tColumnType.typeINT, 17)
            ' .AddColumn(2) = New LoadColumn("PRICE", tColumnType.typeREAL, 13)
            ' .AddColumn(2) = New LoadColumn("PERCENT", tColumnType.typeREAL, 8)

            '.AddColumn(2) = New LoadColumn("QUANT", tColumnType.typeINT, 17)
            '.AddColumn(2) = New LoadColumn("NUMPACK", tColumnType.typeINT, 6)
            '.AddColumn(2) = New LoadColumn("PACKCODE", tColumnType.typeCHAR, 2)
            '.AddColumn(2) = New LoadColumn("BARCODE", tColumnType.typeCHAR, 16)

            .AddColumn(2) = New LoadColumn("FROMCUSTNAME", tColumnType.typeCHAR, 16)
            .AddColumn(2) = New LoadColumn("CUSTNAME2", tColumnType.typeCHAR, 16)
            .AddColumn(2) = New LoadColumn("TOSERIALNAME", tColumnType.typeCHAR, 22)
            .AddColumn(2) = New LoadColumn("ORDNAME2", tColumnType.typeCHAR, 16)
            .AddColumn(2) = New LoadColumn("OLINE", tColumnType.typeINT, 13)
            ' .AddColumn(2) = New LoadColumn("FLAG2", tColumnType.typeCHAR, 1)
            '.AddColumn(2) = New LoadColumn("CHECKING", tColumnType.typeCHAR, 1)
            '.AddColumn(2) = New LoadColumn("BUDCODE", tColumnType.typeCHAR, 24)
            '.AddColumn(2) = New LoadColumn("COSTCNAME", tColumnType.typeCHAR, 8)
            '.AddColumn(2) = New LoadColumn("EXCH", tColumnType.typeREAL, 13)
            '.AddColumn(2) = New LoadColumn("TRANSREFERENCE", tColumnType.typeCHAR, 16)
            '.AddColumn(2) = New LoadColumn("ACTNAME", tColumnType.typeCHAR, 16)
            '.AddColumn(2) = New LoadColumn("TOACTNAME", tColumnType.typeCHAR, 16)
            '.AddColumn(2) = New LoadColumn("TOPALLETNAME", tColumnType.typeCHAR, 16)
        End With
    End Sub

    ' Order Loading Definition
    Private Sub ldDef_Orders(ByRef erl As priority.Loading)
        With erl
            .Clear()
            .Table = "ZSFDC_LOADORDERS"
            .Procedure = "ZSFDC_LOADORDERS"

            .AddColumn(1) = New LoadColumn("CUSTNAME", tColumnType.typeCHAR, 16)
            ' .AddColumn(1) = New LoadColumn("CDES", tColumnType.typeCHAR, 48)
            '.AddColumn(1) = New LoadColumn("NAME", tColumnType.typeCHAR, 24)
            .AddColumn(1) = New LoadColumn("CURDATE", tColumnType.typeDATE, 8)
            '.AddColumn(1) = New LoadColumn("ORDNAME", tColumnType.typeCHAR, 16)
            '.AddColumn(1) = New LoadColumn("BOOKNUM", tColumnType.typeCHAR, 16)
            '.AddColumn(1) = New LoadColumn("DOCNO", tColumnType.typeCHAR, 16)
            '.AddColumn(1) = New LoadColumn("ORDSTATUSDES", tColumnType.typeCHAR, 12)
            '.AddColumn(1) = New LoadColumn("BOOLCLOSED", tColumnType.typeCHAR, 1)
            '.AddColumn(1) = New LoadColumn("FORECASTFLAG", tColumnType.typeCHAR, 1)
            '.AddColumn(1) = New LoadColumn("CPROFNUM", tColumnType.typeCHAR, 16)
            '.AddColumn(1) = New LoadColumn("PLNAME", tColumnType.typeCHAR, 6)
            '.AddColumn(1) = New LoadColumn("DEALNAME", tColumnType.typeCHAR, 16)
            '.AddColumn(1) = New LoadColumn("DETAILS", tColumnType.typeCHAR, 24)
            '.AddColumn(1) = New LoadColumn("RMADOCNUM", tColumnType.typeCHAR, 16)
            '.AddColumn(1) = New LoadColumn("AGENTCODE", tColumnType.typeCHAR, 16)
            '.AddColumn(1) = New LoadColumn("BRANCHNAME", tColumnType.typeCHAR, 6)
            .AddColumn(1) = New LoadColumn("DCODE", tColumnType.typeCHAR, 4)
            '.AddColumn(1) = New LoadColumn("STCODE", tColumnType.typeCHAR, 2)
            '.AddColumn(1) = New LoadColumn("TYPECODE", tColumnType.typeCHAR, 3)
            .AddColumn(1) = New LoadColumn("REFERENCE", tColumnType.typeCHAR, 15)
            '.AddColumn(1) = New LoadColumn("MODELNAME", tColumnType.typeCHAR, 6)
            '.AddColumn(1) = New LoadColumn("QUANT", tColumnType.typeINT, 17)
            '.AddColumn(1) = New LoadColumn("PERCENT", tColumnType.typeREAL, 8)
            '.AddColumn(1) = New LoadColumn("TAXCODE", tColumnType.typeCHAR, 3)
            '.AddColumn(1) = New LoadColumn("TOTPRICE", tColumnType.typeREAL, 16)
            '.AddColumn(1) = New LoadColumn("CODE", tColumnType.typeCHAR, 3)
            '.AddColumn(1) = New LoadColumn("LCODE", tColumnType.typeCHAR, 3)
            '.AddColumn(1) = New LoadColumn("LEXCH", tColumnType.typeREAL, 13)
            '.AddColumn(1) = New LoadColumn("LEXCHTOL", tColumnType.typeREAL, 6)
            '.AddColumn(1) = New LoadColumn("LEXCHNEG", tColumnType.typeCHAR, 1)
            '.AddColumn(1) = New LoadColumn("ADJPRICEFLAG", tColumnType.typeCHAR, 1)
            '.AddColumn(1) = New LoadColumn("LINKOPTIONS", tColumnType.typeCHAR, 1)
            '.AddColumn(1) = New LoadColumn("PAYCODE", tColumnType.typeCHAR, 2)
            '.AddColumn(1) = New LoadColumn("OBFLAG", tColumnType.typeCHAR, 1)
            '.AddColumn(1) = New LoadColumn("ADVPERCENT", tColumnType.typeREAL, 6)
            '.AddColumn(1) = New LoadColumn("DOERNAME", tColumnType.typeCHAR, 20)
            '.AddColumn(1) = New LoadColumn("DOERNAME2", tColumnType.typeCHAR, 20)
            ' .AddColumn(1) = New LoadColumn("DOERNAME3", tColumnType.typeCHAR, 20)
            ' .AddColumn(1) = New LoadColumn("SDATE", tColumnType.typeDATE, 8)
            '.AddColumn(1) = New LoadColumn("STIME", tColumnType.typeTIME, 5)
            '.AddColumn(1) = New LoadColumn("EDATE", tColumnType.typeDATE, 8)
            '.AddColumn(1) = New LoadColumn("ETIME", tColumnType.typeTIME, 5)
            '.AddColumn(1) = New LoadColumn("WARHSNAME", tColumnType.typeCHAR, 4)
            '.AddColumn(1) = New LoadColumn("LOCNAME", tColumnType.typeCHAR, 14)
            '.AddColumn(1) = New LoadColumn("TOWARHSNAME", tColumnType.typeCHAR, 4)
            '.AddColumn(1) = New LoadColumn("TOLOCNAME", tColumnType.typeCHAR, 14)
            '.AddColumn(1) = New LoadColumn("BONUSFLAG", tColumnType.typeCHAR, 1)
            '.AddColumn(1) = New LoadColumn("CCNUM", tColumnType.typeCHAR, 24)
            '.AddColumn(1) = New LoadColumn("PIKALONEFLAG", tColumnType.typeCHAR, 1)
            '.AddColumn(1) = New LoadColumn("STZONECODE", tColumnType.typeCHAR, 2)
            '.AddColumn(1) = New LoadColumn("WTASKDOCCODE", tColumnType.typeCHAR, 4)
            .AddColumn(2) = New LoadColumn("PARTNAME", tColumnType.typeCHAR, 15)
            '.AddColumn(2) = New LoadColumn("PDES", tColumnType.typeCHAR, 48)
            .AddColumn(2) = New LoadColumn("TQUANT", tColumnType.typeINT, 17)
            '.AddColumn(2) = New LoadColumn("PRICE", tColumnType.typeREAL, 13)
            '.AddColumn(2) = New LoadColumn("PURCHASEPRICE", tColumnType.typeREAL, 13)
            '.AddColumn(2) = New LoadColumn("ICODE2", tColumnType.typeCHAR, 3)
            .AddColumn(2) = New LoadColumn("DUEDATE", tColumnType.typeDATE, 8)
            ' .AddColumn(2) = New LoadColumn("CLOSEDBOOL", tColumnType.typeCHAR, 1)
            ' .AddColumn(2) = New LoadColumn("PERCENT2", tColumnType.typeREAL, 8)
            '.AddColumn(2) = New LoadColumn("VATPRICE", tColumnType.typeREAL, 16)
            '.AddColumn(2) = New LoadColumn("NUMPACK", tColumnType.typeINT, 6)
            '.AddColumn(2) = New LoadColumn("PACKCODE", tColumnType.typeCHAR, 2)
            '.AddColumn(2) = New LoadColumn("PLANHOURSD", tColumnType.typeREAL, 16)
            '.AddColumn(2) = New LoadColumn("REVNAME", tColumnType.typeCHAR, 10)
            '.AddColumn(2) = New LoadColumn("VATFLAGA", tColumnType.typeCHAR, 1)
            '.AddColumn(2) = New LoadColumn("COMMISSION", tColumnType.typeREAL, 6)
            '.AddColumn(2) = New LoadColumn("BARCODE", tColumnType.typeCHAR, 16)
            '.AddColumn(2) = New LoadColumn("SERIALNAME", tColumnType.typeCHAR, 22)
            '.AddColumn(2) = New LoadColumn("CUSTPARTNAME", tColumnType.typeCHAR, 15)
            '.AddColumn(2) = New LoadColumn("CUSTPARTBARCODE", tColumnType.typeCHAR, 16)
            '.AddColumn(2) = New LoadColumn("YOURORDLINE", tColumnType.typeINT, 8)
            '.AddColumn(2) = New LoadColumn("UNSPSCCODE", tColumnType.typeCHAR, 8)
            '.AddColumn(2) = New LoadColumn("CLUSTNAME", tColumnType.typeCHAR, 12)
            '.AddColumn(2) = New LoadColumn("MRP", tColumnType.typeCHAR, 1)
            '.AddColumn(2) = New LoadColumn("PRDATE", tColumnType.typeDATE, 8)
            '.AddColumn(2) = New LoadColumn("CASHFLOWDATE", tColumnType.typeDATE, 8)
            '.AddColumn(2) = New LoadColumn("DCODE2", tColumnType.typeCHAR, 4)
            '.AddColumn(2) = New LoadColumn("BUDCODE", tColumnType.typeCHAR, 24)
            '.AddColumn(2) = New LoadColumn("COSTCNAME", tColumnType.typeCHAR, 8)
            '.AddColumn(2) = New LoadColumn("EXCH2", tColumnType.typeREAL, 13)
            '.AddColumn(2) = New LoadColumn("ORDISTATUSDES", tColumnType.typeCHAR, 20)
            '.AddColumn(2) = New LoadColumn("COPYFAVSUPFLAG", tColumnType.typeCHAR, 1)
            '.AddColumn(2) = New LoadColumn("SUPNAME", tColumnType.typeCHAR, 16)
            '.AddColumn(2) = New LoadColumn("OBFLAG2", tColumnType.typeCHAR, 1)
            '.AddColumn(2) = New LoadColumn("PREPAYED", tColumnType.typeCHAR, 1)
            '.AddColumn(2) = New LoadColumn("PAYCUSTNAME", tColumnType.typeCHAR, 16)
            '.AddColumn(2) = New LoadColumn("PARTTREEFLAG", tColumnType.typeCHAR, 1)
            '.AddColumn(2) = New LoadColumn("PURTAXPERCENT", tColumnType.typeREAL, 7)
            '.AddColumn(2) = New LoadColumn("LCODE2", tColumnType.typeCHAR, 3)
            '.AddColumn(2) = New LoadColumn("LEXCH2", tColumnType.typeREAL, 13)
            '.AddColumn(2) = New LoadColumn("PROJCOSTFLAG", tColumnType.typeCHAR, 1)
            '.AddColumn(2) = New LoadColumn("MPARTNAME", tColumnType.typeCHAR, 15)
            '.AddColumn(2) = New LoadColumn("MMPARTNAME", tColumnType.typeCHAR, 15)
            '.AddColumn(2) = New LoadColumn("REMARK1", tColumnType.typeCHAR, 12)
            '.AddColumn(2) = New LoadColumn("REMARK2", tColumnType.typeCHAR, 12)
        End With
    End Sub

    ' Disposal Definition
    Private Sub ldDef_Disposals(ByRef erl As priority.Loading)
        With erl
            .Clear()
            .Table = "ZSFDC_LOADDISPOSAL"
            .Procedure = "ZSFDC_LOADDISPOSAL"

            .AddColumn(1) = New LoadColumn("CURDATE", tColumnType.typeDATE, 8)
            .AddColumn(1) = New LoadColumn("WARHSNAME", tColumnType.typeCHAR, 4)
            .AddColumn(1) = New LoadColumn("LOCNAME", tColumnType.typeCHAR, 14)
            '.AddColumn(1) = New LoadColumn("STATDES", tColumnType.typeCHAR, 12)
            .AddColumn(1) = New LoadColumn("OWNERLOGIN", tColumnType.typeCHAR, 20)
            .AddColumn(1) = New LoadColumn("DETAILS", tColumnType.typeCHAR, 24)

            .AddColumn(2) = New LoadColumn("PARTNAME", tColumnType.typeCHAR, 15)
            .AddColumn(2) = New LoadColumn("SERIALNAME", tColumnType.typeCHAR, 22)
            '.AddColumn(2) = New LoadColumn("ACTNAME", tColumnType.typeCHAR, 16)
            '.AddColumn(2) = New LoadColumn("REVNAME", tColumnType.typeCHAR, 10)
            .AddColumn(2) = New LoadColumn("CUSTNAME", tColumnType.typeCHAR, 16)
            '.AddColumn(2) = New LoadColumn("NUMPACK", tColumnType.typeINT, 6)
            '.AddColumn(2) = New LoadColumn("PACKCODE", tColumnType.typeCHAR, 2)
            .AddColumn(2) = New LoadColumn("QUANT", tColumnType.typeINT, 17)
            '.AddColumn(2) = New LoadColumn("REWORKFLAG", tColumnType.typeCHAR, 1)
        End With
    End Sub

    ' Customer REmarks definition
    Private Sub ldDef_CustRemarks(ByRef erl As priority.Loading)
        With erl
            .Clear()
            .Table = "ZSFDC_LOADCUSTOMERS"
            .Procedure = "ZSFDC_LOADCUSTOMERS"

            .AddColumn(1) = New LoadColumn("CUSTNAME", tColumnType.typeCHAR, 16)
            '.AddColumn(1) = New LoadColumn("CUSTDES", tColumnType.typeCHAR, 48)
            '.AddColumn(1) = New LoadColumn("ECUSTDES", tColumnType.typeCHAR, 48)
            '.AddColumn(1) = New LoadColumn("STATDES", tColumnType.typeCHAR, 12)
            '.AddColumn(1) = New LoadColumn("OWNERLOGIN", tColumnType.typeCHAR, 20)
            '.AddColumn(1) = New LoadColumn("CREATEDDATE", tColumnType.typeDATE, 8)
            '.AddColumn(1) = New LoadColumn("PHONE", tColumnType.typeCHAR, 20)
            '.AddColumn(1) = New LoadColumn("FAX", tColumnType.typeCHAR, 20)
            '.AddColumn(1) = New LoadColumn("EMAIL", tColumnType.typeCHAR, 48)
            '.AddColumn(1) = New LoadColumn("BUSINESSTYPE", tColumnType.typeCHAR, 32)
            '.AddColumn(1) = New LoadColumn("MCUSTNAME", tColumnType.typeCHAR, 16)
            '.AddColumn(1) = New LoadColumn("CTYPECODE", tColumnType.typeCHAR, 2)
            '.AddColumn(1) = New LoadColumn("CTYPE2CODE", tColumnType.typeCHAR, 2)
            '.AddColumn(1) = New LoadColumn("CUSTPART", tColumnType.typeCHAR, 1)
            '.AddColumn(1) = New LoadColumn("NSFLAG", tColumnType.typeCHAR, 1)
            '.AddColumn(1) = New LoadColumn("STCODE", tColumnType.typeCHAR, 2)
            '.AddColumn(1) = New LoadColumn("ZONECODE", tColumnType.typeCHAR, 2)
            '.AddColumn(1) = New LoadColumn("ADDRESS", tColumnType.typeCHAR, 80)
            '.AddColumn(1) = New LoadColumn("ADDRESS2", tColumnType.typeCHAR, 80)
            '.AddColumn(1) = New LoadColumn("ADDRESS3", tColumnType.typeCHAR, 80)
            '.AddColumn(1) = New LoadColumn("STATE", tColumnType.typeCHAR, 40)
            '.AddColumn(1) = New LoadColumn("STATEA", tColumnType.typeCHAR, 40)
            '.AddColumn(1) = New LoadColumn("STATENAME", tColumnType.typeCHAR, 40)
            '.AddColumn(1) = New LoadColumn("ZIP", tColumnType.typeCHAR, 10)
            '.AddColumn(1) = New LoadColumn("COUNTRYNAME", tColumnType.typeCHAR, 18)
            '.AddColumn(1) = New LoadColumn("WTAXNUM", tColumnType.typeCHAR, 16)
            '.AddColumn(1) = New LoadColumn("VATNUM", tColumnType.typeCHAR, 16)
            '.AddColumn(1) = New LoadColumn("AGENTCODE", tColumnType.typeCHAR, 16)
            '.AddColumn(1) = New LoadColumn("AGENTCODE2", tColumnType.typeCHAR, 16)
            '.AddColumn(1) = New LoadColumn("TERRITORYCODE", tColumnType.typeCHAR, 12)
            '.AddColumn(1) = New LoadColumn("COMMISSION", tColumnType.typeREAL, 6)
            '.AddColumn(1) = New LoadColumn("ESTABLISHED", tColumnType.typeCHAR, 4)
            '.AddColumn(1) = New LoadColumn("EMPNUM", tColumnType.typeINT, 7)
            '.AddColumn(1) = New LoadColumn("BRANCHNAME", tColumnType.typeCHAR, 6)
            '.AddColumn(1) = New LoadColumn("PAYCODE", tColumnType.typeCHAR, 2)
            '.AddColumn(1) = New LoadColumn("MAX_CREDIT", tColumnType.typeREAL, 16)
            '.AddColumn(1) = New LoadColumn("MAX_OBLIGO", tColumnType.typeREAL, 16)
            '.AddColumn(1) = New LoadColumn("OBCODE", tColumnType.typeCHAR, 3)
            '.AddColumn(1) = New LoadColumn("DISTRLINECODE", tColumnType.typeCHAR, 3)
            '.AddColumn(1) = New LoadColumn("UNLOADTIME", tColumnType.typeTIME, 5)
            '.AddColumn(1) = New LoadColumn("DISTRORDER", tColumnType.typeINT, 3)
            '.AddColumn(1) = New LoadColumn("BONUSFLAG", tColumnType.typeCHAR, 1)
            '.AddColumn(1) = New LoadColumn("FORECAST", tColumnType.typeCHAR, 1)
            '.AddColumn(1) = New LoadColumn("CHANEL", tColumnType.typeCHAR, 1)
            '.AddColumn(1) = New LoadColumn("DISTRTYPECODE", tColumnType.typeCHAR, 3)
            '.AddColumn(1) = New LoadColumn("SECONDLANGTEXT", tColumnType.typeCHAR, 1)
            '.AddColumn(1) = New LoadColumn("CONFIDENTIAL", tColumnType.typeCHAR, 1)
            '.AddColumn(1) = New LoadColumn("HOSTNAME", tColumnType.typeCHAR, 32)
            '.AddColumn(1) = New LoadColumn("SPEC1", tColumnType.typeCHAR, 32)
            '.AddColumn(1) = New LoadColumn("SPEC2", tColumnType.typeCHAR, 32)
            '.AddColumn(1) = New LoadColumn("SPEC3", tColumnType.typeCHAR, 32)
            '.AddColumn(1) = New LoadColumn("SPEC4", tColumnType.typeCHAR, 32)
            '.AddColumn(1) = New LoadColumn("SPEC5", tColumnType.typeCHAR, 32)
            '.AddColumn(1) = New LoadColumn("SPEC6", tColumnType.typeCHAR, 32)
            '.AddColumn(1) = New LoadColumn("SPEC7", tColumnType.typeCHAR, 32)
            '.AddColumn(1) = New LoadColumn("SPEC8", tColumnType.typeCHAR, 32)
            '.AddColumn(1) = New LoadColumn("SPEC9", tColumnType.typeCHAR, 32)
            '.AddColumn(1) = New LoadColumn("SPEC10", tColumnType.typeCHAR, 32)
            '.AddColumn(1) = New LoadColumn("SPEC11", tColumnType.typeCHAR, 32)
            '.AddColumn(1) = New LoadColumn("SPEC12", tColumnType.typeCHAR, 32)
            '.AddColumn(1) = New LoadColumn("SPEC13", tColumnType.typeCHAR, 32)
            '.AddColumn(1) = New LoadColumn("SPEC14", tColumnType.typeCHAR, 32)
            '.AddColumn(1) = New LoadColumn("SPEC15", tColumnType.typeCHAR, 32)
            '.AddColumn(1) = New LoadColumn("SPEC16", tColumnType.typeCHAR, 32)
            '.AddColumn(1) = New LoadColumn("SPEC17", tColumnType.typeCHAR, 32)
            '.AddColumn(1) = New LoadColumn("SPEC18", tColumnType.typeCHAR, 32)
            '.AddColumn(1) = New LoadColumn("SPEC19", tColumnType.typeCHAR, 32)
            '.AddColumn(1) = New LoadColumn("SPEC20", tColumnType.typeCHAR, 32)
            '.AddColumn(1) = New LoadColumn("WAVESTRATEGYCODE", tColumnType.typeCHAR, 3)
            '.AddColumn(1) = New LoadColumn("AUTOSHPFLAG", tColumnType.typeCHAR, 1)
            '.AddColumn(1) = New LoadColumn("GPSX", tColumnType.typeCHAR, 20)
            '.AddColumn(1) = New LoadColumn("GPSY", tColumnType.typeCHAR, 20)
            '.AddColumn(1) = New LoadColumn("ZROD_GERBERNUM", tColumnType.typeCHAR, 16)
            '.AddColumn(1) = New LoadColumn("ZEMG_MAND_CUST_PO", tColumnType.typeCHAR, 1)
            '.AddColumn(1) = New LoadColumn("ZROD_PHONETIME", tColumnType.typeTIME, 5)
            '.AddColumn(1) = New LoadColumn("ZROD_PREFCALLERLOGIN", tColumnType.typeCHAR, 20)
            '.AddColumn(1) = New LoadColumn("ZROD_CLOSEDFROM", tColumnType.typeDATE, 8)
            '.AddColumn(1) = New LoadColumn("ZROD_CLOSEDTO", tColumnType.typeDATE, 8)
            '.AddColumn(1) = New LoadColumn("ZROD_ROUTENAME1", tColumnType.typeCHAR, 6)
            '.AddColumn(1) = New LoadColumn("ZROD_ROUTENAME2", tColumnType.typeCHAR, 6)
            '.AddColumn(1) = New LoadColumn("ZROD_ROUTENAME3", tColumnType.typeCHAR, 6)
            .AddColumn(2) = New LoadColumn("TEXT", tColumnType.typeCHAR, 68)
        End With
    End Sub

#End Region

#Region "Loadings"

    Public Sub LoadSurvey(ByVal SurveyName As String, ByVal SurveyNode As XmlNode)

        Using erl As New priority.Loading
            ldDef_Survey(erl)
            With erl

                .AddRecordType(1) = New LoadRow( _
                    String.Format(CURDATE.ToString, "CURDATE,DATE,8"), _
                    String.Format(SurveyName, "QUESTFCODE,CHAR,3"), _
                    String.Format("", "CUSTNAME,CHAR,16"), _
                    String.Format(user, "NAME,CHAR,24"), _
                    String.Format(Vehicle, _
                                   "ANSWERBY,CHAR,24") _
                )

                For Each qu As XmlNode In SurveyNode.SelectNodes(".//question")
                    Dim re As XmlNode = qu.SelectSingleNode("response")
                    If re.SelectSingleNode("value").InnerText.Length > 0 Then
                        .AddRecordType(2) = New LoadRow( _
                                    qu.SelectSingleNode("number").InnerText, _
                                    re.SelectSingleNode("value").InnerText _
                                    )
                    ElseIf re.SelectSingleNode("text").InnerText.Length > 0 Then
                        .AddRecordType(2) = New LoadRow( _
                            qu.SelectSingleNode("number").InnerText, _
                            "0" _
                        )
                        For Each Str As String In PriorityText(re.SelectSingleNode("text"))
                            .AddRecordType(3) = New LoadRow(Str)
                        Next

                    End If
                Next

                Dim exp As New Exception
                If Not .Post("http://localhost:8080/loadHandler.ashx", exp) Then Throw exp

            End With
        End Using
    End Sub

    Private Sub LoadSalesInvoices()

        For Each Delivery As XmlNode In thisRequest.SelectNodes("//delivery")
            Select Case Delivery.SelectNodes("parts/part[cquant!=0]").Count
                Case 0 ' No items delivered
                    Select Case Delivery.SelectNodes("parts/part[tquant!=0]").Count
                        Case 0 ' No items due for delivery

                        Case Else
                            ' TODO: Load no delivery reason
                    End Select

                Case Else ' Items delivered

                    Using erl As New priority.Loading
                        ldDef_SalesInvoice(erl)
                        With erl

                            Dim SigFile As String = String.Empty
                            Dim pName As String = String.Empty

                            Signature(Delivery.SelectSingleNode("customersignature"), SigFile, pName)
                            .AddRecordType(1) = New LoadRow( _
                                String.Format(Delivery.SelectSingleNode("sonum").InnerText, "ORDNAME,CHAR,16"), _
                                String.Format(SigFile, "SIGNATURE,CHAR,64"), _
                                String.Format(pName, "PRINT,CHAR,32") _
                            )

                            For Each line As XmlNode In Delivery.SelectNodes("parts/part[cquant>0]")
                                .AddRecordType(3) = New LoadRow( _
                                    String.Format(line.SelectSingleNode("ordi").InnerText.Split("|")(0), "ORDI,INT,13"), _
                                    String.Format(line.SelectSingleNode("cquant").InnerText, "TQUANT,INT,17") _
                                )
                            Next

                            Dim exp As New Exception
                            If Not .Post("http://localhost:8080/loadHandler.ashx", exp) Then Throw exp

                        End With
                    End Using
            End Select
        Next

    End Sub

    Private Sub LoadReturn()

        Using erl As New priority.Loading
            ldDef_Returns(erl)
            With erl

                Dim creditNotes As XmlNodeList = ThisRequest.SelectNodes("//creditnote")

                For Each creditnote As XmlNode In creditNotes
                    .AddRecordType(1) = New LoadRow( _
                        String.Format(creditnote.ParentNode.SelectSingleNode("custnumber").InnerText, "CUSTNAME,CHAR,16"), _
                        String.Format(CURDATE.ToString, "CURDATE,DATE,8"), _
                        String.Format("", "FLAG,CHAR,1"), _
                        String.Format(user, "OWNERLOGIN,CHAR,20"), _
                        String.Format(creditnote.SelectSingleNode("parts/part/reason").InnerText, "ORDNAME,CHAR,16"), _
                        String.Format("", "DETAILS,CHAR,24"), _
                        String.Format("", "ODOCNO,CHAR,16"), _
                        String.Format("", "QDOCNO,CHAR,16"), _
                        String.Format("", "IVNUM,CHAR,16"), _
                        String.Format("", "TOWARHSNAME,CHAR,4"), _
                        String.Format("", "TOLOCNAME,CHAR,14"), _
                        String.Format("", "PARTNAME,CHAR,15"), _
                        String.Format("", "DOCREF,CHAR,10"), _
                        String.Format("", "DCODE,CHAR,4") _
                    )
                    For Each Part As XmlNode In creditnote.SelectNodes("parts/part")
                        .AddRecordType(2) = New LoadRow( _
                            String.Format(Part.SelectSingleNode("name").InnerText, "PARTNAME2,CHAR,15"), _
                            String.Format(Part.SelectSingleNode("qty").InnerText, "TQUANT,INT,17"), _
                            String.Format(Part.SelectSingleNode("ordi").InnerText, "ORDI,INT,17"), _
                            String.Format("", "FROMCUSTNAME,CHAR,16"), _
                            String.Format("", "CUSTNAME2,CHAR,16"), _
                            String.Format("", "TOSERIALNAME,CHAR,22"), _
                            String.Format("", "ORDNAME2,CHAR,16"), _
                            String.Format("0", "OLINE,INT,13") _
                        )
                    Next

                Next

                Dim exp As New Exception
                If Not .Post("http://localhost:8080/loadHandler.ashx", exp) Then Throw exp

            End With
        End Using
    End Sub

    Private Sub LoadOrder()

        Using erl As New priority.Loading
            ldDef_Orders(erl)
            With erl

                Dim orders As XmlNodeList = ThisRequest.SelectNodes("//order[parts/part[qty>0]]")

                For Each order As XmlNode In orders

                    .AddRecordType(1) = New LoadRow( _
                        String.Format(order.ParentNode.ParentNode.SelectSingleNode("custnumber").InnerText, "CUSTNAME,CHAR,16"), _
                        String.Format(CURDATE.ToString, "CURDATE,DATE,8"), _
                        String.Format("", "DCODE,CHAR,4"), _
                        String.Format("", "REFERENCE,CHAR,15") _
                    )
                    For Each pt As XmlNode In order.SelectSingleNode("parts")
                        .AddRecordType(2) = New LoadRow( _
                            String.Format(pt.SelectSingleNode("name").InnerText, "PARTNAME,CHAR,15"), _
                            String.Format(pt.SelectSingleNode("qty").InnerText, "TQUANT,INT,17"), _
                            String.Format(order.SelectSingleNode("deliverydate").InnerText, "DUEDATE,DATE,8") _
                        )
                    Next

                Next
                Dim exp As New Exception
                If Not .Post("http://localhost:8080/loadHandler.ashx", exp) Then Throw exp

            End With

        End Using

    End Sub

    Private Sub LoadDisposal()

        Using erl As New priority.Loading
            ldDef_Disposals(erl)
            With erl

                .AddRecordType(1) = New LoadRow( _
                    String.Format(CURDATE.ToString, "CURDATE,DATE,8"), _
                    String.Format("", "WARHSNAME,CHAR,4"), _
                    String.Format("", "LOCNAME,CHAR,14"), _
                    String.Format(user, "OWNERLOGIN,CHAR,20"), _
                    String.Format("", "DETAILS,CHAR,24") _
                    )
                For Each disposal As XmlNode In ThisRequest.SelectNodes("//creditnote/parts/part[qty>0]")
                    If CDbl(disposal.SelectSingleNode("rcvdqty").InnerText) < CDbl(disposal.SelectSingleNode("qty").InnerText) Then
                        Dim ret As Double = CDbl(disposal.SelectSingleNode("qty").InnerText) - CDbl(disposal.SelectSingleNode("rcvdqty").InnerText)
                        .AddRecordType(2) = New LoadRow( _
                            String.Format(disposal.SelectSingleNode("name").InnerText, "PARTNAME,CHAR,15"), _
                            String.Format("", "SERIALNAME,CHAR,22"), _
                            String.Format(disposal.ParentNode.ParentNode.ParentNode.SelectSingleNode("custnumber").InnerText, "CUSTNAME,CHAR,16"), _
                            String.Format(ret, "QUANT,INT,17") _
                        )
                    End If
                Next
                Dim exp As New Exception
                If Not .Post("http://localhost:8080/loadHandler.ashx", exp) Then Throw exp

            End With

        End Using

    End Sub

    Private Sub LoadCustomer()

        Using erl As New priority.Loading
            With erl

                Dim customers As XmlNodeList = ThisRequest.SelectNodes("//customer[custnumber[text()]]")

                For Each customer As XmlNode In customers

                    .AddRecordType(1) = New LoadRow( _
                     String.Format(customer.SelectSingleNode("custnumber").InnerText, "CUSTNAME,CHAR,16") _
                        )
                    For Each line As String In PriorityText(customer.SelectSingleNode("customerremarks"))
                        .AddRecordType(2) = New LoadRow(line)
                    Next
                    Dim exp As New Exception
                    If Not .Post("http://localhost:8080/loadHandler.ashx", exp) Then Throw exp

                Next

            End With

        End Using

    End Sub

#End Region

End Class
