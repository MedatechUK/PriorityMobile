<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Default.aspx.vb" Inherits="_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Purchase Orders</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AutoGenerateColumns="False"
            CellPadding="4" DataSourceID="SqlDataSource1" ForeColor="#333333" GridLines="None"
            Width="591px">
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
            <Columns>
                <asp:BoundField DataField="SUPNAME" HeaderText="Supplier Code" SortExpression="SUPNAME" />
                <asp:BoundField DataField="SUPDES" HeaderText="Supplier" SortExpression="SUPDES" />
                <asp:BoundField DataField="SUPORDNUM" HeaderText="Your Ref" SortExpression="SUPORDNUM" />
                <asp:HyperLinkField DataNavigateUrlFields="ORDNAME,SUPNAME" DataNavigateUrlFormatString="size.aspx?PO={0}&amp;SUP={1}"
                    DataTextField="ORDNAME" HeaderText="PO#" />
                <asp:BoundField DataField="ORDERDATE" HeaderText="Date" ReadOnly="True" SortExpression="ORDERDATE" />
            </Columns>
            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="#999999" />
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
        </asp:GridView>
    
    </div>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:system %>"
            SelectCommand="SELECT     TOP (100) PERCENT dbo.SUPPLIERS.SUPNAME, dbo.SUPPLIERS.SUPDES, dbo.PORDERS.SUPORDNUM, dbo.PORDERS.ORDNAME, &#13;&#10;                      CONVERT(varchar, dbo.MINTODATE(dbo.PORDERS.CURDATE), 103) AS ORDERDATE&#13;&#10;FROM         dbo.PORDERS INNER JOIN&#13;&#10;                      dbo.SUPPLIERS ON dbo.PORDERS.SUP = dbo.SUPPLIERS.SUP&#13;&#10;WHERE    (SUPNAME = @SUP) AND (dbo.PORDERS.CLOSED <> 'Y') AND (dbo.PORDERS.UFLAG = 'Y')&#13;&#10;ORDER BY dbo.PORDERS.CURDATE DESC ">
            <SelectParameters>
                <asp:QueryStringParameter Name="SUP" QueryStringField="SUP" />
            </SelectParameters>
        </asp:SqlDataSource>
    </form>
</body>
</html>
