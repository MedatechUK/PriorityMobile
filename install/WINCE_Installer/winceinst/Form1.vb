Imports System.Net

Public Class Form1

    Private Sub MenuItem3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem3.Click
        StartProcess("\windows", "conmanclient2.exe")
    End Sub

    Private Sub MenuItem4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem4.Click
        StartProcess("\windows", "cmaccept.exe")
    End Sub

    Private Sub MenuItem6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem6.Click
        StartProcess("\windows", "NETCFv35.wm.armv4i.cab")
    End Sub

    Private Sub MenuItem7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem7.Click
        StartProcess("\windows", "NETCFv35.Messages.EN.wm.cab")
    End Sub

    Private Sub StartProcess(ByVal Folder As String, ByVal file As String)
        Try
            Dim p As New Process
            With p
                With .StartInfo
                    .WorkingDirectory = Folder
                    .FileName = file
                    .UseShellExecute = True
                End With
                .Start()
            End With
            MsgBox(String.Format("Started {0}.", file))

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub

    Private Sub Form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            Dim host As IPHostEntry = System.Net.Dns.GetHostEntry(lookup.Text)
            Me.lbl_DNS_Result.Text = host.AddressList(0).ToString
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

        Try
            Dim host As IPHostEntry = System.Net.Dns.GetHostEntry(lookup.Text)
            Me.lbl_DNS_Result.Text = host.AddressList(0).ToString
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub
End Class
