<?php 

// Include MySQL class
require_once('../widgets/mysql.class.php');
// Include database connection
require_once('../widgets/global.inc.php');
?>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
<link rel="shortcut icon" href="http://www.personal-attack-alarms.net/favicon.ico" />
<title>Deliveries and Returns</title>

<style type="text/css">
<!--
body{font-family:Verdana, Arial, Helvetica, sans-serif;padding: 10px;font-size: 70%;background: #fff;}
#basketconfirm {border-collapse: collapse; width: 100%; border: 1px solid #999;font-size: 90%; }
#basketconfirm th {padding: 3px; background:url(../assets/images/product_h3_bg.gif) #acd700; text-align:left}
#basketconfirm td {padding: 3px;}
#basketconfirm a {color: #F00; }
#basketconfirm a:hover {color: #F00; text-decoration: none;}
#basketconfirm .subtotal {background: #ededed;text-align: right; border-top: 3px double #999;}
#basketconfirm .postage {background: #ededed;text-align: right;}
#basketconfirm .vat {background: #ededed;text-align: right;}
#basketconfirm .total {background: #ededed; text-align: right; font-size: 150%;}
.close {float: right;}
.close img {border:none}
-->
</style>
</head>
<body>

<script type="text/javascript" language="JavaScript">
<!--
document.write('<p class="close"><a href="#" onClick="javascript:window.close();">');
document.write('<img src="../images/close_button.gif" alt="Close Window" />');
document.write('</a></p>');
//-->
</script>

<img src="http://www.personal-attack-alarms.net/assets/images/logo.gif" alt="Personal Attack Alarms Logo" class='logo'/>
<?php

$page_query = mysql_query("select * from SHOP1_content where id='3'");
while($page_result = mysql_fetch_array($page_query))
	{
echo("$page_result[content]");
}
?>

<script type="text/javascript" language="JavaScript">
<!--
document.write('<p class="close"><a href="#" onClick="javascript:window.close();">');
document.write('<img src="../images/close_button.gif" alt="Close Window" />');
document.write('</a></p>');
//-->
</script>
</body>
</html>