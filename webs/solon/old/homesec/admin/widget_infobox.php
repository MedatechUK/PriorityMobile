<div id="infobox">
      <p>Shop Info</p>
      <dl>
        <dt>Orders:</dt>
        <?php
	//ACTUAL ORDERS
	$count = mysql_query("select count(id) from $orders_table");
	$rows = mysql_fetch_array($count);
	echo("<dd>$rows[0]</dd>");
	?>
        <dt>Pending Orders:</dt>
        <?php
	//PENDING ORDERS
	$p = 0;
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
				$p++;
				}
			}
		}
		else
		{
		$p++;
		}
		}
	echo("<dd><span style='font-weight:bold;color:#f90;'>$p</span></dd>");
	?>
        <dt>Dispatched Orders:</dt>
        <?php
	//PENDING ORDERS
	$d = 0;
	$query = mysql_query("select * from $orders_table");
	while ($result = mysql_fetch_array($query))
		{
		$status = mysql_query("select * from $order_status_table where order_id = '$result[id]' order by id desc limit 1 ");
		$numrows = mysql_num_rows($status);
	
		if ($numrows > 0)
		{
		while ($presult = mysql_fetch_array($status))
			{
			if ($presult[action] == "Dispatched")
				{
				$d++;
				}
			}
		}
		}
	echo("<dd><span style='font-weight:bold;color:#090;'>$d</span></dd>");
	?>
        <dt>Refunded Orders:</dt>
        <?php
	//PENDING ORDERS
	$re = 0;
	$query = mysql_query("select * from $orders_table");
	while ($result = mysql_fetch_array($query))
		{
		$status = mysql_query("select * from $order_status_table where order_id = '$result[id]' order by id desc limit 1 ");
		$numrows = mysql_num_rows($status);
	
		if ($numrows > 0)
		{
		while ($presult = mysql_fetch_array($status))
			{
			if ($presult[action] == "Refunded")
				{
				$re++;
				}
			}
		}
		}
	echo("<dd><span style='font-weight:bold;color:#f00;'>$re</span></dd>");
	?>
        <dt>Returned Orders:</dt>
        <?php
	//RETURNED ORDERS
	$r = 0;
	$query = mysql_query("select * from $orders_table");
	while ($result = mysql_fetch_array($query))
		{
		$status = mysql_query("select * from $order_status_table where order_id = '$result[id]' order by id desc limit 1 ");
		$numrows = mysql_num_rows($status);
	
		if ($numrows > 0)
		{
		while ($presult = mysql_fetch_array($status))
			{
			if ($presult[action] == "Returned")
				{
				$r++;
				}
			}
		}
		}
	echo("<dd><span style='font-weight:bold;color:#f00;'>$r</span></dd>");
	?>
        <dt>Cancelled Orders:</dt>
        <?php
	//CANCELLED ORDERS
	$r = 0;
	$query = mysql_query("select * from $orders_table");
	while ($result = mysql_fetch_array($query))
		{
		$status = mysql_query("select * from $order_status_table where order_id = '$result[id]' order by id desc limit 1 ");
		$numrows = mysql_num_rows($status);
	
		if ($numrows > 0)
		{
		while ($presult = mysql_fetch_array($status))
			{
			if ($presult[action] == "Cancelled")
				{
				$r++;
				}
			}
		}
		}
	echo("<dd><span style='font-weight:bold;color:#f00;'>$r</span></dd>");
	?>
        <dt><acronym title="Sum of all dispatched orders. Cancelled &amp; refunded orders are deducted from this sum" style="cursor:help">Value of Orders:</acronym></dt>
        <?php
	//VALUE OF ORDERS
	$r = 0;
	$query = mysql_query("select * from $orders_table");
	while ($result = mysql_fetch_array($query))
		{
		
		$dispatched = mysql_query("select * from $order_status_table where order_id = '$result[id]' order by id desc limit 1");
		$dispatch = mysql_fetch_array($dispatched);
		if ($dispatch[action] == "Dispatched")
			{
			//$order_val = explode("£", );
			$subtotal += $result[total_cost];
			}
		}
		
		
		
	echo("<dd>".profit($subtotal)."</dd>");	
	?>
        <dt>Products:</dt>
        <?php
	//PRODUCT COUNT
	$count = mysql_query("select count(id) from $product_table where sub IS NULL");
	$rows = mysql_fetch_array($count);
	echo("<dd>$rows[0]</dd>");
	?>
	
        
	
	
	
	
      </dl>
	  <p>Geek Stuff</p>
	  <dl>
	  <dt>Site Status:</dt>
        <?php
	//SITE STATUS
	$query = mysql_query("select * from $config_table where id ='1'");
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