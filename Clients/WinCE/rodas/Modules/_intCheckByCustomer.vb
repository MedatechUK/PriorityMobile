Imports System.Data
Public Class interfaceCheckByCustomer
    Inherits SFDCData.iForm
#Region "Table selection - non barcode"
    Private Sub meclick()
        If CtrlTable.Table.SelectedIndices.Count = 0 Then
            Exit Sub

        End If


        With CtrlForm
            If Not (.el(.ColNo("ROUTE")).Data.Length > 0) Then MsgBox("Please select a route.")
            'If Not (.el(.ColNo("WHS")).Data.Length > 0) Then MsgBox("Please select a warehouse.")
        End With

        Dim m As Integer
        m = 1

        Dim h As Integer
        h = CtrlTable.Table.Items.Count
        If h >= 0 Then 'check to see if there are any rows to select
            Dim it As ListViewItem
            For Each it In CtrlTable.Table.Items
                If it.Selected = True Then


                    Dim g As String
                    g = it.SubItems(0).Text
                    CtrlForm.el(4).DataEntry.Text = g
                    CtrlForm.el(4).Text = g
                    CtrlForm.el(4).Update()
                    CtrlForm.el(4).ProcessEntry()

                    Exit For
                End If
                If CtrlTable.Table.Items.Count = 0 Then
                    Exit For
                End If
            Next
        End If





    End Sub
#End Region
#Region "Variables etc"
    Private current_part As String = ""
    Dim FINALLIST As New List(Of PSLIPITEMS)
    Dim changelist As New List(Of ErrorLog)
    '****************************************
    'Dim changelist As New DataTable


    Dim doclist As New List(Of WHSTRAN)
    Private PickDate As Integer
    Private CheckID As Integer = 0
    Private CheckLine As Integer = 0
    Public Enum tSendType
        Route = 0
        PackSlip = 1
        Part = 2
        Warhs = 3
        Bin = 4
        Amount = 5
        AmountCheck = 6
        PickID = 7
        LOT = 8
        time = 9
        PickDate = 10
        Cust = 11
        None = 12
        Header = 13
        NextIndex = 14
        DocNo = 15
    End Enum

