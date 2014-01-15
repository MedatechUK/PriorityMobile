<%@ Page Language="VB" MasterPageFile="~/membership.master" AutoEventWireup="false" CodeFile="500.aspx.vb" Inherits="_500" title="Untitled Page" ValidateRequest="false"%>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
<h2>Page:</h2><p><a href="<% =Request("page")%>"><% =Request("page")%></a></p>
<h2>Error:</h2><p><% =Request("error")%></p>
</asp:Content>

