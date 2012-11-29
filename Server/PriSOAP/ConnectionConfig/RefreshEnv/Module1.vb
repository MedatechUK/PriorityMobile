Imports System.Data.SqlClient
Imports System.io
Imports System.Reflection
Imports System.Threading

Module Module1

    Private Enum helpFile
        none
        syntax
    End Enum

    Private ini As Priority.tabulaini
    Dim ev As New ntEvtlog.evt
    Dim quit As Boolean = False

#Region "Public properties"

    Private _BasePath As String = Nothing
    Public ReadOnly Property BasePath() As String
        Get
            If IsNothing(_BasePath) Then
                Dim fullPath As String = System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase
                If InStr(fullPath, "file:///", CompareMethod.Text) > 0 Then
                    fullPath = Replace(fullPath, "file:///", "")
                End If
                If InStr(fullPath, "/", CompareMethod.Text) > 0 Then
                    fullPath = Replace(fullPath, "/", "\")
                End If
                _BasePath = fullPath.Substring(0, fullPath.LastIndexOf("\"))
                If Strings.Right(_BasePath, 1) <> "\" Then _BasePath += "\"

            End If
            Return _BasePath
        End Get
    End Property

    Private _Username As String = "tabula"
    Public Property Username()
        Get
            Return _Username
        End Get
        Set(ByVal value)
            _Username = value
        End Set
    End Property

    Private _Password As String = ""
    Public Property Password()
        Get
            Return _Password
        End Get
        Set(ByVal value)
            _Password = value
        End Set
    End Property

    Private _InetpubDir As String = ""
    Public Property InetpubDir() As String
        Get
            Return _InetpubDir
        End Get
        Set(ByVal value As String)
            _InetpubDir = value
        End Set
    End Property

    Private _DataSource As String
    Public Property DataSource() As String
        Get
            Return _DataSource
        End Get
        Set(ByVal value As String)
            _DataSource = value
        End Set
    End Property

    Private _Connstr As String
    Public ReadOnly Property ConnectionString() As String
        Get
            Return String.Format( _
                "Data Source={0};Initial Catalog=system;User ID={1};Password={2}", _
                DataSource, Username, Password _
            )
        End Get
    End Property

    Private _AppName As String = Nothing
    Public Property AppName() As String
        Get
            Return _AppName
        End Get
        Set(ByVal value As String)
            _AppName = value
        End Set
    End Property

    Private _EnvSQL As String = Nothing
    Public ReadOnly Property EnvSQL() As String
        Get
            Return Strings.Format("select DNAME from system.dbo.ENVIRONMENT where DNAME <> ''", "")
        End Get
    End Property

    Private _Wait As Boolean = False
    Public Property Wait() As Boolean
        Get
            Return _Wait
        End Get
        Set(ByVal value As Boolean)
            _Wait = value
        End Set
    End Property

#End Region

    Sub Main()

        Try
            doWelcome()
            With ev
                .AppName = "PriPROC"
                .LogMode = ntEvtlog.EvtLogMode.EventLog
                .LogVerbosity = ntEvtlog.EvtLogVerbosity.Normal
            End With
            GetArgs(Command)
            If Not quit Then MakeConfig()

            If Wait Then
                Console.WriteLine("")
                Console.WriteLine("Press any key to continue.")
                Dim strInput As String = Console.ReadKey(False).ToString
                While (strInput = "")
                    Thread.Sleep(100)
                End While
            End If

        Catch ex As Exception
            SafeLog( _
                String.Format("{0}", ex.Message), _
                EventLogEntryType.Error, _
                ntEvtlog.EvtLogVerbosity.Normal _
            )
        End Try

    End Sub

#Region "Parse Arguments"

    Private Sub GetArgs(ByVal Command As String)

        Dim hasuser As Boolean = False
        Dim haspass As Boolean = False
        Dim haspath As Boolean = False

        Dim h As helpFile = helpFile.none
        Dim exStr As String = Nothing
        Dim state As String = ""
        Dim registering As Boolean = False
        If Command.Length = 0 Then Command = "/?"
        For Each arg As String In MakeArgs(Command)
            Select Case Left(arg, 1)
                Case "/", "-"
                    Select Case LCase(Right(arg, arg.Length - 1))
                        Case "u", "user", "username"
                            state = "u"
                        Case "p", "pass", "pwd", "password"
                            state = "p"
                        Case "i", "inet", "inetpub", "inetpubdir"
                            state = "i"
                        Case "w", "wait"
                            state = ""
                            Wait = True
                        Case "h", "help", "?"
                            state = "h"
                            h = helpFile.syntax
                            quit = True
                        Case Else
                            exStr = String.Format("Unknown argument: {0}. Please seek /help.", arg)
                    End Select

                Case Else
                    Select Case state
                        Case "u"
                            Username = arg
                            hasuser = True
                        Case "p"
                            Password = arg
                            haspass = True
                        Case "i"

                            InetpubDir = arg
                            haspath = True
                        Case "h", "help", "?"
                            Select Case LCase(arg)
                                Case "syntax"
                                    h = helpFile.syntax
                                Case Else
                                    exStr = "Invalid syntax. Please seek /help."
                            End Select
                        Case Else
                            exStr = "Invalid syntax. Please seek /help."
                    End Select
            End Select
        Next

        If Not h = helpFile.none Then
            doHelp(h)
        Else
            If Not hasuser And haspass And haspath Then
                exStr = "Username or password not provided. Please seek /help."
            End If
        End If

        If Not IsNothing(exStr) Then
            quit = True
            Throw New Exception(exStr)
        End If

    End Sub

    Private Function MakeArgs(ByVal Value As String) As String()
        Dim ret() As String = Nothing
        Dim sp() As String = Split(Value, ChrW(34))
        For i As Integer = 0 To UBound(sp)
            If EvenNumber(i + 1) Then
                NewArg(ret, sp(i))
            Else
                Dim tmp() As String = Split(sp(i), ChrW(32))
                For Each str As String In tmp
                    NewArg(ret, str)
                Next
            End If
        Next
        Return ret
    End Function

    Private Function EvenNumber(ByVal Value As Integer) As Boolean
        Return Value Mod 2 = 0
    End Function

    Private Sub NewArg(ByRef ArgArray() As String, ByVal NewValue As String)
        If NewValue.Length > 0 Then
            Try
                If Not IsNothing(ArgArray) Then
                    ReDim Preserve ArgArray(UBound(ArgArray) + 1)
                Else
                    ReDim ArgArray(0)
                End If
            Catch ex As Exception
                ReDim ArgArray(0)
            Finally
                ArgArray(UBound(ArgArray)) = NewValue
            End Try
        End If
    End Sub

#End Region

#Region "Console Messages"

    Sub doWelcome()
        With Assembly.GetExecutingAssembly().GetName()
            AppName = .Name
            Console.WriteLine("")
            Console.WriteLine(String.Format("(c){0} eMerge I.T.", Year(Now).ToString))
            Console.WriteLine(String.Format("{0}", AppName))
            Console.WriteLine("By Simon Barnett.")
            Console.WriteLine(String.Format("Build: {0}.{1}.{2}.{3}", _
                .Version.Major, _
                .Version.Minor, _
                .Version.Build, _
                .Version.Revision))
            Console.WriteLine("")
        End With
    End Sub

    Private Sub doHelp(ByVal h As helpFile)

        Dim fn As String = ""

        Select Case h
            Case helpFile.syntax
                fn = "RefreshEnv.txt"

        End Select

        If Not File.Exists(BasePath & "help\" & fn) Then
            Console.WriteLine( _
                String.Format( _
                    "Help file [help\{0}] was not found.", _
                    fn _
                ) _
            )
        Else
            Using sr As New StreamReader(BasePath & "help\" & fn)
                For Each str As String In Split(sr.ReadToEnd, vbCrLf)
                    Console.WriteLine(str)
                Next
            End Using
        End If

    End Sub

#End Region

#Region "Private Subs"

    Private Sub SafeLog(ByVal Entry As String, ByVal EventType As EventLogEntryType, ByVal Verbosity As ntEvtlog.EvtLogVerbosity)
        Try
            ev.Log( _
                Entry, _
                EventType, _
                Verbosity _
              )
        Catch exep As Exception
            Console.WriteLine( _
                "Failed to write to the [{0} {1}] log.{4}" & _
                "The error reported was: {4}{2}{4}" & _
                "The event could not be written to the log because: {4}{3}{4}", _
                ev.LogName, ev.AppName, Entry, exep.Message, vbCrLf _
            )
        End Try
    End Sub

    Private Sub MakeConfig()

        Dim command As SqlCommand = Nothing
        Dim env() As String = Nothing
        Dim dataReader As SqlDataReader

        ini = New Priority.tabulaini(ev)
        If Not (ini.Loaded) Then
            Throw New Exception(String.Format("Could not open tabula.ini. Please check your Priority installation.", ""))
        End If

        DataSource = ini.iniValue("Environment", "Tabula Host")

        Dim Connection As New SqlConnection(ConnectionString)

        Try
            Connection.Open()
        Catch
            Throw New Exception(String.Format("Could not open connection to [{0}].", DataSource))
        End Try
        command = Connection.CreateCommand()

        command.CommandText = EnvSQL
        Try
            dataReader = command.ExecuteReader()
        Catch ex As Exception
            Connection.Close()
            Throw New Exception(String.Format("Could not get environment data from [{0}]. The error was [{1}].", DataSource, ex.Message))
        End Try

        If Not dataReader.HasRows Then
            Connection.Close()
            Throw New Exception(String.Format("No Priority Environments are listed from [{0}].", DataSource))
        End If

        While dataReader.Read
            Try
                ReDim Preserve env(UBound(env) + 1)
            Catch
                ReDim env(0)
            Finally
                env(UBound(env)) = dataReader.Item(0)
            End Try

        End While
        Connection.Close()

        Dim str As String = ""
        Dim strLeft As String = ""
        Dim strRight As String = ""

        Using sr As New StreamReader(InetpubDir)
            With sr
                str = .ReadToEnd
            End With
        End Using
        If Not (InStr(str, "<connectionStrings>", CompareMethod.Text) > 0 And InStr(str, "</connectionStrings>", CompareMethod.Text) > 0) Then
            Throw New Exception(String.Format("Invalid Configuration file [{0}].", InetpubDir))
        End If

        strLeft = Split(str, "<connectionStrings>")(0)
        strRight = Split(str, "</connectionStrings>")(1)

        Using sw As New StreamWriter(InetpubDir)
            With sw
                .WriteLine(strLeft)
                .WriteLine("<connectionStrings>")
                For i As Integer = 0 To UBound(env)
                    .WriteLine(String.Format( _
                        "<add name={0}{2}{0} connectionString={0}Data Source={1};Initial Catalog={2};User ID={3};Password={4}{0} providerName={0}System.Data.SqlClient{0}/>", _
                        Chr(34), DataSource, env(i), Username, Password))
                Next
                .WriteLine("</connectionStrings>")
                .WriteLine(strRight)
            End With
        End Using


    End Sub

#End Region

End Module
