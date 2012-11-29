Public Class InterfaceUNCNT
    Inherits w32SFDCData.iForm

#Region "Initialisation"

    Private tld As Boolean = False

    Public Sub New(Optional ByRef App As Form = Nothing)

        'InitializeComponent()
        CallerApp = App
        CtrlTable.DisableButtons(True, False, True, True, False)
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

    End Sub

    Public Overrides Sub TableSettings()

        With col
            .Name = "_DATE"
            .Title = "Date"
            .initWidth = 50
            .TextAlign = HorizontalAlignment.Left
            .AltEntry = ctrlText.tAltCtrlStyle.ctNone 'ctKeyb 
            .ValidExp = "^.+$"
            ' Second field replaces first field if first field validates ok
            .SQLValidation = "select '%ME%'"
            .DefaultFromCtrl = Nothing '
            .ctrlEnabled = False
            .Mandatory = True
        End With
        CtrlTable.AddCol(col)

        With col
            .Name = "_DOCNO"
            .Title = "Doc"
            .initWidth = 50
            .TextAlign = HorizontalAlignment.Left
            .AltEntry = ctrlText.tAltCtrlStyle.ctNone 'ctKeyb 
            .ValidExp = "^.+$"
            ' Second field replaces first field if first field validates ok
            .SQLValidation = "select DOCNO from DOCUMENTS " & _
                             "where TYPE = 'T' " & _
                             "AND DOCNO = '%ME%'"
            .DefaultFromCtrl = Nothing '
            .ctrlEnabled = False
            .Mandatory = True
        End With
        CtrlTable.AddCol(col)

        ' FROM LOCNAME
        With col
            .Name = "_LOCNAME"
            .Title = "From Bin"
            .initWidth = 30
            .TextAlign = HorizontalAlignment.Left
            .AltEntry = ctrlText.tAltCtrlStyle.ctList ' ctNone 'ctKeyb 
            .ValidExp = ValidStr(tRegExValidation.tLocname)
            .SQLList = "SELECT DISTINCT LOCNAME FROM WAREHOUSES WHERE WARHSNAME = '%WARHS%' AND INACTIVE <> 'Y'"
            .SQLValidation = "SELECT LOCNAME FROM WAREHOUSES WHERE LOCNAME = '%ME%' AND WARHSNAME = '%WARHS%'"
            .DefaultFromCtrl = Nothing 'CtrlForm.el(1)      '******* Barcoded Field - default from Type1 LOCNAME '*******
            .ctrlEnabled = False
            .Mandatory = True
        End With
        CtrlTable.AddCol(col)

        CtrlTable.EditInPlace = False

        ' Set the query to load recordtype 2s
        CtrlTable.RecordsSQL = "select CURDATE, DOCNO, LOCNAME FROM v_RequestedCNT " & _
                                        "where WARHSNAME = '%WARHS%'"

    End Sub

    Public Overrides Sub TableRXData(ByVal Data(,) As String)

        Dim y As Integer
        Dim x As Integer
        Dim i As Integer

        If IsNothing(Data) Then Exit Sub

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

    Public Overrides Sub EndInvokeData(ByVal Data(,) As String)

    End Sub

    Public Overrides Sub ProcessEntry(ByVal ctrl As w32SFDCData.ctrlText, ByVal Valid As Boolean)
        Select Case Valid
            Case False
                Beep()

            Case True
                Try
                    ' Populate the list on sucsessful validation of Warehouse
                    If ctrl.Name = "WARHS" Then

                        CtrlTable.Table.Items.Clear()
                        CtrlTable.BeginLoadRS()

                    End If

                Catch
                End Try

        End Select
    End Sub

    Public Overrides Sub ProcessForm()
        ' *** This module does not send data
    End Sub

    Public Overrides Sub TableScan(ByVal Value As String)

    End Sub

    Public Overrides Function VerifyForm() As Boolean
        Return True
    End Function

    Public Overrides Sub EditOuter()
        MyBase.EditOuter()
        Dim par(1, 1) As String
        par(0, 0) = "CNTNO"
        par(1, 0) = CtrlTable.Table.Items(CtrlTable.Table.SelectedIndices(0)).SubItems(1).Text
        par(0, 1) = "SHOWBAL"
        par(1, 1) = "FALSE"
        doNewForm(o.DOCNT, par)
    End Sub

End Class
