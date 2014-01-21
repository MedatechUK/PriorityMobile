<?php
session_start();

//IF THE USER IS RETURNING FROM A COMPLETE ORDER CLEAR SESSION
if ($_GET[clear] == "all")
	{
	session_destroy();
	}

require_once('assets/widgets/mysql.class.php');
require_once('assets/widgets/global.inc.php');
require_once('assets/widgets/functions.inc.php');
require("assets/widgets/global_variables.php");

$page_query = mysql_query("select * from SHOP1_content where id='11'");
$page_result = mysql_fetch_array($page_query);
?>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
<title><?php echo $page_result[pagetitle] ?></title>
<meta name="keywords" content="<?php echo $page_result[metatags] ?>" />
<meta name="description" content="<?php echo $page_result[metadesc] ?>" />
<link rel="stylesheet" href="assets/css/screen.css" type="text/css" media="screen" />
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
	<a href="/index.php" class="homelink" title="Home Page"><img src="assets/images/clear.gif" alt="Link to Home Page" width="300px" height="50px"/></a>
<?php echo writeShoppingCart(); ?>
  </div>
  <div id="navigation">
    <?php require("assets/widgets/nav.php"); ?>
  </div>
  <div id="main_content">
  <?php 
	echo("$site_notice");
	?>
    <h2>Personal attack alarms</h2>
	<p><strong>All our personal alarms come fitted with batteries and are covered by our 1 year guarantee</strong></p>
	<?php
	
	$query = mysql_query("select * from SHOP1_products where sub IS NULL");
	while ($result = mysql_fetch_array($query))
		{
		echo("<div class='product'>
		<h3>$result[name]</h3>
		<a href='product_info.php?product_id=$result[id]' title='View more information about $result[name]'><img src='$result[thumb]' alt='An image of $result[name]'/></a>
		$result[summary]
		<p class='info'><a href='product_info.php?product_id=$result[id]' title='View more information about $result[name]'>More information</a></p>");
		
		if($result[price_was])
			{
			echo("<p class='was'>Was &pound;$result[price_was]</p>");
			echo("<p class='now'>Now &pound;$result[price]</p>");
			}
		else
			{
			echo("<p class='price'>&pound;$result[price]</p>");
			}
			
			echo("<form action='basket.php?action=add' method='post'>");
			
			
		$att_query = mysql_query("select * from SHOP1_products where sub = '$result[id]' and stock = 'YES'");
		$num_rows = mysql_num_rows($att_query);
		
		if ($num_rows > 0)
			{
			echo("<label>$result[att_name]&nbsp;&nbsp;
			<select name='id'>
			<option value='$result[id]'>$result[att_value] (&pound;$result[price])</option>
			");
			while ($att_result = mysql_fetch_array($att_query))
				{
				echo("<option value='$att_result[id]'>$att_result[att_value] (&pound;$att_result[price])</option>");
				}
			echo("</select></label>");
			
			}
		else
			{
			echo("<input type='hidden' name='id' value='$result[id]'/>");
			}
		
		if ($result[stock] == "NO")
			{
			echo('<span style="color:#F00"><strong>OUT OF STOCK</strong><br/><a href="stock_update.php?product_id='.$result[id].'" title="Email me when back in stock">EMAIL ME WHEN BACK IN STOCK</a></span></form>');
			}
		else
			{
			echo('<input name="submit" type="image" value="Submit" src="assets/images/buy_now.gif" alt="Buy Now" /></form>');
			}
		
		
		echo("</div>");
		
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
