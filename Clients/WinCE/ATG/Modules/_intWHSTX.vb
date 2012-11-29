Imports System
Imports System.IO
Imports System.Threading
Imports System.Text.RegularExpressions

Public Class InterfaceWHTX
    Inherits SFDCData.iForm

    Private Enum tInvoke
        iBarcode = 0
        iLoc = 1
        iWarhsTX = 2
    End Enum

    Private mInvoke As tInvoke = tInvoke.iBarcode

#Region "Initialisation"

    Public Sub New(Optional ByRef App As Form = Nothing)

        InitializeComponent()
        CallerApp = App
        NewArgument("TXTYPE", "INTERWHTX")
        NewArgument("SERIALPARTS", "FALSE")
        NewArgument("DOCNO", "")
        CtrlTable.DisableButtons(False, False, False, False, False)
        CtrlTable.EnableToolbar(False, False, False, False, False)

    End Sub

#End Region

#Region "FormSettings"

    Public Overrides Sub FormSettings()

        Select Case Argument("TXTYPE")
            Case "INTERWHTX"
                FormSettings_INTERWHTX()
            Case "PUTAWAY"
                FormSettings_PUTAWAY()
            Case "CHSTATUS"
                FormSettings_CHSTATUS()
        End Select

    End Sub

    Private Sub FormSettings_INTERWHTX()

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
            .ctrlEnabled = True
        End With
        CtrlForm.AddField(field)

        'TOLOCATION
        With field
            .Name = "TOLOCNAME"
            .Title = "To Loc"
            .ValidExp = ValidStr(tRegExValidation.tLocname)
            .SQLList = "SELECT DISTINCT LOCNAME FROM WAREHOUSES WHERE WARHSNAME = '%TOWARHS%' AND INACTIVE <> 'Y'"
            .SQLValidation = "SELECT LOCNAME FROM WAREHOUSES WHERE LOCNAME = '%ME%' AND WARHSNAME = '%TOWARHS%'"
            .Data = ""      '******** Barcoded field '*******
            .AltEntry = ctrlText.tAltCtrlStyle.ctList 'ctKeyb 
            .ctrlEnabled = False
        End With
        CtrlForm.AddField(field)

    End Sub

    Private Sub FormSettings_PUTAWAY()

        'LOCNAME
        With field
            .Name = "LOCNAME"
            .Title = "From Loc"
            .ValidExp = ValidStr(tRegExValidation.tLocname)
            .SQLList = "SELECT DISTINCT LOCNAME FROM WAREHOUSES WHERE WARHSNAME = '" & Warehouse & "' AND WAREHOUSES.INACTIVE <> 'Y'"
            .SQLValidation = "SELECT LOCNAME FROM WAREHOUSES WHERE LOCNAME = '%ME%' AND WARHSNAME = '" & Warehouse & "' AND WAREHOUSES.INACTIVE <> 'Y'"
            .Data = ""      '******** Barcoded field '*******
            .AltEntry = ctrlText.tAltCtrlStyle.ctList 'ctNone 'ctKeyb 
            .ctrlEnabled = True
        End With
        CtrlForm.AddField(field)

        'TOLOCATION
        With field
            .Name = "TOLOCNAME"
            .Title = "To Loc"
            .ValidExp = ValidStr(tRegExValidation.tLocname)
            .SQLList = "SELECT DISTINCT LOCNAME FROM WAREHOUSES WHERE WARHSNAME = (SELECT WARHSNAME FROM v_USERS where USERLOGIN = '" & UserName & "') AND WAREHOUSES.INACTIVE <> 'Y'"
            .SQLValidation = "SELECT LOCNAME FROM WAREHOUSES WHERE LOCNAME = '%ME%' AND WARHSNAME = (SELECT WARHSNAME FROM v_USERS where USERLOGIN = '" & UserName & "') AND WAREHOUSES.INACTIVE <> 'Y'"
            .Data = ""      '******** Barcoded field '*******
            .AltEntry = ctrlText.tAltCtrlStyle.ctList 'ctKeyb 
            .ctrlEnabled = False
        End With
        CtrlForm.AddField(field)

    End Sub

    Private Sub FormSettings_CHSTATUS()

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

    End Sub

#End Region

