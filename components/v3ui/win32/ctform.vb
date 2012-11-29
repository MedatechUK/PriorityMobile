Imports bind
Imports System.Windows.Forms
Imports System.Text.RegularExpressions

Public Enum eFormView
    view_Form = 1
    view_Table = 2
    view_HTML = 3
    view_Signature = 4    
End Enum

Public Class ctform

    Public Event OpenSubform(ByVal Name As String)
    Public Event SetView(ByVal FormView As eFormView)
    Public Event SetReadOnly(ByVal IsReadOnly As Boolean)

    Private _LoadedForms() As ctform
    Public _parent As oDataSet
    Private _formOrdinal As Integer

    Public Sub FormReference(ByRef Forms() As ctform, ByVal Ordinal As Integer)
        _LoadedForms = Forms
        _formOrdinal = Ordinal
    End Sub

    Public Sub New(ByRef Parent As oDataSet)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        _parent = Parent

    End Sub

    Public _xf As xmlForm = Nothing

#Region "Properties"

    Private _Font As New Font("Tahoma", 10, FontStyle.Regular)
    Public Property FormFont() As Font
        Get
            Return _Font
        End Get
        Set(ByVal value As Font)
            _Font = value
            With Me
                .TabForm.formFont = _Font
                .SubForms.Font = _Font
                .UserText.Font = _Font
            End With
        End Set
    End Property

    Private _formView As eFormView
    Public Property FormView() As eFormView
        Get
            Return _formView
        End Get
        Set(ByVal value As eFormView)
            _formView = value
            hideall()
            With Me
                Select Case _formView
                    Case eFormView.view_Form
                        For Each t As Panel In .TabForm.TabPanels.Values
                            With t
                                For Each pc As PriBaseCtrl In t.Controls
                                    pc.Active = True
                                Next
                            End With
                        Next
                        With .TabForm
                            .Dock = DockStyle.Fill
                            .Visible = True
                        End With
                    Case eFormView.view_Table
                        For Each t As Panel In .TabForm.TabPanels.Values
                            With t
                                For Each pc As PriBaseCtrl In t.Controls
                                    pc.Active = False
                                Next
                            End With
                        Next
                        With .DataGrid
                            .Dock = DockStyle.Fill
                            .Visible = True
                        End With
                    Case eFormView.view_Signature
                        With .PriSign
                            .Dock = DockStyle.Fill
                            .Visible = True
                        End With
                    Case eFormView.view_HTML
                        Select Case ColumnIsReadOnly("TEXT")
                            Case True
                                With .WebBrowser
                                    .Dock = DockStyle.Fill
                                    .Visible = True
                                End With
                            Case Else
                                With .UserText
                                    .Dock = DockStyle.Fill
                                    .Visible = True
                                End With
                        End Select
                End Select

                Select Case _formView
                    Case eFormView.view_Table, eFormView.view_Form
                        With .DataGrid
                            If Not IsNothing(_xf) Then
                                'If _xf.CurrentView = xfView.xfTable Then
                                'If .Columns.Count > 0 Then
                                '    .Columns("Key").Visible = False
                                '    For Each t As xmlTab In _xf.Tabs.Values
                                '        For Each f As xmlField In t.fields.Values
                                '            .Columns(f.Column).Visible = Not (f.Hidden)
                                '            If Not (f.Hidden) Then
                                '                .Columns(f.Column).HeaderText = f.Name
                                '            End If
                                '        Next
                                '    Next
                                'End If
                                'End If
                                Dim ts As New DataGridTableStyle
                                ts.MappingName = _xf.SQLFrom
                                For Each t As xmlTab In _xf.Tabs.Values
                                    For Each f As xmlField In t.fields.Values
                                        Select Case f.FieldStyle
                                            Case xfFieldStyle.xfDate
                                                Dim cstb As New PriDateColumn
                                                With cstb
                                                    .MappingName = f.Column
                                                    .HeaderText = f.Name
                                                    If f.Hidden Then
                                                        .Width = 0
                                                    End If
                                                End With
                                                Try
                                                    ts.GridColumnStyles.Add(cstb)
                                                Catch
                                                End Try
                                            Case Else
                                                Dim cstb As New DataGridTextBoxColumn
                                                With cstb
                                                    .MappingName = f.Column
                                                    .HeaderText = f.Name
                                                    If f.Hidden Then
                                                        .Width = 0
                                                    End If
                                                End With
                                                Try
                                                    ts.GridColumnStyles.Add(cstb)
                                                Catch
                                                End Try
                                        End Select
                                        '.Columns(f.Column).Visible = Not (f.Hidden)
                                        'If Not (f.Hidden) Then
                                        '    .Columns(f.Column).HeaderText = f.Name
                                        'End If
                                    Next
                                Next
                                .TableStyles.Clear()
                                .TableStyles.Add(ts)
                            End If
                        End With
                End Select
            End With
        End Set
    End Property

    Private _Adding As Boolean = False
    Public Property Adding() As Boolean
        Get
            Return _Adding
        End Get
        Set(ByVal value As Boolean)
            _Adding = value
        End Set
    End Property

