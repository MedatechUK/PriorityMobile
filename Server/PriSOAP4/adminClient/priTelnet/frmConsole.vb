Imports System.IO

Public Class frmConsole

#Region "Private Declarations"

    Dim WithEvents c As MyClient

    Private isClosing As Boolean = False

    Private CommandHistory As New Dictionary(Of Integer, String)
    Private CurrentCommandIndex As Integer = 0
    Private CurrentIndex As Integer = 0

    Private Connected As Boolean = False
    Private ThisCommand As New System.Text.StringBuilder
    Private ResponseCommand As String = ""

    Private hKeyResult As Keys
    Private ControlCode As Boolean = False

    Private _Response As Boolean = False
    Private _Result As Boolean = False
    Private _LoggedIn As Boolean = False
    Private loginTimer As System.Timers.Timer

    Private frmLogin As LoginForm1
    Private StartTimer As New System.Timers.Timer

#End Region

#Region "initialisation and finalisation"

    Private Sub Form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            doStartArgs(Environment.GetCommandLineArgs)
        Catch ex As Exception
            MsgBox(String.Format("Invalid argument: {0}", ex.Message), MsgBoxStyle.OkOnly + MsgBoxStyle.Critical, "Error.")
        End Try

        With My.Settings
            Me.DialogForeColour.Color = .forecolour
            Me.DialogBackColour.Color = .backcolour
            Me.DialogFont.Font = .ScreenFont
        End With
        With Me
            With .rtfConsole
                .ForeColor = Me.DialogForeColour.Color
                .BackColor = Me.DialogBackColour.Color
                .Font = Me.DialogFont.Font
            End With
        End With
        Me.rtfConsole.Focus()

        With My.Settings
            Try
                With .hSize
                    If .Height < 100 Then Throw New Exception
                    If .Width < 100 Then Throw New Exception
                End With
            Catch
                Dim HSZ As New Size(800, 400)
                Dim HLC As Point
                With Screen.PrimaryScreen.WorkingArea
                    HLC = New Point((.Width - HSZ.Width) / 2, (.Height - HSZ.Height) / 2)
                End With
                .hSize = HSZ
                .hLocation = HLC
                .Save()
            Finally
                Me.Location = New Point(.hLocation.X, .hLocation.Y)
                Me.Size = New Size(.hSize.Width, .hSize.Height)
            End Try
        End With

        PriPROCSVC = New System.ServiceProcess.ServiceController("PRIPROC4")
        Select Case PriPROCSVC.Status
            Case ServiceProcess.ServiceControllerStatus.Stopped
                StartTimer = New System.Timers.Timer
                With StartTimer
                    .Interval = 3000
                    AddHandler .Elapsed, AddressOf hShowsStartArgs
                    .Enabled = True
                End With
        End Select

    End Sub

    Private Sub hShowsStartArgs(ByVal sender As Object, ByVal e As System.Timers.ElapsedEventArgs)
        With StartTimer
            .Enabled = False
            .Dispose()
        End With
        StartServiceToolStripMenuItem_Click(Me, New System.EventArgs)
    End Sub

    Private Sub frmConsole_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        With My.Settings
            .hLocation = New Point(Me.Location.X, Me.Location.Y)
            .hSize = New Size(Me.Size.Width, Me.Size.Height)
            .Save()
        End With

        isClosing = True
        If Connected Then
            _Response = False
            _Result = False
            c.Send("Quit")
            rtfConsole.AppendText(vbCrLf)
            ResponseCommand = "quit"
            For i As Integer = 0 To 5000000
                Application.DoEvents()
                If _Response Then Exit For
            Next

            _LoggedIn = False
            If Not IsNothing(c) Then
                c.Disconnect()
            End If

            Me.Invalidate()
        Else
            End
        End If

    End Sub

    Private Sub frmConsole_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Me.Paint
        With Me

            .StatusLabel.Text = CurrentIndex.ToString
            .CommandLabel.Text = ThisCommand.ToString
            .CommandHistoryLabel.Text = CurrentCommandIndex.ToString
            .CloseToolStripMenuItem.Enabled = Connected
            .OpenToolStripMenuItem.Enabled = Not (Connected)

            .SuspendLayout()
            With .rtfConsole
                If .SelectedText.Length = 0 Then
                    .Focus()
                    .Select(ReadOnlyLen() + CurrentIndex, 0)
                End If
            End With
            .ResumeLayout()
        End With
    End Sub

