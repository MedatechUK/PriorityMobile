Imports System.Threading

Public Class interfaceKITISSUE
    Inherits SFDCData.iForm
#Region "Initialisation"

    Private gr As Boolean = False
    Private route As String = ""
    Private defect As String = ""


    Public Sub New(Optional ByRef App As Form = Nothing)

        'InitializeComponent()
        CallerApp = App
        CtrlTable.DisableButtons(False, False, True, False, False)
        CtrlTable.EnableToolbar(False, False, False, False, True)


    End Sub

#End Region

    Public Enum tSendType
        GetPart = 1
    End Enum
    Dim SendType As tSendType = tSendType.GetPart

#Region "Column Settings"

    Public Overrides Sub FormSettings()

        'Work Order Number
        With field
            .Name = "SERIALNAME"
            .Title = "Work Order"
            .ValidExp = ValidStr(tRegExValidation.tWO)
            .SQLValidation = "SELECT SERIALNAME " & _
                            "FROM SERIAL " & _
                            "WHERE SERIALNAME = '%ME%' AND CLOSED <> 'C' AND RELEASE='Y'"
            .Data = ""      '******** Barcoded field '*******
            .AltEntry = SFDCData.ctrlText.tAltCtrlStyle.ctNone 'ctNone 'ctKeyb 
            .ctrlEnabled = True
            .MandatoryOnPost = False
        End With
        CtrlForm.AddField(field)

        'FROM WARHSNAME
        With field
            .Name = "WARHS"
            .Title = "From W/H"
            .ValidExp = ValidStr(tRegExValidation.tWarehouse)
            .SQLList = "SELECT DISTINCT WARHSNAME FROM WAREHOUSES WHERE INACTIVE <> 'Y' AND WARHS <> 0"
            .SQLValidation = "SELECT top 1 WARHSNAME FROM WAREHOUSES WHERE UPPER(WARHSNAME) = UPPER('%ME%')"
            .Data = "Main"      '******** Barcoded field '*******
            .AltEntry = SFDCData.ctrlText.tAltCtrlStyle.ctList
            .ctrlEnabled = True
        End With
        CtrlForm.AddField(field)

        'LOCNAME
        With field
            .Name = "LOCNAME"
            .Title = "From Bin"
            .ValidExp = ValidStr(tRegExValidation.tLocname)
            .SQLList = "SELECT DISTINCT LOCNAME FROM WAREHOUSES WHERE UPPER(WARHSNAME) = UPPER('%WARHS%') AND INACTIVE <> 'Y' AND WARHS <> 0"
            .SQLValidation = "SELECT LOCNAME FROM WAREHOUSES WHERE upper(LOCNAME) = upper('%ME%') AND upper(WARHSNAME) = upper('%WARHS%')"
            .Data = ""      '******** Barcoded field '*******
            .AltEntry = SFDCData.ctrlText.tAltCtrlStyle.ctList
            .ctrlEnabled = False
        End With
        CtrlForm.AddField(field)

    End Sub

    Public Overrides Sub TableSettings()

        'Part Name
        With col
            .Name = "_PARTNAME"
            .Title = "Part No"
            .initWidth = 30
            .TextAlign = HorizontalAlignment.Left
            .ValidExp = ValidStr(tRegExValidation.tBarcode)
            .SQLValidation = "SELECT BARCODE, PARTNAME " & _
                            "FROM PART " & _
                            "WHERE BARCODE = '%ME%' AND PART IN " & _
                            "(SELECT PART  " & _
                            "FROM WARHSBAL, WAREHOUSES  " & _
                            "WHERE WARHSBAL.WARHS = WAREHOUSES.WARHS " & _
                            "AND WAREHOUSES.WARHSNAME = '%WARHS%' " & _
                            "AND WAREHOUSES.LOCNAME = '%LOCNAME%' " & _
                            "AND WARHSBAL.BALANCE > 0)"

            .ctrlEnabled = True
        End With
        CtrlTable.AddCol(col)

        ' LOTNUM
        With col
            .Name = "_LOTNUM"
            .Title = "Lot"
            .initWidth = 30
            .TextAlign = HorizontalAlignment.Left
            .AltEntry = SFDCData.ctrlText.tAltCtrlStyle.ctList 'ctKeyb 
            .ValidExp = "^.+$"
            .SQLList = "SELECT DISTINCT ISNULL(dbo.SERIAL.SERIALNAME, 0) AS SERIALNAME " & _
                        "FROM         dbo.PART INNER JOIN " & _
                        "dbo.WARHSBAL ON dbo.PART.PART = dbo.WARHSBAL.PART INNER JOIN " & _
                        "dbo.WAREHOUSES ON dbo.WARHSBAL.WARHS = dbo.WAREHOUSES.WARHS LEFT OUTER JOIN " & _
                        "dbo.SERIAL ON dbo.PART.PART = dbo.SERIAL.PART AND dbo.WARHSBAL.SERIAL = dbo.SERIAL.SERIAL " & _
                        "WHERE     (dbo.WARHSBAL.CUST = - 1) " & _
                        "AND (dbo.PART.PARTNAME = '%_PARTNAME%')  " & _
                        "AND (dbo.WAREHOUSES.WARHSNAME = '%_WARHS%')  " & _
                        "AND (dbo.WAREHOUSES.LOCNAME = '%_LOCNAME%') " & _
                        "AND (dbo.WARHSBAL.BALANCE > 0)"
            .SQLValidation = "SELECT '%ME%'"
            .DefaultFromCtrl = Nothing '******* Barcoded Field - default from Type1 TOLOCATION '*******
            .ctrlEnabled = True
            .Mandatory = False
            .MandatoryOnPost = True
        End With
        CtrlTable.AddCol(col)

        ' Quantity
        With col
            .Name = "_QTY"
            .Title = "Qty"
            .initWidth = 25
            .TextAlign = HorizontalAlignment.Left
            .ValidExp = ValidStr(tRegExValidation.tNumeric)
            .SQLValidation = _
                "select '%ME%' " & _
                "FROM         dbo.WARHSBAL INNER JOIN " & _
                "dbo.WAREHOUSES ON dbo.WARHSBAL.WARHS = dbo.WAREHOUSES.WARHS INNER JOIN " & _
                "dbo.PART ON dbo.WARHSBAL.PART = dbo.PART.PART INNER JOIN " & _
                "dbo.SERIAL ON dbo.WARHSBAL.SERIAL = dbo.SERIAL.SERIAL " & _
                "where dbo.WAREHOUSES.WARHSNAME = '%_WARHS%' " & _
                "AND dbo.WAREHOUSES.LOCNAME = '%_LOCNAME%' " & _
                "and dbo.PART.PARTNAME = '%_PARTNAME%' " & _
                "AND dbo.SERIAL.SERIALNAME = '%_LOTNUM%' " & _
                "AND cast('%ME%' as int) <= (dbo.WARHSBAL.BALANCE / 1000)"

            .AltEntry = SFDCData.ctrlText.tAltCtrlStyle.ctNone 'ctKeyb 
            .DefaultFromCtrl = Nothing
            .ctrlEnabled = True
            .Mandatory = False
            .MandatoryOnPost = True
        End With
        CtrlTable.AddCol(col)

        'TOWARHSNAME
        With col
            .Name = "_WARHS"
            .Title = "W/H"
            .initWidth = 0
            .TextAlign = HorizontalAlignment.Left
            .ValidExp = ValidStr(tRegExValidation.tWarehouse)
            .SQLList = "SELECT DISTINCT WARHSNAME FROM WAREHOUSES AND INACTIVE <> 'Y'"
            .SQLValidation = "SELECT WARHSNAME FROM WAREHOUSES WHERE WARHSNAME = '%ME%' AND INACTIVE <> 'Y'"
            .AltEntry = SFDCData.ctrlText.tAltCtrlStyle.ctList 'ctKeyb 
            .DefaultFromCtrl = CtrlForm.el(1)
            .ctrlEnabled = False
            .Mandatory = False
            .MandatoryOnPost = True
        End With
        CtrlTable.AddCol(col)

        ' TOLOCNAME
        With col
            .Name = "_LOCNAME"
            .Title = "Loc"
            .initWidth = 0
            .TextAlign = HorizontalAlignment.Left
            .AltEntry = SFDCData.ctrlText.tAltCtrlStyle.ctList 'ctKeyb 
            .ValidExp = ValidStr(tRegExValidation.tLocname)
            .SQLList = "SELECT DISTINCT LOCNAME FROM WAREHOUSES WHERE WARHSNAME = '%WARHS%' AND INACTIVE <> 'Y'"
            .SQLValidation = "SELECT LOCNAME FROM WAREHOUSES WHERE LOCNAME = '%ME%' AND WARHSNAME = '%WARHS%'"
            .DefaultFromCtrl = CtrlForm.el(2) '******* Barcoded Field - default from Type1 TOLOCATION '*******
            .ctrlEnabled = False
            .Mandatory = False
            .MandatoryOnPost = True
        End With
        CtrlTable.AddCol(col)

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
                End With
            Next
        Catch e As Exception
            MsgBox(e.Message)
        End Try
    End Sub

