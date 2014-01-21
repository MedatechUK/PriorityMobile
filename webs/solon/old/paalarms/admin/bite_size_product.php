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
<link rel="shortcut icon" href="http://www.personal-attack-alarms.net/favicon.ico" />
<title>Standard Paragraphs</title>

<style type="text/css">
<!--
body{font-family:Verdana, Arial, Helvetica, sans-serif;padding: 10px;font-size: 90%;background: #fff;}
#abasketconfirm {border-collapse: collapse; width: 100%; border: 1px solid #999;font-size: 90%; }
#abasketconfirm th {padding: 3px; background:url(../assets/images/product_h3_bg.gif) #acd700; text-align:left}
#abasketconfirm td {padding: 3px;}
#abasketconfirm a {color: #F00; }
#abasketconfirm a:hover {color: #F00; text-decoration: none;}
#abasketconfirm .subtotal {background: #ededed;text-align: right; border-top: 3px double #999;}
#abasketconfirm .postage {background: #ededed;text-align: right;}
#abasketconfirm .vat {background: #ededed;text-align: right;}
#abasketconfirm .total {background: #ededed; text-align: right; font-size: 150%;}
.close {float: right;}
.close img {border:none}
-->
</style>
</head>
<body>
<script type="text/javascript" language="JavaScript">
<!--
document.write('<p class="close"><a href="#" onClick="javascript:window.close();">');
document.write('<img src="../assets/images/close_button.gif" alt="Close Window" />');
document.write('</a></p>');
//-->
</script>
<?php 
	
	$query = mysql_query("select * from SHOP1_products where id = '$_GET[product_id]'");
	while ($result = mysql_fetch_array($query))
		{
		
		echo("<img src='$result[large]' alt='An image of $result[name]' />");
		
		echo("
		
		<div class='product_info'>
		<h2>$result[name]</h2>
		<div class='product_image'>$medium_image</div>
		<p class='product_code'>Product code: <strong>$result[code]</strong></p>		
		<p class='product_code'>
		<p class='price'>&pound;$result[price] each</p>");
						
		$att_query = mysql_query("select * from SHOP1_products where sub = '$result[id]' and stock != 'NO'");
		$num_rows = mysql_num_rows($att_query);
		
		
				
		if ($result[stock] == "NO")
			{echo('<p><strong style="color:#F00">This product is currently out of stock.</strong></p>');
			}
			echo('<div class="product_text">'.$result[text].'</div>');
			
		
		
		}
		
	?>

<script type="text/javascript" language="JavaScript">
<!--
document.write('<p class="close"><a href="#" onClick="javascript:window.close();">');
document.write('<img src="../assets/images/close_button.gif" alt="Close Window" />');
document.write('</a></p>');
//-->
</script>
</body>
</html>
