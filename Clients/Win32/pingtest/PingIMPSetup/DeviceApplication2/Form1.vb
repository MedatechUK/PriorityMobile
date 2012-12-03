
Imports System.IO
Imports System
Imports System.Collections
Imports System.Text
Imports System.Configuration

Imports System.Diagnostics

Imports System.Net
Imports System.Net.Sockets
Imports System.Threading

Public Class Form1
    Dim myConnect As System.Net.Sockets.Socket

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click

        Dim bidEndPoint As IPEndPoint
        bidEndPoint = New IPEndPoint(IPAddress.Parse("127.0.0.1"), 8022)
        myConnect = New Socket _
                     (AddressFamily.InterNetwork, _
                     SocketType.Stream, _
                     ProtocolType.Tcp)
        Try

            myConnect.Connect(bidEndPoint)
            While Not myConnect.Connected
                Thread.Sleep(10)
            End While

        Catch ex As SocketException
            myConnect.Close()
        End Try



    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Dim bteSend() As Byte
        bteSend = Encoding.ASCII.GetBytes(Me.TextBox1.Text & vbCrLf)
        myConnect.Send _
          (bteSend, 0, bteSend.Length, _
          SocketFlags.DontRoute)
    End Sub
End Class
