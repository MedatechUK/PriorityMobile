	
<ul class="nav">
	<li class="top"><a href="index.php" id="home" class="top_link"><span>Home</span></a></li>
	
	<?php
	if ($_COOKIE[user_id] == "master" || stristr($_COOKIE[user_id], "products-") == TRUE)
	{	
	?>
	
	<li class="top"><a href="manage_products.php" id="products" class="top_link"><span>Product Catalog</span><!--[if gte IE 7]><!--></a><!--<![endif]-->
		<!--[if lte IE 6]><table><tr><td><![endif]-->
		<ul class="sub1">
		<li><a href="add_products.php" title="" class="add_product">Add Products</a></li>
          <li><a href="manage_products.php" title="" class="edproduct">Edit Products</a></li>
          <li><a href="manage_content.php" title="" class="edpage">Edit Pages</a></li>
          <li><a href="manage_category.php?add=ok" title="" class="add_category">Add Category</a></li>
          <li><a href="manage_category.php" title="" class="edit_category">Edit Categories</a></li>
          <li><a href="manage_images.php" title="" class="edit_images">Manage Images</a></li>
		  <li><a href="export_products.php" title="" class="export">Export products</a></li>
        
		</ul>
		<!--[if lte IE 6]></td></tr></table></a><![endif]-->
	</li>
	<?php } ?>
	<?php
	if ($_COOKIE[user_id] == "master" || stristr($_COOKIE[user_id], "orders-") == TRUE)
	{	
	?>
	<li class="top"><a href="orders.php" id="orders" class="top_link"><span>Orders</span><!--[if gte IE 7]><!--></a><!--<![endif]-->
		<!--[if lte IE 6]><table><tr><td><![endif]-->
		<ul class="sub2">
			<li><a href="orders.php" title="" class="orders">Order Database</a></li>
          <li><a href="order_start.php" title="" class="peep">Manual order</a></li>
          <li><a href="export_orders.php" title="" class="export">Export orders (all)</a></li>
          <li><a href="bite_size_xo.php" title="" class="export" rel='moodalbox'>Export orders (select)</a></li>
          <li><a href="alerts_stock.php" title="" class="stock_alerts">Stock Alerts</a></li>
		</ul>
		<!--[if lte IE 6]></td></tr></table></a><![endif]-->
	</li>
	<?php } ?>
	<?php
	if ($_COOKIE[user_id] == "master" || stristr($_COOKIE[user_id], "reports-") == TRUE)
	{	
	?>
	<li class="top"><a href="report_monthly.php" id="reports" class="top_link"><span>Reports</span><!--[if gte IE 7]><!--></a><!--<![endif]-->
		<!--[if lte IE 6]><table><tr><td><![endif]-->
		<ul class="sub3">
			<li><a href="report_popular.php" title="" class="report_popular">Popular Products</a></li>
          <li><a href="report_products.php" title="" class="report_purchased">Purchased Products</a></li>
          <li><a href="report_monthly.php" title="" class="report_monthly">Monthly Sales Report</a></li>
		  <li><a href="report_search.php" title="" class="report_search">Search Report</a></li>
		</ul>
		<!--[if lte IE 6]></td></tr></table></a><![endif]-->
	</li>
	<?php } ?>
	<?php
	if ($_COOKIE[user_id] == "master" || stristr($_COOKIE[user_id], "marketing-") == TRUE)
	{	
	?>
	<li class="top"><a href="#" id="Marketing" class="top_link"><span>Marketing</span><!--[if gte IE 7]><!--></a><!--<![endif]-->
		<!--[if lte IE 6]><table><tr><td><![endif]-->
		<ul class="sub4">
			<li><a href="tracker.php" class="report_tracker">Campaign Tracker</a></li>
          <li><a href="footprint.php" class="footprints">Site Footprints</a></li>
          <li><a href="manage_scripts.php" class="external_scripts">Manage External Scripts</a></li>
          <li><a href="landing_page_manager.php" title="" class="php_icon">Landing Page Manager</a></li>
          <li><a href="seo_manager.php" title="" class="php_icon">SEO Page Manager</a></li>
		</ul>
		<!--[if lte IE 6]></td></tr></table></a><![endif]-->
	</li>
	<?php } ?>
	<?php
	if ($_COOKIE[user_id] == "master" || stristr($_COOKIE[user_id], "external-") == TRUE)
	{	
	?>
	<li class="top"><a href="#" id="external" class="top_link"><span>External Links</span><!--[if gte IE 7]><!--></a><!--<![endif]-->
		<!--[if lte IE 6]><table><tr><td><![endif]-->
		<ul class="sub5">
			<li><a href="https://www.google.com/analytics/home/?hl=en" title="Opens in a new window" target="_blank" class="anal">Google Analytics</a></li>
          <li><a href="https://adwords.google.com/select/Login?sourceid=awo&subid=ww-en-et-gaia" title="Opens in a new window" target="_blank" class="adword">Google Adwords</a></li>
          <li><a href="http://stats.home-security-store.co.uk" title="Opens in a new window" target="_blank" class="anal">Site Statistics</a></li>
		</ul>
		<!--[if lte IE 6]></td></tr></table></a><![endif]-->
	</li>
	<?php } ?>
	<?php
	if ($_COOKIE[user_id] == "master")
	{	
	?>
	<li class="top"><a href="#" id="admin_links" class="top_link"><span>Admin</span><!--[if gte IE 7]><!--></a><!--<![endif]-->
		<!--[if lte IE 6]><table><tr><td><![endif]-->
		<ul class="sub5">
		<li><a href="manage_users.php" title="" class="peep">Manage Users</a></li>
			<li><a href="report_tracker.php" title="" class="logs">Admin Logfile</a></li>
            <li><a href="back_up_database.php" title="" class="backup">Backup Database</a></li>
		</ul>
		<!--[if lte IE 6]></td></tr></table></a><![endif]-->
	</li>
	<?php } ?>
	<li class="top"><a href="logout.php" id="privacy" class="top_link"><span>Logout</span></a></li>
</ul>