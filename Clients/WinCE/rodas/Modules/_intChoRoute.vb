Imports System.Linq
Public Class lot_type
    Private part As String
    Private lot As String
    Private curr_tot As Integer
    Private max_tot As Integer
    Public Property lpart() As String
        Get
            Return part
        End Get
        Set(ByVal value As String)
            part = value
        End Set
    End Property
    Public Property LotRef() As String
        Get
            Return lot
        End Get
        Set(ByVal value As String)
            lot = value
        End Set
    End Property
    Public Property Ctot() As Integer
        Get
            Return curr_tot
        End Get
        Set(ByVal value As Integer)
            curr_tot = value
        End Set
    End Property
    Public Property MTot() As Integer
        Get
            Return max_tot
        End Get
        Set(ByVal value As Integer)
            max_tot = value
        End Set
    End Property
    Public Sub New(ByVal lr As String, ByVal ct As Integer, ByVal mt As Integer)
        LotRef = lr
        Ctot = ct
        MTot = mt
    End Sub
End Class
Public Class interfaceChoRoute
    Inherits SFDCData.iForm
    Private LotScan As Boolean = False

#Region "Table selection - non barcode"
    Private Sub meclick()
        If CtrlTable.Table.SelectedIndices.Count = 0 Then
            Exit Sub

        End If

        If LotScan = False Then
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
                        CtrlForm.el(1).DataEntry.Text = g
                        CtrlForm.el(1).Text = g
                        CtrlForm.el(1).Update()
                        CtrlForm.el(1).ProcessEntry()


                    End If
                    If CtrlTable.Table.Items.Count = 0 Then
                        Exit For
                    End If
                Next
            End If
        End If




    End Sub
#End Region
#Region "Initialisation and finalisation"

    Public Sub New(Optional ByRef App As Form = Nothing)

        'InitializeComponent()
        CallerApp = App
        NewArgument("PickDate", " ")
        NewArgument("RouteNo", 0)

        'set the pickdate (used in the loading) and then sets up the menu buttons for the table.
        'we will be using the edit and the posting button
        CtrlTable.DisableButtons(True, False, True, True, False)
        CtrlTable.EnableToolbar(True, True, True, True, True)
        AddHandler CtrlTable.Table.ItemActivate, AddressOf meclick

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
            .Name = "WHS"
            .Title = "WHouse"
            .ValidExp = ValidStr(tRegExValidation.tWarehouse)
            .SQLValidation = "select upper(WARHSNAME) from WAREHOUSES where upper(WARHSNAME) = upper('%ME%')"
            .SQLList = "" '"Select DISTINCT WARHSNAME FROM V_PICKLIST_PARTS where PARTNAME = '%PART%'"
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
            .Data = ""
            .AltEntry = ctrlText.tAltCtrlStyle.ctNone 'ctKeyb 
            .ctrlEnabled = True
            .MandatoryOnPost = False
        End With
        CtrlForm.AddField(field)
        '.SQLList = "select Distinct SERIALNAME from dbo.V_PICKLIST_PARTS where PARTNAME = '%PART%'"

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

        With field 'using the tfield structure from the ctrlForm
            .Name = "TYPE"
            .Title = "Type"
            .ValidExp = ValidStr(tRegExValidation.tPartType)
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
            .Name = "_PDESC"
            .Title = "Part Desc"
            .initWidth = 45
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
            .Title = "Bin"
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
            .SQLValidation = "SELECT '%ME%'"
            .DefaultFromCtrl = Nothing
            .ctrlEnabled = False
            .Mandatory = False
            .MandatoryOnPost = False
        End With
        CtrlTable.AddCol(col)

        With col
            .Name = "_TYPE"
            .Title = "Type"
            .initWidth = 25
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

