use [a022813]
DECLARE @VANNUM VARCHAR(8) --mandatory
DECLARE @CURDATE BIGINT
DECLARE @WARHS BIGINT
DECLARE @ROUTENAME VARCHAR(8)
DECLARE @ROUTE INT

SET @VANNUM = 'sa04fvg'
SET @WARHS = (SELECT ZROD_WARHS
              FROM   ZEMG_VEHICLES
              WHERE  VEHICLENO = @VANNUM)
SET @CURDATE = dbo.DATETOMIN8(GETDATE())
SET @ROUTE = (SELECT ZROD_ROUTEFORVAN.ROUTE
              FROM   ZROD_ROUTEFORVAN
                     INNER JOIN ZROD_ROUTES
                             ON ZROD_ROUTEFORVAN.ROUTE = ZROD_ROUTES.ROUTE
              WHERE  VANNUM = @VANNUM)
SET @ROUTENAME = (SELECT ROUTENAME
                  FROM   ZROD_ROUTEFORVAN
                         INNER JOIN ZROD_ROUTES
                                 ON ZROD_ROUTEFORVAN.ROUTE = ZROD_ROUTES.ROUTE
                  WHERE  VANNUM = @VANNUM)

set @ROUTENAME = (select isnull(@ROUTENAME, 'No Route'))

