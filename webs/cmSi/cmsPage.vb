Imports System.Xml
Imports System.IO
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.HtmlControls

Public Class cmsPage

#Region "Public Properties"

    Public ReadOnly Property AbsoluteUri() As String
        Get
            Static CachedValue As String = Nothing
            If IsNothing(CachedValue) Then
                Dim e As String = ""
                Dim arui As String = _thisServer.UrlDecode(_thisContext.Request.Url.AbsoluteUri)
                If InStr(arui, "?404;") > 0 Then
                    If Right(arui, 1) = "/" Then arui = Left(arui, arui.Length - 1)
                    e = Split(arui, ":" & _thisContext.Request.Url.Port.ToString & "/")(1)
                    CachedValue = e.Split("/").Last.ToLower.Split("?").First
                ElseIf Not PhysicalFile Then
                    e = ""
                Else
                    CachedValue = arui.Split("/").Last.ToLower.Split("?").First
                End If
            End If
            Return CachedValue
        End Get
    End Property

    Private _PageNode As XmlNode
    Public ReadOnly Property PageNode() As XmlNode
        Get
            Return _PageNode
        End Get
    End Property

    Public ReadOnly Property BoxCount() As Integer
        Get
            Static CachedValue As Integer = -999
            If CachedValue = -999 Then
                Dim bx As Integer = -1
                Try
                    If _PageNode.Attributes("boxcount").Value.Length > 0 Then
                        bx = CInt(_PageNode.Attributes("boxcount").Value)
                    End If
                Catch
                End Try
                CachedValue = bx
            End If
            Return CachedValue
        End Get
    End Property

    Public ReadOnly Property Part() As XmlNode
        Get
            Static CachedValue As XmlNode = Nothing
            Static ld As Boolean = False
            If Not (ld) Then
                ld = True
                If Not IsNothing(_PageNode.Attributes("part")) Then
                    Dim PartName As String = _PageNode.Attributes("part").Value
                    If PartName.Length > 0 Then
                        CachedValue = cmsData.part.SelectSingleNode( _
                            String.Format( _
                            "/*[position()=1]/PARTS/PART[PARTNAME='{0}']", _
                            PartName _
                            ) _
                        )
                    Else
                        CachedValue = Nothing
                    End If
                End If
            End If
            Return CachedValue
        End Get
    End Property

    Public ReadOnly Property thisCat() As XmlNode
        Get
            Return cmsData.cat.SelectSingleNode(String.Format("//cat[@id='{0}']", FormDictionary("id")))
        End Get
    End Property

    Private _FormDictionary As New Dictionary(Of String, String)
    Public ReadOnly Property FormDictionary(ByVal key As String, Optional ByVal strDefault As String = "") As String
        Get
            If _FormDictionary.Keys.Contains(key.ToLower) Then
                Return _FormDictionary(key.ToLower)
            Else
                If key.ToLower = "id" Then
                    If IsNothing(AbsoluteUri) OrElse AbsoluteUri.Length = 0 Then
                        Return cmsData.cat.SelectSingleNode("cat").Attributes("id").Value ' home page
                    Else
                        Return AbsoluteUri
                    End If
                Else
                    Return strDefault
                End If
            End If
        End Get
    End Property

#End Region

#Region "Private Properties"

    Private _thisPage As Page
    Private ReadOnly Property thisPage() As Page
        Get
            Return _thisPage
        End Get
    End Property

    Private _thisContext As HttpContext
    Private ReadOnly Property thisContext() As HttpContext
        Get
            Return _thisContext
        End Get
    End Property

    Private _thisServer As HttpServerUtility
    Private ReadOnly Property thisServer() As HttpServerUtility
        Get
            Return _thisServer
        End Get
    End Property

    Private _PlaceHolders As New List(Of Control)
    Private ReadOnly Property PlaceHolders() As List(Of Control)
        Get
            Return _PlaceHolders
        End Get
    End Property

    Private ReadOnly Property PhysicalFile() As Boolean
        Get
            Dim last As String = _thisServer.UrlDecode(_thisContext.Request.Url.AbsoluteUri).Split("/").Last.Split("?").First
            Return File.Exists(cmsData.rootpath & "/" & last) And Not (String.Compare(last, "default.aspx", True) = 0)
        End Get
    End Property

#End Region

