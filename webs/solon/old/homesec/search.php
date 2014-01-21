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
require_once('assets/widgets/global_variables.php');
require_once('assets/widgets/maintenance.php');
$page_query = mysql_query("select * from $content_table where id='1'");
while($page_result = mysql_fetch_array($page_query))
	{

?>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<?php require("assets/widgets/meta.php"); ?>
<link rel="stylesheet" href="assets/css/house.css" type="text/css" media="screen" />
<link rel="stylesheet" href="assets/css/tooltip.css" type="text/css" media="screen" />
<script type="text/javascript" src="assets/scripts/tooltip.js" language="javascript"></script>
</head>
<body>
<?php echo("$js_notice"); ?>
<?php require("assets/widgets/hidden.php"); ?>
<div id="container">
  <?php require("assets/widgets/header.php"); ?>
  <?php require("assets/widgets/nav.php"); ?>
  <div id="left_col">
  <?php require("assets/widgets/categories.php"); ?>
  <?php require("assets/widgets/best_sellers.php"); ?>
 </div>
  <div id="main_content">
	<?php 
	echo("$site_notice");
	?>
	<?

$search_term 		=  $_GET[search];
$search_length 		=  strlen($search_term);

if ($search_length <= 2)
	{
	echo("<h2>Sorry there was a problem</h2><p>Please ensure that you enter a search longer than 2 characters long. This will help you find the most suitable products</p>");
	
	}
else
	{
	echo("<h2>Search results</h2>");
	
	
	
	$search_query = mysql_query("select * from $product_table WHERE (name LIKE '%$search_term%' || summary LIKE '%$search_term%' || code LIKE '%$search_term%' || text LIKE '%$search_term%' || keywords LIKE '%$search_term%' || description LIKE '%$search_term%' || search_terms LIKE '%$search_term%') and sub is NULL and product_active != 'NO' ORDER BY name asc");
	
	$search_time = date(YmdHis);
	$search_rows = mysql_num_rows($search_query);
	$search_track = mysql_query("insert into $search_table (session_id, search_term, date, ip, products) values ('$session_id', '$search_term', '$search_time', '$refip', '$search_rows')");
	
	
	
	if ($search_rows > 0)
		{
		if ($search_rows > 1)
			{
			$s = "s";
			}
		
		echo("<p>We found $search_rows result$s for the search term '<i>$search_term</i>':</p>");
		
		
		
		while($search_result = mysql_fetch_array($search_query))
			{			
			$img_query = mysql_query("select * from $gallery_table where doc_cat = '$search_result[id]'");
			$img_result = mysql_fetch_array($img_query);
			$img_rows  = mysql_num_rows($img_query);
			if ($img_rows > 0)
				{
				$thumbnail = "<img src='assets/images/products/small/$img_result[name]' alt='An image of $search_result[name]' class='product_img'/>";
				}
			else	
				{
				$thumbnail = "<img src='assets/images/products/no_small.gif' alt='No image available' class='product_img'/>";
				}
		
		// PULL IN SEO PAGES
		if ($search_result[seo])	
			{
			$page_link = "products/".$search_result[seo]."?search_result=".$_GET['search'];
			}
		else
			{
			$page_link = "product_info.php?product_id=".$search_result[id]."&amp;search_result=".$_GET['search'];
			}
		
		
		echo("<div class='related_product'>
		<div class='top_info'>
		<a href='$page_link' title='View more information about $result[name]'>$thumbnail</a>
		<h3>$search_result[name]</h3>
		
		<form action='basket.php?action=add&amp;cat=".$_GET['cat']."' method='post'>
		
		");
		
		$att_query = mysql_query("select * from $product_table where sub = '$search_result[id]'");
		$num_rows = mysql_num_rows($att_query);
		echo("<p class='price'>&pound;$search_result[price]</p>");
		echo("<p class='view'><a href='".$page_link."' title='View more information about $search_result[name]'><img src='assets/images/buttons/view_small.gif' alt='View more information about $search_result[name]'/></a></p></form>");
		echo("<p class='info'><a href='$page_link' title='View more information about $search_result[name]'>More information</a></p></div></div>");
			}
		}
	else
		{
		echo("<p>We found no results for the search term '$search_term'</p>");
		}
	}
	?>
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
<?php } ?>
