Imports ntSoxLib

Public Class Main

    Dim c As MyClient

    Private Sub Form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load      
        Timer.Enabled = True
    End Sub

    Private Sub Timer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer.Tick
        Timer.Enabled = False
        c = New MyClient("127.0.0.1", 8022)
        c.Connect()
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If c.Send("Test") Then

        End If
    End Sub


End Class