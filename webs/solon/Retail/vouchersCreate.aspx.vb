Imports cmSi
Imports System.Xml
Imports System.Globalization

Partial Class VouchersCreate
    Inherits cmsInherit

#Region "Properties"
    Private _vcode As String
    Protected Property vcode() As String
        Get
            Return _vcode
        End Get
        Set(ByVal value As String)
            _vcode = value
        End Set
    End Property

    Private _vtype As String
    Protected Property vtype() As String
        Get
            Return _vtype
        End Get
        Set(ByVal value As String)
            _vtype = value
        End Set
    End Property

    Private isLinkTime As Boolean

    Private clist As List(Of Control)

    Protected vTypeList As String(,) = { _
            {"spendVoucher", "Spend Voucher", "Use this voucher type to define a fixed amount off the order, with an optional minimum spend"}, _
            {"pcSpendVoucher", "Spend Voucher (Percentage)", "Use this voucher type to define a percentage amount off the order, with an optional minimum spend"}, _
            {"partVoucher", "Part Specific Voucher", "Use this voucher type to define a fixed amount off (a) specific part(s)."}, _
            {"pcPartVoucher", "Part Specific Voucher (Percentage)", "Use this voucher type to define a percentage amount off (a) specific part(s)"}, _
            {"groupPartVoucher", "Group Parts Voucher", "Use this voucher type to define a fixed amount off (a) specific part(s). Spend will be calculated based on all applicable parts bought."}, _
            {"pcGroupPartVoucher", "Group Parts Voucher (Percentage)", "Use this voucher type to define a percentage amount off (a) specific part(s). Spend will be calculated based on all applicable parts bought."}, _
            {"multiBuyVoucher", "Multi-Buy Voucher", "Use this voucher type to define a fixed amount off when multiples of the same part are purchased."}, _
            {"pcMultiBuyVoucher", "Multi-Buy Voucher (Percentage)", _
                                            "Use this voucher type to define a percentage amount off when multiples of the same part are purchased."}, _
            {"poolVoucher", "Pool Voucher", "Use this voucher type to define a fixed amount off when a specified quantity of applicable parts are purchased."}, _
            {"pcPoolVoucher", "Pool Voucher (percentage)", _
                                            "Use this voucher type to define a percentage amount off when a specified quantity of applicable parts are purchased."}, _
            {"linkSaveVoucher", "Link Save Voucher", "Use this voucher type to define a fixed amount off specific parts when other, distinct parts are purchased."}, _
            {"pcLinkSaveVoucher", "Link Save Voucher (percentage)", _
                                            "Use this voucher type to define a percentage amount off specific parts when other, distinct parts are purchased."}}
#End Region

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Try

       
            clist = New List(Of Control)

            For Each param As String In Request.Form.AllKeys
                If param.Contains("gpLbx_") Then
                    clist.Add(New Label)
                    Dim lbx As New ListBox
                    lbx.ID = param
                    addParts(lbx)
                    For Each gp As String In Request.Form(param).Split(",")
                        For Each gpItem As ListItem In lbx.Items
                            If gpItem.Value = gp Then
                                gpItem.Selected = True
                            End If
                        Next
                    Next
                    clist.Add(lbx)
                    clist.Add(New Label)
                End If
            Next

            Dim idx As Integer = 0
            For Each c As Control In clist
                Page.Controls(0).FindControl("Main").Controls.AddAt(idx, c)
                idx += 1
            Next
        Catch
        End Try
    End Sub


    Protected Sub Page_Load1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.User.IsInRole("webmaster") Then
            Response.Redirect(cmSi.cmsData.Settings.Get("url") & "/login.aspx")
        End If

        vtype = Request.QueryString("vtype") 'might be nowt
        vcode = Request.QueryString("vcode")

        If Request.QueryString("delete") = "true" Then
            deleteVoucher(vcode)
        End If

        If Not IsNothing(vcode) Then
            Dim thisVoucher As XmlNode = cmSi.cmsData.offers.SelectSingleNode(String.Format("//offer[@code={0}{1}{0}]", Chr(34), vcode))
            If Not IsNothing(thisVoucher) Then
                vtype = thisVoucher.Attributes("type").Value 'if we got a vcode at the entry point, grab the vtype 
            End If
        End If

        Select Case vtype
            Case Nothing 'still nothing? k, fill that listbox.
                With voucherType.Items
                    Dim idx As Integer = vTypeList.GetUpperBound(0)
                    For i As Integer = 0 To idx
                        .Add(New ListItem(vTypeList(i, 1), vTypeList(i, 0)))
                    Next
                End With
            Case "spendVoucher"
                initSpendVoucher()
            Case "pcSpendVoucher"
                initSpendVoucher(pc:=True)
            Case "partVoucher"
                initPartVoucher()
            Case "pcPartVoucher"
                initPartVoucher(pc:=True)
            Case "multiBuyVoucher"
                initMultiBuyVoucher()
            Case "pcMultiBuyVoucher"
                initMultiBuyVoucher(pc:=True)
            Case "poolVoucher"
                initMultiBuyVoucher()
            Case "pcPoolVoucher"
                initMultiBuyVoucher(pc:=True)
            Case "linkSaveVoucher"
                initLinkSaveVoucher()
            Case "pcLinkSaveVoucher"
                initLinkSaveVoucher()
            Case "groupPartVoucher"
                initPartVoucher()
            Case "pcGroupPartVoucher"
                initPartVoucher(pc:=True)
        End Select
    End Sub

