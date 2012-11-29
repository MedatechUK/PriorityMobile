SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[v_USERS]'))
EXEC dbo.sp_executesql @statement = N'
CREATE VIEW [dbo].[v_USERS]
AS
SELECT     system.dbo.USERS.USERLOGIN AS USERLOGIN, system.dbo.USERS.USERNAME, ''123'' AS PASSWORD, system.dbo.USERS.USERID, 
                      ''MAIN'' AS WARHSNAME
FROM         system.dbo.USERSB INNER JOIN
                      system.dbo.USERS ON system.dbo.USERSB.USERID = system.dbo.USERS.USERID
WHERE     (system.dbo.USERSB.USERID > 0) AND (system.dbo.USERSB.INACTIVE <> ''Y'')
' 
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "USERS"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 114
               Right = 189
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'VIEW', @level1name=N'v_USERS'

GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'VIEW', @level1name=N'v_USERS'

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PARTALIAS]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'
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
	where BARCODE <> '''' 

	INSERT INTO @SVCCALL_PARTS
	SELECT PART, PARTNAME, PARTNAME
	FROM PART
	where BARCODE <> ''''
	RETURN
END
' 
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[v_RequestedItems]'))
EXEC dbo.sp_executesql @statement = N'CREATE VIEW [dbo].[v_RequestedItems]
AS
SELECT     dbo.DOCUMENTS.DOCNO, dbo.PART.PARTNAME, WAREHOUSES_1.WARHSNAME AS FROMWHS, WAREHOUSES_1.LOCNAME AS FROMLOC, 
                      dbo.WAREHOUSES.WARHSNAME AS TOWHS, dbo.WAREHOUSES.LOCNAME AS TOLOC, dbo.CUSTOMERS.CUSTNAME AS STATUS, 
                      dbo.REALQUANT(dbo.TRANSORDER.CQUANT) AS QUANT, dbo.TRANSORDER.TRANS
FROM         dbo.DOCUMENTS INNER JOIN
                      dbo.TRANSORDER ON dbo.DOCUMENTS.DOC = dbo.TRANSORDER.DOC INNER JOIN
                      dbo.PART ON dbo.TRANSORDER.PART = dbo.PART.PART INNER JOIN
                      dbo.WAREHOUSES AS WAREHOUSES_1 ON dbo.TRANSORDER.WARHS = WAREHOUSES_1.WARHS INNER JOIN
                      dbo.WAREHOUSES ON dbo.TRANSORDER.TOWARHS = dbo.WAREHOUSES.WARHS INNER JOIN
                      dbo.CUSTOMERS ON dbo.TRANSORDER.CUST = dbo.CUSTOMERS.CUST INNER JOIN
                      dbo.DOCUMENTSA ON dbo.DOCUMENTS.DOC = dbo.DOCUMENTSA.DOC INNER JOIN
                      dbo.DOCSTATS ON dbo.DOCUMENTSA.ASSEMBLYSTATUS = dbo.DOCSTATS.DOCSTAT
WHERE     (dbo.DOCUMENTS.TYPE = ''T'') AND (dbo.DOCSTATS.STATDES = ''Request'') AND (dbo.TRANSORDER.TYPE = ''T'')
' 
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[25] 4[19] 2[24] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1[45] 2[31] 3) )"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4[30] 2[40] 3) )"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 2
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "DOCUMENTS"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 114
               Right = 195
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "TRANSORDER"
            Begin Extent = 
               Top = 7
               Left = 227
               Bottom = 115
               Right = 402
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "PART"
            Begin Extent = 
               Top = 6
               Left = 440
               Bottom = 114
               Right = 611
            End
            DisplayFlags = 280
            TopColumn = 1
         End
         Begin Table = "WAREHOUSES_1"
            Begin Extent = 
               Top = 124
               Left = 0
               Bottom = 232
               Right = 205
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "WAREHOUSES"
            Begin Extent = 
               Top = 165
               Left = 380
               Bottom = 273
               Right = 585
            End
            DisplayFlags = 280
            TopColumn = 7
         End
         Begin Table = "CUSTOMERS"
            Begin Extent = 
               Top = 1
               Left = 624
               Bottom = 109
               Right = 795
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "DOCUMENTSA"
            Begin Extent = 
               Top = 114
               Left = 533
               Bottom = 222
               Right = 726
            End
            DisplayFla' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'VIEW', @level1name=N'v_RequestedItems'

GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'gs = 280
            TopColumn = 4
         End
         Begin Table = "DOCSTATS"
            Begin Extent = 
               Top = 228
               Left = 38
               Bottom = 336
               Right = 209
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 10
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      PaneHidden = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'VIEW', @level1name=N'v_RequestedItems'

GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'VIEW', @level1name=N'v_RequestedItems'

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ZSFDC_PARTLOOKUP]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'-- =============================================
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

	set @PART = (Select PART from PART where PARTNAME = ''11080'')
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
' 
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[POPARTDES]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[POPARTDES] 
(
	-- Add the parameters for the function here
	@ORDI INT
)
RETURNS VARCHAR(255)
AS
BEGIN
	-- Declare the return variable here
	DECLARE @RESULT VARCHAR(255)	
	DECLARE @STD VARCHAR(255)
	DECLARE @NST VARCHAR(255)

	SET @NST = (
		SELECT     dbo.NONSTANDARD.TEXT
		FROM         dbo.PORDERITEMS INNER JOIN
							  dbo.NONSTANDARD ON dbo.PORDERITEMS.NONSTANDARD = dbo.NONSTANDARD.NONSTANDARD INNER JOIN
							  dbo.PART ON dbo.PORDERITEMS.PART = dbo.PART.PART
		WHERE     (dbo.PORDERITEMS.ORDI = @ORDI)
	)

	SET @STD = (
		SELECT     dbo.PART.PARTDES
		FROM         dbo.PORDERITEMS INNER JOIN
							  dbo.NONSTANDARD ON dbo.PORDERITEMS.NONSTANDARD = dbo.NONSTANDARD.NONSTANDARD INNER JOIN
							  dbo.PART ON dbo.PORDERITEMS.PART = dbo.PART.PART
		WHERE     (dbo.PORDERITEMS.ORDI = @ORDI)
	)

	IF LEN(LTRIM(RTRIM(@NST))) = 0 
		BEGIN
			SET @RESULT = @STD
		END
	ELSE
		BEGIN
			SET @RESULT = @NST
		END

	-- Return the result of the function
	RETURN @RESULT

