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

$total = 0;


$headings="Orders between $_POST[s_day]-$_POST[s_month]-$_POST[s_year] and $_POST[e_day]-$_POST[e_month]-$_POST[e_year]\nDate\tOrder Number\tEmail\tStatus\tOrder Total\tPostage\tName\tST Reference\t";

header("Content-type: application/x-msdownload");

if ($_GET[specify] == "yes")
	{
	header("Content-Disposition: attachment; filename=Home-Security-Store-Orders_$_POST[s_day]-$_POST[s_month]-$_POST[s_year]_$_POST[e_day]-$_POST[e_month]-$_POST[e_year].xls");
	}
else
	{
	header("Content-Disposition: attachment; filename=Home-Security-Store-Orders-All.xls");
	}


header("Pragma: no-cache");
header("Expires: 0");  
print "$headings\n";
if ($_GET[specify] == "yes")
	{
	
	
	
	$detailsa = mysql_query("select * from $orders_table where date >= '$_POST[s_year]$_POST[s_month]$_POST[s_day]235959' and date <= '$_POST[e_year]$_POST[e_month]$_POST[e_day]235959' ORDER BY date DESC");
	$tracker=mysql_query("insert into $admin_track_table (name, loggedin, ip, job) values ('$_COOKIE[user_name]', '$tracker_time', '$ip', 'Orders - exported')");
	}
else
	{
	$detailsa = mysql_query("select * from $orders_table ORDER BY date DESC");
	}

while($details = mysql_fetch_array($detailsa))
{
	$status = mysql_query("select * from $order_status_table where order_id = '$details[id]' order by id desc limit 1 ");
	$numrows = mysql_num_rows($status);
	
	if ($numrows > 0)
		{
		$order_status = mysql_fetch_array($status);
		$order_status = $order_status[action];
		}
	else
		{
		$order_status = "Pending";
		}
		

$order_val = $details[total_cost];
$total += $order_val;
$billing = explode("<br/>", $details[billing_address]);

print"".$details[date]."\t".$details[order_number2]."\t".$details[email]."\t".$order_status."\t".number_format($order_val, '2', '.', '')."\t".$details[postage]."\t".$billing[0]."\t".$details[streference]."\n";}


?>