#Region "Table Settings"

    Public Overrides Sub TableSettings()

        Select Case Argument("TXTYPE")
            '**************************************************************************
            Case "INTERWHTX"
                TableSettings_INTERWHTX()
            Case "PUTAWAY"
                TableSettings_PUTAWAY()
            Case "CHSTATUS"
                TableSettings_CHSTATUS()
        End Select

    End Sub

    Private Sub TableSettings_INTERWHTX()

        ' BARCODE
        With col
            .Name = "_PARTNAME"
            .Title = "Part No"
            .initWidth = 30
            .TextAlign = HorizontalAlignment.Left
            .AltEntry = ctrlText.tAltCtrlStyle.ctNone 'ctKeyb 
            .ValidExp = "^.+$"
            ' Second field replaces first field if first field validates ok
            .SQLValidation = "SELECT     ALIAS.BARCODE, ALIAS.PARTNAME " & _
                                "FROM         dbo.PARTALIAS() AS ALIAS INNER JOIN " & _
                                "                      dbo.WARHSBAL ON ALIAS.PART = dbo.WARHSBAL.PART INNER JOIN " & _
                                "                      dbo.WAREHOUSES ON dbo.WARHSBAL.WARHS = dbo.WAREHOUSES.WARHS INNER JOIN " & _
                                "                      dbo.CUSTOMERS ON dbo.WARHSBAL.CUST = dbo.CUSTOMERS.CUST " & _
                                "WHERE     (dbo.WAREHOUSES.WARHSNAME = '%WARHS%') AND (dbo.WAREHOUSES.LOCNAME = '%LOCNAME%') AND (dbo.WARHSBAL.BALANCE > 0) AND " & _
                                "                      (dbo.CUSTOMERS.CUSTNAME = '%STATUS%') AND (ALIAS.BARCODE = '%ME%') "
            .DefaultFromCtrl = Nothing '
            .ctrlEnabled = True
            .Mandatory = True
        End With
        CtrlTable.AddCol(col)

        If CBool(Argument("SERIALPARTS")) Then
            'SERIALNAME
            With col
                .Name = "_SERIALNAME"
                .Title = "Serial"
                .initWidth = 50
                .TextAlign = HorizontalAlignment.Left
                .AltEntry = ctrlText.tAltCtrlStyle.ctList 'ctKeyb 
                .ValidExp = ValidStr(tRegExValidation.tSerial)
                .SQLList = "SELECT DISTINCT SERIALNAME FROM SERIAL, WARHSBAL, WAREHOUSES " & _
                            "WHERE SERIAL.SERIAL = WARHSBAL.SERIAL " & _
                            "AND WAREHOUSES.WARHS = WARHSBAL.WARHS " & _
                            "AND WARHSBAL.PART = (SELECT PART FROM PART WHERE PARTNAME = '%_PARTNAME%') " & _
                            "AND WAREHOUSES.WARHSNAME = '%WARHS%' AND WAREHOUSES.LOCNAME = '%LOCNAME%' AND WARHSBAL.BALANCE > 0 AND WARHSBAL.ACT = 0"
                .SQLValidation = "SELECT DISTINCT SERIALNAME FROM SERIAL, WARHSBAL, WAREHOUSES " & _
                            "WHERE SERIAL.SERIAL = WARHSBAL.SERIAL " & _
                            "AND WAREHOUSES.WARHS = WARHSBAL.WARHS " & _
                            "AND WARHSBAL.PART = (SELECT PART FROM PART WHERE PARTNAME = '%_PARTNAME%') " & _
                            "AND SERIALNAME = '%ME%' AND WAREHOUSES.WARHSNAME = '%WARHS%' AND WAREHOUSES.LOCNAME = '%LOCNAME%' AND WARHSBAL.BALANCE > 0 AND WARHSBAL.ACT = 0"
                .DefaultFromCtrl = Nothing 'CtrlForm.el(1)  '******* Barcoded Field - default from Type1 TOLOCATION '*******
                .ctrlEnabled = True
                .Mandatory = True
                .MandatoryOnPost = True
            End With
            CtrlTable.AddCol(col)
        End If

        ' Availibility
        With col
            .Name = "_AVAILABLE"
            .Title = "Available"
            .initWidth = 30
            .TextAlign = HorizontalAlignment.Left
            .AltEntry = ctrlText.tAltCtrlStyle.ctNone 'ctKeyb 
            .ValidExp = ValidStr(tRegExValidation.tNumeric)
            .SQLValidation = ""
            .DefaultFromCtrl = Nothing
            .ctrlEnabled = False
            .Mandatory = False
            .MandatoryOnPost = False
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
                            "WHERE %ME% <= %_AVAILABLE%"
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
            .SQLList = "SELECT DISTINCT WARHSNAME FROM WAREHOUSES AND INACTIVE <> 'Y'"
            .SQLValidation = "SELECT WARHSNAME FROM WAREHOUSES WHERE WARHSNAME = '%ME%' AND INACTIVE <> 'Y'"
            .AltEntry = ctrlText.tAltCtrlStyle.ctList 'ctKeyb 
            .DefaultFromCtrl = CtrlForm.el(ColNo("TOWARHS"))
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
            .ValidExp = ValidStr(tRegExValidation.tLocname)
            .SQLList = "SELECT DISTINCT LOCNAME FROM WAREHOUSES WHERE WARHSNAME = '%TOWARHS%' AND INACTIVE <> 'Y'"
            .SQLValidation = "SELECT LOCNAME FROM WAREHOUSES WHERE LOCNAME = '%ME%' AND WARHSNAME = '%TOWARHS%'"
            .DefaultFromCtrl = CtrlForm.el(ColNo("TOLOCNAME"))  '******* Barcoded Field - default from Type1 TOLOCATION '*******
            .ctrlEnabled = True
            .Mandatory = False
            .MandatoryOnPost = True
        End With
        CtrlTable.AddCol(col)

        ' TOLOCNAME
        With col
            .Name = "_TRANS"
            .Title = "Transorder"
            .initWidth = 1
            .TextAlign = HorizontalAlignment.Left
            .AltEntry = ctrlText.tAltCtrlStyle.ctList 'ctKeyb 
            .ValidExp = ValidStr(tRegExValidation.tNumeric)
            .SQLList = ""
            .SQLValidation = ""
            .DefaultFromCtrl = Nothing
            .ctrlEnabled = False
            .Mandatory = False
            .MandatoryOnPost = False
        End With
        CtrlTable.AddCol(col)

    End Sub

    Private Sub TableSettings_PUTAWAY()

        With col
            .Name = "_PARTNAME"
            .Title = "Part No"
            .initWidth = 30
            .TextAlign = HorizontalAlignment.Left
            .AltEntry = ctrlText.tAltCtrlStyle.ctNone 'ctKeyb 
            .ValidExp = "^.+$"
            ' Second field replaces first field if first field validates ok
            .SQLValidation = "SELECT     ALIAS.BARCODE, ALIAS.PARTNAME " & _
                                "FROM         dbo.PARTALIAS() AS ALIAS INNER JOIN " & _
                                "                      dbo.WARHSBAL ON ALIAS.PART = dbo.WARHSBAL.PART INNER JOIN " & _
                                "                      dbo.WAREHOUSES ON dbo.WARHSBAL.WARHS = dbo.WAREHOUSES.WARHS INNER JOIN " & _
                                "                      dbo.CUSTOMERS ON dbo.WARHSBAL.CUST = dbo.CUSTOMERS.CUST " & _
                                "WHERE     (dbo.WAREHOUSES.WARHSNAME = '" & Warehouse & "') AND (dbo.WAREHOUSES.LOCNAME = '%LOCNAME%') AND (dbo.WARHSBAL.BALANCE > 0) AND " & _
                                "                      (dbo.CUSTOMERS.CUST = -1) AND (ALIAS.BARCODE = '%ME%') "
            ' CUST = -1 : Only put away goods

            .DefaultFromCtrl = Nothing '
            .ctrlEnabled = True
            .Mandatory = True
        End With
        CtrlTable.AddCol(col)

        ' Availibility
        With col
            .Name = "_AVAILABLE"
            .Title = "Available"
            .initWidth = 30
            .TextAlign = HorizontalAlignment.Left
            .AltEntry = ctrlText.tAltCtrlStyle.ctNone 'ctKeyb 
            .ValidExp = ValidStr(tRegExValidation.tNumeric)
            .SQLValidation = ""
            .DefaultFromCtrl = Nothing
            .ctrlEnabled = False
            .Mandatory = False
            .MandatoryOnPost = False
        End With
        CtrlTable.AddCol(col)

        ' Transfer
        With col
            .Name = "_TRANSFER"
            .Title = "Transfer"
            .initWidth = 30
            .TextAlign = HorizontalAlignment.Left
            .AltEntry = ctrlText.tAltCtrlStyle.ctNone 'ctKeyb 
            .ValidExp = ValidStr(tRegExValidation.tNumeric)
            .SQLValidation = "SELECT %ME% " & _
                            "WHERE %ME% <= %_AVAILABLE%"
            .DefaultFromCtrl = Nothing
            .ctrlEnabled = True
            .Mandatory = True
        End With
        CtrlTable.AddCol(col)

        ' TOLOCNAME
        With col
            .Name = "_TOLOCNAME"
            .Title = "To Loc"
            .initWidth = 30
            .TextAlign = HorizontalAlignment.Left
            .AltEntry = ctrlText.tAltCtrlStyle.ctList 'ctKeyb 
            .ValidExp = ValidStr(tRegExValidation.tLocname)
            .SQLList = "SELECT DISTINCT LOCNAME FROM WAREHOUSES WHERE WARHSNAME = " & _
                "(SELECT WARHSNAME FROM v_USERS where USERLOGIN = '" & UserName & "') " & _
                "AND INACTIVE <> 'Y'"
            .SQLValidation = "SELECT LOCNAME FROM WAREHOUSES WHERE LOCNAME = '%ME%' AND WARHSNAME = " & _
                "(SELECT WARHSNAME FROM v_USERS where USERLOGIN = '" & UserName & "')"
            .DefaultFromCtrl = CtrlForm.el(1)  '******* Barcoded Field - default from Type1 TOLOCATION '*******
            .ctrlEnabled = False
            .Mandatory = False
            .MandatoryOnPost = True
        End With
        CtrlTable.AddCol(col)

        If CBool(Argument("SERIALPARTS")) Then
            'SERIALNAME
            With col
                .Name = "_SERIALNAME"
                .Title = "Serial"
                .initWidth = 50
                .TextAlign = HorizontalAlignment.Left
                .AltEntry = ctrlText.tAltCtrlStyle.ctList 'ctKeyb 
                .ValidExp = ValidStr(tRegExValidation.tSerial)
                .SQLList = "SELECT DISTINCT SERIALNAME FROM SERIAL, WARHSBAL, WAREHOUSES " & _
                            "WHERE SERIAL.SERIAL = WARHSBAL.SERIAL " & _
                            "AND WAREHOUSES.WARHS = WARHSBAL.WARHS " & _
                            "AND WARHSBAL.PART = (SELECT PART FROM PART WHERE PARTNAME = '%_PARTNAME%') " & _
                            "AND WAREHOUSES.WARHSNAME = '" & Warehouse & "' AND WAREHOUSES.LOCNAME = '%LOCNAME%' AND WARHSBAL.ACT = 0"
                .SQLValidation = "SELECT DISTINCT SERIALNAME FROM SERIAL, WARHSBAL, WAREHOUSES " & _
                            "WHERE SERIAL.SERIAL = WARHSBAL.SERIAL " & _
                            "AND WAREHOUSES.WARHS = WARHSBAL.WARHS " & _
                            "AND WARHSBAL.PART = (SELECT PART FROM PART WHERE PARTNAME = '%_PARTNAME%') " & _
                            "AND SERIALNAME = '%ME%' AND WAREHOUSES.WARHSNAME = '" & Warehouse & "' AND WAREHOUSES.LOCNAME = '%LOCNAME%' AND WARHSBAL.ACT = 0"
                .DefaultFromCtrl = Nothing 'CtrlForm.el(1)  '******* Barcoded Field - default from Type1 TOLOCATION '*******
                .ctrlEnabled = True
                .Mandatory = True
                .MandatoryOnPost = True
            End With
            CtrlTable.AddCol(col)
        End If

    End Sub

    Private Sub TableSettings_CHSTATUS()

        With col
            .Name = "_PARTNAME"
            .Title = "Part No"
            .initWidth = 30
            .TextAlign = HorizontalAlignment.Left
            .AltEntry = ctrlText.tAltCtrlStyle.ctNone 'ctKeyb 
            .ValidExp = "^.+$"
            ' Second field replaces first field if first field validates ok
            .SQLValidation = "SELECT     ALIAS.BARCODE, ALIAS.PARTNAME " & _
                                "FROM         dbo.PARTALIAS() AS ALIAS INNER JOIN " & _
                                "                      dbo.WARHSBAL ON ALIAS.PART = dbo.WARHSBAL.PART INNER JOIN " & _
                                "                      dbo.WAREHOUSES ON dbo.WARHSBAL.WARHS = dbo.WAREHOUSES.WARHS INNER JOIN " & _
                                "                      dbo.CUSTOMERS ON dbo.WARHSBAL.CUST = dbo.CUSTOMERS.CUST " & _
                                "WHERE     (dbo.WAREHOUSES.WARHSNAME = '%WARHS%') AND (dbo.WAREHOUSES.LOCNAME = '%LOCNAME%') AND (dbo.WARHSBAL.BALANCE > 0) AND " & _
                                "                      (dbo.CUSTOMERS.CUSTNAME = '%STATUS%') AND (ALIAS.BARCODE = '%ME%') "
            .DefaultFromCtrl = Nothing '
            .ctrlEnabled = True
            .Mandatory = True
        End With
        CtrlTable.AddCol(col)

        If CBool(Argument("SERIALPARTS")) Then
            'SERIALNAME
            With col
                .Name = "_SERIALNAME"
                .Title = "Serial"
                .initWidth = 50
                .TextAlign = HorizontalAlignment.Left
                .AltEntry = ctrlText.tAltCtrlStyle.ctList 'ctKeyb 
                .ValidExp = ValidStr(tRegExValidation.tSerial)
                .SQLList = "SELECT DISTINCT SERIALNAME FROM SERIAL, WARHSBAL, WAREHOUSES " & _
                            "WHERE SERIAL.SERIAL = WARHSBAL.SERIAL " & _
                            "AND WAREHOUSES.WARHS = WARHSBAL.WARHS " & _
                            "AND WARHSBAL.PART = (SELECT PART FROM PART WHERE PARTNAME = '%_PARTNAME%') " & _
                            "AND WAREHOUSES.WARHSNAME = '%WARHS%' AND WAREHOUSES.LOCNAME = '%LOCNAME%' " & _
                            "AND WARHSBAL.BALANCE >0 AND WARHSBAL.ACT = 0"
                .SQLValidation = "SELECT DISTINCT SERIALNAME FROM SERIAL, WARHSBAL, WAREHOUSES " & _
                            "WHERE SERIAL.SERIAL = WARHSBAL.SERIAL " & _
                            "AND WAREHOUSES.WARHS = WARHSBAL.WARHS " & _
                            "AND WARHSBAL.PART = (SELECT PART FROM PART WHERE PARTNAME = '%_PARTNAME%') " & _
                            "AND SERIALNAME = '%ME%' AND WAREHOUSES.WARHSNAME = '%WARHS%' AND WAREHOUSES.LOCNAME = '%LOCNAME%' AND WARHSBAL.ACT = 0"
                .DefaultFromCtrl = Nothing 'CtrlForm.el(1)  '******* Barcoded Field - default from Type1 TOLOCATION '*******
                .ctrlEnabled = True
                .Mandatory = True
                .MandatoryOnPost = True
            End With
            CtrlTable.AddCol(col)
        End If

        ' Availibility
        With col
            .Name = "_AVAILABLE"
            .Title = "Available"
            .initWidth = 30
            .TextAlign = HorizontalAlignment.Left
            .AltEntry = ctrlText.tAltCtrlStyle.ctNone 'ctKeyb 
            .ValidExp = ValidStr(tRegExValidation.tNumeric)
            .SQLValidation = ""
            .DefaultFromCtrl = Nothing
            .ctrlEnabled = False
            .Mandatory = False
            .MandatoryOnPost = False
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
                            "WHERE %ME% <= %_AVAILABLE%"
            .DefaultFromCtrl = Nothing
            .ctrlEnabled = True
            .Mandatory = True
        End With
        CtrlTable.AddCol(col)

        'STATUS1
        With col
            .Name = "_STATUS"
            .Title = "To Status"
            .initWidth = 30
            .TextAlign = HorizontalAlignment.Left
            .AltEntry = ctrlText.tAltCtrlStyle.ctList ' ctNone 'ctKeyb 
            .ValidExp = ValidStr(tRegExValidation.tStatus)
            .SQLValidation = "SELECT CUSTNAME FROM CUSTOMERS WHERE STATUSFLAG = 'Y' AND CUSTNAME = '%ME%'"
            .SQLList = "SELECT CUSTNAME FROM CUSTOMERS WHERE STATUSFLAG = 'Y'"
            .DefaultFromCtrl = CtrlForm.el(2)  '******** Default to 'Goods' '*******
            .ctrlEnabled = True
            .Mandatory = True
            .MandatoryOnPost = True
        End With
        CtrlTable.AddCol(col)

    End Sub

