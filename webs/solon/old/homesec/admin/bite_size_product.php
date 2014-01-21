<?php 
// SECURITY CHECK TO MAKE SURE THE CORRECT USERNAME AND PASSWORD HAS BEEN ENTERED
if(!$_COOKIE[user_name] | !$_COOKIE[user_pwd])
{
header("location: login.php");
exit;
}

// Include MySQL class
require_once('../assets/widgets/mysql.class.php');
// Include database connection
require_once('../assets/widgets/global.inc.php');
// Include functions
require_once('../assets/widgets/functions.inc.php');
?>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
<title>Admin Interface</title>
<link rel="stylesheet" href="../assets/css/bitesize.css" type="text/css" media="screen" />
<link rel="stylesheet" href="../assets/scripts/modal/css/moodalbox.css" type="text/css" media="screen" />
<script type="text/javascript" src="../assets/scripts/modal/js/mootools.js"></script> 
<script type="text/javascript" src="../assets/scripts/modal/js/moodalbox.js"></script> 
<script language="javascript" src="../assets/scripts/calender/cal2.js"></script>
<script language="javascript" src="../assets/scripts/calender/cal_conf2.js"></script>
<script language="javascript" type="text/javascript" src="../assets/scripts/lib.js"></script>
<script language="javascript" type="text/javascript" src="../assets/scripts/popup.js"></script>
<script languagee="javascript">

<!-- Begin
function popUp(URL) {
day = new Date();
id = day.getTime();
eval("page" + id + " = window.open(URL, '" + id + "', 'toolbar=0,scrollbars=1,location=0,statusbar=0,menubar=0,resizable=0,width=600,height=400,left = 440,top = 212');");
}
// End -->
</script>

</head>
<body>
<div id="bitesize">
<?php 
	echo("$site_notice");
	
	$query = mysql_query("select * from $product_table where id = '$_GET[product_id]'");
	while ($result = mysql_fetch_array($query))
		{
		
		$img_query = mysql_query("select * from $gallery_table where doc_cat = '$result[id]'");
		$img_result = mysql_fetch_array($img_query);
		$img_rows  = mysql_num_rows($img_query);
			if ($img_rows > 0)
				{
				$medium_image = "<img src='../assets/images/products/thumbs/$img_result[name]' alt='An image of $result[name]'/>";
				}
			else	
				{
				$medium_image = "<img src='../assets/images/products/no_thumb.gif' alt='No image available'/>";
				}
		
		echo("
		
		<div class='product_info'>
		<h2>$result[name]</h2>
		<div class='product_image'>$medium_image</div>
		<p class='product_code'>Product code: <strong>$result[code]</strong></p>		
		<p class='product_code'>
		<p class='price'>&pound;$result[price] each</p>");
		
		if ($result[dis10] != $result[price])
			{
			echo("<p class='sub_price'>Buy 10+ for &pound;$result[dis10] each</p>");
			}
		if ($result[dis20] != $result[price])
			{
			echo("<p class='sub_price'>Buy 20+ for &pound;$result[dis20] each</p>");
			}
		if ( $result[dis50] != $result[price])
			{
			echo("<p class='sub_price'>Buy 50+ for &pound;$result[dis50] each</p>");
			}
		
				
		$att_query = mysql_query("select * from $product_table where sub = '$result[id]' and stock != 'NO'");
		$num_rows = mysql_num_rows($att_query);
		
		
				
		if ($result[stock] == "NO")
			{echo('<p><strong style="color:#F00">This product is currently out of stock.</strong></p>');
			}
			echo('<div class="product_text">'.$result[text].'</div>');
			
		
		
		}
		
	?>

</div>
</body>
</html>
