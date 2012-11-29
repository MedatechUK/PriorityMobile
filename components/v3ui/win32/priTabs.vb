Imports bind

Public Class priTabs

    Private h As Integer = 30
    Private _Parent As ctform = Nothing

    Private _TabPanels As New Dictionary(Of String, Panel)
    Public Property TabPanels() As Dictionary(Of String, Panel)
        Get
            Return _TabPanels
        End Get
        Set(ByVal value As Dictionary(Of String, Panel))
            _TabPanels = value
        End Set
    End Property

    Private _Font As New Font("Tahoma", 10, FontStyle.Regular)
    Public Property FormFont() As Font
        Get
            Return _Font
        End Get
        Set(ByVal value As Font)
            _Font = value
            With Me
                .TabForm.Font = _Font
                .Tabs.Font = _Font
            End With
        End Set
    End Property

    Sub Draw(ByRef Parent As ctform)
        _Parent = Parent        
        With Tabs
            With .sMenuItems
                For i As Integer = 1 To _Parent._xf.Tabs.Count
                    If Not String.Compare(_Parent._xf.Tabs.Item(i).Name, "HIDDEN", True) = 0 Then
                        .Add(.Count + 1, NewLabel(CStr(i), _Parent._xf.Tabs.Item(i).Name))
                        Dim p As New Panel
                        p.AutoScroll = True
                        p.Tag = i
                        AddHandler p.Paint, AddressOf hTabPaint
                        TabPanels.Add(_Parent._xf.Tabs.Item(i).Name, p)
                        With TabPanels(_Parent._xf.Tabs.Item(i).Name)
                            .Name = _Parent._xf.Tabs.Item(i).Name
                            .Parent = Me.TabForm
                            'If i = 1 Then
                            '    .Visible = True
                            '    .Dock = DockStyle.Fill
                            'Else
                            .Visible = False
                            .Dock = DockStyle.None
                            'End If
                            DrawTab(_Parent._xf.Tabs.Item(i), TabPanels(_Parent._xf.Tabs.Item(i).Name))
                        End With
                    End If
                Next
            End With
            .DrawMenu()           
            .form_Resize(Me, New System.EventArgs)
            hTabClick(.sMenuItems.Item(1))

            'For Each t As Panel In TabPanels.Values
            '    With t
            '        If t.Dock = DockStyle.Fill Then
            '            ResizeMe(t, New System.EventArgs)
            '        End If
            '    End With
            'Next

        End With

    End Sub

    Private Sub DrawTab(ByRef xt As xmlTab, ByRef Container As Panel)
        Try
            With Container
                Dim y As Integer = 0
                For Each f As xmlField In xt.fields.Values
                    Dim ctrl As PriBaseCtrl = newControl(f)
                    With ctrl
                        .FieldStyle = f.FieldStyle
                        .Visible = Not (f.Hidden)
                        If .Visible Then
                            .DisplayOrder = y
                            y += 1
                        End If
                        .FormFont = FormFont
                        .LabelText = f.Name
                        .Mandatory = f.Mandatory
                        .IsReadOnly = f.IsReadOnly
                        .regex = f.regex
                        .Font = FormFont
                        .Column = f.Column
                    End With
                    .Controls.Add(ctrl)
                Next
            End With
        Catch ex As Exception
            Beep()
        End Try
    End Sub

    Private Function newControl(ByVal f As xmlField) As Control

        Select Case f.FieldStyle
            Case xfFieldStyle.xfCombo
                Dim ret As New priComboBox
                With ret
                    Dim BO As oBind = _Parent._parent.DataSet(f.ListSource)
                    With .ComboBox
                        .DataBindings.Add("SelectedValue", _Parent.BindingSource, f.Column)
                        .DataSource = BO.FilterObject
                        .ValueMember = f.ListValueCol
                        .DisplayMember = f.ListTextCol
                    End With
                    .ListValueCol = f.ListValueCol
                    .ListTextCol = f.ListTextCol
                    .ListSource = f.ListSource
                    .ListFilter = f.ListFilter
                End With
                Return ret

            Case xfFieldStyle.xfDate
                Dim RET As New PriDatePick
                RET.DataBindings.Add("Value", _Parent.BindingSource, f.Column)
                Return RET

            Case xfFieldStyle.xfBool
                Dim RET As New PriTickBox
                RET.DataBindings.Add("Value", _Parent.BindingSource, f.Column)
                Return RET

            Case Else
                Dim RET As New priTextBox
                RET.DataBindings.Add("Value", _Parent.BindingSource, f.Column)
                Return RET
        End Select

    End Function

    Private Function NewLabel(ByVal Name As String, ByVal Text As String) As LinkLabel
        Dim ret As New LinkLabel
        With ret
            .Name = Name
            .Text = Text
        End With
        Return ret
    End Function

    Public Sub hTabClick(Optional ByRef MenuItem As LinkLabel = Nothing) Handles Tabs.MenuClick


        If Not IsNothing(MenuItem) Then

            For Each t As Panel In TabPanels.Values
                With t
                    If Strings.StrComp(.Name, MenuItem.Text, CompareMethod.Text) = 0 Then
                        '_Parent.BindingSource.SuspendBinding()
                        .Visible = True
                        .Dock = DockStyle.Fill
                        .SendToBack()
                        ResizeMe(t, New System.EventArgs)
                        _Parent.BindingSource.ResetBindings(True)
                    End If
                End With
            Next

            For Each t As Panel In TabPanels.Values
                With t
                    If Strings.StrComp(.Name, MenuItem.Text, CompareMethod.Text) <> 0 Then
                        .Visible = False
                        .Dock = DockStyle.None
                    Else
                        .BringToFront()
                    End If
                End With
            Next

        End If

    End Sub

    Public Sub ResizeMe(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Resize

        Dim top As Integer = 5
        Dim margin As Integer = 10
        Dim maxTop As Integer = 0

        If String.Compare(sender.GetType.ToString, "System.Windows.Forms.Panel", True) = 0 Then
            Dim p As Panel = sender
            'If p.Dock = DockStyle.Fill Then
            For Each c As PriBaseCtrl In sender.Controls
                With c
                    If .Visible Then
                        .Top = top + h * .DisplayOrder
                        .Left = margin
                        .Width = Width - (margin * 2) - 15
                        '.Label.Left = m - .Label.Width
                        '.hResize(sender, e)
                    End If
                End With
            Next
            Exit Sub
            'End If
        Else
            For Each t As Panel In TabPanels.Values
                With t
                    If t.Dock = DockStyle.Fill Then
                        ResizeMe(t, New System.EventArgs)
                    End If
                End With
            Next
        End If

    End Sub

    Private Sub hTabPaint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs)

        Dim top As Integer = 5
        Dim margin As Integer = 10
        Dim maxTop As Integer = 0
        Dim p As Panel = sender
        Dim StringSize As New SizeF

        Dim m As Integer = 0
        For Each c As PriBaseCtrl In sender.Controls
            With c
                If .Label.Width > m Then
                    m = .Label.Width + 5
                    If m > Me.Width / 3 Then
                        m = Me.Width / 3
                    End If
                End If
            End With
        Next
        For Each c As PriBaseCtrl In sender.Controls
            With c
                If .Visible Then
                    '.Top = top + h * .DisplayOrder
                    '.Left = margin
                    '.Width = Width - (margin * 2) - 15
                    If Not .Label.Left = m - .Label.Width Then
                        .Label.Left = m - .Label.Width
                        .hResize(sender, e)
                    End If
                End If
            End With
        Next

    End Sub

End Class
