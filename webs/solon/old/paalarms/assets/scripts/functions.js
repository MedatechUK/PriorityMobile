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