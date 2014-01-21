
<?php

//==========================================================================
//OAK HOUSE IP ADDRESSES
//==========================================================================
function oakhouse($string)
	{
	
	if ($string == "213.210.21.186" || $string == "78.32.146.37")
		{	
		$string	= str_replace("213.210.21.186", "<acronym title='$string'>Oak House User</acronym>", $string);
		$string	= str_replace("78.32.146.37", "<acronym title='$string'>Oak House User</acronym>", $string);
		}
	else
		{
		$string = "<acronym title='".gethostbyaddr($string)."'>$string</acronym>";
		}
	
	return $string;
	}

//==========================================================================
//HTML CLEANUP
//==========================================================================
function html($string)
	{
	$string = htmlentities($string, ENT_QUOTES);
	return $string;
	}
	

//==========================================================================
//SHOW CART - admin section
//==========================================================================
function adminCart() {
	global $db;
	$cart = $_SESSION['cart'];
	$_SESSION['cartdetail'] = "";
	if ($cart) {
		$items = explode(',',$cart);
		$contents = array();
		foreach ($items as $item) {
			$contents[$item] = (isset($contents[$item])) ? $contents[$item] + 1 : 1;
		}
		$output[] = '<form action="order_start.php?action=update" method="post" id="cart">';	
		$output[] = '<table id="basket"><tr><th></th><th>Item</th><th>Price</th><th>Qty</th><th>Subtotal</th></tr>';
		foreach ($contents as $id=>$qty) {
			$sql = 'SELECT * FROM SHOP1_products WHERE id = '.$id;
			$result = $db->query($sql);
			$row = $result->fetch();
			extract($row);
					
					
					$product_price = $price;
					$discount = "";
			
			$i = 1;
			while ($i <= $qty)
				{
				$_SESSION['cartdetail'] .= " $id | $product_price,";
				$i++;
				}
			
			$output[] = '<tr>';
			$output[] = '<td><a href="order_start.php?action=delete&id='.$id.'" title="Remove this item from your basket" class="remove">Remove</a></td>';
			$output[] = '<td>'.$name.' '.$att_value.'<br/><small>'.$code.'</small></td>';
			$output[] = '<td>&pound;'.number_format($product_price, 2, '.', '').'</td>';
			$output[] = '<td><input type="text" name="qty'.$id.'" value="'.$qty.'" size="4" maxlength="5" /></td>';
			$output[] = '<td>&pound;'.number_format(($product_price * $qty), 2, '.', '').'</td>';
			$total += $product_price * $qty;
			$output[] = '</tr>';
		}
		
		if ($_POST[discount] == "0.00")
			{
			$discount_val = "0.00";
			$output[] = '<tr class="total"><td colspan="5">Discount: <strong>&pound;<input type="text" name="discount" value="'.$discount_val.'" size="7" maxlength="8" /></strong></td></tr>';
			}
		else	
			{
			$discount_val = $_POST[discount];
			$output[] = '<tr class="total"><td colspan="5">Discount: <strong>&pound;<input type="text" name="discount" value="'.$discount_val.'" size="7" maxlength="8" /></strong></td></tr>';
			$_SESSION['discount'] = $_POST[discount];
			}
		
		
		
		$total -= $discount_val;
		
		$output[] = '<tr class="total"><td colspan="5">Total: <strong>&pound;'.number_format($total, 2, '.', '').'</strong></td></tr></table>';
		$output[] = '
		<p class="buttons"><input name="submit" type="image" value="Submit" src="/assets/images/update_basket.gif" alt="Update Basket" /> <a href="order_checkout.php" title="Click here to proceed to the checkout"><img src="/assets/images/checkout.gif" alt="Proceed to Checkout"/></a></p>';
		$output[] = '</form>';
		
	} else {
		$output[] = '<p>Customer shopping basket is empty</p>';
	}
	return join('',$output);
}


//==========================================================================
//CONFIRM CART - orders_checkout.php
//==========================================================================