#Region "initialisation and finalisation"

    Public Sub New(ByRef thisPage As Page, ByRef thisContext As HttpContext, ByRef thisServer As HttpServerUtility, Optional ByVal Create As Boolean = False)

        _thisPage = thisPage
        _thisServer = thisServer
        _thisContext = thisContext

        ' Create a dictionary of the request object
        With thisPage
            For Each K As String In .Request.Form.Keys
                _FormDictionary.Add(Split(K, "$").Last.ToLower, .Request(K))
            Next
            For Each K As String In .Request.QueryString.Keys
                _FormDictionary.Add(Split(K, "$").Last.ToLower, .Request(K))
            Next
        End With

        Dim id As String = FormDictionary("id")
        _PageNode = cmsData.doc.SelectSingleNode(String.Format("//page[@id='{0}']", id))

        If IsNothing(_PageNode) And Create Then
            Dim MenuNode As XmlNode = cmsData.cat.SelectSingleNode(String.Format("//cat[@id='{0}']", id))
            If Not IsNothing(MenuNode) Then
                Dim pages As XmlNode = cmsData.doc.SelectSingleNode("pages")
                pages.AppendChild(NewPage(id, MenuNode.Attributes("name").Value))
                cmsData.doc.Save(cmsData.DocPath)
                _PageNode = cmsData.doc.SelectSingleNode(String.Format("//page[@id='{0}']", id))
            End If
        End If

        If IsNothing(_PageNode) And Create Then
            If File.Exists(cmsData.rootpath & "/" & id) Then
                Dim pages As XmlNode = cmsData.doc.SelectSingleNode("pages")
                pages.AppendChild(NewPage(id, id))
                cmsData.doc.Save(cmsData.DocPath)
                _PageNode = cmsData.doc.SelectSingleNode(String.Format("//page[@id='{0}']", id))
            End If
        End If

        If Not IsNothing(_PageNode) Then
            With _thisPage
                Try
                    .MasterPageFile = _PageNode.Attributes("masterpage").Value
                    .Title = PageNode.Attributes("title").Value
                Catch
                End Try
            End With
            Save()
        Else
            _thisContext.Response.Redirect("404.aspx")
        End If

    End Sub

    Public Sub PageLoad(ByRef thisPage As Page, Optional ByVal ShowEdit As Boolean = False)

        _thisPage = thisPage

        If Not IsNothing(_PageNode) Then
            thisPage.Form.Controls.Add(NewHiddenField("id", FormDictionary("id")))

            ' iterate through placeholders
            For Each c As Control In thisPage.Master.Controls
                Select Case c.GetType.ToString.ToLower
                    Case "system.web.ui.htmlcontrols.htmlform"
                        For Each i As Control In c.Controls
                            With i
                                Select Case .GetType.ToString.ToLower
                                    Case "system.web.ui.webcontrols.contentplaceholder", "system.web.ui.webcontrols.placeholder"

                                        If Not (PhysicalFile) Or _
                                            (PhysicalFile And _
                                             String.Compare( _
                                                .GetType.ToString, _
                                                "system.web.ui.webcontrols.placeholder", _
                                                True) = 0 _
                                            ) Then

                                            _PlaceHolders.Add(i)
                                            Dim section As XmlNode = _PageNode.SelectSingleNode(String.Format("section[@placeholder='{0}']", .ID))

                                            If IsNothing(section) Then
                                                _PageNode.AppendChild(NewSection(.ID))
                                                cmsData.doc.Save(cmsData.DocPath)
                                                section = _PageNode.SelectSingleNode(String.Format("section[@placeholder='{0}']", .ID))
                                            End If

                                            .Controls.Add(New LiteralControl(section.Attributes("html").Value))

                                            If ShowEdit Then
                                                .Controls.Add(btnEditSection(section.Attributes("placeholder").Value))
                                            End If

                                        End If

                                End Select
                            End With
                        Next
                        Exit For
                End Select
            Next

            If Not IsNothing(thisPage.Header) Then
                With thisPage.Header.Controls
                    .Add(Meta("Keywords", _PageNode.Attributes("keywords").Value))
                    .Add(Meta("Description", _PageNode.Attributes("description").Value))
                    .Add(Meta("revisit-after", "7 days"))
                    .Add(Meta("robots", "follow"))
                    .Add(Meta("author", "eMerge-IT"))
                End With
            End If

            If ShowEdit Then
                _thisPage.Form.Controls.AddAt(0, PageEdit)
            End If

        End If

    End Sub

#End Region

