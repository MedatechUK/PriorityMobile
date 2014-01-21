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
require_once('../assets/widgets/admin_functions.inc.php');

function formatfilesize( $data ) {
        // bytes
        if( $data < 1024 ) {
            return $data . " bytes";
         }
        // kilobytes
        elseif( $data < 1024000 ) {
            return round( ( $data / 1024 ), 1 ) . "k";
		}
        // megabytes
        else {
        return round( ( $data / 1024000 ), 1 ) . " MB";
		}   
    }

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
    <h2>Admin Home page</h2>
	<? //echo phpinfo();?>
    <div id="infobox">
      <p>Site Info</p>
      <dl>
        <dt>Orders:</dt>
        <?php
	//ACTUAL ORDERS
	$count = mysql_query("select count(id) from SHOP1_orders");
	$rows = mysql_fetch_array($count);
	echo("<dd>$rows[0]</dd>");
	?>
       
        
	
        <dt>Products:</dt>
        <?php
	//PRODUCT COUNT
	$count = mysql_query("select count(id) from SHOP1_products where sub IS NULL");
	$rows = mysql_fetch_array($count);
	echo("<dd>$rows[0]</dd>");
	?>
	
	
	 </dl>
	   <p>Geek Stuff</p>
	  <dl>
	 <dt>Site Status:</dt>
        <?php
	//SITE STATUS
	$query = mysql_query("select * from SHOP1_config where id ='1'");
	$result = mysql_fetch_array($query);
	echo("<dd>");
	if ($result[status] == "Y")
		{
		echo("<span style='color:#F00; font-weight: bold;'>Offline</span>");
		}
	else
		{
		echo("<span style='color:#093; font-weight: bold;'>Online</span>");
		}
	
	echo("</dd>");
	?>
	  <dt>PHP Version:</dt><dd><?php echo phpversion() ?></dd>
	  <dt>MySQL Version:</dt><dd>
	  <?php 
	  
	  $query = mysql_query("SELECT version()"); 
	  $result = mysql_fetch_array($query);
	  $result = str_replace("-standard-log", "",$result[0]);
	  echo ($result); ?></dd>
	  <dt>Database Size:</dt>
	<?php
	
	$result = mysql_query( "SHOW TABLE STATUS" );
	$dbsize = 0;
	while( $row = mysql_fetch_array( $result ) ) 
		{  
		$dbsize += $row[ "Data_length" ] + $row[ "Index_length" ];
		}
		 echo "<dd>". formatfilesize( $dbsize ) . "</dd>";
	
	?>
	<dt>Your Browser:</dt>
	<dd><?php
	if (stristr($_SERVER['HTTP_USER_AGENT'],"firefox")) {
$agent = "Firefox";
}
elseif (stristr($_SERVER['HTTP_USER_AGENT'],"safari")) {
$agent = "Safari";
}
elseif (stristr($_SERVER['HTTP_USER_AGENT'],"msie 7")) {
$agent = "IE 7";
}
elseif (stristr($_SERVER['HTTP_USER_AGENT'],"opera")) {
$agent = "Opera";
}
elseif (stristr($_SERVER['HTTP_USER_AGENT'],"msie 6")) {
$agent = "IE 6";
}
else {
$agent = "UNKNOWN";
}
echo($agent); ?></dd>
	  </dl>
    </div>
    <p>Welcome to the admin interface for this website. From here you will be able to edit core details of each page and also the product details</p>
	
	<div id="admin_blocks">
	
      <?php
	if ($_COOKIE[user_id] == "master" || stristr($_COOKIE[user_id], "orders-") == TRUE)
	{	
	?>
	<div class="home_block">
	<h3>Orders</h3>
	<ul class="list">
      <li><a href="orders.php" title="" class="orders">Order Database</a></li>
	  <li><a href="order_start.php" title="" class="orders">Order Manual</a></li>
	  <li><a href="alerts_stock.php" title="" class="edproduct">Stock Alerts</a></li>
    </ul>
	</div>
	<?php } ?>
	<?php
	if ($_COOKIE[user_id] == "master" || stristr($_COOKIE[user_id], "products-") == TRUE)
	{	
	?>
	<div class="home_block">
	<h3>Catalog</h3>
    <ul class="list">
      <li><a href="manage_content.php" title="" class="edpage">Edit Pages</a></li>
      <li><a href="manage_products.php" title="" class="edproduct">Edit Products</a></li>
    </ul>
	</div>
	<?php } ?>
	<?php
	if ($_COOKIE[user_id] == "master" || stristr($_COOKIE[user_id], "reports-") == TRUE)
	{	
	?>
	<div class="home_block">
    <h3>Reports</h3>
    <ul class="list">
	<li><a href="report_popular.php" title="" class="report_popular">Popular Products</a></li>
      <li><a href="report_products.php" title="" class="report_purchased">Purchased Products</a></li>
	  <li><a href="report_monthly.php" title="" class="report_search">Monthly reports</a></li>
    </ul>
	</div>
	<?php } ?>
	<?php
	if ($_COOKIE[user_id] == "master" || stristr($_COOKIE[user_id], "marketing-") == TRUE)
	{	
	?>
	<div class="home_block">
    <h3>Marketing</h3>
    <ul class="list">
      <li><a href="https://www.google.com/analytics/home/?hl=en" title="Opens in a new window" target="_blank" class="anal">Google Analytics</a></li>
      <li><a href="https://adwords.google.com/select/Login?sourceid=awo&subid=ww-en-et-gaia" title="Opens in a new window" target="_blank" class="adword">Google Adwords</a></li>
      <li><a href="http://stats.personal-attack-alarms.net" title="Opens in a new window" target="_blank" class="anal">Site Statistics</a></li>
	  <li><a href="tracker.php" class="report_tracker">Campaign Tracker</a></li>
	  <li><a href="footprint.php" class="footprints">Site Footprints</a></li>
	  <li><a href="manage_scripts.php" class="external_scripts">External Scripts</a></li>
    </ul>
	</div>
	 <?php } ?>
	 
      <?php
	if ($_COOKIE[user_id] == "master")
	{	
	?>
	<div class="home_block">
	<h3>Admin Features</h3>
    <ul class="list">
	<li><a href="export_products_base.php" title="" class="export">Exports Base</a></li>
	
	<li><a href="manage_users.php" title="" class="peep">Manage Users</a></li>
	<li><a href="report_tracker.php" title="" class="logs">Admin Logfile</a></li>
	<li><a href="export_orders.php" title="" class="export">Export orders (all)</a></li>
	<li><a href="back_up_database.php" title="" class="backup">Backup Database</a></li>
	<li><a href="email_addresses.php" title="" class="backup">Emails Ayyyye</a></li>
    </ul>
	</div>
	 <?php } ?>
  
	</div></div>
  <div id="footer">
    <?php 
	// THIS PULLS IN THE FOOTER
	require("../assets/widgets/footer.php"); 
	?>
  </div>
</div>
</body>
</html>
