Imports System
Imports System.IO
Imports System.Threading

Public Class PORDERS
    Public Sub New(ByVal VENDOR As String)
        _VENDOR = VENDOR
    End Sub
    Dim _VENDOR As String = ""
    Public Property VENDOR() As String
        Get
            Return _VENDOR
        End Get
        Set(ByVal value As String)
            _VENDOR = value
        End Set
    End Property
    Private _ORDERITEMS As New Dictionary(Of String, String)
    Public Property ORDERITEMS() As Dictionary(Of String, String)
        Get
            Return _ORDERITEMS
        End Get
        Set(ByVal value As Dictionary(Of String, String))
            _ORDERITEMS = value
        End Set
    End Property
End Class

Public Class InterfaceGRV
    Inherits SFDCData.iForm

    Private Enum tInvoke
        iBarcode = 0
        iVendor = 1
    End Enum
    Private mInvoke As tInvoke = tInvoke.iBarcode

    Dim PORD As New Dictionary(Of String, PORDERS)

#Region "Initialisation"

    Public Sub New(Optional ByRef App As Form = Nothing)

        InitializeComponent()
        CallerApp = App
        NewArgument("SCANACTION", "OPENFORM")
        NewArgument("MANUAL", "N")
        CtrlTable.DisableButtons(False, False, True, False, False)

    End Sub

    Public Sub hEndForm() Handles MyBase.EndForm
        Argument("MANUAL") = "N"
    End Sub

