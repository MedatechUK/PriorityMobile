Imports System.io

Public Class Form1

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click

        Dim out As String = ""
        Dim er As String = ""
        runcmd("Net view", out, er)
        MessageBox.Show(out)
        MessageBox.Show(er)
    End Sub

    Private Sub runcmd(ByVal cmd As String, ByRef out As String, ByRef er As String)

        Dim myProcess As Process = New Process()

        myProcess.StartInfo.FileName = "cmd.exe"
        myProcess.StartInfo.UseShellExecute = False
        myProcess.StartInfo.CreateNoWindow = True
        myProcess.StartInfo.RedirectStandardInput = True
        myProcess.StartInfo.RedirectStandardOutput = True
        myProcess.StartInfo.RedirectStandardError = True
        myProcess.Start()
        Dim sIn As StreamWriter = myProcess.StandardInput
        sIn.AutoFlush = True

        Dim sOut As StreamReader = myProcess.StandardOutput
        Dim sErr As StreamReader = myProcess.StandardError
        sIn.Write(cmd & _
           System.Environment.NewLine)
        sIn.Write("exit" & System.Environment.NewLine)
        out = sOut.ReadToEnd()
        er = sErr.ReadToEnd
        If Not myProcess.HasExited Then
            myProcess.Kill()
        End If

        sIn.Close()
        sOut.Close()
        sErr.Close()
        myProcess.Close()

        out = Split(Split(out, cmd & vbCrLf)(1), "exit")(0)
        Dim pt As String = Split(out, vbCrLf)(UBound(Split(out, vbCrLf)))
        Do While InStr(out, vbCrLf & vbCrLf) > 0
            out = Replace(Replace(out, pt, ""), vbCrLf & vbCrLf, vbCrLf)
        Loop

    End Sub

End Class