Imports System.Net
Imports System.IO
Imports System.Web
Imports System.drawing
Imports System.Drawing.Imaging

Public Class cmsPartImg : Implements IDisposable

#Region "Initialisation and Finalisation"

    Public Sub New()
        With HttpContext.Current
            _img = .Request("image")
        End With
    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        _img = Nothing
    End Sub

#End Region

#Region "Private properties"

    Private _img As String = String.Empty
    Private Property Image() As String
        Get
            Return _img
        End Get
        Set(ByVal value As String)
            _img = value
        End Set
    End Property

    Private Property imgLocalPath() As String
        Get
            With HttpContext.Current.Server
                Return CombinePath( _
                        .MapPath("/"), _
                        "my_documents\priimg", _
                        .UrlEncode(_img.Split("\").Last) _
                )
            End With
        End Get
        Set(ByVal value As String)
            _img = value
        End Set
    End Property

    Private ReadOnly Property imgRemotePath() As String
        Get
            Return String.Format( _
                "{0}?image={1}", _
                cmsData.Settings("ImageURL"), _
                _img _
            )
        End Get
    End Property

    Private ReadOnly Property ImgExists() As Boolean
        Get
            Return File.Exists(imgLocalPath)
        End Get
    End Property

#End Region

#Region "Private Methods"

    Private Function CombinePath(ByVal ParamArray Path() As String) As String
        Dim ret As String = String.Empty
        For Each p As String In Path
            ret = System.IO.Path.Combine(ret, p)
        Next
        Return ret
    End Function

    Private Sub DownloadImage()
        Try

            Dim wr As HttpWebRequest = CType(Net.HttpWebRequest.Create(imgRemotePath), HttpWebRequest)
            Dim ws As HttpWebResponse = CType(wr.GetResponse(), HttpWebResponse)
            Dim str As Stream = ws.GetResponseStream()
            Dim inBuf(100000000) As Byte
            Dim bytesToRead As Integer = CInt(inBuf.Length)
            Dim bytesRead As Integer = 0
            While bytesToRead > 0
                Dim n As Integer = str.Read(inBuf, bytesRead, bytesToRead)
                If n = 0 Then
                    Exit While
                End If
                bytesRead += n
                bytesToRead -= n
            End While
            Dim fstr As New FileStream(imgLocalPath, FileMode.OpenOrCreate, FileAccess.Write)
            fstr.Write(inBuf, 0, bytesRead)
            str.Close()
            fstr.Close()

        Catch

        End Try
    End Sub

#End Region

#Region "Public Properties"

    Public ReadOnly Property BitmapImage() As Bitmap
        Get
            If File.Exists(imgLocalPath) Then
                Return New System.Drawing.Bitmap(imgLocalPath)
            Else
                DownloadImage()
                If File.Exists(imgLocalPath) Then
                    Return New System.Drawing.Bitmap(imgLocalPath)
                Else
                    Return New System.Drawing.Bitmap(1, 1, PixelFormat.Format24bppRgb)
                End If
            End If
        End Get
    End Property

    Public ReadOnly Property SaveFormat() As Imaging.ImageFormat
        Get
            Return ImageFormat.Jpeg
        End Get
    End Property

#End Region

End Class
