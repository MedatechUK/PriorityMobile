Module StartUp

    Public Sub Main(ByVal args As String())

        For Each arg As String In args
            Select Case arg.ToLower
                Case "resetforms"
                    My.Settings.formLocation = New System.Drawing.Point(0, 0)
                    My.Settings.CalcLocation = New System.Drawing.Point(0, 0)
                    My.Settings.Save()
            End Select
        Next
        Application.Run(New frmClock) 'frmMain



    End Sub

End Module
