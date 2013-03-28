Public Class interfaceCheckRoute
    Inherits SFDCData.iForm

#Region "Variables etc"
    Private current_part As String = ""

#End Region
#Region "Column Declerations"
    Public Overrides Sub FormSettings()
        With field 'using the tfield structure from the ctrlForm
            .Name = "ROUTE"
            .Title = "Route"
            .ValidExp = "^[0-9A-Za-z]+$"
            .SQLValidation = "SELECT FORROUTE FROM V_UncheckedRoutes where FORROUTE = '%ME%'"
            .SQLList = "SELECT FORROUTE FROM V_UncheckedRoutes"
            .AltEntry = ctrlText.tAltCtrlStyle.ctList 'ctKeyb 
            .ctrlEnabled = True
            .MandatoryOnPost = True
        End With
        CtrlForm.AddField(field)

        With field 'using the tfield structure from the ctrlForm
            .Name = "FORDATE"
            .Title = "Date"
            .ValidExp = "^[0-9A-Za-z]+$"
            .SQLValidation = "select dbo.MINTODATE(PICKEDDATE) from ZROD_PICKS where FORROUTE = '%ROUTE%'"
            .Data = ""
            .AltEntry = ctrlText.tAltCtrlStyle.ctNone 'ctKeyb 
            .ctrlEnabled = False
            .MandatoryOnPost = False
        End With
        CtrlForm.AddField(field)

        With field 'using the tfield structure from the ctrlForm
            .Name = "PICKID"
            .Title = "Pick ID"
            .ValidExp = ValidStr(tRegExValidation.tNumeric)
            .SQLValidation = _
                "SELECT     PICK " & _
                "FROM         ZROD_PICKS " & _
                "WHERE     (FORROUTE = '%ROUTE%')"
            .Data = ""
            .AltEntry = ctrlText.tAltCtrlStyle.ctNone 'ctKeyb 
            .ctrlEnabled = False
            .MandatoryOnPost = False
        End With
        CtrlForm.AddField(field)

        With field 'using the tfield structure from the ctrlForm
            .Name = "PACKING_SLIP"
            .Title = "Pack Slip"
            .ValidExp = ValidStr(tRegExValidation.tPackingSlip)
            .SQLValidation = "select PSNO from V_PICK_MONITOR where PSNO ='%ME%' and ROUTENAME = '%ROUTE%'"
            .Data = ""
            .AltEntry = ctrlText.tAltCtrlStyle.ctNone 'ctKeyb 
            .ctrlEnabled = False
            .MandatoryOnPost = False
        End With
        CtrlForm.AddField(field)


        With field 'using the tfield structure from the ctrlForm
            .Name = "PART"
            .Title = "Part"
            .ValidExp = ValidStr(tRegExValidation.tBarcode)
            .SQLValidation = _
                "SELECT     PARTNAME " & _
                "FROM         dbo.V_PICK_MONITOR " & _
                "WHERE     (PARTNAME = '%ME%')"
            .Data = ""
            .AltEntry = ctrlText.tAltCtrlStyle.ctNone 'ctKeyb 
            .ctrlEnabled = False
            .MandatoryOnPost = False
        End With
        CtrlForm.AddField(field)

        With field 'using the tfield structure from the ctrlForm
            .Name = "AMOUNTREQ"
            .Title = "Amount"
            .ValidExp = ValidStr(tRegExValidation.tNumeric)
            .SQLValidation = "SELECT '%ME%'"
            .Data = ""
            .AltEntry = ctrlText.tAltCtrlStyle.ctNone
            'ctKeyb 
            .ctrlEnabled = False
            .MandatoryOnPost = False
        End With
        CtrlForm.AddField(field)
    End Sub
    Public Overrides Sub TableSettings()

        With col
            .Name = "_PART"
            .Title = "PART No"
            .initWidth = 40
            .TextAlign = HorizontalAlignment.Left
            .AltEntry = ctrlText.tAltCtrlStyle.ctNone 'ctKeyb 
            .ValidExp = ValidStr(tRegExValidation.tBarcode)
            .SQLList = ""
            .SQLValidation = "SELECT '%ME%'"
            .DefaultFromCtrl = Nothing
            .ctrlEnabled = False
            .Mandatory = False
            .MandatoryOnPost = False
        End With
        CtrlTable.AddCol(col)

        With col
            .Name = "_PARTDES"
            .Title = "Part Name"
            .initWidth = 70
            .TextAlign = HorizontalAlignment.Center
            .AltEntry = ctrlText.tAltCtrlStyle.ctNone 'ctKeyb 
            .ValidExp = ValidStr(tRegExValidation.tString)
            .SQLList = ""
            .SQLValidation = ""
            .DefaultFromCtrl = Nothing
            .ctrlEnabled = False
            .Mandatory = False
            .MandatoryOnPost = False
        End With
        CtrlTable.AddCol(col)

        With col
            .Name = "_QUANT"
            .Title = "Quantity"
            .initWidth = 0
            .TextAlign = HorizontalAlignment.Left
            .AltEntry = ctrlText.tAltCtrlStyle.ctNone 'ctKeyb 
            .ValidExp = ValidStr(tRegExValidation.tNumeric)
            .SQLList = ""
            .SQLValidation = "SELECT '%ME%'"
            .DefaultFromCtrl = Nothing
            .ctrlEnabled = False
            .Mandatory = True
            .MandatoryOnPost = False
        End With
        CtrlTable.AddCol(col)

        With col
            .Name = "_CHECKED"
            .Title = "Picked"
            .initWidth = 25
            .TextAlign = HorizontalAlignment.Center
            .AltEntry = ctrlText.tAltCtrlStyle.ctNone 'ctKeyb 
            .ValidExp = ValidStr(tRegExValidation.tString)
            .SQLList = ""
            .SQLValidation = ""
            .DefaultFromCtrl = Nothing
            .ctrlEnabled = False
            .Mandatory = False
            .MandatoryOnPost = False
        End With
        CtrlTable.AddCol(col)

        With col
            .Name = "_COUNT"
            .Title = "Picked"
            .initWidth = 0
            .TextAlign = HorizontalAlignment.Center
            .AltEntry = ctrlText.tAltCtrlStyle.ctNone 'ctKeyb 
            .ValidExp = ValidStr(tRegExValidation.tString)
            .SQLList = ""
            .SQLValidation = ""
            .DefaultFromCtrl = Nothing
            .ctrlEnabled = False
            .Mandatory = False
            .MandatoryOnPost = False
        End With
        CtrlTable.AddCol(col)

    End Sub
