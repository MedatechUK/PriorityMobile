Imports cmSi
Partial Class admin_admin
    Inherits System.Web.UI.MasterPage

    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.User.IsInRole("webmaster") Then
            Response.Redirect(cmsData.Settings.Get("url") & "/login.aspx")
        End If

        Dim ph As ContentPlaceHolder = Master.FindControl("Main")
        Dim img As System.Web.UI.WebControls.Image = ph.FindControl("WebName")
        If Not IsNothing(img) Then
            With img
                .AlternateText = cmsData.Settings("WebName")
                .ImageUrl = "my_documents/images/main-logo.png"
            End With
        End If
    End Sub

End Class

