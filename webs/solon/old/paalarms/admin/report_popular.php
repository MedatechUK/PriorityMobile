<?php 
// SECURITY CHECK TO MAKE SURE THE CORRECT USERNAME AND PASSWORD HAS BEEN ENTERED
if(!$user_name | !$user_pwd)
{
header("location: login.php");
exit;
}

// Include MySQL class
require_once('../assets/widgets/mysql.class.php');
// Include database connection
require_once('../assets/widgets/global.inc.php');
// Include functions
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
echo("<h2>Reports - Most Popular Products</h2>");
echo("<div id='admin'>
	<table width='90%'>
		<thead>
			<tr><th>Product</th><th>Code</th><th>Views</th><th>Last viewed</th></tr>
		</thead>
		<tbody>");


	$query = mysql_query("select * from SHOP1_products where sub IS NULL");
	while ($result = mysql_fetch_array($query))
		{
			
		$count = mysql_query("select count(id) from SHOP1_report_popular where product = '$result[id]'");
		$numrows = mysql_fetch_array($count);
		
		$last_q = mysql_query("select * from SHOP1_report_popular where product = '$result[id]' order by id desc");
		$last_view = mysql_fetch_array($last_q);
		
		if (!$last_view[visit])
			{
			$date = "<span style='color: #CCC'>Unknown</span>";
			$now = "";
			
			}
		else
			{
			//$then = date("d-m-Y H:i", strtotime($result[update_time]));
			
			$then = strtotime($last_view[visit]);
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
		
		echo("<tr class='$class'><td>$result[name]</td><td>$result[code]</td><td>$numrows[0]</td><td>$date</td></tr>");
	
		
		
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