#Region "Save CMS Data"

    Private Sub Save()
        If String.Compare(FormDictionary("act"), "save", True) = 0 Then
            Select Case FormDictionary("ph").ToLower
                Case "page"
                    SavePage()
                Case Else
                    SaveSection()
            End Select
        End If
    End Sub

    Private Sub SavePage()

        With _PageNode

            .Attributes("masterpage").Value = FormDictionary("masterpage")
            .Attributes("title").Value = FormDictionary("title")
            .Attributes("description").Value = FormDictionary("description")
            .Attributes("keywords").Value = FormDictionary("keywords")

            If Not FormDictionary("part") = "NOPART" Then
                Try
                    .Attributes("part").Value = FormDictionary("part")
                Catch
                    Dim at As XmlAttribute = cmsData.doc.CreateAttribute("part")
                    at.Value = FormDictionary("part")
                    .Attributes.Append(at)
                End Try
            Else
                Try
                    .Attributes("part").Value = ""
                Catch
                End Try
            End If
            If Not FormDictionary("boxcount").Length = 0 Then
                Try
                    .Attributes("boxcount").Value = FormDictionary("boxcount")
                Catch
                    Dim at As XmlAttribute = cmsData.doc.CreateAttribute("boxcount")
                    at.Value = FormDictionary("boxcount")
                    .Attributes.Append(at)
                End Try
            Else
                Try
                    .Attributes("boxcount").Value = ""
                Catch
                End Try
            End If

            cmsData.doc.Save(cmsData.DocPath)

        End With

    End Sub

    Private Sub SaveSection()
        Dim section As XmlNode = _PageNode.SelectSingleNode(String.Format("section[@placeholder='{0}']", FormDictionary("ph")))
        If Not IsNothing(section) Then
            section.Attributes("html").Value = FormDictionary("htmltextbox").Replace(vbCrLf, "")
            cmsData.doc.Save(cmsData.DocPath)
        End If
    End Sub

#End Region

#Region "Make Nodes"

    Private Function NewPage(ByVal id As String, ByVal Title As String) As XmlNode
        Dim Ch As System.Xml.XmlElement
        Ch = cmsData.doc.CreateElement("page")
        With Ch
            Ch.SetAttribute("id", id)
            Ch.SetAttribute("masterpage", "blank.master")
            Ch.SetAttribute("title", Title)
            Ch.SetAttribute("description", "")
            Ch.SetAttribute("keywords", "")
        End With
        Return Ch
    End Function

    Private Function NewSection(ByVal PlaceHolder As String) As XmlNode
        Dim Ch As System.Xml.XmlElement
        Ch = cmsData.doc.CreateElement("section")
        With Ch
            Ch.SetAttribute("placeholder", PlaceHolder)
            Ch.SetAttribute("html", "")
        End With
        Return Ch
    End Function

#End Region