#End Region

#Region "toolstrip handlers"

    Private Sub WindToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ForegroundColourToolStripMenuItem.Click
        If DialogForeColour.ShowDialog = Windows.Forms.DialogResult.OK Then
            With Me
                .rtfConsole.ForeColor = DialogForeColour.Color
            End With
            With My.Settings
                .forecolour = Me.DialogForeColour.Color
                .Save()
            End With
        End If
    End Sub

    Private Sub BackgroundColourToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BackgroundColourToolStripMenuItem1.Click
        If DialogBackColour.ShowDialog = Windows.Forms.DialogResult.OK Then
            With Me
                .rtfConsole.BackColor = DialogBackColour.Color
            End With
            With My.Settings
                .backcolour = Me.DialogBackColour.Color
                .Save()
            End With
        End If
    End Sub

    Private Sub FontToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ConsoleFontToolStripMenuItem.Click
        If DialogFont.ShowDialog = Windows.Forms.DialogResult.OK Then
            With Me
                .rtfConsole.Font = DialogFont.Font
            End With
            With My.Settings
                .ScreenFont = Me.DialogFont.Font
                .Save()
            End With
        End If
    End Sub

    Private Sub OpenToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OpenToolStripMenuItem.Click

        If IsNothing(frmLogin) Then
            frmLogin = New LoginForm1(My.Settings.UserName)
        Else
            With frmLogin
                .ResetForm(My.Settings.UserName)
                .Visible = False
            End With
        End If

        If frmLogin.ShowDialog = Windows.Forms.DialogResult.OK Then

            rtfConsole.Text = ""

            If Not IsNothing(c) Then
                c.Disconnect()
                c = Nothing
            End If

            With My.Settings
                c = New MyClient(.ip, .port)
            End With

            _LoggedIn = False
            _Response = False
            _Result = False
            ResponseCommand = "priproc"
            c.Connect()

        End If

    End Sub

    Private Sub CloseToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CloseToolStripMenuItem.Click

        With My.Settings
            .hLocation = New Point(Me.Location.X, Me.Location.Y)
            .hSize = New Size(Me.Size.Width, Me.Size.Height)
            .Save()
        End With

        c.Send("Quit")
        rtfConsole.AppendText(vbCrLf)
        _Response = False
        ResponseCommand = "quit"
        For i As Integer = 0 To 5000000
            Application.DoEvents()
            If _Response Then Exit For
        Next

        _LoggedIn = False
        If Not IsNothing(c) Then
            c.Disconnect()
        End If

        Me.Invalidate()
        If isClosing Then End

    End Sub

    Private Sub QuitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles QuitToolStripMenuItem.Click

        With My.Settings
            .hLocation = New Point(Me.Location.X, Me.Location.Y)
            .hSize = New Size(Me.Size.Width, Me.Size.Height)
            .Save()
        End With

        isClosing = True
        If Connected Then
            CloseToolStripMenuItem_Click(Me, New System.EventArgs)
        Else
            End
        End If
    End Sub

    Private Sub WindowsEventViewerToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles WindowsEventViewerToolStripMenuItem.Click
        Using p As New Process
            With p
                With .StartInfo
                    .Arguments = "/s"
                    .FileName = "eventvwr.msc"
                End With
                .Start()
            End With
        End Using
    End Sub

    Private Sub ServicesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ServiceManagerToolStripMenuItem.Click
        Using p As New Process
            With p
                With .StartInfo
                    .Arguments = ""
                    .FileName = "services.msc"
                End With
                .Start()
            End With
        End Using
    End Sub

    Private Sub InternetInformationServerToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles InternetInformationServerToolStripMenuItem.Click
        Using p As New Process
            With p
                With .StartInfo
                    .Arguments = ""
                    .FileName = "InetMgr.exe"
                End With
                .Start()
            End With
        End Using
    End Sub

    Private Sub PriProcServiceToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PriProcServiceToolStripMenuItem.Click
        Using p As New Process
            With p
                With .StartInfo
                    .Arguments = ""
                    .WorkingDirectory = Environment.CurrentDirectory & "\source\"
                    .FileName = "PriPROCService.sln"
                End With
                .Start()
            End With
        End Using
    End Sub

    Private Sub AdminTerminalToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AdminTerminalToolStripMenuItem.Click
        Using p As New Process
            With p
                With .StartInfo
                    .Arguments = ""
                    .WorkingDirectory = Environment.CurrentDirectory & "\source\"
                    .FileName = "priTerminal.sln"
                End With
                .Start()
            End With
        End Using
    End Sub

    Private Sub VBNETToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles VBNETToolStripMenuItem.Click
        Using p As New Process
            With p
                With .StartInfo
                    .Arguments = ""
                    .WorkingDirectory = Environment.CurrentDirectory & "\examples\"
                    .FileName = "vb.sln"
                End With
                .Start()
            End With
        End Using
    End Sub

    Private Sub CNETToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CNETToolStripMenuItem.Click
        Using p As New Process
            With p
                With .StartInfo
                    .Arguments = ""
                    .WorkingDirectory = Environment.CurrentDirectory & "\examples\"
                    .FileName = "csharp.sln"
                End With
                .Start()
            End With
        End Using
    End Sub

    Private Sub AboutToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AboutToolStripMenuItem.Click
        Using frmABout As New AboutBox1
            frmABout.ShowDialog()
        End Using
    End Sub

    Private Sub DevWikiToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DevWikiToolStripMenuItem.Click
        Using p As New Process
            With p
                With .StartInfo
                    .FileName = "http://dev.emerge-it.co.uk"
                End With
                .Start()
            End With
        End Using
    End Sub

    Private Sub TestToolToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TestToolToolStripMenuItem.Click
        Using p As New Process
            With p
                With .StartInfo
                    .FileName = "wbemtest.exe"
                End With
                .Start()
            End With
        End Using
    End Sub

    Private Sub Install2k8r2Win7WMIHotfixToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Install2k8r2Win7WMIHotfixToolStripMenuItem.Click
        If MsgBox("This will install kb981314 Hotfix. See WMI help for more details. _DO_NOT_run on operating systems other than those listed. Are you sure you wish to proceed?", MsgBoxStyle.YesNo, "kb981314") = MsgBoxResult.Yes Then
            Using p As New Process
                With p
                    With .StartInfo
                        .FileName = "kb981314_zip.exe"
                    End With
                    .Start()
                End With
            End Using
        End If
    End Sub

    Private Sub WMITestToolToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles WMITestToolToolStripMenuItem.Click
        Using p As New Process
            With p
                With .StartInfo
                    .FileName = "wmitester.mht"
                End With
                .Start()
            End With
        End Using
    End Sub

    Private Sub HotFixKBToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HotFixKBToolStripMenuItem.Click
        Using p As New Process
            With p
                With .StartInfo
                    .FileName = "kb981314.mht"
                End With
                .Start()
            End With
        End Using
    End Sub

    Private Sub WMIreadme1stToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles WMIreadme1stToolStripMenuItem.Click
        'http://msdn.microsoft.com/en-us/library/ms257340(v=VS.80).aspx
        Using p As New Process
            With p
                With .StartInfo
                    .FileName = "http://msdn.microsoft.com/en-us/library/ms257340(v=VS.80).aspx"
                End With
                .Start()
            End With
        End Using
    End Sub

    Private Sub TestEventHandlerToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TestEventHandlerToolStripMenuItem.Click
        Using fWMI As New frmWMI
            fWMI.ShowDialog()
        End Using
    End Sub

    Private Sub CopyWebFilesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CopyWebFilesToolStripMenuItem.Click
        Dim dir As New System.IO.DirectoryInfo("c:\inetpub")
        If dir.Exists Then FolderBrowser.SelectedPath = "c:\inetpub"
        If FolderBrowser.ShowDialog = Windows.Forms.DialogResult.OK Then
            CopyDirectory(IO.Path.Combine(Environment.CurrentDirectory, "source\websites\mobile"), _
                          FolderBrowser.SelectedPath, _
                          True)
            Using p As New Process
                With p
                    With .StartInfo
                        .FileName = FolderBrowser.SelectedPath
                    End With
                    .Start()
                End With
            End Using
        End If
    End Sub

    Private Sub OpenWebConfigToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OpenWebConfigToolStripMenuItem.Click
        Using p As New Process
            With p
                With .StartInfo
                    .FileName = "http://localhost:8080/config"
                End With
                .Start()
            End With
        End Using
    End Sub

