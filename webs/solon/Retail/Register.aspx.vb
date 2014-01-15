Imports cmSi

Partial Class Register
    Inherits cmsInherit

    

    Protected Sub CreateUserWizard1_CreatingUser(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.LoginCancelEventArgs) Handles CreateUserWizard1.CreatingUser
        Dim cuw As CreateUserWizard = sender
        cuw.Email = cuw.UserName        
    End Sub

    Public Overrides Function AdminContext() As Boolean
        Return User.IsInRole("webmaster")
    End Function

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        CreateUserWizard1.DuplicateUserNameErrorMessage = "Email address already in use, forgot password? <br/>Click <a href=""lostpassword.aspx"">here</a>"
    End Sub
End Class
