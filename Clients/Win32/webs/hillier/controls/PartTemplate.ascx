<%@ Control Language="VB" AutoEventWireup="false" CodeFile="PartTemplate.ascx.vb" Inherits="controlPart" %>
<table cellspacing="10">
    <tr>
        <td style="vertical-align: top; width: 100px; text-align: center">
            <asp:Image ID="IMAGE" runat="server" /></td>
        <td style="vertical-align: bottom; width: 250px; text-align: left">
<asp:Label ID="txtPARTNAME" runat="server"></asp:Label><br />
<asp:Label ID="txtPARTDES" runat="server"></asp:Label><br />
Price:
<asp:Label ID="txtUNITPRICE" runat="server"></asp:Label>
<asp:Label ID="txtCURRENCY" runat="server"></asp:Label><br />
In Stock:
<asp:Label ID="txtAVAILIBLE" runat="server"></asp:Label><br />
            <br />
Qty:
<asp:TextBox ID="txtQTY" runat="server" Width="37px" AutoPostBack="True">1</asp:TextBox>&nbsp;<br />
<asp:Button ID="btn" runat="server" Text="Add to Basket" />
        </td>
    </tr>
    <tr>
        <td colspan="2" style="vertical-align: top; background-color: #94BE84; text-align: left; color: white;">
            Part Description</td>
    </tr>
    <tr>
        <td colspan="2" style="vertical-align: top; text-align: left">
            <asp:PlaceHolder ID="ph_PARTREMARK" runat="server"></asp:PlaceHolder>
        </td>
    </tr>
    <tr>
        <td colspan="2" style="vertical-align: top; background-color: #94BE84; text-align: left; color: white;">
            Part Specs</td>
    </tr>
    <tr>
        <td colspan="2" style="vertical-align: top; text-align: left">
            <table style="width: 100%">
                <tr>
                    <td style="width: 103px">
                        <asp:Label ID="lblSPEC1" runat="server"></asp:Label></td>
                    <td style="width: 100px">
                        <asp:Label ID="txtSPEC1" runat="server"></asp:Label></td>
                    <td style="width: 100px">
                        <asp:Label ID="lblSPEC4" runat="server"></asp:Label></td>
                    <td style="width: 100px">
                        <asp:Label ID="txtSPEC4" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td style="width: 103px">
                        <asp:Label ID="lblSPEC2" runat="server"></asp:Label></td>
                    <td style="width: 100px">
                        <asp:Label ID="txtSPEC2" runat="server"></asp:Label></td>
                    <td style="width: 100px">
                        <asp:Label ID="lblSPEC5" runat="server"></asp:Label></td>
                    <td style="width: 100px">
                        <asp:Label ID="txtSPEC5" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td style="width: 103px; height: 21px">
                        <asp:Label ID="lblSPEC3" runat="server"></asp:Label></td>
                    <td style="width: 100px; height: 21px">
                        <asp:Label ID="txtSPEC3" runat="server"></asp:Label></td>
                    <td style="width: 100px; height: 21px">
                        <asp:Label ID="lblSPEC6" runat="server"></asp:Label></td>
                    <td style="width: 100px; height: 21px">
                        <asp:Label ID="txtSPEC6" runat="server"></asp:Label></td>
                </tr>
            </table>
        </td>
    </tr>
</table>
