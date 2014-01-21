<?php
function rA($val){
 	//Remove Ampersand Spaces
	$strVal=str_replace("&","and",$val);
	return $strVal;				
}
/*
function slashes($string)
	{
	$string = stripslashes(stripslashes(stripslashes(stripslashes(stripslashes(stripslashes($string))))));
	return $string;
	}
*/

$TAX_RATE=1200;
$TAX_RATE_DEC=0.2;
$TAX_RATE_STRING="20";

$POSTAGE_ONE_PRICE=4.95;
$POSTAGE_TWO_PRICE=9.95;

$POSTAGE_ONE_CODE="F00003";
$POSTAGE_TWO_CODE="F00002";

$cartInfo=$_POST[cartinfo];
$DELIVERTOPARTY=rA(slashes($_POST[delivery_name]));
$DELADDRESSLINE1=rA(slashes($_POST[delivery_address]));
if ($_POST[delivery_address2] != "No Value"){
	$DELADDRESSLINE2=slashes($_POST[delivery_address2]);
}
$DELADDRESSLINE3=slashes($_POST[delivery_town]);
$DELADDRESSLINE4=slashes($_POST[delivery_county]);
$DELPOSTCODE=$_POST[delivery_postcode];
$DELCONTACT=rA(slashes($_POST[delivery_name]));
$DELTEL=$_POST[delivery_tel];
$DELEMAIL=$_POST[delivery_email];

//INVOICE INFO

$INVOICEPARTY=rA(slashes($_POST[delivery_name]));
$INVADDRESSLINE1=rA($xml_order->billing['street_address']);
if ($_POST[delivery_address2] != "No Value"){
	$INVADDRESSLINE2=slashes($_POST[delivery_address2]);
}

$INVADDRESSLINE3=slashes($_POST[delivery_town]);
$INVADDRESSLINE4=slashes($_POST[delivery_county]);
$INVPOSTCODE=$_POST[delivery_postcode];
$INVCONTACT=rA(slashes($_POST[delivery_name]));
$INVTEL=$_POST[delivery_tel];
$INVEMAIL=$_POST[delivery_email];

$POSTAGE = $_POST[deliverymethod];
$xml_orderID= $_POST[orderref];

$slashPos=strpos($xml_orderID,"/");
if(!($slashPos===false)){
	$doArr=explode("/",$xml_orderID);
	$xml_orderID=$doArr[0]."-".$doArr[1];
}

//Get a cart for testing
//---------------------------------------------------------------------------

$host 		= 'mysql.personal-attack-alarms.net';
$user 		= 'paalarms';
$pass 		= '477ack';
$database	= 'PAALARMS';
//---------------------------------------------------------------------------

mysql_connect($host, $user, $pass);
mysql_select_db($database);
/*
$selSql="Select * from SHOP1_orders WHERE id='57'";
$selQry=mysql_query($selSql);
while($selRes=mysql_fetch_array($selQry)){
	$cartInfo=$selRes['cartinfo'];
}
*/
//Deal with items from the order

//Check for comma
$commaPos=strpos($cartInfo,",");
if(!($commaPos===false)){
	//More than one item
	$cartArr=explode(",",$cartInfo);
}else{
	//Just one item
	$cartArr[0]=$cartInfo;
}
//Now have an array with all cartinfo in it
//Create a new array which holds the product and the number of times it has been ordered
$counter=0;
for($i=0;$i<count($cartArr);$i++){
	//echo($cartArr[$i]."<br>");
	if($oldVal!=$cartArr[$i]){
		//Check to see if it already exists
		for($l=0;$l<count($newArr);$l++){
			if($newArr[$l]['id']==$cartArr[$i]){
				$newArr[$l]['val']=($newArr[$l]['val'])+1;
				$wasFound="YES";
			}
		}
		if($wasFound!="YES"){	
			$newArr[$counter]['id']=$cartArr[$i];
			$newArr[$counter]['val']=1;
			$counter++;
		}
	}else{
		for($j=0;$j<count($newArr);$j++){
			if($newArr[$j]['id']==$cartArr[$i]){
				$newArr[$j]['val']=($newArr[$j]['val'])+1;
			}
		}
	}
	$oldVal=$cartArr[$i];
}	
/*Check new Array*/

/*for($k=0;$k<count($newArr);$k++){
	echo($newArr[$k]['id']." = ".$newArr[$k]['val']."<br>");
}
*/
//$newArr now holds the ID of the product and the number of times it appears in the DB
//Loop through the result and pull back data about each of the products

