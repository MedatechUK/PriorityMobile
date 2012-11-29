Imports System
Imports System.Threading
Imports System.Windows.Forms

Public Class CtrlTable

#Region "Declarations"

    Dim mIsData As Boolean = False
    Dim mData(,) = Nothing
    Dim keys As String = ""    
    Public mGotFocus As Boolean = False

    Dim _btnFinishState As Boolean = False

    Public ReadOnly Property btnFinishState() As Boolean
        Get
            Return _btnFinishState
        End Get
    End Property

    Public Structure tCol
        Dim Name As String
        Dim Title As String
        Dim initWidth As Integer
        Dim TextAlign As System.Windows.Forms.HorizontalAlignment
        Dim AltEntry As ctrlText.tAltCtrlStyle
        Dim ValidExp As String
        Dim SQLValidation As String
        Dim SQLList As String
        Dim DefaultFromCtrl As Object
        Dim ctrlEnabled As Boolean
        Dim Mandatory As Boolean
        Dim MandatoryOnPost As Boolean
    End Structure

    Public Enum tButton
        btnAdd = 0
        btnEdit = 1
        btnCopy = 2
        btnDelete = 3
        btnPost = 4
    End Enum

    Public Enum tSendType
        st_Validate = 0
        st_TableData = 1
    End Enum

    Public Event InvokeData(ByVal ctrl As ctrlText, ByVal sql As String)
    Public Event validData(ByVal ctrl As ctrlText, ByVal Valid As Boolean)
    Public Event AltEntry(ByVal ctrl As ctrlText)
    Public Event EndAltEntry(ByVal ctrl As ctrlText, ByVal Top As Integer, ByVal left As Integer, ByVal width As Integer, ByVal height As Integer)
    Public Event ProcessForm()
    Public Event isFocused(ByVal HasFocus As Boolean)
    Public Event TableData(ByVal Data(,))
    Public Event EditOuter(ByVal Index)
    Public Event BeginAdd()
    Public Event BeginEdit()
    Public Event AfterAdd(ByVal TableIndex As Integer)
    Public Event AfterEdit(ByVal TableIndex As Integer)
    Public Event AfterCopy(ByVal TableIndex As Integer)
    Public Event Scan(ByVal Value As String)

    Public el() As ctrlText
    Dim txt() As ctrlText
    Public mCol() As tCol
    Dim cIndex As Integer
    Dim mPanel As Panel

    Private mSendType As tSendType

    Private mTop As Integer
    Private mleft As Integer
    Private mWidth As Integer
    Private mHeight As Integer

    Dim mFieldHeight As Integer
    Dim mFromRS As Boolean = False
    Dim mRecordsSQL As String
    Dim mEditInPlace As Boolean = True

    ' Disable buttons
    Dim mDisablebtnAdd As Boolean = False
    Dim mDisablebtnEdit As Boolean = False
    Dim mDisablebtnCopy As Boolean = False
    Dim mDisablebtnDelete As Boolean = False
    Dim mDisablebtnFinish As Boolean = False

    Dim actctrl As ctrlText

#End Region

#Region "Initialisation"

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        EnableToolbar(True, _
            CBool(Table.SelectedIndices.Count = 1), _
            CBool(Table.SelectedIndices.Count > 0), _
            CBool(Table.SelectedIndices.Count > 0), _
            True)
        Table.Visible = True
        NewPanel()

    End Sub

#End Region

