<%@ Page Language="VB" MasterPageFile="../masterpages/login.master" AutoEventWireup="false" CodeFile="Default.aspx.vb" Inherits="_Default" title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table width="100%" align="center">
<tr align="center">
<td WIDTH="100"></td>
<td align="center">
    <br />
    &nbsp;<asp:LoginView ID="LoginView1" runat="server">
        <AnonymousTemplate>
            <asp:MultiView ID="UnloggedView" runat="server">
                <asp:View ID="Login" runat="server">
    <asp:Login ID="Login1" runat="server" BackColor="#F7F6F3" BorderColor="#E6E2D8" BorderPadding="4"
        BorderStyle="Solid" BorderWidth="1px" CreateUserText="<p align='center'>>> FIRST TIME USERS:<br> CLICK HERE TO REGISTER <<</p>" CreateUserUrl="NewUser.aspx"
        Font-Names="Verdana" Font-Size="0.8em" ForeColor="#333333" style="font-size: larger" Height="252px" Width="100%">
        <TextBoxStyle Font-Size="0.8em" />
        <LoginButtonStyle BackColor="#FFFBFF" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px"
            Font-Names="Verdana" Font-Size="0.8em" ForeColor="#284775" />
        <InstructionTextStyle Font-Italic="True" ForeColor="Black" />
        <TitleTextStyle BackColor="#5D7B9D" Font-Bold="True" Font-Size="X-Large" ForeColor="White" HorizontalAlign="Center" />
        <LayoutTemplate>
            <table border="0" cellpadding="4" cellspacing="0" style="border-collapse: collapse; text-align: center; vertical-align: top; width: 277pt;">
                <tr>
                    <td style="text-align: center" align="center">
                        <table border="0" cellpadding="0" style="width: 100%;">
                            <tr>
                                <td align="center" colspan="2" style="font-weight: bold; font-size: x-large; color: navy;
                                    background-color: #99ccff; padding-bottom: 10px; padding-top: 10px; background-repeat: repeat-x;">
                                    Log In</td>
                            </tr>
                            <tr>
                                <td align="right" colspan="2">
                                    &nbsp;&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td align="right" style="width: 288px">
                                    <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName">User&nbsp;Name:&nbsp</asp:Label></td>
                                <td style="width: 279px; text-align: left;">
                                    <asp:TextBox ID="UserName" runat="server" Font-Size="0.8em"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName"
                                        ErrorMessage="User Name is required." ToolTip="User Name is required." ValidationGroup="Login1">*</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" style="width: 288px">
                                    <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password">Password:&nbsp</asp:Label></td>
                                <td style="width: 279px; text-align: left;">
                                    <asp:TextBox ID="Password" runat="server" Font-Size="0.8em" TextMode="Password" style="text-align: left"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password"
                                        ErrorMessage="Password is required." ToolTip="Password is required." ValidationGroup="Login1">*</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" style="height: 20px">
                                    <asp:CheckBox ID="RememberMe" runat="server" Text="Remember me next time." style="text-align: right" Width="100%" />
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="2" style="color: red">
                                    <asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" colspan="2">
                                    <br />
                                    <asp:Button ID="LoginButton" runat="server" BackColor="#FFFBFF" BorderColor="#CCCCCC"
                                        BorderStyle="Solid" BorderWidth="1px" CommandName="Login" Font-Names="Verdana"
                                        Font-Size="0.8em" ForeColor="#284775" Text=">> Log In" ValidationGroup="Login1" style="font-size: larger" /><br />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <br />
                                    <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="?arg=newuser" Style="font-size: larger;
                                        text-align: center" Width="100%">>> 1ST TIME USER? REGISTER HERE</asp:HyperLink>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:HyperLink ID="CreateUserLink" runat="server" NavigateUrl="?arg=forgot" Style="font-size: larger;
                                        text-align: center" Width="100%">>> FORGOT MY PASSWORD?</asp:HyperLink>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </LayoutTemplate>
    </asp:Login>
                </asp:View>
                <asp:View ID="NewUser" runat="server">
                    <asp:CreateUserWizard ID="CreateUserWizard1" runat="server" BackColor="#F7F6F3" BorderColor="#E6E2D8"
                        BorderStyle="Solid" BorderWidth="1px" ContinueDestinationPageUrl="~/Default.aspx"
                        FinishDestinationPageUrl="~/" Font-Names="Verdana" Font-Size="0.8em" Style="font-size: larger"
                        Width="366px">
                        <SideBarStyle BackColor="#5D7B9D" BorderWidth="0px" Font-Size="0.9em" VerticalAlign="Top" />
                        <SideBarButtonStyle BorderWidth="0px" Font-Names="Verdana" ForeColor="White" />
                        <ContinueButtonStyle BackColor="#FFFBFF" BorderColor="#CCCCCC" BorderStyle="Solid"
                            BorderWidth="1px" Font-Names="Verdana" ForeColor="#284775" />
                        <NavigationButtonStyle BackColor="#FFFBFF" BorderColor="#CCCCCC" BorderStyle="Solid"
                            BorderWidth="1px" Font-Names="Verdana" ForeColor="#284775" />
                        <HeaderStyle BackColor="#5D7B9D" BorderStyle="Solid" Font-Bold="True" Font-Size="0.9em"
                            ForeColor="White" HorizontalAlign="Center" />
                        <CreateUserButtonStyle BackColor="#FFFBFF" BorderColor="#CCCCCC" BorderStyle="Solid"
                            BorderWidth="1px" Font-Names="Verdana" ForeColor="#284775" />
                        <TitleTextStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <StepStyle BorderWidth="0px" />
                        <WizardSteps>
                            <asp:CreateUserWizardStep runat="server">
                            </asp:CreateUserWizardStep>
                            <asp:CompleteWizardStep runat="server">
                            </asp:CompleteWizardStep>
                        </WizardSteps>
                    </asp:CreateUserWizard>
                </asp:View>
                <asp:View ID="LostPass" runat="server">
                    <asp:PasswordRecovery ID="PasswordRecovery1" runat="server" BackColor="#F7F6F3" BorderColor="#E6E2D8"
                        BorderPadding="4" BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana"
                        Font-Size="0.8em">
                        <InstructionTextStyle Font-Italic="True" ForeColor="Black" />
                        <SuccessTextStyle Font-Bold="True" ForeColor="#5D7B9D" />
                        <TextBoxStyle Font-Size="0.8em" />
                        <UserNameTemplate>
                            <table border="0" cellpadding="4" cellspacing="0" style="border-collapse: collapse">
                                <tr>
                                    <td>
                                        <table border="0" cellpadding="0" style="font-size: larger">
                                            <tr>
                                                <td align="center" colspan="2" style="font-weight: bold; font-size: larger; color: white;
                                                    height: 14px; background-color: #5d7b9d">
                                                    Forgot Your Password?</td>
                                            </tr>
                                            <tr>
                                                <td align="center" colspan="2" style="font-size: larger; color: black; font-style: italic">
                                                    <br />
                                                    Enter your User Name to receive your password.<br />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName" Style="font-size: larger">User&nbsp;Name:</asp:Label></td>
                                                <td style="width: 212px">
                                                    <asp:TextBox ID="UserName" runat="server" Font-Size="0.8em"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName"
                                                        ErrorMessage="User Name is required." ToolTip="User Name is required." ValidationGroup="PasswordRecovery1">*</asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" colspan="2" style="color: red">
                                                    <asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right" colspan="2" style="height: 18px">
                                                    <br />
                                                    <asp:Button ID="SubmitButton" runat="server" BackColor="#FFFBFF" BorderColor="#CCCCCC"
                                                        BorderStyle="Solid" BorderWidth="1px" CommandName="Submit" Font-Names="Verdana"
                                                        Font-Size="0.8em" ForeColor="#284775" Text="Submit" ValidationGroup="PasswordRecovery1" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </UserNameTemplate>
                        <TitleTextStyle BackColor="#5D7B9D" Font-Bold="True" Font-Size="0.9em" ForeColor="White" />
                        <SubmitButtonStyle BackColor="#FFFBFF" BorderColor="#CCCCCC" BorderStyle="Solid"
                            BorderWidth="1px" Font-Names="Verdana" Font-Size="0.8em" ForeColor="#284775" />
                    </asp:PasswordRecovery>
                </asp:View>
            </asp:MultiView>
        </AnonymousTemplate>
        <LoggedInTemplate>
            <br />
            <strong><em style="text-align: left">
                <asp:MultiView ID="LoggedInView" runat="server">
                    <asp:View ID="Profile" runat="server">
                        Editing User Profile.<br />
            <br />
            <table style="width: 500pt">
                <tr>
                    <td rowspan="4" style="vertical-align: top; width: 100px">
                        <asp:Label ID="Label1" runat="server" Text="Delivery Address:"></asp:Label></td>
                    <td style="width: 6px">
                    </td>
                    <td style="width: 121px; vertical-align: top; text-align: left;">
                        <asp:TextBox ID="txtDelAddress1" runat="server" Width="200px"></asp:TextBox></td>
                </tr>
                <tr>
                    <td style="width: 6px">
                    </td>
                    <td style="width: 121px; vertical-align: top; text-align: left;">
                        <asp:TextBox ID="txtDelAddress2" runat="server" Width="200px"></asp:TextBox></td>
                </tr>
                <tr>
                    <td style="width: 6px">
                    </td>
                    <td style="width: 121px; vertical-align: top; text-align: left;">
                        <asp:TextBox ID="txtDelAddress3" runat="server" Width="200px"></asp:TextBox></td>
                </tr>
                <tr>
                    <td style="width: 6px">
                    </td>
                    <td style="width: 121px; vertical-align: top; text-align: left;">
                        <asp:TextBox ID="txtDelAddress4" runat="server" Width="200px"></asp:TextBox></td>
                </tr>
                <tr>
                    <td style="width: 100px">
                        <asp:Label ID="Label5" runat="server" Text="Postcode: "></asp:Label></td>
                    <td style="width: 6px">
                    </td>
                    <td style="width: 121px; text-align: left">
                        <asp:TextBox ID="txtDelPostcode" runat="server" Width="60px"></asp:TextBox></td>
                </tr>
            </table>
            <asp:Button ID="Button1" runat="server" Text="Save Details" UseSubmitBehavior="False" />
                    </asp:View>
                    <asp:View ID="LogOut" runat="server">
                        <br />
                        <br />
                        <br />
                        <br />
                        You are now logged out.</asp:View>
                </asp:MultiView></em></strong>
        </LoggedInTemplate>
    </asp:LoginView>
    </td>
    <td WIDTH="100"></td>
    </tr></table>
</asp:Content>
