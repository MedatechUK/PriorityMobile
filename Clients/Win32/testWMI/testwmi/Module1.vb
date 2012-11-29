Imports System.Management

Module Module1

    Sub Main()
        Try
            Dim ret As Boolean = False
            Dim printerQuery As ManagementObjectSearcher
            Dim queryResults As Management.ManagementObjectCollection
            Dim onePrinter As Management.ManagementObject

            printerQuery = New Management.ManagementObjectSearcher("SELECT * FROM Win32_Printer")
            queryResults = printerQuery.Get()

            Console.WriteLine("Printer" & Chr(9) & "Default")
            Console.WriteLine("--------------------------------------")
            For Each onePrinter In queryResults
                If onePrinter!default Then
                    Console.WriteLine(onePrinter!Name & Chr(9) & "X")
                Else
                    Console.WriteLine(onePrinter!Name & Chr(9) & "")
                End If
            Next
        Catch EX As Exception
            Console.WriteLine(EX.Message)
            Console.WriteLine(EX.StackTrace)
        End Try
    End Sub

End Module