#End Region

    Public Sub DrawForm(ByRef xf As xmlForm)
        With Me
            _xf = xf
            Dim BO As oBind = _parent.DataSet(_xf.SQLFrom)
            .BindingSource.DataSource = BO.FilterObject

            .BindingSource.SuspendBinding()

            .FormView = _xf.DefaultView

            Select Case ._formView
                Case eFormView.view_Form, eFormView.view_Table
                    TabForm.Draw(Me)
                    TabForm.ResizeMe(Me, New System.EventArgs)

                Case eFormView.view_HTML
                    Select Case ColumnIsReadOnly("TEXT")
                        Case True
                            ' Draw browser window
                            Dim DOC As String = Me.getData(":$$.DOCNO")
                            Dim ID As Integer = BO.ContainsKey(DOC)
                            If ID > -1 Then
                                Dim a() As String = Split(BO.Item(ID).ToString, Chr(9))
                                With WebBrowser
                                    .DocumentText = a(1)
                                End With
                            End If

                        Case Else
                            ' Draw notepad window
                            Dim DOC As String = Me.getData(":$$.DOCNO")
                            Dim ID As Integer = BO.ContainsKey(DOC)
                            If ID > -1 Then
                                Dim a() As String = Split(BO.Item(ID).ToString, Chr(9))
                                With UserText
                                    .Text = Replace(a(1), "\n", vbCrLf)
                                End With
                            End If

                    End Select

                Case eFormView.view_Signature
                    ' Draw gdi surface
                    Dim DOC As String = Me.getData(":$$.DOCNO")
                    Dim ID As Integer = BO.ContainsKey(DOC)
                    If ID > -1 Then
                        Dim a() As String = Split(BO.Item(ID).ToString, Chr(9))
                        With PriSign
                            .coord = .UnpackSignature(a(1))
                            .ASTEXT.Text = a(2)
                            .Sign.Invalidate()
                        End With
                    End If

            End Select

            With SubForms
                With .sMenuItems
                    For i As Integer = 1 To _xf.SubForm.Count
                        .Add(.Count + 1, NewLabel(CStr(i), _xf.SubForm.Item(i).Name))
                    Next
                End With
                .DrawMenu()
                .form_Resize(Me, New System.EventArgs)
            End With

            Try
                .BindingSource.Filter = ParseFilter(_xf.Filter)
                .DataGrid.DataSource = .BindingSource
                .BindingSource.ResumeBinding()
                .BindingSource.MoveFirst()

            Catch ex As Exception
                MsgBox(ex.Message)
            End Try

            RaiseEvent SetView(.FormView)
            Me.FormView = .FormView
            RaiseEvent SetReadOnly(_xf.IsReadOnly)
        End With

    End Sub

    Private Function ColumnIsReadOnly(ByVal Column As String) As Boolean
        For Each t As xmlTab In _xf.Tabs.Values
            For Each f As xmlField In t.fields.Values
                If f.Name = Column Then
                    Return f.IsReadOnly
                End If
            Next
        Next
    End Function

    Private Function NewLabel(ByVal Name As String, ByVal Text As String) As LinkLabel
        Dim ret As New LinkLabel
        With ret
            .Name = Name
            .Text = Text
        End With
        Return ret
    End Function

