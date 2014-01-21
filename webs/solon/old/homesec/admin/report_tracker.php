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
require_once('../assets/widgets/admin_functions.inc.php');
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
  <h2>Admin Logfile (Last 200 actions)</h2>
  <p><small>Please note that all records are stored in the database</small>
    <?php 
  
$i = 1;

echo("
<div id='reports'>
<table width='90%'>
<thead>
	<tr>
		<th width='20'></th>
		<th width='120'>User</th>
		<th>Ip Address</th>
		<th>Action</th>
		<th>Date</th>
	</tr>
</thead><tbody>");
		//last user logged in
		$late1 = mysql_query("SELECT * FROM $admin_track_table$val order by id desc limit 200");
		while ($latest = mysql_fetch_array($late1))
			{
			
			if (!$latest[loggedin])
			{
			$date = "<span style='color: #CCC'>Unknown</span>";
			$now = "";
			
			}
		else
			{
			$time = date("d-m-Y H:i", strtotime($latest[loggedin]));
			
			$then = strtotime($latest[loggedin]);
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
			
			
			
			
			
			
			
			$job = substr("$latest[job]", 0, 6);	
			if($job == 'Failed')
				{
				$span  	= " class='peep'";
				$bad	= " class='badlogin'";
				}
			elseif ($job == 'Logged')
				{
				$span  	= " class='peep'";
				$bad	= "";
				}
			elseif ($job == 'Produc')
				{
				$span  	= " class='edproduct'";
				$bad	= "";
				}
			elseif ($job == 'Conten')
				{
				$span  	= " class='edpage'";
				$bad	= "";
				}
			elseif ($job == 'Catego')
				{
				$span  	= " class='edit_category'";
				$bad	= "";
				}
			elseif ($job == 'Order ')
				{
				$span  	= " class='orders'";
				$bad	= "";
				}
			elseif ($job == 'Report')
				{
				$span  	= " class='report_search'";
				$bad	= "";
				}
			elseif ($job == 'User -')
				{
				$span  	= " class='peep'";
				$bad	= "";
				}
			elseif ($job == 'Databa')
				{
				$span  	= " class='backup'";
				$bad	= "";
				}
			elseif ($job == 'Graph ')
				{
				$span  	= " class='report_search'";
				$bad	= "";
				}
			elseif ($job == 'SEO Pa')
				{
				$span  	= " class='php_icon'";
				$bad	= "";
				}
			elseif ($job == 'Stock ')
				{
				$span  	= " class='stock_alerts'";
				$bad	= "";
				}
			elseif ($job == 'Image ')
				{
				$span  	= " class='edit_images'";
				$bad	= "";
				}
			
			
			
				
				
			else
				{
				$span  	= "";
				$bad	= "";
				}
				
				
				
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
				echo("<tr$class><td align='right'>$i.</td><td>$latest[name]</td><td>".oakhouse($latest[ip])."</td><td$bad><span$span>$latest[job]</span></td><td><acronym title='$time'>$date</acronym></td></tr>");		
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
