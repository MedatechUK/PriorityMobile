<?php 
// SECURITY CHECK TO MAKE SURE THE CORRECT USERNAME AND PASSWORD HAS BEEN ENTERED
if(!$user_name | !$user_pwd)
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
<link rel="shortcut icon" href="http://www.personal-attack-alarms.net/favicon.ico" />
<link rel="stylesheet" href="../assets/css/admin.css" type="text/css" media="screen" />
</head>
<body>
<noscript>
<h1>Warning</h1>
<p class="noscript">To use this site correctly you need to have JavaScript enabled on your web browser</p>
</noscript>
<div id="hidden">
  <?php require("../assets/widgets/admin_hidden.php"); ?>
</div>
<div id="container">
  <div id="header">
    <h2>Website</h2>
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
	$query = mysql_query("select * from SHOP1_products where id='$_GET[id]'");
	while($result = mysql_fetch_array($query))
	{
		
?>
<h2>Edit Product</h2>
<form method="post" action="manage_products.php?edit=<?php echo("$result[id]"); ?>" enctype="multipart/form-data">
<input name="id" type="hidden" value="<?php echo("$result[id]"); ?>"/>

<fieldset>
<legend>Product Details</legend>

<p><label for="name">Product Name</label>
<input name="name" id="name" type="text" value="<?php echo("$result[name]"); ?>" size="50"/>
</p>

<p><label for="price">Product Price</label>
<input name="price" id="price" type="text" value="<?php echo("$result[price]"); ?>"/>
</p>

<p><label for="code">Product Code</label>
<input name="code" id="code" type="text" value="<?php echo("$result[code]"); ?>"/>
</p>

<p>Product Summary</p>
<?php
$oFCKeditor = new FCKeditor('summary') ;
$oFCKeditor->BasePath = 'FCKeditor/';
$oFCKeditor->Value = $result[summary];
$oFCKeditor->Width  = '300' ;
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
<fieldset>
<legend>Stock control</legend>
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

</fieldset>
<fieldset>
<legend>Meta Tags</legend>
<p>
<label for="keywords">Keywords</label><br />
<textarea name="keywords" rows="5" cols="50"><?php echo("$result[keywords]"); ?></textarea></p>
<p>
<label for="description">Description</label><br />
<textarea name="description" rows="5" cols="50"><?php echo("$result[description]"); ?></textarea></p>
</fieldset>
<p></p>
<div id="right"><input type="submit" name="Submit" value="Update" /></div>

</form>
<?php
}
}
else
	{
	echo("<h2>Product Management</h2>");
	
	
//EDIT PROCESS
if($_GET[edit])
{
	$title =  htmlentities($_POST[name], ENT_QUOTES);

	$process=mysql_query("update SHOP1_products set text='$_POST[text]', summary='$_POST[summary]', name='$title', price='$_POST[price]', description = '$_POST[description]', keywords = '$_POST[keywords]', stock = '$_POST[stock]', code = '$_POST[code]' where id='$_POST[id]'"); 

	if (!$process)
	{
	echo("<div id='status_error'><p><strong>Oops!</strong></p><p>There was an error when you tried to update the product.</p></div>");
	}
	else
	{
	echo("<div id='status_ok'><p>Thank you the product has now been updated.</p></div>");
	$process=mysql_query("insert into SHOP1_admin_tracker (name, loggedin, ip, job) values ('$_COOKIE[user_name]', '$tracker_time', '$ip', 'Product Updated - $_POST[id]')");
	}
}
	
	echo("<div id='admin'>
	<table width='90%'>
		<thead>
			<tr><th>Product</th><th>Stock</th><th>Code</th><th>Options</th></tr>
		</thead>
		<tbody>
	
	");
	$query = mysql_query("select * from SHOP1_products where sub IS NULL order by name asc");
	while($result = mysql_fetch_array($query))
		{
		
		if ($result[stock] == "NO")
			{
			$stock = "<small style='color: #F00;font-weight: bold'>Out of stock</small>";
			}
		else
			{
			$stock = "<small>In stock</small>";
			}
		
		echo("<tr class='row2'><td>$result[name]</td><td>$stock</td><td>$result[code]</td><td><a href='manage_products.php?id=$result[id]' class='edit_product'>Edit</a></td></tr>");		
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
