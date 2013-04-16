Imports System.Web

Public MustInherit Class cmsInherit
    Inherits System.Web.UI.Page

    Public P As cmsPage
    Private Repl As cmsReplace
    Private ReplaceModules As repl_Base()

#Region "Initialisation"

    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        Try
            P = New cmsPage(Page, Context, Server, AdminContext)
            PagePreInit(sender, e)
        Catch
        End Try
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            P.PageLoad(Me, AdminContext)
            LoadReplaceModules()
            Repl = New cmsReplace(ReplaceModules)
            Repl.LoadControls(P, Me, Context, Server)
            PageLoaded(sender, e)
        Catch ex As System.Threading.ThreadAbortException

        Catch ex As Exception

            If Not (String.Compare(HttpContext.Current.Request.Url.AbsoluteUri.Split("/").Last, "500.aspx", True) = 0) Then
                Dim redir As New cmSi.cmsHTTPPost("500.aspx")
                redir.NameValues.Add("Page", HttpContext.Current.Request.Url.AbsoluteUri)
                redir.NameValues.Add("Error", ex.Message)
                redir.Post()
            Else
                Response.Write(ex.Message)
            End If            
        End Try
    End Sub

#End Region

#Region "Public Properties"

    Public ReadOnly Property ts() As Session
        Get
            Return cmsSessions.CurrentSession(HttpContext.Current)
        End Get
    End Property

#End Region

#Region "Public Methods"

    Public Sub addReplaceModule(ByVal ParamArray repl() As repl_Base)
        For Each r As repl_Base In repl
            Try
                ReDim Preserve ReplaceModules(UBound(ReplaceModules) + 1)
            Catch
                ReDim ReplaceModules(0)
            Finally
                ReplaceModules(UBound(ReplaceModules)) = r
            End Try
        Next
    End Sub

#End Region

#Region "Overridable Methods"

    Public Overridable Sub LoadReplaceModules()

    End Sub

    Public Overridable Sub PagePreInit(ByVal sender As Object, ByVal e As System.EventArgs)

    End Sub

    Public Overridable Sub PageLoaded(ByVal sender As Object, ByVal e As System.EventArgs)

    End Sub

    Public Overridable Function AdminContext() As Boolean
        Return False
    End Function

#End Region

End Class
