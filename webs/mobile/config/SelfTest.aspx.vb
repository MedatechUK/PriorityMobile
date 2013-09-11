Partial Class config_SelfTest
    Inherits iSettings 'System.Web.UI.Page

    Dim test As New ConfigValidation

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click

        txtErrDes.Text = test.ErrorDescription(test.ErrorCode(Me))

    End Sub

    Private Sub tick(ByRef iPage As Page, ByVal er As String)
        If Not IsNothing(iPage) Then
            Dim ercheck As CheckBox = Nothing
            ercheck = Page.FindControl(er)
            If IsNothing(ercheck) Then
                ercheck = iPage.Master.FindControl(er)
            End If
            If Not IsNothing(ercheck) Then
                ercheck.Checked = True
            End If
        End If
    End Sub

End Class
