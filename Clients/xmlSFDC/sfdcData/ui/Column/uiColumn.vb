Imports System
Imports System.Xml
Imports System.Windows.Forms

Public Class uiColumn
    Inherits iFormChild

#Region "Private Variables"

    'Private _2d As Boolean = False
    Private _mode As eMode = eMode.label
    Private _mandatory As Boolean = False
    Private _readOnly As Boolean = False
    Private _SwitchView As Boolean
    Private _IgnoreClick As Boolean = False

#End Region

#Region "Inheritance"

    Public Overrides ReadOnly Property ParentForm() As iForm
        Get
            Return _Parent.ParentForm
        End Get
    End Property

    Private _Parent As ColumnPanel
    Public Overloads ReadOnly Property Parent() As ColumnPanel
        Get
            Return _Parent
        End Get
    End Property

#End Region

#Region "Public Properties"

    Private _thisColumn As cColumn
    Public ReadOnly Property thisColumn() As cColumn
        Get
            Return _thisColumn
        End Get
    End Property

    Private _Selected As Boolean = False
    Public Property Selected() As Boolean
        Get
            Return _Selected
        End Get
        Set(ByVal value As Boolean)
            _Selected = value
        End Set
    End Property

    Private _ScanBuffer As ScanBuffer
    Public Property ScanBuffer() As ScanBuffer
        Get
            Return _ScanBuffer
        End Get
        Set(ByVal value As ScanBuffer)
            _ScanBuffer = value
        End Set
    End Property

    Public ReadOnly Property HasScanBuffer() As Boolean
        Get
            Return _ScanBuffer.ToString.Length > 0 Or Me.ColStyle = eColStyle.colList
        End Get
    End Property

#End Region

#Region "Initialisation and Finalisation"

    Public Sub New(ByRef Parent As ColumnPanel, ByRef Col As cColumn)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        _Parent = Parent
        _thisColumn = Col
        _ScanBuffer = New ScanBuffer(Me)
        AddHandler _ScanBuffer.BufferChanged, AddressOf hBufferChanged

        With _thisColumn
            .SetControl(Me)
            _mandatory = .Mandatory
            _readOnly = .isReadOnly
            Label.Text = .Title
        End With
        ColStyle = eColStyle.colDeselected

    End Sub

#End Region

#Region "Public Methods"

    Public Sub ProcessBuffer(ByRef excep As Exception)

        excep = Nothing

        Select Case ColStyle
            Case eColStyle.colList
                thisColumn.Value = list.SelectedItem
                lbl_Value.Text = list.SelectedItem
                ColStyle = eColStyle.colSelected
                Parent.NextControl(True)

            Case eColStyle.colSelected
                Select Case _ScanBuffer.is2d
                    Case True
                        For Each k As String In _ScanBuffer.ScanDictionary.Keys
                            Dim ScanCol As cColumn = _thisColumn.Parent.ScanColumn(k)
                            If Not IsNothing(ScanCol) Then
                                With ScanCol
                                    .Validate(_ScanBuffer.ScanDictionary(k), excep)
                                    If Not IsNothing(excep) Then
                                        'MsgBox(excep.Message, , ScanCol.Title)
                                        .uiCol.Label.Focus()
                                        _ScanBuffer.Clear()
                                        Exit Sub
                                    Else
                                        .uiCol.lbl_Value.Text = _ScanBuffer.ScanDictionary(k)
                                    End If
                                End With
                            End If
                        Next

                        _ScanBuffer.Clear()
                        If IsNothing(Parent.FocusedControl) Then
                            Parent.FirstControl()
                        End If

                        'Dim doc As New Xml.XmlDocument
                        'doc.LoadXml(_ScanBuffer.Value)
                        'For Each item As XmlNode In doc.SelectNodes("in/i")
                        '    Dim ScanCol As cColumn = _thisColumn.Parent.ScanColumn(item.Attributes("n").Value)
                        '    If Not IsNothing(ScanCol) Then
                        '        With ScanCol
                        '            .Validate(item.Attributes("v").Value, excep)
                        '            If Not IsNothing(excep) Then
                        '                MsgBox(excep.Message, , ScanCol.Title)
                        '                .uiCol.Label.Focus()
                        '                _ScanBuffer.Clear()
                        '                Exit Sub
                        '            Else
                        '                .uiCol.lbl_Value.Text = item.Attributes("v").Value
                        '            End If
                        '        End With
                        '    End If
                        'Next

                    Case Else
                        If _ScanBuffer.Length > 0 Then
                            _thisColumn.Validate(_ScanBuffer.Value, excep)
                            _ScanBuffer.Clear()
                        Else
                            excep = Nothing
                        End If

                End Select

            Case eColStyle.colDeselected, eColStyle.colReadOnly
                'ignore

        End Select

    End Sub

    Public Sub CheckDependancy()
        Me.ColStyle = eColStyle.colDeselected
    End Sub

