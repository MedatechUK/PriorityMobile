Imports system.data
Imports System.Data.SqlClient
Imports System.Reflection
Imports System.IO
Imports System.Threading

Module ScreenPop

    Enum RunMode
        config
        screenpop
        SearchForms
        help
    End Enum

    Private p As New Phone
    Private mode As RunMode = RunMode.screenpop

    Sub Main()

        GetArgs(Command)

        Select Case mode
            Case RunMode.config
                Dim result As MsgBoxResult
                Dim settings As New Settings
                Do
                    With settings
                        .BringToFront()
                        .ShowDialog()
                    End With
                    If My.Settings.Changed Then
                        result = MsgBox("You have changed some settings." & vbCrLf & _
                        "Do you wish to test Database connection?", _
                         MsgBoxStyle.OkCancel)
                    End If
                Loop Until _
                    Not (My.Settings.Changed) Or _
                    result = MsgBoxResult.Cancel Or _
                    IsNothing(TestException)

            Case RunMode.screenpop
                Dim form As String = My.Settings.DEFAULTFORM
                Dim id As String = ""
                Dim index As String = ""
                If Not IsNothing(PHONENUM) Then
                    FindPhoneNum(form, id)
                End If
                RunCommand(form, id)

            Case RunMode.SearchForms
                Dim sf As New SearchForms
                p.BindDataGrid(sf.DataGridView)
                sf.DataGridView.DataSource = p
                sf.DataGridView.Columns(4).Visible = False
                sf.ShowDialog()

            Case Else
                MsgBox( _
                    String.Format( _
                        "(c)2010 eMerge-IT{0}{1}{0}By Simon Barnett{0}{2}", _
                        vbCrLf, _
                        StartMessage, _
                        Usage _
                    ) _
                , MsgBoxStyle.Information + MsgBoxStyle.OkOnly)

        End Select


    End Sub

#Region "public properties"

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

    Private _CONFIG As Boolean = False
    Public Property CONFIG() As Boolean
        Get
            Return _CONFIG
        End Get
        Set(ByVal value As Boolean)
            _CONFIG = value
        End Set
    End Property

    Private _PHONENUM As String = Nothing
    Public Property PHONENUM() As String
        Get
            Return _PHONENUM
        End Get
        Set(ByVal value As String)
            Dim tmp As String = ""
            For i As Integer = 1 To value.Length
                Try
                    Select Case CInt(Strings.Mid(value, i, 1))
                        Case 0, 1, 2, 3, 4, 5, 6, 7, 8, 9
                            tmp += Strings.Mid(value, i, 1)
                    End Select
                Catch
                End Try
            Next
            _PHONENUM = tmp
        End Set
    End Property

    Public ReadOnly Property ConStr() As String
        Get
            Return String.Format( _
                    "Data Source={0};Initial Catalog={1};User ID={2};Password={3};", _
                    My.Settings.DATASOURCE, _
                    My.Settings.INITIALCATALOG, _
                    My.Settings.USERID, _
                    My.Settings.PASSWORD _
                )
        End Get
    End Property

    Public ReadOnly Property StartMessage()
        Get
            With Assembly.GetExecutingAssembly().GetName()
                Return String.Format("{0}: Build: {1}.{2}.{3}.{4}", _
                    .Name, _
                    .Version.Major, _
                    .Version.Minor, _
                    .Version.Build, _
                    .Version.Revision _
                )
            End With
        End Get
    End Property

    Public ReadOnly Property Usage() As String
        Get
            Dim ret As String = ""
            Dim fn As String = "syntax.txt"
            If Not File.Exists(BasePath & fn) Then
                Console.WriteLine( _
                    String.Format( _
                        "Help file [\{0}] was not found.", _
                        fn _
                    ) _
                )
            Else
                Using sr As New StreamReader(BasePath & fn)
                    For Each str As String In Split(sr.ReadToEnd, vbCrLf)
                        ret += str & vbCrLf
                    Next
                End Using
            End If

            Return ret
        End Get
    End Property

#End Region

