Imports System
Imports System.IO
Imports System.Threading
Imports System.Xml
Imports System.Text.RegularExpressions

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
    Private Typer As String = "L"
    Private Family As Integer = 0
    Dim unqs() As Integer = {0}
    Dim unq As Integer
    Private Enum tInvoke
        iBarcode = 0
        iVendor = 1
        iPart = 2
    End Enum
    Private mInvoke As tInvoke = tInvoke.iBarcode

    Dim PORD As New Dictionary(Of String, PORDERS)
    Private TBar As String = ""
#Region "Initialisation"

    Public Sub New(Optional ByRef App As Form = Nothing)


        CallerApp = App
        NewArgument("SCANACTION", "OPENFORM")
        NewArgument("MANUAL", "N")
        CtrlTable.DisableButtons(False, False, True, False, False)

    End Sub

    Public Sub hEndForm() Handles MyBase.EndForm
        Argument("MANUAL") = "N"
    End Sub

#End Region
    Public Overrides Sub FormLoaded()
        With CtrlForm
            .el(0).DataEntry.Text = "GRVR"
            .el(0).ProcessEntry()
            .el(1).DataEntry.Text = "0"
            .el(1).ProcessEntry()
        End With
    End Sub
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

        'FROM WARHSNAME
        With field
            .Name = "WARHS"
            .Title = "W/H"
            .ValidExp = ValidStr(tRegExValidation.tString)
            '.SQLList = "SELECT DISTINCT WARHSNAME FROM WAREHOUSES WHERE INACTIVE <> 'Y'"
            .SQLValidation = "SELECT top 1 WARHSNAME FROM WAREHOUSES WHERE UPPER(WARHSNAME) = UPPER('%ME%')"
            .Data = ""      '******** Barcoded field '*******
            .AltEntry = SFDCData.ctrlText.tAltCtrlStyle.ctNone
            .ctrlEnabled = True
        End With
        CtrlForm.AddField(field)

        'To Locname
        With field
            .Name = "TOLOC"
            .Title = "Bin"
            .ValidExp = ValidStr(tRegExValidation.tString)
            '.SQLList = "SELECT DISTINCT LOCNAME FROM WAREHOUSES WHERE UPPER(WARHSNAME) = UPPER('%WARHS%') AND WAREHOUSES.INACTIVE <> 'Y'"
            .SQLValidation = "SELECT LOCNAME FROM WAREHOUSES WHERE UPPER(LOCNAME) = UPPER('%ME%') AND UPPER(WARHSNAME) = UPPER('%WARHS%') AND WAREHOUSES.INACTIVE <> 'Y'"
            .Data = ""
            .AltEntry = ctrlText.tAltCtrlStyle.ctNone 'ctKeyb 
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
                                    "AND (UPPER(ORDNAME) = UPPER('%ME%') OR UPPER(SUPORDNUM) = '%ME%')"
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
        '    .DefaultFromCtrl = CtrlForm.el(CtrlForm.ColNo("VENDOR"))
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
            .ValidExp = ValidStr(tRegExValidation.tString)
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


        ' FAMILY
        With col
            .Name = "_FAMILY"
            .Title = "Family"
            .initWidth = 0
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


        ' TYPE
        With col
            .Name = "_FTYPE"
            .Title = "FTYPE"
            .initWidth = 0
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
        CtrlTable.RecordsSQL = "SELECT PARTNAME, PORDERS.ORDNAME, 0 ,FAMILY,'L'" & _
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
                    .Items(.Items.Count - 1).SubItems.Add(Data(3, y))
                    .Items(.Items.Count - 1).SubItems.Add(Data(4, y))
                    .Items(.Items.Count - 1).SubItems.Add("L")
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
                    If ctrl.Data <> "" Then
                        Select Case ctrl.Name
                            Case "VENDOR"
                                CtrlTable.BeginLoadRS()
                            Case "TOLOC"
                                For Y As Integer = 0 To CtrlTable.Table.Items.Count - 1
                                    If Len(CtrlTable.Table.Items(Y).SubItems(2).Text) = 0 Then
                                        CtrlTable.Table.Items(Y).SubItems(2).Text = CtrlForm.el(CtrlForm.ColNo("PONAME")).Data
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
                    If Len(CtrlForm.el(0).Data) > 0 Then 'And Len(CtrlForm.el(CtrlForm.ColNo("PONAME")).Data) > 0
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
                .RecordType2 = "ORDNAME1,PARTNAME,TQUANT,ZGSM_LINEARMTRG"
                .RecordTypes = "TEXT,TEXT,TEXT,TEXT,TEXT,TEXT,TEXT,TEXT,TEXT"
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
                                    CtrlForm.ItemValue("WARHS"), _
                                    CtrlForm.ItemValue("TOLOC"), _
                                    Argument("MANUAL") _
                                    }
                p.AddRecord(1) = t1


                For y As Integer = 0 To CtrlTable.RowCount
                    If CtrlTable.ItemValue("_ORDNAME", y) = k Then
                        If CtrlTable.ItemValue("_FTYPE", y) = "L" Then
                            Dim t2() As String = { _
                                                                                          CtrlTable.ItemValue("_ORDNAME", y), _
                                                                                          CtrlTable.ItemValue("_PARTNAME", y), _
                                                                                         0, _
                                                                                                  CStr(CInt(1000 * CtrlTable.ItemValue("_RECEIVED", y)))}
                            p.AddRecord(2) = t2
                        Else
                            Dim t2() As String = { _
                                                                                          CtrlTable.ItemValue("_ORDNAME", y), _
                                                                                          CtrlTable.ItemValue("_PARTNAME", y), _
                                                                                          CStr(CInt(1000 * CtrlTable.ItemValue("_RECEIVED", y))), _
                                                                                                  0}
                            p.AddRecord(2) = t2
                        End If

                    End If
                Next

            Next

        Catch e As Exception
            OverControl.msgboxa(e.Message)
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
        Dim v2 As String = ""
        Dim lot As String = ""
        Dim qty As Integer = 0


        Value = Value.Replace(":", "")
        Dim doc As New Xml.XmlDocument
        doc.LoadXml(Value)
        If Regex.IsMatch(Value, "^<in><i") = True Then
            For Each nd As XmlNode In doc.SelectNodes("in/i")
                Dim DataType As String = nd.Attributes("n").Value
                Select Case DataType
                    Case "PROCESS"
                        ProcessForm()
                    Case "CLOSE"
                        Me.CloseMe()
                End Select

            Next
        ElseIf Regex.IsMatch(Value, "^<") = False Then
            OverControl.msgboxa("This doesnt appear to be a valid 2d barcode")
        Else

            For Each nd As XmlNode In doc.SelectNodes("in/g")
                Dim DataType As String = nd.Attributes("n").Value
                Select Case DataType
                    Case "ORDNAME"
                        v2 = nd.Attributes("v").Value
                    Case "PART"
                        mInvoke = tInvoke.iPart
                        InvokeData("SELECT PART.BARCODE,PART.FAMILY FROM PART WHERE PART.BARCODE = '" & nd.Attributes("v").Value & "'")
                        v2 = TBar
                    Case "UNQ"
                        unq = Convert.ToInt32(nd.Attributes("n").Value)
                    Case "PROCESS"
                        ProcessForm()
                    Case "CLOSE"
                        Me.CloseMe()
                End Select

            Next
            If v2 <> "" Then
                If System.Text.RegularExpressions.Regex.IsMatch(v2, ValidStr(tRegExValidation.tBarcode)) Then
                    If Len(CtrlForm.el(CtrlForm.ColNo("PONAME")).Data) = 0 Then
                        CtrlForm.el(CtrlForm.ColNo("PONAME")).Data = ""
                        Beep()
                    Else
                        Dim add As Decimal = 0.0
                        Dim f As Boolean = False

                        If Not PORD.ContainsKey(CtrlForm.el(CtrlForm.ColNo("PONAME")).Data) Then
                            Beep()
                            Exit Sub
                        End If
                        If Not PORD(CtrlForm.el(CtrlForm.ColNo("PONAME")).Data).ORDERITEMS.ContainsKey(v2) Then
                            Beep()
                            OverControl.msgboxa("Scanned item not found in this purchase order")
                            Exit Sub
                        End If

                        If CtrlTable.Table.Items.Count > 0 Then
                            For i As Integer = 0 To CtrlTable.Table.Items.Count - 1
                                If CtrlTable.Table.Items(i).SubItems(1).Text = CtrlForm.el(CtrlForm.ColNo("PONAME")).Data And _
                                    CtrlTable.Table.Items(i).SubItems(0).Text = PORD(CtrlForm.el(CtrlForm.ColNo("PONAME")).Data).ORDERITEMS(v2) Then

                                    f = True
                                    CtrlTable.Table.Items(i).Selected = True

                                    Select Case Argument("SCANACTION")

                                        Case "OPENFORM"

                                            Dim num As New frmNumber
                                            With num
                                                .Text = v2
                                                .ShowDialog()
                                                add = .number
                                                If .Manual Then Argument("MANUAL") = "Y"
                                                .Dispose()
                                            End With

                                            If CtrlTable.Table.Items(i).SubItems(4).Text = "33" Then
                                                Dim g As New frmMetreLen
                                                g.ShowDialog()
                                                If g.DialogResult = Windows.Forms.DialogResult.Yes Then
                                                    CtrlTable.Table.Items(i).SubItems(5).Text = "M"
                                                Else
                                                    CtrlTable.Table.Items(i).SubItems(5).Text = "L"
                                                End If
                                            End If
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
                            If Family = 33 Then
                                Dim g As New frmMetreLen
                                g.ShowDialog()
                                If g.DialogResult = Windows.Forms.DialogResult.Yes Then
                                    Typer = "M"
                                Else
                                    Typer = "L"
                                End If

                            End If
                            Dim lvi As New ListViewItem
                            With CtrlTable.Table
                                .Items.Add(lvi)
                                .Items(.Items.Count - 1).Text = PORD(CtrlForm.el(CtrlForm.ColNo("PONAME")).Data).ORDERITEMS(v2)
                                .Items(.Items.Count - 1).SubItems.Add(CtrlForm.el(CtrlForm.ColNo("PONAME")).Data)
                                .Items(.Items.Count - 1).SubItems.Add(add)
                                .Items(.Items.Count - 1).SubItems.Add(CtrlForm.el(CtrlForm.ColNo("VENDOR")).Data)
                                .Items(.Items.Count - 1).SubItems.Add(Family)
                                .Items(.Items.Count - 1).SubItems.Add(Typer)
                            End With

                        End If

                        'CtrlForm.el(CtrlForm.ColNo("PONAME")).Data = ""
                        'CtrlForm.el(CtrlForm.ColNo("VENDOR")).Data = ""

                    End If

                Else
                    If PORD.ContainsKey(v2) Then

                        CtrlForm.el(CtrlForm.ColNo("PONAME")).Data = v2
                        CtrlForm.el(CtrlForm.ColNo("VENDOR")).Data = PORD(v2).VENDOR

                    Else
                        mInvoke = tInvoke.iVendor
                        InvokeData("SELECT DISTINCT ORDNAME, SUPNAME ,BARCODE, PARTNAME,FAMILY  " & _
                                    "FROM PART, PORDERS, PORDERITEMS, SUPPLIERS " & _
                                    "WHERE PORDERITEMS.PART = PART.PART " & _
                                    "AND PORDERS.ORD = PORDERITEMS.ORD " & _
                                    "AND PORDERS.SUP = SUPPLIERS.SUP " & _
                                    "AND PORDERS.ORDNAME = '" & v2 & "'")
                    End If
                End If
            Else
                OverControl.msgboxa("Code not recognised")
            End If
        End If
        CtrlTable.Focus()


    End Sub

    Public Overrides Sub EndInvokeData(ByVal Data(,) As String)

        Select Case mInvoke
            Case tInvoke.iBarcode
                Dim add As Integer = 0
                Dim f As Boolean = False

                If CtrlTable.Table.Items.Count > 0 Then
                    For i As Integer = 0 To CtrlTable.Table.Items.Count - 1
                        If CtrlTable.Table.Items(i).SubItems(1).Text = CtrlForm.el(CtrlForm.ColNo("PONAME")).Data And _
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
                        .Items(.Items.Count - 1).SubItems.Add(CtrlForm.el(CtrlForm.ColNo("PONAME")).Data)
                        .Items(.Items.Count - 1).SubItems.Add(add)
                        .Items(.Items.Count - 1).SubItems.Add(CtrlForm.el(CtrlForm.ColNo("VENDOR")).Data)
                    End With

                End If


                With CtrlForm
                    .el(.ColNo("PONAME")).Data = ""
                    .el(.ColNo("VENDOR")).Data = ""
                End With


            Case tInvoke.iVendor
                PORD.Add(Data(0, 0), New PORDERS(Data(1, 0)))
                For y As Integer = 0 To UBound(Data, 2)
                    PORD(Data(0, 0)).ORDERITEMS.Add(Data(2, y), Data(3, y))
                Next

                With CtrlForm
                    .el(.ColNo("PONAME")).Data = Data(0, 0)
                    .el(.ColNo("VENDOR")).Data = Data(1, 0)
                End With

            Case tInvoke.iPart
                If Data Is Nothing Then
                    TBar = ""
                Else
                    TBar = Data(0, 0)
                    Family = Data(1, 0)
                End If
        End Select
        CtrlTable.Focus()
    End Sub

End Class

