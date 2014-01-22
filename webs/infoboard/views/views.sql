/****************************************************************************************************************/
/******						Views for Graphs					   ******/
/* Included:	xxx											        */
/*														*/
/*				Execute this as a new query on the desired database				*/
/*					Example of a view given below						*/
/******						PW 14/06/13						   ******/
/****************************************************************************************************************/


/****** Object:  View [dbo].[v_AvgCallValue]    Script Date: 06/14/2013 08:27:28 ******/
/* SET ANSI_NULLS ON									*/
/* GO											*/
/*											*/
/* SET QUOTED_IDENTIFIER ON								*/
/* GO											*/
/*											*/
/* CREATE VIEW [dbo].[v_AvgCallValue]							*/
/* AS											*/
/* SELECT     ISNULL(SUM(dbo.ORDERS.TOTPRICE) / COUNT(*), 0.00) AS AvgCallValue		*/
/* FROM         dbo.ZROD_TELESALECALLS INNER JOIN										*/
/*                       dbo.ZROD_CALLSTATS ON dbo.ZROD_TELESALECALLS.CALLSTAT = dbo.ZROD_CALLSTATS.CALLSTAT INNER JOIN		*/
/*                       dbo.ORDERS ON dbo.ZROD_TELESALECALLS.ORD = dbo.ORDERS.ORD INNER JOIN					*/
/*                       dbo.ORDSTATUS ON dbo.ORDERS.ORDSTATUS = dbo.ORDSTATUS.ORDSTATUS					*/	
/* WHERE     (dbo.ZROD_TELESALECALLS.DUEDATE BETWEEN dbo.DATETOMIN8(GETDATE()) AND dbo.DATETOMIN8(GETDATE()) + 1439) AND					*/
/*                       (dbo.ZROD_CALLSTATS.ORDERPLACEDFLAG = 'Y') AND (dbo.ORDSTATUS.CANCELFLAG <> 'Y') AND (dbo.ZROD_TELESALECALLS.TELESALECALL <> 0)	*/
/*																				*/
/* GO																				*/
/****************************************************************************************************************************************************************/