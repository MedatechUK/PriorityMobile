Imports cmSi

Partial Class page
    Inherits cmsInherit

    Public Overrides Function AdminContext() As Boolean
        Return User.IsInRole("webmaster")
    End Function

    Public Overrides Sub PagePreInit(ByVal sender As Object, ByVal e As System.EventArgs)
        If User.IsInRole("impersonate") Then Response.Redirect("~/impersonate.aspx")
    End Sub

    Public Overrides Sub LoadReplaceModules()
        addReplaceModule( _
            New repl_ChildTable, _
            New repl_Blog _
        )
        If Not IsNothing(P.Part) Then _
            addReplaceModule( _
                New repl_Part _
            )
    End Sub

End Class
