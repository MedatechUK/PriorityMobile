Imports System.Xml
Imports System.Web.UI.WebControls
Imports System.Web.UI.HtmlControls

Friend Class repl_Basic : Inherits repl_Base

    Private lit As Literal

#Region "Control Definitions"

    Public Overrides ReadOnly Property ReplaceModule() As String
        Get
            Return "repl_Basic"
        End Get
    End Property

    Public Overrides ReadOnly Property Controls() As List(Of ReplaceControl)
        Get
            Dim ret As New List(Of ReplaceControl)
            With ret
                .Add(New ReplaceControl("System.Web.UI.WebControls.Image", "MainLogo", AddressOf hMainLogo))
                .Add(New ReplaceControl("System.Web.UI.WebControls.Literal", "WebName", AddressOf hWebName))
                .Add(New ReplaceControl("System.Web.UI.WebControls.Literal", "WebName2", AddressOf hWebName))
                .Add(New ReplaceControl("System.Web.UI.WebControls.Literal", "WebName3", AddressOf hWebName))
                .Add(New ReplaceControl("System.Web.UI.WebControls.Literal", "WebName4", AddressOf hWebName))
                .Add(New ReplaceControl("System.Web.UI.WebControls.Literal", "WebName5", AddressOf hWebName))
                .Add(New ReplaceControl("System.Web.UI.WebControls.Literal", "PageTitle", AddressOf hPageTitle))
                .Add(New ReplaceControl("System.Web.UI.WebControls.Literal", "PageTitle2", AddressOf hPageTitle))
                .Add(New ReplaceControl("System.Web.UI.WebControls.Literal", "PageTitle3", AddressOf hPageTitle))
                .Add(New ReplaceControl("System.Web.UI.WebControls.Literal", "PageTitle4", AddressOf hPageTitle))
                .Add(New ReplaceControl("System.Web.UI.WebControls.Literal", "PageTitle5", AddressOf hPageTitle))
                .Add(New ReplaceControl("System.Web.UI.WebControls.Literal", "BasketCount", AddressOf hBasketCount))
                .Add(New ReplaceControl("System.Web.UI.WebControls.Literal", "BasketCount2", AddressOf hBasketCount))
                .Add(New ReplaceControl("System.Web.UI.WebControls.Literal", "BasketCount3", AddressOf hBasketCount))
                .Add(New ReplaceControl("System.Web.UI.WebControls.Literal", "BasketCount4", AddressOf hBasketCount))
                .Add(New ReplaceControl("System.Web.UI.WebControls.Literal", "BasketCount5", AddressOf hBasketCount))
                .Add(New ReplaceControl("System.Web.UI.WebControls.LinkButton", "FormEmail", AddressOf hFormEmail))
                .Add(New ReplaceControl("System.Web.UI.WebControls.Button", "DefaultButton", AddressOf hDefaultButton))
            End With
            Return ret
        End Get
    End Property

#End Region

#Region "Delegate methods"

    Private Sub hDefaultButton(ByVal sender As Object, ByVal e As repl_Argument)
        Dim btn As System.Web.UI.WebControls.Button = sender
        Dim frm As htmlform = btn.Page.Master.FindControl("Form1")
        frm.DefaultButton = btn.UniqueID
    End Sub

    Private Sub hMainLogo(ByVal sender As Object, ByVal e As repl_Argument)
        Dim img As System.Web.UI.WebControls.Image = sender
        With img
            .AlternateText = cmsData.Settings("WebName")
            .ImageUrl = "my_documents/images/main-logo.png"
        End With
    End Sub

    Public Sub hWebName(ByVal sender As Object, ByVal e As repl_Argument)
        lit = sender
        lit.Text = cmsData.Settings("WebName")
    End Sub

    Public Sub hPageTitle(ByVal sender As Object, ByVal e As repl_Argument)
        lit = sender
        lit.Text = e.thisPage.Title
    End Sub

    Public Sub hBasketCount(ByVal sender As Object, ByVal e As repl_Argument)
        lit = sender
        lit.Text = ts.Basket.BasketItems.Count
    End Sub

    Public Sub hFormEmail(ByVal sender As Object, ByVal e As repl_Argument)
        Dim lnk As LinkButton = sender
        AddHandler lnk.Click, AddressOf FormEmail_click
    End Sub

#End Region

#Region "Event Handlers"

    Protected Sub FormEmail_click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim lnk As LinkButton = sender
        lnk.Page.Response.Redirect("formemail.aspx")
    End Sub


#End Region

End Class
