<%@ WebHandler Language="VB" Class="topmenu" %>

Imports cmSi

Public Class topmenu : Implements IHttpHandler
    
    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        Dim mnu As New cmSi.cmsMenu(context)
        With mnu
            .Add()
            .Add(New cmsMenuItem("~/ce1d8dcd-2011-4d6f-b8fc-f83948ecaf9e", "Products", ""))
            .Add(New cmsMenuItem("~/1531a949-b307-4a6a-90b2-2b49af2b413a", "Special Offers", ""))
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