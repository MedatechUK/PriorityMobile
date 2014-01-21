<?php 
// SECURITY CHECK TO MAKE SURE THE CORRECT USERNAME AND PASSWORD HAS BEEN ENTERED
if(!$_COOKIE[user_name] | !$_COOKIE[user_pwd])
{
header("location: login.php");
exit;
}

require_once('../assets/widgets/mysql.class.php');
require_once('../assets/widgets/global.inc.php');
require_once('../assets/widgets/functions.inc.php');
require_once('../assets/widgets/global_variables.php');
include("FCKeditor/fckeditor.php"); 
?>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
<title>Admin Interface</title>
<link rel="shortcut icon" href="../assets/images/favicon.ico" />
<link rel="stylesheet" href="../assets/css/admin.css" type="text/css" media="screen" />
<link rel="stylesheet" href="../assets/css/autosuggest.css" type="text/css" media="screen" />
<script type="text/javascript" src="../assets/scripts/functions.js"></script>
<script type="text/javascript" src="../assets/scripts/switchcontent.js" ></script>
<script type="text/javascript" src="../assets/scripts/switchicon.js"></script>
</head>
<body>
<noscript>
<h1>Warning</h1>
<p class="noscript">To use this site correctly you need to have JavaScript enabled on your web browser</p>
</noscript>
<div id="hidden">
  <?php require("../assets/widgets/hidden.php"); ?>
</div>
<div id="container">
  <div id="header">
    <h2>Website</h2>
<p>Solon: 01352 762266<br />Sue: 01352 736117<br />The ITC: 01745 828440</p>
 </div>
  <div id="navigation">
    <?php 
	// THIS PULLS IN THE ADMIN LINKS
	require("widget_links.php"); 
	?>
  </div>
  <div id="main_content">
      <?php

	
