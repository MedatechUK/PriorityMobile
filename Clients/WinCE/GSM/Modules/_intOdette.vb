Imports System.Xml
Imports System.Text.RegularExpressions
Public Class interfaceOdette
    Inherits SFDCData.iForm
    Private PRT As String
    Private unq As Integer
    Private cnt As Integer
    Private UNQList As New List(Of Integer)

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
            .ValidExp = ValidStr(tRegExValidation.tString)
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
                        CtrlForm.el(0).Enabled = False
                        CtrlTable.Focus()
                        CtrlTable.BeginLoadRS()
                End Select
        End Select

    End Sub

    Public Overrides Sub ProcessForm()
        Try
            With p
                .DebugFlag = True
                .Procedure = "ZSFDC_ODETTE_CHECK"
                .Table = "ZSFDC_ODETTE_CHECK2"
                .RecordType1 = "DOCNO"
                .RecordType2 = "ODETTE,CHECKED,USERLOGIN"
                .RecordTypes = "TEXT,TEXT,TEXT,TEXT"
            End With

            Dim t1() As String = {CtrlForm.el(0).Data.ToString}
            p.AddRecord(1) = t1

            Dim t2() As String = {CtrlForm.el(0).Data.ToString, "Y", UserName}
            p.AddRecord(2) = t2

        Catch e As Exception
            OverControl.msgboxa(e.Message)
        End Try

       

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
            OverControl.msgboxa(e.Message)
        End Try
    End Sub

    Public Overrides Sub TableScan(ByVal Value As String)
        If System.Text.RegularExpressions.Regex.IsMatch(Value, ValidStr(tRegExValidation.tOd)) Then
            With CtrlForm
                .el(0).Data = Value
                .el(0).DataEntry.Text = Value
                .el(0).ProcessEntry()
            End With
        ElseIf Regex.IsMatch(Value, "^<") Then
            Dim doc As New Xml.XmlDocument
            doc.LoadXml(Value)
            For Each nd As XmlNode In doc.SelectNodes("in/i")
                Dim DataType As String = nd.Attributes("n").Value

            Select DataType
                    Case "CLOSE"
                        Me.CloseMe()
                    Case "PART"
                        PRT = nd.Attributes("v").Value

                    Case "QTY"
                        cnt = nd.Attributes("v").Value

                    Case "UNQ"
                        unq = nd.Attributes("v").Value



                End Select
            Next
            Dim bcofo As Boolean = False
            Dim done As Boolean = False
            Dim lv As ListViewItem
            For Each lv In CtrlTable.Table.Items
                Dim prtcheck As String = lv.SubItems.Item(4).Text
                If prtcheck = PRT Then
                    'found the barcode check to see that the label hasnt been scanned
                    Dim fnd As Boolean = False
                    bcofo = True
                    For Each i As Integer In UNQList
                        If i = unq Then
                            'Dim f As New frmMsgBox
                            'f.Label1.Text = "You have already scanned this label"
                            'f.ShowDialog()
                            'If f.ShowDialog = DialogResult.OK Then
                            '    f.Dispose()
                            'End If
                            Beep()

                            PRT = ""
                            fnd = True
                        End If
                    Next
                    If fnd = False Then
                        Dim fintot As Integer = Convert.ToInt32(lv.SubItems.Item(2).Text)
                        Dim hold As Integer = Convert.ToInt32(lv.SubItems.Item(3).Text)
                        hold += cnt
                        If hold = fintot Then
                            done = True
                        End If
                        UNQList.Add(unq)
                        lv.SubItems.Item(3).Text = hold

                    Else
                        fnd = False
                    End If
                End If
            Next
            If bcofo = False Then
                Dim f As New frmMsgBox
                f.Label1.Text = "Barcode not found in this Odette"
                f.ShowDialog()
                If f.ShowDialog = DialogResult.OK Then
                    f.Dispose()
                End If
            End If
            If done = True Then
               
                CtrlTable.DisableButtons(True, True, True, True, False)
                CtrlTable.EnableToolbar(False, False, False, False, True)

        End If
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

        With col
            .Name = "_CONFAMT"
            .Title = "Checked Qty"
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


        With col
            .Name = "_BCODE"
            .Title = "Part Bcode"
            .initWidth = 0
            .TextAlign = HorizontalAlignment.Left
            .AltEntry = ctrlText.tAltCtrlStyle.ctNone 'ctKeyb 
            .ValidExp = ValidStr(tRegExValidation.tNumeric)
            .SQLValidation = "SELECT %ME% "
            .DefaultFromCtrl = Nothing
            .ctrlEnabled = True
            .Mandatory = False
        End With
        CtrlTable.AddCol(col)


        'With col
        '    .Name = "_PBCODE"
        '    .Title = "Part Bcode"
        '    .ValidExp = "^.+$"
        '    .SQLValidation = "SELECT BARCODE, PARTNAME " & _
        '                    "FROM dbo.PARTALIAS() " & _
        '                    "WHERE UPPER(BARCODE) = upper('%ME%')"
        '    '******** Barcoded field '*******
        '    .AltEntry = ctrlText.tAltCtrlStyle.ctList 'ctNone 'ctKeyb 
        '    .ctrlEnabled = True
        'End With
        'CtrlForm.AddField(field)

        CtrlTable.RecordsSQL = "SELECT PART.PARTNAME,PART.PARTDES,TRANSORDER.TQUANT/1000 AS TQUANT,0 as CNT,PART.BARCODE FROM PART,TRANSORDER WHERE TRANSORDER.PART = PART.PART AND TRANSORDER.ZGSM_ODETTE = '%ODETTE%' AND TRANSORDER.TQUANT > 0"


    End Sub

    Public Overrides Function VerifyForm() As Boolean
        Return True
    End Function

    Public Sub New(Optional ByRef App As Form = Nothing)
        CallerApp = App
        CtrlTable.DisableButtons(True, True, True, True, True)
        CtrlTable.EnableToolbar(False, False, False, False, False)
        NewArgument("BCOD", "")
    End Sub
End Class
