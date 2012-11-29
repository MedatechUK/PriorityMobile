Imports System.Windows.Forms
Imports System.IO
Imports System.Threading
Imports System.Reflection
Imports Bind
Imports dataclasses

Public Class frmMain
    Private pi As System.Reflection.PropertyInfo
    Public ws As New PriWebSVC.Service
    Private m_ChildFormNumber As Integer
    Public EditControl As Windows.Forms.TextBox

#Region "Private Functions"
    Private Function newForm() As frmChild
        ' Create a new instance of the child form.
        Dim ChildForm As New frmChild(Me)
        ' Make it a child of this MDI form before showing it.
        ChildForm.MdiParent = Me

        m_ChildFormNumber += 1
        Return ChildForm
    End Function

    Public Function frmCount() As Integer
        Dim i As Integer = 0
        For Each ChildForm As frmChild In Me.MdiChildren
            i += 1
        Next
        Return i
    End Function

    Private Function ActiveFrm() As Object
        Return Me.ActiveMdiChild
    End Function

    Private Function hasActive() As Boolean
        Return Not IsNothing(ActiveFrm)
    End Function

#End Region

    Private Sub frmMain_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        setEditToolbar(False)
    End Sub

    Private Sub ShowNewForm(ByVal sender As Object, ByVal e As EventArgs) Handles NewToolStripMenuItem.Click, NewWindowToolStripMenuItem.Click
        ' Create a new instance of the child form.
        Dim ChildForm As New frmChild(Me)
        ' Make it a child of this MDI form before showing it.
        ChildForm.MdiParent = Me

        m_ChildFormNumber += 1
        ChildForm.Text = "Window " & m_ChildFormNumber

        ChildForm.Show()
    End Sub

