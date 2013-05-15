USE [system]
GO
/****** Object:  UserDefinedFunction [dbo].[HelpText]    Script Date: 04/29/2013 23:42:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HelpText]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'-- =============================================
-- Author:		Si
-- Create date: 29/04/2013
-- Description:	Get coallesced column help
-- =============================================
CREATE FUNCTION [dbo].[HelpText]
(
	-- Add the parameters for the function here
	@form int,
	@name varchar(100)
)
RETURNS varchar(max)
AS
BEGIN

	declare @trig varchar(max)    
	    	
	select @trig = coalesce(@trig,'''','''') + TEXT
	FROM         dbo.FORMCLTRIGTEXT
	WHERE     (FORM = 0 + @form) AND (NAME = @name) AND (TRIG = 0 + 12)
    and TEXTLINE > 0    order by TEXTORD
	return @trig
	
END
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[FormTriggerSQL]    Script Date: 04/29/2013 23:42:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FormTriggerSQL]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'-- =============================================
-- Author:		Si
-- Create date: 29/04/2013
-- Description:	Get coallesced Form Trigger SQL
-- =============================================
CREATE FUNCTION [dbo].[FormTriggerSQL]
(
	-- Add the parameters for the function here
	@form int,
	@trigger varchar(100)
)
RETURNS varchar(max)
AS
BEGIN

	declare @trig varchar(max)    
	select @trig = coalesce(@trig,'''','''') + FORMTRIGTEXT.TEXT    
	FROM FORMTRIG, FORMTRIGTEXT, TRIGGERS
	WHERE FORMTRIG.FORM = FORMTRIGTEXT.FORM
	AND  FORMTRIG.TRIG = FORMTRIGTEXT.TRIG
	AND TRIGGERS.TRIG = FORMTRIG.TRIG
	AND FORMTRIG.FORM = @form
	and TRIGGERS.TRIGNAME = @trigger
	and TEXTLINE > 0    
	order by TEXTORD
	return @trig
	
END
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[FormColumns]    Script Date: 04/29/2013 23:42:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FormColumns]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'-- =============================================
-- Author:		Si
-- Create date: 29/04/2013
-- Description:	List of form columns
-- =============================================
CREATE FUNCTION [dbo].[FormColumns] 
(
	@e int
)
RETURNS 
@T TABLE 
(
	CNAME VARCHAR(100),
	CTYPE VARCHAR(20),
	CWIDTH INT,
	CREADONLY VARCHAR(1),
	CHIDE VARCHAR(1)
)
AS
BEGIN
	-- Fill the table variable with the rows for your result set
	INSERT INTO @T
	SELECT 		
				NAME,
				COLUMNS.TYPE,
				COLUMNS.WIDTH,
				FORMCLMNS.READONLY,
				FORMCLMNS.HIDE
	FROM   FORMCLMNS 
	join COLUMNS on FORMCLMNS.T$COLUMN = COLUMNS.T$COLUMN
	WHERE  FORMCLMNS.FORM = @e
	and COLUMNS.CNAME not in (''RECORDTYPE'',''LINE'',''LOADED'',''KEY1'',''KEY2'',''KEY3'',''BUBBLEID'')
	ORDER BY FORMCLMNS.POS
	RETURN 
END
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[FORMTRIGGERS]    Script Date: 04/29/2013 23:42:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FORMTRIGGERS]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'-- =============================================
-- Author:		Si
-- Create date: 29/04/2013
-- Description:	List of form Triggers
-- =============================================
create FUNCTION [dbo].[FORMTRIGGERS]
(
	-- Add the parameters for the function here
	@form int
)
RETURNS 
@trig TABLE 
(	
	TriggerName varchar(100)
)
AS
BEGIN	
	insert into @trig
	SELECT     dbo.TRIGGERS.TRIGNAME
	FROM         dbo.FORMTRIG INNER JOIN
						  dbo.TRIGGERS ON dbo.FORMTRIG.TRIG = dbo.TRIGGERS.TRIG
	WHERE     (dbo.FORMTRIG.FORM = @form)
	RETURN 
END
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[FIRSTCHILDFORM]    Script Date: 04/29/2013 23:42:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FIRSTCHILDFORM]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'-- =============================================
-- Author:		si
-- Create date: 29/04/13
-- Description:	Get the first child form T$EXEC
-- =============================================
CREATE FUNCTION [dbo].[FIRSTCHILDFORM] 
(
	-- Add the parameters for the function here
	@e int
)
RETURNS int
AS
BEGIN

	DECLARE @ResultVar int
	set @ResultVar = 
	(
		SELECT top 1 SONFORM 
		FROM FORMLINKS 
		where FATFORM = @e
		order by POS
	)
	
	RETURN @ResultVar

END
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[COLUMNTRIGGERS]    Script Date: 04/29/2013 23:42:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[COLUMNTRIGGERS]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'-- =============================================
-- Author:		Si
-- Create date: 29/04/2013
-- Description:	List of Column Triggers
-- =============================================
CREATE FUNCTION [dbo].[COLUMNTRIGGERS]
(
	-- Add the parameters for the function here
	@form int,
	@name varchar(100)
)
RETURNS 
@trig TABLE 
(	
	TriggerName varchar(100)
)
AS
BEGIN	
	insert into @trig
	SELECT     dbo.TRIGGERS.TRIGNAME
	FROM         dbo.FORMCLTRIG INNER JOIN
						  dbo.TRIGGERS ON dbo.FORMCLTRIG.TRIG = dbo.TRIGGERS.TRIG
	WHERE     (dbo.FORMCLTRIG.FORM = @form) AND (dbo.FORMCLTRIG.NAME = @name)
	RETURN 
END
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[TriggerSQL]    Script Date: 04/29/2013 23:42:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TriggerSQL]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'-- =============================================
-- Author:		Si
-- Create date: 29/04/2013
-- Description:	Get coallesced Trigger SQL
-- =============================================
CREATE FUNCTION [dbo].[TriggerSQL]
(
	-- Add the parameters for the function here
	@form int,
	@name varchar(100),
	@trigger varchar(100)
)
RETURNS varchar(max)
AS
BEGIN

	declare @trig varchar(max)    
	select @trig = coalesce(@trig,'''','''') + FORMCLTRIGTEXT.TEXT    
	FROM FORMCLTRIG, FORMCLTRIGTEXT, TRIGGERS
	WHERE FORMCLTRIG.FORM = FORMCLTRIGTEXT.FORM
	AND  FORMCLTRIG.TRIG = FORMCLTRIGTEXT.TRIG
	AND TRIGGERS.TRIG = FORMCLTRIG.TRIG
	AND FORMCLTRIG.FORM = @form
	AND FORMCLTRIG.NAME = @name
	and TRIGGERS.TRIGNAME = @trigger
    and TEXTLINE > 0    
    order by TEXTORD
    
	return @trig
	
END
' 
END
GO
/****** Object:  View [dbo].[v_ExecMenu]    Script Date: 04/29/2013 23:42:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[v_ExecMenu]'))
EXEC dbo.sp_executesql @statement = N'CREATE VIEW [dbo].[v_ExecMenu]
AS
SELECT     dbo.MENU.POS, dbo.MENU.EXECRUN, dbo.T$EXEC.ENAME, dbo.T$EXEC.T$EXEC, dbo.T$EXEC.TITLE, dbo.T$EXEC.T$TABLE, dbo.T$EXEC.TYPE, 
                      dbo.T$EXEC.APPEND, dbo.T$EXEC.EDES, dbo.T$EXEC.INS, dbo.T$EXEC.DEL, dbo.T$EXEC.UPD, dbo.T$EXEC.ZOOM, dbo.T$EXEC.HDATE
FROM         dbo.MENU RIGHT OUTER JOIN
                      dbo.T$EXEC ON dbo.MENU.T$EXEC = dbo.T$EXEC.T$EXEC
'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_DiagramPaneCount' , N'SCHEMA',N'dbo', N'VIEW',N'v_ExecMenu', NULL,NULL))
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'v_ExecMenu'
GO
/****** Object:  UserDefinedFunction [dbo].[MENUFORMCOUNT]    Script Date: 04/29/2013 23:42:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MENUFORMCOUNT]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'
CREATE FUNCTION [dbo].[MENUFORMCOUNT]
(
	@E int
)
RETURNS INT
AS
BEGIN
	
	DECLARE @ResultVar int

	set @ResultVar = (SELECT count(*)
	FROM dbo.v_ExecMenu
	WHERE T$EXEC IN (SELECT EXECRUN FROM MENU WHERE T$EXEC = @E)
	and TYPE = ''F'' 
	and EDES = ''SFDC'') --

	RETURN @ResultVar

END
' 
END
GO
