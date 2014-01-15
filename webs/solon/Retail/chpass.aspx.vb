Imports cmSi

Partial Class chpass
    Inherits cmsInherit

    Public Overrides Function AdminContext() As Boolean
        Return User.IsInRole("webmaster")
    End Function

End Class
