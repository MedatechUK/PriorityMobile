Module Parser

    Public Enum eType
        eUNSPECIFIED = 0
        eINT = 1
        eCHAR = 2
        eREAL = 3
        eDATE = 4
        eBIT = 5
    End Enum

#Region "Document Header"

    Sub setHeader(ByRef f As MDIChild)
        Dim h As String = HeaderString(f)
        With f
            If Strings.Left(.data.Text, 2) <> "##" Then
                .data.Text = h & .data.Text
                .Changed = True
            Else
                If Not Split(.data.Text, vbCrLf)(0) = h Then
                    .data.Text = h & _
                       Strings.Right(.data.Text, Len(.data.Text) - (InStr(.data.Text, vbCrLf) + 1))
                    .Changed = True
                End If
            End If
        End With
    End Sub

    Private Function HeaderString(ByRef f As MDIChild) As String
        With f
            Return "##" & _
                .CursorName & "," & _
                .StartLabel & "," & _
                .RecordType & "," & _
                .VarPref & "," & _
                .IsSub & "," & _
                .WriteLoad & "," & _
                .SelectVars & "," & _
                .LinkGenLoad & _
                vbCrLf
        End With
    End Function

#End Region

    Public Sub parseWidth(ByRef f As MDIChild)
        With f
            Dim b As String = ""
            Dim l() As String = Split(.output.Text, vbCrLf)
            For i As Integer = 0 To UBound(l)
                If Not (Len(l(i)) > .ConsoleWidth) Then
                    b = b & l(i) & vbCrLf
                Else
                    Dim t As String = ""
                    Dim sp() As String = Split(l(i), " ")
                    For s As Integer = 0 To UBound(sp)
                        If Len(t & sp(s) & " ") > .ConsoleWidth Then
                            b = b & t & vbCrLf
                            t = sp(s) & " "
                        Else
                            t = t & sp(s) & " "
                        End If
                    Next
                    b = b & t & vbCrLf
                End If
            Next
            .output.Text = b
        End With
    End Sub

#Region "Parse Columns"

    Public Sub GetCols(ByRef f As MDIChild)
        With f
            .Cols = Nothing

            For i As Integer = 0 To UBound(.TableLines)
                If Strings.Left(.TableLines(i), Len("CREATE ")) = "CREATE " Then
                    .CurrentTable = Split(.TableLines(i), " ")(2)
                Else
                    AddCol(f, .TableLines(i))
                End If
            Next
        End With
    End Sub

    Public Sub AddCol(ByRef f As MDIChild, ByVal strLine As String)

        Dim hide As Boolean = False

        With f
            If InStr(strLine, " (") > 0 Then

                Dim col As String = Trim(Split(strLine, " (")(0))
                Dim r As String = Strings.Right(strLine, Len(strLine) - InStr(strLine, "("))

                If Right(r, 2) = "XX" Then
                    r = Left(r, Len(r) - 2)
                    hide = True
                Else
                    hide = False
                End If

                r = Strings.Left(r, Len(r) - 1)
                Dim c() As String = Split(r, ",")
                Select Case c(0)
                    Case "INT", "REAL", "CHAR", "DATE"
                        If UBound(c) >= 2 Then
                            Try
                                ReDim Preserve .Cols(5, UBound(.Cols, 2) + 1)
                            Catch
                                ReDim .Cols(5, 0)
                            Finally
                                Dim this As Integer = UBound(.Cols, 2)
                                .Cols(0, this) = col
                                .Cols(1, this) = .CurrentTable
                                .Cols(2, this) = Trim(c(0))
                                .Cols(3, this) = Trim(c(1))
                                If UBound(c) = 3 Then
                                    Try
                                        .Cols(4, this) = Trim(c(3))
                                    Catch
                                        .Cols(4, this) = Trim(c(2))
                                    End Try
                                Else
                                    .Cols(4, this) = Trim(c(2))
                                End If
                                .Cols(5, this) = hide
                            End Try
                        End If
                End Select
            End If
        End With
    End Sub

#End Region

