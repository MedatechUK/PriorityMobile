<?php

$scripts = mysql_query("select * from SHOP1_config where id = '2'");
$scripts_result = mysql_fetch_array ($scripts);

echo("
<!-- Start of External Scripts -->
$scripts_result[content]
<!-- End of External Scripts -->
");

?>

<p class="oasisone"><a href="http://www.flame-media.co.uk" title="North West Web designers based in North Wales">Website design </a>by FLAME:media </p>