#End Region

    Public Overrides Sub TableRXData(ByVal Data(,) As String)

        Try
            With CtrlTable.Table
                .Items.Clear()
                For y As Integer = 0 To UBound(Data, 2)
                    Dim lvi As New ListViewItem
                    .Items.Add(lvi)

                    Select Case Argument("TXTYPE")
                        Case "INTERWHTX"
                            .Items(.Items.Count - 1).Text = Data(0, y)
                            For i As Integer = 1 To UBound(Data, 1)
                                .Items(.Items.Count - 1).SubItems.Add(Data(i, y))
                            Next

                        Case "PUTAWAY"
                            .Items(.Items.Count - 1).Text = Data(0, y)
                            For i As Integer = 1 To UBound(Data, 1)
                                .Items(.Items.Count - 1).SubItems.Add(Data(i, y))
                            Next

                        Case "CHSTATUS"

                    End Select

                Next
            End With
        Catch e As Exception
            MsgBox(e.Message)
        End Try

    End Sub

#Region "Process Entry"

    Public Overrides Sub ProcessEntry(ByVal ctrl As SFDCData.ctrlText, ByVal Valid As Boolean)

        Select Case Valid
            Case False
                Beep()

            Case True
                Try
                    Select Case Argument("TXTYPE")
                        Case "INTERWHTX"
                            Me.ProcessEntry_INTERWHTX(ctrl)
                        Case "PUTAWAY"
                            Me.ProcessEntry_PUTAWAY(ctrl)
                        Case "CHSTATUS"
                            Me.ProcessEntry_CHSTATUS(ctrl)
                    End Select

                Catch

                End Try

        End Select

    End Sub

    Private Sub ProcessEntry_INTERWHTX(ByVal ctrl As SFDCData.ctrlText)

        ' Fill in the TOLOC when default changes (if empty)
        If ctrl.Name = "TOLOCNAME" Then
            With CtrlTable.Table
                For Y As Integer = 0 To .Items.Count - 1
                    If Len(.Items(Y).SubItems(ColNo("_TOWARHS")).Text) = 0 And Len(.Items(Y).SubItems(ColNo("_TOLOCNAME")).Text) = 0 Then
                        With .Items(Y)
                            .SubItems(ColNo("_TOWARHS")).Text = Val("TOWARHS")
                            .SubItems(ColNo("_TOLOCNAME")).Text = Val("TOLOCNAME")
                        End With
                    End If
                Next
            End With
        End If

        Try
            If CBool(Argument("SERIALPARTS")) Then
                If Len(Val("WARHS")) > 0 And Len(Val("LOCNAME")) > 0 And Len(Val("STATUS")) > 0 And Len(Val("_PARTNAME")) > 0 And Len(Val("_SERIALNAME")) > 0 Then
                    Dim sql As String = "SELECT WARHSBAL.BALANCE/1000 " & _
                        "FROM PART, WARHSBAL, WAREHOUSES, CUSTOMERS, SERIAL " & _
                        "WHERE PART.PART = WARHSBAL.PART  " & _
                        "AND WARHSBAL.SERIAL = SERIAL.SERIAL " & _
                        "AND WARHSBAL.WARHS = WAREHOUSES.WARHS  " & _
                        "AND WARHSBAL.CUST = CUSTOMERS.CUST " & _
                        "AND WAREHOUSES.WARHSNAME = '" & Val("WARHS") & "'  " & _
                        "AND WAREHOUSES.LOCNAME = '" & Val("LOCNAME") & "'  " & _
                        "AND CUSTNAME = '" & Val("STATUS") & "' " & _
                        "AND PART.PARTNAME = '" & Val("_PARTNAME") & "'" & _
                        "AND SERIAL.SERIALNAME = '%_SERIALNAME%'"
                    CtrlTable.el(ColNo("_AVAILABLE")).NSInvoke(sql)
                End If
            Else
                If Len(Val("WARHS")) > 0 And Len(Val("LOCNAME")) > 0 And Len(Val("STATUS")) > 0 And Len(Val("_PARTNAME")) > 0 Then
                    Dim sql As String = "SELECT WARHSBAL.BALANCE/1000 " & _
                        "FROM PART, WARHSBAL, WAREHOUSES, CUSTOMERS, SERIAL " & _
                        "WHERE PART.PART = WARHSBAL.PART  " & _
                        "AND WARHSBAL.SERIAL = SERIAL.SERIAL " & _
                        "AND WARHSBAL.WARHS = WAREHOUSES.WARHS  " & _
                        "AND WARHSBAL.CUST = CUSTOMERS.CUST " & _
                        "AND WAREHOUSES.WARHSNAME = '" & Val("WARHS") & "'  " & _
                        "AND WAREHOUSES.LOCNAME = '" & Val("LOCNAME") & "'  " & _
                        "AND CUSTNAME = '" & Val("STATUS") & "' " & _
                        "AND PART.PARTNAME = '" & Val("_PARTNAME") & "'"
                    CtrlTable.el(ColNo("_AVAILABLE")).NSInvoke(sql)
                End If
            End If
        Catch
        End Try

        ' *******************************************************************
        ' *** Set which controls are enabled   
        With CtrlTable

            .EnableToolbar( _
                CBool(.Table.Visible And _
                Len(Val("WARHS")) > 0 And _
                Len(Val("LOCNAME")) > 0 And _
                Len(Val("STATUS")) > 0), _
                .btnEnabled(SFDCData.CtrlTable.tButton.btnEdit), _
                .btnEnabled(SFDCData.CtrlTable.tButton.btnCopy), _
                .btnEnabled(SFDCData.CtrlTable.tButton.btnDelete), _
                .btnEnabled(SFDCData.CtrlTable.tButton.btnPost) _
            )

            Try
                .el(ColNo("_SERIALNAME")).CtrlEnabled = Len(Val("_PARTNAME")) > 0
                .el(ColNo("_TOLOCNAME")).CtrlEnabled = Len(Val("_TOWARHS")) > 0
            Catch
            End Try
        End With

        With CtrlForm
            .el(ColNo("LOCNAME")).CtrlEnabled = Len(Val("WARHS")) > 0
            .el(ColNo("STATUS")).CtrlEnabled = Len(Val("LOCNAME")) > 0
            .el(ColNo("TOLOCNAME")).CtrlEnabled = Len(Val("TOWARHS")) > 0
        End With



    End Sub

    Private Sub ProcessEntry_PUTAWAY(ByVal ctrl As SFDCData.ctrlText)

        Try
            CtrlTable.el(ColNo("_SERIALNAME")).CtrlEnabled = (Len(Val("_PARTNAME")) > 0)
        Catch
        End Try

        CtrlForm.el(ColNo("LOCNAME")).CtrlEnabled = Not (Len(Val("LOCNAME")) > 0)
        CtrlForm.el(ColNo("TOLOCNAME")).CtrlEnabled = False

        Select Case ctrl.Name
            Case "LOCNAME"
                CtrlTable.Table.Focus()

                CtrlTable.EnableToolbar( _
                    CBool(CtrlTable.Table.Visible And _
                    Len(Val("LOCNAME")) > 0), _
                    CtrlTable.btnEnabled(SFDCData.CtrlTable.tButton.btnEdit), _
                    CtrlTable.btnEnabled(SFDCData.CtrlTable.tButton.btnCopy), _
                    CtrlTable.btnEnabled(SFDCData.CtrlTable.tButton.btnDelete), _
                    CtrlTable.btnEnabled(SFDCData.CtrlTable.tButton.btnPost) _
                )

                'CtrlTable.el(3).CtrlEnabled = Len(CtrlForm.el(1).Data) > 0

            Case "_PARTNAME"
                Try
                    If Len(Val("LOCNAME")) > 0 And Len(Val("_PARTNAME")) > 0 Then
                        Dim sql As String = "SELECT SUM(WARHSBAL.BALANCE/1000) " & _
                            "FROM PART, WARHSBAL, WAREHOUSES, CUSTOMERS " & _
                            "WHERE PART.PART = WARHSBAL.PART  " & _
                            "AND WARHSBAL.WARHS = WAREHOUSES.WARHS  " & _
                            "AND WARHSBAL.CUST = CUSTOMERS.CUST " & _
                            "AND WAREHOUSES.WARHSNAME = '" & _
                            Warehouse & _
                            "' AND WAREHOUSES.LOCNAME = '" & Val("LOCNAME") & "'  " & _
                            "AND CUSTOMERS.CUST = -1 " & _
                            "AND PART.PARTNAME = '" & CtrlTable.el(ColNo("_PARTNAME")).Data & "'"
                        CtrlTable.el(ColNo("_AVAILABLE")).NSInvoke(sql)
                    End If
                Catch
                End Try

        End Select

    End Sub

    Private Sub ProcessEntry_CHSTATUS(ByVal ctrl As SFDCData.ctrlText)

        CtrlTable.EnableToolbar( _
                                        CBool(CtrlTable.Table.Visible And _
                                        Len(Val("WARHS")) > 0 And _
                                        Len(Val("LOCNAME")) > 0 And _
                                        Len(Val("STATUS")) > 0), _
                                        CtrlTable.btnEnabled(SFDCData.CtrlTable.tButton.btnEdit), _
                                        CtrlTable.btnEnabled(SFDCData.CtrlTable.tButton.btnCopy), _
                                        CtrlTable.btnEnabled(SFDCData.CtrlTable.tButton.btnDelete), _
                                        CtrlTable.btnEnabled(SFDCData.CtrlTable.tButton.btnPost) _
                                    )

        CtrlForm.el(ColNo("LOCNAME")).CtrlEnabled = Len(Val("WARHS")) > 0
        CtrlForm.el(ColNo("STATUS")).CtrlEnabled = Len(Val("LOCNAME")) > 0

        Try
            If CBool(Argument("SERIALPARTS")) Then
                If Len(Val("WARHS")) > 0 And Len(Val("LOCNAME")) > 0 And Len(Val("STATUS")) > 0 And Len(CtrlTable.el(ColNo("_PARTNAME")).Data) > 0 Then
                    Dim sql As String = "SELECT WARHSBAL.BALANCE/1000 " & _
                        "FROM PART, WARHSBAL, WAREHOUSES, CUSTOMERS, SERIAL " & _
                        "WHERE PART.PART = WARHSBAL.PART  " & _
                        "AND WARHSBAL.SERIAL = SERIAL.SERIAL " & _
                        "AND WARHSBAL.WARHS = WAREHOUSES.WARHS  " & _
                        "AND WARHSBAL.CUST = CUSTOMERS.CUST " & _
                        "AND WAREHOUSES.WARHSNAME = '%WARHS%'  " & _
                        "AND WAREHOUSES.LOCNAME = '%LOCNAME%'  " & _
                        "AND CUSTOMERS.CUSTNAME = '%STATUS%' " & _
                        "AND PART.PARTNAME = '" & CtrlTable.el(ColNo("_PARTNAME")).Data & "' " & _
                        "AND SERIAL.SERIALNAME = '%_SERIALNAME%'"
                    CtrlTable.el(ColNo("_AVAILABLE")).NSInvoke(sql)
                End If
            Else
                If Len(Val("WARHS")) > 0 And Len(Val("LOCNAME")) > 0 And Len(Val("STATUS")) > 0 And Len(CtrlTable.el(ColNo("_PARTNAME")).Data) > 0 Then
                    Dim sql As String = "SELECT WARHSBAL.BALANCE/1000 " & _
                        "FROM PART, WARHSBAL, WAREHOUSES, CUSTOMERS, SERIAL " & _
                        "WHERE PART.PART = WARHSBAL.PART  " & _
                        "AND WARHSBAL.SERIAL = SERIAL.SERIAL " & _
                        "AND WARHSBAL.WARHS = WAREHOUSES.WARHS  " & _
                        "AND WARHSBAL.CUST = CUSTOMERS.CUST " & _
                        "AND WAREHOUSES.WARHSNAME = '%WARHS%'  " & _
                        "AND WAREHOUSES.LOCNAME = '%LOCNAME%'  " & _
                        "AND CUSTOMERS.CUSTNAME = '%STATUS%' " & _
                        "AND PART.PARTNAME = '" & CtrlTable.el(ColNo("_PARTNAME")).Data & "' "
                    CtrlTable.el(ColNo("_AVAILABLE")).NSInvoke(sql)
                End If
            End If
        Catch
        End Try

    End Sub

