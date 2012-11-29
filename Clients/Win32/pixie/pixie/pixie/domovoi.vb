Imports System.IO

Module domovoi

    Dim exe As String = ""
    Dim param As String = ""
    Dim dir As String = ""

    Private ReadOnly Property FullPath()
        Get
            Dim fp As String = System.Reflection.Assembly.GetExecutingAssembly().Location
            While Not fp.EndsWith("\")
                fp = fp.Remove(fp.Length - 1, 1)
            End While
            Return fp
        End Get
    End Property

    Sub Main()

        Try
            'MsgBox(Environment.CommandLine)
            Using sr As New StreamWriter(String.Format("{0}\{1}", FullPath, "log.txt"), True)
                sr.WriteLine( _
                    String.Format( _
                        "{0}{2}{1}", _
                        Now.ToString, _
                        Environment.CommandLine, _
                        Chr(9) _
                    ) _
                )
            End Using

            Dim input As String = Split(Replace(Environment.CommandLine, "pixie:\\", "pixie://", , , CompareMethod.Text), "pixie://", , CompareMethod.Text)(1)
            input = Left(input, input.Length - 1).Replace("\?", "/?")

            If input = "/?" Then
                Using myProcess As System.Diagnostics.Process = New System.Diagnostics.Process()
                    With myProcess
                        With .StartInfo
                            .FileName = FullPath & "\help.html" 'CREATE THIS FILE WITH FILESHAREMODE.NONE 
                            .WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal
                            .CreateNoWindow = False
                            .UseShellExecute = True
                        End With
                        .Start()
                    End With
                End Using

            Else

                If InStr(input, "/?") > 0 Then
                    exe = Microsoft.VisualBasic.Strings.Left(input, InStr(input, "/?") - 1)
                    param = Strings.Right(input, input.Length - InStr(input, "/?") - 1)
                Else
                    exe = Replace(input, ".EXE/", ".EXE", , , CompareMethod.Text)
                    param = ""
                End If

                Dim e() As String = Split(Replace(exe, "/", "\"), "\")
                Dim bstr As String = ""
                exe = e(UBound(e))
                For i As Integer = 0 To UBound(e) - 1
                    bstr += e(i) & "\"
                Next
                dir = bstr

                Dim myProcess As Process = New Process()
                With myProcess
                    With .StartInfo
                        .FileName = exe
                        .Arguments = Trim(param.Replace("  ", " "))
                        .WorkingDirectory = dir
                        .UseShellExecute = True
                        .CreateNoWindow = False
                    End With
                    .Start()
                End With
            End If

        Catch ex As Exception
            MsgBox(String.Format("{0}:{4}{4}   Executable [{1}]{4}   Working Directory [{3}]{4}   Arguments [{2}]{4}{4}Please notify your system administrator.", ex.Message, exe, param, dir, vbCrLf), MsgBoxStyle.Critical, "Priority Pick-EXE (pixie)")
        End Try

    End Sub

End Module
