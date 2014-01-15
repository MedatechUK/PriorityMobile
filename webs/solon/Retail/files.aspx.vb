Imports System.IO

Partial Class files
    Inherits System.Web.UI.Page

    Private rootdir As String

    Protected Sub form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

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

        rootdir = Server.MapPath("")
        If Not Page.IsPostBack Then
            With Me.TreeView1
                .Nodes.Clear()
                .Nodes.Add(New TreeNode("Root", rootdir & "\my_documents"))
                process(rootdir & "\my_documents", .Nodes(0))
            End With
        End If
    End Sub

    Sub process(ByVal dir As String, ByVal node As TreeNode)
        With Me.TreeView1
            Dim dirobj As New DirectoryInfo(dir)
            Dim dirs As DirectoryInfo() = dirobj.GetDirectories("*.*")
            For Each DirectoryName As DirectoryInfo In dirs
                node.ChildNodes.Add(New TreeNode(DirectoryName.Name, DirectoryName.FullName))
                process(DirectoryName.FullName, node.ChildNodes(node.ChildNodes.Count - 1))
            Next

        End With
    End Sub

    Protected Sub TreeView1_SelectedNodeChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TreeView1.SelectedNodeChanged
        FileUpload.Visible = False
        FileInfo.Visible = False
        CreateFolder.Visible = False
        With Me.ListBox1
            .Items.Clear()
            Dim dirobj As New DirectoryInfo(TreeView1.SelectedNode.Value)
            Dim files As FileInfo() = dirobj.GetFiles("*.*")
            For Each fn As FileInfo In files
                Dim li As New ListItem(fn.Name, Replace(Split(TreeView1.SelectedNode.Value & "\" & fn.Name, rootdir & "\", , CompareMethod.Text)(1), "\", "/"))
                .Items.Add(li)
            Next
        End With
    End Sub

    Protected Sub ListBox1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ListBox1.SelectedIndexChanged

        CreateFolder.Visible = False
        FileUpload.Visible = False
        FileInfo.Visible = True

        If Not IsNothing(ListBox1.SelectedValue) Then
            Me.delete.Enabled = True
            Dim tf As Literal = FileInfo.FindControl("fileURL")
            tf.Text = ListBox1.SelectedValue
            Dim p As Image = FileInfo.FindControl("imagepreview")
            Dim hl As HyperLink = FileInfo.FindControl("Filelink")
            Select Case Right(tf.Text.ToLower, 3)
                Case "png", "jpg", "bmp", "gif"
                    p.Visible = True
                    hl.Visible = False
                    p.ImageUrl = ListBox1.SelectedValue
                Case Else
                    p.Visible = False
                    hl.Visible = True
                    hl.Target = "_blank"
                    hl.Text = "Open&nbsp;document:&nbsp;" & ListBox1.SelectedItem.Text
                    hl.NavigateUrl = ListBox1.SelectedValue
            End Select
        End If

    End Sub

    Protected Sub upload_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles upload.Click
        FileUpload.Visible = True
        FileInfo.Visible = False
        CreateFolder.Visible = False
        For Each li As ListItem In Me.ListBox1.Items
            li.Selected = False
        Next
    End Sub

    Protected Sub newfolder_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles newfolder.Click
        CreateFolder.Visible = True
        FileUpload.Visible = False
        FileInfo.Visible = False
        For Each li As ListItem In Me.ListBox1.Items
            li.Selected = False
        Next
    End Sub

    Protected Sub doUpload_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles doUpload.Click
        If FileUpload1.HasFile Then
            Try
                Dim li As New ListItem
                li.Text = FileUpload1.FileName
                li.Value = Replace(Split(TreeView1.SelectedNode.Value & "\" & FileUpload1.FileName, rootdir & "\", , CompareMethod.Text)(1), "\", "/")

                FileUpload1.SaveAs(TreeView1.SelectedValue & _
                   "\" & FileUpload1.FileName)

                ListBox1.Items.Add(li)
                erLabel.Text = ""

            Catch ex As Exception
                erLabel.Text = "ERROR: " & ex.Message.ToString()
            End Try
        Else
            erLabel.Text = "You have not specified a file."
        End If
    End Sub

    Protected Sub CancelUpload_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles CancelUpload.Click, CancelNewFolder.Click
        FileUpload.Visible = False
        CreateFolder.Visible = False
        FileInfo.Visible = False
    End Sub

    Protected Sub MakeFolder_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles MakeFolder.Click
        Dim tf As TextBox = CreateFolder.FindControl("txtNewFolder")
        If Not IsNothing(tf) Then
            If tf.Text.Length > 0 Then
                With Me.TreeView1
                    Try
                        FileSystem.MkDir(.SelectedValue & "\" & tf.Text)
                        .SelectedNode.ChildNodes.Add(New TreeNode(tf.Text, .SelectedValue & "\" & tf.Text))
                        erLabel.Text = ""
                        FileUpload.Visible = False
                        CreateFolder.Visible = False
                        FileInfo.Visible = False
                    Catch ex As Exception
                        erLabel.Text = ex.Message
                    End Try
                End With
            Else
                erLabel.Text = "You have not specified a folder."
            End If
        End If
    End Sub

    Protected Sub Delete_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles Delete.Click
        Dim fn As String = TreeView1.SelectedValue & "\" & ListBox1.SelectedItem.Text
        Try
            File.Delete(fn)
            ListBox1.Items.RemoveAt(ListBox1.SelectedIndex)
            FileInfo.Visible = False

        Catch ex As Exception
            erLabel.Text = "ERROR: " & ex.Message.ToString()
        End Try
    End Sub

End Class
