Imports System.Web.UI.WebControls
Imports System.Web
Imports System.Xml

Public Class repl_Part
    Inherits repl_Base

#Region "Control Definitions"

    Public Overrides ReadOnly Property ReplaceModule() As String
        Get
            Return "repl_Part"
        End Get
    End Property

    Public Overrides ReadOnly Property Controls() As System.Collections.Generic.List(Of ReplaceControl)
        Get
            Dim ret As New List(Of ReplaceControl)
            With ret

                .Add(New ReplaceControl("System.Web.UI.WebControls.Label", "partname", AddressOf hpartname))
                .Add(New ReplaceControl("System.Web.UI.WebControls.Label", "partdes", AddressOf hpartdes))
                .Add(New ReplaceControl("System.Web.UI.WebControls.Label", "available", AddressOf hAvailable))
                .Add(New ReplaceControl("System.Web.UI.WebControls.Label", "currency", AddressOf hcurrency))
                .Add(New ReplaceControl("System.Web.UI.WebControls.Label", "unitprice", AddressOf hunitprice))
                .Add(New ReplaceControl("System.Web.UI.WebControls.Label", "unitprice_special", AddressOf hWasPrice))

                .Add(New ReplaceControl("System.Web.UI.WebControls.Image", "PriImage", AddressOf hPriImage))
                .Add(New ReplaceControl("System.Web.UI.WebControls.Panel", "InBasket", AddressOf hInBasket))

                .Add(New ReplaceControl("System.Web.UI.WebControls.Literal", "PARTREMARK", AddressOf hPARTREMARK))
                .Add(New ReplaceControl("System.Web.UI.WebControls.Literal", "sku", AddressOf hSKU))
                .Add(New ReplaceControl("System.Web.UI.WebControls.Literal", "Rating", AddressOf hRating))
                .Add(New ReplaceControl("System.Web.UI.WebControls.Literal", "Review", AddressOf hReview))
                .Add(New ReplaceControl("System.Web.UI.WebControls.Literal", "Model", AddressOf hModel))
                .Add(New ReplaceControl("System.Web.UI.WebControls.Literal", "Manufacturer", AddressOf hManufacturer))
                .Add(New ReplaceControl("System.Web.UI.WebControls.Literal", "ManufacturerURL", AddressOf hManufacturerURL))

                .Add(New ReplaceControl("System.Web.UI.WebControls.LinkButton", "lAddToBasket", AddressOf hladdtobasket))
                .Add(New ReplaceControl("System.Web.UI.WebControls.LinkButton", "lViewPDF", AddressOf hlViewPDF))
                .Add(New ReplaceControl("System.Web.UI.WebControls.LinkButton", "lBuyNow", AddressOf hlBuyNow))

                .Add(New ReplaceControl("System.Web.UI.WebControls.ListView", "SiblingPart", AddressOf hSiblingPart))
                .Add(New ReplaceControl("System.Web.UI.WebControls.ListView", "PartSpec", AddressOf hPartSpec))

            End With
            Return ret
        End Get
    End Property

#End Region

