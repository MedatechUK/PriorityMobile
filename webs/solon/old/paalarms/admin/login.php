<?php
$host = 'mysql.personal-attack-alarms.net';
$user = 'paalarms';
$pass = '477ack';
$database = 'PAALARMS';
mysql_connect($host, $user, $pass);
  mysql_select_db($database);


//LOGIN CHECKER
if(isset( $_POST['Submit'] ))
{


	// Date and IP Address
	$ip = $_SERVER[REMOTE_ADDR]; 
	$day = date('d-m-Y');
	$time = date('YmdHis');
	
	$password = $_POST[pwd];
	$password = md5($password);
	$username = $_POST[username];

	// If all has been entered it will then check database for details
	if (!$username || !$password) 
		{
		header("Location: login.php?error=1");
		}
	else
	{
	$sql=mysql_query("select * from SHOP1_admin_user where email='$username' and pwd='$password'") ;
	$sql_ar=mysql_fetch_array($sql) ;
	$sql_result=mysql_num_rows($sql) ;

	// If not found in DB will go to error page
	if($sql_result==0)
		{
		$process=mysql_query("insert into SHOP1_admin_tracker (name, loggedin, ip, job) values ('$username', '$time', '$ip', 'Failed login - Not in DB')");
		header("Location: login.php?error=1");
		exit;
		}
		else
		{
		$id = $sql_ar[access_rights];
		$name = $sql_ar[name];
		$admin = $sql_ar[pwd];
		
		setcookie("user_name", $name);
		setcookie("user_id", $id);
		setcookie("user_pwd", $admin);
		
		$process=mysql_query("insert into SHOP1_admin_tracker (name, loggedin, ip, job) values ('$name', '$time', '$ip', 'Logged in - $name')");
		header("Location: index.php");
		exit;
		
			
		}
	}
}
?>	

<?php
/*
//LOGIN CHECKER
if(isset( $_POST['Submit'] ))
{
	//SIMPLE VERIFICATION, BUT SHOULD BE CHANGED TO DB ACCESS OF SOME FORM
	if($_POST['username'] == 'admin' && $_POST['pwd'] =='paad0tn3t')
		{
		$username = $_POST['username'];
		$pwd = $_POST['pwd'];
		
		$ref = $_SERVER['HTTP_REFERER'];
		
		setcookie("user_name", $username);
		setcookie("user_pwd", $pwd);
		header("Location: index.php");
		}
	else
		{
		header("Location: login.php?error=1");
		exit;
		}
}	*/
?>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
<title>Admin Interface</title>
<link rel="shortcut icon" href="http://www.personal-attack-alarms.net/favicon.ico" />
<link rel="stylesheet" href="../assets/css/admin.css" type="text/css" media="screen" />
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
  </div>
  <div id="navigation">
    <ul>
<li><a href="../index.php" title="Click here to go to our home page">Home</a></li>
<li><a href="../products.php" title="Click here to go to our product list">Products</a></li>
<li><a href="../about.php" title="Click here to find out who we are">About us</a></li>
<li><a href="../contact.php" title="Click here to go to get information on how to contact us">Contact us</a></li>
<li><a href="../how_a_personal_attack_alarm_will_help.php" title="Click here to go find out why you should buy a personal attack alarm">Why buy?</a></li>

</ul>
  </div>
  <div id="main_content">
    <h2>Login</h2>
    
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
      <p></p>
      <label for="username">Email or username<br />
      <input type="text" name="username" id="username" />
      </label>
      <p></p>
      <label for="pwd">Password<br />
      <input type="password" name="pwd" id="pwd" />
      </label>
      </fieldset>
      <p>
        <input type="submit" name="Submit" value="Login" />
      </p>
    </form>
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