#End Region

#Region "Client Event Handlers"

    Private Sub honClientConnection() Handles c.onClientConnection

        Connected = True

        loginTimer = New System.Timers.Timer
        With loginTimer
            .Interval = 1
            AddHandler .Elapsed, AddressOf hLoginTimerElapsed
            .Enabled = True
        End With

    End Sub

    Private Sub hLoginTimerElapsed(ByVal sender As Object, ByVal e As System.Timers.ElapsedEventArgs)

        With loginTimer
            .Enabled = False
            .Dispose()
        End With

        For i As Integer = 0 To 5000
            Application.DoEvents()
            If _Response Then Exit For
        Next

        If _Result Then
            System.Threading.Thread.Sleep(500)

            _Response = False
            _Result = False
            ResponseCommand = "user"
            Debug.Write(String.Format("Sending: user {0}", frmLogin.UsernameTextBox.Text))
            c.SendFormat("user {0}", frmLogin.UsernameTextBox.Text)

            For i As Integer = 0 To 500000
                Application.DoEvents()
                If _Response Then Exit For
            Next

            If _Result Then
                System.Threading.Thread.Sleep(500)

                _Response = False
                _Result = False
                ResponseCommand = "pass"
                Debug.Write(String.Format("Sending: pass {0}", frmLogin.PasswordTextBox.Text))
                c.SendFormat("pass {0}", frmLogin.PasswordTextBox.Text)

                For i As Integer = 0 To 500000
                    Application.DoEvents()
                    If _Response Then Exit For
                Next

                If _Result Then
                    _LoggedIn = True
                Else
                    _LoggedIn = False
                    Invoke(New Action(Of Object, MsgBoxStyle, Object)(AddressOf ThreadSafeMsgBox), "Invalid username / password.", MsgBoxStyle.OkOnly, Nothing)
                    c.Disconnect()
                End If

            End If
        End If

    End Sub

    Private Sub ThreadSafeMsgBox(ByVal Prompt As Object, Optional ByVal buttons As MsgBoxStyle = MsgBoxStyle.ApplicationModal, Optional ByVal Title As Object = Nothing)
        MsgBox(Prompt, buttons, Title)
    End Sub

    Public Sub hOnDataStream(ByVal StreamData As String) Handles c.OnDataStream

        Debug.Write(StreamData)
        Invoke(New Action(Of String)(AddressOf threadSafeUpdateRTF), StreamData)

        If InStr(StreamData, String.Format("+{0}", ResponseCommand), CompareMethod.Text) > 0 Then
            _Response = True
            _Result = True
            Debug.WriteLine(String.Format("Found +{0}", ResponseCommand))
        End If

        If InStr(StreamData, String.Format("-{0}", ResponseCommand), CompareMethod.Text) > 0 Then
            _Response = True
            _Result = False
            Debug.WriteLine(String.Format("Found -{0}", ResponseCommand))
        End If

        ' Service is closing
        If InStr(StreamData, "+bye", CompareMethod.Text) > 0 Then
            If Not IsNothing(c) Then
                c.stopping = True
                c.Disconnect()
            End If
            Connected = False
            _LoggedIn = False
            MsgBox("Service Stopped.", MsgBoxStyle.OkOnly)
            Me.Invalidate()
        End If

        ' We quit
        If InStr(StreamData, "+quit", CompareMethod.Text) > 0 Then
            If Not IsNothing(c) Then
                c.stopping = True
                c.Disconnect()
            End If
            Connected = False
            _LoggedIn = False            
            Me.Invalidate()
        End If

    End Sub

    Private Sub honClientConnectionFail(ByVal ex As String) Handles c.onClientConnectionFail
        If Not IsNothing(c) Then
            c.stopping = True
            c.Disconnect()
        End If
        Connected = False
        _LoggedIn = False
        MsgBox("Connection failed.", MsgBoxStyle.OkOnly)
        Me.Invalidate()
    End Sub

    Private Sub honClientDisconnection() Handles c.onClientDisconnection
        If Not IsNothing(c) Then
            c.stopping = True
            c.Disconnect()
        End If
        _LoggedIn = False
        Connected = False
        Me.Invalidate()
    End Sub

