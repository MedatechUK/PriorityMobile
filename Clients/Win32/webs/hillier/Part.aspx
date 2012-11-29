<%@ Page Language="VB" MasterPageFile="~/Masterpages/default.master" AutoEventWireup="false" CodeFile="Part.aspx.vb" Inherits="Part" title="Untitled Page" %>
<%@ Register TagPrefix="PRIORITY" TagName="CURRENCY" src="~/controls/currencyselect.ascx"%> 
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <priority:currency id="currency" runat="server"></priority:currency>
    &nbsp;&nbsp;<br />
    <br />
    <br />
</asp:Content>

