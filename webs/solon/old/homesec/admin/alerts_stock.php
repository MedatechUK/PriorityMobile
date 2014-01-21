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
<script type="text/javascript" src="../assets/scripts/functions.js"></script>
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
echo("<h2>Alerts - Stock updates</h2>");

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////	
//		DELETE PROCESS
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////	
if($_GET[delete])
	{
	//DELETE FROM DATABASE
	$delete_db=mysql_query("delete from $stock_table where id='$_GET[delete]'");
	//LOG IN TRACKER
	if(!$delete_db)
		{
		echo("<div id='status_error'><p>The alert entry could not be removed</p></div>");
		}
	else
		{
		echo("<div id='status_ok'><p>The alert entry has been removed</p></div>");
		$process=mysql_query("insert into $admin_track_table (name, loggedin, ip, job) values ('$_COOKIE[user_name]', '$tracker_time', '$ip', 'Stock alert - Deleted')");
		}			
	}
	
	
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////	
//		NOTIFY PROCESS
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////	
if($_GET[notify])
	{
		
		$query = mysql_query("select * from $stock_table where id = '$_GET[notify]'");
		while ($result = mysql_fetch_array($query))
			{
			$product_q = mysql_query("select * from $product_table where id = '$result[product_id]'");
			$product = mysql_fetch_array($product_q);

			if (!$product[seo])
				{
				$product_url = "<a href='".$site_url."product_info.php?product_id=$result[product_id]' target='_blank'>".$site_url."product_info.php?product_id=$result[product_id]</a>";
				}
			else
				{
				$product_url = "<a href='".$site_url."products/$product[seo]' target='_blank'>".$site_url."products/$product[seo]</a>";
				}
			
			$img_query = mysql_query("select * from $gallery_table where doc_cat = '$product[id]'");
			$img_result = mysql_fetch_array($img_query);
			$img_rows  = mysql_num_rows($img_query);
				if ($img_rows > 0)
					{
					$medium_image = "<a href='".$site_url."product_info.php?product_id=$result[product_id]' target='_blank'><img src='".$site_url."assets/images/products/medium/$img_result[name]' alt='An image of $product[name]' style='marign-left: 10px; border: 3px solid #f7941d' align='right'/></a>";
					}
				else	
					{
					$medium_image = "<a href='".$site_url."product_info.php?product_id=$result[product_id]' target='_blank'><img src='".$site_url."assets/images/products/no_medium.gif' alt='No image available' style='marign-left: 10px; border: 3px solid #f7941d' align='right'/></a>";
					}
			
			
			$autoresponse = '
<html>
<style>
<!--
a {color: #f7941d;font-weight: bold;}
a:hover {color: #666;text-decoration:none;}
-->
</style>
<body style="font-family:Verdana;font-size:12px;background:#fff;">
<table width="550" border="0" cellpadding="0" style="margin:5px;">
<tr>
	<td>
	<img src="'.$site_url.'assets/images/email_banner.gif" alt="'.$site_name.'"/>
</td>
</tr>
<tr>
	<td style="padding:10px 0">
	'.$medium_image.'
	<h2 style="color: #000; padding: 0; margin: 0; font-size: 110%;padding-right: 10px">Great news regarding the '.$product[name].'</h2>
<p style="font-size:80%; padding-right: 10px">Hi '.$result[name].'</p>
<p style="font-size:80%; padding-right: 10px">We\'re sending you this email because you wanted us to let you know when the <strong>'.$product[name].'</strong> was back in stock. So, guess what... it\'s back in stock!</p>
<p style="font-size:80%; padding-right: 10px">If you are still interested in the product then the direct link is '.$product_url.'</p>
<p style="font-size:80%; padding-right: 10px">Thanks again for contacting <a href="'.$site_url.'">'.$site_name.'</a>.</p>

		</td>
  </tr>
  <tr>
	<td style="padding:10px 0">
	<p style="font-size:70%; color: #666;">We respect your privacy and please be assured that this email is being sent due to a request via our website. We will not send you any spam as we get just as annoyed as anyone else about junk email.</p>
</td>
</tr>
<tr>
  <td style="padding:10px;margin:0;font-size:70%;color:#999;background:#333;width:100%;" colspan="2">
<p>Copyright 2004-'.date("Y").' Redline Security. All Rights Reserved. '.$site_name.' is a trading name of TIHS Ltd.</p>
<p>Our registered office is: Oak House, Groes Lwyd, Abergele, Conwy LL22 7SU. We are registered in England and Wales. Company No 5325603. VAT Number 855446109.</p>
</td>
</tr>
</table>
</body>
</html>';	
	

$headers  = "MIME-Version: 1.0\r\n";
$headers .= "Content-type: text/html; charset=iso-8859-1\r\n";
$headers .= "From: Home Security <".$site_email.">\r\n";
$headers .= "To:  ".$result[name]." <".$result[email].">\r\n";
$subject = "Stock Update | ".$site_name." Customer Support";
mail($to, $subject, $autoresponse, $headers);	

$date_added = date("YmdHis");
$process = mysql_query("update $stock_table set email_sent='$date_added', status='Notified' where id='$_GET[notify]'");
		
		if(!$process)
			{
			echo("<div id='status_error'><p>There was an error. The user hs not be notified</p></div>");
			}
		else
			{
			echo("<div id='status_ok'><p>The user has been notified</p></div>");
			$process=mysql_query("insert into $admin_track_table (name, loggedin, ip, job) values ('$_COOKIE[user_name]', '$tracker_time', '$ip', 'Stock Alert - Notified User')");
			}
		}
	}
	
	

	echo("<div id='admin'>
	<table width='90%'>
		<thead>
			<tr><th></th><th>Name</th><th>Email</th><th>Product</th><th>Added</th><th>Status</th><th></th></tr>
		</thead>
		<tbody>");

	$i = 1;
	$query = mysql_query("select * from $stock_table order by id asc");
	$numrows = mysql_num_rows($query);
	if ($numrows > 0)
		{
	while ($result = mysql_fetch_array($query))
		{
		
		$last_q = mysql_query("select * from $product_table where id = '$result[product_id]'");
		$last_view = mysql_fetch_array($last_q);
		
		if (!$result[date_added])
			{
			$date = "<span style='color: #CCC'>Unknown</span>";
			$now = "";
			
			}
		else
			{
			//$then = date("d-m-Y H:i", strtotime($result[update_time]));
			
			$then = strtotime($result[date_added]);
			$now = strtotime("now");
			$date = $now - $then;
			
			// MINUTES
			if ($date < 3600)
				{
				$date = floor($date / 60);
				if ($date > 1)
					{
					$date .= " minutes ago";
					}
				elseif ($date == 0)
					{
					$date = " less than a minute ago";
					}	
				elseif ($date == 1)
					{
					$date .= " minunte ago";
					}
				}
				
			// HOURS
			if ($date >= 3600 && $date < 86400)
				{
				$date = floor($date / (60 * 60));
				if ($date > 1)
					{
					$date .= " hours ago";
					}
				else
					{
					$date .= " hour ago";
					}				
				}
				
			// DAYS	
			if ($date >= 86400 && $date < 2629743)
				{
				$date = floor($date / (60 * 60 * 24));
				if ($date > 1)
					{
					$date .= " days ago";
					}
				else
					{
					$date .= " day ago";
					}
				}
			
			//MONTHS
			if ($date >= 2629743 && $date < 31556926)
				{
				$date = floor($date / (60 * 60 * 24 * 30));
				if ($date > 1)
					{
					$date .= " months ago";
					}
				else
					{
					$date .= " month ago";
					}
				}		
			}
		
		if($num == "1")
					{
					$class=" class='row1'";
					$num=2;
					}
				else
					{
					$class=" class='row2'";
					$num=1;
					}
				echo("<tr$class><td>$i.</td><td><small>$result[name]</small></td><td><small>$result[email]</small></td><td><small>$last_view[name]</small></td><td><small>$date</small></td><td><small>");
		
		if ($result[status] == "Open")
			{
			echo("<span style='font-weight:bold;color:#c60;background:url(\"../assets/images/icons/alert.gif\") no-repeat left;padding:2px 0 2px 20px;'>Open</span>");
			}
		else
			{
			echo("<span style='font-weight:bold;color:#090;background:url(\"../assets/images/icons/dispatched.gif\") no-repeat left;padding:2px 0 2px 20px;'>Notified</span>");
			}
		
		echo("</small></td><td><small><a href='alerts_stock.php?delete=$result[id]' style='color:#F00;'onClick='return confirmDelete()'>Delete</a>");
		
		
		if ($last_view[stock] == "YES" && $result[status] == "Open")
			{
			echo(" | <a href='alerts_stock.php?notify=$result[id]'>Notify</a>");
			}
		
		
		
		echo("</small></td></tr>");
	$i++;
		
		
		}
			}
		else
			{
			echo("<tr><td colspan='6'><p style='text-align: center'><strong>No data recorded</strong></p></td></tr>");
			}
		
		
	echo("</tbody></table></div>");
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
