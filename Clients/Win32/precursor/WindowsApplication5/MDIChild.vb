Imports System.IO

Public Enum eActiveControl
    eSource = 0
    eOutput = 1
    eVariables = 2
    eWhere = 3
    eLoadref = 4
End Enum

Public Class MDIChild
    Inherits Form

#Region "Initialisation and finalisation"

    Private mAC As eActiveControl = eActiveControl.eSource
    Private mCaller As MDIParent

    Public Sub New(ByRef Caller As MDIParent)
        InitializeComponent()
        mCaller = Caller
    End Sub

    Private Sub MDIChild_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.GotFocus
        doToolbar()
    End Sub

    Private Sub Form1_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        With mCaller
            If Not .SaveChanges(Me) = MsgBoxResult.Ok Then
                e.Cancel = True
            Else
                If .frmCount <= 1 Then .HideChildMenu()
                doToolbar(True)
            End If
        End With
    End Sub

#End Region

#Region "Set Active Control value"

    Private Sub data_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles data.GotFocus
        mAC = eActiveControl.eSource
    End Sub

    Private Sub output_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles output.GotFocus
        mAC = eActiveControl.eOutput
    End Sub

    Private Sub lstVariables_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstVariables.GotFocus
        mAC = eActiveControl.eVariables
    End Sub

    Private Sub txtWhere_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtWhere.GotFocus
        mAC = eActiveControl.eWhere
        'MalformedWhere(True)
    End Sub

    Private Sub LoadRef_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles LoadRef.GotFocus
        mAC = eActiveControl.eLoadref
    End Sub

    Private Sub tabSource_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles tabSource.Click
        mAC = eActiveControl.eSource
    End Sub

    Private Sub tabVariables_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles tabVariables.Click
        mAC = eActiveControl.eVariables
    End Sub

    Private Sub tabWhere_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles tabWhere.Click
        mAC = eActiveControl.eWhere
    End Sub

    Private Sub tabLoadRef_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles tabLoadRef.Click
        mAC = eActiveControl.eLoadref
    End Sub

#End Region

#Region "Private declarations"

    Dim mWriteLoad As Boolean = False
    Dim mSub As Boolean = False

    Dim mCursor As String = "MyCursor"
    Dim labelStart As Integer = 0
    Dim mRecordType As String = "1"

    Dim mFilename As String = ""
    Dim mChanged As Boolean = False
    Dim mVarPref As String = ""
    Dim mWhere As String = ""
    Dim mTableLines() As String
    Dim mCols(,) As String
    Dim mTable As String = ""
    Dim mSelectVars As Boolean
    Dim mLinkGenLoad As Boolean

    Dim mIntegers() As String = Nothing
    Dim mReals() As String = Nothing
    Dim mStrings() As String = Nothing
    Dim mBit() As String = Nothing
    Dim mDates() As String = Nothing
    Dim mLoadVars(,) As String
    Dim mBuildID As Integer = -1

#End Region

#Region "Public Variable Array Properies"

    Public ReadOnly Property VarIntegers() As String()
        Get
            Return Columns(Me, , eType.eINT, False)
        End Get
    End Property

    Public ReadOnly Property VarReals() As String()
        Get
            Return Columns(Me, , eType.eREAL, False)
        End Get
    End Property

    Public ReadOnly Property VarStrings() As String()
        Get
            Return Columns(Me, , eType.eCHAR, False)
        End Get
    End Property

    Public ReadOnly Property VarBits() As String()
        Get
            Return Columns(Me, , eType.eBIT, False)
        End Get
    End Property

    Public ReadOnly Property VarDates() As String()
        Get
            Return Columns(Me, , eType.eDATE, False)
        End Get
    End Property

    Public ReadOnly Property showINT() As Boolean
        Get
            Try
                Return UBound(Me.VarIntegers) >= 0
            Catch
                Return False
            End Try
        End Get
    End Property

    Public ReadOnly Property showREAL() As Boolean
        Get
            Try
                Return UBound(Me.VarReals) >= 0
            Catch
                Return False
            End Try
        End Get
    End Property

    Public ReadOnly Property showCHAR() As Boolean
        Get
            Try
                Return UBound(Me.VarStrings) >= 0
            Catch
                Return False
            End Try
        End Get
    End Property

    Public ReadOnly Property showDATE() As Boolean
        Get
            Try
                Return UBound(Me.VarDates) >= 0
            Catch
                Return False
            End Try
        End Get
    End Property

    Public ReadOnly Property showBIT() As Boolean
        Get
            Try
                Return UBound(Me.VarBits) >= 0
            Catch
                Return False
            End Try
        End Get
    End Property

