<%@ Page Language="VB" MasterPageFile="~/Masterpages/default.master" AutoEventWireup="false" CodeFile="Customers.aspx.vb" Inherits="Customers" title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    &nbsp;<PRIORITY:VIEW ID="View" runat="server"
            UID="CUST" 
            FilterView="ZWEBV_WEBPARTFILTER" 
            DataView="ZWEBV_WEBCUST" 
            Template="CustomerTemplate.htm" 
             />
    <br />
</asp:Content>            