#Region "SpendVouchers"

    Protected Sub initSpendVoucher(Optional ByVal pc As Boolean = False)

        Page.Title = "Create Spend Voucher"
        uPnlVType.Visible = False 'hide vouchertypes
        pnlVoucherBasic.Visible = True
        pnlSpendVoucher.Visible = True
        addVoucher.Visible = True
        If pc Then
            lblVoucherDiscount.Text = "<h2>Discount Percentage</h2> Enter a discount percentage for this voucher <br /> <br />"
        End If
        Dim vcode As String = Request.QueryString("vcode")
        If Not IsNothing(vcode) Then
            Dim thisVoucher As XmlNode = cmSi.cmsData.offers.SelectSingleNode(String.Format("//offer[@code={0}{1}{0}]", Chr(34), vcode))
            lblVoucherMessage.Visible = True
            If IsNothing(thisVoucher) Then
                lblVoucherMessage.Text = "Voucher not found, please create voucher."
            ElseIf Not IsPostBack Then

                lblVoucherMessage.Text = String.Format("Now editing voucher {0}. Click submit to save changes.", vcode)

                txtVoucherCode.Text = vcode
                txtVoucherDes.Text = thisVoucher.Attributes("des").Value
                txtVoucherSpend.Text = thisVoucher.SelectSingleNode("buy/spend").Attributes("amount").Value
                txtVoucherDiscount.Text = thisVoucher.SelectSingleNode("get/discount").Attributes("amount").Value
                cdrVoucherExpiry.SelectedDate = DateTime.ParseExact(thisVoucher.Attributes("expiry").Value, "dd/MM/yyyy", CultureInfo.InvariantCulture)

            End If
        End If

        AddHandler addVoucher.Click, AddressOf addSpendVoucher
    End Sub

    Protected Sub addSpendVoucher()
        vcode = txtVoucherCode.Text
        Dim vdes As String = txtVoucherDes.Text
        Dim vexpiry As String = cdrVoucherExpiry.SelectedDate.ToString("dd/MM/yyyy")
        Dim vdiscount As String = txtVoucherDiscount.Text
        Dim vspend As String = txtVoucherSpend.Text

        If vspend = "" Then
            vspend = "0"
        End If

        If Not IsNothing(cmSi.cmsData.offers.SelectSingleNode(String.Format("//offer[@code={0}{1}{0}]", _
                                                                                 Chr(34), vcode))) Then
            'update
            With cmSi.cmsData.offers.SelectSingleNode(String.Format("//offer[@code={0}{1}{0}]", _
                                                                                 Chr(34), vcode))
                .Attributes("code").Value = vcode
                .Attributes("des").Value = vdes
                .Attributes("expiry").Value = vexpiry
                .SelectSingleNode("buy/spend").Attributes("amount").Value = vspend
                .SelectSingleNode("get/discount").Attributes("amount").Value = vdiscount
            End With
        Else
            With cmSi.cmsData.offers
                Dim newVoucher As XmlNode = .CreateElement("offer")

                Dim xCode As XmlAttribute = .CreateAttribute("code")
                xCode.Value = vcode
                newVoucher.Attributes.Append(xCode)

                Dim xType As XmlAttribute = .CreateAttribute("type")
                xType.Value = vtype
                newVoucher.Attributes.Append(xType)

                Dim xDes As XmlAttribute = .CreateAttribute("des")
                xDes.Value = vdes
                newVoucher.Attributes.Append(xDes)

                Dim xExpiry As XmlAttribute = .CreateAttribute("expiry")
                xExpiry.Value = vexpiry
                newVoucher.Attributes.Append(xExpiry)

                Dim xDiscount As XmlAttribute = .CreateAttribute("amount")
                xDiscount.Value = vdiscount
                newVoucher.AppendChild(.CreateElement("get") _
                         ).AppendChild(.CreateElement("discount") _
                         ).Attributes.Append(xDiscount)

                Dim xSpend As XmlAttribute = .CreateAttribute("amount")
                xSpend.Value = vspend
                newVoucher.AppendChild(.CreateElement("buy") _
                         ).AppendChild(.CreateElement("spend") _
                         ).Attributes.Append(xSpend)

                .SelectSingleNode("offers").AppendChild(newVoucher)
            End With
        End If

        cmSi.cmsData.offers.Save(cmSi.cmsData.offersPath)
        Response.Redirect("vouchers.aspx")
    End Sub

