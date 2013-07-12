Public Class interfaceCheckRoute
    Inherits SFDCData.iForm

    Public Overrides Sub FormClose()
        FINALLIST.Clear()
        changelist.Clear()
    End Sub

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
    Dim trList As New List(Of WHSTRAN)
    Private curdate As Date
    Private tabDat As Boolean = True
    Private PickDate As Integer
    Private lot As String = ""
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
        Time = 9
        TableCheck = 10
        PickDate = 11
        Cust = 12
        LOTPick = 13
        None = 14
    End Enum

#End Region
#Region "Column Declerations"
    Public Overrides Sub FormSettings()
        With field 'using the tfield structure from the ctrlForm
            .Name = "ROUTE"
            .Title = "Route"
            .ValidExp = "^[0-9A-Za-z]+$"
            .SQLValidation = "SELECT ROUTENAME FROM V_PICKED_ROUTE where ROUTENAME = '%ME%'"
            .SQLList = "SELECT distinct ROUTENAME FROM V_PICKED_ROUTE WHERE ZROD_PICKTYPE <> 'S'"
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
            .Name = "PACKING_SLIP"
            .Title = "Pack Slip"
            .ValidExp = ValidStr(tRegExValidation.tPackingSlip)
            .SQLValidation = "select PSNO from V_PICK_MONITOR where PSNO ='%ME%' and ROUTENAME = '%ROUTE%'"
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
                "SELECT     PARTNAME " & _
                "FROM         V_PICKED_MONITOR " & _
                "WHERE     (PARTNAME = '%ME%')"
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
            Case tSendType.TableCheck
                If Data Is Nothing Then
                    tabDat = False
                End If

            Case tSendType.Time
                curdate = FormatDateTime(Data(0, 0), DateFormat.GeneralDate)

            Case tSendType.LOT
                With CtrlForm
                    With .el(.ColNo("PART"))
                        .DataEntry.Text = Data(0, 0)
                        .ProcessEntry()
                    End With
                End With

            Case tSendType.None
                If Data Is Nothing Then
                    'MsgBox("There are no unchecked picks for this route.")
                Else
                    'With CtrlForm
                    '    With .el(.ColNo("ROUTE"))
                    '        .Enabled = False
                    '        Me.Argument("HoldWARHS") = .Data & "PI"
                    '    End With
                    '    With .el(.ColNo("PICKID"))
                    '        .DataEntry.Text = Data(0, 0)
                    '        .ProcessEntry()
                    '    End With
                    'End With
                End If


            Case tSendType.PickDate
                Try
                    'With CtrlForm
                    '    With .el(.ColNo("FORDATE"))
                    '        Dim pdate As Date
                    '        pdate = FormatDateTime("1/1/1988", DateFormat.ShortDate)
                    '        Dim am As Integer = 0
                    '        am = Convert.ToInt32(Data(0, 0))
                    '        If am <> 0 Then
                    '            .DataEntry.Text = DateAdd(DateInterval.Minute, am, pdate)
                    '            Me.Argument("PickDate") = DateAdd(DateInterval.Minute, am, pdate)
                    '        End If
                    '        '.DataEntry.Text = Data(0, 0)
                    '    End With
                    '    With .el(.ColNo("PACKING_SLIP"))
                    '        .DataEntry.Text = Data(1, 0)
                    '    End With

                    'End With
                    PickDate = Data(0, 0)
                Catch ex As Exception
                    MsgBox(ex.ToString)
                End Try

                '   SendType = tSendType.TableCheck
                '   InvokeData( _
                '    CtrlTable.RecordsSQL = _
                '"select PARTNAME,PARTDES,dbo.REALQUANT(QUANT) as PICKED,'o' as CHECK_SUM, 'o' as CHECK_COUNT " & _
                '"from V_PICKED_MONITOR " & _
                '"WHERE ROUTENAME = '%ROUTE%' AND DUEDATE = " & PickDate & _
                '"order BY PARTNAME")

                'Set the query to load recordtype 2s

                CtrlTable.RecordsSQL = _
              "select PARTNAME,PARTDES,sum(dbo.REALQUANT(QUANT)) as PICKED,'o' as CHECK_SUM, 'o' as CHECK_COUNT,CONV,PACKING,NOTFIXEDCONV " & _
              "from V_PICKED_MONITOR " & _
              "WHERE ROUTENAME = '%ROUTE%' AND DUEDATE = " & PickDate & _
              " group by PARTNAME,PARTDES,CONV,PACKING,NOTFIXEDCONV " & _
              "order BY PARTNAME"
                If tabDat = True Then
                    With CtrlTable
                        .Table.Items.Clear()
                        .BeginLoadRS()
                        .Table.Focus()
                    End With
                    SendType = tSendType.Warhs
                    InvokeData( _
                    "select PARTNAME,PARTDES,dbo.REALQUANT(QUANT) as QUANT,ORDNAME,LINE,ORDI,CONV,SERIALNAME " & _
                      "from V_PICK_MONITOR2 " & _
                      "WHERE ROUTENAME = '%ROUTE%' AND DUEDATE = " & PickDate & _
                      "order BY PARTNAME")
                Else
                    MsgBox("There are no records to display for this Check")
                    Me.CloseMe()
                End If

            Case tSendType.Warhs
                'select ZROD_PICKLINE.PARTNAME,PART.PARTDES,dbo.REALQUANT(ZROD_PICKLINE.AMOUNTPICKED) as amount,serialname
                trList.Clear()

                If IsNothing(Data) = False Then

                    For y As Integer = 0 To UBound(Data, 2)
                        'SendType = tSendType.AmountCheck
                        'InvokeData("UPDATE ZROD_PICKS SET ISCHECKED = 'Y' WHERE PICK = " & Data(4, y))
                        Dim pics As New WHSTRAN(Data(0, y), Data(2, y), Data(3, y), Data(4, y), Data(5, y), Data(6, y), Data(7, y))
                        trList.Add(pics)
                    Next
                End If

            Case tSendType.LOTPick
                If IsNothing(Data) = False Then
                    lot = Data(0, 0)
                Else
                    lot = ""
                End If

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
                                        " ", " ", " ", 0, 0)
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
                                       " ", " ", " ", 0, 0)
                                        FINALLIST.Add(finlist)
                                        Dim err As New ErrorLog("Return", it.SubItems(1).Text, amount)
                                        changelist.Add(err)
                                        With CtrlTable
                                            .Table.Items.Remove(it)
                                        End With
                                    ElseIf amount < 0 Then
                                        amount *= -1

                                        MsgBox("You appear to have picked too few items for this part, please TAKE " & amount & " from stock")
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
                                       " ", " ", " ", 0, 0)
                                        FINALLIST.Add(finlist)
                                        Dim err As New ErrorLog("TAKE", it.SubItems(1).Text, amount)
                                        changelist.Add(err)
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
                                       " ", " ", " ", 0, 0)
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

                                SendType = tSendType.PickDate
                                InvokeData("SELECT DUEDATE FROM V_PICKED_ROUTE WHERE PICKDATE = '%FORDATE%' AND ROUTENAME = '%ROUTE%'")

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

                            Case "PART"
                                SendType = tSendType.Part

                                InvokeData("SELECT PARTNAME FROM V_PICKED_MONITOR WHERE PARTNAME = '%PART%'")
                            Case "CUSTOMER"

                                CtrlTable.RecordsSQL = _
             "select PARTNAME,PARTDES,dbo.REALQUANT(QUANT) as PICKED,'o' as CHECK_SUM, 'o' as CHECK_COUNT,CONV,PACKING,NOTFIXEDCONV " & _
             "from V_PICKED_MONITOR " & _
             "WHERE ROUTENAME = '%ROUTE%' AND DUEDATE = " & PickDate & _
             "order BY PARTNAME"

                                With CtrlTable
                                    .Table.Items.Clear()
                                    .BeginLoadRS()
                                    .Table.Focus()
                                End With
                                SendType = tSendType.Warhs
                                InvokeData( _
                                "select PARTNAME,PARTDES,QUANT,ORDNAME,LINE,ORDI,CONV, SERIALNAME" & _
                                  "from V_PICK_MONITOR2 " & _
                                  "WHERE ROUTENAME = '%ROUTE%' AND DUEDATE = " & PickDate & _
                                  "order BY PARTNAME")
                        End Select
                    End If
                Catch ex As Exception

                End Try
        End Select

    End Sub
    Public Overrides Sub ProcessForm()
        With CtrlForm

            With p
                .DebugFlag = False
                .Procedure = "ZSFDC_LOADZROD_CHECK"
                .Table = "ZSFDC_LOADZROD_CHECK"
                .RecordType1 = "PICKEDDATE,TOWARHSNAME,ISCHECKED,PACKSLIP,USERLOGIN,WARHSNAME"
                .RecordType2 = "PARTNAME,AMOUNTPICKED,ORDNAME,OLINE,SERIALNAME"
                .RecordTypes = "TEXT,TEXT,TEXT,TEXT,TEXT,TEXT,TEXT,,TEXT,TEXT,TEXT"
            End With
            SendType = tSendType.Time
            InvokeData("select getdate() as curdate")



            Dim startdate As Date = FormatDateTime("1/1/1988", DateFormat.GeneralDate)
            Dim t1() As String = { _
                                 DateDiff(DateInterval.Minute, startdate, curdate), _
                                CtrlForm.ItemValue("ROUTE"), _
                                "Y", _
                                CtrlForm.ItemValue("PACKING_SLIP"), _
                                UserName, _
                                CtrlForm.ItemValue("ROUTE") & "PI" _
                                }
            p.AddRecord(1) = t1
            

            '"select ZROD_PICKLINE.PARTNAME,PART.PARTDES,dbo.REALQUANT(ZROD_PICKLINE.AMOUNTPICKED) as amount,SERIALNAME,ZROD_PICKLINE.PICK " & _
            '"from ZROD_PICKLINE,PART,ZROD_PICKS " & _
            '"WHERE ZROD_PICKS.FORROUTE = '%ROUTE%' AND PART.PARTNAME = ZROD_PICKLINE.PARTNAME AND ZROD_PICKS.PICK = ZROD_PICKLINE.PICK AND ZROD_PICKS.ISCHECKED = 'N' AND ZROD_PICKS.FORDATE = (SELECT VALUE FROM LASTS WHERE NAME = 'PICK_DATE')")

            For y As Integer = 0 To (trList.Count - 1)
                
                

                Dim t2() As String = { _
                            trList(y).trPart, _
                            (trList(y).trAmount * 1000), _
                            trList(y).trORD, _
                            trList(y).trLine, _
                            trList(y).trSerial _
                            }
                'SendType = tSendType.None
                'InvokeData("UPDATE ORDERITEMS SET ZROD_IN_CHECK = 'Y', ZROD_CHECKED_ON = " & DateDiff(DateInterval.Minute, startdate, curdate) & ", ZROD_CHECKED_BY = '" & UserName & "' WHERE ORDI = " & trList(y).trOrdi)

                p.AddRecord(2) = t2

            Next




            ' Type 1 records

            FINALLIST.Clear()
            trList.Clear()
        End With
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



            f.ShowDialog()
            changelist.Clear()
        End If

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
                    pack = tot / Data(6, y)
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
                Throw New Exception("Scanned item is not a part.")
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
