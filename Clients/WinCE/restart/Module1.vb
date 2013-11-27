Imports System.Runtime.InteropServices
Module Module1

    Const POWER_STATE_RESET As Integer = &H800000
    <DllImport("coredll")> _
    Private Function SetSystemPowerState(ByVal psState As String, ByVal StateFlags As Integer, ByVal Options As Integer) As Integer
    End Function

    Sub Main()
        Dim StartTime As Date = Now
        Do
            If DateDiff(DateInterval.Hour, StartTime, Now) > 0 Then
                If Now.Minute = 30 And Now.Hour = 16 Then
                    SetSystemPowerState(Nothing, POWER_STATE_RESET, 0)
                End If
            End If
            Threading.Thread.Sleep(500)
        Loop
    End Sub

End Module
