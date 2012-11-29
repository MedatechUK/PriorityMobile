Module Module1
    Sub Main()
        Dim WB As New WebBrowser
        WB.Navigate("C:\priority_emgdv\autodocs\tosend\A86000022_IVC201210081309.htm")
        For i As Integer = 0 To 500
            Application.DoEvents()
            Threading.Thread.Sleep(10)
        Next
        WB.Print()
        For i As Integer = 0 To 1500
            Application.DoEvents()
            Threading.Thread.Sleep(10)
        Next
        End
    End Sub
End Module
