<? 
/*
// Initialize the session.
// If you are using session_name("something"), don't forget it now!
session_start();
header("Cache-control: private"); // IE 6 Fix.

// Unset all of the session variables.
$_SESSION = array();

// If it's desired to kill the session, also delete the session cookie.
// Note: This will destroy the session, and not just the session data!
if (isset($_COOKIE[session_name()])) {
   setcookie(session_name(), '', time()-42000, '/');
}

// Finally, destroy the session.
session_destroy();

*/
setcookie("user_pwd");
setcookie("user_name");
$ref = $_SERVER['HTTP_REFERER'];
header("Location: login.php");
?>