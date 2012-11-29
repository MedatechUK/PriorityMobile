Public Class interfacePSLIP
    Inherits w32SFDCData.iForm

#Region "Initialisation"

    Public Sub New(Optional ByRef App As Form = Nothing)

        'InitializeComponent()
        CallerApp = App
        CtrlTable.DisableButtons(True, False, False, False, False)
        CtrlTable.EnableToolbar(False, False, False, False, False)

    End Sub

#End Region

    Public Enum tSendType
        NextPS = 0
        TableScan = 1
    End Enum
    Dim SendType As tSendType = tSendType.NextPS

    Public Overrides Sub FormLoaded()
        MyBase.FormLoaded()
        SendType = tSendType.NextPS
        InvokeData("select dbo.NEXTPS('" & UserName & "')")
    End Sub

    Public Overrides Sub FormSettings()

        'ORDNAME
        With field
            .Name = "PSNO"
            .Title = "Packing Slip"
            .ValidExp = "^PS[0-9]+$"
            .SQLValidation = "SELECT DISTINCT DOCNO FROM DOCUMENTS " & _
                            "WHERE TYPE = 'A' " & _
                            "AND DOCNO = '%ME%'"
            .Data = ""
            .AltEntry = ctrlText.tAltCtrlStyle.ctNone 'ctKeyb 
            .ctrlEnabled = True
            .MandatoryOnPost = True
        End With
        CtrlForm.AddField(field)

    End Sub

    Public Overrides Sub TableSettings()

        ' BARCODE
        With col
            .Name = "_PARTNAME"
            .Title = "Part"
            .initWidth = 30
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

        ' TOLOCNAME
        With col
            .Name = "_TOLOC"
            .Title = "From Bin"
            .initWidth = 30
            .TextAlign = HorizontalAlignment.Left
            .AltEntry = ctrlText.tAltCtrlStyle.ctList 'ctKeyb 
            .ValidExp = ValidStr(tRegExValidation.tLocname)
            .SQLList = "SELECT DISTINCT LOCNAME FROM WAREHOUSES WHERE WARHSNAME = (SELECT WARHSNAME FROM v_USERS where USERLOGIN = 'tabula') AND WAREHOUSES.INACTIVE <> 'Y'"
            .SQLValidation = "SELECT LOCNAME FROM WAREHOUSES WHERE LOCNAME = '%ME%' AND WARHSNAME = (SELECT WARHSNAME FROM v_USERS where USERLOGIN = '" & UserName & "') AND WAREHOUSES.INACTIVE <> 'Y'"
            .DefaultFromCtrl = Nothing
            .ctrlEnabled = True
            .Mandatory = False
            .MandatoryOnPost = True
        End With
        CtrlTable.AddCol(col)

        ' TQUANT
        With col
            .Name = "_QUANT"
            .Title = "To Pack"
            .initWidth = 30
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
            .initWidth = 30
            .TextAlign = HorizontalAlignment.Left
            .AltEntry = ctrlText.tAltCtrlStyle.ctNone 'ctKeyb 
            .ValidExp = ValidStr(tRegExValidation.tNumeric)
            .SQLValidation = "SELECT %ME% " & _
                                    "FROM DOCUMENTS, TRANSORDER, PART " & _
                                    "WHERE DOCUMENTS.DOC = TRANSORDER.DOC " & _
                                    "AND TRANSORDER.PART = PART.PART " & _
                                    "AND DOCUMENTS.TYPE = 'A' " & _
                                    "AND DOCUMENTS.DOCNO = '%PSNO%' " & _
                                    "AND PART.PARTNAME = '%_PARTNAME%' " & _
                                    "AND TRANSORDER.TQUANT/1000 <= %ME% "
            .DefaultFromCtrl = Nothing
            .ctrlEnabled = True
            .Mandatory = True
            .MandatoryOnPost = True
        End With
        CtrlTable.AddCol(col)

        ' Set the query to load recordtype 2s
        CtrlTable.RecordsSQL = "SELECT PART.PARTNAME, WAREHOUSES.LOCNAME, TRANSORDER.TQUANT/1000 " & _
                                        "FROM DOCUMENTS, TRANSORDER , WAREHOUSES, PART " & _
                                        "WHERE DOCUMENTS.DOC = TRANSORDER.DOC " & _
                                        "AND DOCUMENTS.WARHS = WAREHOUSES.WARHS " & _
                                        "AND TRANSORDER.PART = PART.PART " & _
                                        "AND DOCUMENTS.TYPE = 'A' " & _
                                        "AND DOCUMENTS.DOCNO = '%PSNO%'"

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
                    .Items(.Items.Count - 1).SubItems.Add("")
                End With
            Next
        Catch e As Exception
            MsgBox(e.Message)
        End Try

    End Sub

    Public Overrides Sub BeginAdd()
        CtrlTable.mCol(0).ctrlEnabled = False
        CtrlTable.mCol(1).ctrlEnabled = True
        CtrlTable.mCol(2).ctrlEnabled = False
        CtrlTable.mCol(3).ctrlEnabled = True
    End Sub

    Public Overrides Sub BeginEdit()
        CtrlTable.mCol(0).ctrlEnabled = False
        CtrlTable.mCol(1).ctrlEnabled = True
        CtrlTable.mCol(2).ctrlEnabled = False
        CtrlTable.mCol(3).ctrlEnabled = True
    End Sub

    Public Overrides Sub AfterEdit(ByVal TableIndex As Integer)

    End Sub

    Public Overrides Sub ProcessEntry(ByVal ctrl As w32SFDCData.ctrlText, ByVal Valid As Boolean)
        Select Case Valid
            Case False
                Beep()

            Case True
                Try
                    If ctrl.Data <> "" Then
                        Select Case ctrl.Name
                            Case "PSNO"
                                CtrlTable.BeginLoadRS()
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

    Public Overrides Function VerifyForm() As Boolean
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
        SendType = tSendType.TableScan
        InvokeData("SELECT PARTNAME FROM PART WHERE BARCODE = '" & Value & "'")
    End Sub

    Public Overrides Sub EndInvokeData(ByVal Data(,) As String)

        Select Case SendType
            Case tSendType.NextPS
                If IsNothing(Data) Then
                    MsgBox("No more packing slips.")
                    Posted = True
                    Me.Close()
                Else
                    With CtrlForm.el(0)
                        .DataEntry.Text = Data(0, 0)
                        .ProcessEntry()
                    End With
                End If

            Case tSendType.TableScan
                For i As Integer = 0 To CtrlTable.Table.Items.Count - 1
                    With CtrlTable
                        If .Table.Items(i).SubItems(0).Text = Data(0, 0) Then
                            .Table.Items(i).Selected = True
                            .SetEdit()
                            Exit Sub
                        End If
                    End With
                Next
                MsgBox("Scanned part is not on the Packing Slip.")
        End Select

    End Sub

End Class