#End Region

#Region "rtf control handlers"

    Private Sub hKeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles rtfConsole.KeyDown

        hKeyResult = e.KeyCode

        If Not _LoggedIn Then e.SuppressKeyPress = True

        Select Case e.KeyData
            Case Keys.Enter

                ControlCode = True
                e.SuppressKeyPress = True
                If rtfConsole.SelectedText.Length > 0 Then
                    Clipboard.SetText(rtfConsole.SelectedText)
                    rtfConsole.Select(rtfConsole.TextLength, 0)
                    Me.Invalidate()
                Else
                    rtfConsole.AppendText(vbCrLf)

                    CurrentIndex = 0
                    CurrentCommandIndex += 1


                    Select Case ThisCommand.ToString.ToLower
                        Case "quit"
                            If Connected Then CloseToolStripMenuItem_Click(Me, New System.EventArgs)
                        Case "cls"
                            rtfConsole.Text = ""
                            If Connected Then c.Send("rem")
                        Case ""
                            If Connected Then c.Send("rem")
                        Case Else
                            If Connected Then c.Send(ThisCommand.ToString)
                    End Select


                    Try
                        CommandHistory.Add(CommandHistory.Keys.Max + 1, ThisCommand.ToString)
                    Catch
                        CommandHistory.Add(0, ThisCommand.ToString)
                    Finally
                        ThisCommand = New System.Text.StringBuilder
                    End Try
                End If

            Case Keys.Left
                ControlCode = True
                e.SuppressKeyPress = True
                If CurrentIndex > 0 Then
                    CurrentIndex -= 1
                End If

            Case Keys.Right
                ControlCode = True
                e.SuppressKeyPress = True
                If CurrentIndex < ThisCommand.Length Then
                    CurrentIndex += 1
                End If

            Case Keys.Home
                ControlCode = True
                e.SuppressKeyPress = True
                CurrentIndex = 0

            Case Keys.End
                ControlCode = True
                e.SuppressKeyPress = True
                CurrentIndex = ThisCommand.Length

            Case Keys.Back
                ControlCode = True
                e.SuppressKeyPress = True
                If CurrentIndex > 0 Then
                    rtfConsole.Text = rtfConsole.Text.Remove((ReadOnlyLen() + CurrentIndex) - 1, 1)
                    ThisCommand.Remove(CurrentIndex - 1, 1)
                    CurrentIndex -= 1
                End If

            Case Keys.Delete
                ControlCode = True
                e.SuppressKeyPress = True
                If CurrentIndex < ThisCommand.Length Then
                    rtfConsole.Text = rtfConsole.Text.Remove((ReadOnlyLen() + CurrentIndex), 1)
                    ThisCommand.Remove(CurrentIndex, 1)
                End If

            Case Keys.Up, Keys.PageUp
                ControlCode = True
                e.SuppressKeyPress = True
                If CommandHistory.Keys.Count > 0 Then
                    If CurrentCommandIndex - 1 >= CommandHistory.Keys.Min Then
                        rtfConsole.Text = rtfConsole.Text.Remove(ReadOnlyLen(), ThisCommand.Length)
                        CurrentCommandIndex -= 1
                        ThisCommand = New System.Text.StringBuilder(CommandHistory(CurrentCommandIndex))
                        rtfConsole.AppendText(ThisCommand.ToString)
                        CurrentIndex = ThisCommand.Length
                    End If
                End If

            Case Keys.Down, Keys.PageDown
                ControlCode = True
                e.SuppressKeyPress = True
                If CommandHistory.Keys.Count > 0 Then
                    If CurrentCommandIndex + 1 <= CommandHistory.Keys.Max Then
                        rtfConsole.Text = rtfConsole.Text.Remove(ReadOnlyLen(), ThisCommand.Length)
                        CurrentCommandIndex += 1
                        ThisCommand = New System.Text.StringBuilder(CommandHistory(CurrentCommandIndex))
                        rtfConsole.AppendText(ThisCommand.ToString)
                        CurrentIndex = ThisCommand.Length
                    End If
                End If

            Case Keys.Escape
                ControlCode = True
                e.SuppressKeyPress = True
                rtfConsole.Text = rtfConsole.Text.Remove(ReadOnlyLen(), ThisCommand.Length)
                If CommandHistory.Keys.Count > 0 Then
                    CurrentCommandIndex = CommandHistory.Keys.Max + 1
                End If
                ThisCommand = New System.Text.StringBuilder()
                CurrentIndex = 0

            Case Keys.Control + Keys.V
                Dim cbt As String = Clipboard.GetText
                Clipboard.Clear()
                ThisCommand.Append(cbt)
                rtfConsole.AppendText(cbt)
                CurrentIndex = ThisCommand.Length
                Me.Invalidate()

            Case Keys.Control + Keys.C
                ControlCode = True
                e.SuppressKeyPress = True
                Select Case Connected
                    Case False
                        OpenToolStripMenuItem_Click(Me, New System.EventArgs)
                    Case Else
                        If rtfConsole.SelectedText.Length > 0 Then
                            Clipboard.SetText(rtfConsole.SelectedText)
                            rtfConsole.Select(rtfConsole.TextLength, 0)
                            Me.Invalidate()
                        Else
                            CloseToolStripMenuItem_Click(Me, New System.EventArgs)
                        End If

                End Select

            Case Keys.Control + Keys.A
                ControlCode = True
                e.SuppressKeyPress = True
                rtfConsole.SelectAll()

            Case Else
                ControlCode = False
        End Select

        If e.SuppressKeyPress Then Me.Invalidate()

    End Sub

    Private Sub hKeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles rtfConsole.KeyPress
        If ControlCode Then
            e.Handled = True
        Else
            ThisCommand.Insert(CurrentIndex, e.KeyChar, 1)
            CurrentIndex += 1
        End If
        Me.Invalidate()
    End Sub

    Private Sub threadSafeUpdateRTF(ByVal StreamData As String)
        If _LoggedIn Then
            rtfConsole.AppendText(StreamData)
            Me.Invalidate()
        End If
    End Sub

    Private Sub rtfConsole_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles rtfConsole.MouseClick
        If Not rtfConsole.SelectedText.Length > 0 Then Me.Invalidate()
    End Sub

    Private Sub rtfConsole_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles rtfConsole.MouseDown
        If e.Button = Windows.Forms.MouseButtons.Right Then
            Dim cbt As String = Clipboard.GetText
            Clipboard.Clear()
            ThisCommand.Append(cbt)
            rtfConsole.AppendText(cbt)
            CurrentIndex = ThisCommand.Length
            Me.Invalidate()
        End If
    End Sub

    Private Function ReadOnlyLen() As Integer
        Return Me.rtfConsole.TextLength - ThisCommand.Length
    End Function

