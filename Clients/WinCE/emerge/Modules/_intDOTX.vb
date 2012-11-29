Public Class InterfaceDOTX
    Inherits SFDCData.iForm

#Region "Initialisation"

    Public Sub New(Optional ByRef App As Form = Nothing)

        InitializeComponent()
        CallerApp = App
        NewArgument("WTNO", "")
        CtrlTable.DisableButtons(True, False, False, True, False)

    End Sub

#End Region

    Public Overrides Sub FormLoaded()
        MyBase.FormLoaded()
        With CtrlForm.el(0)
            .DataEntry.Text = Argument("WTNO")
            .ProcessEntry()
        End With
    End Sub

    Public Overrides Sub FormSettings()

        ' DOCNO
        With field
            .Name = "DOCNO"
            .Title = "Doc"
            .ValidExp = "^.+$"
            ' Second field replaces first field if first field validates ok
            .SQLValidation = "select DOCNO from DOCUMENTS " & _
                             "where TYPE = 'T' " & _
                             "AND DOCNO = '%ME%'"
            .Data = ""      '******** Barcoded field '*******
            .AltEntry = ctrlText.tAltCtrlStyle.ctNone 'ctNone 'ctKeyb 
            .ctrlEnabled = False
        End With
        CtrlForm.AddField(field)

        'Status
        With field
            .Name = "STATDES"
            .Title = "Status"
            .ValidExp = ValidStr(tRegExValidation.tStatus)
            .SQLList = "select STATDES from DOCSTATS where TYPE = 'T'"
            .SQLValidation = "select STATDES from DOCSTATS where TYPE = 'T' and STATDES = '%ME%'"
            .Data = "Request"      '******** Barcoded field '*******
            .AltEntry = ctrlText.tAltCtrlStyle.ctList 'ctNone 'ctKeyb 
            .ctrlEnabled = True
        End With
        CtrlForm.AddField(field)

    End Sub

    Public Overrides Sub TableSettings()

        ' BARCODE
        With col
            .Name = "_PARTNAME"
            .Title = "Part No"
            .initWidth = 30
            .TextAlign = HorizontalAlignment.Left
            .AltEntry = ctrlText.tAltCtrlStyle.ctNone 'ctKeyb 
            .ValidExp = ValidStr(tRegExValidation.tBarcode)
            ' Second field replaces first field if first field validates ok
            .SQLValidation = "SELECT BARCODE, PARTNAME " & _
                            "FROM PART, WARHSBAL, WAREHOUSES, CUSTOMERS " & _
                            "WHERE PART.PART = WARHSBAL.PART  " & _
                            "AND WARHSBAL.WARHS = WAREHOUSES.WARHS  " & _
                            "AND WARHSBAL.CUST = CUSTOMERS.CUST " & _
                            "AND WAREHOUSES.WARHSNAME = '%WARHS%'  " & _
                            "AND WAREHOUSES.LOCNAME = '%_LOCNAME%'  " & _
                            "AND WARHSBAL.BALANCE > 0 " & _
                            "AND CUSTNAME = '%_STATUS%' " & _
                            "AND BARCODE = '%ME%'"
            .DefaultFromCtrl = Nothing '
            .ctrlEnabled = False
            .Mandatory = True
        End With
        CtrlTable.AddCol(col)

        'FROM WARHSNAME
        With col
            .Name = "_WARHS"
            .Title = "From W/H"
            .initWidth = 30
            .TextAlign = HorizontalAlignment.Left
            .AltEntry = ctrlText.tAltCtrlStyle.ctList 'ctKeyb 
            .ValidExp = ValidStr(tRegExValidation.tWarehouse)
            .SQLList = "SELECT DISTINCT WARHSNAME FROM WAREHOUSES WHERE INACTIVE <> 'Y'"
            .SQLValidation = "SELECT WARHSNAME FROM WAREHOUSES WHERE WARHSNAME = '%ME%'"
            .DefaultFromCtrl = Nothing 'CtrlForm.el(0) '
            .ctrlEnabled = False
            .Mandatory = True
        End With
        CtrlTable.AddCol(col)

        ' FROM LOCNAME
        With col
            .Name = "_LOCNAME"
            .Title = "From Bin"
            .initWidth = 30
            .TextAlign = HorizontalAlignment.Left
            .AltEntry = ctrlText.tAltCtrlStyle.ctList ' ctNone 'ctKeyb 
            .ValidExp = ValidStr(tRegExValidation.tLocname)
            .SQLList = "SELECT DISTINCT LOCNAME FROM WAREHOUSES WHERE WARHSNAME = '%_WARHS%' AND INACTIVE <> 'Y'"
            .SQLValidation = "SELECT LOCNAME FROM WAREHOUSES WHERE LOCNAME = '%ME%' AND WARHSNAME = '%_WARHS%'"
            .DefaultFromCtrl = Nothing 'CtrlForm.el(1)      '******* Barcoded Field - default from Type1 LOCNAME '*******
            .ctrlEnabled = True
            .Mandatory = True
        End With
        CtrlTable.AddCol(col)

        'TOWARHSNAME
        With col
            .Name = "_TOWARHS"
            .Title = "To W/H"
            .initWidth = 30
            .TextAlign = HorizontalAlignment.Left
            .ValidExp = ValidStr(tRegExValidation.tWarehouse)
            .SQLList = "SELECT DISTINCT WARHSNAME FROM WAREHOUSES WHERE INACTIVE <> 'Y'"
            .SQLValidation = "SELECT WARHSNAME FROM WAREHOUSES WHERE WARHSNAME = '%ME%'"
            .AltEntry = ctrlText.tAltCtrlStyle.ctList 'ctKeyb 
            .DefaultFromCtrl = Nothing 'CtrlForm.el(2)
            .ctrlEnabled = False
        End With
        CtrlTable.AddCol(col)

        ' TOLOCNAME
        With col
            .Name = "_TOLOCNAME"
            .Title = "To Bin"
            .initWidth = 30
            .TextAlign = HorizontalAlignment.Left
            .AltEntry = ctrlText.tAltCtrlStyle.ctList 'ctKeyb 
            .ValidExp = ValidStr(tRegExValidation.tLocname)
            .SQLList = "SELECT DISTINCT LOCNAME FROM WAREHOUSES WHERE WARHSNAME = '%TOWARHS%' AND INACTIVE <> 'Y'"
            .SQLValidation = "SELECT LOCNAME FROM WAREHOUSES WHERE LOCNAME = '%ME%' AND WARHSNAME = '%_TOWARHS%'"
            .DefaultFromCtrl = Nothing 'CtrlForm.el(3)  '******* Barcoded Field - default from Type1 TOLOCATION '*******
            .ctrlEnabled = False
            .Mandatory = False
            .MandatoryOnPost = True
        End With
        CtrlTable.AddCol(col)

        'STATUS1
        With col
            .Name = "_STATUS"
            .Title = "From Status"
            .initWidth = 30
            .TextAlign = HorizontalAlignment.Left
            .AltEntry = ctrlText.tAltCtrlStyle.ctList ' ctNone 'ctKeyb 
            .ValidExp = ValidStr(tRegExValidation.tStatus)
            .SQLValidation = "SELECT CUSTNAME FROM CUSTOMERS WHERE STATUSFLAG = 'Y' AND CUSTNAME = '%ME%'"
            .SQLList = "SELECT CUSTNAME FROM CUSTOMERS WHERE STATUSFLAG = 'Y'"
            .DefaultFromCtrl = Nothing 'CtrlForm.el(4)  '******** Default to 'Goods' '*******
            .ctrlEnabled = False
            .Mandatory = True
            .MandatoryOnPost = True
        End With
        CtrlTable.AddCol(col)

        ' TQUANT
        With col
            .Name = "_TQUANT"
            .Title = "Qty"
            .initWidth = 30
            .TextAlign = HorizontalAlignment.Left
            .AltEntry = ctrlText.tAltCtrlStyle.ctNone 'ctKeyb 
            .ValidExp = ValidStr(tRegExValidation.tNumeric)
            .SQLValidation = "SELECT %ME% " & _
                            "FROM PART, WARHSBAL, WAREHOUSES, CUSTOMERS " & _
                            "WHERE PART.PART = WARHSBAL.PART  " & _
                            "AND WARHSBAL.WARHS = WAREHOUSES.WARHS  " & _
                            "AND WARHSBAL.CUST = CUSTOMERS.CUST " & _
                            "AND WAREHOUSES.WARHSNAME = '%WARHS%'  " & _
                            "AND WAREHOUSES.LOCNAME = '%_LOCNAME%'  " & _
                            "AND WARHSBAL.BALANCE/1000 > %ME% " & _
                            "AND CUSTNAME = '%_STATUS%' " & _
                            "AND PART.PARTNAME = '%_PARTNAME%'"
            .DefaultFromCtrl = Nothing
            .ctrlEnabled = True
            .Mandatory = True
        End With
        CtrlTable.AddCol(col)

        ' Trans
        With col
            .Name = "_TRANS"
            .Title = "TRANS"
            .initWidth = 30
            .TextAlign = HorizontalAlignment.Left
            .AltEntry = ctrlText.tAltCtrlStyle.ctNone 'ctKeyb 
            .ValidExp = ValidStr(tRegExValidation.tNumeric)
            .SQLValidation = "SELECT %ME%"
            .DefaultFromCtrl = Nothing
            .ctrlEnabled = False
            .Mandatory = False
        End With
        CtrlTable.AddCol(col)

        CtrlTable.RecordsSQL = "select PARTNAME,FROMWHS,FROMLOC,TOWHS, TOLOC,STATUS,QUANT,TRANS " & _
                        "from v_RequestedItems " & _
                        "where DOCNO = '%DOCNO%'"

    End Sub

    Public Overrides Sub TableRXData(ByVal Data(,) As String)

        Dim y As Integer
        Dim x As Integer
        Dim i As Integer

        Try
            For y = 0 To UBound(Data, 2)
                Dim lvi As New ListViewItem
                lvi.Text = "***"
                CtrlTable.Table.Items.Add(lvi)
                For i = 0 To CtrlTable.Table.Items.Count - 1
                    If CtrlTable.Table.Items(i).Text = "***" Then
                        CtrlTable.Table.Items(i).Text = Data(0, y)

                        For x = 1 To UBound(CtrlTable.mCol)
                            CtrlTable.Table.Items(i).SubItems.Add("")
                        Next

                        For x = 1 To UBound(Data, 1)
                            CtrlTable.Table.Items(i).SubItems(x).Text = Data(x, y)
                        Next

                        Exit For
                    End If
                Next
            Next
        Catch e As Exception
            MsgBox(e.Message)
        End Try

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
                    If ctrl.Name = "DOCNO" Then
                        CtrlTable.BeginLoadRS()
                    End If

                    ' Fill in the TOLOC when default changes (if empty)
                    If ctrl.Name = "TOLOCNAME" Then
                        Dim Y As Integer
                        For Y = 0 To CtrlTable.Table.Items.Count - 1
                            If Len(CtrlTable.Table.Items(Y).SubItems(2).Text) = 0 Then
                                CtrlTable.Table.Items(Y).SubItems(2).Text = ctrl.Data
                            End If
                        Next
                    End If

                    'If ctrl.Name = "_PARTNAME" Then
                    Try
                        If Len(CtrlForm.el(0).Data) > 0 And Len(CtrlTable.el(1).Data) > 0 And Len(CtrlForm.el(4).Data) > 0 And Len(CtrlTable.el(0).Data) > 0 Then
                            Dim sql As String = "SELECT WARHSBAL.BALANCE/1000 " & _
                                "FROM PART, WARHSBAL, WAREHOUSES, CUSTOMERS " & _
                                "WHERE PART.PART = WARHSBAL.PART  " & _
                                "AND WARHSBAL.WARHS = WAREHOUSES.WARHS  " & _
                                "AND WARHSBAL.CUST = CUSTOMERS.CUST " & _
                                "AND WAREHOUSES.WARHSNAME = '" & CtrlForm.el(0).Data & "'  " & _
                                "AND WAREHOUSES.LOCNAME = '" & CtrlTable.el(1).Data & "'  " & _
                                "AND CUSTNAME = '" & CtrlForm.el(4).Data & "' " & _
                                "AND PART.PARTNAME = '" & CtrlTable.el(0).Data & "'"
                            CtrlTable.el(5).NSInvoke(sql)
                        End If
                    Catch
                    End Try

                    ' *******************************************************************
                    ' *** Set which controls are enabled
                    CtrlForm.el(1).CtrlEnabled = Len(CtrlForm.el(0).Data) > 0
                    CtrlForm.el(3).CtrlEnabled = Len(CtrlForm.el(2).Data) > 0

                    CtrlTable.el(0).CtrlEnabled = Len(CtrlTable.el(1).Data) > 0 And Len(CtrlTable.el(3).Data) > 0
                    CtrlTable.el(1).CtrlEnabled = Len(CtrlForm.el(0).Data) > 0
                    CtrlTable.el(2).CtrlEnabled = Len(CtrlForm.el(2).Data) > 0
                    'CtrlTable.el(4).CtrlEnabled = Len(CtrlTable.el(3).Data) > 0
                    'CtrlTable.el(0).CtrlEnabled = Len(CtrlTable.el(1).Data) > 0 And Len(CtrlTable.el(2).Data) > 0 And Len(CtrlTable.el(5).Data) > 0
                    CtrlTable.el(6).CtrlEnabled = Len(CtrlTable.el(0).Data) > 0 And Len(CtrlTable.el(1).Data) > 0 And Len(CtrlTable.el(3).Data) > 0
                    ' ******************************************************************

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