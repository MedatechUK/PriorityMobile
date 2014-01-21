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

//==========================================================================
//SIMPLE COUNT OF CART
//==========================================================================
function writeShoppingCart() {
global $db;
	$cart = $_SESSION['cart'];
	if (!$cart) {
		return '<div class="basket_empty"><h4>Shopping basket</h4><span class="preview_basket">Your basket is empty</span></div>';
	} else {
		// Parse the cart session variable
		$items = explode(',',$cart);
		$s = (count($items) > 1) ? 's':'';
		
		foreach ($items as $item) {
			$contents[$item] = (isset($contents[$item])) ? $contents[$item] + 1 : 1;
		}
		
		foreach ($contents as $id=>$qty) {
			$sql = 'SELECT * FROM SHOP2_products WHERE id = '.$id;
			$result = $db->query($sql);
			$row = $result->fetch();
			extract($row);
		
		// DISCOUNT FUNCTION
			if ($qty >= 10 && $qty < 20)
				{
				if (!$dis10 || $dis10 == 0 || $dis10 == $price)
					{
					$product_price = $price;
					$discount = "";
					}
				else
					{
					$product_price = $dis10;
					$discount = "<span style='text-decoration:line-through;color:#f00;'>&pound;$price</span>";
					}
				}
			elseif ($qty >= 20 && $qty < 50)
				{
				if (!$dis20 || $dis20 == 0 || $dis20 == $price)
					{
					$product_price = $price;
					$discount = "";
					}
				else
					{
					$product_price = $dis20;
					$discount = "<span style='text-decoration:line-through;color:#f00;'>&pound;$price</span>";
					}
				}
			elseif ($qty >= 50)
				{
				if (!$dis50 || $dis50 == 0 || $dis50 == $price)
					{
					$product_price = $price;
					$discount = "";
					}
				else
					{
					$product_price = $dis50;
					$discount = "<span style='text-decoration:line-through;color:#f00;'>&pound;$price</span>";
					}
				}
			else
				{
				$product_price = $price;
				$discount = "";
				}
			
			$total += $product_price * $qty;
			}
		
		return '
		<div class="basket_summary">
		<h4>Shopping basket</h4>
		<span class="preview_basket">Items: <strong>'.count($items).'</strong></span>
		<span class="preview_total">Sub total: <strong>'.profit($total).'</strong></span>
		<span class="preview_links"><a href="/basket.php">Go to checkout</a></span>
		</div>
		';
	}
}

