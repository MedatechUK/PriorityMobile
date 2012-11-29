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
        'Dim objGraphics As Graphics = Nothing
        Dim img As String = Request.Params("Image")
        
        If Not IsNothing(img) Then
            If File.Exists(img) Then                
                Try
                    b = New System.Drawing.Bitmap(img)
                    'objGraphics = Graphics.FromImage(b)
                Catch ex As Exception
                    Response.Write(ex.Message)
                End Try
            End If
        End If
        
        If IsNothing(b) Then
            b = New System.Drawing.Bitmap(1, 1, PixelFormat.Format24bppRgb)
            'objGraphics = Graphics.FromImage(b)
            'objGraphics.Clear(Color.White)
        End If
                        
        b.Save(Response.OutputStream, ImageFormat.Jpeg)
        b.Dispose()
        'objGraphics.Dispose()
        
    End Sub
 
    
</Script>