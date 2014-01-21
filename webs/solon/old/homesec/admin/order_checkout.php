<?php

if(!$_COOKIE[user_name] | !$_COOKIE[user_pwd])
{
header("location: login.php");
exit;
}

require_once('../assets/widgets/mysql.class.php');
require_once('../assets/widgets/global.inc.php');
require_once('../assets/widgets/functions.inc.php');
require_once('../assets/widgets/admin_functions.inc.php');
require("../assets/widgets/global_variables.php");

session_start();
?>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
<title>Admin Interface</title>
<link rel="shortcut icon" href="../assets/images/favicon.ico" />
<link rel="stylesheet" href="../assets/css/admin.css" type="text/css" media="screen" />
<script type="text/javascript" src="../assets/scripts/functions.js"></script>
<script language="javascript" type="text/javascript" src="../assets/scripts/lib.js"></script>
<script language="javascript" type="text/javascript" src="../assets/scripts/popup.js"></script>
<script language="JavaScript" type="text/javascript">
<!--

function checkCheckBox(f){
if (f.agree.checked == false )
{
alert('Please ensure the client has read our terms and conditions and agree to them.');
return false;
}else
return true;
}
//-->
</script>	
</head>
<body>
<noscript>
<h1>Warning</h1>
<p class="noscript">To use this site correctly you need to have JavaScript enabled on your web browser</p>
</noscript>
<div id="hidden">
  <?php require("../assets/widgets/hidden.php"); ?>
</div>
<div id="container">
  <div id="header">
    <h2>Website</h2>
