<?php
session_start();
//IF THE USER IS RETURNING FROM A COMPLETE ORDER CLEAR SESSION
if ($_GET[clear] == "all")
	{
	session_destroy();
	}

require("assets/widgets/global_variables.php");
require_once('assets/widgets/mysql.class.php');
require('assets/widgets/global.inc.php');
require_once('assets/widgets/functions.inc.php');

require_once('assets/widgets/maintenance.php');
// Process actions
$cart = $_SESSION['cart'];
$action = $_GET['action'];
switch ($action) {
	case 'add':
		if ($cart) {
		
		// IF THE SESSION IS HERE DO THIS FOR THE QUANITY ADDED
		if ($_POST['amount'] == "1" || !$_POST['amount'])
			{
			$cart .= ','.$_POST['id'];
			}
		else
			{
			$i = 1;
			while ($i <= $_POST['amount'] || !$_POST['amount'])
				{
				$cart .= ','.$_POST['id'];
				$i++;
				}
			}
		
			
		} 
		
		else {
		
		// IF NO SESSION IS HERE DO THIS FOR THE QUANITY ADDED
			if ($_POST['amount'] == "1")
				{
				$cart = $_POST['id'];
				}
			else
				{
				$i = 1;
				$cart = $_POST['id'];
				while ($i < $_POST['amount'])
					{
					$cart .= ','.$_POST['id'];
					$i++;
					}
				}
		
		
			
		}
		break;
	case 'delete':
		if ($cart) {
			$items = explode(',',$cart);
			$newcart = '';
			foreach ($items as $item) {
				if ($_GET['id'] != $item) {
					if ($newcart != '') {
						$newcart .= ','.$item;
					} else {
						$newcart = $item;
					}
				}
			}
			$cart = $newcart;
		}
		break;
	case 'update':
	if ($cart) {
		$newcart = '';
		foreach ($_POST as $key=>$value) {
			if (stristr($key,'qty')) {
				$id = str_replace('qty','',$key);
				$items = ($newcart != '') ? explode(',',$newcart) : explode(',',$cart);
				$newcart = '';
				foreach ($items as $item) {
					if ($id != $item) {
						if ($newcart != '') {
							$newcart .= ','.$item;
						} else {
							$newcart = $item;
						}
					}
				}
				for ($i=1;$i<=$value;$i++) {
					if ($newcart != '') {
						$newcart .= ','.$id;
					} else {
						$newcart = $id;
					}
				}
			}
		}
	}
	$cart = $newcart;
	break;
}
$_SESSION['cart'] = $cart;

?>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
<?php 
$page_type = "basket";
require("assets/widgets/meta.php"); 
?>
</head>
<body>
<?php 
echo("$js_notice");
?>
  <?php require("assets/widgets/hidden.php"); ?>
<div id="container">
  <?php require("assets/widgets/header.php"); ?>
  <?php require("assets/widgets/nav.php"); ?>
 <div id="left_col">
  <?php require("assets/widgets/categories.php"); ?>
 </div>
  <div id="main_content">
     <?php 
	echo("$site_notice");
	?>
	
	<?php
echo showCart();
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
