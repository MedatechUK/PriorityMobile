<?php
session_start();
$thispage = "M";

//IF THE USER IS RETURNING FROM A COMPLETE ORDER CLEAR SESSION
if ($_GET[clear] == "all")
	{
	session_destroy();
	header ("location: index.php");
	}

require("assets/widgets/global_variables.php");

?>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<title><?php echo $site_name ?> | Crucial Maintenance</title>
<link rel="shortcut icon" href="<?php echo $site_url ?>assets/images/favicon.ico" />
<style>
<!--
body{
text-align: center;
font-family:Verdana, Arial, Helvetica, sans-serif;
margin: 38px 0 20px 0;
padding: 0;
font-size: 90%;
background: URL("assets/images/shell/background.gif") repeat-x #737373;}


a {color: #f7941d;font-weight: bold;}
a:hover {color: #f7941d;text-decoration:none;}


#hidden {display: none}

#container {
width:700px;
padding: 20px; 
margin:0 auto;
text-align: center;
background: #fff;
}

#container p {font-size: 180%;}

#footer {text-align: left; font-size: 10pt; border-top: 1px solid #ddd; padding-top: 10px}
#footer p {font-size: 8pt;}

#footer img {float: right; border: 1px solid #000;}


-->
</style>
</head>
<body>
<div id="container">
<p><img src="assets/images/large_logo.gif" alt="<?php echo $site_name ?>" /></p>
<p>The site is currently not open to the public.</p>
<p><?php echo ("$site_tel | <a href='$site_email'>$site_email</a>");?></p>
 <div id="footer">
    <?php 
	// THIS PULLS IN THE FOOTER
	require("assets/widgets/footer.php"); 
	?>
  </div>
</div>
</body>
</html>
