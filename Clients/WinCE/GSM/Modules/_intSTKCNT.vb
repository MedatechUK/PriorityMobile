Imports System.Collections.Generic
Imports System.Xml
Imports System.Text.RegularExpressions
Public Class ScanItem
    Public Sub New(ByVal Ordinal As Integer, ByVal Qty As String)

        _ordinal = Ordinal
        If IsNothing(Qty) Then
            Qty = 0
        ElseIf Qty.length = 0 Then
            Qty = 0
        Else
            _qty = CDec(Qty)
        End If
    End Sub

    Private _ordinal As Integer = 0
    Public Property Ordinal() As Integer
        Get
            Return _ordinal
        End Get
        Set(ByVal value As Integer)
            _ordinal = value
        End Set
    End Property
    Private _qty As Integer
    Public Property Qty() As Decimal
        Get
            Return _qty
        End Get
        Set(ByVal value As Decimal)
            _qty = value
        End Set
    End Property
End Class

Public Class InterfaceSTKCNT
    Inherits SFDCData.iForm

#Region "Initialisation"

    Private tld As Boolean = False
    Private TBar As String = ""
    Private Enum tInvoke
        iBarcode = 0
        iVendor = 1
        iPart = 2
    End Enum
    Private mInvoke As tInvoke = tInvoke.iBarcode
    Public Sub New(Optional ByRef App As Form = Nothing)


        CallerApp = App
        NewArgument("SHOWBAL", "INTERWHTX")
        NewArgument("SCANACTION", "OPENFORM")
        NewArgument("MANUAL", "N")
        CtrlTable.DisableButtons(False, False, False, False, False)
        CtrlTable.EnableToolbar(False, False, False, False, False)

    End Sub

    Public Overrides Sub FormLoaded()
        MyBase.FormLoaded()
        With CtrlForm.el(0)
            .DataEntry.Text = "Main"
            .ProcessEntry()
        End With
    End Sub

