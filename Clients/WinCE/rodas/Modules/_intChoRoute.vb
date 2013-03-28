Public Class interfaceChoRoute
    Inherits SFDCData.iForm

#Region "Initialisation and finalisation"

    Public Sub New(Optional ByRef App As Form = Nothing)

        'InitializeComponent()
        CallerApp = App
        NewArgument("PickDate", " ")
        'set the pickdate (used in the loading) and then sets up the menu buttons for the table.
        'we will be using the edit and the posting button
        CtrlTable.DisableButtons(True, False, True, True, False)
        CtrlTable.EnableToolbar(True, True, True, True, True)
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    Public Overrides Sub FormLoaded()
        MyBase.FormLoaded()
        CtrlTable.DisableButtons(True, False, True, True, False)
        CtrlTable.EnableToolbar(True, True, True, True, True)
    End Sub

#End Region

#Region "Column Declarations"
    'the formsettings control the layout of the form (the top part of the screen. Each field will be on a seperate line.
    'this is the level one of the loading
    Public Overrides Sub FormSettings()
        With field 'using the tfield structure from the ctrlForm
            .Name = "ROUTE"
            .Title = "Route"
            .ValidExp = "^[0-9A-Za-z]+$"
            .SQLValidation = "SELECT ROUTENAME FROM V_UNPICKED_ROUTE where ROUTENAME = '%ME%'"
            .SQLList = "Select ROUTENAME from dbo.V_UNPICKED_ROUTE"
            .AltEntry = ctrlText.tAltCtrlStyle.ctList 'ctKeyb 
            .ctrlEnabled = True
            .MandatoryOnPost = True
        End With
        CtrlForm.AddField(field)

        With field 'using the tfield structure from the ctrlForm
            .Name = "WHS"
            .Title = "WHouse"
            .ValidExp = ValidStr(tRegExValidation.tWarehouse)
            .SQLValidation = "select upper(WARHSNAME) from WAREHOUSES where upper(WARHSNAME) = upper('%ME%')"
            .SQLList = "Select DISTINCT WARHSNAME FROM V_PICKLIST_PARTS"
            .Data = ""
            .AltEntry = ctrlText.tAltCtrlStyle.ctList 'ctKeyb 
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
                "FROM         dbo.V_PICKLIST_PARTS " & _
                "WHERE     (PARTNAME = '%ME%')"
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
            .Name = "LOT"
            .Title = "Lot No"
            .ValidExp = ValidStr(tRegExValidation.tLotNumber)
            '.SQLValidation = _
            '    "SELECT DISTINCT dbo.V_PICKLIST_PARTS.SERIALNAME, dbo.WAREHOUSES.WARHSNAME, dbo.WAREHOUSES.LOCNAME " & _
            '    "FROM         dbo.WAREHOUSES RIGHT OUTER JOIN " & _
            '    "                      dbo.WARHSBAL ON dbo.WAREHOUSES.WARHS = dbo.WARHSBAL.WARHS RIGHT OUTER JOIN " & _
            '    "                      dbo.V_PICKLIST_PARTS ON dbo.WARHSBAL.PART = dbo.V_PICKLIST_PARTS.PART " & _
            '    "WHERE     (dbo.V_PICKLIST_PARTS.SERIALNAME = '%ME%') AND (dbo.WARHSBAL.WARHS <> 0) AND (dbo.WAREHOUSES.LOCNAME = N'0') AND  " & _
            '    "                      (dbo.WAREHOUSES.WARHSNAME = N'%WHS%')"
            .SQLValidation = "select Distinct SERIALNAME from dbo.V_PICKLIST_PARTS where SERIALNAME = '%ME%'"
            .SQLList = "select Distinct SERIALNAME from dbo.V_PICKLIST_PARTS where PARTNAME = '%PART%'"
            .Data = ""
            .AltEntry = ctrlText.tAltCtrlStyle.ctList 'ctKeyb 
            .ctrlEnabled = True
            .MandatoryOnPost = False
        End With
        CtrlForm.AddField(field)

        'With field 'using the tfield structure from the ctrlForm
        '    .Name = "BIN"
        '    .Title = "Bin"
        '    .ValidExp = ValidStr(tRegExValidation.tLocname)
        '    .SQLValidation = "SELECT LOCNAME FROM WAREHOUSES WHERE LOCNAME = '%ME%' AND WARHSNAME = '%WHS%'"
        '    .Data = ""
        '    .AltEntry = ctrlText.tAltCtrlStyle.ctNone 'ctKeyb 
        '    .ctrlEnabled = False
        '    .MandatoryOnPost = False
        'End With
        'CtrlForm.AddField(field)

        With field 'using the tfield structure from the ctrlForm
            .Name = "AVAILABLE"
            .Title = "Amount in Bin"
            .ValidExp = ValidStr(tRegExValidation.tNumeric)
            .SQLValidation = "select '%ME%'"
            .Data = ""
            .AltEntry = ctrlText.tAltCtrlStyle.ctNone 'ctKeyb 
            .ctrlEnabled = False
            .MandatoryOnPost = False
        End With
        CtrlForm.AddField(field)

        With field 'using the tfield structure from the ctrlForm
            .Name = "AMOUNT"
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
    'the table contains the lines of data. The structure used to contain said data is a ListView with each column being set out below.
    'these will be the line 2 of the loading
    Public Overrides Sub TableSettings()


        With col
            .Name = "_PART"
            .Title = "PART No"
            .initWidth = 25
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
            .Name = "_QUANT"
            .Title = "Quantity"
            .initWidth = 25
            .TextAlign = HorizontalAlignment.Left
            .AltEntry = ctrlText.tAltCtrlStyle.ctNone 'ctKeyb 
            .ValidExp = ValidStr(tRegExValidation.tNumeric)
            .SQLList = ""
            .SQLValidation = "SELECT '%ME%'"
            .DefaultFromCtrl = Nothing
            .ctrlEnabled = False
            .Mandatory = True
            .MandatoryOnPost = False
        End With
        CtrlTable.AddCol(col)

     
        With col
            .Name = "_WARHS"
            .Title = "DWarehouse"
            .initWidth = 0
            .TextAlign = HorizontalAlignment.Left
            .AltEntry = ctrlText.tAltCtrlStyle.ctNone 'ctKeyb 
            .ValidExp = ValidStr(tRegExValidation.tWarehouse)
            .SQLList = ""
            .SQLValidation = "SELECT '%ME%'"
            .DefaultFromCtrl = Nothing
            .ctrlEnabled = False
            .Mandatory = False
            .MandatoryOnPost = False
        End With
        CtrlTable.AddCol(col)

        With col
            .Name = "_BIN"
            .Title = "Description"
            .initWidth = 0
            .TextAlign = HorizontalAlignment.Left
            .AltEntry = ctrlText.tAltCtrlStyle.ctNone 'ctKeyb 
            .ValidExp = ValidStr(tRegExValidation.tLocname)
            .SQLList = ""
            .SQLValidation = "SELECT '%ME%'"
            .DefaultFromCtrl = Nothing
            .ctrlEnabled = False
            .Mandatory = False
            .MandatoryOnPost = False
        End With
        CtrlTable.AddCol(col)

        With col
            .Name = "_PICKED"
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
            .Name = "_PDESC"
            .Title = "Part Description"
            .initWidth = 45
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
    End Sub

