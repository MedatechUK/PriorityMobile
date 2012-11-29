Imports System.Reflection
Imports System.IO
Imports System.Threading

Module Register

#Region "Private Properties"

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

    Private _AppName As String = Nothing
    Public Property AppName() As String
        Get
            Return _AppName
        End Get
        Set(ByVal value As String)
            _AppName = value
        End Set
    End Property

    Private _RegisterName As String = Nothing
    Public Property RegisterName() As String
        Get
            Return _RegisterName
        End Get
        Set(ByVal value As String)
            _RegisterName = value
        End Set
    End Property

#End Region

    Sub Main()
        Try
            doWelcome()
            GetArgs(Command)
            If Not IsNothing(RegisterName) Then
                Dim ev As New ntEvtlog.evt
                With ev
                    .AppName = RegisterName
                    .LogVerbosity = ntEvtlog.EvtLogVerbosity.Arcane
                    .RegisterLog()
                End With
            End If
        Catch ex As Exception
            Console.WriteLine(ex.Message)
        End Try

        Console.WriteLine("")
        Console.WriteLine("Press any key to continue.")
        Dim strInput As String = Console.ReadKey(False).ToString
        While (strInput = "")
            Thread.Sleep(100)
        End While

    End Sub

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

    Private Sub doHelp()

        Dim fn As String = "register.txt"

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

#Region "Parse Arguments"

    Private Sub GetArgs(ByVal Command As String)

        Dim exStr As String = Nothing
        Dim state As String = ""
        If Command.Length = 0 Then Command = "/?"
        For Each arg As String In MakeArgs(Command)
            Select Case Left(arg, 1)
                Case "/", "-"
                    Select Case LCase(Right(arg, arg.Length - 1))
                        Case "r", "register"
                            state = "r"
                        Case "h", "help", "?"
                            state = LCase(Right(arg, arg.Length - 1))
                            doHelp()
                        Case Else
                            exStr = String.Format("Unknown argument: {0}. Please seek /help.", arg)
                    End Select
                Case Else
                    Select Case state
                        Case "r"
                            RegisterName = arg
                        Case Else
                            exStr = "Invalid syntax. Please seek /help."
                    End Select
            End Select
        Next

        If Not IsNothing(exStr) Then
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

End Module
