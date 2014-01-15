Imports System.Xml
Imports System.IO
Imports cmSi

Partial Class _Default
    Inherits System.Web.UI.Page
    Private FirstNode As String = "/*[position()=1]"

    Protected ReadOnly Property btndelenabled() As Boolean
        Get
            Return Not (node.HasChildNodes)
        End Get
    End Property

    Protected ReadOnly Property btnupenabled() As Boolean
        Get
            Try
                Return Not (node.Attributes("id").Value = node.ParentNode.FirstChild.Attributes("id").Value)
            Catch
                Return False
            End Try
        End Get
    End Property

    Protected ReadOnly Property btndownenabled() As Boolean
        Get
            Try
                Return Not (node.Attributes("id").Value = node.ParentNode.LastChild.Attributes("id").Value)
            Catch
                Return False
            End Try
        End Get
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.User.IsInRole("webmaster") Then
            Response.Redirect(cmSi.cmsData.Settings.Get("url") & "/login.aspx")
        End If

        Dim img As System.Web.UI.WebControls.Image = Master.FindControl("MainLogo")
        If Not IsNothing(img) Then
            With img
                .AlternateText = cmSi.cmsData.Settings("MainLogo")
                .ImageUrl = "my_documents/images/main-logo.png"
            End With
        End If

        Dim lit As System.Web.UI.WebControls.Literal = Master.FindControl("WebName")
        If Not IsNothing(lit) Then
            With lit
                .Text = cmSi.cmsData.Settings("WebName")
            End With
        End If

        If IsNothing(TreeView1.SelectedNode) Then
            TreeView1.DataBind()
            TreeView1.Nodes(0).Selected = True
            thiscat.XPath = FirstNode
        End If

    End Sub

    Protected Sub TreeView1_SelectedNodeChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TreeView1.SelectedNodeChanged
        thiscat.XPath = String.Format("//cat[@id='{0}']", TreeView1.SelectedNode.Value)
        Dim ph As ContentPlaceHolder = Master.FindControl("Main")
        Dim tf As Literal = ph.FindControl("fileURL")
        tf.Text = TreeView1.SelectedNode.Value
    End Sub

    Private Function node() As XmlNode
        Return cmSi.cmsData.cat.SelectSingleNode(String.Format("//cat[@id='{0}']", TreeView1.SelectedNode.Value))
    End Function

    Protected Sub FormView1_ItemCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.FormViewCommandEventArgs) Handles FormView1.ItemCommand

        If Not IsNothing(TreeView1.SelectedNode) Then
            thiscat.XPath = String.Format("//cat[@id='{0}']", TreeView1.SelectedNode.Value)
        Else
            thiscat.XPath = FirstNode
        End If

        Dim name As TextBox = Me.FormView1.FindControl("NameTextBox")
        Dim img As TextBox = Me.FormView1.FindControl("ImgTextBox")
        Dim show As CheckBox = Me.FormView1.FindControl("showonmenu")

        Select Case e.CommandName.ToLower
            Case "cancel"
                FormView1.ChangeMode(FormViewMode.ReadOnly)

            Case "edit"
                FormView1.ChangeMode(FormViewMode.Edit)

            Case "addchild"
                FormView1.ChangeMode(FormViewMode.Insert)

            Case "doinsert"
                If name.Text.Length > 0 Then
                    Dim newID As String = System.Guid.NewGuid.ToString
                    Dim NewTreeNode As TreeNode = New TreeNode(name.Text, newID)

                    Select Case Request.Form("AddAt")
                        Case "Top"
                            node.PrependChild(NewCategory(newID, name.Text, img.Text, show.Checked))
                            TreeView1.SelectedNode.ChildNodes.AddAt(0, NewTreeNode)
                        Case "Bottom"
                            node.AppendChild(NewCategory(newID, name.Text, img.Text, show.Checked))
                            TreeView1.SelectedNode.ChildNodes.Add(NewTreeNode)
                    End Select
                    cmSi.cmsData.cat.Save(cmSi.cmsData.catPath)

                    FormView1.ChangeMode(FormViewMode.ReadOnly)
                    NewTreeNode.Parent.ExpandAll()
                End If

            Case "doupdate"
                With node()
                    .Attributes("name").Value = name.Text
                    .Attributes("img").Value = img.Text
                    .Attributes("showonmenu").Value = show.Checked
                End With
                cmSi.cmsData.cat.Save(cmSi.cmsData.catPath)

                TreeView1.SelectedNode.Text = name.Text

                FormView1.ChangeMode(FormViewMode.ReadOnly)

            Case "dodelete"
                Dim thisParent As XmlNode = node.ParentNode
                If node.ChildNodes.Count = 0 Then
                    Dim this As XmlNode = node.ParentNode
                    node.ParentNode.RemoveChild(node)
                    cmSi.cmsData.cat.Save(cmSi.cmsData.catPath)
                    drawtree(thisParent)
                    thiscat.XPath = String.Format("//cat[@id='{0}']", thisParent.Attributes("id").Value)
                Else
                    MsgBox("Cannot remove node " & node.Attributes("name").Value & "." & vbCrLf & "Node has children.", MsgBoxStyle.OkOnly, "Delete")
                End If

            Case "move"
                Dim thisParent As XmlNode = node.ParentNode
                Dim this As XmlNode = node.Clone
                Dim tmp As XmlNode = NewCategory(System.Guid.NewGuid.ToString, "Temp", "", True)
                Dim swap As XmlNode = Nothing

                With thisParent
                    Select Case e.CommandArgument
                        Case "up"
                            swap = node.PreviousSibling.Clone
                            .ReplaceChild(tmp, node.PreviousSibling)
                        Case "down"
                            swap = node.NextSibling.Clone
                            .ReplaceChild(tmp, node.NextSibling)
                    End Select
                    .ReplaceChild(swap, node)
                    .ReplaceChild(this, tmp)
                End With

                cmSi.cmsData.cat.Save(cmSi.cmsData.catPath)

                drawtree(this)
                thiscat.XPath = String.Format("//cat[@id='{0}']", this.Attributes("id").Value)
        End Select

    End Sub

    Private Sub drawtree(ByVal this As XmlNode)
        Dim NewTreeNode As TreeNode
        Dim homeNode As XmlNode = cmSi.cmsData.cat.SelectSingleNode(FirstNode)
        With TreeView1
            .Nodes.Clear()
            NewTreeNode = New TreeNode(homeNode.Attributes("name").Value, homeNode.Attributes("id").Value)
            .Nodes.Add(NewTreeNode)
        End With

        reDrawTree(homeNode, NewTreeNode, this)
        NewTreeNode.ExpandAll()
    End Sub

    Private Sub reDrawTree(ByVal node As XmlNode, ByVal Parent As TreeNode, ByVal swap As XmlNode)
        With TreeView1

            For Each n As XmlNode In node.ChildNodes
                Dim NewTreeNode As TreeNode = New TreeNode(n.Attributes("name").Value, n.Attributes("id").Value)
                Parent.ChildNodes.Add(NewTreeNode)
                If n.Attributes("id").Value = swap.Attributes("id").Value Then
                    NewTreeNode.Selected = True
                End If
                reDrawTree(n, NewTreeNode, swap)
            Next

        End With
    End Sub

    Private Sub UpdateTreeNodes(ByVal Nodes As TreeNodeCollection, ByVal this As XmlNode, ByVal swap As XmlNode)
        For Each n As TreeNode In Nodes
            With n
                If .Value = swap.Attributes("id").Value Then
                    .Value = this.Attributes("id").Value
                    .Text = this.Attributes("name").Value

                    .Selected = True
                ElseIf .Value = this.Attributes("id").Value Then
                    .Value = swap.Attributes("id").Value
                    .Text = swap.Attributes("name").Value
                Else
                    UpdateTreeNodes(n.ChildNodes, this, swap)
                End If
            End With
        Next

    End Sub

    Private Function NewCategory(ByVal ID As String, ByVal Name As String, ByVal Image As String, ByVal showonmenu As Boolean) As XmlElement

        Dim Ch As System.Xml.XmlElement
        Ch = cmSi.cmsData.cat.CreateElement("cat")
        With Ch
            .SetAttribute("id", ID)
            .SetAttribute("name", Name)
            .SetAttribute("img", Image)
            .SetAttribute("showonmenu", showonmenu)
        End With
        Return Ch

    End Function

End Class
