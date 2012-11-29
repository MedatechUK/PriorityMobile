<%@ Page ContentType = "image/gif"%>
<%@ Import Namespace = "System.Drawing" %>
<%@ Import Namespace = "System.Drawing.Text" %>
<%@ Import Namespace = "System.Drawing.Imaging" %>
<%@ Import Namespace = "System.IO" %>
<%@ Import Namespace = "System.NET" %>
<%@ Import Namespace = "Priority" %>

<Script Runat = "Server"> 
    
    Dim SignaturePath As String = System.AppDomain.CurrentDomain.BaseDirectory.ToString() & "signatures\"
    
    Sub Page_Load()
        
        Dim objGraphics As Graphics
        Dim ar As New MyCls.MyArray
        Dim ThisArray(,) As String = Nothing
        Dim X1, X2, Y1, Y2
        
        Dim b As Bitmap = New System.Drawing.Bitmap(240, 101, PixelFormat.Format24bppRgb)
        objGraphics = Graphics.FromImage(b)
        objGraphics.Clear(Color.White)
        
        Try
            
            If ar.ArrayFromFile(ThisArray, SignaturePath & Request("sig")) Then
                                                                       
                For i As Integer = 1 To UBound(ThisArray, 2)

                
                    X1 = CInt(ThisArray(0, i - 1))
                    Y1 = CInt(ThisArray(1, i - 1))
                    X2 = CInt(ThisArray(0, i))
                    Y2 = CInt(ThisArray(1, i))

                    If Not (X1 = 0 And Y1 = 0) And Not (X2 = 0 And Y2 = 0) Then
                        objGraphics.DrawLine(Pens.Black, X1, Y1, X2, Y2)
                    End If

                Next
 
            End If           
            
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
        
        b.Save(Response.OutputStream, ImageFormat.Jpeg)
        b.Dispose()
        objGraphics.Dispose()
        
    End Sub
 
    
</Script>