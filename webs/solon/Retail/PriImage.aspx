<%@ Page ContentType = "image/gif"%>
<%@ Import Namespace = "cmSi" %>

<Script Runat = "Server"> 
    
    Sub Page_Load()
        
        Using i As New cmSi.cmsPartImg()
            i.BitmapImage.Save(Response.OutputStream, i.SaveFormat)
        End Using
        
    End Sub
     
</Script>