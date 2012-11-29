Imports System.Threading
Imports System.Windows.Forms
Imports System.io
Imports System.Environment

Public Class main
    Private ws As New PriWebSvc.Service
    Private sd As New Priority.SerialData
    Private ar As New Priority.MyArray
    Private tuser As Integer = -1

    Private filename As String = _
        Environment.GetFolderPath(SpecialFolder.ApplicationData) & "\AllowedList.txt"

    Private _AllowedList As String(,)
    Public Property AllowedList() As String(,)
        Get
            Return _AllowedList
        End Get
        Set(ByVal value As String(,))
            _AllowedList = value
        End Set
    End Property

    Private Sub main_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        ar.ArrayFromFile(AllowedList, filename)
        Dim result As New MsgBoxResult
        With My.Settings
            If .remember Then
                result = LogInResult(.wpfUser, .remember)
            Else
                result = LogInResult(Nothing, .remember)
            End If
            Select Case result
                Case DialogResult.Cancel
                    Me.Close()
            End Select
        End With

    End Sub

#Region "Log in Functions"

    Private Function LogInResult(ByVal UserName As String, ByVal Remember As Boolean, Optional ByVal showdialog As Boolean = False) As MsgBoxResult

        If Not (showdialog) Then
            If Not IsNothing(UserName) Then
                If UserName.Length > 0 Then
                    If login(UserName, Remember) = MsgBoxResult.Ok Then
                        Return MsgBoxResult.Ok
                    End If
                End If
            End If
        End If

        Using dologin As New LoginDialog
            With dologin
                .StartPosition = FormStartPosition.CenterScreen
                .UsernameTextBox.Text = UserName
                .CheckBox1.Checked = Remember

                Do
                    Select Case .ShowDialog
                        Case DialogResult.Cancel
                            Return MsgBoxResult.Cancel
                        Case Else
                            Select Case login(.UsernameTextBox.Text, .CheckBox1.Checked)
                                Case MsgBoxResult.Ok
                                    Return MsgBoxResult.Ok
                                Case MsgBoxResult.Cancel
                                    If MsgBox("Invalid Priority username.", MsgBoxStyle.RetryCancel + MsgBoxStyle.Question) = MsgBoxResult.Cancel Then
                                        Return MsgBoxResult.Cancel
                                    End If
                            End Select
                    End Select
                Loop
            End With
        End Using
    End Function

    Private Function login(ByVal UserName As String, ByVal Remember As Boolean) As MsgBoxResult

        Dim data(,) As String = Nothing
        Dim exc As Exception = Nothing
        Dim resp As MsgBoxResult

        Dim sql As String = _
            "SELECT T$USER FROM system.dbo.USERS " & _
            "where LOWER(USERLOGIN) = '" & LCase(UserName) & "'"
        Do
            Try
                'Throw New Exception("test")
                data = sd.DeSerialiseData(ws.GetData(sql))
            Catch ex As Exception
                exc = ex
                resp = Nothing
                resp = MsgBox("Failed to connect to service.", MsgBoxStyle.RetryCancel)
            End Try
        Loop Until IsNothing(exc) Or resp = MsgBoxResult.Cancel

        If resp = MsgBoxResult.Cancel Then
            Return MsgBoxResult.Abort
        End If

        If Not IsNothing(data) Then
            If Strings.Left(data(0, 0), 1) = "!" Then
                MsgBox(Strings.Right(data(0, 0), Len(data(0, 0)) - 1), MsgBoxStyle.Critical)
                Return MsgBoxResult.Abort
            End If
            '********************* Sucsessful login
            tuser = CInt(data(0, 0))

            Timer1.Enabled = True
            With My.Settings
                .wpfUser = UserName
                .remember = Remember
                .Save()
            End With
            Return MsgBoxResult.Ok
        Else
            Return MsgBoxResult.Cancel
        End If

    End Function

#End Region

#Region "Menu Handlers"

    Private Sub PriorityUserToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PriorityUserToolStripMenuItem.Click
        With My.Settings
            LogInResult(.wpfUser, .remember, True)
        End With
    End Sub

    Private Sub NotifyIcon_MouseDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles NotifyIcon.MouseDoubleClick
        doAllow()
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        Me.NotifyIcon.Visible = False
        Me.Close()
    End Sub

