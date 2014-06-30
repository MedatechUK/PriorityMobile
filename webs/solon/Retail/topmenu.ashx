<%@ WebHandler Language="VB" Class="topmenu" %>

Imports cmSi

Public Class topmenu : Implements IHttpHandler
    
    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        Dim mnu As New cmSi.cmsMenu(context)
        With mnu
            .Add()
            .Add(New cmsMenuItem("~/products-by-category", "Products", ""))
            .Add(New cmsMenuItem("~/special-offers", "Special Offers", ""))
            .Add("//cat/cat[@name='topmenu']/cat")
            .ProcessRequest()
        End With
        
    End Sub
    
    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class