for($i=0;$i<count($newArr);$i++){
	$itemsArr[$i]['qty']=$newArr[$i]['val'];
	$selSql="";
	$selSql="SELECT * FROM SHOP1_products WHERE id='".$newArr[$i]['id']."'";
	//echo($selSql);
	$selQry=mysql_query($selSql);
	while($selRes=mysql_fetch_array($selQry)){
		$itemsArr[$i]['id']=$selRes['id'];
		$itemsArr[$i]['name']=$selRes['name'];
		$itemsArr[$i]['summary']=$selRes['summary'];
		$itemsArr[$i]['code']=$selRes['code'];
		//Make a tax line as these appear to be inclusive prices
				
		$itemPrice=$selRes['price'];
		
		//$taxThisItem=($itemPrice*1000)/1175;
		$itemPricePreTax=($itemPrice*1000)/$TAX_RATE;
		$itemsArr[$i]['tax']=$taxThisItem;
		$itemsArr[$i]['price']=$itemPricePreTax;
		//$itemsArr[$i]['price']=$selRes['price'];
	}
}



$noOfProducts=count($itemsArr);
$xml_orderDATETIME=date("Y-m-d");
$xml_orderDATEARR=explode("-",$xml_orderDATETIME);

for($i=0;$i<$noOfProducts;$i++){

	$xml_orderLINES.="<OrderLine Action=\"Add\" TypeCode=\"GDS\" TypeDescription=\"Goods &amp; Services\">
  	<LineNumber Preserve=\"true\">".($i+1)."</LineNumber>";

	$xml_orderLINES.= "<Product>
      <SuppliersProductCode>".$itemsArr[$i]['id']."</SuppliersProductCode>
      <BuyersProductCode>".rA($itemsArr[$i]['code'])."</BuyersProductCode>
      <Description>".rA($itemsArr[$i]['name'])."</Description></Product>";

	$xml_orderLINES.="<Quantity UOMCode=\"Pieces\" UOMDescription=\"PCE\">
      <Amount>".$itemsArr[$i]['qty']."</Amount>
    </Quantity>
    <Price UOMCode=\"PCE\" UOMDescription=\"Piece\">
      <Units>".$itemsArr[$i]['qty']."</Units>
      <UnitPrice>".round($itemsArr[$i]['price'],2)."</UnitPrice>
     </Price>";
	$TAXFORTHISROW=round(($itemsArr[$i]['qty']*$itemsArr[$i]['price']*$TAX_RATE_DEC),2);
	$PRODUCTCOSTTHISLINE=round(($itemsArr[$i]['qty']*$itemsArr[$i]['price']),2);
	$xml_orderLINES.="<LineTax>
      <MixedRateIndicator>1</MixedRateIndicator>
      <TaxRate Code=\"S\">".$TAX_RATE_STRING."</TaxRate>
      <TaxValue>".$TAXFORTHISROW."</TaxValue>
      <TaxRef Code=\"UKC\" Codelist=\"\">UK Chargeable</TaxRef>
    </LineTax>
    <LineTotal>".round($PRODUCTCOSTTHISLINE,2)."</LineTotal>
  </OrderLine>";
	$OVERALLPRODUCTCOST=$OVERALLPRODUCTCOST+$PRODUCTCOSTTHISLINE;
	$OVERALLTAXCOST=$OVERALLTAXCOST+$TAXFORTHISROW;
	$CHECKSUM_SUM_LINE=($i+1)*$itemsArr[$i]['qty'];
	$CHECKSUM_SUM=$CHECKSUM_SUM+$CHECKSUM_SUM_LINE;
}

switch($POSTAGE){
      case $POSTAGE_ONE_PRICE:
      		$POSTAGE=$POSTAGE_ONE_PRICE;
      		$POSTPRODCODE=$POSTAGE_ONE_CODE;
			$PostageDescription="Postage and Packing";
      break;
      case $POSTAGE_TWO_PRICE:
      		$POSTAGE=$POSTAGE_TWO_PRICE;
     		$POSTPRODCODE=$POSTAGE_TWO_CODE;
			$PostageDescription="Special Delivery";
      break;
      default:
      		$POSTAGE=$POSTAGE;
      break;
}

//$GOODSTOTAL=round($OVERALLPRODUCTCOST,2);
//Calculate postage without tax

$POSTAGEPRETAX=($POSTAGE*1000)/$TAX_RATE;
$POSTAGETAX=$POSTAGE-$POSTAGEPRETAX;
$noOfProducts++;
//This gives us the amount for the Postage amount

