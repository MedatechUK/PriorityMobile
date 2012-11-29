Imports System.Windows.Forms
Imports System.IO

Public Class MDIParent

    Private m_ChildFormNumber As Integer = 0
    Private mCwidth As Integer = 80

#Region "Initialisation"

    Private Function newForm() As MDIChild
        ' Create a new instance of the child form.
        Dim ChildForm As New MDIChild(Me)
        ' Make it a child of this MDI form before showing it.
        ChildForm.MdiParent = Me

        m_ChildFormNumber += 1
        Return ChildForm
    End Function

    Public Function frmCount() As Integer
        Dim i As Integer = 0
        For Each ChildForm As MDIChild In Me.MdiChildren
            i += 1
        Next
        Return i
    End Function

    Private Function ActiveFrm() As MDIChild
        Return Me.ActiveMdiChild
    End Function

    Private Function hasActive() As Boolean
        Return Not IsNothing(ActiveFrm)
    End Function

    Private Sub MDIParent_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        With Me
            .ToolStripStatusLabel1.Text = ""
            .ToolStripStatusLabel2.Text = ""
            .ToolStripStatusLabel3.Text = ""
            .ToolStripStatusLabel4.Text = ""
            .ToolStripStatusLabel5.Text = ""
            .ToolStripStatusLabel6.Text = ""

            HideChildMenu()
        End With
    End Sub

#End Region

#Region "Public Properties"

    Public Property ConsoleWidth() As Integer
        Get
            Return mCwidth
        End Get
        Set(ByVal value As Integer)
            mCwidth = value
        End Set
    End Property

#End Region

#Region "Open/New MDI Child Menu Items"

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        Dim ChildForm As MDIChild = newForm()
        With ChildForm
            .Text = "Untitled Document " & m_ChildFormNumber
            .doToolbar()
            .WindowState = FormWindowState.Maximized
            setHeader(ChildForm)
            GetWhere(ChildForm)
            .Show()
            ActivateToolStrip()
            .Changed = False
        End With
    End Sub

    Private Sub OpenFile(ByVal sender As Object, ByVal e As EventArgs) Handles OpenToolStripMenuItem.Click

        Dim OpenFileDialog As New OpenFileDialog
        With OpenFileDialog
            .InitialDirectory = My.Computer.FileSystem.SpecialDirectories.MyDocuments
            .Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*"

            If (.ShowDialog(Me) = System.Windows.Forms.DialogResult.OK) Then
                Dim fname As String = .FileName
                For Each ChildForm As MDIChild In Me.MdiChildren
                    If LCase(Trim(ChildForm.Filename)) = LCase(Trim(fname)) Then
                        ChildForm.BringToFront()
                        Exit Sub
                    End If
                Next

                Dim inst As New StreamReader(fname)
                Dim part() As String = Split(inst.ReadToEnd, "##")
                inst.Close()

                For i As Integer = 0 To UBound(part)
                    If Len(Trim(part(i))) > 0 Then

                        Dim ChildForm As MDIChild = newForm()
                        With ChildForm
                            .Filename = fname

                            If i > 0 Then
                                part(i) = "##" & part(i)
                                Try
                                    Dim sett() As String = _
                                    Split(Split(Strings.Right(part(i), Len(part(i)) - 2), vbCrLf)(0), ",")

                                    .CursorName = sett(0)
                                    .StartLabel = sett(1)
                                    .RecordType = sett(2)
                                    .VarPref = sett(3)
                                    .IsSub = sett(4)
                                    .WriteLoad = sett(5)
                                    .SelectVars = sett(6)
                                    .LinkGenLoad = sett(7)
                                Catch

                                End Try
                            End If

                            .doToolbar()
                            .FormData = part(i)
                            .FormOutput = ""

                            GetWhere(ChildForm)
                            GetCols(ChildForm)
                            If Not IsNothing(.Cols) Then
                                DrawVarTable(ChildForm, .lstVariables)
                            End If

                            .WindowState = FormWindowState.Maximized
                            .Show()
                            ActivateToolStrip()
                            .Changed = False
                        End With
                    End If
                Next
            End If
        End With

    End Sub

#End Region