#End Region

#Region "ProcessForm"

    Public Overrides Sub ProcessForm()

        Try
            Select Case CBool(Argument("SERIALPARTS"))
                Case True
                    Select Case Argument("TXTYPE")
                        Case "INTERWHTX"
                            ProcessForm_INTERWHTX()
                        Case "PUTAWAY"
                            ProcessForm_PUTAWAY()
                        Case "CHSTATUS"
                            ProcessForm_CHSTATUS()
                    End Select
                Case Else
                    Select Case Argument("TXTYPE")
                        Case "INTERWHTX"
                            ProcessForm_nsINTERWHTX()
                        Case "PUTAWAY"
                            ProcessForm_nsPUTAWAY()
                        Case "CHSTATUS"
                            ProcessForm_nsCHSTATUS()
                    End Select
            End Select


        Catch e As Exception
            MsgBox(e.Message)
        End Try

    End Sub

    Private Sub ProcessForm_INTERWHTX()

        With p
            .DebugFlag = True
            .Procedure = "ZSFDCP_LOAD_WHTX"
            .Table = "ZSFDC_LOAD_WHTX"
            .RecordType1 = "OWNERLOGIN,DOCNO,WARHSNAME,LOCNAME,TOWARHSNAME,TOLOCATION"
            .RecordType2 = "PARTNAME,TQUANT,TOWARHSNAME2,TOLOCNAME,STATUS1,TOSTATUS,SERIALNAME,TRANS"
            .RecordTypes = "TEXT,TEXT,TEXT,TEXT,TEXT,TEXT,TEXT,,TEXT,TEXT,TEXT,TEXT,TEXT,"
        End With

        ' Type 1 records
        Dim t1() As String = { _
                            UserName, _
                            Argument("DOCNO"), _
                            CtrlForm.ItemValue("WARHS"), _
                            CtrlForm.ItemValue("LOCNAME"), _
                            CtrlForm.ItemValue("TOWARHS"), _
                            CtrlForm.ItemValue("TOLOCNAME") _
                            }
        p.AddRecord(1) = t1

        For y As Integer = 0 To CtrlTable.RowCount
            Dim t As String = CtrlTable.ItemValue("_TRANS", y)
            If t.Length = 0 Then t = "0"
            Dim t2() As String = { _
                        CtrlTable.ItemValue("_PARTNAME", y), _
                        CStr(CInt(CtrlTable.ItemValue("_TQUANT", y)) * 1000), _
                        CtrlTable.ItemValue("_TOWARHS", y), _
                        CtrlTable.ItemValue("_TOLOCNAME", y), _
                        CtrlForm.ItemValue("STATUS"), _
                        CtrlForm.ItemValue("STATUS"), _
                        CtrlTable.ItemValue("_SERIALNAME", y), _
                        t _
                                }
            p.AddRecord(2) = t2
        Next

    End Sub

    Private Sub ProcessForm_PUTAWAY()


        With p
            .DebugFlag = True
            .Procedure = "ZSFDCP_LOAD_WHTX"
            .Table = "ZSFDC_LOAD_WHTX"
            .RecordType1 = "OWNERLOGIN,WARHSNAME,LOCNAME,TOWARHSNAME"
            .RecordType2 = "PARTNAME,TQUANT,TOWARHSNAME2,TOLOCNAME,TOSTATUS,SERIALNAME"
            .RecordTypes = "TEXT,TEXT,TEXT,TEXT,TEXT,,TEXT,TEXT,TEXT,TEXT"
        End With

        ' Type 1 records
        Dim t1() As String = { _
                            UserName, _
                            Warehouse, _
                            CtrlForm.ItemValue("LOCNAME"), _
                            Warehouse _
                            }
        p.AddRecord(1) = t1

        For y As Integer = 0 To CtrlTable.RowCount
            Dim t2() As String = { _
                        CtrlTable.ItemValue("_PARTNAME", y), _
                        CStr(CInt(CtrlTable.ItemValue("_TRANSFER", y)) * 1000), _
                        Warehouse, _
                        CtrlTable.ItemValue("_TOLOCNAME", y), _
                        "Goods", _
                        CtrlTable.ItemValue("_SERIALNAME", y) _
                                }
            p.AddRecord(2) = t2
        Next

    End Sub

    Private Sub ProcessForm_CHSTATUS()

        With p
            .DebugFlag = True
            .Procedure = "ZSFDCP_LOAD_WHTX"
            .Table = "ZSFDC_LOAD_WHTX"
            .RecordType1 = "OWNERLOGIN,WARHSNAME,LOCNAME,TOWARHSNAME,TOLOCATION"
            .RecordType2 = "PARTNAME,TQUANT,STATUS1,TOSTATUS,SERIALNAME"
            .RecordTypes = "TEXT,TEXT,TEXT,TEXT,TEXT,TEXT,,TEXT,TEXT,TEXT"
        End With

        ' Type 1 records
        Dim t1() As String = { _
                            UserName, _
                            CtrlForm.ItemValue("WARHS"), _
                            CtrlForm.ItemValue("LOCNAME"), _
                            CtrlForm.ItemValue("WARHS"), _
                            CtrlForm.ItemValue("LOCNAME") _
                            }
        p.AddRecord(1) = t1

        For y As Integer = 0 To CtrlTable.RowCount
            Dim t2() As String = { _
                        CtrlTable.ItemValue("_PARTNAME", y), _
                        CStr(CInt(CtrlTable.ItemValue("_TQUANT", y)) * 1000), _
                        CtrlForm.ItemValue("STATUS"), _
                        CtrlTable.ItemValue("_STATUS", y), _
                        CtrlTable.ItemValue("_SERIALNAME", y) _
                                }
            p.AddRecord(2) = t2
        Next

    End Sub

    Private Sub ProcessForm_nsINTERWHTX()

        With p
            .DebugFlag = True
            .Procedure = "ZSFDCP_LOAD_WHTX"
            .Table = "ZSFDC_LOAD_WHTX"
            .RecordType1 = "OWNERLOGIN,DOCNO,WARHSNAME,LOCNAME,TOWARHSNAME,TOLOCATION"
            .RecordType2 = "PARTNAME,TQUANT,TOWARHSNAME2,TOLOCNAME,STATUS1,TOSTATUS,TRANS"
            .RecordTypes = "TEXT,TEXT,TEXT,TEXT,TEXT,TEXT,TEXT,,TEXT,TEXT,TEXT,TEXT,"
        End With

        ' Type 1 records
        Dim t1() As String = { _
                            UserName, _
                            Argument("DOCNO"), _
                            CtrlForm.ItemValue("WARHS"), _
                            CtrlForm.ItemValue("LOCNAME"), _
                            CtrlForm.ItemValue("TOWARHS"), _
                            CtrlForm.ItemValue("TOLOCNAME") _
                            }
        p.AddRecord(1) = t1

        For y As Integer = 0 To CtrlTable.RowCount
            Dim t As String = CtrlTable.ItemValue("_TRANS", y)
            If t.Length = 0 Then t = "0"
            Dim t2() As String = { _
                        CtrlTable.ItemValue("_PARTNAME", y), _
                        CStr(CInt(CtrlTable.ItemValue("_TQUANT", y)) * 1000), _
                        CtrlTable.ItemValue("_TOWARHS", y), _
                        CtrlTable.ItemValue("_TOLOCNAME", y), _
                        CtrlForm.ItemValue("STATUS"), _
                        CtrlForm.ItemValue("STATUS"), _
                        t _
                                }
            p.AddRecord(2) = t2
        Next

    End Sub

    Private Sub ProcessForm_nsPUTAWAY()

        With p
            .DebugFlag = True
            .Procedure = "ZSFDCP_LOAD_WHTX"
            .Table = "ZSFDC_LOAD_WHTX"
            .RecordType1 = "OWNERLOGIN,WARHSNAME,LOCNAME,TOWARHSNAME"
            .RecordType2 = "PARTNAME,TQUANT,TOWARHSNAME2,TOLOCNAME,TOSTATUS"
            .RecordTypes = "TEXT,TEXT,TEXT,TEXT,TEXT,,TEXT,TEXT,TEXT"
        End With

        ' Type 1 records
        Dim t1() As String = { _
                            UserName, _
                            Warehouse, _
                            CtrlForm.ItemValue("LOCNAME"), _
                            Warehouse _
                            }
        p.AddRecord(1) = t1

        For y As Integer = 0 To CtrlTable.RowCount
            Dim t2() As String = { _
                        CtrlTable.ItemValue("_PARTNAME", y), _
                        CStr(CInt(CtrlTable.ItemValue("_TRANSFER", y)) * 1000), _
                        Warehouse, _
                        CtrlTable.ItemValue("_TOLOCNAME", y), _
                        "Goods" _
                                }
            p.AddRecord(2) = t2
        Next

    End Sub

    Private Sub ProcessForm_nsCHSTATUS()

        With p
            .DebugFlag = True
            .Procedure = "ZSFDCP_LOAD_WHTX"
            .Table = "ZSFDC_LOAD_WHTX"
            .RecordType1 = "OWNERLOGIN,WARHSNAME,LOCNAME,TOWARHSNAME,TOLOCATION"
            .RecordType2 = "PARTNAME,TQUANT,STATUS1,TOSTATUS"
            .RecordTypes = "TEXT,TEXT,TEXT,TEXT,TEXT,TEXT,,TEXT,TEXT"
        End With

        ' Type 1 records
        Dim t1() As String = { _
                            UserName, _
                            CtrlForm.ItemValue("WARHS"), _
                            CtrlForm.ItemValue("LOCNAME"), _
                            CtrlForm.ItemValue("WARHS"), _
                            CtrlForm.ItemValue("LOCNAME") _
                            }
        p.AddRecord(1) = t1

        For y As Integer = 0 To CtrlTable.RowCount
            Dim t2() As String = { _
                        CtrlTable.ItemValue("_PARTNAME", y), _
                        CStr(CInt(CtrlTable.ItemValue("_TQUANT", y)) * 1000), _
                        CtrlForm.ItemValue("STATUS"), _
                        CtrlTable.ItemValue("_STATUS", y) _
                                }
            p.AddRecord(2) = t2
        Next

    End Sub

