<?php 
// SECURITY CHECK TO MAKE SURE THE CORRECT USERNAME AND PASSWORD HAS BEEN ENTERED
if(!$_COOKIE[user_name] | !$_COOKIE[user_pwd])
{
header("location: login.php");
exit;
}
?>
<?php 
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
    <h2>Footprints</h2>
    <div id='status'>
   <?php
$basket_query = mysql_query("select * from $footprint_table where this_url like '%basket.php%'");
$basket_rows = mysql_num_rows($basket_query);

echo("<p>$basket_rows visits to the basket</p>");

$basket_query = mysql_query("select * from $footprint_table where this_url like '%checkout.php%'");
$basket_rows = mysql_num_rows($basket_query);

echo("<p>$basket_rows visits to the checkout</p>");

$basket_query = mysql_query("select * from $footprint_table where this_url like '%confirm.php%'");
$basket_rows = mysql_num_rows($basket_query);

echo("<p>$basket_rows visits to the confirmation page</p>");

if (!$_GET['view'])
{

function paginate($display, $pg, $total) {
/* make sure pagination doesn't interfere with other query string variables */
  if(isset($_SERVER['QUERY_STRING']) && trim($_SERVER['QUERY_STRING']) != '') {
    if(stristr($_SERVER['QUERY_STRING'], 'pg='))
      $query_str = '?'.preg_replace('/pg=\d+/', 'pg=', $_SERVER['QUERY_STRING']);
    else
      $query_str = '?'.$_SERVER['QUERY_STRING'].'&amp;pg=';
  } else
    $query_str = '?pg=';
    
  /* find out how many pages we have */
  $pages = ($total <= $display) ? 1 : ceil($total / $display);
    
  /* create the links */
  $first 	= '<a href="'.$_SERVER['PHP_SELF'].$query_str.'1">First</a>';
  $prev 	= '<a href="'.$_SERVER['PHP_SELF'].$query_str.($pg - 1).'">&#171; Previous</a>';
  $next 	= '<a href="'.$_SERVER['PHP_SELF'].$query_str.($pg + 1).'">Next &#187;</a>';
  $last 	= '<a href="'.$_SERVER['PHP_SELF'].$query_str.$pages.'">Last</a>';
   
  /* display opening navigation */
  echo '<div class="pagination"><p><small>';
  //echo ($pg > 1) ? "$first : $prev |" : '<span>First : &#171; Previous</span> |';
  echo ($pg > 1) ? "$prev |" : '<span>&#171; Previous</span> |';
  
  /* limit the number of page links displayed */
  $begin = $pg - 6;
  while($begin < 1)
    $begin++;
  $end = $pg + 6;
  while($end > $pages)
    $end--;
  for($i=$begin; $i<=$end; $i++)
    echo ($i == $pg) ? ' <strong>'.$i.'</strong> ' : ' <a href="'.$_SERVER['PHP_SELF'].$query_str.$i.'">'.$i.'</a> ';
    
  /* display ending navigation */
  //echo ($pg < $pages) ? "| $next : $last" : '| <span>Next &#187; : Last</span>';
  echo ($pg < $pages) ? "| $next" : '| <span>Next &#187;</span>';
  echo '</small></p></div>';
}

/* set pagination variables */
$display = 20;
$pg = (isset($_REQUEST['pg']) && ctype_digit($_REQUEST['pg'])) ?
  $_REQUEST['pg'] : 1;
$start = $display * $pg - $display;

/* paginating from a database */
$result 		= mysql_query("SELECT * FROM $footprint_table where session_id IS NOT NULL group by session_id");
$total 			= mysql_num_rows($result);
$pagination 	= mysql_query("SELECT * FROM $footprint_table where session_id IS NOT NULL group by session_id order by id desc LIMIT $start, $display");


paginate($display, $pg, $total);
?>
<?php






$query = mysql_query("SELECT * FROM $footprint_table where session_id IS NOT NULL group by session_id");
$numrows = mysql_num_rows($query);
echo("<p>$numrows entries in the system</p>");

echo("
<table width='100%' border='0' cellspacing='0' cellpadding='0' summary='for design only' class='tab'>
<thead>
	<tr>
	<th width='20'></th>
	<th width='120'>Date</th>
	<th>IP Address</th>
	<th>Page impressions</th>
	<th>Time on site</th>
	<th>Options</th>
	</tr>
</thead>");
		
		
		if ($pg == 1)
			{
			$i = 1;
			}
		else
			{
			$i = $display * ($pg-1) + 1;
			}
		
		
		//$query = mysql_query("SELECT * FROM SHOP1_footprints group by session_id order by id desc LIMIT $page, $limit");
		while ($result = mysql_fetch_array($pagination))
			{
				if($num == "1")
					{
					$class=" class='alt'";
					$num=2;
					}
				else
					{
					$class='';
					$num=1;
					}
				$date = explode(" ", $result[date]);
				
				echo("
				<tr$class>
				<td align='right' width='20'>$i.&nbsp;</td>
				<td width='150'><strong>$date[0]</strong> <em>$date[1]</em></td>
				<td>".oakhouse($result[ip])."</td><td>");
				
				//COUNT PAGE IMPRESSIONS
				$count = mysql_query("select * from $footprint_table where session_id = '$result[session_id]'");
				$pages = mysql_num_rows($count);
				
				echo("$pages</td><td>");
				
				$minidq = mysql_query("select min(id) from $footprint_table where session_id = '$result[session_id]'");
				$maxidq = mysql_query("select max(id) from $footprint_table where session_id = '$result[session_id]'");
				$minid = mysql_fetch_array($minidq);
				$maxid = mysql_fetch_array($maxidq);
				
				//Earliest Time on site
				$min_time_q = mysql_query("select * from $footprint_table where session_id = '$result[session_id]' and id = '$minid[0]'");
				$min_time = mysql_fetch_array($min_time_q);
				
				//Last recorded time on site
				$max_time_q = mysql_query("select * from $footprint_table where session_id = '$result[session_id]' and id ='$maxid[0]'");
				$max_time = mysql_fetch_array($max_time_q);
				
				$start_time = strtotime($min_time["date"]);
				$finish_time = strtotime($max_time["date"]);
				$date = $finish_time - $start_time;
				
				
				// MINUTES
			if ($date < 3600)
				{
				$date = floor($date / 60);
				if ($date > 1)
					{
					$date .= " min";
					}
				elseif ($date == 0)
					{
					$date = "<span style='color: #CCC;'>Session not logged</span>";
					}	
				elseif ($date == 1)
					{
					$date .= " min";
					}
				}
			// HOURS
			if ($date >= 3600 && $date < 18000)
				{
				$date = floor($date / (60 * 60));
				if ($date > 1)
					{
					$date .= " hours";
					}
				else
					{
					$date .= " hour";
					}				
				}
				
			// DAYS	
			if ($date >= 18000)
				{
				$date = "<span style='color: #F00;'>Session not closed</span>";
				}
			
			
			
				
				echo("$date");
				
				
				echo("</td>
				<td><a href='footprint.php?view=$result[session_id]'>View</a></td>
				</tr>");		
				$i++;
			}
			
		echo("</table>");
}
else
{
// SESSION LIST

$query = mysql_query("SELECT * FROM $footprint_table where session_id ='".$_GET['view']."' order by id asc limit 1");
$result = mysql_fetch_array($query);


//$timestamp = strtotime("$result[date]");
//print date( 'His', $timestamp );




echo("<p><small>IP Address: <strong>$result[ip]</strong></small></p>");
echo("<p><small>Session ID: <strong>$result[session_id]</strong></small></p>");
$date = explode(" ", $result[date]);
echo("<p><small>Entry time: <strong>$date[0] $date[1]</strong></small></p>");

echo("
<table width='100%' border='0' cellspacing='0' cellpadding='0' summary='for design only' class='tab'>
<thead>
	<tr>
	<th width='20'></th>
	<th width='120'>Date</a></th>
	<th>Previous URL</th>
	<th>This URL</th>
	<th>Duration</th>
	</tr>
</thead>");
		
		$i = 1;		
		
		$query = mysql_query("SELECT * FROM $footprint_table where session_id ='".$_GET['view']."' order by id asc");
		while ($result = mysql_fetch_array($query))
			{
				if($num == "1")
					{
					$class=" class='alt'";
					$num=2;
					}
				else
					{
					$class='';
					$num=1;
					}
				$date = explode(" ", $result[date]);
					
				echo("
				<tr$class>
				<td align='right' width='20'>$i.&nbsp;</td>
				<td width='150'><strong>$date[0]</strong> <em>$date[1]</em></td>
				<td>");
				
				if($result[url])
				{
				echo("<form><input type='text' value='$result[url]' style='width: 150px;font-size:80%' onFocus='if (this.value==this.defaultValue) this.select()'/>&nbsp;&nbsp;&nbsp;<a href='$result[url]' target='_blank'>Jump</a></form>");
				}
				else
				{
				echo("<p style='color:#CCC;padding:0;margin:0;'>NO URL</p>");
				}
				
				
				
				
				echo("</td><td>");
				
				if($result[this_url])
				{
				echo("<form><input type='text' value='$result[this_url]' style='width: 150px;font-size:80%' onFocus='if (this.value==this.defaultValue) this.select()'/>&nbsp;&nbsp;&nbsp;<a href='$result[this_url]' target='_blank'>Jump</a></form>");
				}
				else
				{
				echo("<p style='color:#CCC;padding:0;margin:0;'>NO URL</p>");
				}
				
				echo("</td>");
				
				echo("<td></td>");
				echo("</tr>");		
				$i++;
			}
			
		echo("</table>");
}	

?></div>
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