function admin_confirmCart() {
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
			$product_price = $price;
			//$discount = "";
			
			$output[] = '<tr>';
			$output[] = '<td>'.$name.' '.$att_value.'<br/><small>'.$code.'</small></td>';
			$output[] = '<td>&pound;'.$product_price.' '.$discount.'</td>';
			$output[] = '<td>'.$qty.'</td>';
			$output[] = '<td>&pound;'.number_format(($product_price * $qty), 2, '.', '').'</td>';
			$total += $product_price * $qty;
			$output[] = '</tr>';
		}
		$exvat = $total / 1.175;
		$output[] = "<tr class='subtotal'><td colspan='4' align='right'>Subtotal of items: <strong>&pound;".number_format($total, 2, '.', '')."</strong></td></tr>";
		
		
		
		
		if ($_POST['deliverymethod'] == "Other")
			{
			$total += $_POST[custompostage];
			$postage = $_POST[custompostage];
			}
		else
			{
			$total += $_POST['deliverymethod'];
			$postage = $_POST[deliverymethod];
			}
		
		
		
		
		//$total += $_POST[deliverymethod];		
		$output[] = "<tr class='postage'><td colspan='4' align='right'>Postage and Packaging";
		
		if ($_POST[deliverymethod] == 8.95)
			{
			$output[] = " (Next Day Delivery)";
			}
		
		$output[] = ": <strong>&pound;".number_format($postage, 2, '.', '')."</strong></td></tr>";
		
		
		
		
		
		// IF DISCOUNT ADDED
		if ($_SESSION['discount'] > 0)
			{		
			$output[] = "<tr class='postage'><td colspan='4'>Discount: <strong>&pound;".number_format($_SESSION['discount'], 2, '.', '')."</strong></td></tr>";
		
			$total = $total - $_SESSION['discount'];
		}
		
		
		
		$output[] = "<tr class='total'><td colspan='4'>Total: <strong>&pound;".number_format($total, 2, '.', '')."</strong></td></tr></table>";
		$output[] = "<input type='hidden' name='inputamount' value='".number_format($total, 2, '.', '')."'/>";
	} else {
		$output[] = '<p>You have no items in the basket.</p>';
	}
	return join('',$output);
}

//==========================================================================
//CHECKOUT CART - ON CONFIRMATION
//==========================================================================
function admin_checkoutCart() {
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
			
			
				$product_price = $price;
				$discount = "";
			
			$output[] = '<tr>';
			$output[] = '<td>'.$name.' '.$att_value.'<br/><small>'.$code.'</small></td>';
			$output[] = '<td>&pound;'.$product_price.' '.$discount.'</td>';
			$output[] = '<td>'.$qty.'</td>';
			$output[] = '<td>&pound;'.number_format(($product_price * $qty), 2, '.', '').'</td>';
			$total += $product_price * $qty;
			$output[] = '</tr>';
		}
	
		$output[] = "<tr class='subtotal'><td colspan='4' align='right'>Subtotal of items: <strong>&pound;".number_format($total, 2, '.', '')."</strong></td></tr>";
		
		if ($_POST['deliverymethod'] == "Other")
			{
			$total += $_POST[custompostage];
			$postage = $_POST[custompostage];
			}
		else
			{
			$total += $_POST['deliverymethod'];
			$postage = $_POST[deliverymethod];
			}
			
				
				$output[] = "<tr class='postage'><td colspan='4' align='right'>Postage and Packaging: <strong>&pound;".number_format($postage, 2, '.', '')."</strong></td></tr>";
				
		// IF DISCOUNT ADDED
		if ($_SESSION['discount'] > 0)
			{		
			$output[] = "<tr class='postage'><td colspan='4'>Discount: <strong>&pound;".number_format($_SESSION['discount'], 2, '.', '')."</strong></td></tr>";
		
			$total = $total - $_SESSION['discount'];
		}
		
				
				$output[] = "<tr class='total'><td colspan='4'>Total: <strong>&pound;".number_format($total, 2, '.', '')."</strong></td></tr></table>";
	} 
	return join('',$output);
}
?>