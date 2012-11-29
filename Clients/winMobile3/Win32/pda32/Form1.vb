Imports ntEvtlog
Imports uiCtrls
Imports Bind

Public Class Form1

    Dim config As xmlConfiguration
    Dim LoadedForms() As ctform
    Dim App As String = "PDA"
    Dim ev As New ntEvtlog.evt(EvtLogMode.File, EvtLogVerbosity.Normal, App)
    Dim WithEvents ds As New oDataSet("DataClasses.dll", App, ev)
    Dim formPath As String = ""

    Private Sub Form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ds.QueryConsts("username") = "service"
        ds.QueryConsts("van") = "VAN"

        config = New xmlConfiguration(ds.BasePath() & "config.xml")

        Dim menu As New priFormMenu(config)
        menu.Dock = DockStyle.Fill
        AddHandler menu.OpenForm, AddressOf hOpenForm
        MenuPanel.Controls.Add(menu)
        showMenu()

    End Sub

    Private Sub hRefresh() Handles ds.Refresh
        LoadedForms(UBound(LoadedForms)).Refresh()        
    End Sub

    Private Sub hOpenSubform(ByVal Name As String)
        For i As Integer = 0 To UBound(LoadedForms)
            With LoadedForms(i)
                .Dock = DockStyle.None
                .Visible = False
            End With
        Next
        formPath += "." & Name
        NewForm()
    End Sub

    Private Sub hGetIntValue(ByVal Name As String, ByRef value As String) Handles ds.GetInterfaceValue
        value = LoadedForms(UBound(LoadedForms)).getData(Name)
    End Sub

    Private Sub hSetIntValue(ByVal Name As String, ByVal value As String) Handles ds.SetInterfaceValue
        LoadedForms(UBound(LoadedForms)).SetData(Name, value)
    End Sub

    Private Sub hSetReadonly(ByVal IsReadOnly As Boolean)
        With Me
            .btnAdd.Enabled = Not (IsReadOnly)
            .btnDelete.Enabled = Not (IsReadOnly)
        End With
    End Sub

    Private Sub hSetView(ByVal FormView As eFormView)
        With Me

            Select Case FormView
                Case eFormView.view_Form
                    '.ImageIndex = 2 ' table
                    .btnView.Enabled = True
                    .btnView.ToolTipText = "Table"
                    .btnAdd.Enabled = True
                    .btnFirst.Enabled = True
                    .btnLast.Enabled = True
                    .btnBack.Enabled = True
                    .btnNext.Enabled = True
                Case eFormView.view_Table
                    '.ImageIndex = 1 'form
                    .btnView.Enabled = True
                    .btnView.ToolTipText = "Form"
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

    Private Sub hOpenForm(ByVal FormName As String)

        formPath = FormName

        With Me
            '.Cover.Visible = True

            .btnUp.Enabled = True
            .btnView.Enabled = True
            .btnFirst.Enabled = True
            .btnBack.Enabled = True
            .btnNext.Enabled = True
            .btnLast.Enabled = True
            .btnAdd.Enabled = True
            .btnDelete.Enabled = True

            With .Panel
                .Visible = True
            End With

            NewForm()

            With .MenuPanel
                .Visible = False
            End With

            '.Cover.Visible = False

        End With



    End Sub

    Private Sub NewForm()
        Try
            ReDim Preserve LoadedForms(UBound(LoadedForms) + 1)
        Catch
            ReDim LoadedForms(0)
        Finally
            LoadedForms(UBound(LoadedForms)) = New ctform(ds)
            With LoadedForms(UBound(LoadedForms))

                AddHandler .OpenSubform, AddressOf hOpenSubform
                AddHandler .SetView, AddressOf hSetView
                AddHandler .SetReadOnly, AddressOf hSetReadonly


                .Parent = Panel
                .Dock = DockStyle.Fill

                .FormReference(LoadedForms, UBound(LoadedForms))
                .DrawForm(config.FormPath(formPath))

            End With
            Me.Text = Replace(formPath, ".", ">")
        End Try
    End Sub

    Private Sub btnUp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUp.Click
        With LoadedForms(UBound(LoadedForms))
            RemoveHandler .OpenSubform, AddressOf hOpenSubform
            RemoveHandler .SetView, AddressOf hSetView
            RemoveHandler .SetReadOnly, AddressOf hSetReadonly
            .Parent = Nothing
            .Close()
            .Dispose()
        End With

        Try
            ReDim Preserve LoadedForms(UBound(LoadedForms) - 1)
            If LoadedForms.Length = 0 Then LoadedForms = Nothing
        Catch ex As Exception
            LoadedForms = Nothing
        End Try

        If Not IsNothing(LoadedForms) Then
            With LoadedForms(UBound(LoadedForms))
                .Parent = Panel
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
                hSetReadonly(._xf.IsReadOnly)
            End With
        Else
            showMenu()
        End If
    End Sub

    Private Sub showMenu()
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

            .Panel.Visible = False
            .MenuPanel.Visible = True
        End With
    End Sub

    Private Sub btnView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView.Click
        With LoadedForms(UBound(LoadedForms))
            Select Case .FormView
                Case eFormView.view_Form
                    .FormView = eFormView.view_Table
                    'With e.Button
                    '    .ImageIndex = 1 'form
                    '    .ToolTipText = "Form"
                    'End With
                Case eFormView.view_Table
                    .FormView = eFormView.view_Form
                    'With e.Button
                    '    .ImageIndex = 2 ' table
                    '    .ToolTipText = "Table"
                    'End With
            End Select
        End With
    End Sub

    Private Sub btnSync_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSync.Click
        For Each o As oBind In ds.DataSet.Values
            o.Sync()
            o.Save()
        Next
        If Not IsNothing(LoadedForms) Then
            With LoadedForms(UBound(LoadedForms))
                .BindingSource.ResetBindings(True)
            End With
        End If
    End Sub

    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        With LoadedForms(UBound(LoadedForms))

            If Not .Adding Then
                .Adding = True
                SetButtonsAdd(Not (.Adding))
                .BindingSource.SuspendBinding()
                LoadedForms(UBound(LoadedForms)).FormView = eFormView.view_Form
            Else
                If .HasMandatory Then
                    If .HasRegex Then
                        Dim ob As oBind = ds.DataSet(LoadedForms(UBound(LoadedForms))._xf.SQLFrom)
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

    Private Sub btnNav_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLast.Click, btnBack.Click, btnNext.Click, btnFirst.Click
        Dim op As Integer
        Dim btn As ToolStripButton = sender
        op = CInt(btn.Tag)
        With LoadedForms(UBound(LoadedForms))
            Select Case op
                Case -2
                    .BindingSource.MoveFirst()
                Case -1
                    .BindingSource.MovePrevious()
                Case 1
                    .BindingSource.MoveNext()
                Case 2
                    .BindingSource.MoveLast()
            End Select
        End With
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        With LoadedForms(UBound(LoadedForms))
            If Not IsNothing(.BindingSource.Current) Then
                If Not .Adding Then
                    If MsgBox("This will delete the selected record.", MsgBoxStyle.OkCancel) = MsgBoxResult.Ok Then
                        Dim ob As oBind = ds.DataSet(._xf.SQLFrom)
                        ob.Remove(ob.KeyProperty.GetValue(.BindingSource.Current, Nothing))
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
        End With
    End Sub

End Class
