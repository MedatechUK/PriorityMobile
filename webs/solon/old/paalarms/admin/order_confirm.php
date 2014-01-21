<?php
session_start();
if(!$_COOKIE[user_name] | !$_COOKIE[user_pwd])
{
header("location: login.php");
exit;
}

require_once('../assets/widgets/mysql.class.php');
require_once('../assets/widgets/global.inc.php');
require_once('../assets/widgets/functions.inc.php');
require_once('../assets/widgets/admin_functions.inc.php');
require("../assets/widgets/global_variables.php");


?>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
<title>Admin Interface</title>
<link rel="shortcut icon" href="http://www.personal-attack-alarms.net/favicon.ico" />
<link rel="stylesheet" href="../assets/css/admin.css" type="text/css" media="screen" />
</head>
<body>
<noscript>
<h1>Warning</h1>
<p class="noscript">To use this site correctly you need to have JavaScript enabled on your web browser</p>
</noscript>
<div id="hidden">
  <?php require("../assets/widgets/admin_hidden.php"); ?>
</div>
<div id="container">
  <div id="header">
    <h2>Website</h2>
  </div>
  <div id="navigation">
    <?php 
	// THIS PULLS IN THE ADMIN LINKS
	require("widget_links.php"); 
	?>
  </div>
  <div id="main_content">
  <h2>Admin Checkout > Complete</h2>
    <p class="step">Admin Checkout > Delivery information > Confirmation > <strong>Complete</strong></p>
	
    <?php

		$sent = date('d-m-Y H:i');



mysql_connect($host, $user, $pass);
mysql_select_db($database);
//DB VARIABLES
$date_added = date("YmdHis");

//DELIVERY DETAILS
$delivery_details = "".slashes($_POST[delivery_name])."";
$delivery_details .= "<br/>".slashes($_POST[delivery_address])."";
if ($_POST[delivery_address2] != "No Value")
{
$delivery_details .= "<br/>".slashes($_POST[delivery_address2])."";
}
$delivery_details .= "<br/>".slashes($_POST[delivery_town])."";
$delivery_details .= "<br/>".slashes($_POST[delivery_county])."";
$delivery_details .= "<br/>".slashes($_POST[delivery_postcode])."";
$delivery_details .= "<br/>".slashes($_POST[delivery_tel])."";
$delivery_details .= "<br/>".slashes($_POST[delivery_email])."";

//BILLING DETAILS
$billing_details = "".slashes($_POST[name])."";
$billing_details .= "<br/>".slashes($_POST[address])."";
if ($_POST[address2] != "No Value"){$billing_details .= "<br/>".slashes($_POST[address2])."";};
$billing_details .= "<br/>".slashes($_POST[town])."";
$billing_details .= "<br/>".slashes($_POST[county])."";
$billing_details .= "<br/>".slashes($_POST[postcode])."";
$billing_details .= "<br/>".slashes($_POST[telephone])."";
$billing_details .= "<br/>".slashes($_POST[email])."";

$billing_details = str_replace("'", "\'", $billing_details);
$delivery_details = str_replace("'", "\'", $delivery_details);

// GET PREVIOUS ORDER NUMBER
$ordernum_query=mysql_query("select order_number2 from SHOP1_orders order by id desc limit 1");
$ordernum_result = mysql_fetch_array($ordernum_query);
$order_number = $ordernum_result[0] + 1;

// ----------------------------------------------------------------------------------------------------------------------- //		
//												ORDER ADDED TO DATABASE
// ----------------------------------------------------------------------------------------------------------------------- //	

$total 		= "£".$_POST[inputamount];
$process=mysql_query("INSERT INTO SHOP1_orders (order_number, cartinfo, orderinfo, total_cost, email, date, streference, stauthcode, stresult, billing_address, delivery_address, cartdetail, order_number2, postage) VALUES ('$_POST[orderref]', '$_POST[cartinfo]', '$_POST[orderinfo]', '$total', '$_POST[email]', '$date_added', '$_POST[payment]', '$_POST[payment]', '$_POST[payment]', '$billing_details', '$delivery_details', '$_POST[cartdetail]', '$order_number', '$_POST[postage]')"); 
  
  
$tax = $_POST[total] / 100 * 17.5;   
  
$items = explode(',',$_POST[cartinfo]);
$contents = array();
foreach ($items as $item) {
$contents[$item] = (isset($contents[$item])) ? $contents[$item] + 1 : 1;
}