#End Region
#Region "Column Declerations"
    Public Overrides Sub FormSettings()
        With field 'using the tfield structure from the ctrlForm
            .Name = "ROUTE"
            .Title = "Route"
            .ValidExp = "^[0-9A-Za-z]+$"
            .SQLValidation = "SELECT ROUTENAME FROM V_PICKED_ROUTE where ROUTENAME = '%ME%'"
            .SQLList = "SELECT distinct ROUTENAME FROM V_PICKED_ROUTE WHERE ZROD_PICKTYPE = 'S'"
            .AltEntry = ctrlText.tAltCtrlStyle.ctList 'ctKeyb 
            .ctrlEnabled = True
            .MandatoryOnPost = False
        End With
        CtrlForm.AddField(field)

        With field 'using the tfield structure from the ctrlForm
            .Name = "FORDATE"
            .Title = "Date"
            .ValidExp = ".*"
            .SQLValidation = "SELECT '%ME%'"
            .SQLList = "SELECT PICKDATE FROM V_PICKED_ROUTE where ROUTENAME = '%ROUTE%' ORDER BY DUEDATE ASC"
            .Data = ""
            .AltEntry = ctrlText.tAltCtrlStyle.ctList 'ctKeyb 
            .ctrlEnabled = True
            .MandatoryOnPost = False
        End With
        CtrlForm.AddField(field)

        With field 'using the tfield structure from the ctrlForm
            .Name = "PICKID"
            .Title = "Pick ID"
            .ValidExp = ValidStr(tRegExValidation.tNumeric)
            .SQLValidation = _
                "SELECT     PICK " & _
                "FROM         ZROD_PICKS " & _
                "WHERE     (FORROUTE = '%ROUTE%')"
            .Data = ""
            .AltEntry = ctrlText.tAltCtrlStyle.ctNone 'ctKeyb 
            .ctrlEnabled = False
            .MandatoryOnPost = False
        End With
        CtrlForm.AddField(field)

        With field 'using the tfield structure from the ctrlForm
            .Name = "CUSTOMER"
            .Title = "Customer"
            .ValidExp = ValidStr(tRegExValidation.tPackingSlip)
            .SQLValidation = "select '%ME%'"
            .Data = ""
            .AltEntry = ctrlText.tAltCtrlStyle.ctNone 'ctKeyb 
            .ctrlEnabled = False
            .MandatoryOnPost = False
        End With
        CtrlForm.AddField(field)


        With field 'using the tfield structure from the ctrlForm
            .Name = "PART"
            .Title = "Part"
            .ValidExp = ValidStr(tRegExValidation.tString)
            .SQLValidation = _
                "SELECT     '%ME%'"
            .Data = ""
            .AltEntry = ctrlText.tAltCtrlStyle.ctNone 'ctKeyb 
            .ctrlEnabled = False
            .MandatoryOnPost = False
        End With
        CtrlForm.AddField(field)

        With field 'using the tfield structure from the ctrlForm
            .Name = "AMOUNTREQ"
            .Title = "Amount"
            .ValidExp = ValidStr(tRegExValidation.tNumeric)
            .SQLValidation = "SELECT '%ME%'"
            .Data = ""
            .AltEntry = ctrlText.tAltCtrlStyle.ctNone
            'ctKeyb 
            .ctrlEnabled = False
            .MandatoryOnPost = False
        End With
        CtrlForm.AddField(field)

    End Sub
    Public Overrides Sub TableSettings()
        '0 - part
        With col
            .Name = "_PART"
            .Title = "PART No"
            .initWidth = 40
            .TextAlign = HorizontalAlignment.Left
            .AltEntry = ctrlText.tAltCtrlStyle.ctNone 'ctKeyb 
            .ValidExp = ValidStr(tRegExValidation.tBarcode)
            .SQLList = ""
            .SQLValidation = "SELECT '%ME%'"
            .DefaultFromCtrl = Nothing
            .ctrlEnabled = False
            .Mandatory = False
            .MandatoryOnPost = False
        End With
        CtrlTable.AddCol(col)

        '1 - PARTDES
        With col
            .Name = "_PARTDES"
            .Title = "Part Name"
            .initWidth = 70
            .TextAlign = HorizontalAlignment.Center
            .AltEntry = ctrlText.tAltCtrlStyle.ctNone 'ctKeyb 
            .ValidExp = ValidStr(tRegExValidation.tString)
            .SQLList = ""
            .SQLValidation = ""
            .DefaultFromCtrl = Nothing
            .ctrlEnabled = False
            .Mandatory = False
            .MandatoryOnPost = False
        End With
        CtrlTable.AddCol(col)

        '2 - QUANTITY
        With col
            .Name = "_QUANT"
            .Title = "Quantity"
            .initWidth = 30
            .TextAlign = HorizontalAlignment.Left
            .AltEntry = ctrlText.tAltCtrlStyle.ctNone 'ctKeyb 
            .ValidExp = ValidStr(tRegExValidation.tNumeric)
            .SQLList = ""
            .SQLValidation = "SELECT '%ME%'"
            .DefaultFromCtrl = Nothing
            .ctrlEnabled = False
            .Mandatory = False
            .MandatoryOnPost = False
        End With
        CtrlTable.AddCol(col)

        '3 - CHECKED
        With col
            .Name = "_CHECKED"
            .Title = "Picked"
            .initWidth = 25
            .TextAlign = HorizontalAlignment.Center
            .AltEntry = ctrlText.tAltCtrlStyle.ctNone 'ctKeyb 
            .ValidExp = ValidStr(tRegExValidation.tString)
            .SQLList = ""
            .SQLValidation = ""
            .DefaultFromCtrl = Nothing
            .ctrlEnabled = False
            .Mandatory = False
            .MandatoryOnPost = False
        End With
        CtrlTable.AddCol(col)

        '4 - CHECK COUNT (AMOUNT OF TIMES CHECKED)
        With col
            .Name = "_COUNT"
            .Title = "Picked"
            .initWidth = 0
            .TextAlign = HorizontalAlignment.Center
            .AltEntry = ctrlText.tAltCtrlStyle.ctNone 'ctKeyb 
            .ValidExp = ValidStr(tRegExValidation.tString)
            .SQLList = ""
            .SQLValidation = ""
            .DefaultFromCtrl = Nothing
            .ctrlEnabled = False
            .Mandatory = False
            .MandatoryOnPost = False
        End With
        CtrlTable.AddCol(col)

        '5 - DONE FLAG

        With col
            .Name = "_DONE"
            .Title = "DONE"
            .initWidth = 0
            .TextAlign = HorizontalAlignment.Center
            .AltEntry = ctrlText.tAltCtrlStyle.ctNone 'ctKeyb 
            .ValidExp = ValidStr(tRegExValidation.tString)
            .SQLList = ""
            .SQLValidation = ""
            .DefaultFromCtrl = Nothing
            .ctrlEnabled = False
            .Mandatory = False
            .MandatoryOnPost = False
        End With
        CtrlTable.AddCol(col)


        '15 - Packing Amount
        With col
            .Name = "_PACKING"
            .Title = "PACKING"
            .initWidth = 0
            .TextAlign = HorizontalAlignment.Center
            .AltEntry = ctrlText.tAltCtrlStyle.ctNone 'ctKeyb 
            .ValidExp = ValidStr(tRegExValidation.tString)
            .SQLList = ""
            .SQLValidation = "SELECT '%ME%'"
            .DefaultFromCtrl = Nothing
            .ctrlEnabled = False
            .Mandatory = False
            .MandatoryOnPost = False
        End With
        CtrlTable.AddCol(col)

        '16 - CONVERTER flag
        With col
            .Name = "_CONVFLAG"
            .Title = "CONVFLAG"
            .initWidth = 0
            .TextAlign = HorizontalAlignment.Center
            .AltEntry = ctrlText.tAltCtrlStyle.ctNone 'ctKeyb 
            .ValidExp = ValidStr(tRegExValidation.tString)
            .SQLList = ""
            .SQLValidation = "SELECT '%ME%'"
            .DefaultFromCtrl = Nothing
            .ctrlEnabled = False
            .Mandatory = False
            .MandatoryOnPost = False
        End With
        CtrlTable.AddCol(col)

        '17 - pack count
        With col
            .Name = "_PACKS"
            .Title = "PACKS"
            .initWidth = 20
            .TextAlign = HorizontalAlignment.Center
            .AltEntry = ctrlText.tAltCtrlStyle.ctNone 'ctKeyb 
            .ValidExp = ValidStr(tRegExValidation.tString)
            .SQLList = ""
            .SQLValidation = "SELECT '%ME%'"
            .DefaultFromCtrl = Nothing
            .ctrlEnabled = False
            .Mandatory = False
            .MandatoryOnPost = False
        End With
        CtrlTable.AddCol(col)

        '18 - single units
        With col
            .Name = "_UNITS"
            .Title = "UNITS"
            .initWidth = 20
            .TextAlign = HorizontalAlignment.Center
            .AltEntry = ctrlText.tAltCtrlStyle.ctNone 'ctKeyb 
            .ValidExp = ValidStr(tRegExValidation.tString)
            .SQLList = ""
            .SQLValidation = "SELECT '%ME%'"
            .DefaultFromCtrl = Nothing
            .ctrlEnabled = False
            .Mandatory = False
            .MandatoryOnPost = False
        End With
        CtrlTable.AddCol(col)

    End Sub
