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

$headings="Product\tCode\tPrice\t";

header("Content-type: application/x-msdownload");
header("Content-Disposition: attachment; filename=Home-Security-Products-All.xls");


header("Pragma: no-cache");
header("Expires: 0");  
print "$headings\n";
$detailsa = mysql_query("select * from $product_table ORDER BY name asc");
while($details = mysql_fetch_array($detailsa))
{
print"".$details[name]."\t".$details[code]."\t".number_format($details[price], '2', '.', '')."\t".number_format($details[dis10], '2', '.', '')."\t".number_format($details[dis20], '2', '.', '')."\t".number_format($details[dis50], '2', '.', '')."\n";
}

?>