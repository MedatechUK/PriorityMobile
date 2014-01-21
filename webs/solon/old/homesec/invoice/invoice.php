<?php 
// SECURITY CHECK TO MAKE SURE THE CORRECT USERNAME AND PASSWORD HAS BEEN ENTERED
if(!$_COOKIE[user_name] | !$_COOKIE[user_pwd])
{
header("location: /notfound.php");
exit;
}
?>
<?php 
require_once('../assets/widgets/mysql.class.php');
require_once('../assets/widgets/global.inc.php');
require_once('../assets/widgets/functions.inc.php');
require_once('../assets/widgets/global_variables.php');

$query = mysql_query("select * from $orders_table where id='$_GET[id]'");
	while($result = mysql_fetch_array($query))
	{
	$date = date("d/m/Y H:i:s", strtotime($result[date]));
	?>
<!doctype html public "-//W3C//DTD HTML 4.01 Transitional//EN">
<html dir="ltr" lang="en">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1">
<title><?php echo $site_name; ?> Invoice - Order Number #: <?php echo ($result[order_number2]); ?> </title>
<link rel="stylesheet" href="<?php echo $site_url; ?>assets/css/invoice.css" type="text/css" media="screen" />
<link rel="stylesheet" href="<?php echo $site_url; ?>assets/css/print.css" type="text/css" media="print" />
<link rel="shortcut icon" href="<?php echo $site_url; ?>assets/images/favicon.ico" />
<script language="JavaScript" type="text/javascript">function printPage() { print(document); }</script>
</head>
<body style="width:100%">
<!-- body_text //-->
<div class="print_page"><a href="#" id="printpage" onClick="window.print()" title="Print a paper friendly version of this page">Print Invoice</a></div>
<table width="95%" border="0" align="center" cellpadding="2" cellspacing="0">
  <tr align="left">
    <td class="titleHeading"><img src="<?php echo $site_url; ?>assets/images/clear.gif" border="0" alt="" width="1" height="25"></td>
  </tr>
  <tr>
    <td><table border="0" align="center" width="100%" cellspacing="0" cellpadding="0">
        <tr>
          <td><table border="0" align="center" width="100%" cellspacing="0" cellpadding="0">
              <tr>
                <td style="color:#666; font-size:80%"><small>Redline Security<br>
                  Unit 40<br>
                  Manor Industrial Estate<br>
                  Flint<br>
                  Flintshire<br>
                  CH6 5UY</small></td>
                <td class="pageHeading" align="right"><img src="<?php echo $site_url; ?>assets/images/logo.gif" alt="Redline Security" width="300" height="60"/></td>
              </tr>
              <tr align="left">
                <td colspan="2" class="titleHeading"><img src="<?php echo $site_url; ?>assets/images/clear.gif" border="0" alt="" width="1" height="10"></td>
              </tr>
            </table></td>
        </tr>
      </table></td>
  </tr>
  <tr>
    <td align="left" class="main"><table width="100%" border="0" cellspacing="0" cellpadding="2">
        <tr>
          <td><small><b>Order Number:</b> <?php echo ($result[order_number2]); ?> : <?php echo ($result[order_number]); ?></small></td>
        </tr>
        <tr>
          <td class="main"><small><b>Date Purchased:</b> <?php echo $date; ?></small></td>
        </tr>
        <tr>
          <td>&nbsp;</td>
        </tr>
      </table></td>
  </tr>
  <tr>
    <td align="center"><table align="center" width="100%" border="0" cellspacing="0" cellpadding="2">
        <tr>
          <td align="center" valign="top"><table align="center" width="100%" border="0" cellspacing="0" cellpadding="1" bgcolor="#ededed">
              <tr>
                <td align="center" valign="top"><table align="center" width="100%" border="0" cellspacing="0" cellpadding="2">
                    <tr style="border: 1px solid #999;">
                      <td background="<?php echo $site_url; ?>assets/images/product_h3_bg.gif" style="font-size: 90%"><b>SOLD TO:</b></td>
                    </tr>
                    <tr class="dataTableRow">
                      <td class="dataTableContent" style="font-size: 90%"><?php echo("<small>$result[billing_address]</small>"); ?></td>
                    </tr>
                  </table></td>
              </tr>
            </table></td>
          <td align="center" valign="top"><table align="center" width="100%" border="0" cellspacing="0" cellpadding="1" bgcolor="#ededed">
              <tr>
                <td align="center" valign="top"><table align="center" width="100%" border="0" cellspacing="0" cellpadding="2">
                    <tr class="dataTableHeadingRow">
                      <td background="<?php echo $site_url; ?>assets/images/product_h3_bg.gif" style="font-size: 90%"><b>DELIVERED TO:</b></td>
                    </tr>
                    <tr class="dataTableRow">
                      <td class="dataTableContent" style="font-size: 90%"><?php echo("<small>$result[delivery_address]</small>"); ?></td>
                    </tr>
                  </table></td>
              </tr>
            </table></td>
        </tr>
      </table></td>
  </tr>
  <tr>
    <td><img src="<?php echo $site_url; ?>assets/images/clear.gif" border="0" alt="" width="1" height="10"></td>
  </tr>
  <tr>
    <td><table border="0" width="100%" cellspacing="0" cellpadding="1" bgcolor="#ededed">
        <tr>
          <td style="font-size: 80%"><?php echo slashes($result[orderinfo]);?></td>
        </tr>
      </table></td>
  </tr>
  <tr>
    <td id="footer"><h3 style="font-size: 120%; padding:10px 0 0 10px;">Terms and Conditions</h3>
      <font color="#999999"><p style="font-size:70%">All customers wishing to return a product must first contact our customer services on <?php echo $site_tel; ?> or email <a href="mailto:<?php echo $site_email; ?>" style="color: #f7941d;font-weight: bold;"><?php echo $site_email; ?></a>. Please include your order number and a reason for why you want to return the item (for example if the wrong item was delivered, or an incorrect quantity). You will then be sent out a returns pack.</p></font>
      <p style="font-size:70%">Customers have a money back guarantee if they want to return goods within 7 full working days of receipt, as long as they are in a re-saleable condition and have not been removed from their packaging. If the goods are not faulty, the cost of the goods and outward delivery will be refunded, but not the return postage and relevant insurance. The goods remain under the Buyer\'s title until they are received by Redline Security.</p>
      <p style="font-size:70%">Non faulty goods must be returned by the Buyer at the Buyer's expense and should be adequately insured during the return journey. The Buyer will receive a refund of all monies paid for the Goods (including outward delivery charges but excluding return postal charges and insurance) within 15 days of cancellation.</p>
      <p style="font-size:70%">Please ensure that you have read and understood our terms and conditions, which can be found on our website at <a href="<?php echo $site_url; ?>terms_and_conditions.php" style="color: #f7941d;font-weight: bold;"><?php echo $site_url; ?>terms_and_conditions.php</a></p>
      <p style="font-size:70%"> <b>Redline Security is a trading name of TIHS Ltd.</b></p>
      <p style="font-size:70%"> Our registered office is at: Oak House, Groes Lwyd, Abergele, Conwy, LL22 7SU.</p>
      <p style="font-size:70%"> We are registered in England and Wales, <b>Company No 5325603</b>. <b>VAT Number 855446109</b>.</p></td>
  </tr>
</table>
<!-- body_text_eof //-->
</body>
</html>
<?php } ?>
