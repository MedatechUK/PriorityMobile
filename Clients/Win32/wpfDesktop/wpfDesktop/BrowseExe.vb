Imports System.Windows.Forms
Imports System.io

Public Class BrowseExe

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click

        If Not (File.Exists(Me.exePath.Text)) Then
            MsgBox("Invalid execuatable path")
            Exit Sub
        End If

        If Me.exeName.Text.Length = 0 Then
            MsgBox("Please enter a name for the executable.")
            Exit Sub
        End If

        If Not Strings.Right(LCase(exeName.Text), 4) = ".exe" Then
            exeName.Text = exeName.Text & ".EXE"
        End If

        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()

    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        With OpenFile
            .Filter = "Executables|*.exe"
            If .ShowDialog = Windows.Forms.DialogResult.OK Then
                Me.exePath.Text = .FileName
            End If
        End With

    End Sub

    Private Sub BrowseExe_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.exeName.Focus()
    End Sub
End Class
