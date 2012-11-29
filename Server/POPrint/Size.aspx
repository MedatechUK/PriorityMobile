<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Size.aspx.vb" Inherits="Size" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:HiddenField ID="PO" runat="server" />
        <asp:HiddenField ID="SUP" runat="server" />
        Please Select your label style:<br />
    
    </div>
        <asp:ListBox ID="ListBox1" runat="server" Height="198px" Width="270px">
            <asp:ListItem Value='681/227'>Avery 6871</asp:ListItem>
<asp:ListItem Value='795/303'>Avery 5160</asp:ListItem>
<asp:ListItem Value='1041/202'>Avery 5066</asp:ListItem>
<asp:ListItem Value='1041/284'>Avery 5026</asp:ListItem>
<asp:ListItem Value='1136/606'>Avery 6873</asp:ListItem>
<asp:ListItem Value='1212/303'>Avery 5161</asp:ListItem>
<asp:ListItem Value='1212/403'>Avery 5162</asp:ListItem>
<asp:ListItem Value='1212/454'>Avery 5197</asp:ListItem>
<asp:ListItem Value='1212/606'>Avery 5163</asp:ListItem>


        </asp:ListBox>
        <br />
        <br />
        <asp:Button ID="Button1" runat="server" Text="Print" />
        <asp:Button ID="Button2" runat="server" Text="Back" /><br />
    </form>
</body>
</html>