#Region "Where Clause commands"

    Public Sub GetWhere(ByRef f As MDIChild)

        Dim part() As String = Nothing
        Dim l() As String

        With f
            Dim t As String = .data.Text
            RemoveBlankLines(t)

            If InStr(t, "WHERE") > 0 Then
                part = Split(.data.Text, "WHERE", , CompareMethod.Text)
                l = Split(part(0), vbCrLf)
                .WhereClause = Trim(part(1))
                RemoveBlankLines(.WhereClause)
            Else
                l = Split(.data.Text, vbCrLf)
            End If

            .TableLines = l
            .txtWhere.Text = "WHERE " & .WhereClause

        End With

    End Sub

    Public Sub SetWhere(ByRef f As MDIChild)

        Dim part() As String = Nothing

        With f
            Dim t As String = .data.Text
            RemoveBlankLines(t)

            If InStr(t, "WHERE") > 0 Then
                part = Split(.data.Text, "WHERE", , CompareMethod.Text)
                .data.Text = part(0) & .txtWhere.Text
            Else
                .data.Text = .data.Text & .txtWhere.Text
            End If

        End With

    End Sub

    Private Sub RemoveBlankLines(ByRef Str As String)
        ' Remove blank lines
        Dim tt() As String = Split(Str, vbCrLf)
        Dim bstr As String = ""
        For i As Integer = 0 To UBound(tt)
            If Not (Len(Trim(tt(i))) = 0) Then
                bstr += tt(i) & vbCrLf
            End If
        Next
        Str = bstr
    End Sub

#End Region

    Public Sub DrawVarTable(ByRef f As MDIChild, ByRef tbl As ListView)

        MakeLoadVars(f)

        Dim lv As ListViewItem = Nothing
        Dim s As ListViewItem.ListViewSubItem
        With tbl
            .Items.Clear()
            .Columns.Clear()
            .Columns.Add("Hidden", 75, HorizontalAlignment.Center)
            .Columns.Add("Column Name", 75, HorizontalAlignment.Left)
            .Columns.Add("Table Name", 75, HorizontalAlignment.Left)
            .Columns.Add("Column Type", 75, HorizontalAlignment.Left)
            .Columns.Add("Column Width", 75, HorizontalAlignment.Left)
            .Columns.Add("Comment", 75, HorizontalAlignment.Left)
        End With

        With f
            For i As Integer = 0 To UBound(.Cols, 2)

                s = New ListViewItem.ListViewSubItem
                Select Case CBool(.Cols(5, i))
                    Case True
                        lv = tbl.Items.Add("X")
                    Case False
                        lv = tbl.Items.Add("-")
                End Select

                s = New ListViewItem.ListViewSubItem
                s.Text = .Cols(0, i)
                lv.SubItems.Add(s)

                s = New ListViewItem.ListViewSubItem
                s.Text = Replace(.Cols(1, i), "*", "")
                lv.SubItems.Add(s)

                s = New ListViewItem.ListViewSubItem
                s.Text = .Cols(2, i)
                lv.SubItems.Add(s)

                s = New ListViewItem.ListViewSubItem
                s.Text = .Cols(3, i)
                lv.SubItems.Add(s)

                Try
                    s = New ListViewItem.ListViewSubItem
                    Dim c As String = ""
                    c = Left(.Cols(4, i), Len(.Cols(4, i)) - 1)
                    c = Right(c, Len(c) - 1)
                    s.Text = c
                    lv.SubItems.Add(s)
                Catch
                End Try

            Next

            maxopColumn(tbl)

        End With

    End Sub

    Public Function Tables(ByRef f As MDIChild) As String()
        With f
            Dim tt As String = ""
            Dim fr() As String = Nothing
            If Not IsNothing(.Cols) Then
                For i As Integer = 0 To UBound(.Cols, 2)
                    If Not .Cols(1, i) = tt Then
                        Try
                            ReDim Preserve fr(UBound(fr) + 1)
                        Catch
                            ReDim fr(0)
                        Finally
                            fr(UBound(fr)) = .Cols(1, i)
                        End Try
                        tt = .Cols(1, i)
                    End If
                Next
                Return fr
            Else
                Return Nothing
            End If
        End With
    End Function

    Public Function Columns(ByRef f As MDIChild, Optional ByVal Table As String = "", Optional ByVal ColType As eType = eType.eUNSPECIFIED, Optional ByVal ShowHidden As Boolean = True) As String()

        Dim unknowntable As Boolean = True
        If Len(Table) > 0 Then
            Dim tt() As String = Tables(f)
            For i As Integer = 0 To UBound(tt)
                If LCase(Trim(tt(i))) = LCase(Trim(Table)) Then
                    unknowntable = False
                    Exit For
                End If
            Next
        End If

        If unknowntable And Not Len(Table) = 0 Then
            MsgBox("Unknown Table: " & Table, MsgBoxStyle.Critical)
            Return Nothing
        End If

        With f
            Dim fr() As String = Nothing
            For i As Integer = 0 To UBound(.Cols, 2)
                If (Len(Trim(Table)) = 0) Or (LCase(Trim(Table)) = LCase(Trim(.Cols(1, i)))) Then
                    If ColType = eType.eUNSPECIFIED Or ColumnType(f, i) = ColType Then
                        If (ShowHidden) Or (Not (ShowHidden) And Not (CBool(.Cols(5, i)))) Then
                            Try
                                ReDim Preserve fr(UBound(fr) + 1)
                            Catch
                                ReDim fr(0)
                            Finally
                                fr(UBound(fr)) = columnname(f, i)
                            End Try
                        End If
                    End If
                End If
            Next
            Return fr
        End With
    End Function

    Public Function ColumnName(ByRef f As MDIChild, ByVal ID As Integer) As String
        With f
            If InStr(.Cols(1, ID), "*") > 0 Then
                Return .Cols(0, ID) & Split(.Cols(1, ID), "*")(1)
            Else
                Return .Cols(0, ID)
            End If
        End With
    End Function

    Public Function ColumnType(ByRef f As MDIChild, ByVal i As Integer) As eType

        With f
            Select Case .Cols(2, i)
                Case "INT"
                    Return eType.eINT
                Case "REAL"
                    Return eType.eREAL
                Case "CHAR"
                    Select Case Trim(.Cols(3, i))
                        Case 1
                            Return eType.eBIT
                        Case Else
                            Return eType.eCHAR
                    End Select
                Case "DATE"
                    Return eType.eDATE
                Case Else
                    Return eType.eUNSPECIFIED
            End Select
        End With

    End Function

