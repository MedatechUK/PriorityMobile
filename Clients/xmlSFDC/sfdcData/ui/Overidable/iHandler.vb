Imports Priority
Imports System.Windows.Forms

Public MustInherit Class iHandler

#Region "Initialisation and finalisation"

    Private _CloseHandler As System.EventHandler
    Public Property CloseHandler() As System.EventHandler
        Get
            Return _CloseHandler
        End Get
        Set(ByVal value As System.EventHandler)
            _CloseHandler = value
        End Set
    End Property

    Public Sub Close(ByVal thisForm As iForm)
        _CloseHandler.Invoke(thisForm, New System.EventArgs)
    End Sub

#End Region

#Region "Overridable Button Methods"

    Public Overridable Sub DisabledButtons(ByRef Add As Boolean, ByRef Edit As Boolean, ByRef Copy As Boolean, ByRef Delete As Boolean, ByRef Print As Boolean)
        Add = True
        Edit = True
        Copy = True
        Delete = True
        Print = True
    End Sub

    Public Overridable Sub btn_AddPress(ByRef thisForm As iForm)
        With thisForm.ViewMain.TableView
            .TableView = TableView.eTableView.vForm
        End With
    End Sub

    Public Overridable Sub btn_EditPress(ByRef thisForm As iForm)
        With thisForm.ViewMain.TableView
            .TableView = TableView.eTableView.vForm
        End With
    End Sub

    Public Overridable Sub btn_CopyPress(ByRef thisForm As iForm)
        With thisForm.ViewMain

        End With
    End Sub

    Public Overridable Sub btn_DeletePress(ByRef thisForm As iForm)
        With thisForm.ViewMain

        End With
    End Sub

    Public Overridable Sub btn_PrintPress(ByRef thisForm As iForm)
        With thisForm.ViewMain

        End With
    End Sub

    Public Overridable Sub btn_SubmitPress(ByRef thisForm As iForm)
        With thisForm.ViewMain.TableView
            .TableView = TableView.eTableView.vTable
        End With
    End Sub

    Public Overridable Sub btn_PostPress(ByRef thisForm As iForm)
        With thisForm.ViewMain

        End With
    End Sub

    Public Overridable Sub PrintForm(ByRef thisForm As iForm)

    End Sub

#End Region

#Region "Dialogs"

    Public Overridable Sub CloseDialog(ByRef thisForm As iForm, ByRef frmDialog As UserDialog)

    End Sub

#End Region

#Region "Loading"

    Public Overridable Sub Post(ByRef thisForm As iForm, ByRef xl As Priority.Loading)

        Dim args() As String

        With thisForm.ViewMain

            For Each thisCol As cColumn In .FormView.ViewForm.Columns.Values
                If thisCol.Postable Then
                    Select Case thisCol.ColumnType.ToLower
                        Case "int"
                            xl.AddColumn(1) = New LoadColumn(thisCol.Name, xl.NamedType(thisCol.ColumnType), , thisCol.Decimals)
                        Case Else
                            xl.AddColumn(1) = New LoadColumn(thisCol.Name, xl.NamedType(thisCol.ColumnType))
                    End Select

                End If
            Next

            For Each thisCol As cColumn In .TableView.ViewTable.Columns.Values
                If thisCol.Postable Then
                    Select Case thisCol.ColumnType.ToLower
                        Case "int"
                            xl.AddColumn(2) = New LoadColumn(thisCol.Name, xl.NamedType(thisCol.ColumnType), , thisCol.Decimals)
                        Case Else
                            xl.AddColumn(2) = New LoadColumn(thisCol.Name, xl.NamedType(thisCol.ColumnType))
                    End Select
                End If
            Next

            args = Nothing
            For Each thisCol As cColumn In .FormView.ViewForm.Columns.Values
                If thisCol.Postable Then
                    Try
                        ReDim Preserve args(UBound(args) + 1)
                    Catch ex As Exception
                        ReDim args(0)
                    Finally
                        args(UBound(args)) = thisCol.Value.ToString
                    End Try
                End If
            Next
            xl.AddRecordType(1) = New LoadRow(args)

            With .TableView.ViewTable
                For Each lvi As ListViewItem In .thisTable.Items
                    args = Nothing
                    Dim i As Integer = 0
                    For Each thisCol As cColumn In .Columns.Values
                        If thisCol.Postable Then
                            Try
                                ReDim Preserve args(UBound(args) + 1)
                            Catch ex As Exception
                                ReDim args(0)
                            Finally
                                args(UBound(args)) = lvi.SubItems(i).Text
                            End Try
                        End If
                        i += 1
                    Next
                    xl.AddRecordType(2) = New LoadRow(args)
                Next
            End With

        End With

    End Sub

#End Region

#Region "Override data Entry"

    Public Overridable Sub AltEntry(ByRef uiCol As uiColumn)

        With uiCol

            Select Case EvalType(.thisColumn.ColumnType)
                Case ePriorityType.INT_Type, ePriorityType.REAL_Type, ePriorityType.UNSIGNED_Type
                    ' Numeric Type - use calc control
                    With .ParentForm
                        .ViewCalc.InitSetting(InitCalc(uiCol))
                        AddHandler .ViewCalc.SetNumber, AddressOf uiCol.hCalc
                        .View = iForm.eiFromView.ViewCalc
                    End With

                Case Else
                    ' Has Choose Trigger - use drop down list
                    If uiCol.thisColumn.Triggers.Keys.Contains("CHOOSE-FIELD") Then
                        uiCol.ColStyle = eColStyle.colList

                    ElseIf InStr(.thisColumn.Name.ToLower, "signature") > 0 Then
                        With .ParentForm
                            .ViewSignature.Clear()
                            AddHandler .ViewSignature.SaveSignature, AddressOf uiCol.hSaveSignature
                            .View = iForm.eiFromView.ViewSignature
                        End With

                    End If

            End Select

        End With

    End Sub

    Public Overridable Function InitCalc(ByRef uiCol As uiColumn) As calcSetting

        Dim Val As Double = 0.0
        Dim cs As calcSetting = Nothing

        With uiCol
            If .thisColumn.Value.Length > 0 Then
                If IsNumeric(.thisColumn.Value) Then
                    Val = CDbl(.thisColumn.Value)
                End If
            End If

            Select Case EvalType(.thisColumn.ColumnType)
                Case ePriorityType.INT_Type
                    cs = New calcSetting(CInt(Val), -999, , .thisColumn.Title)
                Case ePriorityType.REAL_Type
                    cs = New calcSetting(CDbl(Val), -999, , .thisColumn.Title)
                Case ePriorityType.UNSIGNED_Type
                    cs = New calcSetting(CInt(Val), 0, , .thisColumn.Title)
            End Select

        End With

        Return cs

    End Function

    Public Overridable Sub CheckField(ByRef Validate As Boolean, ByVal uiCol As uiColumn)

    End Sub

    Public Overridable Sub PostField(ByRef uiCol As uiColumn)

    End Sub

    Public Overridable Sub Scan1d(ByRef sb As TablePanel)

    End Sub

    Public Overridable Sub Scan2d(ByRef sb As TablePanel)
        For Each k As String In sb.ScanBuffer.ScanDictionary.Keys

        Next
    End Sub

#End Region

End Class