#Region "Make controls"

    Private Function NewBtnImg(ByVal name As String, ByVal img As String, ByVal handler As System.Web.UI.ImageClickEventHandler) As ImageButton
        Dim btn As New ImageButton
        With btn
            .ID = name
            .ImageUrl = img
        End With
        AddHandler btn.Click, handler
        Return btn
    End Function

    Private Function LabelRow(ByVal strLabel As String, ByVal Ctrl As Control) As TableRow
        Dim tr As New TableRow
        With tr
            Dim c1 As New TableCell
            Dim c2 As New TableCell
            Dim c1label As New Label()

            c1label.Text = strLabel
            c1.Controls.Add(c1label)
            c1.VerticalAlign = VerticalAlign.Top
            c1.Wrap = False

            c2.Controls.Add(Ctrl)
            c2.VerticalAlign = VerticalAlign.Top

            .Controls.Add(c1)
            .Controls.Add(c2)
        End With
        Return tr
    End Function

    Private Function btnEditSection(ByVal placeholder As String) As ImageButton
        Dim link As New ImageButton
        With link
            .ImageUrl = "~/ckeditor/btn/btnEditSection.png"
            .CommandName = "edit"
            .CommandArgument = placeholder
            .CssClass = "cmsBtn"
            ' .OnClientClick = "sheet = document.getElementById(""ctl00_remove""); sheet.parentNode.removeChild(sheet);"
        End With
        AddHandler link.Click, AddressOf hbtnEdit
        Return link
    End Function

    Private Function NewEditHTML(ByVal html As String) As TextBox
        Dim U As New System.Web.UI.WebControls.Unit("100%")
        Dim tb As New TextBox
        With tb
            .ID = "htmlTextBox"
            .TextMode = TextBoxMode.MultiLine
            .Width = U
            .Height = U
            .Text = html
        End With
        Return tb
    End Function

    Private Function NewHiddenField(ByVal Name As String, ByVal Value As String) As HiddenField
        Dim hf As New HiddenField
        With hf
            .ID = Name
            .Value = Value
        End With
        Return hf
    End Function

    Private Function PageEdit() As PlaceHolder

        Dim ph As New PlaceHolder
        With ph
            .ID = "PageEdit"

            .Controls.Add(New LiteralControl("<div class='adminbar'>"))
            .Controls.Add(New LiteralControl(String.Format("<table><tr><td width={0}100%{0}>", Chr(34))))

            If _thisContext.User.IsInRole("webmaster") Then
                Dim hl As New HyperLink()
                With hl
                    .Text = "Edit Menu"
                    .NavigateUrl = "cat.aspx"
                    .ImageUrl = "~/ckeditor/btn/btnEditMenu.png"
                End With
                .Controls.Add(hl)

                '.Controls.Add(New LiteralControl("&nbsp;&nbsp;"))

                Dim fhl As New HyperLink()
                With fhl
                    .Text = "Edit Resources"
                    .NavigateUrl = "files.aspx"
                    .ImageUrl = "~/ckeditor/btn/btnEditRes.png"
                End With
                .Controls.Add(fhl)

                '.Controls.Add(New LiteralControl("&nbsp;&nbsp;"))

                Dim link As New ImageButton
                With link
                    .ImageUrl = "~/ckeditor/btn/btnEditPage.png"
                    .CommandName = "edit"
                    .CommandArgument = "page"
                End With
                .Controls.Add(link)
                AddHandler link.Click, AddressOf hbtnPageEdit

            End If

            .Controls.Add(New LiteralControl(String.Format("</td><td NOWRAP>", Chr(34))))

            Dim conf As New HyperLink()
            With conf
                .Text = "Web Configuration"
                .NavigateUrl = "config.aspx"
                .ImageUrl = "~/ckeditor/btn/btnConfig.png"
            End With
            .Controls.Add(conf)

            Dim memb As New HyperLink()
            With memb
                .Text = "Membership"
                .NavigateUrl = "members.aspx"
                .ImageUrl = "~/ckeditor/btn/btnMembership.png"
            End With
            .Controls.Add(memb)

            'Dim chpass As New HyperLink()
            'With chpass
            '    .Text = "Change Password"
            '    .NavigateUrl = _thisContext.Application.Get("ChPassPage")
            '    .ImageUrl = "~/ckeditor/btn/btnChPass.png"
            'End With
            '.Controls.Add(chpass)

            'Dim logout As New HyperLink()
            'With logout
            '    .Text = "Log Off"
            '    .NavigateUrl = "/logoff.aspx"
            '    .ImageUrl = "~/ckeditor/btn/btnLogout.png"
            'End With
            '.Controls.Add(logout)
            .Controls.Add(New LiteralControl(String.Format("</td></tr></table>", Chr(34))))

            .Controls.Add(New LiteralControl("<hr width='100%'>"))

            .Controls.Add(New LiteralControl(String.Format("</div>", Chr(34))))

        End With
        Return ph
    End Function

    Private Function Meta(ByVal Name As String, ByVal strValue As String) As HtmlMeta
        Dim m As New HtmlMeta
        With m
            .Name = Name
            .Content = strValue
        End With
        Return m
    End Function

#End Region

