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




header("Content-type: application/x-msdownload");

if ($_GET[specify] == "yes")
	{
	header("Content-Disposition: attachment; filename=PAA-Orders_$_POST[s_day]-$_POST[s_month]-$_POST[s_year]_$_POST[e_day]-$_POST[e_month]-$_POST[e_year].xls");
	$headings="Orders between $_POST[s_day]-$_POST[s_month]-$_POST[s_year] and $_POST[e_day]-$_POST[e_month]-$_POST[e_year]\nDate\tOrder Number\tEmail\tStatus\tOrder Total\tPostage\tName\tST Reference\t";
	}
else
	{
	header("Content-Disposition: attachment; filename=PAA-Orders-All.xls");
	$headings="Date\tTime\tOrder Number\tEmail\tStatus\tOrder Total\tPostage\tName\tST Reference\t";
	}


header("Pragma: no-cache");
header("Expires: 0");  
print "$headings\n";
if ($_GET[specify] == "yes")
	{
	
	
	
	$detailsa = mysql_query("select * from SHOP1_orders where date >= '$_POST[s_year]$_POST[s_month]$_POST[s_day]235959' and date <= '$_POST[e_year]$_POST[e_month]$_POST[e_day]235959' ORDER BY date DESC");
	$tracker=mysql_query("insert into SHOP1_admin_tracker (name, loggedin, ip, job) values ('$_COOKIE[user_name]', '$tracker_time', '$ip', 'Orders - exported')");
	}
else
	{
	$detailsa = mysql_query("select * from SHOP1_orders ORDER BY date DESC");
	}

while($details = mysql_fetch_array($detailsa))
{
	$status = mysql_query("select * from SHOP1_order_status where order_id = '$details[id]' order by id desc limit 1 ");
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



$order_val 	= $details[total_cost];
$order_val 	= str_replace("£", "", $order_val);
$total 	= $order_val - $details[postage];
$billing 	= explode("<br/>", $details[billing_address]);
$date		= date("d/m/Y", strtotime($details[date]));
$time		= date("H:i:s", strtotime($details[date]));;

print"".$date."\t".$time."\t".$details[order_number]."\t".$details[email]."\t".$order_status."\t".number_format($total, '2', '.', '')."\t".$details[postage]."\t".$billing[0]."\t".$details[streference]."\n";}


?>