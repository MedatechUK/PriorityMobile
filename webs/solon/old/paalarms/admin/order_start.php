<?php
session_start();
if(!$_COOKIE[user_name] | !$_COOKIE[user_pwd])
{
header("location: login.php");
exit;
}

require_once('../assets/widgets/mysql.class.php');
require_once('../assets/widgets/global.inc.php');
require_once('../assets/widgets/functions.inc.php');
require_once('../assets/widgets/admin_functions.inc.php');
require("../assets/widgets/global_variables.php");


?>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
<title>Admin Interface</title>
<link rel="shortcut icon" href="http://www.personal-attack-alarms.net/favicon.ico" />
<link rel="stylesheet" href="../assets/css/admin.css" type="text/css" media="screen" />
<script language="javascript" type="text/javascript" src="../assets/scripts/lib.js"></script>
<script language="javascript" type="text/javascript" src="../assets/scripts/popup.js"></script>
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
  <h2>Create Order</h2>
   <?php
   // Process actions
$cart = $_SESSION['cart'];
$action = $_GET['action'];
switch ($action) {
	case 'add':
		if ($cart) {
		
		// IF THE SESSION IS HERE DO THIS FOR THE QUANITY ADDED
		if ($_POST['amount'] == "1" || !$_POST['amount'])
			{
			$cart .= ','.$_POST['id'];
			}
		else
			{
			$i = 1;
			while ($i <= $_POST['amount'] || !$_POST['amount'])
				{
				$cart .= ','.$_POST['id'];
				$i++;
				}
			}
		
			
		} 
		
		else {
		
		// IF NO SESSION IS HERE DO THIS FOR THE QUANITY ADDED
			if ($_POST['amount'] == "1")
				{
				$cart = $_POST['id'];
				}
			else
				{
				$i = 1;
				$cart = $_POST['id'];
				while ($i < $_POST['amount'])
					{
					$cart .= ','.$_POST['id'];
					$i++;
					}
				}
		
		
			
		}
		break;
	case 'delete':
		if ($cart) {
			$items = explode(',',$cart);
			$newcart = '';
			foreach ($items as $item) {
				if ($_GET['id'] != $item) {
					if ($newcart != '') {
						$newcart .= ','.$item;
					} else {
						$newcart = $item;
					}
				}
			}
			$cart = $newcart;
		}
		break;
	case 'update':
	if ($cart) {
		$newcart = '';
		foreach ($_POST as $key=>$value) {
			if (stristr($key,'qty')) {
				$id = str_replace('qty','',$key);
				$items = ($newcart != '') ? explode(',',$newcart) : explode(',',$cart);
				$newcart = '';
				foreach ($items as $item) {
					if ($id != $item) {
						if ($newcart != '') {
							$newcart .= ','.$item;
						} else {
							$newcart = $item;
						}
					}
				}
				for ($i=1;$i<=$value;$i++) {
					if ($newcart != '') {
						$newcart .= ','.$id;
					} else {
						$newcart = $id;
					}
				}
			}
		}
	}
	$cart = $newcart;
	break;
}
$_SESSION['cart'] = $cart;
echo adminCart();

// LIST OF PRODUCTS
    echo("<div id='admin'>
	<table width='90%'>
		<thead>
			<tr><th></th><th>Product</th><th>Price</th><th>Stock</th><th>Code</th><th width='61'>Options</th></tr>
		</thead>
		<tbody>
	
	");
	
	$num = 1;
	
	$query = mysql_query("select * from SHOP1_products where sub IS NULL order by name asc");
	while($result = mysql_fetch_array($query))
		{
		
		if($class =="row2")
			{
				$class = 'row5';
			}
			else
			{
				$class = 'row2';
			}
		
		if ($result[stock] == "NO")
			{
			$stock = "<small style='color: #F00;font-weight: bold'>Out of stock</small>";
			}
		else
			{
			$stock = "<small>In stock</small>";
			}
		
		echo("<tr class='$class'><td><small>$num.</small></td><td><small><a href='bite_size_product.php?product_id=$result[id]' onclick=\"raw_popup('bite_size_product.php?product_id=$result[id]'); return false\" title=\"Opens in a new window\" class=\"new_window_small\" target=\"_blank\">$result[name]</a></small></td><td><small>&pound;$result[price]</small></td><td>$stock</td><td><small>$result[code]</small></td><td><small><form action='order_start.php?action=add' method='post'>
		<input type='hidden' name='id' value='$result[id]'/>
		<input name=\"submit\" type=\"image\" value=\"Submit\" src=\"/assets/images/add.png\" alt=\"Add to Basket\" />
		</form></small></td></tr>");
		$num++;
		$subquery = mysql_query("select * from SHOP1_products where sub = '$result[id]' order by id asc");
		while($sresult = mysql_fetch_array($subquery))
		{
			if($sclass =="row5")
			{
				$sclass = 'row2';
			}
			else
			{
				$sclass = 'row5';
			}
			
			if ($sresult[stock] == "NO")
			{
			$stock = "<small style='color: #F00;font-weight: bold'>Out of stock</small>";
			}
		else
			{
			$stock = "<small>In stock</small>";
			}
		
		echo("<tr class='$class'><td></td><td><small>&#0187;&#0187;&nbsp;<a href='bite_size_product.php?product_id=$result[id]' onclick=\"raw_popup('bite_size_product.php?product_id=$result[id]'); return false\" title=\"Opens in a new window\" class=\"new_window_small\" target=\"_blank\">$sresult[att_name]: $sresult[att_value]</a></small>
		
		
		</td><td><small>&pound;$sresult[price]</small></td><td>$stock</td><td><small>$sresult[code]</small></td><td><small><form action='order_start.php?action=add' method='post'>
		<input type='hidden' name='id' value='$sresult[id]'/>
		<input name=\"submit\" type=\"image\" value=\"Submit\" src=\"/assets/images/add.png\" alt=\"Add to Basket\" />
		</form></small></td></tr>");
		}
		
				
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
