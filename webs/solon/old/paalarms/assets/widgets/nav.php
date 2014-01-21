<ul>
<li><a href="index.php" title="Click here to go to our home page">Home</a></li>
<li><a href="products.php" title="Click here to go to our product list">Personal Alarms</a></li>
<li><a href="about.php" title="Click here to find out who we are">About us</a></li>
<li><a href="contact.php" title="Click here to go to get information on how to contact us">Contact us</a></li>
<li><a href="how_a_personal_attack_alarm_will_help.php" title="Click here to go find out why you should buy a personal attack alarm">Why buy?</a></li>

</ul>

<?php
$refurl 	= 	$_SERVER['HTTP_REFERER'];
$refdate 	= 	date("d-m-y G:i:s");
$refip 		= 	$_SERVER['REMOTE_ADDR'];
$this_url 	=	selfURL();
$session_id	=	session_id();

$footprint = mysql_query("insert into SHOP1_footprints (url, session_id, this_url, date, ip) values ('$refurl', '$session_id', '$this_url', '$refdate', '$refip')");

//print($this_url);print($session_id);

?>