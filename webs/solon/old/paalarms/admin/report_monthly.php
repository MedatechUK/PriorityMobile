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
require_once('../assets/widgets/functions.inc.php');


?>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
<title>Admin Interface</title>
<link rel="shortcut icon" href="http://www.personal-attack-alarms.net/favicon.ico" />
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
	

$year 			= date("Y");
$month 			= date("m");
$count			= date("n");
$this_year 		= date("Y");
$y_total 		= 0;
$y_postage 		= 0;
$y_expap		= 0;
$y_profit		= 0;
$y_profit_costs	= 0;
$y_avg_order	= 0;
$y_orders		= 0;
$y_add_costs	= 0;




echo("<h2>Reports - Monthly for $year</h2><p><small>All figures below are based on orders which have been recored as being dispatched</small></p>");

echo("<ul>");

for ($ycount=1; $ycount<=5; $ycount++)
	{
	$year_query 	= mysql_query("select * from SHOP1_orders where date like '$this_year%' limit 1");
	$year_numrows 	= mysql_num_rows($year_query);
	
	if ($year_numrows > 0)
		{
		while($year_result 	= mysql_fetch_array($year_query))
			{
			echo("<li><a href='bite_size_chart.php?year=$this_year' rel='moodalbox 800 600' title='Graph of income, product sales and profit for $this_year'>Sales graph ($this_year)</a></li>
			<li><a href='bite_size_chart.php?year=$this_year&amp;type=orders' rel='moodalbox 800 600' title='Graph of orders per month for $this_year'>Order graph ($this_year)</a></li>
			<li><a href='bite_size_chart.php?year=$this_year&amp;type=average_order' rel='moodalbox 800 600' title='Graph of average order value per month for $this_year'>Average order graph ($this_year)</a></li>
			");
			}
		}
	$this_year = $this_year - 1;
	}

echo("</ul>");

//==========================================================================
//EDIT PROCESS
//==========================================================================
if($_GET[edited])
{

	$costs = str_replace("£", "", $_POST[costs]);
	$postage = str_replace("£", "", $_POST[postage]);
	$process=mysql_query("update SHOP1_report_monthly set costs='$costs', postage='$postage', notes='$_POST[notes]' where id='$_POST[id]'"); 

	if (!$process)
	{
	echo("<div id='status_error'><p><strong>Oops!</strong></p><p>There was an error when you tried to update the costs.</p></div>");
	}
	else
	{
	echo("<div id='status_ok'><p>Thank you the costs have now been updated.</p></div>");
	}
}


//==========================================================================
//ADD PROCESS
//==========================================================================
if($_GET[added])
{
$costs = str_replace("£", "", $_POST[costs]);
$postage = str_replace("£", "", $_POST[postage]);
$process=mysql_query("insert into SHOP1_report_monthly (costs, postage, notes, month, year) values ('$costs', '$postage', '$_POST[notes]', '$_POST[month]', '$_POST[year]')"); 

	if (!$process)
	{
	echo("<div id='status_error'><p><strong>Oops!</strong></p><p>There was an error when you tried to add the costs.</p></div>");
	}
	else
	{
	echo("<div id='status_ok'><p>Thank you the costs have now been added.</p></div>");
	}
}


echo("<div id='reports'>
	<table width='90%'>
		<thead>
			<tr>
			<th>Month</th>
			<th><acronym title='Total order value (inc VAT)'>Gross Income</acronym></th>
			<th><acronym title='The total postage costs'>P&amp;P</acronym></th>
			<th><acronym title='Gross income - P&amp;P'>Product Sales</acronym></th>
			<th><acronym title='Total number of orders'>Orders</acronym></th>
			<th><acronym title='Product sales divided by number of orders'>Avg Order</acronym></th>
			<th><acronym title='Product sales x 30% = Commission'>30% cut</acronym></th>
			<th><acronym title='(Orders x 38p) + (2% of gross income)'>Pay Fees</acronym></th>
			<th width='200'><acronym title='Marketing costs, etc'>Other costs</acronym></th>
			<th><acronym title='30% cut - costs'>ITC Profit</acronym></th></tr>
		</thead>
		<tbody>");
	
	$month_query = mysql_query("select * from SHOP1_orders where date like '$year%';");
	while ($month_result = mysql_fetch_array($month_query))
		{
		//for ($count=12; $count>=1; $count--)
		if ($count >= 1)
			{
			
				$total 			= 0;
				$postage 		= 0;
				$expap			= 0;
				$profit			= 0;
				$avg_order		= 0;
				$orders			= 0;
				$add_costs		= 0;
				$profit_pro		= 0;
				$profit_costs 	= 0;
				$pay_fees		= 0;
				
				
				$query = mysql_query("select * from SHOP1_orders where date like '$year-$month%'");
				$numrows = mysql_num_rows($query);
				if ($numrows > 0)
					{
					
					while ($result = mysql_fetch_array($query))
						{
						// CHECK TO SEE IF ORDER HAS BEEN COMPLETED
						$dispatched = mysql_query("select * from SHOP1_order_status where order_id = '$result[id]' order by id desc limit 1");
						$dispatch = mysql_fetch_array($dispatched);
						if ($dispatch[action] == "Dispatched")
							{
							$total_cost = str_replace("£", "", $result[total_cost]);
							$total 		+= $total_cost;
							$postage 	+= $result[postage];
							$orders++;
							}
						}						
					}
					
					// CHECK FOR ADDITIONAL COSTS
					$costs_query 	= mysql_query("select * from SHOP1_report_monthly where month = '$month' and year = '$year'");
					$costs_result 	= mysql_fetch_array($costs_query);
					
					$add_costs			+= $costs_result[costs];
					//$postage			+= $costs_result[postage];
					$pay_fees 			= ($orders * 0.38) + ($total / 100 * 2);
					$expap				= $total - $postage;
					$profit				= $expap / 100 * 30;
					$profit_costs		= $expap / 100 * 30 - $add_costs - $pay_fees;
					
					if ($orders > 0)
						{
						$avg_orders = $expap / $orders;
						$profit_pro	= $profit_costs / $orders;
						}
					else
						{
						$avg_orders = 0;
						$profit_pro = 0;
						}
					
					
					//ADD TO YEARLY TOTAL
					$y_total 			+= $total;
					$y_postage 			+= $postage;
					$y_expap			+= $expap;
					$y_profit			+= $profit;
					$y_orders			+= $orders;
					$y_add_costs		+= $add_costs;
					$y_profit_costs		+= $profit_costs;
					$y_pay_fees			+= $pay_fees;
					
					
					$text_month = date("M", mktime(0, 0, 0, $month, 1, $year));
					
					if($class =="monthly2")
						{
						$class = 'monthly1';
						}
					else
						{
						$class = 'monthly2';
						}
									
					
					echo("<tr class='$class'>
						<td>$text_month</td>
						<td>".profit($total)."</td>
						<td>". profit($postage)."</td>
						<td>". profit($expap)."</td>
						<td>$orders</td>
						<td>". profit($avg_orders)."</td>
						<td>". profit($profit)."</td>
						<td>". profit($pay_fees)."</td>
						<td>
						<span class='costs'>".profit($add_costs)."</span>&nbsp;
						<a href='bite_size_mr.php?month=$month&amp;year=$year' rel='moodalbox' title='Additonal costs information'>Notes</a> | 
						
						<a href='bite_size_costs.php?edit=$month&amp;year=$year' rel='moodalbox' title='Edit costs for $text_month $year'>edit</a></td>
						<td>".profit($profit_costs)."</td>
					</tr>");
					
					
					if ($month == "01")
						{
						$month = 12;
						$count--;
						}
					else
						{
						$month = $month-1;
						$count--;
						$str = strlen($month);
						if ($str == 1)
							{
							$month = "0".$month;
							}
						}
			
					}
				}
	
//DISPLAY TOTALS	

if ($y_orders > 0)
	{
	$y_avg_orders = $y_expap / $y_orders;
	$y_profit_pro	= $y_profit_costs / $y_orders;
	}
else
	{
	$y_avg_orders = 0;
	}
	
	
echo("<tr class='month'><td colspan='10'>Year to date for $year</td></tr>");
echo("
<tr class='totals'>
<td>Totals</td>
<td>". profit($y_total)."</td>
<td>". profit($y_postage)."</td>
<td>". profit($y_expap)."</td>
<td>$y_orders</td>
<td>". profit($y_avg_orders)."</td>
<td>". profit($y_profit)."</td>
<td>". profit($y_pay_fees)."</td>
<td>". profit($y_add_costs)."</td>
<td>". profit($y_profit_costs)."</td>
</tr>");
					
	echo("</tbody></table>");








// ADDITIONAL YEARS

$year = 2007;
$month = 12;
$count = 12; 
$y_total 		= 0;
$y_postage 		= 0;
$y_expap		= 0;
$y_profit		= 0;
$y_profit_costs	= 0;
$y_avg_order	= 0;
$y_orders		= 0;
$y_add_costs	= 0;



$month_query = mysql_query("select * from SHOP1_orders where date like '$year%';");
$numrows = mysql_num_rows($month_query);
if ($numrows > 0)
	{

	echo("<table width='90%'>
		<thead>
			<tr>
			<th>Month</th>
			<th><acronym title='Total order value (inc VAT)'>Gross Income</acronym></th>
			<th><acronym title='The total postage costs'>P&amp;P</acronym></th>
			<th><acronym title='Gross income - P&amp;P'>Product Sales</acronym></th>
			<th><acronym title='Total number of orders'>Orders</acronym></th>
			<th><acronym title='Product sales divided by number of orders'>Avg Order</acronym></th>
			<th><acronym title='Product sales x 30% = Commission'>30% cut</acronym></th>
			<th><acronym title='(Orders x 38p) + (2% of gross income)'>Pay Fees</acronym></th>
			<th width='200'><acronym title='Marketing costs, etc'>Other costs</acronym></th>
			<th><acronym title='30% cut - costs'>ITC Profit</acronym></th></tr>
		</thead>
		<tbody>");
	
	//while ($month_result = mysql_fetch_array($month_query))
	//	{
		for ($count=12; $count>=1; $count--)
		//if ($count >= 1)
			{
			
				$total 			= 0;
				$postage 		= 0;
				$expap			= 0;
				$profit			= 0;
				$avg_order		= 0;
				$orders			= 0;
				$add_costs		= 0;
				$profit_pro		= 0;
				$profit_costs 	= 0;
				$pay_fees		= 0;
				
				
				$query = mysql_query("select * from SHOP1_orders where date like '$year-$month%'");
				$numrows = mysql_num_rows($query);
				if ($numrows > 0)
					{
					
					while ($result = mysql_fetch_array($query))
						{
						// CHECK TO SEE IF ORDER HAS BEEN COMPLETED
						$dispatched = mysql_query("select * from SHOP1_order_status where order_id = '$result[id]' order by id desc limit 1");
						$dispatch = mysql_fetch_array($dispatched);
						if ($dispatch[action] == "Dispatched")
							{
							$total_cost = str_replace("£", "", $result[total_cost]);
							$total 		+= $total_cost;
							$postage 	+= $result[postage];
							$orders++;
							}
						}						
					}
					
					// CHECK FOR ADDITIONAL COSTS
					$costs_query 	= mysql_query("select * from SHOP1_report_monthly where month = '$month' and year = '$year'");
					$costs_result 	= mysql_fetch_array($costs_query);
					
					$add_costs		+= $costs_result[costs];
					//$postage			+= $costs_result[postage];
					$pay_fees = ($orders * 0.38) + ($total / 100 * 2);
					$expap				= $total - $postage;
					$profit				= $expap / 100 * 30;
					$profit_costs		= $expap / 100 * 30 - $add_costs - $pay_fees;
					
					if ($orders > 0)
						{
						$avg_orders = $expap / $orders;
						$profit_pro	= $profit_costs / $orders;
						}
					else
						{
						$avg_orders = 0;
						$profit_pro = 0;
						}
					
					
					//ADD TO YEARLY TOTAL
					$y_total 			+= $total;
					$y_postage 			+= $postage;
					$y_expap			+= $expap;
					$y_profit			+= $profit;
					$y_orders			+= $orders;
					$y_add_costs		+= $add_costs;
					$y_profit_costs		+= $profit_costs;
					$y_pay_fees			+= $pay_fees;
					
					
					$text_month = date("M", mktime(0, 0, 0, $month, 1, $year));
					
					if($class =="monthly2")
						{
						$class = 'monthly1';
						}
					else
						{
						$class = 'monthly2';
						}
									
					
					echo("<tr class='$class'>
						<td>$text_month</td>
						<td>".profit($total)."</td>
						<td>". profit($postage)."</td>
						<td>". profit($expap)."</td>
						<td>$orders</td>
						<td>". profit($avg_orders)."</td>
						<td>". profit($profit)."</td>
						<td>". profit($pay_fees)."</td>
						<td>
						<span class='costs'>".profit($add_costs)."</span>&nbsp;
						<a href='bite_size_mr.php?month=$month&amp;year=$year' rel='moodalbox' title='Additonal costs information'>Notes</a> | 
						
						<a href='bite_size_costs.php?edit=$month&amp;year=$year' rel='moodalbox' title='Edit costs for $text_month $year'>edit</a></td>
						<td>".profit($profit_costs)."</td>
					</tr>");
					
					
					if ($month == "01")
						{
						$month = 12;
						//$count--;
						}
					else
						{
						$month = $month-1;
						//$count--;
						$str = strlen($month);
						if ($str == 1)
							{
							$month = "0".$month;
							}
						}
			
					}
			//	}
	
//DISPLAY TOTALS	

if ($y_orders > 0)
	{
	$y_avg_orders = $y_expap / $y_orders;
	$y_profit_pro	= $y_profit_costs / $y_orders;
	}
else
	{
	$y_avg_orders = 0;
	}
	
	
echo("<tr class='month'><td colspan='10'>Year to date for $year</td></tr>");
echo("
<tr class='totals'>
<td>Totals</td>
<td>". profit($y_total)."</td>
<td>". profit($y_postage)."</td>
<td>". profit($y_expap)."</td>
<td>$y_orders</td>
<td>". profit($y_avg_orders)."</td>
<td>". profit($y_profit)."</td>
<td>". profit($y_pay_fees)."</td>
<td>". profit($y_add_costs)."</td>
<td>". profit($y_profit_costs)."</td>
</tr>");
					
	echo("</tbody></table>");
	}
	
	
	echo("</div>");
	?>
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
