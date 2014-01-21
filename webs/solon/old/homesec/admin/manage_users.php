<?php 
// SECURITY CHECK TO MAKE SURE THE CORRECT USERNAME AND PASSWORD HAS BEEN ENTERED
if(!$_COOKIE[user_name] | !$_COOKIE[user_pwd])
{
header("location: login.php");
exit;
}

require_once('../assets/widgets/mysql.class.php');
require_once('../assets/widgets/global.inc.php');
require_once('../assets/widgets/functions.inc.php');
require_once('../assets/widgets/global_variables.php');
?>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
<title>Admin Interface</title>
<link rel="shortcut icon" href="../assets/images/favicon.ico" />
<link rel="stylesheet" href="../assets/css/admin.css" type="text/css" media="screen" />
<script language="javascript" src="../assets/scripts/functions.js" type="text/javascript"></script>
<script language="javascript" type="text/javascript">
    
    var minpwlength = 4;
    var fairpwlength = 7;
    
    var STRENGTH_SHORT = 0;  // less than minpwlength 
    var STRENGTH_WEAK = 1;  // less than fairpwlength
    var STRENGTH_FAIR = 2;  // fairpwlength or over, no numbers
    var STRENGTH_STRONG = 3; // fairpwlength or over with at least one number
    
    img0 = new Image(); 
    img1 = new Image();
    img2 = new Image();
    img3 = new Image();
    
    img0.src = '../assets/images/admin/tooshort.jpg';
    img1.src = '../assets/images/admin/fair.jpg';
    img2.src = '../assets/images/admin/medium.jpg';
    img3.src = '../assets/images/admin/strong.jpg';
    
    var strengthlevel = 0;
    
    var strengthimages = Array( img0.src,
                                img1.src,
                                img2.src,
                                img3.src );
    
    function updatestrength( pw ) {
    
        if( istoosmall( pw ) ) {
    
            strengthlevel = STRENGTH_SHORT;
    
        }
        else if( !isfair( pw ) ) { 
    
            strengthlevel = STRENGTH_WEAK;
    
        }    
        else if( hasnum( pw ) ) {
    
            strengthlevel = STRENGTH_STRONG;
    
        }
        else {
    
            strengthlevel = STRENGTH_FAIR;
    
        }
    
        document.getElementById( 'strength' ).src = strengthimages[ strengthlevel ];
    
    }
    
    function isfair( pw ) {
    
        if( pw.length < fairpwlength ) {
    
            return false;
    
        }
        else { 
    
            return true;
    
        }
    
    }
    
    function istoosmall( pw ) {
    
        if( pw.length < minpwlength ) {
    
            return true;
    
        }
        else {
    
            return false;

        }
    
    }
    
    function hasnum( pw ) {
    
        var hasnum = false;
    
        for( var counter = 0; counter < pw.length; counter ++ ) {
    
            if( !isNaN( pw.charAt( counter ) ) ) {
    
                hasnum = true;
    
            }
    
        }
    
    
        return hasnum;
    
    }

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
//==========================================================================
//ADD USER
//==========================================================================

if($_GET[add] == "ok")
	{
		
?>
<h2>Add user</h2>
<form method="post" action="manage_users.php?added=ok" enctype="multipart/form-data">

<fieldset>
<legend>Details</legend>
<p><label for="name"><span>Name</span>
<input name="name" id="name" type="text"/></label>
</p>
<p><label for="email"><span>Email or username</span>
<input name="email" id="email" type="text"/></label>
</p>
<p><label for="pwd"><span>Password</span>
<input name="usrpwd" id="usrpwd" type="password" onkeyup="updatestrength( this.value );"/>&nbsp;<img src="../assets/images/admin/tooshort.jpg" id="strength" alt="" /></label>
</p>

</fieldset>
<fieldset>
<legend>Access Rights</legend>
<p><label><span>Order</span><input name="orders" type="checkbox" value="orders-"/></label></p>
<p><label><span>Catalog</span><input name="products" type="checkbox" value="products-"/></label></p>
<p><label><span>Marketing</span><input name="marketing" type="checkbox" value="marketing-"/></label></p>
<p><label><span>External Links</span><input name="external" type="checkbox" value="external-"/></label></p>
<p><label><span>Reports</span><input name="reports" type="checkbox" value="reports-"/></label></p>
</fieldset>

<p></p>
<div id="right"><input type="submit" name="Submit" value="Add user" /></div>

</form>
<?php
}
	