#End Region
#Region "invocations"
    Public Overrides Sub EndInvokeData(ByVal Data(,) As String)
        Select Case SendType
            Case tSendType.PickDate
                'This is used to set the argument that stores the picking date from the database
                PickDate = Convert.ToInt64(Data(0, 0))

            Case tSendType.DocNo
                doclist.Clear()
                Try
                    For y As Integer = 0 To UBound(Data, 2)
                        Dim d As New WHSTRAN(Data(0, y), 0, "", "", 0, 0, "")
                        doclist.Add(d)
                    Next
                Catch e As Exception
                    MsgBox(e.Message)
                End Try



            Case tSendType.time
                With CtrlForm


                    'With .el(.ColNo("FORDATE"))

                    '    Dim pdate As Date
                    '    pdate = FormatDateTime("1/1/1988", DateFormat.ShortDate)
                    '    Dim am As Integer = 0
                    '    am = Convert.ToInt32(Data(0, 0))
                    '    If am <> 0 Then
                    '        .DataEntry.Text = DateAdd(DateInterval.Minute, am, pdate)
                    '        .Data = DateAdd(DateInterval.Minute, am, pdate)
                    '        Me.Argument("PickDate") = DateAdd(DateInterval.Minute, am, pdate)
                    '    End If
                    '.DataEntry.Text = Data(0, 0)
                    ' End With
                End With

            Case tSendType.LOT
                With CtrlForm
                    With .el(.ColNo("PART"))
                        .DataEntry.Text = Data(0, 0)
                        .ProcessEntry()
                    End With
                End With

            Case tSendType.Route
                With CtrlForm
                    With .el(.ColNo("ROUTE"))
                        .Enabled = False
                        Me.Argument("HoldWARHS") = .Data & "PI"
                    End With
                    SendType = tSendType.Bin
                    'check to see if a customer exists for this route and if so get the first one

                End With


                With CtrlForm
                    Me.Text = Data(0, 0)
                    With .el(.ColNo("CUSTOMER"))
                        .DataEntry.Text = Data(0, 0)
                        .ProcessEntry()

                    End With

                End With

            Case tSendType.Header
                If Data Is Nothing Then
                Else
                    Me.Text = Data(0, 0)
                End If

                'InvokeData("insert into ZROD_CHECK(CHECK_ROUTE,CHECK_DATE,CHECK_PICKDATE,CHECK_BY) VALUES(" & CtrlForm.el(0).Data & "," & PickDate & ", " & PickDate & ", " & UserName & "); SELECT SCOPE_IDENTITY()")
                'SendType = tSendType.NextIndex
                'InvokeData("SELECT MAX(CHECK_LINE) FROM ZROD_CHECK_LINE")

            Case tSendType.NextIndex
                CheckLine = Data(0, 0)
                CheckLine += 1


            Case tSendType.Part
                'ITERATE THROUGH THE TABLE TO DESELECT ALL PARTS
                Dim it As ListViewItem
                For Each it In CtrlTable.Table.Items
                    it.Selected = False
                Next
                Dim m As Integer
                m = 1

                Dim h As Integer
                h = CtrlTable.Table.Items.Count
                If h >= 0 Then 'check to see if there are any rows to select
                    For Each it In CtrlTable.Table.Items
                        If it.SubItems(0).Text = Data(0, 0) Then
                            'find the correct part in the table
                            it.Selected = True
                            'select the line so the user has visual confirmation
                            If it.SubItems(4).Text <> "o" Then
                                'fires if we have hit this item previously we then need to get the counted amount
                                'the subitems(4) holds the count of the amount of times this item has been checked
                                'and the subitems(3) holds the last count
                                Dim add As Integer
                                Dim num As New frmNumber
                                With num
                                    .Text = it.SubItems(1).Text
                                    .ShowDialog()
                                    add = .number
                                    .Dispose()
                                End With


                                'next we check the counted quantity
                                Dim expected, lastcount As Integer
                                expected = Convert.ToInt32(it.SubItems(2).Text)
                                lastcount = Convert.ToInt32(it.SubItems(3).Text)
                                SendType = tSendType.None
                                InvokeData("insert into ZROD_CHECK_LINE(CHECK_UNIQUE,PART,ACTUAL,EXPECTED) VALUES(" & CheckLine & ",'" & it.SubItems(0).Text & "', " & add & ", " & expected & ")")
                                CheckLine += 1
                                'Dim SQL As String
                                'SQL = "UPDATE ZROD_CHECKLINES SET PARTNO = '" & it.SubItems(0).Text & "', COUNTED = " & add & ", EXPECTED =" & expected & ", COUNT_BY ='" & UserName & ", COUNT_ON = " & PickDate
                                'SendType = tSendType.None
                                'InvokeData(SQL)
                                If lastcount = add Then
                                    'we have had 2 counts the same so ......
                                    'we need to compare the amount we have counted (twice) against what is expected
                                    Dim amount As Integer
                                    amount = add - expected
                                    'amount holds the difference between what we counted and what we expected
                                    If amount = 0 Then
                                        'THE ITEMS MATCH SO WE NEED TO ADD IT TO THE FINISHED LIST
                                        Dim finlist As New PSLIPITEMS( _
                                        0, _
                                        CtrlForm.el(0).Data, _
                                        CtrlForm.el(3).Data, _
                                        CtrlForm.el(4).Data, _
                                        add, _
                                        it.SubItems(1).Text, _
                                        "0", _
                                        Me.Argument("HoldWARHS"), _
                                       "0", _
                                        " ", " ", " ", 0, 0, CtrlForm.el(3).Data)
                                        FINALLIST.Add(finlist)

                                        With CtrlTable
                                            .Table.Items.Remove(it)
                                        End With
                                    ElseIf amount > 0 Then
                                        'we have too many items intruct user to return surplus


                                        MsgBox("You appear to have picked too many items for this part, please RETURN " & amount & " back to stock")
                                        Dim finlist As New PSLIPITEMS( _
                                       0, _
                                       CtrlForm.el(0).Data, _
                                       CtrlForm.el(3).Data, _
                                       CtrlForm.el(4).Data, _
                                       add, _
                                       it.SubItems(1).Text, _
                                       "0", _
                                       Me.Argument("HoldWARHS"), _
                                      "0", _
                                       " ", " ", " ", 0, 0, CtrlForm.el(3).Data)
                                        FINALLIST.Add(finlist)
                                        Dim err As New ErrorLog("Return", it.SubItems(1).Text, amount)
                                        changelist.Add(err)
                                        err = Nothing
                                        'changelist.Add("Return " & amount & " of " & it.SubItems(1).Text & " back to stock")
                                        With CtrlTable
                                            .Table.Items.Remove(it)
                                        End With
                                    ElseIf amount < 0 Then
                                        MsgBox("You appear to have picked too few items for this part, please TAKE " & -amount & " from stock")
                                        Dim finlist As New PSLIPITEMS( _
                                       0, _
                                       CtrlForm.el(0).Data, _
                                       CtrlForm.el(3).Data, _
                                       CtrlForm.el(4).Data, _
                                       add, _
                                       it.SubItems(1).Text, _
                                       "0", _
                                       Me.Argument("HoldWARHS"), _
                                      "0", _
                                       " ", " ", " ", 0, 0, CtrlForm.el(3).Data)
                                        FINALLIST.Add(finlist)
                                        Dim err As New ErrorLog("Take", it.SubItems(1).Text, -amount)
                                        changelist.Add(err)
                                        err = Nothing
                                        'Dim msg As String
                                        'msg = "Take " & amount & " of " & it.SubItems(1).Text & " from stock"
                                        'Try
                                        '    changelist.Add(msg)
                                        'Catch ex As Exception
                                        '    MsgBox(ex.ToString)
                                        'End Try

                                        With CtrlTable
                                            .Table.Items.Remove(it)
                                        End With
                                    End If
                                Else

                                    MsgBox("This count does not match the expected amount, please RECOUNT and try again")
                                    it.SubItems(3).Text = add
                                    Dim counts As Integer
                                    counts = Convert.ToInt32(it.SubItems(4).Text)
                                    counts += 1
                                    it.SubItems(4).Text = counts

                                End If

                            Else

                                'we havent had a check on this item yet as the check is still set to o
                                'so we will need to get the amount counted by calling the number pad
                                Dim add As Integer
                                Dim num As New frmNumber
                                With num
                                    .Text = it.SubItems(1).Text
                                    .ShowDialog()
                                    add = .number
                                    .Dispose()
                                End With
                                'now we check the number against the expected amount
                                Dim expected As Integer
                                expected = Convert.ToInt32(it.SubItems(2).Text)
                                SendType = tSendType.None
                                InvokeData("insert into ZROD_CHECK_LINE(CHECK_UNIQUE,PART,ACTUAL,EXPECTED) VALUES(" & CheckLine & ",'" & it.SubItems(0).Text & "', " & add & ", " & expected & ")")
                                CheckLine += 1
                                'Dim SQL As String
                                'SQL = "UPDATE ZROD_CHECKLINES SET PARTNO = '" & it.SubItems(0).Text & "', COUNTED = " & add & ", EXPECTED =" & expected & ", COUNT_BY ='" & UserName & ", COUNT_ON = " & PickDate
                                'SendType = tSendType.None
                                'InvokeData(SQL)
                                If add = expected Then
                                    Dim finlist As New PSLIPITEMS( _
                                       0, _
                                       CtrlForm.el(0).Data, _
                                       CtrlForm.el(3).Data, _
                                       CtrlForm.el(4).Data, _
                                       add, _
                                       it.SubItems(1).Text, _
                                       "0", _
                                       Me.Argument("HoldWARHS"), _
                                      "0", _
                                       " ", " ", " ", 0, 0, CtrlForm.el(3).Data)
                                    FINALLIST.Add(finlist)
                                    'the check is fine, this line can be hidden
                                    'TODO add hiding ability or add a boolean field to check against so that any attempts to recheck a done line error out
                                    CtrlTable.Table.Items.Remove(it)

                                ElseIf add < expected Then
                                    MsgBox("This count does not match the expected amount, please RECOUNT and try again")
                                    it.SubItems(3).Text = add
                                    it.SubItems(4).Text = 1
                                ElseIf add > expected Then
                                    MsgBox("This count does not match the expected amount, please RECOUNT and try again")
                                    it.SubItems(3).Text = add
                                    it.SubItems(4).Text = 1
                                End If
                                current_part = it.SubItems(0).Text


                            End If

                        End If
                    Next
                End If
        End Select
    End Sub
    Dim SendType As tSendType = tSendType.Route


