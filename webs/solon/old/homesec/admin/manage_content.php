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
<script language="javascript" src="../assets/scripts/functions.js" type="text/javascript"></script>
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

	
//EDIT PAGE CONTENT
if($_GET[id])
	{
	$query = mysql_query("select * from $content_table where id='$_GET[id]'");
	while($result = mysql_fetch_array($query))
	{
		
?>
<h2>Edit <?php echo("$result[friendly]"); ?> Page</h2>
<form method="post" action="manage_content.php?edit=<?php echo("$result[id]"); ?>" enctype="multipart/form-data">
<input name="id" type="hidden" value="<?php echo("$result[id]"); ?>"/>

<fieldset>
<legend>Visible Content</legend>
<p><label for="friendly">Page Title</label>
<input name="friendly" id="friendly" type="text" value="<?php echo("$result[pagetitle]"); ?>"/> (Appears in title bar)
</p>
<p>Main content</p>
<?php
//THIS IS AN INSTANCE OF THE PRETTY TEXT EDITOR

//THIS IS THE NAME OF THE FIELD
$oFCKeditor = new FCKeditor('content') ;

//IGNORE THIS LINE
$oFCKeditor->BasePath = 'FCKeditor/';

//THIS IS THE VALUE OF THE TEXT AREA (PULLS OUT OF THE DATABASE)
$oFCKeditor->Value = $result[content];

//WIDTH AND HEIGHT OF TEXT AREA
$oFCKeditor->Width  = '670' ;
$oFCKeditor->Height = '350' ;

// IGNORE THESE
$oFCKeditor->ToolbarSet = 'Pete';
$oFCKeditor->Create() ;
?>
</fieldset>
<fieldset>
<legend>Meta Tags</legend>
<p>
<label for="metatags">Keywords</label><br />
<textarea name="metatags" rows="5" cols="50"><?php echo("$result[metatags]"); ?></textarea></p>
<p>
<label for="metadesc">Description</label><br />
<textarea name="metadesc" rows="5" cols="50"><?php echo("$result[metadesc]"); ?></textarea></p>
</fieldset>
<fieldset>
<legend>Rollback</legend>
<div id='admin'>
	<table width='90%'>
		<thead>
			<tr><th>Updated</th><th>Word count</th><th>Options</th></tr>
		</thead>
		<tbody>
<?php
$r_query = mysql_query("select * from $archive_table where friendly='$_GET[id]' order by id desc");
	while($r_result = mysql_fetch_array($r_query))
	{
	if (!$r_result[update_time])
			{
			$date = "<span style='color: #CCC'>Unknown</span>";			
			}
		else
			{	
			$date = date("d/m/Y H:i:s", strtotime($r_result[update_time]));			
			}
		$count = strlen($r_result[content]);
	echo("<tr><td>$date</td><td>$count</td><td><a href='page_preview.php?id=$r_result[id]' target='_blank'>View</a> | <a href='manage_content.php?rollback=$r_result[id]' class='delete' onClick='return confirmRollback()'>Rollback</a></td></tr>");
	}

?>
</tbody>
</table>
</fieldset>
<p></p>
<div id="right"><input type="submit" name="Submit" value="Update" /></div>

</form>
<?php
}
}
else
	{
	echo("<h2>Page Management</h2>");
	
	
//EDIT PROCESS
if($_GET[edit])
{
	$title =  htmlentities($_POST[friendly], ENT_QUOTES);
	$update = date('YmdHis');
	$process=mysql_query("update $content_table set content='$_POST[content]', pagetitle='$title', metatags='$_POST[metatags]', metadesc = '$_POST[metadesc]', update_time='$update' where id='$_POST[id]'"); 
	
	$process_archive=mysql_query("insert into $archive_table (friendly, content, pagetitle, metatags, metadesc, update_time) VALUES ('$_GET[edit]', '$_POST[content]', '$title', '$_POST[metatags]', '$_POST[metadesc]', '$update')");

	if (!$process || !$process_archive)
	{
	echo("<div id='status_error'><p><strong>Oops!</strong></p><p>There was an error when you tried to update the page.</p></div>");
	}
	else
	{
	echo("<div id='status_ok'><p>Thank you the page has now been updated.</p></div>");
	$tracker=mysql_query("insert into $admin_track_table (name, loggedin, ip, job) values ('$_COOKIE[user_name]', '$tracker_time', '$ip', 'Content - edited ($title)')");
	}
}

//ROLLBACK PROCESS
if($_GET[rollback])
{

// GET INFO OUT OF ARCHIVE TABLE

$query= mysql_query("select * from $archive_table where id = '$_GET[rollback]'");
$result = mysql_fetch_array($query);

// UPDATE CURRENT CONTENT WITH ARCHIVE INFO

$update = date('YmdHis');
$process=mysql_query("update $content_table set content='$result[content]', pagetitle='$result[pagetitle]', metatags='$result[metatags]', metadesc = '$result[metadesc]', update_time='$update' where id='$result[friendly]'"); 

$process_archive=mysql_query("insert into $archive_table (friendly, content, pagetitle, metatags, metadesc, update_time) VALUES ('$result[friendly]', '$result[content]', '$result[pagetitle]', '$result[metatags]', '$result[metadesc]', '$update')");

	if (!$process || !$process_archive)
	{
	echo("<div id='status_error'><p><strong>Oops!</strong></p><p>There was an error when you tried to roll back the page.</p></div>");
	}
	else
	{
	echo("<div id='status_ok'><p>Thank you the page has now been rolled back.</p></div>");
	$tracker=mysql_query("insert into $admin_track_table (name, loggedin, ip, job) values ('$_COOKIE[user_name]', '$tracker_time', '$ip', 'Content - rolled back')");
	}
}
	
	echo("<div id='admin'>
	<table width='90%'>
		<thead>
			<tr><th>Page name</th><th>Updated</th><th>Word count</th><th>Options</th></tr>
		</thead>
		<tbody>
	
	");
	$query = mysql_query("select * from $content_table order by friendly asc");
	while($result = mysql_fetch_array($query))
		{
		$count = strlen($result[content]);
		if($class =="row2")
			{
				$class = 'row1';
			}
			else
			{
				$class = 'row2';
			}
		
		if (!$result[update_time])
			{
			$date = "<span style='color: #CCC'>Unknown</span>";
			$now = "";
			
			}
		else
			{
			//$then = date("d-m-Y H:i", strtotime($result[update_time]));
			
			$then = strtotime($result[update_time]);
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
			$class=" class='row2'";
			$num=2;
			}
		else
			{
			$class=" class='row1'";
			$num=1;
			}
		
		
		echo("<tr$class><td>$result[friendly]</td><td><small>$date</small></td><td>$count</td><td><a href='manage_content.php?id=$result[id]' class='edit_page'>Edit</a></td></tr>");		
		}
	echo("</tbody></table></div>");
	}
?></div>
  <div id="footer">
  <?php 
	// THIS PULLS IN THE FOOTER
	require("../assets/widgets/footer.php"); 
	?>
  </div>
</div>
</body>
</html>