#Region "Loading reference"

    Public Sub DrawLoadingReference(ByRef f As MDIChild, ByRef textStr As String)
        MakeLoadVars(f)
        With f
            'textStr += "/*** Load reference " & vbCrLf & "Record Type: " & .RecordType & vbCrLf
            For i As Integer = 0 To UBound(.LoadVars, 2)
                textStr += .LoadVars(0, i) & Chr(9) & .LoadVars(1, i) & vbCrLf
            Next
            'textStr += "*/"
        End With
    End Sub

    Public Sub DrawLoadingReference(ByRef f As MDIChild, ByRef tbl As ListView)

        MakeLoadVars(f)

        Dim lv As ListViewItem
        Dim s As ListViewItem.ListViewSubItem
        With tbl
            .Items.Clear()
            .Columns.Clear()
            .Columns.Add("Variable Name", 75, HorizontalAlignment.Left)
            .Columns.Add("Gen. Load Column", 75, HorizontalAlignment.Left)
            .Columns.Add("Column Width", 75, HorizontalAlignment.Left)
            .Columns.Add("Comment", 25, HorizontalAlignment.Left)
        End With

        With f
            .lblNoref.Visible = False
            .LoadRef.Visible = True

            If Not IsNothing(.LoadVars) Then
                For i As Integer = 0 To UBound(.LoadVars, 2)

                    lv = tbl.Items.Add(.LoadVars(0, i))

                    s = New ListViewItem.ListViewSubItem
                    s.Text = .LoadVars(1, i)
                    lv.SubItems.Add(s)

                    s = New ListViewItem.ListViewSubItem
                    s.Text = .LoadVars(3, i)
                    lv.SubItems.Add(s)

                    s = New ListViewItem.ListViewSubItem
                    s.Text = .LoadVars(2, i)
                    lv.SubItems.Add(s)

                Next
                maxopColumn(tbl)
            End If
        End With

    End Sub

#End Region

    Private Sub maxopColumn(ByVal tbl As ListView)
        With tbl
            For c As Integer = 0 To .Columns.Count - 1
                .Columns(c).AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent)
                Dim w1 As Integer = .Columns(c).Width
                .Columns(c).AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize)
                Dim w2 As Integer = .Columns(c).Width
                .Columns(c).Width = MaxOP(w1, w2)
            Next
        End With
    End Sub

    Private Function MaxOP(ByVal a As Integer, ByVal b As Integer)
        If a > b Then
            Return a
        Else
            Return b
        End If
    End Function

    Public Sub addTable(ByRef f As MDIChild, ByVal TableDef As String)
        With f
            .Changed = True
            If Strings.Left(.data.Text, 2) = "##" Then
                Dim i As Integer = InStr(.data.Text, vbCrLf) + 1
                Dim l As String = Strings.Mid(.data.Text, 1, i)
                Dim r As String = Strings.Mid(.data.Text, i + 1, Len(.data.Text) - i)
                .data.Text = l & TableDef & r
            Else
                .data.Text = TableDef & .data.Text
            End If

        End With
    End Sub

End Module
