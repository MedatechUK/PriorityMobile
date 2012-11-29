Module Output

    Public Function UnHidden(ByVal f As MDIChild) As Integer()
        Dim c() As Integer = Nothing
        With f
            For i As Integer = 0 To UBound(.Cols, 2)
                If Not (CBool(.Cols(5, i))) Then
                    Try
                        ReDim Preserve c(UBound(c) + 1)
                    Catch
                        ReDim c(0)
                    Finally
                        c(UBound(c)) = i
                    End Try
                End If
            Next
        End With
        Return c
    End Function

    Public Sub DeclareVars(ByRef f As MDIChild, ByRef Text As String)
        With f
            If .showINT Then
                Text += "/* Integers */" & vbCrLf
                Declaration(f, eType.eINT, Text)
                Text += "0;" & vbCrLf
            End If

            If .showREAL Then
                Text += "/* Reals */" & vbCrLf
                Declaration(f, eType.eREAL, Text)
                Text += "0.0;" & vbCrLf
            End If

            If .showBIT Then
                Text += "/* Boolean*/" & vbCrLf
                Declaration(f, eType.eBIT, Text)
                Text += "'\n';" & vbCrLf
            End If

            If .showCHAR Then
                Text += "/* Strings */" & vbCrLf
                Declaration(f, eType.eCHAR, Text)
                Text += "'';" & vbCrLf
            End If

            If .showDATE Then
                Text += "/* Dates */" & vbCrLf
                Declaration(f, eType.eDATE, Text)
                Text += "1/1/88;" & vbCrLf
            End If
        End With
    End Sub

    Public Sub CursorSelect(ByRef f As MDIChild, ByRef text As String)

        With f
            text += "DECLARE " & .CursorName & " CURSOR FOR " & vbCrLf
            text += "SELECT " & vbCrLf

            Dim c() As Integer = UnHidden(f)            
            For i As Integer = 0 To UBound(c)
                text += "   " & Replace(.Cols(1, c(i)), "*", "") & "." & .Cols(0, c(i))
                If i < UBound(c) Then text += ", "
                text += "/* " & Replace(.Cols(4, c(i)), "'", "") & " */" & vbCrLf
            Next

            Dim fr() As String = Tables(f)

            text += "FROM "
            For i As Integer = 0 To UBound(fr)
                If InStr(fr(i), "*") > 0 Then
                    text += Split(fr(i), "*")(0) & " " & Replace(fr(i), "*", "")
                Else
                    text += fr(i)
                End If
                If i < UBound(fr) Then
                    text += ", "
                Else
                    text += " "
                End If
            Next
            text += vbCrLf

            text += "WHERE 0 = 0" & vbCrLf & "AND " & .WhereClause & ";" & vbCrLf

        End With

    End Sub

    Public Sub InsertIntoGeneralLoad(ByRef f As MDIChild, ByRef textStr As String)

        MakeLoadVars(f)

        With f
            If .WriteLoad Then
                textStr += "INSERT INTO GENERALLOAD ( " & vbCrLf & _
                    "   RECORDTYPE, " & vbCrLf & _
                    "   LINE, " & vbCrLf

                For i As Integer = 0 To UBound(.LoadVars, 2)
                    textStr += "   " & .LoadVars(0, i)
                    If i < UBound(.LoadVars, 2) Then
                        textStr += ", "
                    Else
                        textStr += " "
                    End If
                    textStr += .LoadVars(2, i) & vbCrLf
                Next

                textStr += " ) VALUES ( " & vbCrLf & "   '" & .RecordType & "', " & vbCrLf & "   :LINE, " & vbCrLf

                For i As Integer = 0 To UBound(.LoadVars, 2)
                    textStr += "   " & ":" & .VarPref & .LoadVars(1, i)
                    If i < UBound(.LoadVars, 2) Then
                        textStr += ", "
                    Else
                        textStr += " "
                    End If
                    textStr += .LoadVars(2, i) & vbCrLf
                Next
                textStr += ");" & vbCrLf
                textStr += "SELECT :LINE + 1 INTO :LINE FROM DUMMY; " & vbCrLf
            End If
        End With
    End Sub

    Public Sub CursorFetch(ByRef f As MDIChild, ByRef Text As String)

        Dim c() As Integer = UnHidden(f)
        With f
            Text += "OPEN " & .CursorName & " ;" & vbCrLf
            Text += "LABEL " & CStr(.StartLabel + 10) & ";" & vbCrLf
            Text += "FETCH " & .CursorName & " INTO " & vbCrLf

            For i As Integer = 0 To UBound(c)
                Text += "   :" & .VarPref & columnname(f, c(i))
                If i < UBound(c) Then
                    Text += ", "
                Else
                    Text += " "
                End If
                Text += "/* " & Replace(.Cols(4, c(i)), "'", "") & " */" & vbCrLf
            Next
            Text = Text & ";" & vbCrLf
        End With

    End Sub

    Public Sub WriteSelectVars(ByRef f As MDIChild, ByRef Text As String)
        Dim c() As Integer = UnHidden(f)
        With f
            If .SelectVars Then
                Text += "SELECT " & vbCrLf
                For i As Integer = 0 To UBound(c)
                    Text += "   :" & .VarPref & columnname(f, c(i))
                    If i < UBound(c) Then
                        Text += ", "
                    Else
                        Text += " "
                    End If
                    Text += "/* " & Replace(.Cols(4, c(i)), "'", "") & " */" & vbCrLf
                Next
                Text += "FROM DUMMY FORMAT;" & vbCrLf
            End If
        End With

    End Sub

    Public Sub QuitOnError(ByRef f As MDIChild, ByRef text As String)
        With f
            text += "GOTO " & CStr(.StartLabel + 99) & " WHERE :RETVAL <= 0;" & vbCrLf
        End With
    End Sub

    Public Sub ProcStart(ByRef f As MDIChild, ByRef Text As String)
        With f
            If Not .IsSub Then
                If .WriteLoad Then
                    Text += "/* Initialise :LINE Variable */" & vbCrLf
                    Text += "SELECT 1 INTO :LINE FROM DUMMY; " & vbCrLf
                    If .LinkGenLoad Then
                        Text += "/* Link to table GENERALLOAD */" & vbCrLf
                        Text += "LINK GENERALLOAD TO :$.LNK;" & vbCrLf
                        QuitOnError(f, Text)
                    End If
                End If
            Else
                Text += "/*SUB***********************/" & vbCrLf
                Text += "SUB " & .StartLabel & " ;" & vbCrLf
            End If
        End With
    End Sub

    Public Sub ProcEnd(ByRef f As MDIChild, ByRef Text As String)
        With f

            Text += "LOOP " & CStr(.StartLabel + 10) & " ;" & vbCrLf
            Text += "LABEL " & CStr(.StartLabel + 99) & " ;" & vbCrLf
            Text += "CLOSE " & .CursorName & ";" & vbCrLf

            If Not .IsSub Then
                If .WriteLoad Then
                    If .LinkGenLoad Then
                        Text += "/* Run the interface on the linked file */" & vbCrLf
                        Text += "EXECUTE INTERFACE 'MYINTERFACE', SQL.TMPFILE, '-L', :$.LNK ;" & vbCrLf
                        Text += "/* Insert the message that the interface gave to :PAR1 */" & vbCrLf
                        Text += "SELECT MESSAGE INTO :PAR1 FROM ERRMSGS" & vbCrLf
                        Text += "WHERE USER = ATOI(RSTRIND(SQL.CLIENTID,1,9))" & vbCrLf
                        Text += "AND TYPE = 'i' AND LINE = 1;" & vbCrLf
                        Text += "/* Display the message if any lines were not loaded successfully */" & vbCrLf
                        Text += "ERRMSG1 WHERE EXISTS (SELECT 'X' FROM GENERALLOAD" & vbCrLf
                        Text += "WHERE LOADED <> ‘Y’ AND LINE > 0);" & vbCrLf
                        Text += "UNLINK GENERALLOAD ;" & vbCrLf
                    Else
                        Text += "SELECT * FROM GENERALLOAD FORMAT;" & vbCrLf
                    End If
                End If
                Text += "END " & vbCrLf & ";" & vbCrLf
            Else
                Text += "RETURN ;" & vbCrLf
                Text += "/*END SUB ******************/" & vbCrLf
            End If
        End With
    End Sub

    Public Sub Declaration(ByRef f As MDIChild, ByVal ColType As eType, ByRef Text As String)
        With f
            If ColType = eType.eINT And Not (.IsSub) And .WriteLoad Then
                Text += ":LINE = /* Line Value for Loading */" & vbCrLf
            End If
            For i As Integer = 0 To UBound(.Cols, 2)
                If ColumnType(f, i) = ColType And Not (CBool(.Cols(5, i))) Then
                    Text += ":" & .VarPref & ColumnName(f, i) & " = /* " & Replace(.Cols(4, i), "'", "") & " */" & vbCrLf
                End If
            Next
        End With
    End Sub

    Public Sub MakeLoadVars(ByVal f As MDIChild)

        Dim var(,) As String = Nothing

        Dim ldp As String = ""
        Dim RC As Integer = 1
        Dim IC As Integer = 1
        Dim CC As Integer = 1
        Dim DC As Integer = 1
        Dim TX As Integer = 1

        With f
            For i As Integer = 0 To UBound(.Cols, 2)
                If Not CBool(.Cols(5, i)) Then
                    Select Case ColumnType(f, i)
                        Case eType.eINT
                            ldp = "INT" & CStr(IC)
                            IC = IC + 1
                        Case eType.eREAL
                            ldp = "REAL" & CStr(RC)
                            RC = RC + 1
                        Case eType.eBIT
                            ldp = "CHAR" & CStr(CC)
                            CC = CC + 1
                        Case eType.eCHAR
                            ldp = "TEXT" & CStr(TX)
                            TX = TX + 1
                        Case eType.eDATE
                            ldp = "DATE" & CStr(DC)
                            DC = DC + 1
                        Case eType.eUNSPECIFIED
                            ldp = ""
                    End Select
                    NewVar(var, ldp, ColumnName(f, i), "/* " & Replace(.Cols(4, i), "'", "") & " */", .Cols(3, i))
                End If
            Next
            .LoadVars = var
        End With

    End Sub

    Private Sub NewVar(ByRef Var(,) As String, ByVal ldp As String, ByVal vString As String, ByVal Comment As String, ByVal Width As String)
        Try
            ReDim Preserve Var(3, UBound(Var, 2) + 1)
        Catch
            ReDim Var(3, 0)
        Finally
            Var(0, UBound(Var, 2)) = ldp
            Var(1, UBound(Var, 2)) = vString
            Var(2, UBound(Var, 2)) = Comment
            Var(3, UBound(Var, 2)) = Width
        End Try
    End Sub

End Module
