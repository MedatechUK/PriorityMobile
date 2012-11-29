Imports System.Reflection
Imports System.Xml
Imports System.Data.SqlClient

'EXEC sp_configure 'show advanced options', 1
'GO
'-- To update the currently configured value for advanced options.
'RECONFIGURE
'GO
'-- To enable the feature.
'EXEC sp_configure 'xp_cmdshell', 1
'GO
'-- To update the currently configured value for this feature.
'RECONFIGURE
'GO

'CREATE TABLE [dbo].[ZEMG_RMX](
'	[I] [varchar](5) NOT NULL,
'	[O] [varchar](5) NOT NULL
') ON [PRIMARY]

'CREATE TRIGGER rmx_insert
'   ON  ZEMG_RMX
'   AFTER INSERT
'AS 
'BEGIN
'	-- SET NOCOUNT ON added to prevent extra result sets from
'	-- interfering with SELECT statements.
'	SET NOCOUNT ON;
'	declare @cmd varchar(100)
'	declare @db varchar(100)
'	declare @i varchar(30)
'	declare @o varchar(30)
'	set @db = (CAST(SERVERPROPERTY ('sERVERName') AS VARCHAR(100)))
'	set @i = (select top 1 I from inserted)
'	set @o = (select top 1 O from inserted)
'	set @cmd = ('rmxCli.exe /i ' + @i+ ' /o ' + @o + ' /db '+ @db + ' /env ' + DB_NAME() + ' /u tabula /p Tabula!')
'	exec xp_cmdshell @cmd    

'END
'GO

