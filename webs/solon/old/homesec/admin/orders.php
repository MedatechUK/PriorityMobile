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
include("FCKeditor/fckeditor.php"); 
?>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
<title>Admin Interface</title>
<link rel="shortcut icon" href="../assets/images/favicon.ico" />
<link rel="stylesheet" href="../assets/css/admin.css" type="text/css" media="screen" /> 


<link rel="stylesheet" href="../assets/scripts/modal/css/moodalbox.css" type="text/css" media="screen" />
<script type="text/javascript" src="../assets/scripts/modal/js/mootools.js"></script> 
<script type="text/javascript" src="../assets/scripts/modal/js/moodalbox.js"></script> 


<script language="javascript" src="../assets/scripts/calender/cal2.js"></script>
<script language="javascript" src="../assets/scripts/calender/cal_conf2.js"></script>
<script language="javascript" type="text/javascript" src="../assets/scripts/lib.js"></script>
<script language="javascript" type="text/javascript" src="../assets/scripts/popup.js"></script>
<script languagee="javascript">

<!-- Begin
function popUp(URL) {
day = new Date();
id = day.getTime();
eval("page" + id + " = window.open(URL, '" + id + "', 'toolbar=0,scrollbars=1,location=0,statusbar=0,menubar=0,resizable=0,width=600,height=400,left = 440,top = 212');");
}
// End -->
</script>

</head>
<body>
<noscript>
<h1>Warning</h1>
<p class="noscript">To use this site correctly you need to have JavaScript enabled on your web browser</p>
</noscript>
<div id="hidden">
  <?php require("../assets/widgets/hidden.php"); ?>
</div>
<div id="container">
  <div id="header">
    <h2>Website</h2>
	<p>Solon: 01352 762266<br />Sue: 01352 736117<br />The ITC: 01745 828440</p>
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
		$process = mysql_query("insert into $order_status_table (order_id, action, notes, date) VALUES ('$_GET[id]', '$_POST[status]', '$_POST[notes]', '$date_added')");
		$tracker=mysql_query("insert into $admin_track_table (name, loggedin, ip, job) values ('$_COOKIE[user_name]', '$tracker_time', '$ip', 'Order - Updated ($_GET[id] - $_POST[status])')");
		
		if (!$process)
		{
		echo("<div id='status_error'><p><strong>Oops!</strong><br/>There was a problem when updating the order.</p></div>");
		}
	else
		{
		
		
		//SEND UPDATE EMAIL
		if ($_POST[send_email] == 'yes')
		{
		$query = mysql_query("select * from $orders_table where id = '$_GET[id]'");
		while ($result = mysql_fetch_array($query))
			{
			$name = explode("<br/>", $result[billing_address]);
		
		$message = '<html>
<body style="font-family:Verdana;font-size: 80%;background:#fff;">
<table width="550" border="0" cellpadding="0" style="margin:5px;">
<tr><td>
<img src="'.$site_url.'assets/images/email_banner.gif" alt="'.$site_name.'"/>
</td></tr>
<tr><td style="padding:10px">
'.slashes($_POST[notes]).'';

$message .= '<p style="font-size:80%">Email: <a href="mailto:'.$site_email.'" style="color: #f7941d;font-weight: bold;">'.$site_email.'</a> | Tel: '.$site_tel.'</p><h3 style="color: #000; padding: 0; margin: 0; font-size: 100%">Terms and Conditions</h3>
<p style="font-size:70%">All customers wishing to return a product must first contact our customer services on '.$site_tel.' or email <a href="mailto:'.$site_email.'" style="color: #f7941d;font-weight: bold;">'.$site_email.'</a>. Please include your order number and a reason for why you want to return the item (for example if the wrong item was delivered, or an incorrect quantity). You will then be sent out a returns pack.</p>
<p style="font-size:70%">Customers have a money back guarantee if they want to return goods within 7 full working days of receipt, as long as they are in a re-saleable condition and have not been removed from their packaging. If the goods are not faulty, the cost of the goods and outward delivery will be refunded, but not the return postage and relevant insurance. The goods remain under the Buyer\'s title until they are received by Redline Security.</p>
<p style="font-size:70%">Non faulty goods must be returned by the Buyer at the Buyer\'s expense and should be adequately insured during the return journey. The Buyer will receive a refund of all monies paid for the Goods (including outward delivery charges but excluding return postal charges and insurance) within 15 days of cancellation.</p>
<p style="padding-bottom:10px;font-size:70%">Please ensure that you have read and understood our terms and conditions, which can be found on our website at <a href="'.$site_url.'terms_and_conditions.php" style="color: #f7941d;font-weight: bold;">'.$site_url.'terms_and_conditions.php</a></p></td></tr>
<tr><td style="padding:10px;margin:0;font-size:70%;color:#999;background:#333;width:100%;">
<p>Copyright 2004-'.date("Y").' Redline Security. All Rights Reserved. Redline Security and Home Security Store are trading names of TIHS Ltd.</p>
<p>Our registered office is: Oak House, Groes Lwyd, Abergele, Conwy LL22 7SU. We are registered in England and Wales. Company No 5325603. VAT Number 855446109.</p>
</td></tr>
</table>
</body>
</html>';
		
		
$headers  = "MIME-Version: 1.0\r\n";
$headers .= "Content-type: text/html; charset=iso-8859-1\r\n";
$headers .= "To: ".$name[0]." <".$result[email].">\r\n";
$headers .= "From: ".$site_name." Customer Support<".$site_email.">\r\n";
$subject = "".$site_name." | Order Update ".$result[order_number2]."";
mail($to, $subject, $message, $headers);

$headers  = "MIME-Version: 1.0\r\n";
$headers .= "Content-type: text/html; charset=iso-8859-1\r\n";
$headers .= "To: Redline Security <".$site_email.">\r\n";
$headers .= "From:  ".$site_name." Customer Support<".$site_email.">\r\n";
$subject = "".$site_name." | Copy of order update ".$result[order_number2]."";
mail($to, $subject, $message, $headers);
			}
			
		$email_ok = " The customer has been notified.</p>";
		}
		//END OF SEND UPDATE
		
		
		echo("<div id='status_ok'><p><strong>Order status updated!</strong>$email_ok</p></div>");
		}
	}	
	$query = mysql_query("select * from $orders_table where id='$_GET[id]'");
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
	
	$statusquery = mysql_query("select * from $order_status_table where order_id = '$_GET[id]' order by id asc");
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
<a href="javascript:popUp('sp.php?id=<?php echo $result[id] ?>')">Standard Paragraphs</a></span>
	  
	  
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
	   <p class="buttons"><input name="submit" type="image" value="Submit" src="../assets/images/buttons/update_status.jpg" alt="Update Order" /></p> 
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
  
	if (!($_GET[limit])){
$limit = 25;} // Default results per-page.
else
	{
	$limit = 25;
	}
