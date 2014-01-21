<?php 
// SECURITY CHECK TO MAKE SURE THE CORRECT USERNAME AND PASSWORD HAS BEEN ENTERED
if(!$user_name | !$user_pwd)
{
header("location: login.php");
exit;
}

// Include MySQL class
require_once('../assets/widgets/mysql.class.php');
require_once('../assets/widgets/global.inc.php');
require_once('../assets/widgets/functions.inc.php');


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
      <?php
echo("<h2>Alerts - Stock updates</h2>");

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////	
//		DELETE PROCESS
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////	
if($_GET[delete])
	{
			//DELETE FROM DATABASE
		$delete_db=mysql_query("delete from SHOP1_stock where id='$_GET[delete]'");
		//LOG IN TRACKER
		if(!$delete_db)
			{
			echo("<p style='mmargin: 20px auto; padding: 20px;border: 1px solid #b0d2b7;background: #F30; width: 60%; font-size: 130%;'>The alert entry could not be removed</p>");
			}
		else
			{
			echo("<p style='margin: 20px auto; padding: 20px;border: 1px solid #999;background: #acd700; width: 60%; font-size: 130%;'>The alert entry has been removed</p>");
			}
			
	}
	echo("<div id='admin'>
	<table width='90%'>
		<thead>
			<tr><th></th><th>Name</th><th>Email</th><th>Product</th><th>Added</th><th></th></tr>
		</thead>
		<tbody>");

	$i = 1;
	$query = mysql_query("select * from SHOP1_stock order by id asc");
	while ($result = mysql_fetch_array($query))
		{
		
		$last_q = mysql_query("select * from SHOP1_products where id = '$result[product_id]'");
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
		
		echo("<tr class='$class'><td>$i.</td><td><small>$result[name]</small></td><td><small>$result[email]</small></td><td><small>$last_view[name]</small></td><td><small>$date</small></td><td><small><a href='alerts_stock.php?delete=$result[id]' style='color:#F00;'onClick='return confirmDelete()'>Delete</a></small></td></tr>");
	$i++;
		
		
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
