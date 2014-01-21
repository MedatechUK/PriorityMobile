<?php
session_start();

//IF THE USER IS RETURNING FROM A COMPLETE ORDER CLEAR SESSION
if ($_GET[clear] == "all")
	{
	session_destroy();
	}

// Include MySQL class
require_once('assets/widgets/mysql.class.php');
// Include database connection
require_once('assets/widgets/global.inc.php');
// Include functions
require_once('assets/widgets/functions.inc.php');
require_once('assets/widgets/global_variables.php');

$page_query = mysql_query("select * from SHOP1_content where id='5'");
while($page_result = mysql_fetch_array($page_query))
	{

?>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
<title><?php echo $site_name; ?> | <?php echo $page_result[pagetitle] ?></title>
<meta name="keywords" content="<?php echo $page_result[metatags] ?>" />
<meta name="description" content="<?php echo $page_result[metadesc] ?>" />
<link rel="stylesheet" href="assets/css/screen.css" type="text/css" media="screen" />
<link rel="stylesheet" href="assets/css/print.css" type="text/css" media="print" />
<link rel="shortcut icon" href="/favicon.ico" />
</head>
<body>
<?php 
echo("$js_notice");
?>
<div id="hidden">
  <?php require("assets/widgets/hidden.php"); ?>
</div>
<div id="container">
  <div id="header">
    <h2><?php echo $site_name; ?></h2>
	<a href="/index.php" class="homelink" title="Home Page"><img src="assets/images/clear.gif" alt="Link to Home Page" width="300px" height="50px"/></a>
<?php echo writeShoppingCart(); ?>
  </div>
  <div id="navigation">
    <?php require("assets/widgets/nav.php"); ?>
  </div>
  <div id="main_content">
   <?php 
	echo("$site_notice");
	?>
          <h2>Tell a friend</h2>
     <?php
	 
function ValidEmail($addr){
	list($local, $domain) = explode("@", $addr);
	
	$pattern_local = '^([0-9a-z]*([-|_]?[0-9a-z]+)*)(([-|_]?)\.([-|_]?)[0-9a-z]*([-|_]?[0-9a-z]+)+)*([-|_]?)$';
	$pattern_domain = '^([0-9a-z]+([-]?[0-9a-z]+)*)(([-]?)\.([-]?)[0-9a-z]*([-]?[0-9a-z]+)+)*\.[a-z]{2,4}$';

	$match_local = eregi($pattern_local, $local);
	$match_domain = eregi($pattern_domain, $domain);
	
	return ($match_local && $match_domain && gethostbyname($domain));
}	 
	 
if (!$_SERVER['HTTP_REFERER'])
	die("<p>Sorry, but I did not get the address of the page to send. This information may be being blocked by your browser settings, or your firewall.</p>");

if ($_SERVER['REQUEST_METHOD']=="POST")
{
	$to_email=$_POST['to_email'];
	$to_name=$_POST['to_name'];
	$from_name=$_POST['from_name'];
	$from_email=$_POST['from_email'];
	$url_to_send=$_POST['url_to_send'];
	$errs="";
	
	if (!$to_email)
		$errs.="<p>Sorry you did not enter an eMail address to send the link to. <a href=javascript:history.go(-1) title='Back to previous page'>go back</a> and ensure all the details are entered.</p>";
	elseif (!ValidEmail($to_email))
		$errs.="<p>The eMail address <b>$to_email</b> does not appear to be valid. Please <a href=javascript:history.go(-1) title='Back to previous page'>go back</a> and ensure all the details are entered.</p>";
	if (!$to_name)
		$errs.="<p>Sorry you did not enter a name of the recipient of the link. <a href=javascript:history.go(-1) title='Back to previous page'>go back</a> and ensure all the details are entered.</p>";
	if (!$from_name)
		$errs.="<p>Please ensure you enter your name before sending. <a href=javascript:history.go(-1) title='Back to previous page'>go back</a> and ensure all the details are entered.</p>";
	if (!$from_email)
		$errs.="<p>Please ensure you enter your eMail address before sending. <a href=javascript:history.go(-1) title='Back to previous page'>go back</a> and ensure all the details are entered.</p>";
	elseif (!ValidEmail($from_email))
		$errs.="<p>The eMail address <b>$from_email</b> does not appear to be valid. <a href=javascript:history.go(-1) title='Back to previous page'>go back</a> and ensure all the details are entered.</p>";
	if (!$url_to_send)
		$errs.="<p>URL to page not recieved. It may be blocked by your firewall or browser.<a href=javascript:history.go(-1) title='Back to previous page'>go back</a> and ensure all the details are entered.</p>";
	elseif (strpos($url_to_send, $_SERVER['HTTP_HOST']) != 7)
		$errs.="<p>Bad referring page. <a href=javascript:history.go(-1) title='Back to previous page'>go back</a> and ensure all the details are entered.</p>";
	if ($errs)
		echo "<p>Sorry we could not send the link because of the following error(s):</p><p>$errs</p>";
	else {
		
		
		
$message='
<html>
<body style="font-family:Verdana;font-size: 80%;background:#fff;">
<table width="500" border="0" cellpadding="0">
<tr><td>
<img src="http://www.personal-attack-alarms.net/assets/images/email_header.jpg" alt="personal-attack-alarms.net" width="550" height="200"/>

<p style="font-size:80%">'.$from_name.' ('.$from_email.') has sent you this email.</p>
  <p style="font-size:80%">'.$from_name.' thought you might be interested in personal attack alarms from Redline Security...</p>
  <p style="padding-bottom:10px;font-size:80%"><a href='.$url_to_send.' style="color: #4c7a08;font-weight: bold;">'.$url_to_send.'</a></p>

</td>
</tr>
<tr>
<td style="padding:10px;margin:0;font-size:70%;color:#999;background:#333;width:100%;">
  

<p>Copyright 2004-'.date("Y").' Redline Security. All Rights Reserved. Redline Security is a trading name of TIHS Ltd.</p>
<p>Our registered office is: Oak House, Groes Lwyd, Abergele, Conwy LL22 7SU. We are registered in England and Wales. Company No 5325603. VAT Number 855446109.</p>

</td></tr>
</table>
</body>
</html>
';
		
	
		
$headers  = "MIME-Version: 1.0\r\n";
$headers .= "Content-type: text/html; charset=iso-8859-1\r\n";
$headers .= "To: ".$to_name." <".$to_email.">\r\n";
$headers .= "From:  ".$from_name." <".$from_email.">\r\n";
$subject = "".$from_name." thought you'd like to see...";
mail($to, $subject, $message, $headers);
		
		
$headers  = "MIME-Version: 1.0\r\n";
$headers .= "Content-type: text/html; charset=iso-8859-1\r\n";
$headers .= "To: Airs <aaron.hughes@theitc.co.uk>\r\n";
$headers .= "From:  <$order_email>\r\n";
$subject = "Tell a friend link sent";
mail($to, $subject, $message, $headers);		
		
		
		
		echo "<p>Thank you</p><p>The link was successfully sent.</p><p><a href='$url_to_send' title='Return to refering page'>Return to the page you came from</a>.";
		
	}
} else {
?>
<p>Just fill in the details below and your friend will be sent an email with a link to this website or the product you want them to see.</p>

<p>We don't store personal details when you use this form; the information is only used to send your friend the link. Please read our <a href="privacy_policy.php" title="Privacy Policy">Privacy Policy</a>.</p>

<form action="<?php echo $_SERVER['/PHP_SELF'];?>" method="post">

<fieldset>
      <legend>Enter details below</legend>
	  <p>Please fill in all fields below and click 'send'</p>
      

	<input type="hidden" name="url_to_send" value="<?php echo $_SERVER['HTTP_REFERER'];?>" />
			<label for="to_name">Friend's name</label><br/>
			<input name="to_name" type="text" id="to_name" />
			<p></p><label for="to_name">Friend's eMail address </label><br/>
			<input name="to_email" type="text" id="to_email" />
			<p></p>
			<label for="from_name">Your name</label><br/>
			<input name="from_name" type="text" id="from_name" />
			<p></p>
			<label for="from_email">Your eMail address</label><br/>
			<input name="from_email" type="text" id="from_email" />
			<p></p>
			<input type="submit" name="Submit" value="Send"  class="submit"/>
</fieldset></form>
<?php 
}
?>
  </div>
  <div id="footer">
    <?php 
	// THIS PULLS IN THE FOOTER
	require("assets/widgets/footer.php"); 
	?>
  </div>
</div>
<?php require("assets/widgets/google.php"); ?>
</body>
</html>
<?php } ?>
