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
        NewArgument("USEDEFWHS", "FALSE") ' Use the default(users) warehouse ?
        NewArgument("GOODSINLOC", "") ' Default Goods in location
        CtrlTable.DisableButtons(True, False, False, True, False)

    End Sub

#End Region

    Public Overrides Sub FormSettings()

        'ORDNAME
        With field
            .Name = "PONAME"
            .Title = "PO Number"
            .ValidExp = ValidStr(tRegExValidation.tGRV)
            .SQLValidation = "SELECT DISTINCT ORDNAME FROM ZSFDC_OPENPO " & _
                            "WHERE  ORDNAME = '%ME%'"
            .Data = ""
            .AltEntry = ctrlText.tAltCtrlStyle.ctNone 'ctKeyb 
            .ctrlEnabled = True
            .MandatoryOnPost = True
        End With
        CtrlForm.AddField(field)

        If Not CBool(Argument("USEDEFWHS")) Then
            'FROM WARHSNAME
            With field
                .Name = "WARHS"
                .Title = "To W/H"
                .ValidExp = ValidStr(tRegExValidation.tWarehouse)
                .SQLList = "SELECT DISTINCT WARHSNAME FROM WAREHOUSES WHERE INACTIVE <> 'Y'"
                .SQLValidation = "SELECT WARHSNAME FROM WAREHOUSES WHERE WARHSNAME = '%ME%'"
                .Data = ""      '******** Barcoded field '*******
                .AltEntry = ctrlText.tAltCtrlStyle.ctList 'ctNone 'ctKeyb 
                .ctrlEnabled = True
            End With
            CtrlForm.AddField(field)
        End If

        If Len(Argument("GOODSINLOC")) = 0 Then
            'LOCNAME
            With field
                .Name = "LOCNAME"
                .Title = "To Loc"
                .ValidExp = ValidStr(tRegExValidation.tLocname)
                If Not CBool(Argument("USEDEFWHS")) Then
                    .SQLList = "SELECT DISTINCT LOCNAME FROM WAREHOUSES WHERE WARHSNAME = '%WARHS%' AND INACTIVE <> 'Y'"
                    .SQLValidation = "SELECT LOCNAME FROM WAREHOUSES WHERE LOCNAME = '%ME%' AND WARHSNAME = '%WARHS%'"
                Else
                    .SQLList = "SELECT DISTINCT LOCNAME FROM WAREHOUSES WHERE WARHSNAME = '%WARHS%' AND INACTIVE <> 'Y'"
                    .SQLValidation = "SELECT LOCNAME FROM WAREHOUSES WHERE LOCNAME = '%ME%' AND WARHSNAME = '" & GetWarehouseName() & "'"
                End If
                .Data = ""      '******** Barcoded field '*******
                .AltEntry = ctrlText.tAltCtrlStyle.ctList 'ctNone 'ctKeyb 
                .ctrlEnabled = CBool(Argument("USEDEFWHS")) 'ENABLED IF NOT USING DEFAULT
            End With
            CtrlForm.AddField(field)
        End If

        'BOOKNUM
        With field
            .Name = "BOOKNUM"
            .Title = "Vendor Doc."
            .ValidExp = "^.+$"
            .SQLValidation = "SELECT '%ME%'"
            .Data = ""
            .AltEntry = ctrlText.tAltCtrlStyle.ctNone 'ctKeyb 
            .ctrlEnabled = True
            .MandatoryOnPost = False
        End With
        CtrlForm.AddField(field)

    End Sub

    Public Overrides Sub TableSettings()

        ' BARCODE
        With col
            .Name = "_PARTNAME"
            .Title = "Part"
            .initWidth = 20
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

        With col
            .Name = "_PARTDES"
            .Title = "Name"
            .initWidth = 50
            .TextAlign = HorizontalAlignment.Left
            .AltEntry = ctrlText.tAltCtrlStyle.ctNone 'ctKeyb 
            .ValidExp = "^.+$"
            .SQLValidation = "SELECT '%ME%'"
            .DefaultFromCtrl = Nothing '
            .ctrlEnabled = False
            .Mandatory = False
            .ctrlEnabled = False
        End With
        CtrlTable.AddCol(col)

        With col
            .Name = "_VPART"
            .Title = "Vendor Part"
            .initWidth = 50
            .TextAlign = HorizontalAlignment.Left
            .AltEntry = ctrlText.tAltCtrlStyle.ctNone 'ctKeyb 
            .ValidExp = "^.+$"
            .SQLValidation = "SELECT '%ME%'"
            .DefaultFromCtrl = Nothing '
            .ctrlEnabled = False
            .Mandatory = False
            .ctrlEnabled = False
        End With
        CtrlTable.AddCol(col)

        With col
            .Name = "_DUEDATE"
            .Title = "Due Date"
            .initWidth = 50
            .TextAlign = HorizontalAlignment.Left
            .AltEntry = ctrlText.tAltCtrlStyle.ctNone 'ctKeyb 
            .ValidExp = "^.+$"
            .SQLValidation = "SELECT '%ME%'"
            .DefaultFromCtrl = Nothing '
            .ctrlEnabled = False
            .Mandatory = False
            .ctrlEnabled = False
        End With
        CtrlTable.AddCol(col)

        ' TQUANT
        With col
            .Name = "_QUANT"
            .Title = "Ord"
            .initWidth = 20
            .TextAlign = HorizontalAlignment.Left
            .AltEntry = ctrlText.tAltCtrlStyle.ctNone 'ctKeyb 
            .ValidExp = ValidStr(tRegExValidation.tNumeric)
            .SQLValidation = "SELECT %ME%"
            .DefaultFromCtrl = Nothing
            .ctrlEnabled = False
            .Mandatory = True
            .MandatoryOnPost = True
        End With
        CtrlTable.AddCol(col)

        ' TQUANT
        With col
            .Name = "_RECEIVED"
            .Title = "Rcvd"
            .initWidth = 20
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

        ' STATUS
        With col
            .Name = "_STATUS"
            .Title = "Status"
            .initWidth = 30
            .TextAlign = HorizontalAlignment.Left
            .AltEntry = ctrlText.tAltCtrlStyle.ctList 'ctKeyb 
            .ValidExp = ValidStr(tRegExValidation.tStatus)
            .SQLValidation = "SELECT CUSTNAME FROM CUSTOMERS WHERE STATUSFLAG = 'Y' AND CUSTNAME = '%ME%'"
            .SQLList = "SELECT CUSTNAME FROM CUSTOMERS WHERE STATUSFLAG = 'Y'"
            .DefaultFromCtrl = Nothing
            .ctrlEnabled = True
            .Mandatory = True
            .MandatoryOnPost = True
        End With
        CtrlTable.AddCol(col)

        If Not CBool(Argument("USEDEFWHS")) Then
            'FROM WARHSNAME
            With col
                .Name = "_WARHS"
                .Title = "To W/H"
                .initWidth = 30
                .TextAlign = HorizontalAlignment.Left
                .ValidExp = ValidStr(tRegExValidation.tWarehouse)
                .SQLList = "SELECT DISTINCT WARHSNAME FROM WAREHOUSES WHERE INACTIVE <> 'Y'"
                .SQLValidation = "SELECT WARHSNAME FROM WAREHOUSES WHERE WARHSNAME = '%ME%'"
                .AltEntry = ctrlText.tAltCtrlStyle.ctList 'ctNone 'ctKeyb 
                .ctrlEnabled = True
                .Mandatory = False
                .MandatoryOnPost = True
            End With
            CtrlTable.AddCol(col)
        End If

        If Len(Argument("GOODSINLOC")) = 0 Then
            'LOCNAME
            With col
                .Name = "_LOCNAME"
                .Title = "To Loc"
                .initWidth = 30
                .TextAlign = HorizontalAlignment.Left
                .ValidExp = ValidStr(tRegExValidation.tLocname)
                If Not CBool(Argument("USEDEFWHS")) Then
                    .SQLList = "SELECT DISTINCT LOCNAME FROM WAREHOUSES WHERE WARHSNAME = '%_WARHS%' AND INACTIVE <> 'Y'"
                    .SQLValidation = "SELECT LOCNAME FROM WAREHOUSES WHERE LOCNAME = '%ME%' AND WARHSNAME = '%_WARHS%'"
                Else
                    .SQLList = "SELECT DISTINCT LOCNAME FROM WAREHOUSES WHERE WARHSNAME = '%_WARHS%' AND INACTIVE <> 'Y'"
                    .SQLValidation = "SELECT LOCNAME FROM WAREHOUSES WHERE LOCNAME = '%ME%' AND WARHSNAME = '" & GetWarehouseName() & "'"
                End If
                .AltEntry = ctrlText.tAltCtrlStyle.ctList 'ctNone 'ctKeyb 
                .ctrlEnabled = CBool(Argument("USEDEFWHS")) 'ENABLED IF NOT USING DEFAULT
                .Mandatory = False
                .MandatoryOnPost = True
            End With
            CtrlTable.AddCol(col)
        End If

        ' ordi
        With col
            .Name = "_ORDI"
            .Title = "Order Item"
            .initWidth = 0
            .TextAlign = HorizontalAlignment.Left
            .AltEntry = ctrlText.tAltCtrlStyle.ctList 'ctKeyb 
            .ValidExp = ValidStr(tRegExValidation.tStatus)
            .DefaultFromCtrl = Nothing
            .ctrlEnabled = False
            .Mandatory = False
            .MandatoryOnPost = False
        End With
        CtrlTable.AddCol(col)

        ' Set the query to load recordtype 2s
        CtrlTable.RecordsSQL = "SELECT PARTNAME, PARTDES, SUPPARTNAME, DUEDATE, " & _
                                "ORDERED, RECEIVED, STATUS, WARHS, LOCNAME, ORDI " & _
                                "FROM ZSFDC_GRVITEMS " & _
                                "WHERE ORDNAME = '%PONAME%' "


    End Sub

    Public Overrides Sub TableRXData(ByVal Data(,) As String)

        Try
            With CtrlTable.Table
                .Items.Clear()
                For y As Integer = 0 To UBound(Data, 2)
                    Dim lvi As New ListViewItem
                    .Items.Add(lvi)
                    .Items(.Items.Count - 1).Text = Data(0, y)
                    For i As Integer = 1 To CtrlTable.Table.Columns.Count - 1
                        If i <= UBound(Data, 1) Then
                            .Items(.Items.Count - 1).SubItems.Add(Data(i, y))
                        Else
                            .Items(.Items.Count - 1).SubItems.Add("")
                        End If
                    Next
                Next
            End With
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
                            Case "PONAME"
                                CtrlTable.BeginLoadRS()
                            Case "LOCNAME"
                                For Y As Integer = 0 To CtrlTable.Table.Items.Count - 1
                                    If Len(Argument("GOODSINLOC")) = 0 Then
                                        If Len(CtrlTable.Table.Items(Y).SubItems(ColNo("_LOCNAME")).Text) = 0 Then
                                            CtrlTable.Table.Items(Y).SubItems(ColNo("_LOCNAME")).Text = Val("LOCNAME")
                                            If Not CBool(Argument("USEDEFWHS")) Then
                                                CtrlTable.Table.Items(Y).SubItems(ColNo("_WARHS")).Text = Val("WARHS")
                                            End If
                                        End If
                                    End If
                                Next
                        End Select
                    End If

                    ' *******************************************************************
                    ' *** Set which controls are enabled

                    CtrlForm.el(ColNo("PONAME")).CtrlEnabled = Not (Len(CtrlForm.el(ColNo("PONAME")).Data) > 0)
                    If Len(Argument("GOODSINLOC")) = 0 Then
                        If Not CBool(Argument("USEDEFWHS")) Then
                            CtrlForm.el(ColNo("LOCNAME")).CtrlEnabled = Len(Val("WARHS")) > 0
                            If Not CtrlTable.Table.Visible Then CtrlTable.el(ColNo("_LOCNAME")).CtrlEnabled = Len(Val("_WARHS")) > 0
                        Else
                            CtrlForm.el(ColNo("LOCNAME")).CtrlEnabled = True
                            If Not CtrlTable.Table.Visible Then CtrlTable.el(ColNo("_LOCNAME")).CtrlEnabled = True
                        End If
                    End If


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
                .DebugFlag = True
                .Procedure = "ZSFDCP_LOAD_GRV"
                .Table = "ZSFDC_LOAD_GRV"
                .RecordType1 = "ORDNAME,BOOKNUM,USERLOGIN,TOWARHSNAME,TOLOCNAME"
                .RecordType2 = "PARTNAME,TOWARHSNAME,TOLOCNAME,TQUANT,STATUS,ORDNAME1,ORDI"
                .RecordTypes = "TEXT,TEXT,TEXT,TEXT,TEXT,TEXT,TEXT,TEXT,,TEXT,TEXT,TEXT"
            End With

            ' Type 1 records
            Dim t1() As String = { _
                                CtrlForm.ItemValue("PONAME"), _
                                CtrlForm.ItemValue("BOOKNUM"), _
                                UserName, _
                                GetWarehouseName(), _
                                GetLocName() _
                                }
            p.AddRecord(1) = t1

            For y As Integer = 0 To CtrlTable.RowCount
                Dim t2() As String = { _
                            CtrlTable.ItemValue("_PARTNAME", y), _
                            GetWarehouseName(y), _
                            GetLocName(y), _
                            CStr(CInt(CtrlTable.ItemValue("_RECEIVED", y)) * 1000), _
                            CtrlTable.ItemValue("_STATUS", y), _
                            CtrlForm.ItemValue("PONAME"), _
                            CtrlTable.ItemValue("_ORDI", y) _
                                    }
                p.AddRecord(2) = t2
            Next

        Catch e As Exception
            MsgBox(e.Message)
        End Try

    End Sub

    Public Overrides Sub BeginAdd()
        CtrlTable.mCol(ColNo("_PARTNAME")).ctrlEnabled = True
        CtrlTable.mCol(ColNo("_PARTDES")).ctrlEnabled = False
        CtrlTable.mCol(ColNo("_QUANT")).ctrlEnabled = False
        CtrlTable.mCol(ColNo("_RECEIVED")).ctrlEnabled = True
        CtrlTable.mCol(ColNo("_STATUS")).ctrlEnabled = True
    End Sub

    Public Overrides Sub AfterAdd(ByVal TableIndex As Integer)

    End Sub

    Public Overrides Sub BeginEdit()
        CtrlTable.mCol(ColNo("_PARTNAME")).ctrlEnabled = False
        CtrlTable.mCol(ColNo("_PARTDES")).ctrlEnabled = False
        CtrlTable.mCol(ColNo("_QUANT")).ctrlEnabled = False
        CtrlTable.mCol(ColNo("_RECEIVED")).ctrlEnabled = True
        CtrlTable.mCol(ColNo("_STATUS")).ctrlEnabled = True

        If Len(Argument("GOODSINLOC")) = 0 Then
            If Not CBool(Argument("USEDEFWHS")) Then
                CtrlTable.mCol(ColNo("_LOCNAME")).ctrlEnabled = Len(CtrlTable.Table.Items(CtrlTable.Table.SelectedIndices(0)).SubItems(ColNo("_WARHS")).Text) > 0
            Else
                CtrlTable.el(ColNo("_LOCNAME")).CtrlEnabled = True
            End If
        End If

    End Sub

    Public Overrides Sub AfterEdit(ByVal TableIndex As Integer)

    End Sub

    Public Overrides Sub TableScan(ByVal Value As String)
        InvokeData("SELECT PARTNAME FROM PART WHERE BARCODE = '" & Value & "'")
    End Sub

    Public Overrides Sub EndInvokeData(ByVal Data(,) As String)

        For i As Integer = 0 To CtrlTable.Table.Items.Count
            If CtrlTable.Table.Items(i).SubItems(ColNo("_PARTNAME")).Text = Data(0, 0) Then

                Dim exp As Integer = CInt(CtrlTable.Table.Items(i).SubItems(ColNo("_QUANT")).Text)
                Dim rcvd As Integer = CInt(CtrlTable.Table.Items(i).SubItems(ColNo("_RECEIVED")).Text)
                Dim add As Integer

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

                If add > 0 Then

                    If rcvd + add > exp Then
                        If MsgBox("Receiving more than ordered. Continue?", MsgBoxStyle.OkCancel, "Part " & Data(0, 0)) = MsgBoxResult.Ok Then
                            CtrlTable.Table.Items(i).SubItems(ColNo("_RECEIVED")).Text = CStr(rcvd + add)
                        End If
                    Else
                        CtrlTable.Table.Items(i).SubItems(ColNo("_RECEIVED")).Text = CStr(CInt(CtrlTable.Table.Items(i).SubItems(ColNo("_RECEIVED")).Text) + add)
                    End If

                End If
                Exit For

            End If
        Next

    End Sub

    Private Function GetWarehouseName(Optional ByVal y As Integer = -1) As String
        If CBool(Argument("USEDEFWHS")) Then
            Return Warehouse
        Else
            If Not y = -1 Then
                Return CtrlTable.Table.Items(y).SubItems(ColNo("_WARHS")).Text
            Else
                Return Val("WARHS")
            End If
        End If
    End Function

    Private Function GetLocName(Optional ByVal y As Integer = -1) As String
        If Not Len(Argument("GOODSINLOC")) = 0 Then
            Return Argument("GOODSINLOC")
        Else
            If Not y = -1 Then
                Return CtrlTable.Table.Items(y).SubItems(ColNo("_LOCNAME")).Text
            Else
                Return Val("LOCNAME")
            End If
        End If
    End Function

End Class
