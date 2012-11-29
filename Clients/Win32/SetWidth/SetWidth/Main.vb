'-f ftestf2.txt -p *:80 -p [0-9]:120 -w
Imports System.Threading
Imports System.Reflection
Imports System.io
Imports ConsoleApp
Imports System.Text.RegularExpressions

Module Main

    Dim widstr As New Dictionary(Of Integer, tWidthStr)
    Dim inFile As String = ""
    Dim outFile As String = ""
    Dim LineEnd As String = vbCrLf
    Dim defWid As Integer = -1
    Dim OutputLineEnd As String = Nothing

    Enum myRunMode As Integer
        Normal = 0
        Config = 1
    End Enum

    Dim WithEvents cApp As New ConsoleApp.CA

    Sub Main()
        With cApp
            .StrDeliminator = "%"
            .RunMode = myRunMode.Normal
            .doWelcome(Assembly.GetExecutingAssembly())

            'Console.WriteLine("")
            'Console.WriteLine("Press any key to continue.")
            'Dim strInput As String = Console.ReadKey(False).ToString
            'While (strInput = "")
            '    Thread.Sleep(100)
            'End While

            Try
                .GetArgs(Command)
            Catch ex As Exception
                Console.WriteLine(ex.Message)
                .Quit = True
            End Try

            If Not .Quit Then
                If defWid = -1 Then
                    Console.WriteLine("Please specify a default line width like this: -p *:80.")
                    .Quit = True
                End If

                If inFile.Length = 0 Then
                    Console.WriteLine("No input file specified.")
                    .Quit = True
                End If
            End If

            If outFile.Length = 0 Then outFile = inFile

            If Not .Quit Then
                Select Case .RunMode
                    Case myRunMode.Normal
                        PAYLOAD()
                End Select
            End If

            cApp.Finalize()

        End With
    End Sub

    Private Sub cApp_Switch(ByVal StrVal As String, ByRef State As String, ByRef Valid As Boolean) Handles cApp.Switch
        Try
            With cApp
                Select Case StrVal
                    Case "f", "input"
                        State = "f"
                    Case "o", "output"
                        State = "f"
                    Case "p", "wid", "width"
                        State = "p"
                    Case "le"
                        State = "le"
                    Case "ole"
                        State = "ole"
                    Case Else
                        Valid = False
                End Select
            End With
        Catch ex As Exception
            Console.Write(ex.Message)
        End Try
    End Sub

    Private Sub cApp_SwitchVar(ByVal State As String, ByVal StrVal As String, ByRef Valid As Boolean) Handles cApp.SwitchVar
        Try
            With cApp
                Select Case State
                    Case "f"
                        If Not File.Exists(StrVal) Then
                            Valid = False
                            Console.WriteLine("Invalid input file.")
                            Exit Sub
                        End If
                        inFile = StrVal
                    Case "o"
                        If Not File.Exists(StrVal) Then
                            Valid = False
                            Console.WriteLine("Invalid output.")
                            Exit Sub
                        End If
                        outFile = StrVal
                    Case "p"
                        If Not InStr(StrVal, ":") > 0 Then
                            Valid = False
                            Console.WriteLine("Invalid width input.")
                            Exit Sub
                        End If
                        Dim w() As String = Split(StrVal, ":")
                        Select Case w(0)
                            Case "*"
                                defWid = CInt(w(1))
                            Case Else
                                With widstr
                                    .Add(.Count + 1, New tWidthStr(w(0), CInt(w(1))))
                                End With
                        End Select
                    Case "le"
                        LineEnd = StrVal
                        LineEnd = Replace(LineEnd, "LF", Chr(10), , , CompareMethod.Text)
                        LineEnd = Replace(LineEnd, "CR", Chr(13), , , CompareMethod.Text)
                    Case "ole"
                        OutputLineEnd = StrVal
                        OutputLineEnd = Replace(OutputLineEnd, "LF", Chr(10), , , CompareMethod.Text)
                        OutputLineEnd = Replace(OutputLineEnd, "CR", Chr(13), , , CompareMethod.Text)

                    Case Else
                        Valid = False
                End Select
            End With
        Catch ex As Exception
            Console.Write(ex.Message)
        End Try
    End Sub

    Sub PAYLOAD()

        If IsNothing(OutputLineEnd) Then
            OutputLineEnd = LineEnd
        End If

        Dim f As Boolean = False
        Dim lines() As String = Nothing
        Dim sr As New StreamReader(inFile)
        Using sr
            Dim fit As String = sr.ReadToEnd()
            lines = Split(fit, LineEnd)
        End Using
        For i As Integer = 0 To UBound(lines)
            f = False
            For Each ws As tWidthStr In widstr.Values
                Dim regex As Regex = New Regex("^" & ws.WidthRegEx)
                If regex.IsMatch(lines(i)) Then
                    lines(i) = Left(lines(i), ws.Width)
                    f = True
                    Exit For
                End If
            Next
            If Not (f) Then lines(i) = Left(lines(i), defWid)
        Next
        Dim sw As New StreamWriter(outFile)
        Using sw
            For i As Integer = 0 To UBound(lines)
                sw.Write(lines(i) & OutputLineEnd)
            Next
        End Using

    End Sub

End Module
