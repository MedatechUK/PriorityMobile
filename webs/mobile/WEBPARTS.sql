select 
	(SELECT '',
		(SELECT      WEBCUR.CODE as "@CODE" , dbo.PART.PARTNAME AS "@DEFDEL"
			FROM         dbo.ZWEB_CURRENCIES AS WEBCUR INNER JOIN
			dbo.PART ON WEBCUR.DEFDEL = dbo.PART.PART
			WHERE     (WEBCUR.CODE <> '')
			for XML PATH('CODE'), TYPE)
		for XML PATH('CURRENCY'), TYPE)
			,		
		(select
		
			(select dbo.WEBISDELPART(WEBPART.PART) AS "@DELIVERY",
				PARTNAME ,
				PARTDES , 				
				dbo.WEBDELFAMILY(WEBPART.PART) AS PACKFAMILY,				
				dbo.AVAILDESC(dbo.WEBPARTAV(WEBPART.PART)) as AVAILABLE,	
				WEBPARTTEXT as PARTREMARK,
				WEBPART.BARCODE AS BARCODE,
				(SELECT EXTFILENAME FROM PARTEXTFILE WHERE EXTFILENUM =-1 AND PART = WEBPART.PART) as PRIIMG,
					(Select '',
						(select 'SPEC' + LTRIM(RTRIM(STR(NUMBER))) AS [@NAME], 
						SPECDES AS [@DES], 
						dbo.WEBPARTSPEC(WEBPART.PART,NUMBER) AS [@VALUE]
								
						from PARTSPECTYPES
						WHERE SPECDES <> '' 
						for XML PATH('SPEC'),type)
								
					where dbo.WEBISDELPART(WEBPART.PART) is null
					for XML PATH('SPECS'),type)
					,
			
			-- Standard Pricing		
			(select 1 as [@DEFAULT], 
				(SELECT CODE as [@CURSTR], 	
				dbo.WEBTAX(WEBPART.PART,WEBCUR.CODE,null) as [@TAXRATE],
					(select PRICE as [@PRICE],
					QTY AS [@QTY]
					from dbo.v_WEBPRICE
						where  PARTNAME = WEBPART.PARTNAME
						and CODE = WEBCUR.CODE 				
						for XML PATH('BREAK'),type
						)
					FROM         ZWEB_CURRENCIES as WEBCUR
					WHERE CODE <> ''
					for XML PATH('CURRENCY'),type
				)
				where dbo.WEBISDELPART(WEBPART.PART) is null
				for XML PATH('PRICE'),type),
			
			-- Delivery Pricing	
			(select '', 
				(SELECT CODE as [@CURSTR], 	
				dbo.WEBTAX(WEBPART.PART,WEBCUR.CODE,null) as [@TAXRATE],	
					(select FAMILY AS [@FAMILY],
					PRICE as [@PRICE],
					INCREMENT AS [@INCREMENT]
					from dbo.v_WEBDELFAMILY	
						where CODE = WEBCUR.CODE 
						AND ZWEB_DELPART	= WEBPART.PART			
						for XML PATH('FAMILY'),type
						)
					FROM         ZWEB_CURRENCIES as WEBCUR
					WHERE CODE <> ''
					for XML PATH('CURRENCY'),type
				)
				where dbo.WEBISDELPART(WEBPART.PART) is not null
				for XML PATH('PRICE'),type),
				
				-- Customer Pricing
				(select CUSTNAME as [@CUSTNAME],
					(SELECT CODE as [@CURSTR], 	
					dbo.WEBTAX(WEBPART.PART,WEBCUR.CODE,WEBCUST.CUST) as [@TAXRATE],
						(SELECT MIN(PRICE) as [@PRICE],
							QUANT AS [@QTY]
							 FROM dbo.v_WEBCUSTPART
							WHERE CUST = WEBCUST.CUST
							AND PARTNAME = WEBPART.PARTNAME
							AND CODE = WEBCUR.CODE
							group by PARTNAME,QUANT
							for XML PATH('BREAK'),type
						)
						FROM  ZWEB_CURRENCIES as WEBCUR
						WHERE WEBCUR.CODE IN (
						SELECT CODE
							FROM dbo.v_WEBCUSTPART WHERE
							CUST = WEBCUST.CUST
							AND PARTNAME = WEBPART.PARTNAME
							)		
					for XML PATH('CURRENCY'),type)
					
				FROM dbo.v_WEBCUST AS WEBCUST 
				where dbo.WEBISDELPART(WEBPART.PART) is null 
				for XML PATH('PRICE'),type)		

			FROM dbo.v_WEBPART as WEBPART	
			order by dbo.WEBISDELPART(WEBPART.PART)	DESC, FAMILY, PARTDES
			for XML PATH('PART'),type)
	
		for XML PATH('PARTS'),type)

for XML PATH('BASKET'),type