#End Region

#Region "PartVouchers"

    Protected Sub initPartVoucher(Optional ByVal pc As Boolean = False)

        Page.Title = "Create Part Voucher"
        uPnlVType.Visible = False 'hide vouchertypes
        pnlVoucherBasic.Visible = True
        pnlBuyParts.Visible = True
        addParts(buyParts)
        addVoucher.Visible = True
        pnlSpendVoucher.Visible = True

        If pc Then
            lblVoucherDiscount.Text = "<h2>Discount Percentage</h2> Enter a discount percentage for this voucher <br /> <br />"
        End If
        Dim vcode As String = Request.QueryString("vcode")
        If Not IsNothing(vcode) Then
            Dim thisVoucher As XmlNode = cmSi.cmsData.offers.SelectSingleNode(String.Format("//offer[@code={0}{1}{0}]", Chr(34), vcode))
            lblVoucherMessage.Visible = True
            If IsNothing(thisVoucher) Then
                lblVoucherMessage.Text = "Voucher not found, please create voucher."
            ElseIf Not IsPostBack Then

                lblVoucherMessage.Text = String.Format("Now editing voucher {0}. Click submit to save changes.", vcode)

                txtVoucherCode.Text = vcode
                txtVoucherDes.Text = thisVoucher.Attributes("des").Value
                txtVoucherSpend.Text = thisVoucher.SelectSingleNode("buy/spend").Attributes("amount").Value
                txtVoucherDiscount.Text = thisVoucher.SelectSingleNode("get/discount").Attributes("amount").Value
                cdrVoucherExpiry.SelectedDate = DateTime.ParseExact(thisVoucher.Attributes("expiry").Value, "dd/MM/yyyy", CultureInfo.InvariantCulture)
                For Each bp As XmlNode In thisVoucher.SelectNodes("buy//part")
                    Dim x As ListItem = buyParts.Items.FindByValue(bp.Attributes("name").Value)

                    If Not IsNothing(x) Then
                        buyParts.Items(buyParts.Items.IndexOf(x)).Selected = True
                    End If
                Next
            End If
        End If

        AddHandler addVoucher.Click, AddressOf addPartVoucher
    End Sub

    Protected Sub addPartVoucher()
        vcode = txtVoucherCode.Text
        Dim vdes As String = txtVoucherDes.Text
        Dim vexpiry As String = cdrVoucherExpiry.SelectedDate.ToString("dd/MM/yyyy")
        Dim vdiscount As String = txtVoucherDiscount.Text
        Dim vspend As String = txtVoucherSpend.Text

        If vspend = "" Then
            vspend = "0"
        End If

        If Not IsNothing(cmSi.cmsData.offers.SelectSingleNode(String.Format("//offer[@code={0}{1}{0}]", _
                                                                                 Chr(34), vcode))) Then
            'update
            With cmSi.cmsData.offers.SelectSingleNode(String.Format("//offer[@code={0}{1}{0}]", _
                                                                                 Chr(34), vcode))
                .Attributes("code").Value = vcode
                .Attributes("des").Value = vdes
                .Attributes("expiry").Value = vexpiry
                .SelectSingleNode("buy").RemoveAll()
                Dim spendnode As XmlNode = cmSi.cmsData.offers.CreateElement("spend")

                spendnode.Attributes.Append(cmSi.cmsData.offers.CreateAttribute("amount"))
                spendnode.Attributes("amount").Value = vspend
                .SelectSingleNode("buy").AppendChild(spendnode)

                For Each i As ListItem In buyParts.Items
                    If i.Selected = True Then
                        Dim xPart As XmlElement = cmSi.cmsData.offers.CreateElement("part")
                        xPart.Attributes.Append(cmSi.cmsData.offers.CreateAttribute("name"))
                        xPart.Attributes("name").Value = i.Value
                        .SelectSingleNode("buy").AppendChild(xPart)
                    End If
                Next
                .SelectSingleNode("get/discount").Attributes("amount").Value = vdiscount
            End With
        Else
            With cmSi.cmsData.offers
                Dim newVoucher As XmlNode = .CreateElement("offer")

                Dim xCode As XmlAttribute = .CreateAttribute("code")
                xCode.Value = vcode
                newVoucher.Attributes.Append(xCode)

                Dim xType As XmlAttribute = .CreateAttribute("type")
                xType.Value = vtype
                newVoucher.Attributes.Append(xType)

                Dim xDes As XmlAttribute = .CreateAttribute("des")
                xDes.Value = vdes
                newVoucher.Attributes.Append(xDes)

                Dim xExpiry As XmlAttribute = .CreateAttribute("expiry")
                xExpiry.Value = vexpiry
                newVoucher.Attributes.Append(xExpiry)


                Dim xDiscount As XmlAttribute = .CreateAttribute("amount")
                xDiscount.Value = vdiscount
                newVoucher.AppendChild(.CreateElement("get") _
                         ).AppendChild(.CreateElement("discount") _
                         ).Attributes.Append(xDiscount)

                Dim xSpend As XmlAttribute = .CreateAttribute("amount")
                xSpend.Value = vspend
                newVoucher.AppendChild(.CreateElement("buy") _
                         ).AppendChild(.CreateElement("spend") _
                         ).Attributes.Append(xSpend)

                For Each i As ListItem In buyParts.Items
                    If i.Selected = True Then
                        Dim xPart As XmlElement = .CreateElement("part")
                        xPart.Attributes.Append(.CreateAttribute("name"))
                        xPart.Attributes("name").Value = i.Value
                        newVoucher.SelectSingleNode("buy").AppendChild(xPart)
                    End If
                Next


                .SelectSingleNode("offers").AppendChild(newVoucher)
            End With
        End If

        cmSi.cmsData.offers.Save(cmSi.cmsData.offersPath)
        Response.Redirect("vouchers.aspx")
    End Sub
