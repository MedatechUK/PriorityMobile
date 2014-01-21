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

 function formatfilesize( $data ) {
        // bytes
        if( $data < 1024 ) {
            return $data . " bytes";
         }
        // kilobytes
        elseif( $data < 1024000 ) {
            return round( ( $data / 1024 ), 1 ) . "k";
		}
        // megabytes
        else {
        return round( ( $data / 1024000 ), 1 ) . " MB";
		}   
    }

//==========================================================================
//BACKUP PROCESS
//==========================================================================	
if ($_GET[backup])
	{	
	// TO SERVER
	if ($_GET[backup] == "server")
		{
		$backupFile = '/secure/o/oasisone/personal-attack-alarms.net/admin/backups/'. $name . date("Y-m-d-H-i-s") . '.gz';
		$command = "mysqldump -h$host -u$user -p$pass $name | gzip> $backupFile";
		system($command);
		}
	
	
	
	if ($_GET[backup] == "server")
		{
		$status = "<div id='status_ok'><p>The database has been backup successfully</p></div>";
		$process=mysql_query("insert into SHOP1_admin_tracker (name, loggedin, ip, job) values ('$_COOKIE[user_name]', '$tracker_time', '$ip', 'Database - backed up')");
		}
	else
		{
		$status = "<div id='status_error'><p>The backup failed</p></div>";
		}	
	}

//==========================================================================
//DELETE PROCESS
//==========================================================================	
if ($_GET[delete])
	{
	$backupFile = '/secure/o/oasisone/personal-attack-alarms.net/admin/backups/'. $_GET[delete];
	unlink("$backupFile");
	$status = "<div id='status_ok'><p>The file $_GET[delete] has been deleted</p></div>";
	$process=mysql_query("insert into SHOP1_admin_tracker (name, loggedin, ip, job) values ('$_COOKIE[user_name]', '$tracker_time', '$ip', 'Database - deleted')");
	}


//==========================================================================
//FOOTPRINT PROCESS
//==========================================================================	
if ($_GET[footprints])
	{
	$process=mysql_query("DELETE FROM SHOP1_footprints");
	if ($process)
		{
		$status = "<div id='status_ok'><p>The footprints table has been cleared</p></div>";
		$process=mysql_query("insert into SHOP1_admin_tracker (name, loggedin, ip, job) values ('$_COOKIE[user_name]', '$tracker_time', '$ip', 'Database - Footprints cleared')");
		}
	else
		{
		$status = "<div id='status_error'><p>There was an error when clearing the data from the footprint table</p></div>";
		}	
	}

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
  <?php require("../assets/widgets/admin_hidden.php"); ?>
</div>
<div id="container">
  <div id="header">
    <h2>Website</h2>
  </div>
  <div id="navigation">
    <?php 
	// THIS PULLS IN THE ADMIN LINKS
	require("widget_links.php"); 
	?>
  </div>
  <div id="main_content">
          <?php
echo("<h2>Backup database</h2>");
   

	

if (!$_GET[backup])
	{
	echo("<p><a href='back_up_database.php?backup=server'>Backup to server</a></p>");
	}


$dir = opendir('/secure/o/oasisone/personal-attack-alarms.net/admin/backups/');
$loc = "/secure/o/oasisone/personal-attack-alarms.net/admin/backups/";
echo ''.$status.'<div id="orders"><table><thead><tr><th>File name</th><th>Filesize</th><th>Created</th><th>Options</th></tr></thead><tbody>';

while ($read = readdir($dir))
	{
	if ($read!='.' && $read!='..')
		{
		if ($read == "index.html")
			{
			}
		else
			{
			$file = $loc.$read;
   			$fileinfo = stat($file);
    
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
				echo"<tr$class><td>" . $read . "</td>\t";
			echo "<td>" . round( ( $fileinfo[ "size" ] / 1024 ), 1 )  . "k</td>\n"; //  size in bytes 
			echo "<td>" . date( "H:i:s, d/m/y", $fileinfo[ "mtime" ] ) . "</td>\n"; //  time of last modification (Unix timestamp) 
			echo("<td><a href='back_up_database.php?delete=$read' style='color:#F00;'onClick='return confirmDelete()'>Delete</a></td>");
			echo '</tr>';
			}
		}
	}

echo '</tbody></table>';

closedir($dir); 

?>
<h2>Database Statistics</h2>

<table>
<thead>
  <tr>
    <th><strong>Table Name</strong></th>
    <th><strong>Rows</strong></th>
    <th><strong>Average Row Length</strong></th>
    <th><strong>Data Length</strong></th>
    <th><strong>Index Length</strong></th>
    <th><strong>Total Length</strong></th>
    <th><strong>Update Time</strong></th>
	<th></th>
  </tr>
</thead>
<tbody>
<?php

//$query = "SHOW TABLE STATUS";
$result = mysql_query("SHOW TABLE STATUS");

  $totalsize 	= 0;
  $t_rows		= 0;  
  $t_avg_row	= 0;
  $t_data		= 0;
  $t_ilength	= 0;
  $t_length		= 0;
  
  
        while( $row = mysql_fetch_array( $result ) ) {
        
            extract( $row );

                    $title = "";
                    $re = mysql_query( "EXPLAIN $Name" );
                    
                    $totallength = $Data_length + $Index_length;
                    $totalsize += $totallength;
                    
					
					$t_rows		+= $Rows;  
  					$t_avg_row	+= $Avg_row_length;
  					$t_data		+= $Data_length;
  					$t_ilength	+= $Index_length;
  					$t_length	+= $totallength;
					
                    $totallength = "<strong>" . formatfilesize( $totallength ) . "</strong>";
                    $Data_length = formatfilesize( $Data_length );
                    $Index_length = formatfilesize( $Index_length );
                    
					$Name = str_replace("SHOP1_", "", $Name);
					$Name = str_replace("_", " ", $Name);
                                    
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
				echo("<tr$class>");
                    echo "  <td class='grammar'>$Name</td>
                            <td>$Rows</td>
                            <td>$Avg_row_length</td>
                            <td>$Data_length</td>
                            <td>$Index_length</td>
                            <td>$totallength</td>
                            <td>$Update_time</td>
							<td>";
							
							if ($Name == "footprints")
								{
								echo("<a href='back_up_database.php?footprints=go_for_it' class='delete' onClick='return confirmDelete()'>Purge</a>");
								}
								
							if ($Name == "report popular")
								{
								echo("<a href='back_up_database.php?popular=go_for_it' class='delete' onClick='return confirmDelete()'>Purge</a>");
								}
								
							if ($Name == "search")
								{
								echo("<a href='back_up_database.php?search_terms=go_for_it' class='delete' onClick='return confirmDelete()'>Purge</a>");
								}
							
							
							
                    echo '</td></tr>';
					
					
        }

echo '<tr class="totals">';
echo "  <td>Totals</td>
		<td>$t_rows</td>
		<td>$t_avg_row</td>
		<td>".formatfilesize($t_data)."</td>
		<td>".formatfilesize($t_ilength)."</td>
		<td>".formatfilesize($totalsize)."</td>
		<td></td>
		<td></td>";
echo '</tr>';

?>
</tbody>
</table>   
</div>
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
