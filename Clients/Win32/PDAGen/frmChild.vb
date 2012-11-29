Imports System.Text.RegularExpressions
Imports Bind
Imports Loading
Imports dataclasses

Public Class frmChild

    Private mCaller As frmMain
    Public WithEvents cols As oBind
    Public WithEvents SQL As oBind
    Public WithEvents intitem As oBind
    Public WithEvents field As oBind
    Private ret As String

    Dim App As String = "SDK"
    Dim ev As ntEvtlog.evt = Nothing '(EvtLogMode.File, EvtLogVerbosity.Arcane, App)
    Private ds As New oDataSet("DataClasses.dll", App, ev)
    Private QueryConst As New Dictionary(Of String, String)

    ' Set datagrid column styles 
    ' Define comboCountry variable as a ComboBoxColumn column style object of DataGridComboBoxColumn class 
    Private WithEvents comboType As Windows.Forms.DataGridViewComboBoxColumn
    ' Define comboCapital variable as a ComboBoxColumn column style object of DataGridComboBoxColumn class 
    Private WithEvents comboColumn As Windows.Forms.DataGridViewComboBoxColumn

#Region "Properties"

    Public ReadOnly Property Editable() As Boolean
        Get
            Return txtSQL.Focused
        End Get
    End Property

    Public ReadOnly Property editControl() As Windows.Forms.TextBox
        Get
            Return txtSQL
        End Get
    End Property

    Public ReadOnly Property vbOutput() As String
        Get
            mCaller.StatusStrip.Items(0).Text = "Processing ..."
            System.Windows.Forms.Application.DoEvents()
            ret = ""
            For Each t As Windows.Forms.TreeNode In Me.library.Nodes(0).Nodes
                Select Case LCase(t.Tag)
                    Case "table"
                        Dim cont As Boolean = False
                        Dim v() As ColumnItem = Nothing
                        Dim k() As ColumnItem = Nothing
                        Dim u() As ColumnItem = Nothing

                        With Me.BindSource
                            '.RemoveSort()
                            .RemoveFilter()                            
                            .Filter = "TABLE='" & t.Text & "'"
                            .ResetBindings(False)
                            .Sort = "ORD"
                        End With

                        For Each ci As ColumnItem In cols.FilterObject
                            'If String.Compare(ci.TABLE, t.Text) = 0 Then
                            With ci
                                Try
                                    ReDim Preserve v(UBound(v) + 1)
                                Catch
                                    ReDim v(0)
                                Finally
                                    v(UBound(v)) = ci
                                End Try
                                cont = True
                            End With
                            'End If
                        Next
                        If cont Then
                            cont = False
                            For Each ci As ColumnItem In v
                                If String.Compare(ci.KEYCOL, "Y") = 0 Then
                                    With ci
                                        Try
                                            ReDim Preserve k(UBound(k) + 1)
                                        Catch
                                            ReDim k(0)
                                        Finally
                                            k(UBound(k)) = ci
                                        End Try
                                        cont = True
                                    End With
                                End If
                            Next
                            If Not cont Then
                                MsgBox(String.Format("The table {0} contains no key columns.", t.Name))
                            End If
                        End If
                        If cont Then
                            cont = False
                            For Each ci As ColumnItem In v
                                If String.Compare(ci.UPDATECOL, "Y") = 0 Then
                                    With ci
                                        Try
                                            ReDim Preserve u(UBound(u) + 1)
                                        Catch
                                            ReDim u(0)
                                        Finally
                                            u(UBound(u)) = ci
                                        End Try
                                        cont = True
                                    End With
                                End If
                            Next
                            If Not cont Then
                                MsgBox(String.Format("The table {0} contains no updatable columns.", t.Name))
                            End If
                        End If

                        If cont Then
                            wr("Imports Bind", True)
                            wr(String.Format("Public Class {0}", t.Text), True)
                            wr(String.Format("Inherits DatasetObjectBase", ""), True)
                            wr(String.Format("#Region {0}Private Variables{0}", Chr(34)), True)
                            For i As Integer = 0 To UBound(v)
                                If Not IsNothing(v(i)) Then
                                    wr(String.Format("Private _{0} As {1}", v(i).COLNAME, v(i).COLTYPE), True)
                                End If
                            Next
                            wr(String.Format("#End Region", ""), True)

                            wr(String.Format("#Region {0}Initialisation{0}", Chr(34)), True)
                            wr(String.Format("Public Sub New(){0}MyBase.New(){0}End Sub", vbCrLf), True)
                            wr(String.Format("Public Sub New(", ""), False)
                            For i As Integer = 0 To UBound(v)
                                wr(String.Format("ByVal {0} As {1}", v(i).COLNAME, v(i).COLTYPE), False)
                                If i < UBound(v) Then
                                    wr(String.Format(", ", "", False))
                                End If
                            Next
                            wr(String.Format(")", ""), True)
                            wr(String.Format("MyBase.New()", ""), True)
                            For i As Integer = 0 To UBound(v)
                                wr(String.Format("_{0} = {0}", v(i).COLNAME), True)
                            Next
                            wr(String.Format("end sub{0}#End Region", vbCrLf), True)

                            wr(String.Format("#Region {0}Overrides{0}", Chr(34)), True)
                            wr(String.Format("Public Overrides Function ConQuery() As String", Chr(34)), True)
                            wr(String.Format("' The SQL statement to populate the dataset from.", Chr(34)), True)
                            wr(String.Format("' Return Nothing if the dataset is not updated from the SOAP service.", Chr(34)), True)
                            Dim sqli As SQLItem = SQL.Item(t.Name)
                            If IsNothing(sqli) Then
                                wr(String.Format("Return Nothing", Chr(34)), True)
                            ElseIf sqli.SQL.Trim.Length = 0 Then
                                wr(String.Format("Return Nothing", Chr(34)), True)
                            Else
                                Dim l() As String = Split(sqli.SQL, vbCrLf)
                                Dim cnt As Integer = 0
                                wr("Return _", True)
                                For Each st As String In l
                                    wr(String.Format("{0}{1} {0}", Chr(34), st), False)
                                    If cnt < UBound(l) Then
                                        wr(" & _", True)
                                    End If
                                    cnt += 1
                                Next
                                wr("", True)
                            End If
                            wr(String.Format("End Function", Chr(34)), True)
                            wr(String.Format("Public Overrides Function Columns() As String()", Chr(34)), True)
                            wr(String.Format("' List of columns in the table", Chr(34)), True)
                            wr("Dim myCols() As String = { _", True)
                            For i As Integer = 0 To UBound(v)
                                If Not IsNothing(v(i)) Then
                                    wr(String.Format("{0}{1}{0}", Chr(34), v(i).COLNAME))
                                End If
                                If i < UBound(v) Then
                                    wr(", ")
                                End If
                            Next
                            wr(" _", True)
                            wr("}", True)
                            wr(String.Format("Return myCols", Chr(34)), True)
                            wr(String.Format("End Function", Chr(34)), True)

                            wr(String.Format("Public Overrides Function KeyColumns() As String()", Chr(34)), True)
                            wr(String.Format("' List of colums that combine to create a unique key", Chr(34)), True)
                            wr("Dim myCols() As String = { _", True)
                            For i As Integer = 0 To UBound(k)
                                If Not IsNothing(k(i)) Then
                                    wr(String.Format("{0}{1}{0}", Chr(34), k(i).COLNAME))
                                End If
                                If i < UBound(k) Then
                                    wr(", ")
                                End If
                            Next
                            wr(" _", True)
                            wr("}", True)
                            wr(String.Format("Return myCols", Chr(34)), True)
                            wr(String.Format("End Function", Chr(34)), True)

                            wr(String.Format("Public Overrides Function UpdateColumns() As String()", Chr(34)), True)
                            wr(String.Format("' List of columns updated by a SOAP Syncronisation.", Chr(34)), True)
                            wr("Dim myCols() As String = { _", True)
                            For i As Integer = 0 To UBound(u)
                                If Not IsNothing(u(i)) Then
                                    wr(String.Format("{0}{1}{0}", Chr(34), u(i).COLNAME))
                                End If
                                If i < UBound(u) Then
                                    wr(", ")
                                End If
                            Next
                            wr(" _", True)
                            wr("}", True)
                            wr(String.Format("Return myCols", Chr(34)), True)
                            wr(String.Format("End Function", Chr(34)), True)

                            wr(String.Format("#End Region", Chr(34)), True)


                            wr(String.Format("#Region {0}Public Properties{0}", Chr(34)), True)
                            For i As Integer = 0 To UBound(v)
                                wr(String.Format("Public Property {0}() As {1}", v(i).COLNAME, v(i).COLTYPE, Chr(34)), True)
                                wr(String.Format("Get", v(i).COLNAME, Chr(34)), True)
                                wr(String.Format("Return _{0}", v(i).COLNAME, Chr(34)), True)
                                wr(String.Format("End Get", v(i).COLNAME, Chr(34)), True)
                                wr(String.Format("Set(ByVal value As {0})", v(i).COLTYPE, Chr(34)), True)
                                wr(String.Format("If _{0} <> value Then", v(i).COLNAME, Chr(34)), True)
                                wr(String.Format("OnPropertyChanging({1}{0}{1}, value)", v(i).COLNAME, Chr(34)), True)
                                wr(String.Format("If Not GetCancelEdit() Then", v(i).COLNAME, Chr(34)), True)
                                wr(String.Format("_{0} = value", v(i).COLNAME, Chr(34)), True)
                                wr(String.Format("OnPropertyChanged({1}{0}{1})", v(i).COLNAME, Chr(34)), True)
                                wr(String.Format("End If", v(i).COLNAME, Chr(34)), True)
                                wr(String.Format("End If", v(i).COLNAME, Chr(34)), True)
                                wr(String.Format("End Set", v(i).COLNAME, Chr(34)), True)
                                wr(String.Format("End Property", v(i).COLNAME, Chr(34)), True)
                            Next
                            wr(String.Format("#End Region", ""), True)
                            wr(String.Format("End Class", ""), True)
                            wr(String.Format("", ""), True)
                        End If

                End Select
            Next
            mCaller.StatusStrip.Items(0).Text = "Done."
            System.Windows.Forms.Application.DoEvents()
            Return ret
        End Get
    End Property

    Public ReadOnly Property xmlOutput() As String
        Get
            mCaller.StatusStrip.Items(0).Text = "Processing ..."
            System.Windows.Forms.Application.DoEvents()
            ret = ""
            wr(String.Format("<?xml version={0}1.0{0} encoding={0}utf-8{0}?>", Chr(34)), True)
            wr("<Configuration>", True)
            wr(String.Format("<version major={0}1{0} minor={0}1{0}/>", Chr(34)), True)
            For Each node As Windows.Forms.TreeNode In library.Nodes(1).Nodes
                xml(node)
            Next
            wr("</Configuration>", True)
            mCaller.StatusStrip.Items(0).Text = "Done."
            System.Windows.Forms.Application.DoEvents()
            Return ret
        End Get
    End Property

    Private Sub xml(ByVal node As Windows.Forms.TreeNode)

        Dim hiddenFields As New List(Of String)
        For Each fi As FormItem In field.OriginalList
            If String.Compare(fi.FullPath, node.FullPath) = 0 Then
                With fi
                    Dim B As oBind = ds.DataSet("ColumnItem")
                    For Each ci As ColumnItem In B.OriginalList
                        If String.Compare(fi.SQLFrom, ci.TABLE, True) = 0 Then
                            hiddenFields.Add(ci.COLNAME)
                        End If
                    Next
                    wr(String.Format("<form name={0}{1}{0} DefaultView={0}{2}{0} readonly={0}{3}{0} from={0}{4}{0} where={0}{5}{0}>", _
                        Chr(34), _
                        fi.Name, _
                        ViewName(fi.DefaultView), _
                        CStr(fi.IsReadOnly), _
                        fi.SQLFrom, _
                        fi.Filter), _
                        True)
                End With
                For Each subnode As Windows.Forms.TreeNode In node.Nodes
                    If subnode.Tag = "tab" Then
                        wr(String.Format("<tab name={0}{1}{0}>", _
                            Chr(34), _
                            subnode.Text _
                            ), _
                        True)

                        With Me.fieldSource
                            .Sort = "ORD"
                            .Filter = "FullPath='" & subnode.FullPath & "'"
                            '.ResetBindings(False)
                        End With

                        For Each ii As InterfaceItem In Me.fieldSource 'intitem.FilterObject
                            With ii
                                'If String.Compare(.FullPath, subnode.FullPath) = 0 Then
                                With hiddenFields
                                    If .Contains(ii.Column) Then
                                        .Remove(ii.Column)
                                    End If
                                End With
                                wr(String.Format("<field name={0}{1}{0} fieldstyle={0}{2}{0} mandatory={0}{3}{0} hidden={0}{4}{0} readonly={0}{5}{0} regex={0}{6}{0} ", _
                                    Chr(34), _
                                    .Name, _
                                    FieldStyleName(CInt(.FieldStyle)), _
                                    CStr(.Mandatory), _
                                    CStr(.Hidden), _
                                    CStr(.IsReadOnly), _
                                    .REGEX _
                                    ), False)
                                If String.Compare(FieldStyleName(CInt(.FieldStyle)), "list", True) = 0 Then
                                    wr(String.Format("ListSource={0}{1}{0} ListValueCol={0}{2}{0} ListTextCol={0}{3}{0} ListFilter={0}{4}{0}", _
                                        Chr(34), _
                                        .ListSource, _
                                        .ListValueCol, _
                                        .ListTextCol, _
                                        .ListFilter _
                                    ), False)
                                End If
                                wr(String.Format(">{0}</field>", .Column), True)
                                'End If
                            End With
                        Next
                        wr("</tab>", True)
                    End If
                Next
                If hiddenFields.Count > 0 Then
                    wr(String.Format("<tab name={0}{1}{0}>", _
                        Chr(34), _
                        "Hidden" _
                        ), _
                    True)
                    For Each h As String In hiddenFields
                        wr(String.Format("<field name={0}{1}{0} fieldstyle={0}{2}{0} mandatory={0}{3}{0} hidden={0}{4}{0} readonly={0}{5}{0} regex={0}{6}{0} ", _
                            Chr(34), _
                            h, _
                            FieldStyleName(xfFieldStyle.xfText), _
                            CStr(False), _
                            CStr(True), _
                            CStr(True), _
                            "" _
                            ), False)
                        wr(String.Format(">{0}</field>", h), True)
                    Next
                    wr("</tab>", True)
                End If
                For Each subnode As Windows.Forms.TreeNode In node.Nodes
                    If subnode.Tag = "form" Then
                        xml(subnode)
                    End If
                Next
                wr("</form>", True)
            End If
        Next
    End Sub

    Private Function ViewName(ByVal view As xfView) As String
        Select Case view
            Case xfView.xfForm
                Return "form"
            Case xfView.xfTable
                Return "Table"
            Case xfView.xfHtml
                Return "HTML"
            Case xfView.xfSignature
                Return "Signature"
            Case Else
                Return ""
        End Select
    End Function

    Private Function FieldStyleName(ByVal style As xfFieldStyle)
        Select Case style
            Case xfFieldStyle.xfText
                Return "text"
            Case xfFieldStyle.xfCombo
                Return "list"
            Case xfFieldStyle.xfDate
                Return "date"
            Case xfFieldStyle.xfBool
                Return "bool"
            Case Else
                Return ""
        End Select
    End Function

    Private Sub wr(ByVal str As String, Optional ByVal endline As Boolean = False)
        ret += Replace(Str, "int32", "Integer", , , CompareMethod.Text)
        If endline Then ret += vbCrLf
    End Sub

    Private _FileName As String = Nothing
    Public Property FileName() As String
        Get
            Return _FileName
        End Get
        Set(ByVal value As String)
            _FileName = value
        End Set
    End Property

    Private _OutputID As String = Nothing
    Public Property OutputID() As String
        Get
            Return _OutputID
        End Get
        Set(ByVal value As String)
            _OutputID = value
        End Set
    End Property

    Public ReadOnly Property topNode() As String
        Get
            Dim n As Windows.Forms.TreeNode = library.SelectedNode
            Do Until n.FullPath.ToLower = "tables" Or n.FullPath.ToLower = "forms"
                n = n.Parent
            Loop
            Return n.FullPath.ToLower
        End Get
    End Property

