Imports System
Imports System.IO
Imports System.Threading

Public Class InterfaceGRV
    Inherits w32SFDCData.iForm

#Region "Initialisation"

    Public Sub New(Optional ByRef App As Form = Nothing)

        InitializeComponent()
        CallerApp = App
        NewArgument("SCANACTION", "OPENFORM")
        CtrlTable.DisableButtons(True, False, False, True, False)

    End Sub

#End Region

    Public Overrides Sub FormSettings()

        'ORDNAME
        With field
            .Name = "PONAME"
            .Title = "PO Number"
            .ValidExp = "^(PO)|(MPR)[0-9]+$"
            .SQLValidation = "SELECT DISTINCT ORDNAME FROM PORDERS " & _
                            "WHERE CLOSED = '' " & _
                            "AND ORDNAME <> '' " & _
                            "AND ORDNAME = '%ME%'"
            .Data = ""
            .AltEntry = ctrlText.tAltCtrlStyle.ctNone 'ctKeyb 
            .ctrlEnabled = True
            .MandatoryOnPost = True
        End With
        CtrlForm.AddField(field)

        'BOOKNUM
        With field
            .Name = "BOOKNUM"
            .Title = "Vendor Doc."
            .ValidExp = "^.+$"
            .SQLValidation = "SELECT '%ME%'"
            .Data = ""
            .AltEntry = ctrlText.tAltCtrlStyle.ctNone 'ctKeyb 
            .ctrlEnabled = True
            .MandatoryOnPost = False
        End With
        CtrlForm.AddField(field)

    End Sub

    Public Overrides Sub TableSettings()

        ' BARCODE
        With col
            .Name = "_PARTNAME"
            .Title = "Part"
            .initWidth = 20
            .TextAlign = HorizontalAlignment.Left
            .AltEntry = ctrlText.tAltCtrlStyle.ctNone 'ctKeyb 
            .ValidExp = ValidStr(tRegExValidation.tBarcode)
            .SQLValidation = "SELECT BARCODE, PARTNAME " & _
                            "FROM PART " & _
                            "WHERE BARCODE = '%ME%'"
            .DefaultFromCtrl = Nothing '
            .ctrlEnabled = True
            .Mandatory = True
            .ctrlEnabled = True
        End With
        CtrlTable.AddCol(col)

        With col
            .Name = "_PARTDES"
            .Title = "Name"
            .initWidth = 50
            .TextAlign = HorizontalAlignment.Left
            .AltEntry = ctrlText.tAltCtrlStyle.ctNone 'ctKeyb 
            .ValidExp = "^.+$"
            .SQLValidation = "SELECT '%ME%'"
            .DefaultFromCtrl = Nothing '
            .ctrlEnabled = False
            .Mandatory = False
            .ctrlEnabled = True
        End With
        CtrlTable.AddCol(col)

        ' TQUANT
        With col
            .Name = "_QUANT"
            .Title = "Ord"
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
            .Name = "_RECEIVED"
            .Title = "Rcvd"
            .initWidth = 20
            .TextAlign = HorizontalAlignment.Left
            .AltEntry = ctrlText.tAltCtrlStyle.ctNone 'ctKeyb 
            .ValidExp = ValidStr(tRegExValidation.tNumeric)
            .SQLValidation = "SELECT %ME%"
            .DefaultFromCtrl = Nothing
            .ctrlEnabled = True
            .Mandatory = True
            .MandatoryOnPost = True
        End With
        CtrlTable.AddCol(col)

        ' STATUS
        With col
            .Name = "_STATUS"
            .Title = "Status"
            .initWidth = 30
            .TextAlign = HorizontalAlignment.Left
            .AltEntry = ctrlText.tAltCtrlStyle.ctList 'ctKeyb 
            .ValidExp = ValidStr(tRegExValidation.tStatus)
            .SQLValidation = "SELECT CUSTNAME FROM CUSTOMERS WHERE STATUSFLAG = 'Y' AND CUSTNAME = '%ME%'"
            .SQLList = "SELECT CUSTNAME FROM CUSTOMERS WHERE STATUSFLAG = 'Y'"
            .DefaultFromCtrl = Nothing
            .ctrlEnabled = True
            .Mandatory = True
            .MandatoryOnPost = True
        End With
        CtrlTable.AddCol(col)

        ' Set the query to load recordtype 2s
        CtrlTable.RecordsSQL = "SELECT PARTNAME, PARTDES, SUM(PORDERITEMS.TBALANCE/1000), 'Goods'" & _
                                "FROM PART, PORDERITEMS, PORDERS " & _
                                "WHERE PORDERITEMS.PART = PART.PART " & _
                                "AND PORDERS.ORD = PORDERITEMS.ORD " & _
                                "AND PORDERS.ORDNAME = '%PONAME%' " & _
                                "AND PORDERITEMS.REQDATE <=  dbo.DATETOMIN(getdate()) " & _
                                "GROUP BY PARTNAME, PARTDES"

    End Sub

    Public Overrides Sub TableRXData(ByVal Data(,) As String)

        Try
            For y As Integer = 0 To UBound(Data, 2)
                Dim lvi As New ListViewItem
                With CtrlTable.Table
                    .Items.Add(lvi)
                    .Items(.Items.Count - 1).Text = Data(0, y)
                    .Items(.Items.Count - 1).SubItems.Add(Data(1, y))
                    '.Items(.Items.Count - 1).SubItems.Add("")
                    .Items(.Items.Count - 1).SubItems.Add(Data(2, y))
                    .Items(.Items.Count - 1).SubItems.Add("0")
                    .Items(.Items.Count - 1).SubItems.Add(Data(3, y))
                End With
            Next
        Catch e As Exception
            MsgBox(e.Message)
        End Try

    End Sub

    Public Overrides Sub ProcessEntry(ByVal ctrl As w32SFDCData.ctrlText, ByVal Valid As Boolean)

        Select Case Valid
            Case False
                Beep()

            Case True
                Try
                    If ctrl.Data <> "" Then
                        Select Case ctrl.Name
                            Case "PONAME"
                                CtrlTable.BeginLoadRS()
                                'Case "TOLOC"
                                '    For Y As Integer = 0 To CtrlTable.Table.Items.Count - 1
                                '        If Len(CtrlTable.Table.Items(Y).SubItems(2).Text) = 0 Then
                                '            CtrlTable.Table.Items(Y).SubItems(2).Text = CtrlForm.el(1).Data
                                '        End If
                                '    Next
                        End Select
                    End If

                    ' *******************************************************************
                    ' *** Set which controls are enabled

                    CtrlForm.el(0).CtrlEnabled = Not (Len(CtrlForm.el(0).Data) > 0)
                    'CtrlTable.el(1).CtrlEnabled = Len(CtrlTable.el(0).Data) > 0

                    ' *******************************************************************
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
                .Procedure = "ZSFDCP_LOAD_GRV"
                .Table = "ZSFDC_LOAD_GRV"
                .RecordType1 = "ORDNAME,BOOKNUM,USERLOGIN,TOWARHSNAME,TOLOCNAME"
                .RecordType2 = "PARTNAME,TOWARHSNAME,TOLOCNAME,TQUANT,STATUS,ORDNAME1"
                .RecordTypes = "TEXT,TEXT,TEXT,TEXT,TEXT,TEXT,TEXT,TEXT,,TEXT,TEXT"
            End With

            ' Type 1 records
            Dim t1() As String = { _
                                CtrlForm.ItemValue("PONAME"), _
                                CtrlForm.ItemValue("BOOKNUM"), _
                                UserName, _
                                Warehouse, _
                                "GOODS IN" _
                                }
            p.AddRecord(1) = t1

            For y As Integer = 0 To CtrlTable.RowCount
                Dim t2() As String = { _
                            CtrlTable.ItemValue("_PARTNAME", y), _
                            Warehouse, _
                            "GOODS IN", _
                            CStr(CInt(CtrlTable.ItemValue("_RECEIVED", y)) * 1000), _
                            CtrlTable.ItemValue("_STATUS", y), _
                             CtrlForm.ItemValue("PONAME") _
                                    }
                p.AddRecord(2) = t2
            Next

        Catch e As Exception
            MsgBox(e.Message)
        End Try

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
        CtrlTable.mCol(1).ctrlEnabled = True
        CtrlTable.mCol(2).ctrlEnabled = False
        CtrlTable.mCol(3).ctrlEnabled = True
        CtrlTable.mCol(4).ctrlEnabled = True
    End Sub

    Public Overrides Sub AfterEdit(ByVal TableIndex As Integer)

    End Sub

    Public Overrides Sub TableScan(ByVal Value As String)
        InvokeData("SELECT PARTNAME FROM PART WHERE BARCODE = '" & Value & "'")
    End Sub

    Public Overrides Sub EndInvokeData(ByVal Data(,) As String)

        For i As Integer = 0 To CtrlTable.Table.Items.Count
            If CtrlTable.Table.Items(i).SubItems(0).Text = Data(0, 0) Then

                Dim exp As Integer = CInt(CtrlTable.Table.Items(i).SubItems(2).Text)
                Dim rcvd As Integer = CInt(CtrlTable.Table.Items(i).SubItems(3).Text)
                Dim add As Integer

                Select Case Argument("SCANACTION")

                    Case "OPENFORM"

                        Dim num As New frmNumber
                        With num
                            .Text = "Box quantity."
                            .ShowDialog()
                            add = .number
                            .Dispose()
                        End With

                    Case "INCREMENT"
                        add = 1

                End Select

                If add > 0 Then

                    If rcvd + add > exp Then
                        If MsgBox("Receiving more than ordered. Continue?", MsgBoxStyle.OkCancel, "Part " & Data(0, 0)) = MsgBoxResult.Ok Then
                            CtrlTable.Table.Items(i).SubItems(3).Text = CStr(rcvd + add)
                        End If
                    Else
                        CtrlTable.Table.Items(i).SubItems(3).Text = CStr(CInt(CtrlTable.Table.Items(i).SubItems(3).Text) + add)
                    End If

                End If
                Exit For

            End If
        Next

    End Sub

End Class
