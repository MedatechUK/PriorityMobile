<%@ Page Language="VB" MasterPageFile="SoapService.master" AutoEventWireup="false" CodeFile="uptime.aspx.vb" Inherits="config_uptime" title="Priority Mobile - System Uptime" %>
<%@ Register assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" namespace="System.Web.UI" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
    <script type="text/javascript">function init(){CallServer('uptime:', null);}</script>
    <br />
    <div style="text-align: left">
        <table border="0" cellpadding="20" style="font-size: small; width: 600px; font-family: Verdana">
            <tr>
                <td align="right" style="width: 96px">
                    System Uptime:</td>
                <td style="width: 84px">
                    <asp:TextBox ID="txterror" runat="server" BackColor="Silver" Width="354px" ForeColor="Black" ReadOnly="True"></asp:TextBox></td>
            </tr>
        </table>
    </div>
    <br />
    <br />
</asp:Content>

