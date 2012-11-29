Module main

    Public PriPROCSVC As System.ServiceProcess.ServiceController

    Sub main(ByVal args() As String)
        Try
            doStartArgs(args)
        Catch ex As Exception
            MsgBox(String.Format("Invalid argument: {0}", ex.Message), MsgBoxStyle.OkOnly + MsgBoxStyle.Critical, "Error.")
        Finally
            Application.Run(New frmConsole)
        End Try

    End Sub

End Module
