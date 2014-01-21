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
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////	
//		DELETE PROCESS
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////	
if($_GET[img_id])
	{
	$query = mysql_query("select * from $gallery_table where id ='$_GET[img_id]'");
	while($result = mysql_fetch_array($query))
		{
		//DELETE FILES FROM FOLDERS
		unlink("../assets/images/products/originals/".$result[name]."");
		unlink("../assets/images/products/large/".$result[name]."");
		unlink("../assets/images/products/thumbs/".$result[name]."");
		unlink("../assets/images/products/medium/".$result[name]."");
		unlink("../assets/images/products/small/".$result[name]."");
		
		//DELETE FROM DATABASE
		$delete_db=mysql_query("delete from $gallery_table where id='$result[id]'");
		//LOG IN TRACKER
		if(!$delete_db)
			{
			echo("<div id='status_error'><p>The file $result[name] ($result[doc_name]) could not be removed</p></div>");
			}
		else
			{
			echo("<div id='status_ok'><p>The file $result[name] ($result[doc_name]) has been removed from the system</p></div>");
			$tracker=mysql_query("insert into $admin_track_table (name, loggedin, ip, job) values ('$_COOKIE[user_name]', '$tracker_time', '$ip', 'Image - deleted')");
			}
		}	
	}
	
if($_GET[cat_id])
	{
	$query = mysql_query("select * from $gallery_table where id ='$_GET[img_id]'");
	while($result = mysql_fetch_array($query))
		{
		//DELETE FILES FROM FOLDERS
		unlink("../assets/images/products/originals/".$result[name]."");
		unlink("../assets/images/products/categories/".$result[name]."");
		unlink("../assets/images/products/thumbs/".$result[name]."");
		//unlink("../assets/images/products/medium/".$result[name]."");
		
		//DELETE FROM DATABASE
		$delete_db=mysql_query("delete from $gallery_table where id='$result[id]'");
		//LOG IN TRACKER
		if(!$delete_db)
			{
			echo("<div id='status_error'><p>The file $result[name] ($result[doc_name]) could not be removed</p></div>");
			}
		else
			{
			echo("<div id='status_ok'><p>The file $result[name] ($result[doc_name]) has been removed from the system</p></div>");
			$tracker=mysql_query("insert into $admin_track_table (name, loggedin, ip, job) values ('$_COOKIE[user_name]', '$tracker_time', '$ip', 'Image - deleted')");
			}
		}	
	}

	echo("<div id='admin'>");
	
			echo("<h3>Product Images</h3><table summary='List of Images online'>
			<thead>
				<tr>
					<th>Image</th>
					<th>Product</th>
					<th>Code</th>
					<th>Options</th>
				</tr>
			</thead>
			<tbody>");
	
	$query = mysql_query("select * from $gallery_table where doc_cat not like 'CAT%' order by id asc");
	while($result = mysql_fetch_array($query))
		{
				
		$prod_query = mysql_query("select * from $product_table where id = '$result[doc_cat]'");
		$prod_result = mysql_fetch_array($prod_query);

		echo("
		<tr$class>
			<td width='100'><img src='../assets/images/products/small/$result[name]' alt='$result[name]' /></td>
			<td>$prod_result[name]</td>
			<td>$prod_result[code]</td>
			<td class='del'><a href='manage_images.php?img_id=$result[id]' class='delete' onClick='return confirmDelete()'>Delete</a></td>
		</tr>");
		}
	echo("</tbody></table>");
	
	
	// CATEGORY IMAGES
	echo("<h3>Category Images</h3><table summary='List of Images online'>
			<thead>
				<tr>
					<th>Image</th>
					<th>Category</th>
					<th>Options</th>
				</tr>
			</thead>
			<tbody>");
	
	$query = mysql_query("select * from $gallery_table where doc_cat like 'CAT%' order by id asc");
	while($result = mysql_fetch_array($query))
		{
		$cat_id 		=	str_replace("CAT","",$result[doc_cat]);
		$prod_query 	= 	mysql_query("select * from $category_table where id = '$cat_id'");
		$prod_result 	= 	mysql_fetch_array($prod_query);
		
		echo("
		<tr$class>
			<td width='100'><img src='../assets/images/products/thumbs/$result[name]' alt='$result[name]' /></td>
			<td>$prod_result[name]</td>
			<td class='del'><a href='manage_images.php?cat_id=$result[id]' class='delete' onClick='return confirmDelete()'>Delete</a></td>
		</tr>");
		}
	echo("</tbody></table>");
	
	echo("</div>");
	
 
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
