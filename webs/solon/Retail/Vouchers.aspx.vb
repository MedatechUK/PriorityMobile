Imports cmSi

Partial Class Vouchers
    Inherits cmsInherit

    Protected Sub Page_Load1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.User.IsInRole("webmaster") Then
            Response.Redirect(cmSi.cmsData.Settings.Get("url") & "/login.aspx")
        End If

        Dim vgData As New System.Data.DataSet

        vgData.ReadXml(Server.MapPath("~/offers.xml"))
        VoucherGrid.DataSource = vgData.Tables(0).DefaultView
        VoucherGrid.DataBind()


    End Sub

    Protected Sub hEdit(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) _
    Handles VoucherGrid.RowEditing
        Response.Redirect(String.Format("voucherscreate.aspx?vcode={0}", sender.datakeys(e.NewEditIndex).value()))
    End Sub


    Protected Sub CreateVoucher_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CreateVoucher.Click
        Response.Redirect("/voucherscreate.aspx")
    End Sub


    Protected Sub hDelete(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) _
    Handles VoucherGrid.RowDeleting
        Response.Redirect(String.Format("voucherscreate.aspx?vcode={0}&delete=true", sender.datakeys(e.RowIndex).value()))
    End Sub

End Class

