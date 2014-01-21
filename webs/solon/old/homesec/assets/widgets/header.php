<div id="header">
    <h2><?php echo $site_name; ?></h2>
	<p><a href="<?php echo $site_url ?>index.php" class="homelink" title="Home Page"><img src="<?php echo $site_url ?>assets/images/clear.gif" alt="Link to Home Page" class="header_img"/></a></p>
	
<form method="get" action="/search.php">
      <fieldset>
	  <legend>Enter a keyword here to search the website</legend>
	  <label><span>Search:</span>
      <input type="text" name="search" />
      </label>
	  <span class="submit"><input type="image" name="Submit" src="/assets/images/buttons/search.gif" alt="Search" /></span>
	  </fieldset>
    </form>
  </div>