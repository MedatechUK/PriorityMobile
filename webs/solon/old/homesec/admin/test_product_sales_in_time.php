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
function padOne($strToPad){
	if (strlen($strToPad)==1){
		$strToPad="0".$strToPad;
		
	}
	return $strToPad;
}

if(isset($_POST['formsubmit'])){
	$fromDay = padOne($_POST['fromday']);
	$fromMonth= padOne($_POST['frommonth']);
	$fromYear=$_POST['fromyear'];
	$toDay= padOne($_POST['today']);
	$toMonth= padOne($_POST['tomonth']);
	$toYear=$_POST['toyear'];
	if(!(checkdate($fromMonth  , $fromDay  , $fromYear))){
		$errMessage="The from date is not valid.<br>";
	}
	if(!(checkdate($toMonth  , $toDay  , $toYear))){
		$errMessage.="The to date is not valid.<br>";
	}
	if(strlen($errMessage)<1){
		$fromDate=$fromYear."-".$fromMonth."-".$fromDay;
		$toDate=$toYear."-".$toMonth."-".$toDay;
		
		$showFromDate=$fromDay."-".$fromMonth."-".$fromYear;
		$showToDate=$toDay."-".$toMonth."-".$toYear;
	}else{
		$fromDate="2009-01-01";
		$toDate="2009-09-01";
		
		$showFromDate="01-01-2009";
		$showToDate="01-09-2009";
	}
	
}else{
	$fromDate="2009-01-01";
	$toDate="2009-09-01";
		
	$showFromDate="01-01-2009";
	$showToDate="01-09-2009";
}
?>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
<title>Admin Interface</title>
<link rel="shortcut icon" href="../assets/images/favicon.ico" />
<link rel="stylesheet" href="../assets/css/admin.css" type="text/css" media="screen" />
<link rel="stylesheet" href="../assets/scripts/modal/css/moodalbox.css" type="text/css" media="screen" />
<script type="text/javascript" src="../assets/scripts/modal/js/mootools.js"></script> 
<script type="text/javascript" src="../assets/scripts/modal/js/moodalbox.js"></script> 
</head>
<body>
<noscript>
<h1>Warning</h1>
<p class="noscript">To use this site correctly you need to have JavaScript enabled on your web browser</p>
</noscript>
<div id="hidden">
  <?php require("../assets/widgets/hidden.php"); ?>
</div>
<div id="container">
  <div id="header">
    <h2>Website</h2>
<p>Solon: 01352 762266<br />Sue: 01352 736117<br />The ITC: 01745 828440</p>
 </div>
  <div id="navigation">
    <?php 
	// THIS PULLS IN THE ADMIN LINKS
	require("widget_links.php"); 
	?>
  </div>
  <div id="main_content">
    <form name="dateform" action="<?echo($_SERVER['PHP_SELF']);?>" method="post">
  FROM&nbsp;&nbsp;Day: <select name="fromday">
  		<?for($i=1;$i<32;$i++){
  			echo("<option value=\"".$i."\">".$i."</option>\n");
  		}
  		?>
  	</select>&nbsp;Month: 
  	<select name="frommonth">
  		<?for($i=1;$i<13;$i++){
  			echo("<option value=\"".$i."\">".$i."</option>\n");
  		}
  		?>
  	</select>&nbsp;Year: 
  	<select name="fromyear">
  		<?for($i=2008;$i<2011;$i++){
  			echo("<option value=\"".$i."\">".$i."</option>\n");
  		}
  		?>
  	</select>
  	&nbsp;&nbsp;TO:&nbsp;&nbsp;
  	  <form name="dateform" action="<?echo($_SERVER['PHP_SELF']);?>" method="post">
  	Day: <select name="today">
  		<?for($i=1;$i<32;$i++){
  			echo("<option value=\"".$i."\">".$i."</option>\n");
  		}
  		?>
  	</select>&nbsp;Month: 
  	<select name="tomonth">
  		<?for($i=1;$i<13;$i++){
  			echo("<option value=\"".$i."\">".$i."</option>\n");
  		}
  		?>
  	</select>&nbsp;Year: 
  	<select name="toyear">
  		<?for($i=2008;$i<2011;$i++){
  			echo("<option value=\"".$i."\">".$i."</option>\n");
  		}
  		?>
  	</select>
  	<input type="submit" name="formsubmit" value="Submit">
  </form>
      <?php
      
      
	  //$tracker=mysql_query("insert into $admin_track_table (name, loggedin, ip, job) values ('$_COOKIE[user_name]', '$tracker_time', '$ip', 'Report - Viewed best sellers')");	
	  
	  $getOrders="SELECT `cartinfo` FROM SHOP2_orders WHERE `date` BETWEEN '".$fromDate."' AND '".$toDate."'";
	  $getOrdersQry=mysql_query($getOrders);
	  while($getOrdersRes=mysql_fetch_array($getOrdersQry)){
	  	//Process each line
	  	if(strpos($getOrdersRes['cartinfo'],",")>0){
	  		$tempArr=explode(",",$getOrdersRes['cartinfo']);
	  		for($i=0;$i<count($tempArr);$i++){
	  			$finalArr[$tempArr[$i]]=$finalArr[$tempArr[$i]] + 1;
	  		}
	  	}else{
	  		$finalArr[$getOrdersRes['cartinfo']]=$finalArr[$getOrdersRes['cartinfo']]+1;
	  	}
	  }
	  //var_dump($finalArr);
	  
	  foreach ($finalArr AS $key=>$value){
	  	if(is_numeric($key)){
	  		$prodSql="SELECT `code`,  `name` FROM SHOP2_products WHERE id=".$key;
	  		$prodQry=mysql_query($prodSql);
	  		$prodRes=mysql_fetch_array($prodQry);
	  		$prodArr[]=array("code"=> $prodRes['code'], "name"=>$prodRes['name'], "quantity"=>$value, "id"=>$key);
	  	}
	  }
	  if(strlen($errMessage)>1){
	  	 echo("<h4 style=\"color:red;\">".$errMessage."</h4>");
	  }	 
	   echo("<h2>Report: Product Sold Quantities From ".$showFromDate." To ".$showToDate."</h2>");
	
	  echo('<div id="orders"><table><tr><th>Product ID</th><th>Code</th><th>Name</th><th>Quantity</th></tr>');
	  for($s=0;$s<count($prodArr);$s++){
		  echo("<tr><td>".$prodArr[$s]['id']."</td>");
		  echo("<td>".$prodArr[$s]['code']."</td>");
		  echo("<td>".$prodArr[$s]['name']."</td>");
		  echo("<td>".$prodArr[$s]['quantity']."</td></tr>");		  
	  }
	echo("</table></div>");	