#End Region

#Region "Variables"

    Public PItems As New List(Of PSLIPITEMS)
    Private pi_amount As Integer
    Public PickedList As New List(Of PickedItems)

    Public Property pick_amount() As Integer
        Get
            Return pi_amount
        End Get
        Set(ByVal value As Integer)
            pi_amount = value
        End Set
    End Property

#End Region

#Region "invokations"

    Dim SendType As tSendType = tSendType.Route
    'the sendtype is used to identify the type of data being handled by the EndInvokeData Sub
    Public Enum tSendType
        Route = 0
        PackSlip = 1
        Part = 2
        Warhs = 3
        Bin = 4
        Amount = 5
        AmountCheck = 6
        TableScan = 7
        PickDate = 8
        SCANW = 9
        SCANP = 10
        TableFill = 11
    End Enum
    'The endinvoke is called to handle the data sent by the calling query. The call syntax is InvokeData(<sql query>). this call must be preceded by a 
    'sendtype so that the data can be handled correctly.
    Public Overrides Sub EndInvokeData(ByVal Data(,) As String)
        Select Case SendType
            Case tSendType.PickDate
                'This is used to set the argument that stores the picking date from the database
                Me.Argument("PickDate") = Data(0, 0)
                'due to an unresolved issue I have had to reset the menu bar to ensure that the edit button is visible.
                CtrlTable.DisableButtons(True, False, True, True, False)
                CtrlTable.EnableToolbar(True, True, True, True, True)
            Case tSendType.TableScan
                'not currently used but can be utilised if ever needed
            Case tSendType.Route
                'this fires after a route has been chosen and validated
                Dim f As Boolean = False
                Do
                    If IsNothing(Data) Then Exit Do
                    If IsNothing(Data(0, 0)) Then Exit Do
                    ' There is a packing slip
                    With CtrlForm
                        With .el(.ColNo("PACKING_SLIP"))
                            .DataEntry.Text = Data(0, 0)
                            .ProcessEntry()
                        End With
                    End With
                    ' Set the query to load recordtype 2s
                    CtrlTable.RecordsSQL = _
                        "select PARTNAME,QUANT, '' as WARHS, '' as BIN, '0' as PICKED,PARTDES " & _
                        "from V_PICK_MONITOR " & _
                        "WHERE ROUTENAME = '%ROUTE%' " & _
                        "AND PSNO = '%PACKING_SLIP%'"
                    f = True

                Loop Until True

                If Not f Then

                    ' Set the query to load recordtype 2s
                    CtrlTable.RecordsSQL = _
                        "select PARTNAME,SUM(QUANT), '' as WARHS, '' as BIN, '0' as PICKED,PARTDES " & _
                        "from V_PICK_MONITOR " & _
                        "WHERE ROUTENAME = '%ROUTE%' GROUP BY PARTNAME,PARTDES"
                End If

                'once the query is set we now ensure that the table is empty and then fill it with data and give it focus.
                With CtrlTable
                    .Table.Items.Clear()
                    .BeginLoadRS()
                    .Table.Focus()
                End With
                'check for previous picks for this route/date combo. If they exist we will need to alter the downloaded data to reflect this
                'if the route is fully picked then we will error. TO FACILITATE THIS WE WILL CREATE A LIST OF ALREADY PICKED ITEMS AND
                'use it to alter the counts stored in the table
                SendType = tSendType.TableFill
                InvokeData("SELECT * FROM V_PICKEDITEMS WHERE FORROUTE = '%ROUTE%'")
                'this will fill the DATA structure with the results of the query. The view this is taken from utilises the same PickDate as the Route View

                If PickedList.Count <> 0 Then
                    'checks to see if the picklist HAS any records. If it does we need to iterate through the listview data table and update each matching part
                    Dim it As ListViewItem
                    For Each it In CtrlTable.Table.Items
                        Dim pa As PickedItems
                        For Each pa In PickedList
                            If it.SubItems(0).Text = pa.Part Then
                                'as the items match we update the picked column
                                it.SubItems(4).Text = pa.picked
                            End If
                        Next

                    Next
                    'after that we need to remove anylines that are fully picked
                    For Each it In CtrlTable.Table.Items
                        If it.SubItems(1).Text = it.SubItems(4).Text Then
                            'if the amount to pick = the amount picked then the line is done...kill it!!
                            CtrlTable.Table.Items.Remove(it)
                        End If
                    Next
                    'next we check to see if the table has any data left, if it doesnt then the picking is done and the user will be informed that there 
                    'is nothing left to do on this pick and the page will then close
                    If CtrlTable.Table.Items.Count = 0 Then
                        MsgBox("There are no lines left to pick, the pickings form will now close.")
                        Me.CloseMe()

                    End If
                End If


            Case tSendType.Part
                'firstly detect if scanned part is valid (done by settings in the table / form!!)
                'next check to see if that part is still on the list of parts to be picked
                Dim it As ListViewItem
                Dim fnd As Boolean = False


                For Each it In CtrlTable.Table.Items
                    If it.SubItems(0).Text = Data(0, 0) Then
                        fnd = True
                    End If
                Next
                If fnd = True Then
                    'so the part is valid and exists on the list of parts to be picked we now need to check if its a created part or a bought part
                    If Data(1, 0) = "R" Then
                        With CtrlForm
                            .el(.ColNo("LOT")).DataEntry.Text = "0"
                        End With
                    End If
                    If CtrlTable.Table.SelectedIndices.Count = 0 Then
                        Dim m As Integer
                        m = 1

                        Dim h As Integer
                        h = CtrlTable.Table.Items.Count
                        If h >= 0 Then 'check to see if there are any rows to select

                            For Each it In CtrlTable.Table.Items
                                If it.SubItems(0).Text = Data(0, 0) Then

                                    it.Selected = True


                                End If
                            Next
                        End If
                    Else
                        'if there is an already selected item we need to deselect it

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

                                    it.Selected = True

                                End If
                            Next
                        End If
                    End If
                    If Data(1, 0) = "R" Then
                        With CtrlForm
                            .el(.ColNo("LOT")).DataEntry.Text = "0"
                            .el(.ColNo("whs")).DataEntry.Text = Data(2, 0)
                        End With
                        Dim add As Integer
                        Dim num As New frmNumber
                        With num
                            .Text = "Box quantity."
                            .ShowDialog()
                            add = .number

                            .Dispose()
                        End With
                        With CtrlForm
                            With .el(.ColNo("AMOUNT"))
                                .DataEntry.Text = add
                                .ProcessEntry()
                            End With
                        End With
                    End If
                Else
                    'this part is no longer available to be picked so we need to do 2 things. First inform the user and then clear the part.
                    MsgBox("This part is not in the list of parts to pick")
                    With CtrlForm
                        .el(.ColNo("PART")).DataEntry.Text = ""
                        .el(.ColNo("PART")).ProcessEntry()
                    End With
                End If

            Case tSendType.Warhs

            Case tSendType.AmountCheck
                pick_amount = Val(Data(0, 0))

            Case tSendType.Amount
                Dim I As Integer
                If Data Is Nothing Then
                    Dim it As ListViewItem
                    For Each it In CtrlTable.Table.Items
                        If it.Selected = True Then
                            Dim g As String
                            g = it.SubItems(1).Text
                            Dim h As Integer

                            If IsNumeric(g) Then
                                h = Convert.ToInt16(g)
                            End If
                            If h < pi_amount Then
                                MessageBox.Show("You have picked too many items please check and try again.", "Error")
                                With CtrlForm.el(6)
                                    .DataEntry.Text = ""
                                    .CtrlEnabled = True
                                    .Enabled = True

                                End With
                            Else


                                it.SubItems(1).Text = Convert.ToInt16(g) - pi_amount
                                Dim tot_picked As Integer = pick_amount



                                If it.SubItems(4).Text <> "" Then
                                    tot_picked = Convert.ToInt16(it.SubItems(4).Text)
                                    tot_picked += pick_amount
                                    CtrlTable.Table.Items(it.Index).SubItems(4).Text = tot_picked

                                Else

                                    CtrlTable.Table.Items(it.Index).SubItems(4).Text = tot_picked
                                End If
                                CtrlTable.Table.Refresh()
                                CtrlTable.Update()

                                If Convert.ToInt16(it.SubItems(1).Text) = 0 Then
                                    CtrlTable.Table.Items.Remove(it)
                                End If
                                CtrlTable.DisableButtons(True, False, True, True, False)
                                CtrlTable.EnableToolbar(True, True, True, True, True)
                                Dim j As PSLIPITEMS
                                j = New PSLIPITEMS(0, _
                                    CtrlForm.el(0).Data, _
                                    CtrlForm.el(3).Data, _
                                    CtrlForm.el(2).Data, _
                                    CtrlForm.el(6).Data, " ", _
                                CtrlForm.el(4).Data, _
                                CtrlForm.el(1).Data, _
                                " ")
                                PItems.Add(j)
                                Dim x As Integer
                                x = 2
                                Do While x <= 7
                                    CtrlForm.el(x).Data = ""
                                    x += 1
                                Loop
                                'j = New PSLIPITEMS(

                            End If
                        End If


                    Next
                    CtrlTable.DisableButtons(True, False, True, True, False)
                    CtrlTable.EnableToolbar(True, True, True, True, True)
                Else
                    Select Case Data(0, 0)
                        Case 1
                            MessageBox.Show("There are not enough items in this lot / bin to allow a pick of this size please check and try again.", "Error")
                            With CtrlForm.el(6)
                                .DataEntry.Text = ""
                                '.CtrlEnabled = True
                                '.Enabled = True

                            End With
                        Case 0


                            SendType = tSendType.AmountCheck
                            InvokeData("select %AMOUNT%")
                            SendType = tSendType.Amount
                            Dim it As ListViewItem
                            For Each it In CtrlTable.Table.Items
                                If it.Selected = True Then



                                    Dim g As String
                                    g = it.SubItems(1).Text
                                    Dim h As Integer

                                    If IsNumeric(g) Then
                                        h = Convert.ToInt16(g)
                                    End If
                                    If h < pi_amount Then
                                        MessageBox.Show("You have picked too many items please check and try again.", "Error")
                                        With CtrlForm.el(6)
                                            .DataEntry.Text = ""
                                            .CtrlEnabled = True
                                            .Enabled = True

                                        End With
                                    Else


                                        it.SubItems(1).Text = Convert.ToInt16(g) - pi_amount
                                        Dim tot_picked As Integer = pick_amount



                                        If it.SubItems(4).Text <> "" Then
                                            tot_picked = Convert.ToInt16(it.SubItems(4).Text)
                                            tot_picked += pick_amount
                                            CtrlTable.Table.Items(it.Index).SubItems(4).Text = tot_picked

                                        Else

                                            CtrlTable.Table.Items(it.Index).SubItems(4).Text = tot_picked
                                        End If
                                        CtrlTable.Table.Refresh()
                                        CtrlTable.Update()

                                        If Convert.ToInt16(it.SubItems(1).Text) = 0 Then
                                            CtrlTable.Table.Items.Remove(it)
                                        End If
                                        Dim j As PSLIPITEMS
                                        j = New PSLIPITEMS(0, _
                                            CtrlForm.el(0).Data, _
                                            CtrlForm.el(3).Data, _
                                            CtrlForm.el(2).Data, _
                                            CtrlForm.el(6).Data, " ", _
                                        CtrlForm.el(4).Data, _
                                        CtrlForm.el(1).Data, _
                                        " ")
                                        PItems.Add(j)
                                        Dim x As Integer
                                        x = 2
                                        Do While x <= 7
                                            CtrlForm.el(x).Data = ""
                                            x += 1
                                        Loop
                                        'j = New PSLIPITEMS(

                                    End If
                                End If


                            Next


                    End Select
                End If

            Case tSendType.SCANW
                ' InvokeData("select distinct [PARTNAME],[SERIALNAME],[balance],[EXPIRYDATE],[WARHSNAME],[LOCNAME],[WARHS],[TYPE] from dbo.V_PICKLIST_PARTS where SERIALNAME = '" & Value & "'")
                Dim fnd As Boolean = False
                Dim it As ListViewItem
                For Each it In CtrlTable.Table.Items
                    If it.SubItems(0).Text = Data(0, 0) Then
                        fnd = True
                    End If
                Next
                If fnd = True Then
                    With CtrlForm
                        If .el(.ColNo("ROUTE")).DataEntry.Text = "" Then
                            If Data(6, 0) = "R" Then
                                MsgBox("This is not a manufactured part, please scan the items barcode")
                            Else
                                If .el(.ColNo("WHS")).DataEntry.Text <> Data(4, 0) Then
                                    Dim g As MsgBoxResult = MsgBox("This lot is not in the selected warehouse do you want to change the warehouse to match the lOT?", MsgBoxStyle.YesNo)
                                    If g = MsgBoxResult.Yes Then
                                        .el(.ColNo("WHS")).DataEntry.Text = Data(4, 0)
                                        .el(.ColNo("WHS")).ProcessEntry()

                                    End If
                                    .el(.ColNo("PART")).DataEntry.Text = Data(0, 0)
                                    .el(.ColNo("PART")).ProcessEntry()
                                    .el(.ColNo("LOT")).DataEntry.Text = Data(1, 0)
                                    .el(.ColNo("LOT")).ProcessEntry()
                                End If


                            End If
                        End If

                    End With
                Else
                    MsgBox("There are no parts with this lot number required in this picking.")
                    Exit Sub
                End If

            Case tSendType.TableFill
                PickedList.Clear()
                If IsNothing(Data) = False Then

                    For y As Integer = 0 To UBound(Data, 2)
                        Dim pics As New PickedItems(Data(0, y), Data(1, y), Data(2, y), Data(3, y))
                        PickedList.Add(pics)
                    Next
                End If

        End Select
    End Sub

