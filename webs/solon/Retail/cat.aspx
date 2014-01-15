<%@ Page Language="VB" MasterPageFile="admin.master" AutoEventWireup="false" CodeFile="cat.aspx.vb" Inherits="_Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
    
    <TEXTAREA ID="holdtext" STYLE="display:none;">
    </TEXTAREA>
    <SPAN ID="copytext" STYLE="height:1;width:1;display:none">
        <asp:Literal ID="FileURL" runat="server"></asp:Literal>                                                        
    </SPAN>

        <table class="style1">
            <tr>
                <td class="style2" valign="top" align="left" width="200">
    
                    <asp:TreeView ID="TreeView1" runat="server" DataSourceID="cat" 
                        ExpandDepth="1" ImageSet="Events">
                        <ParentNodeStyle Font-Bold="False" ImageUrl="~/ckeditor/btn/treeImgHTML.png" />
                        <HoverNodeStyle Font-Underline="False" ForeColor="Red" />
                        <SelectedNodeStyle Font-Underline="True" 
                        HorizontalPadding="0px" VerticalPadding="0px" 
                        ImageUrl="~/ckeditor/btn/treeImgHTML.png" />
            <DataBindings>
                <asp:TreeNodeBinding DataMember="cat" ValueField="id" 
                    SelectAction="SelectExpand" TextField="name" />
            </DataBindings>
            <RootNodeStyle ImageUrl="~/ckeditor/btn/treeImgMenu.png" />
            <NodeStyle Font-Names="Verdana" Font-Size="8pt" ForeColor="Black" 
                HorizontalPadding="5px" NodeSpacing="0px" VerticalPadding="0px" 
                ImageUrl="~/ckeditor/btn/treeImgHTML.png" />
        </asp:TreeView>
        <asp:XmlDataSource ID="cat" runat="server" DataFile="~/cat.xml" EnableCaching="False">
        </asp:XmlDataSource>
    
                </td>
                <td class="style4"></td>
                <td class="style2" valign="top">
                    <asp:FormView ID="FormView1" runat="server" DataSourceID="thiscat" Width="100%">
                        <EditItemTemplate>
                        <table class="style3">
                                <tr>
                                    <td class="style5">
                            <asp:imagebutton ID="UpdateButton" runat="server" CausesValidation="True" 
                                CommandName="doUpdate" Text="Update" 
                                ImageUrl="~/ckeditor/btn/btnSaveChanges.png" />
                            &nbsp;<asp:imagebutton ID="UpdateCancelButton" runat="server" 
                                CausesValidation="False" CommandName="Cancel" 
                                 Text="Cancel" ImageUrl="~/ckeditor/btn/btnCancel.png" /><hr>                                                    
</td></tr>                                 </table>
                            Name:
                            <asp:TextBox ID="NameTextBox" runat="server" Text='<%# Bind("Name") %>' />
                            <br />
                            Image:
                            <asp:TextBox ID="imgTextBox" runat="server" Text='<%# Bind("img") %>' />
                            <br />
                            Show on Menu:<asp:CheckBox ID="ShowOnMenu" runat="server" 
                                Checked='<%# Bind("showonmenu") %>' TextAlign="Left" />
                            <br />
                            <br />

                            <br />
                            <asp:TextBox ID="idTextBox" runat="server" Text='<%# Bind("id") %>' 
                                Visible="False" />
                        </EditItemTemplate>
                        <InsertItemTemplate>
                        <table class="style3">
                                <tr>
                                    <td class="style5">
                        <asp:imagebutton ID="InsertButton" runat="server" CausesValidation="True" 
                                CommandName="doInsert" Text="Insert" 
                                ImageUrl="~/ckeditor/btn/btnSaveChanges.png" />
                            &nbsp;<asp:imagebutton ID="InsertCancelButton" runat="server" 
                                CausesValidation="False" CommandName="Cancel" Text="Cancel" 
                                ImageUrl="~/ckeditor/btn/btnCancel.png" /><hr>
                                </td></t></table>
                            Name:
                            <asp:TextBox ID="NameTextBox" runat="server" Text='<%# Bind("Name") %>' />
                            <br />
                            Image:
                            <asp:TextBox ID="imgTextBox" runat="server" Text='<%# Bind("img") %>' />
                            <br />
                            Add at:&nbsp;<select ID="AddAt" name="AddAt">
                                <option>Bottom</option>
                                <option>Top</option>
                            </select><br />
                            Show on Menu:<asp:CheckBox ID="ShowOnMenu" runat="server" Checked="True" 
                                TextAlign="Left" />
                            <br />
                            <br />

                            &nbsp;<br />
                            <asp:TextBox ID="idTextBox" runat="server" 
                                 Text='<%# Bind("id") %>' 
                                Visible="False" />
                        </InsertItemTemplate>
                        <ItemTemplate>
                            <table class="style3">
                                <tr>
                                    <td class="style5">
                                        <asp:imagebutton ID="UpdateButton" runat="server" CausesValidation="True" 
                                            CommandName="edit" Text="Update" ImageUrl="~/ckeditor/btn/btnEdit.png" />
                                        <asp:imagebutton ID="UpdateCancelButton" runat="server" 
                                            CausesValidation="False" CommandName="addchild" Text="New Child" 
                                            ImageUrl="~/ckeditor/btn/btnNewChild.png" />
                                        <asp:imagebutton ID="moveupButton" runat="server" CausesValidation="False" 
                                            CommandArgument="up" CommandName="move" Text="Move Up" 
                                            visible="<%# btnupenabled %>" ImageUrl="~/ckeditor/btn/btnMoveUp.png"/>
                                        <asp:imagebutton ID="movedownButton" runat="server" CausesValidation="False" 
                                            CommandArgument="down" CommandName="move" Text="Move Down" 
                                            visible="<%# btndownenabled %>" ImageUrl="~/ckeditor/btn/btnMoveDown.png"/>
                                        <input type="image" src="ckeditor/btn/btnCopyURL.png" onClick="window.clipboardData.setData( 'text' , copytext.innerText);" alt="Copy URL">
                                    </td>
                                    <td style="text-align: right">
                                        <asp:imagebutton ID="deleteButton" runat="server" CausesValidation="False" 
                                            CommandName="dodelete" style="text-align: right" 
                                            Text="Delete" visible="<%# btndelenabled %>" 
                                            ImageUrl="~/ckeditor/btn/btnDelete.png"/>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style5" colspan="2">
                                        <hr></hr>
                                    </td>
                                </tr>
                            </table>Name:
                            <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# Eval("id") %>' 
                                Text='<%# Eval("name") %>'></asp:HyperLink>
                            &nbsp;&nbsp;

                            <br />
                            Image:
                            <asp:Label ID="imgLabel" runat="server" Text='<%# Bind("img") %>' />
                            <br />
                            Show on Menu:<asp:CheckBox ID="ShowOnMenu" runat="server" 
                                Checked='<%# Bind("showonmenu") %>' Enabled="False" TextAlign="Left" />
                            <br />
                            <br />
                            &nbsp;&nbsp;<br />
                        </ItemTemplate>
                    </asp:FormView>
                    <asp:XmlDataSource ID="thiscat" runat="server" DataFile="~/cat.xml" 
                        EnableCaching="False">
                    </asp:XmlDataSource>
                </td>
            </tr>
            <tr>
                <td class="style2" valign="top" align="left" width="200">
    
                    &nbsp;</td>
                <td class="style4">&nbsp;</td>
                <td class="style2" valign="top">
                    &nbsp;</td>
            </tr>
        </table>
    <br/>
</asp:Content>