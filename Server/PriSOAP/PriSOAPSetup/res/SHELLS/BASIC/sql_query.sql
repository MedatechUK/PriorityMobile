GO
/****** Object:  UserDefinedFunction [dbo].[INTQUANT]    Script Date: 11/28/2009 15:13:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Si
-- Create date: 4/12/07
-- Description:	return number to divide by for shifted ints
-- =============================================
create FUNCTION [dbo].[INTQUANT] 
(
@Quant int
)
RETURNS int
AS
BEGIN
	-- Declare the return variable here
	DECLARE @result int

	set @result = @Quant * (SELECT     POWER(10, VALUE)  
                            FROM          system.dbo.SYSCONST
                            WHERE      (NAME = 'DECIMAL'))

	-- Return the result of the function
	RETURN @result

END
GO
/****** Object:  UserDefinedFunction [dbo].[DATETOMIN]    Script Date: 11/28/2009 15:13:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Si
-- Create date: 4/12/07
-- =============================================
CREATE FUNCTION [dbo].[DATETOMIN] 
(
	-- Add the parameters for the function here
	@DT as datetime
)
RETURNS int
AS
BEGIN
	-- Declare the return variable here
	DECLARE @ResultVar as int

	-- Add the T-SQL statements to compute the return value here
	SELECT @ResultVar = (DATEdiff(MI, '19880101',@DT ))

	-- Return the result of the function
	RETURN @ResultVar

END
GO
/****** Object:  UserDefinedFunction [dbo].[MINTODATE]    Script Date: 11/28/2009 15:13:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Si
-- Create date: 4/12/07
-- =============================================
CREATE FUNCTION [dbo].[MINTODATE]
(
	-- Add the parameters for the function here
	@MIN INT
)
RETURNS DATETIME
AS
BEGIN
	-- Declare the return variable here
	DECLARE @ResultVar datetime

	-- Add the T-SQL statements to compute the return value here
	SELECT @ResultVar = (DATEADD(MI, @MIN, '19880101'))

	-- Return the result of the function
	RETURN @ResultVar

END
GO
/****** Object:  UserDefinedFunction [dbo].[REALQUANT]    Script Date: 11/28/2009 15:13:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Si
-- Create date: 4/12/07
-- Description:	return number to divide by for shifted ints
-- =============================================
CREATE FUNCTION [dbo].[REALQUANT] 
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