Public Class frmYesNo

    Private Sub PictureBox1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox1.Click
        Me.DialogResult = Windows.Forms.DialogResult.Yes
    End Sub

    Private Sub PictureBox2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox2.Click
        Me.DialogResult = Windows.Forms.DialogResult.No
    End Sub

    Private Sub frmYesNo_Closing(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        If Not Me.DialogResult = Windows.Forms.DialogResult.Yes Then
            Me.DialogResult = Windows.Forms.DialogResult.No
        End If
    End Sub

    Private Sub Label1_ParentChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label1.ParentChanged

    End Sub

    Private Sub frmYesNo_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Dim p As System.Drawing.Point
        p.X = (Screen.PrimaryScreen.WorkingArea.Width - Me.Width) / 2
        p.Y = (Screen.PrimaryScreen.WorkingArea.Height - Me.Height) / 2
        Me.Location = p
    End Sub
End Class