#Region "Private Methods"

    Private Function Plural(ByVal number As Integer) As String
        If number <> 1 Then
            Return "es"
        Else
            Return ""
        End If
    End Function

    Private Sub SpecValue(ByRef p As XmlNode, ByRef l As Literal, ByVal SpecName As String, ByVal DefaultValue As String)

        l.Text = DefaultValue
        If Not IsNothing(p) Then
            If Not IsNothing(p.Attributes("part")) Then
                If p.Attributes("part").Value.Length > 0 Then

                    Dim spec As XmlNode = cmsData.part.SelectSingleNode( _
                            String.Format( _
                                "//PART[PARTNAME='{0}']/SPECS/SPEC[@DES='{1}']", _
                                p.Attributes("part").Value, _
                                SpecName _
                            ) _
                        )
                    If Not IsNothing(spec) Then                        
                        l.Text = spec.Value.ToString
                    End If
                End If

            End If

        End If

    End Sub

    Private Function SpecString(ByRef p As XmlNode, ByVal SpecName As String, ByVal DefaultValue As String)

        Dim ret As String = DefaultValue
        If Not IsNothing(p) Then
            If Not IsNothing(p.Attributes("part")) Then
                If p.Attributes("part").Value.Length > 0 Then

                    Dim spec As XmlNode = cmsData.part.SelectSingleNode( _
                            String.Format( _
                                "//PART[PARTNAME='{0}']/SPECS/SPEC[@DES='{1}']", _
                                p.Attributes("part").Value, _
                                SpecName _
                            ) _
                        )
                    If Not IsNothing(spec) Then
                        ret = spec.Value.ToString
                    End If
                End If
            End If
        End If
        Return ret

    End Function

    Private Function RandomNum(ByVal count As Integer) As List(Of Integer)

        Dim objRandom As New System.Random(CType(System.DateTime.Now.Ticks Mod System.Int32.MaxValue, Integer))
        Dim unRnd As New List(Of Integer)
        Dim Rnd As New List(Of Integer)
        Dim nextRnd As Integer

        For i As Integer = 0 To count - 1
            unRnd.Add(i)
        Next

        Do
            nextRnd = objRandom.Next(unRnd.Count - 1)
            Rnd.Add(unRnd(nextRnd))
            unRnd.Remove(unRnd(nextRnd))
        Loop While Not unRnd.Count = 0

        Return Rnd

    End Function

#End Region

#Region "Delegate Methods"

#Region "LinkButtons"

    Public Sub hladdtobasket(ByVal sender As Object, ByVal e As repl_Argument)
        Dim lnk As System.Web.UI.WebControls.LinkButton = sender
        AddHandler lnk.Click, AddressOf laddtobasket
    End Sub

    Public Sub hlViewPDF(ByVal sender As Object, ByVal e As repl_Argument)
        Dim lnk As System.Web.UI.WebControls.LinkButton = sender
        lnk.Enabled = False
        If Not IsNothing(e.thisCMSPage.PageNode.Attributes("part")) Then
            lnk.Enabled = System.IO.File.Exists( _
                lnk.Page.Server.MapPath( _
                    String.Format( _
                        "/my_documents/pdfs/{0}.pdf", _
                        e.thisCMSPage.PageNode.Attributes("part").Value _
                    ) _
                ) _
            )
            AddHandler lnk.Click, AddressOf lViewPDF
        End If
    End Sub

    Public Sub hlBuyNow(ByVal sender As Object, ByVal e As repl_Argument)
        Dim lnk As System.Web.UI.WebControls.LinkButton = sender
        AddHandler lnk.Click, AddressOf lBuyNow
    End Sub

