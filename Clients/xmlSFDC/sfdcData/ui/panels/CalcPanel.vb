Imports System.Windows.Forms
Imports System.Drawing

Public Class CalcPanel
    Inherits iFormPanel

    Dim btn(14) As Button
    Private _cSetting As calcSetting
    Private mclosing As Boolean = True
    Private PressDot As Boolean = False

    Public Event SetNumber(ByRef cSetting As calcSetting)

#Region "Inheritance"

    Private _ParentForm As iForm
    Public Overrides ReadOnly Property ParentForm() As iForm
        Get
            Return _ParentForm
        End Get
    End Property

#End Region

#Region "Public Properties"

    Private _txt_Number As New TextBox
    Public Property txt_Number() As TextBox
        Get
            Return _txt_Number
        End Get
        Set(ByVal value As TextBox)
            _txt_Number = value
        End Set
    End Property

#End Region

#Region "Initialisation and finalisation"

    Sub New()

        With txt_Number
            AddHandler .KeyDown, AddressOf hKeyDown
            AddHandler .GotFocus, AddressOf txt_Number_GotFocus
        End With

        With Me.Controls
            .Add(txt_Number)
            With .Item(.Count - 1)
                .Dock = DockStyle.Top
            End With
        End With

        For i As Integer = 0 To 14
            btn(i) = New Button
            btn(i).Width = Me.Width / 3
            btn(i).Height = (Me.Height - txt_Number.Height) / 5
            Select Case i
                Case 0, 1, 2, 3, 4, 5, 6, 7, 8
                    btn(i).Text = CStr(i + 1)
                Case 9
                    btn(i).Text = "."
                Case 10
                    btn(i).Text = "0"
                Case 11
                    btn(i).Text = "-"
                Case 12
                    btn(i).Text = "<"
                Case 13
                    btn(i).Text = "Ok"
                Case 14
                    btn(i).Text = "X"

            End Select
            AddHandler btn(i).Click, AddressOf Btn_Click
            AddHandler btn(i).KeyDown, AddressOf hKeyDown
            Me.Controls.Add(btn(i))
        Next

    End Sub

    Public Sub Load(ByRef thisForm As iForm)
        _ParentForm = thisForm
    End Sub

    Public Sub InitSetting(ByRef cSetting As calcSetting)

        _cSetting = cSetting

        btn(9).Enabled = _cSetting.permitDouble
        btn(11).Enabled = _cSetting.permitNeg

        If Not (String.Compare(_cSetting.FormTitle, String.Empty) = 0) Then
            Dim mnu As New ContextMenu
            Dim mi As New MenuItem
            mi.Text = _cSetting.FormTitle
            mnu.MenuItems.Add(mi)
            Me.txt_Number.ContextMenu = mnu
        Else
            Me.txt_Number.ContextMenu = Nothing
        End If

        Me.txt_Number.Text = _cSetting.DNUM.ToString
        Me.btn(9).Focus()
    End Sub

#End Region

