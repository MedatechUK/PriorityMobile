
Partial Class Size
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.PO.Value = Request("PO")
        Me.SUP.Value = Request("SUP")
    End Sub

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        If Len(Me.ListBox1.SelectedValue) > 0 Then
            Dim url As String = "label.aspx?PO=" & Me.PO.Value & _
            "&xwid=" & _
            Split(Me.ListBox1.SelectedValue, "/")(0) & _
            "&ywid=" & _
            Split(Me.ListBox1.SelectedValue, "/")(1)
            Page.Server.Transfer(url)
        End If

    End Sub

    Protected Sub Button2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button2.Click
        'Dim url As String = "default.aspx?SUP=" & Me.SUP.Value
        'Page.Server.Transfer(url)
        Response.Redirect("/" & Me.SUP.Value)
    End Sub
End Class
