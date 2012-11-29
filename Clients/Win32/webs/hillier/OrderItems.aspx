<%@ Page Language="VB" MasterPageFile="~/Masterpages/default.master" AutoEventWireup="false" CodeFile="OrderItems.aspx.vb" Inherits="OrderItems" title="Untitled Page" %><%@ Register TagPrefix="PRIORITY" TagName="VIEW" src="~/controls/Enclosure.ascx" %>  
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    &nbsp;<PRIORITY:VIEW ID="View" runat="server"
            UID="ORDI" 
            FilterView="ZWEBV_WEBORDITEMFILTER" 
            DataView="ZWEBV_WEBORDERITEMS" 
            Template="OrderItemsTemplate.htm" 
             />
    <br />
</asp:Content>