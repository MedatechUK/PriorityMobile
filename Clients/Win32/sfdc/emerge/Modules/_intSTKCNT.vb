Public Class InterfaceSTKCNT
    Inherits w32SFDCData.iForm

#Region "Initialisation"

    Private tld As Boolean = False

    Public Sub New(Optional ByRef App As Form = Nothing)

        InitializeComponent()
        CallerApp = App
        NewArgument("SHOWBAL", "INTERWHTX")
        CtrlTable.DisableButtons(False, False, False, False, False)
        CtrlTable.EnableToolbar(False, False, False, False, False)

    End Sub

#End Region

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
            .Title = "From Bin"
            .ValidExp = ValidStr(tRegExValidation.tLocname)
            .SQLList = "SELECT DISTINCT LOCNAME FROM WAREHOUSES WHERE WARHSNAME = '%WARHS%' AND INACTIVE <> 'Y'"
            .SQLValidation = "SELECT LOCNAME FROM WAREHOUSES WHERE LOCNAME = '%ME%' AND WARHSNAME = '%WARHS%'"
            .Data = ""      '******** Barcoded field '*******
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
            .SQLValidation = "SELECT BARCODE, PARTNAME " & _
                            "FROM dbo.SVCCALL_PARTS() " & _
                            "WHERE BARCODE = '%ME%'"
            .DefaultFromCtrl = Nothing '
            .ctrlEnabled = True
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
            .ctrlEnabled = True
            .Mandatory = False
        End With
        CtrlTable.AddCol(col)

        If Argument("SHOWBAL") = "TRUE" Then
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
                .ctrlEnabled = True
                .Mandatory = False
            End With
            CtrlTable.AddCol(col)
        End If

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

        If Argument("SHOWBAL") = "TRUE" Then
            ' Set the query to load recordtype 2s
            CtrlTable.RecordsSQL = "select PART.PARTNAME, PART.PARTDES, CUSTOMERS.CUSTNAME, dbo.REALQUANT(BALANCE) as BALANCE, '' AS CQUANT " & _
                                    "from WARHSBAL, WAREHOUSES, CUSTOMERS, PART " & _
                                    "where WARHSBAL.WARHS = WAREHOUSES.WARHS " & _
                                    "and WARHSBAL.PART = PART.PART " & _
                                    "and WARHSBAL.CUST = CUSTOMERS.CUST " & _
                                    "and WARHSBAL.WARHS = " & _
                                    "(select WARHS from WAREHOUSES where WARHSNAME= '%WARHS%' " & _
                                    "and LOCNAME = '%LOCNAME%') " & _
                                    "ORDER BY PART.PARTNAME"
        Else
            ' Set the query to load recordtype 2s
            CtrlTable.RecordsSQL = "select PART.PARTNAME, PART.PARTDES, CUSTOMERS.CUSTNAME, '' AS CQUANT " & _
                                    "from WARHSBAL, WAREHOUSES, CUSTOMERS, PART " & _
                                    "where WARHSBAL.WARHS = WAREHOUSES.WARHS " & _
                                    "and WARHSBAL.PART = PART.PART " & _
                                    "and WARHSBAL.CUST = CUSTOMERS.CUST " & _
                                    "and WARHSBAL.WARHS = " & _
                                    "(select WARHS from WAREHOUSES where WARHSNAME= '%WARHS%' " & _
                                    "and LOCNAME = '%LOCNAME%') " & _
                                    "ORDER BY PART.PARTNAME"
        End If

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
        Dim bol As Boolean = False

        If Argument("SHOWBAL") = "TRUE" Then
            bol = Len(CtrlTable.Table.Items(TableIndex).SubItems(3).Text) > 0
        Else
            bol = Len(CtrlTable.Table.Items(TableIndex).SubItems(2).Text) > 0
        End If

        If bol Then
            Dim lvi As New ListViewItem
            lvi.Text = "***"

            CtrlTable.Table.Items.Add(lvi)
            For i As Integer = 0 To CtrlTable.Table.Items.Count - 1
                If CtrlTable.Table.Items(i).Text = "***" Then
                    CtrlTable.Table.Items(i).Text = CtrlTable.Table.Items(TableIndex).Text
                    For n As Integer = 1 To CtrlTable.Table.Items(TableIndex).SubItems.Count - 1
                        CtrlTable.Table.Items(i).SubItems.Add(CtrlTable.Table.Items(TableIndex).SubItems(n).Text)
                    Next
                End If
            Next

            lvi = CtrlTable.Table.Items(TableIndex)
            CtrlTable.Table.Items.Remove(lvi)

        End If
    End Sub

    Public Overrides Sub BeginAdd()
        If Argument("SHOWBAL") = "TRUE" Then
            CtrlTable.mCol(0).ctrlEnabled = True
            CtrlTable.mCol(0).Mandatory = True

            CtrlTable.mCol(1).ctrlEnabled = False
            CtrlTable.mCol(1).Mandatory = False

            CtrlTable.mCol(2).ctrlEnabled = True
            CtrlTable.mCol(2).Mandatory = True

            CtrlTable.mCol(3).ctrlEnabled = False
            CtrlTable.mCol(3).Mandatory = False

            CtrlTable.mCol(4).ctrlEnabled = True
            CtrlTable.mCol(4).Mandatory = True

        Else
            CtrlTable.mCol(0).ctrlEnabled = True
            CtrlTable.mCol(0).Mandatory = True

            CtrlTable.mCol(1).ctrlEnabled = False
            CtrlTable.mCol(1).Mandatory = False

            CtrlTable.mCol(2).ctrlEnabled = True
            CtrlTable.mCol(2).Mandatory = True

            CtrlTable.mCol(3).ctrlEnabled = True
            CtrlTable.mCol(3).Mandatory = True

        End If
    End Sub

    Public Overrides Sub BeginEdit()
        If Argument("SHOWBAL") = "TRUE" Then
            CtrlTable.mCol(0).ctrlEnabled = False
            CtrlTable.mCol(0).Mandatory = False
            CtrlTable.mCol(1).ctrlEnabled = False
            CtrlTable.mCol(1).Mandatory = False
            CtrlTable.mCol(2).ctrlEnabled = False
            CtrlTable.mCol(2).Mandatory = False
            CtrlTable.mCol(3).ctrlEnabled = False
            CtrlTable.mCol(3).Mandatory = False
            CtrlTable.mCol(4).ctrlEnabled = True
            CtrlTable.mCol(4).Mandatory = True
        Else
            CtrlTable.mCol(0).ctrlEnabled = False
            CtrlTable.mCol(0).Mandatory = False
            CtrlTable.mCol(1).ctrlEnabled = False
            CtrlTable.mCol(1).Mandatory = False
            CtrlTable.mCol(2).ctrlEnabled = False
            CtrlTable.mCol(2).Mandatory = False
            CtrlTable.mCol(3).ctrlEnabled = True
            CtrlTable.mCol(3).Mandatory = True
        End If
    End Sub

    Public Overrides Sub ProcessEntry(ByVal ctrl As w32SFDCData.ctrlText, ByVal Valid As Boolean)
        Select Case Valid
            Case False
                Beep()

            Case True
                Try

                    If ctrl.Name = "_PARTNAME" Then
                        CtrlTable.el(1).NSInvoke("select PARTDES " & _
                                                "from PART " & _
                                                "where PART =  " & _
                                                "(select PART from PART where PARTNAME = '" & CtrlTable.el(0).Data & "')")
                    End If

                    Try
                        If ctrl.Name = "LOCNAME" Then
                            If Not tld Then
                                CtrlTable.Table.Items.Clear()
                                CtrlForm.el(0).Enabled = False
                                CtrlForm.el(1).Enabled = False
                                CtrlTable.BeginLoadRS()
                                tld = True
                            End If
                        End If
                    Catch
                    End Try

                    ' *******************************************************************
                    ' *** Set which controls are enabled
                    'CtrlForm.el(1).CtrlEnabled = Len(CtrlForm.el(0).Data) > 0
                    'CtrlForm.el(3).CtrlEnabled = Len(CtrlForm.el(2).Data) > 0

                    'CtrlTable.el(0).CtrlEnabled = Len(CtrlTable.el(1).Data) > 0 And Len(CtrlTable.el(3).Data) > 0
                    'CtrlTable.el(1).CtrlEnabled = Len(CtrlForm.el(0).Data) > 0
                    'CtrlTable.el(2).CtrlEnabled = Len(CtrlForm.el(2).Data) > 0
                    ''CtrlTable.el(4).CtrlEnabled = Len(CtrlTable.el(3).Data) > 0
                    ''CtrlTable.el(0).CtrlEnabled = Len(CtrlTable.el(1).Data) > 0 And Len(CtrlTable.el(2).Data) > 0 And Len(CtrlTable.el(5).Data) > 0
                    'CtrlTable.el(6).CtrlEnabled = Len(CtrlTable.el(0).Data) > 0 And Len(CtrlTable.el(1).Data) > 0 And Len(CtrlTable.el(3).Data) > 0
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
        InvokeData("SELECT PARTNAME, BARCODE FROM PART WHERE BARCODE = '" & Value & "'")
    End Sub

    Public Overrides Sub EndInvokeData(ByVal Data(,) As String)
        Dim f As Boolean = False
        Dim fat As Integer = 0

        For i As Integer = 0 To CtrlTable.Table.Items.Count - 1
            If CtrlTable.Table.Items(i).Text = Data(0, 0) Then
                f = True
                fat = i
            Else
                CtrlTable.Table.Items(i).Selected = False
            End If
        Next

        Dim opt As frmOption = New frmOption
        If f Then
            opt.NewOption("opt1", "Open part for count")
        Else
            opt.NewOption("opt2", "Add part to count")
        End If
        opt.NewOption("opt3", "Open stock lookup")

        Select Case opt.ShowDialog()
            Case Windows.Forms.DialogResult.OK
                Select Case opt.Selected
                    Case "opt1"
                        CtrlTable.Table.Items(fat).Selected = True
                        CtrlTable.SetEdit()
                    Case "opt2"
                        CtrlTable.SetAdd()
                        CtrlTable.el(0).DataEntry.Text = Data(1, 0)
                        CtrlTable.el(0).ProcessEntry()
                    Case "opt3"
                        'Type of form to start, The parameter to pass to the form
                        Dim par(1, 0) As String
                        par(0, 0) = "BARCODE"
                        par(1, 0) = Data(1, 0)
                        Me.doNewForm(o.PARTLU, par)
                End Select
            Case Else
                ' Do nothing
        End Select
    End Sub

End Class