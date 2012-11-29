Imports bind
Imports System.Windows.Forms
Imports System.Text.RegularExpressions
Imports System.Xml

Public Enum eFormView
    view_Form = 1
    view_Table = 2
    view_HTML = 3
    view_Signature = 4
    view_DateSel = 5
End Enum

Public Class ctform

    Public Event OpenSubform(ByVal Name As XmlNode)
    Public Event SetView(ByVal FormView As eFormView)
    Public Event SetReadOnly(ByVal IsReadOnly As Boolean)
    Public Event PreLoadField(ByVal FormName As String, ByVal TabName As String, ByVal ColumnName As String)

    Private _LoadedForms() As ctform
    Public MyDataSet As oDataSet
    Private _formOrdinal As Integer
    Private bo As oBind

#Region "Initialisation"

    Public Sub New(ByRef form As XmlNode, ByRef DataSet As oDataSet) ', , ByRef LoadedForms() As ctform

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        _XmlMe = form
        MyDataSet = DataSet
        FormName = _XmlMe.Attributes("name").Value
        bo = MyDataSet.DataSet(_XmlMe.Attributes("from").Value)
        Me.BindingSource.DataSource = bo.FilterObject        

    End Sub

#End Region

#Region "Properties"

    Private _XmlMe As XmlNode
    Public ReadOnly Property XML() As XmlNode
        Get
            Return _XmlMe
        End Get
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
                .SubForms.Font = _Font
                .UserText.Font = _Font
            End With
        End Set
    End Property

    Private _defaultView As eFormView
    Public Property Defaultview() As eFormView
        Get
            Return _defaultView
        End Get
        Set(ByVal value As eFormView)
            _defaultView = value
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

                With .BindingSource
                    .SuspendBinding()
                End With

                Select Case _formView
                    Case eFormView.view_Form
                        For Each t As Panel In .TabForm.Values
                            With t
                                For Each pc As PriBaseCtrl In t.Controls
                                    With pc
                                        .Active = True
                                    End With
                                Next
                            End With
                        Next
                        With .TabForm
                            .Dock = DockStyle.Fill
                            .Visible = True
                        End With
                        .BindingSource.Filter = ParseFilter(_XmlMe.Attributes("where").Value)

                    Case eFormView.view_Table
                        For Each t As Panel In .TabForm.Values
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
                        .BindingSource.Filter = ParseFilter(_XmlMe.Attributes("where").Value)

                    Case eFormView.view_DateSel
                        For Each t As Panel In .TabForm.Values
                            With t
                                For Each pc As PriBaseCtrl In t.Controls
                                    pc.Active = False
                                Next
                            End With
                        Next
                        With .PriDateSel
                            .Dock = DockStyle.Fill
                            .Visible = True
                        End With

                        .BindingSource.Filter = String.Format("[{0}]>'{1}' AND [{0}]<'{2}'", _
                            _XmlMe.Attributes("datecol").Value, _
                            PriDateSel.FromMin, _
                            PriDateSel.ToMin _
                        )

                    Case eFormView.view_Signature
                        With .PriSign
                            .Dock = DockStyle.Fill
                            .Visible = True
                        End With

                        ' Draw gdi surface
                        Dim DOC As String = Me.getData(":$$.DOCNO")
                        Dim ID As Integer = bo.ContainsKey(DOC)
                        If ID > -1 Then
                            Dim a() As String = Split(bo.Item(ID).ToString, Chr(9))
                            With PriSign
                                .coord = .UnpackSignature(a(1))
                                .ASTEXT.Text = a(2)
                                .Sign.Invalidate()
                            End With
                        End If

                    Case eFormView.view_HTML
                        Select Case CBool(_XmlMe.SelectSingleNode("tab/field[@name='TEXT']").Attributes("readonly").Value)
                            Case True
                                With .WebBrowser
                                    .Dock = DockStyle.Fill
                                    .Visible = True
                                End With
                                ' Draw browser window
                                Dim DOC As String = Me.getData(":$$.DOCNO")
                                Dim ID As Integer = bo.ContainsKey(DOC)
                                If ID > -1 Then
                                    Dim a() As String = Split(bo.Item(ID).ToString, Chr(9))
                                    With WebBrowser
                                        .DocumentText = a(1)
                                    End With
                                End If

                            Case Else
                                With .UserText
                                    .Dock = DockStyle.Fill
                                    .Visible = True
                                End With
                                ' Draw notepad window
                                Dim DOC As String = Me.getData(":$$.DOCNO")
                                Dim ID As Integer = bo.ContainsKey(DOC)
                                If ID > -1 Then
                                    Dim a() As String = Split(bo.Item(ID).ToString, Chr(9))
                                    With UserText
                                        .Text = Replace(a(1), "\n", vbCrLf)
                                    End With
                                End If
                        End Select

                End Select

                'Select Case _formView
                '    Case eFormView.view_Table, eFormView.view_Form
                '        GridFormat(.DataGrid)

                '    Case eFormView.view_DateSel
                '        GridFormat(.PriDateSel.DataGrid)
                'End Select

                Try
                    With .BindingSource
                        .ResumeBinding()
                        .ResetBindings(False)
                    End With

                Catch ex As Exception
                    MsgBox(ex.Message)
                End Try

            End With

            RaiseEvent SetView(Me.FormView)

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

    Private _FormName As String = ""
    Public Property FormName() As String
        Get
            Return _FormName
        End Get
        Set(ByVal value As String)
            _FormName = value
        End Set
    End Property

    Public ReadOnly Property AllowedViews() As List(Of eFormView)
        Get
            Dim l As New List(Of eFormView)
            With l
                Select Case Defaultview
                    Case eFormView.view_Form, eFormView.view_Table, eFormView.view_DateSel
                        .Add(eFormView.view_Form)
                        .Add(eFormView.view_Table)
                        Try
                            Dim test As String = _XmlMe.Attributes("datecol").Value
                            .Add(eFormView.view_DateSel)
                        Catch
                        End Try
                    Case eFormView.view_HTML
                        .Add(eFormView.view_HTML)
                    Case eFormView.view_Signature
                        .Add(eFormView.view_Signature)
                End Select
            End With
            Return l
        End Get
    End Property

