Imports System.Xml
Imports cmSi

Partial Class cmsImg
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        With Page.Controls

            Dim t As New Table

            For Each img As XmlNode In cmSi.cmsData.part.SelectNodes("BASKET/PARTS/PART/PRIIMG")
                If img.InnerText.Length > 0 Then
                    With t
                        .BorderWidth = 1
                        .Width = 500
                        .Rows.Add(New TableRow())
                        With .Rows(.Rows.Count - 1)
                            .Width = 300
                            .BorderWidth = 1
                            .Cells.Add(New TableCell)
                            With .Cells(.Cells.Count - 1)
                                .BorderWidth = 1
                                Dim iname As New Literal()
                                iname.Text = img.InnerText
                                .Controls.Add(iname)
                            End With
                            .Cells.Add(New TableCell)
                            With .Cells(.Cells.Count - 1)
                                .BorderWidth = 1
                                .Width = 200
                                Dim i As New Image()
                                i.ImageUrl = String.Format("http://windows-direct.ntsa.org.uk/priimage.aspx?image={0}", img.InnerText)
                                i.Width = 200
                                .Controls.Add(i)
                            End With
                        End With
                    End With
                End If
            Next
            .Add(t)
        End With
    End Sub

End Class
