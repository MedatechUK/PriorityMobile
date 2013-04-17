Public Class frmDrop

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Me.DialogResult = Windows.Forms.DialogResult.OK
    End Sub

    Private Sub frmDrop_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim p As System.Drawing.Point
        p.X = (Screen.PrimaryScreen.WorkingArea.Width - Me.Width) / 2
        p.Y = (Screen.PrimaryScreen.WorkingArea.Height - Me.Height) / 2
        Me.Location = p
    End Sub
End Class