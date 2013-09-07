Imports System.Xml
Imports System.IO

Public Class UserEnv

#Region "Initialisation and finalisation"

    Public Sub New(ByVal User As String, ByVal Server As Uri)
        _User = User
        _Server = Server
    End Sub

#End Region

#Region "Public Properties"

    Private _User As String
    Public Property User() As String
        Get
            Return _User
        End Get
        Set(ByVal value As String)
            _User = value
        End Set
    End Property

    Private _Server As Uri
    Public Property Server() As Uri
        Get
            Return _Server
        End Get
        Set(ByVal value As Uri)
            _Server = value
        End Set
    End Property

#End Region

#Region "Public Methods"

    Public Function LocalFolder() As String

        Dim dir As New DirectoryInfo( _
            String.Format( _
                "{0}\PriorityMobile\{1}\{2}", _
                 Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), _
                 _Server.Host, _
                _User _
                ) _
            )
        With dir
            Debug.Print("Local Environment = " & dir.FullName)
            If Not .Exists Then .Create()
            Return dir.FullName
        End With

    End Function

#End Region

End Class