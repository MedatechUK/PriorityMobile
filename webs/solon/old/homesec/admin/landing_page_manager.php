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



?>



<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
<title>Admin Interface</title>
<link rel="shortcut icon" href="../assets/images/favicon.ico" />
<link rel="stylesheet" href="../assets/css/admin.css" type="text/css" media="screen" />
<script type="text/javascript" src="../assets/scripts/functions.js"></script>
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
<?php
$this_page = "landing_page_manager";
$this_url = "landing_page_manager.php";

//==========================================================================
//ADD PROCESS
//==========================================================================	
if ($_GET[process] == "ok")
	{
	$page_name_formatted 	= 	str_replace(" ", "_", $_POST[page_name]);

	if ($page_name_formatted == $this_page)
		{
		echo("<div id='status_error'><p>Page could not be created as it is the same name as the builder</p></div>");
		}
	else
		{
		
		$template = "<?php\n
require_once('../assets/widgets/mysql.class.php');\n
require_once('../assets/widgets/global.inc.php');\n
require_once('../assets/widgets/functions.inc.php');\n
//==========================================================================\n\n

\$refurl = \$_SERVER[HTTP_REFERER];\n
\$refdate = date(\"d-m-y G:i:s\");\n
\$refip = \$_SERVER[REMOTE_ADDR];\n\n

\$trackit = mysql_query(\"insert into \$tracker_table (url, date, ip, campaign) values ('\$refurl', '\$refdate', '\$refip', '$_POST[page_name]')\");\n
\n
header(\"location: ../$_POST[destination]\");\n
?>";
		
		
		
			$myFile 				= 	"../landing_pages/".$page_name_formatted.".php";
			$fh 					= 	fopen($myFile, 'w') or die("can't open file");
			$stringData 			= 	"$template\n";
			fwrite($fh, $stringData);
			fclose($fh);
		echo("<div id='status_ok'><p>The page <a href='$myFile'>$myFile</a> has been created</p></div>");
		$tracker=mysql_query("insert into $admin_track_table (name, loggedin, ip, job) values ('$_COOKIE[user_name]', '$tracker_time', '$ip', 'Landing Page - Built ($_POST[page_name])')");
		}
	}
	
//==========================================================================
//DELETE PROCESS
//==========================================================================	
if ($_GET[delete])
	{
	$page_name_formatted 	= 	"../landing_pages/".$_GET[delete].".php";
	
	if ($page_name_formatted == $this_page)
		{
		echo("<div id='status_error'><p>Page could not be deleted as it is the same name as the builder</p></div>");
		}
	else
		{
		
		$query = mysql_query("select * from $product_table where seo='$_GET[delete].php'");
		$result = mysql_fetch_array($query);
		
		$process = mysql_query("update $product_table set seo='' where id='$result[id]'");
		
		
		unlink("$page_name_formatted");
		echo("<div id='status_ok'><p>The page $page_name_formatted has been deleted</p></div>");
		$tracker=mysql_query("insert into $admin_track_table (name, loggedin, ip, job) values ('$_COOKIE[user_name]', '$tracker_time', '$ip', 'Landing Page - deleted ($page_name_formatted)')");
		}
	}
?>


<form method="post" action="landing_page_manager.php?process=ok">
  <fieldset>
  <legend>Page Details</legend>
  <p>
	<label for="page_name"><span>Campaign</span></label>
	<input name="page_name" id="page_name" type="text" size="50"/>
  </p>
  <p>
	<label for="destination"><span>Destination</span></label>
	<input name="destination" id="destination" type="text" size="50"/><font>&nbsp;Only the info after the main URL</font>
  </p>
  </fieldset>
	<input type="submit" name="Submit" value="Add Landing Page" />
</form>

<?php

$dir = opendir('../landing_pages/');
echo '<ul>';

while ($read = readdir($dir))
{

if ($read!='.' && $read!='..')
{
if ($read == $this_url)
	{
	echo '<li><a href="'.$read.'">'.$read.'</a></li>';
	}
else
	{
	$page_name	= 	str_replace(".php", "", $read);
	echo '<li><a href="../landing_pages/'.$read.'">'.$read.'</a> - <a href="landing_page_manager.php?delete='.$page_name.'">Delete</a></li>';
	}
}

}

echo '</ul>';

closedir($dir); 

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
