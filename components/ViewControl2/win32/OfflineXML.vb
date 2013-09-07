Imports System.Xml
Imports System.IO

Public Class OfflineXML

#Region "Initialisation and Finalisation"

    Public Sub New(ByRef UE As UserEnv, ByVal Filename As String, ByVal FileURL As String)
        _LocalFile = String.Format("{0}\{1}", UE.LocalFolder, Filename)
        _FileURL = String.Format("{0}{1}", UE.Server.AbsoluteUri.ToString, FileURL)
        If Not File.Exists(_LocalFile) Then
            Try
                File.Copy(Filename, _LocalFile)
            Catch EX As Exception
            End Try
        End If
        _Document.Load(_LocalFile)
    End Sub

#End Region

#Region "Public Properties"

    Private _LocalFile As String
    Public Property LocalFile() As String
        Get
            Return _LocalFile
        End Get
        Set(ByVal value As String)
            _LocalFile = value
        End Set
    End Property
    Private _FileURL As String
    Public Property FileURL() As String
        Get
            Return _FileURL
        End Get
        Set(ByVal value As String)
            _FileURL = value
        End Set
    End Property
    Private _Document As New XmlDocument
    Public Property Document() As XmlDocument
        Get
            Return _Document
        End Get
        Set(ByVal value As XmlDocument)
            _Document = value
        End Set
    End Property

#End Region

#Region "Public Methods"

    Public Sub Sync()
        '#TODO
    End Sub

    Public Sub DeleteLocalCache()
        If File.Exists(LocalFile) Then
            Try
                File.Delete(LocalFile)                
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        End If
    End Sub

#End Region

End Class