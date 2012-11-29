Public Class InterfacePRODREP
    Inherits SFDCData.iForm

    Public Sub New(Optional ByRef App As Form = Nothing)

        InitializeComponent()
        CallerApp = App
        NewArgument("BARCODE", "")
        CtrlTable.DisableButtons(True, False, True, True, False)
        CtrlTable.EnableToolbar(False, True, False, False, True)

    End Sub

    Public Overrides Sub FormLoaded()
        MyBase.FormLoaded()

    End Sub

    Public Overrides Sub FormSettings()

        'FROM WARHSNAME
        With field
            .Name = "WARHS"
            .Title = "From W/H"
            .ValidExp = ValidStr(tRegExValidation.tWarehouse)
            .SQLList = "SELECT DISTINCT WARHSNAME FROM WAREHOUSES WHERE INACTIVE <> 'Y'"
            .SQLValidation = "SELECT WARHSNAME FROM WAREHOUSES WHERE WARHSNAME = '%ME%'"
            .Data = ""      '******** Barcoded field '*******
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

    End Sub

    Public Overrides Sub TableSettings()

        ' WARHSNAME
        With col
            .Name = "_SERIALNAME"
            .Title = "W/Order"
            .initWidth = 25
            .TextAlign = HorizontalAlignment.Left
            .AltEntry = ctrlText.tAltCtrlStyle.ctNone 'ctKeyb 
            .ValidExp = ValidStr(tRegExValidation.tString)
            '.SQLList = "SELECT DISTINCT WARHSNAME FROM WAREHOUSES"
            .SQLValidation = "SELECT '%ME%'"
            .DefaultFromCtrl = Nothing
            .ctrlEnabled = False
            .Mandatory = False
        End With
        CtrlTable.AddCol(col)

        ' LOCNAME
        With col
            .Name = "_TOPALLET"
            .Title = "Roll"
            .initWidth = 25
            .TextAlign = HorizontalAlignment.Left
            .AltEntry = ctrlText.tAltCtrlStyle.ctNone ' ctNone 'ctKeyb 
            .ValidExp = ValidStr(tRegExValidation.tString)
            '.SQLList = "SELECT DISTINCT LOCNAME FROM WAREHOUSES WHERE WARHSNAME = '%WARHS%'"
            .SQLValidation = "SELECT '%ME%'"
            .DefaultFromCtrl = Nothing      '******* Barcoded Field - default from Type1 LOCNAME '*******
            .ctrlEnabled = False
            .Mandatory = False
        End With
        CtrlTable.AddCol(col)

        ' Counted Quantity
        With col
            .Name = "_MQUANT"
            .Title = "QTY"
            .initWidth = 20
            .TextAlign = HorizontalAlignment.Left
            .AltEntry = ctrlText.tAltCtrlStyle.ctNone 'ctKeyb 
            .ValidExp = ValidStr(tRegExValidation.tNumeric)
            .SQLValidation = ""
            .DefaultFromCtrl = Nothing
            .ctrlEnabled = True
            .Mandatory = False
        End With
        CtrlTable.AddCol(col)

        ' STATUS
        With col
            .Name = "_STATUS"
            .Title = "Status"
            .initWidth = 25
            .TextAlign = HorizontalAlignment.Left
            .AltEntry = ctrlText.tAltCtrlStyle.ctList 'ctKeyb 
            .ValidExp = ValidStr(tRegExValidation.tStatus)
            .SQLList = "SELECT CUSTNAME FROM CUSTOMERS WHERE STATUSFLAG = 'Y'"
            .SQLValidation = "SELECT CUSTNAME FROM CUSTOMERS WHERE STATUSFLAG = 'Y' AND CUSTNAME = '%ME%'"
            .DefaultFromCtrl = Nothing
            .ctrlEnabled = False
            .Mandatory = False
        End With
        CtrlTable.AddCol(col)

        ' Set the query to load recordtype 2s
        CtrlTable.RecordsSQL = ""

    End Sub

    Public Overrides Sub TableRXData(ByVal Data(,) As String)
        Dim y As Integer
        Dim x As Integer
        Dim i As Integer

        Try
            If Not IsNothing(Data) Then
                For y = 0 To UBound(Data, 2)
                    Dim lvi As New ListViewItem
                    lvi.Text = "***"
                    CtrlTable.Table.Items.Add(lvi)
                    For i = 0 To CtrlTable.Table.Items.Count - 1
                        If CtrlTable.Table.Items(i).Text = "***" Then
                            CtrlTable.Table.Items(i).Text = ""

                            For x = 1 To UBound(CtrlTable.mCol)
                                CtrlTable.Table.Items(i).SubItems.Add("")
                            Next

                            For x = 0 To UBound(Data, 1)
                                CtrlTable.Table.Items(i).SubItems(x).Text = Data(x, y)
                            Next
                            Exit For
                        End If
                    Next
                Next
            End If
        Catch e As Exception
            MsgBox(e.Message)
        End Try

    End Sub

    Public Overrides Sub AfterEdit(ByVal TableIndex As Integer)

    End Sub

    Public Overrides Sub AfterAdd(ByVal TableIndex As Integer)

    End Sub

    Public Overrides Sub BeginAdd()
        CtrlTable.mCol(0).ctrlEnabled = True
        CtrlTable.mCol(1).ctrlEnabled = True
        CtrlTable.mCol(2).ctrlEnabled = True
        CtrlTable.mCol(3).ctrlEnabled = False        
    End Sub

    Public Overrides Sub BeginEdit()
        CtrlTable.mCol(0).ctrlEnabled = False
        CtrlTable.mCol(1).ctrlEnabled = False
        CtrlTable.mCol(2).ctrlEnabled = False
        CtrlTable.mCol(3).ctrlEnabled = False        
    End Sub

    Public Overrides Sub ProcessEntry(ByVal ctrl As SFDCData.ctrlText, ByVal Valid As Boolean)

        Select Case Valid
            Case False
                Beep()

            Case True
                Try

                    CtrlForm.el(1).CtrlEnabled = Len(CtrlForm.el(0).Data) > 0

                    If Len(CtrlForm.el(0).Data) > 0 And Len(CtrlForm.el(1).Data) > 0 Then
                        CtrlTable.Table.Focus()
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
                .Table = "ZSFDC_LOADALINE" ' specifies the name of the Priority loading table
                .Procedure = "ZSFDC_LOADALINE" ' the name of the procedure                
                .RecordType1 = "SERIALNAME," & _
                                "PARTNAME," & _
                                "ACTNAME," & _
                                "WORKCNAME," & _
                                "USERID," & _
                                "QUANT," & _
                                "SQUANT," & _
                                "DEFECTCODE," & _
                                "MQUANT," & _
                                "TQUANT," & _
                                "TSQUANT," & _
                                "TMQUANT," & _
                                "NUMPACK," & _
                                "PACKCODE," & _
                                "STIME," & _
                                "ETIME," & _
                                "ASPAN," & _
                                "EMPSTIME," & _
                                "EMPETIME," & _
                                "EMPASPAN," & _
                                "SHIFTNAME," & _
                                "TOOLNAME," & _
                                "RTYPEBOOL," & _
                                "MODE," & _
                                "WARHSNAME," & _
                                "LOCNAME," & _
                                "NEWPALLET," & _
                                "TOPALLETNAME," & _
                                "ACTCANCEL," & _
                                "SERCANCEL"

                .RecordType2 = "T2" ' and type 2 columns (ie will be loaded as type 2)

                .RecordTypes = "TEXT," & _
                                    "TEXT," & _
                                    "TEXT," & _
                                    "TEXT," & _
                                    "," & _
                                    "," & _
                                    "," & _
                                    "TEXT," & _
                                    "," & _
                                    "," & _
                                    "," & _
                                    "," & _
                                    "," & _
                                    "TEXT," & _
                                    "," & _
                                    "," & _
                                    "," & _
                                    "," & _
                                    "," & _
                                    "," & _
                                    "TEXT," & _
                                    "TEXT," & _
                                    "TEXT," & _
                                    "TEXT," & _
                                    "TEXT," & _
                                    "TEXT," & _
                                    "TEXT," & _
                                    "TEXT," & _
                                    "TEXT," & _
                                    "TEXT,TEXT"
                'defines the type of data to be loaded ie with apostrophes (or without) around the data to define it as text or numeric. Values may be TEXT or blank (for numbers)."
            End With

            For y As Integer = 0 To CtrlTable.RowCount
                Dim t1() As String = { _
                        String.Format(CtrlTable.ItemValue("_SERIALNAME", y), "M,CHAR,22,Work Order"), _
                                String.Format("", "M,CHAR,15,Part Number"), _
                                String.Format("", "CHAR,16,Operation"), _
                                String.Format("", "CHAR,6,Work Cell"), _
                                String.Format("0", "INT,16,0,Employee ID"), _
                                String.Format("0", "INT,17,3,Qty Completed"), _
                                String.Format("0", "INT,17,3,Qty Rejected"), _
                                String.Format("", "CHAR,3,Defect Code"), _
                                String.Format(CStr(CInt(CtrlTable.ItemValue("_MQUANT", y)) * 1000), "INT,17,3,Qty for MRB"), _
                                String.Format("0", "INT,17,3,Completed (Buy/Sell)"), _
                                String.Format("0", "INT,17,3,Rejected (Buy/Sell)"), _
                                String.Format("0", "INT,17,3,MRB (Buy/Sell Units)"), _
                                String.Format("0", "INT,6,Packing Crates (No.)"), _
                                String.Format("", "CHAR,2,Packing Crate Code"), _
                                String.Format("0", "TIME,5,Start Time"), _
                                String.Format("0", "TIME,5,End Time"), _
                                String.Format("0", "TIME,6,Span"), _
                                String.Format("0", "TIME,5,Start Labor"), _
                                String.Format("0", "TIME,5,End Labor"), _
                                String.Format("0", "TIME,6,Labor Span"), _
                                String.Format("", "CHAR,8,Shift"), _
                                String.Format("", "CHAR,15,Tool"), _
                                String.Format("", "CHAR,1,Rework?"), _
                                String.Format("", "CHAR,1,Set-up/Break (S/B)"), _
                                String.Format(CtrlForm.ItemValue("WARHS"), "CHAR,4,To Warehouse"), _
                                String.Format(CtrlForm.ItemValue("LOCNAME"), "CHAR,14,Bin"), _
                                String.Format("", "CHAR,1,New Pallet?"), _
                                String.Format(CtrlTable.ItemValue("_SERIALNAME", y) & "/" & CtrlTable.ItemValue("_TOPALLET", y), "CHAR,16,To Pallet"), _
                                String.Format("", "CHAR,1,Remove Oper. Number?"), _
                                String.Format("", "CHAR,1,Remove Wk Order No.?") _
                        } 'Type1 records to load'

                p.AddRecord(1) = t1
                Dim t2() As String = {""}
                p.AddRecord(2) = t2

            Next



        Catch e As Exception
            MsgBox(e.Message)
        End Try

    End Sub

    Public Overrides Sub TableScan(ByVal Value As String)
        Dim count As Integer = 0
        For Each ch As String In Value.ToCharArray
            If String.Compare(ch, "/") = 0 Then
                count += 1
            End If
        Next
        If count > 1 Then
            With CtrlTable.Table.Items
                .Add(New ListViewItem)
                With .Item(.Count - 1)
                    .SubItems(0).Text = Value.Split("/")(0)
                    .SubItems.Add(New System.Windows.Forms.ListViewItem.ListViewSubItem)
                    .SubItems(.SubItems.Count - 1).Text = Value.Split("/")(1)
                    .SubItems.Add(New System.Windows.Forms.ListViewItem.ListViewSubItem)
                    .SubItems(.SubItems.Count - 1).Text = Value.Split("/")(2)
                    .SubItems.Add(New System.Windows.Forms.ListViewItem.ListViewSubItem)
                    .SubItems(.SubItems.Count - 1).Text = "MRB"
                End With
            End With
        End If
    End Sub

    Public Overrides Sub EndInvokeData(ByVal Data(,) As String)

    End Sub

End Class