<?php
if ($_GET[ref] == "good")
		{
		$clear = "?clear=all";
		}

function slashes($string)
	{
	$string = stripslashes(stripslashes(stripslashes(stripslashes(stripslashes(stripslashes($string))))));
	return $string;
	}
?>



<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
<title>Home Security Store | Confirmation</title>
<link rel="stylesheet" href="http://www.home-security-store.co.uk/assets/css/screen.css" type="text/css" media="screen" />
<link rel="stylesheet" href="http://www.home-security-store.co.uk/assets/css/print.css" type="text/css" media="print" />
<link rel="shortcut icon" href="http://www..home-security-store.co.uk/favicon.ico" />
<script language="JavaScript" type="text/javascript">function printPage() { print(document); }</script>
</head>
<body onLoad="javascript:__utmSetTrans()">

<script src="http://www.google-analytics.com/urchin.js" type="text/javascript">
</script>
<script type="text/javascript">
_uacct = "UA-514855-12";
_udn="none";
_ulink=1;
urchinTracker();
</script>

<noscript>
<div class="mainalert">
<h1>Javascript is disabled</h1>
<p class="noscript">We recommend you enable javascript on your web browser to use this website.</p>
</div>
</noscript><div id="hidden">
  <div id="hidden">

<a href="http://www.home-security-store.co.uk/<?php echo $clear ?>" title="Home Page"><img src="http://www.home-security-store.co.uk/assets/images/logo.gif" alt="Home Security Store Logo" /></a>
</div></div>
<div id="container">
  <div id="header">
    <h2>Home Security</h2>
	<p><a href="http://www.home-security-store.co.uk/<?php echo $clear ?>" class="homelink" title="Home Page"><img src="http://www.home-security-store.co.uk/assets/images/clear.gif" alt="Link to Home Page" width="300px" height="50px"/></a></p>
	

  </div>  <div id="navigation">

<p>
01745 828440</p>

<ul>
<li><a href="http://www.home-security-store.co.uk/index.php<?php echo $clear ?>" title="Click here to go to our home page">Home</a></li>
<li><a href="http://www.home-security-store.co.uk/products.php<?php echo $clear ?>" title="Click here to go to our product list">Products</a></li>
<li><a href="http://www.home-security-store.co.uk/about.php<?php echo $clear ?>" title="Click here to find out who we are">About us</a></li>
<li><a href="http://www.home-security-store.co.uk/contact.php<?php echo $clear ?>" title="Click here to go to get information on how to contact us">Contact us</a></li>
</ul>





