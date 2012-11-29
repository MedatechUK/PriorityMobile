
Public Class About

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Me.Close()
    End Sub

    Private Sub About_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown, Button1.KeyDown
        Select Case e.KeyCode
            Case Keys.Enter, Keys.Cancel, Keys.Escape
                Me.Close()
        End Select
    End Sub

    Private Sub About_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Dim p As System.Drawing.Point
        p.X = (Screen.PrimaryScreen.WorkingArea.Width - Me.Width) / 2
        p.Y = (Screen.PrimaryScreen.WorkingArea.Height - Me.Height) / 2
        Me.Location = p

        Dim a As String = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Major
        Dim b As String = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Minor
        Dim c As String = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Build
        Dim d As String = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Revision
        Me.lbl_Version.Text = a & "." & b & "." & c & "." & d

    End Sub

End Class