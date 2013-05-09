Imports DTI

Module emgQR

    Sub main()
        Dim barCode As New DTI.ImageMan.Barcode.BarcodeEncoder()
        Dim args() As String = System.Environment.GetCommandLineArgs()
        Dim dataIn As String = ""
        

        Try
            If Not IsNumeric(args(1)) Then
                Console.WriteLine("String arguments must be preceded by number of arguments to parse")
                Console.ReadLine()
                Exit Sub
            Else
                For i As Integer = 2 To CInt(args(1)) + 1
                    dataIn += args(i)
                Next
            End If

        Catch ex As Exception
            Console.WriteLine("No/malformed data!")
            Console.ReadLine()
            Exit Sub
        End Try

        If Not IsNothing(dataIn) Then
            barCode.Content = dataIn
            barCode.BarcodeFormat = ImageMan.Barcode.BarcodeFormat.QRCode
        End If

        barCode.Width = 200
        barCode.Height = 200
        Dim pathA As String = System.IO.Path.GetFullPath(System.IO.Path.Combine(System.Environment.CurrentDirectory, "..\..\labels\qr"))

        Dim img As DTI.ImageMan.ImImage = barCode.GenerateBarcode
        Dim jpg As New DTI.ImageMan.Codecs.JpgEncoder
        Dim path As String = String.Format("{0}\{1}.jpg", pathA, dataIn)

        System.IO.Directory.CreateDirectory(pathA)

        Dim stream As New System.IO.FileStream(path, IO.FileMode.OpenOrCreate)

        jpg.Save(stream, img, Nothing)

    End Sub

End Module
