/****** Object:  View [dbo].[V_SVCCALL_WARHS]    Script Date: 11/28/2009 15:13:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[V_SVCCALL_WARHS]
AS
SELECT     PARTNAME, PARTDES, QTY, PRICE, WARHSNAME
FROM         dbo.WARHS_EXT() AS WARHS_EXT_1
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
         Configuration = "(H (1[27] 2[47] 3) )"
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
         Begin Table = "WARHS_EXT_1"
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
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'V_SVCCALL_WARHS'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'V_SVCCALL_WARHS'
GO
/****** Object:  UserDefinedFunction [dbo].[QUESTIONTEXT]    Script Date: 11/28/2009 15:13:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[QUESTIONTEXT]
(
	-- Add the parameters for the function here
	@QUESTF int,
	@QUESTNUM int

)
RETURNS varchar(max)
AS
BEGIN

	declare @question varchar(max)
	declare @text varchar(68)

	set @question = (SELECT QUESTDES 
	FROM QUESTIONS 
	WHERE QUESTNUM = @QUESTNUM AND QUESTF = @QUESTF)

	DECLARE qcur CURSOR FOR 
	select TEXT from QUESTIONSTEXT 
	where QUESTNUM = @QUESTNUM AND QUESTF =@QUESTF
	and TEXTORD > 2
	order by TEXTORD

	OPEN qcur

	FETCH NEXT FROM qcur 
	INTO @text

	WHILE @@FETCH_STATUS = 0
	BEGIN
		
		set @question = (@question + ' ' + rtrim(@text))

			-- Get the next vendor.
		FETCH NEXT FROM qcur 
		INTO @text
	END 
	CLOSE qcur
	DEALLOCATE qcur

	return @question

END
GO
/****** Object:  UserDefinedFunction [dbo].[WARHS_EXT]    Script Date: 11/28/2009 15:13:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[WARHS_EXT] (
) 
RETURNS 
@WARHS_EXT TABLE 
(
	WARHSNAME varchar(10),
	PARTNAME varchar(20),
	PARTDES Varchar(50), 
	QTY REAL,
	PRICE MONEY
)
AS
BEGIN

	-- Fill the table variable with the rows for your result set
	INSERT INTO @WARHS_EXT
	SELECT dbo.WAREHOUSES.WARHSNAME, 
	dbo.PART.PARTNAME, 
	dbo.PART.PARTDES, 
	dbo.REALQUANT(dbo.WARHSBAL.BALANCE) AS QTY, 
	0 /*dbo.PARTPRICE.PRICE*/
	FROM         dbo.WARHSBAL INNER JOIN
	dbo.PART ON dbo.WARHSBAL.PART = dbo.PART.PART INNER JOIN
	dbo.WAREHOUSES ON dbo.WARHSBAL.WARHS = dbo.WAREHOUSES.WARHS /*INNER JOIN
	dbo.PARTPRICE ON dbo.WARHSBAL.PART = dbo.PARTPRICE.PART*/
	WHERE     (dbo.WARHSBAL.WARHS IN
	(SELECT     WARHS
	FROM          dbo.USERSA
	WHERE      (WARHS > 0)))  
	AND (dbo.REALQUANT(dbo.WARHSBAL.BALANCE) > 0)
	/*AND (dbo.PARTPRICE.PLIST = -1)*/
	and (dbo.PART.PARTNAME is not null)

/*AND (dbo.PARTPRICE.PLIST = 15)*/

	INSERT INTO @WARHS_EXT
	SELECT      dbo.WAREHOUSES.WARHSNAME, 
				dbo.PART.PARTNAME, 
				dbo.PART.PARTDES, 
				9999,
				0 /*dbo.PARTPRICE.PRICE*/
	FROM         dbo.WAREHOUSES CROSS JOIN
						  /*dbo.PARTPRICE RIGHT OUTER JOIN*/
						  dbo.PART RIGHT OUTER JOIN
						  dbo.ZPDA_KANBAN_PART  ON dbo.PART.PART = dbo.ZPDA_KANBAN_PART.PART /*ON dbo.PARTPRICE.PART = dbo.ZPDA_KANBAN_PART.PART*/
	WHERE     (dbo.WAREHOUSES.WARHS IN
							  (SELECT     WARHS
								FROM          dbo.USERSA
								WHERE      (WARHS > 0))) AND (dbo.ZPDA_KANBAN_PART.PART > 0) /*AND (dbo.PARTPRICE.PLIST = -1)	*/
	and (dbo.PART.PARTNAME is not null)
/**/

