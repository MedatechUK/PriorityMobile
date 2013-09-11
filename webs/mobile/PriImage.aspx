<%@ Page ContentType = "image/gif"%>
<%@ Import Namespace = "System.Drawing" %>
<%@ Import Namespace = "System.Drawing.Text" %>
<%@ Import Namespace = "System.Drawing.Imaging" %>
<%@ Import Namespace = "System.IO" %>
<%@ Import Namespace = "System.NET" %>
<%@ Import Namespace = "Priority" %>

<Script Runat = "Server"> 
    
    Sub Page_Load()
        
        
        Dim b As Bitmap = Nothing
        Dim g As Graphics = Nothing
        
        Try
            b = New System.Drawing.Bitmap(Request.Params("Image"))                        
        Catch ex As Exception
            b = New System.Drawing.Bitmap(1, 1, PixelFormat.Format24bppRgb)
        End Try

        Dim n As Bitmap = New System.Drawing.Bitmap(b.Width, b.Height, PixelFormat.Format24bppRgb)
        g = Graphics.FromImage(n)
        g.DrawImage(b, 0, 0, b.Width, b.Height)

        b.Dispose()
        g.Dispose()
        
        n.Save(Response.OutputStream, ImageFormat.Jpeg)
        n.Dispose()

        
    End Sub
 
    
</Script>