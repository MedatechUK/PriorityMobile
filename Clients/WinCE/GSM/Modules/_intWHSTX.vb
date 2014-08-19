Imports System
Imports System.IO
Imports System.Threading
Imports System.Text.RegularExpressions
Imports System.Xml

Public Class InterfaceWHTX
    Inherits SFDCData.iForm

    Private Enum tInvoke
        iBarcode = 0
        iLoc = 1
        iPart = 2
        iStatus = 3
    End Enum
    Private Prt As String = ""
    Private Lt As String = ""
    Private Cnt As String = ""
    Private mInvoke As tInvoke = tInvoke.iBarcode
    Private TBar As String = ""
    Private UNQList As New List(Of Integer)
    Dim unq As Integer = 0
#Region "Initialisation"

    Public Sub New(Optional ByRef App As Form = Nothing)


        CallerApp = App
        NewArgument("TXTYPE", "INTERWHTX")
        NewArgument("MANUAL", "N")
        NewArgument("WHouse", "GRVR")
        CtrlTable.DisableButtons(False, False, False, False, False)
        CtrlTable.EnableToolbar(False, False, False, False, False)

    End Sub

    Public Sub hEndForm() Handles MyBase.EndForm
        Argument("MANUAL") = "N"
    End Sub
    Public Overrides Sub FormLoaded()
        UNQList.add(0)

        Select Case Argument("TXTYPE")
            Case "PUTAWAY"
                With CtrlForm
                    .el(0).DataEntry.Text = Argument("WHouse")
                    .el(0).ProcessEntry()
                    .el(1).DataEntry.Text = "0"
                    .el(1).ProcessEntry()
                End With
        End Select
        CtrlTable.Focus()
    End Sub
