Imports cmSi
Imports System.Xml

Partial Class basket
    Inherits cmsInherit

    Public Overrides Sub LoadReplaceModules()
        addReplaceModule( _
            New repl_Basket _
        )
    End Sub

    Public Overrides Function AdminContext() As Boolean
        Return User.IsInRole("webmaster")
    End Function

    Private ReadOnly Property btn_card() As LinkButton
        Get
            Dim ph As ContentPlaceHolder = Master.FindControl("Main")
            Dim lv As LoginView = ph.FindControl("loginview")
            Return lv.FindControl("btn_card")
        End Get
    End Property

    Private ReadOnly Property btn_Invoice() As LinkButton
        Get
            Dim ph As ContentPlaceHolder = Master.FindControl("Main")
            Dim lv As LoginView = ph.FindControl("loginview")
            Return lv.FindControl("btn_invoice")
        End Get
    End Property

    Public Overrides Sub PageLoaded(ByVal sender As Object, ByVal e As System.EventArgs)
        If Not IsNothing(btn_Invoice) Then btn_Invoice.Visible = Profile.CUSTNAME.Length > 0
        ts.cart.PostGuid = Nothing
    End Sub

    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        If Not IsNothing(btn_Invoice) Then btn_Invoice.Enabled = CDbl(ts.cart.Total) > 0
        If Not IsNothing(btn_card) Then btn_card.Enabled = CDbl(ts.cart.Total) > 0
        With ConfigurationManager.AppSettings
            If Not IsNothing(btn_Invoice) Then btn_Invoice.Visible = CBool(.Get("PayByInvoice"))
            If Not IsNothing(btn_card) Then Me.btn_card.Visible = CBool(.Get("PayByCard"))
        End With
        If ts.Basket.vouchers.Count = 0 Then
            Master.FindControl("Main").FindControl("btnRmv").Visible = False

        End If
        If ts.Basket.vouchers.Count > 0 Then
            btnRmv.Visible = True
            btnVoucher.Visible = False
            txtVoucher.Visible = False
            lblEnter.Visible = False
        End If

        Dim r As ListView = Master.FindControl("Main").FindControl("lvVoucher")
        r.DataSource = ts.Basket.vouchers
        r.DataBind()
    End Sub

    Protected Sub btnVoucher_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnVoucher.Click
        Dim code As String = txtVoucher.Text
        Dim v As XmlNode = cmSi.cmsData.offers.SelectSingleNode(String.Format("//offer[@code = ""{0}""]", code))
        If ts.Basket.vouchers.Count > 0 Then
            lblVoucher.Text = "One voucher per order."
            lblVoucher.Visible = True
            Exit Sub
        End If
        If IsNothing(v) Then
            lblVoucher.Text = "Invalid voucher code."
            lblVoucher.Visible = True
            Exit Sub
        End If

        Dim args() As XmlNode = {v}
        Try
            Dim vr As Voucher = System.Activator.CreateInstance("cmSi", _
                                                               "cmSi." & v.Attributes("type").Value, _
                                                               True, _
                                                               Reflection.BindingFlags.Default, _
                                                               Nothing, _
                                                               args, _
                                                               Nothing, _
                                                               Nothing, _
                                                               Nothing).Unwrap
            If vr.Expiry < DateTime.Now Then
                lblVoucher.Visible = True
                lblVoucher.Text = "Voucher expired!"
            Else
                ts.Basket.vouchers.Add(vr)
                lblEnter.Visible = False
            End If

            Try
                ts.Basket.BindBasket(Master.FindControl("Main").FindControl("BasketGrid"))
            Catch ex As Exception
                ts.Basket.vouchers.Remove(vr)
                lblVoucher.Visible = True
                lblVoucher.Text = ex.Message
            End Try


        Catch ex As Exception
            lblVoucher.Visible = True
            lblVoucher.Text = ex.Message
        End Try
    End Sub

    Protected Sub btnRmv_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRmv.Click
        ts.Basket.vouchers.Clear()
        ts.Basket.BindBasket(Master.FindControl("Main").FindControl("BasketGrid"))
    End Sub
End Class