#End Region

#Region "Service Controller"

    Private Sub PriPROCToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) _
        Handles PriPROCToolStripMenuItem.Click, PriPROCToolStripMenuItem.MouseDown, PriPROCToolStripMenuItem.MouseEnter

        Me.StartServiceToolStripMenuItem.Enabled = False
        Me.StopServiceToolStripMenuItem.Enabled = False
        Me.PauseServiceToolStripMenuItem.Enabled = False
        Me.ResumeServiceToolStripMenuItem.Enabled = False

        PriPROCSVC = New System.ServiceProcess.ServiceController("PRIPROC4")
        Select Case PriPROCSVC.Status
            Case ServiceProcess.ServiceControllerStatus.Running
                Me.PauseServiceToolStripMenuItem.Enabled = True
                Me.StopServiceToolStripMenuItem.Enabled = True

            Case ServiceProcess.ServiceControllerStatus.Paused
                Me.ResumeServiceToolStripMenuItem.Enabled = True

            Case ServiceProcess.ServiceControllerStatus.Stopped
                Me.StartServiceToolStripMenuItem.Enabled = True

        End Select

    End Sub

    Private Sub StartServiceToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StartServiceToolStripMenuItem.Click
        Dim args() As String = Nothing
        Using startArg As New frmStartArgs
            With startArg
                If .ShowDialog() = Windows.Forms.DialogResult.OK Then

                    If .CheckBox1.Checked Then
                        addArg(args, "-PROVIDER", .lstProvider.Text)
                        addArg(args, "-DATASOURCE", .DATASOURCE.Text)
                        addArg(args, "-PRIORITYUSER", .UsernameTextBox.Text)
                        addArg(args, "-PRIORITYPWD", .PasswordTextBox.Text)
                        addArg(args, "-PRIORITYDIR", .PRIORITYDIR.Text)
                        addArg(args, "-PRIUNC", .PRIUNC.Text)
                        addArg(args, "-SERVICEPORT", .SERVICEPORT.Text)
                    End If

                    If Not IsNothing(args) Then
                        PriPROCSVC.Start(args)
                    Else
                        PriPROCSVC.Start()
                    End If

                    Using Busy As New frmBusy("Service Starting ...", ServiceProcess.ServiceControllerStatus.Running)
                        With Busy                            
                            .StartPosition = FormStartPosition.Manual
                            .Top = Me.Top + ((Me.Height - .Height) / 2)
                            .Left = Me.Left + ((Me.Width - .Width) / 2)
                            .ShowInTaskbar = False
                            .ShowDialog()
                        End With
                    End Using

                End If
            End With
        End Using
    End Sub

    Private Sub addArg(ByRef Args() As String, ByVal Name As String, ByVal Value As String)
        If Value.Length > 0 Then
            Try
                ReDim Preserve Args(UBound(Args) + 1)
            Catch
                ReDim Args(0)
            Finally
                Args(UBound(Args)) = Name
                ReDim Preserve Args(UBound(Args) + 1)
                Args(UBound(Args)) = Value
                With My.Settings
                    If Name = "-PRIORITYUSER" Then
                        .UserName = Value
                    ElseIf Name = "-SERVICEPORT" Then
                        .port = Value
                    End If
                    .Save()
                End With
            End Try
        End If
    End Sub

    Private Sub StopServiceToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StopServiceToolStripMenuItem.Click
        PriPROCSVC.Stop()
        Using Busy As New frmBusy("Service Stopping ...", ServiceProcess.ServiceControllerStatus.Stopped)
            With Busy
                .StartPosition = FormStartPosition.Manual
                .Top = Me.Top + ((Me.Height - .Height) / 2)
                .Left = Me.Left + ((Me.Width - .Width) / 2)
                .ShowInTaskbar = False
                .ShowDialog()
            End With
        End Using
    End Sub

    Private Sub PauseServiceToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PauseServiceToolStripMenuItem.Click
        PriPROCSVC.Pause()
        Using Busy As New frmBusy("Service Pausing ...", ServiceProcess.ServiceControllerStatus.Paused)
            With Busy
                .StartPosition = FormStartPosition.Manual
                .Top = Me.Top + ((Me.Height - .Height) / 2)
                .Left = Me.Left + ((Me.Width - .Width) / 2)
                .ShowInTaskbar = False
                .ShowDialog()
            End With
        End Using
    End Sub

    Private Sub ResumeServiceToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ResumeServiceToolStripMenuItem.Click
        PriPROCSVC.Continue()
        Using Busy As New frmBusy("Service Resuming ...", ServiceProcess.ServiceControllerStatus.Running)
            With Busy
                .StartPosition = FormStartPosition.Manual
                .Top = Me.Top + ((Me.Height - .Height) / 2)
                .Left = Me.Left + ((Me.Width - .Width) / 2)
                .ShowInTaskbar = False
                .ShowDialog()
            End With
        End Using
    End Sub

