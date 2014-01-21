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
      <?php
	  
	 // $tracker=mysql_query("insert into $admin_track_table (name, loggedin, ip, job) values ('$_COOKIE[user_name]', '$tracker_time', '$ip', 'Report - Viewed Search Results')");	
	  
echo("<h2>Reports - Search Terms</h2>");
echo("<p><a href='export_search.php'>Export search terms</a></p>");
echo("<div id='orders'>
	<table width='90%'>
		<thead>
			<tr><th></th><th>Search Term</th><th>Results</th><th>Ip address</th><th>Date</th></tr>
		</thead>
		<tbody>");
	
	$i = 1;
	$query = mysql_query("select * from $search_table order by id desc");
	$numrows = mysql_num_rows($query);
	if ($numrows > 0)
		{
	while ($result = mysql_fetch_array($query))
		{
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
		$date = date("d/m/Y H:i:s", strtotime($result[date]));
		echo("<tr$class><td>$i.</td><td>$result[search_term]</td><td>$result[products]</td><td>".oakhouse($result[ip])."</td><td>$date</td></tr>");
		$i++;
		}
	}
		else
			{
			echo("<tr><td colspan='4'><p style='text-align: center'><strong>No data recorded</strong></p></td></tr>");
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
