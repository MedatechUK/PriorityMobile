<?php

if ($ref == "good")
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
<title>personal-attack-alarms.net</title>
<link rel="stylesheet" href="http://www.personal-attack-alarms.net/assets/css/screen.css" type="text/css" media="screen" />
<link rel="stylesheet" href="http://www.personal-attack-alarms.net/assets/css/print.css" type="text/css" media="print" />
<link rel="shortcut icon" href="http://www.personal-attack-alarms.net/favicon.ico" />
<script language="JavaScript" type="text/javascript">function printPage() { print(document); }</script>
</head>
<body>
<div id="hidden">
 <div id="hide">
<a href="http://www.personal-attack-alarms.net/" title="Home Page"><img src="http://www.personal-attack-alarms.net/assets/images/logo.gif" alt="Personal Attack Alarms Logo" /></a>
<!-- <script>
microsoft_adcenterconversion_domainid = 42872;
 microsoft_adcenterconversion_cp = 5050; 
 </script>
<script src="https://0.r.msn.com/scripts/microsoft_adcenterconversion.js"></script>
<noscript><img width="1" height="1" src="https://42872.r.msn.com/?type=1&cp=1"/></noscript> -->
</div>

<p><a href="mailto:paa@redlinesecurity.co.uk" title="Click here to email us">paa@redlinesecurity.co.uk</a> | 0845 450 1373</p>
</div>
<div id="container">
  <div id="header">
    <h2>Checkout</h2>
  </div>
  <div id="navigation">
  <ul>
<li><a href="http://www.personal-attack-alarms.net/index.php<?php echo $clear;?>">Home</a></li>
<li><a href="http://www.personal-attack-alarms.net/products.php<?php echo $clear;?>">Products</a></li>
<li><a href="http://www.personal-attack-alarms.net/about.php<?php echo $clear;?>">About us</a></li>
<li><a href="http://www.personal-attack-alarms.net/contact.php<?php echo $clear;?>">Contact us</a></li>
<li><a href="http://www.personal-attack-alarms.net/how_a_personal_attack_alarm_will_help.php<?php echo $clear;?>">Why buy?</a></li>