SELECT '',
	   (Select '', 
		  ( select 'pdadata/maintainance/' FOR XML PATH('object'), type ) ,
		  ( select 'pdadata/stdpricelist/' FOR XML PATH('object'), type ) ,
		  ( select 'pdadata/home/' FOR XML PATH('object'), type ) ,
	      ( select 'pdadata/warehouse/' FOR XML PATH('object'), type ) ,
		  ( select 'pdadata/reasons/' FOR XML PATH('object'), type ) 
	   FOR XML PATH('sync'), type),

       (SELECT '',
               (SELECT dbo.pda_QuestRemarks(SURVEY.QUESTF)
                FOR XML PATH('text'), type),
               (SELECT '',
                       dbo.pda_Survey(SURVEY.QUESTF, 1)
                FOR XML PATH('checks'), type),
               (SELECT '',
                       dbo.pda_Survey(SURVEY.QUESTF, 2)
                FOR XML PATH('cleanliness'), type),
               (SELECT '',
                       dbo.pda_Survey(SURVEY.QUESTF, 3)
                FOR XML PATH('damage'), type),
               (SELECT '',
                       dbo.pda_Survey(SURVEY.QUESTF, 4)
                FOR XML PATH('mileage'), type)
        FROM   QUESTFORM AS SURVEY
        WHERE  QUESTF <> 0
               AND TYPE = 'C'
               AND QUESTFCODE = 'MN'
        FOR XML PATH('maintainance'), type),
       (SELECT '',
               (SELECT familyname,
						(select '', 
						   (SELECT name,
								   des,
								   barcode,
								   part as "id",
								   (SELECT '',
										   (SELECT tquant,
												   price
											FROM   dbo.pda_PartsStdPrice(part.part)
											FOR XML path ('break'), type)
									FOR XML path ('breaks'), type)
							FROM   dbo.pda_FamilyParts(family.family) AS part
							FOR XML path ('part'), type)
                        FOR XML path ('parts'), type)
                FROM   pda_Family() AS family
                FOR XML path ('family'), root('families'), type)
        FOR xml path('stdpricelist'), type),
        
       (SELECT @CURDATE   AS curdate,
               @ROUTENAME AS routenumber,
               @VANNUM    AS vehiclereg,
               (SELECT '',
                       (SELECT ordinal,
                               custnumber,
                               postcode,
                               sonum,
                               showprices,
                               delivered,
                               nodeliveryreason,
                               (SELECT '',
                                       (SELECT ordi,
                                               name,
                                               des,
                                               parttype,
											   cheese,
                                               barcode,
											   unitprice as "price",
                                               lotnumber,
                                               tquant,
                                               cquant,
											   0 as "weight"
                                        FROM   dbo.pda_DeliveryItems(@ROUTE, delivery.ord)
                                        FOR XML PATH('part'), TYPE)
                                FOR XML PATH('parts'), TYPE),
                               (SELECT ZROD_SIGNATURE AS mandatory,
                                       ''             AS image,
                                       ''             AS [print]
                                FROM   CUSTOMERS
                                WHERE  CUST = delivery.cust
                                FOR xml path ('customersignature'), type),
                               (SELECT custnumber,
                                       custname,
                                       contact,
                                       phone,
                                       address,
                                       address2,
                                       address3,
                                       address4,
                                       postcode,
                                       (SELECT '',
                                               (SELECT ivnum,
                                                       ivdate,
                                                       duedate,
                                                       total,
                                                       (SELECT '',
                                                               (SELECT ordi,
                                                                       name,
                                                                       barcode,
                                                                       des,
                                                                       qty,
                                                                       unitprice
                                                                FROM   dbo.pda_InvoiceItems(invoice.iv)
                                                                FOR xml path ('part'), type)
                                                        FOR xml path ('parts'), type)
                                                FROM   dbo.pda_Invoices(delivery.cust) AS invoice
                                                FOR XML path('invoice'), type)
                                        FOR xml path ('invoices'), type),
                                       (SELECT '',
                                               (SELECT '',
                                                       (SELECT 0   AS ivnum,
                                                               0   AS ordi,
                                                               '0' AS name,
                                                               '0' AS des,
                                                               0   AS qty,
                                                               0.0 AS unitprice,
                                                               0   AS rcvdqty,
                                                               '0' AS reason
                                                        FOR XML path ('part'), type)
                                                FOR XML path ('parts'), type)
                                        FOR XML path ('creditnote'), type),
                                       (SELECT '',
                                               (SELECT dbo.NextDeliveryDate(delivery.cust,7)    AS deliverydate,
                                                       0    AS ponum,
                                                       0.00 AS value,
                                                       (SELECT '',
                                                               (SELECT '0' AS name,
                                                                       '0' AS barcode,
                                                                       '0' AS des,
                                                                       0   AS qty,
                                                                       0.0 AS unitprice
                                                                FOR XML path ('part'), type)
                                                        FOR XML path ('parts'), type)
                                                FOR XML path ('order'), type)
                                        FOR XML path ('orders'), type),
                                       (SELECT dbo.pda_CustomerAccInfo(delivery.cust) AS text
                                        FOR xml path ('accountinfo'), type),
                                       (SELECT dbo.pda_CustomerRemarks(delivery.cust) AS text
                                        FOR xml path ('customerremarks'), type),
                                       (SELECT '' AS text
                                        FOR xml path ('addremark'), type),
                                       (SELECT '',
                                               (SELECT '',
                                                       (SELECT name,
                                                               barcode,
                                                               des,
                                                               tquant,
                                                               price
                                                        FROM   dbo.pda_CustomerPriceList(delivery.cust)
                                                        FOR xml path('part'), type)
                                                FOR xml path('parts'), type)
                                        FOR xml path('customerpricelist'), type),
                                        (SELECT '',
											(SELECT family as "@name",
													parts as "@parts"
												FROM dbo.pda_CustomerFamily(delivery.cust)
											FOR xml path('family'), type)
                                        FOR xml path('custpart'), type)
                                FROM   dbo.pda_CustomerDetails(delivery.phone, delivery.cust)
                                FOR xml path ('customer'), type),
                               (SELECT paymentterms,
                                       overduepayment,
                                       dueamount,
                                       todaysinvoicetotal,
                                       cash,
                                       cheque
                                FROM   dbo.pda_Payment(delivery.cust)
                                FOR XML path('payment'), type)
                        FROM   dbo.pda_Deliveries(@ROUTE, @CURDATE) AS delivery
                        FOR XML PATH ('delivery'), TYPE)
                FOR xml path ('deliveries'), type)
        FOR xml path('home'), type),
       (SELECT '',
               (SELECT '',
                       (SELECT PARTNAME AS name,
                               BARCODE  AS barcode,
                               (SELECT '',
                                       (SELECT serial AS name,
                                               qty    AS qty,
                                               expdate AS expirydate
                                        FROM   dbo.pda_PartSerials1(warhsbal.PART, @WARHS)
                                        FOR xml path('lot'), type)
                                FOR xml path('lots'), type)
                        FROM   dbo.pda_WhsParts(@WARHS) AS warhsbal
                        FOR xml path ('part'), type)
                FOR xml path('parts'), type)
        FOR xml path ('warehouse'), type),
       (SELECT '',
               (SELECT '',
                       (SELECT REASON AS reason
                        FROM   ZROD_CREDITREASON
                        WHERE  REASON <> ''
                        FOR xml path(''), type)
                FOR xml path('credit'), type),
               (SELECT '',
                       (SELECT REASON AS reason
                        FROM   ZROD_NODELREASON
                        WHERE  REASON <> ''
                        FOR xml path(''), type)
                FOR xml path('nodelivery'), type)
        FOR xml path('reasons'), type)
FOR XML PATH ('pdadata'), type 