#End Region

    Public Sub htbl_spec(ByVal sender As Object, ByVal e As repl_Argument)

        Dim t As System.Web.UI.WebControls.Table = sender
        Dim x As Integer = 0

        With t
            .Rows.Add(New TableRow)
            .Rows(.Rows.Count - 1).Cells.Add(New TableCell)
            .Rows(.Rows.Count - 1).Cells.Add(New TableCell)
            .Rows(.Rows.Count - 1).Cells.Add(New TableCell)
            .Rows(.Rows.Count - 1).Cells.Add(New TableCell)

            For Each spec As XmlNode In e.thisCMSPage.Part.SelectNodes("SPECS/SPEC")
                If spec.Attributes("VALUE").InnerText.Trim.Length > 0 Then

                    Dim lbl As New Label
                    With lbl
                        .Text = spec.Attributes("DES").InnerText
                        .Font.Bold = True
                    End With
                    .Rows(t.Rows.Count - 1).Cells(x).Controls.Add(lbl)
                    Dim val As New Label
                    With val
                        .Text = spec.Attributes("VALUE").InnerText
                        .Font.Bold = False
                    End With
                    .Rows(t.Rows.Count - 1).Cells(x + 1).Controls.Add(val)

                    x += 2
                    If x > 3 Then
                        .Rows.Add(New TableRow)
                        .Rows(.Rows.Count - 1).Cells.Add(New TableCell)
                        .Rows(.Rows.Count - 1).Cells.Add(New TableCell)
                        .Rows(.Rows.Count - 1).Cells.Add(New TableCell)
                        .Rows(.Rows.Count - 1).Cells.Add(New TableCell)
                        x = 0
                    End If

                End If
            Next
        End With

    End Sub

    Public Sub hpartname(ByVal sender As Object, ByVal e As repl_Argument)
        Dim lab As Label = sender
        With lab
            .Text = e.thisCMSPage.Part.SelectSingleNode("PARTNAME").InnerText
        End With
    End Sub

    Public Sub hpartdes(ByVal sender As Object, ByVal e As repl_Argument)
        Dim lab As Label = sender
        With lab
            .Text = e.thisCMSPage.Part.SelectSingleNode("PARTDES").InnerText
        End With
    End Sub

    Public Sub hAvailable(ByVal sender As Object, ByVal e As repl_Argument)
        Dim lab As Label = sender
        With lab
            .Text = e.thisCMSPage.Part.SelectSingleNode("AVAILABLE").InnerText
        End With
    End Sub

    Public Sub hcurrency(ByVal sender As Object, ByVal e As repl_Argument)
        Dim lab As Label = sender
        With lab
            .Text = ts.Basket.CURRENCY
        End With
    End Sub

    Public Sub hWasPrice(ByVal sender As Object, ByVal e As repl_Argument)
        Dim lab As Label = sender
        With lab
            With e.thisPage
                Dim spec As XmlNode = e.thisCMSPage.Part.SelectSingleNode("SPECS/SPEC/wasprice")
                If Not IsNothing(spec) Then
                    If spec.Attributes("VALUE").InnerText.Length > 0 Then
                        lab.Text = e.thisCMSPage.Part.SelectSingleNode("SPECS/SPEC/wasprice").InnerText
                    Else
                        lab.Text = "99.99"
                    End If
                Else
                    lab.Text = "99.99"
                End If
            End With
        End With
    End Sub

    Public Sub hunitprice(ByVal sender As Object, ByVal e As repl_Argument)
        Dim lab As Label = sender
        With lab
            With e.thisPage

                Dim cur As XmlNode
                If IsAuthenticated Then
                    cur = xmlFunc.PartCurrency(e.thisCMSPage.Part, ts.Basket.CURRENCY, HttpContext.Current.Profile("CUSTNAME"))
                Else
                    cur = xmlFunc.PartCurrency(e.thisCMSPage.Part, ts.Basket.CURRENCY, "")
                End If

                If Not IsNothing(cur) Then

                    Dim quant As Integer
                    Dim q As TextBox = .Master.FindControl("basketqty")
                    Dim l As DropDownList = .Master.FindControl("basketlist")
                    l.EnableViewState = False
                    Dim bx As DropDownList = .Master.FindControl("lstBoxCount")
                    bx.EnableViewState = False

                    If cur.SelectNodes("BREAK").Count = 1 Then
                        If e.thisCMSPage.BoxCount = -1 Then
                            quant = CInt(q.Text)
                            q.Visible = True
                            l.Visible = False
                            bx.Visible = False
                        Else
                            q.Visible = False
                            l.Visible = False
                            bx.Visible = True
                            Dim sel As Boolean = False
                            With bx
                                .ClearSelection()
                                .Items.Clear()
                                For bq As Integer = 1 To 10
                                    .Items.Add(New ListItem(String.Format("{0} box{2} ({1})", bq.ToString, bq * e.thisCMSPage.BoxCount.ToString, Plural(CInt(bq.ToString))), bq * e.thisCMSPage.BoxCount()))
                                    .Items(.Items.Count - 1).Selected = False
                                    If lab.Page.IsPostBack Then
                                        Try
                                            bx.Items(bx.Items.Count - 1).Selected = CBool(CInt(bq * e.thisCMSPage.BoxCount()) = CInt(lab.Page.Request("ctl00$lstBoxCount")))
                                            If bx.Items(bx.Items.Count - 1).Selected Then sel = True
                                        Catch
                                        End Try
                                    End If
                                Next
                                If Not sel Then
                                    .Items(0).Selected = True
                                End If
                            End With
                        End If
                    Else
                        q.Visible = False
                        l.Visible = True
                        bx.Visible = False

                        For Each b As XmlNode In cur.SelectNodes("BREAK")
                            l.Items.Add(New ListItem(String.Format("{0}: {1} {2}", _
                                 b.Attributes("QTY").Value, _
                                 cur.Attributes("CURSTR").Value, _
                                 String.Format("{0:f2}", CDbl(b.Attributes("PRICE").Value))), _
                                 b.Attributes("QTY").Value))

                            If .IsPostBack Then
                                l.Items(l.Items.Count - 1).Selected = CBool(CInt(b.Attributes("QTY").Value) = CInt(.Request("ctl00$basketlist")))
                            Else
                                lab.Text = String.Format("{0:f2}", CDbl(xmlFunc.QTYPrice(cur, 1)))
                            End If
                        Next
                        quant = CInt(l.SelectedValue)
                    End If

                    If Not IsNothing(q) Then
                        If q.Visible Then
                            lab.Text = String.Format("{0:f2}", CDbl(xmlFunc.QTYPrice(cur, CInt(q.Text))))
                        ElseIf l.Visible Then
                            lab.Text = String.Format("{0:f2}", CDbl(xmlFunc.QTYPrice(cur, CInt(l.SelectedValue))))
                        ElseIf bx.Visible Then
                            lab.Text = String.Format("{0:f2}", CDbl(xmlFunc.QTYPrice(cur, CInt(bx.SelectedValue))) * e.thisCMSPage.BoxCount())
                        End If
                    End If
                End If

            End With
        End With

    End Sub

    Public Sub haddtobasket(ByVal sender As Object, ByVal e As repl_Argument)
        Dim btn As Button = sender
        AddHandler btn.Click, AddressOf AddToBasket
    End Sub

    Public Sub hPriImage(ByVal sender As Object, ByVal e As repl_Argument)
        Dim i As System.Web.UI.WebControls.Image = sender
        If i.ID = "PriImage" Then
            Try
                Dim ix As XmlNode = e.thisCMSPage.Part.SelectSingleNode("PRIIMG")
                If Not IsNothing(ix) Then
                    Dim img As String = ix.InnerText
                    If img.Length > 0 Then
                        i.ImageUrl = "priimage.aspx?image=" & img
                    Else
                        i.ImageUrl = "my_documents/priimg/noimage.png"
                    End If
                Else
                    i.ImageUrl = "my_documents/priimg/noimage.png"
                End If
            Catch
                i.ImageUrl = "my_documents/priimg/noimage.png"
            End Try
        End If
    End Sub

