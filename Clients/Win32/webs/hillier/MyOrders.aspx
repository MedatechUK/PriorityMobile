<%@ Page Language="VB" MasterPageFile="~/Masterpages/default.master" AutoEventWireup="false" CodeFile="MyOrders.aspx.vb" Inherits="MyOrders" title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    &nbsp;<PRIORITY:VIEW ID="View" runat="server"
            UID="PART" 
            FilterView="ZWEBV_WEBPARTFILTER" 
            DataView="ZWEBV_WEBPARTS" 
            Template="PartTemplate.htm" 
             />
    <br />
</asp:Content>