#End Region

#Region "public Properties"

    Public Property SelectVars() As Boolean
        Get
            Return mSelectVars
        End Get
        Set(ByVal value As Boolean)
            mSelectVars = value
        End Set
    End Property

    Public Property LinkGenLoad() As Boolean
        Get
            Return mLinkGenLoad
        End Get
        Set(ByVal value As Boolean)
            mLinkGenLoad = value
        End Set
    End Property

    Public Property BuildID() As Integer
        Get
            Return mBuildID
        End Get
        Set(ByVal value As Integer)
            mBuildID = value
        End Set
    End Property

    Public ReadOnly Property TypeMyActiveControl() As eActiveControl
        Get
            Return mAC
        End Get
    End Property

    Public ReadOnly Property MyActiveControl() As Control
        Get
            With Me
                Select Case mAC
                    Case eActiveControl.eSource
                        Return .data
                    Case eActiveControl.eOutput
                        Return .output
                    Case eActiveControl.eVariables
                        Return .lstVariables
                    Case eActiveControl.eWhere
                        Return .txtWhere
                    Case eActiveControl.eLoadref
                        Return .LoadRef
                    Case Else
                        Return Nothing
                End Select
            End With
        End Get
    End Property

    Public Property Filename() As String
        Get
            Return mFilename
        End Get
        Set(ByVal value As String)
            mFilename = value
            Me.Text = Split(value, "\")(UBound(Split(value, "\")))
        End Set
    End Property

    Public Property CursorName() As String
        Get
            Return mCursor
        End Get
        Set(ByVal value As String)
            mCursor = value
        End Set
    End Property

    Public Property Changed() As Boolean
        Get
            Return mChanged
        End Get
        Set(ByVal value As Boolean)
            mChanged = value
        End Set
    End Property

    Public Property IsSub() As Boolean
        Get
            Return mSub
        End Get
        Set(ByVal value As Boolean)
            mSub = value
        End Set
    End Property

    Public Property WriteLoad() As Boolean
        Get
            Return mWriteLoad
        End Get
        Set(ByVal value As Boolean)
            mWriteLoad = value
        End Set
    End Property

    Public Property StartLabel() As Integer
        Get
            Return labelStart
        End Get
        Set(ByVal value As Integer)
            labelStart = value
        End Set
    End Property

    Public Property RecordType() As String
        Get
            Return mRecordType
        End Get
        Set(ByVal value As String)
            mRecordType = value
        End Set
    End Property

    Public Property VarPref() As String
        Get
            Return mVarPref
        End Get
        Set(ByVal value As String)
            mVarPref = value
        End Set
    End Property

    Public Property FormData() As String
        Get
            Return Me.data.Text
        End Get
        Set(ByVal value As String)
            Me.data.Text = value
        End Set
    End Property

    Public Property FormOutput() As String
        Get
            Return Me.output.Text
        End Get
        Set(ByVal value As String)
            Me.output.Text = value
        End Set
    End Property

    Public ReadOnly Property ConsoleWidth() As Integer
        Get
            Return mCaller.ConsoleWidth
        End Get
    End Property

    Public Property WhereClause() As String
        Get
            Return mWhere
        End Get
        Set(ByVal value As String)
            mWhere = value
        End Set
    End Property

    Public Property TableLines() As String()
        Get
            Return mTableLines
        End Get
        Set(ByVal value() As String)
            mTableLines = value
        End Set
    End Property

    Public Property Cols() As String(,)
        Get
            Return mCols
        End Get
        Set(ByVal value(,) As String)
            mCols = value
        End Set
    End Property

    Public Property CurrentTable() As String
        Get
            Return mTable
        End Get
        Set(ByVal value As String)
            mTable = value
        End Set
    End Property

    Public Property LoadVars() As String(,)
        Get
            Return mLoadVars
        End Get
        Set(ByVal value(,) As String)
            mLoadVars = value
        End Set
    End Property

#End Region

    Public Sub Execute()

        setHeader(Me)
        GetWhere(Me)
        GetCols(Me)

        If Not IsNothing(UnHidden(Me)) Then

            DrawVarTable(Me, lstVariables)
            DrawLoadingReference(Me, LoadRef)

            With output
                .Text = ""

                DeclareVars(Me, .Text)
                ProcStart(Me, .Text)
                CursorSelect(Me, .Text)
                CursorFetch(Me, .Text)
                QuitOnError(Me, .Text)

                .Text += "/* " & vbCrLf & "Processing of current record " & vbCrLf & "*/" & vbCrLf
                WriteSelectVars(Me, .Text)
                InsertIntoGeneralLoad(Me, .Text)

                ProcEnd(Me, .Text)

            End With

            parseWidth(Me)

        End If

    End Sub

    Private Sub data_TextmChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        mChanged = True
    End Sub

    Public Sub doToolbar(Optional ByVal Blank As Boolean = False)

        With mCaller
            If Not Blank Then
                .ToolStripStatusLabel1.Text = "Cursor Name: " & mCursor
                .ToolStripStatusLabel2.Text = "Start Label: " & labelStart
                .ToolStripStatusLabel3.Text = "Record Type: " & mRecordType
                .ToolStripStatusLabel4.Text = "Var Preface: " & mVarPref
                .ToolStripStatusLabel5.Text = "Is Sub: " & mSub
                .ToolStripStatusLabel6.Text = "Write Load: " & mWriteLoad
            Else
                .ToolStripStatusLabel1.Text = ""
                .ToolStripStatusLabel2.Text = ""
                .ToolStripStatusLabel3.Text = ""
                .ToolStripStatusLabel4.Text = ""
                .ToolStripStatusLabel5.Text = ""
                .ToolStripStatusLabel6.Text = ""
            End If
        End With

    End Sub

#Region "Text edition context menus"

    Private Sub UndoToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UndoToolStripMenuItem.Click, ToolStripMenuItem2.Click
        mCaller.UndoToolStripMenuItem_Click(sender, e)
    End Sub

    Private Sub UpperToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UpperToolStripMenuItem.Click, ToolStripMenuItem4.Click
        mCaller.UpperToolStripMenuItem_Click(sender, e)
    End Sub

    Private Sub LowerToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LowerToolStripMenuItem.Click, ToolStripMenuItem5.Click
        mCaller.LowerToolStripMenuItem_Click(sender, e)
    End Sub

    Private Sub CutToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CutToolStripMenuItem.Click, ToolStripMenuItem6.Click
        mCaller.CutToolStripMenuItem_Click(sender, e)
    End Sub

    Private Sub CopyToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CopyToolStripMenuItem.Click, ToolStripMenuItem7.Click
        mCaller.CopyToolStripMenuItem_Click(sender, e)
    End Sub

    Private Sub PasteToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PasteToolStripMenuItem.Click, ToolStripMenuItem8.Click
        mCaller.PasteToolStripMenuItem_Click(sender, e)
    End Sub

    Private Sub SelectAllToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SelectAllToolStripMenuItem.Click, ToolStripMenuItem9.Click
        mCaller.SelectAllToolStripMenuItem_Click(sender, e)
    End Sub

#End Region

#Region "Table Selection Context Menus"

    Public Sub HideShow(Optional ByVal show As Integer = -1)

        Dim skip As Boolean
        Me.Changed = True

        Dim ch(,) As String = Nothing
        skip = False
        With lstVariables
            For i As Integer = 0 To .Items.Count - 1
                If .Items(i).Selected Then

                    If show = -1 Then
                        Select Case .Items(i).SubItems(0).Text
                            Case "-"
                                .Items(i).SubItems(0).Text = "X"
                            Case "X"
                                .Items(i).SubItems(0).Text = "-"
                        End Select
                    Else
                        Select Case show
                            Case 1
                                If Not .Items(i).SubItems(0).Text = "-" Then
                                    .Items(i).SubItems(0).Text = "-"
                                Else
                                    skip = True
                                End If
                            Case 0
                                If Not .Items(i).SubItems(0).Text = "X" Then
                                    .Items(i).SubItems(0).Text = "X"
                                Else
                                    skip = True
                                End If
                        End Select
                    End If

                    If Not skip Then
                        Try
                            ReDim Preserve ch(2, UBound(ch, 2) + 1)
                        Catch ex As Exception
                            ReDim ch(2, 0)
                        Finally
                            ch(0, UBound(ch, 2)) = .Items(i).SubItems(0).Text
                            ch(1, UBound(ch, 2)) = .Items(i).SubItems(1).Text
                            ch(2, UBound(ch, 2)) = .Items(i).SubItems(2).Text
                        End Try
                    End If
                End If
            Next
        End With

        If Not IsNothing(ch) Then
            For v As Integer = 0 To UBound(Cols, 2)
                For z As Integer = 0 To UBound(ch, 2)
                    If Cols(0, v) = ch(1, z) And Cols(1, v) = ch(2, z) Then
                        Cols(5, v) = CBool(ch(0, z) = "X")
                    End If
                Next
            Next

            Dim tl() As String = Split(data.Text, vbCrLf)
            Dim ct As String = ""
            Dim tmp As String = ""

            For i As Integer = 0 To UBound(tl)
                If Strings.Left(tl(i), Len("CREATE ")) = "CREATE " Then
                    ct = Split(tl(i), " ")(2)
                Else
                    For z As Integer = 0 To UBound(ch, 2)
                        If Replace(ct, "*", "") = ch(2, z) Then
                            If Split(tl(i), " ")(0) = ch(1, z) Then
                                Select Case CBool(ch(0, z) = "X")
                                    Case True
                                        If Not (Strings.Right(tl(i), 2)) = "XX" Then tl(i) = tl(i) & "XX"
                                    Case False
                                        If (Strings.Right(tl(i), 2)) = "XX" Then tl(i) = Strings.Left(tl(i), Len(tl(i)) - 2)
                                End Select
                            End If
                        End If
                    Next
                End If

                tmp += tl(i) & vbCrLf
            Next
            data.Text = tmp
        End If

    End Sub

    Private Sub cMenuVars_Opening(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles cMenuVars.Opening
        TableToolStripMenuItem.DropDownItems.Clear()
        Dim t() As String = Tables(Me)
        If Not IsNothing(t) Then
            For Each table As String In t
                Dim tsi As New System.Windows.Forms.ToolStripMenuItem
                With tsi
                    .Name = Replace(table, "*", "")
                    .Text = Replace(table, "*", "")
                End With
                AddHandler tsi.Click, AddressOf Me.hTSI_Click
                TableToolStripMenuItem.DropDownItems.Add(tsi)
            Next
        End If

    End Sub

    Private Sub lstVariables_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles lstVariables.KeyPress
        Select Case e.KeyChar
            Case " "
                HideShow()
        End Select
    End Sub

    Private Sub AllToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AllToolStripMenuItem.Click
        With lstVariables
            For i As Integer = 0 To .Items.Count - 1
                .Items(i).Selected = True
            Next
        End With
    End Sub

    Private Sub NoneToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NoneToolStripMenuItem.Click
        With lstVariables
            For i As Integer = 0 To .Items.Count - 1
                .Items(i).Selected = False
            Next
        End With
    End Sub

    Private Sub InvertToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles InvertToolStripMenuItem.Click
        With lstVariables
            For i As Integer = 0 To .Items.Count - 1
                .Items(i).Selected = Not (.Items(i).Selected)
            Next
        End With
    End Sub

    Private Sub INTToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles INTToolStripMenuItem.Click
        With lstVariables
            For i As Integer = 0 To .Items.Count - 1
                .Items(i).Selected = CBool(.Items(i).SubItems(3).Text = "INT")
            Next
        End With
    End Sub

    Private Sub REALToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles REALToolStripMenuItem.Click
        With lstVariables
            For i As Integer = 0 To .Items.Count - 1
                .Items(i).Selected = CBool(.Items(i).SubItems(3).Text = "REAL")
            Next
        End With
    End Sub

    Private Sub CHARToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CHARToolStripMenuItem.Click
        With lstVariables
            For i As Integer = 0 To .Items.Count - 1
                .Items(i).Selected = CBool(.Items(i).SubItems(3).Text = "CHAR")
            Next
        End With
    End Sub

    Private Sub DATEToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DATEToolStripMenuItem.Click
        With lstVariables
            For i As Integer = 0 To .Items.Count - 1
                .Items(i).Selected = CBool(.Items(i).SubItems(3).Text = "DATE")
            Next
        End With
    End Sub

    Private Sub ToggleHiddenToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToggleHiddenToolStripMenuItem.Click
        HideShow()
    End Sub

    Private Sub VisibleToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles VisibleToolStripMenuItem.Click
        With lstVariables
            For i As Integer = 0 To .Items.Count - 1
                .Items(i).Selected = CBool(.Items(i).SubItems(0).Text = "-")
            Next
        End With
    End Sub

    Private Sub HiddenToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HiddenToolStripMenuItem.Click
        With lstVariables
            For i As Integer = 0 To .Items.Count - 1
                .Items(i).Selected = CBool(.Items(i).SubItems(0).Text = "X")
            Next
        End With
    End Sub

    Private Sub hTSI_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        With lstVariables
            For i As Integer = 0 To .Items.Count - 1
                .Items(i).Selected = CBool(.Items(i).SubItems(2).Text = sender.name)
            Next
        End With
    End Sub

    Private Sub HideSelectionToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HideSelectionToolStripMenuItem.Click
        HideShow(False)
    End Sub

    Private Sub ShowSelectionToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ShowSelectionToolStripMenuItem.Click
        HideShow(True)
    End Sub

#End Region

#Region "Where tab context menus"

    Private Sub cMenuWhere_Opening(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles cMenuWhere.Opening

        TablesToolStripMenuItem_Click(sender, e)
        DataTypeToolStripMenuItem1_Click(sender, e)
        ToolStripMenuItem14_Click(sender, e)

    End Sub

    Private Sub TablesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        With TablesToolStripMenuItem
            .DropDownItems.Clear()
            Dim t() As String = Tables(Me)
            If Not IsNothing(t) Then
                For Each table As String In t
                    Dim tsi As New System.Windows.Forms.ToolStripMenuItem
                    With tsi
                        .Name = Replace(table, "*", "")
                        .Text = Replace(table, "*", "")
                    End With

                    .DropDownItems.Add(tsi)

                    For Each column As String In Columns(Me, table)

                        Dim subtsi As New System.Windows.Forms.ToolStripMenuItem
                        With subtsi
                            .Name = Replace(table, "*", "") & "." & column
                            .Text = column
                        End With

                        tsi.DropDownItems.Add(subtsi)
                        AddHandler subtsi.Click, AddressOf Me.hSubTSI_Click
                    Next
                Next
            End If
        End With
    End Sub

    Private Sub DataTypeToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim dataTypes() As Priority.Parser.eType = {eType.eINT, eType.eREAL, eType.eCHAR, eType.eDATE, eType.eBIT}

        With DataTypeToolStripMenuItem1
            .DropDownItems.Clear()
            Dim t() As String = Tables(Me)
            If Not IsNothing(t) Then
                For Each table As String In t
                    Dim tsi As New System.Windows.Forms.ToolStripMenuItem
                    With tsi
                        .Name = Replace(table, "*", "")
                        .Text = Replace(table, "*", "")
                    End With

                    .DropDownItems.Add(tsi)

                    For Each dt As Priority.Parser.eType In dataTypes

                        Dim ty As New System.Windows.Forms.ToolStripMenuItem
                        With ty
                            Select Case dt
                                Case eType.eINT
                                    .Name = "INT"
                                    .Text = "INT"
                                Case eType.eREAL
                                    .Name = "REAL"
                                    .Text = "REAL"
                                Case eType.eCHAR
                                    .Name = "CHAR"
                                    .Text = "CHAR"
                                Case eType.eDATE
                                    .Name = "DATE"
                                    .Text = "DATE"
                                Case eType.eBIT
                                    .Name = "BIT"
                                    .Text = "BIT"
                            End Select
                        End With
                        tsi.DropDownItems.Add(ty)

                        Dim thiscols() = Columns(Me, table, dt)
                        If Not IsNothing(thiscols) Then
                            For Each column As String In thiscols
                                Dim subtsi As New System.Windows.Forms.ToolStripMenuItem
                                With subtsi
                                    .Name = Replace(table, "*", "") & "." & column
                                    .Text = column
                                End With

                                ty.DropDownItems.Add(subtsi)
                                AddHandler subtsi.Click, AddressOf Me.hSubTSI_Click
                            Next
                        End If
                    Next
                Next
            End If
        End With
    End Sub

    Private Sub ToolStripMenuItem14_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        With ToolStripMenuItem14
            .DropDownItems.Clear()
            For Each ChildForm As MDIChild In mCaller.MdiChildren
                Dim tsi As New System.Windows.Forms.ToolStripMenuItem
                With tsi
                    .Name = ChildForm.Text
                    .Text = ChildForm.Text
                End With
                .DropDownItems.Add(tsi)
                If Not (ChildForm.IsSub) And ChildForm.WriteLoad Then
                    Dim subtsi As New System.Windows.Forms.ToolStripMenuItem
                    With subtsi
                        .Name = ":LINE"
                        .Text = ":LINE"
                    End With
                    tsi.DropDownItems.Add(subtsi)
                    AddHandler subtsi.Click, AddressOf Me.hSubTSI_Click
                End If
                For i As Integer = 0 To UBound(ChildForm.LoadVars, 2)
                    Dim subtsi As New System.Windows.Forms.ToolStripMenuItem
                    With subtsi
                        .Name = ":" & ChildForm.VarPref & ChildForm.LoadVars(1, i)
                        .Text = ":" & ChildForm.VarPref & ChildForm.LoadVars(1, i)
                    End With
                    tsi.DropDownItems.Add(subtsi)
                    AddHandler subtsi.Click, AddressOf Me.hSubTSI_Click
                Next
            Next
        End With
    End Sub

    Private Sub hSubTSI_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim tmp As String = Clipboard.GetText
        Clipboard.SetText(sender.name)
        Me.txtWhere.Paste()
        Clipboard.SetText(tmp)
    End Sub

    Private Sub JoinToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles JoinToolStripMenuItem.Click
        Dim j As New frmJoin(Me)
        With j
            .lstTable1.Items.Clear()
            .lstTable2.Items.Clear()
            For Each table As String In Tables(Me)
                .lstTable1.Items.Add(table)
                .lstTable2.Items.Add(table)
            Next
            If .ShowDialog = Windows.Forms.DialogResult.OK Then
                Dim tmp As String = Clipboard.GetText
                Clipboard.SetText(Replace(.lstTable1.Items(.lstTable1.SelectedIndex), "*", "") & _
                "." & _
                .lstColumn1.Items(.lstColumn1.SelectedIndex) & _
                " = " & _
                Replace(.lstTable2.Items(.lstTable2.SelectedIndex), "*", "") & _
                "." & _
                .lstColumn2.Items(.lstColumn2.SelectedIndex))
                Me.txtWhere.Paste()
                Clipboard.SetText(tmp)
            End If
        End With
    End Sub

#End Region

    Private Sub TableDictionaryToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TableDictionaryToolStripMenuItem.Click
        mCaller.TableDictonaryToolStripMenuItem_Click(sender, e)
    End Sub

#Region "position Noref label"

    Private Sub MDIChild_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
        With Me.lblNoref
            Dim p As Point
            p.X = (Me.tabLoadRef.Width - .Width) / 2
            p.Y = (Me.tabLoadRef.Height - .Height) / 2
            .Location = p
        End With

    End Sub

    Private Sub tab_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles tab.Click
        With Me.lblNoref
            Dim p As Point
            p.X = (Me.tabLoadRef.Width - .Width) / 2
            p.Y = (Me.tabLoadRef.Height - .Height) / 2
            .Location = p
        End With
    End Sub

#End Region

    Private Sub TToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TToolStripMenuItem.Click
        Dim cl As String = ""
        DrawLoadingReference(Me, cl)
        Clipboard.SetText(cl)
    End Sub

    Private Sub data_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles data.LostFocus
        If MalformedWhere() Then Exit Sub
        GetWhere(Me)
        GetCols(Me)
        If Not IsNothing(Me.Cols) Then
            DrawVarTable(Me, Me.lstVariables)
        End If
    End Sub

    Private Sub txtWhere_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtWhere.LostFocus
        If MalformedWhere() Then Exit Sub
        SetWhere(Me)
        Me.Changed = True
    End Sub

    Private Function MalformedWhere(Optional ByVal ShowEr As Boolean = False) As Boolean
        Dim ret As Boolean = False
        If UBound(Split(Me.txtWhere.Text, "WHERE ")) <> 1 Then
            If ShowEr Then MsgBox("Malformed where statement.", MsgBoxStyle.Critical)
            tab.SelectedTab = tabWhere
            Me.txtWhere.Focus()
            ret = True
        End If

        If Strings.Left(Replace(Replace(Me.txtWhere.Text, " ", ""), vbCrLf, ""), Len("WHERE")) <> "WHERE" Then
            If ShowEr Then MsgBox("Malformed where statement.", MsgBoxStyle.Critical)
            tab.SelectedTab = tabWhere
            Me.txtWhere.Focus()
            ret = True
        End If
        Return ret
    End Function

    Private Sub data_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles data.TextChanged

    End Sub

    Private Sub txtWhere_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtWhere.TextChanged

    End Sub
End Class