#Region "String Lierals"

    Public Sub hInBasket(ByVal sender As Object, ByVal e As repl_Argument)
        Dim lit As System.Web.UI.WebControls.Panel = sender
        lit.Visible = False
        Dim thisPart As String = e.thisCMSPage.PageNode.Attributes("part").Value
        If Not IsNothing(thisPart) Then
            If ts.Basket.BasketItems.Keys.Contains(thisPart) Then
                lit.Visible = True
            End If
        End If
    End Sub

    Public Sub hPARTREMARK(ByVal sender As Object, ByVal e As repl_Argument)
        Dim l As System.Web.UI.WebControls.Literal = sender
        Dim ix As XmlNode = e.thisCMSPage.Part.SelectSingleNode("PARTREMARK")
        If Not IsNothing(ix) Then
            l.Text = cmsCleanHTML.Clean(ix.InnerText)
        Else
            l.Text = "Please enter some text in the Prioprity Part Remark sub-form."
        End If
    End Sub

    Public Sub hRating(ByVal sender As Object, ByVal e As repl_Argument)
        Dim lab As Literal = sender

        Dim Rating As String = SpecString(e.thisCMSPage.PageNode, "rating", "3")
        Dim Str As New System.Text.StringBuilder
        Str.AppendFormat("<meta itemprop='value' content='{0}'>", Rating)
        Str.AppendFormat("<meta itemprop='best' content='10'>")
        Str.AppendFormat("<meter value='{0}' min='0' max='10'>{0} / 10</meter>", Rating)

        lab.Text = Str.ToString
    End Sub

    Public Sub hReview(ByVal sender As Object, ByVal e As repl_Argument)
        Dim lab As Literal = sender

        Dim Review As String = SpecString(e.thisCMSPage.PageNode, "review", "0")
        Dim reviewCount As String = SpecString(e.thisCMSPage.PageNode, "reviewcount", "0")

        Dim Str As New System.Text.StringBuilder
        Str.AppendFormat("<meta itemprop='rating' content='{0}'>", Review)
        Str.AppendFormat("<meter value='{0}' min='0' max='5'>Based on <span itemprop='count'>{1}</span> reviews.</meter>", Review, reviewCount)

        lab.Text = Str.ToString
    End Sub

    Public Sub hModel(ByVal sender As Object, ByVal e As repl_Argument)
        Dim lab As Literal = sender
        SpecValue(e.thisCMSPage.PageNode, sender, "reviewcount", e.thisCMSPage.Part.SelectSingleNode("PARTNAME").InnerText)
    End Sub

    Public Sub hSKU(ByVal sender As Object, ByVal e As repl_Argument)
        Dim lab As Literal = sender
        With lab
            .Text = e.thisCMSPage.Part.SelectSingleNode("PARTNAME").InnerText
        End With
    End Sub

    Public Sub hManufacturer(ByVal sender As Object, ByVal e As repl_Argument)
        SpecValue(e.thisCMSPage.PageNode, sender, "manufacturer", "Unbranded")
    End Sub

    Public Sub hManufacturerURL(ByVal sender As Object, ByVal e As repl_Argument)
        SpecValue(e.thisCMSPage.PageNode, sender, "manufacturerurl", cmsData.Settings.Get("URL"))
    End Sub

