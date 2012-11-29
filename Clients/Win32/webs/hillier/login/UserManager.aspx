<%@ Page Language="VB" MasterPageFile="~/Masterpages/default.master" AutoEventWireup="false" CodeFile="UserManager.aspx.vb" Inherits="login_UserManager" title="Untitled Page" %>
<%@ Register TagPrefix="PRIORITY" TagName="USERS" src="~/controls/UserManager.ascx"%> 
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<PRIORITY:USERS ID="UserManager" runat="server" />
</asp:Content>

