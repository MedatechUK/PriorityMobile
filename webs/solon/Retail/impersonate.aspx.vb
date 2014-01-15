Imports System.Xml

Partial Class members
    Inherits System.Web.UI.Page

    Protected Sub Impersonate_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        FormsAuthentication.SetAuthCookie(Request("user"), True)
        Response.Redirect("/")
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsNothing(Request("user")) Then
            FormsAuthentication.SetAuthCookie(Request("user"), True)
            Response.Redirect("/")
        End If
    End Sub

End Class