//==========================================================================
//SHOW CART - basket.php
//==========================================================================
function showCart() {
	global $db;
	$cart = $_SESSION['cart'];
	$_SESSION['cartdetail'] = "";
	if ($cart) {
		$items = explode(',',$cart);
		$contents = array();
		foreach ($items as $item) {
			$contents[$item] = (isset($contents[$item])) ? $contents[$item] + 1 : 1;
		}
		$output[] = '<h2>Shopping basket</h2><form action="basket.php?action=update" method="post" id="cart">';	
		$output[] = '<table id="basket"><tr><th></th><th>Item</th><th>Price</th><th>Qty</th><th>Subtotal</th></tr>';
		foreach ($contents as $id=>$qty) {
			$sql = 'SELECT * FROM SHOP2_products WHERE id = '.$id;
			$result = $db->query($sql);
			$row = $result->fetch();
			extract($row);
					
			// DISCOUNT FUNCTION
			if ($qty >= 10 && $qty < 20)
				{
				if (!$dis10 || $dis10 == 0 || $dis10 == $price)
					{
					$product_price = $price;
					$discount = "";
					}
				else
					{
					$product_price = $dis10;
					$discount = "<span style='text-decoration:line-through;color:#f00;'>&pound;$price</span>";
					}
				}
			elseif ($qty >= 20 && $qty < 50)
				{
				if (!$dis20 || $dis20 == 0 || $dis20 == $price)
					{
					$product_price = $price;
					$discount = "";
					}
				else
					{
					$product_price = $dis20;
					$discount = "<span style='text-decoration:line-through;color:#f00;'>&pound;$price</span>";
					}
				}
			elseif ($qty >= 50 && $qty  < 100)
				{
				if (!$dis50 || $dis50 == 0 || $dis50 == $price)
					{
					$product_price = $price;
					$discount = "";
					}
				else
					{
					$product_price = $dis50;
					$discount = "<span style='text-decoration:line-through;color:#f00;'>&pound;$price</span>";
					}
				}
			elseif ($qty >= 100)
				{
				if (!$dis50 || $dis50 == 0 || $dis50 == $price)
					{
					$product_price = $price;
					$discount = "";
					$call = "<tr class=\"total\"><td colspan=\"5\"><strong>Please call us on 01745 828429 for bulk order discounts</strong></td></tr>";
					$quantity = "100";
					}
				else
					{
					$product_price = $dis50;
					$discount = "<span style='text-decoration:line-through;color:#f00;'>&pound;$price</span>";
					$call = "<tr class=\"total\"><td colspan=\"5\"><strong>Please call us on 01745 828429 for bulk order discounts</strong></td></tr>";
					$quantity = "100";
					}
				}
			else
				{
				$product_price = $price;
				$discount = "";
				}
				
			
			$i = 1;
			while ($i <= $qty)
				{
				$_SESSION['cartdetail'] .= " $id | $product_price,";
				$i++;
				}
			
			$img_query = mysql_query("select * from SHOP2_gallery where doc_cat = '$id'");
			$img_result = mysql_fetch_array($img_query);
			$img_rows  = mysql_num_rows($img_query);
			if ($img_rows > 0)
				{
				$thumbnail = "<img src='assets/images/products/small/$img_result[name]' alt='An image of $name' class='basket_img'/>";
				}
			else	
				{
				$thumbnail = "<img src='assets/images/products/no_small.gif' alt='No image available' class='basket_img'/>";
				}
			
			// PULL IN SEO PAGES
		if ($seo)	
			{
			$page_link = "products/".$seo;
			}
		else
			{
			$page_link = "product_info.php?product_id=".$id;
			}
			
			$output[] = '<tr>';
			$output[] = '<td width="100"><small><a href="basket.php?action=delete&id='.$id.'" title="Remove this item from your basket" class="remove">Remove</a></small></td>';
			$output[] = '<td><a href="'.$page_link.'">'.$thumbnail.''.$name.' '.$att_value.'</a><br/><small><em>'.$code.'</em></small></td>';
			$output[] = '<td>&pound;'.$product_price.' '.$discount.'</td>';
			$output[] = '<td><input type="text" name="qty'.$id.'" value="'.$qty.'" size="2" maxlength="3" /></td>';
			$output[] = '<td width="100">&pound;'.number_format(($product_price * $qty), 2, '.', '').'</td>';
			$total += $product_price * $qty;
			$output[] = '</tr>';
		}
		
		$output[]	= $call;
		
		$output[] = '<tr class="total"><td colspan="5">Total: <strong>&pound;'.number_format($total, 2, '.', '').'</strong></td></tr></table>';
		
		if ($quantity < "100")
			{
		
		$output[] = '<div class="info_alert"><p>If you change the quantity of any item in your basket please remember to click \'Update basket\' <strong>before</strong> you go to \'Checkout\'.</p></div>';
		}
		else
			{
			$output[] = '<div class="info_alert"><p>Please make sure that you have not entered a quantity of over 99 as we cannot process these online. Please call us for bulk discounts.</p></div>';
			}
		$output[] = '<p class="basket_button"><a href="products.php?cat='.$_GET['cat'].'" title="Click here to go back to product list"><img src="assets/images/buttons/back_2_products.jpg" alt="Back to product list"/></a> <input name="submit" type="image" value="Submit" src="assets/images/buttons/update.jpg" alt="Update Basket" /> ';
		
		if ($quantity < "100")
			{
			$output[] = ' <a href="checkout.php" title="Click here to proceed to the checkout"><img src="assets/images/buttons/checkout.gif" alt="Proceed to Checkout"/></a>';
			}
			$output[] = '</p></form>';
		
	} else {
		$output[] = '<img src="assets/images/icons/basket_empty.gif" alt="" class="imgright"/><h2>Shopping basket</h2><p>Your shopping basket is empty</p>';
	}
	return join('',$output); 
}