#End Region

#Region "button Handlers"

    Public Overrides Sub AfterAdd(ByVal TableIndex As Integer)

    End Sub

    Public Overrides Sub AfterEdit(ByVal TableIndex As Integer)

    End Sub

    Public Overrides Sub BeginAdd()

    End Sub

    Public Overrides Sub BeginEdit()
        CtrlTable.EditInPlace = False
        CtrlTable.CancelEdit = True
        If PItems.Count <> 0 Then
            Dim f As New frmDisplay
            f.DataGrid1.DataSource = PItems
            f.Show()
        End If

    End Sub

#End Region

#Region "Form Processing"



    Public Overrides Sub ProcessEntry(ByVal ctrl As SFDCData.ctrlText, ByVal Valid As Boolean)
        Select Case Valid
            Case False
                Beep()
                MsgBox("Scanned Item failed validation")
            Case True
                Try
                    Dim i As String
                    i = ctrl.Data.ToString

                    If ctrl.Data <> "" Then
                        Select Case ctrl.Name
                            Case "ROUTE"
                                SendType = tSendType.Route
                                InvokeData("select dbo.FUNC_ROUTE_PS('%ROUTE%') as DOCNO")
                                SendType = tSendType.PickDate
                                InvokeData("SELECT VALUE FROM LASTS WHERE NAME = 'PICK_DATE'")

                            Case "PACKING_SLIP"
                                SendType = tSendType.PackSlip
                                CtrlTable.Table.Items.Clear()

                                CtrlTable.BeginLoadRS()
                                CtrlTable.Focus()
                                Select Case CtrlForm.el(1).Data.Length
                                    Case 0
                                        InvokeData("exec dbo.SP_SFDC_UPDATEITEMS '%ROUTE%'")
                                    Case Else
                                        InvokeData("exec dbo.SP_SFDC_UPDATEPACKSLIP '%ROUTE%','%PACKING_SLIP%'")

                                End Select

                            Case "LOT"


                                SendType = tSendType.Warhs
                                InvokeData("select distinct [PARTNAME],[SERIALNAME],[balance],[EXPIRYDATE],[WARHSNAME],[LOCNAME] from dbo.V_PICKLIST_PARTS where PARTNAME = '%PART%' and SERIALNAME = '%LOT%'")

                                Dim add As Integer
                                Dim num As New frmNumber
                                With num
                                    .Text = "Box quantity."
                                    .ShowDialog()
                                    add = .number

                                    .Dispose()
                                End With
                                With CtrlForm
                                    With .el(.ColNo("AMOUNT"))
                                        .DataEntry.Text = add
                                        .ProcessEntry()
                                    End With
                                End With
                               

                            Case "AMOUNT"
                                SendType = tSendType.Amount
                                pick_amount = i
                                Dim check As Integer
                                check = Convert.ToInt32(i)


                                Try
                                    If PItems.Count > 0 Then
                                        For Each a As PSLIPITEMS In PItems
                                            Dim lt, pa As String
                                            With CtrlForm
                                                lt = .el(4).Data.ToString
                                                pa = .el(2).Data.ToString
                                            End With


                                            If a.Lot = lt And a.PART = pa Then
                                                check += a.Quant
                                            End If
                                        Next
                                    End If


                                Catch ex As Exception
                                    MsgBox(ex.ToString)
                                End Try
                                'cHECK TO SEE IF ANY OF THIS ITEM / LOT COMBINATION HAVE BEEN PREVIOUSLY PICKED. IF SO WE NEED TO ADD THIS TO THE AMOUNT BEING CHECKED AGAINST TO SEE IF THERE ARE ENOUGH LEFT IN THE LOT AFTER THIS PICKING

                                InvokeData("select CAST(case when balance < " & check & " then 1 else 0 END AS INTEGER) AS PICKY from dbo.V_PICKLIST_PARTS where PARTNAME = '%PART%' AND SERIALNAME ='%LOT%'")

                            Case "PART"
                                SendType = tSendType.Part
                                InvokeData("select PARTNAME,TYPE,WARHSNAME from dbo.V_PICKLIST_PARTS WHERE PARTNAME = '%PART%'")
                        End Select
                    End If

                    ' *******************************************************************
                    ' *** Set which controls are enabled

                    With CtrlForm
                        .el(.ColNo("ROUTE")).CtrlEnabled = _
                            Not .el(.ColNo("ROUTE")).Data.Length > 0
                        .el(.ColNo("LOT")).CtrlEnabled = _
                            (.el(.ColNo("ROUTE")).Data).Length > 0 And _
                            (.el(.ColNo("PACKING_SLIP")).Data.Length > 0 And _
                            Not .el(.ColNo("LOT")).Data.Length) > 0
                    End With

                    ' *******************************************************************
                Catch

                End Try

        End Select
    End Sub

    Public Overrides Function VerifyForm() As Boolean
        Try
            Dim lvi As New ListViewItem
            With CtrlTable.Table
                For y As Integer = 0 To .Items.Count - 1
                    If Not .Items(y).SubItems(1).Text = .Items(y).SubItems(4).Text Then
                        Return (MsgBox("Not all items have been picked. Are you sure you wish to proceed?", MsgBoxStyle.OkCancel) = MsgBoxResult.Ok)
                    End If
                Next
                Return True
            End With

        Catch e As Exception
            MsgBox(e.Message)
        End Try
    End Function

    Public Overrides Sub ProcessForm()
        If Me.Argument("PickDate").ToString = " " Or CtrlForm.ItemValue("ROUTE") = "" Then
            MsgBox("Not all items have been picked. Are you sure you wish to proceed?", MsgBoxStyle.OkCancel)
        End If
        Try
            With p
                .DebugFlag = False
                .Procedure = "ZSFDC_LOADZROD_PICK"
                .Table = "ZSFDC_LOADZROD_PICK"
                .RecordType1 = "PICKEDDATE,FORDATE,FORROUTE,ISCHECKED,PACKSLIP,USERLOGIN"
                .RecordType2 = "PARTNAME,AMOUNTPICKED,WARHSNAME,LOCNAME,SERIALNAME"
                .RecordTypes = "TEXT,TEXT,TEXT,TEXT,TEXT,TEXT,TEXT,,TEXT,TEXT,TEXT"
            End With

        Catch e As Exception
            MsgBox(e.Message)
        End Try

        ' Type 1 records
        Dim startdate As Date = FormatDateTime("1/1/1988", DateFormat.LongDate)
        Dim t1() As String = { _
                            DateDiff(DateInterval.Minute, startdate, Now()), _
                            Me.Argument("PickDate"), _
                            CtrlForm.ItemValue("ROUTE"), _
                            "N", _
                            CtrlForm.ItemValue("PACKING_SLIP"), _
                            UserName _
                            }
        p.AddRecord(1) = t1

        For y As Integer = 0 To (PItems.Count - 1)
            Dim t2() As String = { _
                        PItems(y).PART, _
                        (PItems(y).Quant * 1000), _
                        PItems(y).WARHS, _
                        PItems(y).Bin, _
                        PItems(y).Lot _
                                }
            p.AddRecord(2) = t2
        Next
        PItems.Clear()

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
                    .Items(.Items.Count - 1).SubItems.Add(Data(5, y))
                End With
            Next
        Catch e As Exception
            MsgBox(e.Message)
        End Try
    End Sub