#End Region

#Region "Column Style"

    Private _ColStyle As eColStyle = eColStyle.colDeselected
    Public Property ColStyle() As eColStyle
        Get
            Return _ColStyle
        End Get
        Set(ByVal value As eColStyle)
            Try
                With Me

                    _ColStyle = value
                    Me.Enabled = True

                    If thisColumn.isReadOnly Then
                        Me.Enabled = False
                        _ColStyle = eColStyle.colReadOnly
                    Else
                        For Each dep As cColumn In thisColumn.Depends
                            If dep.Value.Length = 0 Then
                                Me.Enabled = False
                                thisColumn.Value = ""
                                _ColStyle = eColStyle.colReadOnly
                                Exit For
                            End If
                        Next
                    End If

                    Select Case _ColStyle
                        Case eColStyle.colDeselected
                            If Not _mode = eMode.label Then ShowCtrl(eMode.label)
                            With .lbl_Value
                                .Text = _thisColumn.Value
                                .ForeColor = Drawing.Color.Black
                                .BackColor = Me.BackColor
                            End With
                            With Label
                                '.Enabled = Not (thisColumn.isReadOnly)
                                Select Case _mandatory
                                    Case True
                                        .ForeColor = Drawing.Color.Red
                                    Case Else
                                        .ForeColor = Drawing.Color.Blue
                                End Select
                            End With                            

                        Case eColStyle.colSelected
                            If Not _mode = eMode.label Then ShowCtrl(eMode.label)
                            With .lbl_Value
                                .Text = ""
                                .ForeColor = Drawing.Color.Black
                                .BackColor = Drawing.Color.Red
                            End With
                            With Label
                                '.Enabled = Not (thisColumn.isReadOnly)
                                Select Case _mandatory
                                    Case True
                                        .ForeColor = Drawing.Color.Red
                                    Case Else
                                        .ForeColor = Drawing.Color.Blue
                                End Select
                                .Focus()
                                If .Enabled Then Selected = True
                            End With

                        Case eColStyle.colReadOnly
                            With .lbl_Value
                                .Visible = True
                                .ForeColor = Drawing.Color.Black
                                .BackColor = Me.BackColor
                            End With
                            With .Label
                                '.Enabled = Not (thisColumn.isReadOnly)
                                .ForeColor = Drawing.Color.DarkGray
                            End With
                            Me.Enabled = Not (thisColumn.isReadOnly)

                        Case eColStyle.colList
                            If Not _mode = eMode.list Then
                                Dim data As Data.DataTable = _thisColumn.Triggers("CHOOSE-FIELD").Execute()
                                If Not IsNothing(data) Then
                                    Dim selIndex As Integer = -1
                                    With Me.list
                                        With .Items
                                            .Clear()
                                            For Each r As Data.DataRow In data.Rows
                                                .Add(r.Item(1))
                                                If String.Compare(r.Item(1).ToString.ToLower, thisColumn.Value.ToLower) = 0 Then
                                                    selIndex = .Count - 1
                                                End If
                                            Next
                                        End With
                                        .SelectedIndex = selIndex
                                    End With
                                End If
                                ShowCtrl(eMode.list)
                            End If

                            With Label
                                Select Case _mandatory
                                    Case True
                                        .ForeColor = Drawing.Color.Red
                                    Case Else
                                        .ForeColor = Drawing.Color.Blue
                                End Select
                            End With

                    End Select
                End With
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        End Set
    End Property

    Private Sub ShowCtrl(ByVal show As eMode)

        _SwitchView = True
        Select Case show
            Case eMode.label

                With list
                    .Update()
                    .Hide()
                    .Visible = False
                    .Dock = Windows.Forms.DockStyle.None
                End With

                With lbl_Value
                    .Dock = Windows.Forms.DockStyle.Right
                    .Width = Me.Width - Label.Width - 2
                    .Visible = True
                End With
                Label.Focus()

                _mode = eMode.label
                Me.Invalidate()

            Case eMode.list
                With lbl_Value
                    .Visible = False
                    .Dock = Windows.Forms.DockStyle.None
                End With

                With list
                    .Show()
                    .Dock = Windows.Forms.DockStyle.Right
                    .Width = Me.Width - Label.Width - 2
                    .Visible = True
                    .Focus()
                End With

                _mode = eMode.list

        End Select
        _SwitchView = False

    End Sub