#Region "Handlers"

    Private Sub hbtnEdit(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim section As XmlNode = _PageNode.SelectSingleNode(String.Format("section[@placeholder='{0}']", sender.CommandArgument))
        If Not IsNothing(section) Then
            For Each c As Control In PlaceHolders()
                With c
                    If String.Compare(.ID, sender.CommandArgument, True) = 0 Then
                        .Controls.Clear()
                        .Controls.Add(NewEditHTML(section.Attributes("html").Value))
                        Dim tb As Control = .FindControl("htmlTextBox")
                        .Controls.Add(New LiteralControl("<script type=""text/javascript"">sheet = document.getElementById(""ctl00_remove""); sheet.parentNode.removeChild(sheet);</script>"))
                        .Controls.Add(New LiteralControl(String.Format("<script type={0}text/javascript{0}>CKEDITOR.replace('{1}');</script>", Chr(34), tb.ClientID)))
                        .Controls.Add(NewHiddenField("act", "save"))
                        .Controls.Add(NewHiddenField("ph", section.Attributes("placeholder").Value))
                    End If
                End With
            Next
        End If
    End Sub

    Private Sub hbtnPageEdit(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim ddl As New DropDownList
        With ddl
            .ID = "masterpage"
            Dim dirInfo As New DirectoryInfo(cmsData.rootpath)
            .DataSource = dirInfo.GetFiles("*.master")
            .DataBind()
            .SelectedValue = _PageNode.Attributes("masterpage").Value
        End With

        Dim Title As New TextBox
        With Title
            .ID = "title"
            .Text = _PageNode.Attributes("title").Value
            .Width = 400
        End With

        Dim part As New DropDownList
        With part
            .ID = "part"

            Dim nit As New ListItem
            With nit
                .Value = "NOPART"
                .Text = "No part"
            End With
            .Items.Add(nit)
            For Each node As XmlNode In cmsData.part.SelectNodes("/*[position()=1]/PARTS/PART[not(@DELIVERY)]")
                Dim it As New ListItem
                With it
                    .Value = node.SelectSingleNode("PARTNAME").InnerText
                    .Text = node.SelectSingleNode("PARTNAME").InnerText & " - " & node.SelectSingleNode("PARTDES").InnerText
                End With
                .Items.Add(it)
            Next

            Try
                If _PageNode.Attributes("part").Value.Length > 0 Then
                    .SelectedValue = _PageNode.Attributes("part").Value
                Else
                    .SelectedValue = "NOPART"
                End If
            Catch
                .SelectedValue = "NOPART"
            End Try
        End With

        Dim Description As New TextBox
        With Description
            .ID = "description"
            .TextMode = TextBoxMode.MultiLine
            .Text = _PageNode.Attributes("description").Value
            .Width = 400
            .Height = 100
        End With

        Dim Keywords As New TextBox
        With Keywords
            .ID = "keywords"
            .TextMode = TextBoxMode.MultiLine
            .Text = _PageNode.Attributes("keywords").Value
            .Width = 400
            .Height = 100
        End With

        Dim BoxCount As New TextBox
        With BoxCount
            .ID = "boxcount"
            .Width = 100
            Try
                If _PageNode.Attributes("boxcount").Value.Length > 0 Then
                    .Text = _PageNode.Attributes("boxcount").Value
                Else
                    .Text = ""
                End If
            Catch
                .Text = ""
            End Try
        End With

        With _thisPage.Form.FindControl("PageEdit").Controls
            .Clear()
            .Add(New LiteralControl("<div class='adminbar'>"))
            .Add(New LiteralControl(String.Format("<table ><tr><td width={0}100%{0}>", Chr(34))))

            If _thisPage.User.IsInRole("webmaster") Then

                Dim hl As New HyperLink()
                With hl
                    .Text = "Edit Menu"
                    .NavigateUrl = "cat.aspx"
                    .ImageUrl = "~/ckeditor/btn/btnEditMenu.png"
                End With
                .Add(hl)

                '.Controls.Add(New LiteralControl("&nbsp;&nbsp;"))

                Dim fhl As New HyperLink()
                With fhl
                    .Text = "Edit Resources"
                    .NavigateUrl = "files.aspx"
                    .ImageUrl = "~/ckeditor/btn/btnEditRes.png"
                End With
                .Add(fhl)

                '.Controls.Add(New LiteralControl("&nbsp;&nbsp;"))

                Dim link As New ImageButton
                With link
                    .ImageUrl = "~/ckeditor/btn/btnSaveChanges.png"
                End With
                .Add(link)

            End If

            .Add(New LiteralControl(String.Format("</td><td NOWRAP>", Chr(34))))
            Dim conf As New HyperLink()
            With conf
                .Text = "Web Configuration"
                .NavigateUrl = "config.aspx"
                .ImageUrl = "~/ckeditor/btn/btnConfig.png"
            End With
            .Add(conf)

            Dim memb As New HyperLink()
            With memb
                .Text = "Membership"
                .NavigateUrl = "members.aspx"
                .ImageUrl = "~/ckeditor/btn/btnMembership.png"
            End With
            .Add(memb)

            .Add(New LiteralControl(String.Format("</td></tr></table>", Chr(34))))

            .Add(New LiteralControl("<hr width='100%'>"))

            .Add(NewHiddenField("act", "save"))
            .Add(NewHiddenField("ph", "page"))

            Dim T As New Table
            With T
                .Controls.Add(LabelRow("Title", Title))
                .Controls.Add(LabelRow("Master Page", ddl))
                .Controls.Add(LabelRow("Part", part))
                .Controls.Add(LabelRow("Box Count", BoxCount))
                .Controls.Add(LabelRow("Description", Description))
                .Controls.Add(LabelRow("Keywords", Keywords))
            End With
            .Add(T)

            .Add(New LiteralControl("<hr width='100%'>"))

            .Add(New LiteralControl(String.Format("</div>", Chr(34))))


        End With

    End Sub

#End Region

End Class
