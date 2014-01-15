Imports HiQPdf

Friend Class PdfParameters

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

#Region "Handling HiQPDF page sizing"
    'HiQPDF's page sizing is handled through property access methods. As such, you can't set an optional param's default. 
    'For the sake of nice intellisense, I have included the enum.
    'For the sake of not cluttering up the ctor, I have included the SetSize function. 
    Protected Friend Enum Size As Integer
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
    End Sub


    'TODO: HANDLE ARGS IN IMPLEMENTATION (COPY, PASTE, FIX, COMPILE LOL) 


    'Public Sub New(ByVal args As String())
    '    SetDefaultParams()
    '    For i As Integer = 1 To args.Length - 1
    '        Select Case Left(args(i), 1)
    '            Case "/", "-"
    '                Select Case Right(args(i), args(i).Length - 1).ToLower

    '                    Case "a"

    '                        Try
    '                            _author = args(i + 1)
    '                        Catch : End Try

    '                    Case "u"
    '                        Try
    '                            _subject = args(i + 1)
    '                        Catch : End Try

    '                    Case "k"
    '                        _keepsource = True

    '                    Case "w"
    '                        _isreadonly = False

    '                    Case "f"
    '                        _hasfooter = True
    '                        Try
    '                            'if present, take next value to be pixel height
    '                            If IsNumeric(args(i + 1)) Then
    '                                FooterHeight += CInt(args(i + 1))
    '                            End If
    '                        Catch : End Try


    '                    Case "h"
    '                        _hasheader = True
    '                        Try
    '                            'if present, take next value to be pixel height 
    '                            If IsNumeric(args(i + 1)) Then
    '                                HeaderHeight += CInt(args(i + 1))
    '                            End If
    '                        Catch : End Try


    '                    Case "m"
    '                        Try
    '                            If IsNumeric(args(i + 1)) Then
    '                                _margins = CInt(args(i + 1))
    '                            End If
    '                        Catch : End Try


    '                    Case "s"
    '                        Select Case args(i + 1).ToLower
    '                            Case "letter"
    '                                _pagesize = PdfPageSize.Letter
    '                                Dim s As Char = "s"
    '                                asci()
    '                            Case "legal"
    '                                _pagesize = PdfPageSize.Legal
    '                            Case "ledger"
    '                                _pagesize = PdfPageSize.Ledger
    '                            Case "b0"
    '                                _pagesize = PdfPageSize.B0
    '                            Case "b1"
    '                                _pagesize = PdfPageSize.B1
    '                            Case "b2"
    '                                _pagesize = PdfPageSize.B2
    '                            Case "b3"
    '                                _pagesize = PdfPageSize.B3
    '                            Case "b4"
    '                                _pagesize = PdfPageSize.B4
    '                            Case "b5"
    '                                _pagesize = PdfPageSize.B5
    '                            Case "a10"
    '                                _pagesize = PdfPageSize.A10
    '                            Case "a9"
    '                                _pagesize = PdfPageSize.A9
    '                            Case "a8"
    '                                _pagesize = PdfPageSize.A8
    '                            Case "a7"
    '                                _pagesize = PdfPageSize.A7
    '                            Case "a6"
    '                                _pagesize = PdfPageSize.A6
    '                            Case "a5"
    '                                _pagesize = PdfPageSize.A5
    '                            Case "a3"
    '                                _pagesize = PdfPageSize.A3
    '                            Case "a2"
    '                                _pagesize = PdfPageSize.A2
    '                            Case "a1"
    '                                _pagesize = PdfPageSize.A1
    '                            Case "a0"
    '                                _pagesize = PdfPageSize.A0
    '                            Case Else
    '                                _pagesize = PdfPageSize.A4
    '                        End Select


    '                    Case "p"
    '                        Try
    '                            Select Case args(i + 1).ToLower

    '                                Case "t"
    '                                    _footerpaginate = False
    '                                    _headerpaginate = True
    '                                    _hasheader = True
    '                                    _headerheight += 15

    '                                Case "b"
    '                                    _footerpaginate = True
    '                                    _headerpaginate = False
    '                                    _footerheight += 15
    '                                    _hasfooter = True

    '                            End Select
    '                            Try
    '                                Select Case args(i + 2).ToLower
    '                                    Case "l"
    '                                        _paginationalign = PdfTextHAlign.Left
    '                                    Case "r"
    '                                        _paginationalign = PdfTextHAlign.Right
    '                                    Case "c"
    '                                        _paginationalign = PdfTextHAlign.Center
    '                                End Select

    '                            Catch ex As Exception
    '                                _paginationalign = PdfTextHAlign.Center
    '                            End Try

    '                        Catch
    '                            _footerpaginate = True
    '                            _hasfooter = True
    '                            _footerheight += 15
    '                            _paginationalign = PdfTextHAlign.Center
    '                        End Try


    '                    Case "d"
    '                        _inputfile = args(i + 1)

    '                    Case "z"
    '                        Try
    '                            _width = 2000 - 10 * (CInt(args(i + 1)))
    '                        Catch ex As Exception
    '                            Console.WriteLine(ex.Message)
    '                        End Try

    '                    Case "o"
    '                        _orientation = PdfPageOrientation.Landscape
    '                End Select
    '            Case Else
    '        End Select
    '    Next

    'End Sub




End Class