#End Region

    Public Overrides Sub FormSettings()
        'FROM WARHSNAME
        With field
            .Name = "WARHS"
            .Title = "To W/H"
            .ValidExp = ValidStr(tRegExValidation.tWarehouse)
            .SQLList = "SELECT DISTINCT WARHSNAME FROM WAREHOUSES WHERE INACTIVE <> 'Y'"
            .SQLValidation = "SELECT top 1 WARHSNAME FROM WAREHOUSES WHERE upper(WARHSNAME) = upper('%ME%') AND INACTIVE <> 'Y'"
            .Data = ""      '******** Barcoded field '*******
            .AltEntry = ctrlText.tAltCtrlStyle.ctList 'ctNone 'ctKeyb 
            .ctrlEnabled = True
        End With
        CtrlForm.AddField(field)

        'LOCNAME
        With field
            .Name = "LOCNAME"
            .Title = "To Bin"
            .ValidExp = ValidStr(tRegExValidation.tLocname)
            .SQLList = "SELECT DISTINCT LOCNAME FROM WAREHOUSES WHERE upper(WARHSNAME) = upper('%WARHS%') AND INACTIVE <> 'Y'"
            .SQLValidation = "SELECT top 1 LOCNAME FROM WAREHOUSES WHERE upper(LOCNAME) = upper('%ME%') AND upper(WARHSNAME) = upper('%WARHS%') AND INACTIVE <> 'Y'"
            .Data = ""      '******** Barcoded field '*******
            .AltEntry = ctrlText.tAltCtrlStyle.ctList 'ctNone 'ctKeyb 
            .ctrlEnabled = True
        End With
        CtrlForm.AddField(field)
    End Sub

    Public Overrides Sub TableSettings()
        ' BARCODE
        With col
            .Name = "_PARTNAME"
            .Title = "Part No"
            .initWidth = 30
            .TextAlign = HorizontalAlignment.Left
            .AltEntry = ctrlText.tAltCtrlStyle.ctNone 'ctKeyb 
            .ValidExp = ValidStr(tRegExValidation.tBarcode)
            .SQLValidation = "SELECT BARCODE, PARTNAME " & _
                            "FROM PART " & _
                            "WHERE BARCODE = '%ME%'"
            .DefaultFromCtrl = Nothing '
            .ctrlEnabled = True
            .Mandatory = True
        End With
        CtrlTable.AddCol(col)

        ' PARTDES
        With col
            .Name = "_PARTDES"
            .Title = "Part"
            .initWidth = 30
            .TextAlign = HorizontalAlignment.Left
            .AltEntry = ctrlText.tAltCtrlStyle.ctNone 'ctKeyb 
            .ValidExp = "^.+$"
            ' Second field replaces first field if first field validates ok
            .SQLValidation = "SELECT '%ME%'"
            .DefaultFromCtrl = Nothing '
            .ctrlEnabled = False
            .Mandatory = True
        End With
        CtrlTable.AddCol(col)

        ' STATUS
        With col
            .Name = "_STATUS"
            .Title = "Status"
            .initWidth = 25
            .TextAlign = HorizontalAlignment.Left
            .AltEntry = ctrlText.tAltCtrlStyle.ctList 'ctKeyb 
            .ValidExp = ValidStr(tRegExValidation.tStatus)
            .SQLList = "SELECT CUSTNAME FROM CUSTOMERS WHERE STATUSFLAG = 'Y'"
            .SQLValidation = "SELECT CUSTNAME FROM CUSTOMERS WHERE STATUSFLAG = 'Y' AND CUSTNAME = '%ME%'"
            .DefaultFromCtrl = Nothing
            .ctrlEnabled = True
            .Mandatory = False
        End With
        CtrlTable.AddCol(col)

        ' STATUS
        With col
            .Name = "_SERIALNAME"
            .Title = "W/O"
            .initWidth = 25
            .TextAlign = HorizontalAlignment.Left
            .AltEntry = ctrlText.tAltCtrlStyle.ctList 'ctKeyb 
            .ValidExp = ValidStr(tRegExValidation.tString)
            .SQLList = "SELECT distinct SERIALNAME FROM SERIAL, WARHSBAL " & _
                "WHERE WARHSBAL.SERIAL = SERIAL.SERIAL " & _
                "and WARHSBAL.PART = (SELECT PART FROM PART WHERE PARTNAME = '%_PARTNAME%') "

            .SQLValidation = "SELECT SERIALNAME FROM SERIAL, WARHSBAL " & _
                "WHERE WARHSBAL.SERIAL = SERIAL.SERIAL " & _
                "and WARHSBAL.PART = (SELECT PART FROM PART WHERE PARTNAME = '%_PARTNAME%') " & _
                " AND SERIAL.SERIALNAME = '%ME%'"

            .DefaultFromCtrl = Nothing
            .ctrlEnabled = True
            .Mandatory = False
        End With
        CtrlTable.AddCol(col)

        ' STATUS
        With col
            .Name = "_ACTNAME"
            .Title = "Lot"
            .initWidth = 25
            .TextAlign = HorizontalAlignment.Left
            .AltEntry = ctrlText.tAltCtrlStyle.ctList 'ctKeyb 
            .ValidExp = ValidStr(tRegExValidation.tString)
            .SQLList = "SELECT '0' AS ACTNAME " & _
                "UNION ALL " & _
                "SELECT DISTINCT ACTNAME  " & _
                "FROM ACT, WARHSBAL, SERIAL  " & _
                "WHERE WARHSBAL.ACT = ACT.ACT  " & _
                "AND WARHSBAL.SERIAL = SERIAL.SERIAL  " & _
                "and WARHSBAL.PART = (SELECT PART FROM PART WHERE PARTNAME = '%_PARTNAME%')  " & _
                "and SERIAL.SERIALNAME = '%_SERIALNAME%'  " & _
                "AND ACTNAME <> 0 "

            .SQLValidation = "SELECT '0' AS ACTNAME " & _
                "UNION ALL " & _
                "SELECT DISTINCT ACTNAME  " & _
                "FROM ACT, WARHSBAL, SERIAL  " & _
                "WHERE WARHSBAL.ACT = ACT.ACT  " & _
                "AND WARHSBAL.SERIAL = SERIAL.SERIAL  " & _
                "and WARHSBAL.PART = (SELECT PART FROM PART WHERE PARTNAME = '%_PARTNAME%')  " & _
                "and SERIAL.SERIALNAME = '%_SERIALNAME%'  " & _
                "AND ACTNAME <> 0 " & _
                "AND ACT.ACTNAME = '%ME%'"

            .DefaultFromCtrl = Nothing
            .ctrlEnabled = True
            .Mandatory = False
        End With
        CtrlTable.AddCol(col)

        If Argument("SHOWBAL") = "TRUE" Then
            ' TQUANT
            With col
                .Name = "_TQUANT"
                .Title = "Qty"
                .initWidth = 20
                .TextAlign = HorizontalAlignment.Left
                .AltEntry = ctrlText.tAltCtrlStyle.ctNone 'ctKeyb 
                .ValidExp = ValidStr(tRegExValidation.tString)
                .SQLValidation = ""
                .DefaultFromCtrl = Nothing
                .ctrlEnabled = True
                .Mandatory = False
            End With
            CtrlTable.AddCol(col)
        End If

        ' Counted Quantity
        With col
            .Name = "_CQUANT"
            .Title = "Updated QTY"
            .initWidth = 20
            .TextAlign = HorizontalAlignment.Left
            .AltEntry = ctrlText.tAltCtrlStyle.ctNone 'ctKeyb 
            .ValidExp = ValidStr(tRegExValidation.tString)
            .SQLValidation = "select '%ME%'"
            .DefaultFromCtrl = Nothing
            .ctrlEnabled = True
            .Mandatory = False
        End With
        CtrlTable.AddCol(col)

        ' Barcode
        With col
            .Name = "_BARCODE"
            .Title = "Barcode"
            .initWidth = 0
            .TextAlign = HorizontalAlignment.Left
            .AltEntry = ctrlText.tAltCtrlStyle.ctNone 'ctKeyb 
            .ValidExp = ValidStr(tRegExValidation.tBarcode)
            .SQLValidation = "select %ME%"
            .DefaultFromCtrl = Nothing
            .ctrlEnabled = True
            .Mandatory = False
        End With
        CtrlTable.AddCol(col)

        If Argument("SHOWBAL") = "TRUE" Then
            ' Set the query to load recordtype 2s
            CtrlTable.RecordsSQL = "select PART.PARTNAME, PART.PARTDES, CUSTOMERS.CUSTNAME, SERIAL.SERIALNAME, ACT.ACTNAME " & _
                                    "dbo.REALQUANT(BALANCE) as BALANCE, '' AS CQUANT " & _
                                    "from WARHSBAL, WAREHOUSES, CUSTOMERS, PART, SERIAL, ACT " & _
                                    "where WARHSBAL.WARHS = WAREHOUSES.WARHS " & _
                                    "and WARHSBAL.PART = PART.PART " & _
                                    "and WARHSBAL.SERIAL = SERIAL.SERIAL " & _
                                    "and WARHSBAL.ACT = ACT.ACT " & _
                                    "and WARHSBAL.CUST = CUSTOMERS.CUST " & _
                                    "and WARHSBAL.WARHS = " & _
                                    "(select WARHS from WAREHOUSES where WARHSNAME= '%WARHS%' " & _
                                    "and LOCNAME = '%LOCNAME%') " & _
                                    "ORDER BY PART.PARTNAME"
        Else
            ' Set the query to load recordtype 2s
            CtrlTable.RecordsSQL = "select PART.PARTNAME, PART.PARTDES, CUSTOMERS.CUSTNAME, SERIAL.SERIALNAME, ACT.ACTNAME, " & _
                                    "'0' AS CQUANT " & _
                                    "from WARHSBAL, WAREHOUSES, CUSTOMERS, PART, SERIAL, ACT " & _
                                    "where WARHSBAL.WARHS = WAREHOUSES.WARHS " & _
                                    "and WARHSBAL.PART = PART.PART " & _
                                    "and WARHSBAL.SERIAL = SERIAL.SERIAL " & _
                                    "and WARHSBAL.ACT = ACT.ACT " & _
                                    "and WARHSBAL.CUST = CUSTOMERS.CUST " & _
                                    "and WARHSBAL.WARHS = " & _
                                    "(select WARHS from WAREHOUSES where WARHSNAME= '%WARHS%' " & _
                                    "and LOCNAME = '%LOCNAME%') " & _
                                    "ORDER BY PART.PARTNAME"
        End If

    End Sub

    Public Overrides Sub TableRXData(ByVal Data(,) As String)
        Dim y As Integer
        Dim x As Integer
        Dim i As Integer

        Try
            If Not IsNothing(Data) Then
                For y = 0 To UBound(Data, 2)
                    Dim lvi As New ListViewItem
                    lvi.Text = "***"
                    CtrlTable.Table.Items.Add(lvi)
                    For i = 0 To CtrlTable.Table.Items.Count - 1
                        If CtrlTable.Table.Items(i).Text = "***" Then
                            CtrlTable.Table.Items(i).Text = Data(0, y)

                            For x = 1 To UBound(CtrlTable.mCol)
                                CtrlTable.Table.Items(i).SubItems.Add("")
                            Next

                            For x = 1 To UBound(Data, 1)
                                CtrlTable.Table.Items(i).SubItems(x).Text = Data(x, y)
                            Next

                            Exit For
                        End If
                    Next
                Next
            End If
        Catch e As Exception
            MsgBox(e.Message)
        End Try
    End Sub

    Public Overrides Sub AfterEdit(ByVal TableIndex As Integer)
        Dim bol As Boolean = False

        If Argument("SHOWBAL") = "TRUE" Then
            bol = Len(CtrlTable.Table.Items(TableIndex).SubItems(3).Text) > 0
        Else
            bol = Len(CtrlTable.Table.Items(TableIndex).SubItems(2).Text) > 0
        End If

        If bol Then
            Dim lvi As New ListViewItem
            lvi.Text = "***"

            CtrlTable.Table.Items.Add(lvi)
            For i As Integer = 0 To CtrlTable.Table.Items.Count - 1
                If CtrlTable.Table.Items(i).Text = "***" Then
                    CtrlTable.Table.Items(i).Text = CtrlTable.Table.Items(TableIndex).Text
                    For n As Integer = 1 To CtrlTable.Table.Items(TableIndex).SubItems.Count - 1
                        CtrlTable.Table.Items(i).SubItems.Add(CtrlTable.Table.Items(TableIndex).SubItems(n).Text)
                    Next
                End If
            Next

            lvi = CtrlTable.Table.Items(TableIndex)
            CtrlTable.Table.Items.Remove(lvi)

        End If
    End Sub

    Public Overrides Sub AfterAdd(ByVal TableIndex As Integer)

    End Sub

    Public Overrides Sub BeginAdd()
        If Argument("SHOWBAL") = "TRUE" Then
            CtrlTable.mCol(0).ctrlEnabled = True
            CtrlTable.mCol(0).Mandatory = True

            CtrlTable.mCol(1).ctrlEnabled = False
            CtrlTable.mCol(1).Mandatory = False

            CtrlTable.mCol(2).ctrlEnabled = True
            CtrlTable.mCol(2).Mandatory = True

            CtrlTable.mCol(3).ctrlEnabled = True
            CtrlTable.mCol(3).Mandatory = True

            CtrlTable.mCol(4).ctrlEnabled = True
            CtrlTable.mCol(4).Mandatory = True

            CtrlTable.mCol(5).ctrlEnabled = False
            CtrlTable.mCol(5).Mandatory = False

            CtrlTable.mCol(6).ctrlEnabled = True
            CtrlTable.mCol(6).Mandatory = True

        Else
            CtrlTable.mCol(0).ctrlEnabled = True
            CtrlTable.mCol(0).Mandatory = True

            CtrlTable.mCol(1).ctrlEnabled = False
            CtrlTable.mCol(1).Mandatory = False

            CtrlTable.mCol(2).ctrlEnabled = True
            CtrlTable.mCol(2).Mandatory = True

            CtrlTable.mCol(3).ctrlEnabled = True
            CtrlTable.mCol(3).Mandatory = False

            CtrlTable.mCol(4).ctrlEnabled = True
            CtrlTable.mCol(4).Mandatory = False

            CtrlTable.mCol(5).ctrlEnabled = True
            CtrlTable.mCol(5).Mandatory = True

        End If
    End Sub

    Public Overrides Sub BeginEdit()
        If Argument("SHOWBAL") = "TRUE" Then
            CtrlTable.mCol(0).ctrlEnabled = False
            CtrlTable.mCol(0).Mandatory = False
            CtrlTable.mCol(1).ctrlEnabled = False
            CtrlTable.mCol(1).Mandatory = False

            CtrlTable.mCol(2).ctrlEnabled = True
            CtrlTable.mCol(2).Mandatory = True
            CtrlTable.mCol(3).ctrlEnabled = True
            CtrlTable.mCol(3).Mandatory = True
            CtrlTable.mCol(4).ctrlEnabled = True
            CtrlTable.mCol(4).Mandatory = True

            CtrlTable.mCol(5).ctrlEnabled = False
            CtrlTable.mCol(5).Mandatory = False
            CtrlTable.mCol(6).ctrlEnabled = True
            CtrlTable.mCol(6).Mandatory = True
        Else
            CtrlTable.mCol(0).ctrlEnabled = False
            CtrlTable.mCol(0).Mandatory = False
            CtrlTable.mCol(1).ctrlEnabled = False
            CtrlTable.mCol(1).Mandatory = False
            CtrlTable.mCol(2).ctrlEnabled = True
            CtrlTable.mCol(2).Mandatory = True
            CtrlTable.mCol(3).ctrlEnabled = True
            CtrlTable.mCol(3).Mandatory = False
            CtrlTable.mCol(4).ctrlEnabled = True
            CtrlTable.mCol(4).Mandatory = False
            CtrlTable.mCol(5).ctrlEnabled = True
            CtrlTable.mCol(5).Mandatory = True
        End If
    End Sub

    Public Overrides Sub ProcessEntry(ByVal ctrl As SFDCData.ctrlText, ByVal Valid As Boolean)
        Select Case Valid
            Case False
                Beep()

            Case True
                Try

                    If ctrl.Name = "WARHS" Then
                        tld = False
                    End If

                    If ctrl.Name = "_PARTNAME" Then
                        CtrlTable.el(1).NSInvoke("select PARTDES " & _
                                                "from PART " & _
                                                "where PART =  " & _
                                                "(select PART from PART where PARTNAME = '" & CtrlTable.el(0).Data & "')")
                    End If

                    Try
                        If ctrl.Name = "LOCNAME" Then
                            If Not tld Then
                                CtrlTable.Table.Items.Clear()
                                CtrlForm.el(0).Enabled = False
                                CtrlForm.el(1).Enabled = False
                                CtrlTable.BeginLoadRS()
                                tld = True
                            End If
                        End If
                    Catch
                    End Try

                    If ctrl.Name = "LOCNAME" Then
                        CtrlTable.Table.Focus()
                    End If

                    ' *******************************************************************
                    ' *** Set which controls are enabled
                    'CtrlForm.el(1).CtrlEnabled = Len(CtrlForm.el(0).Data) > 0
                    'CtrlForm.el(3).CtrlEnabled = Len(CtrlForm.el(2).Data) > 0

                    'CtrlTable.el(0).CtrlEnabled = Len(CtrlTable.el(1).Data) > 0 And Len(CtrlTable.el(3).Data) > 0
                    'CtrlTable.el(1).CtrlEnabled = Len(CtrlForm.el(0).Data) > 0
                    'CtrlTable.el(2).CtrlEnabled = Len(CtrlForm.el(2).Data) > 0
                    ''CtrlTable.el(4).CtrlEnabled = Len(CtrlTable.el(3).Data) > 0
                    ''CtrlTable.el(0).CtrlEnabled = Len(CtrlTable.el(1).Data) > 0 And Len(CtrlTable.el(2).Data) > 0 And Len(CtrlTable.el(5).Data) > 0
                    'CtrlTable.el(6).CtrlEnabled = Len(CtrlTable.el(0).Data) > 0 And Len(CtrlTable.el(1).Data) > 0 And Len(CtrlTable.el(3).Data) > 0
                    ' ******************************************************************

                Catch

                End Try
        End Select
    End Sub

    Public Overrides Function verifyForm() As Boolean
        Return True
    End Function

    Public Overrides Sub ProcessForm()
        Try
            With p
                .DebugFlag = True
                .Procedure = "ZSFDC_LOAD_COUNT"
                .Table = "ZSFDC_LOAD_COUNT"
                .RecordType1 = "USERLOGIN,WARHSNAME,LOCNAME"
                .RecordType2 = "PARTNAME,STATUS,SERIALNAME,ACTNAME,CQUANT,FLAG"
                .RecordTypes = "TEXT,TEXT,TEXT,TEXT,TEXT,TEXT,TEXT,,TEXT"
            End With

            With CtrlTable
                ' Type 1 records
                Dim t1() As String = { _
                                    UserName, _
                                    CtrlForm.ItemValue("WARHS"), _
                                    CtrlForm.ItemValue("LOCNAME") _
                                    }
                p.AddRecord(1) = t1

                For y As Integer = 0 To .RowCount
                    If .ItemValue("_CQUANT", y).Length > 0 Then

                        Dim t2() As String = { _
                            .ItemValue("_PARTNAME", y), _
                            .ItemValue("_TOCUSTNAME", y), _
                            .ItemValue("_SERIALNAME", y), _
                            .ItemValue("_ACTNAME", y), _
                            CStr(CInt(.ItemValue("_CQUANT", y)) * 1000), _
                            "Y" _
                        }
                        p.AddRecord(2) = t2
                    End If
                Next
            End With

        Catch e As Exception
            MsgBox(e.Message)
        End Try
    End Sub

    Public Overrides Sub TableScan(ByVal Value As String)
        Dim v2 As String = ""
        If Value = "" Then
            Exit Sub
        End If
        Dim doc As New Xml.XmlDocument
        doc.LoadXml(Value)
        If regex.ismatch(Value, "^<") = False Then
            MsgBox("This doesnt appear to be a valid barcode")
        Else



            For Each nd As XmlNode In doc.SelectNodes("in/i")
                Dim DataType As String = nd.Attributes("n").Value
                Select Case DataType
                    Case "PART"
                        mInvoke = tInvoke.iPart
                        InvokeData("SELECT PART.BARCODE FROM PART WHERE PART.PARTNAME = '" & nd.Attributes("v").Value & "'")
                        v2 = TBar
                    Case "WARHS"
                        With CtrlForm
                            .el(0).Data = nd.Attributes("v").Value
                            .el(0).DataEntry.Text = nd.Attributes("v").Value
                            .el(0).ProcessEntry()
                        End With
                    Case "BIN"
                        With CtrlForm
                            .el(1).Data = nd.Attributes("v").Value
                            .el(1).DataEntry.Text = nd.Attributes("v").Value
                            .el(1).ProcessEntry()
                        End With
                End Select

            Next
            Dim f As Boolean = False
            Dim add As Integer = 0
            If v2 <> "" Then


                If CtrlForm.el(0).Enabled Or CtrlForm.el(1).Enabled Then
                    MsgBox("Please select a warehouse and location first.")
                    Exit Sub
                End If

                For i As Integer = 0 To CtrlTable.Table.Items.Count - 1
                    If CtrlTable.ItemValue("_BARCODE", i) = v2 Then
                        f = True
                        Select Case Argument("SCANACTION")

                            Case "OPENFORM"

                                Dim num As New frmNumber
                                With num
                                    .Text = "Box quantity."
                                    .ShowDialog()
                                    add = .number
                                    If .Manual Then Argument("MANUAL") = "Y"
                                    .Dispose()
                                End With

                            Case "INCREMENT"
                                add = 1

                        End Select
                        CtrlTable.Table.Items(i).SubItems(3).Text = CInt(CtrlTable.ItemValue("_CQUANT", i)) + add
                        Exit For
                    End If
                Next

                If Not f Then
                    mInvoke = tInvoke.iBarcode
                    InvokeData("SELECT PARTNAME, BARCODE, PARTDES FROM PART WHERE BARCODE = '" & v2 & "'")
                End If
            End If
        End If
    End Sub

    Public Overrides Sub EndInvokeData(ByVal Data(,) As String)

        'Dim add As Integer = 0
        'Select Case Argument("SCANACTION")
        '    Case "OPENFORM"
        '        Dim num As New frmNumber
        '        With num
        '            .Text = "Box quantity."
        '            .ShowDialog()
        '            add = .number
        '            If .Manual Then Argument("MANUAL") = "Y"
        '            .Dispose()
        '        End With
        '    Case "INCREMENT"
        '        add = 1
        'End Select
        Select Case mInvoke
            Case tInvoke.iBarcode
                Dim lvi As New ListViewItem
                With lvi
                    .SubItems(0).Text = Data(0, 0)
                    .SubItems.Add(Data(2, 0))
                    .SubItems.Add("")
                    .SubItems.Add("")
                    .SubItems.Add("")

                    If Argument("SHOWBAL") = "TRUE" Then
                        .SubItems.Add("")
                    End If
                    .SubItems.Add(CStr(0))
                    .SubItems.Add(CStr(Data(1, 0)))
                    .Selected = True
                End With

                With CtrlTable
                    .Table.Items.Add(lvi)
                    .Table.Items(.Table.Items.Count - 1).Selected = True
                    .EnableToolbar(False, False, False, False, True)
                    .SetEdit()
                End With

            Case tInvoke.iPart
                If Data Is Nothing Then
                    TBar = ""
                Else
                    TBar = Data(0, 0)
                End If

        End Select


    End Sub

End Class