#Region "public properties"

    Public Sub NewPanel()
        mPanel = New Panel
        mPanel.Name = "mPanel"
        Controls.Add(mPanel)
        EnablePanel(False)
    End Sub

    Public ReadOnly Property ColNo(ByVal ColumnName As String) As Integer
        Get
            Dim ret As Integer = -1
            If Not IsNothing(mCol) Then
                For i As Integer = 0 To UBound(mCol)
                    If LCase(Trim(ColumnName)) = LCase(Trim(mCol(i).Name)) Then
                        ret = i
                        Exit For
                    End If
                Next
            End If
            Return ret
        End Get
    End Property

    Public Property EditInPlace() As Boolean

        ' Get and set Height of the fields
        Get
            EditInPlace = mEditInPlace
        End Get
        Set(ByVal Value As Boolean)
            mEditInPlace = Value
        End Set

    End Property

    Public Property FieldHeight() As Integer

        ' Get and set Height of the fields
        Get
            FieldHeight = mFieldHeight
        End Get
        Set(ByVal Height As Integer)
            mFieldHeight = Height
        End Set

    End Property

    Public Property FromRS() As Boolean
        Get
            FromRS = mFromRS
        End Get
        Set(ByVal value As Boolean)
            mFromRS = value
        End Set
    End Property

    Public Property RecordsSQL() As String
        Get
            RecordsSQL = mRecordsSQL
        End Get
        Set(ByVal value As String)
            mRecordsSQL = value
        End Set
    End Property

    Public ReadOnly Property RowCount() As Integer
        Get
            Return Me.Table.Items.Count - 1
        End Get
    End Property

    Public ReadOnly Property ItemValue(ByVal strName As String, ByVal Row As Integer) As String
        Get
            Try
                For i As Integer = 0 To UBound(mCol)
                    If LCase(Trim(mCol(i).Name)) = LCase(Trim(strName)) Then
                        Return Table.Items(Row).SubItems(i).Text
                    End If
                Next
                Return ""
            Catch
                Return ""
            End Try

        End Get
    End Property