#End Region
#Region "Form Processing"
    Public Overrides Sub AfterAdd(ByVal TableIndex As Integer)

    End Sub

    Public Overrides Sub AfterEdit(ByVal TableIndex As Integer)

    End Sub

    Public Overrides Sub BeginAdd()

    End Sub

    Public Overrides Sub BeginEdit()

    End Sub

    Public Overrides Sub ProcessEntry(ByVal ctrl As SFDCData.ctrlText, ByVal Valid As Boolean)
        Select Case Valid
            Case False
                Beep()

            Case True
                Try
                    Dim i As String
                    i = ctrl.Data.ToString

                    If ctrl.Data <> "" Then
                        Select Case ctrl.Name
                            Case "FORDATE"
                                'SendType = tSendType.Route

                                'InvokeData("SELECT CUSTNAME FROM dbo.V_Route_Customer  WHERE ZROD_ROUTE = '%ROUTE%' ")
                                Me.CtrlTable.Table.Items.Clear()
                                Dim j As DateTime = FormatDateTime("1/1/1988", DateFormat.LongDate)
                                Dim hh As Integer = DateDiff(DateInterval.Minute, j, Now)

                                SendType = tSendType.PickDate
                                InvokeData("SELECT DUEDATE FROM V_PICKED_MONITOR WHERE PICKDATE = '%FORDATE%' AND ROUTENAME = '%ROUTE%'")

                                SendType = tSendType.Cust
                                Dim PD As Integer
                                Dim t As String = PickDate
                                Try
                                    PD = Convert.ToInt64(t)
                                Catch ex As Exception
                                    MsgBox(ex.ToString)
                                End Try

                                'InvokeData("exec dbo.SP_SFDC_UPDATEITEMS " & RouteID & "," & PD)
                                SendType = tSendType.Route
                                InvokeData("select dbo.FUNC_ROUTE_CHECKED('%ROUTE%'," & PickDate & ") as DOCNO")

                                'SendType = tSendType.Header
                                'InvokeData("insert into ZROD_CHECK(CHECK_ROUTE,CHECK_DATE,CHECK_PICKDATE,CHECK_BY) VALUES('" & CtrlForm.el(0).Data & "'," & hh & ", " & PickDate & ", '" & UserName & "'); SELECT SCOPE_IDENTITY()")

                            Case "PICKID"
                                SendType = tSendType.PickID
                                InvokeData("SELECT FORDATE,PACKSLIP FROM ZROD_PICKS WHERE PICK = '%PICKID%'")

                            Case "PART"
                                SendType = tSendType.Part

                                InvokeData("SELECT PARTNAME FROM V_PICKED_MONITOR WHERE PARTNAME = '%PART%'")
                            Case "CUSTOMER"

                                CtrlTable.RecordsSQL = _
             "select PARTNAME,PARTDES,sum(dbo.REALQUANT(QUANT)) as PICKED,'o' as CHECK_SUM, 'o' as CHECK_COUNT,CONV,PACKING,NOTFIXEDCONV " & _
             "from V_PICKED_MONITOR " & _
             "WHERE CUSTNAME = '%CUSTOMER%' AND ROUTENAME = '%ROUTE%' AND DUEDATE = " & PickDate & _
             " group by PARTNAME,PARTDES,CONV,PACKING,NOTFIXEDCONV " & _
             "order BY PARTNAME"

                                With CtrlTable
                                    .Table.Items.Clear()
                                    .BeginLoadRS()
                                    .Table.Focus()
                                End With
                                SendType = tSendType.Header
                                InvokeData("SELECT CUSTDES FROM V_PICKED_MONITOR WHERE CUSTNAME = '%CUSTOMER%'")

                                doclist.Clear()

                                SendType = tSendType.DocNo
                                InvokeData("SELECT DISTINCT DOCNO FROM V_Route_Customer WHERE CUSTNAME = '%CUSTOMER%' AND ZROD_ROUTE = '%ROUTE%' AND PICKDATE = '%FORDATE%'")
                                'changelist.Columns.Add("Trans Type", GetType(String))
                                'changelist.Columns.Add("Part", GetType(String))
                                'changelist.Columns.Add("Quant", GetType(Integer))

                        End Select
                    End If
                Catch ex As Exception

                End Try
        End Select

    End Sub

    Public Overrides Sub ProcessForm()
        With CtrlForm
            If .el(3).Data.Length = 0 Then

            Else
                Try
                    With p
                        .DebugFlag = False
                        .Procedure = "ZSFDC_LOAD_VAN"
                        .Table = "ZSFDC_LOAD_VAN"
                        .RecordType1 = "CURDATE,WARHSNAME,LOCNAME,TOWARHSNAME,TOLOCNAME"
                        .RecordType2 = "DOCNO"
                        .RecordTypes = "TEXT,TEXT,TEXT,TEXT,TEXT,TEXT"
                    End With

                Catch e As Exception
                    MsgBox(e.Message)
                End Try

                ' Type 1 records


                Dim startdate As Date = FormatDateTime("1/1/1988", DateFormat.LongDate)
                Dim nowdate As Integer = DateDiff(DateInterval.Minute, startdate, Now())
                Dim t1() As String = { _
                                    nowdate, _
                                    CtrlForm.ItemValue("ROUTE") & "PI", _
                                    "0", _
                                    CtrlForm.ItemValue("ROUTE"), _
                                    "0" _
                                    }
                p.AddRecord(1) = t1
               
                For y As Integer = 0 To (doclist.Count - 1)
                    Dim t2() As String = {doclist(y).trPart}
                    p.AddRecord(2) = t2
                Next
                doclist.Clear()

                FINALLIST.Clear()
                If changelist.Count > 0 Then
                    Dim f As New frmDisplay
                    f.Text = "Error Report"
                    Dim startpos As New Point(3, 11)
                    Dim errsize As New Size(400, 30)

                    For Each h As ErrorLog In changelist
                        Dim errlab As Label = New Label
                        errlab.Location = startpos
                        errlab.Size = errsize
                        startpos.Y += 70
                        errlab.Text = h.EType.ToUpper & " " & h.Amount & " " & h.Desc
                        f.Panel1.Controls.Add(errlab)
                    Next

                   

                    Argument("PickDate") = ""
                    Argument("HoldWARHS") = ""
                    Me.Text = ""
                    f.ShowDialog()
                    changelist.Clear()
                    doclist.Clear()

                End If
                'SendType = tSendType.time
                'InvokeData("UPDATE ORDERITEMS SET ZROD_IN_CHECK = 'Y', ZROD_CHECKED_ON = dbo.DATETOMIN(GETDATE()), ZROD_CHECKED_BY = " & UserName & " WHERE  CUSTNAME = '%CUSTOMER%' AND ROUTENAME = '%ROUTE%' AND DUEDATE = " & pickdate)
            End If
        End With



    End Sub
