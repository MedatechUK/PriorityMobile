<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
<title>Admin Interface</title>
<link rel="stylesheet" href="../assets/css/bitesize.css" type="text/css" media="screen" />
</head>
<body>
<div id="bitesize">
<?php

//include charts.php to access the InsertChart function
include "../assets/widgets/charts/charts.php";

if ($_GET[type] == "orders")
	{
	echo InsertChart ( "../assets/widgets/charts/charts.swf", "../assets/widgets/charts/charts_library", "widget_chart_data_orders.php?year=".$_GET[year]."",750,550, "FFFFFF", false );
	}
elseif ($_GET[type] == "average_order")
	{
	echo InsertChart ( "../assets/widgets/charts/charts.swf", "../assets/widgets/charts/charts_library", "widget_chart_data_avgorder.php?year=".$_GET[year]."",750,550, "FFFFFF", false );
	}
elseif ($_GET[type] == "pp")
	{
	echo InsertChart ( "../assets/widgets/charts/charts.swf", "../assets/widgets/charts/charts_library", "widget_chart_data_purchased.php",750,550, "FFFFFF", false );
	}
elseif ($_GET[type] == "pop")
	{
	echo InsertChart ( "../assets/widgets/charts/charts.swf", "../assets/widgets/charts/charts_library", "widget_chart_data_popular.php",750,550, "FFFFFF", false );
	}
else
	{
	echo InsertChart ( "../assets/widgets/charts/charts.swf", "../assets/widgets/charts/charts_library", "widget_chart_data.php?year=".$_GET[year]."",750,550, "FFFFFF", false );
	}



?>
</div>
</body>
</html>
