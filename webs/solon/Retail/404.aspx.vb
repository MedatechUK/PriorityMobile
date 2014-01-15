Imports cmSi

Partial Class _404
    Inherits cmsInherit

    Public Overrides Function AdminContext() As Boolean
        Return User.IsInRole("webmaster")
    End Function

End Class