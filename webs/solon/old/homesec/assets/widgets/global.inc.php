<?php
$host 			= 'mysql.home-security-store.co.uk';
$user 			= 'homesec';
$pass 			= 'h1zzle';
$name 			= 'HOMESEC';
$db = &new MySQL($host,$user,$pass,$name);

$config_table 				=		"SHOP2_config";
$content_table				=		"SHOP2_content";
$archive_table				=		"SHOP2_content_archive";
$category_table				=		"SHOP2_categories";
$footprint_table			=		"SHOP2_footprints";
$order_status_table			=		"SHOP2_order_status";
$orders_table				=		"SHOP2_orders";
$product_table				=		"SHOP2_products";
$report_popular_table		=		"SHOP2_report_popular";
$report_products_table		=		"SHOP2_report_products";
$report_monthly_table		=		"SHOP2_report_monthly";
$stock_table				=		"SHOP2_stock";
$tracker_table				=		"SHOP2_tracker";
$gallery_table				=		"SHOP2_gallery";
$admin_track_table			=		"SHOP2_admin_tracker";
$admin_table				=		"SHOP2_admin_user";
$search_table				=		"SHOP2_search";

$POSTAGE_ONE_PRICE=4.95;
$POSTAGE_TWO_PRICE=9.95;

$POSTAGE_ONE_CODE="F00003";
$POSTAGE_TWO_CODE="F00002";
?>