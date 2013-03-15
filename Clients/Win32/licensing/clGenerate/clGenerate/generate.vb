Imports licensing

Module generate

    Sub Main()
        Console.Write("Assembly name: ")
        Dim assembly As String = Console.ReadLine()
        Console.Write("MAC address (press enter to use this machine's): ")
        Dim mac As String = Console.ReadLine()

        If mac = "" Then
            mac = System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces()(0).GetPhysicalAddress.ToString
        ElseIf mac.Length < 12 Then
            Console.WriteLine("Invalid mac address!")
            Exit Sub
        End If

        Dim gn As New generator(assembly, mac)
        Console.WriteLine(gn.license)
        Console.ReadLine()

    End Sub

End Module