//==========================================================================
//EDIT USER
//==========================================================================
elseif($_GET[id])
	{
	$query = mysql_query("select * from $admin_table where id='$_GET[id]'");
	while($result = mysql_fetch_array($query))
	{
		
?>
<h2>Edit <?php echo("$result[name]"); ?> </h2>
<form method="post" action="manage_users.php?edit=<?php echo("$result[id]"); ?>" enctype="multipart/form-data">
<input name="id" type="hidden" value="<?php echo("$result[id]"); ?>"/>

<fieldset>
<legend>Details</legend>
<p><label for="name"><span>Name</span>
<input name="name" id="name" type="text" value="<?php echo("$result[name]"); ?>"/></label>
</p>
<p><label for="email"><span>Email or username</span>
<input name="email" id="email" type="text" value="<?php echo("$result[email]"); ?>"/></label>
</p>
<p><label><span>Password</span>Password is encrypted (to reset please delete user and add new)</label></p>

</fieldset>
<fieldset>
<legend>Access Rights</legend>
<p><label><span>Order</span><input name="orders" type="checkbox" value="orders-"<?php if (stristr($result[access_rights], "orders-") == TRUE) {echo(" checked='checked'");}?>/></label></p>
<p><label><span>Catalog</span><input name="products" type="checkbox" value="products-" <?php if (stristr($result[access_rights], "products-") == TRUE) {echo(" checked='checked'");}?>/></label></p>
<p><label><span>Marketing</span><input name="marketing" type="checkbox" value="marketing-" <?php if (stristr($result[access_rights], "marketing-") == TRUE) {echo(" checked='checked'");}?>/></label></p>
<p><label><span>External Links</span><input name="external" type="checkbox" value="external-" <?php if (stristr($result[access_rights], "external-") == TRUE) {echo(" checked='checked'");}?>/></label></p>
<p><label><span>Reports</span><input name="reports" type="checkbox" value="reports-" <?php if (stristr($result[access_rights], "reports-") == TRUE) {echo(" checked='checked'");}?>/></label></p>
</fieldset>

<p></p>
<div id="right"><input type="submit" name="Submit" value="Update" /></div>

</form>
<?php
}
}
else
	{
	echo("<h2>User Management</h2>");
	
	
//==========================================================================
//EDIT PROCESS
//==========================================================================
if($_GET[edit])
{
	$title 		=  	htmlentities($_POST[name], ENT_QUOTES);
	$update 	= 	date('YmdHis');
	//$pwd 		= 	md5($_POST[usrpwd]);
	
	$rights 	= 	"";
	
	if ($_POST[orders])
		{
		$rights .= $_POST[orders];
		}
	if ($_POST[marketing])
		{
		$rights .= $_POST[marketing];
		}
	if ($_POST[products])
		{
		$rights .= $_POST[products];
		}
	if ($_POST[external])
		{
		$rights .= $_POST[external];
		}
	if ($_POST[reports])
		{
		$rights .= $_POST[reports];
		}

	$process=mysql_query("update $admin_table set email='$_POST[email]', name='$title', access_rights='$rights' where id='$_POST[id]'"); 

	if (!$process)
	{
	echo("<div id='status_error'><p><strong>Oops!</strong></p><p>There was an error when you tried to update the user.</p></div>");
	}
	else
	{
	echo("<div id='status_ok'><p>Thank you the user has now been updated.</p></div>");
	$tracker=mysql_query("insert into $admin_track_table (name, loggedin, ip, job) values ('$_COOKIE[user_name]', '$tracker_time', '$ip', 'User - updated')");
	}
}



//==========================================================================
//ADD PROCESS
//==========================================================================
if($_GET[added])
{
	$title 		=  	htmlentities($_POST[name], ENT_QUOTES);
	$update 	= 	date('YmdHis');
	$pwd 		= 	md5($_POST[usrpwd]);
	
	$rights 	= 	"";
	
	if ($_POST[orders])
		{
		$rights .= $_POST[orders];
		}
	if ($_POST[marketing])
		{
		$rights .= $_POST[marketing];
		}
	if ($_POST[products])
		{
		$rights .= $_POST[products];
		}
	if ($_POST[external])
		{
		$rights .= $_POST[external];
		}
	if ($_POST[reports])
		{
		$rights .= $_POST[reports];
		}

	
	$process=mysql_query("insert into $admin_table (email, name, pwd, access_rights) values ('$_POST[email]', '$title', '$pwd', '$rights')");
	

	if (!$process)
	{
	echo("<div id='status_error'><p><strong>Oops!</strong><br/>There was an error when you tried to add the user.</p></div>");
	}
	else
	{
	echo("<div id='status_ok'><p>Thank you the user has now been added.</p></div>");
	$tracker=mysql_query("insert into $admin_track_table (name, loggedin, ip, job) values ('$_COOKIE[user_name]', '$tracker_time', '$ip', 'User - added ($title)')");
	}
}

//==========================================================================
//DELETE PROCESS
//==========================================================================
if($_GET[delete])
{

$query = mysql_query("select * from $admin_table where id='$_GET[delete]'");
$result = mysql_fetch_array($query);

$delete_db=mysql_query("delete from $admin_table where id='$_GET[delete]'");
if(!$delete_db)
	{
	echo("<div id='status_error'><p>The user could not be removed</p></div>");
	}
else
	{
	echo("<div id='status_ok'><p>The user has been removed from the system</p></div>");
	$tracker=mysql_query("insert into $admin_track_table (name, loggedin, ip, job) values ('$_COOKIE[user_name]', '$tracker_time', '$ip', 'User - deleted ($result[name])')");
	}
}

/////////////////////////////////////////////////////////



	echo("<p><a href='manage_users.php?add=ok'>Add new user</a></p>");
	echo("<div id='admin'>
	<table width='90%'>
		<thead>
			<tr><th>Name</th><th>Email / username</th><th></th><th>Options</th></tr>
		</thead>
		<tbody>
	
	");
	$query = mysql_query("select * from $admin_table where access_rights != 'master' order by name asc");
	while($result = mysql_fetch_array($query))
		{
		if($num == "1")
					{
					$class=" class='row1'";
					$num=2;
					}
				else
					{
					$class=" class='row2'";
					$num=1;
					}
				echo("<tr$class><td>$result[name]</td><td><small>$result[email]</small></td><td></td><td><a href='manage_users.php?id=$result[id]'>Edit</a> | <a href='manage_users.php?delete=$result[id]' class='delete' onClick='return confirmDelete()'>Delete</a></td></tr>");		
		}
	echo("</tbody></table></div>");
	}
?></div>
  <div id="footer">
  <?php 
	// THIS PULLS IN THE FOOTER
	require("../assets/widgets/footer.php"); 
	?>
  </div>
</div>
</body>
</html>
