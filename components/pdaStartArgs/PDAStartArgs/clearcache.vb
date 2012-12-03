Imports System.Reflection

Module clearcache

    Public Function AppPath() As String

        Dim fullPath As String = System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase
        Return fullPath.Substring(0, fullPath.LastIndexOf("\"))

    End Function

    Sub Main()
        Dim p As New Process()
        With p
            With .StartInfo                
                .UseShellExecute = True
                .FileName = AppPath() & "\StraussPDA.exe"
                If MsgBox("This will clear the cache. Updated files will be downloaded from the server. Continue?", MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton1 + MsgBoxStyle.Question) = MsgBoxResult.Yes Then
                    .Arguments = "clearcache"
                End If
            End With
            .Start()
        End With
    End Sub

End Module
