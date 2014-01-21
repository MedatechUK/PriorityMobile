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
      <?php
	  //$tracker=mysql_query("insert into $admin_track_table (name, loggedin, ip, job) values ('$_COOKIE[user_name]', '$tracker_time', '$ip', 'Report - Viewed best sellers')");	
	  
echo("<h2>Reports - Most Purchased Products</h2><p><a href='bite_size_chart.php?type=pp' rel='moodalbox 800 600' title='Graph of most purchased products'>View as graph</a></p>");
		
		echo '<div id="orders"><table><tr><th></th><th>Item</th><th>Qty</th><th>Price (Now)</th><th>Price (Avg)</th><th>Subtotal</th></tr>';
		
		$grand 		= 0;
		$total 		= 0;
		$postage 	= 0;	
		$i 			= 1;
		$products = array();
		
		$product_query = mysql_query("select * from $product_table order by id asc");
		while ($product_result = mysql_fetch_array($product_query))
			{
			$data_query = mysql_query("select * from $report_products_table where product_id = '$product_result[id]'");
			$numrows = mysql_num_rows($data_query);
			if ($numrows > 0)
				{
				while ($data_result = mysql_fetch_array($data_query))
					{
					$products["".$data_result[product_id].""] += $data_result[qty];
					}						
				}			
			}
			
			
			arsort($products); // DISPLAY ARRAY IN REVERSE ORDER BASED ON VALUE
				
			foreach ($products as $key => $val) {
			
					
			$product_query = mysql_query("select * from $product_table where id = '$key'");
			while ($product_result = mysql_fetch_array($product_query))		
				{
				$product_qty 		= 0;
				$product_total 		= 0;
			$get_query = mysql_query("select * from $report_products_table where product_id = '$key'");
			while ($get_result = mysql_fetch_array($get_query))
				{
				$product_qty 		+= $get_result[qty];
				$product_total 		+= $get_result[price] * $get_result[qty];	
				}
				
					$price_avg = $product_total / $product_qty;
					if($num == "1")
					{
					$class=" class='row1'";
					$num=2;
					}
				else
					{
					$class=" class='row2'";
					$num=1;
					}
					echo "<tr$class>";
					echo '<td>'.$i.'</td>';
					echo '<td>'.$product_result[name].' '.$product_result[att_value].'</td>';
					echo '<td>'.$product_qty.'</td>';
					echo '<td>'.profit($product_result[price]).'</td>';
					echo '<td>'.profit($price_avg).'</td>';
					echo '<td>'.profit($product_total).'</td>';
					echo '</tr>';
					$i++;
					$total 		+= $product_total;
					$tqty	 	+= $product_qty;
				}
			}			
		
echo '
<tr class="totals"><td>Totals:</td><td></td><td><strong>'.$tqty.'</strong></td><td><strong>-</strong></td><td><strong>-</strong></td><td><strong>'.profit($total).'</strong></td></tr></table></div>';
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
