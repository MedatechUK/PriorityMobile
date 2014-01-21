<?php 
// SECURITY CHECK TO MAKE SURE THE CORRECT USERNAME AND PASSWORD HAS BEEN ENTERED
if(!$user_name | !$user_pwd)
{
header("location: login.php");
exit;
}

require_once('../assets/widgets/mysql.class.php');
require_once('../assets/widgets/global.inc.php');
require_once('../assets/widgets/global_variables.php');
require_once('../assets/widgets/functions.inc.php');
include("FCKeditor/fckeditor.php"); 
?>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
<title>Admin Interface</title>
<link rel="shortcut icon" href="http://www.personal-attack-alarms.net/favicon.ico" />
<link rel="stylesheet" href="../assets/css/admin.css" type="text/css" media="screen" />
<script language="javascript" src="../assets/scripts/calender/cal2.js"></script>
<script language="javascript" src="../assets/scripts/calender/cal_conf2.js"></script>
<script language="javascript" type="text/javascript" src="../assets/scripts/lib.js"></script>
<script language="javascript" type="text/javascript" src="../assets/scripts/popup.js"></script>
<script>
function modelesswin(url,mwidth,mheight){
if (document.all&&window.print) //if ie5
eval('window.showModelessDialog(url,"","help:0;resizable:1;dialogWidth:'+mwidth+'px;dialogHeight:'+mheight+'px")')
else
eval('window.open(url,"","width='+mwidth+'px,height='+mheight+'px,resizable=1,scrollbars=1")')
}
</script>

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
    <?php

	
