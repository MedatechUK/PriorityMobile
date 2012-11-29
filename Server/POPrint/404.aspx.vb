
Partial Class _404
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If InStr(Replace(Page.Request.Url.ToString(), "http://", ""), "/") > 0 Then
            Dim urlpart() As String = Split(Page.Request.Url.ToString(), "/")
            Page.Server.Transfer("default.aspx?SUP=" & urlpart(UBound(urlpart)))
        End If
    End Sub
End Class
