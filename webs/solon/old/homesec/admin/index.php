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
require_once('../assets/widgets/global_variables.php');

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
    <h2>Admin Home page</h2>
    <?php require("widget_infobox.php"); ?>
    <?php
	// NEXT DAY DELIVERY ALERT
	$n 	= 0;
	$s	= 0;
	$query = mysql_query("select * from $orders_table");
	while ($result = mysql_fetch_array($query))
		{
		$status = mysql_query("select * from $order_status_table where order_id = '$result[id]' order by id desc limit 1 ");
		$numrows = mysql_num_rows($status);
	
		if ($numrows > 0)
		{
		while ($presult = mysql_fetch_array($status))
			{
			if ($presult[action] == "Pending")
				{
				if ($result[postage] != "8.95")
					{
					$n++;
					}
				else
					{
					$s++;
					}
				}
			}
		}
		else
			{
			if ($result[postage] != "8.95")
					{
					$n++;
					}
				else
					{
					$s++;
					}
			}
		}
		if ($s > 0)
			{
			$special = "<p>$s outstanding order(s) requiring next day delivery</p>";
			}
		if ($n > 0)
			{
			$normal = "<p>$n outstanding order(s) requiring standard delivery</p>";
			}
	if ($n > 0 || $s > 0)
	{
	
	echo("<div class='notice'>$special $normal</div>");
	}
	?>
	<?php 
	//DISPLAY INFO
	//echo("$admin_whats_new"); 
	?>
	
    <div id="admin_blocks">
      <?php
	if ($_COOKIE[user_id] == "master" || stristr($_COOKIE[user_id], "products-") == TRUE)
	{	
	?>
      <div class="home_block">
        <h3>Product Catalog</h3>
        <ul class="list">
          <li><a href="add_products.php" title="" class="add_product">Add Products</a></li>
          <li><a href="manage_products.php" title="" class="edproduct">Edit Products</a></li>
          <li><a href="manage_content.php" title="" class="edpage">Edit Pages</a></li>
          <li><a href="manage_category.php?add=ok" title="" class="add_category">Add Category</a></li>
          <li><a href="manage_category.php" title="" class="edit_category">Edit Categories</a></li>
          <li><a href="manage_images.php" title="" class="edit_images">Manage Images</a></li>
		  <li><a href="export_products.php" title="" class="export">Export products</a></li>
        </ul>
      </div>
      <?php } ?>
      <?php
	if ($_COOKIE[user_id] == "master" || stristr($_COOKIE[user_id], "orders-") == TRUE)
	{	
	?>
      <div class="home_block">
        <h3>Orders</h3>
        <ul class="list">
          <li><a href="orders.php" title="" class="orders">Order Database</a></li>
          <li><a href="order_start.php" title="" class="peep">Manual order</a></li>
          <li><a href="export_orders.php" title="" class="export">Export orders (all)</a></li>
          <li><a href="bite_size_xo.php" title="" class="export" rel='moodalbox'>Export orders (select)</a></li>
          <li><a href="alerts_stock.php" title="" class="stock_alerts">Stock Alerts</a></li>
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
          <li><a href="report_monthly.php" title="" class="report_monthly">Monthly Sales Report</a></li>
		  <li><a href="report_search.php" title="" class="report_search">Search Report</a></li>
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
          <li><a href="tracker.php" class="report_tracker">Campaign Tracker</a></li>
          <li><a href="footprint.php" class="footprints">Site Footprints</a></li>
          <li><a href="manage_scripts.php" class="external_scripts">Manage External Scripts</a></li>
          <li><a href="landing_page_manager.php" title="" class="php_icon">Landing Page Manager</a></li>
          <li><a href="seo_manager.php" title="" class="php_icon">SEO Page Manager</a></li>
        </ul>
      </div>
      <?php } ?>
      <?php
	if ($_COOKIE[user_id] == "master" || stristr($_COOKIE[user_id], "external-") == TRUE)
	{	
	?>
      <div class="home_block">
        <h3>External Links</h3>
        <ul class="list">
          <li><a href="https://www.google.com/analytics/home/?hl=en" title="Opens in a new window" target="_blank" class="anal">Google Analytics</a></li>
          <li><a href="https://adwords.google.com/select/Login?sourceid=awo&subid=ww-en-et-gaia" title="Opens in a new window" target="_blank" class="adword">Google Adwords</a></li>
          <li><a href="http://stats.home-security-store.co.uk" title="Opens in a new window" target="_blank" class="anal">Site Statistics</a></li>
        </ul>
      </div>
      <?php } ?>
      <?php
	if ($_COOKIE[user_id] == "master" || stristr($_COOKIE[user_id], "admin-") == TRUE)
	{	
	?>
      <div class="home_block">
        <h3>Admin</h3>
        <ul class="list">
			<li><a href="manage_users.php" title="" class="peep">Manage Users</a></li>
			<li><a href="report_tracker.php" title="" class="logs">Admin Logfile</a></li>
            <li><a href="back_up_database.php" title="" class="backup">Backup Database</a></li>
			<li><a href="export_products_base.php" title="" class="export">Export products (Base)</a></li>
        </ul>
      </div>
      <?php } ?>
    </div>
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
