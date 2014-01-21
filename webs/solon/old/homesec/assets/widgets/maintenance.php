<?php
// CHECK TO SEE IF SITE SHOULD BE CLOSED
if (!$thispage)
	{
	$closed = mysql_query("select * from $config_table where id='1'");
	$closed_result = mysql_fetch_array($closed);
	if ($closed_result[status] == "Y")
		{
		header("location: /maintenance.php");
		}
	}
//
//if (!$_COOKIE[admin])	{
//	header("location:/maintenance.php");
//}
?>