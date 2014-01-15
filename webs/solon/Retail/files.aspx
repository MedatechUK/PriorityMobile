<%@ Page Language="VB" MasterPageFile="admin.master" AutoEventWireup="false" CodeFile="files.aspx.vb" Inherits="files" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">

<TEXTAREA ID="holdtext" STYLE="width:0px;height:0px;padding:0">
</TEXTAREA>
        
        <table style="width: 500px">
        <tr><td rowspan="2" valign="top">
                    <asp:TreeView ID="TreeView1" runat="server" ImageSet="Events" 
                        Width="200px">
                        <ParentNodeStyle Font-Bold="False" ImageUrl="~/ckeditor/btn/treeImgNewFolder.png" />
                        <HoverNodeStyle Font-Underline="False" ForeColor="Red" />
                        <SelectedNodeStyle Font-Underline="True" 
                            HorizontalPadding="0px" VerticalPadding="0px" 
                            ImageUrl="~/ckeditor/btn/treeImgNewFolder.png" />
                        <RootNodeStyle ImageUrl="~/ckeditor/btn/treeImgResource.png" />
                        <NodeStyle Font-Names="Verdana" Font-Size="8pt" ForeColor="Black" 
                            HorizontalPadding="5px" NodeSpacing="0px" VerticalPadding="0px" 
                            ImageUrl="~/ckeditor/btn/treeImgNewFolder.png" />
                    </asp:TreeView>
                </td><td style="vertical-align: top; width: 400px;">
                                <asp:ImageButton ID="upload" runat="server" 
                                    ImageUrl="~/ckeditor/btn/btnUploadFile.png" style="text-align: left" />
                                <asp:ImageButton ID="newfolder" runat="server" 
                                    ImageUrl="~/ckeditor/btn/btnNewFolder.png" Width="88px" />
                </td><td width="50">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;</td><td rowspan="2">
                    <asp:Panel ID="FileInfo" runat="server" Height="141px" CssClass="style1" 
                        Visible="False" Width="350px">
                        <INPUT TYPE="image" SRC="../ckeditor/btn/btnCopyURL.png" onClick="window.clipboardData.setData( 'text' , copytext.innerText);" ALT="Copy URL">

                        &nbsp;
                        
                            <asp:ImageButton ID="Delete" runat="server" ImageUrl="~/ckeditor/btn/btnDElete.png" />
                            <br />
                            <hr width="300" align="left"/>
                            File Info:<br />
                            <span id="copytext" style="height:150;width:162;">
                                <asp:Literal ID="FileURL" runat="server"></asp:Literal>
                            </span>
                        <br />
                            <br />
                        <asp:Image ID="imagepreview" runat="server" Visible="False" 
                                AlternateText="Image Preview" BorderStyle="Dotted" BorderWidth="2px" />
                        <asp:HyperLink ID="Filelink" runat="server" Visible="False">HyperLink</asp:HyperLink>
                        <br />
                        
                    </asp:Panel>
                    <asp:Panel ID="FileUpload" runat="server" Height="132px" CssClass="style1" 
                        Visible="False" Width="350px">
                        Upload File:<br />
                        <asp:FileUpload ID="FileUpload1" runat="server" Width="216px" />
                        <br />
                        <br />
                        &nbsp;<asp:ImageButton ID="doUpload" runat="server" 
                            ImageUrl="~/ckeditor/btn/btnUploadFile.png" />
                        <asp:ImageButton ID="CancelUpload" runat="server" 
                            ImageUrl="~/ckeditor/btn/btnCancel.png" />
                    
                    </asp:Panel>
                                        <asp:Panel ID="CreateFolder" runat="server" Height="132px" CssClass="style1" 
                        Visible="False" Width="350px">
                                            New Folder Name:<br />
                                            <asp:TextBox ID="txtNewFolder" runat="server" Width="350px"></asp:TextBox>
                        <br />
                        <br />
                        &nbsp;<asp:ImageButton ID="MakeFolder" runat="server" ImageUrl="~/ckeditor/btn/btnNewFolder.png" />
                                            <asp:ImageButton ID="CancelNewFolder" runat="server" 
                                                ImageUrl="~/ckeditor/btn/btnCancel.png" />
                    
                    </asp:Panel>
            <asp:Label ID="erLabel" runat="server" Font-Names="Verdana" ForeColor="#CC0000"></asp:Label>
            </td></tr>
            <tr>
                <td valign="Top" width="200">
                    <asp:ListBox ID="ListBox1" runat="server" Height="334px" Width="220px" 
                        AutoPostBack="True" CssClass="style1">
                    </asp:ListBox>
                </td>
                <td valign="Top">
                    &nbsp;
                </td>
            </tr>
            </table>
    <BR/>
</asp:Content>