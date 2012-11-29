<%@ Page Language="VB" MasterPageFile="~/Masterpages/default.master" AutoEventWireup="false" CodeFile="Basket.aspx.vb" Inherits="Basket" title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <strong>

Currency Select is placed on the page with the HTML:</strong><br />&lt;PRIORITY:CURRENCY runat=&quot;server&quot; ID=&quot;currency&quot; /&gt;<br />
    <br />
    <strong>
The basket is created with the HTML:</strong><br/>
&lt;PRIORITY:PRICART ID=&quot;Cart&quot; runat=&quot;server&quot; /&gt;<br/><hr/><br/>


    <table>
        <tr>
            <td style="vertical-align: top; width: 100px; text-align: right">
                &nbsp;<PRIORITY:CURRENCY ID="Currency" runat="server" />
            </td>
        </tr>
        <tr>
            <td style="vertical-align: top; width: 100px; text-align: left">
    <PRIORITY:PRICART ID="Cart" runat="server" /></td>
        </tr>
    </table>
    <br />
</asp:Content>