RETURN 
END
GO
/****** Object:  View [dbo].[V_SVCCALL_STATUS]    Script Date: 11/28/2009 15:13:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[V_SVCCALL_STATUS]
AS
SELECT     CODE
FROM         dbo.CALLSTATUSES
WHERE     (CALLSTATUS <> 0) AND (ZPDA_PDASTATUS = 'Y')
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
         Begin Table = "CALLSTATUSES"
            Begin Extent = 
               Top = 6
               Left = 227
               Bottom = 114
               Right = 394
            End
            DisplayFlags = 280
            TopColumn = 9
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
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'V_SVCCALL_STATUS'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'V_SVCCALL_STATUS'
GO
/****** Object:  UserDefinedFunction [dbo].[SVCCALL_DETAILS2]    Script Date: 11/28/2009 15:13:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE FUNCTION [dbo].[SVCCALL_DETAILS2]()
RETURNS 
@SVCCALL_DETAILS2 TABLE 
(
	-- Add the column definitions for the TABLE variable here	 
	DOCNO VARCHAR(24),
	TXT CHAR(68),
	USERLOGIN CHAR(8)
)
AS
BEGIN
	-- Fill the table variable with the rows for your result set
	INSERT INTO  @SVCCALL_DETAILS2 
	SELECT       dbo.DOCUMENTS.DOCNO + '-' + rtrim(ltrim(str(dbo.DOCUMENTSTEXT.TEXTORD))), dbo.DOCUMENTSTEXT.TEXT, system.dbo.USERS.USERLOGIN
	FROM         dbo.CUSTOMERS INNER JOIN
						  dbo.DOCUMENTS INNER JOIN
						  dbo.DOCUMENTSTEXT ON - dbo.DOCUMENTS.DOC = dbo.DOCUMENTSTEXT.DOC INNER JOIN
						  dbo.SERVCALLS INNER JOIN
						  dbo.PHONEBOOK ON dbo.SERVCALLS.PHONE = dbo.PHONEBOOK.PHONE INNER JOIN
						  dbo.CALLSTATUSES ON dbo.SERVCALLS.CALLSTATUS = dbo.CALLSTATUSES.CALLSTATUS ON dbo.DOCUMENTS.DOC = dbo.SERVCALLS.DOC ON 
						  dbo.CUSTOMERS.CUST = dbo.DOCUMENTS.CUST INNER JOIN
						  system.dbo.USERS ON dbo.SERVCALLS.TECHNICIAN = system.dbo.USERS.T$USER
	WHERE     (dbo.DOCUMENTS.TYPE = 'Q') AND (dbo.CALLSTATUSES.ACTIVEFLAG = 'Y') AND (dbo.SERVCALLS.PDATE >= dbo.DATETOMIN(GETDATE() - 1)) AND 
						  (system.dbo.USERS.USERLOGIN NOT LIKE 'UA%') AND (system.dbo.USERS.USERLOGIN <> 'Services') AND (dbo.DOCUMENTSTEXT.TEXTORD > 0)
	ORDER BY dbo.DOCUMENTS.DOCNO, dbo.DOCUMENTSTEXT.TEXTORD

	INSERT INTO  @SVCCALL_DETAILS2 
	SELECT     TOP (100) PERCENT dbo.DOCUMENTS.DOCNO + '-' + rtrim(ltrim(str(dbo.DOCUMENTSTEXT.TEXTORD))), dbo.DOCUMENTSTEXT.TEXT, system.dbo.USERS.USERLOGIN
	FROM         dbo.CUSTOMERS INNER JOIN
						  dbo.DOCUMENTS INNER JOIN
						  dbo.DOCUMENTSTEXT ON - dbo.DOCUMENTS.DOC = dbo.DOCUMENTSTEXT.DOC INNER JOIN
						  dbo.SERVCALLS INNER JOIN
						  dbo.PHONEBOOK ON dbo.SERVCALLS.PHONE = dbo.PHONEBOOK.PHONE INNER JOIN
						  dbo.CALLSTATUSES ON dbo.SERVCALLS.CALLSTATUS = dbo.CALLSTATUSES.CALLSTATUS ON dbo.DOCUMENTS.DOC = dbo.SERVCALLS.DOC ON 
						  dbo.CUSTOMERS.CUST = dbo.DOCUMENTS.CUST INNER JOIN
						  system.dbo.USERS ON dbo.SERVCALLS.TECHNICIAN2 = system.dbo.USERS.T$USER
	WHERE     (dbo.DOCUMENTS.TYPE = 'Q') AND (dbo.CALLSTATUSES.ACTIVEFLAG = 'Y') AND (dbo.SERVCALLS.PDATE >= dbo.DATETOMIN(GETDATE() - 1)) AND 
						  (system.dbo.USERS.USERLOGIN NOT LIKE 'UA%') AND (system.dbo.USERS.USERLOGIN <> '') AND (dbo.DOCUMENTSTEXT.TEXTORD > 0) AND 
						  (system.dbo.USERS.USERLOGIN <> 'Services')
	ORDER BY dbo.DOCUMENTS.DOCNO, dbo.DOCUMENTSTEXT.TEXTORD

	RETURN 
END
GO
/****** Object:  UserDefinedFunction [dbo].[SVCCALL_CODE]    Script Date: 11/28/2009 15:13:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[SVCCALL_CODE]
(
	-- Add the parameters for the function here
	@DOC INT
)
RETURNS VARCHAR(20)
AS
BEGIN
	-- Declare the return variable here
	DECLARE @ResultVar VARCHAR(20)
	DECLARE @ECODE  VARCHAR(20)
	DECLARE @CODE VARCHAR(20)

	SET @ECODE = (SELECT ECODE
				FROM SERVCALLS, CALLSTATUSES
				WHERE SERVCALLS.CALLSTATUS   = CALLSTATUSES.CALLSTATUS
				AND SERVCALLS.DOC = @DOC)
	
	SET @ResultVar = (SELECT CODE
				FROM SERVCALLS, CALLSTATUSES
				WHERE SERVCALLS.CALLSTATUS   = CALLSTATUSES.CALLSTATUS
				AND SERVCALLS.DOC = @DOC)

	/*IF @ECODE <> ''
		begin
			set @ResultVar = @ECODE
		end */

	return @ResultVar
