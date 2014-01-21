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
	echo("<div class='home_text'>$page_result[content]</div>"); 
	
	
	?>
	<p class="tip">Hover over areas of the house and click to see products relating to that area</p>
	<ul id="house">
		<li id="cat1"><a href="products.php?cat=3"><img src="assets/images/clear.gif" alt="" border="0"/><span>Security products for all types of doors.</span></a></li>
		<li id="cat2"><a href="products.php?cat=2"><img src="assets/images/clear.gif" alt="" border="0"/><span>Alarms for doors and windows.</span></a></li>
		<li id="cat3"><a href="products.php?cat=1"><img src="assets/images/clear.gif" alt="" border="0"/><span>Locks and security devices for all types of windows.</span></a></li>
		<li id="cat4"><a href="products.php?cat=19"><img src="assets/images/clear.gif" alt="" border="0"/><span>CCTV systems, cameras, dummy cameras.</span></a></li>
		<li id="cat5"><a href="products.php?cat=1"><img src="assets/images/clear.gif" alt="" border="0"/><span>Locks and security devices for all types of windows.</span></a></li>
		<li id="cat6"><a href="products.php?cat=1"><img src="assets/images/clear.gif" alt="" border="0"/><span>Locks and security devices for all types of windows.</span></a></li>
		<li id="cat7"><a href="products.php?cat=19"><img src="assets/images/clear.gif" alt="" border="0"/><span>CCTV systems, cameras, dummy cameras.</span></a></li>
		<li id="cat8"><a href="products.php?cat=20"><img src="assets/images/clear.gif" alt="" border="0"/><span>Products to protect your property from intruders</span></a></li>
		<li id="cat9"><a href="products.php?cat=20"><img src="assets/images/clear.gif" alt="" border="0"/><span>Products to protect your property from intruders</span></a></li>
	</ul>
	
  </div>
  <div id="product_footer">
  
  <?php
  $bal = 1;
  $cat_query = mysql_query("select * from $category_table order by list_order asc limit 6");
	while ($cat_result = mysql_fetch_array($cat_query))
		{
		$balloon = "";
		echo("<ul>");
		echo("<li><a href='products.php?cat=$cat_result[id]'>$cat_result[name]</a></li>");
		
		$cat_items = explode(',',$cat_result[products]);
		$cat_contents = array();
		
		foreach ($cat_items as $cat_item) {
			$cat_contents[$cat_item] = (isset($cat_contents[$cat_item])) ? $cat_contents[$cat_item] + 1 : 1;
		}
		
		$count = 1;
		//$cat_contents = shuffle($cat_contents);
		foreach ($cat_contents as $cat_id=>$cat_qty) {
	
	if ($count <= 4)
		{
		$query = mysql_query("select * from $product_table where id = '".$cat_id."'");
		while ($result = mysql_fetch_array($query))
			{
			
			$img_query = mysql_query("select * from $gallery_table where doc_cat = '$result[id]'");
			$img_result = mysql_fetch_array($img_query);
			$img_rows  = mysql_num_rows($img_query);
			if ($img_rows > 0)
				{
				$thumbnail = "<img src='assets/images/products/small/$img_result[name]' alt='An image of $result[name]' class='product_img'/>";
				}
			else	
				{
				$thumbnail = "<img src='assets/images/products/no_small.gif' alt='No image available' class='product_img'/>";
				}
				
			if ($result[seo])	
			{
			$page_link = "products/".$result[seo]."?cat=".$cat_result['id'];
			}
		else
			{
			$page_link = "product_info.php?product_id=".$result[id]."&amp;cat=".$cat_result['id'];
			}
			
			echo("<li class='product_link'><a href='".$page_link."' title='$result[description]' rel='balloon$bal'>$result[name]</a></li>");
			$count++;
			
			$summary = str_replace("<ul>", "<p>", $result[summary]);
			$summary = str_replace("</ul>", "</p>", $summary);
			$summary = str_replace("<li>", "", $summary);
			$summary = str_replace("<br/>", "", $summary);
			$summary = str_replace("<br />", "", $summary);
			$summary = str_replace("</li>", ".<br/>", $summary);
			
			$balloon .= "<div id='balloon$bal' class='balloonstyle'>$thumbnail<p><strong>Info:</strong></p>$summary<p class='price'>&pound;$result[price]</p></div>";
			$bal++;
			}
			
			
		}
		}
		
		echo("</ul>\n\n");
		echo ("$balloon");
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
