Imports cmSi

Partial Class login
    Inherits cmsInherit

    Public Overrides Sub LoadReplaceModules()

    End Sub

    Public Overrides Sub PageLoaded(ByVal sender As Object, ByVal e As System.EventArgs)

    End Sub

    Public Overrides Function AdminContext() As Boolean
        Return User.IsInRole("webmaster")
    End Function

    Protected Sub Login1_LoggedIn(ByVal sender As Object, ByVal e As System.EventArgs) Handles Login1.LoggedIn
        Dim arui As String = Server.UrlDecode(Context.Request.Url.AbsoluteUri)
        If Right(arui, 1) = "/" Then arui = Left(arui, arui.Length - 1)
        If Not IsNothing(Request("redirect")) Then
            Response.Redirect(Request("redirect"))
        Else
            If String.Compare(Request.ServerVariables("SCRIPT_NAME"), "/" & Split(arui, "/").Last, True) = 0 Then
                Response.Redirect("/default.aspx")
            Else
                Response.Redirect("/" & _
                    Split(arui, "/")(UBound(Split(arui, "/"))) _
                )
            End If
        End If
    End Sub

End Class