//VIEW ORDER INFO
if($_GET[id])
	{
	if($_GET[update] == 'yes')
		{
		$date_added = date("YmdHis");
		$process = mysql_query("insert into SHOP1_order_status (order_id, action, notes, date) VALUES ('$_GET[id]', '$_POST[status]', '$_POST[notes]', '$date_added')");
		
		if (!$process)
		{
		echo("<div class='errormessage'><p><strong>Oops!</strong></p><p>There was a problem when updating the order.</p></div>");
		}
	else
		{
		
		
		//SEND UPDATE EMAIL
		if ($_POST[send_email] == 'yes')
		{
		$query = mysql_query("select * from SHOP1_orders where id = '$_GET[id]'");
		while ($result = mysql_fetch_array($query))
			{
			$name = explode("<br/>", $result[billing_address]);
		
		$message = '<html>
<body style="font-family:Verdana;font-size: 80%;background:#fff;">
<table width="550" border="0" cellpadding="0" style="margin:5px; border: 1px solid #999">
<tr><td>
<img src="http://www.personal-attack-alarms.net/assets/images/email_header.jpg" alt="personal-attack-alarms.net"/>
</td></tr>
<tr><td style="padding:10px">
'.slashes($_POST[notes]).'';

$message .= '<p style="font-size:80%">Email: <a href="mailto:paa@redlinesecurity.co.uk" style="color: #4c7a08;font-weight: bold;">paa@redlinesecurity.co.uk</a> | Tel: 01745 828499</p><h3 style="color: #000; padding: 0; margin: 0; font-size: 100%">Terms and Conditions</h3>
<p style="font-size:70%">All customers wishing to return a product must first contact our customer services on 01745 828499 or email <a href="mailto:paa-returns@redlinesecurity.co.uk" style="color: #4c7a08;font-weight: bold;">paa-returns@redlinesecurity.co.uk</a>. Please include your order number and a reason for why you want to return the item (for example if the wrong item was delivered, or an incorrect quantity). You will then be sent out a returns pack.</p>
<p style="font-size:70%">Customers have a money back guarantee if they want to return goods within 7 full working days of receipt, as long as they are in a re-saleable condition and have not been removed from their packaging. If the goods are not faulty, the cost of the goods and outward delivery will be refunded, but not the return postage and relevant insurance. The goods remain under the Buyer\'s title until they are received by Redline Security.</p>
<p style="font-size:70%">Non faulty goods must be returned by the Buyer at the Buyer\'s expense and should be adequately insured during the return journey. The Buyer will receive a refund of all monies paid for the Goods (including outward delivery charges but excluding return postal charges and insurance) within 15 days of cancellation.</p>
<p style="padding-bottom:10px;font-size:70%">Please ensure that you have read and understood our terms and conditions, which can be found on our website at <a href="http://www.personal-attack-alarms.net/terms_and_conditions.php" style="color: #4c7a08;font-weight: bold;">personal-attack-alarms.net/terms_and_conditions.php</a></p></td></tr>
<tr><td style="padding:10px;margin:0;font-size:70%;color:#999;background:#333;width:100%;">
<p>Copyright 2004-'.date("Y").' Redline Security. All Rights Reserved. Redline Security is a trading name of TIHS Ltd.</p>
<p>Our registered office is: Oak House, Groes Lwyd, Abergele, Conwy LL22 7SU. We are registered in England and Wales. Company No 5325603. VAT Number 855446109.</p>
</td></tr>
</table>
</body>
</html>';
		
		
$headers  = "MIME-Version: 1.0\r\n";
$headers .= "Content-type: text/html; charset=iso-8859-1\r\n";
$headers .= "To: ".$name[0]." <".$result[email].">\r\n";
$headers .= "From: Redline Security | personal-attack-alarms.net<paa@redlinesecurity.co.uk>\r\n";
$subject = "personal-attack-alarms.net | Order Update ".$result[order_number]."";
mail($to, $subject, $message, $headers);

$headers  = "MIME-Version: 1.0\r\n";
$headers .= "Content-type: text/html; charset=iso-8859-1\r\n";
$headers .= "To: Redline Security <paa@redlinesecurity.co.uk>\r\n";
$headers .= "From:  personal-attack-alarms.net | Copy of order update\r\n";
$subject = "personal-attack-alarms.net | Copy of order update ".$result[order_number]."";
mail($to, $subject, $message, $headers);
			}
			
		echo("<div class='okmessage'><p><strong>Email sent to customer</strong></p><p>The customer has been notified.</p></div>");
		}
		
		$tracker=mysql_query("insert into SHOP1_admin_tracker (name, loggedin, ip, job) values ('$_COOKIE[user_name]', '$tracker_time', '$ip', 'Order Updated - ($_POST[status] - $_GET[id])')");
		//END OF SEND UPDATE
		
		
		echo("<div class='okmessage'><p><strong>Order updated!</strong></p><p>The order status has been updated.</p></div>");
		}
	}	
	$query = mysql_query("select * from SHOP1_orders where id='$_GET[id]'");
	while($result = mysql_fetch_array($query))
	{
	$date = date("d/m/Y H:i:s", strtotime($result[date]));
	echo("<p class='buttons'><a href='orders.php'>Back to table of orders</a></p>");
	echo("<h2>Order Information</h2>");
	echo("<div class='admin_order'>".slashes($result[orderinfo])."</div>");
	echo("<div class='admin_delivery'><h3>Delivery Address</h3><p>$result[delivery_address]</p></div>");
	echo("<div class='admin_billing'><h3>Billing Address</h3><p>$result[billing_address]</p></div>");
	echo("<div class='admin_payment'><h3>Payment Information</h3>");
	echo("<p>Order total: <strong>$result[total_cost] (GBP)</strong><br/>ST Reference: <strong>$result[streference]</strong><br/>ST Authcode: <strong>$result[stauthcode]</strong><br/>Order date: <strong>$date</strong></p></div>");
	
	
	?>
	<h2>Order History</h2>
	<div id='status'>
        <table>
          <tr>
            <th width="150">Date</th>
            <th width="120">Status</th>
            <th>Note</th>
          </tr>
          <?php 
	
	echo("<tr><td>$date</td><td><span style='font-weight:bold;color:#c60;background:url(\"../assets/images/icons/alert.gif\") no-repeat left;padding:2px 0 2px 20px;'>Pending</span></td><td></td></tr>");
	
	$statusquery = mysql_query("select * from SHOP1_order_status where order_id = '$_GET[id]' order by id asc");
	$numrows = mysql_num_rows($statusquery);
	
	if ($numrows > 0)
		{
		while($statusresult = mysql_fetch_array($statusquery))
			{
			$statusdate = date("d/m/Y H:i:s", strtotime($statusresult[date]));
			echo("<tr><td>$statusdate</td><td>");
			
			if ($statusresult[action] == "Pending")
				{
				echo("<span style='font-weight:bold;color:#c60;background:url(\"../assets/images/icons/alert.gif\") no-repeat left;padding:2px 0 2px 20px;'>Pending</span>");
				}
			elseif ($statusresult[action] == "Refunded")
				{
				echo("<span style='font-weight:bold;color:#F00;background:url(\"../assets/images/icons/bad.gif\") no-repeat left;padding:2px 0 2px 20px;'>Refunded</span>");
				}
			elseif ($statusresult[action] == "Returned")
				{
				echo("<span style='font-weight:bold;color:#F00;background:url(\"../assets/images/icons/alert.gif\") no-repeat left;padding:2px 0 2px 20px;'>Returned</span>");
				}
			elseif ($statusresult[action] == "Cancelled")
				{
				echo("<span style='font-weight:bold;color:#F00;background:url(\"../assets/images/icons/bad.gif\") no-repeat left;padding:2px 0 2px 20px;'>Cancelled</span>");
				}
			elseif ($statusresult[action] == "Awaiting Stock")
				{
				echo("<span style='font-weight:bold;color:#F90;background:url(\"../assets/images/icons/out_of_stock.gif\") no-repeat left;padding:2px 0 2px 20px;'>Awaiting Stock</span>");
				}
			else
				{
				echo("<span style='font-weight:bold;color:#090;background:url(\"../assets/images/icons/dispatched.gif\") no-repeat left;padding:2px 0 2px 20px;'>$statusresult[action]</span>");
				}
			echo("</td><td>$statusresult[notes]</td></tr>");
			}
	
		}
	
	
	
	?>
        </table>
      </div>
    <form method="post" action="orders.php?id=<?php echo("$_GET[id]");?>&amp;update=yes">
      <fieldset>
      <legend>Order Status</legend>
      
      <p>
        <label><span>Status</span>
        <select name="status">
		  <option value="Pending">Pending</option>
          <option value="Dispatched">Dispatched</option>
          <option value="Refunded">Refunded</option>
          <option value="Awaiting Stock">Awaiting Stock</option>
          <option value="Returned">Returned</option>
		  <option value="Cancelled">Cancelled</option>
        </select>
        </label>
      </p>
	  <p>
	  <label><span>Notes<br />
<a href="javascript:modelesswin('sp.php?id=<?php echo $result[id] ?>',600,400)" title="Opens in a new window" class="new_window">Standard paragraphs</a></span>
	  
	  
        <?php
//THIS IS AN INSTANCE OF THE PRETTY TEXT EDITOR

//THIS IS THE NAME OF THE FIELD
$oFCKeditor = new FCKeditor('notes') ;

//IGNORE THIS LINE
$oFCKeditor->BasePath = 'FCKeditor/';


//WIDTH AND HEIGHT OF TEXT AREA
$oFCKeditor->Width  = '490' ;
$oFCKeditor->Height = '300' ;

// IGNORE THESE
$oFCKeditor->ToolbarSet = 'summary';
$oFCKeditor->Create() ;
?>
        </label>
      </p>
	  <p>
        <label><span>Notify customer</span>
        <input type="checkbox" value="yes" name="send_email" onClick="JAVASCRIPT:if (this.checked){alert('Caution. This action will be sent by email to the customer.');} else {alert('The action will be added to the system only.');}" />
        </label>
      </p>
	  <p><strong>If dispatching goods remember to add courier company name/postage method and tracking number to the email text.</strong></p>
	   <p class="buttons"><input name="submit" type="image" value="Submit" src="../assets/images/update_order.gif" alt="Update Order" /></p> 
      </fieldset>
    </form>
    <?php
	echo("<p class='buttons'><a href='orders.php'>Back to table of orders</a></p>");
}
}
else
	{
?>
<div class="search">
  <form name="search" method="get" action="<?php $PHP_SELF;  ?>?search=yes">
  <fieldset>
  <legend>Search by date range</legend>
<input type="text" name="firstinput" size="10" value="Today"> <span><a href="javascript:showCal('Calendar1')">Select Date</a></span> <input type="text" name="secondinput" size="10" value="Today"> <span><a href="javascript:showCal('Calendar2')">Select Date</a></span><input type="submit" name="submit" value="Go" />
</fieldset>
</form>
  </div>
<?php  
  if($_GET[submit] == "Go")
  	{
	
	if ($_GET[firstinput] == "Today")
		{
		$first	= date("Y-m-d");
		}
	else
		{
		$first 	= str_replace("/", "-", $_GET[firstinput]);
		}
		
	if ($_GET[secondinput] == "Today")
		{
		$second	= date("Y-m-d");
		}
	else
		{
		$second = str_replace("/", "-", $_GET[secondinput]);
		}
	
	$firstdate = date("d/m/Y", strtotime($first));
	$seconddate = date("d/m/Y", strtotime($second));
	$link_string 		= "&amp;firstinput=$_GET[firstinput]&amp;secondinput=$_GET[secondinput]&amp;submit=Go";
	$search_string 		= "WHERE date >= '".$first." 00:00:00' and date <= '".$second." 23:59:59'";
	$friendly_string 	= "<p><small>Results for date range <strong>$firstdate</strong> to <strong>$seconddate</strong></small></p>";
  	}
  
	if (!($limit)){
$limit = 25;} // Default results per-page.
if (!($page)){
$page = 0;} // Default page value.
$numresults = mysql_query("select * from SHOP1_orders $search_string"); // the query.
$numrows = mysql_num_rows($numresults); // Number of rows returned from above query.

$pages = intval($numrows/$limit); // Number of results pages.

// $pages now contains int of pages, unless there is a remainder from division.

if ($numrows%$limit) {
$pages++;} // has remainder so add one page

$current = ($page/$limit) + 1; // Current page number.

if (($pages < 1) || ($pages == 0)) {
$total = 1;} // If $pages is less than one or equal to 0, total pages is 1.

else {
$total = $pages;} // Else total pages is $pages value.

$first = $page + 1; // The first result.

if (!((($page + $limit) / $limit) >= $pages) && $pages != 1) {
$last = $page + $limit;} //If not last results page, last result equals $page plus $limit.
 
else{
$last = $numrows;} // If last results page, last result equals total number of results.

	echo("<h2>Order Information</h2>");
	echo("$friendly_string<p><strong>Orders $first to $last of $numrows</strong></p>");
	echo("<div id='orders'>
	<table width='90%'>
		<thead>
			<tr><th>Order Ref.</th><th width='250'>Name</th><th width='100'>Value</th><th width='110'>Date</th><th width='110'>Status</th><th width='150'>Options</th></tr>
		</thead>
		<tbody>
	
	");
	$subtotal = 0;
	$query = mysql_query("select * from SHOP1_orders $search_string ORDER BY id DESC LIMIT $page, $limit");
	while($result = mysql_fetch_array($query))
		{
		
		$order_val = explode("£", $result[total_cost]);
		
		$subtotal += $order_val[1];
		
		$date = date("d/m/Y", strtotime($result[date]));
		$billing = explode("<br/>", $result[billing_address]);
		$order = explode("/", $result[order_number]);
		
		echo("<tr class='row2'><td>$order[1]</td><td>$billing[0]</td><td>$result[total_cost]</td><td>$date</td><td>");
		
		
	$statusquery = mysql_query("select * from SHOP1_order_status where order_id = '$result[id]' order by id desc limit 1 ");
	$numrows = mysql_num_rows($statusquery);
	
	if ($numrows > 0)
		{
		while($statusresult = mysql_fetch_array($statusquery))
			{
			if ($statusresult[action] == "Pending")
				{
				echo("<span style='font-weight:bold;color:#F00;background:url(\"../assets/images/icons/alert.gif\") no-repeat left;padding:2px 0 2px 20px;'>Pending</span>");
				}
			elseif ($statusresult[action] == "Refunded")
				{
				echo("<span style='font-weight:bold;color:#F00;background:url(\"../assets/images/icons/bad.gif\") no-repeat left;padding:2px 0 2px 20px;'>Refunded</span>");
				}
			elseif ($statusresult[action] == "Returned")
				{
				echo("<span style='font-weight:bold;color:#F00;background:url(\"../assets/images/icons/alert.gif\") no-repeat left;padding:2px 0 2px 20px;'>Returned</span>");
				}
			elseif ($statusresult[action] == "Cancelled")
				{
				echo("<span style='font-weight:bold;color:#F00;background:url(\"../assets/images/icons/bad.gif\") no-repeat left;padding:2px 0 2px 20px;'>Cancelled</span>");
				}
			elseif ($statusresult[action] == "Awaiting Stock")
				{
				echo("<span style='font-weight:bold;color:#F90;background:url(\"../assets/images/icons/out_of_stock.gif\") no-repeat left;padding:2px 0 2px 20px;'>Awaiting Stock</span>");
				}
			else
				{
				echo("<span style='font-weight:bold;color:#090;background:url(\"../assets/images/icons/dispatched.gif\") no-repeat left;padding:2px 0 2px 20px;'>$statusresult[action]</span>");
				}
			}
		}
	else
		{
		echo("<span style='font-weight:bold;color:#F00;background:url(\"../assets/images/icons/alert.gif\") no-repeat left;padding:2px 0 2px 20px;'>Pending</span>");
		}
		
		echo("</td><td><a href='orders.php?id=$result[id]' class='view_order'>View</a> | ");
		
		?>
		<a href='invoice.php?id=$result[id]' target='_blank' onclick="raw_popup('invoice.php?id=<?php echo $result[id] ?>'); return false">Invoice</a>
		
		<?php
		echo("</td></tr>");		
		}
	echo("<tr class='summary'><td colspan='6'>Sum of above orders: <strong>&pound;".number_format($subtotal, 2, '.', '')."</strong> | <a href='export_orders.php' class='export'>Export orders</a></td>");
	
	echo("</tbody></table></div>");
	
	echo("<div class='pages'><p>");


if ($page != 0) { // Don't show back link if current page is first page.
$back_page = $page - $limit;
echo("<a href='$PHP_SELF?page=$back_page&amp;limit=$limit$link_string'>previous page</a> ");}

for ($i=1; $i <= $pages; $i++) // loop through each page and give link to it.
{
 $ppage = $limit*($i - 1);
 if ($ppage == $page){
 echo("<strong>[$i]</strong> ");} // If current page don't give link, just text.
 else{
 echo("<a href='$PHP_SELF?page=$ppage&amp;limit=$limit$link_string'>$i</a> ");}
}

if (!((($page+$limit) / $limit) >= $pages) && $pages != 1) { // If last page don't give next link.
$next_page = $page + $limit;
echo(" <a href='$PHP_SELF?page=$next_page&amp;limit=$limit$link_string'>next page</a>");}
	
	echo("</p></div>");
	
	?>
    <form method="get" action="search_orders.php">
      <p><strong>Search for an order</strong></p>
      <label>Keyword
      <input type="text" name="search" />
      </label>
      <input type="submit" name="Submit" value="Search" />
    </form>
    <?php	
	}
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