foreach ($contents as $id=>$qty) {

			$pr_query = mysql_query("SELECT * FROM SHOP1_products WHERE id = ".$id."");
			while ($pr_result = mysql_fetch_array($pr_query))
				{
				
				$product_price = $pr_result[price];
			
			// ADD PRODUCTS PURCHASED + VALUE INTO REPORTS TABLE
			//$report_process = mysql_query("INSERT INTO SHOP1_report_products (product_id, price, date_added, order_id, qty) VALUES ('$id', '$product_price', '$date_added', '$order_number', '$qty')"); 		
			}			
} 
  
  
// ----------------------------------------------------------------------------------------------------------------------- //		
//												START DISPLAYING ORDER RESULT
// ----------------------------------------------------------------------------------------------------------------------- //	
	if (!$process)
		{
		echo("<div class='errormessage'><p><strong>Oops!</strong></p><p>There was a problem when adding the order to the system. Please retain your order email for future reference.</p></div>");
		}
	else
		{
		echo("<!-- Order added to DB -->");
		}
		
		
		echo('<div class="print_page"><a href="#" id="printpage" onclick="printPage()" title="Print a paper friendly version of this page">Print order details</a></div>');
		echo("<h3>The order has been processed</h3>");
		//echo("<p>An email has been sent to $_POST[email] with the details of your order.</p>");
		echo("<h3>Order Summary</h3><p>Order Number : ".$_POST[orderref]."</p><p>Order Date: ".$sent."</p>");
		echo slashes($_POST[orderinfo]);
		
		echo ('<div class="summary_delivery"><h3>Delivery Address</h3><p>'.slashes($_POST[delivery_name]).'<br />'.slashes($_POST[delivery_address]).'<br />');

if ($_POST[delivery_address2] != "No Value")
	{
	echo (''.slashes($_POST[delivery_address2]).'<br/>');
	}

echo (''.slashes($_POST[delivery_town]).'<br/>'.slashes($_POST[delivery_county]).'<br/>'.slashes($_POST[delivery_postcode]).'</p><p>Telephone: '.$_POST[delivery_tel].'<br/>Email: '.$_POST[delivery_email].'</p></div><div class="summary_billing"><h3>Billing Address</h3><p>'.slashes($_POST[name]).'<br />'.slashes($_POST[address]).'<br />');

if ($_POST[address2] != "No Value")
	{
	echo (''.slashes($_POST[address2]).'<br />');
	}

