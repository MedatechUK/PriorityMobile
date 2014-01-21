<?php

$site_name 		= 	"personal-attack-alarms.net";
$site_email		=	"paa@redlinesecurity.co.uk";
$order_email	=	"paa@redlinesecurity.co.uk";
$ip				= 	$_SERVER[REMOTE_ADDR]; 
$tracker_time 	= 	date('YmdHis');
$site_url		=	"http://www.personal-attack-alarms.net/";

//$site_notice	=	"<p class='errormessage'>THIS SHOP IS NOT YET OPERATIONAL. WE CANNOT HONOUR ANY ORDERS MADE</p>";
//$site_notice	=	"<div class='notice'><p>Please note, due to stock taking at our warehouse, any orders placed after 9am on Monday 1st October will not be dispatched until Wednesday 3rd October. Normal service will resume on the 3rd October.</p></div>";

//$site_notice	=	"<div class='xdelivery'><p></p></div>";

$site_notice	=	'<a href="products.php" title="Click here for our Winter Sale on Personal Attack Alarms"><img src="assets/images/winter_banner_final.jpg" alt="Personal Attack Alarm Winter Sale Now On" border="0"/></a>';

//$site_notice	.=	"<div class='notice'><p>Due to annual stocktake we are not planning on making any despatches on Weds 1st or Thurs 2nd October.  Orders placed on these days will be despatched on Friday 3rd October.  However if you have any urgent requests could you please call us on: 01745 828499 and we will do what we can to help you</p></div>";

//$site_notice	=	"<div class='notice'><p>Please note, due to planned industrial action by the Communication Workers Union between 4th October and 9th October 2007, there may be disruption to our ability to deliver goods next day. Please check details and for updates at the Royal Mail Website <a href='http://www.royalmail.com' target='_blank'>www.royalmail.com</a>.</p><p>Please also read our terms and conditions.</p></div>";


$js_notice	=	'
<h1 class="title">Personal Attack Alarms by Redline Security</h1>
<noscript>
<div class="mainalert">
<p>Javascript is disabled</p>
<p class="noscript">We recommend you enable javascript on your web browser to use this website.</p>
</div>
</noscript>
';



ini_set('arg_separator.output','&amp;');
?>