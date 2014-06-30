Imports System.Xml
Imports System.Web.UI.WebControls
Imports System.Web

Public Class repl_ChildTable : Inherits repl_Base

#Region "Control Definitions"

    Public Overrides ReadOnly Property ReplaceModule() As String
        Get
            Return "repl_ChildTable"
        End Get
    End Property

    Public Overrides ReadOnly Property Controls() As List(Of ReplaceControl)
        Get
            Dim ret As New List(Of ReplaceControl)
            With ret
                .Add(New ReplaceControl("System.Web.UI.WebControls.Table", "partlist", AddressOf hChildTable))
                .Add(New ReplaceControl("System.Web.UI.WebControls.ListView", "ChildPart", AddressOf hChildPart))
                .Add(New ReplaceControl("System.Web.UI.WebControls.ListView", "ChildPage", AddressOf hChildPage))
                .Add(New ReplaceControl("System.Web.UI.WebControls.ListView", "ChildPage2", AddressOf hChildPage))
                .Add(New ReplaceControl("System.Web.UI.WebControls.ListView", "ChildPage3", AddressOf hChildPage))
                .Add(New ReplaceControl("System.Web.UI.WebControls.ListView", "ChildPage4", AddressOf hChildPage))
            End With
            Return ret
        End Get
    End Property

#End Region