$xml_orderLINES.="<OrderLine Action=\"Add\" TypeCode=\"GDS\" TypeDescription=\"Goods &amp; Services\">
    <LineNumber Preserve=\"true\">".($noOfProducts)."</LineNumber>";

  $xml_orderLINES.= "<Product>
      <SuppliersProductCode>".$POSTPRODCODE."</SuppliersProductCode>
      <BuyersProductCode>".$POSTPRODCODE."</BuyersProductCode>
      <Description>$PostageDescription</Description>
    </Product>";

  $xml_orderLINES.="<Quantity UOMCode=\"Pieces\" UOMDescription=\"PCE\">
      <Amount>1</Amount>
    </Quantity>
    <Price UOMCode=\"PCE\" UOMDescription=\"Piece\">
      <Units>1</Units>
      <UnitPrice>".round($POSTAGEPRETAX,2)."</UnitPrice>
     </Price>";
  $TAXFORTHISROW=round($POSTAGETAX,2);
  $PRODUCTCOSTTHISLINE=round($POSTAGEPRETAX,2);
  $xml_orderLINES.="<LineTax>
      <MixedRateIndicator>1</MixedRateIndicator>
      <TaxRate Code=\"S\">".$TAX_RATE_STRING."</TaxRate>
      <TaxValue>".round($POSTAGETAX,2)."</TaxValue>
      <TaxRef Code=\"UKC\" Codelist=\"\">UK Chargeable</TaxRef>
    </LineTax>
    <LineTotal>".round($POSTAGEPRETAX,2)."</LineTotal>
  </OrderLine>";
  $OVERALLPRODUCTCOST=$OVERALLPRODUCTCOST+$PRODUCTCOSTTHISLINE;
  $OVERALLTAXCOST=$OVERALLTAXCOST+$TAXFORTHISROW;
  $CHECKSUM_SUM_LINE=($noOfProducts)*1;
  $CHECKSUM_SUM=$CHECKSUM_SUM+$CHECKSUM_SUM_LINE;

$GOODSTOTAL=$OVERALLPRODUCTCOST;
$TOTALTAX=round($OVERALLTAXCOST,2);
$GROSSVALUE=round(($GOODSTOTAL+$TOTALTAX),2);
$xml_orderTOTAL= "<OrderTotal>  
    <GoodsValue>".$GOODSTOTAL."</GoodsValue>
    <FreightCharges>".$POSTAGE."</FreightCharges>
    <TaxTotal>$TOTALTAX</TaxTotal>
    <GrossValue>$GROSSVALUE</GrossValue>
  </OrderTotal>";

