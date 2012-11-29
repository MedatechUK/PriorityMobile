Imports ntEvtlog
Imports uiCtrls
Imports Bind
Imports System.XML
Imports System.Threading

Public Class BaseForm

    Private App As String = "PDA"
    Public ev As New ntEvtLog.evt(EvtLogMode.File, EvtLogVerbosity.Normal, App)
    Public WithEvents ds As oDataSet
    Public WithEvents config As xmlConfiguration
    Public formPath As String = ""
    Public LoadedForms() As ctform

    Private _CachedForms As New Dictionary(Of String, ctform)
    Public Property CachedForms() As Dictionary(Of String, ctform)
        Get
            Return _CachedForms
        End Get
        Set(ByVal value As Dictionary(Of String, ctform))
            _CachedForms = value
        End Set
    End Property

#Region "Delegates"

#Region "List Item delegates"
    Delegate Sub AddListItem(ByVal f As XmlNode)
    Private Sub AddListItemMethod(ByVal f As XmlNode)
        lstModules.Items.Add(f.Attributes("name").Value)
    End Sub
    Public AddListItemDelegate As AddListItem
#End Region

#Region "Version delegates"
    Delegate Sub SetVersionText(ByVal vers As String)
    Private Sub SetVersionTextMethod(ByVal vers As String)
        txtVersion.Text = vers
    End Sub
    Public SetVersionTextDelegate As SetVersionText
#End Region

#Region "Start Button delegates"

    Delegate Sub SetTitle(ByVal Title As String)
    Private Sub SetTitleMethod(ByVal Title As String)
        lbl_Title.Text = Title
        Application.DoEvents()
    End Sub
    Public SetTitleDelegate As SetTitle

    Delegate Sub SetDetail(ByVal Detail As String)
    Private Sub SetDetailMethod(ByVal Detail As String)
        lbl_Detail.Text = Detail
        Application.DoEvents()
    End Sub
    Public SetDetailDelegate As SetDetail

    Delegate Sub SetPgsBarMax(ByVal Max As Integer)
    Private Sub SetPgsBarMaxMethod(ByVal Max As Integer)
        With Me
            .lbl_Detail.Text = ""
            With .ProgressBar
                .Value = 0
                .Maximum = Max
            End With
        End With
        Application.DoEvents()
    End Sub
    Public SetPgsBarMaxDelegate As SetPgsBarMax

    Delegate Sub SetPgsBarVal()
    Private Sub SetPgsBarValMethod()
        With Me.ProgressBar
            .Value += 1
        End With
        Application.DoEvents()
    End Sub
    Public SetPgsBarValDelegate As SetPgsBarVal

    Delegate Sub SetLoadComplete()
    Private Sub SetLoadCompleteMethod()
        With Me
            With .ProgressPanel
                .Visible = False
                .Dock = DockStyle.None
            End With
            With .PanelModuleList
                .Visible = True
                .Dock = DockStyle.Fill
            End With
            .lstModules.Height = .PanelModuleList.Height - .bttnStart.Height
        End With
    End Sub
    Public SetLoadCompleteDelegate As SetLoadComplete

#End Region

#End Region

#Region "Preload Handlers"

#Region "Handles Dataset events"

    Public Sub hBeginLoadType(ByVal LoadType As String, ByVal Max As Integer)
        Me.Invoke(SetTitleDelegate, String.Format("Load: [{0}]", LoadType))
        Me.Invoke(SetPgsBarMaxDelegate, Max)
    End Sub

    Public Sub hLoadType(ByVal TypeName As String)
        Me.Invoke(SetDetailDelegate, String.Format("{0}...", TypeName))
        Me.Invoke(SetPgsBarValDelegate)
    End Sub

#End Region

#Region "Handles Config events"

    Private Sub hPreloadField(ByVal FormName As String, ByVal TabName As String, ByVal ColumnName As String)
        Me.Invoke(SetDetailDelegate, String.Format("{0}.{1}...", TabName, ColumnName))
        Me.Invoke(SetPgsBarValDelegate)
    End Sub

#End Region

#End Region

