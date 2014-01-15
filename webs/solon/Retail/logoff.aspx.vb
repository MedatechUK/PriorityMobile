Imports cmSi

Partial Class logoff
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If User.Identity.IsAuthenticated Then            
            cmsSessions.CloseSession(Session.SessionID)
            FormsAuthentication.SignOut()
        End If
        Response.Redirect("login.aspx")
    End Sub
End Class
