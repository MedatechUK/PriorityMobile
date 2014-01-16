Imports HiQPdf

Public Enum Size As Integer
    A0 = 0
    A1 = 1
    A2 = 2
    A3 = 3
    A4 = 4
    A5 = 5
    A6 = 6
    A7 = 7
    A8 = 8
    A9 = 9
    A10 = 10
    ArchA = 11
    ArchB = 12
    ArchC = 13
    ArchD = 14
    ArchE = 15
    B0 = 16
    B1 = 17
    B2 = 18
    B3 = 19
    B4 = 20
    B5 = 21
    Flsa = 22
    HalfLetter = 23
    Ledger = 24
    Legal = 25
    Letter = 26
    Letter11x17 = 27
    Note = 28
End Enum

Friend Class PdfParameters

#Region "Properties"
    Private _author As String
    ''' <summary>
    ''' Gets the Author parameter for the PDF document.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property Author() As String
        Get
            Return _author
        End Get
    End Property

    Private _subject As String
    ''' <summary>
    ''' Gets the Subject parameter for the PDF document.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property Subject() As String
        Get
            Return _subject
        End Get
    End Property

    Private _inputfile As String
    ''' <summary>
    ''' Gets the file path for the input HTML file.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property InputFile() As String
        Get
            Return _inputfile
        End Get
    End Property

    Private _isreadonly As Boolean
    ''' <summary>
    ''' Gets the IsReadOnly property for the PDF document.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property IsReadOnly() As Boolean
        Get
            Return _isreadonly
        End Get
    End Property

    Private _keepsource As Boolean
    ''' <summary>
    ''' Gets the KeepSource property for the PDF document. 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property KeepSource() As Boolean
        Get
            Return _keepsource
        End Get
    End Property

    Private _footerpaginate As Boolean
    ''' <summary>
    ''' Gets the FooterPaginate property for the PDF document.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property FooterPaginate() As Boolean
        Get
            Return _footerpaginate
        End Get
    End Property

    Private _headerpaginate As Boolean
    ''' <summary>
    ''' Gets the HeaderPaginate property for the PDF document.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property HeaderPaginate() As Boolean
        Get
            Return _headerpaginate
        End Get
    End Property

    Private _hasheader As Boolean
    ''' <summary>
    ''' Gets the HasHeader property for the PDF document.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property HasHeader() As Boolean
        Get
            Return _hasheader
        End Get
        Set(ByVal value As Boolean)
            _hasheader = value
        End Set
    End Property

    Private _hasfooter As Boolean
    ''' <summary>
    ''' Gets the HasFooter property for the PDF document.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property HasFooter() As Boolean
        Get
            Return _hasfooter
        End Get
        Set(ByVal value As Boolean)
            _hasfooter = value
        End Set
    End Property

    Private _headerheight As Integer
    ''' <summary>
    ''' Gets or sets the headerheight property for the PDF document.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property HeaderHeight() As Integer
        Get
            Return _headerheight
        End Get
        Set(ByVal value As Integer)
            _headerheight = value
        End Set
    End Property

    Private _footerheight As Integer
    ''' <summary>
    ''' Gets or sets the FooterHeight property for the PDF document. 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property FooterHeight() As Integer
        Get
            Return _footerheight
        End Get
        Set(ByVal value As Integer)
            _footerheight = value
        End Set
    End Property

    Private _width As Integer
    ''' <summary>
    ''' Gets the Width property for the PDF document.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property Width() As Integer
        Get
            Return _width
        End Get
    End Property

    Private _margins As Integer
    ''' <summary>
    ''' Gets the Margins property for the PDF document.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property Margins() As Integer
        Get
            Return _margins
        End Get
    End Property

    Private _paginationalign As PdfTextHAlign
    ''' <summary>
    ''' Gets the PaginationAlign property for the PDF document. 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property PaginationAlign() As PdfTextHAlign
        Get
            Return _paginationalign
        End Get
    End Property

    Private _pagesize As PdfPageSize
    ''' <summary>
    ''' Gets the PageSize property for the PDF document.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property PageSize() As PdfPageSize
        Get
            Return _pagesize
        End Get
    End Property

    Private _orientation As PdfPageOrientation
    ''' <summary>
    ''' Gets the Orientation property for the PDF document.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property Orientation() As PdfPageOrientation
        Get
            Return _orientation
        End Get
    End Property

