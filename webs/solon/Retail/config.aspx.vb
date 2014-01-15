
Partial Class config
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.User.IsInRole("webmaster") Then
            Response.Redirect(cmSi.cmsData.Settings.Get("url") & "/login.aspx")
        End If

        Dim img As System.Web.UI.WebControls.Image = Master.FindControl("MainLogo")
        If Not IsNothing(img) Then
            With img
                .AlternateText = cmSi.cmsData.Settings("MainLogo")
                .ImageUrl = "my_documents/images/main-logo.png"
            End With
        End If

        Dim lit As System.Web.UI.WebControls.Literal = Master.FindControl("WebName")
        If Not IsNothing(lit) Then
            With lit
                .Text = cmSi.cmsData.Settings("WebName")
            End With
        End If

        For Each k As String In ConfigurationManager.AppSettings.Keys
            If Not k = "lastOrd" Then
                Dim r As New TableRow
                With r
                    Dim c1 As New TableCell
                    With c1
                        Dim l As New Label()
                        l.Text = k
                        l.Font.Name = "Verdana"
                        .Controls.Add(l)
                    End With
                    Dim c2 As New TableCell
                    With c2
                        Dim t As New TextBox
                        With t
                            .ID = k
                            .Width = 500
                            .Font.Name = "Verdana"
                            If Not Page.IsPostBack Then
                                .Text = ConfigurationManager.AppSettings.Item(k)
                            End If
                        End With
                        .Controls.Add(t)
                    End With
                    .Cells.Add(c1)
                    .Cells.Add(c2)
                End With
                tbl_citem.Rows.Add(r)
            End If
        Next


    End Sub

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim kv As New Dictionary(Of String, String)
        For Each k As String In ConfigurationManager.AppSettings.Keys
            If Not k = "lastOrd" Then
                Dim ph As ContentPlaceHolder = Master.FindControl("Main")
                Dim t As TextBox = ph.FindControl(k)
                kv.Add(k, t.Text)
            End If
        Next
        Dim config As System.Configuration.Configuration = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~")        

        For Each K As String In kv.Keys            
            config.AppSettings.Settings(K).Value = kv(K)
        Next
        config.Save()

    End Sub
End Class
