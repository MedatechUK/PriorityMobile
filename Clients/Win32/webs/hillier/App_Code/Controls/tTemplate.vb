Imports Microsoft.VisualBasic
Imports System.IO

Public Structure tTemplate

    Private _Path As String
    Private _TemplateName As String
    Public _html As String
    Public PreHTML As String
    Public PostHTML As String

    Public Sub New(ByVal ipage As Page, ByVal Template As String)

        _html = ""
        _Path = ipage.Server.MapPath("/HTMLTemplates/" & Template)
        If File.Exists(_Path) Then            
            Dim sr As New StreamReader(_Path)
            With sr
                _html = .ReadToEnd
                .Close()
            End With
        End If

        If InStr(_html, "$REPEAT$") > 0 Then
            Dim tmp() As String = Split(_html, "$REPEAT$", , CompareMethod.Text)            
            PreHTML = tmp(0)
            _html = tmp(1)
        End If

        If InStr(_html, "$/REPEAT$") > 0 Then
            Dim tmp() As String = Split(_html, "$/REPEAT$", , CompareMethod.Text)
            PostHTML = tmp(1)
            _html = tmp(0)
        End If

    End Sub

    Public Sub Parser(ByRef Html As String, ByVal NamePair(,) As String, ByRef ph As PlaceHolder, ByVal MOD2 As Integer)

        For i As Integer = 0 To UBound(NamePair, 2)
            Html = Replace(Html, "%" & NamePair(0, i) & "%", NamePair(1, i), , , CompareMethod.Text)
        Next
        Html = Replace(Html, "%MOD2%", MOD2, , , CompareMethod.Text)
        With ph
            Dim htmlpart() As String = getHTMLPart(Html)
            Dim i As Integer = 0

            While i <= UBound(htmlpart)
                If i Mod 2 = 0 Then
                    If Len(Trim(htmlpart(i))) > 0 Then
                        .Controls.Add(New LiteralControl(htmlpart(i)))
                    End If
                    i = i + 1
                Else
                    If Len(Trim(htmlpart(i))) > 0 Then
                        Dim cmd() As String = Split(Trim(htmlpart(i)), " ")
                        Dim str As String = Trim(Right(Trim(htmlpart(i)), Len(Trim(htmlpart(i))) - (Len(cmd(0)))))

                        ' Perform as script command
                        Select Case LCase(Trim(cmd(0)))
                            Case "if"
                                Dim haselse As Boolean = _
                                    CBool(LCase(Trim(Split(Trim(htmlpart(i + 2)), " ")(0))) = "else")
                                If Evaluate(str, NamePair) Then
                                    .Controls.Add(New LiteralControl(htmlpart(i + 1)))
                                Else
                                    If haselse Then
                                        .Controls.Add(New LiteralControl(htmlpart(i + 3)))
                                    End If
                                End If

                                If haselse Then
                                    i = i + 5
                                Else
                                    i = i + 3
                                End If

                            Case "ph"
                                Dim tph As New PlaceHolder
                                tph.ID = str
                                .Controls.Add(tph)
                                i = i + 1

                        End Select
                    Else
                        i = i + 1
                    End If
                End If
            End While

        End With

    End Sub

    Private Function getHTMLPart(ByVal html) As String()

        Dim ret() As String = Nothing

        If Not InStr(html, "<%", CompareMethod.Text) > 0 Then
            newpart(ret, html)
        Else
            ' Has script
            While InStr(html, "<%", CompareMethod.Text) > 0
                Dim t() As String
                t = Split(html, "<%", , CompareMethod.Text)
                newpart(ret, t(0))
                html = Right(html, Len(html) - (Len(t(0)) + 2))
                t = Split(html, "%>", , CompareMethod.Text)
                newpart(ret, t(0))
                html = Right(html, Len(html) - (Len(t(0)) + 2))
            End While
            newpart(ret, html)
        End If
        Return ret

    End Function

    Private Sub newpart(ByRef Ar() As String, ByVal item As String)
        Try
            ReDim Preserve Ar(UBound(Ar) + 1)
        Catch ex As Exception
            ReDim Ar(0)
        Finally
            Ar(UBound(Ar)) = item
        End Try
    End Sub

    Private Function Evaluate(ByRef Condition As String, ByVal NamePair(,) As String) As Boolean

        Dim ret As Boolean = False
        Dim c() As String = Split(LCase(Condition), "and")
        Dim barray(UBound(c)) As Boolean

        Dim name As String = ""
        Dim value As String = ""
        Dim compare As Integer = 0

        For i As Integer = 0 To UBound(c)
            If InStr(c(i), "=") > 0 Then
                name = Split(c(i), "=")(0)
                value = Split(c(i), "=")(1)
                compare = 1
            ElseIf InStr(c(i), "<>") > 0 Then
                name = Split(c(i), "<>")(0)
                value = Split(c(i), "<>")(1)
                compare = 2
            End If
            For y As Integer = 0 To UBound(NamePair, 2)
                name = LCase(Trim(Replace(name, NamePair(0, y), NamePair(1, y), , , CompareMethod.Text)))
                value = LCase(Trim(Replace(value, NamePair(0, y), NamePair(1, y), , , CompareMethod.Text)))
            Next
            Select Case compare
                Case 1
                    barray(i) = CBool(name = value)
                Case 2
                    barray(i) = Not (CBool(name = value))
            End Select
        Next

        For i As Integer = 0 To UBound(barray)
            If Not barray(i) Then
                Return False
            End If
        Next
        Return True
    End Function

End Structure
