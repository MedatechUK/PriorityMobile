Imports System.Xml
Imports System.IO
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.HtmlControls

Public Class repl_Argument
    Inherits System.EventArgs

#Region "Public Properties"

    Private _thisCMSPage As cmsPage
    Public ReadOnly Property thisCMSPage() As cmsPage
        Get
            Return _thisCMSPage
        End Get
    End Property

    Private _thisPage As Page
    Public ReadOnly Property thisPage() As Page
        Get
            Return _thisPage
        End Get
    End Property

    Private _thisContext As HttpContext
    Public ReadOnly Property thisContext() As HttpContext
        Get
            Return _thisContext
        End Get
    End Property

    Private _thisServer As HttpServerUtility
    Public ReadOnly Property thisServer() As HttpServerUtility
        Get
            Return _thisServer
        End Get
    End Property

#End Region

#Region "Initialisation and finalisation"

    Public Sub New(ByRef thisCMSPage As cmsPage, ByRef thisPage As Page, ByRef thisContext As HttpContext, ByRef thisServer As HttpServerUtility)
        _thisCMSPage = thisCMSPage
        _thisPage = thisPage
        _thisServer = thisServer
        _thisContext = thisContext
    End Sub

#End Region

End Class

Public MustInherit Class repl_Base

    Public MustOverride ReadOnly Property Controls() As List(Of ReplaceControl)
    Public MustOverride ReadOnly Property ReplaceModule() As String

    Public ReadOnly Property ts() As Session
        Get
            Return cmsSessions.CurrentSession(HttpContext.Current)
        End Get
    End Property

    Public ReadOnly Property IsAuthenticated() As Boolean
        Get
            Return HttpContext.Current.User.Identity.IsAuthenticated
        End Get
    End Property
End Class

Public Class ReplaceControl

#Region "public properties"

    Private _ControlID As String
    Public Property ControlID() As String
        Get
            Return _ControlID.ToLower
        End Get
        Set(ByVal value As String)
            _ControlID = value.ToLower
        End Set
    End Property

    Private _ControlType As String
    Public Property ControlType() As String
        Get
            Return _ControlType.ToLower
        End Get
        Set(ByVal value As String)
            _ControlType = value.ToLower
        End Set
    End Property

    Private _ControlHandler As System.EventHandler(Of repl_Argument)
    Public Property ControlHandler() As System.EventHandler(Of repl_Argument)
        Get
            Return _ControlHandler
        End Get
        Set(ByVal value As System.EventHandler(Of repl_Argument))
            _ControlHandler = value
        End Set
    End Property

#End Region

#Region "Initialisation and finalisation"

    Public Sub New(ByVal ControlType As String, ByVal ControlID As String, ByRef ControlHandler As System.EventHandler(Of repl_Argument))
        _ControlType = ControlType
        _ControlID = ControlID
        _ControlHandler = ControlHandler
    End Sub

#End Region

End Class

