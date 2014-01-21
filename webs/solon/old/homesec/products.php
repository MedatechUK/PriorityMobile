<?php
session_start();

//IF THE USER IS RETURNING FROM A COMPLETE ORDER CLEAR SESSION
if ($_GET[clear] == "all")
	{
	session_destroy();
	header ("location: index.php");
	}

require_once('assets/widgets/mysql.class.php');
require_once('assets/widgets/global.inc.php');
require_once('assets/widgets/functions.inc.php');
require("assets/widgets/global_variables.php");
require_once('assets/widgets/maintenance.php');	

?>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<?php 
if (!$_GET[cat])
	{
	$page_query 	= mysql_query("select * from $content_table where id='14'");
	$page_result 	= mysql_fetch_array($page_query);
	$page_type		= "products";
	}
else
	{
	$page_query 	= mysql_query("select * from $category_table where id = '$_GET[cat]'");
	$page_result 	= mysql_fetch_array($page_query);
	$page_type		= "products";
	}
require("assets/widgets/meta.php"); 

?>
</head>
<body>
<?php 
echo("$js_notice");
?>
<?php require("assets/widgets/hidden.php"); ?>
<div id="container">
  <?php require("assets/widgets/header.php"); ?>
  <?php require("assets/widgets/nav.php"); ?>
  <div id="left_col">
  <?php require("assets/widgets/categories.php"); ?>
 </div>
  <div id="main_content">
    <?php 
	echo("$site_notice");
	?>
    
    <?php
	
	if ($_GET[cat])
		{
	 
	$catlist_query = mysql_query("select * from $category_table where id = '$_GET[cat]'");
	while ($catlist_result = mysql_fetch_array($catlist_query))
		{
		
		$count = -1;
		$cat_items = explode(',',$catlist_result[products]);
		$cat_contents = array();
		
		// NEED TO EXPLODE THE ARRAY AND QUERY THE DATABASE TO GET THE PRODUCTS IN A SPECIFIC ORDER
		
		
		foreach ($cat_items as $cat_item) {
			$cat_contents[$cat_item] = (isset($cat_contents[$cat_item])) ? $cat_contents[$cat_item] + 1 : 1;
			$count++;
		}
		
		$img_query = mysql_query("select * from $gallery_table where doc_cat = 'CAT$catlist_result[id]'");
		$img_result = mysql_fetch_array($img_query);
		$img_rows  = mysql_num_rows($img_query);
			if ($img_rows > 0)
				{
				$thumbnail = "<img src='assets/images/products/categories/$img_result[name]' alt='An image of $catlist_result[name]' class='imgright'/>";
				}
		
		
		echo ("<div class='cat_text'>$thumbnail $catlist_result[cat_info]</div>");
		 	$product_order = array();
	
		
		foreach ($cat_contents as $cat_id=>$cat_qty) 
			{
			$query = mysql_query("select * from $product_table where id = '".$cat_id."' and product_active != 'NO'");
			while ($result = mysql_fetch_array($query))
				{
				$product_order["".$result[id].""] = $result[name];
				}

			}
		


asort($product_order); 
		
foreach ($product_order as $cat_id=>$cat_qty) {
	
	$query = mysql_query("select * from $product_table where id = '".$cat_id."' and product_active != 'NO'");
	while ($result = mysql_fetch_array($query))
		{
		$img_query = mysql_query("select * from $gallery_table where doc_cat = '$result[id]'");
		$img_result = mysql_fetch_array($img_query);
		$img_rows  = mysql_num_rows($img_query);
			if ($img_rows > 0)
				{
				$thumbnail = "<img src='assets/images/products/thumbs/$img_result[name]' alt='An image of $result[name]' class='product_img'/>";
				}
			else	
				{
				$thumbnail = "<img src='assets/images/products/no_thumb.gif' alt='No image available' class='product_img'/>";
				}
		
		// PULL IN SEO PAGES
		if ($result[seo])	
			{
			$page_link = "products/".$result[seo]."?cat=".$_GET['cat'];
			}
		else
			{
			$page_link = "product_info.php?product_id=".$result[id]."&amp;cat=".$_GET['cat'];
			}
		
		
		echo("<div class='product'>
		<div class='top_info'>
		<a href='$page_link' title='View more information about $result[name]'>$thumbnail</a>
		<h3>$result[name]</h3>
		
		<form action='basket.php?action=add&amp;cat=".$_GET['cat']."' method='post'>
		
		");
		
		$att_query = mysql_query("select * from $product_table where sub = '$result[id]' and stock != 'NO' and product_active != 'NO'");
		$num_rows = mysql_num_rows($att_query);
		
		if ($num_rows > 0)
			{
					
			echo("<p class='price'>&pound;$result[price]</p>");
			echo("<p class='view'><a href='".$page_link."' title='View more information about $result[name]'><img src='assets/images/buttons/buy_now.gif' alt='View more information about $result[name]'/></a></p></form>");
			}
		else
			{
			echo("<p class='price'>&pound;$result[price]</p><input type='hidden' name='id' value='$result[id]'/>");
			
			if ($result[stock] == "NO") // IF THERE NEEDS TO BE AN OUT OF STOCK NOTICE CHANCE HERE
			{
			echo("<p class='view'><a href='".$page_link."' title='View more information about $result[name]'><img src='assets/images/buttons/buy_now.gif' alt='View more information about $result[name]'/></a></p></form>");
			}
		else
			{
			echo('<p class="view"><input name="submit" type="image" value="Submit" src="assets/images/buttons/buy_now.gif" alt="Add to Basket" /></p></form>');
			}
			}
				
		echo("<p class='info'><a href='$page_link' title='View more information about $result[name]'>More information</a></p></div><div class='description'>$result[summary]</div></div>");
		
		}
	}	
	}
	}
	else
	{
	$catlist_query = mysql_query("select * from $category_table order by list_order asc");
	while ($catlist_result = mysql_fetch_array($catlist_query))
		{
		echo("<div class='category_item'><h3><a href='products.php?cat=$catlist_result[id]' title='Click here to view products in this category $catlist_result[name]'>$catlist_result[name]</a></h3>");
		$img_query = mysql_query("select * from $gallery_table where doc_cat = 'CAT$catlist_result[id]'");
		$img_result = mysql_fetch_array($img_query);
		$img_rows  = mysql_num_rows($img_query);
			if ($img_rows > 0)
				{
				$thumbnail = "<img src='assets/images/products/thumbs/$img_result[name]' alt='An image of $catlist_result[name]'/>";
				}
			else	
				{
				$thumbnail = "<img src='assets/images/products/no_thumb.gif' alt='No image available'/>";
				}
		echo("<a href='products.php?cat=$catlist_result[id]' title='Click here to view products in this category $catlist_result[name]'>$thumbnail</a>");

		$count = -1;
		$cat_items = explode(',',$catlist_result[products]);
		$cat_contents = array();
		foreach ($cat_items as $cat_item) {
			$cat_contents[$cat_item] = (isset($cat_contents[$cat_item])) ? $cat_contents[$cat_item] + 1 : 1;
			$count++;
		}
		
		if ($count == 1)
			{
			echo("<p>$count product</p>");
			}
		else
			{
			//$count = $count - 1;
			echo("<p>$count products</p>");
			}

		echo("</div>");
		}
	}
	
	?>
	<p class="clear"><small></small></p>
  </div>
  <div id="footer">
    <?php 
	// THIS PULLS IN THE FOOTER
	require("assets/widgets/footer.php"); 
	?>
  </div>
</div>
<?php require("assets/widgets/google.php"); ?>
</body>
</html>
