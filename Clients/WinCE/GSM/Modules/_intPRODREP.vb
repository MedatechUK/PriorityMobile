Imports System.Threading
Imports System.Data
Imports System.Text.RegularExpressions
Imports System.Xml
Imports OpenNETCF.Net.Mail
Imports System.Windows.Forms
Imports System




Public Class interfacePRODREP
    Inherits SFDCData.iForm

#Region "Initialisation"

    Private gr As Boolean = False
    Private route As String = ""
    Private defect As String = ""
    Private userid As String = ""
    Private starttime As Integer
    Private FOATIME As Integer = 0
    Private CurrentWO As String = ""
    Private curdate As Integer = 0
    Private pid As Integer = 0
    Private SystemTime As String = ""
    Private wono As String = ""
    Private PartsList As New List(Of Parts)
    Private newact As Boolean = True
    Private ACT As Integer = 0
    Private custname As String = ""


    Public Sub New(Optional ByRef App As Form = Nothing)

        'InitializeComponent()
        CallerApp = App
        CtrlTable.DisableButtons(True, False, True, True, False)
        CtrlTable.EnableToolbar(False, False, False, False, True)
        'Create the CurrentWO argument
        NewArgument("CurrentWO", "")

    End Sub

    Public Overrides Sub FormLoaded()
        MyBase.FormLoaded()
        'set the value of the argument to ""
        Argument("CurrentWO") = ""
        'get the current date and set the variable curdate
        Dim dhold As DateTime = FormatDateTime("1/1/1988")
        curdate = DateDiff(DateInterval.Minute, dhold, Now)
        CtrlTable.Focus()
    End Sub

    Public Overrides Sub FormClose()
        MyBase.FormClose()
        'clear the argument on closing
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
        Part = 10
        PartUpdate = 11
        GetFlag = 12
        GetFOATime = 13
        GetUpdate = 14
        getAct = 15
        GetDefect2 = 16
    End Enum
    Dim SendType As tSendType = tSendType.PopulateForm
    Private GetPass As Boolean = False
#Region "Column Settings"
    Public Overrides Sub FormSettings()

        'Work Order Number
        With field
            .Name = "SERIALNAME"
            .Title = "Work Order"
            .ValidExp = ValidStr(tRegExValidation.tWO)
            .SQLValidation = "SELECT SERIALNAME " & _
                            "FROM SERIAL " & _
                            "WHERE SERIALNAME = '%ME%' AND CLOSED <> 'C' AND RELEASE = 'Y'"
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

        'Stop Field
        With field
            .Name = "FOA"
            .Title = "FOA / Run"
            .ValidExp = ".+"
            .SQLList = "SELECT 'Yes','No' as YesNo"
            .SQLValidation = "SELECT '%ME%'"
            .Data = ""      '******** Barcoded field '*******
            .AltEntry = SFDCData.ctrlText.tAltCtrlStyle.ctList 'ctNone 'ctKeyb 
            .ctrlEnabled = True
            .MandatoryOnPost = False
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
            .ValidExp = ".*"
            .SQLList = "SELECT DISTINCT(DEFECTCODES.DEFECTDESC)" & _
            " FROM DEFECTCODES, ZGSM_OPDEFECTCODES, ACT" & _
            " WHERE ZGSM_OPDEFECTCODES.ACT = ACT.ACT AND" & _
            " DEFECTCODES.DEFECT = ZGSM_OPDEFECTCODES.DEFECT" & _
            " AND DEFECTCODES.INACTIVE <> 'Y'" & _
            " AND ACT.ACTDES = '%ACTDES%'"

            .SQLValidation = "SELECT '%ME%' FROM DEFECTCODES"
            .AltEntry = SFDCData.ctrlText.tAltCtrlStyle.ctList
            .DefaultFromCtrl = Nothing
            .ctrlEnabled = True
            .Mandatory = False
            .MandatoryOnPost = False
        End With
        CtrlTable.AddCol(col)
        'We need to get the table ready so we add 2 rows approved and defect

        CtrlTable.RecordsSQL = "Select 'Approved' AS STATUS ,0 as QTY ,'' as REASON " & _
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
            OverControl.msgboxa(e.Message)
        End Try
        CtrlTable.Focus()
    End Sub