#End Region

    Private _CancelEdit As Boolean = False
    Public Property CancelEdit() As Boolean
        Get
            Return _CancelEdit
        End Get
        Set(ByVal value As Boolean)
            _CancelEdit = value
        End Set
    End Property

    Public Sub AddCol(ByVal Col As tCol)

        Try
            ReDim Preserve mCol(UBound(mCol) + 1)
        Catch ex As Exception
            ReDim mCol(0)
        End Try

        mCol(UBound(mCol)) = Col
        Dim pcentWid As Integer = ((Col.initWidth / 100) * Table.Width)
        Table.Columns.Add(Col.Title, pcentWid, Col.TextAlign)

    End Sub

    Private Sub CtrlTable_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.GotFocus
        Me.Table.Focus()
    End Sub

    Private Sub CtrlTable_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize

        With Me.Table
            .Top = ToolStrip.Height + 5 'ToolStrip.Height
            .Left = 0
            .Width = Me.Width '- (Table.Margin.Left + Table.Margin.Right)
            .Height = Me.Height - (ToolStrip.Height + 5) '+ Table.Margin.Bottom + Table.Margin.Top
        End With
        With Me.Panel
            .Top = ToolStrip.Height + 5 'ToolStrip.Height
            .Left = 0
            .Width = Me.Width '- (Table.Margin.Left + Table.Margin.Right)
            .Height = Me.Height - (ToolStrip.Height + 5) '+ Table.Margin.Bottom + Table.Margin.Top
        End With
        'With Me.ToolStrip
        '    .Width = Me.Width
        'End With
    End Sub

    Public Function btnEnabled(ByVal btn As tButton)
        Select Case btn
            Case tButton.btnAdd
                Return btnAdd.Enabled
            Case tButton.btnEdit
                Return btnEdit.Enabled
            Case tButton.btnCopy
                Return btnCopy.Enabled
            Case tButton.btnDelete
                Return btnDelete.Enabled
            Case tButton.btnPost
                Return btnFinish.Enabled
            Case Else
                Return False
        End Select
    End Function

    Public Sub EnableToolbar(ByVal Add As Boolean, ByVal Edit As Boolean, ByVal copy As Boolean, ByVal Delete As Boolean, ByVal Finish As Boolean)
        If Not mDisablebtnAdd Then btnAdd.Enabled = Add Else btnAdd.Enabled = False
        If Not mDisablebtnEdit Then btnEdit.Enabled = Edit Else btnEdit.Enabled = False
        If Not mDisablebtnCopy Then btnCopy.Enabled = copy Else btnCopy.Enabled = False
        If Not mDisablebtnDelete Then btnDelete.Enabled = Delete Else btnDelete.Enabled = False
        If Not mDisablebtnFinish Then btnFinish.Enabled = Finish Else btnFinish.Enabled = False
    End Sub

    Public Sub DisableButtons(ByVal Add As Boolean, ByVal Edit As Boolean, ByVal copy As Boolean, ByVal Delete As Boolean, ByVal Finish As Boolean)
        mDisablebtnAdd = Add
        mDisablebtnEdit = Edit
        mDisablebtnCopy = copy
        mDisablebtnDelete = Delete
        mDisablebtnFinish = Finish

        If mDisablebtnAdd Then btnAdd.Enabled = False
        If mDisablebtnEdit Then btnEdit.Enabled = False
        If mDisablebtnCopy Then btnAdd.Enabled = False
        If mDisablebtnDelete Then btnAdd.Enabled = False
        If mDisablebtnFinish Then btnAdd.Enabled = False
    End Sub

    Sub EnablePanel(ByVal en As Boolean)
        Select Case en
            Case True
                With mPanel
                    .Visible = True
                    .Left = Table.Left
                    .Top = Table.Top
                    .Width = Table.Width
                    .Height = Table.Height
                    .BringToFront()
                End With
                ' Scroll if required
                mPanel.AutoScroll = CBool((UBound(mCol) + 1) * mFieldHeight > Table.Height)
                Table.Visible = False

            Case False
                With mPanel
                    .Visible = False
                    .Left = 0
                    .Top = 0
                    .Width = 0
                    .Height = 0
                End With
                Table.Visible = True
                Table.BringToFront()
                Table.Focus()

        End Select
    End Sub

    Sub DrawCtrls(Optional ByVal Index As Integer = -1)

        Dim i As Integer
        cIndex = Index
        el = Nothing

        ReDim txt(UBound(mCol))
        For i = 0 To UBound(mCol)


            txt(i) = New ctrlText

            Dim intw As Integer = Panel.Width
            If Panel.AutoScroll Then intw = intw - 20

            With txt(i)
                .Name = mCol(i).Name
                .Parent = mPanel
                .CtrlDes = mCol(i).Title
                .ValidExp = mCol(i).ValidExp
                .SQLValidation = mCol(i).SQLValidation
                .ListExp = mCol(i).SQLList
                .Label = mCol(i).Title & ":"
                .Data = ""
                .AltCtrlStyle = mCol(i).AltEntry
                .Height = mFieldHeight
                .Width = intw
                .Top = i * mFieldHeight
                .CtrlEnabled = mCol(i).ctrlEnabled
                If Not mCol(i).initWidth > 0 Then
                    .Height = 0
                    .Visible = False
                End If
            End With

            If Not mCol(i).DefaultFromCtrl Is Nothing Then
                Dim t As ctrlText = mCol(i).DefaultFromCtrl
                txt(i).Data = t.Data
            End If

            AddHandler txt(i).ScanBegin, AddressOf Me.Handles_ScanBegin
            AddHandler txt(i).InvokeData, AddressOf Me.handles_SQLValidation
            AddHandler txt(i).ValidData, AddressOf Me.handles_validData
            AddHandler txt(i).ClickMe, AddressOf Me.Handles_clickme
            'AddHandler txt(i).AltEntry, AddressOf Me.Handles_AltEntry
            'AddHandler txt(i).EndAltEntry, AddressOf Me.Handles_EndAltEntry

            Try
                ReDim Preserve el(UBound(el) + 1)
            Catch ex As Exception
                ReDim el(0)
            End Try

            el(UBound(el)) = txt(i)
            'el(ubound(el)).INVOKELIST()

            If Not (Index = -1) Then
                el(i).Data = Table.Items(Index).SubItems(i).Text
                'el(i).Text = Table.Items(Index).SubItems(i).Text
            End If

        Next

    End Sub

    Sub DisposeCtrls()

        If Not IsNothing(el) Then
            For i As Integer = 0 To UBound(el)
                el(i).Dispose()
            Next
        End If

        Table.Items.Clear()
        Table.Columns.Clear()
        mPanel.Dispose()

        el = Nothing
        mCol = Nothing
        txt = Nothing
        mData = Nothing

    End Sub

    Sub UpdateTable(ByVal Index As Integer)

        Dim i As Integer
        Dim n As Integer

        Dim lvi As New ListViewItem
        lvi.Text = "***"

        Select Case Index
            Case -1
                Table.Items.Add(lvi)
                For i = 0 To Table.Items.Count - 1
                    If Table.Items(i).Text = "***" Then
                        Table.Items(i).Text = el(0).Data
                        For n = 1 To UBound(el)
                            Table.Items(i).SubItems.Add(el(n).Data)
                        Next
                        RaiseEvent AfterAdd(i)
                    End If
                Next

            Case Else
                Try
                    For n = 0 To UBound(el)
                        Table.Items(Index).SubItems(n).Text = (el(n).Data)
                    Next
                Catch
                End Try
                Table.Items(Index).Selected = True
                RaiseEvent AfterEdit(Index)

        End Select

    End Sub

    Sub CopyRecords()

        Dim rec(,) As String
        Dim x As Integer
        Dim y As Integer
        Dim i As Integer

        Dim lvi As New ListViewItem
        lvi.Text = "***"

        For y = 0 To Table.SelectedIndices.Count - 1
            ReDim Preserve rec(Table.Items(Table.SelectedIndices(y)).SubItems.Count - 1, y)
            For x = 0 To Table.Items(Table.SelectedIndices(y)).SubItems.Count - 1
                rec(x, y) = Table.Items(Table.SelectedIndices(y)).SubItems(x).Text
            Next
        Next
        If Not IsNothing(rec) Then
            For y = 0 To UBound(rec, 2)
                Table.Items.Add(lvi)
                For i = 0 To Table.Items.Count - 1
                    If Table.Items(i).Text = "***" Then
                        For x = 0 To UBound(rec, 1)
                            Select Case x
                                Case 0
                                    Table.Items(i).Text = rec(x, y)
                                Case Else
                                    Table.Items(i).SubItems.Add(rec(x, y))
                            End Select
                        Next
                        RaiseEvent AfterCopy(i)
                    End If
                Next
            Next
        End If
    End Sub

    Private Sub Table_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Table.KeyPress
        Select Case e.KeyChar
            Case "."
                If keys.Length = 0 Then
                    Dim data As IDataObject = Clipboard.GetDataObject()
                    If (data.GetDataPresent(DataFormats.Text)) Then
                        If data.GetData(DataFormats.Text).ToString().Length > 0 Then
                            RaiseEvent Scan(data.GetData(DataFormats.Text).ToString())
                            keys = ""
                        End If
                    End If
                End If
            Case Chr(13)
                RaiseEvent Scan(keys)
                keys = ""
            Case Chr(10)
                ' Ignore
            Case Else
                keys = keys & e.KeyChar
        End Select
    End Sub

    Private Sub Table_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Table.SelectedIndexChanged
        If Not mGotFocus Then RaiseEvent isFocused(True)
        EnableToolbar(btnAdd.Enabled, _
            CBool(Table.SelectedIndices.Count = 1), _
            CBool(Table.SelectedIndices.Count > 0), _
            CBool(Table.SelectedIndices.Count > 0), _
            True)

    End Sub

    Public Sub doEdit(ByVal sender As System.Object)
        Dim btn As ToolBarButton = New ToolBarButton
        btn.ImageIndex = 1
        Dim e As System.Windows.Forms.ToolBarButtonClickEventArgs = New System.Windows.Forms.ToolBarButtonClickEventArgs(btn)
        ButtonClick(sender, e)
    End Sub

    Public Sub SetTable()
        Try
            For i As Integer = 0 To UBound(mCol)
                If mCol(i).initWidth > 0 Then
                    If Len(txt(i).DataEntry.Text) > 0 Then el(i).ProcessEntry()
                    If mCol(i).Mandatory And Len(txt(i).Data) = 0 Then
                        Select Case MsgBox("Please fill in all mandatory fields, " & vbCrLf & _
                                        "or cancel to abandon changes.", MsgBoxStyle.OkOnly)
                            Case MsgBoxResult.Ok
                                SetFocusActive()
                                Exit Sub
                        End Select
                    End If
                End If
            Next
        Catch
        End Try

        EnablePanel(False)

        EnableToolbar(True, _
            CBool(Table.SelectedIndices.Count = 1), _
            CBool(Table.SelectedIndices.Count > 0), _
            CBool(Table.SelectedIndices.Count > 0), _
            True)
        Table.Visible = True
        UpdateTable(cIndex)

        mPanel.Dispose()
        NewPanel()
        Table.Focus()

    End Sub

    Public Sub SetEdit()
        EnableToolbar(False, False, False, False, True)
        _CancelEdit = False
        RaiseEvent BeginEdit()
        If Not _CancelEdit Then
            If mEditInPlace Then
                'Try
                Table.Visible = False
                EnablePanel(True)
                DrawCtrls(Table.SelectedIndices(0))
                'Catch ex As Exception
                '    EnablePanel(False)
                '    EnableToolbar(True, _
                '        CBool(Table.SelectedIndices.Count = 1), _
                '        CBool(Table.SelectedIndices.Count > 0), _
                '        CBool(Table.SelectedIndices.Count > 0), _
                '        True)
                '    Table.Visible = True
                'End Try
            Else
                RaiseEvent EditOuter(Table.SelectedIndices(0))
            End If
        End If
    End Sub

    Public Sub SetAdd()
        EnableToolbar(False, False, False, False, True)
        RaiseEvent BeginAdd()
        Table.Visible = False
        EnablePanel(True)
        DrawCtrls()
        RaiseEvent validData(el(0), True)
        MoveCursor(0, 1, False)
    End Sub

    Private Sub ButtonClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles ToolStrip.ButtonClick

        Dim i As Integer
        Dim c As Boolean

        If Not mGotFocus Then RaiseEvent isFocused(True)

        Select Case LCase(e.Button.ImageIndex)
            Case 0 ' "btnadd"
                SetAdd()

            Case 1 '"btnedit"
                SetEdit()

            Case 2 ' "btncopy"
                CopyRecords()

            Case 3 ' "btndelete"
                If MsgBox("Are you sure you wish to delete the selected items?", _
                MsgBoxStyle.DefaultButton2 + MsgBoxStyle.Critical + MsgBoxStyle.OkCancel, _
                "Delete Items") = MsgBoxResult.Ok Then
                    For i = Table.Items.Count - 1 To 0 Step -1
                        If Table.Items(i).Selected Then
                            Remove(i)
                        End If
                    Next
                End If

            Case 4 '"btnfinish"
                c = False
                _btnFinishState = True
                Select Case mPanel.Visible
                    Case True
                        For i = 0 To UBound(mCol)
                            If mCol(i).initWidth > 0 Then
                                If Len(txt(i).DataEntry.Text) > 0 Then el(i).ProcessEntry()
                                If mCol(i).Mandatory And Len(txt(i).Data) = 0 Then
                                    Select Case MsgBox("Please fill in all mandatory fields, " & vbCrLf & _
                                                    "or cancel to abandon changes.", MsgBoxStyle.OkCancel)
                                        Case MsgBoxResult.Ok
                                            SetFocusActive()
                                            Exit Sub
                                        Case MsgBoxResult.Cancel
                                            c = True
                                            Exit For
                                    End Select
                                End If
                            End If
                        Next

                        EnablePanel(False)

                        EnableToolbar(True, _
                            CBool(Table.SelectedIndices.Count = 1), _
                            CBool(Table.SelectedIndices.Count > 0), _
                            CBool(Table.SelectedIndices.Count > 0), _
                            True)
                        Table.Visible = True
                        If Not c Then UpdateTable(cIndex)

                        mPanel.Dispose()
                        NewPanel()
                        Table.Focus()

                        'If Not IsNothing(el) Then
                        '    For q As Integer = 0 To UBound(el)
                        '        el(q).Dispose()
                        '    Next
                        'End If
                        _btnFinishState = False

                    Case False
                        RaiseEvent ProcessForm()

                End Select
        End Select
    End Sub

    Sub Remove(ByVal i As Integer)

        Dim rec(,) As String
        Dim x As Integer
        Dim y As Integer
        Dim q As Integer

        For y = 0 To Table.Items.Count - 1
            If y <> i Then

                Try
                    ReDim Preserve rec(Table.Items(0).SubItems.Count - 1, UBound(rec, 2) + 1)
                Catch
                    ReDim Preserve rec(Table.Items(0).SubItems.Count - 1, 0)
                End Try

                For x = 0 To UBound(rec, 1)
                    rec(x, UBound(rec, 2)) = Table.Items(y).SubItems(x).Text
                Next

            End If
        Next

        'Table.Clear()
        Table.Items.Clear()

        Try
            For y = 0 To UBound(rec, 2)
                Dim lvi As New ListViewItem
                For x = 0 To UBound(rec, 1)
                    Select Case x
                        Case 0
                            lvi.Text = rec(x, y)
                        Case Else
                            Dim r As New System.Windows.Forms.ListViewItem.ListViewSubItem
                            r.Text = rec(x, y)
                            lvi.SubItems.Add(r)
                    End Select
                Next

                Table.Items.Add(lvi)
            Next
        Catch
            ' No records
        End Try

    End Sub

    Sub SetFocusActive()

        Dim i As Integer
        For i = 0 To UBound(mCol)
            If mCol(i).initWidth > 0 Then
                If Not IsNothing(txt(i).DataEntry) Then
                    If txt(i).DataEntry.Visible = True Then
                        txt(i).DataEntry.Focus()
                        Exit For
                    End If
                End If
            End If
        Next
        Me.mGotFocus = True

    End Sub

    Public Sub DoLostFocus()

        Dim i As Integer
        Try
            For i = 0 To UBound(el)
                el(i).LostFocus()
            Next
        Catch
            'NO controls
        End Try
        Me.mGotFocus = False

    End Sub

    '*************************************************************************
    'Handlers
    '*************************************************************************

    Private Sub Handles_ScanBegin(ByVal ctrl As ctrlText)

        Dim i As Integer

        For i = 0 To UBound(el)
            If Not (el(i).Name = ctrl.Name) Then
                el(i).LostFocus()
            End If
        Next

    End Sub

    Public Sub handles_SQLValidation(ByVal ctrl As ctrlText, ByVal sql As String)

        mSendType = tSendType.st_Validate
        actctrl = ctrl
        RaiseEvent InvokeData(ctrl, sql)

    End Sub

    Public Sub handles_validData(ByVal ctrl As ctrlText, ByVal Valid As Boolean)

        Dim i As Integer
        Dim f As Integer
        Dim fnd As Boolean = False
        Dim ExitDir As Integer ' 0=unspecified 1=up arrow 2=down arrow

        If Valid Then RaiseEvent validData(ctrl, Valid)

        Select Case Valid
            Case False
                ctrl.BeginScan()

            Case True
                For i = 0 To UBound(el)
                    If el(i).Name = ctrl.Name Then
                        fnd = True
                        ExitDir = el(i).ExitDir
                        f = i
                        Exit For
                    End If
                Next

                If fnd Then
                    Select Case ExitDir
                        Case 0
                            If Not f = UBound(el) Then
                                MoveCursor(f + 1, 1, False)
                            Else
                                MoveCursor(0, 1, False)
                            End If

                        Case -1
                            If Not f = 0 Then
                                MoveCursor(f - 1, -1)
                            End If

                        Case -2
                            If Not f = 0 Then
                                MoveCursor(f - 1, -1)
                            Else
                                MoveCursor(UBound(el), -1)
                            End If

                        Case 1
                            If Not f = UBound(el) Then
                                MoveCursor(f + 1, 1)
                            End If

                        Case 2
                            If Not f = UBound(el) Then
                                MoveCursor(f + 1, 1)
                            Else
                                MoveCursor(0, 1)
                            End If
                        Case 3
                            If Not f = UBound(el) Then
                                MoveCursor(f + 1, 1, False)
                            Else
                                MoveCursor(0, 1, False)
                            End If
                            'Do nothing
                    End Select

                End If

        End Select

    End Sub

    Sub MoveCursor(ByVal NextPos, ByVal St, Optional ByVal igsp = True)

        Dim i As Integer
        Dim en As Integer
        Dim s As Integer

        Select Case St
            Case 1
                en = UBound(el)
                s = 0
            Case -1
                en = 0
                s = UBound(el)
        End Select

        For i = NextPos To en Step St
            If el(i).CtrlEnabled Then
                If igsp Or el(i).Data = "" Then
                    el(i).BeginScan()
                    Exit Sub
                End If
            End If
        Next
        For i = s To NextPos Step St
            If igsp Or el(i).Data = "" Then
                If el(i).CtrlEnabled Then
                    el(i).BeginScan()
                    Exit Sub
                End If
            End If
        Next

    End Sub

    Private Sub Handles_clickme()
        If Not mGotFocus Then RaiseEvent isFocused(True)
    End Sub

    Public Sub SetPrevCtrlDim(ByVal top As Integer, ByVal left As Integer, ByVal width As Integer, ByVal height As Integer)
        mTop = top
        mleft = left
        mWidth = width
        mHeight = height
    End Sub

    Public Sub NameValues(ByRef Arr(,) As String)

        Dim i As Integer

        Try
            For i = 0 To UBound(mCol)
                Try
                    ReDim Preserve Arr(1, UBound(Arr, 2) + 1)
                Catch ex As Exception
                    ReDim Arr(1, 0)
                End Try

                Arr(0, UBound(Arr, 2)) = el(i).Name
                Arr(1, UBound(Arr, 2)) = el(i).Data

            Next
        Catch

        End Try

    End Sub

    Public Function CanPost(ByRef MissingAr() As String) As Boolean

        Dim y As Integer
        Dim x As Integer

        Dim cp As Boolean = True

        If Panel.Visible Then
            Return False
        Else
            If Table.Items.Count - 1 = -1 Then Return False
            For y = 0 To Table.Items.Count - 1
                For x = 0 To UBound(mCol)
                    If Table.Items(y).SubItems(x).Text = "" And mCol(x).MandatoryOnPost Then
                        cp = False
                        Try
                            ReDim Preserve MissingAr(UBound(MissingAr) + 1)
                        Catch ex As Exception
                            ReDim MissingAr(0)
                        End Try
                        MissingAr(UBound(MissingAr)) = mCol(x).Title
                    End If
                Next
            Next
        End If

        Return cp

    End Function

    Public Sub BeginLoadRS()

        'Dim tio As Integer
        Dim ctrl As ctrlText = Nothing

        Me.FromRS = True
        mSendType = tSendType.st_TableData
        RaiseEvent InvokeData(ctrl, Me.mRecordsSQL)

    End Sub

    Public Sub ReturnData(ByVal Data As Array)

        Select Case mSendType
            Case tSendType.st_TableData
                Me.FromRS = False
                RaiseEvent TableData(Data)
                EnableToolbar(True, _
                    CBool(Table.SelectedIndices.Count = 1), _
                    CBool(Table.SelectedIndices.Count > 0), _
                    CBool(Table.SelectedIndices.Count > 0), _
                    True)
            Case tSendType.st_Validate
                actctrl.ReturnData(Data)
        End Select

    End Sub

End Class