#End Region
#Region "invocations"
    Public Overrides Sub EndInvokeData(ByVal Data(,) As String)
        Select Case SendType
            Case tSendType.Route
                With CtrlForm
                    With .el(.ColNo("PICKID"))
                        .DataEntry.Text = Data(0, 0)
                        .ProcessEntry()
                    End With
                End With
            Case tSendType.PickID
                Try
                    With CtrlForm
                        With .el(.ColNo("FORDATE"))
                            Dim pdate As Date
                            pdate = FormatDateTime("1/1/1988", DateFormat.ShortDate)
                            Dim am As Integer = 0
                            am = Convert.ToInt32(Data(0, 0))
                            If am <> 0 Then
                                .DataEntry.Text = DateAdd(DateInterval.Minute, am, pdate)
                            End If
                            '.DataEntry.Text = Data(0, 0)
                        End With
                        With .el(.ColNo("PACKING_SLIP"))
                            .DataEntry.Text = Data(1, 0)
                        End With

                    End With
                Catch ex As Exception
                    MsgBox(ex.ToString)
                End Try

                'Set the query to load recordtype 2s
                CtrlTable.RecordsSQL = _
               "select ZROD_PICKLINE.PARTNAME,PART.PARTDES,SUM(dbo.REALQUANT(ZROD_PICKLINE.AMOUNTPICKED)) as PICKED,'o' as CHECK_SUM, 'o' as CHECK_COUNT " & _
               "from ZROD_PICKLINE,PART " & _
               "WHERE ZROD_PICKLINE.PICK = '%PICKID%' AND PART.PARTNAME = ZROD_PICKLINE.PARTNAME " & _
               "GROUP BY ZROD_PICKLINE.PARTNAME,PART.PARTDES"

                With CtrlTable
                    .Table.Items.Clear()
                    .BeginLoadRS()
                    .Table.Focus()
                End With

            Case tSendType.Part
                Dim it As ListViewItem
                For Each it In CtrlTable.Table.Items
                    it.Selected = False
                Next
                Dim m As Integer
                m = 1

                Dim h As Integer
                h = CtrlTable.Table.Items.Count
                If h >= 0 Then 'check to see if there are any rows to select
                    For Each it In CtrlTable.Table.Items
                        If it.SubItems(0).Text = Data(0, 0) Then
                            'find the correct part in the table
                            it.Selected = True
                            'select the line so the user has visual confirmation
                            If it.SubItems(4).Text <> "o" Then
                                'fires if we have hit this item previously we then need to get the counted amount
                                'the subitems(4) holds the count of the amount of times this item has been checked
                                'and the subitems(3) holds the last count
                                Dim add As Integer
                                Dim num As New frmNumber
                                With num
                                    .Text = "Box quantity."
                                    .ShowDialog()
                                    add = .number
                                    .Dispose()
                                End With
                                'next we check the counted quantity
                                Dim expected, lastcount As Integer
                                expected = Convert.ToInt32(it.SubItems(2).Text)
                                lastcount = Convert.ToInt32(it.SubItems(3).Text)
                                If lastcount = add Then
                                    'we have had 2 counts the same so ......
                                    'we need to compare the amount we have counted (twice) against what is expected
                                    Dim amount As Integer
                                    amount = add - expected
                                    'amount holds the difference between what we counted and what we expected
                                    If amount = 0 Then
                                        With CtrlTable
                                            .Table.Items.Remove(it)
                                        End With
                                    ElseIf amount > 0 Then
                                        'we have too many items intruct user to return surplus
                                        MsgBox("You appear to have picked too many items for this part, please return " & amount & " back to stock")
                                    ElseIf amount < 0 Then
                                        MsgBox("You appear to have picked too few items for this part, please return " & -amount & " from stock")
                                    End If
                                Else

                                    MsgBox("This count does not match the expected amount, please RECOUNT and try again")
                                    it.SubItems(3).Text = add
                                    Dim counts As Integer
                                    counts = Convert.ToInt32(it.SubItems(4).Text)
                                    counts += 1
                                    it.SubItems(4).Text = counts

                                End If

                            Else
                                
                                'we havent had a check on this item yet as the check is still set to o
                                'so we will need to get the amount counted by calling the number pad
                                Dim add As Integer
                                Dim num As New frmNumber
                                With num
                                    .Text = "Box quantity."
                                    .ShowDialog()
                                    add = .number
                                    .Dispose()
                                End With
                                'now we check the number against the expected amount
                                Dim expected As Integer
                                expected = Convert.ToInt32(it.SubItems(2).Text)
                                If add = expected Then

                                    'the check is fine, this line can be hidden
                                    'TODO add hiding ability or add a boolean field to check against so that any attempts to recheck a done line error out
                                    CtrlTable.Table.Items.Remove(it)
                                ElseIf add < expected Then
                                    MsgBox("This count does not match the expected amount, please RECOUNT and try again")
                                    it.SubItems(3).Text = add
                                    it.SubItems(4).Text = 1
                                ElseIf add > expected Then
                                    MsgBox("This count does not match the expected amount, please RECOUNT and try again")
                                    it.SubItems(3).Text = add
                                    it.SubItems(4).Text = 1
                                End If
                                current_part = it.SubItems(0).Text


                            End If

                        End If
                    Next
                End If
        End Select
    End Sub
    Dim SendType As tSendType = tSendType.Route

    Public Enum tSendType
        Route = 0
        PackSlip = 1
        Part = 2
        Warhs = 3
        Bin = 4
        Amount = 5
        AmountCheck = 6
        TableScan = 7
        PickID = 8
    End Enum
