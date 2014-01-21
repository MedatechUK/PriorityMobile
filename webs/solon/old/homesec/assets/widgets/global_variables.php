<?php

$site_name 		= 	"Home Security Store";
$site_email		=	"hs@redlinesecurity.co.uk";
$site_tel		=	"01745 828499";
$order_email	=	"hs@redlinesecurity.co.uk";
$site_url		=	"http://www.home-security-store.co.uk/";
$ip				= 	$_SERVER[REMOTE_ADDR]; 
$tracker_time 	= 	date('YmdHis');

//$site_notice	=	"<p class='errormessage'>THIS SHOP IS NOT YET OPERATIONAL. WE CANNOT HONOUR ANY ORDERS MADE</p>";
//$site_notice	=	"<div class='notice'><p>Due to annual stocktake we are not planning on making any despatches on Weds 1st or Thurs 2nd October.  Orders placed on these days will be despatched on Friday 3rd October.  However if you have any urgent requests could you please call us on: 01745 828499 and we will do what we can to help you</p></div>";

//$site_notice	=	"<div class='notice'><p>Please note, due to planned industrial action by the Communication Workers Union between 4th October and 9th October 2007, there may be disruption to our ability to deliver goods next day. Please check details and for updates at the Royal Mail Website <a href='http://www.royalmail.com' target='_blank'>www.royalmail.com</a>.</p><p>Please also read our terms and conditions.</p></div>";

//$site_notice	=	"<div class='xdelivery'><p></p></div>";


$js_notice	=	'
<noscript>
<div class="mainalert">
<h1>Javascript is disabled</h1>
<p class="noscript">We recommend you enable javascript on your web browser to use this website.</p>
</div>
</noscript>';

//$postage_notice = "<div class='notice'><p>Please note, due to planned industrial action by the Communication Workers Union  during October 2007, there may be disruption to our ability to deliver goods next day. Please check details and for updates at the Royal Mail Website <a href='http://www.royalmail.com' target='_blank'>www.royalmail.com</a>.</p><p>Please also read our <a href='terms_and_conditions.php'>terms and conditions</a>.</p></div>";

$admin_whats_new = '
<div class="info_alert">
<p><strong>What\'s new</strong><br />
<small>Search report - for those who want to see what people are searching for on the site.<br />
Purge Footprint table - This table can get pretty big and make the DB slower.<br />
Expand and collapse - Product edit pages too big? Not any more (oh and it remembers your choice for the day)</small></p>
</div>';


ini_set('arg_separator.output','&amp;');

?>