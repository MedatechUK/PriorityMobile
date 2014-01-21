<?php 
// SECURITY CHECK TO MAKE SURE THE CORRECT USERNAME AND PASSWORD HAS BEEN ENTERED
if(!$user_name | !$user_pwd)
{
header("location: login.php");
exit;
}
?>
<?php 
// Include MySQL class
require_once('../assets/widgets/mysql.class.php');
// Include database connection
require_once('../assets/widgets/global.inc.php');
// Include functions
require_once('../assets/widgets/functions.inc.php');
// THIS IS THE CONTENT WYSIWYG EDITOR - DO NOT REMOVE
include("FCKeditor/fckeditor.php"); 
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
    <h2>Campaign Tracker</h2>
    <div id='status'>
   <?php

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////	
//		DELETE PROCESS
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////	
if($_GET[delete])
	{
			//DELETE FROM DATABASE
		$delete_db=mysql_query("delete from SHOP1_tracker where id='$_GET[delete]'");
		//LOG IN TRACKER
		if(!$delete_db)
			{
			echo("<p style='mmargin: 20px auto; padding: 20px;border: 1px solid #b0d2b7;background: #F30; width: 60%; font-size: 130%;'>The log entry could not be removed</p>");
			}
		else
			{
			echo("<p style='margin: 20px auto; padding: 20px;border: 1px solid #999;background: #acd700; width: 60%; font-size: 130%;'>The log entry has been removed</p>");
			}
			
	}
$query = mysql_query("SELECT * FROM SHOP1_tracker order by id desc");
$numrows = mysql_num_rows($query);
echo("<p>$numrows entries in the system</p>");


//SELECT * FROM SHOP1_tracker order by id desc
?>
<?php
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
$result 		= mysql_query("SELECT count(*) FROM SHOP1_tracker");
$total 			= mysql_result($result, 0);
$pagination 	= mysql_query("SELECT * FROM SHOP1_tracker order by id desc LIMIT $start, $display");


paginate($display, $pg, $total);
?>

<?php
echo("
<table width='100%' border='0' cellspacing='0' cellpadding='0' summary='for design only' class='tab'>
<thead>
	<tr>
	<th width='20'></th>
	<th width='120'><a href='tracker.php?sort=date'>Date</a></th>
	<th><a href='tracker.php?sort=ip'>IP Address</a></th>
	<th><a href='tracker.php?sort=campaign'>Campaign</a></th>
	<th><a href='tracker.php?sort=url'>Refering URL</a></th>
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
		
		if ($_GET['sort'] == "ip")
			{
			$order = " order by ip desc";
			}
		elseif ($_GET['sort'] == "campaign")
			{
			$order = " order by campaign asc";
			}
		elseif ($_GET['sort'] == "url")
			{
			$order = " order by url desc";
			}
		elseif ($_GET['sort'] == "date")
			{
			$order = " order by id desc";
			}
		else
			{
			$order = " order by id desc";
			}
		
		
		$query = mysql_query("SELECT * FROM SHOP1_tracker order by id desc");
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
				<td>");
				if ($result[ip] == "195.112.48.143" || $result[ip] == "213.210.21.186")
					{
					echo ("<em><acronym title='$result[ip]'>Oak House</acronym></em>");
					}
				else
					{
					echo("$result[ip]");
					}
				
				echo("</td>
				<td>$result[campaign]</td>
				<td>");
				
				if($result[url])
				{
				echo("<form><input type='text' value='$result[url]' style='width: 150px;font-size:80%' onFocus='if (this.value==this.defaultValue) this.select()'/>&nbsp;&nbsp;&nbsp;<a href='$result[url]' target='_blank'>Jump</a></form> ");
				}
				else
				{
				echo("<p style='color:#CCC;padding:0;margin:0;'>NO URL</p>");
				}
				
				
				
				echo("</td>
				<td><a href='tracker.php?delete=$result[id]' style='color:#F00;'onClick='return confirmDelete()'>Delete</a></td>
				</tr>");		
				$i++;
			}
			
		echo("</table>");
	

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
