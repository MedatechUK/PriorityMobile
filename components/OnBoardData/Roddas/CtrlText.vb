Imports System
Imports System.Threading
Imports System.Text.RegularExpressions
Imports System.IO
Imports System.Net
Imports System.Windows.Forms
Imports System.Drawing

Public Class ctrlText

    Public Declare Sub keybd_event Lib "coredll.dll" (ByVal bVK As Byte, _
        ByVal bScan As Byte, ByVal dwFlags As Integer, ByVal dwExtraInfo _
        As Integer)

    Public ExitDir As Integer = 0
    Dim Resizing As Boolean = False

    Public Enum tCtrlState
        ctNone = 0
        ctScan = 1
        ctAlt = 2
    End Enum

    Public Enum tAltCtrlStyle
        ctNone = 0
        ctCalc = 1
        ctKeyb = 2
        ctDate = 3
        ctList = 4
    End Enum

    Public Enum tSendType
        st_Validate = 0
        st_Invoke = 1
        st_List = 2
    End Enum

    Private mSendType As tSendType
    Private mCtrlState As tCtrlState
    Private mAltCtrlStyle As tAltCtrlStyle = tAltCtrlStyle.ctNone
    Private mCtrlEnabled As Boolean = True

    Private mTop As Integer
    Private mleft As Integer
    Private mWidth As Integer
    Private mHeight As Integer

    Public Event InvokeData(ByVal ctrl As ctrlText, ByVal sql As String)
    Public Event ValidData(ByVal ctrl As ctrlText, ByVal Valid As Boolean)
    Public Event ScanBegin(ByVal ctrl As ctrlText)
    Public Event AltEntry(ByVal ctrl As ctrlText)
    Public Event EndAltEntry(ByVal ctrl As ctrlText, ByVal Top As Integer, ByVal left As Integer, ByVal width As Integer, ByVal height As Integer)
    Public Event ClickMe()
    Public Event ClickDataEntry(ByVal sender As Object, ByVal e As System.EventArgs)

    Private mValidExp As String = ""
    Private mListExp As String = ""
    Private mCtrlDes As String = ""
    Private mSQLValidation As String = ""
    Private mData(,)
    Private mIsData As Boolean = False

    Private e As System.EventArgs

    Private Actctrl As CtrlPartial
    Public IsMax As Boolean

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        mAltCtrlStyle = tAltCtrlStyle.ctNone
        mCtrlState = tCtrlState.ctNone
        IsMax = False

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Public Property AltCtrlStyle() As tAltCtrlStyle

        Get
            AltCtrlStyle = mAltCtrlStyle
        End Get

        Set(ByVal value As tAltCtrlStyle)

            Dim ctrlName As String = ""

            If Not mAltCtrlStyle = tAltCtrlStyle.ctNone Then
                Actctrl.Dispose()
            End If

            mAltCtrlStyle = value

            Select Case mAltCtrlStyle
                Case tAltCtrlStyle.ctCalc
                    Actctrl = Nothing
                    'Actctrl = New ctrlNumber
                    ctrlName = "CtrlNumber"
                Case tAltCtrlStyle.ctDate
                    'Actctrl = New CtrlDate
                    ctrlName = "CtrlDate"
                Case tAltCtrlStyle.ctKeyb
                    'Actctrl = New ctrlKeyb
                    ctrlName = "CtrlKeyb"
                Case tAltCtrlStyle.ctList
                    Actctrl = New CtrlList
                    ctrlName = "CtrlList"
                Case tAltCtrlStyle.ctNone

            End Select

            If Not mAltCtrlStyle = tAltCtrlStyle.ctNone Then
                With Actctrl
                    .Parent() = Me
                    .Name = ctrlName
                    .visible = False
                End With
                AddHandler Actctrl.Complete, AddressOf Me.Complete
            End If

        End Set

    End Property

    Public Property CtrlEnabled() As Boolean

        Get
            Return mCtrlEnabled
        End Get

        Set(ByVal value As Boolean)
            mCtrlEnabled = value
            Me.Title.Enabled = mCtrlEnabled
            If mCtrlEnabled = False Then
                LostFocus()
            End If
        End Set

    End Property

    Public Property Label() As String

        Get
            Return Title.Text
        End Get
        Set(ByVal value As String)
            Title.Text = value
            doResize()
        End Set

    End Property

    Public Property Data() As String

        Get
            Try
                If Len(DataEntry.Text) > 0 Then Value.Text = DataEntry.Text
                Return Value.Text
            Catch
                Return Nothing
            End Try
        End Get

        Set(ByVal value As String)
            If Not IsNothing(value) Then
                DataEntry.Text = ""
                Me.Value.Text = value
                doResize()
            End If
        End Set

    End Property

    Public Property ValidExp() As String

        Get
            Return mValidExp
        End Get
        Set(ByVal value As String)
            mValidExp = value
        End Set

    End Property

    Public Property ListExp() As String

        Get
            Return mListExp
        End Get
        Set(ByVal value As String)
            mListExp = value
        End Set

    End Property

    Public Property CtrlDes() As String

        Get
            Return mCtrlDes
        End Get
        Set(ByVal value As String)
            mCtrlDes = value
        End Set

    End Property

    Public Property SQLValidation() As String

        Get
            Return mSQLValidation
        End Get

        Set(ByVal value As String)
            mSQLValidation = value
        End Set

    End Property

    Public Sub BeginScan()

        mCtrlState = tCtrlState.ctScan
        RaiseEvent ScanBegin(Me)
        Value.Visible = False
        DataEntry.Height = Me.Height
        DataEntry.Visible = True
        DataEntry.Focus()

    End Sub

    Private Function IsValidExp(ByVal Value As String) As Boolean

        Dim i As Integer = 0
        Dim tio As Integer = 0
        'Dim tmp(,)

        IsValidExp = True
        'If mAltCtrlStyle = tAltCtrlStyle.ctList Then
        '    RaiseEvent ValidData(Me, False)
        '    Exit Function
        'End If

        If Len(mValidExp) > 0 Then
            If Not Regex.IsMatch(Value, mValidExp) Then
                IsValidExp = False
                RaiseEvent ValidData(Me, False)
                Exit Function
            End If
        End If

        'mIsData = False

        mSendType = tSendType.st_Validate
        RaiseEvent InvokeData(Me, mSQLValidation)

    End Function

    Public Sub NSInvoke(ByVal sql As String)

        Dim tio As Integer = 0
        mIsData = False

        mSendType = tSendType.st_Invoke
        RaiseEvent InvokeData(Me, sql)

    End Sub

    Public Sub InvokeList()

        If mAltCtrlStyle = tAltCtrlStyle.ctList Then
            Dim tio As Integer = 0
            mIsData = False

            mSendType = tSendType.st_List

            RaiseEvent InvokeData(Me, mListExp)

        End If

    End Sub

    Public Sub ReturnData(ByVal data(,) As String)

        'mIsData = True

        Select Case mSendType
            Case tSendType.st_Validate
                Try
                    For i As Integer = 0 To UBound(data, 2)
                        If Strings.StrComp(LCase(Trim(CStr(data(0, i)))), LCase(Trim(CStr(DataEntry.Text))), CompareMethod.Text) = 0 Then

                            If UBound(data, 1) = 1 Then
                                Me.DataEntry.Text = data(1, i)
                            Else
                                Me.DataEntry.Text = data(0, i)
                            End If

                            Value.Text = DataEntry.Text
                            DataEntry.Text = ""
                            DataEntry.Visible = False
                            Value.Visible = True
                            mCtrlState = tCtrlState.ctNone
                            RaiseEvent ValidData(Me, True)
                            Exit Sub

                        End If
                    Next

                    Value.Text = ""
                    DataEntry.Text = ""
                    RaiseEvent ValidData(Me, False)
                    Exit Sub

                Catch

                    Value.Text = ""
                    DataEntry.Text = ""
                    RaiseEvent ValidData(Me, False)
                    Exit Sub

                End Try

            Case tSendType.st_List
                Actctrl.AddItems(data, Value.Text)

            Case tSendType.st_Invoke
                If Me.Enabled Then
                    Me.Data = data(0, 0)
                Else
                    Me.DataEntry.Text = data(0, 0)
                End If

        End Select

    End Sub

    Private Sub Start_AltEntry(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.ClickDataEntry

        RaiseEvent ClickMe()

        If mAltCtrlStyle = tAltCtrlStyle.ctNone Then
            Exit Sub
        End If

        mCtrlState = tCtrlState.ctAlt
        Me.DataEntry.Visible = False
        Actctrl.Data = Value.Text

        Select Case mAltCtrlStyle
            Case tAltCtrlStyle.ctCalc, tAltCtrlStyle.ctKeyb, tAltCtrlStyle.ctDate
                IsMax = True
                RaiseEvent AltEntry(Me)
                Actctrl.Visible = True

            Case tAltCtrlStyle.ctList
                Me.InvokeList()
                IsMax = False
                Actctrl.SetParent(Me.DataEntry)
                Actctrl.Visible = True
                Actctrl.Focus()

        End Select

        Actctrl.BringToFront()

    End Sub

    '********************************************************

    Private Sub Ctrl_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress, MyBase.KeyPress, MyClass.KeyPress, DataEntry.KeyPress

        RaiseEvent ClickMe()

        If mCtrlState = tCtrlState.ctScan Then
            Select Case e.KeyChar
                Case vbCrLf
                    ExitDir = 0
                    ProcessEntry()
                    Actctrl.Visible = False
                Case "."
                    If Me.DataEntry.Text = "" Then
                        e.Handled = True
                        Dim data As IDataObject = Clipboard.GetDataObject()
                        If (data.GetDataPresent(DataFormats.Text)) Then
                            If data.GetData(DataFormats.Text).ToString().Length > 0 Then                                
                                Me.DataEntry.Text = data.GetData(DataFormats.Text).ToString()
                                ExitDir = 0
                                ProcessEntry()
                            End If
                        End If
                    End If
            End Select
        End If

    End Sub

    Private Sub DataEntry_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles DataEntry.KeyDown

        Select Case e.KeyValue
            Case 230
                RaiseEvent ClickMe()

                If mAltCtrlStyle = tAltCtrlStyle.ctNone Then
                    Exit Sub
                End If

                mCtrlState = tCtrlState.ctAlt
                Me.DataEntry.Visible = False
                Actctrl.Data = Value.Text

                Select Case mAltCtrlStyle
                    Case tAltCtrlStyle.ctCalc, tAltCtrlStyle.ctKeyb, tAltCtrlStyle.ctDate
                        IsMax = True
                        RaiseEvent AltEntry(Me)
                        Actctrl.Visible = True

                    Case tAltCtrlStyle.ctList
                        Me.InvokeList()
                        IsMax = False
                        Actctrl.SetParent(Me.DataEntry)
                        Actctrl.Visible = True
                        Actctrl.Focus()

                End Select

                Actctrl.BringToFront()
                Exit Sub
            Case 40
                ExitDir = 1
            Case 39
                ExitDir = 2
            Case 38
                ExitDir = -1
            Case 37
                ExitDir = -2
            Case Else
                If e.KeyValue = 115 Then
                    RaiseEvent ClickDataEntry(sender, e)
                End If
                Exit Sub
        End Select

        If Len(DataEntry.Text) > 0 Then
            If DataEntry.Text <> Value.Text Then
                ProcessEntry()
            Else
                RaiseEvent ValidData(Me, True)
            End If

        Else
            RaiseEvent ValidData(Me, True)
        End If

    End Sub

    Public Sub ResizeCtrls(ByVal e As PaintEventArgs)

        Dim FontName As String = "Arial"
        Dim FontSize = 1
        Dim FS As Font

        If Not IsMax Then

            If Not (mAltCtrlStyle = tAltCtrlStyle.ctList And mCtrlState = tCtrlState.ctAlt) Then

                Dim stringSize As New SizeF

                Do Until stringSize.Height > Me.Height Or stringSize.Width > Me.Width
                    FS = New Font(FontName, FontSize, FontStyle.Regular)
                    stringSize = e.Graphics.MeasureString(Title.Text, FS)
                    FontSize = FontSize + 1
                Loop

                If FontSize > 0 Then FontSize = FontSize - 1
                FS = New Font(FontName, FontSize, FontStyle.Regular)
                stringSize = e.Graphics.MeasureString(Title.Text, FS)

                Title.Font = FS
                Value.Font = FS
                DataEntry.Font = FS

                Title.Top = 0
                Title.Left = 0
                Title.Width = stringSize.Width + (e.Graphics.MeasureString("A", FS).Width / 2)
                Title.Height = Me.Height

                Value.Height = Me.Height
                Value.Top = 0
                Value.Left = Title.Width

                Value.Width = Me.Width - Title.Width

                DataEntry.Top = 0
                DataEntry.Height = Me.Height
                DataEntry.Left = Title.Width
                DataEntry.Top = Value.Top
                DataEntry.Width = Me.Width - Title.Width

            End If

        End If

        Select Case mAltCtrlStyle
            Case tAltCtrlStyle.ctCalc, tAltCtrlStyle.ctDate, tAltCtrlStyle.ctKeyb
                Title.Top = 0
                Title.Left = 0
                Actctrl.Top = Title.Height
                Actctrl.Left = 0
                Actctrl.Height = Me.Height
                Actctrl.Width = Me.Width
            Case tAltCtrlStyle.ctList
                Title.Top = 0
                Title.Left = 0
                Actctrl.Top = 0
                Actctrl.Left = Title.Width
                Actctrl.Height = Me.Height
                Actctrl.Width = Me.Width - Title.Width
        End Select

        'End Select

    End Sub

    Private Sub Title_LinkClicked(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Title.Click

        RaiseEvent ClickMe()

        If mCtrlEnabled Then
            Select Case mCtrlState
                Case tCtrlState.ctScan, tCtrlState.ctAlt
                    LostFocus()

                Case tCtrlState.ctNone
                    mCtrlState = tCtrlState.ctScan
                    BeginScan()

            End Select
        End If

    End Sub

    Public Shadows Sub LostFocus()

        Select Case Me.AltCtrlStyle
            Case tAltCtrlStyle.ctList
                Complete(Actctrl, CtrlPartial.tCtrl.ctrlList)
        End Select

        If Me.AltCtrlStyle = tAltCtrlStyle.ctNone Then
            Try
                If Me.DataEntry.Text <> "" Then
                    ProcessEntry()
                End If
            Catch
            End Try
        End If

        DataEntry.Text = ""
        DataEntry.Visible = False
        Value.Visible = True

        mCtrlState = tCtrlState.ctNone

    End Sub

    Private Sub Complete(ByVal ctrl As Object, ByVal ctrlType As CtrlPartial.tCtrl)

        Select Case ctrlType
            Case CtrlPartial.tCtrl.ctrlList
                Dim thisctrl As CtrlList = ctrl
                DataEntry.Text = thisctrl.Data
                thisctrl.Visible = False
            Case CtrlPartial.tCtrl.ctrlNone
                Dim thisctrl As ctrlText = ctrl
                thisctrl.Visible = False
        End Select

        If IsMax Then RaiseEvent EndAltEntry(Me, mTop, mleft, mWidth, mHeight)
        IsMax = False

        DataEntry.Visible = True

        If mCtrlState = tCtrlState.ctAlt Then
            ExitDir = 3
            ProcessEntry()
        End If

    End Sub

    Public Sub ProcessEntry()
        If Not IsValidExp(DataEntry.Text) Then
            'Value.Text = DataEntry.Text
            'DataEntry.Text = ""
            'DataEntry.Visible = False
            'Value.Visible = True
            'mCtrlState = tCtrlState.ctNone
            'RaiseEvent ValidData(Me, True)
            'Else
            Value.Text = ""
            DataEntry.Text = ""
            'RaiseEvent ValidData(Me, False)
        End If

    End Sub

    Public Sub SetPrevCtrlDim(ByVal top As Integer, ByVal left As Integer, ByVal width As Integer, ByVal height As Integer)
        mTop = top
        mleft = left
        mWidth = width
        mHeight = height
    End Sub

    Private Sub ctrlText_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles MyBase.Paint
        If Resizing Then
            ResizeCtrls(e)
            Resizing = False
        End If
    End Sub

    Private Sub doResize()
        Resizing = True
    End Sub

    Private Sub ctrlText_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Resize
        Resizing = True
    End Sub

    Private Sub DataEntry_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DataEntry.TextChanged

    End Sub
End Class
