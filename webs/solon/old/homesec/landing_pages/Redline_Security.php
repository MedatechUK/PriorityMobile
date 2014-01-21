<?php

require_once('../assets/widgets/mysql.class.php');

require_once('../assets/widgets/global.inc.php');

require_once('../assets/widgets/functions.inc.php');

//==========================================================================



$refurl = $_SERVER[HTTP_REFERER];

$refdate = date("d-m-y G:i:s");

$refip = $_SERVER[REMOTE_ADDR];



$trackit = mysql_query("insert into $tracker_table (url, date, ip, campaign) values ('$refurl', '$refdate', '$refip', 'Redline Security')");



header("location: ../index.php");

?>