#End Region

    Private Sub GridFormat(ByVal DataGrid As DataGrid)
        With DataGrid
            Dim ts As New DataGridTableStyle
            ts.MappingName = _XmlMe.Attributes("from").Value
            For Each t As XmlNode In _XmlMe.SelectNodes("tab")
                For Each f As XmlNode In t.SelectNodes("field")
                    Select Case LCase(f.Attributes("fieldstyle").Value)
                        Case "date"
                            Dim cstb As New PriDateColumn
                            With cstb
                                .MappingName = f.InnerXml
                                .HeaderText = f.Attributes("name").Value
                                If CBool(f.Attributes("hidden").Value) Then
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
                                .MappingName = f.InnerXml
                                .HeaderText = f.Attributes("name").Value
                                If CBool(f.Attributes("hidden").Value) Then
                                    .Width = 0
                                End If
                            End With
                            Try
                                ts.GridColumnStyles.Add(cstb)
                            Catch
                            End Try
                    End Select
                Next
            Next
            .TableStyles.Clear()
            .TableStyles.Add(ts)
        End With
    End Sub

    Private Sub TabForm_LoadingTabColumn(ByVal tab As String, ByVal ColumnName As String) Handles TabForm.LoadingTabColumn
        RaiseEvent PreLoadField(FormName, tab, ColumnName)
    End Sub

    Public Sub DrawForm()
        With Me

            Select Case LCase(_XmlMe.Attributes("defaultview").Value)
                Case "form"
                    .Defaultview = xfView.xfForm
                Case "table"
                    .Defaultview = xfView.xfTable
                Case "html"
                    .Defaultview = xfView.xfHtml
                Case "signature"
                    .Defaultview = xfView.xfSignature
                Case "datesel"
                    .Defaultview = xfView.xfDateSel
            End Select

            Select Case .Defaultview
                Case eFormView.view_Form, eFormView.view_Table, eFormView.view_DateSel

                    With TabForm
                        .Draw(_XmlMe.SelectNodes("tab"))
                        .ResizeMe(Me, New System.EventArgs)
                    End With

                    GridFormat(.DataGrid)
                    GridFormat(.PriDateSel.DataGrid)

                    .DataGrid.DataSource = .BindingSource
                    .PriDateSel.DataGrid.DataSource = .BindingSource

                Case Else
                    For Each t As XmlNode In _XmlMe.SelectNodes("tab")
                        For Each f As XmlNode In t.SelectNodes("field")
                            RaiseEvent PreLoadField(FormName, t.Attributes("name").Value, f.Attributes("name").Value)
                        Next
                    Next

            End Select

            With SubForms
                .Clear()
                For Each f As XmlNode In _XmlMe.SelectNodes("form")
                    .Add(f.Attributes("name").Value)
                Next
            End With

            RaiseEvent SetReadOnly(CBool(_XmlMe.Attributes("readonly").Value))
            TabForm.hTabClick()

        End With

    End Sub

    Public Sub Bind(ByVal LoadedForms() As ctform)
        _LoadedForms = LoadedForms
        _formOrdinal = UBound(LoadedForms)
        With Me.BindingSource
            .ResetBindings(True)
            .MoveFirst()
        End With
    End Sub

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

