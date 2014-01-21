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
include("FCKeditor/fckeditor.php"); 
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
<h2>Additional Monthly costs</h2>
<?php
// EDIT ADDITIONAL COSTS FIELD
if ($_GET[edit])
	{
	$query = mysql_query("select * from $report_monthly_table where month = '$_GET[edit]' and year = '$_GET[year]'");
	$numrows = mysql_num_rows($query);
	if ($numrows > 0) // GET EXISTING ENTRY
		{
		$result = mysql_fetch_array($query);
		?>
    <form method="post" action="report_monthly.php?edited=ok">
      <fieldset>
      <legend>Update Other Cost Details</legend>
      <p>
        <label for="costs"><span>Costs (&pound;)</span></label>
        <input name="costs" id="costs" type="text" size="10" value="<?php echo("$result[costs]"); ?>"/>
      </p>
      <p>Notes</p>
      <?php
		$oFCKeditor = new FCKeditor('notes') ;
		$oFCKeditor->BasePath = 'FCKeditor/';
		$oFCKeditor->Value = $result[notes];
		$oFCKeditor->Width  = '300' ;
		$oFCKeditor->Height = '150' ;
		$oFCKeditor->ToolbarSet = 'summary';
		$oFCKeditor->Create() ;
		?>
      <p>
        <input name="month" type="hidden"  value="<?php echo("$result[month]"); ?>"/>
        <input name="year" type="hidden"  value="<?php echo("$result[year]"); ?>"/>
		<input name="id" type="hidden"  value="<?php echo("$result[id]"); ?>"/>
        <input type="submit" name="Submit" value="Update Costs" />
		<a href="report_monthly.php">Cancel</a>
      </p>
      </fieldset>
    </form>
    <?
		}
	else // CREATE NEW ENTRY
		{
		?>
    <form method="post" action="report_monthly.php?added=ok">
      <fieldset>
      <legend>Add Other Cost Details</legend>
      <p>
        <label for="costs"><span>Costs (£)</span></label>
        <input name="costs" id="costs" type="text" size="10"/>
      </p>
      <p>Notes</p>
      <?php
		$oFCKeditor = new FCKeditor('notes') ;
		$oFCKeditor->BasePath = 'FCKeditor/';
		$oFCKeditor->Width  = '300' ;
		$oFCKeditor->Height = '150' ;
		$oFCKeditor->ToolbarSet = 'summary';
		$oFCKeditor->Create() ;
		?>
      <p>
        <input name="month" type="hidden"  value="<?php echo("$_GET[edit]"); ?>"/>
        <input name="year" type="hidden"  value="<?php echo("$_GET[year]"); ?>"/>
        <input type="submit" name="Submit" value="Add Costs" />
		<a href="report_monthly.php">Cancel</a>
      </p>
      </fieldset>
    </form>
    <?
		}
	}
?>
</div>
</body>
</html>
