function confirmDelete()
{
var agree=confirm("Confirm you want to delete this item");
if (agree)
	return true ;
else
	return false ;
}
function confirmRollback()
{
var agree=confirm("Confirm you want to rollback this page");
if (agree)
	return true ;
else
	return false ;
}

// Expand and Collapse
function expandCollapse() {
for (var i=0; i<expandCollapse.arguments.length; i++) {
var element = document.getElementById(expandCollapse.arguments[i]);
element.style.display = (element.style.display == "none") ? "" : "none";
	}
}