#End Region

#Region "Get / Set Data"

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
            For Each t As Panel In .TabForm.Values
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

    Private Sub hSFClick(ByRef MenuItem As String) Handles SubForms.MenuClick
        If Not IsNothing(BindingSource.Current) Then
            For Each sf As XmlNode In _XmlMe.SelectNodes("form")
                If String.Compare(MenuItem, sf.Attributes("name").Value) = 0 Then
                    RaiseEvent OpenSubform(sf)
                    Exit Sub
                End If
            Next
        End If
    End Sub

#End Region

#Region "resize code"

    Private Sub hideall()
        With Me
            With .TabForm
                If .Visible Then
                    .Visible = False
                    .Dock = DockStyle.None
                    .Top = -1
                    .Left = -1
                    .Width = 0
                    .Height = 0
                End If
            End With
            With .DataGrid
                If .Visible Then
                    .Visible = False
                    .Dock = DockStyle.None
                    .Top = -1
                    .Left = -1
                    .Width = 0
                    .Height = 0
                End If
            End With
            With .UserText
                If .Visible Then
                    .Visible = False
                    .Dock = DockStyle.None
                    .Top = -1
                    .Left = -1
                    .Width = 0
                    .Height = 0
                End If
            End With
            With .WebBrowser
                If .Visible Then
                    .Visible = False
                    .Dock = DockStyle.None
                    .Top = -1
                    .Left = -1
                    .Width = 0
                    .Height = 0
                End If
            End With
            With PriSign
                If .Visible Then
                    .Visible = False
                    .Dock = DockStyle.None
                    .Top = -1
                    .Left = -1
                    .Width = 0
                    .Height = 0
                End If
            End With
            With .PriDateSel
                If .Visible Then
                    .Visible = False
                    .Dock = DockStyle.None
                    .Top = -1
                    .Left = -1
                    .Width = 0
                    .Height = 0
                End If
            End With
        End With
    End Sub

#End Region

    Public Sub Close()
        Select Case Me.FormView
            Case eFormView.view_Signature
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
                If Not CBool(_XmlMe.SelectSingleNode("tab/field[@name='TEXT']").Attributes("readonly").Value) Then
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

        'MyBase.Finalize()
    End Sub

    Private Sub BindingSource_CurrentItemChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles BindingSource.CurrentItemChanged
        bo.Save()
    End Sub

    Public Function HasMandatory() As Boolean
        For Each p As Panel In Me.TabForm.Values
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
        For Each p As Panel In Me.TabForm.Values
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
        For Each p As Panel In Me.TabForm.Values
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
        Dim bo As oBind = MyDataSet.DataSet(_XmlMe.Attributes("from").Value)
        bo.Trigger(tTrigger.PREINSERT, o, , cancel)
        Return cancel
    End Function

    Public Overloads Sub REFRESH()
        If FormView = eFormView.view_Form Then
            If Not BindingSource.Position = -1 Then
                Dim o As Object = BindingSource.Current
                For Each p As Panel In Me.TabForm.Values
                    For Each c As PriBaseCtrl In p.Controls
                        Dim pi As System.Reflection.PropertyInfo = o.GetType.GetProperty(c.Column)
                        c.ControlText = pi.GetValue(o, Nothing)
                    Next
                Next
            End If
        End If
    End Sub

    Private Sub ctform_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Disposed

    End Sub
End Class