#Region "Import routines"

    Private Sub FileMenu_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FileMenu.Click
        With Me
            .ImportToolStripMenuItem.Visible = False
            .ExportToolStripMenuItem.Visible = False
            .impexpblank.Visible = False
            If hasActive() Then
                If String.Compare(ActiveFrm.GetType.ToString.ToLower, "pdagenerator.frmchild", True) = 0 Then
                    .ImportToolStripMenuItem.Visible = True
                    .ExportToolStripMenuItem.Visible = True
                    .impexpblank.Visible = True
                End If
            End If
        End With
    End Sub

    Private Sub FromDLLToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FromDLLToolStripMenuItem.Click
        If hasActive() Then
            If ActiveFrm.GetType.ToString.ToLower = "pdagenerator.frmchild" Then
                Dim OpenFileDialog As New OpenFileDialog
                'OpenFileDialog.InitialDirectory = My.Computer.FileSystem.SpecialDirectories.MyDocuments
                OpenFileDialog.Filter = "Dynamic Link Libraries (*.dll)|*.dll"
                If (OpenFileDialog.ShowDialog(Me) = System.Windows.Forms.DialogResult.OK) Then
                    Dim FileName As String = OpenFileDialog.FileName
                    If File.Exists(FileName) Then
                        LoadDLL(FileName)
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub FromXMLToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FromXMLToolStripMenuItem.Click
        If hasActive() Then
            If ActiveFrm.GetType.ToString.ToLower = "pdagenerator.frmchild" Then
                Dim OpenFileDialog As New OpenFileDialog
                'OpenFileDialog.InitialDirectory = My.Computer.FileSystem.SpecialDirectories.MyDocuments
                OpenFileDialog.Filter = "XML Files (*.xml)|*.xml"
                If (OpenFileDialog.ShowDialog(Me) = System.Windows.Forms.DialogResult.OK) Then
                    Dim FileName As String = OpenFileDialog.FileName
                    If File.Exists(FileName) Then
                        LoadXML(FileName)
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub LoadDLL(ByVal Filename As String)

        Dim t() As System.Type = {}
        Dim args() As Object = {}

        Dim TargetAssembly As Assembly
        TargetAssembly = Assembly.LoadFrom(Filename)

        Dim frm As frmChild = ActiveFrm()
        With frm
            For Each mi As System.Type In TargetAssembly.GetTypes
                Select Case mi.BaseType.Name
                    Case "DatasetObjectBase"

                        Dim prototype As Object = Activator.CreateInstance(mi)
                        Dim ConQuery As String = mi.GetMethod("ConQuery", t).Invoke(prototype, args)
                        Dim Columns() As String = mi.GetMethod("Columns", t).Invoke(prototype, args)
                        Dim KeyColumns() As String = mi.GetMethod("KeyColumns", t).Invoke(prototype, args)
                        Dim UpdateColumns() As String = mi.GetMethod("UpdateColumns", t).Invoke(prototype, args)

                        .SQL.Add(New SQLItem(mi.Name, ConQuery))

                        Dim node As New TreeNode
                        With node
                            .ImageIndex = 2
                            .SelectedImageIndex = 3
                            .Tag = "table"
                            .Name = mi.Name
                            .Text = mi.Name
                        End With
                        .library.Nodes.Item(0).Nodes.Add(node)
                        Dim i As Integer = 1
                        For Each pi As System.Reflection.PropertyInfo In mi.GetProperties
                            If pi.CanWrite Then
                                Dim keyCOL As String = "N"
                                Dim updateCol As String = "Y"
                                If Array.IndexOf(KeyColumns, pi.Name) >= 0 Then keyCOL = "Y"
                                If Not (Array.IndexOf(UpdateColumns, pi.Name) >= 0) Then updateCol = "N"
                                .cols.Add(New ColumnItem(mi.Name, pi.Name, pi.PropertyType.Name, i, keyCOL, updateCol))
                                i += 1
                            End If
                        Next

                End Select
            Next

            .Show()

        End With
    End Sub

    Private Sub LoadXML(ByVal Filename As String)
        Dim child As frmChild = ActiveFrm()
        Dim xmlconfig = New xmlConfiguration(Filename)
        For Each f As xmlForm In xmlconfig.forms.values
            AddChild(child.library.Nodes(1), f)
        Next
    End Sub

    Private Sub AddChild(ByVal N As Windows.Forms.TreeNode, ByVal f As xmlForm)
        Dim child As frmChild = ActiveFrm()
        Dim newnode As New TreeNode
        With newnode
            .ImageIndex = 4
            .SelectedImageIndex = 5
            .Tag = "form"
            .Name = f.Name
            .Text = f.Name
        End With
        Dim table As String = f.SQLFrom
        N.Nodes.Add(newnode)
        With f
            child.field.Add(New FormItem(newnode.FullPath, .Name, .Filter, .DefaultView, .SQLFrom, .IsReadOnly))
        End With
        For Each t As xmlTab In f.Tabs.Values
            Dim tabnode As New TreeNode
            With tabnode
                .ImageIndex = 6
                .SelectedImageIndex = 7
                .Tag = "tab"
                .Name = f.Name & "_" & t.Name
                .Text = t.Name
            End With
            newnode.Nodes.Add(tabnode)
            Dim i As Integer = 1
            For Each c As xmlField In t.fields.Values
                With c
                    child.intitem.Add(New InterfaceItem(tabnode.FullPath, .Name, table, .Column, .FieldStyle, .IsReadOnly, .Hidden, .Mandatory, .ListSource, .ListTextCol, .ListValueCol, "", "", i))
                    i += 1
                End With
            Next
        Next
        For Each sf As xmlForm In f.SubForm.Values
            AddChild(newnode, sf)
        Next
    End Sub

#End Region