#Region "Initialisation"

    Private Sub Form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        AddListItemDelegate = New AddListItem(AddressOf AddListItemMethod)
        SetVersionTextDelegate = New SetVersionText(AddressOf SetVersionTextMethod)

        SetTitleDelegate = New SetTitle(AddressOf SetTitleMethod)
        SetDetailDelegate = New SetDetail(AddressOf SetDetailMethod)
        SetPgsBarMaxDelegate = New SetPgsBarMax(AddressOf SetPgsBarMaxMethod)
        SetPgsBarValDelegate = New SetPgsBarVal(AddressOf SetPgsBarValMethod)
        SetLoadCompleteDelegate = New SetLoadComplete(AddressOf SetLoadCompleteMethod)

        showMenu()
        AddHandler ToolBar1.ButtonClick, AddressOf hClickToolbar

        Application.DoEvents()

        With Me

            With .ProgressPanel
                .Visible = True
                .Dock = DockStyle.Top
            End With
            With .PanelModuleList
                .Visible = False
                .Dock = DockStyle.None
            End With

            .Show()
            .preLoad()

        End With

    End Sub

    Sub preLoad()

        ds = New oDataSet
        With ds
            AddHandler .BeginLoadType, AddressOf hBeginLoadType
            AddHandler .LoadType, AddressOf hLoadType
            .init("DataClasses.dll", App, ev)
            .QueryConsts("username") = "service"
            .QueryConsts("van") = "VAN"
        End With

        config = New xmlConfiguration(ds.BasePath() & "config.xml")
        With config
            Me.Invoke(SetVersionTextDelegate, String.Format("{0}.{1}", _
                .xmlMajorVersion, _
                .xmlMinorVersion) _
            )
        End With

        hBeginLoadType("Forms", config.xmlFieldCount())
        For Each f As XmlNode In config.xmlforms()
            Me.preLoadForms(ds, "", f)
            Me.Invoke(AddListItemDelegate, f)
        Next

        Me.Invoke(SetLoadCompleteDelegate)

    End Sub

    Private Sub preLoadForms(ByRef ds As oDataSet, ByVal formPath As String, ByVal node As XmlNode)
        Dim fname As String = node.Attributes("name").Value
        ' Notify preload form starting
        Me.Invoke(SetTitleDelegate, String.Format("Init: {0}...", fname))
        Me.Invoke(SetDetailDelegate, "")
        If formPath.Length > 0 Then fname = formPath & "." & fname
        ' Create a new form
        CachedForms.Add(fname, New ctform(node, ds))
        Me.Invoke(SetTitleDelegate, String.Format("Load: {0}", CachedForms(fname).FormName))
        ' Add the progress bar handlers
        AddHandler CachedForms(fname).PreLoadField, AddressOf hPreloadField
        CachedForms(fname).DrawForm()
        For Each n As XmlNode In config.xmlforms(node)
            preLoadForms(ds, fname, n)
        Next
    End Sub

#End Region

#Region "Read / Write form values"

    Private Sub hGetIntValue(ByVal Name As String, ByRef value As String) Handles ds.GetInterfaceValue
        value = LoadedForms(UBound(LoadedForms)).getData(Name)
    End Sub

    Private Sub hSetIntValue(ByVal Name As String, ByVal value As String) Handles ds.SetInterfaceValue
        LoadedForms(UBound(LoadedForms)).SetData(Name, value)
    End Sub

#End Region

#Region "Menu Handlers"

    Private Sub btnStart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bttnStart.Click
        With lstModules
            bttnStart.Enabled = False
            For Each f As XmlNode In config.xmlforms()
                If String.Compare(f.Attributes("name").Value, .SelectedItem, True) = 0 Then
                    formPath = f.Attributes("name").Value
                    With Me
                        .btnUp.Enabled = True
                        .btnView.Enabled = True
                        .btnFirst.Enabled = True
                        .btnBack.Enabled = True
                        .btnNext.Enabled = True
                        .btnLast.Enabled = True
                        .btnAdd.Enabled = True
                        .btnDelete.Enabled = True

                        With .FormPanel
                            .Visible = True
                        End With

                        hOpenForm(f)

                        With .MenuPanel
                            .Visible = False
                        End With
                    End With
                End If
            Next
        End With
    End Sub

    Private Sub lstModules_SelectedValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstModules.SelectedIndexChanged
        With lstModules
            bttnStart.Enabled = Not (IsNothing(.SelectedItem))
        End With
    End Sub

#End Region

#Region "Resize code"
    Private Sub BaseForm_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Resize
        With Me
            If .MenuPanel.Visible Then
                If .PanelModuleList.Visible Then
                    .lstModules.Height = .PanelModuleList.Height - .bttnStart.Height
                End If
            End If
        End With
    End Sub
#End Region