#End Region

#Region "MultiBuy Vouchers"

    Protected Sub initMultiBuyVoucher(Optional ByVal pc As Boolean = False)

        Page.Title = "Create MultiBuy Voucher"
        uPnlVType.Visible = False 'hide vouchertypes
        pnlVoucherBasic.Visible = True
        pnlBuyParts.Visible = True
        addParts(buyParts)
        addVoucher.Visible = True
        pnlSpendVoucher.Visible = True
        lblVoucherSpend.Visible = False
        txtVoucherSpend.Visible = False
        txtVoucherBuyQty.Visible = True
        lblVoucherBuyQty.Visible = True
        pnlGetParts.Visible = True

        If pc Then
            lblVoucherDiscount.Text = "<h2>Discount Percentage</h2> Enter a discount percentage for this voucher <br /> <br />"
        End If
        Dim vcode As String = Request.QueryString("vcode")
        If Not IsNothing(vcode) Then
            Dim thisVoucher As XmlNode = cmSi.cmsData.offers.SelectSingleNode(String.Format("//offer[@code={0}{1}{0}]", Chr(34), vcode))
            lblVoucherMessage.Visible = True
            If IsNothing(thisVoucher) Then
                lblVoucherMessage.Text = "Voucher not found, please create voucher."
            ElseIf Not IsPostBack Then

                lblVoucherMessage.Text = String.Format("Now editing voucher {0}. Click submit to save changes.", vcode)

                txtVoucherCode.Text = vcode
                txtVoucherDes.Text = thisVoucher.Attributes("des").Value
                txtVoucherDiscount.Text = thisVoucher.SelectSingleNode("get/discount").Attributes("amount").Value
                cdrVoucherExpiry.SelectedDate = DateTime.ParseExact(thisVoucher.Attributes("expiry").Value, "dd/MM/yyyy", CultureInfo.InvariantCulture)
                For Each bp As XmlNode In thisVoucher.SelectNodes("buy//part")
                    Dim x As ListItem = buyParts.Items.FindByValue(bp.Attributes("name").Value)

                    If Not IsNothing(x) Then
                        buyParts.Items(buyParts.Items.IndexOf(x)).Selected = True
                    End If
                Next
                txtVoucherBuyQty.Text = thisVoucher.SelectSingleNode("buy").Attributes("qty").Value
                txtVoucherGetQty.Text = thisVoucher.SelectSingleNode("get").Attributes("qty").Value
            End If
        End If

        AddHandler addVoucher.Click, AddressOf addMultiBuyVoucher

    End Sub

    Protected Sub addMultiBuyVoucher()
        vcode = txtVoucherCode.Text
        Dim vdes As String = txtVoucherDes.Text
        Dim vexpiry As String = cdrVoucherExpiry.SelectedDate.ToString("dd/MM/yyyy")
        Dim vdiscount As String = txtVoucherDiscount.Text
        Dim vspend As String = txtVoucherSpend.Text
        Dim vbuyqty As String = txtVoucherBuyQty.Text
        Dim vgetqty As String = txtVoucherGetQty.Text


        vspend = "0"


        If Not IsNothing(cmSi.cmsData.offers.SelectSingleNode(String.Format("//offer[@code={0}{1}{0}]", _
                                                                                 Chr(34), vcode))) Then
            'update
            With cmSi.cmsData.offers.SelectSingleNode(String.Format("//offer[@code={0}{1}{0}]", _
                                                                                 Chr(34), vcode))
                .Attributes("code").Value = vcode
                .Attributes("des").Value = vdes
                .Attributes("expiry").Value = vexpiry
                .SelectSingleNode("buy").RemoveAll()

                Dim spendnode As XmlNode = cmSi.cmsData.offers.CreateElement("spend")
                .SelectSingleNode("buy").AppendChild(spendnode)

                Dim buyqty As XmlAttribute = cmSi.cmsData.offers.CreateAttribute("qty")
                buyqty.Value = txtVoucherBuyQty.Text
                .SelectSingleNode("buy").Attributes.Append(buyqty)

                For Each i As ListItem In buyParts.Items
                    If i.Selected = True Then
                        Dim xPart As XmlElement = cmSi.cmsData.offers.CreateElement("part")
                        xPart.Attributes.Append(cmSi.cmsData.offers.CreateAttribute("name"))
                        xPart.Attributes("name").Value = i.Value
                        .SelectSingleNode("buy").AppendChild(xPart)
                    End If
                Next

                Dim getqty As XmlAttribute = cmSi.cmsData.offers.CreateAttribute("qty")
                getqty.Value = txtVoucherGetQty.Text
                .SelectSingleNode("get").Attributes.Append(getqty)

                .SelectSingleNode("get/discount").Attributes("amount").Value = vdiscount
            End With
        Else
            With cmSi.cmsData.offers
                Dim newVoucher As XmlNode = .CreateElement("offer")

                Dim xCode As XmlAttribute = .CreateAttribute("code")
                xCode.Value = vcode
                newVoucher.Attributes.Append(xCode)

                Dim xType As XmlAttribute = .CreateAttribute("type")
                xType.Value = vtype
                newVoucher.Attributes.Append(xType)

                Dim xDes As XmlAttribute = .CreateAttribute("des")
                xDes.Value = vdes
                newVoucher.Attributes.Append(xDes)

                Dim xExpiry As XmlAttribute = .CreateAttribute("expiry")
                xExpiry.Value = vexpiry
                newVoucher.Attributes.Append(xExpiry)


                Dim xDiscount As XmlAttribute = .CreateAttribute("amount")
                xDiscount.Value = vdiscount
                newVoucher.AppendChild(.CreateElement("get") _
                         ).AppendChild(.CreateElement("discount") _
                         ).Attributes.Append(xDiscount)



                Dim xBuyQty As XmlAttribute = .CreateAttribute("qty")
                xBuyQty.Value = vbuyqty
                newVoucher.AppendChild(.CreateElement("buy")).Attributes.Append(xBuyQty)

                For Each i As ListItem In buyParts.Items
                    If i.Selected = True Then
                        Dim xPart As XmlElement = .CreateElement("part")
                        xPart.Attributes.Append(.CreateAttribute("name"))
                        xPart.Attributes("name").Value = i.Value
                        newVoucher.SelectSingleNode("buy").AppendChild(xPart)
                    End If
                Next

                Dim xGetQty As XmlAttribute = .CreateAttribute("qty")
                xGetQty.Value = vgetqty
                newVoucher.SelectSingleNode("get").Attributes.Append(xGetQty)

                .SelectSingleNode("offers").AppendChild(newVoucher)

            End With
        End If

        cmSi.cmsData.offers.Save(cmSi.cmsData.offersPath)
        Response.Redirect("vouchers.aspx")
    End Sub
