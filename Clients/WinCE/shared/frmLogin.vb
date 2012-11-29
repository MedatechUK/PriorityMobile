Public Class frmLogin

    Private mResult As Microsoft.VisualBasic.MsgBoxResult
    Dim p As System.Drawing.Point

    Public ReadOnly Property Result() As MsgBoxResult
        Get
            Return mResult
        End Get
    End Property
    Public Property UserName() As String
        Get
            Return Me.UsernameTextBox.Text
        End Get
        Set(ByVal newUserName As String)
            Me.UsernameTextBox.Text = newUserName
        End Set
    End Property

    Public Property Password() As String
        Get
            Return Me.PasswordTextBox.Text
        End Get
        Set(ByVal newPassword As String)
            Me.PasswordTextBox.Text = newPassword
        End Set
    End Property

    Private Sub frmLogin_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        p.X = (Screen.PrimaryScreen.WorkingArea.Width - Me.Width) / 2
        p.Y = (Screen.PrimaryScreen.WorkingArea.Height - Me.Height) / 2
        Me.Location = p
        mResult = MsgBoxResult.Ok
        If Len(UsernameTextBox.Text) = 0 Then
            UsernameTextBox.Focus()
        ElseIf Len(PasswordTextBox.Text) = 0 Then
            PasswordTextBox.Focus()
        End If
    End Sub

    Private Sub OK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK.Click
        mResult = MsgBoxResult.Ok
        Me.Close()
    End Sub

    Private Sub Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel.Click
        mResult = MsgBoxResult.Cancel
        Me.Close()
    End Sub

    Private Sub UsernameTextBox_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles UsernameTextBox.KeyPress
        Select Case e.KeyChar
            Case Chr(13)
                If Me.UsernameTextBox.Text.Length > 0 Then PasswordTextBox.Focus()
        End Select
    End Sub

    Private Sub PasswordTextBox_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles PasswordTextBox.KeyPress
        Select Case e.KeyChar
            Case Chr(13)
                mResult = MsgBoxResult.Ok
                Me.Close()
        End Select
    End Sub

End Class
