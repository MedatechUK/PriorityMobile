<?php 
// SECURITY CHECK TO MAKE SURE THE CORRECT USERNAME AND PASSWORD HAS BEEN ENTERED
if(!$_COOKIE[user_name] | !$_COOKIE[user_pwd])
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
<link rel="stylesheet" href="../assets/css/bitesize.css" type="text/css" media="screen" />
<link rel="stylesheet" href="../assets/scripts/modal/css/moodalbox.css" type="text/css" media="screen" />
<script type="text/javascript" src="../assets/scripts/modal/js/mootools.js"></script> 
<script type="text/javascript" src="../assets/scripts/modal/js/moodalbox.js"></script> 


</head>
<body>
<div id="bitesize">
<h2>Export orders to Excel</h2>
<div class="search">
  <form name="search" method="post" action="export_orders.php?specify=yes">
  <fieldset>
  <legend>Search by date range</legend>

<?php 
$this_year 	= date("Y");
$this_month = date("m");
$this_day 	= date("j");
?>
 <p>
  <label>Start Date</label>
  <select name="s_day">
  <?php
  for($i=1; $i <=31; $i++)
  	{
	$i_len = strlen($i);
		if ($i_len == 1)
			{
			$day = "0".$i;
			}
		else
			{
			$day = $i;
			}
	echo("<option value='$day'$selected>$day</option>");
	}
	?>
  	
</select>
<select name="s_month">
  <option value="01">Jan</option>
  <option value="02">Feb</option>
  <option value="03">Mar</option>
  <option value="04">Apr</option>
  <option value="05">May</option>
  <option value="06">Jun</option>
  <option value="07">Jul</option>
  <option value="08">Aug</option>
  <option value="09">Sep</option>
  <option value="10">Oct</option>
  <option value="11">Nov</option>
  <option value="12">Dec</option>
</select>
<?php $this_year = date("Y");?>
<select name="s_year">

  <option value="<?php echo $this_year ?>"><?php echo $this_year ?></option>
  <option value="<?php echo $this_year-1 ?>"><?php echo $this_year-1 ?></option>
  <option value="<?php echo $this_year-2 ?>"><?php echo $this_year-2 ?></option>
</select>
</p>

 <p>
  <label>End Date</label>
  <select name="e_day">
  <?php
  for($i=1; $i <=31; $i++)
  	{
	$i_len = strlen($i);
		if ($i_len == 1)
			{
			$day = "0".$i;
			}
		else
			{
			$day = $i;
			}
	if ($this_day == $i)
			{
			$selected = " selected='selected'";
			}
		else
			{
			$selected = "";
			}
	echo("<option value='$day'$selected>$day</option>");
	}
	?>
  	
</select>
<select name="e_month">
  <option value="01" <?php if ($this_month == "01") { echo(" selected='selected'");}?>>Jan</option>
  <option value="02" <?php if ($this_month == "02") { echo(" selected='selected'");}?>>Feb</option>
  <option value="03" <?php if ($this_month == "03") { echo(" selected='selected'");}?>>Mar</option>
  <option value="04" <?php if ($this_month == "04") { echo(" selected='selected'");}?>>Apr</option>
  <option value="05" <?php if ($this_month == "05") { echo(" selected='selected'");}?>>May</option>
  <option value="06" <?php if ($this_month == "06") { echo(" selected='selected'");}?>>Jun</option>
  <option value="07" <?php if ($this_month == "07") { echo(" selected='selected'");}?>>Jul</option>
  <option value="08" <?php if ($this_month == "08") { echo(" selected='selected'");}?>>Aug</option>
  <option value="09" <?php if ($this_month == "09") { echo(" selected='selected'");}?>>Sep</option>
  <option value="10" <?php if ($this_month == "10") { echo(" selected='selected'");}?>>Oct</option>
  <option value="11" <?php if ($this_month == "11") { echo(" selected='selected'");}?>>Nov</option>
  <option value="12" <?php if ($this_month == "12") { echo(" selected='selected'");}?>>Dec</option>
</select>

<select name="e_year">

  <option value="<?php echo $this_year ?>"><?php echo $this_year ?></option>
  <option value="<?php echo $this_year-1 ?>"><?php echo $this_year-1 ?></option>
  <option value="<?php echo $this_year-2 ?>"><?php echo $this_year-2 ?></option>
</select>
</p>
  
<input type="submit" name="submit" value="Go" />
</fieldset>
</form>
</div>
</div>
</body>
</html>
