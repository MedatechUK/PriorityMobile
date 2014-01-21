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
	if ($_GET[sub_product])
		{
		$query = mysql_query("select * from $product_table where id = '$_GET[sub_product]'");
		$result = mysql_fetch_array($query);
		}
?>
	
	
	
<h2>Add Product</h2>

<p style="text-align:right"><small><a href="javascript:faq.sweepToggle('contract')">Contract All</a> | <a href="javascript:faq.sweepToggle('expand')">Expand All</a></small></p>
<form method="post" action="manage_products.php?add=ok" enctype="multipart/form-data" id="imgform" onsubmit="expandCollapse('imgform','sending');<?php //echo("return false"); ?>">

<?php

if ($_GET[sub_product])
	{
	echo("<fieldset><legend>Parent Product</legend>");
	echo("<p><label><span>Parent product</span></label>$result[name]</p>");
	echo("<p><label><span>Parent code</span></label>$result[code]</p>");
	echo("</fieldset>");
	echo("<input name='parent_product' type='hidden' value='$_GET[sub_product]' />");
	}
?>

<fieldset>
<legend>Product Details</legend>

<?php

if ($_GET[sub_product])
	{
	echo("<p><label><span>Product Name</span></label>$result[name]&nbsp;</p>");
	echo("<input name='name' type='hidden' value='$result[name]' />");
	}
else
	{
	?>
	<p><label for="name"><span>Product Name</span></label>
<input name="name" id="name" type="text" size="50"/>
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
  <input type="radio" name="stock" value="YES"<?php if ($_POST[stock] != "NO"){ echo ("checked='checked'"); }?>/>
Yes</label>
  <br />
  <label>
  <input type="radio" name="stock" value="NO" <?php if ($_POST[stock] == "NO"){ echo ("checked='checked'"); }?>/>
No</label>
</p>
<p><label for="product_active">Product visibility</label></p>
<p>
 <label>
  <input type="radio" name="product_active" value="YES"<?php if ($_POST[product_active] != "NO"){ echo ("checked='checked'"); }?>/>
Show on site</label>
  <br />
  <label>
  <input type="radio" name="product_active" value="NO" <?php if ($_POST[product_active] == "NO"){ echo ("checked='checked'"); }?>/>
Hide from site</label>
</p>
</div>
</fieldset>

<p style="text-align:right"><small><a href="javascript:faq.sweepToggle('contract')">Contract All</a> | <a href="javascript:faq.sweepToggle('expand')">Expand All</a></small></p>
<fieldset>
<legend><span id="faq2-title" class="iconspan"><img src="/assets/images/icons/minus.gif" alt="" /></span>&nbsp;&nbsp;&nbsp;Product Options</legend>
<div id="faq2" class="icongroup1">
<?php

if ($_GET[sub_product])
	{
	echo("<p><label><span>Option Name</span></label>$result[att_name]&nbsp;</p>");
	echo("<input name='att_name' type='hidden' value='$result[att_name]' />");
	}
else
	{
	?>
	<p><label for="att_name"><span>Option Name</span></label>
	<input name="att_name" id="att_name" type="text"/>
	</p>
<?php
	}
?>

<p><label for="att_value"><span>Option Value</span></label>
<input name="att_value" id="att_value" type="text"/>
</p>
</div>
</fieldset>

<p style="text-align:right"><small><a href="javascript:faq.sweepToggle('contract')">Contract All</a> | <a href="javascript:faq.sweepToggle('expand')">Expand All</a></small></p>

<fieldset>
<legend><span id="faq3-title" class="iconspan"><img src="/assets/images/icons/minus.gif" alt="" /></span>&nbsp;&nbsp;&nbsp;Product Image</legend>
<div id="faq3" class="icongroup1">
<p><label for="filename"><span>Select File</span></label><input type="file" name="imagefile" id="filename"/></p>
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
<textarea name="search_terms" rows="5" cols="50"><?php echo("$result[search_terms]"); ?></textarea></p>

<p><label for="page_name"><span>SEO page name</span></label>
<input name="page_name" id="page_name" type="text" size="50"/></p>

<input name="preview_size" type="hidden" value="580" />
<input name="thumb_size" type="hidden" value="100" />
<input name="water" type="hidden" value="none"/>
<input name="uploaded" type="hidden" value="hellyeah" />
</div>
</fieldset>

<p style="text-align:right"><small><a href="javascript:faq.sweepToggle('contract')">Contract All</a> | <a href="javascript:faq.sweepToggle('expand')">Expand All</a></small></p>

<fieldset>
<legend><span id="faq5-title" class="iconspan"><img src="/assets/images/icons/minus.gif" alt="" /></span>&nbsp;&nbsp;&nbsp;Related Products</legend>
<div id="faq5" class="icongroup1">
<div class="scroll"><p>
        <?php

$catquery = mysql_query("select * from $product_table where sub is NULL order by name asc");
while ($catresult = mysql_fetch_array($catquery))
	{
	echo("<label><input name='product".$catresult[id]."' type='checkbox' value='".$catresult[id]."' $checked/>&nbsp;&nbsp;".$catresult[name]."</label><br/>");
	}
?>
      </p></div>
	  </div>
</fieldset>
<p></p>
<div id="right"><input type="submit" name="Submit" value="Add Product" /></div>
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