</div> 
  <div id="checkout_content">

    <h2>Checkout > Payment Result</h2>
    <p class="step">Checkout > Delivery information > Confirmation > Payment > <strong>Complete</strong></p>
	
    <?php
	
	
	if ($_GET[ref] == "good")
		{
		$sent = date('d-m-Y H:i');

//---------------------------------------------------------------------------

$host 			= 'mysql.home-security-store.co.uk';
$user 			= 'homesec';
$pass 			= 'h1zzle';
$database 		= 'HOMESEC';
//---------------------------------------------------------------------------


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
$ordernum_query=mysql_query("select order_number2 from SHOP2_orders order by id desc limit 1");
$ordernum_result = mysql_fetch_array($ordernum_query);
$order_number = $ordernum_result[0] + 1;


$total_uf 	= str_replace("£", "", $_POST[total]);
$total 		= str_replace("£", "&pound;", $_POST[total]);




// ----------------------------------------------------------------------------------------------------------------------- //		
//												ORDER ADDED TO DATABASE
// ----------------------------------------------------------------------------------------------------------------------- //	
$process=mysql_query("INSERT INTO SHOP2_orders (order_number, cartinfo, orderinfo, total_cost, email, date, streference, stauthcode, stresult, billing_address, delivery_address, cartdetail, order_number2, postage) VALUES ('$_POST[orderref]', '$_POST[cartinfo]', '$_POST[orderinfo]', '$total_uf', '$_POST[email]', '$date_added', '$_POST[streference]', '$_POST[stauthcode]', '$_POST[stresult]', '$billing_details', '$delivery_details', '$_POST[cartdetail]', '$order_number', '$_POST[postage]')"); 
  
  
$tax = $_POST[total] / 100 * 17.5;   
  
//GOOGLE ECOMMERCE TRANSACTION SCRIPT
echo("
<!-- START OF GOOGLE ECOMMERCE TRANSACTION CODE -->
<form style='display:none;' name='utmform'>
<textarea id='utmtrans'>UTM:T|$order_number|Home Security|$total_uf|$tax|$_POST[postage]|NA|NA|UK \n");




$items = explode(',',$_POST[cartinfo]);
$contents = array();
foreach ($items as $item) {
$contents[$item] = (isset($contents[$item])) ? $contents[$item] + 1 : 1;
}

foreach ($contents as $id=>$qty) {

			$pr_query = mysql_query("SELECT * FROM SHOP2_products WHERE id = ".$id."");
			while ($pr_result = mysql_fetch_array($pr_query))
				{
				// DISCOUNT FUNCTION
			if ($qty >= 10 && $qty < 20)
				{
				if (!$pr_result[dis10])
					{
					$product_price = $pr_result[price];
					}
				else
					{
					$product_price = $pr_result[dis10];
					}
				}
			elseif ($qty >= 20 && $qty < 50)
				{
				if (!$pr_result[dis20])
					{
					$product_price = $pr_result[price];
					}
				else
					{
					$product_price = $pr_result[dis20];
					}
				}
			elseif ($qty >= 50)
				{
				if (!$pr_result[dis50])
					{
					$product_price = $pr_result[price];
					}
				else
					{
					$product_price = $pr_result[dis50];
					}
				}
			else
				{
				$product_price = $pr_result[price];
				}
			
			// ADD PRODUCTS PURCHASED + VALUE INTO REPORTS TABLE
			$report_process = mysql_query("INSERT INTO SHOP2_report_products (product_id, price, date_added, order_id, qty) VALUES ('$id', '$product_price', '$date_added', '$order_number', '$qty')"); 
				echo("UTM:I|$order_number|$pr_result[code]|$pr_result[name]|NA|$product_price|$qty \n");		
			}			
}

echo("</textarea>
</form>
<!-- END OF GOOGLE ECOMMERCE TRANSACTION CODE -->");

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
		echo("<h3>Your order has been processed</h3>");
		echo("<p>An email has been sent to $_POST[email] with the details of your order.</p>");
		echo("<h3>Order Summary</h3><p>Order Number : ".$order_number."</p><p>Order Date: ".$sent."</p>");
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

echo (''.slashes($_POST[town]).'<br />'.slashes($_POST[county]).'<br />'.slashes($_POST[postcode]).'</p><p>&nbsp;<br/>&nbsp;</p></div><p><strong>Please remember to quote your order number when you contact us with any queries about your order.</strong></p>
<div class="info_alert"><p>It is now safe to close your browser window or alternatively continue browsing our site.</p></div>
<p class="buttons"><a href="http://www.home-security-store.co.uk/index.php'.$clear.'"><img src="http://www.home-security-store.co.uk/assets/images/buttons/back_to_hp.gif" alt="Back to home page"/></a></p>
');
	
	
			
// ----------------------------------------------------------------------------------------------------------------------- //		
//												START OF EMAIL TO CUSTOMER & SITE ADMIN
// ----------------------------------------------------------------------------------------------------------------------- //		
		
$message = '<html>
<body>
<body style="font-family:Verdana;font-size: 80%;background:#fff;">
<table width="550" border="0" cellpadding="0" style="margin:5px;">
<tr><td>
<img src="http://www.home-security-store.co.uk/assets/images/email_banner.gif" alt="Home Security Store" width="550" height="134"/>
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
$orderinfo = str_replace("<th>", "<th style='padding: 3px; background:url(http://www.home-security-store.co.uk/assets/images/product_h3_bg.gif) #f7941d; text-align:left'>", $orderinfo);

$orderinfo 		= str_replace("Â£", "&pound;", $orderinfo);

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

$message .= (''.slashes($_POST[town]).'<br />'.slashes($_POST[county]).'<br />'.slashes($_POST[postcode]).'</p><p style="font-size:80%">&nbsp;<br/>&nbsp;</p></td></tr></table>
<p style="font-size:80%"><strong>Please remember to quote your order number when you contact us with any queries about your order.</strong></p>
<p style="font-size:80%">Email: <a href="mailto:hs@redlinesecurity.co.uk" style="color: #f7941d;font-weight: bold;">hs@redlinesecurity.co.uk</a> | Tel: 01745 828499</p>
</td></tr>
<tr><td style="padding:10px">
<h3 style="color: #000; padding: 0; margin: 0; font-size: 100%">Terms and Conditions</h3>
<p style="font-size:70%">All customers wishing to return a product must first contact our customer services on 01745 828499 or email <a href="mailto:hs-returns@redlinesecurity.co.uk" style="color: #f7941d;font-weight: bold;">hs-returns@redlinesecurity.co.uk</a>. Please include your order number and a reason for why you want to return the item (for example if the wrong item was delivered, or an incorrect quantity). You will then be sent out a returns pack.</p>
<p style="font-size:70%">Customers have a money back guarantee if they want to return goods within 7 full working days of receipt, as long as they are in a re-saleable condition and have not been removed from their packaging. If the goods are not faulty, the cost of the goods and outward delivery will be refunded, but not the return postage and relevant insurance. The goods remain under the Buyer\'s title until they are received by Redline Security.</p>
<p style="font-size:70%">Non faulty goods must be returned by the Buyer at the Buyer’s expense and should be adequately insured during the return journey. The Buyer will receive a refund of all monies paid for the Goods (including outward delivery charges but excluding return postal charges and insurance) within 15 days of cancellation.</p>
<p style="padding-bottom:10px;font-size:70%">Please ensure that you have read and understood our terms and conditions, which can be found on our website at <a href="http://www.home-security-store.co.uk/terms_and_conditions.php" style="color: #f7941d;font-weight: bold;">www.home-security-store.co.uk/terms_and_conditions.php</a></p></td></tr>
<tr><td style="padding:10px;margin:0;font-size:70%;color:#999;background:#333;width:100%;">
<p>Copyright 2004-'.date("Y").' Redline Security. All Rights Reserved. Redline Security and Home Security Store are trading names of TIHS Ltd.</p>
<p>Our registered office is: Oak House, Groes Lwyd, Abergele, Conwy LL22 7SU. We are registered in England and Wales. Company No 5325603. VAT Number 855446109.</p>
</td></tr>
</table>
</body>
</html>');


$headers  = "MIME-Version: 1.0\r\n";
$headers .= "Content-type: text/html; charset=iso-8859-1\r\n";
$headers .= "To: ".$_POST[name]." <".$_POST[email].">\r\n";
$headers .= "From: Home Security Store<hs@redlinesecurity.co.uk>\r\n";
$subject = "Home Security Store | Order Confirmation ".$order_number."";
mail($to, $subject, $message, $headers);

/*
$headers  = "MIME-Version: 1.0\r\n";
$headers .= "Content-type: text/html; charset=iso-8859-1\r\n";
$headers .= "To: Steph <admin@theitc.co.uk>\r\n";
$headers .= "Cc: Airs <aaron.hughes@theitc.co.uk>\r\n";
$headers .= "From:  Home Security Store | Copy of order<hs@redlinesecurity.co.uk>\r\n";
$subject = "Home Security Store | Copy of order ".$order_number."";
mail($to, $subject, $message, $headers);
*/
	
	
	
//SAGE TRANSACTIONAL EMAIL (Mike Roberts)
require("assets/widgets/sage_trans.php");

	}
elseif ($_GET[ref] == "bad")
	{
	echo("<h3>Sorry, your order cannot be processed because your payment card has been declined</h3>
<p>Please contact the bank that issued your card to find out why this might have happened. Please also check the expiry date on your card in case the card is no longer valid.<br />
<br />
</p>
<p><strong>Please note, only your issuing bank can tell you why your card has been declined.</strong></p>
<p>If you would like to try the order again with a different card, please go back and confirm all the order details are correct at the <a href='http://www.home-security-store.co.uk/basket.php' title='Click here to return to basket'>basket</a>.</p>
<p>You will then be able to place the order again with the new card payment details. </p>
");
	}
?>

<!-- Google Code for purchase Conversion Page -->
<script language="JavaScript" type="text/javascript">
<!--
var google_conversion_id = 1064249147;
var google_conversion_language = "en_GB";
var google_conversion_format = "1";
var google_conversion_color = "FFFFFF";
if (1) {
  var google_conversion_value = 1;
}
var google_conversion_label = "purchase";
//-->
</script>
<script language="JavaScript" src="http://www.googleadservices.com/pagead/conversion.js">
</script>
<noscript>
<img height=1 width=1 border=0 src="http://www.googleadservices.com/pagead/conversion/1064249147/imp.gif?value=1&label=purchase&script=0">
</noscript>

  </div>
  <div id="footer">
    <p><a href="http://www.securetrading.com" title="A link to the SecureTrading website (This will open in a new window)" target="_blank"><img src="http://www.home-security-store.co.uk/assets/images/shell/secure_trading_logo.jpg" alt="Secure Trading Merchant Logo" /></a></p>
<p><a href="http://www.home-security-store.co.uk/terms_and_conditions.php<?php echo $clear ?>" title="Click here to view our terms and conditions">Terms and Conditions</a> | <a href="http://www.home-security-store.co.uk/deliveries_and_returns.php<?php echo $clear ?>" title="Click here to view information regarding deliveries and returns">Deliveries and Returns</a> | <a href="http://www.home-security-store.co.uk/privacy_policy.php<?php echo $clear ?>" title="Click here to view our privacy policy">Privacy Policy</a> | <a href="http://www.home-security-store.co.uk/site_map.php<?php echo $clear ?>" title="Click here to view our site map">Site Map</a></p>

<p>&copy; <a href="http://www.home-security-store.co.uk/copyright.php<?php echo $clear ?>" title="Click here to view our copyright notice">Copyright</a> 2004-2007 <a href="http://www.redlinesecurity.co.uk" title="A link to our parent website (This will open in a new window)">Redline Security</a> E&amp;OE. All Rights Reserved. Redline Security is a trading name of TIHS Ltd. Our registered office is: Oak House, Groes Lwyd, Abergele, Conwy LL22 7SU. We are registered in England and Wales. Company No 5325603. VAT Number 855446109. </p>
  </div>
</div>
</body>
</html>