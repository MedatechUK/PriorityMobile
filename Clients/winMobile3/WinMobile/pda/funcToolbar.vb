Imports uiCtrls
Imports Bind
Imports System.XML

Module funcToolbar

    Public formPath As String = ""    
    Public LoadedForms() As ctform

    Private Sub hSetReadonly(ByVal IsReadOnly As Boolean)
        With BaseForm
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
        With BaseForm

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

            LoadedForms(UBound(LoadedForms)) = BaseForm.config.AllForms(formPath) 'New ctform(form, BaseForm.ds, LoadedForms)
            With LoadedForms(UBound(LoadedForms))

                AddHandler .OpenSubform, AddressOf hOpenForm
                AddHandler .SetView, AddressOf hSetView
                AddHandler .SetReadOnly, AddressOf hSetReadonly

                .Parent = BaseForm.FormPanel
                .Dock = DockStyle.Fill

                .Bind(LoadedForms)
                .FormView = .Defaultview

            End With
            BaseForm.Text = Replace(formPath, ".", ">")
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
                            BaseForm.bttnStart.Enabled = True
                        Else
                            With LoadedForms(UBound(LoadedForms))
                                .Parent = BaseForm.FormPanel
                                .Visible = True
                                .Dock = DockStyle.Fill
                            End With
                            Dim str() As String = Split(formPath, ".")

                            formPath = ""
                            For i As Integer = 0 To UBound(str) - 1
                                formPath += str(i)
                                If i < UBound(str) - 1 Then formPath += "."
                            Next
                            BaseForm.Text = Replace(formPath, ".", ">")
                            With LoadedForms(UBound(LoadedForms))
                                hSetView(.FormView)
                                hSetReadonly(CBool(.XML.Attributes("readonly").Value))
                            End With
                        End If

                    End Try

                Case "view"
                    .FormView = NextView(.FormView)

                Case "sync"
                    With BaseForm.SyncMenu
                        With .MenuItems
                            .Clear()
                            For Each s As oSync In BaseForm.ds.Sync
                                If String.Compare(s.FormName, Strings.Right(formPath, Len(s.FormName)), True) = 0 _
                                    Or (s.FormInherit And InStr(formPath, "." & s.FormName) > 0) Then
                                    Dim mi As New MenuItem()
                                    mi.Text = s.ToString
                                    AddHandler mi.Click, AddressOf hSyncMenuClick
                                    .Add(mi)
                                End If
                            Next
                        End With
                        .Show(BaseForm.ToolBar1, New System.Drawing.Point(46, -18 * .MenuItems.Count))
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
                                Dim ob As oBind = BaseForm.ds.DataSet(.XML.Attributes("from").Value)
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
                                Dim ob As oBind = BaseForm.ds.DataSet(LoadedForms(UBound(LoadedForms)).XML.Attributes("from").Value)
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
        For Each s As oSync In BaseForm.ds.Sync
            If String.Compare(mi.Text, s.ToString) = 0 Then
                s.DoSync()
                Exit Sub
            End If
        Next
    End Sub

    Public Sub showMenu()
        With BaseForm

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
        With BaseForm
            .btnView.Enabled = b
            .btnUp.Enabled = b
            .btnFirst.Enabled = b
            .btnLast.Enabled = b
            .btnBack.Enabled = b
            .btnNext.Enabled = b
        End With
    End Sub

End Module
