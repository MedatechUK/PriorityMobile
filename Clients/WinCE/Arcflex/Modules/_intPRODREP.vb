Imports System.Threading

Public Class interfacePRODREP
    Inherits SFDCData.iForm
#Region "Initialisation"

    Private gr As Boolean = False
    Private route As String = ""
    Private defect As String = ""
    Private userid As String = ""
    Private starttime As Integer
    Private CurrentWO As String = ""
    Private SystemTime As String = ""

    Public Sub New(Optional ByRef App As Form = Nothing)

        'InitializeComponent()
        CallerApp = App
        CtrlTable.DisableButtons(True, False, True, True, False)
        CtrlTable.EnableToolbar(False, False, False, False, True)
        NewArgument("CurrentWO", "")

    End Sub

    Public Overrides Sub FormLoaded()
        MyBase.FormLoaded()
        SendType = tSendType.GetCurrentJob
        InvokeData("select SERIALNAME, ACTDES from ZSFDC_LOAD_STARTTIME, ACT " & _
                   "where ZSFDC_LOAD_STARTTIME.ACTNAME = ACT.ACTNAME " & _
                   "AND USERID = (SELECT USERSB.USERID  " & _
                    "FROM system.dbo.USERS, system.dbo.USERSB  " & _
                    "WHERE USERS.T$USER = USERSB.T$USER  " & _
                    "AND UPPER(USERS.USERLOGIN) = UPPER('" & UserName & "'))")
    End Sub

    Public Overrides Sub FormClose()
        MyBase.FormClose()
        Argument("CurrentWO") = ""        
    End Sub

