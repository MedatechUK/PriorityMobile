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

$headings="Title\tMpn\tPrice\tDescription\tLink\tBrand\tCondition\tISBN\tProduct Type\tUPC\tImage link\tCurrency\n";

header("Content-type: application/x-msdownload");
header("Content-Disposition: attachment; filename=Home-Security-Products-Base.xls");


header("Pragma: no-cache");
header("Expires: 0");  
print "$headings\n";
$detailsa = mysql_query("select * from $product_table where sub is NULL and product_active = 'YES' ORDER BY name asc");
while($details = mysql_fetch_array($detailsa))
{


$div = '<div class="description">';


$currency 		= "GBP";
$product_type	= "Products";
$brand			= "";
$isbn			= "";
$upc			= "";
$condition		= "new";

$product_url 	= $site_url."product_info.php?product_id=".$details[id];
$summary 		= str_replace("<ul>", "", $details[description]);
$summary 		= str_replace("</ul>", "", $summary);
$summary 		= str_replace("<li>", "", $summary);
$summary 		= str_replace("<br/>", "", $summary);
$summary 		= str_replace("<br />", "", $summary);
$summary 		= str_replace("<br/>", "", $summary);
$summary 		= str_replace("</li>", ".", $summary);
$summary 		= str_replace($div, "", $summary);
$summary 		= str_replace("</div>", "", $summary);
$summary		= '"'.$summary.'"';


$img_query 		= mysql_query("select * from $gallery_table where doc_cat = '$details[id]'");
$img_result 	= mysql_fetch_array($img_query);
$img_rows  		= mysql_num_rows($img_query);


if ($img_rows > 0)
	{
	$image_link = $site_url."assets/images/products/small/$img_result[name]";
	}


print"".$details[name]."\t".$details[code]."\t".number_format($details[price], '2', '.', '')."\t".$summary."\t".$product_url."\t".$brand."\t".$condition."\t".$isbn."\t".$product_type."\t".$upc."\t".$image_link."\t".$currency."\n";
}

?>