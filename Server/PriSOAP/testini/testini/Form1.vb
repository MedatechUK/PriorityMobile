Imports System.threading

Public Module main

    Public Sub Main()
        Dim ini As New Priority.tabulaini
        For Each k As String In ini.iniDictionary.Keys
            For Each n As String In ini.iniDictionary(k).Keys
                Console.WriteLine( _
                    String.Format( _
                        "{0}.{1}={2}", _
                        k, n, ini.iniDictionary(k)(n) _
                    ) _
                )
            Next
        Next

        Console.WriteLine( _
            String.Format( _
                "Priority Directory: {0}", _
                ini.iniValue("Environment", "Priority Directory") _
            ) _
        )


        Dim keypress As String = ""
        Do
            keypress = Console.ReadKey(False).ToString
            Thread.Sleep(100)
        Loop Until keypress.Length > 0

    End Sub

End Module
