Imports bind
Imports System.xml

Public Class priTabs

    Public Event LoadingTabColumn(ByVal tab As String, ByVal ColumnName As String)

    Private h As Integer = 30
    Private myParent As ctform = Nothing
    Private TabPanels As New Dictionary(Of String, Panel)

    Sub Add(ByVal k As String, ByVal p As Panel)
        TabPanels.Add(k, p)
    End Sub

    Function Count() As Integer
        Return TabPanels.Count
    End Function

    Sub Clear()
        TabPanels.Clear()
    End Sub

    Function Item(ByVal k As String) As Panel
        Return TabPanels.Item(k)
    End Function

    Function Values() As System.Collections.Generic.Dictionary(Of String, System.Windows.Forms.Panel).ValueCollection
        Return TabPanels.Values
    End Function

    Private _Font As New Font("Tahoma", 10, FontStyle.Regular)
    Public Overrides Property Font() As Font
        Get
            Return _Font
        End Get
        Set(ByVal value As Font)
            _Font = value
            With Me
                '.TabForm.Font = _Font
                .tabs.Font = _Font
            End With
        End Set
    End Property

    Sub Draw(ByRef tabNodes As XmlNodeList)
        myParent = Parent
        With tabs
            'With .sMenuItems
            For Each tab As XmlNode In tabNodes
                If Not String.Compare(tab.Attributes("name").Value, "hidden", True) = 0 Then
                    .Add(tab.Attributes("name").Value)
                    Dim p As New Panel
                    p.AutoScroll = True
                    p.Tag = tab.Attributes("name").Value
                    'AddHandler p.Paint, AddressOf hTabPaint
                    TabPanels.Add(p.Tag, p)
                    With TabPanels(p.Tag)
                        .Name = p.Tag
                        .Parent = Me.TabForm
                        'If i = 1 Then
                        '    .Visible = True
                        '    .Dock = DockStyle.Fill
                        'Else
                        .Visible = False
                        .Dock = DockStyle.None
                        'End If
                        DrawTab(tab, TabPanels(p.Tag))
                    End With
                End If
            Next
            'End With

            .form_Resize(Me, New System.EventArgs)
            'hTabClick(.Item(1))

            'For Each t As Panel In TabPanels.Values
            '    With t
            '        If t.Dock = DockStyle.Fill Then
            '            ResizeMe(t, New System.EventArgs)
            '        End If
            '    End With
            'Next

        End With

    End Sub

    Private Sub DrawTab(ByRef tabNode As XmlNode, ByRef Container As Panel)
        Try
            With Container
                Dim y As Integer = 0
                For Each f As XmlNode In tabNode.SelectNodes("field")
                    RaiseEvent LoadingTabColumn(tabNode.Attributes("name").Value, f.Attributes("name").Value)
                    Dim ctrl As PriBaseCtrl = newControl(f)
                    With ctrl
                        '.FieldStyle = f.FieldStyle
                        '.Visible = Not (f.Hidden)
                        If .Visible Then
                            .DisplayOrder = y
                            y += 1
                        End If
                        '.Font = Font
                        '.LabelText = f.Name
                        '.Mandatory = f.Mandatory
                        '.IsReadOnly = f.IsReadOnly
                        '.regex = f.regex                        
                        '.Column = f.Column
                    End With
                    .Controls.Add(ctrl)
                Next
            End With
        Catch ex As Exception
            Beep()
        End Try
    End Sub

    Private Function newControl(ByRef f As XmlNode) As Control

        Select Case LCase(f.Attributes("fieldstyle").Value)            
            Case "list", "combo"
                Return New priComboBox(myParent, f)
            Case "date", "time", "datetime"
                Return New PriDatePick(myParent, f)
            Case "tick", "bool", "check"
                Return New PriTickBox(myParent, f)
            Case Else
                Return New priTextBox(myParent, f)
        End Select

        '    Case xfFieldStyle.xfCombo
        '        Return New priComboBox(myParent, f)
        '        'With ret
        '        '    Dim BO As oBind = myParent.MyParent.DataSet(f.ListSource)
        '        '    With .ComboBox
        '        '        .DataBindings.Add("SelectedValue", myParent.BindingSource, f.Column)
        '        '        .DataSource = BO.FilterObject
        '        '        .ValueMember = f.ListValueCol
        '        '        .DisplayMember = f.ListTextCol
        '        '    End With
        '        '    .ListValueCol = f.ListValueCol
        '        '    .ListTextCol = f.ListTextCol
        '        '    .ListSource = f.ListSource
        '        '    .ListFilter = f.ListFilter
        '        'End With
        '        'Return ret

        '    Case xfFieldStyle.xfDate
        '        Return New PriDatePick(myParent, f)
        '        'RET.DataBindings.Add("Value", myParent.BindingSource, f.Column)
        '        'Return RET

        '    Case xfFieldStyle.xfBool
        '        Return New PriTickBox(myParent, f)
        '        'RET.DataBindings.Add("Value", myParent.BindingSource, f.Column)
        '        'Return RET

        '    Case Else
        '        Return New priTextBox(myParent, f)
        '        'RET.DataBindings.Add("Value", myParent.BindingSource, f.Column)
        '        'Return RET
        'End Select

    End Function

    Public Sub hTabClick(Optional ByRef MenuItem As String = Nothing) Handles tabs.MenuClick

        If Not IsNothing(MenuItem) Then

            For Each t As Panel In TabPanels.Values
                With t
                    If Strings.StrComp(.Name, MenuItem, CompareMethod.Text) = 0 Then
                        'myParent.BindingSource.SuspendBinding()
                        .Visible = True
                        .Dock = DockStyle.Fill
                        .SendToBack()
                        ResizeMe(t, New System.EventArgs)                        
                    End If
                End With
            Next

            For Each t As Panel In TabPanels.Values
                With t
                    If Strings.StrComp(.Name, MenuItem, CompareMethod.Text) <> 0 Then
                        .Visible = False
                        .Dock = DockStyle.None
                    Else
                        .BringToFront()
                    End If
                End With
            Next

        Else

            For Each t As Panel In TabPanels.Values
                hTabClick(t.Name)
                Exit For
            Next

        END IF

    End Sub

    Public Sub ResizeMe(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Resize

        Dim top As Integer = 5
        Dim margin As Integer = 10
        Dim maxTop As Integer = 0

        TabForm.Height = Height - 20

        If String.Compare(sender.GetType.ToString, "System.Windows.Forms.Panel", True) = 0 Then
            Dim p As Panel = sender
            'If p.Dock = DockStyle.Fill Then
            For Each c As PriBaseCtrl In p.Controls
                With c
                    If .Visible Then
                        .Top = top + h * .DisplayOrder
                        .Height = h
                        .Left = 5
                        .Width = Width - 20
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

    'Private Sub hTabPaint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs)

    '    Dim top As Integer = 5
    '    Dim margin As Integer = 10
    '    Dim maxTop As Integer = 0
    '    Dim p As Panel = sender
    '    Dim StringSize As New SizeF

    '    Dim m As Integer = 0        
    '    For Each c As PriBaseCtrl In p.Controls
    '        With c
    '            If .Label.Width > m Then
    '                m = .Label.Width + 5
    '                If m > Me.Width / 3 Then
    '                    m = Me.Width / 3
    '                End If
    '            End If
    '        End With
    '    Next
    '    For Each c As PriBaseCtrl In p.Controls
    '        With c
    '            If .Visible Then
    '                '.Top = top + h * .DisplayOrder
    '                '.Left = margin
    '                '.Width = Width - (margin * 2) - 15
    '                If Not .Label.Left = m - .Label.Width Then
    '                    .Label.Left = m - .Label.Width
    '                    .hResize(sender, e)
    '                End If
    '            End If
    '        End With
    '    Next

    'End Sub

End Class
