<?php

//==========================================================================
//NUMBER FORMATING
//==========================================================================
function profit($string)
	{
	
	if ($string < 0)
		{
		$string = "<span class='negative'>&pound;". number_format($string, 2, '.', ',')."</span>";
		}
	else
		{
		$string = "<span class='positive'>&pound;". number_format($string, 2, '.', ',')."</span>";
		}
	
	return $string;
	}


function writeShoppingCart() {
	$cart = $_SESSION['cart'];
	if (!$cart) {
		return '<span class="preview_basket">Basket: 0 items</span>';
	} else {
		// Parse the cart session variable
		$items = explode(',',$cart);
		$s = (count($items) > 1) ? 's':'';
		return '<span class="preview_basket"><a href="basket.php" title="Click here to view the content of your basket">Basket: '.count($items).' item'.$s.'</a></span>';
	}
}

function showCart() {
	global $db;
	$cart = $_SESSION['cart'];
	if ($cart) {
		$items = explode(',',$cart);
		$contents = array();
		foreach ($items as $item) {
			$contents[$item] = (isset($contents[$item])) ? $contents[$item] + 1 : 1;
		}
		$output[] = '<form action="basket.php?action=update" method="post" id="cart">';	
		$output[] = '<table id="basket"><tr><th></th><th>Item</th><th>Price</th><th>Qty</th><th>Subtotal</th></tr>';
		foreach ($contents as $id=>$qty) {
			$sql = 'SELECT * FROM SHOP1_products WHERE id = '.$id;
			$result = $db->query($sql);
			$row = $result->fetch();
			extract($row);
			$output[] = '<tr>';
			$output[] = '<td><a href="basket.php?action=delete&id='.$id.'" title="Remove this item from your basket" class="remove">Remove</a></td>';
			$output[] = '<td>'.$name.' '.$att_value.' ('.$code.')</td>';
			$output[] = '<td>&pound;'.$price.'</td>';
			$output[] = '<td><input type="text" name="qty'.$id.'" value="'.$qty.'" size="3" maxlength="2" /></td>';
			$output[] = '<td>&pound;'.number_format(($price * $qty), 2, '.', '').'</td>';
			$total += $price * $qty;
			$output[] = '</tr>';
		}
		$output[] = '<tr class="total"><td colspan="5">Total: <strong>&pound;'.number_format($total, 2, '.', '').'</strong></td></tr></table>';
		$output[] = '
		<p><small>If you change the quantity of any item in your basket please remember to click \'Update basket\' <strong>before</strong> you go to \'Checkout\'.</small></p>		
		<p class="buttons"><a href="products.php" title="Click here to go back to product list"><img src="assets/images/back_button.gif" alt="Back to product list"/></a> <input name="submit" type="image" value="Submit" src="assets/images/update_basket.gif" alt="Update Basket" /> <a href="checkout.php" title="Click here to proceed to the checkout"><img src="assets/images/checkout.gif" alt="Proceed to Checkout"/></a></p>';
		$output[] = '</form>';
		
	} else {
		$output[] = '<p>Your shopping basket is empty</p>';
	}
	return join('',$output);
}

