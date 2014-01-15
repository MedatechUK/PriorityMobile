<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Register.aspx.vb" Inherits="Register" MasterPageFile="~/membership.master" Title = "Sign Up" validateRequest="false"%>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
                    <asp:CreateUserWizard
                        ID="CreateUserWizard1" 
                        runat="server"                          
                        FinishDestinationPageUrl="/profile.aspx" 
                        CancelDestinationPageUrl="~/default.aspx" 
                        ContinueDestinationPageUrl="/profile.aspx" 
                        DisplayCancelButton="false" 
                        RequireEmail="False" CreateUserButtonType="Link"
                        DuplicateUserNameErrorMessage="Email address already in use, forgot password?"                        
                        ><WizardSteps>
                            <asp:CreateUserWizardStep runat="server">
                            
                                <ContentTemplate>
                                    <table border="0" width="400">
                                        <tr>
                                            <td align="right">
                                                <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName">e-mail:</asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="UserName" runat="server"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" 
                                                    ControlToValidate="UserName" ErrorMessage="User Name is required." 
                                                    ToolTip="User Name is required." ValidationGroup="CreateUserWizard1">*</asp:RequiredFieldValidator>
                                                <asp:RequiredFieldValidator ID="EmailRequired" runat="server" 
                                                    ControlToValidate="UserName" ErrorMessage="E-mail is required." 
                                                    ToolTip="E-mail is required." ValidationGroup="CreateUserWizard1">*</asp:RequiredFieldValidator>
                                                    <asp:RegularExpressionValidator ControlToValidate="UserName" runat="server"  
                                             ValidationExpression="([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$" ID="emailReg" 
                                             ErrorMessage="Your email address was not recognised." ValidationGroup="CreateUserWizard1"></asp:RegularExpressionValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right">
                                                <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password">Password:</asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="Password" runat="server" TextMode="Password"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" 
                                                    ControlToValidate="Password" ErrorMessage="Password is required." 
                                                    ToolTip="Password is required." ValidationGroup="CreateUserWizard1">*</asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right">
                                                <asp:Label ID="ConfirmPasswordLabel" runat="server" 
                                                    AssociatedControlID="ConfirmPassword">Confirm Password:</asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="ConfirmPassword" runat="server" TextMode="Password"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="ConfirmPasswordRequired" runat="server" 
                                                    ControlToValidate="ConfirmPassword" 
                                                    ErrorMessage="Confirm Password is required." 
                                                    ToolTip="Confirm Password is required." ValidationGroup="CreateUserWizard1">*</asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="2">
                                                <asp:CompareValidator ID="PasswordCompare" runat="server" 
                                                    ControlToCompare="Password" ControlToValidate="ConfirmPassword" 
                                                    Display="Dynamic" 
                                                    ErrorMessage="The Password and Confirmation Password must match." 
                                                    ValidationGroup="CreateUserWizard1"></asp:CompareValidator>
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
                    </asp:Content>