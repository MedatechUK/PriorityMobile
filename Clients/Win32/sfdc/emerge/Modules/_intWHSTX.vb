Imports System
Imports System.IO
Imports System.Threading
Imports System.Text.RegularExpressions

Public Class InterfaceWHTX
    Inherits w32SFDCData.iForm

    Private Enum tInvoke
        iBarcode = 0
        iLoc = 1
    End Enum

    Private mInvoke As tInvoke = tInvoke.iBarcode

#Region "Initialisation"

    Public Sub New(Optional ByRef App As Form = Nothing)

        InitializeComponent()
        CallerApp = App
        NewArgument("TXTYPE", "INTERWHTX")
        CtrlTable.DisableButtons(False, False, False, False, False)
        CtrlTable.EnableToolbar(False, False, False, False, False)

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

                '**************************************************************************
            Case "PUTAWAY"

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
                    .SQLList = "SELECT DISTINCT WARHSNAME FROM WAREHOUSES AND INACTIVE <> 'Y'"
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
                    .ValidExp = ValidStr(tRegExValidation.tLocname)
                    .SQLList = "SELECT DISTINCT LOCNAME FROM WAREHOUSES WHERE WARHSNAME = '%TOWARHS%' AND INACTIVE <> 'Y'"
                    .SQLValidation = "SELECT LOCNAME FROM WAREHOUSES WHERE LOCNAME = '%ME%' AND WARHSNAME = '%TOWARHS%'"
                    .DefaultFromCtrl = CtrlForm.el(4)  '******* Barcoded Field - default from Type1 TOLOCATION '*******
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
                    .ValidExp = ValidStr(tRegExValidation.tBarcode)
                    ' Second field replaces first field if first field validates ok
                    .SQLValidation = "SELECT BARCODE, PARTNAME " & _
                                    "FROM PART, WARHSBAL, WAREHOUSES, CUSTOMERS " & _
                                    "WHERE PART.PART = WARHSBAL.PART  " & _
                                    "AND WARHSBAL.WARHS = WAREHOUSES.WARHS  " & _
                                    "AND WARHSBAL.CUST = CUSTOMERS.CUST " & _
                                    "AND WAREHOUSES.WARHSNAME = " & _
                                    "(SELECT WARHSNAME FROM v_USERS where USERLOGIN = '" & UserName & "')  " & _
                                    "AND WAREHOUSES.LOCNAME = '%LOCNAME%'  " & _
                                    "AND WARHSBAL.BALANCE > 0 " & _
                                    "AND CUSTOMERS.CUST= -1 " & _
                                    "AND BARCODE = '%ME%'"
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
                    .Name = "_TRANSFER"
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
            MsgBox(e.Message)
        End Try

    End Sub

    Public Overrides Sub ProcessEntry(ByVal ctrl As w32SFDCData.ctrlText, ByVal Valid As Boolean)

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
                                CtrlTable.btnEnabled(w32SFDCData.CtrlTable.tButton.btnEdit), _
                                CtrlTable.btnEnabled(w32SFDCData.CtrlTable.tButton.btnCopy), _
                                CtrlTable.btnEnabled(w32SFDCData.CtrlTable.tButton.btnDelete), _
                                CtrlTable.btnEnabled(w32SFDCData.CtrlTable.tButton.btnPost) _
                            )

                            CtrlForm.el(1).CtrlEnabled = Len(CtrlForm.el(0).Data) > 0
                            CtrlForm.el(2).CtrlEnabled = Len(CtrlForm.el(1).Data) > 0
                            CtrlForm.el(4).CtrlEnabled = Len(CtrlForm.el(3).Data) > 0

                            CtrlTable.el(4).CtrlEnabled = Len(CtrlTable.el(3).Data) > 0


                        Case "PUTAWAY"

                            If ctrl.Name = "LOCNAME" Then CtrlTable.Table.Focus()

                            CtrlTable.EnableToolbar( _
                                CBool(CtrlTable.Table.Visible And _
                                Len(CtrlForm.el(0).Data) > 0), _
                                CtrlTable.btnEnabled(w32SFDCData.CtrlTable.tButton.btnEdit), _
                                CtrlTable.btnEnabled(w32SFDCData.CtrlTable.tButton.btnCopy), _
                                CtrlTable.btnEnabled(w32SFDCData.CtrlTable.tButton.btnDelete), _
                                CtrlTable.btnEnabled(w32SFDCData.CtrlTable.tButton.btnPost) _
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
                                End If
                            Catch
                            End Try

                        Case "CHSTATUS"

                            CtrlTable.EnableToolbar( _
                                CBool(CtrlTable.Table.Visible And _
                                Len(CtrlForm.el(0).Data) > 0 And _
                                Len(CtrlForm.el(1).Data) > 0 And _
                                Len(CtrlForm.el(2).Data) > 0), _
                                CtrlTable.btnEnabled(w32SFDCData.CtrlTable.tButton.btnEdit), _
                                CtrlTable.btnEnabled(w32SFDCData.CtrlTable.tButton.btnCopy), _
                                CtrlTable.btnEnabled(w32SFDCData.CtrlTable.tButton.btnDelete), _
                                CtrlTable.btnEnabled(w32SFDCData.CtrlTable.tButton.btnPost) _
                            )

                            CtrlForm.el(1).CtrlEnabled = Len(CtrlForm.el(0).Data) > 0
                            CtrlForm.el(2).CtrlEnabled = Len(CtrlForm.el(1).Data) > 0



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
                                End If
                            Catch
                            End Try

                    End Select

                Catch

                End Try

        End Select

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

                Case "INTERWHTX"

                    Try
                        With p
                            .DebugFlag = True
                            .Procedure = "ZSFDCP_LOAD_WHTX"
                            .Table = "ZSFDC_LOAD_WHTX"
                            .RecordType1 = "OWNERLOGIN,WARHSNAME,LOCNAME,TOWARHSNAME,TOLOCATION"
                            .RecordType2 = "PARTNAME,TQUANT,TOWARHSNAME2,TOLOCNAME,STATUS1,TOSTATUS"
                            .RecordTypes = "TEXT,TEXT,TEXT,TEXT,TEXT,TEXT,,TEXT,TEXT,TEXT"
                        End With

                    Catch e As Exception
                        MsgBox(e.Message)
                    End Try

                    ' Type 1 records
                    Dim t1() As String = { _
                                        UserName, _
                                        CtrlForm.ItemValue("WARHS"), _
                                        CtrlForm.ItemValue("LOCNAME"), _
                                        CtrlForm.ItemValue("TOWARHS"), _
                                        CtrlForm.ItemValue("TOLOCNAME") _
                                        }
                    p.AddRecord(1) = t1

                    For y As Integer = 0 To CtrlTable.RowCount
                        Dim t2() As String = { _
                                    CtrlTable.ItemValue("_PARTNAME", y), _
                                    CtrlTable.ItemValue("_TQUANT", y), _
                                    CtrlTable.ItemValue("_TOWARHS", y), _
                                    CtrlTable.ItemValue("_TOLOCNAME", y), _
                                    CtrlForm.ItemValue("STATUS"), _
                                    CtrlForm.ItemValue("STATUS") _
                                            }
                        p.AddRecord(2) = t2
                    Next

                Case "PUTAWAY"

                    Try
                        With p
                            .DebugFlag = True
                            .Procedure = "ZSFDCP_LOAD_WHTX"
                            .Table = "ZSFDC_LOAD_WHTX"
                            .RecordType1 = "OWNERLOGIN,WARHSNAME,LOCNAME,TOWARHSNAME"
                            .RecordType2 = "PARTNAME,TQUANT,TOWARHSNAME2,TOLOCNAME,TOSTATUS"
                            .RecordTypes = "TEXT,TEXT,TEXT,TEXT,TEXT,,TEXT,TEXT,TEXT"
                        End With

                    Catch e As Exception
                        MsgBox(e.Message)
                    End Try

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
                                    CtrlTable.ItemValue("_TRANSFER", y), _
                                    Warehouse, _
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
                            .RecordType1 = "OWNERLOGIN,WARHSNAME,LOCNAME,TOWARHSNAME,TOLOCATION"
                            .RecordType2 = "PARTNAME,TQUANT,STATUS1,TOSTATUS"
                            .RecordTypes = "TEXT,TEXT,TEXT,TEXT,TEXT,TEXT,,TEXT,TEXT"
                        End With

                    Catch e As Exception
                        MsgBox(e.Message)
                    End Try

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
                                    CtrlTable.ItemValue("_TQUANT", y), _
                                    CtrlForm.ItemValue("STATUS"), _
                                    CtrlTable.ItemValue("_STATUS", y) _
                                            }
                        p.AddRecord(2) = t2
                    Next

            End Select

        Catch e As Exception
            MsgBox(e.Message)
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
                CtrlTable.mCol(3).ctrlEnabled = True


        End Select
    End Sub

    Public Overrides Sub BeginEdit()
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
                CtrlTable.mCol(3).ctrlEnabled = True

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

    Public Overrides Sub TableScan(ByVal Value As String)

        Select Case Argument("TXTYPE")
            Case "INTERWHTX", "CHSTATUS"
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
                If Regex.IsMatch(Value, ValidStr(tRegExValidation.tBarcode)) Then
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

                                If Len(CtrlForm.el(1).Value.Text) > 0 Then
                                    CtrlTable.Table.Items(i).SubItems(3).Text = CtrlForm.el(1).Value.Text
                                    CtrlTable.Table.Focus()
                                Else
                                    CtrlTable.SetEdit()
                                    If Len(CtrlForm.el(1).Value.Text) > 0 Then
                                        With CtrlTable
                                            With .el(3)
                                                .DataEntry.Text = CtrlForm.el(1).Value.Text
                                                .ProcessEntry()
                                                .CtrlEnabled = Len(CtrlForm.el(1).Data) > 0
                                            End With
                                        End With
                                    End If
                                End If

                            Case "CHSTATUS"
                                CtrlTable.Table.Items(i).Selected = True
                                CtrlTable.SetEdit()
                        End Select
                        Exit Sub

                    End If
                Next

                ' No record found
                Select Case Argument("TXTYPE")
                    Case "INTERWHTX"
                        If CtrlTable.btnEnabled(w32SFDCData.CtrlTable.tButton.btnAdd) Then
                            With CtrlTable
                                .SetAdd()
                                With .el(0)
                                    .DataEntry.Text = Data(1, 0)
                                    .ProcessEntry()
                                End With
                            End With
                        End If

                    Case "PUTAWAY"
                        If CtrlTable.btnEnabled(w32SFDCData.CtrlTable.tButton.btnAdd) Then
                            With CtrlTable
                                .SetAdd()
                                With .el(0)
                                    .DataEntry.Text = Data(1, 0)
                                    .ProcessEntry()
                                End With
                            End With
                        End If

                    Case "CHSTATUS"
                        If CtrlTable.btnEnabled(w32SFDCData.CtrlTable.tButton.btnAdd) Then
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

        End Select

    End Sub

End Class