#Region "Filter"

    Private Function ParseFilter(ByVal t As String) As String
        Dim ret As String = ""
        If Not IsNothing(t) Then
            ret = t
            Dim myMatches As MatchCollection
            Dim myRegex As New Regex(":[$]+\.[a-zA-Z]+")
            myMatches = myRegex.Matches(t)
            For Each successfulMatch As Match In myMatches
                ret = Replace(t, successfulMatch.Value, getData(successfulMatch.Value))
            Next
        End If
        Return ret
    End Function

    Public Function getData(ByVal str As String) As String
        Dim c As Integer = -1
        Dim x As Integer = 1
        Do
            If Mid(str, x, 1) = "$" Then
                c += 1
            End If
            x += 1
        Loop While Mid(str, x, 1) <> "."

        Dim pi As System.Reflection.PropertyInfo = _
            _LoadedForms(_formOrdinal - c).BindingSource.Current.GetType().GetProperty(Split(str, ".")(1))
        If Not IsNothing(pi) Then
            Return pi.GetValue(_LoadedForms(_formOrdinal - c).BindingSource.Current, Nothing)
        Else
            Return ""
        End If

    End Function

    Public Sub SetData(ByVal str As String, ByVal NewVal As String)
        Dim c As Integer = -1
        Dim x As Integer = 1
        Do
            If Mid(str, x, 1) = "$" Then
                c += 1
            End If
            x += 1
        Loop While Mid(str, x, 1) <> "."

        Dim fld As String = Split(str, ".")(1)
        With _LoadedForms(_formOrdinal - c)
            For Each t As Panel In .TabForm.TabPanels.Values
                For Each b As PriBaseCtrl In t.Controls
                    If String.Compare(b.Column, fld) = 0 Then
                        b.Value = NewVal
                    End If
                Next
            Next
            For l As Integer = 0 To .DataGrid.TableStyles(0).GridColumnStyles.Count - 1
                Dim st As DataGridColumnStyle = .DataGrid.TableStyles(0).GridColumnStyles(l)
                If String.Compare(st.MappingName, fld) = 0 Then
                    .DataGrid.Item(.DataGrid.CurrentRowIndex, l) = NewVal
                End If
            Next
        End With
    End Sub

#End Region

#Region "click handlers"

    Private Sub hSFClick(ByRef MenuItem As LinkLabel) Handles SubForms.MenuClick
        RaiseEvent OpenSubform(MenuItem.Text)
    End Sub

#End Region

#Region "resize code"

    Private Sub hideall()
        With Me
            With .TabForm
                If .Visible Then
                    .Dock = DockStyle.None
                    .Top = -1
                    .Left = -1
                    .Width = 0
                    .Height = 0
                    .Visible = False
                End If
            End With
            With .DataGrid
                If .Visible Then
                    .Dock = DockStyle.None
                    .Top = -1
                    .Left = -1
                    .Width = 0
                    .Height = 0
                    .Visible = False
                End If
            End With
            With .UserText
                If .Visible Then
                    .Dock = DockStyle.None
                    .Top = -1
                    .Left = -1
                    .Width = 0
                    .Height = 0
                    .Visible = False
                End If
            End With
            With .WebBrowser
                If .Visible Then
                    .Dock = DockStyle.None
                    .Top = -1
                    .Left = -1
                    .Width = 0
                    .Height = 0
                    .Visible = False
                End If
            End With
            With PriSign
                If .Visible Then
                    .Dock = DockStyle.None
                    .Top = -1
                    .Left = -1
                    .Width = 0
                    .Height = 0
                    .Visible = False
                End If
            End With
        End With
    End Sub

