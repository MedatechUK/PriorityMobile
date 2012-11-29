Imports System
Imports System.IO
Imports System.Threading

Public Class InterfaceGRV
    Inherits w32SFDCData.iForm

#Region "Initialisation"

    Public Sub New(Optional ByRef App As Form = Nothing)

        InitializeComponent()
        CallerApp = App
        NewArgument("SCANACTION", "OPENFORM")
        CtrlTable.DisableButtons(False, False, True, False, False)

    End Sub

#End Region

    Public Overrides Sub FormSettings()

        ' VENDOR
        With field
            .Name = "VENDOR"
            .Title = "Vendor"
            .ValidExp = ValidStr(tRegExValidation.tString)
            .SQLList = "SELECT DISTINCT SUPNAME FROM SUPPLIERS, PORDERS, PORDSTATS " & _
                            "WHERE SUPPLIERS.SUP = PORDERS.SUP " & _
                            "AND PORDERS.PORDSTAT  = PORDSTATS.PORDSTAT " & _
                            "AND PORDERS.CLOSED <> 'Y' " & _
                            "AND PORDSTATS.APPROVED = 'Y' " & _
                            "AND ORDNAME <> '' " & _
                            "ORDER BY SUPNAME"
            .SQLValidation = "SELECT SUPNAME FROM SUPPLIERS, PORDERS, PORDSTATS " & _
                            "WHERE SUPPLIERS.SUP = PORDERS.SUP " & _
                            "AND PORDERS.PORDSTAT  = PORDSTATS.PORDSTAT " & _
                            "AND PORDERS.CLOSED <> 'Y' " & _
                            "AND PORDSTATS.APPROVED = 'Y' " & _
                            "AND ORDNAME <> '' " & _
                            "AND SUPNAME = '%ME%'"
            .Data = ""
            .AltEntry = ctrlText.tAltCtrlStyle.ctList 'ctKeyb 
            .ctrlEnabled = True
            .MandatoryOnPost = True

        End With
        CtrlForm.AddField(field)

        'To Locname
        With field
            .Name = "TOLOC"
            .Title = "Bin"
            .ValidExp = ValidStr(tRegExValidation.tLocname)
            .SQLList = "SELECT DISTINCT LOCNAME FROM WAREHOUSES WHERE WARHSNAME = '" & Warehouse & "' AND WAREHOUSES.INACTIVE <> 'Y'"
            .SQLValidation = "SELECT LOCNAME FROM WAREHOUSES WHERE LOCNAME = '%ME%' AND WARHSNAME = (SELECT WARHSNAME FROM v_USERS where USERLOGIN = '" & UserName & "') AND WAREHOUSES.INACTIVE <> 'Y'"
            .Data = ""
            .AltEntry = ctrlText.tAltCtrlStyle.ctList 'ctKeyb 
            .ctrlEnabled = True
            .MandatoryOnPost = True
        End With
        CtrlForm.AddField(field)

        'ORDNAME
        With field
            .Name = "PONAME"
            .Title = "PO Number"
            .ValidExp = ValidStr(tRegExValidation.tString)
            .SQLValidation = "SELECT dbo.OldPOName(ORD), ORDNAME " & _
                                    "FROM PORDERS, SUPPLIERS, PORDSTATS " & _
                                    "WHERE SUPPLIERS.SUP = PORDERS.SUP " & _
                                    "AND PORDERS.PORDSTAT  = PORDSTATS.PORDSTAT " & _
                                    "AND ORDNAME <> '' " & _
                                    "AND SUPPLIERS.SUPNAME = '%VENDOR%' " & _
                                    "AND PORDERS.CLOSED <> 'Y' " & _
                                    "AND PORDSTATS.APPROVED = 'Y' " & _
                                    "AND (ORDNAME = '%ME%' OR SUPORDNUM = '%ME%')"
            .Data = ""
            .AltEntry = ctrlText.tAltCtrlStyle.ctNone 'ctKeyb 
            .ctrlEnabled = False
            .MandatoryOnPost = False
        End With
        CtrlForm.AddField(field)

        ''BOOKNUM
        'With field
        '    .Name = "BOOKNUM"
        '    .Title = "Vendor Doc."
        '    .ValidExp = "^.+$"
        '    .SQLValidation = "SELECT '%ME%'"
        '    .Data = ""
        '    .AltEntry = ctrlText.tAltCtrlStyle.ctNone 'ctKeyb 
        '    .ctrlEnabled = True
        '    .MandatoryOnPost = False
        'End With
        'CtrlForm.AddField(field)

    End Sub

    Public Overrides Sub TableSettings()

        ' BARCODE
        With col
            .Name = "_PARTNAME"
            .Title = "Part"
            .initWidth = 40
            .TextAlign = HorizontalAlignment.Left
            .AltEntry = ctrlText.tAltCtrlStyle.ctNone 'ctKeyb 
            .ValidExp = ValidStr(tRegExValidation.tBarcode)
            .SQLValidation = "SELECT BARCODE, PARTNAME " & _
                            "FROM PART " & _
                            "WHERE BARCODE = '%ME%'"
            .DefaultFromCtrl = Nothing '
            .ctrlEnabled = True
            .Mandatory = True
            .ctrlEnabled = True
        End With
        CtrlTable.AddCol(col)

        '' PARTDES
        'With col
        '    .Name = "_PARTDES"
        '    .Title = "Name"
        '    .initWidth = 50
        '    .TextAlign = HorizontalAlignment.Left
        '    .AltEntry = ctrlText.tAltCtrlStyle.ctNone 'ctKeyb 
        '    .ValidExp = "^.+$"
        '    .SQLValidation = "SELECT '%ME%'"
        '    .DefaultFromCtrl = Nothing '
        '    .ctrlEnabled = False
        '    .Mandatory = False
        '    .ctrlEnabled = True
        'End With
        'CtrlTable.AddCol(col)

        ' ORDNAME
        With col
            .Name = "_ORDNAME"
            .Title = "P/Order"
            .initWidth = 40
            .TextAlign = HorizontalAlignment.Left
            .AltEntry = ctrlText.tAltCtrlStyle.ctNone 'ctKeyb 
            .ValidExp = ValidStr(tRegExValidation.tBarcode)
            .SQLValidation = "SELECT BARCODE, PARTNAME " & _
                            "FROM PART " & _
                            "WHERE BARCODE = '%ME%'"
            .DefaultFromCtrl = Nothing '
            .ctrlEnabled = False
            .Mandatory = True
            .ctrlEnabled = True
        End With
        CtrlTable.AddCol(col)

        '' TOLOCNAME
        'With col
        '    .Name = "_TOLOC"
        '    .Title = "To Bin"
        '    .initWidth = 30
        '    .TextAlign = HorizontalAlignment.Left
        '    .AltEntry = ctrlText.tAltCtrlStyle.ctList 'ctKeyb 
        '    .ValidExp = ValidStr(tRegExValidation.tLocname)
        '    .SQLList = "SELECT DISTINCT LOCNAME FROM WAREHOUSES WHERE WARHSNAME = (SELECT WARHSNAME FROM v_USERS where USERLOGIN = 'tabula') AND WAREHOUSES.INACTIVE <> 'Y'"
        '    .SQLValidation = "SELECT LOCNAME FROM WAREHOUSES WHERE LOCNAME = '%ME%' AND WARHSNAME = (SELECT WARHSNAME FROM v_USERS where USERLOGIN = '" & UserName & "') AND WAREHOUSES.INACTIVE <> 'Y'"
        '    .DefaultFromCtrl = CtrlForm.el(2)
        '    .ctrlEnabled = True
        '    .Mandatory = False
        '    .MandatoryOnPost = True
        'End With
        'CtrlTable.AddCol(col)

        '' TQUANT
        'With col
        '    .Name = "_QUANT"
        '    .Title = "Ord"
        '    .initWidth = 20
        '    .TextAlign = HorizontalAlignment.Left
        '    .AltEntry = ctrlText.tAltCtrlStyle.ctNone 'ctKeyb 
        '    .ValidExp = ValidStr(tRegExValidation.tNumeric)
        '    .SQLValidation = "SELECT %ME%"
        '    .DefaultFromCtrl = Nothing
        '    .ctrlEnabled = False
        '    .Mandatory = True
        '    .MandatoryOnPost = True
        'End With
        'CtrlTable.AddCol(col)

        ' TQUANT
        With col
            .Name = "_RECEIVED"
            .Title = "Rcvd"
            .initWidth = 18
            .TextAlign = HorizontalAlignment.Left
            .AltEntry = ctrlText.tAltCtrlStyle.ctNone 'ctKeyb 
            .ValidExp = ValidStr(tRegExValidation.tNumeric)
            .SQLValidation = "SELECT %ME%"
            .DefaultFromCtrl = Nothing
            .ctrlEnabled = True
            .Mandatory = True
            .MandatoryOnPost = True
        End With
        CtrlTable.AddCol(col)

        '' STATUS
        'With col
        '    .Name = "_STATUS"
        '    .Title = "Status"
        '    .initWidth = 30
        '    .TextAlign = HorizontalAlignment.Left
        '    .AltEntry = ctrlText.tAltCtrlStyle.ctList 'ctKeyb 
        '    .ValidExp = ValidStr(tRegExValidation.tStatus)
        '    .SQLValidation = "SELECT CUSTNAME FROM CUSTOMERS WHERE STATUSFLAG = 'Y' AND CUSTNAME = '%ME%'"
        '    .SQLList = "SELECT CUSTNAME FROM CUSTOMERS WHERE STATUSFLAG = 'Y'"
        '    .DefaultFromCtrl = Nothing
        '    .ctrlEnabled = True
        '    .Mandatory = True
        '    .MandatoryOnPost = True
        'End With
        'CtrlTable.AddCol(col)

        ' Set the query to load recordtype 2s
        CtrlTable.RecordsSQL = "SELECT PARTNAME, PORDERS.ORDNAME, 0 " & _
                                        "FROM PART, PORDERITEMS, PORDERS, SUPPLIERS " & _
                                        "WHERE PORDERITEMS.PART = PART.PART " & _
                                        "AND PORDERS.ORD = PORDERITEMS.ORD   " & _
                                        "AND SUPPLIERS.SUP = PORDERS.SUP " & _
                                        "and SUPPLIERS.SUPNAME = '%VENDOR%' " & _
                                        "and PORDERS.CLOSED <> 'Y' " & _
                                        "AND PORDERS.ORDNAME <> '' "

    End Sub

    Public Overrides Sub TableRXData(ByVal Data(,) As String)

        Try
            For y As Integer = 0 To UBound(Data, 2)
                Dim lvi As New ListViewItem
                With CtrlTable.Table
                    .Items.Add(lvi)
                    .Items(.Items.Count - 1).Text = Data(0, y)
                    .Items(.Items.Count - 1).SubItems.Add(Data(1, y))
                    .Items(.Items.Count - 1).SubItems.Add(Data(2, y))
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
                    If ctrl.Data <> "" Then
                        Select Case ctrl.Name
                            Case "VENDOR"
                                'CtrlTable.BeginLoadRS()
                            Case "TOLOC"
                                For Y As Integer = 0 To CtrlTable.Table.Items.Count - 1
                                    If Len(CtrlTable.Table.Items(Y).SubItems(2).Text) = 0 Then
                                        CtrlTable.Table.Items(Y).SubItems(2).Text = CtrlForm.el(1).Data
                                    End If
                                Next
                        End Select
                    End If

                    ' *******************************************************************
                    ' *** Set which controls are enabled

                    CtrlForm.el(0).CtrlEnabled = Not (Len(CtrlForm.el(0).Data) > 0)
                    'CtrlTable.el(1).CtrlEnabled = Len(CtrlTable.el(0).Data) > 0

                    ' *******************************************************************
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
                .DebugFlag = False
                .Procedure = "ZSFDCP_LOAD_GRV"
                .Table = "ZSFDC_LOAD_GRV"
                .RecordType1 = "SUPNAME,USERLOGIN,TOWARHSNAME,TOLOCNAME"
                .RecordType2 = "ORDNAME1,PARTNAME,TQUANT"
                .RecordTypes = "TEXT,TEXT,TEXT,TEXT,TEXT,TEXT,"
            End With

            ' Type 1 records
            Dim t1() As String = { _
                                CtrlForm.ItemValue("VENDOR"), _
                                UserName, _
                                Warehouse, _
                                CtrlForm.ItemValue("TOLOC") _
                                }
            p.AddRecord(1) = t1

            For y As Integer = 0 To CtrlTable.RowCount
                Dim t2() As String = { _
                            CtrlTable.ItemValue("_ORDNAME", y), _
                            CtrlTable.ItemValue("_PARTNAME", y), _
                            CtrlTable.ItemValue("_RECEIVED", y) _
                                    }
                p.AddRecord(2) = t2
            Next

        Catch e As Exception
            MsgBox(e.Message)
        End Try

    End Sub

    Public Overrides Sub BeginAdd()
        CtrlTable.mCol(0).ctrlEnabled = True
        CtrlTable.mCol(1).ctrlEnabled = True
        CtrlTable.mCol(2).ctrlEnabled = True
        'CtrlTable.mCol(3).ctrlEnabled = False
        'CtrlTable.mCol(4).ctrlEnabled = True
    End Sub

    Public Overrides Sub BeginEdit()
        CtrlTable.mCol(0).ctrlEnabled = False
        CtrlTable.mCol(1).ctrlEnabled = False
        CtrlTable.mCol(2).ctrlEnabled = True
        'CtrlTable.mCol(3).ctrlEnabled = True
        'CtrlTable.mCol(4).ctrlEnabled = True
    End Sub

    Public Overrides Sub AfterEdit(ByVal TableIndex As Integer)

    End Sub

    Public Overrides Sub TableScan(ByVal Value As String)

        If System.Text.RegularExpressions.Regex.IsMatch(Value, ValidStr(tRegExValidation.tBarcode)) Then
            If Len(CtrlForm.el(2).Data) = 0 Then
                CtrlForm.el(2).Data = ""
                Beep()
            Else
                InvokeData("SELECT PARTNAME FROM PART WHERE BARCODE = '" & Value & "'")
            End If
        Else
            CtrlForm.el(2).DataEntry.Text = Value
            CtrlForm.el(2).ProcessEntry()
        End If

    End Sub

    Public Overrides Sub EndInvokeData(ByVal Data(,) As String)

        Dim add As Integer = 0
        Dim f As Boolean = False

        If CtrlTable.Table.Items.Count > 0 Then
            For i As Integer = 0 To CtrlTable.Table.Items.Count - 1
                If CtrlTable.Table.Items(i).SubItems(1).Text = CtrlForm.el(2).Data And _
                    CtrlTable.Table.Items(i).SubItems(0).Text = Data(0, 0) Then

                    f = True
                    CtrlTable.Table.Items(i).Selected = True

                    Select Case Argument("SCANACTION")

                        Case "OPENFORM"

                            Dim num As New frmNumber
                            With num
                                .Text = "Box quantity."
                                .ShowDialog()
                                add = .number
                                .Dispose()
                            End With

                        Case "INCREMENT"
                            add = 1

                    End Select

                    CtrlTable.Table.Items(i).SubItems(2).Text = _
                        CStr(CInt(CtrlTable.Table.Items(i).SubItems(2).Text) + add)

                    Exit For

                End If
            Next
        End If

        If Not f Then
            Select Case Argument("SCANACTION")

                Case "OPENFORM"

                    Dim num As New frmNumber
                    With num
                        .Text = "Box quantity."
                        .ShowDialog()
                        add = .number
                        .Dispose()
                    End With

                Case "INCREMENT"
                    add = 1

            End Select

            Dim lvi As New ListViewItem
            With CtrlTable.Table
                .Items.Add(lvi)
                .Items(.Items.Count - 1).Text = Data(0, 0)
                .Items(.Items.Count - 1).SubItems.Add(CtrlForm.el(2).Data)
                .Items(.Items.Count - 1).SubItems.Add(add)
            End With

        End If

        CtrlForm.el(2).Data = ""

    End Sub

End Class