#Region "Delegate methods"

    Public Sub hChildPart(ByVal sender As Object, ByVal e As repl_Argument)

        Dim v As System.Web.UI.WebControls.DataBoundControl = sender

        Dim ds As New System.Web.UI.WebControls.XmlDataSource
        With ds
            .ID = "ChildPartDataSource"
            .EnableCaching = False
        End With

        Dim str As New Text.StringBuilder
        With str

            .AppendLine("<childpart>")

            Dim NoNodes As Boolean = False
            Dim ChildNodes As XmlNodeList = e.thisCMSPage.thisCat.SelectNodes(".//*[@showonmenu='True']")
            If IsNothing(ChildNodes) Then NoNodes = True
            If ChildNodes.Count = 0 Then NoNodes = True

            If Not NoNodes Then

                For Each n As XmlNode In ChildNodes
                    Dim p As XmlNode = cmsData.doc.SelectSingleNode( _
                        String.Format( _
                            "//page[@id='{0}']", _
                            n.Attributes("id").Value _
                        ) _
                    )

                    If Not IsNothing(p) Then
                        If Not IsNothing(p.Attributes("part")) Then
                            If p.Attributes("part").Value.Length > 0 Then

                                Dim thisPart As XmlNode = cmsData.part.SelectSingleNode( _
                                        String.Format( _
                                            "//PART[PARTNAME='{0}']", _
                                            p.Attributes("part").Value _
                                        ) _
                                    )
                                If Not IsNothing(thisPart) Then

                                    Dim price As Double

                                    If IsAuthenticated Then
                                        price = QTYPrice( _
                                            PartCurrency( _
                                                thisPart, _
                                                ts.Basket.CURRENCY, _
                                                HttpContext.Current.Profile("CUSTNAME") _
                                            ), 1, _
                                            CBool(cmsData.Settings("ShowVAT")) _
                                        )
                                    Else
                                        price = QTYPrice( _
                                            PartCurrency( _
                                                thisPart, _
                                                ts.Basket.CURRENCY, _
                                                "" _
                                            ), 1, _
                                            CBool(cmsData.Settings("ShowVAT")) _
                                        )
                                    End If

                                    If Not IsNothing(thisPart) Then

                                        Dim Manufacturer As String = cmsData.Settings.Get("WebName")
                                        Dim ManufacturerURL As String = cmsData.Settings.Get("URL")
                                        Dim Model As String = thisPart.SelectSingleNode("PARTNAME").InnerText

                                        For Each spec As XmlNode In thisPart.SelectNodes("SPECS/SPEC")
                                            Select Case spec.Attributes("VALUE").InnerText.Trim.ToLower
                                                Case "manufacturer"
                                                    Manufacturer = spec.Attributes("VALUE").InnerText.Trim
                                                Case "manufacturerurl"
                                                    Manufacturer = spec.Attributes("VALUE").InnerText.Trim
                                                Case "model"
                                                    Model = spec.Attributes("VALUE").InnerText.Trim
                                            End Select
                                        Next

                                        Dim image As String = "my_documents/priimg/noimage.png"
                                        Try
                                            Dim IX As XmlNode = thisPart.SelectSingleNode("PRIIMG")
                                            If Not IsNothing(IX) Then
                                                If IX.InnerText.Length > 0 Then
                                                    image = "priimage.aspx?image=" & IX.InnerText
                                                Else
                                                    Throw New Exception("Missing Image")
                                                End If
                                            Else
                                                Throw New Exception("Missing Image")
                                            End If
                                        Catch
                                            If n.Attributes("img").Value.Length > 0 Then
                                                image = n.Attributes("img").Value
                                            End If
                                        End Try
                                        Dim trunc As Integer = 20
                                        If e.thisContext.Request.UserAgent.Contains("Safari") And Not e.thisContext.Request.UserAgent.Contains("Chrome") Then
                                            trunc -= 3
                                        End If

                                        .Append("   <childpart ")
                                        .AppendFormat("pagetitle='{0}' ", cmsCleanHTML.htmlEncode(cmsCleanHTML.FixedLen(p.Attributes("title").Value, trunc)))
                                        .AppendFormat("loc='{0}' ", p.Attributes("id").Value)
                                        .AppendFormat("image='{0}' ", image)
                                        .AppendFormat("description='{0}' ", cmsCleanHTML.htmlEncode(cmsCleanHTML.FixedLen(p.Attributes("description").Value, 50)))
                                        .AppendFormat("keywords='{0}' ", p.Attributes("keywords").Value)
                                        .AppendFormat("parentloc='{0}' ", e.thisCMSPage.thisCat.Attributes("id").Value)
                                        .AppendFormat("parenttitle='{0}' ", cmsCleanHTML.htmlEncode(e.thisCMSPage.thisCat.Attributes("name").Value))

                                        .AppendFormat("sku='{0}' ", thisPart.SelectSingleNode("PARTNAME").InnerText)
                                        .AppendFormat("curr='{0}' ", ts.Basket.CURRENCY)
                                        .AppendFormat("price='{0}' ", String.Format("{0:0.00}", price))
                                        .AppendFormat("partdes='{0}' ", cmsCleanHTML.htmlEncode(cmsCleanHTML.FixedLen(thisPart.SelectSingleNode("PARTDES").InnerText, 30)))
                                        .AppendFormat("ean13='{0}' ", thisPart.SelectSingleNode("BARCODE").InnerText)
                                        .AppendFormat("instock='{0}' ", thisPart.SelectSingleNode("AVAILABLE").InnerText)

                                        .AppendFormat("manufacturer='{0}' ", Manufacturer)
                                        .AppendFormat("manufacturerurl='{0}' ", ManufacturerURL)
                                        .AppendFormat("model='{0}' ", Model)

                                        .AppendFormat("lister='{0}' ", cmsData.Settings.Get("WebName"))
                                        .AppendFormat("listerurl='{0}' ", cmsData.Settings.Get("URL"))

                                        .Append("/>")
                                        .AppendLine()

                                    End If
                                End If
                            End If
                        End If
                    End If
                Next

            End If

            .AppendLine("</childpart>")
            ds.Data = .ToString

        End With

        Try
            With v
                .DataSource = ds
                .DataBind()
            End With
        Catch EX As Exception
            Throw New Exception( _
                String.Format( _
                    "Binding to XMLDataSource 'ChildPartDataSource' failed. {0}", _
                    EX.Message _
                ) _
            )
        End Try

    End Sub

    Public Sub hChildPage(ByVal sender As Object, ByVal e As repl_Argument)

        Dim v As System.Web.UI.WebControls.DataBoundControl = sender

        Dim ds As New System.Web.UI.WebControls.XmlDataSource
        With ds
            .ID = "ChildPageDataSource"
            .EnableCaching = False
        End With

        Dim str As New Text.StringBuilder
        With str

            .AppendLine("<children>")

            Dim NoNodes As Boolean = False
            Dim ChildNodes As XmlNodeList

            If IsNothing(v.Attributes("parentNode")) Then
                ChildNodes = e.thisCMSPage.thisCat.ChildNodes
            Else
                Dim thisCat As XmlNode = cmsData.cat.SelectSingleNode(String.Format("//cat[@id='{0}']", v.Attributes("parentNode")))
                ChildNodes = thisCat.ChildNodes
            End If

            If IsNothing(ChildNodes) Then NoNodes = True
            If ChildNodes.Count = 0 Then NoNodes = True

            If Not NoNodes Then

                For Each n As XmlNode In ChildNodes
                    If CBool(n.Attributes("showonmenu").Value) Then
                        Dim p As XmlNode = cmsData.doc.SelectSingleNode( _
                            String.Format( _
                                "//page[@id='{0}']", _
                                n.Attributes("id").Value _
                            ) _
                        )

                        If Not IsNothing(p) Then

                            Dim image As String = "my_documents/priimg/noimage.png"
                            If n.Attributes("img").Value.Length > 0 Then
                                image = n.Attributes("img").Value
                            End If

                            .Append("   <child ")
                            .AppendFormat("pagetitle='{0}' ", cmsCleanHTML.htmlEncode(p.Attributes("title").Value))
                            .AppendFormat("loc='{0}' ", p.Attributes("id").Value)
                            .AppendFormat("image='{0}' ", image)
                            .AppendFormat("description='{0}' ", cmsCleanHTML.htmlEncode(p.Attributes("description").Value))
                            .AppendFormat("keywords='{0}' ", p.Attributes("keywords").Value)
                            .Append("/>")
                            .AppendLine()

                        End If
                    End If
                Next

            End If

            .AppendLine("</children>")
            ds.Data = .ToString

        End With

        Try
            With v
                .DataSource = ds
                .DataBind()
            End With
        Catch EX As Exception
            Throw New Exception( _
                String.Format( _
                    "Binding to XMLDataSource 'ChildPageDataSource' failed. {0}", _
                    EX.Message _
                ) _
            )
        End Try

    End Sub

    Public Sub hChildTable(ByVal sender As Object, ByVal e As repl_Argument)

        Dim t As System.Web.UI.WebControls.Table = sender
        Dim image As String = ""
        Dim row As Integer = 0
        Dim col As Integer = 0
        Dim ChildNodes As XmlNodeList = e.thisCMSPage.thisCat.ChildNodes

        If IsNothing(ChildNodes) Then Exit Sub
        If ChildNodes.Count = 0 Then Exit Sub

        For Each n As XmlNode In ChildNodes

            Dim p As XmlNode = cmsData.doc.SelectSingleNode(String.Format("//page[@id='{0}']", n.Attributes("id").Value))
            If Not IsNothing(p) Then

                Dim haspart As Boolean = False
                If Not IsNothing(p.Attributes("part")) Then
                    If p.Attributes("part").Value.Length > 0 Then
                        haspart = True
                    End If
                End If

                If haspart Then
                    Try
                        Dim IX As XmlNode = cmsData.part.SelectSingleNode(String.Format("//PART[PARTNAME='{0}']/PRIIMG", p.Attributes("part").Value))
                        If Not IsNothing(IX) Then
                            Dim img As String = IX.InnerText
                            If img.Length > 0 Then
                                image = "<img src='" & "priimage.aspx?image=" & img & "' width='100' border='0'>"
                            Else
                                image = "<img src='" & "my_documents/priimg/noimage.png" & "' width='100' border='0'>"
                            End If
                        Else
                            image = "<img src='" & "my_documents/priimg/noimage.png" & "' width='100' border='0'>"
                        End If
                    Catch
                        image = "<img src='" & "my_documents/priimg/noimage.png" & "' width='100' border='0'>"
                    End Try
                Else
                    image = n.Attributes("img").Value
                    If image.Length > 0 Then
                        image = "<img src='" & image & "' width='100' border='0'>"
                    Else
                        image = "<img src='" & "my_documents/priimg/noimage.png" & "' width='100' border='0'>"
                    End If
                End If

                If col > t.Rows(0).Cells.Count - 1 Then
                    col = 0
                    row += 1
                    Dim r As New System.Web.UI.WebControls.TableRow
                    r.VerticalAlign = VerticalAlign.Top
                    For i As Integer = 0 To t.Rows(0).Cells.Count - 1
                        Dim ce As New TableCell
                        ce.VerticalAlign = VerticalAlign.Top
                        r.Cells.Add(ce)
                    Next
                    t.Rows.Add(r)
                End If

                Dim l As New Literal
                l.Text = String.Format("<table border='0' cellpadding='0' width='100%' cellspacing='10'><tr><a href='/{0}'><td align='center' class='partlist'><br><a href='/{0}'>{3}</a><br>{1}</td></a></tr><tr><td align='center'>{2}</td></tr></table>", _
                    p.Attributes("id").Value, _
                    p.Attributes("title").Value, _
                    "", _
                    image _
                )
                t.Rows(row).Cells(col).Controls.Add(l)
                col += 1

            End If

        Next

    End Sub

#End Region

End Class