function confirmCart() {
	global $db;
	$cart = $_SESSION['cart'];
	if ($cart) {
		$items = explode(',',$cart);
		$contents = array();
		foreach ($items as $item) {
			$contents[$item] = (isset($contents[$item])) ? $contents[$item] + 1 : 1;
		}
		$output[] = "<table id='basketconfirm'><tr><th>Item</th><th>Price</th><th>Qty</th><th>Subtotal</th></tr>";
		foreach ($contents as $id=>$qty) {
			$sql = 'SELECT * FROM SHOP1_products WHERE id = '.$id;
			$result = $db->query($sql);
			$row = $result->fetch();
			extract($row);
			$output[] = '<tr>';
			$output[] = '<td>'.$name.' '.$att_value.' ('.$code.')</td>';
			$output[] = '<td>&pound;'.$price.'</td>';
			$output[] = '<td>'.$qty.'</td>';
			$output[] = '<td>&pound;'.number_format(($price * $qty), 2, '.', '').'</td>';
			$total += $price * $qty;
			$output[] = '</tr>';
		}
		
		$output[] = "<tr class='subtotal'><td colspan='4' align='right'>Subtotal of items: <strong>&pound;".number_format($total, 2, '.', '')."</strong></td></tr>";
		$total += $_POST[deliverymethod];		
		$output[] = "<tr class='postage'><td colspan='4' align='right'>Postage and Packaging";
		
		if ($_POST[deliverymethod] == 9.95)
			{
			$output[] = " (Next Day Delivery)";
			}
		
		
		$output[] = ": <strong>&pound;".number_format($_POST[deliverymethod], 2, '.', '')."</strong></td></tr>";
		
		$exvat = $total / 1.2;
		$vat = $total - $exvat;
		
		//$vat=$total*0.2;
		$output[] = "<tr class='subtotal'><td colspan='4' align='right'>VAT (20%): <strong>&pound;".number_format($vat, 2, '.', '')."</strong></td></tr>";
		$output[] = "<tr class='total'><td colspan='4'>Total: <strong>&pound;".number_format($total, 2, '.', '')."</strong></td></tr></table>";
		$output[] = "<input type='hidden' name='inputamount' value='".number_format($total, 2, '.', '')."'/>";
	} else {
		$output[] = '<p>You have no items in the basket.</p>';
	}
	return join('',$output);
}


function checkoutCart() {
	global $db;
	$cart = $_SESSION['cart'];
	if ($cart) {
		$items = explode(',',$cart);
		$contents = array();
		foreach ($items as $item) {
			$contents[$item] = (isset($contents[$item])) ? $contents[$item] + 1 : 1;
		}
		$output[] = "<table id='basketconfirm'><tr><th>Item</th><th>Price</th><th>Qty</th><th>Subtotal</th></tr>";
		foreach ($contents as $id=>$qty) {
			$sql = 'SELECT * FROM SHOP1_products WHERE id = '.$id;
			$result = $db->query($sql);
			$row = $result->fetch();
			extract($row);
			$output[] = '<tr>';
			$output[] = '<td>'.$name.' '.$att_value.' ('.$code.')</td>';
			$output[] = '<td>&pound;'.$price.'</td>';
			$output[] = '<td>'.$qty.'</td>';
			$output[] = '<td>&pound;'.number_format(($price * $qty), 2, '.', '').'</td>';
			$total += $price * $qty;
			$output[] = '</tr>';
		}
	
		$output[] = "<tr class='subtotal'><td colspan='4' align='right'>Subtotal of items: <strong>&pound;".number_format($total, 2, '.', '')."</strong></td></tr>";
		$total += $_POST[deliverymethod];		
		$output[] = "<tr class='postage'><td colspan='4' align='right'>Postage and Packaging: <strong>&pound;".number_format($_POST[deliverymethod], 2, '.', '')."</strong></td></tr>";
		
		$exvat = $total / 1.2;
		$vat = $total - $exvat;
		
		//$vat=$total*0.2;
		$output[] = "<tr class='subtotal'><td colspan='4' align='right'>VAT (20%): <strong>&pound;".number_format($vat, 2, '.', '')."</strong></td></tr>";
				$output[] = "<tr class='total'><td colspan='4'>Total: <strong>&pound;".number_format($total, 2, '.', '')."</strong></td></tr></table>";
	} 
	return join('',$output);
}

function slashes($string)
	{
	$string = stripslashes(stripslashes(stripslashes(stripslashes(stripslashes(stripslashes($string))))));
	return $string;
	}
	
function selfURL() 
	{ 
	$s = empty($_SERVER["HTTPS"]) ? '' : ($_SERVER["HTTPS"] == "on") ? "s" : ""; 
	$protocol = strleft(strtolower($_SERVER["SERVER_PROTOCOL"]), "/").$s; 
	$port = ($_SERVER["SERVER_PORT"] == "80") ? "" : (":".$_SERVER["SERVER_PORT"]); 
	return $protocol."://".$_SERVER['SERVER_NAME'].$port.$_SERVER['REQUEST_URI']; 
	} 
	
function strleft($s1, $s2) 
	{ 
	return substr($s1, 0, strpos($s1, $s2)); 
	}
?>