Imports system.threading

Module Module1

    Function Main() As Integer

        Console.WriteLine("")
        Console.WriteLine(Command)
        Console.WriteLine("")
        Console.Write("Press any key to continue.")
        Dim strinput As String = Console.ReadKey(False).ToString
        While strinput = ""
            Thread.Sleep(100)
        End While

        Return 0

    End Function

End Module
