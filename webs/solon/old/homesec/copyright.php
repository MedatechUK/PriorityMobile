<?php
session_start();

//IF THE USER IS RETURNING FROM A COMPLETE ORDER CLEAR SESSION
if ($_GET[clear] == "all")
	{
	session_destroy();
	header ("location: index.php");
	}

require_once('assets/widgets/mysql.class.php');
require_once('assets/widgets/global.inc.php');
require_once('assets/widgets/functions.inc.php');
require_once('assets/widgets/global_variables.php');
require_once('assets/widgets/maintenance.php');
$page_query = mysql_query("select * from $content_table where id='8'");
while($page_result = mysql_fetch_array($page_query))
	{

?>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<?php require("assets/widgets/meta.php"); ?>
</head>
<body>
<?php 
echo("$js_notice");
require("assets/widgets/hidden.php"); 
?>
<div id="container">
  <?php require("assets/widgets/header.php"); ?>
  <?php require("assets/widgets/nav.php"); ?>
  <div id="left_col">
  <?php require("assets/widgets/categories.php"); ?>
  <?php require("assets/widgets/best_sellers.php"); ?>
 </div>
  <div id="main_content">
    <?php 
	echo("$site_notice");
	echo("$page_result[content]"); 
	?>	
  </div>
  <div id="footer">
    <?php 
	// THIS PULLS IN THE FOOTER
	require("assets/widgets/footer.php"); 
	?>
  </div>
</div>
<?php require("assets/widgets/google.php"); ?>
</body>
</html>
<?php } ?>