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
<script type="text/javascript" src="../assets/scripts/functions.js"></script>
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
  <h2>SEO page manager</h2>
<?php
$this_page = "seo_manager";
$this_url = "seo_manager.php";


//==========================================================================
//DELTE PROCESS
//==========================================================================	
if ($_GET[delete])
	{
	$page_name_formatted 	= 	"../products/".$_GET[delete].".php";
	
	if ($page_name_formatted == $this_page)
		{
		echo("<div id='status_error'><p>Page could not be deleted as it is the same name as the builder</p></div>");
		}
	else
		{
		
		$query = mysql_query("select * from $product_table where seo='$_GET[delete].php'");
		$result = mysql_fetch_array($query);
		
		$process = mysql_query("update $product_table set seo='' where id='$result[id]'");
		
		
		unlink("$page_name_formatted");
		echo("<div id='status_ok'><p>The page $page_name_formatted has been deleted</p></div>");
		$tracker=mysql_query("insert into $admin_track_table (name, loggedin, ip, job) values ('$_COOKIE[user_name]', '$tracker_time', '$ip', 'SEO Page - Deleted')");
		}
	}
?>

<?php

$dir = opendir('../products/');
echo("<div id='admin'>
	<table width='90%'>
		<thead>
			<tr><th>Product</th><th>Page name</th><th></th></tr>
		</thead>
		<tbody>");
		
	$product_q = mysql_query("select * from $product_table where seo !=''");
	$numrows = mysql_num_rows($product_q);
	if ($numrows > 0)
		{
	while ($product = mysql_fetch_array($product_q))
		{
		$page_name	= 	str_replace(".php", "", $product[seo]);
		echo '<tr><td>'.$product[name].'</td><td><a href="../products/'.$product[seo].'">'.$product[seo].'</a></td><td><a href="seo_manager.php?delete='.$page_name.'">Delete</a></td></tr>';
		}
			}
		else
			{
			echo("<tr><td colspan='3'><p style='text-align: center'><strong>There are no SEO pages in the system</strong></p></td></tr>");
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