#End Region

#Region "EVENT HANDLERS"

    Public Overrides Sub AfterAdd(ByVal TableIndex As Integer)

    End Sub

    Public Overrides Sub AfterEdit(ByVal TableIndex As Integer)

    End Sub

    Public Overrides Sub BeginAdd()

    End Sub

    Public Overrides Sub BeginEdit()
        'CtrlTable.mCol(0).ctrlEnabled = True
        'CtrlTable.mCol(1).ctrlEnabled = False
        'CtrlTable.mCol(2).ctrlEnabled = True

        'If CtrlForm.el(3).Data.Length = 0 Then
        '    CtrlTable.CancelEdit = True
        '    MsgBox("Please select the operation")
        '    Exit Sub
        'End If
        '' at this point we need to throw a numeric form
        '' to capture  the amount of product for the selected status
        'Dim num As New frmNumber
        'With num
        '    .Text = "Quantity."
        '    .ShowDialog()
        '    ' update the number in the table
        '    Dim q As String = CStr(.number)
        '    With CtrlTable
        '        With .Table
        '            .Items(.SelectedIndices(0)).SubItems(1).Text = q
        '        End With
        '        .CancelEdit = Not (String.Compare(.Table.Items(.Table.SelectedIndices(0)).SubItems(0).Text, "Reject") = 0)
        '    End With
        '    .Dispose()
        'End With


    End Sub