#End Region

#Region "Initialisation / finalisation"
    Public Sub New(ByRef Caller As frmMain)
        InitializeComponent()
        mCaller = Caller
        cols = ds.DataSet("ColumnItem")
        SQL = ds.DataSet("SQLItem")
        intitem = ds.DataSet("InterfaceItem")
        field = ds.DataSet("FormItem")
    End Sub

    Private Sub frmChild_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        With Me
            '.library.Width = My.Settings.libWidth
            '.FieldProperties.Width = My.Settings.propWidth
            '.FormDetail.Width = My.Settings.propWidth
            .WindowState = Windows.Forms.FormWindowState.Maximized

            .BindSource.DataSource = .cols.FilterObject
            .fieldSource.DataSource = .intitem.FilterObject
            With .ColumnView
                .DataSource = Me.BindSource
                With .Columns
                    ' Set datagrid ComboBox Column Style for Country field 
                    comboType = New Windows.Forms.DataGridViewComboBoxColumn()
                    With comboType
                        .DataPropertyName = "COLTYPE"
                        .HeaderText = "TYPE"
                        With .Items
                            .Clear()
                            .Add("String")
                            .Add("Int32")
                        End With
                    End With
                    .Add(comboType)
                End With
                .AutoResizeColumns()
            End With
            With .KeyView
                .DataSource = Me.BindSource
            End With
            With .UpdateView
                .DataSource = Me.BindSource
            End With
            With .FieldView
                .DataSource = Me.fieldSource
                With .Columns
                    ' Set datagrid ComboBox Column Style for Country field 
                    comboColumn = New Windows.Forms.DataGridViewComboBoxColumn()
                    With comboColumn
                        .DataPropertyName = "Column"
                        .HeaderText = "ColumnName"
                        With .Items
                            .Clear()
                        End With
                    End With
                    .Add(comboColumn)
                End With
            End With
        End With
    End Sub

    Private Sub frmChild_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Select Case MsgBox(String.Format("Save file {0} before closing?", Me.Text), MsgBoxStyle.YesNoCancel)
            Case MsgBoxResult.Yes
                mCaller.SaveToolStripMenuItem_Click(Me, New System.EventArgs)
            Case MsgBoxResult.Cancel
                e.Cancel = True
        End Select
    End Sub

