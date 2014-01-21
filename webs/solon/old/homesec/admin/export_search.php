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
require_once('../assets/widgets/admin_functions.inc.php');
require_once('../assets/widgets/global_variables.php');

$headings="Date\tSearch Term\tProducts\tIP Address\n";

header("Content-type: application/x-msdownload");
header("Content-Disposition: attachment; filename=Home-Security-Search-Terms.xls");


header("Pragma: no-cache");
header("Expires: 0");  
print "$headings\n";
$detailsa = mysql_query("select * from $search_table order by id desc");
while($details = mysql_fetch_array($detailsa))
{
$date = date("d/m/Y H:i:s", strtotime($details[date]));
print"".$date."\t".$details[search_term]."\t".$details[products]."\t".$details[ip]."\n";
}

?>