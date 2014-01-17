Imports HtmlAgilityPack
Imports HiQPdf
Imports System.drawing

Public Class emgPDFDoc

    Private _html As HTMLSourceCollection
    ''' <summary>
    ''' Returns emgPDFDoc instance's list of HTMLSource objects
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private ReadOnly Property HTML() As HTMLSourceCollection
        Get
            Return _html
        End Get
    End Property

    ''' <summary>
    ''' Instantiates new emgPDFDoc object, taking parameters (including html) as input.
    ''' Object's primary purpose is contain HiQPDF document objects for merge.
    ''' </summary>
    ''' <param name="params"></param>
    ''' <remarks></remarks>
    Public Sub New(ByVal params As PdfParameters)
        If IsNothing(params.InputFile) Then
            Throw New Exception("Path to file is a required parameter")
            Exit Sub
        End If
        _html = CreateHTMLDocuments(params.InputFile)

    End Sub

    ''' <summary>
    ''' Takes string as input and returns collection of HTMLSource objects
    ''' </summary>
    ''' <param name="path"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CreateHTMLDocuments(ByVal path As String) As HTMLSourceCollection
        Dim inputDoc As New HtmlDocument()
        inputDoc.Load(path)

        Return New HTMLSourceCollection(inputDoc)
        'todo debug html and the weird lack thereof.
    End Function

    Public Sub Convert(ByVal outPath As String)

        If IsNothing(HTML) Then
            Throw New Exception("Conversion cannot take place with empty source.")
            Exit Sub
        End If

        Dim outputDocument As New PdfDocument

        outputDocument.SerialNumber = PDFShared.Serial
        With PDFShared.PDFParams
            For Each document As HTMLSource In HTML

                Dim pdfConvert As New HtmlToPdf
                pdfConvert.Document.FitPageWidth = True
                AddHandler pdfConvert.PageCreatingEvent, AddressOf pagecreate

                Dim PDFDoc As New PdfDocument
                Dim headerHTML As PdfHtml
                Dim footerHTML As PdfHtml


                Dim headerPosition As Integer = 0

                'push rest of header below pagination 
                If .HeaderPaginate Then
                    headerPosition += 15
                End If


                If .HasHeader Then
                    headerHTML = New PdfHtml(0, headerPosition, document.Header, Nothing)
                End If

                'pagination does not affect footers
                If .HasFooter Then
                    footerHTML = New PdfHtml(0, 0, document.Footer, Nothing)
                End If

                If Not IsNothing(headerHTML) Then
                    setheader(pdfConvert.Document, headerHTML, document.HeaderHeight, .HeaderPaginate, .PaginationAlign)
                End If

                If Not IsNothing(footerHTML) Then
                    setfooter(pdfConvert.Document, footerHTML, document.FooterHeight, .FooterPaginate, .PaginationAlign)
                End If

                pdfConvert.Document.Margins = New PdfMargins(.Margins)
                pdfConvert.Document.ForceFitPageWidth = True

                Try
                    PDFDoc = pdfConvert.ConvertHtmlToPdfDocument(document.Body, Nothing)
                Catch ex As Exception
                    'todo?
                End Try
                outputDocument.AddDocument(PDFDoc)
                RemoveHandler pdfConvert.PageCreatingEvent, AddressOf pagecreate
            Next

            outputDocument.WriteToFile(outPath)

            If .IsReadOnly Then
                Dim rOnly As System.IO.FileAttributes = FileAttribute.ReadOnly
                System.IO.File.SetAttributes(outPath, rOnly)
            End If



        End With

    End Sub

    Private Sub setheader(ByVal doc As PdfDocumentControl, ByVal header As PdfHtml, ByVal headerHeight As Integer, ByVal hasPagination As Boolean, _
                           ByVal bWidth As Integer, Optional ByVal align As PdfTextHAlign = PdfTextHAlign.Center)
        With doc.Header
            .Enabled = True
            .Height = headerHeight
            doc.ForceFitPageWidth = True
            header.ForceFitDestWidth = True
            header.FitDestWidth = True
            header.BrowserWidth = 1000
            Dim width As Single = doc.PageSize.Width

            If doc.PageOrientation = PdfPageOrientation.Landscape Then
                width = doc.PageSize.Height
            End If

            If hasPagination Then
                Dim Font As New System.Drawing.Font(New System.Drawing.FontFamily("Arial"), 8, GraphicsUnit.Point)
                Dim pageNumberingText As New PdfText(5, 5, "Page {CrtPage} of {PageCount}     ", Font)
                pageNumberingText.HorizontalAlign = align
                pageNumberingText.EmbedSystemFont = True
                .Layout(pageNumberingText)
            End If
            .Layout(header)

        End With
    End Sub

    Private Sub setfooter(ByVal doc As PdfDocumentControl, ByVal footer As PdfHtml, ByVal footerHeight As Integer, ByVal hasPagination As Boolean, Optional ByVal align As PdfTextHAlign = PdfTextHAlign.Center)
        With doc.Footer
            .Enabled = True
            footer.ForceFitDestWidth = True
            footer.BrowserWidth = 1000
            .Height = footerHeight
            doc.ForceFitPageWidth = True
            Dim width As Single = doc.PageSize.Width

            'sets width to height for landscape documents
            If doc.PageOrientation = PdfPageOrientation.Landscape Then
                width = doc.PageSize.Height
            End If

            If hasPagination Then
                Dim Font As New System.Drawing.Font(New System.Drawing.FontFamily("Arial"), 8, GraphicsUnit.Point)
                Dim pageNumberingText As New PdfText(5, footerHeight - 12, "Page {CrtPage} of {PageCount}", Font)
                pageNumberingText.HorizontalAlign = align
                pageNumberingText.EmbedSystemFont = True
                .Layout(pageNumberingText)
            End If
            .Layout(footer)
        End With

    End Sub

    Private Sub pagecreate(ByVal eventparams As PdfPageCreatingParams)
        Dim pdfpage As PdfPage = eventparams.PdfPage
        pdfpage.DisplayFooter = True
        pdfpage.DisplayHeader = True
        If eventparams.PdfPageNumber > 1000 Then
            eventparams.CancelConversion = True
        End If
    End Sub


End Class