#End Region

#Region "EVENT HANDLERS"

    Public Overrides Sub AfterAdd(ByVal TableIndex As Integer)

    End Sub

    Public Overrides Sub AfterEdit(ByVal TableIndex As Integer)
        'we need to handle the Reject rows being changed and add a new blank row for the next reject reason
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


    Public Overrides Sub BeginEdit()
        CtrlTable.mCol(0).ctrlEnabled = True
        CtrlTable.mCol(1).ctrlEnabled = False
        CtrlTable.mCol(2).ctrlEnabled = True

        If CtrlForm.el(3).Data.Length = 0 Then
            CtrlTable.CancelEdit = True
            OverControl.msgboxa("Please select the operation")
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
                custname = Data(3, 0)
                CtrlTable.Focus()
                CtrlForm.el(2).Focus()
                'CtrlForm.el(3).Data = ""
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

            Case tSendType.GetDefect2
                If Data Is Nothing = False Then
                    Dim q As Integer

                    Dim amt As New frmNumber


                    amt.ShowDialog()
                    If amt.DialogResult = Windows.Forms.DialogResult.Cancel Then
                        q = amt.number
                        Dim lvi As New ListViewItem
                        With CtrlTable.Table
                            .Items.Add(lvi)
                            .Items(.Items.Count - 1).Text = "Reject"
                            .Items(.Items.Count - 1).SubItems.Add(q)
                            .Items(.Items.Count - 1).SubItems.Add(Data(0, 0))
                        End With
                    End If
                   

                End If



            Case tSendType.GetCurrentJob
                If Not IsNothing(Data) Then
                    For y As Integer = 0 To UBound(Data, 2)
                        Dim f As New frmYesNo
                        f.Text = Data(0, y) & " in progress "
                        f.Label1.Text = "Continue working on job " & Data(0, y) & " " & Data(1, y) & "?"
                        f.ShowDialog()

                        If f.DialogResult = DialogResult.Yes Then
                            Argument("CurrentWO") = Data(0, y)
                            wono = Data(0, y)
                            With CtrlForm
                                'With .el(.ColNo("SERIALNAME"))
                                '    .DataEntry.Text = Data(0, y)
                                '    .ProcessEntry()
                                'End With
                                newact = False
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
                    FOATIME = Data(1, 0)
                Catch
                    starttime = "0"
                    FOATIME = 0
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

            Case tSendType.GetFOATime
                Try
                    FOATIME = Data(0, 0)
                Catch
                    FOATIME = 0
                Finally

                End Try

            Case tSendType.ClearStartTime, tSendType.SetStartTime
                'Do nothing

            Case tSendType.Part
                If Data Is Nothing Then
                    Exit Select
                End If
                'now we open a copy of the form
                Dim f As New frmAddParts
                Try
                    'fill the list of parts
                    Dim i As Integer
                    i = UBound(Data, 2)
                    For y As Integer = 0 To UBound(Data, 2)
                        Dim PA As New Parts(Data(0, y), Data(1, y), Data(2, y), 0, " ", Data(3, y), "N", Data(4, y))

                        PartsList.Add(PA)
                    Next
                    'next we add these parts to the form



                    Dim p1 As New DataColumn
                    Dim p2 As New DataColumn
                    Dim p3 As New DataColumn
                    Dim p4 As New DataColumn
                    Dim p5 As New DataColumn
                    Dim p6 As New DataColumn
                    Dim p7 As New DataColumn


                    'Col 1 is the Part ID
                    p1.DataType = System.Type.GetType("System.Int32")
                    p1.ColumnName = "ID"
                    p1.Caption = "ID"
                    p1.AutoIncrement = False
                    f.pars.Columns.Add(p1)

                    'Col 2 is the Part Description
                    p2.DataType = System.Type.GetType("System.String")
                    p2.ColumnName = "Description"
                    p2.Caption = "Description"
                    p2.AutoIncrement = False
                    f.pars.Columns.Add(p2)

                    ''Col 3 is the Part Quantity
                    'p3.DataType = System.Type.GetType("System.Int32")
                    'p3.ColumnName = "Quantity"
                    'p3.Caption = "Quantity"
                    'p3.AutoIncrement = False
                    'f.pars.Columns.Add(p3)

                    'Col 2 is the LOT
                    p4.DataType = System.Type.GetType("System.String")
                    p4.ColumnName = "Name"
                    p4.Caption = "Part Name"
                    p4.AutoIncrement = False
                    f.pars.Columns.Add(p4)

                    'Col 2 is the Current Op
                    'p5.DataType = System.Type.GetType("System.String")
                    'p5.ColumnName = "Operation"
                    'p5.Caption = "Operation"
                    'p5.AutoIncrement = False
                    'f.pars.Columns.Add(p5)

                    'p6.DataType = System.Type.GetType("System.String")
                    'p6.ColumnName = "Checked"
                    'p6.Caption = "Checked"
                    'p6.AutoIncrement = False
                    'f.pars.Columns.Add(p6)

                    p7.DataType = System.Type.GetType("System.String")
                    p7.ColumnName = "Barcode"
                    p7.Caption = "Barcode"
                    p7.AutoIncrement = False
                    f.pars.Columns.Add(p7)

                Catch ex As Exception
                    OverControl.msgboxa(ex.ToString)

                End Try
                'Now we iterate through the list and take all the parts for the currently chosen operation

                If PartsList.Count <> 0 Then
                    For Each p As Parts In PartsList

                        Dim pr As DataRow
                        pr = f.pars.NewRow
                        pr("ID") = p.pID
                        pr("Description") = p.Desc
                        pr("Name") = p.pName
                        'pr("Quantity") = p.qua
                        'pr("Lot") = p.lt
                        'pr("Operation") = p.op
                        'pr("Checked") = p.pChecked
                        pr("Barcode") = p.pBarcode
                        f.pars.Rows.Add(pr)

                    Next
                End If

                'then we fill the datafrid with the parts
                f.DataGrid1.DataSource = f.pars

                f.ShowDialog()
                If f.DialogResult = Windows.Forms.DialogResult.OK Then
                    SendType = tSendType.GetUserID

                    InvokeData("SELECT USERSB.USERID  " & _
                 "FROM system.dbo.USERS, system.dbo.USERSB  " & _
                 "WHERE USERS.T$USER = USERSB.T$USER  " & _
                 "AND UPPER(USERS.USERLOGIN) = UPPER('" & UserName & "')")
                    SendType = tSendType.PartUpdate
                    For Each p As Parts In PartsList
                        pid = p.pID
                        InvokeData("Select SERIAL from SERIAL where SERIALNAME = '" & wono & "'")
                    Next
                    PartsList.Clear()

                End If
                f.Dispose()

            Case tSendType.getAct
                If Data Is Nothing Then
                    OverControl.msgboxa("No defect codes found for this operation")
                Else
                    ACT = Data(0, 0)
                End If
            Case tSendType.PartUpdate


                If Data Is Nothing Then
                    ' OverControl.msgboxa("!")
                Else
                    InvokeData("update KITITEMS set ZGSM_UDATE = '" & curdate & "', ZGSM_CHECKED = 'Y', ZGSM_USER = " & userid & " where SERIAL = '" & Data(0, 0) & "' and PART = " & pid)
                End If

            Case tSendType.GetFlag
                If Data Is Nothing Then
                    GetPass = False
                Else
                    If Data(1, 0) = "Y" Then
                        GetPass = True
                    Else
                        GetPass = False
                    End If
                End If
        End Select
        CtrlTable.Focus()
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
                        SendType = tSendType.getAct
                        InvokeData("SELECT ACT FROM ACT WHERE ACTDES = '%ACTDES%'")
                        ctrl.Enabled = False
                        CtrlTable.Focus()
                    End If

                    If ctrl.Name = "SERIALNAME" Then
                        ctrl.Enabled = CBool(ctrl.Data.Length = 0)
                        CtrlTable.Focus()
                        SendType = tSendType.GetCurrentJob
                        InvokeData("select SERIALNAME, ACTDES from ZSFDC_LOAD_STARTTIME, ACT " & _
                                   "where ZSFDC_LOAD_STARTTIME.ACTNAME = ACT.ACTNAME " & _
                                   "and ZSFDC_LOAD_STARTTIME.SERIALNAME = '%SERIALNAME%' " & _
                                   "AND USERID = (SELECT USERSB.USERID  " & _
                                    "FROM system.dbo.USERS, system.dbo.USERSB  " & _
                                    "WHERE USERS.T$USER = USERSB.T$USER  " & _
                                    "AND UPPER(USERS.USERLOGIN) = UPPER('" & UserName & "'))")
                    End If

                    If ctrl.Name = "SERIALNAME" Then
                        SendType = tSendType.PopulateForm
                        InvokeData("select SERIAL.SERIALNAME, PARTNAME, (SBALANCE / 1000), CUSTOMERS.CUSTDES " & _
                                   "from PART, SERIAL,ORDERITEMS,ORDERS,CUSTOMERS " & _
                                   "where PART.PART = SERIAL.PART AND SERIAL.ORDI = ORDERITEMS.ORDI and ORDERITEMS.ORD = ORDERS.ORD AND ORDERS.CUST = CUSTOMERS.CUST " & _
                                   "and SERIALNAME =  " & _
                                "(select SERIALNAME from SERIAL where SERIALNAME = '" & CtrlForm.el(0).Data & "')")
                        wono = CtrlForm.el(0).Data
                        CtrlTable.Table.Items.Clear()
                        CtrlTable.BeginLoadRS()
                    End If

                    If ctrl.Name = "FOA" Then



                        SendType = tSendType.GetFlag
                        InvokeData("SELECT ACT.ACTNAME, PROCACT.ZGSM_STOP" & _
            " FROM ACT, SERACT, SERIAL, PROCESS, PROCACT" & _
            " WHERE SERIAL.SERIAL = SERACT.SERIAL" & _
            " AND SERACT.ACT = ACT.ACT" & _
            " AND SERIAL.PRODSERIAL = PROCESS.T$PROC" & _
            " AND PROCESS.T$PROC = PROCACT.T$PROC" & _
            " AND ACT.ACT = PROCACT.ACT" & _
            " AND SERIALNAME =  '%SERIALNAME%'" & _
            " AND ACTDES = '%ACTDES%'")
                        Select Case GetPass
                            Case False
                                If ctrl.Data = "Yes" And CtrlForm.el(3).Data <> "" Then
                                    GetVars()

                                    SendType = tSendType.GetFOATime
                                    ' query to retreive start time
                                    InvokeData("select (datepart(hh, getdate()) * 60) + datepart(mi, getdate())")
                                    SendType = tSendType.GetFOATime
                                    ' query to retreive start time
                                    InvokeData(" UPDATE ZSFDC_LOAD_STARTTIME " & _
                                               "SET FOATIME = " & FOATIME & _
                                   " WHERE SERIALNAME = '%SERIALNAME%' " & _
                                    " AND ACTNAME = '" & route & "' " & _
                                    " AND USERID = " & userid & " ")
                                    'OverControl.msgboxa("Updated the FOA Time")
                                    Me.FormClose()

                                End If
                            Case True
                                Try
                                    Dim msg As New MailMessage
                                    Dim client As New SmtpClient

                                    Dim smtp As String = "MRMCHENRY.gsmgroup.gsm" '"mobile-b.gsmautomotive.net"
                                    Dim from As String = "DoNotReply@gsmautomotive.co.uk"
                                    Dim sto As String = "quality@gsmautomotive.co.uk"

                                    Dim FroAddy As New MailAddress(from)

                                    msg.From = FroAddy
                                    msg.To.Add(sto)
                                    msg.Subject = "Attention Needed"

                                    Dim bd As String = "Production user - " & UserName & " has hit a stop sequence on W/O - " & CtrlForm.el(0).Data & ", operation - " & CtrlForm.el(3).Data & ". Producing " & CtrlForm.el(1).Data & " for " & custname & ""
                                    msg.Body = bd

                                    client.Host = smtp
                                    client.Port = 25

                                    client.Credentials = New SmtpCredential("", "", "GSM") ' New SmtpCredential("emerge.priority", "1amBatman", "")


                                    client.Send(msg)

                                Catch ex As Exception
                                    Dim snd As New Sound("Chord.wav")
                                    snd.Play()

                                    OverControl.msgboxa("Error sending email, please contact quality!")

                                End Try
                                Dim f As New frmPassCode
                                f.ShowDialog()
                                If f.DialogResult = Windows.Forms.DialogResult.OK Then

                                    If ctrl.Data = "Yes" And CtrlForm.el(3).Data <> "" Then
                                        GetVars()

                                        SendType = tSendType.GetFOATime
                                        ' query to retreive start time
                                        InvokeData("select (datepart(hh, getdate()) * 60) + datepart(mi, getdate())")
                                        SendType = tSendType.GetFOATime
                                        ' query to retreive start time
                                        InvokeData(" UPDATE ZSFDC_LOAD_STARTTIME " & _
                                                   "SET FOATIME = " & FOATIME & _
                                       " WHERE SERIALNAME = '%SERIALNAME%' " & _
                                        " AND ACTNAME = '" & route & "' " & _
                                        " AND USERID = " & userid & " ")
                                        'OverControl.msgboxa("Updated the FOA Time")
                                        Me.FormClose()

                                    End If
                                Else

                                End If
                        End Select

                        'If OverControl.msgboxa("This will record the job start time", OverControl.msgboxaStyle.OkCancel) = OverControl.msgboxaResult.Ok Then

                        'Else
                        'Return False
                        'End If





                    End If
                Catch
                End Try

        End Select
        CtrlTable.Focus()
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
    Private Sub getparts()
        SendType = tSendType.Part
        InvokeData("SELECT PART,PARTNAME,PARTDES,ACTNAME,BARCODE FROM v_WOACTS WHERE SERIALNAME = '%SERIALNAME%' AND PART <> 0 and ACTNAME = '" & route & "'")

    End Sub

    Public Overrides Sub ProcessForm()
        Try

            GetVars()

            gr = False
            SendType = tSendType.GetStartTime
            ' query to retreive start time
            InvokeData(" SELECT EMPSTIME,FOATIME FROM ZSFDC_LOAD_STARTTIME " & _
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
                                "WORKCNAME," & _
                                "FOATIME"

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
                                "TEXT" & _
                                ",TEXT"
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
                    String.Format("", "CHAR,6,Work Cell"), _
                    String.Format("0", "TIME,5,FOA Time") _
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
                        String.Format("", "CHAR,6,Work Cell"), _
                        String.Format("0", "TIME,5,FOA Time") _
                                }
                    ' Type 2 records

                    p.AddRecord(2) = t3

                End If
            Next

        Catch e As Exception
            OverControl.msgboxa(e.Message)

        End Try
    End Sub

    Public Overrides Sub TableScan(ByVal Value As String)
        Dim v2 As String = ""
        Dim doc As New Xml.XmlDocument

        Try
            Value = Value.Replace(":", "")
            doc.LoadXml(Value)
            If Regex.IsMatch(Value, "^<") = False Then
                OverControl.msgboxa("This doesnt appear to be a valid barcode")
            Else



                For Each nd As XmlNode In doc.SelectNodes("in/i")
                    Dim DataType As String = nd.Attributes("n").Value
                    Select Case DataType
                        Case "SERIAL"
                            With CtrlForm
                                If .el(0).Data = "" Then
                                    .el(0).Data = nd.Attributes("v").Value
                                    .el(0).DataEntry.Text = nd.Attributes("v").Value
                                    .el(0).ProcessEntry()
                                End If
                                wono = nd.Attributes("v").Value
                            End With

                        Case "POS"
                            With CtrlForm
                                .el(3).Data = nd.Attributes("v").Value
                                .el(3).DataEntry.Text = nd.Attributes("v").Value
                                .el(3).ProcessEntry()
                            End With

                        Case "FOA"

                            With CtrlForm
                                .el(4).Data = nd.Attributes("v").Value
                                .el(4).DataEntry.Text = nd.Attributes("v").Value
                                .el(4).ProcessEntry()
                            End With
                            'Case "ACT"
                            '    With CtrlForm
                            '        .el(3).Data = nd.Attributes("v").Value
                            '        .el(3).DataEntry.Text = nd.Attributes("v").Value
                            '        .el(3).ProcessEntry()
                            'End With
                        Case "PROCESS"
                            'Dim i As Control
                            'For Each i In Me.Controls
                            '    If TypeOf i Is SFDCData.CtrlTable Then
                            '        Dim fff As Rectangle
                            '        fff = i.Bounds
                            '        Dim mopos As Point
                            '        mopos.X = (fff.Width - 5)
                            '        mopos.Y = (fff.Top + 5)
                            '        MoveMouse(mopos.Y, mopos.X)
                            '        LeftClick(mopos.Y, mopos.X)

                            '    End If

                            'Next
                            'todo:  ghdfg 

                            ProcessForm()


                        Case "CLOSE"
                            Me.CloseMe()

                        Case "DEF"
                            With CtrlForm
                                If .el(0).Data <> "" And .el(3).Data <> "" Then
                                    SendType = tSendType.GetDefect2
                                    InvokeData("SELECT DISTINCT(DEFECTCODES.DEFECTDESC)" & _
                " FROM DEFECTCODES, ZGSM_OPDEFECTCODES, ACT" & _
                " WHERE ZGSM_OPDEFECTCODES.ACT = ACT.ACT AND" & _
                " DEFECTCODES.DEFECT = ZGSM_OPDEFECTCODES.DEFECT" & _
                " AND DEFECTCODES.INACTIVE <> 'Y'" & _
                " AND ACT.ACTDES = '%ACTDES%' and DEFECTCODES.DEFECTCODE = '" & nd.Attributes("v").Value & "'")
                                    
                                Else
                                    Beep()

                                End If

                            End With



                    End Select

                Next
                Dim f As Boolean = False
                Dim add As Integer = 0
            End If
        Catch ex As XmlException
            OverControl.msgboxa("There is an error within the xml of this code which has made it unable to be translated")

        End Try

        CtrlTable.Focus()

    End Sub

