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
                .Add(New ReplaceControl("System.Web.UI.WebControls.CreateUserWizard", "CreateUserWizard1", AddressOf hRegister))
            End With
            Return ret
        End Get
    End Property

#End Region

#Region "Delegate methods"

    Private Sub hRegister(ByVal sender As Object, ByVal e As repl_Argument)
        Dim cu As System.Web.UI.WebControls.CreateUserWizard = sender
        Dim frm As HtmlForm = cu.Page.Master.FindControl("Form1")
        'frm.DefaultButton = "ctl00$Main$CreateUserWizard1$__CustomNav0$StepNextButtonButton"
        For Each ctrl As Control In cu.Controls
            RecurseCUW(frm, ctrl)
        Next
    End Sub

    Private Sub RecurseCUW(ByRef frm As HtmlForm, ByVal ctrl As Control)

        For Each c As Control In ctrl.Controls
            If Not IsNothing(TryCast(c, LinkButton)) Then
                Dim btn As LinkButton = c
                If InStr(btn.UniqueID, "StepNextButtonLinkButton") > 0 Then
                    frm.DefaultButton = c.UniqueID
                End If
            End If
            RecurseCUW(frm, c)
        Next

    End Sub

    Private Sub hDefaultButton(ByVal sender As Object, ByVal e As repl_Argument)
        Dim btn As System.Web.UI.WebControls.Button = sender
        Dim frm As HtmlForm = btn.Page.Master.FindControl("Form1")
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

        Dim str As New Text.StringBuilder
        str.AppendFormat("Email from {0}.", cmSi.cmsData.Settings.Get("WebName")).AppendLine()
        For Each k As String In lnk.Page.Request.Form.Keys
            If Not IsNothing(k) Then
                If Not (k.Substring(0, 2) = "__") _
                    And Not InStr(k, "$") > 0 Then
                    str.AppendFormat("{0}: {1}", k, lnk.Page.Request.Form(k)).AppendLine()
                End If
            End If
        Next
        str.AppendFormat("Sent: {0}", Now.ToString("dd/MM/yyy @hh:mm")).AppendLine()

        Dim smtp As New Net.Mail.SmtpClient
        For Each recipient As String In cmSi.cmsData.Settings.Get("RcptTO").Split(";")
            Dim mm As New Net.Mail.MailMessage(cmsData.Settings("MailFrom"), recipient)
            With mm
                .Subject = String.Format("Email from {0}.", cmSi.cmsData.Settings.Get("WebName"))
                .Body = str.ToString
                .IsBodyHtml = False
            End With
            smtp.Send(mm)
        Next

        Dim resultlabel As Label = lnk.Page.Master.FindControl("emailResult")
        If Not IsNothing(resultlabel) Then
            resultlabel.Text = "Your email was sent."
        End If

        'lnk.Page.Response.Redirect("formemail.aspx")
    End Sub


#End Region

End Class
