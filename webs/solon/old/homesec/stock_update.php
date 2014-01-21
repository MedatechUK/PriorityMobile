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
 </div>
  <div id="main_content">
    <?php 
	echo("$site_notice");
	?>
    <h2>Stock update</h2>
	<?php
	
	if ($_GET[confirm] == "yes")
		{
		if (!$_POST[from_name] || !$_POST[from_email]|| !ereg('^[-!#$%&\'*+\\./0-9=?A-Z^_`a-z{|}~]+'.
              '@'.
              '[-!#$%&\'*+\\/0-9=?A-Z^_`a-z{|}~]+\.'.
              '[-!#$%&\'*+\\./0-9=?A-Z^_`a-z{|}~]+$', $_POST[from_email]))
			{
			//SHOW VERIFICATION STUFF
			$error = "&nbsp;<font class='error'>Please enter this field</font>";
			?>
			 <form action="stock_update.php?confirm=yes&amp;product_id=<?php echo("$_GET[product_id]"); ?>" method="post" class="stock">
			  <fieldset>
			  <legend>Stock update</legend>
			  <small>Please fill your details below if you would like to be notified when the <?php echo("$result[name]");?> comes back into stock</small>
			  <p>
				<label><span>Your name <strong title="Required" class="required">*</strong></span>
				<input name="from_name" type="text" id="from_name" value="<?php echo stripslashes($_POST[from_name])?>" />
				<?php if (!$_POST[from_name]) { echo ("$error"); } ?></label>
			  </p>
			  <p>
				<label><span>Your email address <strong title="Required" class="required">*</strong></span>
				<input name="from_email" type="text" id="from_email" value="<?php echo stripslashes($_POST[from_email])?>"/>
			  <?php if (!$_POST[from_email]) { echo ("$error"); } ?></label>
			  </p>
			  <p>
				<input name="submit" type="image" value="Submit" src="assets/images/buttons/continue.jpg" alt="Continue"/></p>

			  </fieldset>
    		</form>
			
			<?php
			}
		else
			{
			$update = date('YmdHis');
			$process=mysql_query("INSERT INTO $stock_table (name, email, product_id, date_added, status) VALUES ('$_POST[from_name]', '$_POST[from_email]', '$_GET[product_id]', '$update', 'Open')"); 
			
			if (!$process)
				{
				echo("<p style='color: #F00'>Sorry there was an error when using this service. Please try again later</p>");
				}
			else
				{
				echo("<p>Thank you $_POST[from_name]. You have been added to our stock update alert. Once this product is back in stock you will be removed from the system</p><p><a href='products.php' title='Click here to go back to product list'><img src='assets/images/back_button.gif' alt='Back to product list' border='0'/></a></p>");
				}			
			}
		}
	else
		{
		?>
		
    <form action="stock_update.php?confirm=yes&amp;product_id=<?php echo("$_GET[product_id]"); ?>" method="post" class="stock">
      <fieldset>
      <legend>Stock update</legend>
      <small>Please fill your details below if you would like to be notified when the product comes back into stock</small>
      <p>
        				<label><span>Your name <strong title="Required" class="required">*</strong></span>

        <input name="from_name" type="text" id="from_name" /></label>
      </p>
      <p>
        				<label><span>Your email address <strong title="Required" class="required">*</strong></span>

        <input name="from_email" type="text" id="from_email" /></label>
      </p>
      <p>
        <input name="submit" type="image" value="Submit" src="assets/images/buttons/continue.jpg" alt="Continue"/></p>

      </fieldset>
    </form>
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
<?php } ?>