#Region "Event Handlers"

    Private Sub Btn_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim btn As Button = sender
        Select Case Strings.Left(btn.Text, 1)
            Case "O"
                PressDot = False
                mclosing = True
                RaiseEvent SetNumber(_cSetting)
            Case "C"
                PressDot = False
                _cSetting.DNUM = 0
            Case "<"
                PressDot = False
                If Len(_cSetting.DNUM.ToString) > 1 Then
                    Try
                        _cSetting.DNUM = CDbl(Strings.Left(_cSetting.DNUM.ToString, Len(_cSetting.DNUM.ToString) - 1))
                    Catch
                        _cSetting.DNUM = 0
                    End Try
                Else
                    _cSetting.DNUM = 0
                End If
            Case "."
                If InStr(txt_Number.Text, ".") = 0 Then
                    PressDot = True
                    txt_Number.Text = _cSetting.DNUM.ToString & "."
                End If
            Case "X"
                _cSetting.Result = DialogResult.Cancel
                PressDot = False
                mclosing = True
                RaiseEvent SetNumber(_cSetting)
            Case "-"
                Try
                    _cSetting.DNUM = _cSetting.DNUM - (_cSetting.DNUM * 2)
                Catch
                    _cSetting.DNUM = 0
                End Try
            Case Else
                If PressDot Then
                    _cSetting.DNUM = CDbl(_cSetting.DNUM.ToString & "." & btn.Text)
                    PressDot = False
                Else
                    _cSetting.DNUM = CDbl(_cSetting.DNUM.ToString & btn.Text)
                End If
        End Select

        If Not IsNumeric(_cSetting.DNUM) Then
            _cSetting.DNUM = 0
        Else
            If _cSetting.DNUM > _cSetting.Max Then _cSetting.DNUM = _cSetting.Max
            If _cSetting.DNUM < _cSetting.Min Then _cSetting.DNUM = _cSetting.Min
        End If

        txt_Number.Text = _cSetting.DNUM.ToString

        Me.btn(9).Focus()

    End Sub

    Private Sub txt_Number_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            If ParentForm.View = iForm.eiFromView.ViewCalc Then
                Me.btn(9).Focus()
            End If
        Catch
        End Try
    End Sub

    Private Sub hKeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        e.Handled = True
        Select Case e.KeyValue
            Case Keys.Back
                Btn_Click(Me.btn(12), New System.EventArgs)
            Case Keys.D0
                Btn_Click(Me.btn(10), New System.EventArgs)
            Case Keys.D1
                Btn_Click(Me.btn(0), New System.EventArgs)
            Case Keys.D2
                Btn_Click(Me.btn(1), New System.EventArgs)
            Case Keys.D3
                Btn_Click(Me.btn(2), New System.EventArgs)
            Case Keys.D4
                Btn_Click(Me.btn(3), New System.EventArgs)
            Case Keys.D5
                Btn_Click(Me.btn(4), New System.EventArgs)
            Case Keys.D6
                Btn_Click(Me.btn(5), New System.EventArgs)
            Case Keys.D7
                Btn_Click(Me.btn(6), New System.EventArgs)
            Case Keys.D8
                Btn_Click(Me.btn(7), New System.EventArgs)
            Case Keys.D9
                Btn_Click(Me.btn(8), New System.EventArgs)
            Case 190
                Btn_Click(Me.btn(9), New System.EventArgs)
            Case Keys.Subtract
                Btn_Click(Me.btn(11), New System.EventArgs)
        End Select
    End Sub

#End Region

#Region "Resizing"

    Private Sub ct_number_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize

        'If mclosing Then Exit Sub
        Dim fs As Single

        If Me.Width > 319 Then
            fs = 14
        ElseIf Me.Width > 268 Then
            fs = 12
        ElseIf Me.Width > 258 Then
            fs = 11
        ElseIf Me.Width > 241 Then
            fs = 10
        ElseIf Me.Width > 214 Then
            fs = 9
        ElseIf Me.Width > 199 Then
            fs = 8
        Else
            fs = 8
        End If

        Dim f As New Font("Microsoft Sans Serif", fs + 2, FontStyle.Bold)
        Dim c As New Font("Microsoft Sans Serif", fs + 4, FontStyle.Regular)

        txt_Number.Font = c

        Dim i As Integer = 0
        For y As Integer = 0 To 4
            For x As Integer = 0 To 2
                If IsNothing(btn(i)) Then
                    btn(i) = New Button
                End If
                btn(i).Width = Me.Width / 3
                btn(i).Height = (Me.Height - txt_Number.Height) / 5
                btn(i).Font = f
                btn(i).Top = (txt_Number.Top + txt_Number.Height) + (y * btn(i).Height)
                btn(i).Left = x * btn(i).Width
                i = i + 1
            Next
        Next

    End Sub

#End Region

End Class