//==========================================================================
//CONFIRM CART - checkout.php
//==========================================================================

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
			$sql = 'SELECT * FROM SHOP2_products WHERE id = '.$id;
			$result = $db->query($sql);
			$row = $result->fetch();
			extract($row);
			
			// DISCOUNT FUNCTION
			if ($qty >= 10 && $qty < 20)
				{
				if (!$dis10 || $dis10 == 0 || $dis10 == $price)
					{
					$product_price = $price;
					$discount = "";
					}
				else
					{
					$product_price = $dis10;
					$discount = "<span style='text-decoration:line-through;color:#f00;'>&pound;$price</span>";
					}
				}
			elseif ($qty >= 20 && $qty < 50)
				{
				if (!$dis20 || $dis20 == 0 || $dis20 == $price)
					{
					$product_price = $price;
					$discount = "";
					}
				else
					{
					$product_price = $dis20;
					$discount = "<span style='text-decoration:line-through;color:#f00;'>&pound;$price</span>";
					}
				}
			elseif ($qty >= 50)
				{
				if (!$dis50 || $dis50 == 0 || $dis50 == $price)
					{
					$product_price = $price;
					$discount = "";
					}
				else
					{
					$product_price = $dis50;
					$discount = "<span style='text-decoration:line-through;color:#f00;'>&pound;$price</span>";
					}
				}
			else
				{
				$product_price = $price;
				$discount = "";
				}
			
			$img_query = mysql_query("select * from SHOP2_gallery where doc_cat = '$id'");
			$img_result = mysql_fetch_array($img_query);
			$img_rows  = mysql_num_rows($img_query);
			if ($img_rows > 0)
				{
				$thumbnail = "<img src='assets/images/products/small/$img_result[name]' alt='An image of $name' class='basket_img'/>";
				}
			else	
				{
				$thumbnail = "<img src='assets/images/products/no_small.gif' alt='No image available' class='basket_img'/>";
				}
			
			// PULL IN SEO PAGES
		if ($seo)	
			{
			$page_link = "products/".$seo;
			}
		else
			{
			$page_link = "product_info.php?product_id=".$id;
			}
			
			$output[] = '<tr>';
			$output[] = '<td><a href="'.$page_link.'">'.$thumbnail.''.$name.' '.$att_value.'</a><br/><small><em>'.$code.'</em></small></td>';
			$output[] = '<td>&pound;'.$product_price.' '.$discount.'</td>';
			$output[] = '<td>'.$qty.'</td>';
			$output[] = '<td>&pound;'.number_format(($product_price * $qty), 2, '.', '').'</td>';
			$total += $product_price * $qty;
			$output[] = '</tr>';
		}
		$exvat = $total / 1.2;
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
		$output[] = "<tr class='subtotal'><td colspan='4' align='right'>VAT (20%): <strong>&pound;".number_format($vat, 2, '.', '')."</strong></td></tr>";
		
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
			$sql = 'SELECT * FROM SHOP2_products WHERE id = '.$id;
			$result = $db->query($sql);
			$row = $result->fetch();
			extract($row);
			
			// DISCOUNT FUNCTION
			if ($qty >= 10 && $qty < 20)
				{
				if (!$dis10 || $dis10 == 0 || $dis10 == $price)
					{
					$product_price = $price;
					$discount = "";
					}
				else
					{
					$product_price = $dis10;
					$discount = "<span style='text-decoration:line-through;color:#f00;'>&pound;$price</span>";
					}
				}
			elseif ($qty >= 20 && $qty < 50)
				{
				if (!$dis20 || $dis20 == 0 || $dis20 == $price)
					{
					$product_price = $price;
					$discount = "";
					}
				else
					{
					$product_price = $dis20;
					$discount = "<span style='text-decoration:line-through;color:#f00;'>&pound;$price</span>";
					}
				}
			elseif ($qty >= 50)
				{
				if (!$dis50 || $dis50 == 0 || $dis50 == $price)
					{
					$product_price = $price;
					$discount = "";
					}
				else
					{
					$product_price = $dis50;
					$discount = "<span style='text-decoration:line-through;color:#f00;'>&pound;$price</span>";
					}
				}
			else
				{
				$product_price = $price;
				$discount = "";
				}
			
			
			$output[] = '<tr>';
			$output[] = '<td>'.$name.' '.$att_value.'<br/><small><em>'.$code.'</em></small></td>';
			$output[] = '<td>&pound;'.$product_price.' '.$discount.'</td>';
			$output[] = '<td>'.$qty.'</td>';
			$output[] = '<td>&pound;'.number_format(($product_price * $qty), 2, '.', '').'</td>';
			$total += $product_price * $qty;
			$output[] = '</tr>';
		}
	
		$output[] = "<tr class='subtotal'><td colspan='4' align='right'>Subtotal of items: <strong>&pound;".number_format($total, 2, '.', '')."</strong></td></tr>";
		$total += $_POST[deliverymethod];		
				$output[] = "<tr class='postage'><td colspan='4' align='right'>Postage and Packaging: <strong>&pound;".number_format($_POST[deliverymethod], 2, '.', '')."</strong></td></tr>";
				$exvat = $total / 1.2;
		$vat = $total - $exvat;
		$output[] = "<tr class='subtotal'><td colspan='4' align='right'>VAT (20%): <strong>&pound;".number_format($vat, 2, '.', '')."</strong></td></tr>";
				$output[] = "<tr class='total'><td colspan='4'>Total: <strong>&pound;".number_format($total, 2, '.', '')."</strong></td></tr></table>";
	} 
	return join('',$output);
}

//==========================================================================
//GET RID OF LOTS OF SLASHES
//==========================================================================
function slashes($string)
	{
	$string = stripslashes(stripslashes(stripslashes(stripslashes(stripslashes(stripslashes($string))))));
	return $string;
	}

//==========================================================================
//FIND PAGE URL
//==========================================================================
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