#End Region

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

    Public Overrides Sub BeginAdd()

        Select Case Argument("TXTYPE")
            Case "INTERWHTX"
                CtrlTable.mCol(ColNo("_PARTNAME")).ctrlEnabled = True
                If CBool(Argument("SERIALPARTS")) Then CtrlTable.mCol(ColNo("_SERIALNAME")).ctrlEnabled = False
                CtrlTable.mCol(ColNo("_AVAILABLE")).ctrlEnabled = False
                CtrlTable.mCol(ColNo("_TQUANT")).ctrlEnabled = True
                CtrlTable.mCol(ColNo("_TOWARHS")).ctrlEnabled = True
                CtrlTable.mCol(ColNo("_TOLOCNAME")).ctrlEnabled = True

            Case "PUTAWAY"
                CtrlTable.mCol(ColNo("_PARTNAME")).ctrlEnabled = True
                CtrlTable.mCol(ColNo("_AVAILABLE")).ctrlEnabled = False
                CtrlTable.mCol(ColNo("_TRANSFER")).ctrlEnabled = True
                CtrlTable.mCol(ColNo("_TOLOCNAME")).ctrlEnabled = True
                If CBool(Argument("SERIALPARTS")) Then CtrlTable.mCol(ColNo("_SERIALNAME")).ctrlEnabled = False

            Case "CHSTATUS"
                CtrlTable.mCol(ColNo("_PARTNAME")).ctrlEnabled = True
                CtrlTable.mCol(ColNo("_AVAILABLE")).ctrlEnabled = False
                CtrlTable.mCol(ColNo("_TQUANT")).ctrlEnabled = True
                If CBool(Argument("SERIALPARTS")) Then CtrlTable.mCol(ColNo("_STATUS")).ctrlEnabled = True

        End Select
    End Sub

    Public Overrides Sub BeginEdit()

        Select Case Argument("TXTYPE")
            Case "INTERWHTX"
                CtrlTable.mCol(ColNo("_PARTNAME")).ctrlEnabled = True
                If CBool(Argument("SERIALPARTS")) Then CtrlTable.mCol(ColNo("_SERIALNAME")).ctrlEnabled = True
                CtrlTable.mCol(ColNo("_AVAILABLE")).ctrlEnabled = False
                CtrlTable.mCol(ColNo("_TQUANT")).ctrlEnabled = True
                CtrlTable.mCol(ColNo("_TOWARHS")).ctrlEnabled = True
                CtrlTable.mCol(ColNo("_TOLOCNAME")).ctrlEnabled = True

            Case "PUTAWAY"
                CtrlTable.mCol(ColNo("_PARTNAME")).ctrlEnabled = True
                CtrlTable.mCol(ColNo("_AVAILABLE")).ctrlEnabled = False
                CtrlTable.mCol(ColNo("_TRANSFER")).ctrlEnabled = True
                CtrlTable.mCol(ColNo("_TOLOCNAME")).ctrlEnabled = True
                If CBool(Argument("SERIALPARTS")) Then CtrlTable.mCol(ColNo("_SERIALNAME")).ctrlEnabled = True

            Case "CHSTATUS"
                CtrlTable.mCol(ColNo("_PARTNAME")).ctrlEnabled = True
                CtrlTable.mCol(ColNo("_AVAILABLE")).ctrlEnabled = False
                CtrlTable.mCol(ColNo("_TQUANT")).ctrlEnabled = True
                CtrlTable.mCol(ColNo("_STATUS")).ctrlEnabled = True

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

