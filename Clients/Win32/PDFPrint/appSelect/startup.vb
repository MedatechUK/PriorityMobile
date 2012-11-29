Imports System.Reflection
Imports System.IO

Module startup

    Sub main()

        'MsgBox(Environment.CommandLine)
        Dim silent As Boolean = False
        Dim cmd As String = Replace(Environment.CommandLine, Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "", , , CompareMethod.Text)
        If InStr(cmd, "pdfprint", CompareMethod.Text) > 0 Then cmd = Split(cmd, "pdfprint", , CompareMethod.Text)(1)
        If InStr(cmd, ".exe", CompareMethod.Text) > 0 Then cmd = Split(cmd, ".exe", , CompareMethod.Text)(1)
        If Left(cmd, 1) = Chr(32) Then cmd = Strings.Right(cmd, cmd.Length - 1)

        Dim switch() As String = {"-silent", "/silent", "-shh", "/shh"}
        For Each s As String In switch
            If InStr(cmd, s, CompareMethod.Text) > 0 Then
                cmd = Replace(cmd, s, "", , , CompareMethod.Text)
                silent = True
                Exit For
            End If
        Next

        Try
            Directory.SetCurrentDirectory(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location))
            If silent Then
                Dim waitcmd() As String = {"-w", "/w", "-wait", "/wait"}
                For Each s As String In waitcmd
                    If InStr(cmd, s, CompareMethod.Text) > 0 Then
                        cmd = Replace(cmd, s, "", , , CompareMethod.Text)
                        Exit For
                    End If
                Next
                cmd += " /runmode form"
                Process.Start(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\pdfprintfrm.exe", cmd)
            Else
                If Not InStr(cmd, "-w", CompareMethod.Text) > 0 And Not InStr(cmd, "/w", CompareMethod.Text) Then
                    cmd += " /w"
                End If
                cmd += " /runmode console"
                Process.Start(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\pdfprintcli.exe", cmd)
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try

    End Sub

End Module
