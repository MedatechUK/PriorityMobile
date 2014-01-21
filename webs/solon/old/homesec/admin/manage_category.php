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
require_once('../assets/widgets/admin_functions.inc.php');
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
<script type="text/javascript" src="../assets/scripts/functions.js"></script>
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
if($_GET[add])
	{
		
?>
    <h2>Add Category</h2>
    <form method="post" action="manage_category.php?added=ok" enctype="multipart/form-data" id="imgform" onsubmit="expandCollapse('imgform','sending');<?php //echo("return false"); ?>">
      <fieldset>
      <legend>Category Details</legend>
      <p>
        <label for="name"><span>Category Name</span></label>
        <input name="name" id="name" type="text" size="50"/>
      </p>
      <p>Category Summary</p>
      <?php
$oFCKeditor = new FCKeditor('cat_info') ;
$oFCKeditor->BasePath = 'FCKeditor/';
$oFCKeditor->Width  = '500' ;
$oFCKeditor->Height = '250' ;
$oFCKeditor->ToolbarSet = 'summary';
$oFCKeditor->Create() ;
?>


      </fieldset>
	  <fieldset>
<legend>Meta Tags</legend>
<p>
<label for="metatags">Keywords</label><br />
<textarea name="metatags" rows="5" cols="50"></textarea></p>
<p>
<label for="metadesc">Description</label><br />
<textarea name="metadesc" rows="5" cols="50"></textarea></p>
</fieldset>
	  <fieldset>
	  
      <legend>Products</legend>
      <div class="scroll">
	  <p>
        <?php

$catquery = mysql_query("select * from $product_table where sub is NULL order by name asc");
while ($catresult = mysql_fetch_array($catquery))
	{
	echo("<label><input name='product".$catresult[id]."' type='checkbox' value='".$catresult[id]."' />&nbsp;&nbsp;".$catresult[name]."</label><br/>");
	}
?>
      </p>
	  </div>
      </fieldset>
      <fieldset>
      <legend>Category Image</legend>
      <p>
        <label for="filename"><span>Select File</span></label>
        <input type="file" name="imagefile" id="filename"/>
      </p>
      </fieldset>
      <p></p>
      <div id="right">
	  <input name="preview_size" type="hidden" value="580" />
<input name="thumb_size" type="hidden" value="100" />
<input name="water" type="hidden" value="none"/>
<input name="uploaded" type="hidden" value="hellyeah" />
        <input type="submit" name="Submit" value="Add Category" />
      </div>
    </form>
    <div id="sending" style="display: none;"> <img src="../assets/images/loading.gif" alt="Sending...Please wait." class="sending"/>
      <h3>&nbsp;Uploading category... please wait</h3>
      <p>Please be patient while the site formats the images and adds the category to the database. This may take up to 30 seconds. (<a href="javascript: expandCollapse('imgform','sending');">Cancel</a>)</p>
    </div>
    <?php
}

	
//EDIT PAGE CONTENT
elseif($_GET[id])
	{
	$query = mysql_query("select * from $category_table where id='$_GET[id]'");
	while($result = mysql_fetch_array($query))
	{
		
?>
    <h2>Edit Category</h2>
    <form method="post" action="manage_category.php?edit=<?php echo("$result[id]"); ?>" enctype="multipart/form-data" id="imgform" onsubmit="expandCollapse('imgform','sending');<?php //echo("return false"); ?>">
      <input name="id" type="hidden" value="<?php echo("$result[id]"); ?>"/>
      <fieldset>
      <legend>Category Details</legend>
      <p>
        <label for="name"><span>Category Name</span></label>
        <input name="name" id="name" type="text" value="<?php echo("$result[name]"); ?>" size="50"/>
      </p>
      <p>Category Summary</p>
      <?php
$oFCKeditor = new FCKeditor('cat_info') ;
$oFCKeditor->BasePath = 'FCKeditor/';
$oFCKeditor->Value = $result[cat_info];
$oFCKeditor->Width  = '500' ;
$oFCKeditor->Height = '250' ;
$oFCKeditor->ToolbarSet = 'summary';
$oFCKeditor->Create() ;
?>
      </fieldset>
	  <fieldset>
<legend>Meta Tags</legend>
<p>
<label for="metatags">Keywords</label><br />
<textarea name="metatags" rows="5" cols="50"><?php echo("$result[metatags]"); ?></textarea></p>
<p>
<label for="metadesc">Description</label><br />
<textarea name="metadesc" rows="5" cols="50"><?php echo("$result[metadesc]"); ?></textarea></p>
</fieldset>
      <fieldset>
      <legend>Products</legend>
      <div class="scroll"><p>
        <?php

$cat_items = explode(',',$result[products]);
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
      </p></div>
      </fieldset>
	  <fieldset>
<legend>Category Image</legend>
<?php
$img_query = mysql_query("select * from $gallery_table where doc_cat = 'CAT$result[id]'");
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

</fieldset>
      <p></p>
      <div id="right">
        <input type="submit" name="Submit" value="Update Category" />
      </div>
    </form>
    <div id="sending" style="display: none;"> <img src="../assets/images/loading.gif" alt="Sending...Please wait." class="sending"/>
      <h3>&nbsp;Uploading category... please wait</h3>
      <p>Please be patient while the site formats the images and updates the category to the database. This may take up to 30 seconds. (<a href="javascript: expandCollapse('imgform','sending');">Cancel</a>)</p>
    </div>
    <?php
}
}
else
	{
	echo("<h2>Category Management</h2>");

//==========================================================================
//ADDED PROCESS
//==========================================================================
if($_GET[added])
	{
	
	$max_query		= mysql_query("select * from $category_table order by list_order desc limit 1");
	$max_result		= mysql_fetch_array($max_query);
	$list_order		= $max_result[list_order] + 1;
	
	$product_list = "";
	$catquery = mysql_query("select * from $product_table");
	while ($catresult = mysql_fetch_array($catquery))
		{		
		$prod_id = "product".$catresult[id];
		
		if ($_POST[$prod_id] > 0)
			{
			$product_list .= $_POST[$prod_id].",";
			}
		}
	$title =  html($_POST[name]);
	$process=mysql_query("insert into $category_table (name, cat_info, list_order, products, metatags, metadesc) values ('$title', '$_POST[cat_info]', '$list_order', '$product_list', '$_POST[metatags]', '$_POST[metadesc]')");
	
	$query 			= mysql_query("select * from $category_table order by id desc limit 1");
	$result 		= mysql_fetch_array($query);
	
	if($_FILES['imagefile']['size'] > 0)
		{
		$product_code 		= "CAT$result[id]";
		$process_category 	= "YES";
		$process_thumbnail 	= "YES";
		$process_medium 	= "NO";
		$process_large		= "NO";
		require("image_editor.php"); 
		}
	
	if (!$process)
		{
		echo("<div id='status_error'><p><strong>Oops!</strong></p><p>There was an error when you tried to add the category.</p></div>");
		}
	else
		{
		echo("<div id='status_ok'><p>Thank you the category has now been added.</p></div>");
		$tracker=mysql_query("insert into $admin_track_table (name, loggedin, ip, job) values ('$_COOKIE[user_name]', '$tracker_time', '$ip', 'Category - Added')");
		}
	}
	
//==========================================================================
//EDIT PROCESS
//==========================================================================
if($_GET[edit])
{

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

$title =  html($_POST[name]);
	$process=mysql_query("update $category_table set name='$title', metatags='$_POST[metatags]', metadesc='$_POST[metadesc]', products='$product_list', cat_info='$_POST[cat_info]' where id='$_POST[id]'"); 
		
	if($_FILES['imagefile']['size'] > 0)
		{
		$product_code 		= "CAT$_POST[id]";
		$process_thumbnail 	= "YES";
		$process_category 	= "YES";
		$process_medium 	= "NO";
		$process_large		= "NO";
		require("image_editor.php"); 
		}
	
	
	if (!$process)
	{
	echo("<div id='status_error'><p><strong>Oops!</strong></p><p>There was an error when you tried to update the category.</p></div>");
	}
	else
	{
	echo("<div id='status_ok'><p>Thank you the category has now been updated.</p></div>");
	$tracker=mysql_query("insert into $admin_track_table (name, loggedin, ip, job) values ('$_COOKIE[user_name]', '$tracker_time', '$ip', 'Category - Edited ($_POST[name])')");
	}
}

//==========================================================================
//LIST ORDER PROCESS
//==========================================================================
if($_GET[list_order])
{

if ($_GET[up_value])
	{
	$processa=mysql_query("update $category_table set list_order='$_GET[current_value]' where list_order='$_GET[up_value]'"); 
	$processb=mysql_query("update $category_table set list_order='$_GET[up_value]' where id='$_GET[list_order]'");
		
	if (!$processa && !$processb)
		{
		echo("<div id='status_error'><p><strong>Oops!</strong></p><p>There was an error when you tried to update the list order.</p></div>");
		}
	else
		{
		//echo("<div id='status_ok'><p>Thank you the list order has now been updated.</p></div>");
		$tracker=mysql_query("insert into $admin_track_table (name, loggedin, ip, job) values ('$_COOKIE[user_name]', '$tracker_time', '$ip', 'Category - List order updated')");
		}
	}
	
if ($_GET[down_value])
	{
	$processa=mysql_query("update $category_table set list_order='$_GET[current_value]' where list_order='$_GET[down_value]'"); 
	$processb=mysql_query("update $category_table set list_order='$_GET[down_value]' where id='$_GET[list_order]'");
		
	if (!$processa && !$processb)
		{
		echo("<div id='status_error'><p><strong>Oops!</strong></p><p>There was an error when you tried to update the list order.</p></div>");
		}
	else
		{
		//echo("<div id='status_ok'><p>Thank you the list order has now been updated.</p></div>");
		$tracker=mysql_query("insert into $admin_track_table (name, loggedin, ip, job) values ('$_COOKIE[user_name]', '$tracker_time', '$ip', 'Category - List order updated')");
		}
	}
}
	
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////	
//		DELETE PROCESS
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////	
if($_GET[delete])
	{
	$query = mysql_query("select * from $category_table where id = '$_GET[delete]'");
	while($result = mysql_fetch_array($query))
		{		
		//DEDUCT 1 FROM EACH CATEGORY HIGHER UP THE LIST
		$list_query = mysql_query("select * from $category_table where list_order > '$result[list_order]' order by list_order asc");
		while($list_result = mysql_fetch_array($list_query))
			{
			//echo ("<p>TEST - $list_result[list_order]</p>");
			$list_order = $list_result[list_order] - 1;
			$update = mysql_query("update $category_table set list_order='$list_order' where id='$list_result[id]'");
			}
		
		//DELETE FROM DATABASE
		$delete_db=mysql_query("delete from $category_table where id='$result[id]'");
		//LOG IN TRACKER
		if(!$delete_db)
			{
			echo("<div id='status_error'><p>The category could not be removed</p></div>");
			}
		else
			{
			echo("<div id='status_ok'><p>The category has been removed from the system</p></div>");
			$tracker=mysql_query("insert into $admin_track_table (name, loggedin, ip, job) values ('$_COOKIE[user_name]', '$tracker_time', '$ip', 'Category - Deleted')");
			}
		}	
	}	
	
	echo("<div id='admin'>
	<table width='90%'>
		<thead>
			<tr><th>Name</th><th>Products</th><th>List Order</th><th>Options</th></tr>
		</thead>
		<tbody>
	
	");
	$query = mysql_query("select * from $category_table order by list_order asc");
	$numrows = mysql_num_rows($query);
	$rows = 1;
	while($result = mysql_fetch_array($query))
		{		
		$count = -1;
		$cat_items = explode(',',$result[products]);
		$cat_contents = array();
		foreach ($cat_items as $cat_item) {
			$cat_contents[$cat_item] = (isset($cat_contents[$cat_item])) ? $cat_contents[$cat_item] + 1 : 1;
			$count++;
		}
		
		$up_value 		= 	$result[list_order] + 1;
		$down_value 	= 	$result[list_order] - 1;
		
		$downlink		=	"<a href='manage_category.php?list_order=$result[id]&amp;up_value=$up_value&amp;current_value=$result[list_order]'><img src='../assets/images/admin/down.gif' border='0'/></a>";
		
		$uplink			=	"<a href='manage_category.php?list_order=$result[id]&amp;down_value=$down_value&amp;current_value=$result[list_order]'><img src='../assets/images/admin/up.gif' border='0'/></a>";
		
		if($num == "1")
			{
			$class=" class='row2'";
			$num=2;
			}
		else
			{
			$class=" class='row1'";
			$num=1;
			}
		
		
		echo("<tr$class><td>$result[name]</td><td>$count</td><td>$result[list_order]");
		
		if ($rows == 1)
			{
			echo("&nbsp;&nbsp;$downlink");
			}		
		elseif ($rows < $numrows)
			{
			echo("&nbsp;&nbsp;$downlink $uplink");
			}
		else
			{
			echo("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;$uplink");
			}
		 
		
		
		echo("</td><td><a href='manage_category.php?id=$result[id]' class='edit_product'>Edit</a> | <a href='manage_category.php?delete=$result[id]' class='delete' onClick='return confirmDelete()'>Delete</a></td></tr>");	
		$rows++;	
		}
	echo("</tbody></table></div>");
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
</body>
</html>
