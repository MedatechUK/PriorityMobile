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
$page_query = mysql_query("select * from $content_table where id='9'");
while($page_result = mysql_fetch_array($page_query))
	{

?>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<?php require("assets/widgets/meta.php"); ?>
<link rel="stylesheet" href="assets/css/sitemap.css" type="text/css" media="screen" />
<script type="text/javascript" src="assets/scripts/treemenu.js"></script>
</head>
<body>
<?php 
echo("$js_notice");
require("assets/widgets/hidden.php"); 
?>
<div id="container">
  <?php require("assets/widgets/header.php"); ?>
  <?php require("assets/widgets/nav.php"); ?>
  <div id="left_col">
  <?php require("assets/widgets/categories.php"); ?>
 </div>
  <div id="main_content">
    
		
	<?php 
	echo("$site_notice");
	echo("$page_result[content]"); 
	echo("<p><strong>Product catalog</strong> <small><a href=\"javascript:ddtreemenu.flatten('treemenu2', 'expand')\">Expand all</a> | <a href=\"javascript:ddtreemenu.flatten('treemenu2', 'contact')\">Hide all</a></small></p><p><small>Click on the folder icon to expand the section</small></p>");
	echo("<ul id=\"treemenu2\" class=\"treeview\">\n");
	
	$catlist_query = mysql_query("select * from $category_table order by name asc");
	while ($catlist_result = mysql_fetch_array($catlist_query))
		{
		echo("<li>$catlist_result[name]<ul>\n");
		$cat_items = explode(',',$catlist_result[products]);
		$cat_contents = array();
		foreach ($cat_items as $cat_item) {
			$cat_contents[$cat_item] = (isset($cat_contents[$cat_item])) ? $cat_contents[$cat_item] + 1 : 1;
		}
		
		$product_order = array();
		foreach ($cat_contents as $cat_id=>$cat_qty) 
			{
			$query = mysql_query("select * from $product_table where id = '".$cat_id."'");
			while ($result = mysql_fetch_array($query))
				{
				$product_order["".$result[id].""] = $result[name];
				}
			}
		


asort($product_order); 
		
foreach ($product_order as $cat_id=>$cat_qty) {
	
	$query = mysql_query("select * from $product_table where id = '".$cat_id."'");
	while ($result = mysql_fetch_array($query))
		{
		
		// PULL IN SEO PAGES
		if ($result[seo])	
			{
			$page_link = "products/".$result[seo]."?cat=".$catlist_result[id];
			}
		else
			{
			$page_link = "product_info.php?product_id=".$result[id]."&amp;cat=".$catlist_result[id];
			}
			
			$summary = str_replace("<ul>", "", $result[summary]);
			$summary = str_replace("</ul>", "", $summary);
			$summary = str_replace("<li>", "", $summary);
			$summary = str_replace("<br/>", "", $summary);
			$summary = str_replace("<br />", "", $summary);
			$summary = str_replace("</li>", ", ", $summary);
			$summary = str_replace("<p>", "", $summary);
			$summary = str_replace("</p>", ", ", $summary);
		
		echo("<li><a href='".$page_link."' title='$summary'>$result[name]</a></li>\n");
		
		}
		
		
	}
	echo("</ul>\n");	
	echo("</li>\n");
	}
	
	
	echo("</ul>\n");
	?>	
	<script type="text/javascript">

//ddtreemenu.createTree(treeid, enablepersist, opt_persist_in_days (default is 1))

ddtreemenu.createTree("treemenu1", true)
ddtreemenu.createTree("treemenu2", true)
</script>
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