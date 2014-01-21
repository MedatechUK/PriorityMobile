<?php 
// SECURITY CHECK TO MAKE SURE THE CORRECT USERNAME AND PASSWORD HAS BEEN ENTERED
if(!$user_name | !$user_pwd)
{
header("location: login.php");
exit;
}

// Include MySQL class
require_once('../assets/widgets/mysql.class.php');
// Include database connection
require_once('../assets/widgets/global.inc.php');
// Include functions
require_once('../assets/widgets/functions.inc.php');


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
      <?php
echo("<h2>Reports - Most Purchased Products</h2>");
	
	
	$bigcart = "";
	$query = mysql_query("select cartinfo from SHOP1_orders");
	while($result = mysql_fetch_array($query))
		{
		$bigcart .= $result[cartinfo];
		$bigcart .= ",";
		}
	
$cart = $bigcart;
	$items = explode(',',$cart);
		$contents = array();
		foreach ($items as $item) {
			$contents[$item] = (isset($contents[$item])) ? $contents[$item] + 1 : 1;
		}
		foreach ($contents as $id=>$qty) {
		$pquery = mysql_query("SELECT * FROM SHOP1_products WHERE id = '$id'");
		while($presult = mysql_fetch_array($pquery))
			{		
			$if_query = mysql_query("SELECT * FROM SHOP1_report_products WHERE prod_id = '$id'");
			$if_num_rows = mysql_num_rows($if_query);
			
			if ($if_num_rows == 0)
				{
				$process = mysql_query("insert into SHOP1_report_products (prod_id, qty) values ('$id', '$qty')");
				}
			else
				{
				$process = mysql_query("update SHOP1_report_products set qty='$qty' where prod_id = '$id'");
				}
			}
		}
		
		
		
		
		
		echo '<table id="basket"><tr><th></th><th>Item</th><th>Price</th><th>Qty</th><th>Subtotal</th></tr>';
		
		$grand = 0;
		$total = 0;
		$i = 1;
		
		$query = mysql_query("SELECT * FROM SHOP1_report_products order by qty desc");
		while($result = mysql_fetch_array($query))
			{	
			$tquery = mysql_query("SELECT * FROM SHOP1_products WHERE id = '$result[prod_id]'");
			while($tresult = mysql_fetch_array($tquery))
				{	
				$total += $tresult[price] * $result[qty];
					
				echo '<tr>';
				echo '<td>'.$i.'</td>';
				echo '<td>'.$tresult[name].' '.$tresult[att_value].' ('.$tresult[code].')</td>';
				echo '<td>&pound;'.$tresult[price].'</td>';
				echo '<td>'.$result[qty].'</td>';
				echo '<td>&pound;'.number_format(($tresult[price] * $result[qty]), 2, '.', '').'</td>';
				echo '</tr>';
				$i++;
				}
			}
		
		$oquery = mysql_query("SELECT * FROM SHOP1_orders");
				while($oresult = mysql_fetch_array($oquery))
				{	
				$order_val = explode("£", $oresult[total_cost]);
				$grand += $order_val[1];
				
				}
		
		$postage = $grand-$total;
		
echo '<tr class="total"><td colspan="5">Total: <strong>&pound;'.number_format($total, 2, '.', '').'</strong></td></tr><tr class="total"><td colspan="5">Postage costs: <strong>&pound;'.number_format($postage, 2, '.', '').'</strong></td></tr></table>';
	
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
