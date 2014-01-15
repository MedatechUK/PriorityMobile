Imports cmSi

Partial Class _500
    Inherits cmsInherit

    Public Overrides Function AdminContext() As Boolean
        Return User.IsInRole("webmaster")
    End Function

    Public Overrides Sub PageLoaded(ByVal sender As Object, ByVal e As System.EventArgs)

    End Sub

End Class