END
' 
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ZSFDCFunc_CYCLEPART]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[ZSFDCFunc_CYCLEPART]
(
	-- Add the parameters for the function here
	@ABC CHAR(1)
)
RETURNS VARCHAR(255)
AS
BEGIN

	declare @Today int
	DECLARE @Count int

	set @Count = (SELECT DAYCOUNT 
	FROM ZSFDC_ABC
	where ABC = @ABC)

	declare @min int
	declare @part int
	DECLARE @result varchar(255)

	set @Today = (select count(*) 
	from PARTPARAM
	WHERE dbo.DATETOMIN(getdate()) = ZSFDC_LASTCOUNT
	and ABC = @ABC)

	if @Today < @Count
		begin

			set @min = (select min(ZSFDC_LASTCOUNT) from PARTPARAM 
			where ABC = @ABC) 

			set @part = (select top 1 PART from PARTPARAM 
			where ABC = @ABC
			and ZSFDC_LASTCOUNT = @min)

			set @result = (select PARTNAME from PART where PART = @part)
		end
	else
		begin
			set @result = ''''
		end

	return @result

END

' 
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SUPPARTNAME]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[SUPPARTNAME] 
(
	-- Add the parameters for the function here
	@PART INT,
	@SUP INT
)
RETURNS VARCHAR(30)
AS
BEGIN
	-- Declare the return variable here
	DECLARE @ResultVar VARCHAR(30)

	-- Add the T-SQL statements to compute the return value here
	SET @ResultVar = (SELECT ISNULL(SUPPARTNAME,'''') FROM SUPPART WHERE SUP =@SUP AND PART = @PART)

	-- Return the result of the function
	RETURN ISNULL(@ResultVar,'''')

END
' 
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[v_RequestedTX]'))
EXEC dbo.sp_executesql @statement = N'CREATE VIEW [dbo].[v_RequestedTX]
AS
SELECT     TOP (100) PERCENT dbo.DOCUMENTS.DOCNO, dbo.WAREHOUSES.WARHSNAME AS WHSFROM, dbo.WAREHOUSES.LOCNAME AS LOCFROM, 
                      WAREHOUSES_1.WARHSNAME AS WHSTO, WAREHOUSES_1.LOCNAME AS LOCTO, CONVERT(varchar(10), 
                      dbo.MINTODATE(dbo.DOCUMENTS.CURDATE), 103) AS DATE
FROM         dbo.DOCSTATS INNER JOIN
                      dbo.DOCUMENTSA ON dbo.DOCSTATS.DOCSTAT = dbo.DOCUMENTSA.ASSEMBLYSTATUS INNER JOIN
                      dbo.DOCUMENTS ON dbo.DOCUMENTSA.DOC = dbo.DOCUMENTS.DOC INNER JOIN
                      dbo.WAREHOUSES ON dbo.DOCUMENTS.WARHS = dbo.WAREHOUSES.WARHS INNER JOIN
                      dbo.WAREHOUSES AS WAREHOUSES_1 ON dbo.DOCUMENTS.TOWARHS = WAREHOUSES_1.WARHS INNER JOIN
                      dbo.TRANSORDER ON dbo.DOCUMENTS.DOC = dbo.TRANSORDER.DOC
WHERE     (dbo.DOCSTATS.TYPE = ''T'') AND (dbo.DOCSTATS.STATDES = ''Request'') AND (dbo.DOCUMENTS.DOCNO NOT IN
                          (SELECT     DOCNO
                            FROM          dbo.ZATG_DOTX_LOAD
                            WHERE      (RECORDTYPE = ''1'') AND (LOADED <> ''Y''))) AND (dbo.TRANSORDER.CQUANT > 0)
GROUP BY dbo.DOCUMENTS.DOCNO, dbo.WAREHOUSES.WARHSNAME, dbo.WAREHOUSES.LOCNAME, WAREHOUSES_1.WARHSNAME, 
                      WAREHOUSES_1.LOCNAME, CONVERT(varchar(10), dbo.MINTODATE(dbo.DOCUMENTS.CURDATE), 103), dbo.DOCUMENTS.CURDATE
ORDER BY dbo.DOCUMENTS.CURDATE
' 
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1[41] 2[36] 3) )"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 2
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "DOCSTATS"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 114
               Right = 209
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "DOCUMENTSA"
            Begin Extent = 
               Top = 6
               Left = 247
               Bottom = 114
               Right = 440
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "DOCUMENTS"
            Begin Extent = 
               Top = 6
               Left = 478
               Bottom = 114
               Right = 635
            End
            DisplayFlags = 280
            TopColumn = 1
         End
         Begin Table = "WAREHOUSES"
            Begin Extent = 
               Top = 114
               Left = 38
               Bottom = 222
               Right = 243
            End
            DisplayFlags = 280
            TopColumn = 1
         End
         Begin Table = "WAREHOUSES_1"
            Begin Extent = 
               Top = 114
               Left = 281
               Bottom = 222
               Right = 486
            End
            DisplayFlags = 280
            TopColumn = 1
         End
         Begin Table = "TRANSORDER"
            Begin Extent = 
               Top = 140
               Left = 607
               Bottom = 248
               Right = 782
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'VIEW', @level1name=N'v_RequestedTX'

GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N' = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1665
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      PaneHidden = 
      Begin ColumnWidths = 12
         Column = 1440
         Alias = 975
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'VIEW', @level1name=N'v_RequestedTX'

GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'VIEW', @level1name=N'v_RequestedTX'

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[v_RequestedCNT]'))
EXEC dbo.sp_executesql @statement = N'CREATE VIEW [dbo].[v_RequestedCNT]
AS
SELECT     TOP (100) PERCENT CONVERT(varchar, dbo.MINTODATE(dbo.DOCUMENTS.CURDATE), 103) AS CURDATE, dbo.DOCUMENTS.DOCNO, 
                      dbo.WAREHOUSES.LOCNAME, dbo.WAREHOUSES.WARHSNAME
FROM         dbo.DOCUMENTS INNER JOIN
                      dbo.WAREHOUSES ON dbo.DOCUMENTS.TOWARHS = dbo.WAREHOUSES.WARHS
WHERE     (dbo.DOCUMENTS.TYPE = ''C'') AND (dbo.DOCUMENTS.FINAL <> ''Y'') AND (dbo.DOCUMENTS.CANCEL <> ''Y'') AND 
                      (dbo.DOCUMENTS.CURDATE > dbo.DATETOMIN(GETDATE() - 360)) AND (dbo.DOCUMENTS.TOWARHS IN
                          (SELECT     WARHS
                            FROM          dbo.WAREHOUSES AS WAREHOUSES_1
                            WHERE      (WARHSNAME = ''MAIN''))) AND (dbo.DOCUMENTS.DOC IN
                          (SELECT     DOC
                            FROM          dbo.TRANSORDER
                            WHERE      (CQUANT = 0)))
ORDER BY dbo.DOCUMENTS.DOC
' 
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = -96
         Left = 0
      End
      Begin Tables = 
         Begin Table = "DOCUMENTS"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 114
               Right = 229
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "WAREHOUSES"
            Begin Extent = 
               Top = 6
               Left = 267
               Bottom = 114
               Right = 472
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'VIEW', @level1name=N'v_RequestedCNT'

GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'VIEW', @level1name=N'v_RequestedCNT'

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[v_OPENPO]'))
EXEC dbo.sp_executesql @statement = N'CREATE VIEW [dbo].[v_OPENPO]
AS
SELECT     TOP (100) PERCENT dbo.SUPPLIERS.SUPNAME, dbo.SUPPLIERS.SUPDES, dbo.PORDERS.SUPORDNUM, dbo.PORDERS.ORDNAME, 
                      CONVERT(varchar, dbo.MINTODATE(dbo.PORDERS.CURDATE), 103) AS ORDERDATE
FROM         dbo.PORDERS INNER JOIN
                      dbo.SUPPLIERS ON dbo.PORDERS.SUP = dbo.SUPPLIERS.SUP
WHERE     (dbo.PORDERS.CLOSED <> ''Y'') AND (dbo.PORDERS.UFLAG = ''Y'')
ORDER BY dbo.PORDERS.CURDATE DESC
' 
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[27] 4[35] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "PORDERS"
            Begin Extent = 
               Top = 6
               Left = 285
               Bottom = 114
               Right = 450
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "SUPPLIERS"
            Begin Extent = 
               Top = 114
               Left = 38
               Bottom = 222
               Right = 223
            End
            DisplayFlags = 280
            TopColumn = 2
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1770
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'VIEW', @level1name=N'v_OPENPO'

GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'VIEW', @level1name=N'v_OPENPO'

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[NEXTPS]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[NEXTPS] 
(
	-- Add the parameters for the function here
	@USERLOGIN VARCHAR(30)
)
RETURNS VARCHAR(30)
AS
BEGIN

	DECLARE @DOC INT
	DECLARE @DOCNO VARCHAR(30)
	
	SET @DOCNO = (SELECT TOP 1 DOCNO
				FROM DOCUMENTS, DOCUMENTSA
				WHERE DOCUMENTS.DOC = DOCUMENTSA.DOC 
				AND DOCUMENTS.TYPE = ''A'' -- Packing slips
				AND DOCUMENTS.WARHS IN -- From users warehouse
					(SELECT WARHS FROM WAREHOUSES , v_USERS
					WHERE WAREHOUSES.WARHSNAME = v_USERS.WARHSNAME
					AND v_USERS.USERLOGIN = @USERLOGIN)
				-- AND DOCUMENTSA.PICKED = 0 -- Not Picked
				)

	SET @DOC = (SELECT DOC FROM DOCUMENTS 
				WHERE TYPE = ''A''
				AND DOCNO = @DOCNO)

	/*
	-- Set as picked
	UPDATE DOCUMENTSA 
	SET PICKED = 1 
	WHERE DOC = @DOC
	*/

	-- Return the result of the function
	RETURN @DOCNO

END
' 
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GRVITEMPROJCUST]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[GRVITEMPROJCUST] 
(
	-- Add the parameters for the function here
	@IV INT,
	@KLINE INT
)
RETURNS VARCHAR(30)
AS
BEGIN
	-- Declare the return variable here
	DECLARE @ResultVar VARCHAR(30)

	-- Add the T-SQL statements to compute the return value here
	SET @ResultVar = (SELECT CUSTNAME 
						FROM PROJLINK, DOCUMENTS, CUSTOMERS
						WHERE PROJLINK.DOC = DOCUMENTS.DOC
						AND DOCUMENTS.CUST = CUSTOMERS.CUST
						AND PROJLINK.IV = @IV
						AND PROJLINK.KLINE = @KLINE)

	declare @ret varchar(30)
	if len(ltrim(rtrim(@ResultVar))) = 0
		begin
			set @ret = null
		end
	else
		begin 
			set @ret = @ResultVar
		end
	-- Return the result of the function	

	return @ret
END



' 
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[ZSFDC_OPENPO]'))
EXEC dbo.sp_executesql @statement = N'CREATE VIEW [dbo].[ZSFDC_OPENPO]
AS
SELECT     dbo.PORDERS.ORDNAME
FROM         dbo.PORDERS INNER JOIN
                      dbo.PORDSTATS ON dbo.PORDERS.PORDSTAT = dbo.PORDSTATS.PORDSTAT
WHERE     (dbo.PORDSTATS.CLOSED <> ''Y'') AND (dbo.PORDSTATS.APPROVED = ''Y'')
' 
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "PORDERS"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 214
               Right = 203
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "PORDSTATS"
            Begin Extent = 
               Top = 6
               Left = 453
               Bottom = 114
               Right = 623
            End
            DisplayFlags = 280
            TopColumn = 8
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'VIEW', @level1name=N'ZSFDC_OPENPO'

GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'VIEW', @level1name=N'ZSFDC_OPENPO'

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ZSFDC_DEFWARHS]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[ZSFDC_DEFWARHS] 
(
	-- Add the parameters for the function here
	@PART INTEGER,
	@TYPE CHAR(1)
)
RETURNS VARCHAR(50)
AS
BEGIN
	-- Declare the return variable here
	DECLARE @result varchar(50)
	DECLARE @WARHS integer

	-- Add the T-SQL statements to compute the return value here
	SET @WARHS = (
		SELECT WARHS 
		FROM PARTPARAM
		WHERE PART = @PART
	)

	
	-- Return the result of the function
	if @TYPE = ''W''
		BEGIN
			SET @result = (SELECT WARHSNAME FROM WAREHOUSES WHERE WARHS = @WARHS)
		END
	ELSE
		BEGIN
			SET @result = (SELECT LOCNAME FROM WAREHOUSES WHERE WARHS = @WARHS)
		END

	RETURN @result

END
' 
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[ZSFDC_GRVITEMS]'))
EXEC dbo.sp_executesql @statement = N'CREATE VIEW [dbo].[ZSFDC_GRVITEMS]
AS
SELECT     TOP (100) PERCENT dbo.PORDERS.ORDNAME, dbo.PART.PARTNAME, dbo.POPARTDES(dbo.PORDERITEMS.ORDI) AS PARTDES, 
                      dbo.SUPPARTNAME(dbo.PORDERITEMS.PART, dbo.PORDERS.SUP) AS SUPPARTNAME, dbo.DATETOMINFORMAT(dbo.PORDERITEMS.DUEDATE) 
                      AS DUEDATE, dbo.PORDERITEMS.TBALANCE / 1000 AS ORDERED, dbo.PORDERITEMS.TBALANCE / 1000 AS RECEIVED, 
                      ISNULL(dbo.GRVITEMPROJCUST(dbo.PORDERS.ORD, dbo.PORDERITEMS.KLINE), ''Goods'') AS STATUS, dbo.ZSFDC_DEFWARHS(dbo.PART.PART, ''W'') 
                      AS WARHS, dbo.ZSFDC_DEFWARHS(dbo.PART.PART, ''L'') AS LOCNAME, dbo.PORDERITEMS.ORDI
FROM         dbo.PORDERS RIGHT OUTER JOIN
                      dbo.PART INNER JOIN
                      dbo.PORDERITEMS ON dbo.PART.PART = dbo.PORDERITEMS.PART ON dbo.PORDERS.ORD = dbo.PORDERITEMS.ORD
WHERE     (dbo.PORDERS.ORDNAME <> '''') AND (dbo.PORDERITEMS.TBALANCE > 0) AND (dbo.PORDERS.ORDNAME IN
                          (SELECT     ORDNAME
                            FROM          dbo.ZSFDC_OPENPO))
ORDER BY dbo.PORDERS.ORDNAME
' 
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[21] 4[40] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1[50] 2[25] 3) )"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4[30] 2[40] 3) )"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2[35] 3) )"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 5
   End
   Begin DiagramPane = 
      PaneHidden = 
      Begin Origin = 
         Top = 0
         Left = -625
      End
      Begin Tables = 
         Begin Table = "PORDERS"
            Begin Extent = 
               Top = 106
               Left = 509
               Bottom = 214
               Right = 674
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "PART"
            Begin Extent = 
               Top = 2
               Left = 67
               Bottom = 110
               Right = 247
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "PORDERITEMS"
            Begin Extent = 
               Top = 5
               Left = 306
               Bottom = 113
               Right = 481
            End
            DisplayFlags = 280
            TopColumn = 5
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 11
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1785
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      PaneHidden = 
      Begin ColumnWidths = 11
         Column = 2925
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'VIEW', @level1name=N'ZSFDC_GRVITEMS'

GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'VIEW', @level1name=N'ZSFDC_GRVITEMS'

