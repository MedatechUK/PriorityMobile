<%@ Control Language="VB" AutoEventWireup="false" CodeFile="TestPart.ascx.vb" Inherits="controls_TestPart" %>
<asp:Label ID="txtPARTNAME" runat="server"></asp:Label><br />
<asp:Label ID="txtPARTDES" runat="server"></asp:Label><br />
Price:
<asp:Label ID="txtUNITPRICE" runat="server"></asp:Label>
<asp:Label ID="txtCURRENCY" runat="server"></asp:Label><br />
In Stock:
<asp:Label ID="txtAVAILIBLE" runat="server"></asp:Label><br />
Qty:
<asp:TextBox ID="txtQTY" runat="server" Width="37px" AutoPostBack="True">1</asp:TextBox>&nbsp;<br />
<asp:Button ID="btn" runat="server" Text="Add to Basket" />
