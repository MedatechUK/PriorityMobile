Public Class Form1

    Private Sub Form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        With Me
            With .WebBrowser1
                Dim dt As String = ""
                dt += "><H1>HELLO!</H1>"
                For i As Integer = 0 To 50
                    dt += "123<BR/>"
                Next
                'dt += "</BODY></html>"
                .DocumentText = dt
            End With
        End With
    End Sub
End Class
