<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/Masterpages/default.master" CodeFile="family.aspx.vb" Inherits="family" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <strong>
This view was created with the Priority enclosure control like this:</strong><br/>
&lt;PRIORITY:VIEW 
    <br />
    &nbsp;&nbsp; ID=&quot;View&quot; 
    <br />
    &nbsp;&nbsp; runat=&quot;server&quot;
    <br />
    &nbsp;&nbsp;
            UID=&quot;PART&quot; 
    <br />
    &nbsp;&nbsp;
            FilterView=&quot;ZWEBV_WEBPARTFILTER&quot; 
    <br />
    &nbsp;&nbsp;
            DataView=&quot;ZWEBV_WEBPARTS&quot; 
    <br />
    &nbsp;&nbsp;
            Template=&quot;<a href='/HTMLTemplates/PartTemplate.htm'>PartTemplate.htm</a>&quot; 
    <br />
             /&gt;
             <br/><hr><br/>
    &nbsp;<PRIORITY:VIEW ID="View" runat="server"
            UID="PART" 
            FilterView="ZWEBV_WEBPARTFILTER" 
            DataView="ZWEBV_WEBPARTS" 
            Template="PartTemplate.htm" 
             />
    <br />
</asp:Content>
