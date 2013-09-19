
Partial Class SoapService
    Inherits System.Web.UI.MasterPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        txtServerName.Text = "@&nbsp;" & Request.ServerVariables("HTTP_HOST")
        Dim mnu As Menu = Me.FindControl("menu1")
        If Not IsNothing(mnu) Then
            With mnu.Items
                .Clear()
                .Add(New MenuItem("General", "General", "", "default.aspx"))
                .Add(New MenuItem("Event Log", "Events", "", "events.aspx"))
                '.Add(New MenuItem("Console", "Console", "", "AsyncTest.aspx"))
                .Add(New MenuItem("Uptime", "Uptime", "", "Uptime.aspx"))
            End With
        End If

    End Sub

End Class