#End Region

#Region "Copy Directory"

    Public Function CopyDirectory(ByVal Src As String, ByVal Dest As String, Optional _
        ByVal bQuiet As Boolean = False) As Boolean

        If Not Directory.Exists(Src) Then
            Throw New DirectoryNotFoundException("The directory " & Src & " does not exists")
        End If
        If Directory.Exists(Dest) AndAlso Not bQuiet Then
            If MessageBox.Show("directory " & Dest & " already exists." & vbCrLf & _
            "If you continue, any files with the same name will be overwritten", _
            "Continue?", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, _
            MessageBoxDefaultButton.Button2) = DialogResult.Cancel Then Exit Function
        End If

        'add Directory Seperator Character (\) for the string concatenation shown later
        If Dest.Substring(Dest.Length - 1, 1) <> Path.DirectorySeparatorChar Then
            Dest += Path.DirectorySeparatorChar
        End If
        If Not Directory.Exists(Dest) Then Directory.CreateDirectory(Dest)
        Dim Files As String()
        Files = Directory.GetFileSystemEntries(Src)
        Dim element As String
        For Each element In Files
            If Directory.Exists(element) Then
                'if the current FileSystemEntry is a directory,
                'call this function recursively
                CopyDirectory(element, Dest & Path.GetFileName(element), True)
            Else
                'the current FileSystemEntry is a file so just copy it
                File.Copy(element, Dest & Path.GetFileName(element), True)
            End If
        Next
        Return True
    End Function

#End Region

End Class
