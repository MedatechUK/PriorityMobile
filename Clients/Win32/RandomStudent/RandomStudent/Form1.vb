Public Class Form1

    Private Sub RandomStudentToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RandomStudentToolStripMenuItem.Click
        Dim students As New List(Of String)
        For Each Str As String In Split(Me.txtStudent.Text, vbCrLf)
            If Trim(Str).Length > 0 Then
                students.Add(Trim(Str))
            End If
        Next
        Dim DS As New DisplayStudent(students, My.Settings("rolldelay"), My.Settings("flashdelay"))
        DS.Show()
    End Sub

    Private Sub tbRollDelay_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbRollDelay.Scroll
        lblRollDelay.Text = tbRollDelay.Value
        My.Settings("rolldelay") = tbRollDelay.Value
    End Sub

    Private Sub tbFlashDelay_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbFlashDelay.Scroll
        lblFlashDelay.Text = tbFlashDelay.Value
        My.Settings("flashdelay") = tbFlashDelay.Value
    End Sub

    Private Sub Form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        tbRollDelay.Value = My.Settings("rolldelay")
        lblRollDelay.Text = My.Settings("rolldelay")
        tbFlashDelay.Value = My.Settings("flashdelay")
        lblFlashDelay.Text = My.Settings("flashdelay")
    End Sub

    Private Sub TrackBar1_Scroll_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbFlashDelay.Scroll

    End Sub

    Private Sub SettingsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SettingsToolStripMenuItem.Click
        Me.PanelSettings.Visible = True
        Me.PanelStudents.Visible = False
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Me.PanelSettings.Visible = False
        Me.PanelStudents.Visible = True
        My.Settings.Save()
    End Sub

End Class
