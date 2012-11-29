Imports System
Imports System.IO
Imports System.Threading
Imports System.Text.RegularExpressions

Public Class InterfaceWHFX
    Inherits SFDCData.iForm

    Private Enum tInvoke
        iBarcode = 0
        iLoc = 1
    End Enum

    Private mInvoke As tInvoke = tInvoke.iBarcode

#Region "Initialisation"

    Public Sub New(Optional ByRef App As Form = Nothing)

        InitializeComponent()
        CallerApp = App
        NewArgument("TXTYPE", "FROM")
        NewArgument("MANUAL", "N")
        CtrlTable.DisableButtons(False, False, False, False, False)
        CtrlTable.EnableToolbar(True, True, False, True, False)

    End Sub

    Public Sub hEndForm() Handles MyBase.EndForm
        Argument("MANUAL") = "N"
    End Sub

    Public Overrides Sub FormLoaded()
        With CtrlForm
            With .el(0)
                .DataEntry.Text = "CL"
                .ProcessEntry()
            End With
        End With
    End Sub

#End Region

    Public Overrides Sub FormSettings()

        Select Case Argument("TXTYPE")

            Case "FROM"
                'FROM WARHSNAME
                With field
                    .Name = "WARHS"
                    .Title = "To W/H"
                    .ValidExp = ValidStr(tRegExValidation.tWarehouse)
                    .SQLList = "SELECT DISTINCT WARHSNAME FROM WAREHOUSES WHERE INACTIVE <> 'Y'"
                    .SQLValidation = "SELECT top 1 WARHSNAME FROM WAREHOUSES WHERE WARHSNAME = '%ME%'"
                    .Data = "CL"      '******** Barcoded field '*******
                    .AltEntry = ctrlText.tAltCtrlStyle.ctList 'ctNone 'ctKeyb 
                    .ctrlEnabled = True
                End With
                CtrlForm.AddField(field)

                ''LOCNAME
                'With field
                '    .Name = "LOCNAME"
                '    .Title = "To Loc"
                '    .ValidExp = ValidStr(tRegExValidation.tLocname)
                '    .SQLList = "SELECT DISTINCT LOCNAME FROM WAREHOUSES WHERE WARHSNAME = '%WARHS%' AND INACTIVE <> 'Y'"
                '    .SQLValidation = "SELECT LOCNAME FROM WAREHOUSES WHERE LOCNAME = '%ME%' AND WARHSNAME = '%WARHS%'"
                '    .Data = ""      '******** Barcoded field '*******
                '    .AltEntry = ctrlText.tAltCtrlStyle.ctList 'ctNone 'ctKeyb 
                '    .ctrlEnabled = False
                'End With
                'CtrlForm.AddField(field)


            Case "TO"
                'FROM WARHSNAME
                With field
                    .Name = "WARHS"
                    .Title = "From W/H"
                    .ValidExp = ValidStr(tRegExValidation.tWarehouse)
                    .SQLList = "SELECT DISTINCT WARHSNAME FROM WAREHOUSES WHERE INACTIVE <> 'Y'"
                    .SQLValidation = "SELECT top 1 WARHSNAME FROM WAREHOUSES WHERE WARHSNAME = '%ME%'"
                    .Data = "CL"      '******** Barcoded field '*******
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



        End Select

    End Sub

    Public Overrides Sub TableSettings()

        Select Case Argument("TXTYPE")
            '**************************************************************************
            Case "FROM"

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
                                    "AND WAREHOUSES.WARHSNAME = 'XFER'  " & _
                                    "AND WAREHOUSES.LOCNAME = '0'  " & _
                                    "AND WARHSBAL.BALANCE > 0 " & _
                                    "AND CUSTNAME = 'Goods' " & _
                                    "AND BARCODE = '%ME%'"
                    .DefaultFromCtrl = Nothing '
                    .ctrlEnabled = True
                    .Mandatory = True
                End With
                CtrlTable.AddCol(col)

                ' Availibility
                With col
                    .Name = "_Available"
                    .Title = "Available"
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
                                    "AND WAREHOUSES.WARHSNAME = 'XFER'  " & _
                                    "AND WAREHOUSES.LOCNAME = '0'  " & _
                                    "AND WARHSBAL.BALANCE/1000 >= %ME% " & _
                                    "AND CUSTNAME = 'Goods' " & _
                                    "AND PART.PARTNAME = '%_PARTNAME%'"
                    .DefaultFromCtrl = Nothing
                    .ctrlEnabled = True
                    .Mandatory = True
                End With
                CtrlTable.AddCol(col)

                'TOWARHSNAME
                With col
                    .Name = "_WARHS"
                    .Title = "To W/H"
                    .initWidth = 30
                    .TextAlign = HorizontalAlignment.Left
                    .ValidExp = ValidStr(tRegExValidation.tWarehouse)
                    .SQLList = "SELECT DISTINCT WARHSNAME FROM WAREHOUSES AND INACTIVE <> 'Y'"
                    .SQLValidation = "SELECT WARHSNAME FROM WAREHOUSES WHERE WARHSNAME = '%ME%' AND INACTIVE <> 'Y'"
                    .AltEntry = ctrlText.tAltCtrlStyle.ctList 'ctKeyb 
                    .DefaultFromCtrl = CtrlForm.el(0)
                    .ctrlEnabled = True
                    .Mandatory = False
                    .MandatoryOnPost = True
                End With
                CtrlTable.AddCol(col)

                ' TOLOCNAME
                With col
                    .Name = "_LOCNAME"
                    .Title = "To Loc"
                    .initWidth = 30
                    .TextAlign = HorizontalAlignment.Left
                    .AltEntry = ctrlText.tAltCtrlStyle.ctList 'ctKeyb 
                    .ValidExp = ValidStr(tRegExValidation.tLocname)
                    .SQLList = "SELECT DISTINCT LOCNAME FROM WAREHOUSES WHERE WARHSNAME = '%WARHS%' AND INACTIVE <> 'Y'"
                    .SQLValidation = "SELECT LOCNAME FROM WAREHOUSES WHERE LOCNAME = '%ME%' AND WARHSNAME = '%WARHS%'"
                    .DefaultFromCtrl = Nothing '******* Barcoded Field - default from Type1 TOLOCATION '*******
                    .ctrlEnabled = True
                    .Mandatory = False
                    .MandatoryOnPost = True
                End With
                CtrlTable.AddCol(col)

            Case "TO"

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
                                    "AND CUSTNAME = 'Goods' " & _
                                    "AND BARCODE = '%ME%'"
                    .DefaultFromCtrl = Nothing '
                    .ctrlEnabled = True
                    .Mandatory = True
                End With
                CtrlTable.AddCol(col)

                ' Availibility
                With col
                    .Name = "_Available"
                    .Title = "Available"
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
                                    "AND CUSTNAME = 'Goods' " & _
                                    "AND PART.PARTNAME = '%_PARTNAME%'"
                    .DefaultFromCtrl = Nothing
                    .ctrlEnabled = True
                    .Mandatory = True
                End With
                CtrlTable.AddCol(col)

                'TOWARHSNAME
                With col
                    .Name = "_WARHS"
                    .Title = "From W/H"
                    .initWidth = 30
                    .TextAlign = HorizontalAlignment.Left
                    .ValidExp = ValidStr(tRegExValidation.tWarehouse)
                    .SQLList = "SELECT DISTINCT WARHSNAME FROM WAREHOUSES AND INACTIVE <> 'Y'"
                    .SQLValidation = "SELECT WARHSNAME FROM WAREHOUSES WHERE WARHSNAME = '%ME%' AND INACTIVE <> 'Y'"
                    .AltEntry = ctrlText.tAltCtrlStyle.ctList 'ctKeyb 
                    .DefaultFromCtrl = CtrlForm.el(0)
                    .ctrlEnabled = True
                    .Mandatory = False
                    .MandatoryOnPost = True
                End With
                CtrlTable.AddCol(col)

                ' TOLOCNAME
                With col
                    .Name = "_LOCNAME"
                    .Title = "From Loc"
                    .initWidth = 30
                    .TextAlign = HorizontalAlignment.Left
                    .AltEntry = ctrlText.tAltCtrlStyle.ctList 'ctKeyb 
                    .ValidExp = ValidStr(tRegExValidation.tLocname)
                    .SQLList = "SELECT DISTINCT LOCNAME FROM WAREHOUSES WHERE WARHSNAME = '%WARHS%' AND INACTIVE <> 'Y'"
                    .SQLValidation = "SELECT LOCNAME FROM WAREHOUSES WHERE LOCNAME = '%ME%' AND WARHSNAME = '%WARHS%'"
                    .DefaultFromCtrl = CtrlForm.el(1)  '******* Barcoded Field - default from Type1 TOLOCATION '*******
                    .ctrlEnabled = True
                    .Mandatory = False
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
                        Case "FROM"

                        Case "TO"

                    End Select
                End With
            Next
        Catch e As Exception
            MsgBox(e.Message)
        End Try

    End Sub

    Public Overrides Sub ProcessEntry(ByVal ctrl As SFDCData.ctrlText, ByVal Valid As Boolean)

        Select Case Valid
            Case False
                Beep()

            Case True
                Try
                    Select Case Argument("TXTYPE")

                        Case "FROM"
                            If ctrl.Name = "_TQUANT" Then
                                'If Not CtrlTable.btnFinishState Then CtrlTable.SetTable()
                            End If
                            If ctrl.Name = "_LOCNAME" Then
                                If Not CtrlTable.btnFinishState Then CtrlTable.SetTable()
                            End If

                            'CtrlForm.el(1).CtrlEnabled = Len(CtrlForm.el(0).Data) > 0
                            CtrlTable.EnableToolbar( _
                                CBool(CtrlTable.Table.Visible And _
                                Len(CtrlForm.el(0).Data) > 0), _
                                CtrlTable.btnEnabled(SFDCData.CtrlTable.tButton.btnEdit), _
                                CtrlTable.btnEnabled(SFDCData.CtrlTable.tButton.btnCopy), _
                                CtrlTable.btnEnabled(SFDCData.CtrlTable.tButton.btnDelete), _
                                CtrlTable.btnEnabled(SFDCData.CtrlTable.tButton.btnPost) _
                            )
                            Try
                                If Len(CtrlForm.el(0).Data) > 0 And Len(CtrlTable.el(0).Data) > 0 Then
                                    Dim sql As String = "SELECT WARHSBAL.BALANCE/1000 " & _
                                        "FROM PART, WARHSBAL, WAREHOUSES, CUSTOMERS " & _
                                        "WHERE PART.PART = WARHSBAL.PART  " & _
                                        "AND WARHSBAL.WARHS = WAREHOUSES.WARHS  " & _
                                        "AND WARHSBAL.CUST = CUSTOMERS.CUST " & _
                                        "AND WAREHOUSES.WARHSNAME = 'XFER'  " & _
                                        "AND WAREHOUSES.LOCNAME = '0'  " & _
                                        "AND CUSTNAME = 'Goods' " & _
                                        "AND PART.PARTNAME = '" & CtrlTable.el(0).Data & "'"
                                    CtrlTable.el(1).NSInvoke(sql)
                                End If
                            Catch
                            End Try

                            If ctrl.Name = "WARHS" Then
                                CtrlForm.DoLostFocus()
                                CtrlTable.Focus()
                            End If

                        Case "TO"
                            If ctrl.Name = "_TQUANT" Then
                                If Not CtrlTable.btnFinishState Then CtrlTable.SetTable()
                            End If

                            CtrlForm.el(1).CtrlEnabled = Len(CtrlForm.el(0).Data) > 0
                            CtrlTable.EnableToolbar( _
                                CBool(CtrlTable.Table.Visible And _
                                Len(CtrlForm.el(0).Data) > 0 And _
                                Len(CtrlForm.el(1).Data) > 0), _
                                CtrlTable.btnEnabled(SFDCData.CtrlTable.tButton.btnEdit), _
                                CtrlTable.btnEnabled(SFDCData.CtrlTable.tButton.btnCopy), _
                                CtrlTable.btnEnabled(SFDCData.CtrlTable.tButton.btnDelete), _
                                CtrlTable.btnEnabled(SFDCData.CtrlTable.tButton.btnPost) _
                            )
                            Try
                                If Len(CtrlForm.el(0).Data) > 0 And Len(CtrlForm.el(1).Data) > 0 And Len(CtrlTable.el(0).Data) > 0 Then
                                    Dim sql As String = "SELECT WARHSBAL.BALANCE/1000 " & _
                                        "FROM PART, WARHSBAL, WAREHOUSES, CUSTOMERS " & _
                                        "WHERE PART.PART = WARHSBAL.PART  " & _
                                        "AND WARHSBAL.WARHS = WAREHOUSES.WARHS  " & _
                                        "AND WARHSBAL.CUST = CUSTOMERS.CUST " & _
                                        "AND WAREHOUSES.WARHSNAME = '" & CtrlForm.el(0).Data & "'  " & _
                                        "AND WAREHOUSES.LOCNAME = '" & CtrlForm.el(1).Data & "'  " & _
                                        "AND CUSTNAME = 'Goods' " & _
                                        "AND PART.PARTNAME = '" & CtrlTable.el(0).Data & "'"
                                    CtrlTable.el(1).NSInvoke(sql)
                                End If
                            Catch
                            End Try
                            If ctrl.Name = "LOCNAME" Then
                                CtrlForm.DoLostFocus()
                                CtrlTable.Focus()
                            End If

                    End Select

                Catch

                End Try
                CtrlForm.el(0).CtrlEnabled = Not (CtrlTable.Table.Items.Count > 0)
        End Select

    End Sub

    Public Overrides Function verifyForm() As Boolean
        Select Case Argument("TXTYPE")
            Case "FROM"
                Return True
            Case "TO"
                Return True
        End Select
    End Function

    Public Overrides Sub ProcessForm()

        Try

            Select Case Argument("TXTYPE")

                Case "FROM"

                    Try
                        With p
                            .DebugFlag = True
                            .Procedure = "ZSFDCP_LOAD_WHTX"
                            .Table = "ZSFDC_LOAD_WHTX"
                            .RecordType1 = "OWNERLOGIN,WARHSNAME,LOCNAME,TOWARHSNAME,TOLOCATION,MANUAL"
                            .RecordType2 = "PARTNAME,TQUANT,TOWARHSNAME2,TOLOCNAME,STATUS1,TOSTATUS"
                            .RecordTypes = "TEXT,TEXT,TEXT,TEXT,TEXT,TEXT,TEXT,,TEXT,TEXT,TEXT,TEXT"
                        End With

                    Catch e As Exception
                        MsgBox(e.Message)
                    End Try

                    ' Type 1 records
                    Dim t1() As String = { _
                                        UserName, _
                                        "XFER", _
                                        "0", _
                                        CtrlForm.ItemValue("WARHS"), _
                                        CtrlForm.ItemValue("LOCNAME"), _
                                        Argument("MANUAL") _
                                        }
                    p.AddRecord(1) = t1

                    For y As Integer = 0 To CtrlTable.RowCount
                        Dim t2() As String = { _
                                    CtrlTable.ItemValue("_PARTNAME", y), _
                                    CStr(CInt(1000 * CtrlTable.ItemValue("_TQUANT", y))), _
                                    CtrlTable.ItemValue("_WARHS", y), _
                                    CtrlTable.ItemValue("_LOCNAME", y), _
                                    "Goods", _
                                    "Goods" _
                                            }
                        p.AddRecord(2) = t2
                    Next

                Case "TO"

                    Try
                        With p
                            .DebugFlag = True
                            .Procedure = "ZSFDCP_LOAD_WHTX"
                            .Table = "ZSFDC_LOAD_WHTX"
                            .RecordType1 = "OWNERLOGIN,WARHSNAME,LOCNAME,TOWARHSNAME,TOLOCATION,MANUAL"
                            .RecordType2 = "PARTNAME,TQUANT,TOWARHSNAME2,TOLOCNAME,STATUS1,TOSTATUS"
                            .RecordTypes = "TEXT,TEXT,TEXT,TEXT,TEXT,TEXT,TEXT,,TEXT,TEXT,TEXT,TEXT"
                        End With

                    Catch e As Exception
                        MsgBox(e.Message)
                    End Try

                    Dim d As New List(Of String)
                    For y As Integer = 0 To CtrlTable.RowCount
                        Dim l As String = CtrlTable.ItemValue("_WARHS", y) & "//" & CtrlTable.ItemValue("_LOCNAME", y)
                        If Not d.Contains(l) Then d.Add(l)
                    Next

                    For Each loc As String In d

                        Dim w = Split(loc, "//")(0)
                        Dim L = Split(loc, "//")(1)

                        ' Type 1 records
                        Dim t1() As String = { _
                                            UserName, _
                                            w, _
                                            L, _
                                            "XFER", _
                                            "0", _
                                            Argument("MANUAL") _
                                            }
                        p.AddRecord(1) = t1


                        For y As Integer = 0 To CtrlTable.RowCount
                            If CtrlTable.ItemValue("_WARHS", y) = w And CtrlTable.ItemValue("_LOCNAME", y) = L Then
                                Dim t2() As String = { _
                                            CtrlTable.ItemValue("_PARTNAME", y), _
                                            CStr(CInt(1000 * CtrlTable.ItemValue("_TQUANT", y))), _
                                            "XFER", _
                                            "0", _
                                            "Goods", _
                                            "Goods" _
                                                    }
                                p.AddRecord(2) = t2
                            End If
                        Next
                    Next

            End Select

        Catch e As Exception
            MsgBox(e.Message)
        End Try

    End Sub

    Public Overrides Sub BeginAdd()
        Select Case Argument("TXTYPE")
            Case "FROM"
                CtrlTable.mCol(0).ctrlEnabled = True
                CtrlTable.mCol(1).ctrlEnabled = False
                CtrlTable.mCol(2).ctrlEnabled = True
                CtrlTable.mCol(3).ctrlEnabled = True
                CtrlTable.mCol(4).ctrlEnabled = True

            Case "TO"
                CtrlTable.mCol(0).ctrlEnabled = True
                CtrlTable.mCol(1).ctrlEnabled = False
                CtrlTable.mCol(2).ctrlEnabled = True
                CtrlTable.mCol(3).ctrlEnabled = True
                CtrlTable.mCol(4).ctrlEnabled = True

        End Select
    End Sub

    Public Overrides Sub BeginEdit()
        Argument("MANUAL") = "Y"
        Select Case Argument("TXTYPE")
            Case "FROM"
                CtrlTable.mCol(0).ctrlEnabled = True
                CtrlTable.mCol(1).ctrlEnabled = False
                CtrlTable.mCol(2).ctrlEnabled = True
                CtrlTable.mCol(3).ctrlEnabled = True
                CtrlTable.mCol(4).ctrlEnabled = True

            Case "TO"
                CtrlTable.mCol(0).ctrlEnabled = True
                CtrlTable.mCol(1).ctrlEnabled = False
                CtrlTable.mCol(2).ctrlEnabled = True
                CtrlTable.mCol(3).ctrlEnabled = True
                CtrlTable.mCol(4).ctrlEnabled = True

        End Select

    End Sub

    Public Overrides Sub AfterEdit(ByVal TableIndex As Integer)

        Select Case Argument("TXTYPE")

            Case "FROM"
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

            Case "TO"
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


        End Select

    End Sub

    Public Overrides Sub AfterAdd(ByVal TableIndex As Integer)

    End Sub

    Public Overrides Sub TableScan(ByVal Value As String)

        Select Case Argument("TXTYPE")
            Case "FROM"
                If Regex.IsMatch(Value, ValidStr(tRegExValidation.tBarcode)) Then
                    mInvoke = tInvoke.iBarcode
                    InvokeData("SELECT PARTNAME, BARCODE " & _
                                                "FROM PART, WARHSBAL, WAREHOUSES, CUSTOMERS " & _
                                                "WHERE PART.PART = WARHSBAL.PART  " & _
                                                "AND WARHSBAL.WARHS = WAREHOUSES.WARHS  " & _
                                                "AND WARHSBAL.CUST = CUSTOMERS.CUST " & _
                                                "AND WAREHOUSES.WARHSNAME = 'XFER'  " & _
                                                "AND WAREHOUSES.LOCNAME = '0'  " & _
                                                "AND WARHSBAL.BALANCE > 0 " & _
                                                "AND CUSTNAME = 'Goods' " & _
                                                "AND BARCODE = '" & Value & "'")
                ElseIf Regex.IsMatch(Value, ValidStr(tRegExValidation.tLocname)) Then
                    mInvoke = tInvoke.iLoc
                    InvokeData("SELECT LOCNAME FROM WAREHOUSES WHERE LOCNAME = '" & _
                        Value & "' AND WARHSNAME = '" & _
                        CtrlForm.el(0).Data & "'")
                End If

            Case "TO"
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
                                                "AND CUSTNAME = 'Goods' " & _
                                                "AND BARCODE = '" & Value & "'")
                ElseIf Regex.IsMatch(Value, ValidStr(tRegExValidation.tLocname)) Then
                    mInvoke = tInvoke.iLoc
                    InvokeData("SELECT LOCNAME FROM WAREHOUSES WHERE LOCNAME = '" & _
                        Value & "' AND WARHSNAME = '" & _
                        CtrlForm.el(0).Data & "'")
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

                    ' Found Record
                    Select Case Argument("TXTYPE")
                        Case "FROM"
                            If CtrlTable.Table.Items(i).SubItems(0).Text = Data(0, 0) Then

                                'With CtrlTable
                                '    .Table.Items(i).Selected = True
                                '    .SetEdit()
                                '    With .el(4)
                                '        .DataEntry.Text = ""
                                '        .ProcessEntry()
                                '    End With
                                'End With
                                Dim num As New frmNumber
                                With num
                                    .Text = "Box quantity."
                                    .ShowDialog()
                                    CtrlTable.Table.Items(i).SubItems(2).Text = CStr(CInt(CtrlTable.Table.Items(i).SubItems(2).Text) + .number)
                                    If .Manual Then Argument("MANUAL") = "Y"
                                    .Dispose()
                                End With
                                Exit Sub
                            End If

                            'Dim num As New frmNumber
                            'With num
                            '    .Text = "Box quantity."
                            '    .ShowDialog()
                            '    CtrlTable.Table.Items(i).SubItems(2).Text = CStr(CInt(CtrlTable.Table.Items(i).SubItems(2).Text) + .number)
                            '    If .Manual Then Argument("MANUAL") = "Y"
                            '    .Dispose()
                            'End With
                            'CtrlTable.SetTable()

                        Case "TO"
                            If CtrlTable.Table.Items(i).SubItems(0).Text = Data(0, 0) And CtrlTable.Table.Items(i).SubItems(4).Text = CtrlForm.el(1).Data Then
                                Dim num As New frmNumber
                                With num
                                    .Text = "Box quantity."
                                    .ShowDialog()
                                    CtrlTable.Table.Items(i).SubItems(2).Text = CStr(CInt(CtrlTable.Table.Items(i).SubItems(2).Text) + .number)
                                    If .Manual Then Argument("MANUAL") = "Y"
                                    .Dispose()
                                End With
                                Exit Sub
                            End If
                            'Dim num As New frmNumber
                            'With num
                            '    .Text = "Box quantity."
                            '    .ShowDialog()
                            '    CtrlTable.Table.Items(i).SubItems(2).Text = CStr(CInt(CtrlTable.Table.Items(i).SubItems(2).Text) + .number)
                            '    If .Manual Then Argument("MANUAL") = "Y"
                            '    .Dispose()
                            'End With
                            'CtrlTable.SetTable()

                    End Select                    

                Next

                ' No record found
                Select Case Argument("TXTYPE")
                    Case "FROM"
                        If CtrlTable.btnEnabled(SFDCData.CtrlTable.tButton.btnAdd) Then
                            With CtrlTable
                                .SetAdd()
                                With .el(0)
                                    .DataEntry.Text = Data(1, 0)
                                    .ProcessEntry()
                                End With
                            End With
                        End If

                    Case "TO"
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
                    End With
                End With

        End Select

    End Sub

End Class
