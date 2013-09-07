Imports cumulicity

Module cumulicitycli

    Sub Main()

        Dim Credentials As New cumulocityCredentials( _
            "http://staging-vendme.cumulocity.com/vendme-service/api/", _
            "vendme", _
            "satbinder", _
            "erptest55" _
        )

        'Dim EchoRequest As New cumulocityEcho(Credentials, "Hello World!")
        'With EchoRequest
        '    Dim ex As New Exception
        '    Dim result As responseEcho = .Result(ex)
        '    If IsNothing(ex) Then
        '        Console.Write(result.Message)
        '    Else
        '        Console.Write(ex.Message)
        '    End If
        'End With

        Dim SalesRequest As New cumulocitySales(Credentials, "356895036643990", 0)
        With SalesRequest
            Dim ex As New Exception
            Dim result As responseSales = .Result(ex)
            If IsNothing(ex) Then
                For Each s As Slot In result.Slot
                    Console.WriteLine(s.ToString)
                Next
            Else
                Console.Write(ex.Message)
            End If
        End With

    End Sub

End Module
