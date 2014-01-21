<?php
require_once('assets/widgets/mysql.class.php');
require_once('assets/widgets/global.inc.php');
require_once('assets/widgets/functions.inc.php');
require("assets/widgets/global_variables.php");
require_once('assets/widgets/maintenance.php');
session_start();
?>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<title><?php echo $site_name; ?> | Checkout</title>
<?php require("assets/widgets/meta.php"); ?>
<script language="javascript" type="text/javascript" src="assets/scripts/lib.js"></script>
<script language="javascript" type="text/javascript" src="assets/scripts/popup.js"></script>
<script language="JavaScript" type="text/javascript">
<!--

function checkCheckBox(f){
if (f.agree.checked == false )
{
alert('Please read our terms and conditions and agree to them.');
return false;
}else
return true;
}
//-->
</script>	

</head>
<body>
<?php 
echo("$js_notice");
?>
<?php //require("assets/widgets/google.php"); ?>
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
	if ($_GET[check] == 'yes')
	{	
	if (!$_POST[title] || !$_POST[firstname] || !$_POST[surname] || !$_POST[street] || !$_POST[town] || !$_POST[county] || !$_POST[postcode] || !$_POST[email] || !$_POST[tel] || !ereg('^[-!#$%&\'*+\\./0-9=?A-Z^_`a-z{|}~]+'.
              '@'.
              '[-!#$%&\'*+\\/0-9=?A-Z^_`a-z{|}~]+\.'.
              '[-!#$%&\'*+\\./0-9=?A-Z^_`a-z{|}~]+$', $_POST[email]) || ereg('[^0-9\+\-\(\)\ ]', $_POST[tel]))
		{
		//SHOW VERIFICATION STUFF
		
		$error = "&nbsp;<font class='error'>Please enter this field</font>";
		
		?>
		<h2>Checkout > Delivery</h2>
    <p class="step">Checkout > <strong>Delivery information</strong> > Confirmation > Payment > Complete</p>
    <p class="errormessage">Please enter all the required information</p>
    <form method="post" action="checkout.php?check=yes">
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
		<?php if (!$_POST[tel]) { echo ("$error"); } ?>
		<?php
		if (!$_POST[tel]) { echo ("$error"); } 
		 elseif (ereg('[^0-9\+\-\(\)\ ]', stripslashes($_POST[tel]))){
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
        <label><span>Please select delivery type</span></label>
        <select name="deliverymethod">
          <option value="<?= $POSTAGE_ONE_PRICE;?>" <?php if ($_POST[cover] == $POSTAGE_ONE_PRICE) { echo ("selected='selected'"); } ?>>Royal Mail 1st Class (&#163;<?= $POSTAGE_ONE_PRICE;?>)</option>
          <option value="<?= $POSTAGE_TWO_PRICE; ?>" <?php if ($_POST[cover] == $POSTAGE_TWO_PRICE) { echo ("selected='selected'"); } ?>>Royal Mail Next Day (&#163;<?= $POSTAGE_TWO_PRICE;?>)</option>
        </select> <a href="assets/popups/d_and_r.php" onclick="raw_popup('assets/popups/d_and_r.php'); return false" title="Opens in a new window" target="_blank" class="new_window">Delivery information</a>
      </p>
	  	  <p><label><input type="checkbox" value="yes" name="billing"<?php if ($_POST[billing] == "yes") { echo ("checked='checked'"); } ?>/>Please tick here if the above details are the same as your billing address</label></p>

      <p class="buttons">
	  <input name="submit" type="image" value="Submit" src="assets/images/buttons/continue.jpg" alt="Continue" /> 
      </p>
      </fieldset>
    </form>
	<?php echo $postage_notice; //BRING IN POSTAL NOTICE ?>
    <?php	
		}
	else
		{
		//SHOW CONFIRMATION INFORMATION
	?>
	<h2>Checkout > Confirm</h2>
    <p class="step">Checkout > Delivery information > <strong>Confirmation</strong> > Payment > Complete</p>
    <form method="post" action="<?php echo ("https://securetrading.net/authorize/form.cgi"); ?>" onsubmit="javascript:__utmLinkPost(this);return checkCheckBox(this)">
      <fieldset>
      <legend>Please confirm all details below are correct</legend>
      <div id="confirm">
        <h3>Your items (<a href="basket.php" title="Edit the content of your basket">edit</a>)</h3>
        <?php
		echo confirmCart();
		?>
		<div class="summary_delivery">
        <h3>Delivery Address (<a href="checkout.php" title="Edit your deilvery information">edit</a>)</h3>
        <?php
		
		// Adds the delivery info to session 
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
		$_SESSION['delivery'] .= '|'.$_POST['deliverymethod'];
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
		
		?></div>	
		
			<input type="hidden" name="merchant" value="tihs10321"/>
			<input type="hidden" name="orderref" value="HS <?php echo date("y-mdHi") ?>"/>
			<input type="hidden" name="inputamount" value="<?php echo $total ?>"/>
			<input type="hidden" name="currency" value="GBP"/>
			<input type="hidden" name="merchantemail" value="peter@theitc.co.uk"/>
			<input type="hidden" name="customeremail" value="0"/>
			<input type="hidden" name="callbackurl" value="5"/>
			<input type="hidden" name="failureurl" value="5"/>
			<input type="hidden" name="settlementday" value="1"/>
			<input type="hidden" name="formref" value="5"/>
			<input type="hidden" name="email" value="<?php echo $_POST[email]; ?>"/>
			<input type="hidden" name="orderinfo" value="<?php echo checkoutCart();?>"/>
			<input type="hidden" name="orderdetail" value="<?php echo checkoutCart();?>"/>
			<input type="hidden" name="cartinfo" value="<?php echo $_SESSION['cart'];?>"/>
			<input type="hidden" name="cartdetail" value="<?php echo $_SESSION['cartdetail'];?>"/>
			<input type="hidden" name="total" value="<?php echo $total ?>"/>
			<input type="hidden" name="postage" value="<?php echo $_POST[deliverymethod]; ?>"/>
			<input type="hidden" name="delivery_name" value="<?php echo stripslashes($_POST[title]).' '.stripslashes($_POST[firstname]).' '.stripslashes($_POST[surname])?>"/>
			<input type="hidden" name="delivery_address" value="<?php echo stripslashes($_POST[street])?>"/>
			<input type="hidden" name="delivery_address2" value="<?php echo stripslashes($_POST[address2])?>"/>
			<input type="hidden" name="delivery_town" value="<?php echo stripslashes($_POST[town])?>"/>
			<input type="hidden" name="delivery_county" value="<?php echo stripslashes($_POST[county])?>"/>
			<input type="hidden" name="delivery_postcode" value="<?php echo stripslashes($_POST[postcode])?>"/>
			<input type="hidden" name="delivery_tel" value="<?php echo $_POST[tel]?>"/>
			<input type="hidden" name="delivery_email" value="<?php echo $_POST[email]?>"/>
			<input type="hidden" name="deliverymethod" value="<?php echo $_POST[deliverymethod]?>"/>
			
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
			<p><strong>By completing and submitting the electronic order form you are making an offer to purchase goods which, if accepted by us, will result in a binding contract. Read our <a href="assets/popups/t_and_c.php" onclick="raw_popup('assets/popups/t_and_c.php'); return false" title="Opens in a new window" class="new_window_small" target="_blank">terms and conditions of use</a></strong></p>
			</div>
		
		<noscript><div class="jsalert"><p>Javascript is disabled on your web browser. If you are using a javascript blocker please disable it for this website. You can still continue with the order, and by clicking continue you are indicating that you agree to the terms and conditions.</p></div></noscript>

<p class="buttons"><label>I accept the terms and conditions <input type="checkbox" value="0" name="agree"></label></p>
<p class="buttons">
<input name="submit" type="image" value="Submit" src="assets/images/buttons/continue.jpg" alt="Continue"/></p>
</fieldset>


</form>
<?php echo $postage_notice; //BRING IN POSTAL NOTICE ?>
 <?php
		}
	}
else
	{
	//SHOW STANDARD FORM
	
	//EXPLODE DELIVERY INFO
	
	$delivery = $_SESSION['delivery'];
	$delivery_items = explode("|", $_SESSION[delivery]);
	
	?>
    <p class="step">Checkout > <strong>Delivery information</strong> > Confirmation > Payment > Complete</p>
    <form method="post" action="checkout.php?check=yes">
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
        <label><span>Please select delivery type</span></label>
        <select name="deliverymethod">
           
          <option value="<?= $POSTAGE_ONE_PRICE;?>" <?php if ($delivery_items[10] == $POSTAGE_ONE_PRICE) { echo ("selected='selected'"); } ?>>Royal Mail 1st Class (&#163;<?= $POSTAGE_ONE_PRICE;?>)</option>
          <option value="<?= $POSTAGE_TWO_PRICE;?>" <?php if ($delivery_items[10] == $POSTAGE_TWO_PRICE) { echo ("selected='selected'"); } ?>>Royal Mail Next Day (&#163;<?= $POSTAGE_TWO_PRICE;?>)</option>
        </select> <a href="assets/popups/d_and_r.php" onclick="raw_popup('assets/popups/d_and_r.php'); return false" title="Opens in a new window" target="_blank" class="new_window">Delivery information</a>
      </p>
	  <p><label><input type="checkbox" value="yes" name="billing" <?php if ($delivery_items[11] == "yes") { echo ("checked='checked'"); } ?>/>Please tick here if the above details are the same as your billing address</label></p>
      <p class="buttons">
        <input name="submit" type="image" value="Submit" src="assets/images/buttons/continue.jpg" alt="Continue" />
      </p>
      </fieldset>
    </form>
	<?php echo $postage_notice; //BRING IN POSTAL NOTICE ?>
    <?php
	}
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
