<?php
session_start();

//IF THE USER IS RETURNING FROM A COMPLETE ORDER CLEAR SESSION
if ($_GET[clear] == "all")
	{
	session_destroy();
	header ("location: index.php");
	}

require_once('assets/widgets/mysql.class.php');
require_once('assets/widgets/global.inc.php');
require_once('assets/widgets/functions.inc.php');
require_once('assets/widgets/global_variables.php');
require_once('assets/widgets/maintenance.php');

$page_query = mysql_query("select * from $content_table where id='5'");
while($page_result = mysql_fetch_array($page_query))
	{

?>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<?php require("assets/widgets/meta.php"); ?>
</head>
<body>
<?php 
echo("$js_notice");
require("assets/widgets/hidden.php"); 
?>
<div id="container">
  <?php require("assets/widgets/header.php"); ?>
  <?php require("assets/widgets/nav.php"); ?>
  <div id="left_col">
  <?php require("assets/widgets/categories.php"); ?>
  <?php require("assets/widgets/best_sellers.php"); ?>
 </div>
  <div id="main_content">
     <?php 
	echo("$site_notice");
	echo("$page_result[content]"); 
	?>
	<?php
	  
	  
	  if ($_POST[checker] == "yup")
	  	{
		if(!$_POST[fname] || !$_POST[hmun] || !ereg('^[-!#$%&\'*+\\./0-9=?A-Z^_`a-z{|}~]+'.
              '@'.
              '[-!#$%&\'*+\\/0-9=?A-Z^_`a-z{|}~]+\.'.
              '[-!#$%&\'*+\\./0-9=?A-Z^_`a-z{|}~]+$', $_POST[liame]))
			{
			$alert = "<p class='errormessage'>Please enter all the required information</p>";
			$error = "&nbsp;<font class='error'>Please enter this field</font>";
			?>
			
			<?php echo("$alert"); ?>
			
			   <form method="post" action="contact.php" name="contact" id="small_form">
          <fieldset>
          <legend>Enquiry Form</legend>
          <p>Please note that fields marked with * are required</p>
		  
          <p>
            <label><span>Name</span>
            <input type="text"  class="textf" name="fname" value="<?php echo("$_POST[fname]")?>"/>
            * <?php if (!$_POST[fname]) { echo ("$error"); } ?></label>
          </p>
          <p>
            <label><span>eMail address</span>
            <input type="text"  class="textf" name="liame" value="<?php echo("$_POST[liame]")?>"/>
            * <?php if (!$_POST[liame]) { echo ("$error"); } ?></label>
          </p>
          <p>
            <label><span>Telephone number</span>
            <input type="text"  class="textf" name="hmun" value="<?php echo("$_POST[hmun]")?>"/>
            * <?php if (!$_POST[hmun]) { echo ("$error"); } ?></label>
          </p>
          <p>
            <label>Request<br />
            <textarea name="message" rows="5" cols="40"><?php echo("$_POST[message]")?></textarea>
            </label>
          </p>
          <input type="hidden" name="checker" value="yup" />
          <p class="buttons">
        <input name="submit" type="image" value="Submit" src="assets/images/buttons/continue.jpg" alt="Continue" />
      </p>
          </fieldset>
        </form>
        <?php
			
			}
		else
		{


$query = nl2br($_POST[message]);
		
$message = '<html>
<body style="font-family:Verdana;font-size:12px;background:#fff;">
<table width="550" border="0" cellpadding="0" style="margin:5px;">
<tr><td>
<img src="'.$site_url.'assets/images/email_banner.gif" alt="'.$site_name.'"/>
</td></tr>
<tr><td style="padding:10px"><h2 style="color: #000; padding: 0; margin: 0; font-size: 120%;">Request from Website</h2>
      
      <h3 style="color: #000; padding: 0; margin: 0; font-size: 110%;">Contact information</h3>
      <p style="font-size:80%">
	  	Name: '.$_POST[fname].'<br/>
	 	 Telephone: '.$_POST[hmun].'<br/>
      	eMail: '.$_POST[liame].'<br/>
		Message: '.$query.'
		</p>
		</td>
  </tr>
<tr>
  <td style="padding:10px;margin:0;font-size:70%;color:#999;background:#333;width:100%;">
<p>Copyright 2004-'.date("Y").' Redline Security. All Rights Reserved. '.$site_name.' is a trading name of TIHS Ltd.</p>
<p>Our registered office is: Oak House, Groes Lwyd, Abergele, Conwy LL22 7SU. We are registered in England and Wales. Company No 5325603. VAT Number 855446109.</p>
</td>
</tr>
</table>
</body>
</html>';	

$autoresponse = '<html>
<body style="font-family:Verdana;font-size:12px;background:#fff;">
<table width="550" border="0" cellpadding="0" style="margin:5px;">
<tr><td>
<img src="'.$site_url.'assets/images/email_banner.gif" alt="'.$site_name.'"/>
</td></tr>
<tr><td style="padding:10px"><h2 style="color: #000; padding: 0; margin: 0; font-size: 120%;">Request from Website</h2>
<p style="font-size:80%">Dear '.$_POST[fname].'</p>
<p style="font-size:80%">Thanks for contacting '.$site_name.'. This is just an automated email to let you know that we\'ve received your query. We\'ll be in touch with you as soon as possible.</p>
<p style="font-size:80%">Thanks again for contacting '.$site_name.'.</p>
		</td>
  </tr>
<tr>
  <td style="padding:10px;margin:0;font-size:70%;color:#999;background:#333;width:100%;">
<p>Copyright 2004-'.date("Y").' Redline Security. All Rights Reserved. '.$site_name.' is a trading name of TIHS Ltd.</p>
<p>Our registered office is: Oak House, Groes Lwyd, Abergele, Conwy LL22 7SU. We are registered in England and Wales. Company No 5325603. VAT Number 855446109.</p>
</td>
</tr>
</table>
</body>
</html>';	
	
		
$headers  = "MIME-Version: 1.0\r\n";
$headers .= "Content-type: text/html; charset=iso-8859-1\r\n";
$headers .= "To: Home Security <aaron@redlinesecurity.co.uk>\r\n";
$headers .= "From:  ".$_POST[fname]." <".$_POST[liame].">\r\n";
$subject = "Information Request from Home Security Store";
mail($to, $subject, $message, $headers);		

$headers  = "MIME-Version: 1.0\r\n";
$headers .= "Content-type: text/html; charset=iso-8859-1\r\n";
$headers .= "From: Home Security <".$site_email.">\r\n";
$headers .= "To:  ".$_POST[fname]." <".$_POST[liame].">\r\n";
$subject = "Thanks for contacting ".$site_name." customer support";
mail($to, $subject, $autoresponse, $headers);	

	

	  
	  echo("<p><strong>Your query has been sent</strong></p>");
echo("<p>Thank you $_POST[fname]. We will be in touch as soon as possible.
</p>");
		
		
		
		}
		
		}
	else
		{
	  	?>
		
        <form method="post" action="contact.php" name="contact" id="small_form">
          <fieldset>
          <legend>Enquiry Form</legend>
          <p><small>Please note that fields marked with * are required</small></p>
          <p>
            <label><span>Name</span>
            <input type="text"  class="textf" name="fname"/>
            * </label>
          </p>
          <p>
            <label><span>eMail address</span>
            <input type="text"  class="textf" name="liame" />
            * </label>
          </p>
          <p>
            <label><span>Telephone number</span>
            <input type="text"  class="textf" name="hmun"/>
            * </label>
          </p>
          <p>
            <label>Query<br/>
            <textarea name="message" rows="5" cols="40"></textarea>
            </label>
          </p>
          <input type="hidden" name="checker" value="yup" />
          <p class="buttons">
        <input name="submit" type="image" value="Submit" src="assets/images/buttons/continue.jpg" alt="Continue" />
      </p>
          </fieldset>
        </form>
        <?php } ?>
		
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
<?php } ?>