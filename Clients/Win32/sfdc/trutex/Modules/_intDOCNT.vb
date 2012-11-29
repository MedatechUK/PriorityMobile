Public Class interfaceDOCNT
    Inherits w32SFDCData.iForm

#Region "Initialisation"

    Private tld As Boolean = False

    Public Sub New(Optional ByRef App As Form = Nothing)

        'InitializeComponent()
        CallerApp = App
        NewArgument("CNTNO", "")
        NewArgument("SHOWBAL", "FALSE")
        CtrlTable.DisableButtons(True, False, True, True, False)
        CtrlTable.EnableToolbar(False, False, False, False, False)

    End Sub

#End Region

    Public Overrides Sub FormLoaded()
        MyBase.FormLoaded()
        With CtrlForm.el(0)
            .DataEntry.Text = Argument("CNTNO")
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
                             "where TYPE = 'C' " & _
                             "AND DOCNO = '%ME%'"
            .Data = ""      '******** Barcoded field '*******
            .AltEntry = ctrlText.tAltCtrlStyle.ctNone 'ctNone 'ctKeyb 
            .ctrlEnabled = False
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
            .SQLValidation = "SELECT BARCODE, PARTNAME " & _
                            "FROM dbo.SVCCALL_PARTS() " & _
                            "WHERE BARCODE = '%ME%'"
            .DefaultFromCtrl = Nothing '
            .ctrlEnabled = False
            .Mandatory = True
        End With
        CtrlTable.AddCol(col)

        ' PARTDES
        With col
            .Name = "_PARTDES"
            .Title = "Part"
            .initWidth = 30
            .TextAlign = HorizontalAlignment.Left
            .AltEntry = ctrlText.tAltCtrlStyle.ctNone 'ctKeyb 
            .ValidExp = "^.+$"
            ' Second field replaces first field if first field validates ok
            .SQLValidation = "SELECT '%ME%'"
            .DefaultFromCtrl = Nothing '
            .ctrlEnabled = False
            .Mandatory = True
        End With
        CtrlTable.AddCol(col)

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

        ' Counted Quantity
        With col
            .Name = "_CQUANT"
            .Title = "Counted QTY"
            .initWidth = 20
            .TextAlign = HorizontalAlignment.Left
            .AltEntry = ctrlText.tAltCtrlStyle.ctNone 'ctKeyb 
            .ValidExp = ValidStr(tRegExValidation.tNumeric)
            .SQLValidation = "select %ME%"
            .DefaultFromCtrl = Nothing
            .ctrlEnabled = True
            .Mandatory = False
        End With

        CtrlTable.AddCol(col)

        CtrlTable.RecordsSQL = "select PART.PARTNAME , PART.PARTDES, WARHSNAME, LOCNAME, CUSTNAME , CQUANT " & _
                                        "from TRANSORDER, PART , WAREHOUSES, CUSTOMERS " & _
                                        "where TRANSORDER.PART = PART.PART " & _
                                        "AND TRANSORDER.TOWARHS = WAREHOUSES.WARHS " & _
                                        "AND TRANSORDER.TOCUST = CUSTOMERS.CUST " & _
                                        "and DOC =  " & _
                                        "(select DOC from DOCUMENTS where DOCNO = '%DOCNO%')"

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

    Public Overrides Sub ProcessEntry(ByVal ctrl As w32SFDCData.ctrlText, ByVal Valid As Boolean)
        Select Case Valid
            Case False
                Beep()

            Case True
                Try
                    If ctrl.Name = "DOCNO" Then
                        CtrlTable.BeginLoadRS()
                    End If

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

    End Sub

    Public Overrides Sub EndInvokeData(ByVal Data(,) As String)

    End Sub

End Class
