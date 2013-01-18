Imports System.IO.Ports

Public Class Form1

    Private Sub Form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'For Each port As String In SerialPort.GetPortNames()
        '    Debug.Print(port)
        'Next
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim before As String() = SerialPort.GetPortNames
        MsgBox("plug it in!", MsgBoxStyle.OkOnly)
        For Each port As String In SerialPort.GetPortNames()
            If Not before.Contains(port) Then
                MsgBox(String.Format(("found new port [{0}]"), port))
            End If
        Next
    End Sub
End Class