/*	  
echo("<h2>Reports - Most Purchased Products</h2><p><a href='bite_size_chart.php?type=pp' rel='moodalbox 800 600' title='Graph of most purchased products'>View as graph</a></p>");
		
		echo '<div id="orders"><table><tr><th></th><th>Item</th><th>Qty</th><th>Price (Now)</th><th>Price (Avg)</th><th>Subtotal</th></tr>';
		
		$grand 		= 0;
		$total 		= 0;
		$postage 	= 0;	
		$i 			= 1;
		$products = array();
		
		$product_query = mysql_query("select * from $product_table order by id asc");
		while ($product_result = mysql_fetch_array($product_query))
			{
			$data_query = mysql_query("select * from $report_products_table where product_id = '$product_result[id]'");
			$numrows = mysql_num_rows($data_query);
			if ($numrows > 0)
				{
				while ($data_result = mysql_fetch_array($data_query))
					{
					$products["".$data_result[product_id].""] += $data_result[qty];
					}						
				}			
			}
			
			
			arsort($products); // DISPLAY ARRAY IN REVERSE ORDER BASED ON VALUE
				
			foreach ($products as $key => $val) {
			
					
			$product_query = mysql_query("select * from $product_table where id = '$key'");
			while ($product_result = mysql_fetch_array($product_query))		
				{
				$product_qty 		= 0;
				$product_total 		= 0;
			$get_query = mysql_query("select * from $report_products_table where product_id = '$key'");
			while ($get_result = mysql_fetch_array($get_query))
				{
				$product_qty 		+= $get_result[qty];
				$product_total 		+= $get_result[price] * $get_result[qty];	
				}
				
					$price_avg = $product_total / $product_qty;
					if($num == "1")
					{
					$class=" class='row1'";
					$num=2;
					}
				else
					{
					$class=" class='row2'";
					$num=1;
					}
					echo "<tr$class>";
					echo '<td>'.$i.'</td>';
					echo '<td>'.$product_result[name].' '.$product_result[att_value].'</td>';
					echo '<td>'.$product_qty.'</td>';
					echo '<td>'.profit($product_result[price]).'</td>';
					echo '<td>'.profit($price_avg).'</td>';
					echo '<td>'.profit($product_total).'</td>';
					echo '</tr>';
					$i++;
					$total 		+= $product_total;
					$tqty	 	+= $product_qty;
				}
			}			
		
echo '
<tr class="totals"><td>Totals:</td><td></td><td><strong>'.$tqty.'</strong></td><td><strong>-</strong></td><td><strong>-</strong></td><td><strong>'.profit($total).'</strong></td></tr></table></div>';
*/
	?></div>
  <div id="footer">
  <?php 
	// THIS PULLS IN THE FOOTER
	require("../assets/widgets/footer.php"); 
	?>
  </div>
</div>
</body>
</html>