#End Region

#Region "List Views"

    Public Sub hSiblingPart(ByVal sender As Object, ByVal e As repl_Argument)

        Dim c As Integer = 0
        Dim v As System.Web.UI.WebControls.DataBoundControl = sender

        Dim ds As New System.Web.UI.WebControls.XmlDataSource
        With ds
            .ID = "SiblingPartDataSource"
            .EnableCaching = False
        End With

        Dim str As New Text.StringBuilder
        With str

            .AppendLine("<SiblingPart>")

            Dim NoNodes As Boolean = False
            Dim ChildNodes As XmlNodeList = e.thisCMSPage.thisCat.ParentNode.ChildNodes

            If IsNothing(ChildNodes) Then NoNodes = True
            If ChildNodes.Count = 0 Then NoNodes = True

            If Not NoNodes Then

                Dim rand As List(Of Integer) = RandomNum(ChildNodes.Count)

                For Each r As Integer In rand
                    Dim n As XmlNode = ChildNodes(r)
                    Dim p As XmlNode = cmsData.doc.SelectSingleNode( _
                        String.Format( _
                            "//page[@id='{0}']", _
                            n.Attributes("id").Value _
                        ) _
                    )
                    If Not IsNothing(p) Then
                        If Not String.Compare(e.thisCMSPage.PageNode.Attributes("id").Value, p.Attributes("id").Value) = 0 Then
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
                                                    ), 1 _
                                                )
                                            Else
                                                price = QTYPrice( _
                                                    PartCurrency( _
                                                        thisPart, _
                                                        ts.Basket.CURRENCY, _
                                                        "" _
                                                    ), 1 _
                                                )
                                            End If

                                            If Not IsNothing(thisPart) Then

                                                Dim Manufacturer As String = "Unbranded"
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

                                                .Append("   <childpart ")
                                                .AppendFormat("pagetitle='{0}' ", cmsCleanHTML.htmlEncode(p.Attributes("title").Value))
                                                .AppendFormat("loc='{0}' ", p.Attributes("id").Value)
                                                .AppendFormat("image='{0}' ", image)
                                                .AppendFormat("description='{0}' ", cmsCleanHTML.htmlEncode(p.Attributes("description").Value))
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

                                                c += 1
                                                If c >= 4 Then Exit For

                                            End If
                                        End If
                                    End If
                                End If
                            End If
                        End If
                    End If
                Next

            End If

            .AppendLine("</SiblingPart>")
            ds.Data = .ToString

        End With

        Try
            With v
                .DataSource = ds
                .EnableViewState = False
                .DataBind()
            End With
        Catch EX As Exception
            Throw New Exception( _
                String.Format( _
                    "Binding to XMLDataSource '{0}' failed. {1}", _
                    ds.ID, _
                    EX.Message _
                ) _
            )
        End Try

    End Sub

    Public Sub hPartSpec(ByVal sender As Object, ByVal e As repl_Argument)

        Dim v As System.Web.UI.WebControls.DataBoundControl = sender

        Dim ds As New System.Web.UI.WebControls.XmlDataSource
        With ds
            .ID = "PartSpecDataSource"
            .EnableCaching = False
        End With

        Dim p As XmlNode = e.thisCMSPage.PageNode
        Dim str As New Text.StringBuilder
        With str
            .AppendLine("<partspecs>")
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

                            Dim Manufacturer As String = "Unbranded"
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

                            .AppendFormat( _
                                "   <spec name='{0}' value='{1}' />", _
                                "Model", _
                                Model _
                            ).AppendLine()

                            .AppendFormat( _
                                "   <spec name='{0}' value='{1}' />", _
                                "Manufacturer", _
                                Manufacturer _
                            ).AppendLine()

                            .AppendFormat( _
                                "   <spec name='{0}' value='{1}' />", _
                                "Barcode", _
                                thisPart.SelectSingleNode("BARCODE").InnerText _
                            ).AppendLine()

                            For Each spec As XmlNode In thisPart.SelectNodes("SPECS/SPEC")
                                If spec.Attributes("VALUE").InnerText.Trim.Length > 0 Then
                                    Select Case spec.Attributes("VALUE").InnerText.Trim.ToLower
                                        Case "manufacturer", "manufacturerurl", "model"
                                        Case Else
                                            .AppendFormat( _
                                                "   <spec name='{0}' value='{1}' />", _
                                                spec.Attributes("DES").InnerText, _
                                                spec.Attributes("VALUE").InnerText _
                                            ).AppendLine()
                                    End Select
                                End If
                            Next

                            .AppendFormat( _
                                "   <spec name='{0}' value='{1}' />", _
                                "Stock", _
                                thisPart.SelectSingleNode("AVAILABLE").InnerText _
                            ).AppendLine()

                        End If
                    End If
                End If
            End If

            .AppendLine("</partspecs>")
            ds.Data = .ToString.Replace("'", Chr(34))

        End With

        Try
            With v
                .DataSource = ds
                .DataBind()
                .EnableViewState = False
            End With
        Catch EX As Exception
            Throw New Exception( _
                String.Format( _
                    "Binding to XMLDataSource '{0}' failed. {1}", _
                    ds.ID, _
                    EX.Message _
                ) _
            )
        End Try

    End Sub

