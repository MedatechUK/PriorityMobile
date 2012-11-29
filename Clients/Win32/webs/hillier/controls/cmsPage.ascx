<%@ Control Language="VB" AutoEventWireup="false" CodeFile="cmsPage.ascx.vb" Inherits="controls_cmsPage" %>
<asp:PlaceHolder ID="ph_Content" runat="server"></asp:PlaceHolder>
<br />
<br />
<asp:LoginView ID="LoginView1" runat="server">
    <LoggedInTemplate>
        You are logged in.
        <asp:HyperLink ID="LinkEdit" runat="server" NavigateUrl="?act=edit">Edit Page.</asp:HyperLink>
    </LoggedInTemplate>
</asp:LoginView>