#Region "Handlers"

    Private Sub hSetReadonly(ByVal IsReadOnly As Boolean)
        With Me
            .btnAdd.Enabled = Not (IsReadOnly)
            .btnDelete.Enabled = Not (IsReadOnly)
        End With
    End Sub

    Private Function NextView(ByVal FormView As eFormView) As eFormView
        Select Case FormView
            Case eFormView.view_Form
                Return eFormView.view_Table
            Case eFormView.view_Table
                If LoadedForms(UBound(LoadedForms)).AllowedViews.Contains(eFormView.view_DateSel) Then
                    Return eFormView.view_DateSel
                Else
                    Return eFormView.view_Form
                End If
            Case eFormView.view_DateSel
                Return eFormView.view_Form
            Case Else
                Return FormView
        End Select
    End Function

    Private Sub NextButton(ByVal FormView As eFormView, ByRef ImageIndex As Integer, ByVal ToolTipText As String)
        Select Case NextView(FormView)
            Case eFormView.view_Form
                ImageIndex = 10
                ToolTipText = "Form View"
            Case eFormView.view_Table
                ImageIndex = 1
                ToolTipText = "Table View"
            Case eFormView.view_DateSel
                ImageIndex = 9
                ToolTipText = "Date Select"
            Case eFormView.view_HTML

            Case eFormView.view_Signature

        End Select
    End Sub

    Private Sub hSetView(ByVal FormView As eFormView)
        With Me

            With .ToolBar1.Buttons(1)
                .Enabled = Not CBool(FormView = NextView(FormView))
                NextButton(FormView, .ImageIndex, .ToolTipText)
            End With

            Select Case FormView
                Case eFormView.view_Form
                    .btnView.Enabled = True
                    .btnAdd.Enabled = True
                    .btnFirst.Enabled = True
                    .btnLast.Enabled = True
                    .btnBack.Enabled = True
                    .btnNext.Enabled = True
                Case eFormView.view_Table
                    .btnView.Enabled = True
                    .btnAdd.Enabled = True
                    .btnFirst.Enabled = True
                    .btnLast.Enabled = True
                    .btnBack.Enabled = True
                    .btnNext.Enabled = True
                Case eFormView.view_DateSel
                    '.ImageIndex = 1 'form
                    .btnView.Enabled = True
                    .btnAdd.Enabled = True
                    .btnFirst.Enabled = True
                    .btnLast.Enabled = True
                    .btnBack.Enabled = True
                    .btnNext.Enabled = True
                Case eFormView.view_Signature
                    .btnView.Enabled = False
                    .btnAdd.Enabled = False
                    .btnFirst.Enabled = False
                    .btnLast.Enabled = False
                    .btnBack.Enabled = False
                    .btnNext.Enabled = False
                Case eFormView.view_HTML
                    .btnView.Enabled = False
                    .btnAdd.Enabled = False
                    .btnFirst.Enabled = False
                    .btnLast.Enabled = False
                    .btnBack.Enabled = False
                    .btnNext.Enabled = False
            End Select
        End With
    End Sub

    Public Sub hOpenForm(ByVal form As XmlNode)
        formPath = ""
        Try
            For i As Integer = 0 To UBound(LoadedForms)
                With LoadedForms(i)
                    .Dock = DockStyle.None
                    .Visible = False
                    formPath += .FormName & "."
                End With
            Next
            ReDim Preserve LoadedForms(UBound(LoadedForms) + 1)
        Catch
            ReDim LoadedForms(0)
        Finally
            formPath += form.Attributes("name").Value()

            LoadedForms(UBound(LoadedForms)) = CachedForms(formPath)
            With LoadedForms(UBound(LoadedForms))

                AddHandler .OpenSubform, AddressOf hOpenForm
                AddHandler .SetView, AddressOf hSetView
                AddHandler .SetReadOnly, AddressOf hSetReadonly

                FormPanel.Controls.Add(LoadedForms(UBound(LoadedForms)))

                .Dock = DockStyle.Fill
                .Bind(LoadedForms)
                .FormView = .Defaultview

            End With
            Me.Text = Replace(formPath, ".", ">")
        End Try
    End Sub

    Public Sub hClickToolbar(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs)
        With LoadedForms(UBound(LoadedForms))
            Select Case LCase(e.Button.Tag.ToString)
                Case "up"
                    RemoveHandler .OpenSubform, AddressOf hOpenForm
                    RemoveHandler .SetView, AddressOf hSetView
                    RemoveHandler .SetReadOnly, AddressOf hSetReadonly

                    .Parent = Nothing
                    '.Close()
                    '.Dispose()

                    Try
                        ReDim Preserve LoadedForms(UBound(LoadedForms) - 1)
                        If LoadedForms.Length = 0 Then LoadedForms = Nothing
                    Catch ex As Exception
                        LoadedForms = Nothing
                    Finally
                        If IsNothing(LoadedForms) Then
                            showMenu()
                            bttnStart.Enabled = True
                        Else
                            With LoadedForms(UBound(LoadedForms))
                                .Parent = FormPanel
                                .Visible = True
                                .Dock = DockStyle.Fill
                            End With
                            Dim str() As String = Split(formPath, ".")

                            formPath = ""
                            For i As Integer = 0 To UBound(str) - 1
                                formPath += str(i)
                                If i < UBound(str) - 1 Then formPath += "."
                            Next
                            Me.Text = Replace(formPath, ".", ">")
                            With LoadedForms(UBound(LoadedForms))
                                hSetView(.FormView)
                                hSetReadonly(CBool(.XML.Attributes("readonly").Value))
                            End With
                        End If

                    End Try

                Case "view"
                    .FormView = NextView(.FormView)

                Case "sync"
                    With SyncMenu
                        With .MenuItems
                            .Clear()
                            For Each s As oSync In ds.Sync
                                If String.Compare(s.FormName, Strings.Right(formPath, Len(s.FormName)), True) = 0 _
                                    Or (s.FormInherit And InStr(formPath, "." & s.FormName) > 0) Then
                                    Dim mi As New MenuItem()
                                    mi.Text = s.ToString
                                    AddHandler mi.Click, AddressOf hSyncMenuClick
                                    .Add(mi)
                                End If
                            Next
                        End With
                        .Show(ToolBar1, New System.Drawing.Point(46, -18 * .MenuItems.Count))
                    End With

                    'If Not IsNothing(LoadedForms) Then
                    '    .BindingSource.ResetBindings(True)
                    'End If

                Case "first"
                    .BindingSource.MoveFirst()

                Case "back"
                    .BindingSource.MovePrevious()

                Case "next"
                    .BindingSource.MoveNext()

                Case "last"
                    .BindingSource.MoveLast()

                Case "delete"
                    If Not IsNothing(.BindingSource.Current) Then
                        If Not .Adding Then
                            If MsgBox("This will delete the selected record.", MsgBoxStyle.OkCancel) = MsgBoxResult.Ok Then
                                Dim ob As oBind = ds.DataSet(.XML.Attributes("from").Value)
                                Dim v As String = ob.KeyProperty.GetValue(.BindingSource.Current, Nothing).ToString
                                ob.Remove(v)
                                ob.Save()
                                .BindingSource.ResetBindings(True)
                            End If
                        Else
                            If MsgBox("This will cancel the new record.", MsgBoxStyle.OkCancel) = MsgBoxResult.Ok Then
                                .Adding = False
                                SetButtonsAdd(Not (.Adding))
                                With .BindingSource
                                    .ResumeBinding()
                                    .ResetBindings(True)
                                End With
                            End If
                        End If
                    End If

                Case "add"
                    If Not .Adding Then
                        .Adding = True
                        SetButtonsAdd(Not (.Adding))
                        .BindingSource.SuspendBinding()
                        LoadedForms(UBound(LoadedForms)).FormView = eFormView.view_Form
                    Else
                        If .HasMandatory Then
                            If .HasRegex Then
                                Dim ob As oBind = ds.DataSet(LoadedForms(UBound(LoadedForms)).XML.Attributes("from").Value)
                                Dim o As Object = Activator.CreateInstance(ob.Prototype.GetType)
                                If Not .NewObject(o) Then
                                    If ob.ContainsKey(ob.KeyProperty.GetValue(o, Nothing)) = -1 Then
                                        ob.Add(o)
                                        .Adding = False
                                        SetButtonsAdd(Not (.Adding))
                                        With .BindingSource
                                            .ResumeBinding()
                                            .ResetBindings(True)
                                            .MoveLast()
                                        End With
                                    Else
                                        ob.Add(o)
                                    End If
                                End If
                            End If
                        End If
                    End If
            End Select
        End With
    End Sub

    Private Sub hSyncMenuClick(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim mi As MenuItem = sender
        For Each s As oSync In ds.Sync
            If String.Compare(mi.Text, s.ToString) = 0 Then
                s.DoSync()
                Exit Sub
            End If
        Next
    End Sub

    Public Sub showMenu()
        With Me

            .Text = "Priority Mobile"

            .btnUp.Enabled = False
            .btnView.Enabled = False
            .btnFirst.Enabled = False
            .btnBack.Enabled = False
            .btnNext.Enabled = False
            .btnLast.Enabled = False
            .btnAdd.Enabled = False
            .btnDelete.Enabled = False

            .FormPanel.Visible = False
            .MenuPanel.Visible = True
        End With
    End Sub

    Private Sub SetButtonsAdd(ByVal b As Boolean)
        With Me
            .btnView.Enabled = b
            .btnUp.Enabled = b
            .btnFirst.Enabled = b
            .btnLast.Enabled = b
            .btnBack.Enabled = b
            .btnNext.Enabled = b
        End With
    End Sub

    Private Sub hRefresh() Handles ds.Refresh
        LoadedForms(UBound(LoadedForms)).REFRESH()
    End Sub

#End Region

End Class
