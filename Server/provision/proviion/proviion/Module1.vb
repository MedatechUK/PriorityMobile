Module Module1

    Sub Main()
        Dim usr As New Dictionary(Of String, String)
        Dim url As String = "http://owa.virginstrausswater.com:8080/"
        Using sr As New System.IO.StreamReader("c:\users.txt")
            Do Until sr.EndOfStream
                usr.Add(sr.ReadLine, System.Guid.NewGuid.ToString.Split("-").First)
            Loop
        End Using
        For Each u As String In usr.Keys
            Console.WriteLine("<user ProvisionString=""{1}""> <username>{0}</username> <url>{2}</url> </user>", u, usr(u), url)
        Next
        Console.WriteLine("")
        For Each u As String In usr.Keys
            Console.WriteLine("*{1}*{2}{0}", u, usr(u), Chr(9))
        Next
    End Sub

End Module