#End Region

    Public Overrides Sub FormSettings()

        Select Case Argument("TXTYPE")

            Case "INTERWHTX"
                'FROM WARHSNAME
                With field
                    .Name = "WARHS"
                    .Title = "From W/H"
                    .ValidExp = ValidStr(tRegExValidation.tWarehouse)
                    .SQLList = "SELECT DISTINCT WARHSNAME FROM WAREHOUSES WHERE INACTIVE <> 'Y'"
                    .SQLValidation = "SELECT WARHSNAME FROM WAREHOUSES WHERE WARHSNAME = '%ME%'"
                    .Data = ""      '******** Barcoded field '*******
                    .AltEntry = ctrlText.tAltCtrlStyle.ctList 'ctNone 'ctKeyb 
                    .ctrlEnabled = True
                End With
                CtrlForm.AddField(field)

                'LOCNAME
                With field
                    .Name = "LOCNAME"
                    .Title = "From Loc"
                    .ValidExp = ValidStr(tRegExValidation.tString)
                    .SQLList = "SELECT DISTINCT LOCNAME FROM WAREHOUSES WHERE WARHSNAME = '%WARHS%' AND INACTIVE <> 'Y'"
                    .SQLValidation = "SELECT '%ME%'"
                    .Data = ""      '******** Barcoded field '*******
                    .AltEntry = ctrlText.tAltCtrlStyle.ctList 'ctNone 'ctKeyb 
                    .ctrlEnabled = True
                End With
                CtrlForm.AddField(field)

                'STATDES
                With field
                    .Name = "STATUS"
                    .Title = "Status"
                    .ValidExp = ValidStr(tRegExValidation.tStatus)
                    .SQLList = "select DISTINCT CUSTOMERS.CUSTNAME " & _
                                    "from WARHSBAL, WAREHOUSES, CUSTOMERS " & _
                                    "WHERE WARHSBAL.WARHS = WAREHOUSES.WARHS " & _
                                    "AND CUSTOMERS.CUST = WARHSBAL.CUST " & _
                                    "AND WAREHOUSES.WARHSNAME = '%WARHS%' " & _
                                    "AND WAREHOUSES.LOCNAME = '%LOCNAME%' "
                    .SQLValidation = "select DISTINCT CUSTOMERS.CUSTNAME " & _
                                    "from WARHSBAL, WAREHOUSES, CUSTOMERS " & _
                                    "WHERE WARHSBAL.WARHS = WAREHOUSES.WARHS " & _
                                    "AND CUSTOMERS.CUST = WARHSBAL.CUST " & _
                                    "AND WAREHOUSES.WARHSNAME = '%WARHS%' " & _
                                    "AND LOCNAME = '%LOCNAME%' " & _
                                    "AND CUSTOMERS.CUSTNAME = '%ME%'"
                    .Data = ""      '******** Default to 'Draft' '*******
                    .AltEntry = ctrlText.tAltCtrlStyle.ctList '.tAltCtrlStyle.ctNone 'ctKeyb 
                    .ctrlEnabled = False
                End With
                CtrlForm.AddField(field)

                'TOWARHSNAME
                With field
                    .Name = "TOWARHS"
                    .Title = "To W/H"
                    .ValidExp = ValidStr(tRegExValidation.tWarehouse)
                    .SQLList = "SELECT DISTINCT WARHSNAME FROM WAREHOUSES WHERE INACTIVE <> 'Y'"
                    .SQLValidation = "SELECT WARHSNAME FROM WAREHOUSES WHERE WARHSNAME = '%ME%'"
                    .Data = ""      '******** Barcoded field '*******
                    .AltEntry = ctrlText.tAltCtrlStyle.ctList 'ctKeyb 
                    .ctrlEnabled = False
                End With
                CtrlForm.AddField(field)

                'TOLOCATION
                With field
                    .Name = "TOLOCNAME"
                    .Title = "To Loc"
                    .ValidExp = ValidStr(tRegExValidation.tString)
                    .SQLList = "SELECT DISTINCT LOCNAME FROM WAREHOUSES WHERE upper(WARHSNAME) = upper('%WARHS%') AND INACTIVE <> 'Y'"
                    .SQLValidation = "SELECT '%ME%'"
                    .Data = ""      '******** Barcoded field '*******
                    .AltEntry = ctrlText.tAltCtrlStyle.ctList 'ctKeyb 
                    .ctrlEnabled = False
                End With
                CtrlForm.AddField(field)

                '**************************************************************************
            Case "PUTAWAY"

                'FROM WARHSNAME
                With field
                    .Name = "WARHS"
                    .Title = "From W/H"
                    .ValidExp = ValidStr(tRegExValidation.tWarehouse)
                    '.SQLList = "SELECT DISTINCT WARHSNAME FROM WAREHOUSES WHERE INACTIVE <> 'Y'"
                    .SQLValidation = "SELECT WARHSNAME FROM WAREHOUSES WHERE WARHSNAME = '%ME%'"
                    .Data = ""      '******** Barcoded field '*******
                    .AltEntry = ctrlText.tAltCtrlStyle.ctNone 'ctNone 'ctKeyb 
                    .ctrlEnabled = True
                End With
                CtrlForm.AddField(field)

                'LOCNAME
                With field
                    .Name = "LOCNAME"
                    .Title = "From Loc"
                    .ValidExp = ValidStr(tRegExValidation.tString)
                    '.SQLList = "SELECT DISTINCT LOCNAME FROM WAREHOUSES WHERE WARHSNAME = '%WARHS%' AND WAREHOUSES.INACTIVE <> 'Y'"
                    .SQLValidation = "SELECT LOCNAME FROM WAREHOUSES WHERE LOCNAME = '%ME%' AND WARHSNAME = '%WARHS%' AND WAREHOUSES.INACTIVE <> 'Y'"
                    .Data = ""      '******** Barcoded field '*******
                    .AltEntry = ctrlText.tAltCtrlStyle.ctNone 'ctNone 'ctKeyb 
                    .ctrlEnabled = True
                End With
                CtrlForm.AddField(field)

                'TOWARHSNAME
                With field
                    .Name = "TOWARHS"
                    .Title = "To W/H"
                    .ValidExp = ValidStr(tRegExValidation.tWarehouse)
                    .SQLList = "SELECT DISTINCT WARHSNAME FROM WAREHOUSES WHERE INACTIVE <> 'Y'"
                    .SQLValidation = "SELECT WARHSNAME FROM WAREHOUSES WHERE WARHSNAME = '%ME%'"
                    .Data = ""      '******** Barcoded field '*******
                    .AltEntry = ctrlText.tAltCtrlStyle.ctList 'ctKeyb 
                    .ctrlEnabled = True
                End With
                CtrlForm.AddField(field)

                'TOLOCATION
                With field
                    .Name = "TOLOCNAME"
                    .Title = "To Loc"
                    .ValidExp = ValidStr(tRegExValidation.tString)
                    .SQLList = "SELECT DISTINCT LOCNAME FROM WAREHOUSES WHERE WARHSNAME = '%TOWARHS%' AND WAREHOUSES.INACTIVE <> 'Y'"
                    .SQLValidation = "SELECT LOCNAME FROM WAREHOUSES WHERE LOCNAME = '%ME%' AND WARHSNAME = '%TOWARHS%' AND WAREHOUSES.INACTIVE <> 'Y'"
                    .Data = ""      '******** Barcoded field '*******
                    .AltEntry = ctrlText.tAltCtrlStyle.ctList 'ctKeyb 
                    .ctrlEnabled = True
                End With
                CtrlForm.AddField(field)

                '**************************************************************************
            Case "CHSTATUS"

                'FROM WARHSNAME
                With field
                    .Name = "WARHS"
                    .Title = "Warehouse"
                    .ValidExp = ValidStr(tRegExValidation.tWarehouse)
                    .SQLList = "SELECT DISTINCT WARHSNAME FROM WAREHOUSES WHERE INACTIVE <> 'Y'"
                    .SQLValidation = "SELECT WARHSNAME FROM WAREHOUSES WHERE WARHSNAME = '%ME%'"
                    .Data = ""      '******** Barcoded field '*******
                    .AltEntry = ctrlText.tAltCtrlStyle.ctList 'ctNone 'ctKeyb 
                    .ctrlEnabled = True
                End With
                CtrlForm.AddField(field)

                'LOCNAME
                With field
                    .Name = "LOCNAME"
                    .Title = "Location"
                    .ValidExp = ValidStr(tRegExValidation.tLocname)
                    .SQLList = "SELECT DISTINCT LOCNAME FROM WAREHOUSES WHERE WARHSNAME = '%WARHS%' AND INACTIVE <> 'Y'"
                    .SQLValidation = "SELECT LOCNAME FROM WAREHOUSES WHERE LOCNAME = '%ME%' AND WARHSNAME = '%WARHS%'"
                    .Data = ""      '******** Barcoded field '*******
                    .AltEntry = ctrlText.tAltCtrlStyle.ctList 'ctNone 'ctKeyb 
                    .ctrlEnabled = False
                End With
                CtrlForm.AddField(field)

                'STATDES
                With field
                    .Name = "STATUS"
                    .Title = "From Status"
                    .ValidExp = ValidStr(tRegExValidation.tStatus)
                    .SQLList = "select DISTINCT CUSTOMERS.CUSTNAME " & _
                                    "from WARHSBAL, WAREHOUSES, CUSTOMERS " & _
                                    "WHERE WARHSBAL.WARHS = WAREHOUSES.WARHS " & _
                                    "AND CUSTOMERS.CUST = WARHSBAL.CUST " & _
                                    "AND WAREHOUSES.WARHSNAME = '%WARHS%' " & _
                                    "AND WAREHOUSES.LOCNAME = '%LOCNAME%' "
                    .SQLValidation = "select DISTINCT CUSTOMERS.CUSTNAME " & _
                                    "from WARHSBAL, WAREHOUSES, CUSTOMERS " & _
                                    "WHERE WARHSBAL.WARHS = WAREHOUSES.WARHS " & _
                                    "AND CUSTOMERS.CUST = WARHSBAL.CUST " & _
                                    "AND WAREHOUSES.WARHSNAME = '%WARHS%' " & _
                                    "AND LOCNAME = '%LOCNAME%' " & _
                                    "AND CUSTOMERS.CUSTNAME = '%ME%'"
                    .Data = ""      '******** Default to 'Draft' '*******
                    .AltEntry = ctrlText.tAltCtrlStyle.ctList '.tAltCtrlStyle.ctNone 'ctKeyb 
                    .ctrlEnabled = False
                End With
                CtrlForm.AddField(field)

                'STATDES
                With field
                    .Name = "TOSTATUS"
                    .Title = "To Status"
                    .ValidExp = ValidStr(tRegExValidation.tStatus)
                    .SQLList = "SELECT CUSTNAME FROM CUSTOMERS WHERE STATUSFLAG = 'Y'"
                    .SQLValidation = "SELECT CUSTNAME FROM CUSTOMERS WHERE STATUSFLAG = 'Y' " & _
                                    "AND CUSTOMERS.CUSTNAME = '%ME%'"
                    .Data = ""      '******** Default to 'Draft' '*******
                    .AltEntry = ctrlText.tAltCtrlStyle.ctList '.tAltCtrlStyle.ctNone 'ctKeyb 
                    .ctrlEnabled = False
                End With
                CtrlForm.AddField(field)
        End Select

    End Sub

    Public Overrides Sub TableSettings()

        Select Case Argument("TXTYPE")
            '**************************************************************************
            Case "INTERWHTX"

                ' BARCODE
                With col
                    .Name = "_PARTNAME"
                    .Title = "Part No"
                    .initWidth = 30
                    .TextAlign = HorizontalAlignment.Left
                    .AltEntry = ctrlText.tAltCtrlStyle.ctNone 'ctKeyb 
                    .ValidExp = ValidStr(tRegExValidation.tBarcode)
                    ' Second field replaces first field if first field validates ok
                    .SQLValidation = "SELECT BARCODE, PARTNAME " & _
                                    "FROM PART, WARHSBAL, WAREHOUSES, CUSTOMERS " & _
                                    "WHERE PART.PART = WARHSBAL.PART  " & _
                                    "AND WARHSBAL.WARHS = WAREHOUSES.WARHS  " & _
                                    "AND WARHSBAL.CUST = CUSTOMERS.CUST " & _
                                    "AND WAREHOUSES.WARHSNAME = '%WARHS%'  " & _
                                    "AND WAREHOUSES.LOCNAME = '%LOCNAME%'  " & _
                                    "AND WARHSBAL.BALANCE > 0 " & _
                                    "AND CUSTNAME = '%STATUS%' " & _
                                    "AND BARCODE = '%ME%'"
                    .DefaultFromCtrl = Nothing '
                    .ctrlEnabled = True
                    .Mandatory = True
                End With
                CtrlTable.AddCol(col)

                ' Availibility
                With col
                    .Name = "_AVAILIBLE"
                    .Title = "Availible"
                    .initWidth = 30
                    .TextAlign = HorizontalAlignment.Left
                    .AltEntry = ctrlText.tAltCtrlStyle.ctNone 'ctKeyb 
                    .ValidExp = ValidStr(tRegExValidation.tNumeric)
                    .SQLValidation = ""
                    .DefaultFromCtrl = Nothing
                    .ctrlEnabled = False
                    .Mandatory = False
                End With
                CtrlTable.AddCol(col)

                ' TQUANT
                With col
                    .Name = "_TQUANT"
                    .Title = "Qty"
                    .initWidth = 30
                    .TextAlign = HorizontalAlignment.Left
                    .AltEntry = ctrlText.tAltCtrlStyle.ctNone 'ctKeyb 
                    .ValidExp = ValidStr(tRegExValidation.tNumeric)
                    .SQLValidation = "SELECT %ME% " & _
                                    "FROM PART, WARHSBAL, WAREHOUSES, CUSTOMERS " & _
                                    "WHERE PART.PART = WARHSBAL.PART  " & _
                                    "AND WARHSBAL.WARHS = WAREHOUSES.WARHS  " & _
                                    "AND WARHSBAL.CUST = CUSTOMERS.CUST " & _
                                    "AND WAREHOUSES.WARHSNAME = '%WARHS%'  " & _
                                    "AND WAREHOUSES.LOCNAME = '%LOCNAME%'  " & _
                                    "AND WARHSBAL.BALANCE/1000 >= %ME% " & _
                                    "AND CUSTNAME = '%STATUS%' " & _
                                    "AND PART.PARTNAME = '%_PARTNAME%'"
                    .DefaultFromCtrl = Nothing
                    .ctrlEnabled = True
                    .Mandatory = True
                End With
                CtrlTable.AddCol(col)

                'TOWARHSNAME
                With col
                    .Name = "_TOWARHS"
                    .Title = "To W/H"
                    .initWidth = 30
                    .TextAlign = HorizontalAlignment.Left
                    .ValidExp = ValidStr(tRegExValidation.tWarehouse)
                    .SQLList = "SELECT DISTINCT WARHSNAME FROM WAREHOUSES WHERE INACTIVE <> 'Y'"
                    .SQLValidation = "SELECT WARHSNAME FROM WAREHOUSES WHERE WARHSNAME = '%ME%' AND INACTIVE <> 'Y'"
                    .AltEntry = ctrlText.tAltCtrlStyle.ctList 'ctKeyb 
                    .DefaultFromCtrl = CtrlForm.el(3)
                    .ctrlEnabled = True
                    .Mandatory = False
                    .MandatoryOnPost = True
                End With
                CtrlTable.AddCol(col)

                ' TOLOCNAME
                With col
                    .Name = "_TOLOCNAME"
                    .Title = "To Loc"
                    .initWidth = 30
                    .TextAlign = HorizontalAlignment.Left
                    .AltEntry = ctrlText.tAltCtrlStyle.ctList 'ctKeyb 
                    .ValidExp = ValidStr(tRegExValidation.tString)
                    .SQLList = "SELECT DISTINCT LOCNAME FROM WAREHOUSES WHERE WARHSNAME = '%TOWARHS%' AND INACTIVE <> 'Y'"
                    .SQLValidation = "SELECT '%ME%'"
                    .DefaultFromCtrl = CtrlForm.el(4)  '******* Barcoded Field - default from Type1 TOLOCATION '*******
                    .ctrlEnabled = True
                    .Mandatory = False
                    .MandatoryOnPost = True
                End With
                CtrlTable.AddCol(col)

                'Lot
                With col
                    .Name = "_LOT"
                    .Title = "Lot"
                    .initWidth = 30
                    .TextAlign = HorizontalAlignment.Left
                    .AltEntry = ctrlText.tAltCtrlStyle.ctNone 'ctKeyb 
                    .ValidExp = ValidStr(tRegExValidation.tString)
                    .SQLValidation = "SELECT '%ME%'"
                    .DefaultFromCtrl = Nothing  '******* Barcoded Field - default from Type1 TOLOCATION '*******
                    .ctrlEnabled = True
                    .Mandatory = False
                    .MandatoryOnPost = True
                End With
                CtrlTable.AddCol(col)

                '**************************************************************************
            Case "PUTAWAY"

                With col
                    .Name = "_PARTNAME"
                    .Title = "Part No"
                    .initWidth = 30
                    .TextAlign = HorizontalAlignment.Left
                    .AltEntry = ctrlText.tAltCtrlStyle.ctNone 'ctKeyb 
                    .ValidExp = ValidStr(tRegExValidation.tString)
                    ' Second field replaces first field if first field validates ok
                    .SQLValidation = "select 1 = 1"
                    '.SQLValidation = "SELECT BARCODE, PARTNAME " & _
                    '                "FROM PART, WARHSBAL, WAREHOUSES, CUSTOMERS " & _
                    '                "WHERE PART.PART = WARHSBAL.PART  " & _
                    '                "AND WARHSBAL.WARHS = WAREHOUSES.WARHS  " & _
                    '                "AND WARHSBAL.CUST = CUSTOMERS.CUST " & _
                    '                "AND WAREHOUSES.WARHSNAME = " & _
                    '                "(SELECT WARHSNAME FROM v_USERS where USERLOGIN = '" & UserName & "')  " & _
                    '                "AND WAREHOUSES.LOCNAME = '%LOCNAME%'  " & _
                    '                "AND WARHSBAL.BALANCE > 0 " & _
                    '                "AND CUSTOMERS.CUST= -1 " & _
                    '                "AND BARCODE = '%ME%'"
                    ' CUST = -1 : Only put away goods

                    .DefaultFromCtrl = Nothing '
                    .ctrlEnabled = True
                    .Mandatory = True
                End With
                CtrlTable.AddCol(col)

                ' Availibility
                With col
                    .Name = "_AVAILIBLE"
                    .Title = "Availible"
                    .initWidth = 30
                    .TextAlign = HorizontalAlignment.Left
                    .AltEntry = ctrlText.tAltCtrlStyle.ctNone 'ctKeyb 
                    .ValidExp = ValidStr(tRegExValidation.tNumeric)
                    .SQLValidation = ""
                    .DefaultFromCtrl = Nothing
                    .ctrlEnabled = False
                    .Mandatory = False
                End With
                CtrlTable.AddCol(col)

                ' Transfer
                With col
                    .Name = "_TQUANT"
                    .Title = "Transfer"
                    .initWidth = 30
                    .TextAlign = HorizontalAlignment.Left
                    .AltEntry = ctrlText.tAltCtrlStyle.ctNone 'ctKeyb 
                    .ValidExp = ValidStr(tRegExValidation.tNumeric)
                    .SQLValidation = "SELECT %ME% " & _
                                    "FROM PART, WARHSBAL, WAREHOUSES, CUSTOMERS " & _
                                    "WHERE PART.PART = WARHSBAL.PART  " & _
                                    "AND WARHSBAL.WARHS = WAREHOUSES.WARHS  " & _
                                    "AND WARHSBAL.CUST = CUSTOMERS.CUST " & _
                                    "AND WAREHOUSES.WARHSNAME = " & _
                                    "(SELECT WARHSNAME FROM v_USERS where USERLOGIN = '" & UserName & "') " & _
                                    "AND WAREHOUSES.LOCNAME = '%LOCNAME%'  " & _
                                    "AND WARHSBAL.BALANCE/1000 >= %ME% " & _
                                    "AND CUSTOMERS.CUST = -1 " & _
                                    "AND PART.PARTNAME = '%_PARTNAME%'"
                    .DefaultFromCtrl = Nothing
                    .ctrlEnabled = True
                    .Mandatory = False
                End With
                CtrlTable.AddCol(col)

                With col
                    .Name = "_TOWARHS"
                    .Title = "To W/H"
                    .initWidth = 30
                    .TextAlign = HorizontalAlignment.Left
                    .ValidExp = ValidStr(tRegExValidation.tWarehouse)
                    .SQLList = "SELECT DISTINCT WARHSNAME FROM WAREHOUSES AND INACTIVE <> 'Y'"
                    .SQLValidation = "SELECT WARHSNAME FROM WAREHOUSES WHERE WARHSNAME = '%ME%' AND INACTIVE <> 'Y'"
                    .AltEntry = ctrlText.tAltCtrlStyle.ctList 'ctKeyb 
                    .DefaultFromCtrl = Nothing
                    .ctrlEnabled = True
                    .Mandatory = False
                    .MandatoryOnPost = True
                End With
                CtrlTable.AddCol(col)

                ' TOLOCNAME
                With col
                    .Name = "_TOLOCNAME"
                    .Title = "To Loc"
                    .initWidth = 30
                    .TextAlign = HorizontalAlignment.Left
                    .AltEntry = ctrlText.tAltCtrlStyle.ctList 'ctKeyb 
                    .ValidExp = ValidStr(tRegExValidation.tString)
                    .SQLList = "SELECT DISTINCT LOCNAME FROM WAREHOUSES WHERE WARHSNAME = '%TOWARHS%' AND INACTIVE <> 'Y'"
                    .SQLValidation = "SELECT LOCNAME FROM WAREHOUSES WHERE LOCNAME = '%ME%' AND WARHSNAME = '%TOWARHS%'"
                    .DefaultFromCtrl = Nothing  '******* Barcoded Field - default from Type1 TOLOCATION '*******
                    .ctrlEnabled = True
                    .Mandatory = False
                    .MandatoryOnPost = True
                End With
                CtrlTable.AddCol(col)

                'Lot
                With col
                    .Name = "_LOT"
                    .Title = "Lot"
                    .initWidth = 30
                    .TextAlign = HorizontalAlignment.Left
                    .AltEntry = ctrlText.tAltCtrlStyle.ctNone 'ctKeyb 
                    .ValidExp = ValidStr(tRegExValidation.tString)
                    .SQLValidation = "SELECT '%ME%'"
                    .DefaultFromCtrl = Nothing  '******* Barcoded Field - default from Type1 TOLOCATION '*******
                    .ctrlEnabled = True
                    .Mandatory = False
                    .MandatoryOnPost = True
                End With
                CtrlTable.AddCol(col)


                '**************************************************************************
            Case "CHSTATUS"

                With col
                    .Name = "_PARTNAME"
                    .Title = "Part No"
                    .initWidth = 30
                    .TextAlign = HorizontalAlignment.Left
                    .AltEntry = ctrlText.tAltCtrlStyle.ctNone 'ctKeyb 
                    .ValidExp = ValidStr(tRegExValidation.tBarcode)
                    ' Second field replaces first field if first field validates ok
                    .SQLValidation = "SELECT BARCODE, PARTNAME " & _
                                    "FROM PART, WARHSBAL, WAREHOUSES, CUSTOMERS " & _
                                    "WHERE PART.PART = WARHSBAL.PART  " & _
                                    "AND WARHSBAL.WARHS = WAREHOUSES.WARHS  " & _
                                    "AND WARHSBAL.CUST = CUSTOMERS.CUST " & _
                                    "AND WAREHOUSES.WARHSNAME = '%WARHS%'  " & _
                                    "AND WAREHOUSES.LOCNAME = '%LOCNAME%'  " & _
                                    "AND WARHSBAL.BALANCE > 0 " & _
                                    "AND CUSTNAME = '%STATUS%' " & _
                                    "AND BARCODE = '%ME%'"
                    .DefaultFromCtrl = Nothing '
                    .ctrlEnabled = True
                    .Mandatory = True
                End With
                CtrlTable.AddCol(col)

                ' Availibility
                With col
                    .Name = "_AVAILIBLE"
                    .Title = "Availible"
                    .initWidth = 30
                    .TextAlign = HorizontalAlignment.Left
                    .AltEntry = ctrlText.tAltCtrlStyle.ctNone 'ctKeyb 
                    .ValidExp = ValidStr(tRegExValidation.tNumeric)
                    .SQLValidation = ""
                    .DefaultFromCtrl = Nothing
                    .ctrlEnabled = False
                    .Mandatory = False
                End With
                CtrlTable.AddCol(col)

                ' TQUANT
                With col
                    .Name = "_TQUANT"
                    .Title = "Qty"
                    .initWidth = 30
                    .TextAlign = HorizontalAlignment.Left
                    .AltEntry = ctrlText.tAltCtrlStyle.ctNone 'ctKeyb 
                    .ValidExp = ValidStr(tRegExValidation.tNumeric)
                    .SQLValidation = "SELECT %ME% " & _
                                    "FROM PART, WARHSBAL, WAREHOUSES, CUSTOMERS " & _
                                    "WHERE PART.PART = WARHSBAL.PART  " & _
                                    "AND WARHSBAL.WARHS = WAREHOUSES.WARHS  " & _
                                    "AND WARHSBAL.CUST = CUSTOMERS.CUST " & _
                                    "AND WAREHOUSES.WARHSNAME = '%WARHS%'  " & _
                                    "AND WAREHOUSES.LOCNAME = '%LOCNAME%'  " & _
                                    "AND WARHSBAL.BALANCE/1000 >= %ME% " & _
                                    "AND CUSTNAME = '%STATUS%' " & _
                                    "AND PART.PARTNAME = '%_PARTNAME%'"
                    .DefaultFromCtrl = Nothing
                    .ctrlEnabled = True
                    .Mandatory = True
                End With
                CtrlTable.AddCol(col)

                ''STATUS1
                'With col
                '    .Name = "_STATUS"
                '    .Title = "To Status"
                '    .initWidth = 30
                '    .TextAlign = HorizontalAlignment.Left
                '    .AltEntry = ctrlText.tAltCtrlStyle.ctList ' ctNone 'ctKeyb 
                '    .ValidExp = ValidStr(tRegExValidation.tStatus)
                '    .SQLValidation = "SELECT CUSTNAME FROM CUSTOMERS WHERE STATUSFLAG = 'Y' AND CUSTNAME = '%ME%'"
                '    .SQLList = "SELECT CUSTNAME FROM CUSTOMERS WHERE STATUSFLAG = 'Y'"
                '    .DefaultFromCtrl = CtrlForm.el(2)  '******** Default to 'Goods' '*******
                '    .ctrlEnabled = True
                '    .Mandatory = True
                '    .MandatoryOnPost = True
                'End With
                'CtrlTable.AddCol(col)

        End Select

    End Sub

    Public Overrides Sub TableRXData(ByVal Data(,) As String)

        Try
            For y As Integer = 0 To UBound(Data, 2)
                Dim lvi As New ListViewItem
                With CtrlTable.Table
                    .Items.Add(lvi)
                    Select Case Argument("TXTYPE")
                        Case "INTERWHTX"
                            '.Items(.Items.Count - 1).Text = Data(0, y)
                            '.Items(.Items.Count - 1).SubItems.Add("")
                            '.Items(.Items.Count - 1).SubItems.Add(Data(1, y))
                            '.Items(.Items.Count - 1).SubItems.Add("0")
                            '.Items(.Items.Count - 1).SubItems.Add(Data(2, y))

                        Case "PUTAWAY"

                        Case "CHSTATUS"

                    End Select
                End With
            Next
        Catch e As Exception
            OverControl.msgboxa(e.Message)
        End Try

    End Sub

    Public Overrides Sub ProcessEntry(ByVal ctrl As SFDCData.ctrlText, ByVal Valid As Boolean)

        Select Case Valid
            Case False
                Beep()

            Case True
                Try
                    Select Case Argument("TXTYPE")

                        Case "INTERWHTX"

                            ' Fill in the TOLOC when default changes (if empty)
                            If ctrl.Name = "TOLOCNAME" Then
                                Dim Y As Integer
                                For Y = 0 To CtrlTable.Table.Items.Count - 1
                                    If Len(CtrlTable.Table.Items(Y).SubItems(3).Text) = 0 And Len(CtrlTable.Table.Items(Y).SubItems(4).Text) = 0 Then
                                        CtrlTable.Table.Items(Y).SubItems(3).Text = CtrlForm.el(3).Value.Text
                                        CtrlTable.Table.Items(Y).SubItems(4).Text = CtrlForm.el(4).Value.Text
                                    End If
                                Next
                            End If

                            If ctrl.Name = "STATUS" Then
                                CtrlForm.DoLostFocus()
                                CtrlTable.Focus()
                            End If

                            Try
                                If Len(CtrlForm.el(0).Data) > 0 And Len(CtrlForm.el(1).Data) > 0 And Len(CtrlForm.el(2).Data) > 0 And Len(CtrlTable.el(0).Data) > 0 Then
                                    Dim sql As String = "SELECT WARHSBAL.BALANCE/1000 " & _
                                        "FROM PART, WARHSBAL, WAREHOUSES, CUSTOMERS " & _
                                        "WHERE PART.PART = WARHSBAL.PART  " & _
                                        "AND WARHSBAL.WARHS = WAREHOUSES.WARHS  " & _
                                        "AND WARHSBAL.CUST = CUSTOMERS.CUST " & _
                                        "AND WAREHOUSES.WARHSNAME = '" & CtrlForm.el(0).Data & "'  " & _
                                        "AND WAREHOUSES.LOCNAME = '" & CtrlForm.el(1).Data & "'  " & _
                                        "AND CUSTNAME = '" & CtrlForm.el(2).Data & "' " & _
                                        "AND PART.PARTNAME = '" & CtrlTable.el(0).Data & "'"
                                    CtrlTable.el(1).NSInvoke(sql)
                                    If CtrlTable.el(2).Data.Length = 0 Then
                                        Dim num As New frmNumber
                                        With num
                                            .Text = "Box quantity."
                                            .ShowDialog()
                                            CtrlTable.el(2).DataEntry.Text = CStr(.number)
                                            If .Manual Then Argument("MANUAL") = "Y"
                                            .Dispose()
                                        End With
                                        CtrlTable.SetTable()
                                    End If
                                End If
                            Catch
                            End Try

                            ' *******************************************************************
                            ' *** Set which controls are enabled                 
                            CtrlTable.EnableToolbar( _
                                CBool(CtrlTable.Table.Visible And _
                                Len(CtrlForm.el(0).Data) > 0 And _
                                Len(CtrlForm.el(1).Data) > 0 And _
                                Len(CtrlForm.el(2).Data) > 0), _
                                CtrlTable.btnEnabled(SFDCData.CtrlTable.tButton.btnEdit), _
                                CtrlTable.btnEnabled(SFDCData.CtrlTable.tButton.btnCopy), _
                                CtrlTable.btnEnabled(SFDCData.CtrlTable.tButton.btnDelete), _
                                CtrlTable.btnEnabled(SFDCData.CtrlTable.tButton.btnPost) _
                            )

                            CtrlForm.el(1).CtrlEnabled = Len(CtrlForm.el(0).Data) > 0
                            CtrlForm.el(2).CtrlEnabled = Len(CtrlForm.el(1).Data) > 0

                            Dim rcount As Integer
                            Try
                                rcount = UBound(CtrlTable.el) + 1
                            Catch
                                rcount = 0
                            End Try
                            CtrlForm.el(3).CtrlEnabled = rcount > 0

                            CtrlForm.el(4).CtrlEnabled = Len(CtrlForm.el(3).Data) > 0

                            CtrlTable.el(4).CtrlEnabled = Len(CtrlTable.el(3).Data) > 0


                        Case "PUTAWAY"

                            If ctrl.Name = "LOCNAME" Then CtrlTable.Table.Focus()

                            CtrlTable.EnableToolbar( _
                                CBool(CtrlTable.Table.Visible And _
                                Len(CtrlForm.el(0).Data) > 0), _
                                CtrlTable.btnEnabled(SFDCData.CtrlTable.tButton.btnEdit), _
                                CtrlTable.btnEnabled(SFDCData.CtrlTable.tButton.btnCopy), _
                                CtrlTable.btnEnabled(SFDCData.CtrlTable.tButton.btnDelete), _
                                CtrlTable.btnEnabled(SFDCData.CtrlTable.tButton.btnPost) _
                            )

                            CtrlForm.el(0).CtrlEnabled = Not (Len(CtrlForm.el(0).Data) > 0)
                            CtrlForm.el(1).CtrlEnabled = False

                            CtrlTable.el(3).CtrlEnabled = Len(CtrlForm.el(1).Data) > 0

                            Try
                                If Len(CtrlForm.el(0).Data) > 0 And Len(CtrlTable.el(0).Data) > 0 Then
                                    Dim sql As String = "SELECT WARHSBAL.BALANCE/1000 " & _
                                        "FROM PART, WARHSBAL, WAREHOUSES, CUSTOMERS " & _
                                        "WHERE PART.PART = WARHSBAL.PART  " & _
                                        "AND WARHSBAL.WARHS = WAREHOUSES.WARHS  " & _
                                        "AND WARHSBAL.CUST = CUSTOMERS.CUST " & _
                                        "AND WAREHOUSES.WARHSNAME = " & _
                                        "(SELECT WARHSNAME FROM v_USERS where USERLOGIN = '" & UserName & "') " & _
                                        "AND WAREHOUSES.LOCNAME = '" & CtrlForm.el(0).Data & "'  " & _
                                        "AND CUSTOMERS.CUST = -1 " & _
                                        "AND PART.PARTNAME = '" & CtrlTable.el(0).Data & "'"
                                    CtrlTable.el(1).NSInvoke(sql)
                                    If CtrlTable.el(2).Data.Length = 0 Then
                                        Dim num As New frmNumber
                                        With num
                                            .Text = "Box quantity."
                                            .ShowDialog()
                                            CtrlTable.el(2).DataEntry.Text = CStr(.number)
                                            If .Manual Then Argument("MANUAL") = "Y"
                                            .Dispose()
                                        End With
                                        CtrlTable.SetTable()
                                    End If

                                End If
                            Catch
                            End Try

                        Case "CHSTATUS"

                            CtrlForm.el(1).CtrlEnabled = Len(CtrlForm.el(0).Data) > 0
                            CtrlForm.el(2).CtrlEnabled = Len(CtrlForm.el(1).Data) > 0
                            CtrlForm.el(3).CtrlEnabled = Len(CtrlForm.el(2).Data) > 0

                            If ctrl.Name = "TOSTATUS" Then
                                CtrlForm.DoLostFocus()
                                CtrlTable.Focus()
                            End If

                            Try
                                If Len(CtrlForm.el(0).Data) > 0 And Len(CtrlForm.el(1).Data) > 0 And Len(CtrlForm.el(2).Data) > 0 And Len(CtrlTable.el(0).Data) > 0 Then
                                    Dim sql As String = "SELECT WARHSBAL.BALANCE/1000 " & _
                                        "FROM PART, WARHSBAL, WAREHOUSES, CUSTOMERS " & _
                                        "WHERE PART.PART = WARHSBAL.PART  " & _
                                        "AND WARHSBAL.WARHS = WAREHOUSES.WARHS  " & _
                                        "AND WARHSBAL.CUST = CUSTOMERS.CUST " & _
                                        "AND WAREHOUSES.WARHSNAME = '%WARHS%'  " & _
                                        "AND WAREHOUSES.LOCNAME = '%LOCNAME%'  " & _
                                        "AND CUSTOMERS.CUSTNAME = '%STATUS%' " & _
                                        "AND PART.PARTNAME = '" & CtrlTable.el(0).Data & "'"
                                    CtrlTable.el(1).NSInvoke(sql)
                                    If CtrlTable.el(2).Data.Length = 0 Then
                                        Dim num As New frmNumber
                                        With num
                                            .Text = "Box quantity."
                                            .ShowDialog()
                                            CtrlTable.el(2).DataEntry.Text = CStr(.number)
                                            If .Manual Then Argument("MANUAL") = "Y"
                                            .Dispose()
                                        End With
                                        CtrlTable.SetTable()
                                    End If
                                End If
                            Catch
                            End Try

                            CtrlTable.EnableToolbar( _
                                CBool( _
                                Len(CtrlForm.el(0).Data) > 0 And _
                                Len(CtrlForm.el(1).Data) > 0 And _
                                Len(CtrlForm.el(2).Data) > 0 And _
                                Len(CtrlForm.el(3).Data) > 0), _
                                CtrlTable.btnEnabled(SFDCData.CtrlTable.tButton.btnEdit), _
                                CtrlTable.btnEnabled(SFDCData.CtrlTable.tButton.btnCopy), _
                                CtrlTable.btnEnabled(SFDCData.CtrlTable.tButton.btnDelete), _
                                CtrlTable.btnEnabled(SFDCData.CtrlTable.tButton.btnPost) _
                            )

                    End Select

                Catch

                End Try

        End Select
        CtrlTable.Focus()
    End Sub

    Public Overrides Function verifyForm() As Boolean
        Select Case Argument("TXTYPE")
            Case "INTERWHTX"
                Return True
            Case "PUTAWAY"
                Return True
            Case "CHSTATUS"
                Return True
        End Select
    End Function

    Public Overrides Sub ProcessForm()

        Try

            Select Case Argument("TXTYPE")

                Case "INTERWHTX", "PUTAWAY"

                    Try
                        With p
                            .DebugFlag = True
                            .Procedure = "ZSFDCP_LOAD_WHTX"
                            .Table = "ZSFDC_LOAD_WHTX"
                            .RecordType1 = "OWNERLOGIN,WARHSNAME,LOCNAME,TOWARHSNAME,TOLOCATION,MANUAL"
                            .RecordType2 = "PARTNAME,TQUANT,TOWARHSNAME2,TOLOCNAME,STATUS1,TOSTATUS,SERIALNAME"
                            .RecordTypes = "TEXT,TEXT,TEXT,TEXT,TEXT,TEXT,TEXT,,TEXT,TEXT,TEXT,TEXT,TEXT"
                        End With

                    Catch e As Exception
                        OverControl.msgboxa(e.Message)
                    End Try

                    ' Type 1 records
                    Dim t1() As String = { _
                                        UserName, _
                                        CtrlForm.ItemValue("WARHS"), _
                                        CtrlForm.ItemValue("LOCNAME"), _
                                        CtrlForm.ItemValue("TOWARHS"), _
                                        CtrlForm.ItemValue("TOLOCNAME"), _
                                        Argument("MANUAL") _
                                        }
                    p.AddRecord(1) = t1

                    For y As Integer = 0 To CtrlTable.RowCount
                        Dim amt As Integer
                        Dim a As String = CtrlTable.ItemValue("_TQUANT", y)
                        Dim pt, tw, tl, lot, stat As String
                        pt = CtrlTable.ItemValue("_PARTNAME", y)
                        amt = 1000 * CInt(CtrlTable.ItemValue("_TQUANT", y))
                        tw = CtrlTable.ItemValue("_TOWARHS", y)
                        tl = CtrlTable.ItemValue("_TOLOCNAME", y)
                        lot = CtrlTable.ItemValue("_LOT", y)
                        stat = CtrlForm.ItemValue("STATUS")
                        Dim t2() As String = { _
                                    pt, _
                                    amt, _
                                    tw, _
                                    tl, _
                                    stat, _
                                    stat, _
                                    lot _
                                            }
                        p.AddRecord(2) = t2
                    Next

                Case "PUTAWAYzzz"

                    Try
                        With p
                            .DebugFlag = True
                            .Procedure = "ZSFDCP_LOAD_WHTX"
                            .Table = "ZSFDC_LOAD_WHTX"
                            .RecordType1 = "OWNERLOGIN,WARHSNAME,LOCNAME,TOWARHSNAME,MANUAL"
                            .RecordType2 = "PARTNAME,TQUANT,TOWARHSNAME2,TOLOCNAME,TOSTATUS"
                            .RecordTypes = "TEXT,TEXT,TEXT,TEXT,TEXT,TEXT,,TEXT,TEXT,TEXT"
                        End With

                    Catch e As Exception
                        OverControl.msgboxa(e.Message)
                    End Try

                    ' Type 1 records
                    Dim t1() As String = { _
                                        UserName, _
                                        CtrlForm.ItemValue("WARHS"), _
                                        CtrlForm.ItemValue("LOCNAME"), _
                                        CtrlForm.ItemValue("TOWARHS"), _
                                        Argument("MANUAL") _
                                        }
                    p.AddRecord(1) = t1

                    For y As Integer = 0 To CtrlTable.RowCount
                        Dim t2() As String = { _
                                    CtrlTable.ItemValue("_PARTNAME", y), _
                                    CStr(CInt(1000 * CtrlTable.ItemValue("_TRANSFER", y))), _
                                    CtrlTable.ItemValue("_TOWARHS", y), _
                                    CtrlTable.ItemValue("_TOLOCNAME", y), _
                                    "Goods" _
                                            }
                        p.AddRecord(2) = t2
                    Next

                Case "CHSTATUS"

                    Try
                        With p
                            .DebugFlag = True
                            .Procedure = "ZSFDCP_LOAD_WHTX"
                            .Table = "ZSFDC_LOAD_WHTX"
                            .RecordType1 = "OWNERLOGIN,WARHSNAME,LOCNAME,TOWARHSNAME,TOLOCATION,MANUAL"
                            .RecordType2 = "PARTNAME,TQUANT,TOSTATUS,TOSTATUS"
                            .RecordTypes = "TEXT,TEXT,TEXT,TEXT,TEXT,TEXT,TEXT,,TEXT,TEXT"
                        End With

                    Catch e As Exception
                        OverControl.msgboxa(e.Message)
                    End Try

                    ' Type 1 records
                    Dim t1() As String = { _
                                        UserName, _
                                        CtrlForm.ItemValue("WARHS"), _
                                        CtrlForm.ItemValue("LOCNAME"), _
                                        CtrlForm.ItemValue("WARHS"), _
                                        CtrlForm.ItemValue("LOCNAME"), _
                                        Argument("MANUAL") _
                                        }
                    p.AddRecord(1) = t1

                    For y As Integer = 0 To CtrlTable.RowCount
                        Dim t2() As String = { _
                                    CtrlTable.ItemValue("_PARTNAME", y), _
                                    CStr(CInt(1000 * CtrlTable.ItemValue("_TQUANT", y))), _
                                    CtrlForm.ItemValue("STATUS"), _
                                    CtrlForm.ItemValue("TOSTATUS") _
                                            }
                        p.AddRecord(2) = t2
                    Next

            End Select

        Catch e As Exception
            OverControl.msgboxa(e.Message)
        End Try

    End Sub

    Public Overrides Sub BeginAdd()
        Select Case Argument("TXTYPE")
            Case "INTERWHTX"
                CtrlTable.mCol(0).ctrlEnabled = True
                CtrlTable.mCol(1).ctrlEnabled = False
                CtrlTable.mCol(2).ctrlEnabled = True
                CtrlTable.mCol(3).ctrlEnabled = True
                CtrlTable.mCol(4).ctrlEnabled = True

            Case "PUTAWAY"
                CtrlTable.mCol(0).ctrlEnabled = True
                CtrlTable.mCol(1).ctrlEnabled = False
                CtrlTable.mCol(2).ctrlEnabled = True
                CtrlTable.mCol(3).ctrlEnabled = True

            Case "CHSTATUS"
                CtrlTable.mCol(0).ctrlEnabled = True
                CtrlTable.mCol(1).ctrlEnabled = False
                CtrlTable.mCol(2).ctrlEnabled = True
                'CtrlTable.mCol(3).ctrlEnabled = True


        End Select
    End Sub

    Public Overrides Sub BeginEdit()
        Argument("MANUAL") = "Y"
        Select Case Argument("TXTYPE")
            Case "INTERWHTX"
                CtrlTable.mCol(0).ctrlEnabled = True
                CtrlTable.mCol(1).ctrlEnabled = False
                CtrlTable.mCol(2).ctrlEnabled = True
                CtrlTable.mCol(3).ctrlEnabled = True
                CtrlTable.mCol(4).ctrlEnabled = True

            Case "PUTAWAY"
                CtrlTable.mCol(0).ctrlEnabled = True
                CtrlTable.mCol(1).ctrlEnabled = False
                CtrlTable.mCol(2).ctrlEnabled = True
                CtrlTable.mCol(3).ctrlEnabled = True

            Case "CHSTATUS"
                CtrlTable.mCol(0).ctrlEnabled = True
                CtrlTable.mCol(1).ctrlEnabled = False
                CtrlTable.mCol(2).ctrlEnabled = True
                'CtrlTable.mCol(3).ctrlEnabled = True

        End Select

    End Sub

    Public Overrides Sub AfterEdit(ByVal TableIndex As Integer)

        Select Case Argument("TXTYPE")

            Case "INTERWHTX"
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

            Case "PUTAWAY"
                CtrlTable.Table.Focus()

            Case "CHSTATUS"

        End Select

    End Sub

    Public Overrides Sub AfterAdd(ByVal TableIndex As Integer)

    End Sub

    Public Overrides Sub TableScan(ByVal Value As String)
        If Value = "" Then Exit Sub
        Dim v2 As String = ""
        Cnt = 0
        Prt = ""
        Dim shown As Boolean = False
        Dim rej As Boolean = False
        Dim doc As New Xml.XmlDocument
        doc.LoadXml(Value)
        For Each nd As XmlNode In doc.SelectNodes("in/i")
            Dim DataType As String = nd.Attributes("n").Value
            Select Case DataType
                Case "PART"
                    Prt = nd.Attributes("v").Value
                Case "LOT", "SERIAL"
                    Lt = nd.Attributes("v").Value
                    If Lt = "" Then
                        Lt = "0"
                    End If
                Case "QTY"
                    Cnt = nd.Attributes("v").Value

                Case "UNQ"
                    unq = nd.Attributes("v").Value

                Case "CUST", "ACT"
                    If DataType = "CUST" Then
                        If nd.Attributes("v").Value.ToString = "Reject" And Argument("TXTYPE") = "PUTAWAY" Then
                            rej = True
                        End If
                    End If
                Case "WARHS"
                    Select Case Argument("TXTYPE")
                        '**************************************************************************
                        Case "INTERWHTX"
                            With CtrlForm
                                Select Case .el(0).Enabled
                                    Case True
                                        .el(0).Data = nd.Attributes("v").Value
                                        .el(0).DataEntry.Text = nd.Attributes("v").Value
                                        .el(0).ProcessEntry()

                                    Case False
                                        .el(3).Data = nd.Attributes("v").Value
                                        .el(3).DataEntry.Text = nd.Attributes("v").Value
                                        .el(3).ProcessEntry()
                                        .el(3).Enabled = False
                                End Select

                            End With
                        Case "PUTAWAY"
                            With CtrlForm
                                .el(2).Data = nd.Attributes("v").Value
                                .el(2).DataEntry.Text = nd.Attributes("v").Value
                                .el(2).ProcessEntry()
                            End With
                    End Select

                Case "BIN"
                    Select Case Argument("TXTYPE")
                        '**************************************************************************
                        Case "INTERWHTX"
                            With CtrlForm
                                Select Case .el(0).Enabled
                                    Case True
                                        .el(1).Data = nd.Attributes("v").Value
                                        .el(1).DataEntry.Text = nd.Attributes("v").Value
                                        .el(1).ProcessEntry()
                                        .el(0).Enabled = False
                                        .el(1).Enabled = False
                                        mInvoke = tInvoke.iStatus
                                        Dim q As String = "select top 1 CUSTOMERS.CUSTNAME " & _
                                    "from WARHSBAL, WAREHOUSES, CUSTOMERS " & _
                                    "WHERE WARHSBAL.WARHS = WAREHOUSES.WARHS " & _
                                    "AND CUSTOMERS.CUST = WARHSBAL.CUST " & _
                                    "AND WAREHOUSES.WARHSNAME = '%WARHS%' " & _
                                    "AND WAREHOUSES.LOCNAME = '%LOCNAME%' "
                                        InvokeData(q)
                                    Case False
                                        .el(4).Data = nd.Attributes("v").Value
                                        .el(4).DataEntry.Text = nd.Attributes("v").Value
                                        .el(4).ProcessEntry()
                                        .el(4).Enabled = False
                                End Select

                            End With
                        Case "PUTAWAY"
                            With CtrlForm
                                .el(3).Data = nd.Attributes("v").Value
                                .el(3).DataEntry.Text = nd.Attributes("v").Value
                                .el(3).ProcessEntry()
                            End With
                            CtrlTable.Focus()
                    End Select
                Case "PROCESS"
                    ProcessForm()
                Case "CLOSE"
                    Me.CloseMe()

                Case Else
                    OverControl.msgboxa("Invalid data found, please ensure that you are scanning a part")
                    Exit Sub

            End Select

        Next
        If rej = True Then
            Prt = ""
            MsgBox("Rejected Part scanned")
        End If
        If Prt <> "" Then
            Dim fnd As Boolean = False

            For Each i As Integer In UNQList
                If i = unq And shown = False Then
                    'Dim f As New frmMsgBox
                    'f.Label1.Text = "You have already scanned this label"
                    'f.ShowDialog()
                    'If f.ShowDialog = DialogResult.OK Then
                    '    f.Dispose()
                    'End If
                    Beep()

                    Prt = ""
                    fnd = True
                End If
            Next
            If fnd = False Then
                UNQList.Add(unq)
            Else
                fnd = False
            End If

        End If

        If Prt = "" Then Exit Sub

        mInvoke = tInvoke.iPart
        InvokeData("SELECT PART.BARCODE FROM PART WHERE PART.BARCODE = '" & Prt & "'")
        v2 = TBar

        If v2 <> "" Then


            Select Case Argument("TXTYPE")
                Case "INTERWHTX", "CHSTATUS"
                    If Regex.IsMatch(v2, ValidStr(tRegExValidation.tBarcode)) Then
                        mInvoke = tInvoke.iBarcode
                        InvokeData("SELECT PARTNAME, BARCODE " & _
                                                    "FROM PART, WARHSBAL, WAREHOUSES, CUSTOMERS " & _
                                                    "WHERE PART.PART = WARHSBAL.PART  " & _
                                                    "AND WARHSBAL.WARHS = WAREHOUSES.WARHS  " & _
                                                    "AND WARHSBAL.CUST = CUSTOMERS.CUST " & _
                                                    "AND WAREHOUSES.WARHSNAME = '%WARHS%'  " & _
                                                    "AND WAREHOUSES.LOCNAME = '%LOCNAME%'  " & _
                                                    "AND WARHSBAL.BALANCE > 0 " & _
                                                    "AND CUSTNAME = '%STATUS%' " & _
                                                    "AND BARCODE = '" & v2 & "'")
                    ElseIf Regex.IsMatch(v2, ValidStr(tRegExValidation.tLocname)) Then
                        mInvoke = tInvoke.iLoc
                        InvokeData("SELECT LOCNAME FROM WAREHOUSES WHERE LOCNAME = '" & v2 & "' AND WARHSNAME = " & _
                            "'%WARHS%' AND LOCNAME <> '%LOCNAME%'")
                    End If

                Case "PUTAWAY"
                    If Regex.IsMatch(v2, ValidStr(tRegExValidation.tBarcode)) Then
                        mInvoke = tInvoke.iBarcode
                        InvokeData("SELECT PARTNAME, BARCODE " & _
                                                    "FROM PART, WARHSBAL, WAREHOUSES, CUSTOMERS " & _
                                                    "WHERE PART.PART = WARHSBAL.PART  " & _
                                                    "AND WARHSBAL.WARHS = WAREHOUSES.WARHS  " & _
                                                    "AND WARHSBAL.CUST = CUSTOMERS.CUST " & _
                                                    "AND WAREHOUSES.WARHSNAME = '%WARHS%'" & _
                                                    "AND WAREHOUSES.LOCNAME = '%LOCNAME%'  " & _
                                                    "AND WARHSBAL.BALANCE > 0 " & _
                                                    "AND CUSTOMERS.CUST = -1 " & _
                                                    "AND BARCODE = '" & v2 & "'")
                    ElseIf Regex.IsMatch(v2, ValidStr(tRegExValidation.tLocname)) Then
                        mInvoke = tInvoke.iLoc
                        InvokeData("SELECT LOCNAME FROM WAREHOUSES WHERE LOCNAME = '" & v2 & "' AND WARHSNAME = " & _
                            "'%WARHS%' AND LOCNAME <> '%LOCNAME%'")
                    End If

            End Select
        End If
        CtrlTable.Focus()
    End Sub

    Public Overrides Sub EndInvokeData(ByVal Data(,) As String)

        Select Case mInvoke
            Case tInvoke.iPart
                If Data Is Nothing Then
                    TBar = ""
                Else
                    TBar = Data(0, 0)
                End If

            Case tInvoke.iBarcode
                If IsNothing(Data) Then
                    OverControl.msgboxa("Part does not exist in this location.")
                    Exit Sub
                End If

                For i As Integer = 0 To CtrlTable.Table.Items.Count - 1
                    If CtrlTable.Table.Items(i).SubItems(0).Text = Data(0, 0) Then

                        ' Found Record
                        Select Case Argument("TXTYPE")
                            Case "INTERWHTX"
                                CtrlTable.Table.Items(i).Selected = True
                                'CtrlTable.SetEdit()
                                If Cnt <> 0 Then
                                    CtrlTable.Table.Items(i).SubItems(2).Text = CStr(CInt(CtrlTable.Table.Items(i).SubItems(2).Text) + Cnt)
                                Else
                                    Dim num As New frmNumber
                                    With num
                                        .Text = "Box quantity."
                                        .number = Cnt
                                        .ShowDialog()
                                        CtrlTable.Table.Items(i).SubItems(2).Text = CStr(CInt(CtrlTable.Table.Items(i).SubItems(2).Text) + .number)
                                        If Lt <> "" Then
                                            CtrlTable.Table.Items(i).SubItems(5).Text = CStr(Lt)
                                        End If
                                        If .Manual Then Argument("MANUAL") = "Y"
                                        .Dispose()
                                    End With
                                End If
                                'CtrlTable.SetTable()

                            Case "PUTAWAY"
                                CtrlTable.Table.Items(i).Selected = True


                                'CtrlTable.SetEdit()
                                If Cnt <> 0 Then
                                    CtrlTable.Table.Items(i).SubItems(2).Text = CStr(CInt(CtrlTable.Table.Items(i).SubItems(2).Text) + Cnt)
                                Else
                                    Dim num As New frmNumber
                                    With num
                                        .Text = "Box quantity."
                                        .number = Cnt
                                        .ShowDialog()
                                        CtrlTable.Table.Items(i).SubItems(2).Text = CStr(CInt(CtrlTable.Table.Items(i).SubItems(2).Text) + .number)
                                        If Lt <> "" Then
                                            CtrlTable.Table.Items(i).SubItems(5).Text = CStr(Lt)
                                        End If
                                        If .Manual Then Argument("MANUAL") = "Y"
                                        .Dispose()
                                    End With
                                End If




                                'CtrlTable.SetTable()



                            Case "CHSTATUS"
                                CtrlTable.Table.Items(i).Selected = True
                                'CtrlTable.SetEdit()
                                Dim num As New frmNumber
                                With num
                                    .Text = "Box quantity."
                                    .number = Cnt
                                    .ShowDialog()
                                    CtrlTable.Table.Items(i).SubItems(2).Text = CStr(CtrlTable.Table.Items(i).SubItems(2).Text + .number)
                                    If .Manual Then Argument("MANUAL") = "Y"
                                    .Dispose()
                                End With
                                'CtrlTable.SetTable()
                        End Select
                        Exit Sub

                    End If
                Next
                If CtrlTable.Table.Items.Count Then
                    Dim it As New ListViewItem

                End If
                ' No record found
                Select Case Argument("TXTYPE")
                    Case "INTERWHTX"
                        Try
                            Dim it As ListViewItem
                            'CtrlTable.Table.Items.Add(New{Data(0, 0), "0", "0", CtrlForm.el(2).Data, CtrlForm.el(3).Data})
                            Dim str(6) As String
                            If Cnt <> 0 Then
                                str(2) = Cnt
                            Else
                                Dim num As New frmNumber
                                With num
                                    .Text = "Box quantity."
                                    .number = Cnt
                                    .ShowDialog()
                                    str(2) = .number
                                    If .Manual Then Argument("MANUAL") = "Y"
                                    .Dispose()
                                End With
                            End If

                            If Convert.ToDecimal(str(2)) > Convert.ToDecimal(Cnt) Then
                                OverControl.msgboxa("You have picked too many of this item, please rescan and try again")
                                Exit Sub

                            End If

                            str(0) = Data(0, 0)
                            str(1) = Cnt
                            'str(2) = "0"
                            str(3) = CtrlForm.el(3).Data
                            str(4) = CtrlForm.el(4).Data
                            str(5) = Lt
                            it = New ListViewItem(str)
                            CtrlTable.Table.Items.Add(it)

                        Catch ex As Exception
                            OverControl.msgboxa(ex.ToString)
                        End Try
                    Case "PUTAWAY"
                        Try
                            Dim it As ListViewItem
                            'CtrlTable.Table.Items.Add(New{Data(0, 0), "0", "0", CtrlForm.el(2).Data, CtrlForm.el(3).Data})
                            Dim str(6) As String
                            If Cnt <> 0 Then
                                str(2) = Cnt
                            Else
                                Dim num As New frmNumber
                                With num
                                    .Text = "Box quantity."
                                    .number = Cnt
                                    .ShowDialog()
                                    str(2) = .number
                                    If .Manual Then Argument("MANUAL") = "Y"
                                    .Dispose()
                                End With
                            End If
                            If Convert.ToDecimal(str(2)) > Convert.ToDecimal(Cnt) Then
                                OverControl.msgboxa("You have picked too many of this item, please rescan and try again")
                                Exit Sub

                            End If

                            str(0) = Data(0, 0)
                            str(1) = Cnt
                            'str(2) = "0"
                            str(3) = CtrlForm.el(2).Data
                            str(4) = CtrlForm.el(3).Data
                            str(5) = Lt
                            it = New ListViewItem(str)
                            CtrlTable.Table.Items.Add(it)

                        Catch ex As Exception
                            OverControl.msgboxa(ex.ToString)
                        End Try




                        'If CtrlTable.btnEnabled(SFDCData.CtrlTable.tButton.btnAdd) Then
                        '    With CtrlTable
                        '        .SetAdd()
                        '        With .el(0)
                        '            .DataEntry.Text = Data(1, 0)
                        '            .Data = Data(1, 0)
                        '        End With
                        '    End With
                        'End If

                    Case "CHSTATUS"
                        If CtrlTable.btnEnabled(SFDCData.CtrlTable.tButton.btnAdd) Then
                            With CtrlTable
                                .SetAdd()
                                With .el(0)
                                    .DataEntry.Text = Data(1, 0)
                                    .ProcessEntry()
                                End With
                            End With
                        End If

                End Select

            Case tInvoke.iLoc

                If IsNothing(Data) Then
                    Exit Sub
                End If

                With CtrlForm
                    With .el(1)
                        .DataEntry.Text = Data(0, 0)
                        .ProcessEntry()
                        CtrlTable.Table.Focus()
                    End With
                End With

            Case tInvoke.iStatus
                With CtrlForm
                    With .el(2)
                        .DataEntry.Text = Data(0, 0)
                        .ProcessEntry()
                        CtrlTable.Table.Focus()
                    End With
                End With

        End Select
        CtrlTable.Focus()
    End Sub

End Class

