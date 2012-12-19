Public Class Form1

    Private Sub MenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem1.Click
        Dim frm As New sfdc3ui.iForm("module", New myTestHandler)
    End Sub

    Private Sub Form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim mnu As New sfdc3ui.ctrlMenu
        Me.Menu = mnu
    End Sub

End Class
