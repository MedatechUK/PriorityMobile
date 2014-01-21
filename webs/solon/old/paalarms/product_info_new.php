<?php
session_start();

//IF THE USER IS RETURNING FROM A COMPLETE ORDER CLEAR SESSION
if ($_GET[clear] == "all")
	{
	session_destroy();
	}

// Include MySQL class
require_once('assets/widgets/mysql.class.php');
// Include database connection
require_once('assets/widgets/global.inc.php');
// Include functions
require_once('assets/widgets/functions.inc.php');
require("assets/widgets/global_variables.php");
?>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />

<?php 
$query = mysql_query("select * from SHOP1_products where id = '$_GET[product_id]'");
while ($result = mysql_fetch_array($query))
	{
	?>
<title><?php echo $result[name] ?></title>
<meta name="keywords" content="<?php echo $result[keywords] ?>" />
<meta name="description" content="<?php echo $result[description] ?>" />
	<?php
	}
?>

<link rel="stylesheet" href="assets/css/screen_new.css" type="text/css" media="screen" />
<link rel="stylesheet" href="assets/css/print.css" type="text/css" media="print" />
<link rel="shortcut icon" href="/favicon.ico" />
</head>
<body>
<?php 
echo("$js_notice");
?>
<div id="hidden">
  <?php require("assets/widgets/hidden.php"); ?>
</div>
<div id="container">
  <div id="header">
    <h2><?php echo $site_name; ?></h2>
	<a href="/index.php" class="homelink" title="Home Page"><img src="assets/images/clear.gif" alt="" width="300px" height="50px"/></a>
<?php echo writeShoppingCart(); ?>
  </div>
  <div id="navigation">
    <?php require("assets/widgets/nav.php"); ?>
  </div>
  <div id="main_content">
   <?php 
	echo("$site_notice");
	
	$query = mysql_query("select * from SHOP1_products where id = '$_GET[product_id]'");
	while ($result = mysql_fetch_array($query))
		{
		echo("<div class='product_info'>
		<h2>$result[name]</h2>
		<div class='product_image'>");
		
if ($result[image_a])
	{
	echo('<div class="gallerycontainer">

	<a class="thumbnail" href="#thumb">
	<img src="assets/images/products/thumbs/'.$result[image_a].'" border="0" />
	<span><img src="assets/images/products/large/'.$result[image_a].'"" />
	<br/><small>Key not included</small></span></a>');

	if ($result[image_b])
		{
		echo('<a class="thumbnail" href="#thumb">
		<img src="assets/images/products/thumbs/'.$result[image_b].'" border="0" />
		<span><img src="assets/images/products/large/'.$result[image_b].'"" />
		<br/><small>Bag not included</small></span></a>');
		}

	echo("</div>");
	}
	echo("<img src='$result[large]' alt='An image of $result[name]' class='main_img'/></div>");
		
		
		
		
		
		echo("<p>Product code: <strong>$result[code]</strong></p>		
		$result[text]
		<p class='price'>&pound;$result[price]</p>
		<form action='basket.php?action=add' method='post'>");
		
		$att_query = mysql_query("select * from SHOP1_products where sub = '$result[id]'");
		$num_rows = mysql_num_rows($att_query);
		
		if ($num_rows > 0)
			{
			echo("<p><label>$result[att_name]&nbsp;&nbsp;
			<select name='id'>
			<option value='$result[id]'>$result[att_value] (&pound;$result[price])</option>
			");
			while ($att_result = mysql_fetch_array($att_query))
				{
				echo("<option value='$att_result[id]'>$att_result[att_value] (&pound;$att_result[price])</option>");
				}
			echo("</select></label></p>");
			
			}
		else
			{
			echo("<input type='hidden' name='id' value='$result[id]'/>");
			}
		
		
		
		echo('<a href="products.php" title="Click here to go back to product list"><img src="assets/images/back_button.gif" alt="Back to product list"/></a> <input name="submit" type="image" value="Submit" src="assets/images/add_to_basket.gif" alt="Add to Basket" /></form>');
		echo("<p class='buttons'><a href='tell_a_friend.php' title='Click here to tell a friend about this product'>Tell a friend about this product</a></p></div>");
	
	
		

	
	
	
	
	
	
	
	
	
	
	
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
<script src="http://www.google-analytics.com/urchin.js" type="text/javascript">
</script>
<script type="text/javascript">
_uacct = "UA-514855-9";
_udn="none";
_ulink=1;
urchinTracker();
</script>
</body>
</html>
