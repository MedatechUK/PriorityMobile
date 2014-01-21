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
<title>Admin Interface</title>
<link rel="shortcut icon" href="../assets/images/favicon.ico" />
<link rel="stylesheet" href="../assets/css/admin.css" type="text/css" media="screen" />
<script language="javascript" src="../assets/scripts/functions.js" type="text/javascript"></script>
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
	 $i = 0;
	 $a = 0;
	 
	 
	 
	 
	 //FRIENDLY DISPLAY OF EMAILS
	 echo("<div id='orders'>
	<table>
		<thead>
			<tr><th width='110'>Order</th><th width='250'>Name</th><th width='100'>Email</th><th width='110'>Date of Order</th><th>Status</th></tr>
		</thead>
		<tbody>");
	 
	  $query = mysql_query("select * from SHOP1_orders order by date desc");
		while ($result = mysql_fetch_array($query))
			{
			$statusquery = mysql_query("select * from SHOP1_order_status where order_id = '$result[id]' order by id desc limit 1 ");
			$numrows = mysql_num_rows($statusquery);
	
			if ($numrows > 0)
				{
				while($statusresult = mysql_fetch_array($statusquery))
					{
					if ($statusresult[action] == "Pending")
						{
						$status = ("<span style='font-weight:bold;color:#F00;background:url(\"../assets/images/icons/alert.gif\") no-repeat left;padding:2px 0 2px 20px;'>Pending</span>");
						}
					elseif ($statusresult[action] == "Refunded")
						{
						$status = ("<span style='font-weight:bold;color:#F00;background:url(\"../assets/images/icons/bad.gif\") no-repeat left;padding:2px 0 2px 20px;'>Refunded</span>");
						}
					elseif ($statusresult[action] == "Returned")
						{
						$status = ("<span style='font-weight:bold;color:#F00;background:url(\"../assets/images/icons/alert.gif\") no-repeat left;padding:2px 0 2px 20px;'>Returned</span>");
						}
					elseif ($statusresult[action] == "Cancelled")
						{
						$status = ("<span style='font-weight:bold;color:#F00;background:url(\"../assets/images/icons/bad.gif\") no-repeat left;padding:2px 0 2px 20px;'>Cancelled</span>");
						}
					elseif ($statusresult[action] == "Awaiting Stock")
						{
						$status = ("<span style='font-weight:bold;color:#F90;background:url(\"../assets/images/icons/out_of_stock.gif\") no-repeat left;padding:2px 0 2px 20px;'>Awaiting Stock</span>");
						}
					else
						{
						$status = ("<span style='font-weight:bold;color:#090;background:url(\"../assets/images/icons/dispatched.gif\") no-repeat left;padding:2px 0 2px 20px;'>$statusresult[action]</span>");
						}
					}}
					else
		{
		$status = ("<span style='font-weight:bold;color:#F00;background:url(\"../assets/images/icons/alert.gif\") no-repeat left;padding:2px 0 2px 20px;'>Pending</span>");
		}
						
						
						
							
						$name 	= explode("<br/>", $result[billing_address]);
						$order 	= explode("/", $result[order_number]);
						$date 	= date("d/m/Y", strtotime($result[date]));
			
						if(!$result[comms])
							{
							$comms = "<span style='font-weight:bold;color:#090;background:url(\"../assets/images/icons/dispatched.gif\") no-repeat left;padding:2px 0 2px 20px;'>Subscribed</span>";
							}
						else
							{
							$comms = "<span style='font-weight:bold;color:#F00;background:url(\"../assets/images/icons/bad.gif\") no-repeat left;padding:2px 0 2px 20px;'>Unsubscribed</span>";
							}
						
						echo("<tr><td>$status</td><td>$name[0]</td><td>$result[email]</td><td>$date</td><td>$comms</td></tr>");
						
					
				
			}
	 
	 echo("</tbody></table></div>");
	 
	 
	 
	 
	 
	 
	 
	 
	 
	 
	 echo("<p><textarea rows='10' cols='110'>");
	 $query = mysql_query("select email from SHOP1_orders where comms IS NULL order by email asc");
		while ($result = mysql_fetch_array($query))
			{
			echo("$result[email]; ");
			$i++;
			}
	 echo("</textarea></p>");
	 
	 //PEOPLE WHO DONT WANT TO BE CONTACTED
	 $query = mysql_query("select email from SHOP1_orders where comms IS NOT NULL order by email asc");
		while ($result = mysql_fetch_array($query))
			{
			$a++;
			}
	 echo("<p><small>$i email allowing communication</small></p>");
	 echo("<p><small>$a emails dont want to be contacted</small></p>");
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
