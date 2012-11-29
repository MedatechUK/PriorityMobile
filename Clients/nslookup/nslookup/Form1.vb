Public Class Form1

    Private Sub Label1_ParentChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label1.ParentChanged

    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        With Me
            .TextBox2.Text = System.Net.Dns.GetHostEntry(.TextBox1.Text).AddressList(0).ToString
        End With
    End Sub
End Class
