<div id="navigation">

<!--p>
<a href="callto:+441745828499" title="Click here to call using Skype"><?php echo("$site_tel");?></a>
</p-->
<ul>
<li><a href="<?php echo $site_url ?>index.php" title="Click here to go to our home page" accesskey="h"><em>H</em>ome</a></li>
<li><a href="<?php echo $site_url ?>products.php" title="Click here to go to our product list" accesskey="p"><em>P</em>roducts</a></li>
<li><a href="<?php echo $site_url ?>about.php" title="Click here to find out who we are" accesskey="a"><em>A</em>bout us</a></li>
<li><a href="<?php echo $site_url ?>contact.php" title="Click here to go to get information on how to contact us" accesskey="c"><em>C</em>ontact us</a></li>
<li><a href="<?php echo $site_url ?>home_security_tips.php" title="Tips on how to secure your home and it's content" accesskey="s">Home <em>S</em>ecurity Tips</a></li>
</ul>



<?php
$refurl 	= 	$_SERVER['HTTP_REFERER'];
$refdate 	= 	date("d-m-y G:i:s");
$refip 		= 	$_SERVER['REMOTE_ADDR'];
$this_url 	=	selfURL();
$session_id	=	session_id();

$footprint = mysql_query("insert into $footprint_table (url, session_id, this_url, date, ip) values ('$refurl', '$session_id', '$this_url', '$refdate', '$refip')");

?>


</div>