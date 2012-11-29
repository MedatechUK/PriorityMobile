Public Class ftpDetails

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click

        With My.Settings
            .ftpServer = txt_server.Text
            .ftpUser = txt_user.Text
            .ftpPass = txt_pass.Text
            .Save()
        End With
        Me.Close()

    End Sub

    Private Sub ftpDetails_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        With My.Settings
            txt_server.Text = .ftpServer
            txt_user.Text = .ftpUser
            txt_pass.Text = .ftpPass
        End With
    End Sub

End Class