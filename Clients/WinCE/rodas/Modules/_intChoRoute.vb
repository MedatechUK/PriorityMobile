Imports System.Linq
Imports System.Collections.Generic


Public Class interfaceChoRoute
    Inherits SFDCData.iForm
    
#Region "Variables"
    Private startdate As Date = FormatDateTime("1/1/1988", DateFormat.LongDate)
    Private curdate As Integer = DateDiff(DateInterval.Minute, startdate, Today)
    Public PickedItems As New List(Of PSLIPITEMS)
    Private pi_amount As Integer
    Private LotScan As Boolean = False
    Private expected As Integer = 0
    Private LotList As New List(Of LotS)
    Private cust As String = ""
    Private PartS As String = ""
    Private RouteID As Integer = 0
    Private CheckItems As New List(Of PSLIPITEMS)
    Private custid As Integer
    Private CustName As String = ""
    Private oline As Integer = 0
    Private pType As String = ""
    Private NOSCAN As Boolean = False
    Private bcodetype As String = "s"
    Private OrderList As New List(Of PSLIPITEMS)
    Private CUSTMINPICK As Integer = 0
    Private Pick_Type As String = ""

    Private pick_amount As Integer = 0

#End Region

#Region "Column Declarations"
    'the formsettings control the layout of the form (the top part of the screen. Each field will be on a seperate line.
    'this is the level one of the loading
    Public Overrides Sub FormSettings()
        '0
        With field 'using the tfield structure from the ctrlForm
            .Name = "ROUTE"
            .Title = "Route"
            .ValidExp = "^[0-9A-Za-z]+$"
            .SQLValidation = "SELECT DISTINCT ROUTENAME FROM V_UNPICKED_ROUTE where ROUTENAME = '%ME%'"
            .SQLList = "Select distinct ROUTENAME from dbo.V_UNPICKED_ROUTE"
            .AltEntry = SFDCData.ctrlText.tAltCtrlStyle.ctList 'ctKeyb 
            .ctrlEnabled = True
            .MandatoryOnPost = True
        End With
        CtrlForm.AddField(field)

        '1
        With field 'using the tfield structure from the ctrlForm
            .Name = "PDATE"
            .Title = "Date"
            .ValidExp = ".*"
            .SQLValidation = "SELECT '%ME%'"
            .SQLList = "SELECT PICKDATE FROM V_UNPICKED_ROUTE where ROUTENAME = '%ROUTE%' ORDER BY DUEDATE ASC"
            .AltEntry = SFDCData.ctrlText.tAltCtrlStyle.ctList 'ctKeyb 
            .ctrlEnabled = True
            .MandatoryOnPost = False
            .Data = ""
        End With
        CtrlForm.AddField(field)

        '2
        With field 'using the tfield structure from the ctrlForm
            .Name = "FAMILY"
            .Title = "Family"
            .ValidExp = ".*"
            .SQLValidation = "SELECT '%ME%'"
            .SQLList = "SELECT 'All' AS FTNAME UNION ALL SELECT DISTINCT FTNAME FROM V_PICK_MONITOR where ROUTENAME = '%ROUTE%' AND PICKDATE = '%PDATE%'"
            .AltEntry = SFDCData.ctrlText.tAltCtrlStyle.ctList 'ctKeyb 
            .ctrlEnabled = True
            .MandatoryOnPost = False
            .Data = ""
        End With
        CtrlForm.AddField(field)

        '3
        With field 'using the tfield structure from the ctrlForm
            .Name = "PART"
            .Title = "Part"
            .ValidExp = ValidStr(tRegExValidation.tString)
            '.SQLValidation = "Select '%ME%'"
            .SQLValidation = _
                "SELECT     PARTNAME " & _
                "FROM         dbo.V_PICK_MONITOR " & _
                "WHERE     PARTNAME = '%ME%' AND ROUTENAME = '%ROUTE%'"
            .Data = ""
            .AltEntry = ctrlText.tAltCtrlStyle.ctNone 'ctKeyb 
            .ctrlEnabled = False
            .MandatoryOnPost = False
        End With
        CtrlForm.AddField(field)

        '4
        With field 'using the tfield structure from the ctrlForm
            .Name = "WHS"
            .Title = "WHouse"
            .ValidExp = ValidStr(tRegExValidation.tWarehouse)
            .SQLValidation = "select (WARHSNAME) from WAREHOUSES where (WARHSNAME) =('%ME%')"
            .SQLList = "" '"Select DISTINCT WARHSNAME FROM V_PICKLIST_PARTS where PARTNAME = '%PART%'"
            .Data = ""
            .AltEntry = ctrlText.tAltCtrlStyle.ctNone 'ctKeyb 
            .ctrlEnabled = False
            .MandatoryOnPost = False
        End With
        CtrlForm.AddField(field)

        '5   .SQLValidation = "select CUSTNAME from V_PICK_MONITOR where CUSTNAME = '%ME%'"
        With field 'using the tfield structure from the ctrlForm
            .Name = "PACKING_SLIP"
            .Title = "Cust"
            .ValidExp = ".+"
            .SQLValidation = "SELECT '%ME%'"
            .SQLList = "select distinct CUSTDES FROM V_CUSTFORROUTE WHERE ROUTENAME = '%ROUTE%' AND PICKDATE = '%PDATE%'"
            .Data = ""
            .AltEntry = ctrlText.tAltCtrlStyle.ctList 'ctKeyb 
            .ctrlEnabled = True
            .MandatoryOnPost = False
        End With
        CtrlForm.AddField(field)

        '6
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

        '7
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

        '8
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

        '9
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

        '10
        With field 'using the tfield structure from the ctrlForm
            .Name = "BIN"
            .Title = "Type"
            .ValidExp = ValidStr(tRegExValidation.tString)
            .SQLValidation = "SELECT '%ME%'"
            .Data = ""
            .AltEntry = ctrlText.tAltCtrlStyle.ctNone
            'ctKeyb 
            .ctrlEnabled = False
            .MandatoryOnPost = False
        End With
        CtrlForm.AddField(field)

        '11
        With field 'using the tfield structure from the ctrlForm
            .Name = "EXPIRY"
            .Title = "Expiry"
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
        'table listing
        '****************************************************************
        '* 0 --- Part No
        '* 1 --- Part Desc
        '* 2 --- Quantity
        '* 3 --- DWarehouse
        '* 4 --- Bin
        '* 5 --- Picked
        '* 6 --- Type
        '* 7 --- ORDI
        '* 8 --- OrdName
        '* 9 --- OrdLine
        '* 10 -- Available
        '* 11 -- Balance
        '* 12 -- Lot
        '* 13 -- Expiry
        '* 14 -- Conv
        '* 15 -- Packing Amount
        '* 16 -- Converter Flag
        '* 17 -- Packs
        '* 18 -- Spare qty
        '****************************************************************
        '0
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
        '1
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
        '2
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

        '3
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
        '4
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
        '5
        With col
            .Name = "_PICKED"
            .Title = "Picked"
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
        '6
        With col
            .Name = "_TYPE"
            .Title = "Type"
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

        '7
        With col
            .Name = "_ORDI"
            .Title = "ORDI"
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


        '8:
        With col
            .Name = "_ORDNAME"
            .Title = "OrdName"
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

        '9:
        With col
            .Name = "_OLINE"
            .Title = "OrdLine"
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

        '10 - Part Available
        With col
            .Name = "_AVAILABLE"
            .Title = "Available"
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


        '11 - Part Available
        With col
            .Name = "_balance"
            .Title = "Balance"
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

        '12 - Part Available
        With col
            .Name = "_LOT"
            .Title = "Lot"
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

        '13 - Part Available
        With col
            .Name = "_EXPIRY"
            .Title = "Expiry"
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


        '14 - CONVERTER
        With col
            .Name = "_CONV"
            .Title = "CONVERTER"
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
        PickedItems.Clear()
        OrderList.Clear()

    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    Public Overrides Sub FormLoaded()
        MyBase.FormLoaded()
        CtrlTable.DisableButtons(True, False, True, True, False)
        CtrlTable.EnableToolbar(True, True, True, True, True)
        Me.Text = ""

    End Sub
    
#End Region

