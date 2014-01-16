Imports HiQPdf
Imports HtmlAgilityPack

Module emgPDF2cli

    Sub main()
        HtmlNode.ElementsFlags.Remove("form")

        Dim args As String() = System.Environment.GetCommandLineArgs

        Dim InputFile As String = ""
        Dim Author As String = ""
        Dim Subject As String = ""
        Dim isReadOnly As Boolean = True
        Dim KeepSource As Boolean = False
        Dim HasFooter As Boolean = False
        Dim FooterHeight As Integer = 0
        Dim FooterPaginate As Boolean = False
        Dim HasHeader As Boolean = False
        Dim HeaderHeight As Integer = 0
        Dim HeaderPaginate As Boolean = False
        Dim Margins As Integer = 0
        Dim Width As Integer = 1000
        Dim PaginationAlign As PdfTextHAlign = PdfTextHAlign.Center
        Dim PageSize As Size = Size.A4
        Dim PageOrientation As PdfPageOrientation = PdfPageOrientation.Portrait
        Dim Debug As Boolean = False
        Dim outPath As String


        For i As Integer = 1 To args.Length - 1
            Select Case Left(args(i), 1)
                Case "/", "-"
                    Select Case Right(args(i), args(i).Length - 1).ToLower

                        Case "a"

                            Try
                                Author = args(i + 1)
                            Catch : End Try

                        Case "u"
                            Try
                                Subject = args(i + 1)
                            Catch : End Try

                        Case "k"
                            KeepSource = True

                        Case "w"
                            isReadOnly = False

                        Case "f"
                            HasFooter = True
                            Try
                                'if present, take next value to be pixel height
                                If IsNumeric(args(i + 1)) Then
                                    FooterHeight += CInt(args(i + 1))
                                End If
                            Catch : End Try


                        Case "h"
                            HasHeader = True
                            Try
                                'if present, take next value to be pixel height 
                                If IsNumeric(args(i + 1)) Then
                                    HeaderHeight += CInt(args(i + 1))
                                End If
                            Catch : End Try


                        Case "m"
                            Try
                                If IsNumeric(args(i + 1)) Then
                                    Margins = CInt(args(i + 1))
                                End If
                            Catch : End Try


                        Case "s"
                            Select Case args(i + 1).ToLower
                                Case "letter"
                                    PageSize = Size.Letter

                                Case "legal"
                                    PageSize = Size.Legal
                                Case "ledger"
                                    PageSize = Size.Ledger
                                Case "b0"
                                    PageSize = Size.B0
                                Case "b1"
                                    PageSize = Size.B1
                                Case "b2"
                                    PageSize = Size.B2
                                Case "b3"
                                    PageSize = Size.B3
                                Case "b4"
                                    PageSize = Size.B4
                                Case "b5"
                                    PageSize = Size.B5
                                Case "a10"
                                    PageSize = Size.A10
                                Case "a9"
                                    PageSize = Size.A9
                                Case "a8"
                                    PageSize = Size.A8
                                Case "a7"
                                    PageSize = Size.A7
                                Case "a6"
                                    PageSize = Size.A6
                                Case "a5"
                                    PageSize = Size.A5
                                Case "a3"
                                    PageSize = Size.A3
                                Case "a2"
                                    PageSize = Size.A2
                                Case "a1"
                                    PageSize = Size.A1
                                Case "a0"
                                    PageSize = Size.A0
                                Case Else
                                    PageSize = Size.A4
                            End Select


                        Case "p"
                            Try
                                Select Case args(i + 1).ToLower

                                    Case "t"
                                        FooterPaginate = False
                                        HeaderPaginate = True
                                        HasHeader = True
                                        HeaderHeight += 15

                                    Case "b"
                                        FooterPaginate = True
                                        HeaderPaginate = False
                                        FooterHeight += 15
                                        HasFooter = True

                                End Select
                                Try
                                    Select Case args(i + 2).ToLower
                                        Case "l"
                                            PaginationAlign = PdfTextHAlign.Left
                                        Case "r"
                                            PaginationAlign = PdfTextHAlign.Right
                                        Case "c"
                                            PaginationAlign = PdfTextHAlign.Center
                                    End Select

                                Catch ex As Exception
                                    PaginationAlign = PdfTextHAlign.Center
                                End Try

                            Catch
                                FooterPaginate = True
                                HasFooter = True
                                FooterHeight += 15
                                PaginationAlign = PdfTextHAlign.Center
                            End Try


                        Case "d"
                            Try
                                InputFile = args(i + 1)
                            Catch ex As Exception
                                Console.WriteLine(ex.Message)
                            End Try

                            If Not InputFile.Contains(".htm") Or InputFile.Contains(".html") Then
                                Console.WriteLine(String.Format("{0} is not a valid html file", InputFile))
                            End If

                        Case "z"
                            Try
                                Width = 2000 - 10 * (CInt(args(i + 1)))
                            Catch ex As Exception
                                Console.WriteLine(ex.Message)
                            End Try

                        Case "o"
                            PageOrientation = PdfPageOrientation.Landscape
                        Case "dbg"
                            Debug = True
                    End Select
                Case Else
            End Select
        Next




        PDFShared.PDFParams = New PdfParameters( _
                                                inputFile, _
                                                Author, _
                                                Subject, _
                                                isReadOnly, _
                                                KeepSource, _
                                                HasFooter, _
                                                FooterHeight, _
                                                FooterPaginate, _
                                                HasHeader, _
                                                HeaderHeight, _
                                                HeaderPaginate, _
                                                Margins, _
                                                Width, _
                                                PaginationAlign, _
                                                PageSize, _
                                                PageOrientation _
                                                )

        Dim Docs As New emgPDFDoc(PDFShared.PDFParams)


        outPath = inputFile.Replace(inputFile.Split(".").Last, "pdf")

        Try
            Docs.Convert(outPath)
        Catch ex As Exception
            Console.WriteLine(ex.Message)
        End Try

        If Debug = True Then
            Console.WriteLine("Debug mode is on - press any key to exit.")
            Console.ReadLine()
        End If
    End Sub


End Module

