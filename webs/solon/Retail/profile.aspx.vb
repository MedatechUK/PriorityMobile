Imports cmSi

Partial Class profile
    Inherits cmsInherit

    Public Overrides Sub LoadReplaceModules()
        addReplaceModule(New cmSi.repl_Profile)
    End Sub

    Public Overrides Function AdminContext() As Boolean
        Return User.IsInRole("webmaster")
    End Function

End Class
