<%@ Page Language="VB" MasterPageFile="SoapService.master" AutoEventWireup="false" CodeFile="default.aspx.vb" Inherits="_default" title="Priority Mobile - General" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
    <script type="text/javascript">function init(){CallServer('sysinfo:', null);}</script>
    <br />
    <div style="text-align: left">
        <table border="0" cellpadding="20" style="font-size: small; width: 600px; font-family: Verdana">
            <tr>
                <td align="right" style="width: 180px">
                    System Info:</td>
                <td style="width: 84px">
                    <asp:TextBox ID="txterror" runat="server" BackColor="Silver" ForeColor="Black" ReadOnly="True"
                        Width="354px"></asp:TextBox></td>
            </tr>

            <tr>
                <td align="right" style="width: 180px; text-align: right;">
                    Priority Environment:</td>
                <td style="width: 84px">
                <asp:DropDownList ID="Environments" runat="server">
                </asp:DropDownList></td>
            </tr>

            <tr>
                <td align="right" style="width: 180px; text-align: right;">
                    Logging level:</td>
                <td style="width: 84px">
                    <asp:DropDownList ID="Verbosity" runat="server">
                        <asp:ListItem Value="1">Normal</asp:ListItem>
                        <asp:ListItem Value="10">Verbose</asp:ListItem>
                        <asp:ListItem Value="50">Very Verbose</asp:ListItem>
                        <asp:ListItem Value="99">Arcane</asp:ListItem>
                    </asp:DropDownList></td>
            </tr>

            <tr>
                <td align="right" style="width: 180px; text-align: right;">
                    Loading Timeout:</td>
                <td style="width: 84px">
                    <asp:DropDownList ID="lstTimeout" runat="server">
                        <asp:ListItem Value="60">1 Minute</asp:ListItem>
                        <asp:ListItem Value="120">2 Minutes</asp:ListItem>
                        <asp:ListItem Value="180">3 Minutes</asp:ListItem>
                        <asp:ListItem Value="240">4 Minutes</asp:ListItem>
                        <asp:ListItem Value="300">5 Minutes</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>

            <tr>
                <td style="width: 180px">
                </td>
                <td style="width: 84px">
    <asp:Button ID="Btn" runat="server" Text="Save" /></td>
            </tr>
        </table>
    </div>
    <br />
    <br />
</asp:Content>

