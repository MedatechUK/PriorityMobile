<?php

$scripts = mysql_query("select * from $config_table where id = '2'");
$scripts_result = mysql_fetch_array ($scripts);

echo("
<!-- Start of External Scripts -->
$scripts_result[content]
<!-- End of External Scripts -->
");

?>

<p class="oasisone"><a href="http://www.oasisone.co.uk" title="North West Web designers based in North Wales">Website design by Oasis:One</a></p>