Module rmx

    Private incode As String = Nothing
    Private outcode As String = Nothing
    Private environment As String = "system"
    Private username As String = Nothing
    Private Password As String = Nothing
    Private Database As String = Nothing
    Private debug As Boolean = False

    Dim StartTime As Date = Now

    Enum myRunMode As Integer
        Normal = 0
        Config = 1
    End Enum

    Dim WithEvents cApp As New ConsoleApp.CA

    Private ReadOnly Property DataURL() As String
        Get
            Return String.Format("http://rmx.emerge-it.co.uk/?o={0}&i={1}", outcode, incode)
        End Get
    End Property

    Private ReadOnly Property ConnectionString() As String
        Get
            Return String.Format("Data Source={0};Initial Catalog={1};User ID={2};Password={3}", _
                            Database, _
                            environment, _
                            username, _
                            Password)
        End Get
    End Property

    Public Sub Main()

        With cApp
            .RunMode = myRunMode.Normal
            .doWelcome(Assembly.GetExecutingAssembly())
            Try
                .GetArgs(Command)
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try

            If Not .Quit Then
                Select Case .RunMode
                    Case myRunMode.Normal
                        If IsNothing(incode) Or IsNothing(outcode) Or IsNothing(Database) Or IsNothing(environment) Or IsNothing(username) Or IsNothing(Password) Then
                            Console.WriteLine("Missing Parameter(s). Please seek /help.")
                            .Quit = True
                        End If
                        GetXML()
                    Case myRunMode.Config
                        If IsNothing(Database) Or IsNothing(environment) Or IsNothing(username) Or IsNothing(Password) Then
                            Console.WriteLine("Missing Parameter(s). Please seek /help.")
                            .Quit = True
                        End If
                        doConfig()
                End Select
            End If

            Dim RunLength As System.TimeSpan = Now.Subtract(StartTime)
            Console.WriteLine("")
            If debug Then Console.WriteLine(String.Format("Completed in {0} milliseconds.", RunLength.Milliseconds.ToString))

            cApp.Finalize()

        End With
    End Sub

    Private Sub cApp_Switch(ByVal StrVal As String, ByRef State As String, ByRef Valid As Boolean) Handles cApp.Switch
        Try
            With cApp
                Select Case StrVal.ToLower
                    Case "config"
                        .RunMode = myRunMode.Config
                    Case "i", "in", "incode"
                        State = "i"
                    Case "o", "out", "outcode"
                        State = "o"
                    Case "d", "db", "database"
                        State = "d"
                    Case "e", "env", "environment"
                        State = "e"
                    Case "u", "user", "username"
                        State = "u"
                    Case "p", "pass", "Password"
                        State = "p"
                    Case "v"
                        debug = True
                    Case Else
                        Valid = False
                End Select
            End With
        Catch ex As Exception
            Console.Write(ex.Message)
        End Try
    End Sub

    Private Sub cApp_SwitchVar(ByVal State As String, ByVal StrVal As String, ByRef Valid As Boolean) Handles cApp.SwitchVar
        Try
            With cApp
                Select Case State
                    Case "i"
                        incode = StrVal
                    Case "o"
                        outcode = StrVal
                    Case "e"
                        environment = StrVal
                    Case "d"
                        Database = StrVal
                    Case "u"
                        username = StrVal
                    Case "p"
                        Password = StrVal
                    Case Else
                        Valid = False
                End Select
            End With
        Catch ex As Exception
            Console.Write(ex.Message)
        End Try
    End Sub

    Sub GetXML()

        Dim doc As New Xml.XmlDocument()
        Try
            If debug Then Console.WriteLine(String.Format("Connecting to {0}.", DataURL))
            doc.Load(DataURL)
        Catch EX As Exception
            Console.WriteLine(String.Format("Error connecting to {0}. {1}", DataURL, EX.Message))
            Exit Sub
        End Try

        Dim pcode As XmlNode = doc.SelectSingleNode("postcode")
        Dim address As XmlNodeList = pcode.SelectNodes("//address")

        Using Connection As New SqlConnection(ConnectionString)
            Try
                If debug Then Console.WriteLine(String.Format("Opening connection to [{0}].", ConnectionString))
                Connection.Open()
            Catch ex As Exception
                Console.WriteLine(String.Format("Error connecting to {0}. {1}", Database, ex.Message))
                Exit Sub
            End Try
            Try
                Dim i As Integer = 0
                Dim ret As SqlCommand = Connection.CreateCommand
                ret.CommandText = String.Format("insert into {0}.dbo.[ZEMG_POSTCODES] ", environment)
                For Each ad As XmlNode In address
                    ret.CommandText += "SELECT "
                    If Not IsNothing(ad.SelectSingleNode("ADDRESS1")) Then
                        ret.CommandText += String.Format("'{0}', ", Replace(ad.SelectSingleNode("ADDRESS1").InnerText, "'", "`"))
                    Else
                        ret.CommandText += String.Format("'{0}', ", "")
                    End If
                    If Not IsNothing(ad.SelectSingleNode("ADDRESS2")) Then
                        ret.CommandText += String.Format("'{0}', ", Replace(ad.SelectSingleNode("ADDRESS2").InnerText, "'", "`"))
                    Else
                        ret.CommandText += String.Format("'{0}', ", "")
                    End If
                    If Not IsNothing(ad.SelectSingleNode("ADDRESS3")) Then
                        ret.CommandText += String.Format("'{0}', ", Replace(ad.SelectSingleNode("ADDRESS3").InnerText, "'", "`"))
                    Else
                        ret.CommandText += String.Format("'{0}', ", "")
                    End If
                    If Not IsNothing(ad.SelectSingleNode("CITY")) Then
                        ret.CommandText += String.Format("'{0}', ", Replace(ad.SelectSingleNode("CITY").InnerText, "'", "`"))
                    Else
                        ret.CommandText += String.Format("'{0}', ", "")
                    End If

                    ret.CommandText += String.Format("'{0}', ", pcode.Attributes("incode").Value)
                    ret.CommandText += String.Format("'{0}', ", pcode.Attributes("outcode").Value)

                    If Not IsNothing(ad.SelectSingleNode("OADDRESS")) Then
                        ret.CommandText += String.Format("'{0}', ", Replace(ad.SelectSingleNode("OADDRESS").InnerText, "'", "`"))
                    Else
                        ret.CommandText += String.Format("'{0}', ", "")
                    End If
                    If Not IsNothing(ad.SelectSingleNode("ORGNAME")) Then
                        ret.CommandText += String.Format("'{0}'", Replace(ad.SelectSingleNode("ORGNAME").InnerText, "'", "`"))
                    Else
                        ret.CommandText += String.Format("'{0}'", "")
                    End If

                    If (i < address.Count - 1) Then ret.CommandText += "UNION ALL "
                    i += 1
                Next
                If debug Then Console.WriteLine(ret.CommandText)
                ret.ExecuteNonQuery()
                
            Catch ex As Exception
                Console.WriteLine(String.Format("SQL Insert failed. {0}", ex.Message))
                Exit Sub
            End Try
        End Using
    End Sub

    Sub doConfig()
        Using Connection As New SqlConnection(ConnectionString)
            Try
                If debug Then Console.WriteLine(String.Format("Opening connection to [{0}].", ConnectionString))
                Connection.Open()
            Catch ex As Exception
                Console.WriteLine(String.Format("Error connecting to {0}. {1}", Database, ex.Message))
                Exit Sub
            End Try
            Dim ret As SqlCommand = Connection.CreateCommand
            Try                
                ret.CommandText = _
                    "EXEC sp_configure 'show advanced options', 1 "
                If debug Then Console.WriteLine(ret.CommandText)
                ret.ExecuteNonQuery()
                ret.CommandText = _
                    "RECONFIGURE "
                If debug Then Console.WriteLine(ret.CommandText)
                ret.ExecuteNonQuery()
                ret.CommandText = _
                    "EXEC sp_configure 'xp_cmdshell', 1 "
                If debug Then Console.WriteLine(ret.CommandText)
                ret.ExecuteNonQuery()
                ret.CommandText = _
                    "RECONFIGURE "
                If debug Then Console.WriteLine(ret.CommandText)
                ret.ExecuteNonQuery()
            Catch ex As Exception
                Console.WriteLine(String.Format("sp_configure failed. {0}", ex.Message))
                'Exit Sub
            End Try

            Try
                ret.CommandText = _
                    "CREATE TRIGGER rmx_insert " & _
                    "   ON  ZEMG_RMX " & _
                    "   AFTER INSERT " & _
                    "AS  " & _
                    "BEGIN " & _
                    "	SET NOCOUNT ON; " & _
                    "	declare @cmd varchar(100) " & _
                    "	declare @db varchar(100) " & _
                    "	declare @i varchar(30) " & _
                    "	declare @o varchar(30) " & _
                    "	set @db = (CAST(SERVERPROPERTY ('servername') AS VARCHAR(100))) " & _
                    "	set @i = (select top 1 I from inserted) " & _
                    "	set @o = (select top 1 O from inserted) " & _
                    "	set @cmd = ('rmxCli.exe /i ' + @i+ ' /o ' + @o + ' /db '+ @db + ' /env system /u " & username & " /p " & Password & "') " & _
                    "	exec xp_cmdshell @cmd     " & _
                    "END "
                If debug Then Console.WriteLine(ret.CommandText)
                ret.ExecuteNonQuery()
            Catch ex As Exception
                Console.WriteLine(String.Format("CREATE TRIGGER failed. {0}", ex.Message))
                Exit Sub
            End Try

        End Using
    End Sub

End Module
