<ul>
      <li><a href="index.php" title="Admin Home" class="home">Home</a></li>
      <?php
	if ($_COOKIE[user_id] == "master" || stristr($_COOKIE[user_id], "products-") == TRUE)
	{	
	?>
	  <li><a href="manage_content.php" title="" class="edpage">Edit Pages</a></li>
	  <li><a href="manage_products.php" title="" class="edproduct">Edit Products</a></li>
	  <?php } ?>
	  
	  
	  <?php
	if ($_COOKIE[user_id] == "master" || stristr($_COOKIE[user_id], "orders-") == TRUE)
	{	
	?>
	  	  <li><a href="orders.php" title="List of Orders" class="orders">Orders</a></li>
	  <?php } ?>
	  
	  <?php
	if ($_COOKIE[user_id] == "master" || stristr($_COOKIE[user_id], "reports-") == TRUE)
	{	
	?>
	  <li><a href="report_monthly.php" title="" class="report_search">Monthly reports</a></li>
	  <?php } ?>
	  
	  
	  <li><a href="logout.php" title="Logout" class="error">Logout</a></li>
    </ul>