#End Region

    Private Sub Poll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick

        If tuser > -1 Then
            Dim data(,) As String
            Dim exe As String = Nothing
            Dim sql As String = "exec RemoteCallBack " & CStr(tuser)
            Try
                data = sd.DeSerialiseData(ws.GetData(sql))
                If Not IsNothing(data) Then
                    'MsgBox(data(0, 0) & " - " & data(1, 0))

                    For y As Integer = 0 To UBound(AllowedList, 2)
                        If Strings.StrComp(AllowedList(0, y), data(0, 0), CompareMethod.Text) = 0 Then
                            exe = Chr(34) & AllowedList(1, y) & Chr(34)
                            Exit For
                        End If
                    Next
                    If IsNothing(exe) Then
                        MsgBox("You received a remote call to an unassigned executable." & vbCrLf & _
                            "The executable called was " & data(0, 0) & ". " & vbCrLf & _
                            "Please notify your sysadmin.", _
                             MsgBoxStyle.Critical + MsgBoxStyle.OkOnly)
                        Exit Sub
                    End If

                    Dim sOutput As String = ""
                    Dim sErrs As String = ""
                    Dim myProcess As Process = New Process()


                    With myProcess
                        With .StartInfo
                            .FileName = "cmd.exe"
                            .UseShellExecute = False
                            .CreateNoWindow = True
                            .RedirectStandardInput = True
                            .RedirectStandardOutput = True
                            .RedirectStandardError = True
                        End With
                        .Start()

                        Dim sIn As StreamWriter = myProcess.StandardInput
                        Dim sOut As StreamReader = myProcess.StandardOutput
                        Dim sErr As StreamReader = myProcess.StandardError

                        With sIn
                            Dim cmd As String = exe & " " & data(1, 0)

                            .AutoFlush = True
                            .Write(cmd & _
                                System.Environment.NewLine)
                            .Write("exit" & _
                                System.Environment.NewLine)
                            .Close()

                        End With

                        Dim l As Integer = 0
                        Do Until l = 1000
                            If sOut.Peek <> 0 Then
                                sOutput = sOutput + sOut.ReadLine
                            End If
                            l = l + 1
                            Thread.Sleep(1)
                        Loop

                        If Len(sOutput) > 0 Then
                            sOutput = sOut.ReadToEnd
                            sOut.Close()
                            sErrs = sErr.ReadToEnd()
                            sErr.Close()
                        End If

                        If Not myProcess.HasExited Then
                            myProcess.Kill()
                        End If

                        .Close()

                    End With

                    If Len(sErrs) > 0 Then
                        MsgBox("The executable " & data(0, 0) & " was called, but errors occured. " & vbCrLf & _
                            sErrs & vbCrLf & _
                            "Please notify your sysadmin.", _
                             MsgBoxStyle.Critical + MsgBoxStyle.OkOnly)
                    End If

                End If
            Catch
            End Try
        End If
    End Sub

    Private Sub AllowedEXEToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AllowedEXEToolStripMenuItem.Click
        doAllow()
    End Sub

    Private Sub doAllow()
        Using AE As New AllowedExe()
            With AE

                If Not IsNothing(AllowedList) Then
                    With .exelist
                        For y As Integer = 0 To UBound(AllowedList, 2)
                            .Items.Add(CStr(AllowedList(0, y)))
                            .Items(y).SubItems.Add(CStr(AllowedList(1, y)))
                        Next
                    End With
                End If

                .ShowDialog()

                With .exelist
                    AllowedList = Nothing
                    If .Items.Count > 0 Then
                        ReDim AllowedList(1, .Items.Count - 1)
                        For i As Integer = 0 To .Items.Count - 1
                            AllowedList(0, i) = .Items(i).Text
                            AllowedList(1, i) = .Items(i).SubItems(1).Text
                        Next
                    End If
                End With

                ar.ArrayToFile(filename, AllowedList)

            End With
        End Using
    End Sub

    Private Sub AboutToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AboutToolStripMenuItem.Click
        Using ab As New AboutBox1
            ab.ShowDialog()
        End Using
    End Sub
End Class