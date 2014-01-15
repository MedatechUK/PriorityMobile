<%@ WebHandler Language="VB" Class="topmenu" %>

Imports cmSi

Public Class topmenu : Implements IHttpHandler
    
    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        Dim mnu As New cmSi.cmsMenu(context)
        With mnu
            .Add()
            .Add(New cmsMenuItem("~/ce1d8dcd-2011-4d6f-b8fc-f83948ecaf9e", "Products", ""))
            .Add(New cmsMenuItem("~/e0aadc83-7484-46a0-8730-397e4c609e79", "Special Offers", ""))
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