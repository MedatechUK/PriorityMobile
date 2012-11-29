<%@ Page ContentType = "image/jpeg"%>
<%@ Import Namespace = "System.Drawing" %>
<%@ Import Namespace = "System.Drawing.Text" %>
<%@ Import Namespace = "System.Drawing.Imaging" %>
<%@ Import Namespace = "System.IO" %>
<%@ Import Namespace = "System.NET" %>

<Script Runat = "Server">
        
    Sub Page_Load()
        
        Dim xwid, ywid As Integer
        If Len(Request("xwid")) > 0 Then xwid = Request("xwid") Else xwid = 500
        If Len(Request("ywid")) > 0 Then ywid = Request("ywid") Else ywid = 300
        
        Dim oBitmap As Bitmap = New Bitmap(xwid, ywid)
        oBitmap.SetResolution(300, 300)
        
        Dim oGraphics As Graphics = Graphics.FromImage(oBitmap)

        Try
            
            If Len(Request("po")) > 0 Then
                    
                Dim barcode As String = "*" & Request("po") & "*"
                Dim Text As String = "PO#: " & UCase(Request("po"))
        
                Dim fs As Integer
                Dim oFont As System.Drawing.Font
                Dim oFont2 As System.Drawing.Font
            
                fs = 1
                Do
                    fs = fs + 1
                    oFont = New Font("3 of 9 Barcode", fs)
                Loop While oGraphics.MeasureString(barcode, oFont).Width < xwid
                oFont = New Font("3 of 9 Barcode", fs - 1)
                Dim bch As Integer = oGraphics.MeasureString(barcode, oFont).Height
                
                fs = 1
                Do
                    fs = fs + 1
                    oFont2 = New Font("Arial", fs)
                Loop While oGraphics.MeasureString(Text, oFont2).Width < xwid
                oFont2 = New Font("Arial", fs - 1)
                Dim txh As Integer = oGraphics.MeasureString(barcode, oFont).Height
            
                oGraphics.FillRectangle(Brushes.White, 0, 0, xwid, ywid)
                oGraphics.DrawRectangle(Pens.Black, 1, 1, xwid - 2, ywid - 2)
            
                oGraphics.DrawString(Text, oFont2, Brushes.Black, 0, (ywid / 2) - (txh + (txh / 5)))
                oGraphics.DrawString(barcode, oFont, Brushes.Black, 0, (ywid / 2))
                
                'ly = ly + oGraphics.MeasureString("Z", fnt).Height + 1
            End If
        
            oBitmap.Save(Response.OutputStream, ImageFormat.Jpeg)
            oBitmap.Dispose()
            oGraphics.Dispose()
            
        Catch er As Exception
            Console.Write("An error occured.")
        End Try
    End Sub
        
</Script>