Imports cmSi

Partial Class profile
    Inherits cmsInherit

    Public Overrides Sub LoadReplaceModules()
        addReplaceModule(New cmSi.repl_Profile)
    End Sub

    Public Overrides Function AdminContext() As Boolean
        Return User.IsInRole("webmaster")
    End Function

    Protected Sub Page_Load1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Profile.RegistrationWeb.Length <= 0 Then
            Profile.RegistrationWeb = cmSi.cmsData.Settings("WebName")
        End If
    End Sub
End Class