#End Region

#Region "Pool Vouchers"
    'calls multipart as xml & set up are functionally identical. 
#End Region

#Region "Linksave Vouchers"


    Protected Sub initLinkSaveVoucher()

        Page.Title = "Create MultiBuy Voucher"
        uPnlVType.Visible = False 'hide vouchertypes
        pnlVoucherBasic.Visible = True
        pnlBuyParts.Visible = True
        addParts(buyParts)
        addVoucher.Visible = True
        pnlSpendVoucher.Visible = True
        lblVoucherSpend.Visible = False
        txtVoucherSpend.Visible = False
        txtVoucherBuyQty.Visible = True
        lblVoucherBuyQty.Visible = True
        pnlGetParts.Visible = True

        'If pc Then
        '    lblVoucherDiscount.Text = "<h2>Discount Percentage</h2> Enter a discount percentage for this voucher <br /> <br />"
        'End If
        Dim vcode As String = Request.QueryString("vcode")
        If Not IsNothing(vcode) Then
            Dim thisVoucher As XmlNode = cmSi.cmsData.offers.SelectSingleNode(String.Format("//offer[@code={0}{1}{0}]", Chr(34), vcode))
            lblVoucherMessage.Visible = True
            If IsNothing(thisVoucher) Then
                lblVoucherMessage.Text = "Voucher not found, please create voucher."
            ElseIf Not IsPostBack Then

                lblVoucherMessage.Text = String.Format("Now editing voucher {0}. Click submit to save changes.", vcode)

                txtVoucherCode.Text = vcode
                txtVoucherDes.Text = thisVoucher.Attributes("des").Value
                txtVoucherDiscount.Text = thisVoucher.SelectSingleNode("get/discount").Attributes("amount").Value
                cdrVoucherExpiry.SelectedDate = DateTime.ParseExact(thisVoucher.Attributes("expiry").Value, "dd/MM/yyyy", CultureInfo.InvariantCulture)
                For Each bp As XmlNode In thisVoucher.SelectNodes("buy//part")
                    Dim x As ListItem = buyParts.Items.FindByValue(bp.Attributes("name").Value)

                    If Not IsNothing(x) Then
                        buyParts.Items(buyParts.Items.IndexOf(x)).Selected = True
                    End If
                Next
                txtVoucherBuyQty.Text = thisVoucher.SelectSingleNode("buy").Attributes("qty").Value
                txtVoucherGetQty.Text = thisVoucher.SelectSingleNode("get").Attributes("qty").Value
            End If
        End If
        addVoucher.Text = "Next"

        AddHandler addVoucher.Click, AddressOf initLinkSaveGetParts

    End Sub

    Protected Sub initLinkSaveGetParts()



        isLinkTime = True
        clist = New List(Of Control)
        addVoucher.Visible = False


        addVoucher.Text = "Submit"
        pnlVoucherBasic.Visible = False
        pnlBuyParts.Visible = False
        pnlGetParts.Visible = False
        pnlSpendVoucher.Visible = False
        Dim j As Integer = 0

        For Each i As ListItem In buyParts.Items
            If i.Selected = True Then

                Dim gp As New ListBox
                Dim gpLabel As New Label
                gpLabel.Text = "'Get' parts for 'Buy' part: " & i.Text & "<br />"
                Dim brLabel As New Label

                brLabel.Text = "<br /><br /><br />"
                addParts(gp)
                gp.ID = "gpLbx_" & i.Value
                gp.SelectionMode = ListSelectionMode.Multiple

                If Not IsNothing(vcode) Then
                    Dim thisVoucher As XmlNode = cmSi.cmsData.offers.SelectSingleNode(String.Format("//offer[@code={0}{1}{0}]", Chr(34), vcode))

                    Dim g As XmlNode = thisVoucher.SelectSingleNode(String.Format("buy/part[@name={0}{1}{0}]", Chr(34), i.Value))
                    For Each h As XmlNode In g.SelectNodes("gpart")
                        Dim x As ListItem = gp.Items.FindByValue(h.Attributes("name").Value)

                        If Not IsNothing(x) Then
                            gp.Items(gp.Items.IndexOf(x)).Selected = True
                        End If
                    Next

                End If
                clist.Add(gpLabel)
                clist.Add(gp)
                clist.Add(brLabel)
                'from here
            End If

        Next
        'to here
        Dim idx As Integer = 0
        For Each c As Control In clist
            Page.Controls(0).FindControl("Main").Controls.AddAt(idx, c)
            idx += 1
        Next
        LinkSaveVoucher.Visible = True
        'addHandler(b.Click, AddressOf addLinkSaveVoucher)

    End Sub

    Protected Sub addLinkSaveVoucher()

        'todo from here\


        'todo something around here to make it work gahhhhhh
        'For Each item As ListItem In buyParts.Items
        '    If item.Selected = True Then
        '        For Each j As String In Request.Form.AllKeys
        '            If j.Contains(item.Value) Then
        '                Dim lb As New ListBox
        '                lb.ID = j
        '                addParts(lb)
        '                For Each k As String In Request.Form(j).Split(",")
        '                    lb.Items.FindByValue(k).Selected = True
        '                Next
        '                Page.Controls(0).FindControl("main").Controls.Add(lb)
        '            End If
        '        Next
        '    End If
        'Next

        vcode = txtVoucherCode.Text

        Dim vdes As String = txtVoucherDes.Text
        Dim vexpiry As String = cdrVoucherExpiry.SelectedDate.ToString("dd/MM/yyyy")
        Dim vdiscount As String = txtVoucherDiscount.Text
        Dim vspend As String = txtVoucherSpend.Text
        Dim vbuyqty As String = txtVoucherBuyQty.Text
        Dim vgetqty As String = txtVoucherGetQty.Text


        vspend = "0"


        If Not IsNothing(cmSi.cmsData.offers.SelectSingleNode(String.Format("//offer[@code={0}{1}{0}]", _
                                                                                 Chr(34), vcode))) Then
            'update
            With cmSi.cmsData.offers.SelectSingleNode(String.Format("//offer[@code={0}{1}{0}]", _
                                                                                 Chr(34), vcode))
                .Attributes("code").Value = vcode
                .Attributes("des").Value = vdes
                .Attributes("expiry").Value = vexpiry
                .SelectSingleNode("buy").RemoveAll()

                Dim spendnode As XmlNode = cmSi.cmsData.offers.CreateElement("spend")
                .SelectSingleNode("buy").AppendChild(spendnode)

                Dim buyqty As XmlAttribute = cmSi.cmsData.offers.CreateAttribute("qty")
                buyqty.Value = txtVoucherBuyQty.Text
                .SelectSingleNode("buy").Attributes.Append(buyqty)

                For Each i As ListItem In buyParts.Items
                    If i.Selected = True Then
                        Dim xPart As XmlElement = cmSi.cmsData.offers.CreateElement("part")
                        xPart.Attributes.Append(cmSi.cmsData.offers.CreateAttribute("name"))
                        xPart.Attributes("name").Value = i.Value


                        For Each item In Request.Form.AllKeys
                            If item.Contains(i.Value) Then
                                For Each j As String In Request.Form(item).Split(",")
                                    Dim xgPart As XmlElement = cmSi.cmsData.offers.CreateElement("gpart")
                                    xgPart.Attributes.Append(cmSi.cmsData.offers.CreateAttribute("name"))
                                    xgPart.Attributes("name").Value = j
                                    xPart.AppendChild(xgPart)
                                Next
                            End If
                        Next



                        .SelectSingleNode("buy").AppendChild(xPart)
                    End If
                Next

                Dim getqty As XmlAttribute = cmSi.cmsData.offers.CreateAttribute("qty")
                getqty.Value = txtVoucherGetQty.Text
                .SelectSingleNode("get").Attributes.Append(getqty)

                .SelectSingleNode("get/discount").Attributes("amount").Value = vdiscount
            End With
        Else
            With cmSi.cmsData.offers
                Dim newVoucher As XmlNode = .CreateElement("offer")

                Dim xCode As XmlAttribute = .CreateAttribute("code")
                xCode.Value = vcode
                newVoucher.Attributes.Append(xCode)

                Dim xType As XmlAttribute = .CreateAttribute("type")
                xType.Value = vtype
                newVoucher.Attributes.Append(xType)

                Dim xDes As XmlAttribute = .CreateAttribute("des")
                xDes.Value = vdes
                newVoucher.Attributes.Append(xDes)

                Dim xExpiry As XmlAttribute = .CreateAttribute("expiry")
                xExpiry.Value = vexpiry
                newVoucher.Attributes.Append(xExpiry)


                Dim xDiscount As XmlAttribute = .CreateAttribute("amount")
                xDiscount.Value = vdiscount
                newVoucher.AppendChild(.CreateElement("get") _
                         ).AppendChild(.CreateElement("discount") _
                         ).Attributes.Append(xDiscount)



                Dim xBuyQty As XmlAttribute = .CreateAttribute("qty")
                xBuyQty.Value = vbuyqty
                newVoucher.AppendChild(.CreateElement("buy")).Attributes.Append(xBuyQty)

                For Each i As ListItem In buyParts.Items
                    If i.Selected = True Then
                        Dim xPart As XmlElement = .CreateElement("part")
                        xPart.Attributes.Append(.CreateAttribute("name"))
                        xPart.Attributes("name").Value = i.Value
                        'todo: for each getpart, add an xml node for it before appending the buypart.
                        newVoucher.SelectSingleNode("buy").AppendChild(xPart)
                    End If
                Next

                Dim xGetQty As XmlAttribute = .CreateAttribute("qty")
                xGetQty.Value = vgetqty
                newVoucher.SelectSingleNode("get").Attributes.Append(xGetQty)

                .SelectSingleNode("offers").AppendChild(newVoucher)

            End With
        End If

        cmSi.cmsData.offers.Save(cmSi.cmsData.offersPath)
        Response.Redirect("vouchers.aspx")
    End Sub

