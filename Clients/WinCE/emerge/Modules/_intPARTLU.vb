Imports System
Imports System.IO
Imports System.Threading
Imports System.Text.RegularExpressions

Public Class InterfacePARTLU
    Inherits SFDCData.iForm

    Private Enum tInvoke
        iCycle = 0
    End Enum
    Private mInvoke As tInvoke

#Region "Initialisation and Finalisation"

    Public Sub New(Optional ByRef App As Form = Nothing)

        InitializeComponent()
        CallerApp = App
        NewArgument("BARCODE", "")
        NewArgument("MYPARTNAME", "")
        NewArgument("ABC", "")
        CtrlTable.DisableButtons(False, False, True, False, False)
        CtrlTable.EnableToolbar(False, False, False, False, False)

    End Sub

    Public Overrides Sub FormLoaded()
        MyBase.FormLoaded()
        If Argument("BARCODE").Length > 0 Then
            With CtrlForm.el(ColNo("PARTNAME"))
                .DataEntry.Text = Argument("BARCODE")
                .ProcessEntry()
            End With
        End If
        If Argument("ABC").Length > 0 Then
            mInvoke = tInvoke.iCycle
            InvokeData("select dbo.ZSFDCFunc_CYCLEPART('" & Argument("ABC") & "')")
        End If
    End Sub

    Public Overrides Sub FormClose()
        MyBase.FormClose()
        Argument("BARCODE") = ""
        Argument("MYPARTNAME") = ""
    End Sub