#End Region

    Public Overrides Sub FormSettings()

        '' VENDOR
        'With field
        '    .Name = "VENDOR"
        '    .Title = "Vendor"
        '    .ValidExp = ValidStr(tRegExValidation.tString)
        '    .SQLList = "SELECT DISTINCT SUPNAME FROM SUPPLIERS, PORDERS, PORDSTATS " & _
        '                    "WHERE SUPPLIERS.SUP = PORDERS.SUP " & _
        '                    "AND PORDERS.PORDSTAT  = PORDSTATS.PORDSTAT " & _
        '                    "AND PORDERS.CLOSED <> 'Y' " & _
        '                    "AND PORDSTATS.APPROVED = 'Y' " & _
        '                    "AND ORDNAME <> '' " & _
        '                    "ORDER BY SUPNAME"
        '    .SQLValidation = "SELECT SUPNAME FROM SUPPLIERS, PORDERS, PORDSTATS " & _
        '                    "WHERE SUPPLIERS.SUP = PORDERS.SUP " & _
        '                    "AND PORDERS.PORDSTAT  = PORDSTATS.PORDSTAT " & _
        '                    "AND PORDERS.CLOSED <> 'Y' " & _
        '                    "AND PORDSTATS.APPROVED = 'Y' " & _
        '                    "AND ORDNAME <> '' " & _
        '                    "AND SUPNAME = '%ME%'"
        '    .Data = ""
        '    .AltEntry = ctrlText.tAltCtrlStyle.ctList 'ctKeyb 
        '    .ctrlEnabled = True
        '    .MandatoryOnPost = True

        'End With
        'CtrlForm.AddField(field)

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
            .SQLValidation = "SELECT ORDNAME, ORDNAME " & _
                                    "FROM PORDERS, PORDSTATS " & _
                                    "WHERE PORDERS.PORDSTAT  = PORDSTATS.PORDSTAT " & _
                                    "AND ORDNAME <> '' " & _
                                    "AND PORDERS.CLOSED <> 'C' " & _
                                    "AND PORDSTATS.APPROVED = 'Y' " & _
                                    "AND (ORDNAME = '%ME%' OR SUPORDNUM = '%ME%')"
            .Data = ""
            .AltEntry = ctrlText.tAltCtrlStyle.ctNone 'ctKeyb 
            .ctrlEnabled = False
            .MandatoryOnPost = False
        End With
        CtrlForm.AddField(field)

        'ORDNAME
        With field
            .Name = "VENDOR"
            .Title = "Vendor"
            .ValidExp = ValidStr(tRegExValidation.tString)
            .SQLValidation = "SELECT '%ME%'"
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

        ' STATUS
        With col
            .Name = "_VENDOR"
            .Title = "Vendor"
            .initWidth = 30
            .TextAlign = HorizontalAlignment.Left
            .AltEntry = ctrlText.tAltCtrlStyle.ctNone 'ctList 'ctKeyb 
            .ValidExp = ValidStr(tRegExValidation.tString)
            .SQLValidation = "SELECT '%ME%'"
            .DefaultFromCtrl = Nothing
            .ctrlEnabled = False
            .Mandatory = True
            .MandatoryOnPost = True
        End With
        CtrlTable.AddCol(col)

        '' VENDOR
        'With field
        '    .Name = "VENDOR"
        '    .Title = "Vendor"
        '    .ValidExp = ValidStr(tRegExValidation.tString)
        '    .SQLList = "SELECT DISTINCT SUPNAME FROM SUPPLIERS, PORDERS, PORDSTATS " & _
        '                    "WHERE SUPPLIERS.SUP = PORDERS.SUP " & _
        '                    "AND PORDERS.PORDSTAT  = PORDSTATS.PORDSTAT " & _
        '                    "AND PORDERS.CLOSED <> 'Y' " & _
        '                    "AND PORDSTATS.APPROVED = 'Y' " & _
        '                    "AND ORDNAME <> '' " & _
        '                    "ORDER BY SUPNAME"
        '    .SQLValidation = "SELECT SUPNAME FROM SUPPLIERS, PORDERS, PORDSTATS " & _
        '                    "WHERE SUPPLIERS.SUP = PORDERS.SUP " & _
        '                    "AND PORDERS.PORDSTAT  = PORDSTATS.PORDSTAT " & _
        '                    "AND PORDERS.CLOSED <> 'Y' " & _
        '                    "AND PORDSTATS.APPROVED = 'Y' " & _
        '                    "AND ORDNAME <> '' " & _
        '                    "AND SUPNAME = '%ME%'"
        '    .Data = ""
        '    .AltEntry = ctrlText.tAltCtrlStyle.ctList 'ctKeyb 
        '    .ctrlEnabled = True
        '    .MandatoryOnPost = True

        'End With
        'CtrlForm.AddField(field)

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

    Public Overrides Sub ProcessEntry(ByVal ctrl As SFDCData.ctrlText, ByVal Valid As Boolean)

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
                            Case "PONAME"

                        End Select
                    End If

                    ' *******************************************************************
                    ' *** Set which controls are enabled
                    Dim rcount As Integer
                    Try
                        rcount = UBound(CtrlTable.el) + 1
                    Catch
                        rcount = 0
                    End Try
                    CtrlForm.el(0).CtrlEnabled = Not (Len(CtrlForm.el(0).Data) > 0 And rcount > 0)
                    'CtrlTable.el(1).CtrlEnabled = Len(CtrlTable.el(0).Data) > 0

                    ' *******************************************************************
                    If Len(CtrlForm.el(0).Data) > 0 Then 'And Len(CtrlForm.el(1).Data) > 0
                        CtrlTable.Focus()
                    End If
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
                .RecordType1 = "SUPNAME,USERLOGIN,TOWARHSNAME,TOLOCNAME,MANUAL"
                .RecordType2 = "ORDNAME1,PARTNAME,TQUANT"
                .RecordTypes = "TEXT,TEXT,TEXT,TEXT,TEXT,TEXT,TEXT,"
            End With

            Dim l As New Dictionary(Of String, String)
            For y As Integer = 0 To CtrlTable.RowCount
                If Not l.ContainsKey(CtrlTable.ItemValue("_ORDNAME", y)) Then
                    l.Add(CtrlTable.ItemValue("_ORDNAME", y), CtrlTable.ItemValue("_VENDOR", y))
                End If
            Next

            For Each k As String In l.Keys

                ' Type 1 records
                Dim t1() As String = { _
                                    l(k), _
                                    UserName, _
                                    Warehouse, _
                                    CtrlForm.ItemValue("TOLOC"), _
                                    Argument("MANUAL") _
                                    }
                p.AddRecord(1) = t1


                For y As Integer = 0 To CtrlTable.RowCount
                    If CtrlTable.ItemValue("_ORDNAME", y) = k Then
                        Dim t2() As String = { _
                                    CtrlTable.ItemValue("_ORDNAME", y), _
                                    CtrlTable.ItemValue("_PARTNAME", y), _
                                    CStr(CInt(1000 * CtrlTable.ItemValue("_RECEIVED", y))) _
                                            }
                        p.AddRecord(2) = t2
                    End If
                Next

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
        Argument("MANUAL") = "Y"
        CtrlTable.mCol(0).ctrlEnabled = False
        CtrlTable.mCol(1).ctrlEnabled = False
        CtrlTable.mCol(2).ctrlEnabled = True
        'CtrlTable.mCol(3).ctrlEnabled = True
        'CtrlTable.mCol(4).ctrlEnabled = True
    End Sub

    Public Overrides Sub AfterEdit(ByVal TableIndex As Integer)

    End Sub

    Public Overrides Sub AfterAdd(ByVal TableIndex As Integer)


    End Sub

    Public Overrides Sub TableScan(ByVal Value As String)

        If System.Text.RegularExpressions.Regex.IsMatch(Value, ValidStr(tRegExValidation.tBarcode)) Then
            If Len(CtrlForm.el(1).Data) = 0 Then
                CtrlForm.el(1).Data = ""
                Beep()
            Else
                Dim add As Integer = 0
                Dim f As Boolean = False

                If Not PORD.ContainsKey(CtrlForm.el(1).Data) Then
                    Beep()
                    Exit Sub
                End If
                If Not PORD(CtrlForm.el(1).Data).ORDERITEMS.ContainsKey(Value) Then
                    Beep()
                    Exit Sub
                End If

                If CtrlTable.Table.Items.Count > 0 Then
                    For i As Integer = 0 To CtrlTable.Table.Items.Count - 1
                        If CtrlTable.Table.Items(i).SubItems(1).Text = CtrlForm.el(1).Data And _
                            CtrlTable.Table.Items(i).SubItems(0).Text = PORD(CtrlForm.el(1).Data).ORDERITEMS(Value) Then

                            f = True
                            CtrlTable.Table.Items(i).Selected = True

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
                                If .Manual Then Argument("MANUAL") = "Y"
                                .Dispose()
                            End With

                        Case "INCREMENT"
                            add = 1

                    End Select

                    Dim lvi As New ListViewItem
                    With CtrlTable.Table
                        .Items.Add(lvi)
                        .Items(.Items.Count - 1).Text = PORD(CtrlForm.el(1).Data).ORDERITEMS(Value)
                        .Items(.Items.Count - 1).SubItems.Add(CtrlForm.el(1).Data)
                        .Items(.Items.Count - 1).SubItems.Add(add)
                        .Items(.Items.Count - 1).SubItems.Add(CtrlForm.el(2).Data)
                    End With

                End If

                CtrlForm.el(1).Data = ""
                CtrlForm.el(2).Data = ""

            End If

        Else
            If PORD.ContainsKey(Value) Then

                CtrlForm.el(1).Data = Value
                CtrlForm.el(2).Data = PORD(Value).VENDOR

            Else
                mInvoke = tInvoke.iVendor
                InvokeData("SELECT DISTINCT ORDNAME, SUPNAME ,BARCODE, PARTNAME  " & _
                            "FROM PART, PORDERS, PORDERITEMS, SUPPLIERS " & _
                            "WHERE PORDERITEMS.PART = PART.PART " & _
                            "AND PORDERS.ORD = PORDERITEMS.ORD " & _
                            "AND PORDERS.SUP = SUPPLIERS.SUP " & _
                            "AND PORDERS.ORDNAME = '" & Value & "'")
            End If
        End If

    End Sub

    Public Overrides Sub EndInvokeData(ByVal Data(,) As String)

        Select Case mInvoke
            Case tInvoke.iBarcode
                Dim add As Integer = 0
                Dim f As Boolean = False

                If CtrlTable.Table.Items.Count > 0 Then
                    For i As Integer = 0 To CtrlTable.Table.Items.Count - 1
                        If CtrlTable.Table.Items(i).SubItems(1).Text = CtrlForm.el(1).Data And _
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
                                        If .Manual Then Argument("MANUAL") = "Y"
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
                                If .Manual Then Argument("MANUAL") = "Y"
                                .Dispose()
                            End With

                        Case "INCREMENT"
                            add = 1

                    End Select

                    Dim lvi As New ListViewItem
                    With CtrlTable.Table
                        .Items.Add(lvi)
                        .Items(.Items.Count - 1).Text = Data(0, 0)
                        .Items(.Items.Count - 1).SubItems.Add(CtrlForm.el(1).Data)
                        .Items(.Items.Count - 1).SubItems.Add(add)
                        .Items(.Items.Count - 1).SubItems.Add(CtrlForm.el(2).Data)
                    End With

                End If

                CtrlForm.el(1).Data = ""
                CtrlForm.el(2).Data = ""

            Case tInvoke.iVendor
                PORD.Add(Data(0, 0), New PORDERS(Data(1, 0)))
                For y As Integer = 0 To UBound(Data, 2)
                    PORD(Data(0, 0)).ORDERITEMS.Add(Data(2, y), Data(3, y))
                Next

                CtrlForm.el(1).Data = Data(0, 0)
                CtrlForm.el(2).Data = Data(1, 0)

        End Select

    End Sub

End Class