$CHECKSUM=($xml_orderDATEARR[0]+13*$xml_orderDATEARR[2]-7*($CHECKSUM_SUM) )*($xml_orderDATEARR[1]+17*$noOfProducts+5*$GOODSTOTAL);
$CHECKSUM=floor($CHECKSUM);
$CHECKSUM_LEN=strlen($CHECKSUM);
$CHECKSUM_START=$CHECKSUM_LEN-5;
$CHECKSUM=substr($CHECKSUM,$CHECKSUM_START,5);
$filedata="<?xml version=\"1.0\"?>
<Order xmlns=\"urn:schemas-basda-org:2000:purchaseOrder:xdr:3.01\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:schemaLocation=\"urn:schemas-basda-org:2000:purchaseOrder:xdr:3.01 http://www.ebis-xml.net/schemas/order-v3.xsd\">
  <OrderHead>
    <Schema>
      <Version>3</Version>
    </Schema>
    <Stylesheet>
      <StylesheetOwner>BASDA</StylesheetOwner>
      <StylesheetName>eBIS-XML-sylesheet.xsl</StylesheetName>
      <Version>3</Version>
      <StylesheetType>XSL</StylesheetType>
    </Stylesheet>
    <Parameters>
      <Language>en-GB</Language>
      <DecimalSeparator>.</DecimalSeparator>
      <Precision>20.3</Precision>
    </Parameters>
    <OriginatingSoftware>
      <SoftwareManufacturer>BASDA</SoftwareManufacturer>
      <SoftwareProduct>eBIS-XML</SoftwareProduct>
      <SoftwareVersion>3.0</SoftwareVersion>
    </OriginatingSoftware>
    <TestFlag TestReference=\"Complex\">1
      <Test ExpectedReply=\"Fail\" Mode=\"Manual\"/>
    </TestFlag>
    <Order_Function Code=\"TEO\"/>
    <OrderType Code=\"WEO\">Web Order</OrderType>
    <OrderCurrency>
      <Currency Code=\"GBP\">GB Pounds Sterling</Currency>
    </OrderCurrency>
    <InvoiceCurrency>
      <Currency Code=\"GBP\">GB Pounds Sterling</Currency>
    </InvoiceCurrency>
    <Checksum>$CHECKSUM</Checksum>
  </OrderHead>
  <OrderReferences>
		<BuyersOrderNumber Preserve=\"true\">$xml_orderID</BuyersOrderNumber>
    <ProjectCode Preserve=\"true\">EB5</ProjectCode>
    <ProjectAnalysisCode Preserve=\"true\">Web</ProjectAnalysisCode>
    <SuppliersOrderReference>$xml_orderID</SuppliersOrderReference>
  </OrderReferences>
  <OrderDate>$xml_orderDATETIME</OrderDate>
   <Buyer>
    <BuyerReferences>
      <SuppliersCodeForBuyer>101378</SuppliersCodeForBuyer>
    </BuyerReferences>
    <Party>Big Red Elephant Group</Party>
    <Address>
      <AddressLine>Unit 40</AddressLine>
      <Street>Manor Industrial Estate</Street>
      <City>Flint</City>
      <State></State>
      <PostCode>CH6 5UY</PostCode>
      <Country Code=\"United Kingdom\">United Kingdom</Country>
    </Address>
    <Contact>
      <Name>John Fearnall</Name>
      <Switchboard></Switchboard>
      <Fax></Fax>
      <Email>john.fearnall@solonsecurity.co.uk</Email>
      </Contact>
  </Buyer>
  <Delivery>
    <DeliverTo>
      <DeliverToReferences>
      </DeliverToReferences>
      <Party>$DELIVERTOPARTY</Party>
      <Address>
      <AddressLine>$DELADDRESSLINE1</AddressLine>
      <Street> $DELADDRESSLINE2</Street>
      <City>$DELADDRESSLINE3</City>
      <State>$DELADDRESSLINE4</State>
      <PostCode>$DELPOSTCODE</PostCode>
      <Country Code=\"United Kingdom\">United Kingdom</Country>
    </Address>
    <Contact>
      <Name>$DELCONTACT</Name>
      <Switchboard>$DELTEL</Switchboard>
      <Fax>$DELFAX</Fax>
      <Email>$DELEMAIL</Email>
      </Contact>
    </DeliverTo>
    </Delivery>
  <InvoiceTo>
    <InvoiceToReferences>
      <GLN>$INVOICENUMBER</GLN>
    </InvoiceToReferences>
    <Party>$INVOICEPARTY</Party>
          <Address>
          <AddressLine>$INVADDRESSLINE1</AddressLine>
          <Street>$INVADDRESSLINE2</Street>
          <City>$INVADDRESSLINE3</City>
          <State>$INVADDRESSLINE4</State>
          <PostCode>$INVPOSTCODE</PostCode>
          <Country Code=\"United Kingdom\">United Kingdom</Country>
    </Address>
<Contact>
      <Name>$INVCONTACT</Name>
      <Switchboard>$INVTEL</Switchboard>
      <Fax>$INVFAX</Fax>
      <Email>$INVEMAIL</Email>
      </Contact>
  </InvoiceTo>";
$filedata.=$xml_orderLINES;
$filedata.=$xml_orderTOTAL;
$filedata.="</Order>";
//echo($filedata);

$filenameXML=$xml_orderID."_".$xml_orderDATETIME."_".$CHECKSUM.".xml";
$myDir="../xml_order/";
if(file_exists($myDir.$filenameXML)){
	//SHOULD NOT EXIST YET, ATTACH IT IF IT DOES
	$fp = fopen($myDir.$filenameXML, "r");
	$data = fread($fp, filesize($myDir.$filenameXML));
	fclose($fp);
}else{
	//CREATE THE FILE
	$fp = fopen($myDir.$filenameXML, "w");
	fwrite($fp, $filedata);
	$data = $filedata;
	fclose($fp);
}
require("mime_mail.inc");
require("smtp_mail.inc");
$content_type="application/xml";
$smtp_server = "knight.net-work.net";
$mail = new mime_mail;

//Pete : uncomment the two lines below to use the new email addresses and 
//comment out the two lines below it

$mail->from = "paa@redlinesecurity.co.uk";
$mail->to = "sage@redlinesecurity.co.uk";
//$mail->to = "peter@theitc.co.uk,mark.fleming@solonsecurity.co.uk";
//$mail->from = "shop@redlinesecurity.co.uk";
//$mail->to = "peter@theitc.co.uk";
$mail->subject =  "Personal Attack Alarms Order ". $filenameXML;
$bodyText="Personal Attack Alarms Order";
$mail->body=$bodyText;
$mail->add_attachment($data, $myDir.$filenameXML, $content_type);
$data = $mail->get_mail();
$smtp = new smtp_mail;
$smtp->send_email($smtp_server, $mail->from, $mail->to, $data);

?>
