USE [atg]
GO
/****** Object:  UserDefinedFunction [dbo].[PARTALIAS]    Script Date: 01/25/2012 14:43:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[PARTALIAS]()
RETURNS 
@SVCCALL_PARTS TABLE 
(
	-- Add the column definitions for the TABLE variable here
	PART INT,
	BARCODE varCHAR(32), 
	PARTNAME varCHAR(32)
)
AS
BEGIN
	INSERT INTO @SVCCALL_PARTS
	SELECT PART, BARCODE, PARTNAME
	FROM PART
	where BARCODE <> '' 

	INSERT INTO @SVCCALL_PARTS
	SELECT PART, PARTNAME, PARTNAME
	FROM PART
	where BARCODE <> ''
	RETURN
END

USE [atg]
GO
/****** Object:  UserDefinedFunction [dbo].[ZSFDC_PARTLOOKUP]    Script Date: 01/25/2012 16:34:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE FUNCTION [dbo].[ZSFDC_PARTLOOKUP] 
(
	-- Add the parameters for the function here
	@PARTNAME varchar(50)
)
RETURNS 
@Tab TABLE 
(
	-- Add the column definitions for the TABLE variable here
	WARHSNAME varchar(50) ,
	LOCNAME varchar(50) ,
	CUSTNAME varchar(50) ,
	SERIALNAME varchar(50), 
	ACTNAME varchar(50)  ,
	BALANCE int,
	CQUANT int
)
AS
BEGIN
	-- Fill the table variable with the rows for your result set
	declare @PART int
	declare @DEFWARHS int

	set @PART = (Select PART from PART where PARTNAME = @PARTNAME)
	set @DEFWARHS = (select WARHS from PARTPARAM where PART = @PART)

	IF @DEFWARHS <> 0 
	BEGIN
		insert into @Tab
		select WARHSNAME, LOCNAME , CUSTOMERS.CUSTNAME, SERIAL.SERIALNAME, ACT.ACTNAME, dbo.REALQUANT(BALANCE) as BALANCE, dbo.REALQUANT(BALANCE) AS CQUANT 
		from WARHSBAL, WAREHOUSES, CUSTOMERS, SERIAL, ACT 
		where WARHSBAL.ACT = ACT.ACT 
		AND WARHSBAL.WARHS = WAREHOUSES.WARHS 
		and WARHSBAL.CUST = CUSTOMERS.CUST 
		AND SERIAL.SERIAL = WARHSBAL.SERIAL 
		and WARHSBAL.PART =  
		(select PART from PART where PARTNAME = @PARTNAME) 
		and WAREHOUSES.WARHS = @DEFWARHS
		order by BALANCE desc
	END 

	declare @c int
	set @c = (select count(*) from @Tab)	
	if @c = 0 
	begin
		insert into @Tab
		select WARHSNAME, LOCNAME , 'Goods', '0', '', 0, 0
		from WAREHOUSES
		where WARHS = @DEFWARHS
	end

	insert into @Tab
	select WARHSNAME, LOCNAME , CUSTOMERS.CUSTNAME, SERIAL.SERIALNAME, ACT.ACTNAME, dbo.REALQUANT(BALANCE) as BALANCE, dbo.REALQUANT(BALANCE) AS CQUANT 
	from WARHSBAL, WAREHOUSES, CUSTOMERS, SERIAL, ACT 
	where WARHSBAL.ACT = ACT.ACT 
	AND WARHSBAL.WARHS = WAREHOUSES.WARHS 
	and WARHSBAL.CUST = CUSTOMERS.CUST 
	AND SERIAL.SERIAL = WARHSBAL.SERIAL 
	and WARHSBAL.PART =  
	(select PART from PART where PARTNAME = @PARTNAME) 
	and BALANCE <> 0 
	and WAREHOUSES.WARHS <> @DEFWARHS
	order by BALANCE desc
	
	RETURN 
END


