Imports System
Imports System.Drawing
Imports System.IO
Imports System.Text.RegularExpressions
Imports celoaddata

Public MustInherit Class iForm

#Region "Events"

    Public Event SendArray(ByRef Sender As iForm, ByVal Ar As System.Array)
    Public Event Send(ByRef Sender As iForm, ByVal Command As String)
    Public Event NewForm(ByRef RSS As Integer, ByVal Param(,) As String)
    Public Event RegisterModule(ByRef Sender As iForm, ByVal mItem As MenuItem, ByVal subMenu As String)
    Public Event doTableScan(ByRef Sender As iForm, ByVal Value As String)
    Public Event EndForm()

#End Region

#Region "Must Override subs"

    Public MustOverride Sub FormSettings()
    Public MustOverride Sub TableSettings()
    Public MustOverride Function VerifyForm() As Boolean
    Public MustOverride Sub ProcessForm()
    Public MustOverride Sub ProcessEntry(ByVal ctrl As ctrlText, ByVal Valid As Boolean)
    Public MustOverride Sub BeginEdit()
    Public MustOverride Sub AfterEdit(ByVal TableIndex As Integer)
    Public MustOverride Sub BeginAdd()
    Public MustOverride Sub AfterAdd(ByVal TableIndex As Integer)
    Public MustOverride Sub TableScan(ByVal Value As String)
    Public MustOverride Sub TableRXData(ByVal Data(,) As String)
    Public MustOverride Sub EndInvokeData(ByVal Data(,) As String)

#End Region

#Region "Overridable subs"

    Public Overridable Sub FormLoaded()

    End Sub

    Public Overridable Sub FormClose()
        With Me
            .Hide()
            .Posted = True
            .EndIt()
        End With
    End Sub

    Public Overridable Sub EditOuter()

    End Sub

#End Region

#Region "Declarations"

    Public ar As New ceMyCls.MyArray
    Public p As New ceLoadData.Load

    Private mLoaded As Boolean = False
    Private mPosted As Boolean = False
    Private mModuleName As String = ""
    Private mSubMenu As String = ""
    Private mIsClosing As Boolean = False
    Private mUserName As String = ""
    Private mWarehouse As String = ""
    Private mShowOnMenu As Boolean = True

    Dim actctrl As ctrlText

    Public field As CtrlForm.tField = Nothing
    Public col As CtrlTable.tCol = Nothing
    Public ctrlText As ctrlText = Nothing

    Private myBaseForm As Form
    Public CallerApp As Form

    Private Enum tSendType
        st_Form = 0
        st_Table = 1
        st_User = 2
    End Enum

    Private SendType As tSendType

    Private Structure tArgument
        Dim argName As String
        Dim argValue As String
        Dim argDefault As String
    End Structure

    Private mArguments() As tArgument

#End Region

