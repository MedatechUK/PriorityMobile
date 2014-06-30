<%@ Page Language="VB" MasterPageFile="admin.master" AutoEventWireup="false" CodeFile="members.aspx.vb" Inherits="members" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">

    <asp:MultiView ID="MultiView1" runat="server">
        <asp:View ID="View1" runat="server">
            <div>
                <asp:ImageButton ID="ImageButton4" runat="server" 
                    ImageUrl="~/ckeditor/btn/btnAddMember.png" />
                <br />
                <asp:GridView ID="GridView1" runat="server" AllowPaging="True" 
                    AllowSorting="True" AutoGenerateColumns="False" CellPadding="4" 
                    DataSourceID="SqlDataSource1" ForeColor="#333333" GridLines="None" 
                    style="font-family: Verdana; font-size: small">
                    <RowStyle BackColor="#EFF3FB" />
                    <Columns>
                        <asp:HyperLinkField DataNavigateUrlFields="UserName" 
                            DataNavigateUrlFormatString="?User={0}" DataTextField="UserName" 
                            HeaderText="User" NavigateUrl="~/members.aspx" />
                        <asp:BoundField DataField="CreateDate" HeaderText="CreateDate" />
                        <asp:BoundField DataField="LastActivityDate" HeaderText="LastActivityDate" />
                        <asp:CheckBoxField DataField="IsApproved" HeaderText="IsApproved" />
                        <asp:CheckBoxField DataField="IsLockedOut" HeaderText="IsLockedOut" />
                        <asp:ButtonField CommandName="RemoveUser" Text="Delete" ButtonType="Image" 
                            ImageUrl="~/ckeditor/btn/btnDeleteUser.png" />
                    </Columns>
                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <EditRowStyle BackColor="#2461BF" />
                    <AlternatingRowStyle BackColor="White" />
                </asp:GridView>
            </div>
            <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
                ConnectionString="<%$ ConnectionStrings:LocalSqlServer %>" 
                SelectCommand="SELECT [UserName], [CreateDate] ,[LastActivityDate],[IsLockedOut],[IsApproved] FROM [vw_aspnet_MembershipUsers] ORDER BY [UserName]">
            </asp:SqlDataSource>
            <asp:Label ID="errText" runat="server" Font-Names="Verdana" ForeColor="#FF3300"></asp:Label>
        </asp:View>
        <asp:View ID="View2" runat="server">
            <span class="style1">
            <asp:Table ID="tbl_User" runat="server">
            </asp:Table>
            </span>
        <asp:Table ID="tbl_Links" runat="server" Width="100%">
            <asp:TableRow runat="server">
                <asp:TableCell runat="server"></asp:TableCell>
            </asp:TableRow>
            </asp:Table>
            <hr width="400" align="left"/>
            <asp:Table ID="tbl_Profile" runat="server">
            </asp:Table><hr width="400" align="left"/>
            <span class="style2">
            <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="members.aspx">&lt;&lt; Back</asp:HyperLink>
            </span>
            <br class="style1" />
        </asp:View>
        <asp:View ID="View3" runat="server">
            <asp:CreateUserWizard ID="CreateUserWizard1" runat="server" 
                RequireEmail="False">
                <WizardSteps>
                    <asp:CreateUserWizardStep runat="server">
                        <ContentTemplate>
                            <table border="0" style="font-family: Verdana">
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName">User 
                                        Name:</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="UserName" runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" 
                                            ControlToValidate="UserName" ErrorMessage="User Name is required." 
                                            ToolTip="User Name is required." ValidationGroup="CreateUserWizard1">*</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password">Password:</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="Password" runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" 
                                            ControlToValidate="Password" ErrorMessage="Password is required." 
                                            ToolTip="Password is required." ValidationGroup="CreateUserWizard1">*</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="2">

                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="2" style="color:Red;">
                                        <asp:Literal ID="ErrorMessage" runat="server" EnableViewState="False"></asp:Literal>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:CreateUserWizardStep>
                    <asp:CompleteWizardStep runat="server" />
                </WizardSteps>
            </asp:CreateUserWizard>
        </asp:View>
    </asp:MultiView>
    <br/>
</asp:Content>