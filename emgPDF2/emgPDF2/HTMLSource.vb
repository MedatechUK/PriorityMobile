Imports HtmlAgilityPack
Imports HiQPdf

Friend Class HTMLSourceCollection : Inherits System.Collections.Generic.List(Of HTMLSource)

    Private _css As String
    ''' <summary>
    ''' Property for containing the origin document's CSS.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Property CSS() As String
        Get
            Return _css
        End Get
        Set(ByVal value As String)
            _css = value
        End Set
    End Property

    ''' <summary>
    ''' Constructs the collection of HTMLSource objects, filling the CSS property from the origin document.
    ''' </summary>
    ''' <param name="inputFile"></param>
    ''' <remarks></remarks>
    Protected Friend Sub New(ByVal inputFile As HtmlDocument)
        Dim cssNodes As HtmlNodeCollection = inputFile.DocumentNode.SelectNodes("//style")

        If Not IsNothing(cssNodes) Then
            For Each styleNode As HtmlNode In cssNodes
                CSS += styleNode.OuterHtml
            Next
        End If
        Dim priForms As HtmlNodeCollection = inputFile.DocumentNode.SelectNodes("//form")

        If Not IsNothing(priForms) Then
            For Each document As HtmlNode In priForms
                Me.Add(New HTMLSource(document, CSS))
            Next
        Else
            Me.Add(New HTMLSource(inputFile.DocumentNode, CSS))
        End If

    End Sub


End Class


Friend Class HTMLSource

    Private _header As String
    ''' <summary>
    ''' Returns header html, if any.
    '''</summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Friend ReadOnly Property Header() As String
        Get
            Return _header
        End Get
    End Property

    Private _headerheight As Integer
    ''' <summary>
    ''' Returns Header's height as integer. 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Friend ReadOnly Property HeaderHeight() As Integer
        Get
            Return _headerheight
        End Get
    End Property

    Private _footerheight As Integer
    ''' <summary>
    ''' Returns Footer's height as integer. 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Friend ReadOnly Property FooterHeight() As Integer
        Get
            Return _footerheight
        End Get
    End Property

    Private _footer As String
    ''' <summary>
    ''' Returns footer html, if any.
    '''</summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Friend ReadOnly Property Footer() As String
        Get
            Return _footer
        End Get
    End Property

    Private _body As String
    ''' <summary>
    ''' Returns body html
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Friend ReadOnly Property Body() As String
        Get
            Return _body
        End Get
    End Property

    ''' <summary>
    ''' Instantiates new HTMLSource from string input
    ''' </summary>
    ''' <param name="html"></param>
    ''' <remarks></remarks>
    Public Sub New(ByVal html As HtmlNode, ByVal css As String)

        'hiqpdf automatically page breaks for distinct documents, apparently. Priority sticks this in so page breaking occurs correctly 
        'when html documents are printed. 
        'ergo, the purpose of this is to prevent the library from adding in blank pages by enacting this style rule, effectively, twice.
        Try
            html.SetAttributeValue("style", html.Attributes("style").Value.Replace("page-break-after: always;", ""))
        Catch : End Try

        Dim footerTables As HtmlNodeCollection = html.SelectNodes(".//tr[contains(@class, ""footer"")] | .//tr[contains(@class, ""FOOTER"")]")
        Dim headerTables As HtmlNodeCollection = html.SelectNodes(".//tr[contains(@class, ""header"")] | .//tr[contains(@class, ""HEADER"")]")
        With PDFShared.PDFParams
            ' headers
            If IsNothing(headerTables) Then
                .HasHeader = False
            End If

            If .HasHeader Then
                _header += css
                _header += "<table width=""100%"" class=""ZEMG_HEADER"">"

                For Each table As HtmlNode In headerTables
                    _header += table.OuterHtml
                Next
                _header += "</table>"
                For Each table As HtmlNode In headerTables
                    table.RemoveAll()
                Next
            End If


            'footers
            If IsNothing(footerTables) Then
                .HasFooter = False
            End If

            If .HasFooter Then
                _footer += css
                _footer += "<table width=""100%"" class=""ZEMG_FOOTER"">"
                For Each table As HtmlNode In footerTables
                    _footer += table.OuterHtml
                Next
                _footer += "</table>"
                For Each table As HtmlNode In footerTables
                    table.RemoveAll()
                Next
            End If



            'fixes alignment

            html.SelectSingleNode("//html/body").SetAttributeValue("leftmargin", "7px")
            html.SelectSingleNode("//html/body").SetAttributeValue("rightmargin", "7px")



            ''obtains header & footer height
            With PDFShared.PDFParams
                If .HasHeader And .HeaderHeight < 20 Then
                    Dim tempconvert As New HtmlToPdf
                    tempconvert.BrowserWidth = 1000
                    tempconvert.MinBrowserHeight = 0
                    tempconvert.Document.ForceFitPageWidth = True
                    tempconvert.Document.PageSize = .PageSize
                    tempconvert.Document.PageOrientation = .Orientation
                    tempconvert.Document.FitPageWidth = True
                    tempconvert.Document.PostCardMode = True
                    tempconvert.ConvertHtmlToMemory(_header, Nothing)
                    _headerheight += tempconvert.ConversionInfo.PdfRegions(0).Height
                End If

                If .HasFooter And .FooterHeight < 31 Then
                    Dim tempconvertfoot As New HtmlToPdf
                    tempconvertfoot.MinBrowserHeight = 0
                    tempconvertfoot.BrowserWidth = 1000
                    tempconvertfoot.Document.ForceFitPageWidth = True
                    tempconvertfoot.Document.PageSize = .PageSize
                    tempconvertfoot.Document.PageOrientation = .Orientation
                    tempconvertfoot.Document.FitPageWidth = True
                    tempconvertfoot.Document.PostCardMode = True
                    tempconvertfoot.ConvertHtmlToMemory(_footer, Nothing)
                    _footerheight += tempconvertfoot.ConversionInfo.PdfRegions(0).Height
                End If

                _headerheight += .HeaderHeight
                _footerheight += .FooterHeight

                _body += css & html.OuterHtml
            End With
        End With
    End Sub

End Class
