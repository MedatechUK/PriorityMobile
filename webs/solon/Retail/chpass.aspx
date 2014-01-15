<%@ Page Language="VB" MasterPageFile="~/membership.master" AutoEventWireup="false" CodeFile="chpass.aspx.vb" Inherits="chpass" title="Change Password" validateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
                    <asp:ChangePassword ID="ChangePassword1" runat="server" 
                        CancelDestinationPageUrl="~/default.aspx" ChangePasswordTitleText="" 
                        ContinueDestinationPageUrl="~/default.aspx" DisplayUserName="True">
                    </asp:ChangePassword>
                    <BR/>
</asp:Content>