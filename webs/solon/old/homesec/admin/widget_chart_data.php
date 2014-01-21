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
include_once("../assets/widgets/charts/charts.php");
require_once('../assets/widgets/global_variables.php');
//$tracker=mysql_query("insert into $admin_track_table (name, loggedin, ip, job) values ('$_COOKIE[user_name]', '$tracker_time', '$ip', 'Graph - Monthly Sales')");	

$year = $_GET[year];
$month = "01";
$count = 1; 

$chart [ 'chart_type' ] = array ( "column","column", "line" );

$chart [ 'axis_value' ] = array (   'steps'            =>  "10",  
                                    'prefix'           =>  "£", 
                                    'size'             =>  10); 
$chart [ 'axis_category' ] = array (   'size'          =>  10); 

$chart [ 'legend_label' ] = array (   'size'    =>  12); 

$chart [ 'legend_rect' ] = array (   'margin'          =>  5,
                                     'line_color'      =>  "f7941d",
                                     'line_alpha'      =>  100, 
                                     'line_thickness'  =>  0); 

$chart [ 'series_color' ] = array ( "f7941d", "666666", "99cc33"  );

$chart [ 'series_gap' ] = array (   'bar_gap'  =>  10); 


$chart [ 'chart_value' ] = array (  'prefix'         =>  "£", 
                                    'suffix'         =>  "", 
                                    'decimals'       =>  2,
                                    'decimal_char'   =>  ".",  
                                    'separator'      =>  "",
                                    'position'       =>  "cursor",
                                    'hide_zero'      =>  false, 
                                    'as_percentage'  =>  false, 
                                    'font'           =>  "Arial", 
                                    'bold'           =>  true, 
                                    'size'           =>  14, 
                                    'color'          =>  "000000", 
									'background_color'=>"ffffff",
                                    'alpha'          =>  90
                                  ); 

$chart[ 'draw' ] = array ( 
array ( 'type'=>"text", 'color'=>"000000", 'alpha'=>25, 'font'=>"arial", 'rotation'=>0, 'bold'=>true, 'size'=>20, 'x'=>360, 'y'=>430, 'width'=>100, 'height'=> 100, 'text'=>"".$year."", 'h_align'=>"left", 'v_align'=>"bottom" ), 
array ( 'type'=>"text", 'color'=>"000000", 'alpha'=>25, 'font'=>"arial", 'rotation'=>270, 'bold'=>true, 'size'=>20, 'x'=>0, 'y'=>300, 'width'=>100, 'height'=> 80, 'text'=>"Value (£)", 'h_align'=>"left", 'v_align'=>"middle" ) );

$chart [ 'chart_data' ][ 0 ][ 0 ] = "";
$chart [ 'chart_data' ][ 1 ][ 0 ] = "Product Sales";
$chart [ 'chart_data' ][ 2 ][ 0 ] = "Commission";
$chart [ 'chart_data' ][ 3 ][ 0 ] = "Profit";

		for ($count=1; $count<=12; $count++)
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
				$add_costs		= 0;
				
				$query = mysql_query("select * from $orders_table where date like '$year-$month%'");
				$numrows = mysql_num_rows($query);
								
				if ($numrows > 0)
					{
					
					while ($result = mysql_fetch_array($query))
						{
						// CHECK TO SEE IF ORDER HAS BEEN COMPLETED
						$dispatched = mysql_query("select * from $order_status_table where order_id = '$result[id]' order by id desc limit 1");
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
					$costs_query 	= mysql_query("select * from $report_monthly_table where month = '$month' and year = '$year'");
					$costs_result 	= mysql_fetch_array($costs_query);
					
					$add_costs			= $costs_result[costs];
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
									
					
					$text_month = date("M", mktime(0, 0, 0, $month, 1, $year));
													
					
					$chart [ 'chart_data' ][ 0 ][ $count ] = "$text_month";
					//echo("$text_month - $total - $expap - $profit_costs<br/>");
					$chart [ 'chart_data' ][ 1 ][ $count ] = "$expap";
					$chart [ 'chart_data' ][ 2 ][ $count ] = "$profit";
					$chart [ 'chart_data' ][ 3 ][ $count ] = "$profit_costs";
				
					
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
				$add_costs		= 0;
					if ($month == "12")
						{
						$month = 01;
						}
					else
						{
						$month = $month+1;
						$str = strlen($month);
						if ($str == 1)
							{
							$month = "0".$month;
							}
						}
					
					}
	//		}

/*

$chart [ 'chart_data' ][ 0 ][ 0 ] = "";
$chart [ 'chart_data' ][ 0 ][ 1 ] = "2001";
$chart [ 'chart_data' ][ 0 ][ 2 ] = "2002";
$chart [ 'chart_data' ][ 0 ][ 3 ] = "2003";
$chart [ 'chart_data' ][ 0 ][ 4 ] = "2004";
$chart [ 'chart_data' ][ 1 ][ 0 ] = "Region A";
$chart [ 'chart_data' ][ 1 ][ 1 ] = 5;
$chart [ 'chart_data' ][ 1 ][ 2 ] = 10;
$chart [ 'chart_data' ][ 1 ][ 3 ] = 30;
$chart [ 'chart_data' ][ 1 ][ 4 ] = 63;
$chart [ 'chart_data' ][ 2 ][ 0 ] = "Region B";
$chart [ 'chart_data' ][ 2 ][ 1 ] = 100;
$chart [ 'chart_data' ][ 2 ][ 2 ] = 20;
$chart [ 'chart_data' ][ 2 ][ 3 ] = 65;
$chart [ 'chart_data' ][ 2 ][ 4 ] = 55;
$chart [ 'chart_data' ][ 3 ][ 0 ] = "Region C";
$chart [ 'chart_data' ][ 3 ][ 1 ] = 56;
$chart [ 'chart_data' ][ 3 ][ 2 ] = 21;
$chart [ 'chart_data' ][ 3 ][ 3 ] = 5;
$chart [ 'chart_data' ][ 3 ][ 4 ] = 90;
*/
 
//send the new data to charts.swf
SendChartData($chart);



?>