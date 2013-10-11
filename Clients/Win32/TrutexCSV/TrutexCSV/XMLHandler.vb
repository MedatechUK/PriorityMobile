Imports System.IO
Imports System.Xml
Imports priority

Public Class XMLHandler
    Public FileReader As StreamReader
    Public OrderXML As XmlWriter


    Public Class Order
        Private OrdCustID As String
        Private OrdID As String
        Private OrdSource As String
        Private OrdDelAddr1 As String
        Private OrdDelAddr2 As String
        Private OrdDelAddr3 As String
        Private OrdDelAddr4 As String
        Private OrdDelAddr5 As String
        Private OrdDelPC As String
        Private OrdAdHoc As String
        Private OrdDate As String
        Private OrdLoc As String
        Public Property Location() As String
            Get
                Dim loc As String
                Select Case OrdLoc.Length
                    Case 1
                        loc = "00" & OrdLoc
                    Case 2
                        loc = "0" & OrdLoc
                    Case Else
                        loc = OrdLoc
                End Select
                Return loc
            End Get
            Set(ByVal value As String)
                OrdLoc = value
            End Set
        End Property
        Public Property CustID() As Integer
            Get
                Return OrdCustID
            End Get
            Set(ByVal value As Integer)
                OrdCustID = value
            End Set
        End Property
        Public Property OId() As String
            Get
                Return OrdID
            End Get
            Set(ByVal value As String)
                OrdID = value
            End Set
        End Property
        Public ReadOnly Property OSo()
            Get
                Return "www.testville.com"
            End Get

        End Property
        Public ReadOnly Property OAddr1() As String
            Get
                Return "Test Line 1"
            End Get
        End Property
        Public ReadOnly Property OAddr2() As String
            Get
                Return "Test Line 2"
            End Get
        End Property
        Public ReadOnly Property OAddr3() As String
            Get
                Return "Test Line 3"
            End Get
        End Property
        Public ReadOnly Property OAddr4() As String
            Get
                Return "Test Line 4"
            End Get
        End Property
        Public ReadOnly Property OAddr5() As String
            Get
                Return "Test Line 5"
            End Get
        End Property
        Public ReadOnly Property OAddrPC() As String
            Get
                Return "Test PC"
            End Get
        End Property
        Public Property AdHoc() As String
            Get
                Return OrdAdHoc
            End Get
            Set(ByVal value As String)
                OrdAdHoc = value
            End Set
        End Property
        Public Sub New(ByVal acn As String, ByVal id As String, ByVal oad As String, ByVal od As String, ByVal lc As String)
            OrdCustID = acn
            OrdID = id
            OrdAdHoc = oad
            odate = od
            OrdLoc = lc
        End Sub
        Public Property odate() As String
            Get
                Return OrdDate
            End Get
            Set(ByVal value As String)
                Dim dstart, dend As DateTime
                dend = FormatDateTime(value, DateFormat.ShortDate)
                dstart = FormatDateTime("1/1/1988", DateFormat.ShortDate)
                OrdDate = DateDiff(DateInterval.Minute, dstart, dend)

            End Set
        End Property
    End Class
    Public Class OrderLine
        Private OrdCustRef As String
        Private OrdLocation As String
        Private OrdDelDate As Integer
        Private OrdColL As Char = "L"
        Private OrdProdCode As String
        Private OrdQuantity As Integer
        Private OrdUnitPrice As Integer

        Public Property CustRef() As String
            Get
                Return OrdCustRef
            End Get
            Set(ByVal value As String)
                OrdCustRef = value
            End Set
        End Property
        Public Property Location() As String
            Get
                Dim loc As String
                Select Case OrdLocation.Length
                    Case 1
                        loc = "00" & OrdLocation
                    Case 2
                        loc = "0" & OrdLocation
                    Case Else
                        loc = OrdLocation
                End Select
                Return loc
            End Get
            Set(ByVal value As String)
                OrdLocation = value
            End Set
        End Property
        Public Property deldate() As String
            Get
                Return OrdDelDate
            End Get
            Set(ByVal value As String)
                Dim dstart, dend As DateTime
                dend = FormatDateTime(value, DateFormat.ShortDate)
                dstart = FormatDateTime("1/1/1988", DateFormat.ShortDate)
                OrdDelDate = DateDiff(DateInterval.Minute, dstart, dend)

            End Set
        End Property
        Public ReadOnly Property ColL()
            Get
                Return OrdColL
            End Get
        End Property
        Public Property ProdCode() As String
            Get
                Return OrdProdCode
            End Get
            Set(ByVal value As String)
                OrdProdCode = value
            End Set
        End Property
        Public Property Quantity() As Integer
            Get
                Return OrdQuantity
            End Get
            Set(ByVal value As Integer)
                OrdQuantity = value
            End Set
        End Property
        Public Property UnitPrice() As Double
            Get
                Return OrdUnitPrice
            End Get
            Set(ByVal value As Double)
                OrdUnitPrice = value * 1000
            End Set
        End Property

        Public Sub New(ByRef CRef As String, ByVal Loc As String, ByVal DDate As Date, ByVal ProdC As String, ByVal Quan As Integer, ByVal uprice As Decimal)
            CustRef = CRef
            Location = Loc
            deldate = DDate
            ProdCode = ProdC
            Quantity = Quan
            UnitPrice = uprice
        End Sub

    End Class
    Public Shared Sub ReadData(ByVal fi As String)
        Dim filereader As StreamReader
        Dim line_no As Integer = 1

        filereader = New StreamReader(fi)
        Dim OrderXML As New XmlDocument()
        Dim orderlines As New List(Of OrderLine)
        Dim root As XmlElement 'create the first xml document
        root = OrderXML.CreateElement("order_post") 'create the root order in the xml
        OrderXML.AppendChild(root)
        Using filereader

            Dim li As String
            li = filereader.ReadLine 'read the first line of data in
            Do While (Not li Is Nothing) 'keep reading until the end of the file

                Dim LineOfData() As String = li.Split(",") ' seperate each value of the line


                Dim o As Order 'create an order in memory


                Select Case LineOfData(0) 'check the first column to see if it is the order header
                    Case "H"
                        'is it a header line?
                        Console.WriteLine("Writing Order header data")


                        o = New Order(LineOfData(1), LineOfData(2), LineOfData(5), Today, LineOfData(3)) 'fill the order with the data from the line and then create the xml nodes

                        Dim cu As String = o.CustID
                        Dim cust As XmlNode = OrderXML.CreateElement("customer_id")
                        cust.InnerText = cu
                        root.AppendChild(cust)


                        Dim order As XmlNode = OrderXML.CreateElement("order_id")
                        order.InnerText = o.OId.ToString
                        root.AppendChild(order)

                        Dim sour As XmlNode = OrderXML.CreateElement("ORDERSOURCE")
                        sour.InnerText = o.OSo.ToString
                        root.AppendChild(sour)

                        Dim a1 As XmlNode = OrderXML.CreateElement("delivery_address_1")
                        a1.InnerText = o.OAddr1.ToString
                        root.AppendChild(a1)

                        Dim a2 As XmlNode = OrderXML.CreateElement("delivery_address_2")
                        a2.InnerText = o.OAddr2.ToString
                        root.AppendChild(a2)

                        Dim a3 As XmlNode = OrderXML.CreateElement("delivery_address_3")
                        a3.InnerText = o.OAddr3.ToString
                        root.AppendChild(a3)

                        Dim a4 As XmlNode = OrderXML.CreateElement("delivery_address_4")
                        a4.InnerText = o.OAddr4.ToString
                        root.AppendChild(a4)

                        Dim a5 As XmlNode = OrderXML.CreateElement("delivery_address_5")
                        a5.InnerText = o.OAddr5.ToString
                        root.AppendChild(a5)

                        Dim pc As XmlNode = OrderXML.CreateElement("delivery_address_postcode")
                        pc.InnerText = o.OAddrPC.ToString
                        root.AppendChild(pc)

                        Dim ol As New OrderLine(LineOfData(2), LineOfData(3), LineOfData(4), LineOfData(7), LineOfData(8), LineOfData(9))
                        orderlines.Add(ol)

                    Case Else 'if its not a header line we need to ascertain what type of line it is
                        Dim check As String
                        check = LineOfData(1).Replace(" ", "") & LineOfData(2).Replace(" ", "") & LineOfData(3).Replace(" ", "")
                        Select Case check.Length
                            Case 8 'if its zero length then its a blank line and could either be the first blank or the blank between records. If its 8 then its the line containing only the order number which means we will write our data

                                'write the data out of the lines storage and clear down the variables
                                Dim lines As XmlNode = OrderXML.CreateElement("lines")
                                root.AppendChild(lines)
                                Dim ol As OrderLine
                                For Each ol In orderlines
                                    Dim lin As XmlNode = OrderXML.CreateElement("line")
                                    lines.AppendChild(lin)
                                    Dim ordid As XmlNode = OrderXML.CreateElement("customer_reference")
                                    ordid.InnerText = ol.CustRef
                                    lin.AppendChild(ordid)
                                    Dim loc As XmlNode = OrderXML.CreateElement("location")
                                    loc.InnerText = ol.Location
                                    lin.AppendChild(loc)
                                    Dim l As XmlNode = OrderXML.CreateElement("code_l")
                                    l.InnerText = ol.ColL
                                    lin.AppendChild(l)
                                    Dim sku As XmlNode = OrderXML.CreateElement("sku")
                                    sku.InnerText = ol.ProdCode
                                    lin.AppendChild(sku)
                                    Dim qty As XmlNode = OrderXML.CreateElement("qty")
                                    qty.InnerText = ol.Quantity
                                    lin.AppendChild(qty)
                                Next
                                orderlines = Nothing
                                orderlines = New List(Of OrderLine)

                                OrderXML.Save(("c:\test\" & o.OId & ".xml"))
                                OrderXML = Nothing
                                OrderXML = New XmlDocument
                                root = Nothing
                                root = OrderXML.CreateElement("order_post")
                                OrderXML.AppendChild(root)






                                'If OrderXML.

                            Case 10
                                Dim ol As New OrderLine(LineOfData(2), LineOfData(3), LineOfData(5), LineOfData(7), LineOfData(8), LineOfData(9))
                                orderlines.Add(ol)




                        End Select


                End Select
                li = filereader.ReadLine()
                line_no += 1

            Loop
        End Using

    End Sub
    Public Shared Sub createloading(ByVal fi As String)
        'This procedure will read the data from a csv file (line by line). I will be using the first 4 columns of the csv to determine the type of data row. There are 3 apparent types contained in the csv (apart from blank rows) they are Header (has an H in the first column) which contains the header data (type 1) for the order but also contains an order line, Order line (type 2) which contains a line of item data and lastly a block ending line which just contains the cust Ref with nothing else.

        Dim ServiceURL As String = "http://10.0.0.164:8080" '"http://localhost:8080"
        Dim filereader As StreamReader
        Dim line_no As Integer = 1
        Dim exp As Exception
        filereader = New StreamReader(fi)
        Dim load As Loading = New Loading


        Using filereader
            With load
                .Table = "ZTRX_CSV_ORDERS"
                .Procedure = "ZTRX_CSV_ORDERS" ' "ZTRX_LOAD_LOS_NAD"
                .Environment = "tru"
                '*********** TYPE 1
                .AddColumn(1) = New LoadColumn("CUSTOMER_ID", tColumnType.typeCHAR)
                .AddColumn(1) = New LoadColumn("ORDER_ID", tColumnType.typeCHAR)
                .AddColumn(1) = New LoadColumn("CURDATE", tColumnType.typeDATE)
                .AddColumn(1) = New LoadColumn("LOCATION", tColumnType.typeCHAR)



                '*********** TYPE 2
                .AddColumn(2) = New LoadColumn("SKU", tColumnType.typeCHAR)
                .AddColumn(2) = New LoadColumn("QTY", tColumnType.typeINT)
                .AddColumn(2) = New LoadColumn("UNIT_PRICE", tColumnType.typeREAL)
                .AddColumn(2) = New LoadColumn("TRANS_DATE", tColumnType.typeDATE)
                .AddColumn(2) = New LoadColumn("ORDER_ID2", tColumnType.typeCHAR)



                '*********** TYPE 3
                .AddColumn(3) = New LoadColumn("REFERENCE", tColumnType.typeCHAR)
                .AddColumn(3) = New LoadColumn("ORDER_ID3", tColumnType.typeCHAR)

            End With
            Dim o As Order 'create an order in memory
            Dim ol As OrderLine
            Dim li As String
            li = filereader.ReadLine 'read the first line of data in
            Do While (Not li Is Nothing) 'keep reading until the end of the file

                Dim LineOfData() As String = li.Split(",") ' seperate each value of the line




                Select Case LineOfData(0) 'check the first column to see if it is the order header (denoted with an 'H')
                    Case "H"



                        'IF NOT FIRST PASS THEN
                        'write the address type 3
                        If line_no >= 3 Then


                            With load
                                .AddRecordType(2) = New LoadRow("CL115", "1", CInt("0.00"), ol.deldate, o.OId)
                                .AddRecordType(3) = New LoadRow(o.AdHoc, o.OId)

                                exp = New Exception

                                Try
                                    If Not .Post( _
                                                    ServiceURL, _
                                                    exp _
                                                ) Then Throw exp


                                Catch ex As Exception
                                    MsgBox(ex.Message)
                                End Try
                            End With

                            load = Nothing
                            load = New Loading
                            With load
                                .Table = "ZTRX_CSV_ORDERS"
                                .Procedure = "ZTRX_CSV_ORDERS" ' "ZTRX_LOAD_LOS_NAD"
                                .Environment = "tru"
                                '*********** TYPE 1
                                .AddColumn(1) = New LoadColumn("CUSTOMER_ID", tColumnType.typeCHAR)
                                .AddColumn(1) = New LoadColumn("ORDER_ID", tColumnType.typeCHAR)
                                .AddColumn(1) = New LoadColumn("CURDATE", tColumnType.typeDATE)
                                .AddColumn(1) = New LoadColumn("LOCATION", tColumnType.typeCHAR)



                                '*********** TYPE 2
                                .AddColumn(2) = New LoadColumn("SKU", tColumnType.typeCHAR)
                                .AddColumn(2) = New LoadColumn("QTY", tColumnType.typeINT)
                                .AddColumn(2) = New LoadColumn("UNIT_PRICE", tColumnType.typeREAL)
                                .AddColumn(2) = New LoadColumn("TRANS_DATE", tColumnType.typeDATE)
                                .AddColumn(2) = New LoadColumn("ORDER_ID2", tColumnType.typeCHAR)



                                '*********** TYPE 3
                                .AddColumn(3) = New LoadColumn("REFERENCE", tColumnType.typeCHAR)
                                .AddColumn(3) = New LoadColumn("ORDER_ID3", tColumnType.typeCHAR)
                            End With


                        End If

                        o = New Order(LineOfData(1), LineOfData(3), LineOfData(4), FormatDateTime(Today, DateFormat.ShortDate), LineOfData(5))
                        ol = New OrderLine(LineOfData(3), LineOfData(5), LineOfData(6), LineOfData(8), LineOfData(9), LineOfData(10))
                        'fill the order with the data from the line
                        With load
                            .AddRecordType(1) = New LoadRow(o.CustID, o.OId.ToString, o.odate, o.Location)
                            Console.WriteLine("Writing Order header data (type 1) for order - " & o.OId)
                            .AddRecordType(2) = New LoadRow(ol.ProdCode, ol.Quantity, CInt(ol.UnitPrice), ol.deldate, o.OId)
                        End With





                    Case Else 'if its not a header line we need to ascertain what type of line it is
                        Dim check As Integer = 0 'we will use the lengths of column 1 3 and 3 to check to see what is contained in them
                        'check = LineOfData(1).Replace(" ", "") & LineOfData(3).Replace(" ", "") & LineOfData(5).Replace(" ", "")
                        If LineOfData(1).Replace(" ", "") <> "" Then
                            check += 1
                        End If
                        If LineOfData(3).Replace(" ", "") <> "" Then
                            check += 1
                        End If
                        If LineOfData(5).Replace(" ", "") <> "" Then
                            check += 1
                        End If
                        Select Case check
                            Case 1 'if its zero length then its a blank line and could either be the first blank or the blank between records. If its 1 then its the line containing only the order number which means we will write our data
                                Dim exc As New Exception
                                'Try
                                '    With load
                                '        'add the address details
                                '        .AddRecordType(2) = New LoadRow("", "", "", "", "")
                                '        If Not .Post("", exc) Then
                                '            Throw exc
                                '        End If
                                '    End With
                                'Catch ex As Exception
                                '    MsgBox(ex.Message)
                                'End Try
                                'Clear down all variables and reset the loading for the next order (if there is one)



                            Case 2
                                'As the csv line has just the cust ref in the checking columns it will be read as an order line
                                ol = New OrderLine(LineOfData(3), LineOfData(5), LineOfData(6), LineOfData(8), LineOfData(9), LineOfData(10))
                                load.AddRecordType(2) = New LoadRow(ol.ProdCode, ol.Quantity, CInt(ol.UnitPrice), ol.deldate, o.OId)


                        End Select


                End Select


                li = filereader.ReadLine()
                line_no += 1

            Loop

            'Exiting so write last type 3
            load.AddRecordType(2) = New LoadRow("CL115", "1", CInt("0.00"), ol.deldate, o.OId)
            load.AddRecordType(3) = New LoadRow(o.AdHoc, o.OId)

            'Dim ServiceURL As String = " http://mobile.trutex.com:8080"
            exp = New Exception
            With load
                Try
                    If Not .Post( _
                                    ServiceURL, _
                                    exp _
                                ) Then Throw exp

                   
                Catch ex As Exception
                    MsgBox(ex.Message)
                End Try
            End With
            ' Send to Server

        End Using
    End Sub
End Class