#End Region

    Public Sub Close()
        Select Case Me.FormView
            Case eFormView.view_Signature
                Dim bo As oBind = _parent.DataSet(_xf.SQLFrom)
                Dim DOC As String = getData(":$$.DOCNO")
                With bo
                    Dim ID As Integer = .ContainsKey(DOC)
                    If ID > -1 Then
                        .Value(.Item(ID), "SIGDATA") = PriSign.CompressSignature
                        .Value(.Item(ID), "ASTEXT") = PriSign.ASTEXT.Text

                    Else
                        bo.Add(DOC & Chr(9) & PriSign.CompressSignature & Chr(9) & PriSign.ASTEXT.Text)
                    End If
                    .Save()
                End With

            Case eFormView.view_HTML
                If Not ColumnIsReadOnly("TEXT") Then
                    Dim bo As oBind = _parent.DataSet(_xf.SQLFrom)
                    Dim DOC As String = getData(":$$.DOCNO")
                    With bo
                        Dim ID As Integer = .ContainsKey(DOC)
                        If ID > -1 Then
                            .Value(.Item(ID), "TEXT") = UserText.Text
                        Else
                            bo.Add(DOC & Chr(9) & Replace(UserText.Text, vbCrLf, "\n"))
                        End If
                        .Save()
                    End With
                End If
        End Select

        MyBase.Finalize()
    End Sub

    Private Sub BindingSource_CurrentItemChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles BindingSource.CurrentItemChanged
        Dim bo As oBind = _parent.DataSet(_xf.SQLFrom)
        bo.Save()
    End Sub

    Public Function HasMandatory() As Boolean
        For Each p As Panel In Me.TabForm.TabPanels.Values
            For Each c As PriBaseCtrl In p.Controls
                If c.Mandatory And c.Value.Length = 0 Then
                    MsgBox(String.Format("Missing mandatory field: {0}", c.LabelText))
                    p.Dock = DockStyle.Fill
                    c.Focus()
                    Return False
                End If
            Next
        Next
        Return True
    End Function

    Public Function HasRegex() As Boolean
        For Each p As Panel In Me.TabForm.TabPanels.Values
            For Each c As PriBaseCtrl In p.Controls
                If c.regex.Length > 0 And c.Value.Length > 0 Then
                    If Not System.Text.RegularExpressions.Regex.IsMatch(c.Value, c.regex) Then
                        MsgBox(String.Format("Invalid entry for {0}: '{1}'", c.Name, c.Value))
                        c.Focus()
                        Return False
                    End If
                End If
            Next
        Next
        Return True
    End Function

    Public Function NewObject(ByRef o As Object) As Boolean
        Dim cancel As Boolean = False
        For Each p As Panel In Me.TabForm.TabPanels.Values
            For Each c As PriBaseCtrl In p.Controls
                Dim pi As System.Reflection.PropertyInfo = o.GetType.GetProperty(c.Column)
                Select Case pi.PropertyType.ToString.ToLower
                    Case "system.string"
                        pi.SetValue(o, CStr(c.Value), Nothing)
                    Case "system.int32"
                        pi.SetValue(o, CInt(c.Value), Nothing)
                End Select
            Next
        Next
        Dim bo As oBind = _parent.DataSet(_xf.SQLFrom)
        bo.Trigger(tTrigger.PREINSERT, o, , cancel)
        Return cancel
    End Function

    Public Overloads Sub REFRESH()
        If FormView = eFormView.view_Form Then
            If Not BindingSource.Position = -1 Then
                Dim o As Object = BindingSource.Current
                For Each p As Panel In Me.TabForm.TabPanels.Values
                    For Each c As PriBaseCtrl In p.Controls
                        Dim pi As System.Reflection.PropertyInfo = o.GetType.GetProperty(c.Column)
                        c.ControlText = pi.GetValue(o, Nothing)
                    Next
                Next
            End If
        End If
    End Sub

    Private Sub DataGrid_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs)

    End Sub

    Private Sub DataGrid_Navigate(ByVal sender As System.Object, ByVal ne As System.Windows.Forms.NavigateEventArgs)

    End Sub
End Class
