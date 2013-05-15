USE [system] 

SELECT TITLE AS "@name", 
       (SELECT TITLE AS "@name", 
               (SELECT '', 
                       (SELECT TriggerName                                       AS "@trigger",
                                dbo.FormTriggerSQL(menutable.T$EXEC, TriggerName) AS "@sql"
                        FROM   dbo.FORMTRIGGERS(menutable.T$EXEC) 
                        FOR xml path('triggers'), type), 
                       (SELECT CNAME     AS "@name", 
                               CTYPE     AS "@type", 
                               CWIDTH    AS "@width", 
                               CREADONLY AS "@readonly", 
                               CHIDE     AS "@hidden", 
                               dbo.HelpText(menutable.T$EXEC, CNAME) AS "@help",
                               (SELECT TriggerName                                          AS "@trigger",
                                        dbo.TriggerSQL(menutable.T$EXEC, CNAME, TriggerName) AS "@sql"
                                FROM   COLUMNTRIGGERS(menutable.T$EXEC, CNAME) 
                                FOR xml path('triggers'), type) 
                        FROM   dbo.FormColumns(menutable.T$EXEC) 
                        FOR xml path('column'), type) 
                FOR xml path('form'), type), 
               (SELECT '', 
                       (SELECT TriggerName                                       AS "@trigger",
                                dbo.FormTriggerSQL(menutable.T$EXEC, TriggerName) AS "@sql"
                         FROM   dbo.FORMTRIGGERS(menutable.T$EXEC) 
                        FOR xml path('triggers'), type), 
                       (SELECT CNAME     AS "@name", 
                               CTYPE     AS "@type", 
                               CWIDTH    AS "@width", 
                               CREADONLY AS "@readonly", 
                               CHIDE     AS "@hidden", 
                               dbo.HelpText(dbo.FIRSTCHILDFORM(menutable.T$EXEC), CNAME) AS "@help",
                               (SELECT TriggerName                                                              AS "@trigger",
                                        dbo.TriggerSQL(dbo.FIRSTCHILDFORM(menutable.T$EXEC), CNAME, TriggerName) AS "@sql"
                                 FROM   COLUMNTRIGGERS(dbo.FIRSTCHILDFORM(menutable.T$EXEC), CNAME)
                                 FOR xml path('triggers'), type) 
                        FROM   dbo.FormColumns(dbo.FIRSTCHILDFORM(menutable.T$EXEC))
                        FOR xml path('column'), type) 
                FOR xml path('table'), type) 
        FROM   dbo.v_ExecMenu AS menutable 
        WHERE  EDES = 'SFDC' 
               AND T$EXEC IN (SELECT EXECRUN 
                              FROM   MENU 
                              WHERE  T$EXEC = exetable.T$EXEC) 
               AND TYPE = 'F' 
        FOR xml path('interface'), type) 
FROM   T$EXEC AS exetable 
WHERE  EDES = 'SFDC' 
       AND TYPE = 'M' 
       AND dbo.MENUFORMCOUNT(T$EXEC) > 0 
FOR xml path('menu'), type  