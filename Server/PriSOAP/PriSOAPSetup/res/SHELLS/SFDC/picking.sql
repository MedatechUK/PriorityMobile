
GO

/****** Object:  View [dbo].[V_USERS]    Script Date: 05/10/2011 09:41:39 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE VIEW [dbo].[V_USERS]
AS
SELECT     system.dbo.USERS.USERLOGIN, system.dbo.USERS.USERNAME, system.dbo.USERSB.ZTRX_PINCODE, system.dbo.USERS.USERID
FROM         system.dbo.USERS LEFT OUTER JOIN
                      system.dbo.USERSB ON system.dbo.USERS.T$USER = system.dbo.USERSB.T$USER
WHERE     (system.dbo.USERS.T$USER > 0)





GO

EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[22] 4[4] 2[37] 3) )"
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
         Begin Table = "USERS (system.dbo)"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 114
               Right = 189
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "USERSB (system.dbo)"
            Begin Extent = 
               Top = 6
               Left = 227
               Bottom = 114
               Right = 410
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
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'V_USERS'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'V_USERS'
GO


GO

/****** Object:  View [dbo].[v_USERS]    Script Date: 05/10/2011 09:42:14 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE VIEW [dbo].[v_USERS]
AS
SELECT     TOP (100) PERCENT USERS.USERLOGIN, system.dbo.USERSB.ZTRX_PINCODE AS PASSWORD, 
                      dbo.USERSA.ZTRX_GUNWARHS AS WARHSNAME
FROM         system.dbo.USERSB INNER JOIN
                      system.dbo.USERS AS USERS INNER JOIN
                      dbo.USERSA ON USERS.T$USER = dbo.USERSA.T$USER ON system.dbo.USERSB.T$USER = USERS.T$USER
WHERE     (USERS.USERID > 0)
ORDER BY USERS.USERLOGIN


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
         Begin Table = "USERSB (system.dbo)"
            Begin Extent = 
               Top = 114
               Left = 38
               Bottom = 222
               Right = 221
            End
            DisplayFlags = 280
            TopColumn = 51
         End
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
         Begin Table = "USERSA"
            Begin Extent = 
               Top = 6
               Left = 227
               Bottom = 312
               Right = 547
            End
            DisplayFlags = 280
            TopColumn = 50
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
         Column = 1560
         Alias = 1200
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
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'v_USERS'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'v_USERS'
GO



GO

/****** Object:  StoredProcedure [dbo].[ZSFDC_MarkPicked]    Script Date: 05/10/2011 09:42:37 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[ZSFDC_MarkPicked] 
	-- Add the parameters for the stored procedure here
	@PICKREFNUM  int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
    IF EXISTS (SELECT PICKREFNUM FROM ZTRX_PICKMONITOR WHERE PICKREFNUM  = @PICKREFNUM and TOGUN <> 'Y')
		BEGIN
			update ZTRX_PICKMONITOR set TOGUN = 'Y' where PICKREFNUM  = @PICKREFNUM 
			select @PICKREFNUM
		END
END

GO


GO

/****** Object:  UserDefinedFunction [dbo].[REALQUANT]    Script Date: 05/10/2011 09:42:59 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




-- =============================================
-- Author:		Si
-- Create date: 4/12/07
-- Description:	return number to divide by for shifted ints
-- =============================================
create  FUNCTION [dbo].[REALQUANT] 
(
@Quant int
)
RETURNS int
AS
BEGIN
	-- Declare the return variable here
	DECLARE @result int

	set @result = @Quant / (SELECT     POWER(10, VALUE)  
                            FROM          system.dbo.SYSCONST
                            WHERE      (NAME = 'DECIMAL'))

	-- Return the result of the function
	RETURN @result

END




GO


GO

/****** Object:  UserDefinedFunction [dbo].[ZSFDC_NEXTPICK]    Script Date: 05/10/2011 09:43:10 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[ZSFDC_NEXTPICK] 
(
	-- Add the parameters for the function here
	@USERLOGIN VARCHAR(30)
)
RETURNS VARCHAR(30)
AS
BEGIN
	DECLARE @UN INT -- ID of user
	DECLARE @ASPI INT -- Assigned Pick Note Count
	declare @PREF int -- Picking reference 
	declare @PRETXT varchar(30) -- Pick Reference Text
	
	set @UN = (select T$USER -- Get UserID
		from system.dbo.USERS 
		where upper(USERLOGIN) = upper(@USERLOGIN))
	set @ASPI = (select COUNT(PICKREF) -- Get Count assigned
		from ZTRX_PICKMONITOR 
		WHERE T$USER = @UN
		and TOGUN <> 'Y'
		and RELEASE = 'Y'
		)
		
	if @ASPI > 0 
		begin -- An assigned pick exists
			set @PREF = (select top 1 PICKREF from ZTRX_PICKMONITOR  
			where T$USER = @UN
			and TOGUN <> 'Y'
			and RELEASE = 'Y')
		end
	else
		begin -- No assigned, pick from pool
			set @PREF = (select top 1 PICKREF from ZTRX_PICKMONITOR  
			where T$USER = 0
			and TOGUN <> 'Y'
			and RELEASE = 'Y')	
		end
	
	if not(@PREF=0)
	begin		
		set @PRETXT =(select PICKREFNUM from ZTRX_PICKMONITOR where PICKREF = @PREF)
	end
	else
	begin
		set @PRETXT = '' -- No Pick Notes available
	end
		
	return @PRETXT
	
	
END


GO