</ul>
  </div>
  <div id="main_content">
    <h2>Checkout > Payment Result</h2>
    <p class="step">Checkout > Delivery information > Confirmation > Payment > <strong>Complete</strong></p>
    <?php
	
	if ($ref == "good")
		{
		$sent = date('d-m-Y H:i');
		echo('<div class="print_page"><a href="#" id="printpage" onclick="printPage()" title="Print a paper friendly version of this page">Print order details</a></div>');
		echo("<h3>Your order has been processed</h3>");
		echo("<p>An email has been sent to $_POST[email] with the details of your order.</p>");
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
			
		
//EMAIL TO CUSTOMER & SITE ADMIN
			
$message = '<html>
<body>
<body style="font-family:Verdana;font-size: 80%;background:#fff;">
<table width="550" border="0" cellpadding="0" style="margin:5px; border: 1px solid #999">
<tr><td>
<img src="http://www.personal-attack-alarms.net/assets/images/email_header.jpg" alt="personal-attack-alarms.net"/>
</td></tr>
<tr><td style="padding:10px">
<h2 style="color: #000; padding: 0; margin: 0; font-size: 120%">Order Confirmation</h2>
<p style="font-size:80%">Order Number : '.$_POST[orderref].'</p>
<p style="font-size:80%">Order Date: '.$sent.'</p>
<h3 style="color: #000; padding: 0; margin: 0; font-size: 110%">Products</h3>';

$orderinfo = slashes($_POST[orderinfo]);
$orderinfo = str_replace("<table id='basketconfirm'>", "<table style='border-collapse: collapse; width: 100%; border: 1px solid #999;font-size: 90%;'>", $orderinfo);

$orderinfo = str_replace("<tr class='subtotal'>", "<tr style='background: #ededed;text-align: right; border-top: 3px double #999;'>", $orderinfo);
$orderinfo = str_replace("<tr class='postage'>", "<tr style='background: #ededed;text-align: right;'>", $orderinfo);
$orderinfo = str_replace("<tr class='total'>", "<tr style='background: #ededed;text-align: right;'>", $orderinfo);
$orderinfo = str_replace("<th>", "<th style='padding: 3px; background:url(http://www.personal-attack-alarms.net/assets/images/product_h3_bg.gif) #acd700; text-align:left'>", $orderinfo);

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
<p style="font-size:80%">Email: <a href="mailto:paa@redlinesecurity.co.uk" style="color: #4c7a08;font-weight: bold;">paa@redlinesecurity.co.uk</a> | Tel: 01745 828499</p>
</td></tr>
<tr><td style="padding:10px">
<h3 style="color: #000; padding: 0; margin: 0; font-size: 100%">Terms and Conditions</h3>
<p style="font-size:70%">All customers wishing to return a product must first contact our customer services on 01745 828499 or email <a href="mailto:paa-returns@redlinesecurity.co.uk" style="color: #4c7a08;font-weight: bold;">paa-returns@redlinesecurity.co.uk</a>. Please include your order number and a reason for why you want to return the item (for example if the wrong item was delivered, or an incorrect quantity). You will then be sent out a returns pack.</p>
<p style="font-size:70%">Customers have a money back guarantee if they want to return goods within 7 full working days of receipt, as long as they are in a re-saleable condition and have not been removed from their packaging. If the goods are not faulty, the cost of the goods and outward delivery will be refunded, but not the return postage and relevant insurance. The goods remain under the Buyer\'s title until they are received by Redline Security.</p>
<p style="font-size:70%">Non faulty goods must be returned by the Buyer at the Buyer’s expense and should be adequately insured during the return journey. The Buyer will receive a refund of all monies paid for the Goods (including outward delivery charges but excluding return postal charges and insurance) within 15 days of cancellation.</p>
<p style="padding-bottom:10px;font-size:70%">Please ensure that you have read and understood our terms and conditions, which can be found on our website at <a href="http://www.personal-attack-alarms.net/terms_and_conditions.php" style="color: #4c7a08;font-weight: bold;">personal-attack-alarms.net/terms_and_conditions.php</a></p></td></tr>
<tr><td style="padding:10px;margin:0;font-size:70%;color:#999;background:#333;width:100%;">
<p>Copyright 2004-'.date("Y").' Redline Security. All Rights Reserved. Redline Security is a trading name of TIHS Ltd.</p>
<p>Our registered office is: Oak House, Groes Lwyd, Abergele, Conwy LL22 7SU. We are registered in England and Wales. Company No 5325603. VAT Number 855446109.</p>
</td></tr>
</table>
</body>
</html>');


$headers  = "MIME-Version: 1.0\r\n";
$headers .= "Content-type: text/html; charset=iso-8859-1\r\n";
$headers .= "To: ".$_POST[name]." <".$_POST[email].">\r\n";
$headers .= "From: Redline Security | personal-attack-alarms.net<paa@redlinesecurity.co.uk>\r\n";
$subject = "personal-attack-alarms.net | Order Confirmation ".$_POST[orderref]."";
mail($to, $subject, $message, $headers);

/*
$headers  = "MIME-Version: 1.0\r\n";
$headers .= "Content-type: text/html; charset=iso-8859-1\r\n";
$headers .= "To: Steph <admin@theitc.co.uk>\r\n";
$headers .= "Cc: Airs <aaron.hughes@theitc.co.uk>\r\n";
$headers .= "From:  personal-attack-alarms.net | Copy of order<paa@redlinesecurity.co.uk>\r\n";
$subject = "personal-attack-alarms.net | Copy of order ".$_POST[orderref]."";
mail($to, $subject, $message, $headers);
*/
		
//SAGE TRANSACTIONAL EMAIL (Mike Roberts)
require("assets/widgets/sage_trans.php");


//ADD ORDER TO DB

//---------------------------------------------------------------------------
$host					=	"mysql.personal-attack-alarms.net";
$user					=	"paalarms";
$pass					=	"477ack";
$database				=	"PAALARMS";
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

$process=mysql_query("INSERT INTO SHOP1_orders (order_number, cartinfo, orderinfo, total_cost, email, date, streference, stauthcode, stresult, billing_address, delivery_address, postage) VALUES ('$_POST[orderref]', '$_POST[cartinfo]', '$_POST[orderinfo]', '$_POST[total]', '$_POST[email]', '$date_added', '$_POST[streference]', '$_POST[stauthcode]', '$_POST[stresult]', '$billing_details', '$delivery_details', '$_POST[deliverymethod]')"); 
  
	if (!$process)
		{
		echo("<div class='errormessage'><p><strong>Oops!</strong></p><p>There was a problem when adding the order to the system. Please retain your order email for future reference.</p></div>");
		}
	else
		{
		echo("<!-- Order added to DB -->");
		}
	}
