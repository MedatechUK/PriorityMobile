Imports System.Xml
Imports System.Web.UI.WebControls

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
                .Add(New ReplaceControl("System.Web.UI.WebControls.Literal", "PageTitle", AddressOf hPageTitle))
                .Add(New ReplaceControl("System.Web.UI.WebControls.Literal", "BasketCount", AddressOf hBasketCount))
                .Add(New ReplaceControl("System.Web.UI.WebControls.Literal", "BasketCount2", AddressOf hBasketCount))
                .Add(New ReplaceControl("System.Web.UI.WebControls.Literal", "BasketCount3", AddressOf hBasketCount))
                .Add(New ReplaceControl("System.Web.UI.WebControls.Literal", "BasketCount4", AddressOf hBasketCount))
                .Add(New ReplaceControl("System.Web.UI.WebControls.Literal", "BasketCount5", AddressOf hBasketCount))
            End With
            Return ret
        End Get
    End Property

#End Region

#Region "Delegate methods"

    Public Sub hPageTitle(ByVal sender As Object, ByVal e As repl_Argument)
        lit = sender
        lit.Text = e.thisPage.Title
    End Sub

    Public Sub hBasketCount(ByVal sender As Object, ByVal e As repl_Argument)
        lit = sender
        lit.Text = ts.Basket.BasketItems.Count
    End Sub

#End Region

End Class