#Region "Export routines"

    Private Sub XMLToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles XMLToolStripMenuItem.Click
        If hasActive() Then
            If ActiveFrm.GetType.ToString.ToLower = "pdagenerator.frmchild" Then
                Dim Childfrm As frmChild = ActiveFrm()
                Dim SaveFileDialog As New SaveFileDialog
                SaveFileDialog.InitialDirectory = My.Computer.FileSystem.SpecialDirectories.MyDocuments
                SaveFileDialog.Filter = "XML File (*.XML)|*.XML"
                Dim res As System.Windows.Forms.DialogResult = SaveFileDialog.ShowDialog(Me)
                If (res = System.Windows.Forms.DialogResult.OK) Then
                    Using sw As New StreamWriter(SaveFileDialog.FileName)
                        sw.Write(Childfrm.xmlOutput)
                    End Using
                End If
            End If
        End If
    End Sub

    Private Sub ToSingleFileToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToSingleFileToolStripMenuItem.Click
        If hasActive() Then
            If ActiveFrm.GetType.ToString.ToLower = "pdagenerator.frmchild" Then
                Dim Childfrm As frmChild = ActiveFrm()
                Dim SaveFileDialog As New SaveFileDialog
                SaveFileDialog.InitialDirectory = My.Computer.FileSystem.SpecialDirectories.MyDocuments
                SaveFileDialog.Filter = ".net class File (*.vb)|*.vb"
                Dim res As System.Windows.Forms.DialogResult = SaveFileDialog.ShowDialog(Me)
                If (res = System.Windows.Forms.DialogResult.OK) Then
                    Using sw As New StreamWriter(SaveFileDialog.FileName)
                        sw.Write("Imports Bind" & vbCrLf & Replace(Childfrm.xmlOutput, "Imports Bind" & vbCrLf, ""))
                    End Using
                End If
            End If
        End If
    End Sub

    Private Sub OneClassPerFileToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OneClassPerFileToolStripMenuItem.Click
        If hasActive() Then
            If ActiveFrm.GetType.ToString.ToLower = "pdagenerator.frmchild" Then
                Dim Childfrm As frmChild = ActiveFrm()
                Dim SaveFileDialog As New FolderBrowserDialog
                SaveFileDialog.SelectedPath = My.Computer.FileSystem.SpecialDirectories.MyDocuments
                Dim res As System.Windows.Forms.DialogResult = SaveFileDialog.ShowDialog(Me)
                Dim out() As String = Split(Childfrm.vbOutput, vbCrLf & vbCrLf)
                If (res = System.Windows.Forms.DialogResult.OK) Then
                    For Each o As String In out
                        If o.Length > 0 Then
                            Dim name As String = Split(Split(o, "Public Class ")(1), vbCrLf)(0) & ".vb"
                            Using sw As New StreamWriter(SaveFileDialog.SelectedPath & "\" & name)
                                sw.Write(o)
                            End Using
                        End If
                    Next
                End If
            End If
        End If
    End Sub

#End Region

#Region "Save project to File"

    Private Function saveNodes(ByVal tree As Windows.Forms.TreeView) As String
        Dim str As String = ""
        For Each n As Windows.Forms.TreeNode In tree.Nodes
            saveNode(n, str)
        Next
        If Strings.Right(str, 2) = vbCrLf Then str = Strings.Left(str, str.Length - 2)
        Return str
    End Function

    Private Sub saveNode(ByVal n As Windows.Forms.TreeNode, ByRef Str As String)
        With n
            If InStr(.FullPath, "\") > 0 Then
                Str += String.Format("{1}{0}{2}{0}{3}{0}{4}{0}{5}{0}{6}{7}", _
                            Chr(9), _
                            .FullPath, _
                            .ImageIndex, _
                            .SelectedImageIndex, _
                            .Tag, _
                            .Text, _
                            .Name, _
                            vbCrLf _
                        )
            End If
            For Each sn As Windows.Forms.TreeNode In n.Nodes
                saveNode(sn, Str)
            Next
        End With
    End Sub

    Private Sub SaveAsToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles SaveAsToolStripMenuItem.Click
        Select Case ActiveFrm().GetType.ToString.ToLower
            Case "pdagenerator.frmchild"
                Dim frm As frmChild = ActiveFrm()
                If SaveDialog(frm.FileName) = Windows.Forms.DialogResult.OK Then
                    saveProj(frm)
                End If
        End Select
    End Sub

    Public Sub SaveToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveToolStripMenuItem.Click
        Select Case ActiveFrm().GetType.ToString.ToLower
            Case "pdagenerator.frmchild"
                Dim frm As frmChild = ActiveFrm()
                If Not IsNothing(frm.FileName) Then
                    saveProj(frm)
                Else
                    If SaveDialog(frm.FileName) = Windows.Forms.DialogResult.OK Then
                        saveProj(frm)
                    End If
                End If
        End Select

    End Sub

    Private Function SaveDialog(ByRef filename As String) As System.Windows.Forms.DialogResult
        Dim SaveFileDialog As New SaveFileDialog
        SaveFileDialog.InitialDirectory = My.Computer.FileSystem.SpecialDirectories.MyDocuments
        SaveFileDialog.Filter = "Priority Mobile SDK file (*.mob)|*.mob"
        Dim res As System.Windows.Forms.DialogResult = SaveFileDialog.ShowDialog(Me)
        If (res = System.Windows.Forms.DialogResult.OK) Then
            filename = SaveFileDialog.FileName
        End If
        Return res
    End Function

    Private Sub saveProj(ByVal frm As frmChild)
        ' TODO: Add code here to save the current contents of the form to a file.        
        With frm
            .Text = .FileName
            Using sw As New StreamWriter(.FileName)
                sw.Write( _
                    String.Format("{0}{5}{1}{5}{2}{5}{3}{5}{4}", _
                        saveNodes(.library), _
                        .cols.ToString, _
                        .SQL.ToString, _
                        .intitem.ToString, _
                        .field.ToString, _
                        vbCrLf & vbCrLf _
                    ) _
                )
            End Using
        End With
    End Sub

#End Region

#Region "Load Project file"

    Private Sub OpenFile(ByVal sender As Object, ByVal e As EventArgs) Handles OpenToolStripMenuItem.Click
        Dim OpenFileDialog As New OpenFileDialog
        'OpenFileDialog.InitialDirectory = My.Computer.FileSystem.SpecialDirectories.MyDocuments
        OpenFileDialog.Filter = "Priority Mobile SDK file (*.mob)|*.mob"
        If (OpenFileDialog.ShowDialog(Me) = System.Windows.Forms.DialogResult.OK) Then
            Dim FileName As String = OpenFileDialog.FileName
            If File.Exists(FileName) Then
                LoadProj(FileName)
            End If
        End If
    End Sub

    Private Sub LoadProj(ByVal filename As String)

        ' Create a new instance of the child form.
        Dim ChildForm As New frmChild(Me)
        ' Make it a child of this MDI form before showing it.
        With ChildForm
            ChildForm.MdiParent = Me

            m_ChildFormNumber += 1
            .Text = filename
            .FileName = filename

            Dim p() As String
            Using sr As New StreamReader(filename)
                p = Split(sr.ReadToEnd, vbCrLf & vbCrLf)
            End Using
            LoadNodes(Split(p(0), vbCrLf), ChildForm)

            For Each s As String In Split(p(1), vbCrLf)
                .cols.Add(s)
            Next
            For Each s As String In Split(p(2), vbCrLf)
                .SQL.Add(s)
            Next
            For Each s As String In Split(p(3), vbCrLf)
                .intitem.Add(s)
            Next
            For Each s As String In Split(p(4), vbCrLf)
                .field.Add(s)
            Next

            .Show()

        End With
    End Sub

    Private Sub LoadNodes(ByVal lines() As String, ByVal frm As frmChild)
        Dim nodes As New Dictionary(Of String, Windows.Forms.TreeNode)
        frm.library.Nodes(0).Nodes.Clear()
        frm.library.Nodes(1).Nodes.Clear()
        nodes.Add("Tables", frm.library.Nodes(0))
        nodes.Add("Forms", frm.library.Nodes(1))

        For Each l As String In lines
            If l.Length > 0 Then
                Dim n As New Windows.Forms.TreeNode
                Dim f() As String = Split(l, Chr(9))
                With n
                    .ImageIndex = f(1)
                    .SelectedImageIndex = f(2)
                    .Tag = f(3)
                    .Text = f(4)
                    .Name = f(5)
                End With
                nodes.Add(f(0), n)
                nodes(ParentPath(f(0))).Nodes.Add(n)
            End If
        Next
    End Sub

    Private Function ParentPath(ByVal str As String) As String
        Dim ret As String = ""
        Dim s() As String = Split(str, "\")
        For i As Integer = 0 To UBound(s) - 1
            ret += s(i)
            If i < UBound(s) - 1 Then
                ret += "\"
            End If
        Next
        Return ret
    End Function

#End Region

#Region "Mdi Toolstip clicks"

    Private Sub ExitToolsStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

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

    Private Sub CloseAllToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles CloseAllToolStripMenuItem.Click
        ' Close all child forms of the parent.
        For Each ChildForm As Form In Me.MdiChildren
            ChildForm.Close()
        Next
    End Sub

#End Region

#Region "Text Editing Menu Items"

    Private Sub EditMenu_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EditMenu.Click
        EditControl = Nothing
        setEditToolbar(False)
        If hasActive() Then
            Select Case ActiveFrm.GetType.ToString.ToLower
                Case "pdagenerator.frmchild"
                    Dim c As frmChild = ActiveFrm()
                    If c.Editable Then
                        EditControl = c.editControl
                        setEditToolbar(True)
                    End If
                Case "pdagenerator.output"
                    Dim c As Output = ActiveFrm()
                    If c.Editable Then
                        EditControl = c.editControl
                        setEditToolbar(True)
                    End If
            End Select
        End If
    End Sub

    Public Sub setEditToolbar(ByVal Value As Boolean)
        With Me
            .CaseToolStripMenuItem.Enabled = Value
            .CutToolStripMenuItem.Enabled = Value
            .CopyToolStripMenuItem.Enabled = Value
            .PasteToolStripMenuItem.Enabled = Value
            .SelectAllToolStripMenuItem.Enabled = Value
            .UndoToolStripMenuItem.Enabled = Value
            .UpperToolStripMenuItem.Enabled = Value
            .LowerToolStripMenuItem.Enabled = Value
        End With
    End Sub

    Public Sub CutToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles CutToolStripMenuItem.Click
        EditControl.Cut()
    End Sub

    Public Sub CopyToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles CopyToolStripMenuItem.Click
        EditControl.Copy()
    End Sub

    Public Sub PasteToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles PasteToolStripMenuItem.Click
        EditControl.Paste()
    End Sub

    Public Sub UndoToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UndoToolStripMenuItem.Click
        EditControl.Undo()
    End Sub

    Public Sub SelectAllToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SelectAllToolStripMenuItem.Click
        EditControl.SelectAll()
    End Sub

    Public Sub UpperToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UpperToolStripMenuItem.Click
        If EditControl.SelectionLength = 0 Then
            EditControl.Text = UCase(EditControl.Text)
        Else
            EditControl.Cut()
            Clipboard.SetText(UCase(Clipboard.GetText))
            EditControl.Paste()
        End If
    End Sub

    Public Sub LowerToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LowerToolStripMenuItem.Click
        If EditControl.SelectionLength = 0 Then
            EditControl.Text = LCase(EditControl.Text)
        Else
            EditControl.Cut()
            Clipboard.SetText(LCase(Clipboard.GetText))
            EditControl.Paste()
        End If
    End Sub

#End Region

#Region "tools"

    Private Sub BuildToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) _
        Handles BuildToolStripMenuItem.Click
        If Not IsNothing(ActiveFrm) Then
            Dim ChildForm As Output = Nothing
            Select Case ActiveFrm.GetType.ToString
                Case "PDAGenerator.frmChild"
                    Dim c As frmChild = ActiveFrm()

                    For Each o As Object In Me.MdiChildren
                        If o.GetType.ToString = "PDAGenerator.Output" Then
                            If o.OutputID = c.OutputID Then
                                ChildForm = o
                                Exit For
                            End If
                        End If
                    Next
                    If IsNothing(ChildForm) Then
                        ' Create a new instance of the child form.
                        ChildForm = New Output(Me)
                        c.OutputID = ChildForm.OutputID
                        ' Make it a child of this MDI form before showing it.
                        ChildForm.MdiParent = Me
                        m_ChildFormNumber += 1
                    End If

                    Dim fn() As String = Split(c.FileName, "\")
                    With ChildForm
                        Select Case c.topNode
                            Case "tables"
                                .Text = ".net data objects - " & fn(UBound(fn))
                                .strOutput.Text = c.vbOutput
                            Case "forms"
                                .Text = "XML Interface - " & fn(UBound(fn))
                                .strOutput.Text = c.xmlOutput
                        End Select
                        .Show()
                        .BringToFront()
                    End With

            End Select
        End If
    End Sub

    Private Sub SetHostToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) _
        Handles SetHostToolStripMenuItem.Click
        With Process
            With .StartInfo
                .Arguments = String.Format("{0}{1}{0}", Chr(34), Environment.SystemDirectory & "\drivers\etc\hosts")
            End With
            .Start()
        End With
    End Sub

    Private Sub NSLookupToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) _
        Handles NSLookupToolStripMenuItem.Click
        Dim dlg As New NSLookup
        dlg.ShowDialog()
    End Sub

    Private Sub AboutToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AboutToolStripMenuItem.Click
        Dim dlg As New About
        dlg.ShowDialog()
    End Sub

#End Region

End Class
