<?php 
// SECURITY CHECK TO MAKE SURE THE CORRECT USERNAME AND PASSWORD HAS BEEN ENTERED
if(!$user_name | !$user_pwd)
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
<link rel="shortcut icon" href="http://www.personal-attack-alarms.net/favicon.ico" />
<link rel="stylesheet" href="../assets/css/admin.css" type="text/css" media="screen" />
</head>
<body>
<noscript>
<h1>Warning</h1>
<p class="noscript">To use this site correctly you need to have JavaScript enabled on your web browser</p>
</noscript>
<div id="hidden">
  <?php require("../assets/widgets/admin_hidden.php"); ?>
</div>
<div id="container">
  <div id="header">
    <h2>Website</h2>
  </div>
  <div id="navigation">
    <?php 
	// THIS PULLS IN THE ADMIN LINKS
	require("widget_links.php"); 
	?>
  </div>
  <div id="main_content">
      <?php


	if (!($limit)){
$limit = 80;} // Default results per-page.
if (!($page)){
$page = 0;} // Default page value.

$numresults = mysql_query("select * from SHOP1_orders WHERE order_number LIKE '%$_GET[search]%' || email LIKE '%$_GET[search]%' || total_cost LIKE '%$_GET[search]%' || orderinfo LIKE '%$_GET[search]%' || date LIKE '%$_GET[search]%' || delivery_address LIKE '%$_GET[search]%' || billing_address LIKE '%$_GET[search]%' || streference LIKE '%$_GET[search]%'"); // the query.



$numrows = mysql_num_rows($numresults); // Number of rows returned from above query.

$pages = intval($numrows/$limit); // Number of results pages.

// $pages now contains int of pages, unless there is a remainder from division.

if ($numrows%$limit) {
$pages++;} // has remainder so add one page

$current = ($page/$limit) + 1; // Current page number.

if (($pages < 1) || ($pages == 0)) {
$total = 1;} // If $pages is less than one or equal to 0, total pages is 1.

else {
$total = $pages;} // Else total pages is $pages value.

$first = $page + 1; // The first result.

if (!((($page + $limit) / $limit) >= $pages) && $pages != 1) {
$last = $page + $limit;} //If not last results page, last result equals $page plus $limit.
 
else{
$last = $numrows;} // If last results page, last result equals total number of results.

	echo("<h2>Order Information</h2>");
	echo("<p><strong>$numrows results found for: $_GET[search]</strong></p>");
	echo("<div id='orders'>
	<table width='90%'>
		<thead>
			<tr><th>Order Ref.</th><th>Name</th><th>Value</th><th>Date</th><th>Status</th><th>Options</th></tr>
		</thead>
		<tbody>
	
	");
	$subtotal = 0;
	$query = mysql_query("select * from SHOP1_orders WHERE order_number LIKE '%$_GET[search]%' || email LIKE '%$_GET[search]%' || total_cost LIKE '%$_GET[search]%' || orderinfo LIKE '%$_GET[search]%' || date LIKE '%$_GET[search]%' || delivery_address LIKE '%$_GET[search]%' || billing_address LIKE '%$_GET[search]%' || streference LIKE '%$_GET[search]%' ORDER BY id DESC");
	while($result = mysql_fetch_array($query))
		{
		
		$order_val = explode("£", $result[total_cost]);
		
		$subtotal += $order_val[1];
		
		$date = date("d/m/Y H:i:s", strtotime($result[date]));
		$billing = explode("<br/>", $result[billing_address]);
		echo("<tr class='row2'><td>$result[order_number]</td><td>$billing[0]</td><td>$result[total_cost]</td><td>$date</td><td>");
		
		
	$statusquery = mysql_query("select * from SHOP1_order_status where order_id = '$result[id]' order by id desc limit 1 ");
	$numrows = mysql_num_rows($statusquery);
	
	if ($numrows > 0)
		{
		while($statusresult = mysql_fetch_array($statusquery))
			{
			echo("$statusresult[action]");
			}
		}
	else
		{
		echo("Pending");
		}
		
		echo("</td><td><a href='orders.php?id=$result[id]' class='view_order'>View</a></td></tr>");		
		}
	echo("<tr class='summary'><td colspan='7'>Sum of above orders: <strong>&pound;".number_format($subtotal, 2, '.', '')."</strong></td>");
	
	echo("</tbody></table></div>");
	
	echo("<div class='pages'><p>");


if ($page != 0) { // Don't show back link if current page is first page.
$back_page = $page - $limit;
echo("<a href='$PHP_SELF?page=$back_page&amp;limit=$limit'>previous page</a> ");}

for ($i=1; $i <= $pages; $i++) // loop through each page and give link to it.
{
 $ppage = $limit*($i - 1);
 if ($ppage == $page){
 echo("<strong>$i</strong> ");} // If current page don't give link, just text.
 else{
 echo("<a href='$PHP_SELF?page=$ppage&amp;limit=$limit'>$i</a> ");}
}

if (!((($page+$limit) / $limit) >= $pages) && $pages != 1) { // If last page don't give next link.
$next_page = $page + $limit;
echo(" <a href='$PHP_SELF?page=$next_page&amp;limit=$limit'>next page</a>");}
	
	echo("</p></div>");
	
	?>
	<form method="get" action="search_orders.php">
  
  <p><strong>Search for an order</strong></p>
    <label>Keyword
    <input type="text" name="search" />
    </label>
    <input type="submit" name="Submit" value="Search" />
	</form>
</div>
  <div id="footer">
  <?php 
	// THIS PULLS IN THE FOOTER
	require("../assets/widgets/footer.php"); 
	?>
  </div>
</div>
</body>
</html>
