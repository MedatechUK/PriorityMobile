Imports System
Imports System.IO
Imports System.Threading
Imports System.Text.RegularExpressions

Public Class InterfacePARTLU
    Inherits w32SFDCData.iForm

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
            .ValidExp = "^.+$"
            .SQLValidation = "SELECT BARCODE, PARTNAME " & _
                            "FROM dbo.PARTALIAS() " & _
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

        'SERIALNAME
        With col
            .Name = "_SERIALNAME"
            .Title = "Serial"
            .initWidth = 50
            .TextAlign = HorizontalAlignment.Left
            .AltEntry = ctrlText.tAltCtrlStyle.ctList 'ctKeyb 
            .ValidExp = ValidStr(tRegExValidation.tSerial)
            .SQLList = "SELECT DISTINCT SERIALNAME FROM SERIAL, WARHSBAL, WAREHOUSES " & _
                        "WHERE SERIAL.SERIAL = WARHSBAL.SERIAL " & _
                        "AND WAREHOUSES.WARHS = WARHSBAL.WARHS " & _
                        "AND WARHSBAL.PART = (SELECT PART FROM PART WHERE PARTNAME = '%_PARTNAME%') " & _
                        "AND WAREHOUSES.WARHSNAME = '%WARHS%' AND WAREHOUSES.LOCNAME = '%LOCNAME%' " & _
                        "AND WARHSBAL.BALANCE >0"
            .SQLValidation = "SELECT DISTINCT SERIALNAME FROM SERIAL, WARHSBAL, WAREHOUSES " & _
                        "WHERE SERIAL.SERIAL = WARHSBAL.SERIAL " & _
                        "AND WAREHOUSES.WARHS = WARHSBAL.WARHS " & _
                        "AND WARHSBAL.PART = (SELECT PART FROM PART WHERE PARTNAME = '%_PARTNAME%') " & _
                        "AND SERIALNAME = '%ME%' AND WAREHOUSES.WARHSNAME = '%WARHS%' AND WAREHOUSES.LOCNAME = '%LOCNAME%'"
            .DefaultFromCtrl = Nothing 'CtrlForm.el(ColNo("_LOCNAME"))  '******* Barcoded Field - default from Type1 TOLOCATION '*******
            .ctrlEnabled = True
            .Mandatory = True
            .MandatoryOnPost = True
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
        CtrlTable.RecordsSQL = "select WARHSNAME, LOCNAME , CUSTOMERS.CUSTNAME, SERIAL.SERIALNAME, dbo.REALQUANT(BALANCE) as BALANCE, '' AS CQUANT " & _
                                "from WARHSBAL, WAREHOUSES, CUSTOMERS, SERIAL " & _
                                "where WARHSBAL.WARHS = WAREHOUSES.WARHS " & _
                                "and WARHSBAL.CUST = CUSTOMERS.CUST " & _
                                "AND SERIAL.SERIAL = WARHSBAL.SERIAL " & _
                                "and WARHSBAL.PART =  " & _
                                "(select PART from PART where PARTNAME = '%PARTNAME%') " & _
                                "and BALANCE <> 0 AND ACT = 0"
    End Sub

    Public Overrides Sub TableRXData(ByVal Data(,) As String)
        Try
            With CtrlTable.Table
                .Items.Clear()
                For y As Integer = 0 To UBound(Data, 2)
                    Dim lvi As New ListViewItem
                    .Items.Add(lvi)
                    .Items(.Items.Count - 1).Text = Data(0, y)
                    For i As Integer = 1 To UBound(Data, 1)
                        .Items(.Items.Count - 1).SubItems.Add(Data(i, y))
                    Next
                Next
            End With
        Catch e As Exception
            MsgBox(e.Message)
        End Try
    End Sub

    Public Overrides Sub AfterEdit(ByVal TableIndex As Integer)

    End Sub

    Public Overrides Sub BeginAdd()
        CtrlTable.mCol(ColNo("_WARHS")).ctrlEnabled = True
        CtrlTable.mCol(ColNo("_LOCNAME")).ctrlEnabled = True
        CtrlTable.mCol(ColNo("_STATUS")).ctrlEnabled = True
        CtrlTable.mCol(ColNo("_TQUANT")).ctrlEnabled = False
        CtrlTable.mCol(ColNo("_CQUANT")).ctrlEnabled = True
    End Sub

    Public Overrides Sub BeginEdit()
        CtrlTable.mCol(ColNo("_WARHS")).ctrlEnabled = False
        CtrlTable.mCol(ColNo("_LOCNAME")).ctrlEnabled = False
        CtrlTable.mCol(ColNo("_STATUS")).ctrlEnabled = False
        CtrlTable.mCol(ColNo("_TQUANT")).ctrlEnabled = False
        CtrlTable.mCol(ColNo("_CQUANT")).ctrlEnabled = True
    End Sub

    Public Overrides Sub ProcessEntry(ByVal ctrl As w32SFDCData.ctrlText, ByVal Valid As Boolean)

        Select Case Valid
            Case False
                Beep()

            Case True
                Try

                    If Len(Val("PARTNAME")) > 0 Then

                        'Get description for selected part
                        CtrlForm.el(ColNo("PARTDES")).NSInvoke("select PARTDES " & _
                            "from PARTPARAM, PART, WAREHOUSES " & _
                            "where PARTPARAM.PART = PART.PART  " & _
                            "and PARTPARAM.WARHS = WAREHOUSES.WARHS " & _
                            "and PART.PART =  " & _
                            "(select PART from PART where PARTNAME = '" & Val("PARTNAME") & "')")

                        'Get default warehouse for selected part
                        CtrlForm.el(ColNo("WARHS")).NSInvoke("select  WARHSNAME " & _
                            "from PARTPARAM, PART, WAREHOUSES " & _
                            "where PARTPARAM.PART = PART.PART  " & _
                            "and PARTPARAM.WARHS = WAREHOUSES.WARHS " & _
                            "and PART.PART =  " & _
                            "(select PART from PART where PARTNAME = '" & Val("PARTNAME") & "')")

                        'Get default bin for selected part
                        CtrlForm.el(ColNo("LOCNAME")).NSInvoke("select LOCNAME " & _
                            "from PARTPARAM, PART, WAREHOUSES " & _
                            "where PARTPARAM.PART = PART.PART  " & _
                            "and PARTPARAM.WARHS = WAREHOUSES.WARHS " & _
                            "and PART.PART =  " & _
                            "(select PART from PART where PARTNAME = '" & Val("PARTNAME") & "')")

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