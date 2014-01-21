<?php

$products = array();
  $pop_query = mysql_query("select * from $product_table order by id asc");
	while ($pop_result = mysql_fetch_array($pop_query))
		{
		$data_query = mysql_query("select * from $report_products_table where product_id = '$pop_result[id]'");
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
		$top_product = array_keys($products);
		$pop_query = mysql_query("select * from $product_table where id = '$top_product[0]'");
		$pop_result = mysql_fetch_array($pop_query);
		
		if ($pop_result[sub])
			{
			$pop_query = mysql_query("select * from $product_table where id = '$pop_result[sub]'");
			$pop_result = mysql_fetch_array($pop_query);
			}
		
		$img_query = mysql_query("select * from $gallery_table where doc_cat = '$pop_result[id]'");
		$img_result = mysql_fetch_array($img_query);
		$img_rows  = mysql_num_rows($img_query);
			if ($img_rows > 0)
				{
				$thumbnail = "<img src='assets/images/products/small/$img_result[name]' alt='An image of $pop_result[name]' class='product_img'/>";
				}
			else	
				{
				$thumbnail = "<img src='assets/images/products/no_small.gif' alt='No image available' class='product_img'/>";
				}
		
		// PULL IN SEO PAGES
		if ($result[seo])	
			{
			$page_link = "products/".$pop_result[seo]."?cat=".$_GET['cat'];
			}
		else
			{
			$page_link = "product_info.php?product_id=".$pop_result[id];
			}
		
		
		echo("<div class='best_seller'>
		<h4>BEST SELLERS</h4>
		<p class='bs_img'><a href='$page_link' title='View more information about $pop_result[name]'>$thumbnail</a></p>
		<p class='bs_name'><strong>$pop_result[name]</strong></p>
		
		<p class='bs_price'>&pound;$pop_result[price]</p>
		<p class='bs_link'><a href='$page_link' title='View more information about $pop_result[name]'>More information</a></p>");
		$pop_query = mysql_query("select * from $product_table where id = '$top_product[1]'");
		$pop_result = mysql_fetch_array($pop_query);
		
		if ($pop_result[sub])
			{
			$pop_query = mysql_query("select * from $product_table where id = '$pop_result[sub]'");
			$pop_result = mysql_fetch_array($pop_query);
			}
		
		$img_query = mysql_query("select * from $gallery_table where doc_cat = '$pop_result[id]'");
		$img_result = mysql_fetch_array($img_query);
		$img_rows  = mysql_num_rows($img_query);
			if ($img_rows > 0)
				{
				$thumbnail = "<img src='assets/images/products/small/$img_result[name]' alt='An image of $pop_result[name]' class='product_img'/>";
				}
			else	
				{
				$thumbnail = "<img src='assets/images/products/no_small.gif' alt='No image available' class='product_img'/>";
				}
		
		// PULL IN SEO PAGES
		if ($result[seo])	
			{
			$page_link = "products/".$pop_result[seo]."?cat=".$_GET['cat'];
			}
		else
			{
			$page_link = "product_info.php?product_id=".$pop_result[id];
			}
		
		
		echo("
		<p class='bs_img'><a href='$page_link' title='View more information about $pop_result[name]'>$thumbnail</a></p>
		<p class='bs_name'><strong>$pop_result[name]</strong></p>
		
		<p class='bs_price'>&pound;$pop_result[price]</p>
		<p class='bs_link'><a href='$page_link' title='View more information about $pop_result[name]'>More information</a></p>");
		$pop_query = mysql_query("select * from $product_table where id = '$top_product[2]'");
		$pop_result = mysql_fetch_array($pop_query);
		
		if ($pop_result[sub])
			{
			$pop_query = mysql_query("select * from $product_table where id = '$pop_result[sub]'");
			$pop_result = mysql_fetch_array($pop_query);
			}
		
		$img_query = mysql_query("select * from $gallery_table where doc_cat = '$pop_result[id]'");
		$img_result = mysql_fetch_array($img_query);
		$img_rows  = mysql_num_rows($img_query);
			if ($img_rows > 0)
				{
				$thumbnail = "<img src='assets/images/products/small/$img_result[name]' alt='An image of $pop_result[name]' class='product_img'/>";
				}
			else	
				{
				$thumbnail = "<img src='assets/images/products/no_small.gif' alt='No image available' class='product_img'/>";
				}
		
		// PULL IN SEO PAGES
		if ($result[seo])	
			{
			$page_link = "products/".$pop_result[seo]."?cat=".$_GET['cat'];
			}
		else
			{
			$page_link = "product_info.php?product_id=".$pop_result[id];
			}
		
		
		echo("
		<p class='bs_img'><a href='$page_link' title='View more information about $pop_result[name]'>$thumbnail</a></p>
		<p class='bs_name'><strong>$pop_result[name]</strong></p>
		
		<p class='bs_price'>&pound;$pop_result[price]</p>
		<p class='bs_link'><a href='$page_link' title='View more information about $pop_result[name]'>More information</a></p>
			</div>");
?>