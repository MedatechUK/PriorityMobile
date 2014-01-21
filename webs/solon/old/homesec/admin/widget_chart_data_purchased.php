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

//$tracker=mysql_query("insert into $admin_track_table (name, loggedin, ip, job) values ('$_COOKIE[user_name]', '$tracker_time', '$ip', 'Graph - Purchased Products')");	

$year = $_GET[year];
$month = "01";
$count = 1; 

$chart [ 'chart_type' ] = "3d column";

$chart [ 'axis_value' ] = array (   'steps'            =>  "10",  
                                    'prefix'           =>  "", 
                                    'size'             =>  10); 
$chart [ 'axis_category' ] = array (   'orientation'   =>  "vertical_down",
'size'          =>  10); 

$chart [ 'legend_label' ] = array (   'size'    =>  12); 

$chart [ 'legend_rect' ] = array (   'margin'          =>  5,
                                     'line_color'      =>  "f7941d",
                                     'line_alpha'      =>  100, 
                                     'line_thickness'  =>  0); 

$chart [ 'series_color' ] = array ( "f7941d", "666666", "99cc33"  );

$chart [ 'series_gap' ] = array (   'bar_gap'  =>  10); 


$chart [ 'chart_value' ] = array (  'prefix'         =>  "", 
                                    'suffix'         =>  "", 
                                    'decimals'       =>  0,
                                    'decimal_char'   =>  ".",  
                                    'separator'      =>  "",
                                    'position'       =>  "cursor",
                                    'hide_zero'      =>  false, 
                                    'as_percentage'  =>  false, 
                                    'font'           =>  "Arial", 
                                    'bold'           =>  true, 
                                    'size'           =>  12, 
                                    'color'          =>  "000000", 
                                    'alpha'          =>  90
                                  ); 

$chart[ 'draw' ] = array ( 
array ( 'type'=>"text", 'color'=>"000000", 'alpha'=>25, 'font'=>"arial", 'rotation'=>0, 'bold'=>true, 'size'=>20, 'x'=>360, 'y'=>430, 'width'=>100, 'height'=> 100, 'text'=>"".$year."", 'h_align'=>"left", 'v_align'=>"bottom" ), 
array ( 'type'=>"text", 'color'=>"000000", 'alpha'=>25, 'font'=>"arial", 'rotation'=>270, 'bold'=>true, 'size'=>20, 'x'=>0, 'y'=>300, 'width'=>250, 'height'=> 80, 'text'=>"Value (Units Sold)", 'h_align'=>"left", 'v_align'=>"middle" ) );

$chart [ 'chart_data' ][ 0 ][ 0 ] = "";
$chart [ 'chart_data' ][ 1 ][ 0 ] = "Product";

	
	
	
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
			//print_r($products);	
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
					$chart [ 'chart_data' ][ 0 ][ $i ] = "".$product_result[code]."";
					$chart [ 'chart_data' ][ 1 ][ $i ] = "$product_qty";
					$i++;
					$total += $product_total;
				}
			}			
 
//send the new data to charts.swf
SendChartData($chart);



?>