#Region "Table selection - non barcode"
    Private Sub meclick()
        If CtrlTable.Table.SelectedIndices.Count = 0 Then
            Exit Sub

        End If

        If LotScan = False Then
            With CtrlForm
                If Not (.el(.ColNo("ROUTE")).Data.Length > 0) Then MsgBox("Please select a route.")
                'If Not (.el(.ColNo("PART")).Data.Length > 0) Then MsgBox("Please select a PART.")

                'If Not (.el(.ColNo("WHS")).Data.Length > 0) Then MsgBox("Please select a warehouse.")
            End With

            Dim m As Integer
            m = 1
            NOSCAN = True

            Dim h As Integer
            h = CtrlTable.Table.Items.Count
            If h >= 0 Then 'check to see if there are any rows to select
                Dim it As ListViewItem
                For Each it In CtrlTable.Table.Items
                    If it.Selected = True Then
                        If it.SubItems(10).Text = "N" Then
                            MsgBox("There is no stock available to pick for this item")
                        Else
                            Dim g As String
                            g = it.SubItems(0).Text
                            CtrlForm.el(3).DataEntry.Text = g
                            CtrlForm.el(3).Text = g
                            CtrlForm.el(3).Update()
                            CtrlForm.el(3).ProcessEntry()

                        End If



                        Exit Sub
                    End If
                    If CtrlTable.Table.Items.Count = 0 Then
                        Exit For
                    End If
                Next
            End If
        End If
        LotScan = False



    End Sub
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
        LotFill = 14
        Time = 15
        LotFillNew = 16
        PartName = 17
        RouteID = 18
        Cust2 = 19
        None = 20
        GetParts = 21
        CustName = 22
        getOname = 23
        getminpick = 24
        getPickType = 25
    End Enum
    'The endinvoke is called to handle the data sent by the calling query. The call syntax is InvokeData(<sql query>). this call must be preceded by a 
    'sendtype so that the data can be handled correctly.
    Public Overrides Sub EndInvokeData(ByVal Data(,) As String)
        Select Case SendType

            Case tSendType.None
                If Data Is Nothing Then

                Else
                    Dim m As String
                    m = Data(0, 0)
                End If


            Case tSendType.GetParts
                If Data Is Nothing Then

                Else
                    Try
                        For y As Integer = 0 To UBound(Data, 2)
                            If Data(5, y) <> 1 Then
                                Data(2, y) *= Data(5, y)
                            End If
                            Dim d As New PSLIPITEMS(Data(0, y), "", "", Data(1, y), Data(2, y), "", "", "", "", "", Data(3, y), Data(4, y), 0, Data(5, y), CtrlForm.el(5).Data)
                            OrderList.Add(d)
                        Next
                    Catch e As Exception
                        MsgBox(e.Message)
                    End Try
                End If

            Case tSendType.PickDate
                'This is used to set the argument that stores the picking date from the database
                Me.Argument("PickDate") = Data(0, 0)
                RouteID = Data(1, 0)

                'due to an unresolved issue I have had to reset the menu bar to ensure that the edit button is visible.
                CtrlTable.DisableButtons(True, False, True, True, False)
                CtrlTable.EnableToolbar(True, True, True, True, True)

            Case tSendType.getPickType
                Pick_Type = Data(0, 0)

            Case tSendType.Route
                'this fires after a route has been chosen and validated

                Dim f As Boolean = False
                With CtrlForm
                    With .el(.ColNo("WHS"))
                        .DataEntry.Text = "Main"
                        .Data = "Main"
                    End With
                End With
                Do
                    If IsNothing(Data) Then
                        Me.Text = "Picking"
                        With CtrlForm.el(5)

                            .DataEntry.Text = ""
                            cust = ""
                            .Data = ""
                            .ProcessEntry()
                            Exit Do
                        End With

                    Else
                        With CtrlForm.el(5)
                            .DataEntry.Text = Data(1, 0)
                            .Data = Data(1, 0)

                        End With
                        cust = Data(1, 0)
                        CustName = Data(2, 0)
                        Me.Text = CustName

                        custid = Data(0, 0)
                        CtrlTable.Focus()

                    End If

                    Dim dd As Integer = Convert.ToInt32(Argument("pickdate"))
                    ' Set the query to load recordtype 2s
                    Select Case CtrlForm.el(2).Data
                        Case "All"
                            CtrlTable.RecordsSQL = _
                        "select PARTNAME,PARTDES,SUM(ZROD_TOBEPICKED) as Quant, '' as WARHS, '' as BIN, '0' as PICKED, '' as TYPE, '0' as ORDI,  '' as ORDNAME, '' as  OLINE,CASE when PARTNAME in (SELECT PARTNAME FROM V_PICKLIST_PARTS) THEN 'A' else 'N' end as AVAILABLE,0 AS balance,'' AS SERIALNAME, 0 AS EXPIRYDATE,CONV,PACKING,NOTFIXEDCONV " & _
                        "from V_PICK_MONITOR " & _
                        "WHERE ROUTENAME = '" & CtrlForm.el(0).Data & "'" & _
                        " AND CUSTNAME = '" & cust & "' AND DUEDATE = " & Argument("PickDate") & _
                       " GROUP BY PARTNAME,PARTDES,TYPE,CONV,ZROD_PICKORDER,PACKING,NOTFIXEDCONV" & _
                    " ORDER by ZROD_PICKORDER"

                            '*************************************************************************************************
                            SendType = tSendType.GetParts
                            InvokeData("select ORDI,PARTNAME,QUANT,ORDNAME,LINE,CONV " & _
                                "from V_PICK_MONITOR " & _
                                "WHERE ROUTENAME = '%ROUTE%' AND DUEDATE = " & Argument("PickDate") & " AND CUSTNAME = '" & cust & "'")
                            'InvokeData("select CUST,CUSTNAME,CUSTDES FROM V_CUSTFORROUTE WHERE ROUTENAME = '%ROUTE%' AND DUEDATE =" & Argument("PickDate"))
                        Case Else
                            CtrlTable.RecordsSQL = _
                        "select PARTNAME,PARTDES,SUM(ZROD_TOBEPICKED) as Quant, '' as WARHS, '' as BIN, '0' as PICKED, '' as TYPE, '0' as ORDI,  '' as ORDNAME, '' as  OLINE,CASE when PARTNAME in (SELECT PARTNAME FROM V_PICKLIST_PARTS) THEN 'A' else 'N' end as AVAILABLE,0 AS balance,'' AS SERIALNAME, 0 AS EXPIRYDATE,CONV,PACKING,NOTFIXEDCONV " & _
                        "from V_PICK_MONITOR " & _
                        "WHERE ROUTENAME = '" & CtrlForm.el(0).Data & "'" & _
                        " AND CUSTNAME = '" & cust & "' AND DUEDATE = " & Argument("PickDate") & " and FTNAME = '%FAMILY%'" & _
                       " GROUP BY PARTNAME,PARTDES,TYPE,CONV,ZROD_PICKORDER,PACKING,NOTFIXEDCONV" & _
                    " ORDER by ZROD_PICKORDER"

                            '*************************************************************************************************
                            SendType = tSendType.GetParts
                            InvokeData("select ORDI,PARTNAME,QUANT,ORDNAME,LINE,CONV " & _
                                "from V_PICK_MONITOR " & _
                                "WHERE ROUTENAME = '%ROUTE%' AND DUEDATE = " & Argument("PickDate") & " AND CUSTNAME = '" & cust & "'  and FTNAME = '%FAMILY%'")
                            'InvokeData("select CUST,CUSTNAME,CUSTDES FROM V_CUSTFORROUTE WHERE ROUTENAME = '%ROUTE%' AND DUEDATE =" & Argument("PickDate") & " AND FTNAME = '%FAMILY%'")
                    End Select

                    'CtrlTable.RecordsSQL = _
                    '    "select PARTNAME,PARTDES,SUM(ZROD_TOBEPICKED) as Quant, '' as WARHS, '' as BIN, '0' as PICKED, '' as TYPE, '0' as ORDI,  '' as ORDNAME, '' as  OLINE,CASE when PARTNAME in (SELECT PARTNAME FROM V_PICKLIST_PARTS) THEN 'A' else 'N' end as AVAILABLE,0 AS balance,'' AS SERIALNAME, 0 AS EXPIRYDATE,CONV,PACKING,NOTFIXEDCONV " & _
                    '    "from V_PICK_MONITOR " & _
                    '    "WHERE ROUTENAME = '" & CtrlForm.el(0).Data & "'" & _
                    '    " AND CUSTNAME = '" & cust & "' AND DUEDATE = " & Argument("PickDate") & _
                    '   " GROUP BY PARTNAME,PARTDES,TYPE,CONV,ZROD_PICKORDER,PACKING,NOTFIXEDCONV" & _
                    '" ORDER by ZROD_PICKORDER"
                    Dim S As String = Argument("PickDate")
                    '*************************************************************************************************
                    'SendType = tSendType.GetParts
                    'InvokeData("select ORDI,PARTNAME,QUANT,ORDNAME,LINE,CONV " & _
                    '    "from V_PICK_MONITOR " & _
                    '    "WHERE ROUTENAME = '%ROUTE%' AND DUEDATE = " & Argument("PickDate") & " AND CUSTNAME = '" & cust & "'")
                    f = True
                    ' 
                Loop Until True

                If Not f Then

                    ' Set the query to load recordtype 2s
                    Dim rou As String
                    With CtrlForm
                        rou = .el(.ColNo("ROUTE")).Data
                    End With
                    Dim i As String = Argument("PickDate")
                    Select Case CtrlForm.el(2).Data
                        Case "All"
                            CtrlTable.RecordsSQL = _
                                                   "select PARTNAME,PARTDES,SUM(ZROD_TOBEPICKED) as Quant, '' as WARHS, '' as BIN, '0' as PICKED,TYPE,'' as ORDI, ' ' as ORDNAME,' '  AS OLINE,CASE when PARTNAME in (SELECT PARTNAME FROM V_PICKLIST_PARTS) THEN 'A' else 'N' end as AVAILABLE,0 AS balance,'' AS SERIALNAME, 0 AS EXPIRYDATE ,CONV,PACKING,NOTFIXEDCONV " & _
                                                   "from V_PICK_MONITOR " & _
                                                   "WHERE ROUTENAME = '%ROUTE%' AND DUEDATE = " & Argument("PickDate") & _
                                                   " GROUP BY PARTNAME,PARTDES,TYPE,CONV,ZROD_PICKORDER,PACKING,NOTFIXEDCONV" & _
                                               " ORDER by ZROD_PICKORDER"
                            '*************************************************************************************************
                            SendType = tSendType.GetParts
                            InvokeData("select ORDI,PARTNAME,QUANT,ORDNAME,LINE,CONV " & _
                                "from V_PICK_MONITOR " & _
                                "WHERE ROUTENAME = '%ROUTE%' AND DUEDATE = " & Argument("PickDate"))
                        Case Else
                            CtrlTable.RecordsSQL = _
                                                  "select PARTNAME,PARTDES,SUM(ZROD_TOBEPICKED) as Quant, '' as WARHS, '' as BIN, '0' as PICKED,TYPE,'' as ORDI, ' ' as ORDNAME,' '  AS OLINE,CASE when PARTNAME in (SELECT PARTNAME FROM V_PICKLIST_PARTS) THEN 'A' else 'N' end as AVAILABLE,0 AS balance,'' AS SERIALNAME, 0 AS EXPIRYDATE ,CONV,PACKING,NOTFIXEDCONV " & _
                                                  "from V_PICK_MONITOR " & _
                                                  "WHERE ROUTENAME = '%ROUTE%' AND DUEDATE = " & Argument("PickDate") & " and FTNAME = '%FAMILY%'" & _
                                                  " GROUP BY PARTNAME,PARTDES,TYPE,CONV,ZROD_PICKORDER,PACKING,NOTFIXEDCONV" & _
                                              " ORDER by ZROD_PICKORDER"
                            '*************************************************************************************************
                            SendType = tSendType.GetParts
                            InvokeData("select ORDI,PARTNAME,QUANT,ORDNAME,LINE,CONV " & _
                                "from V_PICK_MONITOR " & _
                                "WHERE ROUTENAME = '%ROUTE%' AND DUEDATE = " & Argument("PickDate") & " and FTNAME = '%FAMILY%'")
                    End Select

                    Dim S As String = Argument("PickDate")

                End If

                'once the query is set we now ensure that the table is empty and then fill it with data and give it focus.
                Try
                    With CtrlTable
                        .Table.Items.Clear()
                        .BeginLoadRS()
                        .Table.Focus()
                    End With
                Catch ex As Exception
                    MsgBox(ex.ToString)
                End Try


                If CtrlTable.Table.Items.Count = 0 Then
                    MsgBox("There are no lines to pick, the pickings form will now close.")
                    Me.CloseMe()
                End If
                'check for previous picks for this route/date combo. If they exist we will need to alter the downloaded data to reflect this
                'if the route is fully picked then we will error. TO FACILITATE THIS WE WILL CREATE A LIST OF ALREADY PICKED ITEMS AND
                'use it to alter the counts stored in the table
                'SendType = tSendType.TableFill
                'InvokeData("SELECT * FROM V_PICKEDITEMS WHERE FORROUTE = '%ROUTE%'")
                'this will fill the DATA structure with the results of the query. The view this is taken from utilises the same PickDate as the Route View


                'cHECK ON THE AVAILABILITY AND RECOLOUR
                Dim IC As ListViewItem
                For Each IC In CtrlTable.Table.Items
                    If IC.SubItems(10).Text = "N" Then
                        IC.BackColor = Color.Red
                    End If
                Next

                CtrlTable.Table.Focus()


                'Case tSendType.PartW

                '    LotScan = False


            Case tSendType.getminpick

                If Data Is Nothing Then
                    CUSTMINPICK = 0
                Else
                    If IsNumeric(Data(0, 0)) Then
                        CUSTMINPICK = Data(0, 0)
                    Else
                        CUSTMINPICK = 0
                        MsgBox("There appears to be an issue with the minimum pick days query please contact Emerge!")
                    End If


                End If

            Case tSendType.Part
                If Data Is Nothing And NOSCAN = False Then
                    MsgBox("There are no lots for this part available please check the expiry dates and the minimum picking days as these could have invalidated the available stock.")
                    Exit Select
                ElseIf Data Is Nothing And NOSCAN = True Then
                    Exit Select
                End If


                Dim SDATE As Date = FormatDateTime("1/1/1988", DateFormat.ShortDate)
                Dim X, y, Z As Integer
                y = DateDiff(DateInterval.Minute, SDATE, Today)

                X = 0
                Z = 0
                Z = UBound(Data, 2)

                If Data(7, 0) = 0 Then

                Else

                    If NOSCAN = False Then
                        If CtrlForm.el(4).Data <> "" Then
                            SendType = tSendType.getminpick
                            InvokeData("select ZROD_MINSHELF from CUSTMINPICKDAYS where CUSTNAME = '" & CtrlForm.el(5).Data & "' AND PARTNAME = '" & Data(4, 0) & "'")
                        End If
                        If CUSTMINPICK <> 0 Then
                            Dim fn As Integer = 0
                            For gg As Integer = 0 To UBound(Data, 2)
                                Data(7, gg) = CUSTMINPICK
                                If ((Data(7, gg) * 1440) + y) > Data(3, gg) Then

                                Else
                                    fn = 1
                                    Data(0, 0) = Data(0, gg)
                                    Data(1, 0) = Data(1, gg)
                                    Data(2, 0) = Data(2, gg)
                                    Data(3, 0) = Data(3, gg)
                                    Data(4, 0) = Data(4, gg)
                                    Data(5, 0) = Data(5, gg)
                                    Data(6, 0) = Data(6, gg)
                                    Data(7, 0) = Data(7, gg)
                                    Exit For
                                End If
                            Next
                            If fn = 0 Then
                                MsgBox("This are no available lots within the Minimum picking days, please use another lot")
                                For Each t As ListViewItem In CtrlTable.Table.Items
                                    t.Selected = False
                                Next
                                Exit Select
                            End If
                            CUSTMINPICK = 0
                        End If

                    End If

                End If
                'For y As Integer = 0 To UBound(Data, 2)

                'AND EXPIRYDATE >=( " & y & " + (ZSFDC_MINPICKDAYS * 1440)
                With CtrlForm
                    If NOSCAN = False Then
                        'Part
                        .el(3).Data = Data(4, 0)
                        .el(3).DataEntry.Text = Data(4, 0)
                        .el(3).ProcessEntry()
                    End If
                    NOSCAN = False
                    'warhs
                    .el(4).Data = Data(1, 0)
                    .el(4).DataEntry.Text = Data(1, 0)
                    'lot
                    .el(6).DataEntry.Text = Data(0, 0)
                    .el(6).Data = Data(0, 0)
                    '.el(5).ProcessEntry()

                    .el(9).Data = Data(5, 0)
                    .el(7).Data = Data(6, 0)

                    .el(10).Data = Data(2, 0)
                    .el(11).Data = Data(3, 0)



                End With
                'firstly detect if scanned part is valid (done by settings in the table / form!!)
                'next check to see if that part is still on the list of parts to be picked and check on its type
                'if its a manufactured part and there is no lot selected then we must error
                Dim it As ListViewItem
                Dim fnd As Boolean = False
                Dim err As Boolean = False

                For Each it In CtrlTable.Table.Items
                    If it.SubItems(0).Text = Data(4, 0) Then
                        'first we see if there is available stock for this part
                        If it.SubItems(10).Text = "N" Then
                            MsgBox("There is no pickable stock for this part")
                            Exit Sub
                        Else
                            'Now we will check to see if thie part has already been picked

                        End If
                        fnd = True
                    End If
                Next
                'we check to see if the part is on the list of parts still needing to be picked. I do this by iterating through the list of parts in the table
                Dim expstring As String = ""
                If fnd = True Then
                    'if its a manufactured part and there is no lot then we raise an error
                    If CtrlForm.el(9).Data <> "R" Then
                        If CtrlForm.el(6).Data = "" Then
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
                                    If it.SubItems(0).Text = Data(4, 0) Then

                                        it.Selected = True
                                        If it.SubItems(17).Text <> "0" Then
                                            expstring = it.SubItems(2).Text & " Units (" & it.SubItems(17).Text & " Pks, " & it.SubItems(18).Text & " Sngls)"
                                        Else
                                            expstring = it.SubItems(2).Text
                                        End If
                                        expected = it.SubItems(2).Text
                                        If it.SubItems(12).Text = "" Then
                                            it.SubItems(12).Text = Data(0, 0)
                                            it.SubItems(11).Text = Data(6, 0)
                                            it.SubItems(13).Text = Data(3, 0)
                                        End If


                                    End If
                                Next
                            End If
                        Else
                            'if there is an already selected item we need to deselect it

                            For Each it In CtrlTable.Table.Items
                                it.Selected = False
                                If it.SubItems(0).Text = Data(4, 0) Then

                                    it.Selected = True
                                    If it.SubItems(17).Text <> "0" Then
                                        expstring = it.SubItems(2).Text & " Units (" & it.SubItems(17).Text & " Pks, " & it.SubItems(18).Text & " Sngls)"
                                    Else
                                        expstring = "Expected = " & it.SubItems(2).Text
                                    End If
                                    expected = it.SubItems(2).Text
                                    If it.SubItems(12).Text = "" Then
                                        it.SubItems(12).Text = Data(0, 0)
                                        it.SubItems(11).Text = Data(6, 0)
                                        it.SubItems(13).Text = Data(3, 0)
                                    End If
                                End If
                            Next
                        End If

                        Dim add As Integer
                        Dim num As New frmNumber
                        With num
                            .Text = expstring
                            .ShowDialog()
                            add = .number

                            .Dispose()
                        End With
                        pick_amount = add

                        With CtrlForm
                            With .el(.ColNo("AMOUNT"))
                                .DataEntry.Text = add
                                .ProcessEntry()
                            End With
                        End With

                    End If


                Else
                    Dim FullPicked As Boolean = False


                    For Each s As PSLIPITEMS In PickedItems
                        If s.PART = Data(4, 0) Then
                            MsgBox("This part has already been fully picked")
                            FullPicked = True
                        End If
                    Next
                    'this part is no longer available to be picked so we need to do 2 things. First inform the user and then clear the part.
                    If FullPicked = False Then
                        MsgBox("This part is not in the list of parts to pick")
                    End If
                    FullPicked = False
                    With CtrlForm
                        .el(.ColNo("PART")).DataEntry.Text = ""
                        .el(.ColNo("PART")).ProcessEntry()
                    End With
                End If


            Case tSendType.Warhs

            Case tSendType.AmountCheck
                pick_amount = Val(Data(0, 0))

            Case tSendType.Amount



            Case tSendType.LotFill
                LotList.Clear()
                For y As Integer = 0 To UBound(Data, 2)
                    Dim lots As New LotS(Data(0, y), Data(1, y), Data(2, y), Data(3, y), Data(4, y), Data(5, y), Data(6, y))
                    LotList.Add(lots)
                Next


            Case tSendType.LotFillNew
                If Data Is Nothing Then
                    MsgBox("Using the last available parts from the present lot and there are no more available lots for this item")
                    CtrlForm.el(6).Data = ""
                Else
                    CtrlForm.el(6).Data = Data(0, 0)
                    CtrlForm.el(4).Data = Data(1, 0)
                    CtrlForm.el(10).Data = Data(2, 0)
                    CtrlForm.el(7).Data = Data(6, 0)
                    CtrlForm.el(11).Data = Data(3, 0)
                End If
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
                                CtrlForm.el(1).Focus()
                                SendType = tSendType.getPickType
                                InvokeData("Select ZROD_PICKTYPE from ZROD_ROUTES where ROUTENAME ='%ROUTE%'")
                            Case "PDATE"
                                Me.CtrlTable.Table.Items.Clear()
                                SendType = tSendType.PickDate
                                InvokeData("SELECT DUEDATE,ROUTE FROM V_UNPICKED_ROUTE WHERE PICKDATE = '%PDATE%' AND ROUTENAME = '%ROUTE%'")

                                Dim PD As Integer
                                Dim t As String = Argument("PickDate")
                                Try
                                    PD = Convert.ToInt64(t)
                                Catch ex As Exception
                                    MsgBox(ex.ToString)
                                End Try


                            Case "FAMILY"
                                CtrlForm.el(5).Focus()
                                If Pick_Type <> "S" Then
                                    SendType = tSendType.Route
                                    InvokeData("select CUST,CUSTNAME,CUSTDES FROM V_CUSTFORROUTE WHERE ROUTENAME = '%ROUTE%' AND DUEDATE =" & Argument("PickDate"))
                                    CtrlForm.el(5).Enabled = False
                                Else
                                    CtrlForm.el(5).Focus()
                                End If
                                'Select Case CtrlForm.el(2).Data
                                '    Case "All"
                                '        InvokeData("select CUST,CUSTNAME,CUSTDES FROM V_CUSTFORROUTE WHERE ROUTENAME = '%ROUTE%' AND DUEDATE =" & Argument("PickDate"))
                                '    Case Else
                                '        InvokeData("select CUST,CUSTNAME,CUSTDES FROM V_CUSTFORROUTE WHERE ROUTENAME = '%ROUTE%' AND DUEDATE =" & Argument("PickDate") & " AND FTNAME = '%FAMILY%'")
                                'End Select



                                'This procedure finds if there are Customers associated with the chosen route annd displays the first available one, then it fills in the data table and the date argument

                            Case "PACKING_SLIP"
                                CtrlTable.Focus()
                                If CtrlForm.el(5).Data <> "" Then
                                    SendType = tSendType.Route

                                    InvokeData("select CUST,CUSTNAME,CUSTDES FROM V_CUSTFORROUTE WHERE ROUTENAME = '%ROUTE%' AND REPLACE(CUSTDES,'''','') = '%PACKING_SLIP%'")
                                Else
                                    SendType = tSendType.Route
                                    InvokeData("select '' as nodata")
                                End If
                                Dim dp As String = Argument("PickDate")
                            Case "LOT"
                                CtrlTable.Focus()

                            Case "AMOUNT"
                                If pick_amount = 0 Then Exit Sub
                                Dim AMOUNT_IN_LOT As Integer = CtrlForm.el(7).Data
                                Dim order_quant As Integer = 0
                                Try
                                    For Each pick_row As ListViewItem In CtrlTable.Table.Items

                                        If pick_row.SubItems(0).Text = CtrlForm.el(3).Data Then
                                            pick_row.Selected = True
                                            If pick_amount > Convert.ToInt32(pick_row.SubItems(2).Text) Then
                                                MsgBox("Too many items picked!")
                                                Exit Sub
                                            End If
                                            'check to see if there is enough in balance for this pick
                                            If bcodetype = "l" Then
                                                Dim adder As Integer = 0
                                                Dim avam As Integer = Convert.ToInt32(CtrlForm.el(7).Data)
                                                For Each a As PSLIPITEMS In PickedItems
                                                    If a.PART = CtrlForm.el(3).Data Then
                                                        If a.Lot = CtrlForm.el(6).Data Then
                                                            adder = a.Quant

                                                        End If
                                                    End If
                                                Next
                                                If adder >= avam Then
                                                    MsgBox("You have already fully picked this lot")
                                                    pick_amount = 0
                                                Else
                                                    If adder = 0 Then
                                                        If (pick_amount) > avam Then
                                                            MsgBox("You have picked to many for this lot, I will use up the remaining stock in this lot (" & avam & "), you will need to scan a different lot to continue picking for this amount.")
                                                            pick_amount = avam
                                                            bcodetype = "s"
                                                        End If
                                                    Else
                                                        avam -= adder
                                                        If (pick_amount) > avam Then
                                                            MsgBox("You have picked to many for this lot, I will use up the remaining stock in this lot (" & avam & "), you will need to scan a different lot to continue picking for this amount.")
                                                            pick_amount = avam
                                                            bcodetype = "s"
                                                        End If
                                                    End If

                                                End If

                                            End If
                                            'there is enough in the lot now to check the orders to use them up for the pick
                                            For Each order As PSLIPITEMS In OrderList

                                                If order.PART = pick_row.SubItems(0).Text And order.Quant <> 0 And pick_amount <> 0 Then
                                                    'we have the part now we check if the orderline amount is > than the picked amount
                                                    If order.Quant >= pick_amount And pick_amount <> 0 Then
                                                        'theres more available than we beed so now we need to check on the amount available in the lot
                                                        AMOUNT_IN_LOT = Convert.ToInt32(pick_row.SubItems(11).Text)
                                                        If AMOUNT_IN_LOT < pick_amount Then
                                                            'there are not enough in this lot for this pick amount so we will take what is available
                                                            'then we will get a new lot

                                                            If AMOUNT_IN_LOT = 0 Then
                                                                'IF THERE ARE NO ITEMS LEFT IN THIS LOT WE MUST GET ANOTHER BEFORE WE CONTINUE
                                                                Dim IBN As String = "Select DISTINCT SERIALNAME,WARHSNAME,LOCNAME,EXPIRYDATE,PARTNAME,TYPE,balance from dbo.V_PICKLIST_PARTS WHERE PARTNAME = '" & pick_row.SubItems(0).Text & "' and EXPIRYDATE >= " & pick_row.SubItems(13).Text & " AND SERIALNAME <> '" & CtrlForm.el(6).Data & "' order by EXPIRYDATE ASC,SERIALNAME DESC"
                                                                SendType = tSendType.LotFillNew
                                                                InvokeData("Select DISTINCT SERIALNAME,WARHSNAME,LOCNAME,EXPIRYDATE,PARTNAME,TYPE,balance from dbo.V_PICKLIST_PARTS WHERE PARTNAME = '" & pick_row.SubItems(0).Text & "' and EXPIRYDATE >= " & pick_row.SubItems(13).Text & " AND SERIALNAME <> '" & CtrlForm.el(6).Data & "' order by EXPIRYDATE ASC,SERIALNAME DESC")
                                                                'check that we have a lot if not we cant proceed
                                                                If CtrlForm.el(6).Data = "" Then ' check to see if a lot no was written by the query
                                                                    'MsgBox("No more lots available for picking")
                                                                    Exit Sub
                                                                Else
                                                                    pick_row.SubItems(3).Text = CtrlForm.el(4).Data 'set the rows whs = to the forms whs
                                                                    pick_row.SubItems(4).Text = CtrlForm.el(10).Data 'set the rows bin = to the forms bin
                                                                    pick_row.SubItems(11).Text = CtrlForm.el(7).Data 'set the rows balance = to the forms available balance
                                                                    AMOUNT_IN_LOT = CtrlForm.el(7).Data 'reset the amount_in_lot variable
                                                                    pick_row.SubItems(12).Text = CtrlForm.el(6).Data 'set the rows LOT = to the forms LOT
                                                                    pick_row.SubItems(13).Text = CtrlForm.el(11).Data 'set the rows expiry = to the forms expiry
                                                                End If


                                                            Else
                                                                Dim to_write As Integer = 0
                                                                Dim g As Integer = 0
                                                                order.Lot = CtrlForm.el(6).Data 'update the orders lot
                                                                order.Bin = CtrlForm.el(10).Data 'update the orders bin
                                                                order.WARHS = CtrlForm.el(4).Data 'update the orders whs
                                                                'we use the remaining amount in the lot as the quantity
                                                                g = AMOUNT_IN_LOT

                                                                'write the order to the picked items line using the amount in lot for the quantity
                                                                Dim NEWLINE As New PSLIPITEMS(order.ORDI, order.ROUTE, order.PSlipNo, order.PART, AMOUNT_IN_LOT, order.Desc, order.Lot, order.WARHS, order.Bin, order.Type, order.oname, order.oline, order.Amount, pick_row.SubItems(14).Text, CtrlForm.el(5).Data)
                                                                PickedItems.Add(NEWLINE)
                                                                'first we calculate the remaining quantity in the table line and the order quant
                                                                Dim j As Integer = 0
                                                                order.Quant -= AMOUNT_IN_LOT 'we take the amount_in_lot from tge ordered quantity as we know that it is less than the pick amount which is less than the ordered quantity

                                                                j = Convert.ToInt32(pick_row.SubItems(2).Text) - AMOUNT_IN_LOT
                                                                pick_row.SubItems(2).Text = j 'we now reset the table line to take account of the amount we have used
                                                                'we will 0 the balance in the table line too
                                                                pick_row.SubItems(11).Text = 0
                                                                '

                                                                'we update the pick_amount also
                                                                pick_amount -= AMOUNT_IN_LOT
                                                                'now we need to redo the lot
                                                                g = pick_amount
                                                                'as we have finished this lot we will set its amount to 0 to prevent it being used again
                                                                AMOUNT_IN_LOT = 0

                                                                Dim CurrLot As String = CtrlForm.el(6).Data
                                                                Do While pick_amount > 0
                                                                    'as there is still some outstanding items that have been picked we need to deal with them now
                                                                    'first up we have exhausted that lot so we need a new one
                                                                    SendType = tSendType.LotFillNew
                                                                    Dim IBN As String = "Select DISTINCT SERIALNAME,WARHSNAME,LOCNAME,EXPIRYDATE,PARTNAME,TYPE,balance from dbo.V_PICKLIST_PARTS WHERE PARTNAME = '" & pick_row.SubItems(0).Text & "' and EXPIRYDATE >= " & pick_row.SubItems(13).Text & " AND SERIALNAME <> '" & CurrLot & "' order by EXPIRYDATE ASC,SERIALNAME DESC"
                                                                    InvokeData("Select DISTINCT SERIALNAME,WARHSNAME,LOCNAME,EXPIRYDATE,PARTNAME,TYPE,balance from dbo.V_PICKLIST_PARTS WHERE PARTNAME = '" & pick_row.SubItems(0).Text & "' and EXPIRYDATE >= " & pick_row.SubItems(13).Text & " AND SERIALNAME <> '" & CurrLot & "' order by EXPIRYDATE ASC,SERIALNAME DESC")
                                                                    'check that we have a lot if not we cant proceed
                                                                    If CtrlForm.el(6).Data = "" Then
                                                                        MsgBox("No more lots available for picking")
                                                                        Exit Sub
                                                                    Else
                                                                        pick_row.SubItems(3).Text = CtrlForm.el(4).Data 'set the rows whs = to the forms whs
                                                                        pick_row.SubItems(4).Text = CtrlForm.el(10).Data 'set the rows bin = to the forms bin
                                                                        pick_row.SubItems(11).Text = CtrlForm.el(7).Data 'set the rows balance = to the forms available balance
                                                                        AMOUNT_IN_LOT = CtrlForm.el(7).Data 'reset the amount_in_lot variable
                                                                        pick_row.SubItems(12).Text = CtrlForm.el(6).Data 'set the rows LOT = to the forms LOT
                                                                        pick_row.SubItems(13).Text = CtrlForm.el(11).Data 'set the rows expiry = to the forms expiry
                                                                    End If
                                                                    'we update the lot in use in the order
                                                                    order.Lot = CtrlForm.el(6).Data
                                                                    order.Bin = CtrlForm.el(10).Data
                                                                    order.WARHS = CtrlForm.el(4).Data
                                                                    'next we see if there is enough in this lot
                                                                    'AMOUNT_IN_LOT
                                                                    If pick_amount <= AMOUNT_IN_LOT Then
                                                                        'as there is less to pick than is in this lot we will just use this one

                                                                        'we will take the full amount outstanding from this lot
                                                                        order.Quant -= pick_amount ' we take the full remaining pick amount from the order
                                                                        Dim jJ As Integer = AMOUNT_IN_LOT - pick_amount
                                                                        pick_row.SubItems(11).Text = jJ ' we update the data row with the updated amount available in the lot
                                                                        Dim hh As Integer = Convert.ToInt32(pick_row.SubItems(2).Text) - pick_amount
                                                                        pick_row.SubItems(2).Text = hh 'we take the amount picked off the expected quantity in the data row
                                                                        'we set the pick amount to 0 to stop the iteration of this part
                                                                        'now we will write a line using the amount in pick
                                                                        Dim NEWLINE2 As New PSLIPITEMS(order.ORDI, order.ROUTE, order.PSlipNo, order.PART, pick_amount, order.Desc, order.Lot, order.WARHS, order.Bin, order.Type, order.oname, order.oline, order.Amount, pick_row.SubItems(14).Text, CtrlForm.el(5).Data)
                                                                        PickedItems.Add(NEWLINE2)
                                                                        pick_amount = 0
                                                                    Else ' there is not enough in the lot to allow us to take the full amount outstanding
                                                                        'we will take all of this lot and then update the pick amount
                                                                        order.Quant -= AMOUNT_IN_LOT 'we update the order to reflect the picking
                                                                        pick_amount -= AMOUNT_IN_LOT ' we take the amount in lot from the amount we need

                                                                        pick_row.SubItems(11).Text = 0 'as we have utilised this lot i will set the available amount to 0
                                                                        Dim hh As Integer = Convert.ToInt32(pick_row.SubItems(2).Text) - pick_amount 'we will reset the table line quant to reflect the amount picked
                                                                        pick_row.SubItems(2).Text = hh
                                                                        'now we will write a line using the amount in lot
                                                                        Dim NEWLINE2 As New PSLIPITEMS(order.ORDI, order.ROUTE, order.PSlipNo, order.PART, AMOUNT_IN_LOT, order.Desc, order.Lot, order.WARHS, order.Bin, order.Type, order.oname, order.oline, order.Amount, pick_row.SubItems(14).Text, CtrlForm.el(5).Data)
                                                                        PickedItems.Add(NEWLINE2)
                                                                    End If

                                                                    AMOUNT_IN_LOT = 0
                                                                    'as this lot has been used if there are still items to pick we will loop round again until we have consumed the full pick amount
                                                                Loop
                                                                'once we have exhausted the picking amount we are done here

                                                            End If

                                                        Else 'we have more in this lot than we need to pick so we will just use the full amount of picking from this lot
                                                            Dim exp As Integer = Convert.ToInt32(pick_row.SubItems(2).Text)
                                                            exp -= pick_amount ' we need to update the table row to represent the pick
                                                            pick_row.SubItems(2).Text = exp

                                                            order.Lot = CtrlForm.el(6).Data
                                                            order.Bin = CtrlForm.el(10).Data
                                                            order.WARHS = CtrlForm.el(4).Data
                                                            'now we add the order line to the final pick and then we update the order line amount
                                                            Dim NEWLINE As New PSLIPITEMS(order.ORDI, order.ROUTE, order.PSlipNo, order.PART, pick_amount, order.Desc, order.Lot, order.WARHS, order.Bin, order.Type, order.oname, order.oline, order.Amount, pick_row.SubItems(14).Text, CtrlForm.el(5).Data)
                                                            PickedItems.Add(NEWLINE)
                                                            order.Quant -= pick_amount 'we update the order now to reflect the pick
                                                            Dim j As Integer = AMOUNT_IN_LOT - pick_amount
                                                            pick_row.SubItems(11).Text = j ' we update the lot quantity too
                                                            pick_amount = 0 ' and we reset the pick amount to 0
                                                        End If

                                                    ElseIf order.Quant < pick_amount And pick_amount <> 0 And order.Quant <> 0 Then
                                                        'there is less in the order than we have picked
                                                        If AMOUNT_IN_LOT < order.Quant Then
                                                            'there are not enough in this lot for this pick amount so we will take what is available
                                                            'then we will get a new lot
                                                            If AMOUNT_IN_LOT = 0 Then
                                                                'IF THERE ARE NO ITEMS LEFT IN THIS LOT WE MUST GET ANOTHER BEFORE WE CONTINUE
                                                                Dim IBN As String = "Select DISTINCT SERIALNAME,WARHSNAME,LOCNAME,EXPIRYDATE,PARTNAME,TYPE,balance from dbo.V_PICKLIST_PARTS WHERE PARTNAME = '" & pick_row.SubItems(0).Text & "' and EXPIRYDATE >= " & pick_row.SubItems(13).Text & " AND SERIALNAME <> '" & CtrlForm.el(6).Data & "' order by EXPIRYDATE ASC,SERIALNAME DESC"
                                                                InvokeData("Select DISTINCT SERIALNAME,WARHSNAME,LOCNAME,EXPIRYDATE,PARTNAME,TYPE,balance from dbo.V_PICKLIST_PARTS WHERE PARTNAME = '" & pick_row.SubItems(0).Text & "' and EXPIRYDATE >= " & pick_row.SubItems(13).Text & " AND SERIALNAME <> '" & CtrlForm.el(6).Data & "' order by EXPIRYDATE ASC,SERIALNAME DESC")
                                                                'check that we have a lot if not we cant proceed
                                                                If CtrlForm.el(6).Data = "" Then
                                                                    MsgBox("No more lots available for picking")
                                                                    Exit Sub
                                                                Else
                                                                    pick_row.SubItems(3).Text = CtrlForm.el(4).Data 'set the rows whs = to the forms whs
                                                                    pick_row.SubItems(4).Text = CtrlForm.el(10).Data 'set the rows bin = to the forms bin
                                                                    pick_row.SubItems(11).Text = CtrlForm.el(7).Data 'set the rows balance = to the forms available balance
                                                                    AMOUNT_IN_LOT = CtrlForm.el(7).Data 'reset the amount_in_lot variable
                                                                    pick_row.SubItems(12).Text = CtrlForm.el(6).Data 'set the rows LOT = to the forms LOT
                                                                    pick_row.SubItems(13).Text = CtrlForm.el(11).Data 'set the rows expiry = to the forms expiry
                                                                End If
                                                            Else
                                                                order.Lot = CtrlForm.el(6).Data
                                                                order.Bin = CtrlForm.el(10).Data
                                                                order.WARHS = CtrlForm.el(4).Data
                                                                'we use the remaining amount in the lot as the quantity
                                                                order.Quant -= AMOUNT_IN_LOT
                                                                'pick_amount -= Convert.ToInt32(pick_row.SubItems(11).Text)
                                                                'we write a line using the amount in this lot
                                                                Dim NEWLINE As New PSLIPITEMS(order.ORDI, order.ROUTE, order.PSlipNo, order.PART, AMOUNT_IN_LOT, order.Desc, order.Lot, order.WARHS, order.Bin, order.Type, order.oname, order.oline, order.Amount, pick_row.SubItems(14).Text, CtrlForm.el(5).Data)
                                                                PickedItems.Add(NEWLINE)
                                                                'first we calculate the remaining quantity in the order line
                                                                Dim j As Integer = 0
                                                                pick_row.SubItems(11).Text = 0

                                                                j = Convert.ToInt32(pick_row.SubItems(2).Text) - AMOUNT_IN_LOT
                                                                pick_row.SubItems(2).Text = j
                                                                'we now reset the order.quant to take account of this

                                                                'now we add the order line to the final pick and then we update the order line amount
                                                                'we update the pick_amount also
                                                                pick_amount -= AMOUNT_IN_LOT
                                                                'now we need to redo the lot
                                                                AMOUNT_IN_LOT = 0

                                                                Dim CurrLot As String = CtrlForm.el(6).Data
                                                                Do While order.Quant <> 0
                                                                    SendType = tSendType.LotFillNew
                                                                    InvokeData("Select DISTINCT SERIALNAME,WARHSNAME,LOCNAME,EXPIRYDATE,PARTNAME,TYPE,balance from dbo.V_PICKLIST_PARTS WHERE PARTNAME = '" & pick_row.SubItems(0).Text & "' and EXPIRYDATE >= " & pick_row.SubItems(13).Text & " AND SERIALNAME <> '" & CurrLot & "' order by EXPIRYDATE ASC,SERIALNAME DESC")
                                                                    'check that we have a lot if not we cant proceed
                                                                    If CtrlForm.el(6).Data = "" Then
                                                                        MsgBox("No more lots available for picking")
                                                                        Exit Sub
                                                                    Else
                                                                        pick_row.SubItems(3).Text = CtrlForm.el(4).Data
                                                                        pick_row.SubItems(4).Text = CtrlForm.el(10).Data
                                                                        pick_row.SubItems(11).Text = CtrlForm.el(7).Data
                                                                        AMOUNT_IN_LOT = CtrlForm.el(7).Data
                                                                        pick_row.SubItems(12).Text = CtrlForm.el(6).Data
                                                                        pick_row.SubItems(13).Text = CtrlForm.el(11).Data
                                                                    End If
                                                                    order.Lot = CtrlForm.el(6).Data
                                                                    order.Bin = CtrlForm.el(10).Data
                                                                    order.WARHS = CtrlForm.el(4).Data
                                                                    'next we see if there is enough in this lot

                                                                    If order.Quant <= AMOUNT_IN_LOT Then

                                                                        'we will take the full amount outstanding from this lot

                                                                        Dim NEWLINE2 As New PSLIPITEMS(order.ORDI, order.ROUTE, order.PSlipNo, order.PART, order.Quant, order.Desc, order.Lot, order.WARHS, order.Bin, order.Type, order.oname, order.oline, order.Amount, pick_row.SubItems(14).Text, CtrlForm.el(5).Data)
                                                                        PickedItems.Add(NEWLINE2)
                                                                        'we now need to update the various areas
                                                                        pick_amount -= order.Quant 'we update the amount still needing picking
                                                                        AMOUNT_IN_LOT -= order.Quant ' this updates the amount remaining in the lot in memory
                                                                        pick_row.SubItems(11).Text = AMOUNT_IN_LOT ' that updates the pick_row to reflect this lot
                                                                        Dim g As Int32 = Convert.ToInt32(pick_row.SubItems(2).Text)
                                                                        g -= order.Quant
                                                                        'now we update the quantity that we still need to pick in the data table
                                                                        pick_row.SubItems(2).Text = g
                                                                        order.Quant = 0
                                                                    Else
                                                                        'we will take all of this lot and then update the pick amount and the order
                                                                        Dim f As Integer = order.Quant
                                                                        'we have reserved the quant
                                                                        'Web take the amount in the lot from the remaining amount to pick and from the order quant

                                                                        order.Quant -= AMOUNT_IN_LOT

                                                                        pick_amount -= AMOUNT_IN_LOT
                                                                        Dim NEWLINE2 As New PSLIPITEMS(order.ORDI, order.ROUTE, order.PSlipNo, order.PART, AMOUNT_IN_LOT, order.Desc, order.Lot, order.WARHS, order.Bin, order.Type, order.oname, order.oline, order.Amount, pick_row.SubItems(14).Text, CtrlForm.el(5).Data)
                                                                        PickedItems.Add(NEWLINE2)
                                                                        Dim g As Int32 = Convert.ToInt32(pick_row.SubItems(2).Text)
                                                                        g -= order_quant
                                                                        pick_row.SubItems(2).Text = g
                                                                        AMOUNT_IN_LOT = 0
                                                                        pick_row.SubItems(11).Text = AMOUNT_IN_LOT ' that updates the pick_row to reflect this lot
                                                                    End If


                                                                Loop
                                                            End If




                                                        Else
                                                            'the order line we are currently on will have to be fully utilised to take part of the amount picked
                                                            Dim exp As Integer = Convert.ToInt32(pick_row.SubItems(2).Text)
                                                            exp -= order.Quant
                                                            pick_row.SubItems(2).Text = exp
                                                            order.Lot = CtrlForm.el(6).Data
                                                            order.Bin = CtrlForm.el(10).Data
                                                            order.WARHS = CtrlForm.el(4).Data
                                                            'now we add the order line to the final pick and then we set the order line amount to 0 to show that it is finished
                                                            Dim NEWLINE As New PSLIPITEMS(order.ORDI, order.ROUTE, order.PSlipNo, order.PART, order.Quant, order.Desc, order.Lot, order.WARHS, order.Bin, order.Type, order.oname, order.oline, order.Amount, pick_row.SubItems(14).Text, CtrlForm.el(5).Data)
                                                            PickedItems.Add(NEWLINE)


                                                            Dim j As Integer = AMOUNT_IN_LOT - order.Quant
                                                            pick_row.SubItems(11).Text = j
                                                            AMOUNT_IN_LOT = j
                                                            pick_amount -= order.Quant
                                                            order.Quant = 0

                                                        End If

                                                    End If
                                                End If
                                            Next

                                        End If

                                    Next
                                Catch ex As Exception
                                    MsgBox(ex.ToString)
                                End Try


                                'after that we need to remove anylines that are fully picked
                                Dim it As ListViewItem
                                Dim itemcount As Integer
                                Dim StillLive As Boolean = False
                                itemcount = CtrlTable.Table.Items.Count
                                For pos As Integer = (itemcount - 1) To 0 Step -1

                                    it = CtrlTable.Table.Items(pos)
                                    'it In CtrlTable.Table.Items
                                    it.Selected = False
                                    Dim g As Integer
                                    g = Convert.ToInt16(it.SubItems(2).Text)
                                    If it.SubItems(2).Text <= 0 Then
                                        CtrlTable.Table.Items.Remove(it)
                                    Else
                                        If it.SubItems(15).Text <> 0 And it.SubItems(16).Text <> "Y" Then
                                            Dim tot As Integer = Convert.ToInt32(it.SubItems(2).Text)
                                            Dim pack, unit, pamount As Integer
                                            pamount = Convert.ToInt32(it.SubItems(15).Text)
                                            If tot < pamount Then
                                                pack = 0
                                                unit = tot Mod pamount
                                            Else
                                                pack = tot \ pamount
                                                unit = tot Mod pamount
                                            End If
                                            it.SubItems(17).Text = pack
                                            it.SubItems(18).Text = unit

                                        End If
                                    End If
                                Next
                                CtrlTable.Focus()

                            Case "PART"
                                If NOSCAN = True Then
                                    If CtrlForm.el(5).Data <> "" Then
                                        SendType = tSendType.getminpick
                                        InvokeData("select ZROD_MINSHELF from CUSTMINPICKDAYS where CUSTNAME = '" & CtrlForm.el(5).Data & "' AND PARTNAME = '%PART%'")
                                    End If
                                    If CUSTMINPICK <> 0 Then
                                        CUSTMINPICK *= 1440
                                        SendType = tSendType.Part
                                        InvokeData("Select DISTINCT SERIALNAME,WARHSNAME,LOCNAME,EXPIRYDATE,PARTNAME,TYPE,balance,ZSFDC_MINPICKDAYS from dbo.V_PICKLIST_PARTS WHERE PARTNAME = '%PART%' AND TYPE = 'R' and EXPIRYDATE >= ((SELECT VALUE FROM LASTS WHERE NAME = 'PICK_DATE') + " & CUSTMINPICK & ") order by EXPIRYDATE ASC,SERIALNAME DESC")

                                    Else
                                        SendType = tSendType.Part
                                        InvokeData("Select DISTINCT SERIALNAME,WARHSNAME,LOCNAME,EXPIRYDATE,PARTNAME,TYPE,balance,ZSFDC_MINPICKDAYS from dbo.V_PICKLIST_PARTS WHERE PARTNAME = '%PART%' AND TYPE = 'R' and EXPIRYDATE >= ((SELECT VALUE FROM LASTS WHERE NAME = 'PICK_DATE') + (ZSFDC_MINPICKDAYS * 1440)) order by EXPIRYDATE ASC,SERIALNAME DESC")

                                    End If
                                    'NOSCAN = False
                                    CUSTMINPICK = 0
                                    CtrlTable.Focus()
                                End If


                            Case "TYPE"

                                CtrlTable.Focus()

                            Case "WHS"
                                SendType = tSendType.Warhs
                                InvokeData("select TYPE from dbo.V_PICKLIST_PARTS WHERE PARTNAME = '%PART%' AND WARHSNAME = '%WHS%'")


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

        With CtrlForm
            If cust.Length <> 0 Then
                '******************************************************************************************************************************

                Try
                    With p
                        .DebugFlag = False
                        .Procedure = "ZSFDC_LOAD_PS"
                        .Table = "ZSFDC_LOAD_PS"
                        .RecordType1 = "CURDATE,CUSTNAME,WARHSNAME,OWNERLOGIN"
                        .RecordType2 = "ORDI,PARTNAME,SERIALNAME,FROMWARHSNAME,LOCNAME,TQUANT,ORDNAME,OLINE"
                        .RecordTypes = "TEXT,TEXT,TEXT,TEXT,TEXT,TEXT,TEXT,TEXT,TEXT,,TEXT,TEXT"
                    End With

                Catch e As Exception
                    MsgBox(e.Message)
                End Try

                Dim t1() As String = { _
                                     curdate, _
                                    PickedItems(0).Cust, _
                                    CtrlForm.ItemValue("ROUTE") & "PI", _
                                    UserName _
                                    }
                p.AddRecord(1) = t1
                Dim c As String = PickedItems(0).Cust
                PickedItems.Sort(Function(x, y) x.cust.compareto(y.cust))


                ' Type 1 records --- pitems
                For y As Integer = 0 To (PickedItems.Count - 1)
                    If y > 0 And PickedItems(y).Cust <> c Then
                        Dim t3() As String = { _
                                     curdate, _
                                    PickedItems(y).Cust, _
                                    CtrlForm.ItemValue("ROUTE") & "PI", _
                                    UserName _
                                    }

                        p.AddRecord(1) = t3
                    End If
                    If PickedItems(y).Con <> 1 Then
                        PickedItems(y).Quant /= PickedItems(y).Con
                    End If
                    Dim t2() As String = { _
                                PickedItems(y).ORDI, _
                                PickedItems(y).PART, _
                                PickedItems(y).Lot, _
                                PickedItems(y).WARHS, _
                                PickedItems(y).Bin, _
                                (PickedItems(y).Quant * 1000), _
                                PickedItems(y).oname, _
                                PickedItems(y).oline _
                                                                      }
                    p.AddRecord(2) = t2
                    SendType = tSendType.None

                Next
                PickedItems.Clear()


            Else



                Try
                    With p
                        .DebugFlag = False
                        .Procedure = "ZSFDC_LOADZROD_PICK"
                        .Table = "ZSFDC_LOADZROD_PICK"
                        .RecordType1 = "PICKEDDATE,FORDATE,FORROUTE,ISCHECKED,PACKSLIP,USERLOGIN"
                        .RecordType2 = "PARTNAME,AMOUNTPICKED,WARHSNAME,LOCNAME,SERIALNAME,ORDI,ORDNAME,OLINE"
                        .RecordTypes = "TEXT,TEXT,TEXT,TEXT,TEXT,TEXT,TEXT,,TEXT,TEXT,TEXT,,TEXT,TEXT"
                    End With

                Catch e As Exception
                    MsgBox(e.Message)
                End Try
                SendType = tSendType.Time


                Dim t1() As String = { _
                                    curdate, _
                                    Me.Argument("PickDate"), _
                                    CtrlForm.ItemValue("ROUTE"), _
                                    "N", _
                                    CtrlForm.ItemValue("PACKING_SLIP"), _
                                    UserName _
                                    }
                p.AddRecord(1) = t1

                For Each PickLine As PSLIPITEMS In PickedItems
                    If PickLine.Con <> 1 Then
                        PickLine.Quant /= PickLine.Con
                    End If

                    Dim t2() As String = {PickLine.PART, _
                                            (PickLine).Quant * 1000, _
                                            PickLine.WARHS, _
                                           PickLine.Bin, _
                                            PickLine.Lot, _
                                            PickLine.ORDI, _
                                            PickLine.oname, _
                                            PickLine.oline _
                                            }

                    p.AddRecord(2) = t2
                Next
                For y As Integer = 0 To (PickedItems.Count - 1)
                    Dim F As Integer = PickedItems(y).ORDI

                    Dim HOL As Integer = PickedItems(y).Quant


                Next

            End If
        End With
        PickedItems.Clear()
        OrderList.Clear()


    End Sub
    Public Sub closing() Handles Me.EndForm
        PickedItems.Clear()
        OrderList.Clear()
    End Sub
    Public Overrides Sub TableRXData(ByVal Data(,) As String)
        If Data Is Nothing Then

        Else
            Try
                For y As Integer = 0 To UBound(Data, 2)
                    Dim lvi As New ListViewItem
                    Dim Q As Integer
                    Q = Data(2, y)
                    If Data(14, y) <> 1 Then
                        Q = Q * Data(14, y)
                    End If
                    Dim pack, singl, tot As Integer
                    pack = 0
                    singl = 0
                    If Data(15, y) <> 0 And Data(16, y) <> "Y" Then
                        tot = Data(2, y)
                        pack = tot / Data(15, y)
                        singl = tot Mod Data(15, y)
                    End If
                    With CtrlTable.Table
                        .Items.Add(lvi)
                        .Items(.Items.Count - 1).Text = Data(0, y)
                        .Items(.Items.Count - 1).SubItems.Add(Data(1, y))
                        .Items(.Items.Count - 1).SubItems.Add(Q)
                        .Items(.Items.Count - 1).SubItems.Add(Data(3, y))
                        .Items(.Items.Count - 1).SubItems.Add(Data(4, y))
                        .Items(.Items.Count - 1).SubItems.Add(Data(5, y))
                        .Items(.Items.Count - 1).SubItems.Add(Data(6, y))
                        .Items(.Items.Count - 1).SubItems.Add(Data(7, y))
                        .Items(.Items.Count - 1).SubItems.Add(Data(8, y))
                        .Items(.Items.Count - 1).SubItems.Add(Data(9, y))
                        .Items(.Items.Count - 1).SubItems.Add(Data(10, y))
                        .Items(.Items.Count - 1).SubItems.Add(Data(11, y))
                        .Items(.Items.Count - 1).SubItems.Add(Data(12, y))
                        .Items(.Items.Count - 1).SubItems.Add(Data(13, y))
                        .Items(.Items.Count - 1).SubItems.Add(Data(14, y))
                        .Items(.Items.Count - 1).SubItems.Add(Data(15, y))
                        .Items(.Items.Count - 1).SubItems.Add(Data(16, y))
                        .Items(.Items.Count - 1).SubItems.Add(pack)
                        .Items(.Items.Count - 1).SubItems.Add(singl)
                    End With
                Next
            Catch e As Exception
                MsgBox(e.Message)
            End Try
        End If
        SendType = tSendType.None
        Dim pd As Integer = Me.Argument("PickDate")
        If Pick_Type = "S" And PickedItems.Count > 0 Then
            Dim lv As ListViewItem
            Dim it As PSLIPITEMS

            For Each lv In CtrlTable.Table.Items
                For Each it In PickedItems
                    If it.Cust = CtrlForm.el(5).Data Then
                        If lv.SubItems(0).Text = it.PART Then
                            Dim currq As Integer = Convert.ToInt32(lv.SubItems(2).Text)
                            currq -= it.Quant
                            lv.SubItems(2).Text = currq
                        End If

                    End If
                Next

            Next

            'after that we need to remove anylines that are fully picked
            Dim ite As ListViewItem
            Dim itemcount As Integer
            Dim StillLive As Boolean = False
            itemcount = CtrlTable.Table.Items.Count
            For pos As Integer = (itemcount - 1) To 0 Step -1

                ite = CtrlTable.Table.Items(pos)
                'it In CtrlTable.Table.Items
                ite.Selected = False
                Dim g As Integer
                g = Convert.ToInt16(ite.SubItems(2).Text)
                If ite.SubItems(2).Text <= 0 Then
                    CtrlTable.Table.Items.Remove(ite)
                Else
                    If ite.SubItems(15).Text <> 0 And ite.SubItems(16).Text <> "Y" Then
                        Dim tot As Integer = Convert.ToInt32(ite.SubItems(2).Text)
                        Dim pack, unit, pamount As Integer
                        pamount = Convert.ToInt32(ite.SubItems(15).Text)
                        If tot < pamount Then
                            pack = 0
                            unit = tot Mod pamount
                        Else
                            pack = tot \ pamount
                            unit = tot Mod pamount
                        End If
                        ite.SubItems(17).Text = pack
                        ite.SubItems(18).Text = unit

                    End If
                End If
            Next
        End If






    End Sub