#End Region

#Region "Key events"

    Private Sub hBufferChanged()
        If _ScanBuffer.is2d Then
            lbl_Value.Text = String.Empty
        Else
            lbl_Value.Text = _ScanBuffer.Value
        End If
    End Sub

    'Private Sub uiColumn_Key(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Label.KeyPress
    '    e.Handled = True
    '    Select Case Asc(e.KeyChar)
    '        Case 13, 10, 8
    '        Case 60
    '            If _ScanBuffer.Length = 0 Then
    '                _2d = True
    '            End If
    '            _ScanBuffer = "<"

    '        Case Else
    '            _ScanBuffer = _ScanBuffer & e.KeyChar.ToString.Replace(Chr(13), "").Replace(Chr(10), "").Trim
    '            If Not _2d Then
    '                lbl_Value.Text = _ScanBuffer
    '            End If
    '    End Select

    'End Sub

    Private Sub uiColumn_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Label.KeyDown

        Select Case e.KeyValue

            Case 8
                e.Handled = True

                If Me.Selected And (Not _ScanBuffer.is2d) Then
                    _ScanBuffer.BackSpace()

                ElseIf Not (Me.Selected) Then
                    If MsgBox("Clear Value?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                        lbl_Value.Text = ""
                        thisColumn.Value = ""
                        _ScanBuffer.Clear()
                    End If
                End If

            Case 40, 39, 38, 37
                e.Handled = True
                Dim ex As New Exception

                Select Case e.KeyValue
                    Case 40, 39
                        If HasScanBuffer Then
                            ProcessBuffer(ex)
                            If IsNothing(ex) Then
                                Deselect()
                                Parent.NextControl(False)
                            Else
                                MsgBox(ex.Message, , thisColumn.Title)
                            End If
                        Else
                            Deselect()
                            Parent.NextControl(False)
                        End If

                    Case 38, 37
                        If HasScanBuffer Then
                            ProcessBuffer(ex)
                            If IsNothing(ex) Then
                                Deselect()
                                Parent.PreviousControl(False)
                            Else
                                MsgBox(ex.Message, , thisColumn.Title)
                            End If
                        Else
                            Deselect()
                            Parent.PreviousControl(False)
                        End If

                End Select

            Case 32
                e.Handled = True                

                If _ScanBuffer.Length = 0 Then
                    ParentForm.thisHandler.AltEntry(Me)
                Else
                    If Not _ScanBuffer.Length = 0 Then
                        _ScanBuffer.Append(e.KeyValue)
                    End If
                End If

            Case 10
                    e.Handled = True

            Case 13
                    e.Handled = True

                    If Me.Selected Then
                        _IgnoreClick = True

                        Dim ex As New Exception
                        ProcessBuffer(ex)

                        If IsNothing(ex) Then
                            Deselect()
                            Parent.NextControl(True)
                        Else
                            MsgBox(ex.Message, , thisColumn.Title)
                        End If

                    End If

            Case 63, 46
                    e.Handled = True
                    With thisColumn
                        MsgBox(.Help, MsgBoxStyle.OkOnly, .Title)
                    End With

        End Select

    End Sub

    Private Sub list_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles list.KeyDown

        Select Case e.KeyValue
            Case 13
                If list.SelectedIndex > -1 Then
                    e.Handled = True
                    Dim seltext As String = list.SelectedItem
                    If seltext.Length > 0 Then
                        Dim ex As New Exception
                        ProcessBuffer(ex)
                    End If
                End If
        End Select

    End Sub

    Private Sub list_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles list.KeyDown

        Select Case e.KeyValue

            Case 40, 39, 38, 37
                e.Handled = True
                Dim ex As New Exception

                Select Case e.KeyValue
                    Case 40, 39
                        If list.SelectedIndex < list.Items.Count - 1 Then
                            list.SelectedIndex = list.SelectedIndex + 1
                        End If

                    Case 38, 37
                        If list.SelectedIndex > 0 Then
                            list.SelectedIndex = list.SelectedIndex - 1
                        End If

                End Select

        End Select

    End Sub

#End Region

#Region "Dialogs"

    Public Sub hCalc(ByRef cSetting As calcSetting)

        With ParentForm
            RemoveHandler .ViewCalc.SetNumber, AddressOf hCalc
            .View = iForm.eiFromView.ViewMain
        End With

        If cSetting.Result = Windows.Forms.DialogResult.OK Then
            _ScanBuffer.Value = cSetting.DNUM.ToString

            Dim ex As New Exception
            ProcessBuffer(ex)

            If IsNothing(ex) Then
                Deselect()
                Parent.NextControl(True)
            Else
                MsgBox(ex.Message, , thisColumn.Title)
            End If
        Else
            Me.Label.Focus()
        End If

    End Sub

    Public Sub hSaveSignature(ByRef Result As String)

        With ParentForm
            RemoveHandler .ViewSignature.SaveSignature, AddressOf hSaveSignature
            .View = iForm.eiFromView.ViewMain
        End With

        If Not String.Compare(Result, String.Empty) = 0 Then
            _ScanBuffer.Value = Result

            Dim ex As New Exception
            ProcessBuffer(ex)

            If IsNothing(ex) Then
                Deselect()
                Parent.NextControl(True)
            Else
                MsgBox(ex.Message, , thisColumn.Title)
            End If
        Else
            Me.Label.Focus()
        End If

    End Sub

#End Region

#Region "Select / Deselect"

    Private Sub Label_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label.Click

        If Not _IgnoreClick Then

            For Each uiCol As uiColumn In ParentForm.ViewMain.VisibleColumns
                With uiCol
                    If Not uiCol.Equals(Me) Then
                        If .Selected Then
                            If .HasScanBuffer Then
                                Dim ex As New Exception
                                .ProcessBuffer(ex)
                                If IsNothing(ex) Then
                                    .Deselect()
                                Else
                                    MsgBox(ex.Message, , thisColumn.Title)
                                    .Focus()
                                    Exit Sub
                                End If
                            Else
                                .Deselect()
                            End If
                        End If
                    End If
                End With
            Next

            With Me
                If .Selected Then
                    .Deselect()
                Else
                    .Focus()
                End If
            End With

        Else
            _IgnoreClick = False
        End If

    End Sub

    Private Sub uiColumn_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.GotFocus
        With Me
            .ColStyle = eColStyle.colSelected
            .Selected = True
            .Label.Focus()
        End With
    End Sub

    Public Sub Deselect()
        With Me
            If Not _mode = eMode.label Then ShowCtrl(eMode.label)
            lbl_Value.Text = _thisColumn.Value
            .ColStyle = eColStyle.colDeselected
            .Selected = False
            _ScanBuffer.Clear()
        End With
    End Sub

#End Region

#Region "Column Resizing"

    Private Sub uiColumn_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Resize
        Label.Width = Screen.PrimaryScreen.WorkingArea.Width / 3
        lbl_Value.Width = Screen.PrimaryScreen.WorkingArea.Width - Label.Width - 2
        list.Width = Screen.PrimaryScreen.WorkingArea.Width - Label.Width - 2
    End Sub

#End Region

#Region "Dummy Scans"

    Private Sub ScanBarcode(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Scan.Click

        With ScanBuffer
            .Value = InputBox("Scan Barcode")
            .is2d = False
        End With

        Dim ex As New Exception
        ex = Nothing
        Me.ProcessBuffer(ex)
        If Not IsNothing(ex) Then
            MsgBox(ex.Message, , thisColumn.Title)
        End If

    End Sub

    Private Sub Scan2dBarcode(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Scan2d.Click
        With ScanBuffer
            .Value = "<in><i n='TOWARHSNAME' v='Main'/><i n='TOLOCNAME' v='0'/></in>".Replace("'", Chr(34))
            .is2d = True
        End With

        Dim ex As New Exception
        ex = Nothing
        Me.ProcessBuffer(ex)
        If Not IsNothing(ex) Then
            MsgBox(ex.Message, , thisColumn.Title)
        End If
    End Sub

#End Region

End Class
