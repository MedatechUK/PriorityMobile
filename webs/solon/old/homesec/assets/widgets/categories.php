<?php

$thiscat = $_GET[cat];

echo("<h3>Categories</h3><ul>");
	$cat_query = mysql_query("select * from $category_table order by list_order asc");
	while ($cat_result = mysql_fetch_array($cat_query))
		{
		if ($thiscat == $cat_result[id])
			{
			$cat_class = " class='active_cat'";
			}
		else	
			{
			$cat_class = "";
			}
		
		echo("<li$cat_class><a href='/products.php?cat=$cat_result[id]'>$cat_result[name]</a></li>");
		}
	echo("</ul>");


?>

<?php echo writeShoppingCart(); ?>