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
<script language="javascript" src="../assets/scripts/calender/cal2.js"></script>
<script language="javascript" src="../assets/scripts/calender/cal_conf2.js"></script>
<script language="javascript" type="text/javascript" src="../assets/scripts/lib.js"></script>
<script language="javascript" type="text/javascript" src="../assets/scripts/popup.js"></script>
<script languagee="javascript">

<!-- Begin
function popUp(URL) {
day = new Date();
id = day.getTime();
eval("page" + id + " = window.open(URL, '" + id + "', 'toolbar=0,scrollbars=1,location=0,statusbar=0,menubar=0,resizable=0,width=600,height=400,left = 440,top = 212');");
}
// End -->
</script>

</head>
<body>
<div id="bitesize">
<h2>Export orders to Excel</h2>
<p><a href='export_orders.php' class='export'>Export all orders</a></p>
<?php




?>

</div>
</body>
</html>