#End Region

#Region "Handling HiQPDF page sizing"
    'HiQPDF's page sizing is handled through property access methods. As such, you can't set an optional param's default. 
    'For the sake of nice intellisense, I have included the enum @ top of file
    'For the sake of not cluttering up the ctor, I have included the SetSize function. 

    Protected Friend Function SetSize(ByVal inSize As Size) As PdfPageSize
        Dim ret As PdfPageSize = PdfPageSize.A4
        Select Case inSize
            Case Size.A1
                ret = PdfPageSize.A1
            Case Size.A2
                ret = PdfPageSize.A2
            Case Size.A3
                ret = PdfPageSize.A3
            Case Size.A4
                ret = PdfPageSize.A4
            Case Size.A5
                ret = PdfPageSize.A5
            Case Size.A6
                ret = PdfPageSize.A6
            Case Size.A7
                ret = PdfPageSize.A7
            Case Size.A8
                ret = PdfPageSize.A8
            Case Size.A9
                ret = PdfPageSize.A9
            Case Size.A10
                ret = PdfPageSize.A10
            Case Size.ArchA
                ret = PdfPageSize.ArchA
            Case Size.ArchB
                ret = PdfPageSize.ArchB
            Case Size.ArchC
                ret = PdfPageSize.ArchC
            Case Size.ArchD
                ret = PdfPageSize.ArchD
            Case Size.ArchE
                ret = PdfPageSize.ArchE
            Case Size.B0
                ret = PdfPageSize.B0
            Case Size.B1
                ret = PdfPageSize.B1
            Case Size.B2
                ret = PdfPageSize.B2
            Case Size.B3
                ret = PdfPageSize.B3
            Case Size.B4
                ret = PdfPageSize.B4
            Case Size.B5
                ret = PdfPageSize.B5
            Case Size.Flsa
                ret = PdfPageSize.Flsa
            Case Size.HalfLetter
                ret = PdfPageSize.HalfLetter
            Case Size.Ledger
                ret = PdfPageSize.Ledger
            Case Size.Legal
                ret = PdfPageSize.Legal
            Case Size.Letter
                ret = PdfPageSize.Letter
            Case Size.Letter11x17
                ret = PdfPageSize.Letter11x17
            Case Size.Note
                ret = PdfPageSize.Note
        End Select

        Return ret
    End Function

#End Region

    Protected Friend Sub New(ByVal InputFile As String, _
          Optional ByVal Author As String = "", _
          Optional ByVal Subject As String = "", _
          Optional ByVal isReadOnly As Boolean = True, _
          Optional ByVal KeepSource As Boolean = False, _
 _
          Optional ByVal HasFooter As Boolean = False, _
          Optional ByVal FooterHeight As Integer = 0, _
          Optional ByVal FooterPaginate As Boolean = False, _
 _
          Optional ByVal HasHeader As Boolean = False, _
          Optional ByVal HeaderHeight As Integer = 0, _
          Optional ByVal HeaderPaginate As Boolean = False, _
 _
          Optional ByVal Margins As Integer = 0, _
          Optional ByVal Width As Integer = 1000, _
 _
          Optional ByVal PaginationAlign As PdfTextHAlign = PdfTextHAlign.Center, _
          Optional ByVal PSize As Size = Size.A4, _
          Optional ByVal PageOrientation As PdfPageOrientation = PdfPageOrientation.Portrait _
          )

        _author = Author
        _subject = Subject
        _isreadonly = isReadOnly
        _keepsource = KeepSource

        _hasfooter = HasFooter
        _footerheight += FooterHeight

        _hasheader = HasHeader
        _headerheight += HeaderHeight

        'pagination overrides any flagged settings, you have been warned.
        If FooterPaginate Then
            _footerpaginate = True
            _headerpaginate = False
            _hasfooter = True
            _footerheight += 15
        End If

        If HeaderPaginate Then
            _headerpaginate = True
            _footerpaginate = False
            _hasheader = True
            _headerheight += 15
        End If

        _paginationalign = PaginationAlign
        _width = 2000 - 10 * Width 'express as percentage
        _orientation = PageOrientation
        _pagesize = SetSize(PSize) 'Psize named for clarity wrt PageSize property
        _margins = Margins

        _inputfile = InputFile
    End Sub

End Class