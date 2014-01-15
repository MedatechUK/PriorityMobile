Imports cmSi
Partial Class search
    Inherits cmSi.cmsInherit

    Public Overrides Function AdminContext() As Boolean
        Return User.IsInRole("webmaster")
    End Function

End Class