<p>Solon: 01352 762266<br />Sue: 01352 736117<br />The ITC: 01745 828440</p>
 </div>
  <div id="navigation">
    <?php 
	// THIS PULLS IN THE ADMIN LINKS
	require("widget_links.php"); 
	?>
  </div>
  <div id="main_content">
    
	<?php
	if ($_GET[check] == 'yes')
	{	
	if (!$_POST[title] || !$_POST[firstname] || !$_POST[surname] || !$_POST[street] || !$_POST[town] || !$_POST[county] || !$_POST[postcode] || !$_POST[email]|| !ereg('^[-!#$%&\'*+\\./0-9=?A-Z^_`a-z{|}~]+'.
              '@'.
              '[-!#$%&\'*+\\/0-9=?A-Z^_`a-z{|}~]+\.'.
              '[-!#$%&\'*+\\./0-9=?A-Z^_`a-z{|}~]+$', $_POST[email]) || ereg('[^0-9\+\-\(\)\ ]', $_POST[tel]))
		{
		//SHOW VERIFICATION STUFF
		
		$error = "&nbsp;<font class='error'>Please enter this field</font>";
		
		?>
		<h2>Admin Checkout > Delivery</h2>
    <p class="step">Admin Checkout > <strong>Delivery information</strong> > Confirmation > Payment > Complete</p>
    <p class="errormessage">Please enter all the required information</p>
    <form method="post" action="order_checkout.php?check=yes">
      <fieldset>
      <legend>Enter your delivery details below</legend>
	  <p>Fields marked <strong title="Required" class="required">*</strong> are required</p>
      <p>
        <label><span>Title <strong title="Required" class="required">*</strong></span>
        <input type="text" name="title" value="<?php echo stripslashes($_POST[title])?>" />
        <?php if (!$_POST[title]) { echo ("$error"); } ?>
        </label>
      </p>
      <p>
        <label><span>First Name <strong title="Required" class="required">*</strong></span>
        <input type="text" name="firstname" value="<?php echo stripslashes($_POST[firstname])?>" />
        <?php if (!$_POST[firstname]) { echo ("$error"); } ?>
        </label>
      </p>
      <p>
        <label><span>Surname <strong title="Required" class="required">*</strong></span>
        <input type="text" name="surname" value="<?php echo stripslashes($_POST[surname])?>" />
        <?php if (!$_POST[surname]) { echo ("$error"); } ?>
        </label>
      </p>
      <p>
        <label><span>Street Address <strong title="Required" class="required">*</strong></span>
        <input type="text" name="street" value="<?php echo stripslashes($_POST[street])?>" />
        <?php if (!$_POST[street]) { echo ("$error"); } ?>
        </label>
      </p>
      <p>
        <label><span>Address Line 2</span>
        <input type="text" name="address2" value="<?php echo stripslashes($_POST[address2])?>" />
        </label>
      </p>
      <p>
        <label><span>Town / City <strong title="Required" class="required">*</strong></span>
        <input type="text" name="town" value="<?php echo stripslashes($_POST[town])?>" />
        <?php if (!$_POST[town]) { echo ("$error"); } ?>
        </label>
      </p>
      <p>
        <label><span>County <strong title="Required" class="required">*</strong></span>
        <input type="text" name="county" value="<?php echo stripslashes($_POST[county])?>" />
        <?php if (!$_POST[county]) { echo ("$error"); } ?>
        </label>
      </p>
      <p>
        <label><span>Post Code <strong title="Required" class="required">*</strong></span>
        <input type="text" name="postcode" value="<?php echo stripslashes($_POST[postcode])?>" />
        <?php if (!$_POST[postcode]) { echo ("$error"); } ?>
        </label>
      </p>
      <p>
        <label><span>Telephone <strong title="Required" class="required">*</strong></span>
        <input type="text" name="tel" value="<?php echo stripslashes($_POST[tel])?>" />
		<?php
		if (ereg('[^0-9\+\-\(\)\ ]', stripslashes($_POST[tel]))){
  			echo "&nbsp;<font class='error'>Please enter a vaild number</font>";
		}
		?>
        </label>
      </p>
      <p>
        <label><span>Email <strong title="Required" class="required">*</strong></span>
        <input type="text" name="email" value="<?php echo stripslashes($_POST[email])?>" />
        <?php 
		 if (!$_POST[email]) { echo ("$error"); } 
		 elseif (!ereg('^[-!#$%&\'*+\\./0-9=?A-Z^_`a-z{|}~]+'.
              '@'.
              '[-!#$%&\'*+\\/0-9=?A-Z^_`a-z{|}~]+\.'.
              '[-!#$%&\'*+\\./0-9=?A-Z^_`a-z{|}~]+$', $_POST[email]))
			  {echo ("&nbsp;<font class='error'>Please enter a vaild email</font>");}
		 
		 ?>
        </label>
      </p>
      <p>
        <label><span>Delivery</span></label>
        <select name="deliverymethod">
          <option value="3.95" <?php if ($_POST[cover] == "3.95") { echo ("selected='selected'"); } ?>>Royal Mail 1st Class (&#163;3.95)</option>
          <option value="8.95" <?php if ($_POST[cover] == "8.95") { echo ("selected='selected'"); } ?>>Royal Mail Next Day (&#163;8.95)</option>
        <option value="Other" <?php if ($_POST[deliverymethod] == "Other") { echo ("selected='selected'"); } ?>>Other</option>
        </select> &nbsp;Other: <input type="text" name="custompostage" value="<?php echo stripslashes($_POST[custompostage]); ?>" />
      </p>
	  	  <input type="hidden" value="yes" name="billing" />
      
      </fieldset>
	  
	  <p class="buttons">
	  <input name="submit" type="image" value="Submit" src="../assets/images/buttons/continue.jpg" alt="Continue" /> 
      </p>
    </form>
	
    <?php
		}
	else
		{
		//SHOW CONFIRMATION INFORMATION
	?>
	<h2>Admin Checkout > Confirm</h2>
    <p class="step">Admin Checkout > Delivery information > <strong>Confirmation</strong> > Payment > Complete</p>
    <form method="post" action="order_confirm.php" onsubmit="return checkCheckBox(this)">
      <fieldset>
      <legend>Please confirm all details below are correct</legend>
      <div id="confirm">
        <h3>Your items (<a href="order_start.php" title="Edit the content of your basket">edit</a>)</h3>
        <?php
				
		echo admin_confirmCart();
		
		?>
		<div class="summary_delivery">
        <h3>Delivery Address (<a href="order_checkout.php" title="Edit your deilvery information">edit</a>)</h3>
        <?php
		
		// Adds the delivery info to session 
		$_SESSION['delivery'] = "";
		$_SESSION['delivery'] = "";
		$_SESSION['delivery'] .= $_POST['title'];
		$_SESSION['delivery'] .= '|'.$_POST['firstname'];
		$_SESSION['delivery'] .= '|'.$_POST['surname'];
		$_SESSION['delivery'] .= '|'.$_POST['street'];
		$_SESSION['delivery'] .= '|'.$_POST['address2'];
		$_SESSION['delivery'] .= '|'.$_POST['town'];
		$_SESSION['delivery'] .= '|'.$_POST['county'];
		$_SESSION['delivery'] .= '|'.$_POST['postcode'];
		$_SESSION['delivery'] .= '|'.$_POST['tel'];
		$_SESSION['delivery'] .= '|'.$_POST['email'];
		
		if ($_POST['deliverymethod'] == "Other")
			{
			$postage = $_POST[custompostage];
			}
		else
			{
			$postage = $_POST[deliverymethod];
			}
		$_SESSION['delivery'] .= $postage;
		$_SESSION['delivery'] .= '|'.$_POST['billing'];
		$delivery = $_SESSION['delivery'];
		
		// End of addding delivery to session
		
		
				
echo('<p>'.stripslashes($_POST[title]).' '.stripslashes($_POST[firstname]).' '.stripslashes($_POST[surname]).'<br />');
echo(''.stripslashes($_POST[street]).'<br />');
if ($_POST[address2])
{
echo(''.stripslashes($_POST[address2]).'<br/>');
}
echo(''.stripslashes($_POST[town]).'<br/>'.stripslashes($_POST[county]).'<br/>'.stripslashes($_POST[postcode]).'</p>');	
echo('<p>Telephone: '.$_POST[tel].'<br/>Email: '.$_POST[email].'</p>');
		?>
		</div><div class="summary_delivery"><h3>Billing Details</h3>
		<?php
		if($_POST[billing] == "yes")
			{
			echo('<p>'.stripslashes($_POST[title]).' '.stripslashes($_POST[firstname]).' '.stripslashes($_POST[surname]).'<br />');
echo(''.stripslashes($_POST[street]).'<br />');
if ($_POST[address2])
{
echo(''.stripslashes($_POST[address2]).'<br/>');
}
echo(''.stripslashes($_POST[town]).'<br/>'.stripslashes($_POST[county]).'<br/>'.stripslashes($_POST[postcode]).'</p>');	
echo('<p>Telephone: '.$_POST[tel].'<br/>Email: '.$_POST[email].'</p>');
			}
		else
			{
			echo("<p>To be confirmed on next page</p>");
			}
			
			
			if ($_POST['deliverymethod'] == "Other")
			{
			$postage = $_POST[custompostage];
			}
		else
			{
			$postage = $_POST[deliverymethod];
			}
		
		?></div>	
			<input type="hidden" name="orderref" value="HS <?php echo date("y-mdHi") ?>"/>
			<input type="hidden" name="currency" value="GBP"/>
			<input type="hidden" name="email" value="<?php echo $_POST[email]; ?>"/>
			<input type="hidden" name="orderinfo" value="<?php echo admin_checkoutCart();?>"/>
			<input type="hidden" name="orderdetail" value="<?php echo admin_checkoutCart();?>"/>
			<input type="hidden" name="cartinfo" value="<?php echo $_SESSION['cart'];?>"/>
			<input type="hidden" name="cartdetail" value="<?php echo $_SESSION['cartdetail'];?>"/>
			<input type="hidden" name="total" value="<?php  echo $total ?>"/>
			<input type="hidden" name="postage" value="<?php echo $postage; ?>"/>
			<input type="hidden" name="delivery_name" value="<?php echo stripslashes($_POST[title]).' '.stripslashes($_POST[firstname]).' '.stripslashes($_POST[surname])?>"/>
			<input type="hidden" name="delivery_address" value="<?php echo stripslashes($_POST[street])?>"/>
			<input type="hidden" name="delivery_address2" value="<?php echo stripslashes($_POST[address2])?>"/>
			<input type="hidden" name="delivery_town" value="<?php echo stripslashes($_POST[town])?>"/>
			<input type="hidden" name="delivery_county" value="<?php echo stripslashes($_POST[county])?>"/>
			<input type="hidden" name="delivery_postcode" value="<?php echo stripslashes($_POST[postcode])?>"/>
			<input type="hidden" name="delivery_tel" value="<?php echo $_POST[tel]?>"/>
			<input type="hidden" name="delivery_email" value="<?php echo $_POST[email]?>"/>
			<input type="hidden" name="deliverymethod" value="<?php echo $postage?>"/>
			
			<?php
			if ($_POST[billing] == "yes")
				{
				?>				
				<input type="hidden" name="name" value="<?php echo ("$_POST[title] $_POST[firstname] $_POST[surname]"); ?>"/>
				<input type="hidden" name="address" value="<?php echo $_POST[street]; ?>"/>
				<input type="hidden" name="address2" value="<?php echo $_POST[address2]; ?>"/>
				<input type="hidden" name="town" value="<?php echo stripslashes($_POST[town]); ?>"/>
				<input type="hidden" name="county" value="<?php echo stripslashes($_POST[county]); ?>"/>
				<input type="hidden" name="postcode" value="<?php echo $_POST[postcode]; ?>"/>
				<input type="hidden" name="telephone" value="<?php echo $_POST[tel]; ?>"/>
				<?php
				}
				?>
				
	
		
<div class="agree">
			<p><strong>By completing and submitting the electronic order form you are making an offer to purchase goods which, if accepted by us, will result in a binding contract. Read our <a href="../assets/popups/t_and_c.php" onclick="raw_popup('../assets/popups/t_and_c.php'); return false" title="Opens in a new window" class="new_window_small" target="_blank">terms and conditions of use</a></strong></p>
			
			<div id="terms">
			<h2>Please read to customer</h2>
			<p>This order is based on your acceptance of our terms and conditions of sale which can be found on our website at www.home-security-store.co.uk/terms_and_conditions.php. You have the right, in addition to your other rights, to cancel the order and receive a refund by informing us (Tihs Ltd trading as Redline Security, registered at Oak House, Groes Lwyd, Abergele, Conwy, LL22 7SU) in writing within 7 full working days of receipt of the goods, as long as they are in re-sellable condition and have not been taken out of their original packaging.</p><p>Any non-faulty goods that you want to return must be returned at your own expense. You will receive a refund of all monies paid for the goods (including outward delivery charges but excluding return postal charges and insurance) within 15 days of cancellation. </p><p>Where a claim of defect or damage is made you will need to return the goods to us, and will be entitled to a full refund (including delivery costs) plus any return postal charges if the goods are found to be defective.</p> <p>We strongly recommend that you refer to our full terms and conditions of sale on our website. A printed copy of our terms and conditions can be sent to you upon request.</p></div>		
			
			
			<p class="buttons"><label>Do you agree to these terms and conditions? <input type="checkbox" value="0" name="agree"></label></p>
			
			<noscript><div class="jsalert"><p>Javascript is disabled on your web browser. If you are using a javascript blocker please disable it for this website. You can still continue with the order, and by clicking continue you are indicating that the customer agree to the terms and conditions.</p></div></noscript>
			
			
			
			
			</div>
</div>
<p class="buttons"><label>Payment method</label>
	<select name="payment">
	<option value="Cheque">Cheque</option>
	<option value="Telephone">Telephone</option>	
	</select>
	</p>

<p>

</p>	
<p class="buttons"><a href="https://securetrading.net/merchantServices/" title="Opens in a new window" class="new_window_small" target="_blank">Open Secure Trading Terminal</a></p>
<p class="buttons">
<input name="submit" type="image" value="Submit" src="../assets/images/buttons/continue.jpg" alt="Continue"/></p>
</fieldset>


</form>

 <?php
		}
	}
else
	{
	//SHOW STANDARD FORM
	
	//EXPLODE DELIVERY INFO
	
	$delivery = $_SESSION['delivery'];
	$delivery_items = explode("|", $delivery);

	?>
    <p class="step">Admin Checkout > <strong>Delivery information</strong> > Confirmation > Payment > Complete</p>
    <form method="post" action="order_checkout.php?check=yes">
      <fieldset>
      <legend>Enter your delivery details below</legend>
	  <p>Fields marked <strong title="Required" class="required">*</strong> are required</p>
      <p>
        <label><span>Title <strong title="Required" class="required">*</strong></span>
        <input type="text" name="title" value="<?php echo stripslashes($delivery_items[0]); ?>" />
        </label>
      </p>
      <p>
        <label><span>First Name <strong title="Required" class="required">*</strong></span>
        <input type="text" name="firstname" value="<?php echo stripslashes($delivery_items[1]); ?>" />
        </label>
      </p>
      <p>
        <label><span>Surname <strong title="Required" class="required">*</strong></span>
        <input type="text" name="surname" value="<?php echo stripslashes($delivery_items[2]); ?>" />
        </label>
      </p>
      <p>
        <label><span>Street Address <strong title="Required" class="required">*</strong></span>
        <input type="text" name="street" value="<?php echo stripslashes($delivery_items[3]); ?>" />
        </label>
      </p>
      <p>
        <label><span>Address Line 2</span>
        <input type="text" name="address2" value="<?php echo stripslashes($delivery_items[4]); ?>" />
        </label>
      </p>
      <p>
        <label><span>Town / City <strong title="Required" class="required">*</strong></span>
        <input type="text" name="town" value="<?php echo stripslashes($delivery_items[5]); ?>" />
        </label>
      </p>
      <p>
        <label><span>County <strong title="Required" class="required">*</strong></span>
        <input type="text" name="county" value="<?php echo stripslashes($delivery_items[6]); ?>" />
        </label>
      </p>
      <p>
        <label><span>Post Code <strong title="Required" class="required">*</strong></span>
        <input type="text" name="postcode" value="<?php echo stripslashes($delivery_items[7]); ?>" />
        </label>
      </p>
      <p>
        <label><span>Telephone <strong title="Required" class="required">*</strong></span>
        <input type="text" name="tel" value="<?php echo stripslashes($delivery_items[8]); ?>" />
        </label>
      </p>
      <p>
        <label><span>Email <strong title="Required" class="required">*</strong></span>
        <input type="text" name="email" value="<?php echo stripslashes($delivery_items[9]); ?>" />
        &nbsp;(This will be the main point of contact) </label>
      </p>
      <p>
        <label><span>Delivery</span></label>
        <select name="deliverymethod">
          <option value="3.95" <?php if ($delivery_items[10] == "3.95") { echo ("selected='selected'"); } ?>>Royal Mail 1st Class (&#163;3.95)</option>
          <option value="8.95" <?php if ($delivery_items[10] == "8.95") { echo ("selected='selected'"); } ?>>Royal Mail Next Day (&#163;8.95)</option>
        <option value="Other" <?php if ($delivery_items[10] == "Other") { echo ("selected='selected'"); } ?>>Other</option>
        </select>&nbsp;Other: <input type="text" name="custompostage" value="<?php echo stripslashes($delivery_items[10]); ?>" />
      </p><input type="hidden" value="yes" name="billing" />
      
      </fieldset>
	  <p class="buttons">
        <input name="submit" type="image" value="Submit" src="../assets/images/buttons/continue.jpg" alt="Continue" />
      </p>
    </form>
	
    <?php
	}
	?>
  </div>
  <div id="footer">
    <?php 
	// THIS PULLS IN THE FOOTER
	require("../assets/widgets/footer.php"); 
	?>
  </div>
</div>

</body>
</html>