#End Region

#Region "Validation"

    Public Overrides Function VerifyForm() As Boolean

        If CtrlForm.el(CtrlForm.ColNo("ACTDES")).Data.Length = 0 Then
            OverControl.msgboxa("Please enter an operation.")
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
            '        If Argument("CurrentWO").Length = 0 Then
            '            SendType = tSendType.GetFlag
            '            InvokeData("SELECT ACT.ACTNAME, PROCACT.ZGSM_STOP" & _
            '" FROM ACT, SERACT, SERIAL, PROCESS, PROCACT" & _
            '" WHERE SERIAL.SERIAL = SERACT.SERIAL" & _
            '" AND SERACT.ACT = ACT.ACT" & _
            '" AND SERIAL.PRODSERIAL = PROCESS.T$PROC" & _
            '" AND PROCESS.T$PROC = PROCACT.T$PROC" & _
            '" AND ACT.ACT = PROCACT.ACT" & _
            '" AND SERIALNAME =  '%SERIALNAME%'" & _
            '" AND ACTDES = '%ACTDES%'")
            '            Select Case GetPass
            '                Case False
            GetVars()
            getparts()

            InvokeData( _
                "INSERT INTO ZSFDC_LOAD_STARTTIME" & _
                "(LINE, SERIALNAME, ACTNAME, USERID, EMPSTIME,RECORDTYPE) " & _
                "select (SELECT MAX(LINE) + 1 FROM ZSFDC_LOAD_STARTTIME), " & _
                "'%SERIALNAME%', " & _
                "'" & route & "', " & _
                userid & ", " & _
                SystemTime & _
                 ",'1'")
            Me.FormClose()
            Return True
            '                Case True
            '                    Try
            '                        Dim msg As New MailMessage
            '                        Dim client As New SmtpClient

            '                        Dim smtp As String = "MRMCHENRY.gsmgroup.gsm" '"mobile-b.gsmautomotive.net"
            '                        Dim from As String = "DoNotReply@gsmautomotive.co.uk"
            '                        Dim sto As String = "quality@gsmautomotive.co.uk"

            '                        Dim FroAddy As New MailAddress(from)

            '                        msg.From = FroAddy
            '                        msg.To.Add(sto)
            '                        msg.Subject = "Attention Needed"

            '                        Dim bd As String = "Production user - " & UserName & " has hit a stop sequence on W/O - " & CtrlForm.el(0).Data & ", operation - " & CtrlForm.el(3).Data & ". Producing " & CtrlForm.el(1).Data & " for " & custname & ""
            '                        msg.Body = bd

            '                        client.Host = smtp
            '                        client.Port = 25

            '                        client.Credentials = New SmtpCredential("", "", "GSM") ' New SmtpCredential("emerge.priority", "1amBatman", "")


            '                        client.Send(msg)

            '                    Catch ex As Exception
            '                        Dim snd As New Sound("Chord.wav")
            '                        snd.Play()

            '                        OverControl.msgboxa("Error sending email, please contact quality!")

            '                    End Try
            '                    Dim f As New frmPassCode
            '                    f.ShowDialog()
            '                    If f.DialogResult = Windows.Forms.DialogResult.OK Then
            '                        Try
            '                            GetVars()
            '                            getparts()

            '                            InvokeData( _
            '                                "INSERT INTO ZSFDC_LOAD_STARTTIME" & _
            '                                "(LINE, SERIALNAME, ACTNAME, USERID, EMPSTIME,RECORDTYPE) " & _
            '                                "select (SELECT MAX(LINE) + 1 FROM ZSFDC_LOAD_STARTTIME), " & _
            '                                "'%SERIALNAME%', " & _
            '                                "'" & route & "', " & _
            '                                userid & ", " & _
            '                                SystemTime & _
            '                                 ",'1'")
            '                            Me.FormClose()
            '                            Return False
            '                        Catch ex As Exception
            '                            OverControl.msgboxa(ex.Message)
            '                        End Try
            '                    Else
            '                        Return False
            '                    End If
            '            End Select

            '            'If OverControl.msgboxa("This will record the job start time", OverControl.msgboxaStyle.OkCancel) = OverControl.msgboxaResult.Ok Then

            '            'Else
            '            'Return False
            '            'End If

            '        End If
        Else
           
        End If
        Return True
    End Function

#End Region

End Class
