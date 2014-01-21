<?php
$host 					= 	'localhost';
$user 					= 	'paalarms';
$pass 					= 	'477ack';
$database				= 	'PAALARMS';
$tracker_table			=	'SHOP1_tracker';

mysql_connect($host, $user, $pass);
mysql_select_db($database);
//==========================================================================

$refurl = $_SERVER['HTTP_REFERER'];
$refdate = date("d-m-y G:i:s");
$refip = $_SERVER['REMOTE_ADDR'];

$trackit = mysql_query("insert into $tracker_table (url, date, ip, campaign) values ('$refurl', '$refdate', '$refip', 'Christmas Email')");



header("location: ../products.php");
?>