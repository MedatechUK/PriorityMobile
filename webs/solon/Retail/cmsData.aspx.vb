Imports cmSi

Partial Class cmsData
    Inherits System.Web.UI.Page

    Private data As New cmSi.cmsData()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        data.Load(Server, ConfigurationManager.AppSettings)
        Response.Redirect("~/")

    End Sub

End Class