#Region "Parse Arguments"

    Private Sub GetArgs(ByVal Command As String)

        Dim state As String = ""

        Try
            For Each arg As String In MakeArgs(Command)
                Select Case Left(arg, 1)
                    Case "/", "-"
                        Select Case LCase(Right(arg, arg.Length - 1))
                            Case "clsid", "phone"
                                state = LCase(Right(arg, arg.Length - 1))
                            Case "config"
                                state = ""
                                mode = RunMode.config
                            Case "f", "forms"
                                state = ""
                                mode = RunMode.SearchForms
                            Case "?", "h", "help"
                                state = ""
                                mode = RunMode.help
                            Case Else
                                Throw New Exception(String.Format("Unknown argument: {0}.", arg))
                        End Select
                    Case Else
                        Select Case state
                            Case "clsid", "phone"
                                PHONENUM = arg
                            Case Else
                                Throw New Exception(String.Format("Invalid syntax: {0}.", arg))
                        End Select
                End Select
            Next
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical)
            End
        End Try
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

    Private Sub FindPhoneNum(ByRef Form As String, ByRef ID As String)

        Using connection As New SqlConnection
            connection.ConnectionString = ConStr
            connection.Open()

            Dim command As New SqlCommand
            command = connection.CreateCommand()

            For Each key As String In p.Keys

                command.CommandText = _
                    String.Format( _
                        "select {0} from {1} where replace({2},' ','') like '%{3}%'", _
                        p(key).FORMID, _
                        p(key).TABLE, _
                        p(key).PHONECOL, _
                        PHONENUM _
                    )
                Console.WriteLine(command.CommandText)

                Dim datareader As SqlDataReader = command.ExecuteReader
                With datareader
                    If .HasRows Then
                        .Read()
                        Form = p(key).FORMNAME

                        ID = CStr(Trim(datareader.Item(0)))
                        Exit For
                    End If
                    .Close()
                End With                

            Next
        End Using
    End Sub

    Private Function TestException() As Exception
        Dim ret As Exception = Nothing        
        Try
            Using connection As New SqlConnection
                Dim cn As String = ConStr
                connection.ConnectionString = cn
                connection.Open()
                Dim command As SqlCommand = connection.CreateCommand()
                command.CommandText = "select 'test'"
                Dim result As String = command.ExecuteScalar
            End Using
            MsgBox("Connected ok!", MsgBoxStyle.Information + MsgBoxStyle.OkOnly)
        Catch ex As Exception
            ret = ex
            MsgBox(ex.Message, MsgBoxStyle.Critical + MsgBoxStyle.OkOnly)
        End Try
        Return ret
    End Function

    Private Sub RunCommand(ByVal Form As String, ByVal ID As String)

        Dim sOutput As String = ""
        Dim sErrs As String = ""
        Dim myProcess As Process = New Process()
        Dim cmd As String = ""

        Try
            With myProcess
                With .StartInfo
                    .FileName = "cmd.exe"
                    .UseShellExecute = False
                    .CreateNoWindow = True
                    .RedirectStandardInput = True
                    .RedirectStandardOutput = True
                    .RedirectStandardError = True
                End With
                .Start()

                Dim sIn As StreamWriter = myProcess.StandardInput
                Dim sOut As StreamReader = myProcess.StandardOutput
                Dim sErr As StreamReader = myProcess.StandardError

                With sIn
                    cmd = String.Format( _
                        "{0}{1}\bin.95\WINRUN.exe{0} {0}{0} {3} {4} {0}{1}\system\prep{0} {5} WINFORM {2} {0}{0} {0}{6}{0} {0}{0} 2", _
                        Chr(34), _
                        My.Settings.PRIORITYPATH, _
                        Form, _
                        My.Settings.PRIUSER, _
                        My.Settings.PRIPASSWORD, _
                        My.Settings.PRICOMPANY, _
                        ID _
                    )
                    cmd = Replace(cmd, "\\", "\")
                    .AutoFlush = True
                    .Write(cmd & _
                        System.Environment.NewLine)
                    .Write("exit" & _
                        System.Environment.NewLine)
                    .Close()

                End With

                Dim l As Integer = 0
                Do Until l = 100
                    If sOut.Peek <> 0 Then
                        sOutput = sOutput + sOut.ReadLine
                    End If
                    l = l + 1
                    Thread.Sleep(1)
                Loop

                If Len(sOutput) > 0 Then
                    sOutput = sOut.ReadToEnd
                    sOut.Close()
                    sErrs = sErr.ReadToEnd()
                    sErr.Close()
                End If

                If Not myProcess.HasExited Then
                    myProcess.Kill()
                End If

                .Close()

                If sErrs.Length > 0 Then
                    MsgBox(sErrs & cmd)
                End If

            End With
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

End Module