#Region "Text Editing Menu Items"

    Public Sub CutToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles CutToolStripMenuItem.Click
        Dim f As MDIChild = ActiveFrm()
        With f
            Select Case .TypeMyActiveControl
                Case eActiveControl.eSource, eActiveControl.eWhere, eActiveControl.eOutput
                    Dim ctrl As TextBox = .MyActiveControl
                    ctrl.Cut()
            End Select
        End With
    End Sub

    Public Sub CopyToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles CopyToolStripMenuItem.Click
        Dim f As MDIChild = ActiveFrm()
        With f
            Select Case .TypeMyActiveControl
                Case eActiveControl.eSource, eActiveControl.eWhere, eActiveControl.eOutput
                    Dim ctrl As TextBox = .MyActiveControl
                    ctrl.Copy()
            End Select
        End With
    End Sub

    Public Sub PasteToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles PasteToolStripMenuItem.Click
        Dim f As MDIChild = ActiveFrm()
        With f
            Select Case .TypeMyActiveControl
                Case eActiveControl.eSource, eActiveControl.eWhere
                    Dim ctrl As TextBox = .MyActiveControl
                    ctrl.Paste()
            End Select
        End With
    End Sub

    Public Sub UndoToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UndoToolStripMenuItem.Click
        Dim f As MDIChild = ActiveFrm()
        With f
            Select Case .TypeMyActiveControl
                Case eActiveControl.eSource, eActiveControl.eWhere, eActiveControl.eOutput
                    Dim ctrl As TextBox = .MyActiveControl
                    ctrl.Undo()
            End Select
        End With
    End Sub

    Public Sub SelectAllToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SelectAllToolStripMenuItem.Click
        Dim f As MDIChild = ActiveFrm()
        With f
            Select Case .TypeMyActiveControl
                Case eActiveControl.eSource, eActiveControl.eWhere, eActiveControl.eOutput
                    Dim ctrl As TextBox = .MyActiveControl
                    ctrl.SelectAll()
                Case eActiveControl.eVariables
                    For i As Integer = 0 To .lstVariables.Items.Count - 1
                        .lstVariables.Items(i).Selected = True
                    Next
            End Select
        End With
    End Sub

    Public Sub UpperToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UpperToolStripMenuItem.Click
        If hasActive() Then
            Dim f As MDIChild = ActiveFrm()
            With f
                Select Case .TypeMyActiveControl
                    Case eActiveControl.eSource, eActiveControl.eWhere, eActiveControl.eOutput
                        Dim ctrl As TextBox = .MyActiveControl
                        If ctrl.SelectionLength = 0 Then
                            ctrl.Text = UCase(ctrl.Text)
                        Else
                            ctrl.Cut()
                            Clipboard.SetText(UCase(Clipboard.GetText))
                            ctrl.Paste()
                        End If
                End Select
            End With
        End If
    End Sub

    Public Sub LowerToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LowerToolStripMenuItem.Click
        If hasActive() Then
            Dim f As MDIChild = ActiveFrm()
            With f
                Select Case .TypeMyActiveControl
                    Case eActiveControl.eSource, eActiveControl.eWhere, eActiveControl.eOutput
                        Dim ctrl As TextBox = .MyActiveControl
                        If ctrl.SelectionLength = 0 Then
                            ctrl.Text = LCase(ctrl.Text)
                        Else
                            ctrl.Cut()
                            Clipboard.SetText(LCase(Clipboard.GetText))
                            ctrl.Paste()
                        End If
                End Select
            End With
        End If
    End Sub

#End Region

