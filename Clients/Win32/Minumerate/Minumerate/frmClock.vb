Public Class frmClock

    Private IsFormBeingDragged As Boolean = False
    Private MouseDownX As Integer
    Private MouseDownY As Integer

    Private Sub frmClock_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load        
        Me.txtPriDate.Text = DateToMin()
        Me.Location = My.Settings.formLocation
        If Me.Location.X = 0 And Me.Location.Y = 0 Then
            With Screen.PrimaryScreen.WorkingArea
                Dim P As New System.Drawing.Point(.Width - Me.Width, 20)
                Me.Location = P
                My.Settings.formLocation = P
                My.Settings.Save()
            End With
        End If
    End Sub

    Private Sub hClosing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        If Not MsgBox("Close the application?", MsgBoxStyle.OkCancel, "Quit?") = MsgBoxResult.Ok Then
            e.Cancel = True        
        End If
    End Sub

    Private Sub Form1_MouseDown(ByVal sender As Object, ByVal e As MouseEventArgs) Handles Button1.MouseDown

        If e.Button = MouseButtons.Left Then
            IsFormBeingDragged = True
            MouseDownX = e.X
            MouseDownY = e.Y
        End If
    End Sub

    Private Sub Form1_MouseUp(ByVal sender As Object, ByVal e As MouseEventArgs) Handles Button1.MouseUp
        If e.Button = MouseButtons.Left Then
            IsFormBeingDragged = False
            My.Settings.formLocation = Me.Location
            My.Settings.CalcLocation = New System.Drawing.Point(0, 0)
            My.Settings.Save()
        End If
    End Sub

    Private Sub Form1_MouseMove(ByVal sender As Object, ByVal e As MouseEventArgs) Handles Button1.MouseMove
        If IsFormBeingDragged Then
            Dim temp As Point = New Point()

            temp.X = Me.Location.X + (e.X - MouseDownX)
            temp.Y = Me.Location.Y + (e.Y - MouseDownY)
            Me.Location = temp
            temp = Nothing
        End If
    End Sub

    Private Sub TickTimer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TickTimer.Tick
        Me.txtPriDate.Text = DateToMin()
        Me.txtPriDate.ForeColor = Color.Black
        Me.BringToFront()
    End Sub

    Private Sub DateCalculatorToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DateCalculatorToolStripMenuItem.Click
        Dim calc As New frmCalc
        calc.ShowDialog()
    End Sub

    Private Sub AboutToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AboutToolStripMenuItem.Click
        Dim about As New frmAbout
        about.ShowDialog()
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        Close()
    End Sub

    Private Sub CopyDateToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CopyDateToolStripMenuItem.Click
        Clipboard.SetText(Me.txtPriDate.Text)
    End Sub

    Private Sub HelpToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HelpToolStripMenuItem.Click
        Using myProcess As System.Diagnostics.Process = New System.Diagnostics.Process()
            With myProcess
                With .StartInfo
                    .FileName = FullPath() & "\PriorityDate.rtf" 'CREATE THIS FILE WITH FILESHAREMODE.NONE 
                    .WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal
                    .CreateNoWindow = False
                    .UseShellExecute = True
                End With
                .Start()
            End With
        End Using
    End Sub
End Class
