Public Class InterfacePARTLU
    Inherits SFDCData.iForm

    Public Sub New(Optional ByRef App As Form = Nothing)

        InitializeComponent()
        CallerApp = App
        NewArgument("BARCODE", "")
        CtrlTable.DisableButtons(True, False, True, True, False)
        CtrlTable.EnableToolbar(False, False, False, False, False)

    End Sub

    Public Overrides Sub FormLoaded()
        MyBase.FormLoaded()
        If Len(Argument("BARCODE")) > 0 Then
            With CtrlForm.el(0)
                .DataEntry.Text = Argument("BARCODE")
                .ProcessEntry()
            End With
        End If
    End Sub

    Public Overrides Sub FormSettings()

        'Part Name
        With field
            .Name = "PARTNAME"
            .Title = "Part No"
            .ValidExp = ValidStr(tRegExValidation.tBarcode)
            .SQLValidation = "SELECT BARCODE, PARTNAME " & _
                            "FROM PART " & _
                            "WHERE BARCODE = '%ME%'"
            .Data = ""      '******** Barcoded field '*******
            .AltEntry = ctrlText.tAltCtrlStyle.ctList 'ctNone 'ctKeyb 
            .ctrlEnabled = True
        End With
        CtrlForm.AddField(field)

        'Part Description
        With field
            .Name = "PARTDES"
            .Title = "Part"
            .ValidExp = "^.+$"
            .SQLValidation = ""
            .Data = ""      '******** Barcoded field '*******
            .AltEntry = ctrlText.tAltCtrlStyle.ctList 'ctNone 'ctKeyb 
            .ctrlEnabled = False
        End With
        CtrlForm.AddField(field)

        'FROM WARHSNAME
        With field
            .Name = "WARHS"
            .Title = "Default W/H"
            .ValidExp = "^.+$"
            .SQLList = ""
            .SQLValidation = ""
            .Data = ""      '******** Barcoded field '*******
            .AltEntry = ctrlText.tAltCtrlStyle.ctList 'ctNone 'ctKeyb 
            .ctrlEnabled = False
        End With
        CtrlForm.AddField(field)

        'LOCNAME
        With field
            .Name = "LOCNAME"
            .Title = "Default Bin"
            .ValidExp = "^.+$"
            .SQLList = ""
            .SQLValidation = ""
            .Data = ""      '******** Barcoded field '*******
            .AltEntry = ctrlText.tAltCtrlStyle.ctList 'ctNone 'ctKeyb 
            .ctrlEnabled = False
        End With
        CtrlForm.AddField(field)

    End Sub

    Public Overrides Sub TableSettings()

        ' WARHSNAME
        With col
            .Name = "_WARHS"
            .Title = "W/H"
            .initWidth = 25
            .TextAlign = HorizontalAlignment.Left
            .AltEntry = ctrlText.tAltCtrlStyle.ctList 'ctKeyb 
            .ValidExp = ValidStr(tRegExValidation.tWarehouse)
            .SQLList = "SELECT DISTINCT WARHSNAME FROM WAREHOUSES"
            .SQLValidation = "SELECT WARHSNAME FROM WAREHOUSES WHERE WARHSNAME = '%ME%'"
            .DefaultFromCtrl = Nothing
            .ctrlEnabled = False
            .Mandatory = False
        End With
        CtrlTable.AddCol(col)

        ' LOCNAME
        With col
            .Name = "_LOCNAME"
            .Title = "Bin"
            .initWidth = 25
            .TextAlign = HorizontalAlignment.Left
            .AltEntry = ctrlText.tAltCtrlStyle.ctList ' ctNone 'ctKeyb 
            .ValidExp = ValidStr(tRegExValidation.tLocname)
            .SQLList = "SELECT DISTINCT LOCNAME FROM WAREHOUSES WHERE WARHSNAME = '%WARHS%'"
            .SQLValidation = "SELECT LOCNAME FROM WAREHOUSES WHERE LOCNAME = '%ME%' AND WARHSNAME = '%_WARHS%'"
            .DefaultFromCtrl = Nothing      '******* Barcoded Field - default from Type1 LOCNAME '*******
            .ctrlEnabled = False
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

        ' TQUANT
        With col
            .Name = "_TQUANT"
            .Title = "Qty"
            .initWidth = 20
            .TextAlign = HorizontalAlignment.Left
            .AltEntry = ctrlText.tAltCtrlStyle.ctNone 'ctKeyb 
            .ValidExp = ValidStr(tRegExValidation.tNumeric)
            .SQLValidation = ""
            .DefaultFromCtrl = Nothing
            .ctrlEnabled = False
            .Mandatory = False
        End With
        CtrlTable.AddCol(col)

        ' Counted Quantity
        With col
            .Name = "_CQUANT"
            .Title = "Updated QTY"
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

        ' Set the query to load recordtype 2s
        CtrlTable.RecordsSQL = "select WARHSNAME, LOCNAME , CUSTOMERS.CUSTNAME, dbo.REALQUANT(BALANCE) as BALANCE, '' AS CQUANT " & _
                                "from WARHSBAL, WAREHOUSES, CUSTOMERS " & _
                                "where WARHSBAL.WARHS = WAREHOUSES.WARHS " & _
                                "and WARHSBAL.CUST = CUSTOMERS.CUST " & _
                                "and PART =  " & _
                                "(select PART from PART where PARTNAME = '%PARTNAME%') " & _
                                "and BALANCE <> 0"

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
        CtrlTable.mCol(4).ctrlEnabled = True
    End Sub

    Public Overrides Sub BeginEdit()
        CtrlTable.mCol(0).ctrlEnabled = False
        CtrlTable.mCol(1).ctrlEnabled = False
        CtrlTable.mCol(2).ctrlEnabled = False
        CtrlTable.mCol(3).ctrlEnabled = False
        CtrlTable.mCol(4).ctrlEnabled = True
    End Sub

    Public Overrides Sub ProcessEntry(ByVal ctrl As SFDCData.ctrlText, ByVal Valid As Boolean)

        Select Case Valid
            Case False
                Beep()

            Case True
                Try

                    If Len(CtrlForm.el(0).Data) > 0 Then

                        'Get description for selected part
                        CtrlForm.el(1).NSInvoke("select PARTDES " & _
                            "from PARTPARAM, PART, WAREHOUSES " & _
                            "where PARTPARAM.PART = PART.PART  " & _
                            "and PARTPARAM.WARHS = WAREHOUSES.WARHS " & _
                            "and PART.PART =  " & _
                            "(select PART from PART where PARTNAME = '" & CtrlForm.el(0).Data & "')")

                        'Get default warehouse for selected part
                        CtrlForm.el(2).NSInvoke("select  WARHSNAME " & _
                            "from PARTPARAM, PART, WAREHOUSES " & _
                            "where PARTPARAM.PART = PART.PART  " & _
                            "and PARTPARAM.WARHS = WAREHOUSES.WARHS " & _
                            "and PART.PART =  " & _
                            "(select PART from PART where PARTNAME = '" & CtrlForm.el(0).Data & "')")

                        'Get default bin for selected part
                        CtrlForm.el(3).NSInvoke("select LOCNAME " & _
                            "from PARTPARAM, PART, WAREHOUSES " & _
                            "where PARTPARAM.PART = PART.PART  " & _
                            "and PARTPARAM.WARHS = WAREHOUSES.WARHS " & _
                            "and PART.PART =  " & _
                            "(select PART from PART where PARTNAME = '" & CtrlForm.el(0).Data & "')")

                        CtrlTable.Table.Items.Clear()
                        CtrlTable.BeginLoadRS()

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
                .DebugFlag = True
                .Procedure = ""
                .Table = ""
                .RecordType1 = ""
                .RecordType2 = ""
                .RecordTypes = ""
            End With

            ' Type 1 records
            Dim t1() As String = { _
                                CtrlForm.ItemValue("") _
                                }
            p.AddRecord(1) = t1

            For y As Integer = 0 To CtrlTable.RowCount
                Dim t2() As String = { _
                            CtrlTable.ItemValue("", y) _
                                    }
                p.AddRecord(2) = t2
            Next

        Catch e As Exception
            MsgBox(e.Message)
        End Try

    End Sub

    Public Overrides Sub TableScan(ByVal Value As String)

    End Sub

    Public Overrides Sub EndInvokeData(ByVal Data(,) As String)

    End Sub

End Class