END
GO
/****** Object:  View [dbo].[V_SVCCALL_MALFUNCTION]    Script Date: 11/28/2009 15:13:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[V_SVCCALL_MALFUNCTION]
AS
SELECT     MALFCODE, MALFDES
FROM         dbo.MALFUNCTIONS
WHERE     (MALF > 0)
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
         Begin Table = "MALFUNCTIONS"
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
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'V_SVCCALL_MALFUNCTION'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'V_SVCCALL_MALFUNCTION'
GO
/****** Object:  UserDefinedFunction [dbo].[SVCCALL_PHONE]    Script Date: 11/28/2009 15:13:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[SVCCALL_PHONE]
(
	-- Add the parameters for the function here
	@DOC INT
)
RETURNS VARCHAR(20)
AS
BEGIN
	-- Declare the return variable here
	DECLARE @ResultVar VARCHAR(20)
	DECLARE @DPHONE VARCHAR(20)
	DECLARE @PPHONE VARCHAR(20)

	--(DESTCODES.PHONENUM <> '' ? DESTCODES.PHONENUM : PHONEBOOK.PHONENUM)
	SET @DPHONE = (SELECT DESTCODES.PHONENUM 
				FROM SERVCALLS, DOCUMENTS, DESTCODES 
				WHERE DOCUMENTS.DESTCODE = DESTCODES.DESTCODE
				AND DOCUMENTS.DOC = SERVCALLS.DOC
				AND DOCUMENTS.DOC = @DOC)
	
	SET @PPHONE = (SELECT PHONEBOOK.PHONENUM 
					FROM SERVCALLS,  PHONEBOOK 
					WHERE SERVCALLS.PHONE = PHONEBOOK.PHONE
					AND SERVCALLS.DOC = @DOC)

	SET @ResultVar = ' '

	IF @DPHONE = '' AND @PPHONE  <> ''
		begin
			set @ResultVar = @PPHONE
		end

	IF @PPHONE= '' AND @DPHONE  <> ''
		begin
			set @ResultVar = @DPHONE
		end

	IF @PPHONE <> '' AND @DPHONE  <> ''
		begin
			set @ResultVar = @DPHONE
		end

	return @ResultVar
END
GO
/****** Object:  UserDefinedFunction [dbo].[SVCCALL_STATUS]    Script Date: 11/28/2009 15:13:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[SVCCALL_STATUS]
(
	-- Add the parameters for the function here
	@DOC INT
)
RETURNS VARCHAR(20)
AS
BEGIN
	-- Declare the return variable here
	DECLARE @ResultVar VARCHAR(20)
	DECLARE @WARDATEFINAL INT
	DECLARE @EXPIRYDATE INT

	SET @WARDATEFINAL = (SELECT WARDATEFINAL FROM SERVCALLS WHERE DOC = @DOC)
	SET @EXPIRYDATE = (SELECT EXPIRYDATE FROM SERVCALLS WHERE DOC = @DOC)
	set @ResultVar = ' '

	IF @WARDATEFINAL <> 0 AND @WARDATEFINAL >= dbo.DATETOMIN(getdate())
		begin
			set @ResultVar = ' (Un. Warr.)'
		end

	IF @EXPIRYDATE <> 0 AND @EXPIRYDATE >= dbo.DATETOMIN(getdate())
		begin
			set @ResultVar = ' (Un. Cont.)'
		end

	return @ResultVar
END
GO
/****** Object:  UserDefinedFunction [dbo].[SVCCALL_TECHS]    Script Date: 11/28/2009 15:13:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE FUNCTION [dbo].[SVCCALL_TECHS]()
RETURNS 
@SVCCALL_TECHS TABLE 
(
	-- Add the column definitions for the TABLE variable here
	SERVCALL INT, 
	TECH INT
)
AS
BEGIN
	-- Fill the table variable with the rows for your result set
	INSERT INTO  @SVCCALL_TECHS 
	SELECT SERVCALLS.DOC, SERVCALLS.TECHNICIAN FROM SERVCALLS
	WHERE SERVCALLS.DOC > 0 AND SERVCALLS.TECHNICIAN > 0
	INSERT INTO  @SVCCALL_TECHS 
	SELECT SERVCALLS.DOC, SERVCALLS.TECHNICIAN2 FROM SERVCALLS
	WHERE SERVCALLS.DOC > 0 AND SERVCALLS.TECHNICIAN2 > 0
	INSERT INTO  @SVCCALL_TECHS 
	SELECT SERVCALLS.DOC, SERVCALLS.TECHNICIAN3 FROM SERVCALLS
	WHERE SERVCALLS.DOC > 0 AND SERVCALLS.TECHNICIAN3 > 0
	RETURN 
END
GO
/****** Object:  View [dbo].[V_SVCCALL_RESOLUTION]    Script Date: 11/28/2009 15:13:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[V_SVCCALL_RESOLUTION]
AS
SELECT     CODE, DES
FROM         dbo.SOLUTIONS
WHERE     (SOLUTION > 0)
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
         Begin Table = "SOLUTIONS"
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
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'V_SVCCALL_RESOLUTION'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'V_SVCCALL_RESOLUTION'
GO
/****** Object:  View [dbo].[V_SVCCALL_SURVEY]    Script Date: 11/28/2009 15:13:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[V_SVCCALL_SURVEY]
AS
SELECT     dbo.QUESTFORM.QUESTFCODE, dbo.QUESTFORM.QUESTFDES, dbo.QUESTIONS.QUESTNUM, dbo.QUESTIONTEXT(dbo.QUESTIONS.QUESTF, 
                      dbo.QUESTIONS.QUESTNUM) AS QUESTDES, dbo.ANSWERS.ANSNUM, dbo.ANSWERS.ANSDES
FROM         dbo.QUESTFORM INNER JOIN
                      dbo.QUESTIONS ON dbo.QUESTFORM.QUESTF = dbo.QUESTIONS.QUESTF INNER JOIN
                      dbo.ANSWERS ON dbo.QUESTIONS.QUESTF = dbo.ANSWERS.QUESTF AND dbo.QUESTIONS.QUESTNUM = dbo.ANSWERS.QUESTNUM
WHERE     (dbo.QUESTFORM.QUESTF > 0)
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[27] 4[35] 2[13] 3) )"
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
         Configuration = "(H (2[33] 3) )"
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
         Begin Table = "QUESTFORM"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 114
               Right = 189
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "QUESTIONS"
            Begin Extent = 
               Top = 6
               Left = 227
               Bottom = 114
               Right = 378
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "ANSWERS"
            Begin Extent = 
               Top = 6
               Left = 416
               Bottom = 114
               Right = 567
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
         Width = 6315
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      PaneHidden = 
      Begin ColumnWidths = 11
         Column = 6930
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
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'V_SVCCALL_SURVEY'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'V_SVCCALL_SURVEY'
GO
/****** Object:  View [dbo].[V_SVCCALL_DETAILS]    Script Date: 11/28/2009 15:13:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[V_SVCCALL_DETAILS]
AS
SELECT     TOP (100) PERCENT DOCNO, TXT AS TEXT, USERLOGIN
FROM         dbo.SVCCALL_DETAILS2() AS SVCCALL_DETAILS2_1
ORDER BY DOCNO, USERLOGIN
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
         Configuration = "(H (1[50] 2[25] 3) )"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1[56] 3) )"
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
         Begin Table = "SVCCALL_DETAILS2_1"
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
         Width = 3420
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
         Filter = 2610
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'V_SVCCALL_DETAILS'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'V_SVCCALL_DETAILS'
GO
/****** Object:  View [dbo].[V_SVCCALLTECHS]    Script Date: 11/28/2009 15:13:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[V_SVCCALLTECHS]
AS
SELECT     SERVCALL, TECH
FROM         dbo.SVCCALL_TECHS() AS SVCCALL_TECHS_1
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
         Begin Table = "SVCCALL_TECHS_1"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 84
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
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'V_SVCCALLTECHS'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'V_SVCCALLTECHS'
GO
/****** Object:  View [dbo].[V_SERVCALL]    Script Date: 11/28/2009 15:13:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[V_SERVCALL]
AS
SELECT     TOP (100) PERCENT dbo.DOCUMENTS.DOCNO, dbo.SERVCALLS.STARTDATE AS [Date Opened], dbo.SERVCALLS.PDATE, 
                      system.dbo.USERS.USERLOGIN, dbo.SVCCALL_CODE(dbo.DOCUMENTS.DOC) + ' ' + dbo.SVCCALL_STATUS(dbo.DOCUMENTS.DOC) AS Status, 
                      dbo.CUSTOMERS.CUSTDES AS [Customer Name], dbo.DESTCODES.ADDRESS AS [Address 2], dbo.DESTCODES.ADDRESSA AS [Address 2.1], 
                      dbo.DESTCODES.STATE AS [Address 3], dbo.DESTCODES.ZIP AS [Post Code], '' AS [City/County], dbo.PHONEBOOK.NAME, 
                      dbo.SVCCALL_PHONE(dbo.DOCUMENTS.DOC) AS [Phone Number], dbo.SERVTYPES.SERVTDES, dbo.SERVCALLS.PTIME
FROM         dbo.PHONEBOOK RIGHT OUTER JOIN
                      system.dbo.USERS AS USERS_2 RIGHT OUTER JOIN
                      dbo.SERVTYPES RIGHT OUTER JOIN
                      dbo.DESTCODES INNER JOIN
                      dbo.DOCUMENTS INNER JOIN
                      dbo.SERVCALLS ON dbo.DOCUMENTS.DOC = dbo.SERVCALLS.DOC ON dbo.DESTCODES.DESTCODE = dbo.DOCUMENTS.DESTCODE INNER JOIN
                      dbo.CALLSTATUSES ON dbo.SERVCALLS.CALLSTATUS = dbo.CALLSTATUSES.CALLSTATUS INNER JOIN
                      dbo.CUSTOMERS ON dbo.DOCUMENTS.CUST = dbo.CUSTOMERS.CUST ON 
                      dbo.SERVTYPES.SERVTYPE = dbo.SERVCALLS.SERVTYPE LEFT OUTER JOIN
                      system.dbo.USERS INNER JOIN
                      dbo.V_SVCCALLTECHS ON system.dbo.USERS.T$USER = dbo.V_SVCCALLTECHS.TECH ON 
                      dbo.SERVCALLS.DOC = dbo.V_SVCCALLTECHS.SERVCALL ON USERS_2.T$USER = dbo.SERVCALLS.TECHNICIAN3 ON 
                      dbo.PHONEBOOK.PHONE = dbo.SERVCALLS.PHONE LEFT OUTER JOIN
                      dbo.DOCUMENTSA ON dbo.DOCUMENTS.DOC = dbo.DOCUMENTSA.DOC
WHERE     (dbo.DOCUMENTS.TYPE = 'Q') AND (dbo.CALLSTATUSES.ACTIVEFLAG = 'Y') AND (dbo.SERVCALLS.PDATE >= dbo.DATETOMIN(GETDATE() - 1)) AND 
                      (dbo.CALLSTATUSES.ZPDA_PDASTATUS = 'Y')
ORDER BY dbo.SERVCALLS.PDATE
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[35] 2[5] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1[33] 4[44] 3) )"
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
         Configuration = "(H (1[65] 3) )"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4[50] 3) )"
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
      ActivePaneConfig = 6
   End
   Begin DiagramPane = 
      PaneHidden = 
      Begin Origin = 
         Top = -288
         Left = 0
      End
      Begin Tables = 
         Begin Table = "DOCUMENTSA"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 114
               Right = 206
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "PHONEBOOK"
            Begin Extent = 
               Top = 516
               Left = 227
               Bottom = 624
               Right = 386
            End
            DisplayFlags = 280
            TopColumn = 24
         End
         Begin Table = "USERS (system.dbo)"
            Begin Extent = 
               Top = 390
               Left = 257
               Bottom = 498
               Right = 408
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "V_SVCCALLTECHS"
            Begin Extent = 
               Top = 362
               Left = 543
               Bottom = 440
               Right = 694
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "DESTCODES"
            Begin Extent = 
               Top = 6
               Left = 244
               Bottom = 114
               Right = 395
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "DOCUMENTS"
            Begin Extent = 
               Top = 114
               Left = 38
               Bottom = 222
               Right = 227
            End
            DisplayFlags = 280
            TopColumn = 6
         End
         Begin Table = "SERVCALLS"
            Begin Extent = 
               Top = 279
               Left = 328
               Bottom = 387
               Right = 49' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'V_SERVCALL'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'9
            End
            DisplayFlags = 280
            TopColumn = 10
         End
         Begin Table = "CALLSTATUSES"
            Begin Extent = 
               Top = 323
               Left = 52
               Bottom = 431
               Right = 219
            End
            DisplayFlags = 280
            TopColumn = 9
         End
         Begin Table = "CUSTOMERS"
            Begin Extent = 
               Top = 224
               Left = 573
               Bottom = 332
               Right = 744
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "USERS_2"
            Begin Extent = 
               Top = 207
               Left = 32
               Bottom = 315
               Right = 183
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "SERVTYPES"
            Begin Extent = 
               Top = 469
               Left = 16
               Bottom = 577
               Right = 167
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
      PaneHidden = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 17
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
         Column = 3945
         Alias = 1710
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 2850
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'V_SERVCALL'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'V_SERVCALL'
GO

ALTER FUNCTION [dbo].[SVCCALL_PHONE]
(
	-- Add the parameters for the function here
	@DOC INT
)
RETURNS VARCHAR(20)
AS
BEGIN
	-- Declare the return variable here
	DECLARE @ResultVar VARCHAR(20)
	DECLARE @DPHONE VARCHAR(20)
	DECLARE @PPHONE VARCHAR(20)
	DECLARE @CPHONE VARCHAR(20)

	--(DESTCODES.PHONENUM <> '' ? DESTCODES.PHONENUM : PHONEBOOK.PHONENUM)
	SET @DPHONE = (SELECT DESTCODES.PHONENUM 
				FROM SERVCALLS, DOCUMENTS, DESTCODES 
				WHERE DOCUMENTS.DESTCODE = DESTCODES.DESTCODE
				AND DOCUMENTS.DOC = SERVCALLS.DOC
				AND DOCUMENTS.DOC = @DOC)
	
	SET @PPHONE = (SELECT PHONEBOOK.PHONENUM 
					FROM SERVCALLS,  PHONEBOOK 
					WHERE SERVCALLS.PHONE = PHONEBOOK.PHONE
					AND SERVCALLS.DOC = @DOC)

	SET @CPHONE = (SELECT CUSTOMERS.PHONE 
					FROM DOCUMENTS,  CUSTOMERS 
					WHERE DOCUMENTS.CUST = CUSTOMERS.CUST
					AND DOCUMENTS.DOC = @DOC)
					
	SET @ResultVar = ' '

	IF @DPHONE = '' AND @PPHONE  <> ''
		begin
			set @ResultVar = @PPHONE
		end

	IF @PPHONE= '' AND @DPHONE  <> ''
		begin
			set @ResultVar = @DPHONE
		end

	IF @PPHONE <> '' AND @DPHONE  <> ''
		begin
			set @ResultVar = @DPHONE
		end

	IF @PPHONE = '' AND @DPHONE  = ''
		begin
			set @ResultVar = @CPHONE
		end
	return @ResultVar
END
GO

create FUNCTION [dbo].[SVCCALL_ADDRESS]
(
	-- Add the parameters for the function here
	@PART varCHAR(5),
	@DOC INT
)
RETURNS VARCHAR(20)
AS
BEGIN
	-- Declare the return variable here
	DECLARE @ResultVar VARCHAR(255)
	DECLARE @DADD VARCHAR(20)
	DECLARE @CADD VARCHAR(20)

if @PART = 'A1'
begin
	--(DESTCODES.PHONENUM <> '' ? DESTCODES.PHONENUM : PHONEBOOK.PHONENUM)
	SET @DADD = (SELECT DESTCODES.ADDRESS 
				FROM SERVCALLS, DOCUMENTS, DESTCODES 
				WHERE DOCUMENTS.DESTCODE = DESTCODES.DESTCODE
				AND DOCUMENTS.DOC = SERVCALLS.DOC
				AND DOCUMENTS.DOC = @DOC)

	SET @CADD = (SELECT CUSTOMERS.ADDRESS 
					FROM DOCUMENTS,  CUSTOMERS 
					WHERE DOCUMENTS.CUST = CUSTOMERS.CUST
					AND DOCUMENTS.DOC = @DOC)
end		


if @PART = 'A2'
begin
	--(DESTCODES.PHONENUM <> '' ? DESTCODES.PHONENUM : PHONEBOOK.PHONENUM)
	SET @DADD = (SELECT DESTCODES.ADDRESSA 
				FROM SERVCALLS, DOCUMENTS, DESTCODES 
				WHERE DOCUMENTS.DESTCODE = DESTCODES.DESTCODE
				AND DOCUMENTS.DOC = SERVCALLS.DOC
				AND DOCUMENTS.DOC = @DOC)

	SET @CADD = ''
end	


if @PART = 'A3'
begin
	--(DESTCODES.PHONENUM <> '' ? DESTCODES.PHONENUM : PHONEBOOK.PHONENUM)
	SET @DADD = (SELECT DESTCODES.STATE 
				FROM SERVCALLS, DOCUMENTS, DESTCODES 
				WHERE DOCUMENTS.DESTCODE = DESTCODES.DESTCODE
				AND DOCUMENTS.DOC = SERVCALLS.DOC
				AND DOCUMENTS.DOC = @DOC)

	SET @CADD = (SELECT CUSTOMERS.STATE 
					FROM DOCUMENTS,  CUSTOMERS 
					WHERE DOCUMENTS.CUST = CUSTOMERS.CUST
					AND DOCUMENTS.DOC = @DOC)
end	

if @PART = 'A4'
begin
	--(DESTCODES.PHONENUM <> '' ? DESTCODES.PHONENUM : PHONEBOOK.PHONENUM)
	SET @DADD = (SELECT DESTCODES.STATEA 
				FROM SERVCALLS, DOCUMENTS, DESTCODES 
				WHERE DOCUMENTS.DESTCODE = DESTCODES.DESTCODE
				AND DOCUMENTS.DOC = SERVCALLS.DOC
				AND DOCUMENTS.DOC = @DOC)

	SET @CADD = ''
end	

if @PART = 'A5'
begin
	--(DESTCODES.PHONENUM <> '' ? DESTCODES.PHONENUM : PHONEBOOK.PHONENUM)
	SET @DADD = (SELECT DESTCODES.ZIP 
				FROM SERVCALLS, DOCUMENTS, DESTCODES 
				WHERE DOCUMENTS.DESTCODE = DESTCODES.DESTCODE
				AND DOCUMENTS.DOC = SERVCALLS.DOC
				AND DOCUMENTS.DOC = @DOC)

	SET @CADD = (SELECT CUSTOMERS.ZIP 
					FROM DOCUMENTS,  CUSTOMERS 
					WHERE DOCUMENTS.CUST = CUSTOMERS.CUST
					AND DOCUMENTS.DOC = @DOC)
end	
			
	SET @ResultVar = ' '

	IF @DADD = '' AND @CADD <> ''
		begin
			set @ResultVar = @CADD
		end

	IF @CADD = '' AND @DADD  <> ''
		begin
			set @ResultVar = @DADD
		end

	IF @CADD <> '' AND @DADD  <> ''
		begin
			set @ResultVar = @DADD
		end

	return @ResultVar
END

GO

ALTER VIEW [dbo].[V_SERVCALL]
AS
SELECT     TOP (100) PERCENT DOCUMENTS.DOCNO, SERVCALLS.STARTDATE AS [Date Opened], dbo.SERVCALLS.PDATE, system.dbo.USERS.USERLOGIN, 
                      dbo.SVCCALL_CODE(dbo.DOCUMENTS.DOC) + ' ' + dbo.SVCCALL_STATUS(dbo.DOCUMENTS.DOC) AS Status, dbo.CUSTOMERS.CUSTDES AS [Customer Name], 
                       dbo.SVCCALL_ADDRESS('A1',dbo.DOCUMENTS.DOC) AS [Address 2],  dbo.SVCCALL_ADDRESS('A2',dbo.DOCUMENTS.DOC) AS [Address 2.1], dbo.SVCCALL_ADDRESS('A2',dbo.DOCUMENTS.DOC) AS [Address 3], 
                       dbo.SVCCALL_ADDRESS('A5',dbo.DOCUMENTS.DOC) AS [Post Code],  dbo.SVCCALL_ADDRESS('A4',dbo.DOCUMENTS.DOC) AS [City/County], dbo.PHONEBOOK.NAME, dbo.SVCCALL_PHONE(dbo.DOCUMENTS.DOC) AS [Phone Number], 
                      dbo.SERVTYPES.SERVTDES, dbo.SERVCALLS.PTIME
FROM         dbo.PHONEBOOK RIGHT OUTER JOIN
                      system.dbo.USERS AS USERS_2 RIGHT OUTER JOIN
                      dbo.SERVTYPES RIGHT OUTER JOIN
                      dbo.DESTCODES INNER JOIN
                      dbo.DOCUMENTS INNER JOIN
                      dbo.SERVCALLS ON dbo.DOCUMENTS.DOC = dbo.SERVCALLS.DOC ON dbo.DESTCODES.DESTCODE = dbo.DOCUMENTS.DESTCODE INNER JOIN
                      dbo.CALLSTATUSES ON dbo.SERVCALLS.CALLSTATUS = dbo.CALLSTATUSES.CALLSTATUS INNER JOIN
                      dbo.CUSTOMERS ON dbo.DOCUMENTS.CUST = dbo.CUSTOMERS.CUST ON dbo.SERVTYPES.SERVTYPE = dbo.SERVCALLS.SERVTYPE LEFT OUTER JOIN
                      system.dbo.USERS INNER JOIN
                      dbo.V_SVCCALLTECHS ON system.dbo.USERS.T$USER = dbo.V_SVCCALLTECHS.TECH ON dbo.SERVCALLS.DOC = dbo.V_SVCCALLTECHS.SERVCALL ON 
                      USERS_2.T$USER = dbo.SERVCALLS.TECHNICIAN3 ON dbo.PHONEBOOK.PHONE = dbo.SERVCALLS.PHONE LEFT OUTER JOIN
                      dbo.DOCUMENTSA ON dbo.DOCUMENTS.DOC = dbo.DOCUMENTSA.DOC
WHERE     (dbo.DOCUMENTS.TYPE = 'Q') AND (dbo.CALLSTATUSES.ACTIVEFLAG = 'Y') AND (dbo.SERVCALLS.PDATE >= dbo.DATETOMIN(GETDATE() - 1)) AND 
                      (dbo.CALLSTATUSES.ZPDA_PDASTATUS = 'Y')
ORDER BY dbo.SERVCALLS.PDATE

GO

ALTER FUNCTION [dbo].[WARHS_EXT] (
) 
RETURNS 
@RET_WARHS_EXT TABLE 
(
	WARHSNAME varchar(10),
	PARTNAME varchar(20),
	PARTDES Varchar(50), 
	QTY REAL,
	PRICE MONEY
)
AS
BEGIN

DECLARE @WARHS_EXT TABLE 
(
	WARHSNAME varchar(10),
	PARTNAME varchar(20),
	PARTDES Varchar(50), 
	QTY REAL,
	PRICE MONEY
)
	-- Fill the table variable with the rows for your result set
	INSERT INTO @WARHS_EXT
	SELECT     dbo.WAREHOUSES.WARHSNAME, dbo.PART.PARTNAME, dbo.PART.PARTDES, SUM(dbo.REALQUANT(dbo.WARHSBAL.BALANCE)) 
						  AS QTY, 0 AS PRICE
	FROM         dbo.WARHSBAL INNER JOIN
						  dbo.PART ON dbo.WARHSBAL.PART = dbo.PART.PART INNER JOIN
						  dbo.WAREHOUSES ON dbo.WARHSBAL.WARHS = dbo.WAREHOUSES.WARHS
	WHERE     (dbo.WARHSBAL.WARHS IN
							  (SELECT DISTINCT WARHS
								FROM          dbo.USERSA
								WHERE      (WARHS > 0))) AND (dbo.WARHSBAL.CUST = - 1)
	GROUP BY dbo.WAREHOUSES.WARHSNAME, dbo.PART.PARTNAME, dbo.PART.PARTDES
	HAVING      (SUM(dbo.REALQUANT(dbo.WARHSBAL.BALANCE)) > 0)

/*AND (dbo.PARTPRICE.PLIST = 15)*/

	INSERT INTO @WARHS_EXT
	SELECT     dbo.WAREHOUSES.WARHSNAME, dbo.PART.PARTNAME, dbo.PART.PARTDES, 9999 AS QTY, 0 AS PRICE
	FROM         dbo.WAREHOUSES CROSS JOIN
						  dbo.PART RIGHT OUTER JOIN
						  dbo.ZPDA_KANBAN_PART ON dbo.PART.PART = dbo.ZPDA_KANBAN_PART.PART
	WHERE     (dbo.WAREHOUSES.WARHS IN
							  (SELECT     WARHS
								FROM          dbo.USERSA
								WHERE      (WARHS > 0))) AND (dbo.ZPDA_KANBAN_PART.PART > 0)
/**/

INSERT INTO @RET_WARHS_EXT
SELECT * FROM @WARHS_EXT
ORDER BY PARTDES

RETURN 
END