elseif ($ref == "bad")
	{
	echo("<h3>Sorry, your order cannot be processed because your payment card has been declined</h3>
<p>Please contact the bank that issued your card to find out why this might have happened. Please also check the expiry date on your card in case the card is no longer valid.<br />
<br />
</p>
<p><strong>Please note, only your issuing bank can tell you why your card has been declined.</strong></p>
<p>If you would like to try the order again with a different card, please go back and confirm all the order details are correct at the <a href='http://www.personal-attack-alarms.net/basket.php' title='Click here to return to basket'>basket</a>.</p>
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
    <a href="http://www.securetrading.com" title="A link to the SecureTrading website (This will open in a new window)" target="_blank"><img src="http://www.personal-attack-alarms.net/assets/images/ST_Merchant_logo.gif" alt="Secure Trading Merchant Logo" /></a><p><a href="http://www.personal-attack-alarms.net/terms_and_conditions.php" title="Click here to view our terms and conditions">Terms and Conditions</a> | <a href="http://www.personal-attack-alarms.net/deliveries_and_returns.php" title="Click here to view information regarding deliveries and returns">Deliveries and Returns</a> | <a href="http://www.personal-attack-alarms.net/privacy_policy.php" title="Click here to view our privacy policy">Privacy Policy</a> | <a href="http://www.personal-attack-alarms.net/copyright.php" title="Click here to view our copyright notice">Copyright Notice</a></p>
<p>&copy; Copyright 2004-<?php echo date("Y"); ?> <a href="http://www.redlinesecurity.co.uk" title="A link to our parent website (This will open in a new window)">Redline Security</a>. All Rights Reserved. Redline Security is a trading name of TIHS Ltd. Our registered office is: Oak House, Groes Lwyd, Abergele, Conwy LL22 7SU. We are registered in England and Wales. Company No 5325603. VAT Number 855446109. </p>
  </div>
</div>
<script src="http://www.google-analytics.com/urchin.js" type="text/javascript">
</script>
<script type="text/javascript">
_uacct = "UA-514855-9";
_udn="none";
_ulink=1;
urchinTracker();
</script>


</body>
</html>
<?php

//FOOTPRINT TRACKER
//---------------------------------------------------------------------------
$host					=	"mysql.personal-attack-alarms.net";
$user					=	"paalarms";
$pass					=	"477ack";
$database				=	"PAALARMS";
//---------------------------------------------------------------------------

mysql_connect($host, $user, $pass);
mysql_select_db($database);


function selfURL() 
	{ 
	$s = empty($_SERVER["HTTPS"]) ? '' : ($_SERVER["HTTPS"] == "on") ? "s" : ""; 
	$protocol = strleft(strtolower($_SERVER["SERVER_PROTOCOL"]), "/").$s; 
	$port = ($_SERVER["SERVER_PORT"] == "80") ? "" : (":".$_SERVER["SERVER_PORT"]); 
	return $protocol."://".$_SERVER['SERVER_NAME'].$port.$_SERVER['REQUEST_URI']; 
	} 
	
function strleft($s1, $s2) 
	{ 
	return substr($s1, 0, strpos($s1, $s2)); 
	}

$refurl 	= 	$_SERVER['HTTP_REFERER'];
$refdate 	= 	date("d-m-y G:i:s");
$refip 		= 	$_SERVER['REMOTE_ADDR'];
$this_url 	=	selfURL();
$session_id	=	session_id();

$footprint = mysql_query("insert into SHOP1_footprints (url, session_id, this_url, date, ip) values ('$refurl', '$session_id', '$this_url', '$refdate', '$refip')");

?>