#End Region
#Region "Form Processing"
    Public Overrides Sub AfterAdd(ByVal TableIndex As Integer)

    End Sub

    Public Overrides Sub AfterEdit(ByVal TableIndex As Integer)

    End Sub

    Public Overrides Sub BeginAdd()

    End Sub

    Public Overrides Sub BeginEdit()

    End Sub

    Public Overrides Sub ProcessEntry(ByVal ctrl As SFDCData.ctrlText, ByVal Valid As Boolean)
        Select Case Valid
            Case False
                Beep()

            Case True
                Try
                    Dim i As String
                    i = ctrl.Data.ToString

                    If ctrl.Data <> "" Then
                        Select Case ctrl.Name
                            Case "ROUTE"
                                SendType = tSendType.Route
                                InvokeData("SELECT PICK FROM ZROD_PICKS WHERE FORROUTE = '%ROUTE%' AND ISCHECKED ='N'")
                                With CtrlForm
                                    With .el(.ColNo("FORDATE"))

                                    End With

                                End With

                            Case "PICKID"
                                SendType = tSendType.PickID
                                InvokeData("SELECT FORDATE,PACKSLIP FROM ZROD_PICKS WHERE PICK = '%PICKID%'")

                            Case "PART"
                                SendType = tSendType.Part

                                InvokeData("select '%PART%' from dbo.V_PICKLIST_PARTS")
                        End Select
                    End If
                Catch ex As Exception

                End Try
        End Select

    End Sub

    Public Overrides Sub ProcessForm()

    End Sub
#End Region
    Public Sub New(Optional ByRef App As Form = Nothing)

        'InitializeComponent()
        CallerApp = App
        NewArgument("PickDate", " ")

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
                    .Items(.Items.Count - 1).SubItems.Add(Data(4, y))
                End With
            Next
        Catch e As Exception
            MsgBox(e.Message)
        End Try
    End Sub


    Public Overrides Sub TableScan(ByVal Value As String)
        Try
            If System.Text.RegularExpressions.Regex.IsMatch(Value, ValidStr(tRegExValidation.tBarcode)) Or System.Text.RegularExpressions.Regex.IsMatch(Value, ValidStr(tRegExValidation.tBarcode2)) Then
                ' Scanning a barcode
                With CtrlForm
                    With .el(.ColNo("Part"))
                        .DataEntry.Text = Value
                        .ProcessEntry()
                    End With
                End With



                'SendType = tSendType.TableScan
                'InvokeData("SELECT PARTNAME, BARCODE FROM PART WHERE BARCODE = '" & Value & "'")


            Else
                Throw New Exception("Scanned item is not a part.")
            End If

        Catch EX As Exception
            MsgBox(String.Format("{0}", EX.Message))
        End Try


    End Sub




    Public Overrides Function VerifyForm() As Boolean

    End Function
End Class