#Region "Variables"
    Public BuySell As List(Of PSLIPITEMS)
    Public PItems As New List(Of PSLIPITEMS)
    Private pi_amount As Integer
    Public PickedList As New List(Of PickedItems)
    Public lotter As List(Of lot_type)

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
        PartW = 12
        Cust = 13
    End Enum
    'The endinvoke is called to handle the data sent by the calling query. The call syntax is InvokeData(<sql query>). this call must be preceded by a 
    'sendtype so that the data can be handled correctly.
    Public Overrides Sub EndInvokeData(ByVal Data(,) As String)
        Select Case SendType
            Case tSendType.Cust
                Me.Text = "Picking - " & Data(0, 0)
            Case tSendType.PickDate
                'This is used to set the argument that stores the picking date from the database
                Me.Argument("PickDate") = Data(0, 0)
                'due to an unresolved issue I have had to reset the menu bar to ensure that the edit button is visible.
                CtrlTable.DisableButtons(True, False, True, True, False)
                CtrlTable.EnableToolbar(True, True, True, True, True)


            Case tSendType.PackSlip
                If Me.Argument("RouteNo") = "0" Then
                    Me.Argument("RouteNo") = Data(0, 0)
                End If


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
                        "select PARTNAME,PARTDES,QUANT, '' as WARHS, '' as BIN, '0' as PICKED,TYPE " & _
                        "from V_PICK_MONITOR " & _
                        "WHERE ROUTENAME = '%ROUTE%' " & _
                        "AND PSNO = '%PACKING_SLIP%' " _
                        & "ORDER by ZROD_PICKORDER"
                    f = True

                Loop Until True

                If Not f Then

                    ' Set the query to load recordtype 2s
                    CtrlTable.RecordsSQL = _
                        "select PARTNAME,PARTDES,SUM(QUANT) as Quant, '' as WARHS, '' as BIN, '0' as PICKED,TYPE " & _
                        "from V_PICK_MONITOR " & _
                        "WHERE ROUTENAME = '%ROUTE%' GROUP BY PARTNAME,PARTDES,ZROD_PICKORDER,TYPE" _
                        & " ORDER by ZROD_PICKORDER"

                End If

                'once the query is set we now ensure that the table is empty and then fill it with data and give it focus.
                With CtrlTable
                    .Table.Items.Clear()
                    .BeginLoadRS()
                    .Table.Focus()
                End With

                If CtrlTable.Table.Items.Count = 0 Then
                    MsgBox("There are no lines left to pick, the pickings form will now close.")
                    Me.CloseMe()
                End If
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
                                it.SubItems(5).Text = pa.picked
                                'we now need to update the REQUIRED column to reflect this
                                Dim g As Integer
                                g = Convert.ToInt16(it.SubItems(2).Text)
                                g = g - pa.picked
                                it.SubItems(2).Text = g
                            End If
                        Next

                    Next
                    it = Nothing

                    'after that we need to remove anylines that are fully picked
                    Dim itemcount As Integer
                    Dim StillLive As Boolean = False

                    itemcount = CtrlTable.Table.Items.Count
                    For d As Integer = 0 To itemcount - 1
                        If CtrlTable.Table.Items(d).SubItems(2).Text <> 0 Then
                            StillLive = True
                        End If
                    Next

                    'next we check to see if the table has any data left, if it doesnt then the picking is done and the user will be informed that there 
                    'is nothing left to do on this pick and the page will then close
                    If StillLive = False Then
                        MsgBox("There are no lines left to pick, the pickings form will now close.")
                        Me.CloseMe()

                    End If
                End If


            Case tSendType.PartW
                'here we will deal with the warehouse. Firstup we check to see how many warehouses are available
                'if its just one and its already selected we will do nothing, if its not already selected we will select it
                If LotScan = False Then
                    Dim CHECK As Integer
                    CHECK = UBound(Data, 2)
                    If UBound(Data, 2) = 0 Then 'we have only 1 warehouse

                        With CtrlForm
                            If .el(.ColNo("WHS")).DataEntry.Text <> Data(0, 0) Then 'only if its not equal do we want to change it
                                .el(.ColNo("WHS")).DataEntry.Text = Data(0, 0)
                                .el(.ColNo("WHS")).ProcessEntry()
                            End If
                        End With
                    Else 'we definetly have more than one!
                        'So we will force the user to choose which warehouse they are in
                        'we will check to see if there is one already chosen
                        Dim f As New frmDrop
                        For fg As Integer = 0 To CHECK - 1
                            f.ComboBox1.Items.Add(Data(0, fg))
                        Next
                        f.ComboBox1.Text = f.ComboBox1.Items(0)
                        f.ShowDialog()
                        If f.DialogResult = Windows.Forms.DialogResult.OK Then

                            With CtrlForm
                                .el(.ColNo("WHS")).DataEntry.Text = f.ComboBox1.Text
                                .el(.ColNo("WHS")).Text = f.ComboBox1.Text
                                .el(.ColNo("WHS")).ProcessEntry()
                            End With
                            CtrlTable.Focus()

                        End If
                    End If
                End If
                LotScan = False



            Case tSendType.Part
                'firstly detect if scanned part is valid (done by settings in the table / form!!)
                'next check to see if that part is still on the list of parts to be picked and check on its type
                'if its a manufactured part and there is no lot selected then we must error
                Dim it As ListViewItem
                Dim fnd As Boolean = False
                Dim err As Boolean = False

                For Each it In CtrlTable.Table.Items
                    If it.SubItems(0).Text = Data(0, 0) Then
                        fnd = True
                    End If
                Next
                If fnd = True Then
                    'so the part is valid and exists on the list of parts to be picked we now need to check if its a created part or a bought part
                    With CtrlForm
                        .el(7).DataEntry.Text = Data(1, 0)
                        .el(7).Data = Data(1, 0)
                    End With
                    If Data(1, 0) = "R" Then
                        With CtrlForm
                            'If lotter.Count = 0 Then
                            '    SendType = tSendType.SCANP
                            '    InvokeData("select SERIALNAME,BALANCE from V_PICKLIST_PARTS WHERE PARTNAME = '%PART% ORDER BY EXPIRYDATE")


                            'End If
                            .el(4).DataEntry.Text = "0"
                        End With
                    Else
                        If CtrlForm.el(4).Data = "" Then
                            MsgBox("This is a manufactured part you must select a LOT")
                            err = True
                        End If
                    End If
                    If err = False Then
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
                                .el(.ColNo("WHS")).DataEntry.Text = Data(2, 0)
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
                        Else
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
                If Data(0, 0) = "R" Then
                    Dim ch As String
                    With CtrlForm
                        With .el(.ColNo("LOT"))
                            ch = .DataEntry.Text

                        End With
                    End With

                    If ch = "" Then Throw New Exception("Please select a PART.")

                End If


            Case tSendType.AmountCheck
                pick_amount = Val(Data(0, 0))

            Case tSendType.Amount
                Dim I As Integer
                If Data Is Nothing Then
                    Dim it As ListViewItem
                    For Each it In CtrlTable.Table.Items
                        If it.Selected = True Then
                            Dim g As String
                            g = it.SubItems(2).Text
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


                                it.SubItems(2).Text = Convert.ToInt16(g) - pi_amount
                                Dim tot_picked As Integer = pick_amount



                                If it.SubItems(5).Text <> "" Then
                                    tot_picked = Convert.ToInt16(it.SubItems(5).Text)
                                    tot_picked += pick_amount
                                    CtrlTable.Table.Items(it.Index).SubItems(4).Text = tot_picked

                                Else

                                    CtrlTable.Table.Items(it.Index).SubItems(4).Text = tot_picked
                                End If
                                'If CtrlTable.Table.Items(6).Text = "R" Then
                                '    SendType = tSendType.SCANP
                                '    InvokeData("select SERIALNAME,BALANCE from V_PICKLIST_PARTS WHERE PARTNAME = '%PART% ORDER BY EXPIRYDATE")

                                'End If


                                CtrlTable.Table.Refresh()
                                CtrlTable.Update()

                                If Convert.ToInt16(it.SubItems(2).Text) = 0 Then
                                    CtrlTable.Table.Items.Remove(it)
                                End If
                                CtrlTable.DisableButtons(True, False, True, True, False)
                                CtrlTable.EnableToolbar(True, True, True, True, True)
                                Dim j As PSLIPITEMS
                                j = New PSLIPITEMS(0, _
                                    CtrlForm.el(0).Data, _
                                    CtrlForm.el(3).Data, _
                                    CtrlForm.el(1).Data, _
                                    CtrlForm.el(6).Data, " ", _
                                CtrlForm.el(4).Data, _
                                CtrlForm.el(2).Data, _
                                " ", CtrlForm.el(7).Data)
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
                            Dim it As ListViewItem
                            For Each it In CtrlTable.Table.Items
                                If it.Selected = True Then
                                    Dim g As String
                                    g = it.SubItems(2).Text
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
                                        MessageBox.Show("There are not enough items in this lot / bin to allow a pick of this size please check and try again.", "Error")
                                        With CtrlForm.el(6)
                                            .DataEntry.Text = ""
                                            '.CtrlEnabled = True
                                            '.Enabled = True

                                        End With
                                    End If
                                End If

                            Next

                        Case 0


                            SendType = tSendType.AmountCheck
                            InvokeData("select %AMOUNT%")
                            SendType = tSendType.Amount
                            Dim it As ListViewItem
                            For Each it In CtrlTable.Table.Items
                                If it.Selected = True Then



                                    Dim g As String
                                    g = it.SubItems(2).Text
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


                                        it.SubItems(2).Text = Convert.ToInt16(g) - pi_amount
                                        Dim tot_picked As Integer = pick_amount



                                        If it.SubItems(5).Text <> "" Then
                                            tot_picked = Convert.ToInt16(it.SubItems(5).Text)
                                            tot_picked += pick_amount
                                            CtrlTable.Table.Items(it.Index).SubItems(4).Text = tot_picked

                                        Else

                                            CtrlTable.Table.Items(it.Index).SubItems(4).Text = tot_picked
                                        End If
                                        CtrlTable.Table.Refresh()
                                        CtrlTable.Update()

                                        If Convert.ToInt16(it.SubItems(2).Text) = 0 Then
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
                                        " ", CtrlForm.el(7).Data)
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
                                    LotScan = True
                                    Dim g As MsgBoxResult = MsgBox("This lot is not in the selected warehouse do you want to change the warehouse to match the lOT?", MsgBoxStyle.YesNo)
                                    If g = MsgBoxResult.Yes Then
                                        .el(.ColNo("WHS")).DataEntry.Text = Data(4, 0)
                                        .el(.ColNo("WHS")).ProcessEntry()

                                    End If
                                    .el(.ColNo("LOT")).DataEntry.Text = Data(1, 0)
                                    .el(.ColNo("LOT")).ProcessEntry()
                                    .el(.ColNo("PART")).DataEntry.Text = Data(0, 0)
                                    .el(.ColNo("PART")).ProcessEntry()

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
            Case tSendType.SCANP
                'For Each a As PSLIPITEMS In PItems
                '    If a.Type = "R" Then
                '        For i As Integer = 0 To UBound(Data, 2)
                '            If Data(1, i) = a.PART And Data(3, i) <> 0 Then
                '                If a.Quant < Data(3, i) Then
                '                    a.Lot
                '                End If
                '            End If
                '        Next
                '    End If
                'Next
        End Select
        CtrlTable.Focus()
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
                                'This procedure finds if there are packing slips associated with the chosen route annd displays the first available one, then it fills in the data table and the date argument

                            Case "PACKING_SLIP"
                                SendType = tSendType.PackSlip
                                
                                Select Case CtrlForm.el(3).Data.Length
                                    Case 0
                                        InvokeData("exec dbo.SP_SFDC_UPDATEITEMS '%ROUTE%'")
                                    Case Else
                                        InvokeData("select ROUTE from ZROD_ROUTES WHERE ROUTENAME = '%ROUTE%'")
                                        InvokeData("exec dbo.SP_SFDC_UPDATEPACKSLIP '" & Me.Argument("RouteNo") & "','%PACKING_SLIP%'")
                                        SendType = tSendType.Cust
                                        InvokeData("select CUSTOMERS.CUSTDES from DOCUMENTS,CUSTOMERS WHERE CUSTOMERS.CUST = DOCUMENTS.CUST AND DOCUMENTS.DOCNO = '%PACKING_SLIP%'")
                                End Select


                            Case "LOT"
                                'SendType = tSendType.Warhs
                                'InvokeData("select distinct [PARTNAME],[SERIALNAME],[balance],[EXPIRYDATE],[WARHSNAME],[LOCNAME] from dbo.V_PICKLIST_PARTS where PARTNAME = '%PART%' and SERIALNAME = '%LOT%'")
                                'Dim add As Integer
                                'Dim num As New frmNumber
                                'With num
                                '    .Text = "Box quantity."
                                '    .ShowDialog()
                                '    add = .number

                                '    .Dispose()
                                'End With
                                'With CtrlForm
                                '    With .el(.ColNo("AMOUNT"))
                                '        .DataEntry.Text = add
                                '        .ProcessEntry()
                                '    End With
                                'End With


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
                                SendType = tSendType.PartW
                                InvokeData("select DISTINCT WARHSNAME from dbo.V_PICKLIST_PARTS WHERE PARTNAME = '%PART%'")

                                SendType = tSendType.Part
                                InvokeData("select PARTNAME,TYPE,WARHSNAME from dbo.V_PICKLIST_PARTS WHERE PARTNAME = '%PART%'")
                            Case "TYPE"
                                SendType = tSendType.TableScan

                            Case "WHS"
                                SendType = tSendType.Warhs
                                InvokeData("select TYPE from dbo.V_PRICKLIST_PARTS WHERE PARTNAME = '%PART%' AND WARHSNAME = '%WHS%'")


                                CtrlTable.Focus()
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
        SendType = tSendType.SCANP
        InvokeData("select SERIALNAME,BALANCE from V_PICKLIST_PARTS WHERE PARTNAME = '%PART% ORDER BY EXPIRYDATE")


        'whs = ((From it In PItems Order By it.WARHS Select it)).ToList
        'For Each c As PSLIPITEMS In whs
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
                    .Items(.Items.Count - 1).SubItems.Add(Data(6, y))
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
                    'If Not (.el(.ColNo("WHS")).Data.Length > 0) Then Throw New Exception("Please select a warehouse.")
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
                    If Not (.el(.ColNo("PART")).Data.Length > 0) Then Throw New Exception("Please select a PART.")
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
                    LotScan = True
                    SendType = tSendType.SCANW
                    InvokeData("select distinct [PARTNAME],[SERIALNAME],[balance],[EXPIRYDATE],[WARHSNAME],[LOCNAME],[TYPE] from dbo.V_PICKLIST_PARTS where SERIALNAME = '" & Value & "'")


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