#Region "Prevent child context menus when no active child"

    Private Sub ToolsMenu_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) _
        Handles ToolsMenu.Click, EditMenu.Click, ViewMenu.Click, FileMenu.Click, WindowsMenu.Click

        ActivateToolStrip()

    End Sub

    Private Sub ActivateToolStrip()

        ExecuteToolStripMenuItem.Enabled = hasActive()
        GeneratorSettingsToolStripMenuItem.Enabled = hasActive()

        CutToolStripMenuItem.Enabled = hasActive()
        CopyToolStripMenuItem.Enabled = hasActive()
        PasteToolStripMenuItem.Enabled = hasActive()
        SelectAllToolStripMenuItem.Enabled = hasActive()

        WrapTextToolStripMenuItem.Enabled = hasActive()
        SaveToolStripMenuItem.Enabled = hasActive()
        SaveAsToolStripMenuItem.Enabled = hasActive()
        CloseToolStripMenuItem.Enabled = hasActive()
        CaseToolStripMenuItem.Enabled = hasActive()

        CloseAllToolStripMenuItem.Enabled = frmCount() > 0
        CascadeToolStripMenuItem.Enabled = frmCount() > 0
        TileVerticalToolStripMenuItem.Enabled = frmCount() > 0
        TileHorizontalToolStripMenuItem.Enabled = frmCount() > 0
        ArrangeIconsToolStripMenuItem.Enabled = frmCount() > 0

        If hasActive() Then
            Dim f As MDIChild = ActiveFrm()
            With f
                UndoToolStripMenuItem.Enabled = .data.CanUndo
            End With
        Else
            UndoToolStripMenuItem.Enabled = False
        End If
    End Sub

    Public Sub HideChildMenu()

        With Me
            .CaseToolStripMenuItem.Enabled = False
            .ExecuteToolStripMenuItem.Enabled = False
            .GeneratorSettingsToolStripMenuItem.Enabled = False
            .CutToolStripMenuItem.Enabled = False
            .CopyToolStripMenuItem.Enabled = False
            .PasteToolStripMenuItem.Enabled = False
            .SelectAllToolStripMenuItem.Enabled = False
            .UndoToolStripMenuItem.Enabled = False
            .WrapTextToolStripMenuItem.Enabled = False
            .SaveToolStripMenuItem.Enabled = False
            .SaveAsToolStripMenuItem.Enabled = False
            .CloseToolStripMenuItem.Enabled = False
            .CloseAllToolStripMenuItem.Enabled = False
            .CascadeToolStripMenuItem.Enabled = False
            .TileVerticalToolStripMenuItem.Enabled = False
            .TileHorizontalToolStripMenuItem.Enabled = False
            .ArrangeIconsToolStripMenuItem.Enabled = False
        End With

    End Sub

#End Region

#Region "Menu Items for user settings"

    Private Sub StatusBarToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles StatusBarToolStripMenuItem.Click
        Me.StatusStrip1.Visible = Me.StatusBarToolStripMenuItem.Checked
    End Sub

    Private Sub WrapTextToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles WrapTextToolStripMenuItem.Click
        Dim f As MDIChild = ActiveFrm()
        With f
            .data.WordWrap = Not (.data.WordWrap)
            WrapTextToolStripMenuItem.Checked = .data.WordWrap
        End With
    End Sub

    Private Sub GeneratorSettingsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GeneratorSettingsToolStripMenuItem.Click
        If hasActive() Then
            Dim f As MDIChild = ActiveFrm()
            Dim opt As New frmOptions(f)
            opt.ShowDialog()
        End If
    End Sub

    Private Sub OptionsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OptionsToolStripMenuItem.Click
        Dim opt As New genOptions(Me)
        opt.int_CWidth.Value = mCwidth
        opt.ShowDialog()
    End Sub

#End Region

#Region "MDI Menu Items "

    Private Sub CascadeToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles CascadeToolStripMenuItem.Click
        Me.LayoutMdi(MdiLayout.Cascade)
    End Sub

    Private Sub TileVerticleToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles TileVerticalToolStripMenuItem.Click
        Me.LayoutMdi(MdiLayout.TileVertical)
    End Sub

    Private Sub TileHorizontalToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles TileHorizontalToolStripMenuItem.Click
        Me.LayoutMdi(MdiLayout.TileHorizontal)
    End Sub

    Private Sub ArrangeIconsToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ArrangeIconsToolStripMenuItem.Click
        Me.LayoutMdi(MdiLayout.ArrangeIcons)
    End Sub

#End Region

#Region "Close and Exit Menu Items"

    Private Sub CloseAllToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles CloseAllToolStripMenuItem.Click
        ' Close all child forms of the parent.
        For Each ChildForm As MDIChild In Me.MdiChildren
            ChildForm.Close()
        Next
        HideChildMenu()
    End Sub

    Private Sub CloseToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CloseToolStripMenuItem.Click
        If hasActive() Then
            Dim f As MDIChild = ActiveFrm()
            With f
                .Close()
            End With
        End If
    End Sub

    Private Sub ExitToolsStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ExitToolStripMenuItem.Click
        Global.System.Windows.Forms.Application.Exit()
    End Sub

