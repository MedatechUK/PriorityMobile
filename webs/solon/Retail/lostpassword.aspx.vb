Imports cmSi

Partial Class lostpassword
    Inherits cmsInherit

    Public Overrides Sub LoadReplaceModules()

    End Sub

    Public Overrides Sub PageLoaded(ByVal sender As Object, ByVal e As System.EventArgs)

    End Sub

    Public Overrides Function AdminContext() As Boolean
        Return User.IsInRole("webmaster")
    End Function

    Protected Sub SubmitButton0_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Response.Redirect("/")
    End Sub

    
End Class