#End Region

#Region "Opening Context menu"
    Private Sub msField_Opening(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles msField.Opening
        With Me
            .RemoveFieldToolStripMenuItem.Visible = Me.FieldView.SelectedRows.Count > 0
            .MoveFieldDownToolStripMenuItem.Visible = False
            .MoveFieldUpToolStripMenuItem.Visible = False
            If .FieldView.SelectedRows.Count > 0 Then
                If Me.FieldView.SelectedRows(0).Index > 0 Then
                    .MoveFieldUpToolStripMenuItem.Visible = True
                End If
                If .FieldView.SelectedRows(0).Index < .FieldView.RowCount - 1 Then
                    .MoveFieldDownToolStripMenuItem.Visible = True
                End If
            End If
        End With
    End Sub
    Private Sub ContextMenuStrip1_Opening(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ContextMenuStrip1.Opening
        Dim first As Boolean = False
        Dim last As Boolean = False
        Dim only As Boolean = False
        With Me
            .NewKeyColumnToolStripMenuItem.Visible = False
            .NewColumnToolStripMenuItem.Visible = False
            .NewUpdateColumnToolStripMenuItem.Visible = False
            .NewTableToolStripMenuItem.Visible = False
            .RenameToolStripMenuItem.Visible = False
            .RenameToolStripMenuItem.Enabled = False
            .ToolStripMenuItem1.Visible = False
            .DeleteTableToolStripMenuItem.Visible = False
            .NewFormTabToolStripMenuItem.Visible = False
            .NewFieldToolStripMenuItem.Visible = False
            .UpDownToolStripMenuItem.Visible = False
            .MoveUpToolStripMenuItem.Visible = False
            .MoveDownToolStripMenuItem.Visible = False
        End With
        If Not IsNothing(library.SelectedNode) Then
            With Me
                Select Case LCase(library.SelectedNode.Tag)
                    Case "table"
                        .ToolStripMenuItem1.Visible = False
                        .NewKeyColumnToolStripMenuItem.Visible = True
                        .NewColumnToolStripMenuItem.Visible = True
                        .NewUpdateColumnToolStripMenuItem.Visible = True
                        .RenameToolStripMenuItem.Visible = True
                        .RenameToolStripMenuItem.Enabled = True
                        .DeleteTableToolStripMenuItem.Visible = True
                    Case "form"
                        .NewFormTabToolStripMenuItem.Visible = True
                        .RenameToolStripMenuItem.Visible = True
                        .RenameToolStripMenuItem.Enabled = True
                        .DeleteTableToolStripMenuItem.Visible = True
                        pos(library.SelectedNode, first, last, only)
                        If Not last And Not only Then
                            .MoveDownToolStripMenuItem.Visible = True
                        End If
                        If Not first And Not only Then
                            .MoveUpToolStripMenuItem.Visible = True
                        End If
                    Case "tab"
                        .NewFieldToolStripMenuItem.Visible = True
                        .RenameToolStripMenuItem.Visible = True
                        .RenameToolStripMenuItem.Enabled = True
                        .DeleteTableToolStripMenuItem.Visible = True
                        pos(library.SelectedNode, first, last, only)
                        If Not last And Not only Then
                            .MoveDownToolStripMenuItem.Visible = True
                        End If
                        If Not first And Not only Then
                            .MoveUpToolStripMenuItem.Visible = True
                        End If                        
                    Case "lib"
                        Me.NewTableToolStripMenuItem.Visible = True
                    Case "int"
                        Me.NewTableToolStripMenuItem.Visible = True
                End Select
                If .MoveUpToolStripMenuItem.Visible Or .MoveDownToolStripMenuItem.Visible Then
                    .UpDownToolStripMenuItem.Visible = True
                End If
            End With
        End If
    End Sub

    Private Sub pos(ByVal node As Windows.Forms.TreeNode, ByRef First As Boolean, ByRef Last As Boolean, ByRef Only As Boolean)
        Dim tag As String = node.Tag
        Dim h As System.IntPtr = node.Handle
        Dim c As Integer = 0
        Dim f As Integer = 0
        For i As Integer = 0 To node.Parent.Nodes.Count - 1
            If node.Parent.Nodes(i).Tag = tag Then
                c += 1
                If node.Parent.Nodes(i).Handle = h Then
                    f = c
                End If
            End If
        Next
        First = CBool(f = 1)
        Last = CBool(f = c)
        Only = CBool(f = 1 And c = 1)
    End Sub

    Private Sub msColumns_Opening(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles msColumns.Opening
        With Me
            .RemoveColToolStripMenuItem.Visible = Me.ColumnView.SelectedRows.Count > 0
            .MoveColDownToolStripMenuItem.Visible = False
            .MoveColUpToolStripMenuItem.Visible = False
            If Me.ColumnView.SelectedRows.Count > 0 Then
                If Me.ColumnView.SelectedRows(0).Index > 0 Then
                    .MoveColUpToolStripMenuItem.Visible = True
                End If
                If Me.ColumnView.SelectedRows(0).Index < Me.ColumnView.RowCount - 1 Then
                    .MoveColDownToolStripMenuItem.Visible = True
                End If
            End If
        End With
    End Sub
    Private Sub msKeys_Opening(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles msKeys.Opening
        With Me
            .RemoveKeyToolStripMenuItem1.Visible = Me.KeyView.SelectedRows.Count > 0
        End With
    End Sub
    Private Sub msUpdate_Opening(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles msUpdate.Opening
        With Me
            .RemoveUpdateToolStripMenuItem2.Visible = Me.UpdateView.SelectedRows.Count > 0
        End With
    End Sub
#End Region

#Region "Context Menu Click handlers"
    Private Sub NewTableToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewTableToolStripMenuItem.Click
        Dim dlg As New frmNewTable
        Dim result As System.Windows.Forms.DialogResult
        Dim node As New Windows.Forms.TreeNode
        With dlg
            Select Case LCase(library.SelectedNode.Tag)
                Case "lib"
                    .Text = "New Table"
                    .Label1.Text = "Table Name"
                Case "int"
                    .Text = "New form"
                    .Label1.Text = "Form Name"
            End Select
            result = .ShowDialog
        End With
        If result = Windows.Forms.DialogResult.OK Then
            Select Case LCase(library.SelectedNode.Tag)
                Case "lib"
                    With node
                        .ImageIndex = 2
                        .SelectedImageIndex = 3
                        .Tag = "table"
                        .Name = dlg.DataStr.Text
                        .Text = dlg.DataStr.Text
                    End With
                    Try
                        SQL.Add(New SQLItem(dlg.DataStr.Text, ""))
                        library.Nodes.Item(0).Nodes.Add(node)
                    Catch ex As Exception
                        MsgBox(ex.Message)
                    End Try
                Case "int"
                    With node
                        .ImageIndex = 4
                        .SelectedImageIndex = 5
                        .Tag = "form"
                        .Name = dlg.DataStr.Text
                        .Text = dlg.DataStr.Text
                    End With
                    library.Nodes.Item(1).Nodes.Add(node)
                    With node
                        Try
                            field.Add(New FormItem(.FullPath, .Name, "", "0", "", False))
                        Catch ex As Exception
                            MsgBox(ex.Message)
                            node.Remove()
                        End Try
                    End With
            End Select

        End If

    End Sub
    Private Sub NewColumnToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewColumnToolStripMenuItem.Click
        Me.TableDetail.SelectedIndex = 1
        NewColumn()
    End Sub
    Private Sub NewKeyColumnToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewKeyColumnToolStripMenuItem.Click
        Me.TableDetail.SelectedIndex = 2
        newKey()
    End Sub
    Private Sub NewUpdateColumnToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewUpdateColumnToolStripMenuItem.Click
        Me.TableDetail.SelectedIndex = 3
        NewUpdate()
    End Sub
    Private Sub AddColToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddColToolStripMenuItem.Click
        NewColumn()
    End Sub
    Private Sub AddKeyToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddKeyToolStripMenuItem1.Click
        newKey()
    End Sub
    Private Sub AddUpdateToolStripMenuItem2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddUpdateToolStripMenuItem2.Click
        NewUpdate()
    End Sub

    Private Sub AddFieldToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddFieldToolStripMenuItem.Click
        newField()
    End Sub

    Private Sub NewFieldToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewFieldToolStripMenuItem.Click
        newField()
    End Sub

#Region "Remove from table"

    Private Sub RemoveColToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RemoveColToolStripMenuItem.Click
        For Each r As Windows.Forms.DataGridViewRow In Me.ColumnView.SelectedRows
            cols.Remove(r.Cells("Key").Value)
        Next
        TabRefresh(1)
    End Sub

    Private Sub RemoveKeyToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RemoveKeyToolStripMenuItem1.Click
        For Each r As Windows.Forms.DataGridViewRow In Me.KeyView.SelectedRows
            Dim ci As ColumnItem = cols.Item(r.Cells("Key").Value)
            ci.KEYCOL = "N"
        Next
        TabRefresh(2)
    End Sub

    Private Sub RemoveUpdateToolStripMenuItem2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RemoveUpdateToolStripMenuItem2.Click
        For Each r As Windows.Forms.DataGridViewRow In Me.UpdateView.SelectedRows
            Dim ci As ColumnItem = cols.Item(r.Cells("Key").Value)
            ci.UPDATECOL = "N"
        Next
        TabRefresh(3)
    End Sub

    Private Sub RemoveFieldToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RemoveFieldToolStripMenuItem.Click
        For Each r As Windows.Forms.DataGridViewRow In Me.FieldView.SelectedRows
            intitem.Remove(r.Cells("Key").Value)
        Next
        TabRefresh(4)
    End Sub

#End Region

    Private Sub RenameToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RenameToolStripMenuItem.Click
        Me.library.LabelEdit = True
        Me.library.SelectedNode.BeginEdit()
    End Sub

    Private Sub DeleteTableToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DeleteTableToolStripMenuItem.Click
        With library.SelectedNode
            If MsgBox("This will delete the selected " & .Tag & "." & vbCrLf & _
                "Are you sure you wish to do this?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                Select Case .Tag
                    Case "table"
                        For Each ci As ColumnItem In cols.OriginalList
                            If String.Compare(ci.TABLE, .Text) = 0 Then
                                cols.Remove(ci.Key)
                            End If
                        Next
                        For Each ci As SQLItem In SQL.OriginalList
                            If String.Compare(ci.TABLE, .Text) = 0 Then
                                SQL.Remove(ci.Key)
                            End If
                        Next

                    Case "form", "tab"
                        Dim nodes As New List(Of Windows.Forms.TreeNode)
                        delnode(library.SelectedNode, nodes)
                        For Each N As Windows.Forms.TreeNode In nodes
                            Select Case N.Tag.ToString.ToLower
                                Case "form"
                                    For Each fi As FormItem In field.OriginalList
                                        If String.Compare(fi.FullPath, N.FullPath) = 0 Then
                                            field.Remove(fi.Key)
                                        End If
                                    Next
                                Case "tab"
                                    For Each ii As InterfaceItem In intitem.OriginalList
                                        If String.Compare(ii.FullPath, N.FullPath) = 0 Then
                                            intitem.Remove(ii.Key)
                                        End If
                                    Next
                            End Select
                        Next

                End Select
                .Remove()
            End If
        End With
    End Sub

    Private Sub delnode(ByVal n As Windows.Forms.TreeNode, ByRef nodes As List(Of Windows.Forms.TreeNode))
        nodes.Add(n)
        For Each subnode As Windows.Forms.TreeNode In n.Nodes
            delnode(subnode, nodes)
        Next
    End Sub

    Private Sub FormToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FormToolStripMenuItem.Click
        Dim dlg As New frmNewTable
        Dim result As System.Windows.Forms.DialogResult
        Dim node As New Windows.Forms.TreeNode
        With dlg
            .Text = "New form"
            .Label1.Text = "Form Name"
            result = .ShowDialog
        End With
        If result = Windows.Forms.DialogResult.OK Then
            With node
                .ImageIndex = 4
                .SelectedImageIndex = 5
                .Tag = "form"
                .Name = dlg.DataStr.Text
                .Text = dlg.DataStr.Text
            End With
            library.SelectedNode.Nodes.Add(node)
            With node
                Try
                    field.Add(New FormItem(.FullPath, .Name, "", "0", "", False))
                Catch ex As Exception
                    MsgBox(ex.Message)
                    node.Remove()
                End Try
            End With
        End If
    End Sub

    Private Sub TabToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TabToolStripMenuItem.Click
        Dim dlg As New frmNewTable
        Dim result As System.Windows.Forms.DialogResult
        Dim node As New Windows.Forms.TreeNode
        With dlg
            .Text = "New Tab"
            .Label1.Text = "Tab Name"
            result = .ShowDialog
        End With
        If result = Windows.Forms.DialogResult.OK Then
            With node
                .ImageIndex = 6
                .SelectedImageIndex = 7
                .Tag = "tab"
                .Name = dlg.DataStr.Text
                .Text = dlg.DataStr.Text
            End With
            For Each n As Windows.Forms.TreeNode In library.SelectedNode.Nodes
                If String.Compare(n.Text, node.Text, True) = 0 Then
                    MsgBox(String.Format("Tab {0} already exists in form.", library.SelectedNode.Text))
                    Exit Sub
                End If
            Next
            library.SelectedNode.Nodes.Add(node)
            Dim ns As New List(Of Windows.Forms.TreeNode)
            For Each n As Windows.Forms.TreeNode In library.SelectedNode.Nodes
                If String.Compare(n.Tag, "form") = 0 Then
                    ns.Add(n)
                End If
            Next
            For Each n As Windows.Forms.TreeNode In ns
                n.Remove()
                library.SelectedNode.Nodes.Add(n)
            Next
        End If
    End Sub

#End Region

    Private Sub TableDetail_Selecting(ByVal sender As Object, ByVal e As System.Windows.Forms.TabControlCancelEventArgs) Handles TableDetail.Selecting
        If Not IsNothing(library.SelectedNode) Then
            With library.SelectedNode
                Select Case LCase(.Tag)
                    Case "table"
                        TabRefresh(e.TabPageIndex)
                End Select
            End With
        End If
    End Sub

#Region "Refresh forms"

    Private Sub TabRefresh(ByVal i As Integer)

        Select Case i
            Case 0
                Dim sqli As SQLItem = SQL.Item(library.SelectedNode.Text)
                If Not IsNothing(sqli) Then
                    txtSQL.Text = sqli.SQL
                Else
                    txtSQL.Text = ""
                End If
            Case 1
                With BindSource
                    '.RemoveSort()
                    .RemoveFilter()                    
                    .Filter = "TABLE='" & library.SelectedNode.Text & "'"
                    .ResetBindings(False)
                    .Sort = "ORD"
                End With
                With Me.ColumnView
                    For Each c As Windows.Forms.DataGridViewColumn In .Columns
                        c.Visible = False
                    Next
                    .Columns(1).Visible = True
                    .Columns(.Columns.Count - 1).Visible = True
                End With

            Case 2
                With BindSource
                    '.RemoveSort()
                    .RemoveFilter()                    
                    .Filter = "TABLE='" & library.SelectedNode.Text & "' AND KEYCOL='Y'"
                    .ResetBindings(False)
                    .Sort = "ORD"
                End With
                With Me.KeyView
                    For Each c As Windows.Forms.DataGridViewColumn In .Columns
                        c.Visible = False
                    Next
                    .Columns(1).Visible = True
                End With

            Case 3
                With BindSource
                    '.RemoveSort()
                    .RemoveFilter()
                    .Filter = "TABLE='" & library.SelectedNode.Text & "' AND UPDATECOL='Y'"
                    .ResetBindings(False)
                    .Sort = "ORD"
                End With
                With Me.UpdateView
                    For Each c As Windows.Forms.DataGridViewColumn In .Columns
                        c.Visible = False
                    Next
                    .Columns(1).Visible = True
                End With

            Case 4
                With Me
                    With .fieldSource
                        .RemoveSort()
                        .RemoveFilter()
                        .Filter = "FullPath='" & library.SelectedNode.FullPath & "'"
                        .Sort = "ORD ASC"
                        .ResetBindings(False)
                    End With
                    .FieldProperties.SelectedObject = Nothing
                    With .FieldView
                        For Each c As Windows.Forms.DataGridViewColumn In .Columns
                            c.Visible = False
                        Next
                        .Columns(1).Visible = True
                        .Columns(.Columns.Count - 1).Visible = True
                    End With

                End With
        End Select
    End Sub

    Private Sub formRefresh()
        Me.FormDetail.SelectedObject = New FormProperties(field.Item(library.SelectedNode.FullPath), ds)
    End Sub

#End Region

#Region "Create Table Columns"

    Private Sub NewColumn()
        Dim i As Integer = 1
        For Each ci As ColumnItem In cols.OriginalList
            If String.Compare(library.SelectedNode.Name, ci.TABLE) = 0 Then
                If ci.ORD >= i Then
                    i = ci.ORD + 1
                End If
            End If
        Next
        Dim dlg As New frmNewColumn(library.SelectedNode.Name, i)
        Dim result As System.Windows.Forms.DialogResult = dlg.ShowDialog
        If result = Windows.Forms.DialogResult.OK Then
            With dlg
                Try
                    cols.Add(New ColumnItem(.table, .ColumnName, .strType, .ord, "N", "Y"))
                Catch ex As Exception
                    MsgBox(ex.Message)
                End Try
            End With
        End If
        dlg = Nothing
        TabRefresh(1)
    End Sub

    Private Sub newKey()
        Dim dlg As New frmNewKey
        For Each ci As ColumnItem In cols.OriginalList
            If Not ci.KEYCOL = "Y" And ci.TABLE = library.SelectedNode.Text Then
                dlg.strKey.Items.Add(ci.COLNAME)
            End If
        Next
        Dim result As System.Windows.Forms.DialogResult = dlg.ShowDialog
        If result = Windows.Forms.DialogResult.OK Then
            If Not IsNothing(dlg.strKey) Then
                For Each ci As ColumnItem In cols.OriginalList
                    If Not ci.KEYCOL = "Y" And ci.TABLE = library.SelectedNode.Text And ci.COLNAME = dlg.strKey.Text Then
                        ci.KEYCOL = "Y"
                    End If
                Next
            End If
        End If
        dlg = Nothing
        TabRefresh(2)
    End Sub

    Private Sub NewUpdate()
        Dim dlg As New frmNewUpdate
        For Each ci As ColumnItem In cols.OriginalList
            If Not ci.UPDATECOL = "Y" And ci.TABLE = library.SelectedNode.Text Then
                dlg.strKey.Items.Add(ci.COLNAME)
            End If
        Next
        Dim result As System.Windows.Forms.DialogResult = dlg.ShowDialog
        If result = Windows.Forms.DialogResult.OK Then
            If Not IsNothing(dlg.strKey) Then
                For Each ci As ColumnItem In cols.OriginalList
                    If Not ci.UPDATECOL = "Y" And ci.TABLE = library.SelectedNode.Text And ci.COLNAME = dlg.strKey.Text Then
                        ci.UPDATECOL = "Y"
                    End If
                Next
            End If
        End If
        dlg = Nothing
        TabRefresh(3)
    End Sub

    Private Sub newField()
        Dim i As Integer = 1
        For Each inti As InterfaceItem In intitem.OriginalList
            If String.Compare(library.SelectedNode.FullPath, inti.FullPath) = 0 Then
                If inti.ORD >= i Then
                    i = inti.ORD + 1
                End If
            End If
        Next
        Dim dlg As New frmNewField()
        Dim result As System.Windows.Forms.DialogResult = dlg.ShowDialog
        If result = Windows.Forms.DialogResult.OK Then
            Dim ii As New InterfaceItem()
            With ii
                .FullPath = Me.library.SelectedNode.FullPath
                .Name = dlg.txt_FieldName.Text
                .Hidden = False
                .Mandatory = False
                .IsReadOnly = False
                .ORD = i
            End With
            Try
                intitem.Add(ii)
            Catch EX As Exception
                MsgBox(EX.Message)
            End Try
        End If
        TabRefresh(4)
        dlg = Nothing
    End Sub

#End Region

    Private Sub library_AfterSelect(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles library.AfterSelect
        With SQLOut
            .Columns.Clear()
        End With

        With TableDetail
            .Visible = False
            .Dock = Windows.Forms.DockStyle.None
        End With
        With FieldDetail
            .Visible = False
            .Dock = Windows.Forms.DockStyle.None
        End With
        With FormDetail
            .Visible = False
            .Dock = Windows.Forms.DockStyle.None
        End With

        Select Case LCase(e.Node.Tag)
            Case "table"
                With TableDetail
                    .Visible = True
                    .Dock = Windows.Forms.DockStyle.Fill
                    TabRefresh(.SelectedIndex)
                End With

            Case "tab"
                With FieldDetail
                    .Visible = True
                    .Dock = Windows.Forms.DockStyle.Fill
                    TabRefresh(4)
                End With

                Dim f As FormItem = field.Item(library.SelectedNode.Parent.FullPath)
                For Each ii As InterfaceItem In intitem.OriginalList
                    If String.Compare(ii.FullPath, library.SelectedNode.FullPath) = 0 Then
                        Dim found As Boolean = False
                        For Each ci As ColumnItem In cols.OriginalList
                            If String.Compare(ci.TABLE, f.SQLFrom) = 0 Then
                                If String.Compare(ci.COLNAME, ii.Column) = 0 Then
                                    found = True
                                    Exit For
                                End If
                            End If
                        Next
                        If Not found Then
                            ii.Column = ""
                        End If
                    End If
                Next

            Case "form"
                With FormDetail
                    .Visible = True
                    .Dock = Windows.Forms.DockStyle.Fill
                    formRefresh()
                End With
        End Select
    End Sub

    Private Sub library_AfterLabelEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.NodeLabelEditEventArgs) Handles library.AfterLabelEdit
        With e
            If IsNothing(.Label) Then
                Me.library.LabelEdit = False
                Exit Sub
            End If
            Select Case library.SelectedNode.Tag
                Case "table"
                    For Each ci As ColumnItem In cols.OriginalList
                        If String.Compare(ci.TABLE, .Node.Text) = 0 Then
                            ci.TABLE = .Label
                        End If
                    Next
                    .Node.Text = .Label
                Case "form", "tab"
                    Dim nodes As New Dictionary(Of IntPtr, String)
                    For Each n As Windows.Forms.TreeNode In library.Nodes(1).Nodes
                        searchnode(n, nodes)
                    Next
                    .Node.Text = .Label
                    For Each n As Windows.Forms.TreeNode In library.Nodes(1).Nodes
                        renameNode(n, nodes)
                    Next
            End Select


        End With
        Me.library.LabelEdit = False
    End Sub

    Private Sub searchnode(ByVal n As Windows.Forms.TreeNode, ByRef nodes As Dictionary(Of IntPtr, String))
        nodes.Add(n.Handle, n.FullPath)
        For Each subnode As Windows.Forms.TreeNode In n.Nodes
            searchnode(subnode, nodes)
        Next
    End Sub

    Private Sub renameNode(ByVal n As Windows.Forms.TreeNode, ByRef nodes As Dictionary(Of IntPtr, String))
        If String.Compare(nodes(n.Handle), n.FullPath) <> 0 Then
            Select Case n.Tag.ToString.ToLower
                Case "form"
                    For Each fi As FormItem In field.OriginalList
                        If String.Compare(fi.FullPath, nodes(n.Handle)) = 0 Then
                            fi.FullPath = n.FullPath
                        End If
                    Next
                Case "tab"
                    For Each ii As InterfaceItem In intitem.OriginalList
                        If String.Compare(ii.FullPath, nodes(n.Handle)) = 0 Then
                            ii.FullPath = n.FullPath
                        End If
                    Next
            End Select
        End If
        For Each subnode As Windows.Forms.TreeNode In n.Nodes
            renameNode(subnode, nodes)
        Next
    End Sub

#Region "SQL Text Box"

    Private Sub RunQueryToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RunQueryToolStripMenuItem.Click

        Dim sd As New SerialData
        Dim data(,) As String = Nothing
        Dim sql As String = txtSQL.Text

        With SQLOut
            .Columns.Clear()
        End With
        For Each m As Match In Regex.Matches(sql, _
                                     "\%[A-Z0-9_]+\%", _
                                     RegexOptions.IgnoreCase)
            'm.NextMatch()
            If Not QueryConst.ContainsKey(m.Value) Then QueryConst.Add(m.Value, "")
            Dim val As String = InputBox(m.Value, , QueryConst(m.Value))
            QueryConst(m.Value) = val
            sql = Replace(sql, m.Value, QueryConst(m.Value))

        Next

        Dim cd As New Loading.ColumnDef(txtSQL.Text)
        For i As Integer = 0 To cd.Column.Count - 1
            With SQLOut.Columns
                Try
                    .Add(cd(i).ToUpper, cd(i).ToUpper)
                Catch
                End Try
            End With
        Next

        Try
            sd.FromStr(mCaller.ws.GetData(sql, ""))
            data = sd.Data
            If Not IsNothing(data) Then
                If Strings.Left(data(0, 0), 1) = "!" Then
                    Throw New Exception(Strings.Right(data(0, 0), data(0, 0).Length - 1))
                End If
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
            data = Nothing
        End Try

        If Not IsNothing(data) Then
            Try
                'Dim str As String = Split(txtSQL.Text, "select ", , CompareMethod.Text)(1)
                'Dim str2 As String = Split(str, "from ", , CompareMethod.Text)(0)
                'For Each c As String In Split(str2, ",")
                '    If InStr(c, ".") > 0 Then
                '        c = Split(c, ".")(1)
                '    End If
                '    If InStr(c, " as ", CompareMethod.Text) > 0 Then
                '        c = Split(c, " as ", , CompareMethod.Text)(1)
                '    End If
                '    For Each k As String In QueryConst.Keys
                '        c = Replace(c, k, QueryConst(k))
                '    Next
                '    SQLOut.Columns.Add(c.Trim().ToUpper, c.Trim().ToUpper)
                'Next
                For y As Integer = 0 To UBound(data, 2)
                    Dim r() As String = Nothing
                    For x As Integer = 0 To UBound(data, 1)
                        Try
                            ReDim Preserve r(UBound(r) + 1)
                        Catch ex As Exception
                            ReDim r(0)
                        Finally
                            r(UBound(r)) = data(x, y)
                        End Try
                    Next
                    SQLOut.Rows.Add(r)
                Next
            Catch
                MsgBox("Invalid SQL.")
            End Try
        End If

    End Sub

    Private Sub txtSQL_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSQL.GotFocus
        With mCaller
            .setEditToolbar(True)
            .EditControl = Me.txtSQL
        End With
    End Sub

    Private Sub txtSQL_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSQL.LostFocus
        With mCaller
            .setEditToolbar(False)
            .EditControl = Nothing
        End With
    End Sub

    Private Sub txtSQL_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtSQL.TextChanged
        Dim sqli As SQLItem = SQL.Item(library.SelectedNode.Text)
        If Not IsNothing(sqli) Then
            sqli.SQL = txtSQL.Text
        Else
            If txtSQL.Text.Length > 0 Then
                SQL.Add(New SQLItem(library.SelectedNode.Text, txtSQL.Text))
            End If
        End If
    End Sub

    Private Sub GenerateColumnsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GenerateColumnsToolStripMenuItem.Click
        Dim cd As New Loading.ColumnDef(Replace(txtSQL.Text, vbCrLf, ""))
        For i As Integer = 0 To cd.Column.Count - 1
            With cols
                Try
                    .Add(New ColumnItem(library.SelectedNode.Text, cd(i), "String", cols.OriginalList.Count, "N", "Y"))
                Catch
                End Try
            End With
        Next
        TabRefresh(1)
        TableDetail.SelectedIndex = 1
    End Sub

#End Region

    Private Sub FieldView_RowEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles FieldView.RowEnter
        With Me
            Dim i As InterfaceItem = intitem.Item(FieldView.Rows(e.RowIndex).Cells("Key").Value)
            .FieldProperties.SelectedObject = New FieldProperties(i, ds)
            With .FieldView
                If Not IsNothing(library.SelectedNode) And Not IsNothing(library.SelectedNode.Parent) Then
                    Dim f As FormItem = field.Item(library.SelectedNode.Parent.FullPath)
                    If Not IsNothing(f) Then
                        Dim cc As Windows.Forms.DataGridViewComboBoxColumn = .Columns(.Columns.Count - 1)
                        With cc.Items
                            .Clear()
                            '.Add(i.Column)
                            For Each ci As ColumnItem In cols.OriginalList
                                Dim add As Boolean = True
                                If String.Compare(ci.TABLE, f.SQLFrom) = 0 Then
                                    For Each ii As InterfaceItem In intitem.OriginalList
                                        If String.Compare(ii.Column, ci.COLNAME) = 0 Then
                                            add = True
                                            Exit For
                                        End If
                                    Next
                                    If add Then
                                        .Add(ci.COLNAME)
                                    End If
                                End If
                            Next
                        End With
                    End If
                End If
            End With
        End With
    End Sub

    Private Sub hErr(ByVal sender As Object, ByVal e As Windows.Forms.DataGridViewDataErrorEventArgs) Handles FieldView.DataError

    End Sub

#Region "Swap Rows"

    Private Sub SwapRow(ByVal tag As String, ByRef g1 As Windows.Forms.DataGridViewRow, ByRef g2 As Windows.Forms.DataGridViewRow)
        Select Case LCase(tag)
            Case "col"
                Dim o1 As ColumnItem = cols.Item(g1.Cells("Key").Value)
                Dim o2 As ColumnItem = cols.Item(g2.Cells("Key").Value)
                Dim ord1 As Integer = o1.ORD
                Dim ord2 As Integer = o2.ORD
                o1.ORD = ord2
                o2.ORD = ord1
                TabRefresh(1)
            Case "fld"
                Dim o1 As InterfaceItem = intitem.Item(g1.Cells("Key").Value)
                Dim o2 As InterfaceItem = intitem.Item(g2.Cells("Key").Value)
                Dim ord1 As Integer = o1.ORD
                Dim ord2 As Integer = o2.ORD
                o1.ORD = ord2
                o2.ORD = ord1
                TabRefresh(4)
        End Select
    End Sub

    Private Sub MoveColUpToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MoveColUpToolStripMenuItem.Click
        With ColumnView
            Dim k As String = .SelectedRows(0).Cells("Key").Value
            SwapRow("col", .SelectedRows(0), .Rows(.SelectedRows(0).Index - 1))
            For Each r As Windows.Forms.DataGridViewRow In .Rows
                r.Selected = CBool(String.Compare(r.Cells("Key").Value, k) = 0)
            Next
        End With
    End Sub

    Private Sub MoveColDownToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MoveColDownToolStripMenuItem.Click
        With ColumnView
            Dim k As String = .SelectedRows(0).Cells("Key").Value
            SwapRow("col", .SelectedRows(0), .Rows(.SelectedRows(0).Index + 1))
            For Each r As Windows.Forms.DataGridViewRow In .Rows
                r.Selected = CBool(String.Compare(r.Cells("Key").Value, k) = 0)
            Next
        End With
    End Sub

    Private Sub MoveFieldUpToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MoveFieldUpToolStripMenuItem.Click
        With FieldView
            Dim k As String = .SelectedRows(0).Cells("Key").Value
            SwapRow("fld", .SelectedRows(0), .Rows(.SelectedRows(0).Index - 1))
            For Each r As Windows.Forms.DataGridViewRow In .Rows
                r.Selected = CBool(String.Compare(r.Cells("Key").Value, k) = 0)
            Next
        End With
    End Sub

    Private Sub MoveFieldDownToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MoveFieldDownToolStripMenuItem.Click
        With FieldView
            Dim k As String = .SelectedRows(0).Cells("Key").Value
            SwapRow("fld", .SelectedRows(0), .Rows(.SelectedRows(0).Index + 1))
            For Each r As Windows.Forms.DataGridViewRow In .Rows
                r.Selected = CBool(String.Compare(r.Cells("Key").Value, k) = 0)
            Next
        End With
    End Sub

#End Region

    Private Sub MoveUpToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MoveUpToolStripMenuItem.Click
        Dim f As Boolean = False
        Dim repnode As Windows.Forms.TreeNode
        For i As Integer = library.SelectedNode.Parent.Nodes.Count - 1 To 0 Step -1
            If library.SelectedNode.Parent.Nodes(i).Tag = library.SelectedNode.Tag Then
                If f Then
                    repnode = library.SelectedNode.Parent.Nodes(i)
                    swapnodes(library.SelectedNode, repnode)
                    Exit Sub
                End If
                If library.SelectedNode.Parent.Nodes(i).Handle = library.SelectedNode.Handle Then
                    f = True
                End If
            End If
        Next
    End Sub

    Private Sub MoveDownToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MoveDownToolStripMenuItem.Click
        Dim f As Boolean = False
        Dim repnode As Windows.Forms.TreeNode
        For i As Integer = 0 To library.SelectedNode.Parent.Nodes.Count - 1
            If library.SelectedNode.Parent.Nodes(i).Tag = library.SelectedNode.Tag Then
                If f Then
                    repnode = library.SelectedNode.Parent.Nodes(i)
                    swapnodes(library.SelectedNode, repnode)
                    Exit Sub
                End If
                If library.SelectedNode.Parent.Nodes(i).Handle = library.SelectedNode.Handle Then
                    f = True
                End If
            End If
        Next
    End Sub

    Public Sub swapnodes(ByRef nodeA As Windows.Forms.TreeNode, ByRef NodeB As Windows.Forms.TreeNode)
        Dim p As Windows.Forms.TreeNode = nodeA.Parent
        Dim nodes As New List(Of Windows.Forms.TreeNode)
        For Each n As Windows.Forms.TreeNode In p.Nodes
            If Not (n.Handle = nodeA.Handle) And Not (n.Handle = NodeB.Handle) Then
                nodes.Add(n)
            ElseIf n.Handle = nodeA.Handle Then
                nodes.Add(NodeB)
            ElseIf n.Handle = NodeB.Handle Then
                nodes.Add(nodeA)
            End If
        Next
        p.Nodes.Clear()
        For Each n As Windows.Forms.TreeNode In nodes
            p.Nodes.Add(n)
        Next
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

End Class