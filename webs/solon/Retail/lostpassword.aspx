<%@ Page Language="VB" AutoEventWireup="false" CodeFile="lostpassword.aspx.vb" Inherits="lostpassword" MasterPageFile="~/text_page.master" Title = "Lost Password" validateRequest="false"%>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
<asp:PasswordRecovery ID="PasswordRecovery1" runat="server">
                        <InstructionTextStyle Font-Italic="True" ForeColor="Black" />
                        <SuccessTextStyle Font-Bold="True"  />                        
                    </asp:PasswordRecovery>
                    <BR/>
                    </asp:Content>