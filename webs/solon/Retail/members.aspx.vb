Imports System.Xml

Partial Class members
    Inherits System.Web.UI.Page

    Dim lnktbl As Table
    Dim tblu As Table
    Dim linkcell As TableCell
    Dim tbl As Table

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

        Dim Display As String = Nothing
        Dim hl As HyperLink
        Dim lb As Label
        Dim lb2 As Label
        Dim tx As TextBox
        Dim ch As CheckBox
        Dim btn As Button

        If Request.Params.AllKeys.Contains("User") Then

            Me.MultiView1.ActiveViewIndex = 1

            Dim usr As String = Request("User")
            Dim p As ProfileBase = Profile.GetProfile(usr)

            lb = New Label
            With lb
                .Text = "User Name: "
                .Font.Name = "Verdana"
            End With
            lb2 = New Label
            With lb2
                .Font.Name = "Verdana"
                .Text = p.UserName
            End With
            addUserRow(lb, lb2)

            lb = New Label
            With lb
                .Text = "Last Activity: "
                .Font.Name = "Verdana"
            End With
            lb2 = New Label
            With lb2
                .Font.Name = "Verdana"
                .Text = p.LastActivityDate
            End With
            addUserRow(lb, lb2)

            Dim doc As New XmlDocument
            doc.Load(Server.MapPath("/") & "web.config")

            If Request.Params.AllKeys.Contains("Section") Then
                Display = Request("Section")
            End If

            Dim imp As New LinkButton
            With imp
                .Text = "Impersonate"
                .Font.Name = "Verdana"
                AddHandler .Click, AddressOf Impersonate_Click
            End With
            addlinkbutton(imp)

            Dim prof As XmlNode = doc.SelectSingleNode("//profile/properties")
            hl = New HyperLink
            With hl
                .Text = "Profile"
                .Font.Name = "Verdana"
                .NavigateUrl = String.Format("?User={0}&Section={1}", usr, "Default")
            End With
            addlink(hl)

            If IsNothing(Display) Or Display = "Default" Then
                For Each el As XmlNode In prof.SelectNodes("add")

                    lb = New Label
                    lb.Font.Name = "Verdana"
                    lb.Text = NameEl(el)

                    tx = New TextBox
                    tx.ID = NameEl(el)
                    tx.Text = p.GetPropertyValue(NameEl(el))

                    addRow(lb, tx)

                Next

                btn = New Button
                btn.ID = "btn_SaveProfile"
                btn.Text = "Save Profile"
                AddHandler btn.Click, AddressOf hSaveProfile

                addRow(Nothing, btn)

            End If

            For Each gr As XmlNode In prof.SelectNodes("group")
                hl = New HyperLink
                With hl
                    .Text = NameEl(gr)
                    .Font.Name = "Verdana"
                    .NavigateUrl = String.Format("?User={0}&Section={1}", usr, NameEl(gr))
                End With
                addlink(hl)


                If Display = NameEl(gr) Then
                    For Each el As XmlNode In gr.SelectNodes("add")

                        lb = New Label
                        lb.Text = NameEl(el)
                        lb.Font.Name = "Verdana"

                        tx = New TextBox
                        tx.ID = NameEl(gr) & "_" & NameEl(el)
                        tx.Text = p.GetPropertyValue(NameEl(gr) & "." & NameEl(el))

                        addRow(lb, tx)

                    Next

                    btn = New Button
                    btn.ID = "btn_SaveProfile"
                    btn.Text = "Save " & NameEl(gr)
                    AddHandler btn.Click, AddressOf hSaveProfile

                    addRow(Nothing, btn)

                End If

            Next

            hl = New HyperLink
            With hl
                .Font.Name = "Verdana"
                .Text = "Roles"
                .NavigateUrl = String.Format("?User={0}&Section={1}", usr, "Roles")
            End With
            addlink(hl)

            If Display = "Roles" Then
                For Each r As String In Roles.GetAllRoles

                    lb = New Label
                    lb.Font.Name = "Verdana"
                    lb.Text = r

                    ch = New CheckBox
                    With ch
                        .ID = r
                        .Checked = Roles.IsUserInRole(usr, r)
                        .AutoPostBack = True
                        AddHandler .CheckedChanged, AddressOf hRoleChanged
                    End With

                    addRow(lb, ch)

                Next
            End If

            hl = New HyperLink
            With hl
                .Text = "Security"
                .Font.Name = "Verdana"
                .NavigateUrl = String.Format("?User={0}&Section={1}", usr, "Security")
            End With
            addlink(hl)

            If Display = "Security" Then
                Dim mu As MembershipUser = Membership.GetUser(usr)

                lb = New Label
                lb.Font.Name = "Verdana"
                lb.Text = "Is Approved"

                ch = New CheckBox
                ch.ID = "IsApproved"
                ch.Checked = mu.IsApproved
                ch.AutoPostBack = True
                AddHandler ch.CheckedChanged, AddressOf hApprovedChange

                addRow(lb, ch)

                lb = New Label
                lb.Font.Name = "Verdana"
                lb.Text = "Is Locked Out"

                ch = New CheckBox
                With ch
                    .ID = "IsLockedOut"
                    .Checked = mu.IsLockedOut
                    .AutoPostBack = True
                    .Enabled = mu.IsLockedOut
                    AddHandler .CheckedChanged, AddressOf hLockedChange
                End With
                addRow(lb, ch)

                lb = New Label
                lb.Text = "Reset Password"
                lb.Font.Name = "Verdana"

                tx = New TextBox
                tx.ID = "NewPassword"                
                tx.Text = ""

                addRow(lb, tx)

                btn = New Button
                btn.ID = "btn_NewPassword"
                btn.Text = "Set Password"
                AddHandler btn.Click, AddressOf hSetPassword

                addRow(Nothing, btn)

            End If


        Else
            Me.MultiView1.ActiveViewIndex = 0
        End If

    End Sub

    Private Function NameEl(ByVal Node As XmlNode) As String
        Return Node.Attributes("name").Value
    End Function

    Private Sub addlink(ByVal hl As HyperLink)
        If IsNothing(lnktbl) Then
            lnktbl = Master.FindControl("main").FindControl("tbl_Links")
        End If
        If Not IsNothing(lnktbl) Then
            linkcell = lnktbl.Rows(0).Cells(0)
        End If
        If Not IsNothing(linkcell) Then
            linkcell.Controls.Add(hl)
            Dim L As New Literal
            L.Text = "&nbsp;&nbsp;"
            linkcell.Controls.Add(L)
        End If
    End Sub

    Private Sub addlinkbutton(ByVal hl As LinkButton)
        If IsNothing(lnktbl) Then
            lnktbl = Master.FindControl("Main").FindControl("tbl_Links")
        End If
        If Not IsNothing(lnktbl) Then
            linkcell = lnktbl.Rows(0).Cells(0)
        End If
        If Not IsNothing(linkcell) Then
            linkcell.Controls.Add(hl)
            Dim L As New Literal
            L.Text = "&nbsp;&nbsp;"
            linkcell.Controls.Add(L)
        End If
    End Sub

    Private Sub addRow(ByVal Lb As Label, ByVal ctrl As System.Web.UI.Control)

        If IsNothing(tbl) Then
            Dim ph As ContentPlaceHolder = Master.FindControl("Main")
            tbl = ph.FindControl("tbl_Profile")
        End If

        With tbl.Rows
            .Add(New TableRow)
            With .Item(.Count - 1).Cells
                .Add(New TableCell)
                With .Item(.Count - 1)
                    If Not IsNothing(Lb) Then
                        .Controls.Add(Lb)
                    End If
                End With
            End With
            With .Item(.Count - 1).Cells
                .Add(New TableCell)
                With .Item(.Count - 1)
                    .Controls.Add(ctrl)
                End With
            End With
        End With

    End Sub

    Private Sub addUserRow(ByVal Lb As Label, ByVal ctrl As System.Web.UI.Control)

        If IsNothing(tblu) Then
            Dim ph As ContentPlaceHolder = Master.FindControl("Main")
            tblu = ph.FindControl("tbl_User")
        End If

        With tblu.Rows
            .Add(New TableRow)
            With .Item(.Count - 1).Cells
                .Add(New TableCell)
                With .Item(.Count - 1)
                    'If Not IsNothing(Lb) Then
                    .Controls.Add(Lb)
                    'End If
                End With
            End With
            With .Item(.Count - 1).Cells
                .Add(New TableCell)
                With .Item(.Count - 1)
                    .Controls.Add(ctrl)
                End With
            End With
        End With

    End Sub

    Protected Sub hRoleChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim ch As CheckBox = sender
        If Roles.IsUserInRole(Request("User"), ch.ID) Then
            Roles.RemoveUserFromRole(Request("User"), ch.ID)
        Else
            Dim un() As String = {Request("User")}
            Roles.AddUsersToRole(un, ch.ID)
        End If        
    End Sub

    Protected Sub hApprovedChange(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim Ch As CheckBox = sender
        Dim mu As MembershipUser = Membership.GetUser(Request("User"))
        mu.IsApproved = Ch.Checked
        Membership.UpdateUser(mu)
    End Sub

    Protected Sub hLockedChange(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim Ch As CheckBox = sender
        Dim mu As MembershipUser = Membership.GetUser(Request("User"))
        If Not Ch.Checked Then mu.UnlockUser()
        Membership.UpdateUser(mu)
    End Sub

    Protected Sub hSetPassword(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim NewPassword As TextBox = FindControl("NewPassword")        
        Dim mu As MembershipUser = Membership.GetUser(Request("User"))
        mu.ChangePassword(mu.GetPassword, NewPassword.Text)
        Membership.UpdateUser(mu)
        NewPassword.Text = ""
    End Sub

    Protected Sub hSaveProfile(ByVal sender As Object, ByVal e As System.EventArgs)
        If IsNothing(tbl) Then
            tbl = FindControl("tbl_Profile")
        End If
        Dim p As ProfileBase = Profile.GetProfile(Request("User"))
        For Each row As TableRow In tbl.Rows
            For Each cl As TableCell In row.Cells
                For Each ctrl As Control In cl.Controls
                    Select Case ctrl.ToString
                        Case "System.Web.UI.WebControls.TextBox"
                            Dim tb As System.Web.UI.WebControls.TextBox = ctrl
                            p.SetPropertyValue(Replace(tb.ID, "_", "."), tb.Text)
                    End Select
                Next
            Next
        Next
        p.Save()
    End Sub

    Protected Sub GridView1_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridView1.RowCommand
        Select Case e.CommandName.ToLower
            Case "removeuser"

                Dim gv As GridView = sender
                Dim hl As System.Web.UI.WebControls.HyperLink = gv.Rows(e.CommandArgument).Cells(0).Controls(0)
                Dim mu As MembershipUser = Membership.GetUser(hl.Text)

                If Roles.GetRolesForUser(mu.UserName).Count = 0 Then
                    Membership.DeleteUser(mu.UserName)
                    Response.Redirect("members.aspx")
                Else
                    errText.Text = String.Format("User {0} has roles assigned. Revoke roles to delete.", mu.UserName)
                End If
        End Select
    End Sub

    Protected Sub CreateUserWizard1_CreatedUser(ByVal sender As Object, ByVal e As System.EventArgs) Handles CreateUserWizard1.CreatedUser
        Response.Redirect("members.aspx")
    End Sub

    Protected Sub CreateUserWizard1_CreatingUser(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.LoginCancelEventArgs) Handles CreateUserWizard1.CreatingUser
        Dim cuw As CreateUserWizard = sender
        cuw.Email = cuw.UserName
    End Sub

    Protected Sub ImageButton4_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton4.Click
        Me.MultiView1.ActiveViewIndex = 2
    End Sub

    Protected Sub Impersonate_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        FormsAuthentication.SetAuthCookie(Request("user"), True)
        Response.Redirect("/")
    End Sub
End Class
