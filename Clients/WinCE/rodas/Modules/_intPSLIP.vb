Public Class interfacePSLIP
    Inherits SFDCData.iForm

#Region "Initialisation"

    Public Sub New(Optional ByRef App As Form = Nothing)

        'InitializeComponent()
        CallerApp = App
        CtrlTable.DisableButtons(True, False, False, True, False)
        CtrlTable.EnableToolbar(False, False, False, False, False)

    End Sub

#End Region

    Public Enum tSendType
        NextPS = 0
        TableScan = 1
        GetWarhs = 2
        Loc = 3
        SetUser = 4
    End Enum
    Dim SendType As tSendType = tSendType.NextPS

    Public Overrides Sub FormLoaded()
        MyBase.FormLoaded()
        If CtrlForm.el(0).Data.Length = 0 Then
            SendType = tSendType.NextPS
            InvokeData("SELECT dbo.ZSFDC_NEXTPICK('" & UserName & "')")
        End If
    End Sub

    Public Overrides Sub FormSettings()

        'ORDNAME
        With field 'using the tfield structure from the ctrlForm
            .Name = "PSNO"
            .Title = "Pick Note"
            .ValidExp = "^[0-9]+$"
            .SQLValidation = "exec dbo.ZSFDC_MarkPicked '%ME%'"
            .Data = ""
            .AltEntry = ctrlText.tAltCtrlStyle.ctNone 'ctKeyb 
            .ctrlEnabled = True
            .MandatoryOnPost = True
        End With
        CtrlForm.AddField(field)

        ''TOWARHSNAME
        'With field
        '    .Name = "TOWARHS"
        '    .Title = "W/H"
        '    .ValidExp = ValidStr(tRegExValidation.tWarehouse)
        '    .SQLList = "SELECT DISTINCT WARHSNAME FROM WAREHOUSES WHERE INACTIVE <> 'Y'"
        '    .SQLValidation = "SELECT WARHSNAME FROM WAREHOUSES WHERE WARHSNAME = '%ME%'"
        '    .Data = ""      '******** Barcoded field '*******
        '    .AltEntry = ctrlText.tAltCtrlStyle.ctList 'ctKeyb 
        '    .ctrlEnabled = False
        'End With
        'CtrlForm.AddField(field)

        ' TOLOCNAME
        With field
            .Name = "TOLOC"
            .Title = "Bin"
            .AltEntry = ctrlText.tAltCtrlStyle.ctList 'ctKeyb 
            .ValidExp = ValidStr(tRegExValidation.tLocname)
            .SQLList = "SELECT DISTINCT TOP (100) PERCENT dbo.WAREHOUSES.LOCNAME " & _
                        "FROM         dbo.ZTRX_PICKMONITOR INNER JOIN " & _
                        "                      dbo.ZTRX_GROUPPICK ON dbo.ZTRX_PICKMONITOR.PICKREF = dbo.ZTRX_GROUPPICK.PICKREF INNER JOIN " & _
                        "                      dbo.PART ON dbo.ZTRX_GROUPPICK.PART = dbo.PART.PART INNER JOIN " & _
                        "                      dbo.WAREHOUSES ON dbo.ZTRX_GROUPPICK.WARHS = dbo.WAREHOUSES.WARHS " & _
                        "WHERE     (dbo.ZTRX_PICKMONITOR.PICKREFNUM = '%PSNO%') " & _
                        "ORDER BY dbo.WAREHOUSES.LOCNAME"
            .SQLValidation = "SELECT DISTINCT TOP (100) PERCENT dbo.WAREHOUSES.LOCNAME " & _
                            "FROM         dbo.ZTRX_PICKMONITOR INNER JOIN " & _
                            "                      dbo.ZTRX_GROUPPICK ON dbo.ZTRX_PICKMONITOR.PICKREF = dbo.ZTRX_GROUPPICK.PICKREF INNER JOIN " & _
                            "                      dbo.PART ON dbo.ZTRX_GROUPPICK.PART = dbo.PART.PART INNER JOIN " & _
                            "                      dbo.WAREHOUSES ON dbo.ZTRX_GROUPPICK.WARHS = dbo.WAREHOUSES.WARHS " & _
                            "WHERE     (dbo.ZTRX_PICKMONITOR.PICKREFNUM = '%PSNO%') AND LOCNAME = '%ME%'" & _
                            "ORDER BY dbo.WAREHOUSES.LOCNAME"
            .DefaultFromCtrl = Nothing
            .ctrlEnabled = False
            .MandatoryOnPost = False
        End With
        CtrlForm.AddField(field)

        'Part Name
        With field
            .Name = "PARTNAME"
            .Title = "Part No"
            .ValidExp = ".+"
            .SQLValidation = "SELECT '%ME%' " & _
                            "ORDER BY dbo.WAREHOUSES.LOCNAME"
            .Data = ""      '******** Barcoded field '*******
            .AltEntry = ctrlText.tAltCtrlStyle.ctNone 'ctNone 'ctKeyb 
            .ctrlEnabled = False
            .MandatoryOnPost = False
        End With
        CtrlForm.AddField(field)

        'Part Name
        With field
            .Name = "PICKED"
            .Title = "Picked"
            .ValidExp = ValidStr(tRegExValidation.tString)
            .SQLValidation = "SELECT '%ME%'"
            .Data = ""      '******** Barcoded field '*******
            .AltEntry = ctrlText.tAltCtrlStyle.ctNone 'ctNone 'ctKeyb 
            .ctrlEnabled = False
            .MandatoryOnPost = False            
        End With
        CtrlForm.AddField(field)

    End Sub

    Public Overrides Sub TableSettings()

        ' TOLOCNAME
        With col
            .Name = "_TOLOC"
            .Title = "Bin"
            .initWidth = 25
            .TextAlign = HorizontalAlignment.Left
            .AltEntry = ctrlText.tAltCtrlStyle.ctList 'ctKeyb 
            .ValidExp = ValidStr(tRegExValidation.tLocname)
            .SQLList = "SELECT DISTINCT LOCNAME FROM WAREHOUSES WHERE WARHSNAME = '%TOWARHS%'"
            .SQLValidation = "SELECT LOCNAME FROM WAREHOUSES WHERE LOCNAME = '%ME%' AND WARHSNAME = '%TOWARHS%'"
            .DefaultFromCtrl = Nothing
            .ctrlEnabled = False
            .Mandatory = False
            .MandatoryOnPost = True
        End With
        CtrlTable.AddCol(col)

        With col
            .Name = "_PARTDES"
            .Title = "Part Desc"
            .initWidth = 95
            .TextAlign = HorizontalAlignment.Left
            .AltEntry = ctrlText.tAltCtrlStyle.ctNone 'ctKeyb 
            .ValidExp = ValidStr(tRegExValidation.tBarcode)
            .SQLValidation = "SELECT '%ME%'"
            .DefaultFromCtrl = Nothing '
            .ctrlEnabled = False
            .Mandatory = False
        End With
        CtrlTable.AddCol(col)

        ' BARCODE
        With col
            .Name = "_PARTNAME"
            .Title = "Part"
            .initWidth = 55
            .TextAlign = HorizontalAlignment.Left
            .AltEntry = ctrlText.tAltCtrlStyle.ctNone 'ctKeyb 
            .ValidExp = ValidStr(tRegExValidation.tBarcode)
            .SQLValidation = "SELECT BARCODE, PARTNAME " & _
                            "FROM PART " & _
                            "WHERE BARCODE = '%ME%'"
            .DefaultFromCtrl = Nothing '
            .ctrlEnabled = True
            .Mandatory = True
            .ctrlEnabled = False
        End With
        CtrlTable.AddCol(col)

        ''TOWARHSNAME
        'With col
        '    .Name = "_TOWARHS"
        '    .Title = "W/H"
        '    .initWidth = 20
        '    .TextAlign = HorizontalAlignment.Left
        '    .AltEntry = ctrlText.tAltCtrlStyle.ctList 'ctKeyb 
        '    .ValidExp = ValidStr(tRegExValidation.tWarehouse)
        '    .SQLList = "SELECT DISTINCT WARHSNAME FROM WAREHOUSES WHERE INACTIVE <> 'Y'"
        '    .SQLValidation = "SELECT WARHSNAME FROM WAREHOUSES WHERE WARHSNAME = '%ME%'"
        '    .DefaultFromCtrl = Nothing
        '    .ctrlEnabled = False
        '    .Mandatory = False
        '    .MandatoryOnPost = True
        'End With
        'CtrlTable.AddCol(col)

        ' TQUANT
        With col
            .Name = "_QUANT"
            .Title = "To Pack"
            .initWidth = 20
            .TextAlign = HorizontalAlignment.Left
            .AltEntry = ctrlText.tAltCtrlStyle.ctNone 'ctKeyb 
            .ValidExp = ValidStr(tRegExValidation.tNumeric)
            .SQLValidation = "SELECT %ME%"
            .DefaultFromCtrl = Nothing
            .ctrlEnabled = False
            .Mandatory = True
            .MandatoryOnPost = True
        End With
        CtrlTable.AddCol(col)

        ' TQUANT
        With col
            .Name = "_PACKED"
            .Title = "Packed"
            .initWidth = 20
            .TextAlign = HorizontalAlignment.Left
            .AltEntry = ctrlText.tAltCtrlStyle.ctNone 'ctKeyb 
            .ValidExp = ValidStr(tRegExValidation.tNumeric)
            .SQLValidation = "SELECT %ME% where %ME% <= %_QUANT%"
            .DefaultFromCtrl = Nothing
            .ctrlEnabled = True
            .Mandatory = True
            .MandatoryOnPost = True
        End With
        CtrlTable.AddCol(col)

        '' TQUANT
        'With col
        '    .Name = "_TRANS"
        '    .Title = "Trans"
        '    .initWidth = 0
        '    .TextAlign = HorizontalAlignment.Left
        '    .AltEntry = ctrlText.tAltCtrlStyle.ctNone 'ctKeyb 
        '    .ValidExp = ValidStr(tRegExValidation.tNumeric)
        '    .SQLValidation = "SELECT %ME%"
        '    .DefaultFromCtrl = Nothing
        '    .ctrlEnabled = False
        '    .Mandatory = True
        '    .MandatoryOnPost = True
        'End With
        'CtrlTable.AddCol(col)

        '' TQUANT
        'With col
        '    .Name = "_ORDI"
        '    .Title = "Ordi"
        '    .initWidth = 0
        '    .TextAlign = HorizontalAlignment.Left
        '    .AltEntry = ctrlText.tAltCtrlStyle.ctNone 'ctKeyb 
        '    .ValidExp = ValidStr(tRegExValidation.tNumeric)
        '    .SQLValidation = "SELECT %ME%"
        '    .DefaultFromCtrl = Nothing
        '    .ctrlEnabled = False
        '    .Mandatory = True
        '    .MandatoryOnPost = True
        'End With
        'CtrlTable.AddCol(col)

        ' Set the query to load recordtype 2s
        CtrlTable.RecordsSQL = "SELECT     TOP (100) PERCENT dbo.WAREHOUSES.LOCNAME, dbo.PART.PARTDES, dbo.PART.PARTNAME, dbo.ZTRX_GROUPPICK.QTY " & _
                                "FROM         dbo.ZTRX_PICKMONITOR INNER JOIN " & _
                                "                      dbo.ZTRX_GROUPPICK ON dbo.ZTRX_PICKMONITOR.PICKREF = dbo.ZTRX_GROUPPICK.PICKREF INNER JOIN " & _
                                "                      dbo.PART ON dbo.ZTRX_GROUPPICK.PART = dbo.PART.PART INNER JOIN " & _
                                "                      dbo.WAREHOUSES ON dbo.ZTRX_GROUPPICK.WARHS = dbo.WAREHOUSES.WARHS " & _
                                "WHERE     (dbo.ZTRX_PICKMONITOR.PICKREFNUM = '%PSNO%') " & _
                                "ORDER BY dbo.WAREHOUSES.LOCNAME "

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
                    .Items(.Items.Count - 1).SubItems.Add("")
                End With
            Next
        Catch e As Exception
            MsgBox(e.Message)
        End Try

    End Sub

    Public Overrides Sub BeginAdd()
        CtrlTable.mCol(0).ctrlEnabled = False
        CtrlTable.mCol(1).ctrlEnabled = False
        CtrlTable.mCol(2).ctrlEnabled = False
        CtrlTable.mCol(3).ctrlEnabled = True
        'CtrlTable.mCol(4).ctrlEnabled = False
        'CtrlTable.mCol(5).ctrlEnabled = False
    End Sub

    Public Overrides Sub BeginEdit()
        CtrlTable.mCol(0).ctrlEnabled = False
        CtrlTable.mCol(1).ctrlEnabled = False
        CtrlTable.mCol(2).ctrlEnabled = False
        CtrlTable.mCol(3).ctrlEnabled = True
        'CtrlTable.mCol(4).ctrlEnabled = False
        'CtrlTable.mCol(5).ctrlEnabled = False
    End Sub

    Public Overrides Sub AfterEdit(ByVal TableIndex As Integer)

    End Sub

    Public Overrides Sub AfterAdd(ByVal TableIndex As Integer)

    End Sub

    Public Overrides Sub ProcessEntry(ByVal ctrl As SFDCData.ctrlText, ByVal Valid As Boolean)
        Select Case Valid
            Case False
                Beep()

            Case True
                Try
                    If ctrl.Data <> "" Then
                        Select Case ctrl.Name
                            Case "PSNO"                                
                                'SendType = tSendType.GetWarhs
                                'InvokeData("SELECT     distinct WAREHOUSES.WARHSNAME " & _
                                '            "FROM         dbo.PART INNER JOIN  " & _
                                '            "                      dbo.ORDERITEMS ON dbo.PART.PART = dbo.ORDERITEMS.PART RIGHT OUTER JOIN  " & _
                                '            "                      dbo.ZTRX_PICKLINES RIGHT OUTER JOIN " & _
                                '            "                      dbo.ZTRX_PICKMONITOR ON dbo.ZTRX_PICKLINES.PICKREF = dbo.ZTRX_PICKMONITOR.PICKREF LEFT OUTER JOIN " & _
                                '            "                      dbo.WAREHOUSES ON dbo.ZTRX_PICKLINES.WARHS = dbo.WAREHOUSES.WARHS ON dbo.ORDERITEMS.ORDI = dbo.ZTRX_PICKLINES.ORDI  " & _
                                '            "WHERE     (dbo.ZTRX_PICKMONITOR.PICKREFNUM = '%PSNO%') ")
                                CtrlTable.BeginLoadRS()
                                CtrlTable.Focus()

                            Case "TOLOC"
                                CtrlTable.Focus()

                        End Select
                    End If

                    ' *******************************************************************
                    ' *** Set which controls are enabled

                    CtrlForm.el(0).CtrlEnabled = Not (Len(CtrlForm.el(0).Data) > 0)
                    'CtrlForm.el(1).CtrlEnabled = Not (Len(CtrlForm.el(0).Data) > 0)
                    'CtrlForm.el(1).CtrlEnabled = Len(CtrlForm.el(0).Data) > 0

                    'CtrlTable.el(1).CtrlEnabled = Len(CtrlTable.el(0).Data) > 0

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
                    If Not .Items(y).SubItems(3).Text = .Items(y).SubItems(4).Text Then
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
        Try
            With p
                .DebugFlag = True
                .Procedure = "ZSFDCP_LOAD_PS"
                .Table = "ZSFDC_GRPPKRCV"
                .RecordType1 = "OWNERLOGIN"
                .RecordType2 = "PICKREFNUM,PARTNAME,WARHS,LOCNAME,TPICK,APICK"
                .RecordTypes = "TEXT,TEXT,TEXT,TEXT,TEXT,,"
            End With

            ' Type 1 records
            Dim t1() As String = { _
                                UserName _
                                }
            p.AddRecord(1) = t1

            For y As Integer = 0 To CtrlTable.RowCount
                Dim t2() As String = { _
                            CtrlForm.ItemValue("PSNO"), _
                            CtrlTable.ItemValue("_PARTNAME", y), _
                            "CL", _
                            CtrlTable.ItemValue("_TOLOC", y), _
                            CStr(CInt(1000 * CtrlTable.ItemValue("_QUANT", y))), _
                            CStr(CInt(1000 * CtrlTable.ItemValue("_PACKED", y))) _
                                    }
                p.AddRecord(2) = t2
            Next

        Catch e As Exception
            MsgBox(e.Message)
        End Try
    End Sub

    Public Overrides Sub TableScan(ByVal Value As String)

        If System.Text.RegularExpressions.Regex.IsMatch(Value, ValidStr(tRegExValidation.tBarcode)) Then
            SendType = tSendType.TableScan
            InvokeData("SELECT PARTNAME, BARCODE FROM PART WHERE BARCODE = '" & Value & "'")

        ElseIf System.Text.RegularExpressions.Regex.IsMatch(Value, "^[A-Z]+[0-9]+$") Or Value = "ZZZ" Then
            With CtrlForm.el(1)
                .DataEntry.Text = Value
                CtrlForm.el(2).Value.Text = ""
                .ProcessEntry()
            End With

        ElseIf System.Text.RegularExpressions.Regex.IsMatch(Value, ValidStr(tRegExValidation.tNumeric)) Then
            For i As Integer = 0 To CtrlTable.Table.Items.Count - 1
                With CtrlTable
                    If .Table.Items(i).SubItems(2).Text = CtrlForm.ItemValue("PARTNAME") And .Table.Items(i).SubItems(0).Text = CtrlForm.ItemValue("TOLOC") Then
                        If CInt(.Table.Items(i).SubItems(4).Text) + CInt(Value) > CInt(.Table.Items(i).SubItems(3).Text) Then
                            MsgBox("To many items scanned.")
                        Else
                            .Table.Items(i).SubItems(4).Text = CStr(CInt(.Table.Items(i).SubItems(4).Text) + CInt(Value))
                            CtrlForm.el(3).Value.Text = .Table.Items(i).SubItems(4).Text & " OF " & .Table.Items(i).SubItems(3).Text
                        End If
                        Exit For
                    End If
                End With
            Next
        End If

    End Sub

    Public Overrides Sub EndInvokeData(ByVal Data(,) As String)

        Select Case SendType
            Case tSendType.NextPS
                If Data(0, 0).Length = 0 Then
                    MsgBox("No more packing slips.")
                    Posted = True
                    Me.Close()
                Else
                    With CtrlForm.el(0)
                        .DataEntry.Text = Data(0, 0)
                        .ProcessEntry()
                    End With

                    SendType = tSendType.SetUser
                    InvokeData("UPDATE ZTRX_PICKMONITOR SET T$USER = (SELECT T$USER FROM system.dbo.USERS where UPPER(USERLOGIN) = '" & UserName.ToUpper & "') WHERE PICKREFNUM = '" & Data(0, 0) & "'")

                End If

            Case tSendType.SetUser
                'do nothing

            Case tSendType.TableScan
                With CtrlForm.el(2)
                    If Not .Value.Text = Data(0, 0) Then
                        .Value.Text = Data(0, 0)
                        '.DataEntry.Text = Data(0, 0)
                        '.ProcessEntry()
                    End If
                End With

                For i As Integer = 0 To CtrlTable.Table.Items.Count - 1
                    With CtrlTable
                        If .Table.Items(i).SubItems(2).Text = CtrlForm.ItemValue("PARTNAME") And .Table.Items(i).SubItems(0).Text = CtrlForm.ItemValue("TOLOC") Then
                            .Table.Items(i).Selected = True
                            If .Table.Items(i).SubItems(4).Text.Length > 0 Then
                                If CInt(.Table.Items(i).SubItems(4).Text) + 1 > CInt(.Table.Items(i).SubItems(3).Text) Then
                                    MsgBox("To many items scanned.")
                                    Exit Sub
                                Else
                                    .Table.Items(i).SubItems(4).Text = CStr(CInt(.Table.Items(i).SubItems(4).Text) + 1)
                                End If
                            Else
                                .Table.Items(i).SubItems(4).Text = "0"
                            End If
                            CtrlForm.el(3).Value.Text = .Table.Items(i).SubItems(4).Text & " OF " & .Table.Items(i).SubItems(3).Text
                            Exit Sub
                        End If
                    End With
                Next
                CtrlForm.el(2).Value.Text = ""
                Beep()


        End Select

    End Sub

End Class