#End Region

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
            .Name = "_WARHSNAME"
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
            .SQLList = "SELECT DISTINCT LOCNAME FROM WAREHOUSES WHERE WARHSNAME = '%_WARHSNAME%'"
            .SQLValidation = "SELECT LOCNAME FROM WAREHOUSES WHERE LOCNAME = '%ME%' AND WARHSNAME = '%_WARHSNAME%'"
            .DefaultFromCtrl = Nothing      '******* Barcoded Field - default from Type1 LOCNAME '*******
            .ctrlEnabled = False
            .Mandatory = False
        End With
        CtrlTable.AddCol(col)

        ' STATUS
        With col
            .Name = "_TOCUSTNAME"
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
            .Name = "_TOSERIALNAME"
            .Title = "Serial"
            .initWidth = 50
            .TextAlign = HorizontalAlignment.Left
            .AltEntry = ctrlText.tAltCtrlStyle.ctNone 'ctKeyb 
            .ValidExp = ValidStr(tRegExValidation.tSerial) & "|^0$"
            .SQLValidation = "SELECT %ME%"
            .DefaultFromCtrl = Nothing 'CtrlForm.el(ColNo("_LOCNAME"))  '******* Barcoded Field - default from Type1 TOLOCATION '*******
            .ctrlEnabled = False
            .Mandatory = True
            .MandatoryOnPost = True
        End With
        CtrlTable.AddCol(col)

        'ACT NAME
        With col
            .Name = "_ACTNAME"
            .Title = "Act"
            .initWidth = 50
            .TextAlign = HorizontalAlignment.Left
            .AltEntry = ctrlText.tAltCtrlStyle.ctList 'ctKeyb 
            .ValidExp = "^.+$"
            .SQLList = "SELECT DISTINCT ACTNAME FROM ACT "
            .SQLValidation = "SELECT %ME%"
            .DefaultFromCtrl = Nothing 'CtrlForm.el(ColNo("_LOCNAME"))  '******* Barcoded Field - default from Type1 TOLOCATION '*******
            .ctrlEnabled = False
            .Mandatory = False
            .MandatoryOnPost = False
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
            .SQLValidation = "SELECT %ME%"
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
            .SQLValidation = "SELECT %ME%"
            .DefaultFromCtrl = Nothing
            .ctrlEnabled = True
            .Mandatory = False
            .MandatoryOnPost = Argument("ABC").Length > 0

        End With
        CtrlTable.AddCol(col)

        ' Set the query to load recordtype 2s
        CtrlTable.RecordsSQL = "select * from dbo.ZSFDC_PARTLOOKUP('%PARTNAME%')"

    End Sub

    Public Overrides Sub TableRXData(ByVal Data(,) As String)
        Try
            With CtrlTable.Table
                .Items.Clear()
                If Not IsNothing(Data) Then
                    For y As Integer = 0 To UBound(Data, 2)
                        Dim lvi As New ListViewItem
                        .Items.Add(lvi)
                        .Items(.Items.Count - 1).Text = Data(0, y)
                        For i As Integer = 1 To UBound(Data, 1)
                            .Items(.Items.Count - 1).SubItems.Add(Data(i, y))
                        Next
                    Next
                End If
            End With
        Catch e As Exception
            MsgBox(e.Message)
        End Try
    End Sub

    Public Overrides Sub AfterEdit(ByVal TableIndex As Integer)

    End Sub

    Public Overrides Sub BeginAdd()
        CtrlTable.mCol(ColNo("_WARHSNAME")).ctrlEnabled = True
        CtrlTable.mCol(ColNo("_LOCNAME")).ctrlEnabled = True
        CtrlTable.mCol(ColNo("_TOCUSTNAME")).ctrlEnabled = True
        CtrlTable.mCol(ColNo("_TQUANT")).ctrlEnabled = False
        CtrlTable.mCol(ColNo("_CQUANT")).ctrlEnabled = True
        CtrlTable.mCol(ColNo("_ACTNAME")).ctrlEnabled = True
        CtrlTable.mCol(ColNo("_TOSERIALNAME")).ctrlEnabled = True

    End Sub

    Public Overrides Sub AfterAdd(ByVal TableIndex As Integer)
        Try
            CtrlTable.Table.Items(TableIndex).SubItems(ColNo("_TQUANT")).Text = "0"
        Catch e As Exception
            MsgBox(e.Message)
        End Try
    End Sub

    Public Overrides Sub BeginEdit()
        CtrlTable.mCol(ColNo("_WARHSNAME")).ctrlEnabled = False
        CtrlTable.mCol(ColNo("_LOCNAME")).ctrlEnabled = False
        CtrlTable.mCol(ColNo("_TOCUSTNAME")).ctrlEnabled = False
        CtrlTable.mCol(ColNo("_TQUANT")).ctrlEnabled = False
        CtrlTable.mCol(ColNo("_CQUANT")).ctrlEnabled = True
        CtrlTable.mCol(ColNo("_ACTNAME")).ctrlEnabled = False
        CtrlTable.mCol(ColNo("_TOSERIALNAME")).ctrlEnabled = False
    End Sub

    Public Overrides Sub ProcessEntry(ByVal ctrl As SFDCData.ctrlText, ByVal Valid As Boolean)

        Select Case Valid
            Case False
                Beep()

            Case True
                Try

                    If Len(Val("PARTNAME")) > 0 Then
                        If (Not (Argument("MYPARTNAME") = Val("PARTNAME"))) Or Len(Val("PARTDES")) = 0 Then

                            Argument("MYPARTNAME") = Val("PARTNAME")

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
                    End If

                    ' *******************************************************************
                    ' *** Set which buttons are enabled   
                    With CtrlTable
                        .EnableToolbar( _
                            CBool(.Table.Visible And _
                            Len(Val("PARTNAME")) > 0), _
                            .btnEnabled(SFDCData.CtrlTable.tButton.btnEdit), _
                            .btnEnabled(SFDCData.CtrlTable.tButton.btnCopy), _
                            .btnEnabled(SFDCData.CtrlTable.tButton.btnDelete), _
                            .btnEnabled(SFDCData.CtrlTable.tButton.btnPost) _
                        )
                    End With

                Catch
                End Try

        End Select

    End Sub

    Public Overrides Function verifyForm() As Boolean
        Return True
    End Function

    Public Overrides Sub ProcessForm()

        Dim cont As Boolean = False
        With CtrlTable
            For y As Integer = 0 To .RowCount
                If .ItemValue("_CQUANT", y).Length > 0 Then
                    cont = True
                    Exit For
                End If
            Next
        End With

        If Not cont Then
            p.NoData = True
            Exit Sub
        End If

        Try
            With p
                .DebugFlag = True
                .Procedure = "ZSFDCP_LOAD_COUNT"
                .Table = "ZSFDC_COUNT_LOAD"
                .RecordType1 = "OWNERLOGIN,WARHSNAME"
                .RecordType2 = "OWNERLOGIN,WARHSNAME,LOCNAME,PARTNAME,TOCUSTNAME,TOSERIALNAME,ACTNAME,TQUANT,CQUANT,ABC"
                .RecordTypes = "TEXT,TEXT,TEXT,TEXT,TEXT,TEXT,TEXT,TEXT,TEXT,,,TEXT"
            End With

            With CtrlTable
                For y As Integer = 0 To .RowCount
                    If .ItemValue("_CQUANT", y).Length > 0 Then
                        ' Type 1 records
                        Dim t1() As String = { _
                                            UserName, _
                                            CtrlTable.ItemValue("_WARHSNAME", y) _
                                            }
                        p.AddRecord(1) = t1

                        Dim t2() As String = { _
                            UserName, _
                            .ItemValue("_WARHSNAME", y), _
                            .ItemValue("_LOCNAME", y), _
                            CtrlForm.ItemValue("PARTNAME"), _
                            .ItemValue("_TOCUSTNAME", y), _
                            .ItemValue("_TOSERIALNAME", y), _
                            .ItemValue("_ACTNAME", y), _
                            CStr(CInt(.ItemValue("_TQUANT", y)) * 1000), _
                            CStr(CInt(.ItemValue("_CQUANT", y)) * 1000), _
                            Argument("ABC") _
                        }
                        p.AddRecord(2) = t2
                    End If
                Next
            End With

        Catch e As Exception
            MsgBox(e.Message)
        End Try

    End Sub

    Public Overrides Sub TableScan(ByVal Value As String)

    End Sub

    Public Overrides Sub EndInvokeData(ByVal Data(,) As String)

        If Not IsNothing(Data) Then
            Select Case mInvoke
                Case tInvoke.iCycle
                    With CtrlForm.el(ColNo("PARTNAME"))
                        .DataEntry.Text = Data(0, 0)
                        .ProcessEntry()
                    End With
            End Select
        End If
    End Sub

End Class