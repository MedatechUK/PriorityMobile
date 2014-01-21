<?php
require("assets/widgets/global_variables.php");

//LOGIN CHECKER
if(isset( $_POST['Submit'] ))
{
	//SIMPLE VERIFICATION, BUT SHOULD BE CHANGED TO DB ACCESS OF SOME FORM
	if($_POST['username'] == 'preview' && $_POST['pwd'] =='preview')
		{
		$username = $_POST['username'];
		$pwd = $_POST['pwd'];
		
		$ref = $_SERVER['HTTP_REFERER'];
		
		setcookie("admin", $username);
		header("Location: index.php");
		}
	else
		{
		header("Location: login.php?error=1");
		exit;
		}
}	
?>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<title><?php echo $site_name ?> | Site open soon</title>
<link rel="shortcut icon" href="<?php echo $site_url ?>assets/images/favicon.ico" />
<style>
<!--
body{
text-align: center;
font-family:Verdana, Arial, Helvetica, sans-serif;
margin: 38px 0 20px 0;
padding: 0;
font-size: 90%;
background: URL("assets/images/shell/background.gif") repeat-x #737373;}


a {color: #f7941d;font-weight: bold;}
a:hover {color: #f7941d;text-decoration:none;}


#hidden {display: none}

#container {
width:700px;
padding: 20px; 
margin:0 auto;
text-align: center;
background: #fff;
}

#container p {font-size: 100%;}

#footer {text-align: left; font-size: 10pt; border-top: 1px solid #ddd; padding-top: 10px}
#footer p {font-size: 8pt;}

#footer img {float: right; border: 1px solid #000;}


-->
</style>
</head>
<body>
<div id="container">
<p><img src="assets/images/large_logo.gif" alt="<?php echo $site_name ?>" /></p>
<?php
	
	//IF THERE IS AN ERROR LOGGING IN DISPLAY THIS MESSAGE
	if ($_GET[error] == 1)
		{
		echo("<h3>THE DETAILS YOU ENTERED WERE INCORRECT</h3>");
		}
	?>
	
    <form name="login" method="post" action="login.php">
      <fieldset>
      <legend>Enter your username and password</legend>
      <p>
      <label for="username">Username</label>
      <input type="text" name="username" id="username" />
      </p>
      <p>
      <label for="pwd">Password</label>
      <input type="password" name="pwd" id="pwd" />
      </p>
	  <p>
        <input type="submit" name="Submit" value="Login" />
      </p>
      </fieldset>
      
    </form>
<p><?php echo ("$site_tel | <a href='$site_email'>$site_email</a>");?></p>
 <div id="footer">
    <?php 
	// THIS PULLS IN THE FOOTER
	require("assets/widgets/footer.php"); 
	?>
  </div>
</div>
</body>
</html>
