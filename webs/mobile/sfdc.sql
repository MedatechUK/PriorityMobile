USE [system] 
declare @user varchar(32) --mandatory
set @user = 'service' 
select '',(
	SELECT TITLE AS "@name", 
		   (SELECT TITLE AS "@name", 
					dbo.FormProperty(menutable.FORM,'CustomHdlr') AS "@handler",
					dbo.FormProperty(menutable.FORM,'ldTable') AS "@ldTable",
					dbo.FormProperty(menutable.FORM,'ldProcedure') AS "@ldProcedure",
					dbo.FormProperty(menutable.FORM,'ldEnv') AS "@ldEnv",
				   (SELECT '', 
						   (SELECT TriggerName                                       AS "@trigger",
									dbo.FormTriggerSQL(menutable.FORM, TriggerName) AS "@sql"
							FROM   dbo.FORMTRIGGERS(menutable.FORM) 
							FOR xml path('triggers'), type), 
						   (SELECT CNAME     AS "@name", 
								   CTITLE    AS "@title",						   
								   CPOS      AS "@pos",
								   CTYPE     AS "@type", 
								   CWIDTH    AS "@width", 
								   CREADONLY AS "@readonly", 
								   CHIDE     AS "@hidden", 
								   MANDATORY AS "@mandatory", 
								   REGEX     AS "@regex",
								   BARCODE2D AS "@barcode2d",
								   dbo.HelpText(menutable.FORM, CNAME) AS "@help",
								   (SELECT TriggerName                                          AS "@trigger",
											dbo.TriggerSQL(menutable.FORM, CNAME, TriggerName) AS "@sql"
									FROM   COLUMNTRIGGERS(menutable.FORM, CNAME) 
									FOR xml path('triggers'), type) 
							FROM   dbo.FormColumns(menutable.FORM) 
							FOR xml path('column'), type) ,
						(select '',
							(SELECT NUM as "@num",
							dbo.MessageText(menutable.FORM,NUM) as "@text"					
							from TRIGMSG
							where T$EXEC = menutable.FORM
							FOR xml path('message'), type)
						FOR xml path('messages'), type)
					FOR xml path('form'), type), 
				   (SELECT '', 
						   (SELECT TriggerName                                       AS "@trigger",
									dbo.FormTriggerSQL(dbo.FIRSTCHILDFORM(menutable.FORM), TriggerName) AS "@sql"
							 FROM   dbo.FORMTRIGGERS(dbo.FIRSTCHILDFORM(menutable.FORM))
							FOR xml path('triggers'), type), 
						   (SELECT CNAME     AS "@name", 
								   CTITLE    AS "@title",						   
								   CPOS      AS "@pos",
								   CTYPE     AS "@type", 
								   CWIDTH    AS "@width", 
								   CREADONLY AS "@readonly", 
								   CHIDE     AS "@hidden", 
								   MANDATORY AS "@mandatory", 
								   REGEX     AS "@regex",
								   BARCODE2D AS "@barcode2d",
								   dbo.HelpText(dbo.FIRSTCHILDFORM(menutable.FORM), CNAME) AS "@help",
								   (SELECT TriggerName                                                              AS "@trigger",
											dbo.TriggerSQL(dbo.FIRSTCHILDFORM(menutable.FORM), CNAME, TriggerName) AS "@sql"
									 FROM   COLUMNTRIGGERS(dbo.FIRSTCHILDFORM(menutable.FORM), CNAME)
									 FOR xml path('triggers'), type) 
							FROM   dbo.FormColumns(dbo.FIRSTCHILDFORM(menutable.FORM))
							FOR xml path('column'), type) ,
						(select '',
							(SELECT NUM as "@num",
							dbo.MessageText(dbo.FIRSTCHILDFORM(menutable.FORM),NUM) as "@text"					
							from TRIGMSG
							where T$EXEC = dbo.FIRSTCHILDFORM(menutable.FORM)
							FOR xml path('message'), type)
						FOR xml path('messages'), type)
					FOR xml path('table'), type) 
			FROM   dbo.v_ExecMenu AS menutable 
		    where FORM in (
				select FORM 
				from v_SfdcForms 
				where PARENT = exetable.T$EXEC 
				and UGROUP = dbo.UserGroup(@user)
			)
			FOR xml path('interface'), type) 
	FROM   dbo.v_SfdcMenu AS exetable 
	where exists (
		select FORM 
		from dbo.v_SfdcForms 
		where PARENT = exetable.T$EXEC 
		and UGROUP = dbo.UserGroup(@user)
	)
	FOR xml path('menu'), type  )
FOR xml path('sfdc'), type 	