#End Region

#Region "Table Scanning"

    Public Overrides Sub TableScan(ByVal Value As String)

        Try
            If System.Text.RegularExpressions.Regex.IsMatch(Value, ValidStr(tRegExValidation.tBarcode)) Or System.Text.RegularExpressions.Regex.IsMatch(Value, ValidStr(tRegExValidation.tBarcode2)) Then
                ' Scanning a barcode
                With CtrlForm
                    If Not (.el(.ColNo("ROUTE")).Data.Length > 0) Then Throw New Exception("Please select a route.")
                    If Not (.el(.ColNo("WHS")).Data.Length > 0) Then Throw New Exception("Please select a warehouse.")
                    With .el(.ColNo("PART"))
                        .DataEntry.Text = Value
                        .ProcessEntry()
                    End With
                End With

            ElseIf System.Text.RegularExpressions.Regex.IsMatch(Value, ValidStr(tRegExValidation.tWarehouse)) Then
                ' Scanning a warehouse
                With CtrlForm
                    ' A route must be selected
                    If Not (.el(.ColNo("ROUTE")).Data.Length > 0) Then Throw New Exception("Please select a route.")
                    With .el(.ColNo("WHS"))
                        .DataEntry.Text = Value
                        .ProcessEntry()
                    End With
                End With

            ElseIf System.Text.RegularExpressions.Regex.IsMatch(Value, ValidStr(tRegExValidation.tLotNumber)) Then
                ' Scanning a Lot Number
                With CtrlForm
                    ' A warehouse must be selected
                    'If Not (.el(.ColNo("WHS")).Data.Length > 0) Then Throw New Exception("Please select a warehouse.")
                    SendType = tSendType.SCANW
                    InvokeData("select distinct [PARTNAME],[SERIALNAME],[balance],[EXPIRYDATE],[WARHSNAME],[LOCNAME],[TYPE] from dbo.V_PICKLIST_PARTS where SERIALNAME = '" & Value & "'")

                    ' Do we have a part number?
                    'If .el(.ColNo("PART")).Data.Length > 0 Then
                    '    ' Yes - pass the lot to the lot field for validation
                    '    With .el(.ColNo("LOT"))
                    '        .DataEntry.Text = Value
                    '        .ProcessEntry()
                    '    End With
                    'Else
                    '    ' Lot has been scanned before the part
                    '    With .el(.ColNo("LOT"))
                    '        .DataEntry.Text = Value
                    '        .ProcessEntry()
                    '    End With
                    'End If

                End With

                'SendType = tSendType.TableScan
                'InvokeData("SELECT PARTNAME, BARCODE FROM PART WHERE BARCODE = '" & Value & "'")



            End If

        Catch EX As Exception
            MsgBox(String.Format("{0}", EX.Message))
        End Try


    End Sub

#End Region

End Class
