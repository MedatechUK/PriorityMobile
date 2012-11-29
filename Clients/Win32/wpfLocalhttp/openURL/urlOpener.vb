Module urlOpener

    Sub Main()
        Try
            Dim sOutput As String = ""
            Dim sErrs As String = ""
            Dim myProcess As Process = New Process()

            With myProcess
                With .StartInfo
                    .FileName = "iexplore.exe"
                    .Arguments = "http://localhost:2000"
                    .UseShellExecute = True
                    .CreateNoWindow = False
                End With
                .Start()
            End With

        Catch EX As Exception
            MsgBox(EX.Message, MsgBoxStyle.OkOnly + MsgBoxStyle.Critical, "Error")
        End Try
    End Sub

End Module
