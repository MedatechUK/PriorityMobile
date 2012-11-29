Public Class LoginForm1

    ' TODO: Insert code to perform custom authentication using the provided username and password 
    ' (See http://go.microsoft.com/fwlink/?LinkId=35339).  
    ' The custom principal can then be attached to the current thread's principal as follows: 
    '     My.User.CurrentPrincipal = CustomPrincipal
    ' where CustomPrincipal is the IPrincipal implementation used to perform authentication. 
    ' Subsequently, My.User will return identity information encapsulated in the CustomPrincipal object
    ' such as the username, display name, etc.

    Private StartTimer As System.Timers.Timer

    Public Sub New(Optional ByVal Username As String = Nothing)
        InitializeComponent()
        Me.UsernameTextBox.Focus()
        If Not IsNothing(Username) Then
            If Username.Length > 0 Then
                Me.UsernameTextBox.Text = Username                
                StartTimer = New System.Timers.Timer
                With StartTimer
                    .Interval = 10
                    AddHandler .Elapsed, AddressOf hStarttimer
                    .Enabled = True
                End With
            End If
        End If
    End Sub

    Public Sub ResetForm(Optional ByVal Username As String = Nothing)
        Me.UsernameTextBox.Focus()
        If Not IsNothing(Username) Then
            If Username.Length > 0 Then                
                With Me
                    .UsernameTextBox.Text = Username
                    .PasswordTextBox.Text = ""
                    Select Case .PasswordTextBox.Text.Length
                        Case 0
                            .PasswordTextBox.Focus()
                        Case Else
                            .PasswordTextBox.Focus()
                    End Select
                End With
            End If
        End If
    End Sub

    Private Sub hStartTimer(ByVal sender As Object, ByVal e As System.Timers.ElapsedEventArgs)
        With StartTimer
            .Enabled = False
            .Dispose()
            Invoke(New Action(AddressOf ThreadSafeSetFocus))
        End With

    End Sub

    Private Sub ThreadSafeSetFocus()
        With Me
            .PasswordTextBox.Text = ""
            Select Case .PasswordTextBox.Text.Length
                Case 0
                    .PasswordTextBox.Focus()
                Case Else
                    .PasswordTextBox.Focus()
            End Select
        End With
    End Sub

    Private Sub OK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK.Click
        With My.Settings
            .UserName = Me.UsernameTextBox.Text
            .Save()
        End With

        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

End Class