#Region "Table Scan"

    Public Overrides Sub TableScan(ByVal Value As String)

        Select Case Argument("TXTYPE")
            Case "INTERWHTX"
                If Regex.IsMatch(Value, ValidStr(tRegExValidation.tBarcode)) Then
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
                                                "AND BARCODE = '" & Value & "'")

                ElseIf Regex.IsMatch(Value, ValidStr(tRegExValidation.tWhsTX)) Then
                    If Len(Argument("DOCNO")) = 0 Then
                        mInvoke = tInvoke.iWarhsTX
                        InvokeData("SELECT DOCNO, " & _
                        "FWH.WARHSNAME, " & _
                        "FWH.LOCNAME, " & _
                        "(SELECT CUSTNAME FROM CUSTOMERS WHERE CUST = -1), " & _
                        "TWH.WARHSNAME, " & _
                        "TWH.LOCNAME " & _
                        "FROM DOCUMENTS, WAREHOUSES AS FWH, WAREHOUSES AS TWH " & _
                        "WHERE DOCUMENTS.TOWARHS = TWH.WARHS " & _
                        "AND DOCUMENTS.WARHS = FWH.WARHS " & _
                        "AND DOC = (SELECT DOC FROM DOCUMENTS WHERE DOCNO = '" & Value & "') " & _
                        "AND FINAL <> 'Y'")
                    End If
                    End If

            Case "CHSTATUS"
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
                                                "AND BARCODE = '" & Value & "'")

            Case "PUTAWAY"
                    If Regex.IsMatch(Value, ValidStr(tRegExValidation.tGRV)) Then

                    CtrlForm.el(ColNo("_PARTNAME")).LostFocus()
                        CtrlTable.Table.Focus()


                        Dim sql As String = "SELECT LOCNAME " & _
                            "FROM WAREHOUSES, DOCUMENTS " & _
                            "WHERE DOCUMENTS.TOWARHS = WAREHOUSES.WARHS " & _
                            "AND DOCNO = '" & Value & "'"
                        CtrlForm.el(0).NSInvoke(sql)


                        ' Set the query to load recordtype 2s
                        If CBool(Argument("SERIALPARTS")) Then
                            CtrlTable.RecordsSQL = "SELECT PART.PARTNAME, BALANCE/1000 , BALANCE/1000 , '', SERIAL.SERIALNAME " & _
                                                    "FROM TRANSORDER , PART ,WARHSBAL ,SERIAL " & _
                                                    "WHERE TRANSORDER.PART = PART.PART " & _
                                                    "AND WARHSBAL.PART = TRANSORDER.PART " & _
                                                    "AND WARHSBAL.CUST = TRANSORDER.CUST " & _
                                                    "AND WARHSBAL.WARHS = TRANSORDER.TOWARHS " & _
                                                    "AND WARHSBAL.ACT = TRANSORDER.ACT " & _
                                                    "AND WARHSBAL.SERIAL = TRANSORDER.SERIAL " & _
                                                    "AND TRANSORDER.SERIAL = SERIAL.SERIAL " & _
                                                    "AND TRANSORDER.PART IN  " & _
                                                    "(SELECT TRANSORDER.PART FROM TRANSORDER WHERE DOC = " & _
                                                    "(SELECT DOC FROM DOCUMENTS WHERE DOCNO = '" & Value & "')" & _
                                                    ")" & _
                                                    "AND WARHSBAL.WARHS IN " & _
                                                    "(SELECT TRANSORDER.TOWARHS FROM TRANSORDER WHERE DOC = " & _
                                                    "(SELECT DOC FROM DOCUMENTS WHERE DOCNO = '" & Value & "')" & _
                                                    ")" & _
                                                    "AND BALANCE > 0 AND WARHSBAL.ACT = 0"
                        Else
                            CtrlTable.RecordsSQL = "SELECT PART.PARTNAME, BALANCE/1000 , BALANCE/1000 , '' " & _
                                                    "FROM TRANSORDER , PART ,WARHSBAL ,SERIAL " & _
                                                    "WHERE TRANSORDER.PART = PART.PART " & _
                                                    "AND WARHSBAL.PART = TRANSORDER.PART " & _
                                                    "AND WARHSBAL.CUST = TRANSORDER.CUST " & _
                                                    "AND WARHSBAL.WARHS = TRANSORDER.TOWARHS " & _
                                                    "AND WARHSBAL.ACT = TRANSORDER.ACT " & _
                                                    "AND WARHSBAL.SERIAL = TRANSORDER.SERIAL " & _
                                                    "AND TRANSORDER.SERIAL = SERIAL.SERIAL " & _
                                                    "AND TRANSORDER.PART IN  " & _
                                                    "(SELECT TRANSORDER.PART FROM TRANSORDER WHERE DOC = " & _
                                                    "(SELECT DOC FROM DOCUMENTS WHERE DOCNO = '" & Value & "')" & _
                                                    ")" & _
                                                    "AND WARHSBAL.WARHS IN " & _
                                                    "(SELECT TRANSORDER.TOWARHS FROM TRANSORDER WHERE DOC = " & _
                                                    "(SELECT DOC FROM DOCUMENTS WHERE DOCNO = '" & Value & "')" & _
                                                    ")" & _
                                                    "AND BALANCE > 0 AND WARHSBAL.ACT = 0"
                        End If

                        CtrlTable.BeginLoadRS()

                        mInvoke = tInvoke.iBarcode
                        InvokeData("SELECT LOCNAME FROM WAREHOUSES WHERE LOCNAME = '" & Value & "' AND WARHSNAME = " & _
                        "(SELECT WARHSNAME FROM v_USERS where USERLOGIN = '" & UserName & "')")


                    ElseIf Regex.IsMatch(Value, ValidStr(tRegExValidation.tBarcode)) Then
                        mInvoke = tInvoke.iBarcode
                        InvokeData("SELECT PARTNAME, BARCODE " & _
                                                    "FROM PART, WARHSBAL, WAREHOUSES, CUSTOMERS " & _
                                                    "WHERE PART.PART = WARHSBAL.PART  " & _
                                                    "AND WARHSBAL.WARHS = WAREHOUSES.WARHS  " & _
                                                    "AND WARHSBAL.CUST = CUSTOMERS.CUST " & _
                                                    "AND WAREHOUSES.WARHSNAME = " & _
                                                    "(SELECT WARHSNAME FROM v_USERS where USERLOGIN = '" & UserName & "')  " & _
                                                    "AND WAREHOUSES.LOCNAME = '%LOCNAME%'  " & _
                                                    "AND WARHSBAL.BALANCE > 0 " & _
                                                    "AND CUSTOMERS.CUST = -1 " & _
                                                    "AND BARCODE = '" & Value & "'")

                    ElseIf Regex.IsMatch(Value, ValidStr(tRegExValidation.tLocname)) Then
                        mInvoke = tInvoke.iLoc
                        InvokeData("SELECT LOCNAME FROM WAREHOUSES WHERE LOCNAME = '" & Value & "' AND WARHSNAME = " & _
                            "(SELECT WARHSNAME FROM v_USERS where USERLOGIN = '" & UserName & "')")
                    End If

        End Select

    End Sub

    Public Overrides Sub EndInvokeData(ByVal Data(,) As String)

        Dim f As Boolean = False
        Select Case mInvoke

            Case tInvoke.iBarcode
                If IsNothing(Data) Then
                    MsgBox("Part does not exist in this location.")
                    Exit Sub
                End If

                For i As Integer = 0 To CtrlTable.Table.Items.Count - 1
                    If CtrlTable.Table.Items(i).SubItems(0).Text = Data(0, 0) Then

                        ' Found Record
                        Select Case Argument("TXTYPE")
                            Case "INTERWHTX"
                                CtrlTable.Table.Items(i).Selected = True
                                CtrlTable.SetEdit()

                            Case "PUTAWAY"
                                CtrlTable.Table.Items(i).Selected = True

                                If Len(CtrlForm.el(ColNo("TOLOCNAME")).Value.Text) > 0 Then
                                    CtrlTable.Table.Items(i).SubItems(ColNo("_TOLOCNAME")).Text = CtrlForm.el(ColNo("TOLOCNAME")).Value.Text
                                    CtrlTable.Table.Focus()
                                Else
                                    CtrlTable.SetEdit()
                                    If Len(CtrlForm.el(ColNo("TOLOCNAME")).Value.Text) > 0 Then
                                        With CtrlTable
                                            With .el(ColNo("_TOLOCNAME"))
                                                .DataEntry.Text = CtrlForm.el(ColNo("TOLOCNAME")).Value.Text
                                                .ProcessEntry()
                                                .CtrlEnabled = Len(CtrlForm.el(ColNo("TOLOCNAME")).Data) > 0
                                            End With
                                        End With
                                    End If
                                End If

                            Case "CHSTATUS"
                                CtrlTable.Table.Items(i).Selected = True
                                CtrlTable.SetEdit()
                        End Select
                        f = True

                    End If
                Next

                If Not f Then
                    ' No record found
                    Select Case Argument("TXTYPE")
                        Case "INTERWHTX"
                            If CtrlTable.btnEnabled(SFDCData.CtrlTable.tButton.btnAdd) Then
                                With CtrlTable
                                    .SetAdd()
                                    With .el(.ColNo("_PARTNAME"))
                                        .DataEntry.Text = Data(1, 0)
                                        .ProcessEntry()
                                    End With
                                End With
                            End If

                        Case "PUTAWAY"
                            If CtrlTable.btnEnabled(SFDCData.CtrlTable.tButton.btnAdd) Then
                                With CtrlTable
                                    .SetAdd()
                                    With .el(.ColNo("_PARTNAME"))
                                        .DataEntry.Text = Data(1, 0)
                                        .ProcessEntry()
                                    End With
                                End With
                            End If

                        Case "CHSTATUS"
                            If CtrlTable.btnEnabled(SFDCData.CtrlTable.tButton.btnAdd) Then
                                With CtrlTable
                                    .SetAdd()
                                    With .el(.ColNo("_PARTNAME"))
                                        .DataEntry.Text = Data(1, 0)
                                        .ProcessEntry()
                                    End With
                                End With
                            End If

                    End Select
                End If

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

            Case tInvoke.iWarhsTX

                If Not IsNothing(Data) Then

                    Argument("DOCNO") = Data(0, 0)

                    With CtrlForm
                        With .el(ColNo("WARHS"))
                            .DataEntry.Text = Data(1, 0)
                            .ProcessEntry()
                        End With
                    End With

                    With CtrlForm
                        With .el(ColNo("LOCNAME"))
                            .DataEntry.Text = Data(2, 0)
                            .ProcessEntry()
                        End With
                    End With

                    With CtrlForm
                        With .el(ColNo("STATUS"))
                            .DataEntry.Text = Data(3, 0)
                            .ProcessEntry()
                        End With
                    End With

                    With CtrlForm
                        With .el(ColNo("TOWARHS"))
                            .DataEntry.Text = Data(4, 0)
                            .ProcessEntry()
                        End With
                    End With

                    With CtrlForm
                        With .el(ColNo("TOLOCNAME"))
                            .DataEntry.Text = Data(5, 0)
                            .ProcessEntry()
                        End With
                    End With

                    CtrlTable.Table.Focus()

                    ' Set the query to load recordtype 2s
                    If CBool(Argument("SERIALPARTS")) Then
                        CtrlTable.RecordsSQL = "SELECT PART.PARTNAME, TQUANT/1000 , TQUANT/1000 , SERIAL.SERIALNAME, WAREHOUSES.WARHSNAME, WAREHOUSES.LOCNAME, TRANSORDER.TRANS " & _
                                                "FROM TRANSORDER , PART , SERIAL, WAREHOUSES  " & _
                                                "WHERE TRANSORDER.PART = PART.PART  " & _
                                                "AND WAREHOUSES.WARHS = TRANSORDER.TOWARHS " & _
                                                "AND TRANSORDER.SERIAL = SERIAL.SERIAL  " & _
                                                "AND TRANSORDER.DOC  =  " & _
                                                "(SELECT DOC FROM DOCUMENTS WHERE DOCNO = '" & Data(0, 0) & "')"
                    Else
                        CtrlTable.RecordsSQL = "SELECT PART.PARTNAME, TQUANT/1000 , TQUANT/1000 , WAREHOUSES.WARHSNAME, WAREHOUSES.LOCNAME, TRANSORDER.TRANS " & _
                                                "FROM TRANSORDER , PART , WAREHOUSES  " & _
                                                "WHERE TRANSORDER.PART = PART.PART  " & _
                                                "AND WAREHOUSES.WARHS = TRANSORDER.TOWARHS " & _
                                                "AND TRANSORDER.DOC  =  " & _
                                                "(SELECT DOC FROM DOCUMENTS WHERE DOCNO = '" & Data(0, 0) & "')"
                    End If
                    CtrlTable.BeginLoadRS()

                End If

        End Select

    End Sub

#End Region

    Private Sub Ending() Handles MyBase.EndForm
        Argument("DOCNO") = ""
    End Sub

End Class
