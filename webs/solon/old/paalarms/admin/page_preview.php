<?php
// Include MySQL class
require_once('../assets/widgets/mysql.class.php');
// Include database connection
require_once('../assets/widgets/global.inc.php');
// Include functions
require_once('../assets/widgets/functions.inc.php');
require_once('../assets/widgets/global_variables.php');

$page_query = mysql_query("select * from SHOP1_content_archive where id='$_GET[id]'");
while($page_result = mysql_fetch_array($page_query))
	{

?>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta name="verify-v1" content="PcKB8JufPh6arWgIKV3prAwQ+HhTUj7Qos+yTXeNM3Q=" />
<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
<title>ARCHIVE - <?php echo $page_result[pagetitle] ?></title>
<meta name="keywords" content="<?php echo $page_result[metatags] ?>" />
<meta name="description" content="<?php echo $page_result[metadesc] ?>" />
<link rel="stylesheet" href="../assets/css/screen.css" type="text/css" media="screen" />
<link rel="stylesheet" href="../assets/css/print.css" type="text/css" media="print" />
<link rel="shortcut icon" href="/favicon.ico" />
<style>

.notice h1{border: 1px solid #f00;border-bottom: none;padding: 5px;background: #f8b3b3;font-weight: bold;color: #f00;width:760px;margin:0 auto;font-size: 100%;}
.notice p{border: 1px solid #f00;border-top: none;font-size: 80%;padding: 5px;background: #f8b3b3;font-weight: bold;width:760px;margin:0 auto 10px auto;}

#preview_keywords, #preview_desc{ border: 1px solid #888; background: #bbb; padding: 5px; margin: 2px auto; width: 760px; text-align: left;}
#preview_keywords h2, #preview_desc h2{font-size: 110%; padding: 0; margin: 0;}

#preview_desc {margin-bottom: 10px;}


</style>
</head>
<body>
<?php 
echo("$js_notice");
?>
<div class="notice">
<h1>THIS IS AN ARCHIVED PAGE</h1>
<p class="noscript">Links will not work on this page as it is an archived page</p>
</div>
<div id="preview_keywords">
<h2>Keywords</h2>
<p><code><?php echo $page_result[metatags] ?></code></p>
</div>
<div id="preview_desc">
<h2>Description</h2>
<p><code><?php echo $page_result[metadesc] ?></code></p>
</div>

<div id="hidden">
  <?php require("../assets/widgets/admin_hidden.php"); ?>
</div>
<div id="container">
  <div id="header">
    <h2><?php echo $site_name; ?></h2>
	<a href="/index.php" class="homelink" title="Home Page"><img src="../assets/images/clear.gif" alt="" width="300px" height="50px"/></a>
<?php echo writeShoppingCart(); ?>
  </div>
  <div id="navigation">
    <?php require("../assets/widgets/nav.php"); ?>
  </div>
  <div id="main_content">
     <?php 
	echo("$site_notice");
	echo("$page_result[content]"); 
	?>
  </div>
  <div id="footer">
    <?php 
	// THIS PULLS IN THE FOOTER
	require("../assets/widgets/footer.php"); 
	?>
  </div>
</div>
<?php require("../assets/widgets/google.php"); ?>
<p></p>
<div class="notice">
<h1>THIS IS AN ARCHIVED PAGE</h1>
<p class="noscript">Links will not work on this page as it is an archived page</p>
</div>
</body>
</html>
<?php } ?>