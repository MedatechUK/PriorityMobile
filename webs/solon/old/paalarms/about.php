<?php
session_start();

//IF THE USER IS RETURNING FROM A COMPLETE ORDER CLEAR SESSION
if ($_GET[clear] == "all")
	{
	session_destroy();
	}

// Include MySQL class
require_once('assets/widgets/mysql.class.php');
// Include database connection
require_once('assets/widgets/global.inc.php');
// Include functions
require_once('assets/widgets/functions.inc.php');
require_once('assets/widgets/global_variables.php');

$page_query = mysql_query("select * from SHOP1_content where id='4'");
while($page_result = mysql_fetch_array($page_query))
	{

?>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
<title><?php echo $site_name; ?> | <?php echo $page_result[pagetitle] ?></title>
<meta name="keywords" content="<?php echo $page_result[metatags] ?>" />
<meta name="description" content="<?php echo $page_result[metadesc] ?>" />
<link rel="stylesheet" href="assets/css/screen.css" type="text/css" media="screen" />
<link rel="stylesheet" href="assets/css/print.css" type="text/css" media="print" />
<link rel="shortcut icon" href="/favicon.ico" />
</head>
<body>
<?php 
echo("$js_notice");
?>
<div id="hidden">
  <?php require("assets/widgets/hidden.php"); ?>
</div>
<div id="container">
  <div id="header">
    <h2><?php echo $site_name; ?></h2>
	<a href="/index.php" class="homelink" title="Home Page"><img src="assets/images/clear.gif" alt="Link to Home Page" width="300px" height="50px"/></a>
<?php echo writeShoppingCart(); ?>
  </div>
  <div id="navigation">
    <?php require("assets/widgets/nav.php"); ?>
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