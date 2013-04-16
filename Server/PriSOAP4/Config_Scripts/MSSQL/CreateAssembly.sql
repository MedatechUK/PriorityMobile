CREATE ASSEMBLY Priority FROM 'c:\pLicence.dll';
GO

CREATE FUNCTION Activated(@EntityID int, @EDES NCHAR(4)) RETURNS INT 
AS EXTERNAL NAME Priority.licence.Activated; 
GO

DROP FUNCTION Activated

drop assembly Priority