#End Region

    Public Enum tSendType
        PopulateForm = 1
        GetRouting = 2
        GetDefect = 3
        GetCurrentJob = 4
        GetUserID = 5
        GetStartTime = 6
        ClearStartTime = 7
        GetSystemTime = 8
        SetStartTime = 9
    End Enum
    Dim SendType As tSendType = tSendType.PopulateForm

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
            .MandatoryOnPost = True
        End With
        CtrlForm.AddField(field)

        'Part Name
        With field
            .Name = "PARTNAME"
            .Title = "Part No"
            .ValidExp = ".+"
            .SQLValidation = "SELECT PARTNAME " & _
                            "FROM PART, SERIAL " & _
                            "WHERE PART.PART = SERIAL.PART " & _
                            "AND SERIALNAME = '%SERIALNAME%'"
            .Data = ""      '******** Barcoded field '*******
            .AltEntry = SFDCData.ctrlText.tAltCtrlStyle.ctNone 'ctNone 'ctKeyb 
            .ctrlEnabled = False
            .MandatoryOnPost = False
        End With
        CtrlForm.AddField(field)

        'Qty Outstanding
        With field
            .Name = "SBALANCE"
            .Title = "Qty Outstanding"
            .ValidExp = ".+"
            .SQLValidation = "SELECT SUM(SBALANCE / 1000) " & _
                             " FROM SERIAL " & _
                             " WHERE SERIALNAME = '%SERIALNAME%'"
            .Data = ""      '******** Barcoded field '*******
            .AltEntry = SFDCData.ctrlText.tAltCtrlStyle.ctNone 'ctNone 'ctKeyb 
            .ctrlEnabled = False
            .MandatoryOnPost = False
        End With
        CtrlForm.AddField(field)

        ''Routing
        'With field
        '    .Name = "ACTNAME"
        '    .Title = "Routing"
        '    .ValidExp = ".+"
        '    .SQLList = "SELECT DISTINCT(ACTNAME) FROM ACT, SERACT, SERIAL " & _
        '               " WHERE SERIAL.SERIAL = SERACT.SERIAL " & _
        '               " AND SERACT.ACT = ACT.ACT " & _
        '               " AND SERIALNAME = '%SERIALNAME%'"
        '    .SQLValidation = "SELECT '%ME%' "
        '    .Data = ""      '******** Barcoded field '*******
        '    .AltEntry = SFDCData.ctrlText.tAltCtrlStyle.ctList 'ctNone 'ctKeyb 
        '    .ctrlEnabled = True
        '    .MandatoryOnPost = False
        'End With
        'CtrlForm.AddField(field)

        'Routing Description
        With field
            .Name = "ACTDES"
            .Title = "Op"
            .ValidExp = ".+"
            .SQLList = "SELECT ACTDES " & _
                            " FROM ACT, SERACT, SERIAL " & _
                            " WHERE SERIAL.SERIAL = SERACT.SERIAL " & _
                            " AND SERACT.ACT = ACT.ACT " & _
                            "AND SERIALNAME = '%SERIALNAME%'"
            .SQLValidation = "SELECT ACTDES " & _
                            " FROM ACT, SERACT, SERIAL " & _
                            " WHERE SERIAL.SERIAL = SERACT.SERIAL " & _
                            " AND SERACT.ACT = ACT.ACT " & _
                            "AND SERIALNAME = '%SERIALNAME%' " & _
                            "AND ACT.ACT <> 0 " & _
                            "and ACTDES = '%ME%'"
            .Data = ""      '******** Barcoded field '*******
            .AltEntry = SFDCData.ctrlText.tAltCtrlStyle.ctList 'ctNone 'ctKeyb 
            .ctrlEnabled = True
            .MandatoryOnPost = True
        End With
        CtrlForm.AddField(field)

    End Sub

    Public Overrides Sub TableSettings()

        ' Status
        With col
            .Name = "_STATUS"
            .Title = "Status"
            .initWidth = 25
            .TextAlign = HorizontalAlignment.Left
            .ValidExp = ValidStr(tRegExValidation.tNumeric)
            .SQLValidation = "SELECT '%ME%'"
            .DefaultFromCtrl = Nothing
            .ctrlEnabled = False
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
            .SQLValidation = "SELECT '%ME%'"
            .DefaultFromCtrl = Nothing
            .ctrlEnabled = True
            .Mandatory = False
            .MandatoryOnPost = True
        End With
        CtrlTable.AddCol(col)

        ' Reason
        With col
            .Name = "_REASON"
            .Title = "Reason"
            .initWidth = 58
            .TextAlign = HorizontalAlignment.Left
            .ValidExp = ValidStr(tRegExValidation.tString)
            .SQLList = "SELECT DISTINCT(DEFECTDESC) FROM DEFECTCODES"
            .SQLValidation = "SELECT '%ME%' FROM DEFECTCODES"
            .AltEntry = SFDCData.ctrlText.tAltCtrlStyle.ctList
            .DefaultFromCtrl = Nothing
            .ctrlEnabled = True
            .Mandatory = False
            .MandatoryOnPost = False
        End With
        CtrlTable.AddCol(col)

        ' this bit should load 3 predefined rows
        ' these sould be approved, reject or MRB.
        ' Each row is added with 0 balance and the 
        ' status should be .ctrlEnabled = false

        CtrlTable.RecordsSQL = "Select 'Approved' AS STATUS ,0 as QTY ,'' as REASON " & _
                                "union all " & _
                                "select 'MRB' AS STATUS ,0 as QTY ,'' as REASON " & _
                                "union all " & _
                                "select 'Reject' AS STATUS ,0 as QTY ,'' as REASON "

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
        With CtrlTable.Table
            Select Case .Items(TableIndex).Text
                Case "Reject"
                    .Items.Add(New ListViewItem)
                    With .Items(.Items.Count - 1)
                        .Text = "Reject"
                        .SubItems.Add(New ListViewItem.ListViewSubItem)
                        .SubItems(.SubItems.Count - 1).Text = "0"
                        .SubItems.Add(New ListViewItem.ListViewSubItem)
                        .SubItems(.SubItems.Count - 1).Text = ""
                    End With
            End Select
        End With
    End Sub

    Public Overrides Sub BeginAdd()

    End Sub

    Public Overrides Sub BeginEdit()
        CtrlTable.mCol(0).ctrlEnabled = True
        CtrlTable.mCol(1).ctrlEnabled = False
        CtrlTable.mCol(2).ctrlEnabled = True

        If CtrlForm.el(3).Data.Length = 0 Then
            CtrlTable.CancelEdit = True
            MsgBox("Please select the operation")
            Exit Sub
        End If

        ' at this point we need to throw a numeric form
        ' to capture  the amount of product for the selected status
        Dim num As New frmNumber
        With num
            .Text = "Quantity."
            .ShowDialog()
            ' update the number in the table
            Dim q As String = CStr(.number)
            With CtrlTable
                With .Table
                    .Items(.SelectedIndices(0)).SubItems(1).Text = q
                End With
                .CancelEdit = Not (String.Compare(.Table.Items(.Table.SelectedIndices(0)).SubItems(0).Text, "Reject") = 0)
            End With
            .Dispose()
        End With

    End Sub

    Public Overrides Sub EndInvokeData(ByVal Data(,) As String)
        Select Case SendType
            Case tSendType.PopulateForm
                CtrlForm.el(0).Data = Data(0, 0)
                CtrlForm.el(1).Data = Data(1, 0)
                CtrlForm.el(2).Data = Data(2, 0)

            Case tSendType.GetRouting
                Try
                    route = Data(0, 0)
                Catch
                Finally
                    gr = True
                End Try

            Case tSendType.GetDefect
                Try
                    defect = Data(0, 0)
                Catch
                Finally
                    gr = True
                End Try

            Case tSendType.GetCurrentJob
                If Not IsNothing(Data) Then
                    For y As Integer = 0 To UBound(Data, 2)
                        If MsgBox("Activate job [" & Data(0, y) & "]?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                            Argument("CurrentWO") = Data(0, y)
                            With CtrlForm
                                With .el(.ColNo("SERIALNAME"))
                                    .DataEntry.Text = Data(0, y)
                                    .ProcessEntry()
                                End With
                                With .el(.ColNo("ACTDES"))
                                    .DataEntry.Text = Data(1, y)
                                    .ProcessEntry()
                                End With
                            End With
                            Exit For
                        End If
                    Next
                End If

            Case tSendType.GetUserID
                Try
                    userid = Data(0, 0)
                Catch
                    userid = 0
                Finally
                    gr = True
                End Try

            Case tSendType.GetStartTime
                Try
                    starttime = Data(0, 0)
                Catch
                    starttime = "0"
                Finally
                    ' Delete start time from ZSFDC_LOAD_STARTTIME
                    SendType = tSendType.ClearStartTime
                    InvokeData(" DELETE FROM ZSFDC_LOAD_STARTTIME " & _
                           " WHERE SERIALNAME = '%SERIALNAME%' " & _
                            " AND ACTNAME = '" & route & "' " & _
                            " AND USERID = " & userid & " ")
                    gr = True
                End Try

            Case tSendType.GetSystemTime
                Try
                    SystemTime = Data(0, 0)
                Catch
                    SystemTime = 0
                Finally
                    gr = True
                End Try

            Case tSendType.ClearStartTime, tSendType.SetStartTime
                'Do nothing

        End Select
    End Sub

#End Region

#Region "processing"

    Public Overrides Sub ProcessEntry(ByVal ctrl As SFDCData.ctrlText, ByVal Valid As Boolean)

        Select Case Valid
            Case False
                Beep()

            Case True
                Try
                    If ctrl.Name = "_STATUS" Then
                        CtrlTable.SetTable()
                    End If

                    If ctrl.Name = "ACTDES" Then
                        ctrl.Enabled = False
                    End If

                    If ctrl.Name = "SERIALNAME" Then
                        ctrl.Enabled = CBool(ctrl.Data.Length = 0)
                    End If

                    If ctrl.Name = "SERIALNAME" Then
                        SendType = tSendType.PopulateForm
                        InvokeData("select SERIAL.SERIALNAME, PARTNAME, SBALANCE / 1000 " & _
                                   "from PART, SERIAL " & _
                                   "where PART.PART = SERIAL.PART " & _
                                   "and SERIALNAME =  " & _
                                "(select SERIALNAME from SERIAL where SERIALNAME = '" & CtrlForm.el(0).Data & "')")
                        CtrlTable.Table.Items.Clear()
                        CtrlTable.BeginLoadRS()
                    End If

                Catch
                End Try

        End Select

    End Sub

    Private Sub GetVars()

        gr = False
        SendType = tSendType.GetRouting
        ' query to retreive opperation id from description
        InvokeData("SELECT ACT.ACTNAME " & _
                "FROM ACT, SERACT, SERIAL " & _
                "WHERE SERIAL.SERIAL = SERACT.SERIAL " & _
                "AND SERACT.ACT = ACT.ACT " & _
                "and SERIALNAME =  '%SERIALNAME%' " & _
                "and ACTDES = '%ACTDES%'")

        While Not gr
            Thread.Sleep(1000)
        End While

        gr = False
        SendType = tSendType.GetUserID
        ' query to retreive USERID id 
        InvokeData("SELECT USERSB.USERID  " & _
                "FROM system.dbo.USERS, system.dbo.USERSB  " & _
                "WHERE USERS.T$USER = USERSB.T$USER  " & _
                "AND UPPER(USERS.USERLOGIN) = UPPER('" & UserName & "')")

        While Not gr
            Thread.Sleep(1000)
        End While

        gr = False
        SendType = tSendType.GetSystemTime
        ' query to retreive start time
        InvokeData("select (datepart(hh, getdate()) * 60) + datepart(mi, getdate())")
        While Not gr
            Thread.Sleep(1000)
        End While

    End Sub

    Public Overrides Sub ProcessForm()
        Try

            GetVars()

            gr = False
            SendType = tSendType.GetStartTime
            ' query to retreive start time
            InvokeData(" SELECT EMPSTIME FROM ZSFDC_LOAD_STARTTIME " & _
                       " WHERE SERIALNAME = '%SERIALNAME%' " & _
                        " AND ACTNAME = '" & route & "' " & _
                        " AND USERID = " & userid & " ")
            While Not gr
                Thread.Sleep(1000)
            End While

            With p
                .DebugFlag = False
                .Procedure = "ZSFDC_LOAD_PRODREP"
                .Table = "ZSFDC_LOAD_PRODREP"
                .RecordType1 = _
                                "CURDATE," & _
                                "DETAILS," & _
                                "FINAL," & _
                                "FORMNAME," & _
                                "SHIFTNAME"
                .RecordType2 = _
                                "USERNAME," & _
                                "ACTCANCEL," & _
                                "ACTNAME," & _
                                "ASPAN," & _
                                "DEFECTCODE," & _
                                "EMPASPAN," & _
                                "EMPETIME," & _
                                "EMPSTIME," & _
                                "ETIME," & _
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
                                "SHIFTNAME2," & _
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

                .RecordTypes = _
                                "," & _
                                "TEXT," & _
                                "TEXT," & _
                                "TEXT," & _
                                "TEXT," & _
                                "TEXT," & _
                                "TEXT," & _
                                "TEXT," & _
                                "," & _
                                "TEXT," & _
                                "," & _
                                "," & _
                                "," & _
                                "," & _
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
                                "TEXT"
            End With


            ' Type 1 records
            Dim t1() As String = { _
                                String.Format(DateDiff(DateInterval.Minute, #1/1/1988#, Now).ToString, "M,DATE,8,Date"), _
                                String.Format("", "CHAR,24,Details"), _
                                String.Format("", "CHAR,1,Final"), _
                                String.Format("", "CHAR,16,Form Number"), _
                                String.Format("", "CHAR,8,Shift") _
                                }
            p.AddRecord(1) = t1

            Dim t2() As String = { _
                    UserName, _
                    String.Format("", "CHAR,1,Remove Oper. Number?"), _
                    String.Format(route, "CHAR,16,Operation"), _
                    String.Format("0", "TIME,6,Span"), _
                    String.Format("", "CHAR,3,Defect Code"), _
                    String.Format("0", "TIME,6,Labor Span"), _
                    String.Format("0", "TIME,5,End Labor"), _
                    String.Format(starttime, "TIME,5,Start Labor"), _
                    String.Format(SystemTime, "TIME,5,End Time"), _
                    String.Format("", "CHAR,14,Bin"), _
                    String.Format("", "CHAR,1,Set-up/Break (S/B)"), _
                    String.Format("0", "INT,17,Qty for MRB"), _
                    String.Format("", "CHAR,1,New Pallet?"), _
                    String.Format("0", "INT,6,Packing Crates (No.)"), _
                    String.Format("", "CHAR,2,Packing Crate Code"), _
                    String.Format(CtrlForm.ItemValue("PARTNAME"), "M,CHAR,22,Part Number"), _
                    String.Format("0", "INT,17,Qty Completed"), _
                    String.Format("", "CHAR,1,Rework?"), _
                    String.Format("", "CHAR,1,Remove Wk Order No.?"), _
                    String.Format(CtrlForm.ItemValue("SERIALNAME"), "M,CHAR,22,Work Order"), _
                    String.Format("", "CHAR,8,Shift"), _
                    String.Format("0", "INT,17,Qty Rejected"), _
                    String.Format("0", "TIME,5,Start Time"), _
                    String.Format("0", "INT,17,MRB (Buy/Sell Units)"), _
                    String.Format("", "CHAR,22,Tool"), _
                    String.Format("", "CHAR,16,To Pallet"), _
                    String.Format("0", "INT,17,Completed (Buy/Sell)"), _
                    String.Format("0", "INT,17,Rejected (Buy/Sell)"), _
                    String.Format(userid, "INT,8,0,Employee ID"), _
                    String.Format("", "CHAR,4,To Warehouse"), _
                    String.Format("", "CHAR,6,Work Cell") _
            }
            ' Type 2 records

            p.AddRecord(2) = t2

            For y As Integer = 0 To CtrlTable.RowCount

                ' Ignore empty lines
                If Not (CInt(CtrlTable.Table.Items(y).SubItems(1).Text) = 0) Then

                    Dim ap As Integer = 0
                    Dim rj As Integer = 0
                    Dim mr As Integer = 0
                    Dim re As String = ""
                    defect = ""

                    Select Case CtrlTable.Table.Items(y).SubItems(0).Text.ToLower
                        Case "approved"
                            ap = CInt(CtrlTable.Table.Items(y).SubItems(1).Text)
                        Case "reject"
                            rj = CInt(CtrlTable.Table.Items(y).SubItems(1).Text)
                            gr = False
                            SendType = tSendType.GetDefect
                            ' query to retreive defect id from description
                            InvokeData("SELECT DEFECTCODE " & _
                                    "FROM DEFECTCODES " & _
                                    "WHERE DEFECTDESC = '" & CtrlTable.Table.Items(y).SubItems(2).Text & "'")
                            While Not gr
                                Thread.Sleep(1000)
                            End While
                        Case "mrb"
                            mr = CInt(CtrlTable.Table.Items(y).SubItems(1).Text)
                    End Select

                    Dim t3() As String = { _
                        UserName, _
                        String.Format("", "CHAR,1,Remove Oper. Number?"), _
                        String.Format(route, "CHAR,16,Operation"), _
                        String.Format("0", "TIME,6,Span"), _
                        String.Format(defect, "CHAR,3,Defect Code"), _
                        String.Format("0", "TIME,6,Labor Span"), _
                        String.Format("0", "TIME,5,End Labor"), _
                        String.Format("0", "TIME,5,Start Labor"), _
                        String.Format("0", "TIME,5,End Time"), _
                        String.Format("", "CHAR,14,Bin"), _
                        String.Format("", "CHAR,1,Set-up/Break (S/B)"), _
                        String.Format((CStr(mr) * 1000), "INT,17,Qty for MRB"), _
                        String.Format("", "CHAR,1,New Pallet?"), _
                        String.Format("0", "INT,6,Packing Crates (No.)"), _
                        String.Format("", "CHAR,2,Packing Crate Code"), _
                        String.Format(CtrlForm.ItemValue("PARTNAME"), "M,CHAR,22,Part Number"), _
                        String.Format((CStr(ap) * 1000), "INT,17,Qty Completed"), _
                        String.Format("", "CHAR,1,Rework?"), _
                        String.Format("", "CHAR,1,Remove Wk Order No.?"), _
                        String.Format(CtrlForm.ItemValue("SERIALNAME"), "M,CHAR,22,Work Order"), _
                        String.Format("", "CHAR,8,Shift"), _
                        String.Format((CStr(rj) * 1000), "INT,17,Qty Rejected"), _
                        String.Format("0", "TIME,5,Start Time"), _
                        String.Format((CStr(mr) * 1000), "INT,17,MRB (Buy/Sell Units)"), _
                        String.Format("", "CHAR,22,Tool"), _
                        String.Format("", "CHAR,16,To Pallet"), _
                        String.Format((CStr(ap) * 1000), "INT,17,Completed (Buy/Sell)"), _
                        String.Format((CStr(rj) * 1000), "INT,17,Rejected (Buy/Sell)"), _
                        String.Format(userid, "INT,8,0,Employee ID"), _
                        String.Format("", "CHAR,4,To Warehouse"), _
                        String.Format("", "CHAR,6,Work Cell") _
                                }
                    ' Type 2 records

                    p.AddRecord(2) = t3

                End If
            Next

        Catch e As Exception
            MsgBox(e.Message)

        End Try
    End Sub

    Public Overrides Sub TableScan(ByVal Value As String)

    End Sub

#End Region

#Region "Validation"

    Public Overrides Function VerifyForm() As Boolean

        If CtrlForm.el(CtrlForm.ColNo("ACTDES")).Data.Length = 0 Then
            MsgBox("Please enter an operation.")
            Return False
        End If

        Dim count As Integer = 0
        For y As Integer = 0 To CtrlTable.RowCount
            count += CInt(CtrlTable.Table.Items(y).SubItems(1).Text)
            'Select Case CtrlTable.Table.Items(y).SubItems(0).Text.ToLower
            '    Case "approved"
            '        count += CInt(CtrlTable.Table.Items(y).SubItems(1).Text)
            '    Case "reject"
            '        count += CInt(CtrlTable.Table.Items(y).SubItems(1).Text)
            '    Case "mrb"
            '        count += CInt(CtrlTable.Table.Items(y).SubItems(1).Text)
            'End Select
        Next

        If count = 0 Then
            If Argument("CurrentWO").Length = 0 Then
                If MsgBox("This will record the job start time", MsgBoxStyle.OkCancel) = MsgBoxResult.Ok Then
                    Try
                        GetVars()
                        InvokeData( _
                            "INSERT INTO ZSFDC_LOAD_STARTTIME" & _
                            "(LINE, SERIALNAME, ACTNAME, USERID, EMPSTIME) " & _
                            "select (SELECT MAX(LINE) + 1 FROM ZSFDC_LOAD_STARTTIME), " & _
                            "'%SERIALNAME%', " & _
                            "'" & route & "', " & _
                            userid & ", " & _
                            SystemTime _
                            )
                        MyBase.FormClose()
                        Return False
                    Catch ex As Exception
                        MsgBox(ex.Message)
                    End Try
                Else
                    Return False
                End If
            Else
                Return MsgBox("This will record the job end time", MsgBoxStyle.OkCancel) = MsgBoxResult.Ok
            End If
        End If
        Return True
    End Function

#End Region

End Class