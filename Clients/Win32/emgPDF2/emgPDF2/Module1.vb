Imports HiQPdf
Imports HtmlAgilityPack

Module Module1

    Sub main()
        HtmlNode.ElementsFlags.Remove("form")
        PDFShared.PDFParams = New PdfParameters(System.Environment.GetCommandLineArgs())
        Dim Docs As New emgPDFDoc(PDFShared.PDFParams)

    End Sub

    Public Sub ParseArgs()

    End Sub

#Region "parse html into separate documents:: input single html file"

#End Region

#Region "handle headers, footers per document html:: input html as strings"

#End Region

#Region "handle multiple document merge- pagination?:: input collection of htmls?"

#End Region


End Module