#End Region

#Region "Table Scanning"

    Public Overrides Sub TableScan(ByVal Value As String)
        bcodetype = "s"
        Try
            If System.Text.RegularExpressions.Regex.IsMatch(Value, ValidStr(tRegExValidation.tBarcode)) Or System.Text.RegularExpressions.Regex.IsMatch(Value, ValidStr(tRegExValidation.tBarcode2)) Then
                ' Scanning a barcode
                With CtrlForm
                    If Not (.el(.ColNo("ROUTE")).Data.Length > 0) Then Throw New Exception("Please select a route.")
                    'If Not (.el(.ColNo("WHS")).Data.Length > 0) Then Throw New Exception("Please select a warehouse.")
                    SendType = tSendType.PartName
                    InvokeData("Select PARTNAME FROM PART WHERE BARCODE ='" & Value & "'")
                    With CtrlForm
                        With .el(.ColNo("PART"))
                            .DataEntry.Text = PartS
                            .ProcessEntry()
                        End With

                    End With

                End With
            ElseIf System.Text.RegularExpressions.Regex.IsMatch(Value, ValidStr(tRegExValidation.tMANDS)) Then
                Dim tycheck As String = Value.Substring(0, 2)
                With CtrlForm
                    If Not (.el(.ColNo("ROUTE")).Data.Length > 0) Then Throw New Exception("Please select a route.")
                    'If Not (.el(.ColNo("WHS")).Data.Length > 0) Then Throw New Exception("Please select a warehouse.")

                End With
                LotScan = True
                bcodetype = "l"
                Dim dstring As String = Value.Substring(12, 6)
                Dim SDATE As Date = FormatDateTime("1/1/1988", DateFormat.ShortDate)
                Dim pa As String = Value.Substring(2, 10)
                Dim X, y As Integer
                Dim dholder As String = dstring.Substring(4, 2) & "/" & dstring.Substring(2, 2) & "/" & dstring.Substring(0, 2)
                Dim S As Date = FormatDateTime(dholder, DateFormat.ShortDate)
                X = DateDiff(DateInterval.Minute, SDATE, S)
                y = DateDiff(DateInterval.Minute, SDATE, Today)
                SendType = tSendType.Part
                InvokeData("Select DISTINCT SERIALNAME,WARHSNAME,LOCNAME,EXPIRYDATE,PARTNAME,TYPE,balance,ZSFDC_MINPICKDAYS from dbo.V_PICKLIST_PARTS WHERE BARCODE = '" & pa & "' and EXPIRYDATE = " & X & " order by EXPIRYDATE ASC,SERIALNAME DESC")


            ElseIf System.Text.RegularExpressions.Regex.IsMatch(Value, ValidStr(tRegExValidation.tPar28)) Then

                Dim tycheck As String = Value.Substring(1, 2)
                With CtrlForm
                    If Not (.el(.ColNo("ROUTE")).Data.Length > 0) Then Throw New Exception("Please select a route.")
                    'If Not (.el(.ColNo("WHS")).Data.Length > 0) Then Throw New Exception("Please select a warehouse.")

                End With



                Select Case tycheck
                    Case "01"
                        LotScan = True
                        bcodetype = "l"
                        Dim dstring As String = Value.Substring(22, 6)
                        Dim SDATE As Date = FormatDateTime("1/1/1988", DateFormat.ShortDate)
                        Dim pa As String = Value.Substring(4, 14)
                        Dim X, y As Integer
                        Dim dholder As String = dstring.Substring(4, 2) & "/" & dstring.Substring(2, 2) & "/" & dstring.Substring(0, 2)
                        Dim S As Date = FormatDateTime(dholder, DateFormat.ShortDate)
                        X = DateDiff(DateInterval.Minute, SDATE, S)
                        y = DateDiff(DateInterval.Minute, SDATE, Today)
                        SendType = tSendType.Part
                        InvokeData("Select DISTINCT SERIALNAME,WARHSNAME,LOCNAME,EXPIRYDATE,PARTNAME,TYPE,balance,ZSFDC_MINPICKDAYS from dbo.V_PICKLIST_PARTS WHERE BARCODE = '" & pa & "' and EXPIRYDATE = " & X & " order by EXPIRYDATE ASC,SERIALNAME DESC")

                    Case "00"
                        LotScan = True
                        bcodetype = "l"
                        Dim dstring As String = Value.Substring(4, 6)
                        Dim SDATE As Date = FormatDateTime("1/1/1988", DateFormat.ShortDate)
                        Dim pa As String = Value.Substring(7, 10)
                        Dim X, y As Integer
                        Dim dholder As String = dstring.Substring(4, 2) & "/" & dstring.Substring(2, 2) & "/" & dstring.Substring(0, 2)
                        Dim S As Date = FormatDateTime(dholder, DateFormat.ShortDate)
                        X = DateDiff(DateInterval.Minute, SDATE, S)
                        y = DateDiff(DateInterval.Minute, SDATE, Today)
                        SendType = tSendType.Part
                        InvokeData("Select DISTINCT SERIALNAME,WARHSNAME,LOCNAME,EXPIRYDATE,PARTNAME,TYPE,balance,ZSFDC_MINPICKDAYS from dbo.V_PICKLIST_PARTS WHERE BARCODE = '" & pa & "' and EXPIRYDATE = " & X & " order by EXPIRYDATE ASC,SERIALNAME DESC")

                    Case Else
                        MsgBox("Barcode not recognised")
                        Exit Select

                End Select

            ElseIf System.Text.RegularExpressions.Regex.IsMatch(Value, ValidStr(tRegExValidation.tRod2)) Then

                Dim tycheck As String = Value.Substring(1, 2)
                With CtrlForm
                    If Not (.el(.ColNo("ROUTE")).Data.Length > 0) Then Throw New Exception("Please select a route.")
                    'If Not (.el(.ColNo("WHS")).Data.Length > 0) Then Throw New Exception("Please select a warehouse.")

                End With

                LotScan = True
                bcodetype = "l"
                Dim dstring As String = Value.Substring(6, 6)
                Dim SDATE As Date = FormatDateTime("1/1/1988", DateFormat.ShortDate)
                Dim pa As String = Value.Substring(0, 6)
                Dim X, y As Integer
                Dim dholder As String = dstring.Substring(4, 2) & "/" & dstring.Substring(2, 2) & "/" & dstring.Substring(0, 2)
                Dim S As Date = FormatDateTime(dholder, DateFormat.ShortDate)
                X = DateDiff(DateInterval.Minute, SDATE, S)
                y = DateDiff(DateInterval.Minute, SDATE, Today)
                SendType = tSendType.Part
                InvokeData("Select DISTINCT SERIALNAME,WARHSNAME,LOCNAME,EXPIRYDATE,PARTNAME,TYPE,balance,ZSFDC_MINPICKDAYS from dbo.V_PICKLIST_PARTS WHERE BARCODE = '" & pa & "' and EXPIRYDATE = " & X & " order by EXPIRYDATE ASC,SERIALNAME DESC")


            ElseIf System.Text.RegularExpressions.Regex.IsMatch(Value, ValidStr(tRegExValidation.tRod1)) Then
                With CtrlForm
                    If Not (.el(.ColNo("ROUTE")).Data.Length > 0) Then Throw New Exception("Please select a route.")
                    'If Not (.el(.ColNo("WHS")).Data.Length > 0) Then Throw New Exception("Please select a warehouse.")

                End With
                SendType = tSendType.Part
                InvokeData("Select DISTINCT SERIALNAME,WARHSNAME,LOCNAME,EXPIRYDATE,PARTNAME,TYPE,balance,ZSFDC_MINPICKDAYS from dbo.V_PICKLIST_PARTS WHERE SERIALNAME = '" & Value & "' order by EXPIRYDATE ASC,SERIALNAME DESC")

            ElseIf System.Text.RegularExpressions.Regex.IsMatch(Value, ValidStr(tRegExValidation.tGSI128)) Then

                Dim tycheck As String = Value.Substring(0, 2)
                With CtrlForm
                    If Not (.el(.ColNo("ROUTE")).Data.Length > 0) Then Throw New Exception("Please select a route.")
                    'If Not (.el(.ColNo("WHS")).Data.Length > 0) Then Throw New Exception("Please select a warehouse.")

                End With
                LotScan = True
                bcodetype = "l"


                Select Case tycheck
                    Case "01"
                        LotScan = True
                        bcodetype = "l"
                        Dim dstring As String = Value.Substring(18, 6)
                        Dim SDATE As Date = FormatDateTime("1/1/1988", DateFormat.ShortDate)
                        Dim pa As String = Value.Substring(2, 14)
                        Dim X, y As Integer
                        Dim dholder As String = dstring.Substring(4, 2) & "/" & dstring.Substring(2, 2) & "/" & dstring.Substring(0, 2)
                        Dim S As Date = FormatDateTime(dholder, DateFormat.ShortDate)
                        X = DateDiff(DateInterval.Minute, SDATE, S)
                        y = DateDiff(DateInterval.Minute, SDATE, Today)
                        SendType = tSendType.Part
                        InvokeData("Select DISTINCT SERIALNAME,WARHSNAME,LOCNAME,EXPIRYDATE,PARTNAME,TYPE,balance,ZSFDC_MINPICKDAYS from dbo.V_PICKLIST_PARTS WHERE BARCODE = '" & pa & "' and EXPIRYDATE = " & X & " order by EXPIRYDATE ASC,SERIALNAME DESC")

                    Case "00"
                        LotScan = True
                        bcodetype = "l"
                        Dim dstring As String = Value.Substring(12, 6)
                        Dim SDATE As Date = FormatDateTime("1/1/1988", DateFormat.ShortDate)
                        Dim pa As String = Value.Substring(2, 10)
                        Dim X, y As Integer
                        Dim dholder As String = dstring.Substring(4, 2) & "/" & dstring.Substring(2, 2) & "/" & dstring.Substring(0, 2)
                        Dim S As Date = FormatDateTime(dholder, DateFormat.ShortDate)
                        X = DateDiff(DateInterval.Minute, SDATE, S)
                        y = DateDiff(DateInterval.Minute, SDATE, Today)
                        SendType = tSendType.Part
                        InvokeData("Select DISTINCT SERIALNAME,WARHSNAME,LOCNAME,EXPIRYDATE,PARTNAME,TYPE,balance,ZSFDC_MINPICKDAYS from dbo.V_PICKLIST_PARTS WHERE BARCODE = '" & pa & "' and EXPIRYDATE = " & X & " order by EXPIRYDATE ASC,SERIALNAME DESC")

                    Case Else
                        MsgBox("Barcode not recognised")
                        Exit Select

                End Select

            ElseIf System.Text.RegularExpressions.Regex.IsMatch(Value, ValidStr(tRegExValidation.tPar1415)) Then
                With CtrlForm
                    If Not (.el(.ColNo("ROUTE")).Data.Length > 0) Then Throw New Exception("Please select a route.")


                End With
                LotScan = True
                SendType = tSendType.Part
                InvokeData("Select DISTINCT SERIALNAME,WARHSNAME,LOCNAME,EXPIRYDATE,PARTNAME,TYPE,balance,ZSFDC_MINPICKDAYS from dbo.V_PICKLIST_PARTS WHERE BARCODE = '" & Value & "' and EXPIRYDATE >= (SELECT VALUE FROM LASTS WHERE NAME = 'PICK_DATE') order by EXPIRYDATE ASC,SERIALNAME DESC")
            Else
                MsgBox("Barcode not recognised")


            End If

        Catch EX As Exception
            MsgBox(String.Format("{0}", EX.Message))
        End Try


    End Sub

#End Region

End Class