#Region "Public Properties"

    Public ReadOnly Property Val(ByVal Name) As String
        Get
            Dim col As Integer = CtrlForm.ColNo(Name)
            If col = -1 Then
                col = CtrlTable.ColNo(Name)
                If col = -1 Then
                    Return ""
                Else
                    If Not IsNothing(CtrlTable.el) Then
                        If Len(CtrlTable.el(col).Value.Text) > 0 Then
                            Return CtrlTable.el(col).Value.Text
                        Else
                            Return CtrlTable.el(col).Data
                        End If
                    Else
                        Return ""
                    End If
                End If
            Else
                If Not IsNothing(CtrlForm.el) Then
                    If Len(CtrlForm.el(col).Value.Text) > 0 Then
                        Return CtrlForm.el(col).Value.Text
                    Else
                        Return CtrlForm.el(col).Data
                    End If
                Else
                    Return ""
                End If
            End If
        End Get
    End Property

    Public ReadOnly Property ColNo(ByVal ColumnName As String) As Integer
        Get
            Dim ret As Integer = -1
            ret = CtrlForm.ColNo(ColumnName)
            If ret = -1 Then
                ret = CtrlTable.ColNo(ColumnName)
            End If
            Return ret
        End Get
    End Property

    Public Property Argument(ByVal argName As String) As String
        Get
            For i As Integer = 0 To UBound(mArguments)
                If LCase(Trim(mArguments(i).argName)) = LCase(Trim(argName)) Then
                    If Len(mArguments(i).argValue) > 0 Then
                        Return mArguments(i).argValue
                    Else
                        Return mArguments(i).argDefault
                    End If
                    Exit Property
                End If
            Next
            Return Nothing
        End Get
        Set(ByVal value As String)
            For i As Integer = 0 To UBound(mArguments)
                If LCase(Trim(mArguments(i).argName)) = LCase(Trim(argName)) Then
                    mArguments(i).argValue = value
                    Exit Property
                End If
            Next
        End Set
    End Property

    Public Sub NewArgument(ByVal argName, ByVal argDefault)
        Try
            ReDim Preserve mArguments(UBound(mArguments) + 1)
        Catch ex As Exception
            ReDim mArguments(0)
        End Try
        With mArguments(UBound(mArguments))
            .argName = argName
            .argDefault = argDefault
        End With

    End Sub

    Public Property ModuleName() As String
        Get
            Return mModuleName
        End Get
        Set(ByVal value As String)
            mModuleName = value
        End Set
    End Property

    Public Property SubMenu() As String
        Get
            Return mSubMenu
        End Get
        Set(ByVal value As String)
            mSubMenu = value
        End Set
    End Property

    Public Property Posted() As Boolean
        Get
            Return mPosted
        End Get
        Set(ByVal value As Boolean)
            mPosted = value
        End Set
    End Property

    Public Property UserName() As String
        Get
            Return mUserName
        End Get
        Set(ByVal value As String)
            mUserName = value
        End Set
    End Property

    Public Property Warehouse() As String
        Get
            Return mWarehouse
        End Get
        Set(ByVal value As String)
            mWarehouse = value
        End Set
    End Property

    Public Property ShowOnMenu() As Boolean
        Get
            Return mShowOnMenu
        End Get
        Set(ByVal value As Boolean)
            mShowOnMenu = value
        End Set
    End Property

#End Region

#Region "Close form delegate"

    Delegate Sub CloseMeDelegate()

    Sub CloseMe()
        MyBase.Close()
    End Sub

#End Region

#Region "Initialisation and finalisation"

    Public Sub SetBaseForm(ByRef Basef As Form)

        myBaseForm = Basef
        Me.Text = mModuleName & "."

        Dim mi As New MenuItem
        With mi
            .Text = mModuleName
        End With
        RaiseEvent RegisterModule(Me, mi, mSubMenu)

    End Sub

    Public Sub doNewForm(ByVal RSS As Integer, ByVal Param(,) As String)

        RaiseEvent NewForm(RSS, Param)

    End Sub

    Public Sub Loading()
        With Me
            .Top = 0
            .Left = 0
        End With
        Show()
        CtrlForm.MoveCursor(0, 1)
        FormLoaded()
    End Sub

    Private Sub iForm_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing

        e.Cancel = True
        Dim en As Boolean

        With Me

            If Not .Posted Then
                If MsgBox("Cancel without posting?", MsgBoxStyle.OkCancel) = MsgBoxResult.Ok Then
                    en = True
                Else
                    en = False
                End If
            Else
                en = True
            End If

            If en Then

                .Hide()

                CtrlForm.DisposeCtrls()
                CtrlTable.DisposeCtrls()

                FormSettings()
                ' Create the form controls as defined above
                CtrlForm.DrawCtrls(CtrlForm)
                TableSettings()

                CtrlTable.NewPanel()

                .Posted = True
                myBaseForm.Focus()
                RaiseEvent EndForm()

            End If

        End With

    End Sub

    Public Sub EndIt() 'Handles c.sent

        'Me.Close()
        Dim del As New CloseMeDelegate(AddressOf CloseMe)
        Me.Invoke(del)

    End Sub

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        CtrlTable.EnablePanel(False)
        'CtrlTable.ToolStrip.BringToFront()

        Me.Width = Screen.PrimaryScreen.WorkingArea.Width
        Me.Height = Screen.PrimaryScreen.WorkingArea.Height

        Dim p As System.Drawing.Point
        p.X = (Screen.PrimaryScreen.WorkingArea.Width - Me.Width) / 2
        p.Y = (Screen.PrimaryScreen.WorkingArea.Height - Me.Height) / 2
        Me.Location = p

        Me.CtrlForm.Width = Me.Width - 20 - (Me.CtrlForm.Left * 2)
        Me.CtrlTable.Width = Me.Width - 20 - (Me.CtrlTable.Left * 2)

        CtrlForm.FieldHeight = 20
        CtrlTable.FieldHeight = 20

        FormSettings()
        ' Create the form controls as defined above
        CtrlForm.DrawCtrls(CtrlForm)
        TableSettings()
        mLoaded = True

        Me.Form1_Resize(sender, e)

        'Move the cirsor to the first field (0), stepping forward (1)
        CtrlForm.MoveCursor(0, 1)

    End Sub

