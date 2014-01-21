<?php

//---------------------------------------------------------------------------
$database		=	"HOMESEC";
$host 			= 	'mysql.home-security-store.co.uk';
$user 			= 	'homesec';
$pass 			= 	'h1zzle';

$product_table	= 	"SHOP2_products";

//==========================================================================

  mysql_connect($host, $user, $pass);
  mysql_select_db($database);

/*
note:
this is just a static test version using a hard-coded countries array.
normally you would be populating the array out of a database

the returned xml has the following structure
<results>
	<rs>foo</rs>
	<rs>bar</rs>
</results>
*/

header ("Expires: Mon, 26 Jul 1997 05:00:00 GMT"); // Date in the past
header ("Last-Modified: " . gmdate("D, d M Y H:i:s") . " GMT"); // always modified
header ("Cache-Control: no-cache, must-revalidate"); // HTTP/1.1
header ("Pragma: no-cache"); // HTTP/1.0

$aResults = array();
$query = mysql_query("SELECT * FROM $product_table order by name asc");

while ($row = mysql_fetch_array($query))
	{
	$aResults[] = $row['id'];
	//$aResults[1] = $row['code'];
	//$aResults[2] = $row['price'];
	} 


if (isset($_REQUEST['json']))
	{
		header("Content-Type: application/json");
		$b = 1;
		echo "{\"results\": [";
		$arr = array();
		for ($i=0;$i<count($aResults);$i++)
		{
			$query = mysql_query("SELECT * FROM $product_table where id = '$aResults[$i]'");
			while ($result = mysql_fetch_array($query))
				{
				$arr[] = "{\"id\": \"".$result[id]."\", \"value\": \"".$result[name]." | ".$result[code]."\", \"info\": \"\"}";
				$b++;
				}
		}
		echo implode(", ", $arr);
		echo "]}";
	}
	else
	{
		
		header("Content-Type: text/xml");
		$b = 1;
		echo "<?xml version=\"1.0\" encoding=\"utf-8\" ?><results>";
		for ($i=0;$i<count($aResults);$i++)
		{
			echo "<rs id=\"".$b."\" info=\"".$aResults[$i]['id']."\">".$aResults[$i]."</rs>";
			$b++;
		}
		echo "</results>";
	}
?>