#End Region
    Public Sub New(Optional ByRef App As Form = Nothing)

        'InitializeComponent()
        CallerApp = App
        NewArgument("PickDate", " ")
        NewArgument("HoldWARHS", " ")
        AddHandler CtrlTable.Table.ItemActivate, AddressOf meclick
    End Sub



    Public Overrides Sub TableRXData(ByVal Data(,) As String)
        If Data Is Nothing Then
            Exit Sub
        End If
        Try
            For y As Integer = 0 To UBound(Data, 2)
                Dim lvi As New ListViewItem
                Dim Q As Integer
                Q = Data(2, y)
                If Data(5, y) <> 1 Then
                    Q = Q * Data(5, y)
                End If
                Dim pack, singl, tot As Integer
                pack = 0
                singl = 0
                If Data(6, y) <> 0 And Data(7, y) <> "Y" Then
                    tot = Data(2, y)
                    pack = tot \ Data(6, y)
                    singl = tot Mod Data(6, y)
                End If
                With CtrlTable.Table
                    .Items.Add(lvi)
                    .Items(.Items.Count - 1).Text = Data(0, y)
                    .Items(.Items.Count - 1).SubItems.Add(Data(1, y))
                    .Items(.Items.Count - 1).SubItems.Add(Q)
                    .Items(.Items.Count - 1).SubItems.Add(Data(3, y))
                    .Items(.Items.Count - 1).SubItems.Add(Data(4, y))
                    .Items(.Items.Count - 1).SubItems.Add("N")
                    .Items(.Items.Count - 1).SubItems.Add(Data(6, y))
                    .Items(.Items.Count - 1).SubItems.Add(Data(7, y))
                    .Items(.Items.Count - 1).SubItems.Add(pack)
                    .Items(.Items.Count - 1).SubItems.Add(singl)
                End With
            Next
        Catch e As Exception
            MsgBox(e.Message)
        End Try
    End Sub


    Public Overrides Sub TableScan(ByVal Value As String)
        Try
            If System.Text.RegularExpressions.Regex.IsMatch(Value, ValidStr(tRegExValidation.tBarcode)) Or System.Text.RegularExpressions.Regex.IsMatch(Value, ValidStr(tRegExValidation.tBarcode2)) Then
                ' Scanning a barcode
                With CtrlForm
                    With .el(.ColNo("PART"))
                        .DataEntry.Text = Value
                        .ProcessEntry()
                    End With
                End With



                'SendType = tSendType.TableScan
                'InvokeData("SELECT PARTNAME, BARCODE FROM PART WHERE BARCODE = '" & Value & "'")
            ElseIf System.Text.RegularExpressions.Regex.IsMatch(Value, ValidStr(tRegExValidation.tLotNumber)) Then
                ' Scanning a Lot Number
                With CtrlForm

                    ' A warehouse must be selected
                    'If Not (.el(.ColNo("WHS")).Data.Length > 0) Then Throw New Exception("Please select a warehouse.")
                    SendType = tSendType.LOT
                    InvokeData("select distinct [PARTNAME],[SERIALNAME],[balance],[EXPIRYDATE],[WARHSNAME],[LOCNAME],[TYPE] from dbo.V_PICKLIST_PARTS where SERIALNAME = '" & Value & "'")
                End With


            Else
                Throw New Exception("Scanned item is not a part or a Lot")
            End If

        Catch EX As Exception
            MsgBox(String.Format("{0}", EX.Message))
        End Try


    End Sub




    Public Overrides Function VerifyForm() As Boolean
        If CtrlTable.Table.Items.Count = 0 Then
            Return True
        Else
            MsgBox("Not all items have been checked, please check them before continuing.")
            Return False
        End If
    End Function
End Class
