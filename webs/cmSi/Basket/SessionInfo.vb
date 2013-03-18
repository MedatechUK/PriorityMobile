Imports Microsoft.VisualBasic
Imports System.Collections.Generic
Imports System.Xml
Imports System.IO
Imports System.Web
Imports System.Web.UI.WebControls

Public Class SessionInfo

#Region "Initialisation"

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal CurrentContext As HttpContext)
        MyBase.new()
        With CurrentContext
            _SessionStart = CStr(Now)
            _UserAgent = .Request.UserAgent
            _UserHostAddress = .Request.UserHostAddress
            _ViewingPage = .Request.RawUrl
        End With
    End Sub

#End Region

#Region "Public Properties"

    Private _SessionStart As String
    Public Property SessionStart() As String
        Get
            Return _SessionStart
        End Get
        Set(ByVal value As String)
            _SessionStart = value
        End Set
    End Property

    Private _UserAgent As String
    Public Property UserAgent() As String
        Get
            Return _UserAgent
        End Get
        Set(ByVal value As String)
            _UserAgent = value
        End Set
    End Property

    Private _UserHostAddress As String
    Public Property UserHostAddress() As String
        Get
            Return _UserHostAddress
        End Get
        Set(ByVal value As String)
            _UserHostAddress = value
        End Set
    End Property

    Private _ViewingPage As String
    Public Property ViewingPage() As String
        Get
            Return _ViewingPage
        End Get
        Set(ByVal value As String)
            _ViewingPage = value
        End Set
    End Property

#End Region

End Class