#End Region

#End Region

#Region "Event Handlers"

    Public Sub laddtobasket(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim lnk As System.Web.UI.WebControls.LinkButton = sender
        With lnk.Page
            Dim q As TextBox = .Master.FindControl("basketqty")
            Dim l As DropDownList = .Master.FindControl("basketlist")
            Dim bx As DropDownList = .Master.FindControl("lstBoxCount")
            Dim P As cmsPage = New cmsPage(.Page, HttpContext.Current, .Page.Server)

            If Not IsNothing(q) Then
                With ts.Basket
                    If q.Visible Then
                        .AddBasketItem(New BasketItem(P.PageNode.Attributes("part").Value, CInt(q.Text), "/" & P.PageNode.Attributes("id").Value))
                    ElseIf l.Visible Then
                        .AddBasketItem(New BasketItem(P.PageNode.Attributes("part").Value, CInt(l.SelectedValue), "/" & P.PageNode.Attributes("id").Value))
                    ElseIf bx.Visible Then
                        .AddBasketItem(New BasketItem(P.PageNode.Attributes("part").Value, CInt(bx.SelectedValue), "/" & P.PageNode.Attributes("id").Value, P.PageNode.Attributes("boxcount").Value))
                    End If

                    Dim BasketCount As System.Web.UI.WebControls.Literal = lnk.Page.Master.FindControl("BasketCount")
                    If Not IsNothing(BasketCount) Then BasketCount.Text = ts.Basket.BasketItems.Count
                    Dim BasketCount2 As System.Web.UI.WebControls.Literal = lnk.Page.Master.FindControl("BasketCount2")
                    If Not IsNothing(BasketCount2) Then BasketCount2.Text = ts.Basket.BasketItems.Count
                    Dim BasketCount3 As System.Web.UI.WebControls.Literal = lnk.Page.Master.FindControl("BasketCount3")
                    If Not IsNothing(BasketCount3) Then BasketCount3.Text = ts.Basket.BasketItems.Count
                    Dim BasketCount4 As System.Web.UI.WebControls.Literal = lnk.Page.Master.FindControl("BasketCount4")
                    If Not IsNothing(BasketCount4) Then BasketCount4.Text = ts.Basket.BasketItems.Count
                    Dim BasketCount5 As System.Web.UI.WebControls.Literal = lnk.Page.Master.FindControl("BasketCount5")
                    If Not IsNothing(BasketCount5) Then BasketCount5.Text = ts.Basket.BasketItems.Count

                    Dim InBasket As System.Web.UI.WebControls.Panel = lnk.Page.Master.FindControl("InBasket")
                    If Not IsNothing(InBasket) Then
                        hInBasket(InBasket, New repl_Argument(P, lnk.Page, HttpContext.Current, lnk.Page.Server))
                    End If
                End With
            End If
        End With

    End Sub

    Public Sub lViewPDF(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim lnk As System.Web.UI.WebControls.LinkButton = sender
        Dim P As cmsPage = New cmsPage(lnk.Page, HttpContext.Current, lnk.Page.Server)
        lnk.Page.Response.Redirect( _
            String.Format( _
                "~/my_documents/pdfs/{0}.pdf", _
                P.PageNode.Attributes("part").Value _
            ) _
        )
    End Sub

    Public Sub lBuyNow(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim lnk As System.Web.UI.WebControls.LinkButton = sender
        With lnk.Page
            Dim q As TextBox = .Master.FindControl("basketqty")
            Dim l As DropDownList = .Master.FindControl("basketlist")
            Dim bx As DropDownList = .Master.FindControl("lstBoxCount")
            Dim P As cmsPage = New cmsPage(.Page, HttpContext.Current, .Page.Server)

            If Not IsNothing(q) Then
                With ts.Basket
                    If q.Visible Then
                        .AddBasketItem(New BasketItem(P.PageNode.Attributes("part").Value, CInt(q.Text), "/" & P.PageNode.Attributes("id").Value))
                    ElseIf l.Visible Then
                        .AddBasketItem(New BasketItem(P.PageNode.Attributes("part").Value, CInt(l.SelectedValue), "/" & P.PageNode.Attributes("id").Value))
                    ElseIf bx.Visible Then
                        .AddBasketItem(New BasketItem(P.PageNode.Attributes("part").Value, CInt(bx.SelectedValue), "/" & P.PageNode.Attributes("id").Value, P.PageNode.Attributes("boxcount").Value))
                    End If
                    lnk.Page.Response.Redirect("basket.aspx")
                End With
            End If
        End With
    End Sub

    Public Sub AddToBasket(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim btn As System.Web.UI.WebControls.Button = sender
        With btn.Page
            Dim q As TextBox = .Master.FindControl("basketqty")
            Dim l As DropDownList = .Master.FindControl("basketlist")
            Dim bx As DropDownList = .Master.FindControl("lstBoxCount")
            Dim P As cmsPage = New cmsPage(.Page, HttpContext.Current, .Page.Server)

            If Not IsNothing(q) Then
                With ts.Basket
                    If q.Visible Then
                        .AddBasketItem(New BasketItem(P.PageNode.Attributes("part").Value, CInt(q.Text), "/" & P.PageNode.Attributes("id").Value))
                    ElseIf l.Visible Then
                        .AddBasketItem(New BasketItem(P.PageNode.Attributes("part").Value, CInt(l.SelectedValue), "/" & P.PageNode.Attributes("id").Value))
                    ElseIf bx.Visible Then
                        .AddBasketItem(New BasketItem(P.PageNode.Attributes("part").Value, CInt(bx.SelectedValue), "/" & P.PageNode.Attributes("id").Value, P.PageNode.Attributes("boxcount").Value))
                    End If
                    btn.Page.Response.Redirect("basket.aspx")
                End With
            End If
        End With
    End Sub

#End Region

End Class

