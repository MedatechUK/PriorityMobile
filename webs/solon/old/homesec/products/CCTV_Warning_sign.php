<?php
session_start();

//IF THE USER IS RETURNING FROM A COMPLETE ORDER CLEAR SESSION
if ($_GET[clear] == "all")
	{
	session_destroy();
	}

require_once('../assets/widgets/mysql.class.php');
require_once('../assets/widgets/global.inc.php');
require_once('../assets/widgets/functions.inc.php');
require_once('../assets/widgets/global_variables.php');
require_once('../assets/widgets/maintenance.php');

?>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
<?php
$query = mysql_query("select * from $product_table where id = '170' and product_active != 'NO'");
$result = mysql_fetch_array($query);
?>	
<title><?php echo $result[name] ?></title>
<meta name="keywords" content="<?php echo $result[keywords] ?>" />
<meta name="description" content="<?php echo $result[description] ?>" />
<link rel="stylesheet" href="../assets/css/screen.css" type="text/css" media="screen" />
<link rel="stylesheet" href="../assets/css/print.css" type="text/css" media="print" />
<link rel="shortcut icon" href="../assets/images/favicon.ico" />
<script language="javascript">
function show(targetElement,contentElement) {
      var tarElement = document.getElementById(targetElement);
      var srcElement = document.getElementById(contentElement);
      
      if(tarElement != null && srcElement  != null) {
       tarElement.innerHTML=srcElement.innerHTML;
      }
  }
  </script>
</head>
<body>
<?php 
echo("$js_notice");
?>
  <?php require("../assets/widgets/hidden.php"); ?>