if (!($_GET[page])){
$page = 0;} // Default page value.
else
	{
	$page = $_GET[page];
	}
if ($_GET[check] == "OK")
		{
		$numresults = mysql_query("select * from $orders_table WHERE order_number LIKE '%$_POST[search]%' || email LIKE '%$_POST[search]%' || total_cost LIKE '%$_POST[search]%' || orderinfo LIKE '%$_POST[search]%' || date LIKE '%$_POST[search]%' || delivery_address LIKE '%$_POST[search]%' || billing_address LIKE '%$_POST[search]%' || streference LIKE '%$_POST[search]%' ORDER BY id DESC");
		}
	else
		{
		$numresults = mysql_query("select * from $orders_table $search_string");
		}

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

	echo("<h2>Order Database</h2>");
	
	if ($_GET[check])
		{
		echo("<p><strong>$numrows results found for: $_POST[search]</strong></p>");
		}
	else
		{
		echo("$friendly_string<p><strong>Orders $first to $last of $numrows</strong></p>");
		}
	echo("<div id='orders'>
	<table width='90%'>
		<thead>
			<tr><th width='50'>Ref.</th><th>Name</th><th width='100'>Value</th><th width='110'>Date</th><th width='110'>Status</th><th width='150'>Options</th></tr>
		</thead>
		<tbody>
	
	");
	$subtotal = 0;
	
	if ($_GET[check] == "OK")
		{
		$query = mysql_query("select * from $orders_table WHERE order_number LIKE '%$_POST[search]%' || email LIKE '%$_POST[search]%' || total_cost LIKE '%$_POST[search]%' || orderinfo LIKE '%$_POST[search]%' || date LIKE '%$_POST[search]%' || delivery_address LIKE '%$_POST[search]%' || billing_address LIKE '%$_POST[search]%' || streference LIKE '%$_POST[search]%' ORDER BY id DESC");
		}
	else
		{
		$query = mysql_query("select * from $orders_table $search_string ORDER BY id DESC LIMIT $page, $limit");
		}
	
	
	while($result = mysql_fetch_array($query))
		{
		
		
		$subtotal += $result[total_cost];
		
		$date = date("d/m/Y", strtotime($result[date]));
		$billing = explode("<br/>", $result[billing_address]);
		//$order = explode("/", $result[order_number]);
		
		if($num == "1")
					{
					$class=" class='row2'";
					$num=2;
					}
				else
					{
					$class=" class='row1'";
					$num=1;
					}
				echo("<tr$class><td>$result[order_number2]</td><td>");
		
		$statusquery = mysql_query("select * from $order_status_table where order_id = '$result[id]' order by id desc limit 1 ");
	$numrows = mysql_num_rows($statusquery);
		
		
		if ($result[postage] == "9.95" && $numrows < 1)
			{
			echo("<span style='font-weight:bold;color:#c60;background:url(\"../assets/images/icons/alert.gif\") no-repeat left;padding:2px 0 2px 20px;'>$billing[0] Next Day Delivery</span>");
			}
			else
			{
			echo $billing[0];
			}
		
		echo("</td><td>&pound;$result[total_cost]</td><td>$date</td><td>");
		
		
	
	
	if ($numrows > 0)
		{
		while($statusresult = mysql_fetch_array($statusquery))
			{
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
			}
		}
	else
		{
		echo("<span style='font-weight:bold;color:#c60;background:url(\"../assets/images/icons/alert.gif\") no-repeat left;padding:2px 0 2px 20px;'>Pending</span>");
		}
		
		echo("</td><td><a href='orders.php?id=$result[id]' class='view_order'>View</a> | ");
		
		?>
		<a href='../admin/invoice.php?id=<?php echo $result[id] ?>' target='_blank' onclick="raw_popup('../admin/invoice.php?id=<?php echo $result[id] ?>'); return false">Invoice</a>
		<?php
		
		//echo("<a href='invoice.php?id=$result[id]' rel='moodalbox' title='Edit costs for $text_month $year'>edit</a>");
		
		echo("</td></tr>");		
		}
	echo("<tr class='summary'><td colspan='6'>Sum of above orders: <strong>&pound;".number_format($subtotal, 2, '.', ',')."</strong></td>");
	
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
    <form method="post" action="orders.php?check=OK">
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
