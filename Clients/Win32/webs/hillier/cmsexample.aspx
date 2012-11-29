<%@ Page validateRequest="false" Language="VB" MasterPageFile="~/Masterpages/default.master" AutoEventWireup="false" CodeFile="cmsexample.aspx.vb" Inherits="cmsexample" title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <strong>
I make this page editable by adding the following HTML:</strong><br/>
&lt;PRIORITY:CMS ID=&quot;CMS&quot; runat=&quot;server&quot; /&gt;
    <br />
    <hr />
    <br />
    &lt;!--Start editable region --&gt;<br />
<PRIORITY:CMS ID="CMS" runat="server" />
    <br />
    &lt;!-- End editable region --&gt;
</asp:Content>

