<?php 
// SECURITY CHECK TO MAKE SURE THE CORRECT USERNAME AND PASSWORD HAS BEEN ENTERED
if(!$user_name | !$user_pwd)
{
header("location: login.php");
exit;
}
?>
<?php 
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
    <h2>Edit Site Scripts</h2>
      <?php

	
//EDIT PAGE CONTENT
//EDIT PROCESS
if($_GET[edit])
{
	$title =  htmlentities($_POST[friendly], ENT_QUOTES);
	$update = date('YmdHis');

	$process=mysql_query("update SHOP1_config set content='$_POST[scripts]' where id='2'"); 

	if (!$process)
	{
	echo("<div id='status_error'><p><strong>Oops!</strong></p><p>There was an error when you tried to update the scripts.</p></div>");
	}
	else
	{
	echo("<div id='status_ok'><p>Thank you the scripts has now been updated.</p></div>");
	$process=mysql_query("insert into SHOP1_admin_tracker (name, loggedin, ip, job) values ('$_COOKIE[user_name]', '$tracker_time', '$ip', 'Site Script Updated')");
	}
}

$query = mysql_query("select * from SHOP1_config where id='2'");
	while($result = mysql_fetch_array($query))
	{
		
?>

<form method="post" action="manage_scripts.php?edit=yes" enctype="multipart/form-data">

<fieldset>
<legend>Please be careful when doing this</legend>
<p>Use this feature to insert Google Analytics / Google Adwords, or any other external script that is required. Please note that the scripts will be placed just before:</p>
 <p><code>&lt;/body&gt;<br/>&lt;/html&gt;</code></p>
<p><label>Insert Scripts here<br/>
<textarea name="scripts" rows="15" cols="70"><?php echo("$result[content]"); ?></textarea></label></p>
</fieldset>

<p></p>
<div id="right"><input type="submit" name="Submit" value="Update" /></div>

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
