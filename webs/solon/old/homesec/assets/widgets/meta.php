<meta name="verify-v1" content="o9RaNbGlNu01IZ+u91KiB0EWtz3mY5R1iOuBxxDHXMs=" />
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title>

<?php 
if ($page_type == "products" && $_GET[cat])
	{
	echo("$page_result[name] | $site_name");
	}
elseif ($page_type == "basket")	
	{
	echo("$site_name | Basket");
	}
else
	{
	echo $page_result[pagetitle];
	}

?>


</title>
<meta name="keywords" content="<?php echo $page_result[metatags] ?>" />
<meta name="description" content="<?php echo $page_result[metadesc] ?>" />
<link rel="stylesheet" href="<?php echo $site_url ?>assets/css/screen.css" type="text/css" media="screen" />
<link rel="stylesheet" href="<?php echo $site_url ?>assets/css/print.css" type="text/css" media="print" />
<link rel="shortcut icon" href="<?php echo $site_url ?>assets/images/favicon.ico" />