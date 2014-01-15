<%@ Page Language="VB" AutoEventWireup="false" CodeFile="login.aspx.vb" Inherits="login" MasterPageFile="~/membership.master" Title = "Log in." validateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
                    <asp:Label ID="Label1" runat="server" 
                        Text="You were logged off.&lt;br&gt;Thanks for visiting!" Visible="False">
                    </asp:Label>
                    <asp:Login ID="Login1" runat="server" 
                        TextLayout="TextOnTop" 
                        CreateUserUrl="~/Register.aspx" 
                        DisplayRememberMe="False" 
                        PasswordRecoveryUrl="~/lostpassword.aspx" 
                        DestinationPageUrl="~/default.aspx" 
                        UserNameLabelText="Email:" TitleText="">
                        <LayoutTemplate>
                            <table border="0" cellpadding="1" cellspacing="0" 
                                style="border-collapse:collapse;">
                                <tr>
                                    <td>
                                        <table border="0" cellpadding="0">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName">Email:</asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="UserName" runat="server"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" 
                                                        ControlToValidate="UserName" ErrorMessage="User Name is required." 
                                                        ToolTip="User Name is required." ValidationGroup="Login1">*</asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password">Password:</asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="Password" runat="server" TextMode="Password"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" 
                                                        ControlToValidate="Password" ErrorMessage="Password is required." 
                                                        ToolTip="Password is required." ValidationGroup="Login1">*</asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" style="color:Red;">
                                                    <asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <asp:Button ID="defaultbutton" runat="server" CommandName="Login" Text="Log In" 
                                                        ValidationGroup="Login1"  />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </LayoutTemplate>
                    </asp:Login>
                    <BR/>
</asp:Content>