<div id="container">
  <?php require("../assets/widgets/header.php"); ?>
  <?php require("../assets/widgets/nav.php"); ?>
  <div id="left_col">
  <?php require("../assets/widgets/categories.php"); ?>
 </div>
  <div id="main_content">
   <?php 
	echo("$site_notice");
	
	$query = mysql_query("select * from $product_table where id = '170' and product_active != 'NO'");
	while ($result = mysql_fetch_array($query))
		{
		
		$img_query = mysql_query("select * from $gallery_table where doc_cat = '$result[id]'");
		$img_result = mysql_fetch_array($img_query);
		$img_rows  = mysql_num_rows($img_query);
			if ($img_rows > 0)
				{
				$medium_image = "<img src='../assets/images/products/medium/$img_result[name]' alt='An image of $result[name]'/>";
				}
			else	
				{
				$medium_image = "<img src='../assets/images/products/no_medium.gif' alt='No image available'/>";
				}
		
		echo("
		
		<div class='product_info'>
		<h2>$result[name]</h2>
		<div class='product_image'>$medium_image</div>
		<div id='change'>
		<p class='product_code'>Product code: <strong>$result[code]</strong></p>
		<p class='price'>&pound;$result[price] each</p>");
		
		if ($result[dis10] != $result[price])
			{
			echo("<p class='sub_price'>Buy 10+ for &pound;$result[dis10] each</p>");
			}
		if ($result[dis20] != $result[price])
			{
			echo("<p class='sub_price'>Buy 20+ for &pound;$result[dis20] each</p>");
			}
		if ( $result[dis50] != $result[price])
			{
			echo("<p class='sub_price'>Buy 50+ for &pound;$result[dis50] each</p>");
			}
		
		echo("</div>");
		echo("<form action='../basket.php?action=add&amp;cat=$_GET[cat]' method='post'>");
		
		$att_query = mysql_query("select * from $product_table where sub = '$result[id]' and stock != 'NO' and product_active != 'NO'");
		$num_rows = mysql_num_rows($att_query);
		
		if ($num_rows > 0)
			{
			echo("<p><label>$result[att_name]&nbsp;&nbsp;");
			echo('<select name="id" onchange="show(\'change\',\'p\'+this.value)">');
			echo("<option value='$result[id]'>$result[att_value] (&pound;$result[price])</option>");
			while ($att_result = mysql_fetch_array($att_query))
				{
				echo("<option value='$att_result[id]'>$att_result[att_value] (&pound;$att_result[price])</option>");
				}
			echo("</select></label></p>");
			
			}
		else
			{
			echo("<input type='hidden' name='id' value='$result[id]'/>");
			}
		
		
		if ($result[stock] == "NO")
			{
			echo('<p><strong class="out_of_stock">This product is currently out of stock.</strong></p>
			<p class="prod_button">
			<a href="/stock_update.php?product_id='.$result[id].'" title="Click here to get an alert when the '.$result[name].' is back in stock"><img src="/assets/images/buttons/out_of_stock_large.gif" alt="Out of stock"/></a><br/>');
			
			//IF THE SOURCE HAS COME FROM SEARCH RESULTS 
			if (!$_GET[search_result])
				{
				echo('<a href="/products.php?cat='.$_GET['cat'].'" title="Click here to go back to product list"><img src="/assets/images/buttons/back_2_products.jpg" alt="Back to product list"/></a>');
				}
			else
				{
				echo('<a href="/search.php?search='.$_GET[search_result].'" title="Click here to go back to search results"><img src="/assets/images/buttons/back_2_results.gif" alt="Back to search results"/></a>');
				}
			
			echo('</p>
			</form><div class="product_text">'.$result[text].'</div>');
			}
		else
			{
			echo('
			<p><label>Quantity: <input type="text" name="amount" value="1" maxlength="2" style="width: 50px"/></label></p>
			<p class="prod_button"><input name="submit" type="image" value="Submit" src="/assets/images/buttons/basket.jpg" alt="Add to Basket" /><br/>');
			
			//IF THE SOURCE HAS COME FROM SEARCH RESULTS 
			if (!$_GET[search_result])
				{
				echo('<a href="/products.php?cat='.$_GET['cat'].'" title="Click here to go back to product list"><img src="/assets/images/buttons/back_2_products.jpg" alt="Back to product list"/></a>');
				}
			else
				{
				echo('<a href="/search.php?search='.$_GET[search_result].'" title="Click here to go back to search results"><img src="/assets/images/buttons/back_2_results.gif" alt="Back to search results"/></a>');
				}
			
			echo('</p></form><div class="product_text">'.$result[text].'</div>');
			}
		
		
		echo("<p class='tell_a_friend'><a href='/tell_a_friend.php' title='Click here to tell a friend about this product'><img src='/assets/images/buttons/tell_a_friend.gif' alt='Back to search results'/></a></p></div>");
		
//==========================================================================
//RELATED PRODUCTS
//==========================================================================
		
		if ($result[related_products])
		{
		echo("<h3>People who bought this also bought</h3>");
		
		
		$count = -1;
		$cat_items = explode(',',$result[related_products]);
		$cat_contents = array();
		foreach ($cat_items as $cat_item) {
			$cat_contents[$cat_item] = (isset($cat_contents[$cat_item])) ? $cat_contents[$cat_item] + 1 : 1;
			$count++;
		}
		
		
		$product_order = array();
	
		
		foreach ($cat_contents as $cat_id=>$cat_qty) 
			{
			$query = mysql_query("select * from $product_table where id = '$cat_id'");
			while ($result = mysql_fetch_array($query))
				{
				$product_order["$result[id]"] = $result[name];
				}

			}
		


asort($product_order); 
		
foreach ($product_order as $cat_id=>$cat_qty) {
	
	$query = mysql_query("select * from $product_table where id = '$cat_id.' and product_active != 'NO'");
	while ($result = mysql_fetch_array($query))
		{
		$img_query = mysql_query("select * from $gallery_table where doc_cat = '$result[id]'");
		$img_result = mysql_fetch_array($img_query);
		$img_rows  = mysql_num_rows($img_query);
			if ($img_rows > 0)
				{
				$thumbnail = "<img src='/assets/images/products/small/$img_result[name]' alt='An image of $result[name]' class='product_img'/>";
				}
			else	
				{
				$thumbnail = "<img src='/assets/images/products/no_small.gif' alt='No image available' class='product_img'/>";
				}
		
		// PULL IN SEO PAGES
		if ($result[seo])	
			{
			$page_link = "/products/".$result[seo]."?cat=".$_GET['cat'];
			}
		else
			{
			$page_link = "/product_info.php?product_id=".$result[id]."&amp;cat=".$_GET['cat'];
			}
		
		
		echo("<div class='related_product'>
		<div class='top_info'>
		<a href='$page_link' title='View more information about $result[name]'>$thumbnail</a>
		<h3>$result[name]</h3>
		
		<form action='basket.php?action=add&amp;cat=".$_GET['cat']."' method='post'>
		
		");
		
		$att_query = mysql_query("select * from $product_table where sub = '$result[id]' and stock != 'NO' and product_active != 'NO'");
		$num_rows = mysql_num_rows($att_query);
		echo("<p class='price'>&pound;$result[price]</p>");
		echo("<p class='view'><a href='".$page_link."' title='View more information about $result[name]'><img src='/assets/images/buttons/view_small.gif' alt='View more information about $result[name]'/></a></p></form>");
			
			
		
		
		echo("<p class='info'><a href='$page_link' title='View more information about $result[name]'>More information</a></p></div></div>");
		
		}
	}	
		
}		


//HIDDEN PRICE VALUES
$query = mysql_query("select * from $product_table where id = '170' and product_active != 'NO'");
	while ($result = mysql_fetch_array($query))
		{
		echo("<div id='p$result[id]' style='display:none'>");
		echo("<p class='product_code'>Product code: <strong>$result[code]</strong></p>
		<p class='price'>&pound;$result[price] each</p>");
		
		if ($result[dis10] != $result[price])
			{
			echo("<p class='sub_price'>Buy 10+ for &pound;$result[dis10] each</p>");
			}
		if ($result[dis20] != $result[price])
			{
			echo("<p class='sub_price'>Buy 20+ for &pound;$result[dis20] each</p>");
			}
		if ( $result[dis50] != $result[price])
			{
			echo("<p class='sub_price'>Buy 50+ for &pound;$result[dis50] each</p>");
			}
		echo("</div>");
	}


// Sub products
$att_query = mysql_query("select * from $product_table where sub = '170' and product_active != 'NO'");
$num_rows = mysql_num_rows($att_query);

if ($num_rows > 0)
	{
	while ($att_result = mysql_fetch_array($att_query))
		{
		echo("<div id='p$att_result[id]' style='display:none'>");
		
		echo("<p class='product_code'>Product code: <strong>$att_result[code]</strong></p><p class='price'>&pound;$att_result[price] each</p>");
		
		if ($att_result[dis10] != $att_result[price])
			{
			echo("<p class='sub_price'>Buy 10+ for &pound;$att_result[dis10] each</p>");
			}
		if ($att_result[dis20] != $att_result[price])
			{
			echo("<p class='sub_price'>Buy 20+ for &pound;$att_result[dis20] each</p>");
			}
		if ( $att_result[dis50] != $att_result[price])
			{
			echo("<p class='sub_price'>Buy 50+ for &pound;$att_result[dis50] each</p>");
			}
		
		echo("</div>");
		}
	}
	
			
//==========================================================================
//SITE FOOTPRINTS
//==========================================================================
		$date_added = date("YmdHis");
		$ip = $_SERVER[REMOTE_ADDR];
		$process = mysql_query("insert into $report_popular_table (product, ip, visit) VALUES ('170', '$ip', '$date_added')");
		}
		
	?>
  </div>
  <div id="footer">
    <?php 
	// THIS PULLS IN THE FOOTER
	require("../assets/widgets/footer.php"); 
	?>
  </div>
</div>
<?php require("../assets/widgets/google.php"); ?>
</body>
</html>