#End Region

#Region "Saving Menu Items"

    Private Sub SaveToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveToolStripMenuItem.Click
        If hasActive() Then
            Dim f As MDIChild = ActiveFrm()
            Save(f)
        End If
    End Sub

    Private Sub SaveAsToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles SaveAsToolStripMenuItem.Click
        If hasActive() Then
            Dim f As MDIChild = ActiveFrm()
            With f
                Dim Fi As String = .Filename
                If SaveAs(Fi) Then
                    .Filename = Fi
                    WriteTofile(f)
                End If
            End With
        End If
    End Sub

#End Region

#Region "Save Routines"

    Private Function Save(ByVal f As MDIChild) As Boolean
        With f
            If Len(.Filename) = 0 Then
                Dim Fi As String = .Filename
                If SaveAs(Fi) Then
                    .Filename = Fi
                    WriteTofile(f)
                End If
            Else
                WriteTofile(f)
            End If
        End With
    End Function

    Private Function WriteTofile(ByVal f As MDIChild)
        Dim ret As Boolean = False
        Try
            With f
                Dim inst As New StreamWriter(.Filename)
                inst.Write(.data.Text)
                inst.Close()
                .Changed = False
            End With
            ret = True
        Catch
            ret = False
        End Try
        Return ret
    End Function

    Private Function SaveAs(ByRef FileName As String) As Boolean
        Dim ret As Boolean = False
        Dim SaveFileDialog As New SaveFileDialog
        SaveFileDialog.InitialDirectory = My.Computer.FileSystem.SpecialDirectories.MyDocuments
        SaveFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*"

        If (SaveFileDialog.ShowDialog(Me) = System.Windows.Forms.DialogResult.OK) Then
            FileName = SaveFileDialog.FileName
            ret = True
        End If
        Return ret
    End Function

    Public Function SaveChanges(ByVal f As MDIChild) As MsgBoxResult
        Dim res As MsgBoxResult = MsgBoxResult.Ok
        With f
            If .Changed Then
                Select Case MsgBox("The text in file " & .Filename & " has Changed." & vbCrLf & _
                "Do you wish to save changes?", MsgBoxStyle.YesNoCancel + MsgBoxStyle.Exclamation)
                    Case MsgBoxResult.Yes
                        Save(f)
                    Case MsgBoxResult.No

                    Case MsgBoxResult.Cancel
                        res = MsgBoxResult.Cancel
                End Select
                .Changed = False
            End If
            Return res
        End With
    End Function

#End Region