echo (''.slashes($_POST[town]).'<br />'.slashes($_POST[county]).'<br />'.slashes($_POST[postcode]).'</p><p>Telephone: '.$_POST[telephone].'<br/>Email: '.$_POST[email].'</p></div><p><strong>Please remember to quote your order number when you contact us with any queries about your order.</strong></p>');
	
	
		/*	
// ----------------------------------------------------------------------------------------------------------------------- //		
//												START OF EMAIL TO CUSTOMER & SITE ADMIN
// ----------------------------------------------------------------------------------------------------------------------- //		
		
$message = '<html>
<body>
<body style="font-family:Verdana;font-size: 80%;background:#fff;">
<table width="550" border="0" cellpadding="0" style="margin:5px; border: 1px solid #999">
<tr><td>
<img src="http://www.oasisone.co.uk/dev/D026/assets/images/email_header.jpg" alt="personal-attack-alarms.net"/>
</td></tr>
<tr><td style="padding:10px">
<h2 style="color: #000; padding: 0; margin: 0; font-size: 120%">Order Confirmation</h2>
<p style="font-size:80%">Order Number : '.$order_number.'</p>
<p style="font-size:80%">Order Date: '.$sent.'</p>
<h3 style="color: #000; padding: 0; margin: 0; font-size: 110%">Products</h3>';

$orderinfo = slashes($_POST[orderinfo]);
$orderinfo = str_replace("<table id='basketconfirm'>", "<table style='border-collapse: collapse; width: 100%; border: 1px solid #999;font-size: 90%;'>", $orderinfo);

$orderinfo = str_replace("<tr class='subtotal'>", "<tr style='background: #ededed;text-align: right; border-top: 3px double #999;'>", $orderinfo);
$orderinfo = str_replace("<tr class='postage'>", "<tr style='background: #ededed;text-align: right;'>", $orderinfo);
$orderinfo = str_replace("<tr class='total'>", "<tr style='background: #ededed;text-align: right;'>", $orderinfo);
$orderinfo = str_replace("<th>", "<th style='padding: 3px; background:url(http://www.oasisone.co.uk/dev/D026/assets/images/product_h3_bg.gif) #acd700; text-align:left'>", $orderinfo);

$message .= $orderinfo;
$message.=('</td></tr><tr><td style="padding:10px"><table><tr><td style="padding:10px" width="50%"><h3 style="color: #000; padding: 0; margin: 0; font-size: 100%">Delivery Address</h3><p style="font-size:80%">'.slashes($_POST[delivery_name]).'<br />'.slashes($_POST[delivery_address]).'<br />');

if ($_POST[delivery_address2] != "No Value")
	{
	$message .= (''.slashes($_POST[delivery_address2]).'<br/>');
	}

$message .= (''.slashes($_POST[delivery_town]).'<br/>'.slashes($_POST[delivery_county]).'<br/>'.slashes($_POST[delivery_postcode]).'</p><p style="font-size:80%">Telephone: '.$_POST[delivery_tel].'<br/>Email: '.$_POST[delivery_email].'</p></td><td style="padding:10px"><h3 style="color: #000; padding: 0; margin: 0; font-size: 100%">Billing Address</h3><p style="font-size:80%">'.slashes($_POST[name]).'<br />'.slashes($_POST[address]).'<br />');

if ($_POST[address2] != "No Value")
	{
	$message .= (''.slashes($_POST[address2]).'<br />');
	}

$message .= (''.slashes($_POST[town]).'<br />'.slashes($_POST[county]).'<br />'.slashes($_POST[postcode]).'</p><p style="font-size:80%">Telephone: '.$_POST[telephone].'<br/>Email: '.$_POST[email].'</p></td></tr></table>
<p style="font-size:80%"><strong>Please remember to quote your order number when you contact us with any queries about your order.</strong></p>
<p style="font-size:80%">Email: <a href="mailto:paa@redlinesecurity.co.uk" style="color: #4c7a08;font-weight: bold;">paa@redlinesecurity.co.uk</a> | Tel: 0845 450 1373</p>
</td></tr>
<tr><td style="padding:10px">
<h3 style="color: #000; padding: 0; margin: 0; font-size: 100%">Terms and Conditions</h3>
<p style="font-size:70%">All customers wishing to return a product must first contact our customer services on 0845 450 1373 or email <a href="mailto:paa-returns@redlinesecurity.co.uk" style="color: #4c7a08;font-weight: bold;">paa-returns@redlinesecurity.co.uk</a>. Please include your order number and a reason for why you want to return the item (for example if the wrong item was delivered, or an incorrect quantity). You will then be sent out a returns pack.</p>
<p style="font-size:70%">Customers have a money back guarantee if they want to return goods within 7 full working days of receipt, as long as they are in a re-saleable condition and have not been removed from their packaging. If the goods are not faulty, the cost of the goods and outward delivery will be refunded, but not the return postage and relevant insurance. The goods remain under the Buyer\'s title until they are received by Redline Security.</p>
<p style="font-size:70%">Non faulty goods must be returned by the Buyer at the Buyer's expense and should be adequately insured during the return journey. The Buyer will receive a refund of all monies paid for the Goods (including outward delivery charges but excluding return postal charges and insurance) within 15 days of cancellation.</p>
<p style="padding-bottom:10px;font-size:70%">Please ensure that you have read and understood our terms and conditions, which can be found on our website at <a href="http://www.oasisone.co.uk/dev/D026/terms_and_conditions.php" style="color: #4c7a08;font-weight: bold;">personal-attack-alarms.net/terms_and_conditions.php</a></p></td></tr>
<tr><td style="padding:10px;margin:0;font-size:70%;color:#999;background:#333;width:100%;">
<p>Copyright 2004-'.date("Y").' Redline Security. All Rights Reserved. Redline Security is a trading name of TIHS Ltd.</p>
<p>Our registered office is: Oak House, Groes Lwyd, Abergele, Conwy LL22 7SU. We are registered in England and Wales. Company No 5325603. VAT Number 855446109.</p>
</td></tr>
</table>
</body>
</html>');
*/
/*
$headers  = "MIME-Version: 1.0\r\n";
$headers .= "Content-type: text/html; charset=iso-8859-1\r\n";
$headers .= "To: ".$_POST[name]." <".$_POST[email].">\r\n";
$headers .= "From: Redline Security | personal-attack-alarms.net<paa@redlinesecurity.co.uk>\r\n";
$subject = "personal-attack-alarms.net | Order Confirmation ".$order_number."";
mail($to, $subject, $message, $headers);

$headers  = "MIME-Version: 1.0\r\n";
$headers .= "Content-type: text/html; charset=iso-8859-1\r\n";
$headers .= "To: Peter <peter@theitc.co.uk>\r\n";
$headers .= "Cc: Airs <aaron.hughes@theitc.co.uk>\r\n";
$headers .= "From:  personal-attack-alarms.net | Copy of order<paa@redlinesecurity.co.uk>\r\n";
$subject = "personal-attack-alarms.net | Copy of order ".$order_number."";
mail($to, $subject, $message, $headers);
		
	*/
	
	//$tracker=mysql_query("insert into $admin_track_table (name, loggedin, ip, job) values ('$_COOKIE[user_name]', '$tracker_time', '$ip', 'Order - Manual Process')");
	
	
//SAGE TRANSACTIONAL EMAIL (Mike Roberts)
require("../assets/widgets/sage_trans_admin.php");

$tracker=mysql_query("insert into SHOP1_admin_tracker (name, loggedin, ip, job) values ('$_COOKIE[user_name]', '$tracker_time', '$ip', 'Manual Order Processed - $_POST[orderref]')");
session_destroy();
?>
  </div>
  <div id="footer">
    <?php 
	// THIS PULLS IN THE FOOTER
	require("../assets/widgets/footer.php"); 
	?>
  </div>
</div>

</body>
</html>
