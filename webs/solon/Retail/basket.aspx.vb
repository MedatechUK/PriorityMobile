Imports cmSi

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
    End Sub

End Class
