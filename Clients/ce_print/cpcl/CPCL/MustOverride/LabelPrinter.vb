Imports System.IO

Public Delegate Sub PrinterConnectionHandler()

Public MustInherit Class LabelPrinter

#Region "Public Properties"

    Private _ImageFolder As String = ""
    Public Property ImageFolder() As String
        Get
            Return _ImageFolder
        End Get
        Set(ByVal value As String)
            _ImageFolder = value
        End Set
    End Property

    Private _macAddress As String
    Public Property macAddress() As [String]
        Get
            Return _macAddress
        End Get
        Set(ByVal value As [String])
            _macAddress = value
        End Set
    End Property

    Private _Connected As Boolean = False
    Public ReadOnly Property Connected() As Boolean
        Get
            Return _Connected
        End Get
    End Property

    Private _RefreshImages As Boolean
    Public Property RefreshImages() As Boolean
        Get
            Return _RefreshImages
        End Get
        Set(ByVal value As Boolean)
            _RefreshImages = value
        End Set
    End Property

    Private _PIN As String = ""
    Public Property PIN() As String
        Get
            Return _PIN
        End Get
        Set(ByVal value As String)
            _PIN = value
        End Set
    End Property

    Private _dpi As Point
    Public Property dpi() As Point
        Get
            Return _dpi
        End Get
        Set(ByVal value As Point)
            _dpi = value
        End Set
    End Property

    Private _Dimensions As Size
    Public Property Dimensions() As Size
        Get
            Return _Dimensions
        End Get
        Set(ByVal value As Size)
            _Dimensions = value
        End Set
    End Property

#End Region

#Region "Base Events and Method Stubs"

    Public Event connectionEstablished As PrinterConnectionHandler
    Public Event connectionClosed As PrinterConnectionHandler

    Public MustOverride Function fileNames() As [String]()
    Public MustOverride Sub BeginConnect(ByVal macAddress As [String], Optional ByVal PIN As String = Nothing, Optional ByVal RefreshImages As Boolean = False)
    Public MustOverride Sub Print(ByVal Bytes As Byte())
    Public MustOverride Sub StoreImage(ByVal Filename As String, ByVal Image As Bitmap)

#End Region

#Region "Public Methods"

    Public Sub RaiseConnect()
        _Connected = True
        RaiseEvent connectionEstablished()
    End Sub

    Public Sub RaiseDisconnect()
        _Connected = False
        RaiseEvent connectionClosed()
    End Sub

    Public Sub LoadImages(Optional ByVal Refresh As Boolean = False)

        If Me.ImageFolder.Length = 0 Then Exit Sub
        Dim fls() As [String] = Me.fileNames
        For Each LocalFile As FileInfo In New DirectoryInfo(Me.ImageFolder).GetFiles("*.bmp")
            If Refresh Or Not fls.Contains(LocalFile.FullName.ToUpper.Split("\").Last.Replace(".BMP", ".PCX")) Then
                Dim bmp As New Bitmap(LocalFile.FullName)
                StoreImage(LocalFile.FullName.ToLower.Split("\").Last, bmp)
            End If
        Next
    End Sub

#End Region

End Class
