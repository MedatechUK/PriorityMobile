Imports System.ComponentModel
Imports System.Text
Imports DTI.ImageMan.Barcode
Imports DTI.ImageMan.Barcode.DataMatrix.detector
Imports DTI.ImageMan.Twain
Imports PdfSharp
Imports PdfSharp.Drawing
Imports PdfSharp.Pdf
Imports System.IO
Imports System.Collections
Public Class Form1
    Private imgs As ArrayList
    Private decoder As New DTI.ImageMan.Barcode.BarcodeDecoder
    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub
    Private Sub process()
        Dim img As Image
        Dim count As Integer = 0
        Dim imgind As Integer = 0
        Dim start As Integer = 0





        Dim bformats As New List(Of BarcodeFormat)
        bformats.Add(BarcodeFormat.Code39)
        Dim decodedBarcode As Result
        Dim doc As New PdfDocument
        doc.Pages.Add(New PdfPage)
        For Each img In imgs
            imgind = imgs.IndexOf(img)

            Try

                decodedBarcode = Nothing
                decodedBarcode = decoder.Decode(img, bformats)
                'End If

                If decodedBarcode IsNot Nothing Then
                    ListBox1.Items.Add("Page " & count & " Has Been checked and contains a barcode - " & decodedBarcode.BarcodeFormat.ToString())
                    ListBox1.Update()

                    If imgind <> 0 Then



                        'If File.Exists("c:\test" & decodedBarcode.BarcodeFormat.ToString() & ".jpg") Then
                        '    File.Delete("c:\test" & decodedBarcode.BarcodeFormat.ToString() & ".jpg")
                        'End If
                        'imgs(i).Save("c:\test" & decodedBarcode.BarcodeFormat.ToString() & ".jpg")



                        doc.Save("c:\test" & decodedBarcode.BarcodeFormat.ToString() & imgind & ".pdf")
                        doc = Nothing
                        doc = New PdfDocument
                        doc.Pages.Add(New PdfPage)
                        start = imgind + 1
                        count = 0



                    End If
                    doc.AddPage()
                    Dim xgr As XGraphics
                    xgr = XGraphics.FromPdfPage(doc.Pages(count))
                    Dim imgx As XImage
                    imgx = XImage.FromGdiPlusImage(img)

                    xgr.DrawImage(imgx, 0, 0)
                End If

            Catch e1 As DTI.ImageMan.Barcode.NotFoundException
                ListBox1.Items.Add("Page " & imgind & " Has Been checked but contains no barcode.")
                ListBox1.Update()
                doc.AddPage()
                Dim xgr As XGraphics
                xgr = XGraphics.FromPdfPage(doc.Pages(count))
                Dim imgx As XImage
                imgx = XImage.FromGdiPlusImage(img)

                xgr.DrawImage(imgx, 0, 0)
            Catch ex As Exception
                TextBox1.Text = ex.Message
            End Try
            count += 1

        Next
        doc.Save("c:\testfinal" & imgind & ".pdf")
        doc = Nothing
        ListBox1.Items.Add("----------------- Done ------------------")
    End Sub
End Class
