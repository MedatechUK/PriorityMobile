Imports System.Management
Public Class Form1

    Function DefaultPrinter() As String

        Dim ret As Boolean = False
        Dim printerQuery As ManagementObjectSearcher
        Dim queryResults As Management.ManagementObjectCollection
        Dim onePrinter As Management.ManagementObject

        printerQuery = New Management.ManagementObjectSearcher("SELECT * FROM Win32_Printer")
        queryResults = printerQuery.Get()

        For Each onePrinter In queryResults
            If onePrinter!default Then
                Return onePrinter!name
                Exit For
            End If
        Next
        Return Nothing

    End Function

    Private Sub PrintToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PrintToolStripMenuItem.Click

        Dim WB As New WebBrowser
        WB.Navigate("C:\priority_emgdv\autodocs\tosend\A86000022_IVC201210081309.htm")        
        For i As Integer = 0 To 500
            'Application.DoEvents()
            Threading.Thread.Sleep(10)
        Next
        WB.Print()
        For i As Integer = 0 To 1500
            'Application.DoEvents()
            Threading.Thread.Sleep(10)
        Next
        End

        'tmr = New Timer
        'With tmr            
        '    AddHandler .Tick, AddressOf PrintIt
        '    .Interval = 10000
        '    .Enabled = True
        'End With


        'Using myProcess As System.Diagnostics.Process = New System.Diagnostics.Process()
        '    With myProcess
        '        With .StartInfo
        '            .FileName = "C:\priority_emgdv\autodocs\tosend\A86000022_IVC201210081309.htm" 'CREATE THIS FILE WITH FILESHAREMODE.NONE 
        '            .WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal
        '            .CreateNoWindow = False
        '            .Verb = "printto"
        '            .Arguments = DefaultPrinter()
        '            .UseShellExecute = True
        '        End With
        '        Dim Print_Check_Counter As Integer = 0
        '        Try
        '            .Start()
        '        Catch ex As Exception
        '            MsgBox(ex.Message)
        '        End Try
        '    End With
        'End Using
    End Sub


End Class