#End Region

#Region "Control Handlers"

    Sub handles_BeginAdd() Handles CtrlTable.BeginAdd
        BeginAdd()
    End Sub

    Sub handles_AfterAdd(ByVal TableIndex As Integer) Handles CtrlTable.AfterAdd
        AfterAdd(TableIndex)
    End Sub

    Sub handles_BeginEdit() Handles CtrlTable.BeginEdit
        BeginEdit()
    End Sub

    Sub handles_AfterEdit(ByVal TableIndex As Integer) Handles CtrlTable.AfterEdit
        AfterEdit(TableIndex)
    End Sub

    Private Sub CtrlTable_ProcessForm() Handles CtrlTable.ProcessForm

        Dim MissArg() As String = Nothing
        If Not CtrlForm.CanPost(MissArg) Then
            MsgBox("Please fill in all required fields")
            CtrlForm.MoveCursor(0, 1)
            Exit Sub
        End If

        If Not CtrlTable.CanPost(MissArg) Then
            MsgBox("Please fill in all required fields")
            CtrlForm.MoveCursor(0, 1)
            Exit Sub
        End If

        If Not VerifyForm() Then Exit Sub
        If Not (MsgBox("Post Transaction", MsgBoxStyle.OkCancel) = MsgBoxResult.Ok) Then Exit Sub

        With p
            .Clear()
            .NoData = False
            ProcessForm()
            Dim d(,) As String = .Data
            If Not (.NoData) And Not IsNothing(d) Then
                RaiseEvent SendArray(Me, d)
            End If
            FormClose()
        End With

    End Sub

    Private Sub validData(ByVal ctrl As ctrlText, ByVal Valid As Boolean) _
        Handles _
        CtrlForm.validData, _
        CtrlTable.validData

        ' *****************************************
        ' *** This code is run when a control is validated

        ProcessEntry(ctrl, Valid)

    End Sub

    Private Sub handles_TableData(ByVal Data(,)) Handles _
        CtrlTable.TableData

        TableRXData(Data)

    End Sub

    Private Sub handles_EditOuter(ByVal Index) Handles CtrlTable.EditOuter

        ' **************************************************************
        ' *** If the EditInPlace flag of the table control is set to false then
        ' *** this function is called when the edit event occurs in the table control

        'Type of form to start, The parameter to pass to the form
        EditOuter()

    End Sub

    Private Sub Handles_CtrlTable_InvokeData(ByVal ctrl As ctrlText, ByVal sql As String) _
        Handles CtrlTable.InvokeData

        ParseSQL(sql, ctrl)
        SendType = tSendType.st_Table
        RaiseEvent Send(Me, sql)

    End Sub

    Private Sub Handles_CtrlForm_InvokeData(ByVal ctrl As ctrlText, ByVal sql As String) _
    Handles CtrlForm.InvokeData

        ParseSQL(sql, ctrl)
        SendType = tSendType.st_Form
        RaiseEvent Send(Me, sql)

    End Sub

    Public Sub InvokeData(ByVal sql As String)

        ParseSQL(sql)
        SendType = tSendType.st_User
        RaiseEvent Send(Me, sql)

    End Sub

    Private Sub ParseSQL(ByRef sql As String, Optional ByVal ctrl As ctrlText = Nothing)

        Dim Arr(,) = Nothing
        Dim i As Integer

        actctrl = ctrl

        Try
            If Not IsNothing(ctrl) Then sql = Replace(sql, "%ME%", ctrl.Data, , , CompareMethod.Text)
            sql = Replace(sql, "%USER%", UserName, , , CompareMethod.Text)
            sql = Replace(sql, "%USERWARHS%", Warehouse, , , CompareMethod.Text)
            sql = Replace(sql, "%DATE%", DateDiff(DateInterval.Minute, #1/1/1988#, Now).ToString)
        Catch
            ' No selected control
        End Try

        CtrlForm.NameValues(Arr)
        CtrlTable.NameValues(Arr)

        For i = 0 To UBound(Arr, 2)
            sql = Replace(sql, "%" & Arr(0, i) & "%", Arr(1, i), , , CompareMethod.Text)
        Next

    End Sub

    Private Sub handles_TableScan(ByVal Value As String) Handles CtrlTable.Scan

        RaiseEvent doTableScan(Me, Value)

    End Sub

#End Region

#Region "Form Sizing Code"

    Private Sub Form1_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize

        If mLoaded Then

            Me.CtrlForm.Width = Me.Width - (Me.CtrlForm.Left * 2)
            Me.CtrlTable.Width = Me.Width - (Me.CtrlTable.Left * 2)

            Dim fh As Integer = 20
            Dim th As Integer = 96

            Do Until fh + th >= Me.Height - ((Me.CtrlForm.Top * 2) + 50)
                If fh < CtrlForm.InternalHeight Then fh = fh + 1
                th = th + 1
            Loop

            Me.CtrlForm.Height = fh
            Me.CtrlTable.Height = th

            Me.CtrlTable.Top = Me.CtrlForm.Top + Me.CtrlForm.Height + 10

        End If

    End Sub

    Private Sub AltEntry(ByVal ctrl As ctrlText) _
    Handles _
    CtrlForm.AltEntry, _
    CtrlTable.AltEntry

        With ctrl
            .SetPrevCtrlDim(ctrl.Top, ctrl.Left, ctrl.Width, ctrl.Height)
            .Top = CtrlForm.Top
            .Left = CtrlForm.Left
            .Width = Me.Width - CtrlForm.Left
            .Height = Me.Height - CtrlForm.Top
            .BringToFront()
        End With

    End Sub

    Private Sub EndAltEntry(ByVal ctrl As ctrlText, ByVal Top As Integer, ByVal left As Integer, ByVal width As Integer, ByVal height As Integer) _
        Handles _
        CtrlForm.EndAltEntry, _
        CtrlTable.EndAltEntry

        ctrl.Top = Top
        ctrl.Left = left
        ctrl.Width = width
        ctrl.Height = height

    End Sub

    Sub formlf(ByVal fc As Boolean) Handles CtrlTable.isFocused
        If Not fc Then CtrlTable.SetFocusActive()
        CtrlForm.DoLostFocus()
        CtrlTable.mGotFocus = True
        CtrlForm.mGotFocus = False
    End Sub

    Sub tablelf(ByVal fc As Boolean) Handles CtrlForm.isFocused
        If Not fc Then CtrlForm.SetFocusActive()
        CtrlTable.DoLostFocus()
        CtrlForm.mGotFocus = True
        CtrlTable.mGotFocus = False
    End Sub

#End Region

#Region "Incoming Data"

    Public Sub c_Command(ByVal Arguments As System.Array) 'Handles c.Command

        Dim mArgs() As String = Arguments
        MsgBox(mArgs(0), MsgBoxStyle.OkOnly)

    End Sub

    Public Sub c_ReceivedArray(ByVal data(,) As String) 'Handles c.ReceivedArray

        Try
            Select Case SendType
                Case tSendType.st_Table
                    CtrlTable.ReturnData(data)
                Case tSendType.st_Form
                    actctrl.ReturnData(data)
                Case tSendType.st_User
                    EndInvokeData(data)
            End Select

        Catch e As Exception
            'Debug.Print(e.Message)
        End Try

    End Sub

#End Region

    Private Sub CtrlTable_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles CtrlTable.DoubleClick
        RaiseEvent doTableScan(Me, InputBox("Scan Value"))
    End Sub

End Class
