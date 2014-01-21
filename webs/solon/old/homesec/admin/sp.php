<?php 
// SECURITY CHECK TO MAKE SURE THE CORRECT USERNAME AND PASSWORD HAS BEEN ENTERED
if(!$_COOKIE[user_name] | !$_COOKIE[user_pwd])
{
header("location: login.php");
exit;
}

require_once('../assets/widgets/mysql.class.php');
require_once('../assets/widgets/global.inc.php');
require_once('../assets/widgets/functions.inc.php');
require_once('../assets/widgets/global_variables.php');
?>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
<link rel="shortcut icon" href="../assets/images/favicon.ico" />
<title>Standard Paragraphs</title>

<style type="text/css">
<!--
body{font-family:Verdana, Arial, Helvetica, sans-serif;padding: 10px;font-size: 90%;background: #fff;}
#abasketconfirm {border-collapse: collapse; width: 100%; border: 1px solid #999;font-size: 90%; }
#abasketconfirm th {padding: 3px; background:url(../assets/images/product_h3_bg.gif) #acd700; text-align:left}
#abasketconfirm td {padding: 3px;}
#abasketconfirm a {color: #F00; }
#abasketconfirm a:hover {color: #F00; text-decoration: none;}
#abasketconfirm .subtotal {background: #ededed;text-align: right; border-top: 3px double #999;}
#abasketconfirm .postage {background: #ededed;text-align: right;}
#abasketconfirm .vat {background: #ededed;text-align: right;}
#abasketconfirm .total {background: #ededed; text-align: right; font-size: 150%;}
.close {float: right;}
.close img {border:none}
-->
</style>
</head>
<body>
<script type="text/javascript" language="JavaScript">
<!--
document.write('<p class="close"><a href="#" onClick="javascript:window.close();">');
document.write('<img src="../assets/images/buttons/close_window.jpg" alt="Close Window" />');
document.write('</a></p>');
//-->
</script>
<?php

$query = mysql_query("select * from $orders_table where id = '$_GET[id]'");
while ($result = mysql_fetch_array($query))
	{
$name = explode("<br/>", $result[billing_address]);

$orderinfo = slashes($result[orderinfo]);
$orderinfo = str_replace("<table id='basketconfirm'>", "<table style='border-collapse: collapse; width: 100%; border: 1px solid #999;font-size: 90%;'>", $orderinfo);

$orderinfo = str_replace("<tr class='subtotal'>", "<tr style='background: #ededed;text-align: right; border-top: 3px double #999;'>", $orderinfo);
$orderinfo = str_replace("<tr class='postage'>", "<tr style='background: #ededed;text-align: right;'>", $orderinfo);
$orderinfo = str_replace("<tr class='total'>", "<tr style='background: #ededed;text-align: right;'>", $orderinfo);
$orderinfo = str_replace("<th>", "<th style='padding: 3px; background:url(".$site_url."assets/images/product_h3_bg.gif) #f7941d; text-align:left'>", $orderinfo);


////////////////////////////////////////////////////////////////////////
echo("<h2 style='color: #000; padding: 0; margin: 0; font-size: 120%'>Order Dispatched:</h2><p style='font-size:80%'>Dear $name[0]</p><p style='font-size:80%'>The following items have been dispatched to you</p>");
echo("".slashes($orderinfo)."");
echo("<p style='font-size:80%'>The above items have been dispatched to you at the following address: </p>
<p style='font-size:80%'>$result[delivery_address]</p>");

echo("<h2 style='color: #000; padding: 0; margin: 0; font-size: 120%'>Standard Delivery Notice</h2><p style='font-size:80%'>Your order has now been dispatched via Royal Mail. Your tracking number is XXXX. You can track your order by entering your tracking number on the Royal Mail website at <a href='http://www.royalmail.com/portal/rm/track' target='_blank'>www.royalmail.com/portal/rm/track</a></p>
 
<h2 style='color: #000; padding: 0; margin: 0; font-size: 120%'>Special Delivery Notice</h2>
<p style='font-size:80%'>Your order has now been dispatched via ParcelForce. Your tracking number is XXXX. You can track your order by entering your tracking number on the ParcelForce website at <a href='http://www.parcelforce.com/portal/pw' target='_blank'>www.parcelforce.com/portal/pw</a></p>");

echo("<h2 style='color: #000; padding: 0; margin: 0; font-size: 120%'>Refunded processed: </h2><p style='font-size:80%'>Dear $name[0]</p><p style='font-size:80%'>We have now processed your refund and the value of the order has been refunded to your payment card account. Please retain this refund confirmation for your records.</p>
<h2 style='color: #000; padding: 0; margin: 0; font-size: 120%'>Awaiting stock: </h2><p style='font-size:80%'>Dear $name[0]</p><p style='font-size:80%'>Thank you for your order. The item(s) in your order are not currently in stock. New stock will arrive very soon and we will dispatch your order to you as soon as we receive the stock. We will send you an update by email when your order is dispatched.</p>
<h2 style='color: #000; padding: 0; margin: 0; font-size: 120%'>Returned goods received: </h2><p style='font-size:80%'>Dear $name[0]</p><p style='font-size:80%'>We have now received the returned items from you and we will process your refund as soon as possible. We will send you a confirmation message by email when your refund has been processed.</p>");

}
?>
<script type="text/javascript" language="JavaScript">
<!--
document.write('<p class="close"><a href="#" onClick="javascript:window.close();">');
document.write('<img src="../assets/images/buttons/close_window.jpg" alt="Close Window" />');
document.write('</a></p>');
//-->
</script>
</body>
</html>
