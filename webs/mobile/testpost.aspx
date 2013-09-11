<%@ Page Language="VB" AutoEventWireup="false" CodeFile="testpost.aspx.vb" Inherits="testpost" validateRequest=false%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:TextBox ID="xmldata" runat="server" Height="482px" TextMode="MultiLine" Width="895px">&lt;?xml version=&quot;1.0&quot;?&gt;
&lt;order_post test=&quot;1&quot;&gt;
	&lt;customer_id&gt;1000005&lt;/customer_id&gt;
        &lt;order_id&gt;123456789&lt;/order_id&gt;
        &lt;payment&gt;
                 &lt;trans&gt;234567890-45678&lt;/trans&gt;
                 &lt;authcode&gt;12345&lt;/authcode&gt;
                 &lt;amount&gt;99.99&lt;/amount&gt;
        &lt;/payment&gt;
	&lt;delivery_address_1&gt;Losource Limited&lt;/delivery_address_1&gt;
	&lt;delivery_address_2&gt;Street 1&lt;/delivery_address_2&gt;
	&lt;delivery_address_3&gt;Street 2&lt;/delivery_address_3&gt;
	&lt;delivery_address_4&gt;Street 3&lt;/delivery_address_4&gt;
	&lt;delivery_address_5&gt;Street 4&lt;/delivery_address_5&gt;
	&lt;delivery_address_postcode&gt;HX7 8AS&lt;/delivery_address_postcode&gt;
	&lt;lines&gt;
		&lt;line&gt;
			&lt;sku&gt;GWP-BLK-23&lt;/sku&gt;
			&lt;qty&gt;50&lt;/qty&gt;
                        &lt;unit_price&gt;19.99&lt;/unit_price&gt;
			&lt;delivery_date&gt;2011-8-27&lt;/delivery_date&gt;
		&lt;/line&gt;
		&lt;line&gt;
			&lt;sku&gt;GWP-GRN-23&lt;/sku&gt;
			&lt;qty&gt;100&lt;/qty&gt;
                        &lt;unit_price&gt;19.99&lt;/unit_price&gt;
			&lt;delivery_date&gt;2011-8-20&lt;/delivery_date&gt;
		&lt;/line&gt;
		&lt;line&gt;
			&lt;sku&gt;GWP-GRY-23&lt;/sku&gt;
			&lt;qty&gt;50&lt;/qty&gt;
                        &lt;unit_price&gt;19.99&lt;/unit_price&gt;
			&lt;delivery_date&gt;2011-8-27&lt;/delivery_date&gt;
		&lt;/line&gt;
	&lt;/lines&gt;
&lt;/order_post&gt;
</asp:TextBox><br />
        <asp:Button ID="Button1" runat="server" Text="Button" />&nbsp;</div>
    </form>
</body>
</html>