//EDIT PAGE CONTENT
if($_GET[id])
	{
	
	$product_id = $_GET[id];
	
	
	$query = mysql_query("select * from $product_table where id='$product_id'");
	while($result = mysql_fetch_array($query))
	{
	if ($result[sub])
		{
		$subquery = mysql_query("select * from $product_table where id = '$result[sub]'");
		$subresult = mysql_fetch_array($subquery);
		}
?>

<h2>Edit Product</h2>
<p style="text-align:right"><small><a href="javascript:faq.sweepToggle('contract')">Contract All</a> | <a href="javascript:faq.sweepToggle('expand')">Expand All</a></small></p>
<form method="post" action="manage_products.php?edit=<?php echo("$result[id]"); ?>" enctype="multipart/form-data" id="imgform" onsubmit="expandCollapse('imgform','sending');<?php //echo("return false"); ?>">
<input name="id" type="hidden" value="<?php echo("$result[id]"); ?>"/>
<?php

if ($result[sub])
	{
	echo("<fieldset><legend>Parent Product</legend>");
	echo("<p><label><span>Parent product</span></label>$subresult[name]</p>");
	echo("<p><label><span>Parent code</span></label>$subresult[code]</p>");
	echo("</fieldset>");
	echo("<input name='parent_product' type='hidden' value='$result[sub]' />");
	}
?>
<fieldset>
<legend>Product Details</legend>
<?php

if ($result[sub])
	{
	echo("<p><label><span>Product Name</span></label>$subresult[name]&nbsp;</p>");
	echo("<input name='name' type='hidden' value='$subresult[name]' />");
	}
else
	{
	?>
<p><label for="name"><span>Product Name</span></label>
<input name="name" id="name" type="text" value="<?php echo("$result[name]"); ?>" size="50"/>
</p>
<?php
	}
?>
<p><label for="price"><span>Price</span></label>
<input name="price" id="price" type="text" value="<?php echo("$result[price]"); ?>"/>
</p>
<p><label for="dis10"><span>Price 10+</span></label>
<input name="dis10" id="dis10" type="text" value="<?php echo("$result[dis10]"); ?>"/>
</p>
<p><label for="dis20"><span>Price 20+</span></label>
<input name="dis20" id="dis20" type="text" value="<?php echo("$result[dis20]"); ?>"/>
</p>
<p><label for="dis50"><span>Price 50+</span></label>
<input name="dis50" id="dis50" type="text" value="<?php echo("$result[dis50]"); ?>"/>
</p>

<p><label for="code"><span>Product Code</span></label>
<input name="code" id="code" type="text" value="<?php echo("$result[code]"); ?>"/>
</p>

<p>Product Summary</p>
<?php
$oFCKeditor = new FCKeditor('summary') ;
$oFCKeditor->BasePath = 'FCKeditor/';
$oFCKeditor->Value = $result[summary];
$oFCKeditor->Width  = '400' ;
$oFCKeditor->Height = '150' ;
$oFCKeditor->ToolbarSet = 'summary';
$oFCKeditor->Create() ;
?>

<p>Product Information</p>
<?php
$oFCKeditor = new FCKeditor('text') ;
$oFCKeditor->BasePath = 'FCKeditor/';
$oFCKeditor->Value = $result[text];
$oFCKeditor->Width  = '670' ;
$oFCKeditor->Height = '350' ;
$oFCKeditor->ToolbarSet = 'Pete';
$oFCKeditor->Create() ;
?>
</fieldset>
<p style="text-align:right"><small><a href="javascript:faq.sweepToggle('contract')">Contract All</a> | <a href="javascript:faq.sweepToggle('expand')">Expand All</a></small></p>
<fieldset>
<legend><span id="faq1-title" class="iconspan"><img src="/assets/images/icons/minus.gif" alt="" /></span>&nbsp;&nbsp;&nbsp;Stock control</legend>
<div id="faq1" class="icongroup1">
<p><label for="stock">In stock</label></p>
<p>
 <label>
  <input type="radio" name="stock" value="YES"<?php if ($result[stock] != "NO"){ echo ("checked='checked'"); }?>/>
Yes</label>
  <br />
  <label>
  <input type="radio" name="stock" value="NO" <?php if ($result[stock] == "NO"){ echo ("checked='checked'"); }?>/>
No</label>
</p>
<p>
 <label>
  <input type="radio" name="product_active" value="YES"<?php if ($result[product_active] != "NO"){ echo ("checked='checked'"); }?>/>
Show on site</label>
  <br />
  <label>
  <input type="radio" name="product_active" value="NO" <?php if ($result[product_active] == "NO"){ echo ("checked='checked'"); }?>/>
Hide from site</label>
</p>
</div>
</fieldset>
<p style="text-align:right"><small><a href="javascript:faq.sweepToggle('contract')">Contract All</a> | <a href="javascript:faq.sweepToggle('expand')">Expand All</a></small></p>
<fieldset>
<legend><span id="faq2-title" class="iconspan"><img src="/assets/images/icons/minus.gif" alt="" /></span>&nbsp;&nbsp;&nbsp;Product Options</legend>
<div id="faq2" class="icongroup1">

<?php

if ($result[sub])
	{
	echo("<p><label><span>Option Name</span></label>$result[att_name]&nbsp;</p>");
	echo("<input name='att_name' type='hidden' value='$result[att_name]' />");
	}
else
	{
	?>
<p><label for="att_name"><span>Option Name</span></label>
<input name="att_name" id="att_name" type="text" value="<?php echo("$result[att_name]"); ?>"/>
</p>
<?php
	}
?>

<p><label for="att_value"><span>Option Value</span></label>
<input name="att_value" id="att_value" type="text" value="<?php echo("$result[att_value]"); ?>"/>
</p>
</div>
</fieldset>
<p style="text-align:right"><small><a href="javascript:faq.sweepToggle('contract')">Contract All</a> | <a href="javascript:faq.sweepToggle('expand')">Expand All</a></small></p>
<fieldset>
<legend><span id="faq3-title" class="iconspan"><img src="/assets/images/icons/minus.gif" alt="" /></span>&nbsp;&nbsp;&nbsp;Product Image</legend>
<div id="faq3" class="icongroup1">
<?php
$img_query = mysql_query("select * from $gallery_table where doc_cat = '$result[id]'");
$img_result = mysql_fetch_array($img_query);
$img_rows  = mysql_num_rows($img_query);
	if ($img_rows > 0)
		{
		echo "<p><img src='../assets/images/products/thumbs/$img_result[name]' alt='An image of $result[name]'/></p><p>If you wish to change the image you need to do this through the image manager</p>";
		}
	else	
		{
		?>
		<p><label for="filename"><span>Select File</span></label><input type="file" name="imagefile" id="filename"/></p>
		<input name="preview_size" type="hidden" value="240">
		<input name="thumb_size" type="hidden" value="100">
		<input name="water" type="hidden" value="none">
		<input name="uploaded" type="hidden" value="hellyeah">
		<?php
		}

?>
</div>
</fieldset>
<p style="text-align:right"><small><a href="javascript:faq.sweepToggle('contract')">Contract All</a> | <a href="javascript:faq.sweepToggle('expand')">Expand All</a></small></p>
<fieldset>
<legend><span id="faq4-title" class="iconspan"><img src="/assets/images/icons/minus.gif" alt="" /></span>&nbsp;&nbsp;&nbsp;SEO Stuff</legend>
<div id="faq4" class="icongroup1"><p>
<label for="keywords"><span>Keywords</span></label>
<textarea name="keywords" rows="5" cols="50"><?php echo("$result[keywords]"); ?></textarea></p>
<p><label for="description"><span>Description</span></label>
<textarea name="description" rows="5" cols="50"><?php echo("$result[description]"); ?></textarea></p>
<p><label for="search_terms"><span>Search Terms<br/>For use with site search</span></label>
<textarea name="search_terms" rows="5" cols="50"><?php echo("$result[search_terms]"); ?></textarea> </p>
<p>
<label for="page_name"><span>SEO page name</span></label>
<?php
$page_name 	= 	str_replace(".php", "", $result[seo]);

?>
<input name="page_name" id="page_name" type="text" size="50" value="<?php echo("$page_name"); ?>"/></p>
</div>
</fieldset>
<p style="text-align:right"><small><a href="javascript:faq.sweepToggle('contract')">Contract All</a> | <a href="javascript:faq.sweepToggle('expand')">Expand All</a></small></p>
<fieldset>
<legend><span id="faq5-title" class="iconspan"><img src="/assets/images/icons/minus.gif" alt="" /></span>&nbsp;&nbsp;&nbsp;Related Products</legend>
<div id="faq5" class="icongroup1">
<div class="scroll"><p>
        <?php

$cat_items = explode(',',$result[related_products]);
$cat_contents = array();
foreach ($cat_items as $cat_item) 
	{
	array_push($cat_contents, $cat_item);
	}

//print_r($cat_contents);

$catquery = mysql_query("select * from $product_table where sub is NULL order by name asc");
while ($catresult = mysql_fetch_array($catquery))
	{
		if (in_array($catresult[id], $cat_contents)) 
			{
    		$checked = "checked ";
			}
		else
			{
			$checked = "";
			}

	echo("<label><input name='product".$catresult[id]."' type='checkbox' value='".$catresult[id]."' $checked/>&nbsp;&nbsp;".$catresult[name]."</label><br/>");
	}
?>
      </p></div></div>
</fieldset>
<p></p>
<div id="right"><input type="submit" name="Submit" value="Update" /></div>

</form>

<script type="text/javascript">
var faq=new switchicon("icongroup1", "div") //Limit scanning of switch contents to just "div" elements
faq.setHeader('<img src="/assets/images/icons/minus.gif" />', '<img src="/assets/images/icons/plus.gif" />') //set icon HTML
faq.collapsePrevious(false) //Allow only 1 content open at any time
faq.setPersist(true) //No persistence enabled
faq.defaultExpanded() //Set 1st content to be expanded by default
faq.init()
</script>

<div id="sending" style="display: none;"> <img src="../assets/images/loading.gif" alt="Sending...Please wait." class="sending"/>
      <h3>&nbsp;Uploading product... please wait</h3>
      <p>Please be patient while the site formats the images and adds the product to the database. This may take up to 30 seconds. (<a href="javascript: expandCollapse('imgform','sending');">Cancel</a>)</p>
    </div>
<?php
}
}
else
	{
	echo("<h2>Product Management</h2>");
	
//==========================================================================
//ADD PROCESS
//==========================================================================	
if ($_GET[add] == "ok")
	{
	$title =  htmlentities($_POST[name], ENT_QUOTES);
	$added = date('YmdHis');
	
	if ($_POST[parent_product])
		{
			$parent_c = ", sub";
			$parent_a = ", '$_POST[parent_product]'";
		}
		
		if (!$_POST[dis10])
			{
			$dis10 = $_POST[price];
			}
		else
			{
			$dis10 = $_POST[dis10];
			}
		if (!$_POST[dis20])
			{
			$dis20 = $_POST[price];
			}
		else
			{
			$dis20 = $_POST[dis20];
			}
		if (!$_POST[dis50])
			{
			$dis50 = $_POST[price];
			}
		else
			{
			$dis50 = $_POST[dis50];
			}
		
		// ADD RELATED PRODUCTS
		$product_list = "";
		$catquery = mysql_query("select * from $product_table");
		while ($catresult = mysql_fetch_array($catquery))
			{		
			$prod_id = "product".$catresult[id];
			
			if ($_POST[$prod_id] > 0)
				{
				//echo ("$_POST[$prod_id]<br/>");
				$product_list .= $_POST[$prod_id].",";
				}
			}
		//echo $product_list;
		
		
		$process=mysql_query("insert into $product_table (text, summary, name, price, dis10, dis20, dis50, description, keywords, stock, code, date_added, att_name, seo, search_terms, related_products, product_active, att_value$parent_c) values ('$_POST[text]', '$_POST[summary]', '$title', '$_POST[price]', '$dis10', '$dis20', '$dis50', '$_POST[description]', '$_POST[keywords]', '$_POST[stock]', '$_POST[code]', '$added', '$_POST[att_name]', '$myPage', '$_POST[search_terms]', '$product_list', '$_POST[product_active]', '$_POST[att_value]'$parent_a)"); 
	
	$product_query 		= mysql_query("select * from $product_table order by id desc limit 1");
	$product_result 	= mysql_fetch_array($product_query);
	$product_code 		= "$product_result[id]";
	
	//ADD IMAGES
	if($_FILES['imagefile']['size'] > 0)
		{
		$process_thumbnail 	= "YES";
		$process_medium 	= "YES";
		$process_large		= "YES";
		require("image_editor.php"); 
		}
	
	
	// BUILD SEO PAGE
	if ($_POST[page_name])
		{
		$page_name_formatted 	= 	str_replace(" ", "_", $_POST[page_name]);
		$page_name_formatted 	= 	str_replace(".php", "", $page_name_formatted);
		
		if ($page_name_formatted == $this_page)
			{
			echo("<p>There was a problem. The page could not be created</p>");
			}
		else
			{
			require("page_builder.php");
			}
		}
	
	
	if(!$process)
		{
		echo("<div id='status_error'><p><strong>Oops!</strong></p><p>There was an error when you tried to add the product.</p></div>");
		}
	else
		{
		echo("<div id='status_ok'><p><small>Thank you the product has now been added.</small></p></div>");
		$tracker=mysql_query("insert into $admin_track_table (name, loggedin, ip, job) values ('$_COOKIE[user_name]', '$tracker_time', '$ip', 'Product - added')");
		}	
	}

	
//==========================================================================
//EDIT PROCESS
//==========================================================================
if($_GET[edit])
{
	$title =  htmlentities($_POST[name], ENT_QUOTES);

if (!$_POST[dis10])
	{
	$dis10 = $_POST[price];
	}
else
	{
	$dis10 = $_POST[dis10];
	}
if (!$_POST[dis20])
	{
	$dis20 = $_POST[price];
	}
else
	{
	$dis20 = $_POST[dis20];
	}
if (!$_POST[dis50])
	{
	$dis50 = $_POST[price];
	}
else
	{
	$dis50 = $_POST[dis50];
	}

	// ADD RELATED PRODUCTS
	$product_list = "";
	$catquery = mysql_query("select * from $product_table");
	while ($catresult = mysql_fetch_array($catquery))
		{		
		$prod_id = "product".$catresult[id];
		
		if ($_POST[$prod_id] > 0)
			{
			//echo ("$_POST[$prod_id]<br/>");
			$product_list .= $_POST[$prod_id].",";
			}
		}
	//echo $product_list;

	$process=mysql_query("update $product_table set text='$_POST[text]', summary='$_POST[summary]', name='$title', price='$_POST[price]', dis10='$dis10', dis20='$dis20', dis50='$dis50', description = '$_POST[description]', keywords = '$_POST[keywords]', stock = '$_POST[stock]', code = '$_POST[code]', att_name = '$_POST[att_name]', att_value = '$_POST[att_value]', seo = '$myPage', related_products = '$product_list', search_terms= '$_POST[search_terms]', product_active='$_POST[product_active]' where id='$_POST[id]'"); 

$product_code = "$_POST[id]";
	
	// UPDATE IMAGES
	if($_FILES['imagefile']['size'] > 0)
		{
		$process_thumbnail 	= "YES";
		$process_medium 	= "YES";
		$process_large		= "YES";
		require("image_editor.php"); 
		}
	
	if ($_POST[page_name])
		{
		$page_name_formatted 	= 	str_replace(" ", "_", $_POST[page_name]);
		$page_name_formatted 	= 	str_replace(".php", "", $page_name_formatted);
		
		if ($page_name_formatted == $this_page)
			{
			echo("<p>There was a problem. The page could not be created</p>");
			}
		else
			{
			require("page_builder.php");
			$process=mysql_query("update $product_table set seo = '$myPage' where id='$_POST[id]'"); 
			}
		}
	
	
	if (!$process)
	{
	echo("<div id='status_error'><p><strong>Oops!</strong></p><p>There was an error when you tried to update the product.</p></div>");
	}
	else
	{
	echo("<div id='status_ok'><p><small>Thank you the product has now been updated.</small></p></div>");
	$tracker=mysql_query("insert into $admin_track_table (name, loggedin, ip, job) values ('$_COOKIE[user_name]', '$tracker_time', '$ip', 'Product - edited ($title)')");
	}
}

//==========================================================================
//DELETE PROCESS
//==========================================================================
if($_GET[delete])
{
$query = mysql_query("select * from $product_table where id='$_GET[delete]'");
$result = mysql_fetch_array($query);

if ($result[seo])
	{
	unlink("../products/".$result[seo]."");
	echo("<p><small>The Search Engine Friendly page ($result[seo]) has been removed</small></p>");
	}

$delete_db=mysql_query("delete from $product_table where id='$_GET[delete]'");
if(!$delete_db)
	{
	echo("<div id='status_error'><p>The product could not be removed</p></div>");
	}
else
	{
	echo("<div id='status_ok'><p>The product has been removed from the system</p></div>");
	$tracker=mysql_query("insert into $admin_track_table (name, loggedin, ip, job) values ('$_COOKIE[user_name]', '$tracker_time', '$ip', 'Product - deleted')");
	}
}	

//==========================================================================
//DELETE MAIN PROCESS
//==========================================================================
if($_GET[delete_main])
{

// REMOVE ANY SUB PRODUCTS FROM THIS FILE
$query = mysql_query("select * from $product_table where sub='$_GET[delete_main]'");
while($result = mysql_fetch_array($query))
	{
	$process = mysql_query("update $product_table set sub=NULL where id='$result[id]'");
	}

// REMOVE FROM CATEGORY LISTS
$query = mysql_query("select * from $category_table where products like '%$_GET[delete_main],%'");
while($result = mysql_fetch_array($query))
	{
	$product_list = str_replace("$_GET[delete_main],","",$result[products]);
	$process = mysql_query("update $category_table set products='$product_list' where id='$result[id]'");
	}

// REMOVE ANY IMAGES FROM SYSTEM	
$query = mysql_query("select * from $gallery_table where doc_cat ='$_GET[delete_main]'");
while($result = mysql_fetch_array($query))
	{
	//DELETE FILES FROM FOLDERS
	unlink("../assets/images/products/originals/".$result[name]."");
	unlink("../assets/images/products/large/".$result[name]."");
	unlink("../assets/images/products/thumbs/".$result[name]."");
	unlink("../assets/images/products/medium/".$result[name]."");
	
	//DELETE FROM DATABASE
	$process=mysql_query("delete from $gallery_table where doc_cat='$_GET[delete_main]'");
	}

// REMOVE SEO FILE
$query = mysql_query("select * from $product_table where id='$_GET[delete_main]'");
$result = mysql_fetch_array($query);

if ($result[seo])
	{
	unlink("../products/".$result[seo]."");
	echo("<p><small>The Search Engine Friendly page ($result[seo]) has been removed</small></p>");
	}

//FINALLY DELETE PRODUCT
$delete_db=mysql_query("delete from $product_table where id='$_GET[delete_main]'");
if(!$delete_db)
	{
	echo("<div id='status_error'><p>The product could not be removed</p></div>");
	}
else
	{
	echo("<div id='status_ok'><p>The product has been removed from the system and its sub product(s) have now become individual items in the system</p></div>");
	$tracker=mysql_query("insert into $admin_track_table (name, loggedin, ip, job) values ('$_COOKIE[user_name]', '$tracker_time', '$ip', 'Product - deleted')");
	}
}	


?>

<?php





	echo("<div id='admin'>
	<table width='90%'>
		<thead>
			<tr><th></th><th>Product</th><th>Price</th><th>Stock</th><th>Live</th><th>Code</th><th>Options</th></tr>
		</thead>
		<tbody>
	
	");
	
	$num = 1;
	
	$query = mysql_query("select * from $product_table where sub IS NULL order by name asc");
	while($result = mysql_fetch_array($query))
		{
		
		if($class =="row2")
			{
				$class = 'row5';
			}
			else
			{
				$class = 'row2';
			}
		
		if ($result[stock] == "NO")
			{
			$stock = "<small style='color: #F00;font-weight: bold'>Out of stock</small>";
			}
		else
			{
			$stock = "<small>In stock</small>";
			}
		if ($result[product_active] == "NO")
			{
			$active = "<small style='color: #F00;font-weight: bold'>No</small>";
			}
		else
			{
			$active = "<small style='color: #3C0;font-weight: bold'>Yes</small>";
			}
		
		echo("<tr class='$class'><td>$num.</td><td><small>$result[name]</small></td><td><small>&pound;$result[price]</small></td><td>$stock</td><td>$active</td><td><small>$result[code]</small></td><td><small><a href='manage_products.php?id=$result[id]' class='edit_product'>Edit</a> | <a href='add_products.php?sub_product=$result[id]' class='edit_product'>Add sub</a> | <a href='manage_products.php?delete_main=$result[id]' class='delete' onClick='return confirmDelete()'>Delete</a></small></td></tr>");
		$num++;
		$subquery = mysql_query("select * from $product_table where sub = '$result[id]' order by id asc");
		while($sresult = mysql_fetch_array($subquery))
		{
			if($sclass =="row5")
			{
				$sclass = 'row2';
			}
			else
			{
				$sclass = 'row5';
			}
			
			if ($sresult[stock] == "NO")
			{
			$stock = "<small style='color: #F00;font-weight: bold'>Out of stock</small>";
			}
		else
			{
			$stock = "<small>In stock</small>";
			}
			
		if ($sresult[product_active] == "NO")
			{
			$active = "<small style='color: #F00;font-weight: bold'>No</small>";
			}
		else
			{
			$active = "<small style='color: #3C0;font-weight: bold'>Yes</small>";
			}
		
		echo("<tr class='$class'><td></td><td><small>&#0187;&#0187;&nbsp;$sresult[att_name]: $sresult[att_value]</small></td><td><small>&pound;$sresult[price]</small></td><td>$stock</td><td>$active</td><td><small>$sresult[code]</small></td><td><small><a href='manage_products.php?id=$sresult[id]' class='edit_product'>Edit</a> | <a href='manage_products.php?delete=$sresult[id]' class='delete' onClick='return confirmDelete()'>Delete</a></small></td></tr>");
		}
		
				
		}
	echo("</tbody></table></div>");
	}
?></div>
  <div id="footer">
  <?php 
	// THIS PULLS IN THE FOOTER
	require("../assets/widgets/footer.php"); 
	?>
  </div>
</div>

</body>
</html>
