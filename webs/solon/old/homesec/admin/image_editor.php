    <?php

if(isset( $_POST['uploaded'] ))
{
//If the form submitted do:
echo("<div class='view_large'>");

 if ($_FILES['imagefile'])
	{

	$maxsize = "200000";
	$friendly = "200KB (0.2MB)";

	if ($_FILES['imagefile']['size'] > $maxsize)
		{
		echo("<h3>Oops there was a problem</h3><p class='cat'>File size exceeds maximum size ($friendly).</p><p><a href='gallery_add.php'>Add another image</a></p>");
		}
	else
		{

		$sent = date("d-m-Y H:i:s");
		$ip = $_SERVER['REMOTE_ADDR'];

		if($_FILES['imagefile']['size'] > 0)
		{
		$fileName = $_FILES['imagefile']['name'];
		$tmpName  = $_FILES['imagefile']['tmp_name'];
		$fileSize = $_FILES['imagefile']['size'];
		$fileType = $_FILES['imagefile']['type'];

		$fp      = fopen($tmpName, 'r');
		$content = fread($fp, filesize($tmpName));
		$content = addslashes($content);
		fclose($fp);

		if(!get_magic_quotes_gpc())
		{
			$fileName = addslashes($fileName);
		}

		$exists = mysql_query("select count(id) from $gallery_table where name ='$fileName'");
		$countem = mysql_fetch_array($exists);

		if($countem[0] > 0)
		{
		echo("	<h3>Oops there was a problem</h3>
				<p class='cat'>Sorry the image you are uploading already exists</p>");
		}
		else
		{
		// COPY ORIGINAL TO ORIGINALS FOLDER
		copy ($_FILES['imagefile']['tmp_name'], "../assets/images/products/originals/".$_FILES['imagefile']['name'])
		or die ("Could not copy");


//==========================================================================
//==========================================================================

if ($process_thumbnail == "YES")
{
// CREATE THUMBNAIL AND SEND TO THUMBS FOLDER
// The file
$filename = "../assets/images/products/originals/".$_FILES['imagefile']['name'];

// Create Image from MIME Type

$mimetype = $_FILES['imagefile']['type'];
switch($mimetype) {
case "image/jpg":
case "image/jpeg":
case "image/pjpeg":
$i = imagecreatefromjpeg($filename);
break;
case "image/gif":
$i = imagecreatefromgif($filename);
break;
case "image/png":
$i = imagecreatefrompng($filename);
break;
}

//VARIABLES

$nw=100; //The Width Of The Thumbnails
$nh=100; //The Height Of The Thumbnails

$tpath = "../assets/images/products/thumbs";

$dimensions = getimagesize($filename);

$thname = "$tpath/".$_FILES['imagefile']['name'];

$w=$dimensions[0];
$h=$dimensions[1];

$thumb = imagecreatetruecolor($nw,$nh);
	
$wm = $w/$nw;
$hm = $h/$nh;
	
$h_height = $nh/2;
$w_height = $nw/2;
	
if($w > $h){
	
	$adjusted_width = $w / $hm;
	$half_width = $adjusted_width / 2;
	$int_width = $half_width - $w_height;
	
	imagecopyresampled($thumb,$i,-$int_width,0,0,0,$adjusted_width,$nh,$w,$h); 
	imagejpeg($thumb,$thname,95); 
	imagedestroy($thumb);
	
}elseif(($w < $h) || ($w == $h)){
	
	$adjusted_height = $h / $wm;
	$half_height = $adjusted_height / 2;
	$int_height = $half_height - $h_height;
	
	imagecopyresampled($thumb,$i,0,-$int_height,0,0,$nw,$adjusted_height,$w,$h); 
	imagejpeg($thumb,$thname,95); 
	imagedestroy($thumb);
	
}else{
	imagecopyresampled($thumb,$i,0,0,0,0,$nw,$nh,$w,$h); 	
	imagejpeg($thumb,$thname,95); 
	imagedestroy($thumb);
}



// CREATE SMALL AND SEND TO SMALL FOLDER
// The file
$filename = "../assets/images/products/originals/".$_FILES['imagefile']['name'];

// Create Image from MIME Type

$mimetype = $_FILES['imagefile']['type'];
switch($mimetype) {
case "image/jpg":
case "image/jpeg":
case "image/pjpeg":
$i = imagecreatefromjpeg($filename);
break;
case "image/gif":
$i = imagecreatefromgif($filename);
break;
case "image/png":
$i = imagecreatefrompng($filename);
break;
}

//VARIABLES

$nw=75; //The Width Of The Thumbnails
$nh=75; //The Height Of The Thumbnails

$tpath = "../assets/images/products/small";

$dimensions = getimagesize($filename);

$thname = "$tpath/".$_FILES['imagefile']['name'];

$w=$dimensions[0];
$h=$dimensions[1];

$thumb = imagecreatetruecolor($nw,$nh);
	
$wm = $w/$nw;
$hm = $h/$nh;
	
$h_height = $nh/2;
$w_height = $nw/2;
	
if($w > $h){
	
	$adjusted_width = $w / $hm;
	$half_width = $adjusted_width / 2;
	$int_width = $half_width - $w_height;
	
	imagecopyresampled($thumb,$i,-$int_width,0,0,0,$adjusted_width,$nh,$w,$h); 
	imagejpeg($thumb,$thname,95); 
	imagedestroy($thumb);
	
}elseif(($w < $h) || ($w == $h)){
	
	$adjusted_height = $h / $wm;
	$half_height = $adjusted_height / 2;
	$int_height = $half_height - $h_height;
	
	imagecopyresampled($thumb,$i,0,-$int_height,0,0,$nw,$adjusted_height,$w,$h); 
	imagejpeg($thumb,$thname,95); 
	imagedestroy($thumb);
	
}else{
	imagecopyresampled($thumb,$i,0,0,0,0,$nw,$nh,$w,$h); 	
	imagejpeg($thumb,$thname,95); 
	imagedestroy($thumb);
}


}
//==========================================================================
//==========================================================================
if ($process_large == "YES")
{
		// CREATE THUMBNAIL AND SEND TO THUMBS FOLDER
		// The file
		$filename = "../assets/images/products/originals/".$_FILES['imagefile']['name'];

		// Create Image from MIME Type

$mimetype = $_FILES['imagefile']['type'];
switch($mimetype) {
case "image/jpg":
case "image/jpeg":
case "image/pjpeg":
$i = imagecreatefromjpeg($filename);
break;
case "image/gif":
$i = imagecreatefromgif($filename);
break;
case "image/png":
$i = imagecreatefrompng($filename);
break;
}

//VARIABLES

$nw=500; //The Width Of The Thumbnails
$nh=500; //The Height Of The Thumbnails

$tpath = "../assets/images/products/large";

$dimensions = getimagesize($filename);

$thname = "$tpath/".$_FILES['imagefile']['name'];

$w=$dimensions[0];
$h=$dimensions[1];

$thumb = imagecreatetruecolor($nw,$nh);
	
$wm = $w/$nw;
$hm = $h/$nh;
	
$h_height = $nh/2;
$w_height = $nw/2;
	
if($w > $h){
	
	$adjusted_width = $w / $hm;
	$half_width = $adjusted_width / 2;
	$int_width = $half_width - $w_height;
	
	imagecopyresampled($thumb,$i,-$int_width,0,0,0,$adjusted_width,$nh,$w,$h); 
	imagejpeg($thumb,$thname,95); 
	imagedestroy($thumb);
	
}elseif(($w < $h) || ($w == $h)){
	
	$adjusted_height = $h / $wm;
	$half_height = $adjusted_height / 2;
	$int_height = $half_height - $h_height;
	
	imagecopyresampled($thumb,$i,0,-$int_height,0,0,$nw,$adjusted_height,$w,$h); 
	imagejpeg($thumb,$thname,95); 
	imagedestroy($thumb);
	
}else{
	imagecopyresampled($thumb,$i,0,0,0,0,$nw,$nh,$w,$h); 	
	imagejpeg($thumb,$thname,95); 
	imagedestroy($thumb);
}


}
//==========================================================================


if ($process_medium == "YES")
{
//CREATE LARGE IMAGE AND SEND TO IMAGES FOLDER
// The file
$filename = "../assets/images/products/originals/".$_FILES['imagefile']['name'];

// Create Image from MIME Type

$mimetype = $_FILES['imagefile']['type'];
switch($mimetype) {
case "image/jpg":
case "image/jpeg":
case "image/pjpeg":
$i = imagecreatefromjpeg($filename);
break;
case "image/gif":
$i = imagecreatefromgif($filename);
break;
case "image/png":
$i = imagecreatefrompng($filename);
break;
}

//VARIABLES

$nw=240; //The Width Of The Thumbnails
$nh=240; //The Height Of The Thumbnails

$tpath = "../assets/images/products/medium";

$dimensions = getimagesize($filename);

$thname = "$tpath/".$_FILES['imagefile']['name'];

$w=$dimensions[0];
$h=$dimensions[1];

$thumb = imagecreatetruecolor($nw,$nh);
	
$wm = $w/$nw;
$hm = $h/$nh;
	
$h_height = $nh/2;
$w_height = $nw/2;
	
//if($w > $h){
//	
//	$adjusted_width = $w / $hm;
//	$half_width = $adjusted_width / 2;
//	$int_width = $half_width - $w_height;
//	
//	imagecopyresampled($thumb,$i,-$int_width,0,0,0,$adjusted_width,$nh,$w,$h); 
//	imagejpeg($thumb,$thname,95); 
//	imagedestroy($thumb); 
	
//}elseif(($w < $h) || ($w == $h)){
	
	$adjusted_height = $h / $wm;
	$half_height = $adjusted_height / 2;
	$int_height = $half_height - $h_height;
	
	imagecopyresampled($thumb,$i,0,-$int_height,0,0,$nw,$adjusted_height,$w,$h); 
	imagejpeg($thumb,$thname,95); 
	imagedestroy($thumb);
}	

//==========================================================================
//==========================================================================
if ($process_category == "YES")
{
		// CREATE THUMBNAIL AND SEND TO THUMBS FOLDER
		// The file
		$filename = "../assets/images/products/originals/".$_FILES['imagefile']['name'];

		// Create Image from MIME Type

$mimetype = $_FILES['imagefile']['type'];
switch($mimetype) {
case "image/jpg":
case "image/jpeg":
case "image/pjpeg":
$i = imagecreatefromjpeg($filename);
break;
case "image/gif":
$i = imagecreatefromgif($filename);
break;
case "image/png":
$i = imagecreatefrompng($filename);
break;
}

//VARIABLES

$nw=75; //The Width Of The Thumbnails
$nh=75; //The Height Of The Thumbnails

$tpath = "../assets/images/products/categories";

$dimensions = getimagesize($filename);

$thname = "$tpath/".$_FILES['imagefile']['name'];

$w=$dimensions[0];
$h=$dimensions[1];

$thumb = imagecreatetruecolor($nw,$nh);
	
$wm = $w/$nw;
$hm = $h/$nh;
	
$h_height = $nh/2;
$w_height = $nw/2;
	
if($w > $h){
	
	$adjusted_width = $w / $hm;
	$half_width = $adjusted_width / 2;
	$int_width = $half_width - $w_height;
	
	imagecopyresampled($thumb,$i,-$int_width,0,0,0,$adjusted_width,$nh,$w,$h); 
	imagejpeg($thumb,$thname,95); 
	imagedestroy($thumb);
	
}elseif(($w < $h) || ($w == $h)){
	
	$adjusted_height = $h / $wm;
	$half_height = $adjusted_height / 2;
	$int_height = $half_height - $h_height;
	
	imagecopyresampled($thumb,$i,0,-$int_height,0,0,$nw,$adjusted_height,$w,$h); 
	imagejpeg($thumb,$thname,95); 
	imagedestroy($thumb);
	
}else{
	imagecopyresampled($thumb,$i,0,0,0,0,$nw,$nh,$w,$h); 	
	imagejpeg($thumb,$thname,95); 
	imagedestroy($thumb);
}


}
//==========================================================================	
	
// ADD TO DB


$query = "INSERT INTO $gallery_table (name, size, type, doc_cat, doc_des, doc_name) VALUES ('$fileName', '$fileSize', '$fileType', '$product_code', '$_POST[des]', '$_POST[doc_name]')";

mysql_query($query) or die('Error, query failed');
$success = 1;
		}
		}
		}
	}
	
echo("</div>");


}
?>