#End Region

#Region "vType handlers"
    Protected Sub voucherType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles voucherType.SelectedIndexChanged
        VoucherLongDes.Text = vTypeList(voucherType.SelectedIndex, 2)
    End Sub

    Protected Sub submitType_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles submitType.Click
        Response.Redirect(String.Format("~/vouchersCreate.aspx?vtype={0}", voucherType.SelectedItem.Value))
    End Sub
#End Region


    Protected Sub deleteVoucher(ByVal vCode As String)
        Dim toDelete As XmlNode = cmSi.cmsData.offers.SelectSingleNode(String.Format("//offer[@code={0}{1}{0}]", Chr(34), vCode))
        toDelete.ParentNode.RemoveChild(toDelete)
        cmSi.cmsData.offers.Save(cmSi.cmsData.offersPath)
        Response.Redirect("vouchers.aspx")
    End Sub

    Protected Sub addParts(ByRef parts As ListBox)
        Dim partsDict As Dictionary(Of String, String) = xmlFunc.AllParts(cmSi.cmsData.part)
        For Each item As String In partsDict.Keys
            parts.Items.Add(New ListItem(item & " - " & partsDict(item), item))
        Next
    End Sub


    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click
        Response.Redirect("~/vouchers.aspx")
    End Sub

    Protected Sub LinkSaveVoucher_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkSaveVoucher.Click
        addLinkSaveVoucher()
    End Sub


End Class
