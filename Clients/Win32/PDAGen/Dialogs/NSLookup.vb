Imports System.Windows.Forms

Public Class NSLookup

    Private Sub btn_lookup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_lookup.Click
        Try
            With Me
                Me.txtIP.Text = System.Net.Dns.GetHostEntry(.txtHost.Text).AddressList(0).ToString()
                btn_copy.Enabled = True
            End With
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub btn_copy_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_copy.Click
        Clipboard.SetText(Me.txtIP.Text & Chr(9) & "soapsvc")
        MsgBox("Copied details to clipboard. Paste into hosts.")
    End Sub

    Private Sub txtHost_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtHost.TextChanged
        btn_copy.Enabled = False
    End Sub

End Class