#Region "Execute and Compile"

    Private Sub ExecuteToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExecuteToolStripMenuItem.Click
        If hasActive() Then
            Dim f As MDIChild = ActiveFrm()
            With f
                .Execute()
            End With
        End If
    End Sub

    Private Sub CompileToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CompileToolStripMenuItem.Click

        Dim CountMain As Integer = -1
        Dim SortedMDI() As MDIChild = SortMDI()
        For i As Integer = 0 To UBound(SortedMDI)
            setHeader(SortedMDI(i))
            GetWhere(SortedMDI(i))
            GetCols(SortedMDI(i))

            If Not IsNothing(SortedMDI(i).Cols) Then
                DrawVarTable(SortedMDI(i), SortedMDI(i).lstVariables)
                DrawLoadingReference(SortedMDI(i), SortedMDI(i).LoadRef)
            End If

            If Not (SortedMDI(i).IsSub) Then
                CountMain += 1
            End If
        Next

        Select Case CountMain
            Case -1
                MsgBox("Compile Failed." & vbCrLf & "All windows are subs.", MsgBoxStyle.Exclamation)
                Exit Sub
            Case 0
                With SortedMDI(0).output
                    .Text = ""
                    For i As Integer = 0 To UBound(SortedMDI)
                        If Not IsNothing(UnHidden(SortedMDI(i))) Then
                            DeclareVars(SortedMDI(i), .Text)
                        End If
                    Next
                    For i As Integer = 0 To UBound(SortedMDI)
                        If Not IsNothing(UnHidden(SortedMDI(i))) Then
                            ProcStart(SortedMDI(i), .Text)
                            CursorSelect(SortedMDI(i), .Text)
                            CursorFetch(SortedMDI(i), .Text)
                            QuitOnError(SortedMDI(i), .Text)
                            .Text += "/* " & vbCrLf & "Processing of current record " & vbCrLf & "*/" & vbCrLf
                            WriteSelectVars(SortedMDI(i), .Text)
                            InsertIntoGeneralLoad(SortedMDI(i), .Text)
                            If i = 0 Then
                                For z As Integer = 1 To UBound(SortedMDI)
                                    .Text += "GOSUB " & SortedMDI(z).StartLabel & " ;" & vbCrLf
                                Next
                            End If
                            ProcEnd(SortedMDI(i), .Text)
                        End If
                    Next

                End With
                SortedMDI(0).BringToFront()
                parseWidth(SortedMDI(0))

            Case Else
                MsgBox("Compile Failed." & vbCrLf & "More than one main sub.", MsgBoxStyle.Exclamation)

        End Select

    End Sub

    Private Sub BuildOrder(ByRef Count As Integer)

        Dim sl As Integer
        Dim repeat As Boolean = True
        Dim cur As MDIChild

        'reset build Ids
        For Each ChildForm As MDIChild In Me.MdiChildren
            ChildForm.BuildID = -1
        Next

        While repeat
            sl = 9999999
            For Each ChildForm As MDIChild In Me.MdiChildren
                If ChildForm.BuildID = -1 And ChildForm.StartLabel < sl Then
                    cur = ChildForm
                    sl = ChildForm.StartLabel
                End If
            Next
            Count += 1
            cur.BuildID = Count

            repeat = False
            For Each ChildForm As MDIChild In Me.MdiChildren
                If ChildForm.BuildID = -1 Then
                    repeat = True
                    Exit For
                End If
            Next

        End While

    End Sub

    Private Function SortMDI() As MDIChild()

        Dim Count As Integer = -1
        Dim SortedMDI() As MDIChild = Nothing

        BuildOrder(Count)
        For i As Integer = 0 To Count
            For Each ChildForm As MDIChild In Me.MdiChildren
                If ChildForm.BuildID = i Then
                    Try
                        ReDim Preserve SortedMDI(UBound(SortedMDI) + 1)
                    Catch ex As Exception
                        ReDim SortedMDI(0)
                    Finally
                        SortedMDI(UBound(SortedMDI)) = ChildForm
                    End Try
                End If
            Next
        Next
        Return SortedMDI

    End Function

#End Region

    Public Sub TableDictonaryToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TableDictonaryToolStripMenuItem.Click

        Dim di As New frmTable(Me)


        di.OK_Button.Enabled = Me.hasActive
        If di.ShowDialog() = Windows.Forms.DialogResult.OK Then

            For tab As Integer = 0 To di.lst_selected.Items.Count - 1
                Dim tname As String = di.lst_selected.Items(tab)
                'GetCols(Me.ActiveFrm)                
                Dim Ordinal As Integer = 1
                Dim tex As Boolean
                Do
                    tex = False
                    Dim ta() As String = Tables(Me.ActiveFrm)
                    If Not IsNothing(ta) Then
                        For Each t As String In ta
                            If t = di.lst_Selected.Items(tab) Then
                                tex = True
                                If InStr(di.lst_Selected.Items(tab), "*") Then
                                    Ordinal += 1
                                    di.lst_Selected.Items(tab) = Split(di.lst_Selected.Items(tab), "*")(0) & "*" & Ordinal
                                Else
                                    di.lst_Selected.Items(tab) = di.lst_selected.Items(tab) & "*" & Ordinal
                                End If
                            End If
                        Next
                    End If
                Loop Until Not tex
                addTable(Me.ActiveFrm, Split("CREATE TABLE " & di.lst_selected.Items(tab) & Split(My.Resources.TableDictionary, "CREATE TABLE " & tname)(1), ";")(0) & ";" & vbCrLf)
            Next

            Dim f As MDIChild = ActiveFrm()
            With f
                GetWhere(f)
                GetCols(f)
                If Not IsNothing(.Cols) Then
                    DrawVarTable(f, .lstVariables)
                End If
            End With
        End If

    End Sub

    Private Sub AboutToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AboutToolStripMenuItem.Click
        Dim about As New AboutBox
        about.ShowDialog()
    End Sub

End Class
