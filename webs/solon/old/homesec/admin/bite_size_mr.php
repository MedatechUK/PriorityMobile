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
</head>
<body>
<div id="bitesize">
<?php
$query = mysql_query("select * from $report_monthly_table where month = '$_GET[month]' and year = '$_GET[year]'");
	$numrows = mysql_num_rows($query);
	if ($numrows > 0) // GET EXISTING ENTRY
		{
		$result = mysql_fetch_array($query);
		$text_month = date("F", mktime(0, 0, 0, $result[month], 1, $result[year]));
		echo("<h2>Costings for $text_month $result[year]</h2>");
		echo("<p>Costs: &pound;".number_format($result[costs], 2, '.', '')."</p>");
		echo("<h3>Notes</h3>$result[notes]");
		}
	else
		{
		$text_month = date("F", mktime(0, 0, 0, $_GET[month], 1, $_GET[year]));
		echo("<h2>Costings for $text_month $_GET[year]</h2>");
		echo("<p>There is no data for this month</p>");
		}
?>
</div>
</body>
</html>
