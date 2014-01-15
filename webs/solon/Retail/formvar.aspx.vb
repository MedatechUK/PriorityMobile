
Partial Class formvar
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        For Each k As String In Request.Form.Keys
            Response.Write(k & " = " & Request.Params(k) & "<br>")
        Next
    End Sub

End Class
