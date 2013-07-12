Public Class interfaceOdette
    Inherits SFDCData.iForm

    Public Overrides Sub AfterAdd(ByVal TableIndex As Integer)

    End Sub

    Public Overrides Sub AfterEdit(ByVal TableIndex As Integer)

    End Sub

    Public Overrides Sub BeginAdd()

    End Sub

    Public Overrides Sub BeginEdit()

    End Sub

    Public Overrides Sub EndInvokeData(ByVal Data(,) As String)

    End Sub

    Public Overrides Sub FormSettings()
        With field
            .Name = "ODETTE"
            .Title = "Odette No"
            .ValidExp = ValidStr(tRegExValidation.tOd)
            .SQLValidation = "SELECT '%ME%'"
            .SQLList = ""
            .Data = ""      '******** Barcoded field '*******
            .AltEntry = ctrlText.tAltCtrlStyle.ctNone 'ctNone 'ctKeyb 
            .ctrlEnabled = True
        End With
        CtrlForm.AddField(field)
    End Sub

    Public Overrides Sub ProcessEntry(ByVal ctrl As SFDCData.ctrlText, ByVal Valid As Boolean)
        Select Case Valid
            Case False
                Beep()

            Case True
                Select Case ctrl.Name
                    Case "ODETTE"
                        CtrlTable.BeginLoadRS()
                End Select
        End Select

    End Sub

    Public Overrides Sub ProcessForm()

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

    Public Overrides Sub TableScan(ByVal Value As String)
        If System.Text.RegularExpressions.Regex.IsMatch(Value, ValidStr(tRegExValidation.tOd)) Then
            With CtrlForm
                .el(0).Data = Value
                .el(0).DataEntry.Text = Value
                .el(0).ProcessEntry()
            End With


        End If
    End Sub

    Public Overrides Sub TableSettings()
        With col
            .Name = "_PARTNAME"
            .Title = "Part No"
            .initWidth = 30
            .TextAlign = HorizontalAlignment.Left
            .AltEntry = ctrlText.tAltCtrlStyle.ctNone 'ctKeyb 
            .ValidExp = ValidStr(tRegExValidation.tBarcode)
            ' Second field replaces first field if first field validates ok
            .SQLValidation = "SELECT = '%ME%'"
            .DefaultFromCtrl = Nothing '
            .ctrlEnabled = True
            .Mandatory = True
        End With
        CtrlTable.AddCol(col)

        With col
            .Name = "_PARTdes"
            .Title = "Part Des"
            .initWidth = 30
            .TextAlign = HorizontalAlignment.Left
            .AltEntry = ctrlText.tAltCtrlStyle.ctNone 'ctKeyb 
            .ValidExp = ValidStr(tRegExValidation.tString)
            ' Second field replaces first field if first field validates ok
            .SQLValidation = "SELECT = '%ME%'"
            .DefaultFromCtrl = Nothing '
            .ctrlEnabled = True
            .Mandatory = True
        End With
        CtrlTable.AddCol(col)

        With col
            .Name = "_TQUANT"
            .Title = "Qty"
            .initWidth = 30
            .TextAlign = HorizontalAlignment.Left
            .AltEntry = ctrlText.tAltCtrlStyle.ctNone 'ctKeyb 
            .ValidExp = ValidStr(tRegExValidation.tNumeric)
            .SQLValidation = "SELECT %ME% "
            .DefaultFromCtrl = Nothing
            .ctrlEnabled = True
            .Mandatory = True
        End With
        CtrlTable.AddCol(col)

        CtrlTable.RecordsSQL = "SELECT PART.PARTNAME,PART.PARTDES,TRANSORDER.TQUANT/1000 AS TQUANT FROM PART,TRANSORDER WHERE TRANSORDER.PART = PART.PART AND TRANSORDER.ZGSM_ODETTE = '%ODETTE%' AND TRANSORDER.TQUANT > 0"


    End Sub

    Public Overrides Function VerifyForm() As Boolean

    End Function

    Public Sub New(Optional ByRef App As Form = Nothing)
        CallerApp = App
        CtrlTable.DisableButtons(True, True, True, True, True)
        CtrlTable.EnableToolbar(False, False, False, False, False)
    End Sub
End Class
