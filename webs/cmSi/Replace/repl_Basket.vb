Imports System.Xml
Imports System.Web.UI.WebControls

Public Class repl_Basket : Inherits repl_Base

#Region "Control Definitions"

    Public Overrides ReadOnly Property ReplaceModule() As String
        Get
            Return "repl_Basket"
        End Get
    End Property

    Public Overrides ReadOnly Property Controls() As List(Of ReplaceControl)
        Get
            Dim ret As New List(Of ReplaceControl)
            With ret
                .Add(New ReplaceControl("System.Web.UI.WebControls.Label", "lbllstCurrency", AddressOf hlbllstCurrency))
                .Add(New ReplaceControl("System.Web.UI.WebControls.DropDownList", "lstCurrency", AddressOf hlstCurrency))
                .Add(New ReplaceControl("System.Web.UI.WebControls.GridView", "BasketGrid", AddressOf hBasketGrid))
                .Add(New ReplaceControl("System.Web.UI.WebControls.LinkButton", "btn_card", AddressOf hbtn_card))
            End With
            Return ret
        End Get
    End Property

#End Region

#Region "Delegate methods"

    Public Sub hlbllstCurrency(ByVal sender As Object, ByVal e As repl_Argument)
        Dim lab As System.Web.UI.WebControls.Label = sender
        Dim nodes As XmlNodeList = xmlFunc.WebCurrencies(cmsData.part)
        With lab
            If IsNothing(nodes) Then
                .Visible = False
            Else
                If nodes.Count > 1 Then
                    .Visible = True
                Else
                    .Visible = False
                End If
            End If
        End With
    End Sub

    Public Sub hlstCurrency(ByVal sender As Object, ByVal e As repl_Argument)

        Dim ddl As System.Web.UI.WebControls.DropDownList = sender
        Dim nodes As XmlNodeList = xmlFunc.WebCurrencies(cmsData.part)

        AddHandler ddl.SelectedIndexChanged, AddressOf hCurrency_Changed
        With ddl
            If Not .Page.IsPostBack Then                
                If Not IsNothing(nodes) Then
                    For Each n As XmlNode In nodes
                        Dim li As New ListItem
                        With ts
                            li.Text = n.Attributes("CODE").Value
                            li.Value = n.Attributes("CODE").Value
                            If .Basket.CURRENCY = n.Attributes("CODE").Value Then
                                li.Selected = True
                            Else
                                li.Selected = False
                            End If
                            ddl.Items.Add(li)
                        End With
                    Next
                End If
            End If

            If IsNothing(nodes) Then
                .Visible = False
            Else
                If nodes.Count > 1 Then
                    .Visible = True
                Else
                    .Visible = False
                End If
            End If

        End With

    End Sub

    Public Sub hBasketGrid(ByVal sender As Object, ByVal e As repl_Argument)
        Dim BasketGrid As System.Web.UI.WebControls.GridView = sender
        With ts.Basket
            .AddHandlers(BasketGrid)
            'If Not BasketGrid.Page.IsPostBack Then
            .BindBasket(BasketGrid)
            'End If
        End With
    End Sub

    Public Sub hbtn_card(ByVal sender As Object, ByVal e As repl_Argument)
        Dim btn As System.Web.UI.WebControls.LinkButton = sender
        AddHandler btn.Click, AddressOf btn_card_click
    End Sub

#End Region

#Region "Event Handlers"

    Protected Sub btn_card_click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim btn As System.Web.UI.WebControls.LinkButton = sender
        btn.Page.Response.Redirect("checkout.aspx")
    End Sub

    Protected Sub hCurrency_Changed(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim ddl As System.Web.UI.WebControls.DropDownList = sender
        With ts
            .Basket.CURRENCY = sender.selecteditem.value
            .cart.CURRENCY = sender.selecteditem.value
            .Basket.UpdateDeliveryOptions()
            ddl.Page.Response.Redirect(.Info.ViewingPage)
        End With
    End Sub

#End Region

End Class
