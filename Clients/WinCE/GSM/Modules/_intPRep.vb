Imports System.Data
Public Class interfacePRep
    Inherits SFDCData.iForm
    Public Enum tSendType
        Router = 0
        WO = 1
        UserScan = 2
        Reject = 3
        Barcode = 4
        Part = 5
    End Enum
    Private user As String = ""
    Private operation As String = ""
    Private WONO As String = ""
    Public UserList As New List(Of Users)
    Public RejectList As New List(Of Rejects)
    Public PartsList As New List(Of Parts)
    Dim SendType As tSendType = tSendType.Router
    Private Rej As Integer = 0
    Private comp As Integer = 0
    Public Overrides Sub AfterAdd(ByVal TableIndex As Integer)

    End Sub

    Public Overrides Sub AfterEdit(ByVal TableIndex As Integer)

    End Sub

    Public Overrides Sub BeginAdd()

    End Sub

    Public Overrides Sub BeginEdit()
        CtrlTable.EditInPlace = False
        CtrlTable.CancelEdit = True
        Dim f As New frmEdit

        Dim lvi As ListViewItem
        For Each lvi In CtrlTable.Table.Items
            If lvi.Selected = True Then
                'this item needs the new data written to it 
                'bearing in mind we are using the following table structure
                '0 - WONO
                '1 - OPERATION
                '2 - COMPLETED
                '3 - REJECTED
                '4 - USER
                '5 - START
                '6 - END
                f.Label5.Text = "OPERATION - " & lvi.SubItems(1).Text
                f.TextBox4.Text = lvi.SubItems(2).Text
                f.TextBox3.Text = lvi.SubItems(3).Text
                f.TextBox1.Text = lvi.SubItems(5).Text
                f.TextBox2.Text = lvi.SubItems(6).Text


                'Tab 2 - Parts we need to create a data table to hold the parts and then add columns to it. After that we will fill it with the parts from the list (we will delete the current parts and then rewrite them to take any edits made)


                'create the columns for the table
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

                'Col 3 is the Part Quantity
                p3.DataType = System.Type.GetType("System.Int32")
                p3.ColumnName = "Quantity"
                p3.Caption = "Quantity"
                p3.AutoIncrement = False
                f.pars.Columns.Add(p3)

                'Col 2 is the LOT
                p4.DataType = System.Type.GetType("System.String")
                p4.ColumnName = "Lot"
                p4.Caption = "Lot"
                p4.AutoIncrement = False
                f.pars.Columns.Add(p4)

                'Col 2 is the Current Op
                p5.DataType = System.Type.GetType("System.String")
                p5.ColumnName = "Operation"
                p5.Caption = "Operation"
                p5.AutoIncrement = False
                f.pars.Columns.Add(p5)

                p6.DataType = System.Type.GetType("System.String")
                p6.ColumnName = "Checked"
                p6.Caption = "Checked"
                p6.AutoIncrement = False
                f.pars.Columns.Add(p6)

                p7.DataType = System.Type.GetType("System.String")
                p7.ColumnName = "Barcode"
                p7.Caption = "Barcode"
                p7.AutoIncrement = False
                f.pars.Columns.Add(p7)


                'Now we iterate through the list and take all the parts for the currently chosen operation

                If PartsList.Count <> 0 Then
                    For Each p As Parts In PartsList
                        If p.op = lvi.SubItems(1).Text Then
                            Dim pr As DataRow
                            pr = f.pars.NewRow
                            pr("ID") = p.pID
                            pr("Description") = p.Desc
                            pr("Quantity") = p.qua
                            pr("Lot") = p.lt
                            pr("Operation") = p.op
                            pr("Checked") = p.pChecked
                            pr("Barcode") = p.pBarcode
                            f.pars.Rows.Add(pr)
                        End If
                    Next
                End If

                'then we fill the datafrid with the parts
                f.DataGrid1.DataSource = f.pars
                '*****************************************************************************************
                '*****************************************************************************************
                'Tab 3 - Rejects

                'create the columns for the table
                Dim c1 As New DataColumn
                Dim c2 As New DataColumn

                'Col 1 is the quantity
                c1.DataType = System.Type.GetType("System.Int32")
                c1.ColumnName = "Quant"
                c1.Caption = "Quant"
                c1.AutoIncrement = False
                f.Rej.Columns.Add(c1)

                'Col 2 is the reason
                c2.DataType = System.Type.GetType("System.String")
                c2.ColumnName = "Reason"
                c2.Caption = "Reason"
                c2.AutoIncrement = False
                f.Rej.Columns.Add(c2)


                If RejectList.Count <> 0 Then
                    For Each r As Rejects In RejectList
                        If r.op = lvi.SubItems(1).Text Then
                            Dim rr As DataRow
                            rr = f.Rej.NewRow
                            rr("Quant") = r.quan
                            rr("Reason") = r.reas
                            f.Rej.Rows.Add(rr)
                        End If
                    Next
                End If
                f.DataGrid2.DataSource = f.Rej
                '*****************************************************************************************
                '*****************************************************************************************
                'Tab 4 - Users

                'create the columns for the table
                Dim u1 As New DataColumn
                Dim u2 As New DataColumn
                Dim u3 As New DataColumn
                Dim u4 As New DataColumn

                'Col 1 is the quantity
                u1.DataType = System.Type.GetType("System.String")
                u1.ColumnName = "Name"
                u1.Caption = "Name"
                u1.AutoIncrement = False
                f.usr.Columns.Add(u1)

                'Col 2 is the reason
                u2.DataType = System.Type.GetType("System.String")
                u2.ColumnName = "Started"
                u2.Caption = "Started"
                u2.AutoIncrement = False
                f.usr.Columns.Add(u2)

                'Col 3 is the reason
                u3.DataType = System.Type.GetType("System.String")
                u3.ColumnName = "Ended"
                u3.Caption = "Ended"
                u3.AutoIncrement = False
                f.usr.Columns.Add(u3)

                'Col 4 is the reason
                u4.DataType = System.Type.GetType("System.String")
                u4.ColumnName = "Operation"
                u4.Caption = "Operation"
                u4.AutoIncrement = False
                f.usr.Columns.Add(u4)


                If UserList.Count <> 0 Then
                    For Each u As Users In UserList
                        If u.op = lvi.SubItems(1).Text Then
                            Dim ur As DataRow
                            ur = f.usr.NewRow
                            ur("Name") = u.nam
                            ur("Started") = u.stime
                            ur("Ended") = u.etime
                            ur("Operation") = u.op
                            f.usr.Rows.Add(ur)
                        End If
                    Next
                End If
                f.DataGrid3.DataSource = f.usr
                f.ShowDialog()
                If f.DialogResult = Windows.Forms.DialogResult.OK Then
                    'the user may have made changes so now we need to write back all the data
                    'first up I will write back the operation line
                    'this item needs the new data written to it 
                    'bearing in mind we are using the following table structure
                    '0 - WONO
                    '1 - OPERATION
                    '2 - COMPLETED
                    '3 - REJECTED
                    '4 - USER
                    '5 - START
                    '6 - END
                    lvi.SubItems(2).Text = f.TextBox4.Text
                    lvi.SubItems(3).Text = f.TextBox3.Text
                    lvi.SubItems(5).Text = f.TextBox1.Text
                    lvi.SubItems(6).Text = f.TextBox2.Text

                    'Next up we need to remove the old data from the list and write back the new (if any). 
                    'To avoid a list positioning error I will start at the bottom and work up
                    RejectList.RemoveAll(Function(x) x.op = lvi.SubItems(1).Text)


                    Dim rr As DataRow
                    rr = f.Rej.NewRow
                    For Each rr In f.Rej.Rows
                        Dim rejhold As New Rejects(rr("Quant"), lvi.SubItems(1).Text, rr("Reason"), " ")
                        RejectList.Add(rejhold)
                    Next
                  

                End If
            End If
        Next

       

    End Sub


    Public Overrides Sub EndInvokeData(ByVal Data(,) As String)
        Select Case SendType
            Case tSendType.UserScan
                Dim f As New frmAddUser
                f.TextBox1.Text = Data(0, 0)
                f.ShowDialog()
                If f.DialogResult = Windows.Forms.DialogResult.OK Then
                    Dim st, et As String
                    st = FormatDateTime(f.DateTimePicker1.Value, DateFormat.ShortTime)
                    et = FormatDateTime(f.DateTimePicker2.Value, DateFormat.ShortTime)
                    Dim u As New Users(f.TextBox1.Text, st, et, operation)
                    UserList.Add(u)
                End If

            Case tSendType.Reject
                Dim res As New List(Of String)


                If IsNothing(Data) = False Then

                    For y As Integer = 0 To UBound(Data, 2)
                        res.Add(Data(0, y))
                    Next
                End If
                Dim f As New frmReject
                f.operation = operation
                f.rejected = Rej
                f.ComboBox1.DataSource = res
                f.Text = "Rejected Parts - " & Rej
                f.ShowDialog()
                If f.DialogResult = Windows.Forms.DialogResult.OK Then
                    Dim i As DataRow
                    For Each i In f.Rejs.Rows
                        Dim rej As New Rejects(i("Quant"), operation, i("Reason"), "")
                        RejectList.Add(rej)
                    Next
                End If
                Dim lvi As ListViewItem
                For Each lvi In CtrlTable.Table.Items
                    If lvi.SubItems(1).Text = operation Then
                        'this item needs the new data written to it 
                        'bearing in mind we are using the following table structure
                        '0 - WONO
                        '1 - OPERATION
                        '2 - COMPLETED
                        '3 - REJECTED
                        '4 - USER
                        '5 - START
                        '6 - END
                        lvi.SubItems(2).Text = comp
                        lvi.SubItems(3).Text = Rej
                        lvi.SubItems(6).Text = FormatDateTime(Now, DateFormat.ShortTime)
                    End If
                Next

            Case tSendType.Barcode
                If Data Is Nothing Then
                    MsgBox("The scanned part is not needed for this work order")
                    Exit Select
                Else
                    Dim part As String
                    part = Data(0, 0)
                    SendType = tSendType.Part
                    Dim bal As String
                    InvokeData("Select * from dbo.V_CheckParts where PART =" & part)

                End If

            Case tSendType.Part
                If Data Is Nothing Then
                    Exit Select
                End If
                'now we open a copy of the form
                Dim f As New frmAddParts

                'fill the list of parts 
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

                'Col 3 is the Part Quantity
                p3.DataType = System.Type.GetType("System.Int32")
                p3.ColumnName = "Quantity"
                p3.Caption = "Quantity"
                p3.AutoIncrement = False
                f.pars.Columns.Add(p3)

                'Col 2 is the LOT
                p4.DataType = System.Type.GetType("System.String")
                p4.ColumnName = "Lot"
                p4.Caption = "Lot"
                p4.AutoIncrement = False
                f.pars.Columns.Add(p4)

                'Col 2 is the Current Op
                p5.DataType = System.Type.GetType("System.String")
                p5.ColumnName = "Operation"
                p5.Caption = "Operation"
                p5.AutoIncrement = False
                f.pars.Columns.Add(p5)

                p6.DataType = System.Type.GetType("System.String")
                p6.ColumnName = "Checked"
                p6.Caption = "Checked"
                p6.AutoIncrement = False
                f.pars.Columns.Add(p6)

                p7.DataType = System.Type.GetType("System.String")
                p7.ColumnName = "Barcode"
                p7.Caption = "Barcode"
                p7.AutoIncrement = False
                f.pars.Columns.Add(p7)


                'Now we iterate through the list and take all the parts for the currently chosen operation

                If PartsList.Count <> 0 Then
                    For Each p As Parts In PartsList

                        Dim pr As DataRow
                        pr = f.pars.NewRow
                        pr("ID") = p.pID
                        pr("Description") = p.Desc
                        pr("Quantity") = p.qua
                        pr("Lot") = p.lt
                        pr("Operation") = p.op
                        pr("Checked") = p.pChecked
                        pr("Barcode") = p.pBarcode
                        f.pars.Rows.Add(pr)

                    Next
                End If

                'then we fill the datafrid with the parts
                f.DataGrid1.DataSource = f.pars
        End Select
    End Sub

    Public Overrides Sub FormSettings()
        'ROUTER - 0
        With field
            .Name = "ROUTER"
            .Title = "Router"
            .ValidExp = ValidStr(tRegExValidation.tWO)
            .SQLValidation = "SELECT '%ME%'"
            .Data = ""      '******** Barcoded field '*******
            .AltEntry = SFDCData.ctrlText.tAltCtrlStyle.ctNone 'ctNone 'ctKeyb 
            .ctrlEnabled = False
            .MandatoryOnPost = False
        End With
        CtrlForm.AddField(field)
        '.SQLValidation = "SELECT SERIALNAME " & _
        '                "FROM SERIAL " & _
        '                "WHERE SERIALNAME = '%ME%' AND CLOSED <> 'C' AND RELEASE='Y'"

        'Works Order - 1
        With field
            .Name = "SERIALNAME"
            .Title = "Work Order"
            .ValidExp = ValidStr(tRegExValidation.tWO)
            .SQLValidation = "SELECT DISTINCT '%ME%' FROM dbo.v_WOACTS"
            .Data = ""      '******** Barcoded field '*******
            .AltEntry = SFDCData.ctrlText.tAltCtrlStyle.ctNone 'ctNone 'ctKeyb 
            .ctrlEnabled = False
            .MandatoryOnPost = False
        End With
        CtrlForm.AddField(field)

        'Date - 2
        With field
            .Name = "CURDATE"
            .Title = "Date"
            .ValidExp = ValidStr(tRegExValidation.tString)
            .SQLValidation = "SELECT '%ME%'"
            .Data = ""
            .AltEntry = SFDCData.ctrlText.tAltCtrlStyle.ctNone 'ctNone 'ctKeyb 
            .ctrlEnabled = False
            .MandatoryOnPost = False
        End With
        CtrlForm.AddField(field)

        'Assisted - 3. This is a flag field and will be hidden
        With field
            .Name = "ASSISTED"
            .Title = "Assisted"
            .ValidExp = ValidStr(tRegExValidation.tString)
            .SQLValidation = "SELECT '%ME%'"
            .Data = "N"
            .AltEntry = SFDCData.ctrlText.tAltCtrlStyle.ctNone 'ctNone 'ctKeyb 
            .ctrlEnabled = False
            .MandatoryOnPost = False
        End With
        CtrlForm.AddField(field)


        'Parts - 4. This is a flag field and will be hidden
        With field
            .Name = "APARTS"
            .Title = "Parts"
            .ValidExp = ValidStr(tRegExValidation.tString)
            .SQLValidation = "SELECT '%ME%'"
            .Data = "N"
            .AltEntry = SFDCData.ctrlText.tAltCtrlStyle.ctNone 'ctNone 'ctKeyb 
            .ctrlEnabled = False
            .MandatoryOnPost = False
        End With
        CtrlForm.AddField(field)


        'Current OP - 5. This is a flag field and will be hidden
        With field
            .Name = "OP"
            .Title = "Operation"
            .ValidExp = ValidStr(tRegExValidation.tOperation)
            .SQLValidation = "SELECT POS where SERIALNAME = '%SERIALNAME%' and POS = '%ME%'"
            .Data = ""
            .AltEntry = SFDCData.ctrlText.tAltCtrlStyle.ctNone 'ctNone 'ctKeyb 
            .ctrlEnabled = False
            .MandatoryOnPost = False
        End With
        CtrlForm.AddField(field)
    End Sub

    Public Overrides Sub ProcessEntry(ByVal ctrl As SFDCData.ctrlText, ByVal Valid As Boolean)
        Select Valid
            Case False
                Beep()
            Case True
                Select Case ctrl.Name
                    Case "OP"

                End Select
        End Select

    End Sub

    Public Overrides Sub ProcessForm()
        Try
            With p
                .DebugFlag = False
                .Procedure = "ZSFDC_LOADWO  "
                .Table = "ZSFDC_LOADWO  "
                .RecordType1 = "SERIALNAME," & _
                                "ASSISTED," & _
                                "PARTS," & _
                                "ROUTER," & _
                                "CDATE," & _
                                "LOT" & _
                                "LINETYPE"
                .RecordType2 = "SERIALNAME" & _
                                "OPERATION," & _
                                "QUANTC," & _
                                "QUANTR," & _
                                "USER," & _
                                "TIMESTARTED," & _
                                "TIMEFINISHED," & _
                                "PART," & _
                                "USER1," & _
                                "REJQUANT," & _
                                "REJREASON," & _
                                "LINETYPE"
                .RecordTypes = "STRING," & _
                                "STRING," & _
                                "STRING," & _
                                "STRING," & _
                                "STRING," & _
                                "STRING," & _
                                "STRING," & _
                                "STRING," & _
                                "," & _
                                "," & _
                                "STRING," & _
                                "STRING," & _
                                "STRING," & _
                                "STRING," & _
                                "STRING," & _
                                "STRING," & _
                                "STRING," & _
                                "STRING"
                Dim assisted As String = "N"
                Dim parts As String = "N"
                If UserList.Count >= 1 Then
                    assisted = "Y"
                End If
                If PartsList.Count >= 1 Then
                    parts = "Y"
                End If
                Dim cdat As Integer = 0
                Dim start As Date = FormatDateTime("1/1/1988", DateFormat.ShortDate)
                cdat = DateDiff(DateInterval.Minute, start, Now)
                Dim t1() As String = { _
                                        WONO, _
                                        assisted, _
                                        parts, _
                                        CtrlTable.el(0).Data, _
                                        cdat, _
                                        WONO, _
                                        1}
                p.AddRecord(1) = t1
                Dim lvi As ListViewItem
                For Each lvi In CtrlTable.Table.Items
                    Dim t2() As String = { _
                        WONO, _
                        lvi.SubItems(1).Text, _
                        lvi.SubItems(2).Text, _
                        lvi.SubItems(3).Text, _
                        lvi.SubItems(4).Text, _
                        lvi.SubItems(5).Text, _
                        lvi.SubItems(6).Text, _
                        "", "", "", "", _
                        2 _
                        }
                    p.AddRecord(2) = t2
                Next
                Dim pa As Parts
                For Each pa In PartsList
                    Dim t3() As String = { _
                                            "", "", 0, 0, "", "", "", _
                                            pa.pID, _
                                            "", "", "", _
                                            3 _
                                            }
                    p.AddRecord(2) = t3
                Next

                Dim us As Users
                For Each us In UserList
                    Dim t4() As String = { _
                                           "", "", 0, 0, "", "", "", _
                                           "", us.nam, _
                                            "", "", _
                                           4 _
                                           }
                    p.AddRecord(2) = t4
                Next
                Dim rej As Rejects
                For Each rej In RejectList
                    Dim t5() As String = { _
                                           "", "", 0, 0, "", "", "", _
                                           "", "", _
                                            rej.quan, rej.code, _
                                           5 _
                                           }
                    p.AddRecord(2) = t5
                Next


















            End With
        Catch ex As Exception

        End Try
    End Sub

    Public Overrides Sub TableRXData(ByVal Data(,) As String)

    End Sub

    Public Overrides Sub TableScan(ByVal Value As String)
        Try
            If System.Text.RegularExpressions.Regex.IsMatch(Value, ValidStr(tRegExValidation.tQR)) Then
                ' Scanning a Router. This is made up of the part number and the wo number (first 8 didgits are the wono, last 7 are the part no)
                'first up though we need to check that they have logged a user onto the device
                If user = "" Then
                    Throw New Exception("Please LOGIN first.")
                    Exit Try
                End If
                Dim qrcode As String() = Value.Split(";")

                Dim wonol As String = qrcode(1)

                Dim part As String = qrcode(2)
                WONO = wonol
                'MsgBox("Wono - " & wono & " Part - " & part)
                With CtrlForm
                    .el(0).Data = part
                    .el(1).Data = wonol
                    .el(1).DataEntry.Text = wonol
                    '.el(1).ProcessEntry()
                End With
                'Now we need to get the user to check off the list of parts
                'we will now load the parts screen with the list of parts from the query
                


            ElseIf System.Text.RegularExpressions.Regex.IsMatch(Value, ValidStr(tRegExValidation.tUser)) Then
                ' Scanning a User
                With CtrlForm
                    Select Case .el(0).Data.Length
                        Case 0
                            'set the user for the job
                            user = Value
                            MsgBox("Welcome " & user)
                        Case Else
                            'next we check to see that we have a current operation if not we beep and ignore
                            If operation = "" Then
                                MsgBox("You have not yet registered an operation, please do so before trying to add a user.")
                                Beep()
                                Exit Try
                            End If
                            'the job has already been started so we are adding a new user so the first step is to read the operation number

                            SendType = tSendType.UserScan
                            InvokeData("select USERNAME FROM dbo.v_USERS where UPPER(USERLOGIN) = '" & Value.ToUpper & "'")

                    End Select
                End With
            ElseIf System.Text.RegularExpressions.Regex.IsMatch(Value, ValidStr(tRegExValidation.tOperation)) Then
                Dim op As String = Value.Remove(0, 1)
                operation = Value.Remove(0, 1)

                If user = "" Then
                    Throw New Exception("Please LOGIN first.")
                    Exit Try
                End If
                Dim i As Integer = 0
                i = CtrlTable.Table.Items.Count
                If i = 0 Then
                    'we need to add our first line of data as there are no operations added yet

                    'bearing in mind we are using the following table structure
                    '0 - WONO
                    '1 - OPERATION
                    '2 - COMPLETED
                    '3 - REJECTED
                    '4 - USER
                    '5 - START
                    '6 - END
                    Dim nt As String = FormatDateTime(Now, DateFormat.ShortTime)
                    Dim won As String = CtrlForm.el(1).Data
                    Dim lvi As New ListViewItem(New String() {won, operation, "-", "-", user, nt, "00:00"})
                    With CtrlTable.Table
                        .Items.Add(lvi)
                    End With
                    SendType = tSendType.Part
                    InvokeData("SELECT PART,PARTNAME,PARTDE S,ACTNAME,BARCODE FROM v_WOACTS WHERE SERIALNAME = '" & WONO & "' AND PART <> 0 and POS = '" & op & "'")
                ElseIf i >= 1 Then
                    'we need to find out if the current operation has a line associated with it
                    Dim fnd As Boolean = False

                    Dim lvi As ListViewItem
                    For Each lvi In CtrlTable.Table.Items
                        If lvi.SubItems(2).Text = operation Then
                            'this item exists already 
                            fnd = True
                            MsgBox("This operation is already underway. To end the current operation either scan a new one or end the job.")
                            Exit Try
                        End If
                    Next
                    If fnd = False Then
                        MsgBox("New Operation detected, closing off old operation.")
                        'first up we need to get the values for the Completed and the Rejected

                        comp = 0
                        Rej = 0
                        Dim num As New frmNumber
                        With num
                            .Text = "Completed Quantity"
                            .ShowDialog()
                            comp = .number

                            .Dispose()
                        End With
                        Dim num2 As New frmNumber
                        With num2
                            .Text = "Rejected Quantity"
                            .ShowDialog()
                            Rej = .number

                            .Dispose()
                        End With
                        If Rej >= 1 Then
                            'there are rejects so we need to gather the information about them
                            SendType = tSendType.Reject
                            InvokeData("SELECT DISTINCT DEFECTDESC FROM DEFECTCODES WHERE INACTIVE <> 'Y'")
                            operation = Value
                            Dim nt As String = FormatDateTime(Now, DateFormat.ShortTime)
                            Dim won As String = CtrlForm.el(1).Data
                            Dim lv As New ListViewItem(New String() {won, operation, "-", "-", user, nt, "00:00"})
                            With CtrlTable.Table
                                .Items.Add(lv)
                            End With
                        End If
                        SendType = tSendType.Part
                        InvokeData("SELECT PART,PARTNAME,PARTDES,ACTNAME,BARCODE FROM v_WOACTS WHERE SERIALNAME = '" & WONO & "' AND PART <> 0 and POS = '" & op & "'")
                    End If
                End If
            ElseIf System.Text.RegularExpressions.Regex.IsMatch(Value, ValidStr(tRegExValidation.tBarcode)) Then
                If user = "" Then
                    Throw New Exception("Please LOGIN first.")
                    Exit Try
                End If

                If operation = "" Then
                    Throw New Exception("Please start an OPERATION first.")
                    Exit Try
                End If
                'right we have a part
                'we need to check 2 things, the part is in stock and that it is right for this wo
                SendType = tSendType.Barcode
                InvokeData("" & _
                            "SELECT PART.PART,PART.BARCODE, SERIAL.SERIAL, SERIALNAME" & _
                            " FROM PART, SERIAL, KITITEMS" & _
                            " WHERE(KITITEMS.PART = PART.PART)" & _
                            " AND KITITEMS.SERIAL = SERIAL.SERIAL" & _
                            " AND PART.BARCODE = '" & Value & "' AND SERIAL.SERIALNAME = '%WONO%'")

            Else
                MsgBox("Barcode not recognised")


            End If

        Catch EX As Exception
            MsgBox(String.Format("{0}", EX.Message))
        End Try
    End Sub

    Public Overrides Sub TableSettings()
        '0
        With col
            .Name = "_WONO"
            .Title = "WO No"
            .initWidth = 25
            .TextAlign = HorizontalAlignment.Left
            .AltEntry = ctrlText.tAltCtrlStyle.ctNone 'ctKeyb 
            .ValidExp = ValidStr(tRegExValidation.tWO)
            .SQLList = ""
            .SQLValidation = "SELECT '%ME%'"
            .DefaultFromCtrl = Nothing
            .ctrlEnabled = False
            .Mandatory = False
            .MandatoryOnPost = False
        End With
        CtrlTable.AddCol(col)

        '1
        With col
            .Name = "_OP"
            .Title = "Operation No"
            .initWidth = 25
            .TextAlign = HorizontalAlignment.Left
            .AltEntry = ctrlText.tAltCtrlStyle.ctNone 'ctKeyb 
            .ValidExp = ValidStr(tRegExValidation.tOperation)
            .SQLList = ""
            .SQLValidation = "SELECT '%ME%'"
            .DefaultFromCtrl = Nothing
            .ctrlEnabled = False
            .Mandatory = False
            .MandatoryOnPost = False
        End With
        CtrlTable.AddCol(col)

        '2
        With col
            .Name = "_QCOMP"
            .Title = "Completed"
            .initWidth = 25
            .TextAlign = HorizontalAlignment.Left
            .AltEntry = ctrlText.tAltCtrlStyle.ctNone 'ctKeyb 
            .ValidExp = ValidStr(tRegExValidation.tString)
            .SQLList = ""
            .SQLValidation = "SELECT '%ME%'"
            .DefaultFromCtrl = Nothing
            .ctrlEnabled = False
            .Mandatory = False
            .MandatoryOnPost = False
        End With
        CtrlTable.AddCol(col)

        '3
        With col
            .Name = "_PQREJ"
            .Title = "Rejected"
            .initWidth = 25
            .TextAlign = HorizontalAlignment.Left
            .AltEntry = ctrlText.tAltCtrlStyle.ctNone 'ctKeyb 
            .ValidExp = ValidStr(tRegExValidation.tString)
            .SQLList = ""
            .SQLValidation = "SELECT '%ME%'"
            .DefaultFromCtrl = Nothing
            .ctrlEnabled = False
            .Mandatory = False
            .MandatoryOnPost = False
        End With
        CtrlTable.AddCol(col)

        '4
        With col
            .Name = "_USER"
            .Title = "User"
            .initWidth = 25
            .TextAlign = HorizontalAlignment.Left
            .AltEntry = ctrlText.tAltCtrlStyle.ctNone 'ctKeyb 
            .ValidExp = ValidStr(tRegExValidation.tUser)
            .SQLList = ""
            .SQLValidation = "SELECT '%ME%'"
            .DefaultFromCtrl = Nothing
            .ctrlEnabled = False
            .Mandatory = False
            .MandatoryOnPost = False
        End With
        CtrlTable.AddCol(col)

        '5
        With col
            .Name = "_START"
            .Title = "Start time"
            .initWidth = 25
            .TextAlign = HorizontalAlignment.Left
            .AltEntry = ctrlText.tAltCtrlStyle.ctNone 'ctKeyb 
            .ValidExp = ValidStr(tRegExValidation.tString)
            .SQLList = ""
            .SQLValidation = "SELECT '%ME%'"
            .DefaultFromCtrl = Nothing
            .ctrlEnabled = False
            .Mandatory = False
            .MandatoryOnPost = False
        End With
        CtrlTable.AddCol(col)

        '6
        With col
            .Name = "_END"
            .Title = "End Time"
            .initWidth = 25
            .TextAlign = HorizontalAlignment.Left
            .AltEntry = ctrlText.tAltCtrlStyle.ctNone 'ctKeyb 
            .ValidExp = ValidStr(tRegExValidation.tString)
            .SQLList = ""
            .SQLValidation = "SELECT '%ME%'"
            .DefaultFromCtrl = Nothing
            .ctrlEnabled = False
            .Mandatory = False
            .MandatoryOnPost = False
        End With
        CtrlTable.AddCol(col)
    End Sub

    Public Overrides Function VerifyForm() As Boolean

    End Function

    Public Sub New(Optional ByRef App As Form = Nothing)

        'InitializeComponent()
        CallerApp = App
        CtrlTable.DisableButtons(True, False, True, True, False)
        CtrlTable.EnableToolbar(False, False, False, False, True)


    End Sub
End Class