#End Region

#Region "processing"

    Public Overrides Sub ProcessEntry(ByVal ctrl As SFDCData.ctrlText, ByVal Valid As Boolean)

        Select Case Valid
            Case False
                Beep()

            Case True
                Try

                    Select ctrl.Name
                        Case "SERIALNAME"
                            ctrl.Enabled = (CtrlForm.el(2).Data = "")
                        Case "WARHS"
                            CtrlForm.el(2).Data = ""
                        Case "LOCNAME"
                            CtrlForm.DoLostFocus()
                            CtrlTable.Focus()
                    End Select

                    CtrlForm.el(2).CtrlEnabled = CBool(CtrlForm.el(0).Data.Length > 0 And CtrlForm.el(1).Data.Length > 0)

                    ' *** Set which controls are enabled                 
                    CtrlTable.EnableToolbar( _
                        CBool(CtrlTable.Table.Visible And _
                        Len(CtrlForm.el(0).Data) > 0 And _
                        Len(CtrlForm.el(1).Data) > 0 And _
                        Len(CtrlForm.el(2).Data) > 0), _
                        CtrlTable.btnEnabled(SFDCData.CtrlTable.tButton.btnEdit), _
                        CtrlTable.btnEnabled(SFDCData.CtrlTable.tButton.btnCopy), _
                        CtrlTable.btnEnabled(SFDCData.CtrlTable.tButton.btnDelete), _
                        CtrlTable.btnEnabled(SFDCData.CtrlTable.tButton.btnPost) _
                    )
                Catch
                End Try

        End Select

    End Sub

    Public Overrides Sub ProcessForm()
        Try

            With p
                .DebugFlag = False
                .Procedure = "ZSFDC_LOADALINE_ONE"
                .Table = "ZSFDC_LOADALINE_ONE"
                .RecordType1 = _
                                "USERNAME," & _
                                "ACTCANCEL," & _
                                "ACTNAME," & _
                                "ASPAN," & _
                                "CURDATE," & _
                                "DEFECTCODE," & _
                                "EMPASPAN," & _
                                "EMPETIME," & _
                                "EMPSTIME," & _
                                "ETIME," & _
                                "FINAL," & _
                                "LOCNAME," & _
                                "MODE," & _
                                "MQUANT," & _
                                "NEWPALLET," & _
                                "NUMPACK," & _
                                "PACKCODE," & _
                                "PARTNAME," & _
                                "QUANT," & _
                                "RTYPEBOOL," & _
                                "SERCANCEL," & _
                                "SERIALNAME," & _
                                "SHIFTNAME," & _
                                "SQUANT," & _
                                "STIME," & _
                                "TMQUANT," & _
                                "TOOLNAME," & _
                                "TOPALLETNAME," & _
                                "TQUANT," & _
                                "TSQUANT," & _
                                "USERID," & _
                                "WARHSNAME," & _
                                "WORKCNAME"

                .RecordType2 = _
                                "ACTNAME2," & _
                                "CUSTNAME2," & _
                                "LOCNAME2," & _
                                "PARTNAME2," & _
                                "QUANT2," & _
                                "REVNAME2," & _
                                "REWORKFLAG2," & _
                                "SERIALNAME2," & _
                                "SERNUM2," & _
                                "WARHSNAME2"

                .RecordTypes = _
                                "TEXT," & _
                                "TEXT," & _
                                "TEXT," & _
                                "," & _
                                "," & _
                                "TEXT," & _
                                "," & _
                                "," & _
                                "," & _
                                "," & _
                                "TEXT," & _
                                "TEXT," & _
                                "TEXT," & _
                                "," & _
                                "TEXT," & _
                                "," & _
                                "TEXT," & _
                                "TEXT," & _
                                "," & _
                                "TEXT," & _
                                "TEXT," & _
                                "TEXT," & _
                                "TEXT," & _
                                "," & _
                                "," & _
                                "," & _
                                "TEXT," & _
                                "TEXT," & _
                                "," & _
                                "," & _
                                "," & _
                                "TEXT," & _
                                "TEXT," & _
                                "TEXT," & _
                                "TEXT," & _
                                "TEXT," & _
                                "TEXT," & _
                                "," & _
                                "TEXT," & _
                                "TEXT," & _
                                "TEXT," & _
                                "TEXT," & _
                                "TEXT"
            End With


            ' Type 1 records
            Dim t1() As String = { _
                    String.Format(UserName, "UserName"), _
                    String.Format("", "CHAR,1,Remove Oper. Number?"), _
                    String.Format("", "CHAR,16,Operation"), _
                    String.Format("0", "TIME,6,Span"), _
                    String.Format(DateDiff(DateInterval.Minute, #1/1/1988#, Now).ToString, "M,DATE,8,Date of Report"), _
                    String.Format("", "CHAR,3,Defect Code"), _
                    String.Format("0", "TIME,6,Labor Span"), _
                    String.Format("0", "TIME,5,End Labor"), _
                    String.Format("0", "TIME,5,Start Labor"), _
                    String.Format("0", "TIME,5,End Time"), _
                    String.Format("", "CHAR,1,Final"), _
                    String.Format("", "CHAR,14,Bin"), _
                    String.Format("", "CHAR,1,Set-up/Break (S/B)"), _
                    String.Format("0", "INT,17,3,Qty for MRB"), _
                    String.Format("", "CHAR,1,New Pallet?"), _
                    String.Format("0", "INT,6,Packing Crates (No.)"), _
                    String.Format("", "CHAR,2,Packing Crate Code"), _
                    String.Format("", "M,CHAR,22,Part Number"), _
                    String.Format("0", "INT,17,3,Qty Completed"), _
                    String.Format("", "CHAR,1,Rework?"), _
                    String.Format("", "CHAR,1,Remove Wk Order No.?"), _
                    String.Format(CtrlForm.ItemValue("SERIALNAME"), "M,CHAR,22,Work Order"), _
                    String.Format("", "CHAR,8,Shift"), _
                    String.Format("0", "INT,17,3,Qty Rejected"), _
                    String.Format("0", "TIME,5,Start Time"), _
                    String.Format("0", "INT,17,3,MRB (Buy/Sell Units)"), _
                    String.Format("", "CHAR,22,Tool"), _
                    String.Format("", "CHAR,16,To Pallet"), _
                    String.Format("0", "INT,17,3,Completed (Buy/Sell)"), _
                    String.Format("0", "INT,17,3,Rejected (Buy/Sell)"), _
                    String.Format("0", "INT,8,0,Employee ID"), _
                    String.Format("", "CHAR,4,To Warehouse"), _
                    String.Format("", "CHAR,6,Work Cell") _
                                }
            p.AddRecord(1) = t1

            For y As Integer = 0 To CtrlTable.RowCount

                Dim t2() As String = { _
                    String.Format("", "CHAR,16,Operation/Pallet"), _
                    String.Format("Goods", "M,CHAR,16,Status"), _
                    String.Format(CtrlTable.ItemValue("_LOCNAME", y), "M,CHAR,14,Bin"), _
                    String.Format(CtrlTable.ItemValue("_PARTNAME", y), "M,CHAR,22,Part Number"), _
                    String.Format(CStr(CDbl(CtrlTable.ItemValue("_QTY", y)) * 1000), "INT,17,3,Quantity"), _
                    String.Format("", "CHAR,10,Part Revision No."), _
                    String.Format("", "CHAR,1,Rework?"), _
                    String.Format(CtrlTable.ItemValue("_LOTNUM", y), "M,CHAR,22,Work Order/Lot"), _
                    String.Format("", "CHAR,20,Serial Number"), _
                    String.Format(CtrlTable.ItemValue("_WARHS", y), "M,CHAR,4,Warehouse") _
                            }
                ' Type 2 records

                p.AddRecord(2) = t2

            Next

        Catch e As Exception
            MsgBox(e.Message)
        End Try
    End Sub

    Public Overrides Sub TableScan(ByVal Value As String)

        If CtrlForm.ItemValue("SERIALNAME").Length = 0 Or CtrlForm.ItemValue("WARHS").Length = 0 Or CtrlForm.ItemValue("LOCNAME").Length = 0 Then Exit Sub

        If System.Text.RegularExpressions.Regex.IsMatch(Value, ValidStr(tRegExValidation.tBarcode)) Then
            SendType = tSendType.GetPart
            InvokeData("SELECT     dbo.PART.PARTNAME, ISNULL(SERIALNAME,0), dbo.WAREHOUSES.WARHSNAME, dbo.WAREHOUSES.LOCNAME, SUM(dbo.WARHSBAL.BALANCE / 1000) AS BALANCE, BARCODE " & _
                        "FROM         dbo.PART INNER JOIN " & _
                        "dbo.WARHSBAL ON dbo.PART.PART = dbo.WARHSBAL.PART INNER JOIN " & _
                        "dbo.WAREHOUSES ON dbo.WARHSBAL.WARHS = dbo.WAREHOUSES.WARHS LEFT OUTER JOIN " & _
                        "dbo.SERIAL ON dbo.PART.PART = dbo.SERIAL.PART AND dbo.WARHSBAL.SERIAL = dbo.SERIAL.SERIAL " & _
                        "WHERE     (dbo.WARHSBAL.CUST = - 1)  " & _
                        "AND (dbo.PART.BARCODE = '" & Value & "')  " & _
                        "AND (dbo.WAREHOUSES.WARHSNAME = '%WARHS%')  " & _
                        "AND (dbo.WAREHOUSES.LOCNAME = '%LOCNAME%') " & _
                        "AND (dbo.WARHSBAL.BALANCE > 0) " & _
                        "GROUP BY dbo.PART.PARTNAME, SERIALNAME, dbo.WAREHOUSES.WARHSNAME, dbo.WAREHOUSES.LOCNAME, BARCODE")
        ElseIf System.Text.RegularExpressions.Regex.IsMatch(Value, ValidStr(tRegExValidation.tWarehouse)) Then
            With CtrlForm.el(1)
                .DataEntry.Text = Value
                .ProcessEntry()
            End With
        ElseIf System.Text.RegularExpressions.Regex.IsMatch(Value, ValidStr(tRegExValidation.tLocname)) Then
            With CtrlForm.el(2)
                .DataEntry.Text = Value
                .ProcessEntry()
            End With
        End If

    End Sub

    Public Overrides Sub EndInvokeData(ByVal Data(,) As String)
        Select Case SendType
            Case tSendType.GetPart
                If IsNothing(Data) Then
                    MsgBox("Part does not exist Goods Status at this location.")
                    Exit Sub
                End If

                Dim F As Boolean = False
                If CtrlTable.Table.Items.Count > 0 Then
                    For i As Integer = 0 To CtrlTable.Table.Items.Count - 1
                        If _
                            CtrlTable.ItemValue("_PARTNAME", i) = Data(0, 0) And _
                            CtrlTable.ItemValue("_LOTNUM", i) = Data(1, 0) And _
                            CtrlTable.ItemValue("_WARHS", i) = Data(2, 0) And _
                            CtrlTable.ItemValue("_LOCNAME", i) = Data(3, 0) Then

                            F = True
                            CtrlTable.Table.Items(i).Selected = True
                            CtrlTable.SetEdit()
                            Exit For

                        End If
                    Next
                End If

                If Not F Then
                    CtrlTable.SetAdd()
                    With CtrlTable.el(0)
                        .DataEntry.Text = Data(5, 0)
                        .ProcessEntry()
                    End With
                End If

        End Select
    End Sub

#End Region

#Region "Validation"

    Public Overrides Function VerifyForm() As Boolean
        'Try
        '    Dim lvi As New ListViewItem
        '    With CtrlTable.Table
        '        For y As Integer = 0 To .Items.Count - 1
        '            If Not .Items(y).SubItems(3).Text = .Items(y).SubItems(4).Text Then
        '                Return (MsgBox("Total quantity not reported. Are you sure you wish to proceed?", MsgBoxStyle.OkCancel) = MsgBoxResult.Ok)
        '            End If
        '        Next
        '        Return True
        '    End With

        'Catch e As Exception
        '    MsgBox(e.Message)
        'End Try
        Return True
    End Function

#End Region

End Class