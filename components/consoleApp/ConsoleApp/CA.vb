Imports System.Reflection
Imports System.Threading
Imports System.io

Public Class CA

    Public Event Switch(ByVal StrVal As String, ByRef State As String, ByRef Valid As Boolean)
    Public Event SwitchVar(ByVal State As String, ByVal StrVal As String, ByRef Valid As Boolean)


#Region "Emumerations"

    Private Enum helpFile
        none
        syntax
        file
    End Enum

#End Region

    Public Shadows Sub Finalize()
        If Wait Then
            Console.WriteLine("")
            Console.WriteLine("Press any key to continue.")
            Dim strInput As String = Console.ReadKey(False).ToString
            While (strInput = "")
                Thread.Sleep(100)
            End While
        End If
    End Sub

#Region "Public Properties"

    Private _wait As Boolean
    Private Property Wait() As Boolean
        Get
            Return _Wait
        End Get
        Set(ByVal value As Boolean)
            _Wait = value
        End Set
    End Property

    Private _quit As Boolean = False
    Public Property Quit() As Boolean
        Get
            Return _quit
        End Get
        Set(ByVal value As Boolean)
            _quit = value
        End Set
    End Property

    Private _AppName As String = Nothing
    Public ReadOnly Property AppName() As String
        Get
            Return _AppName
        End Get
    End Property

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

    Private _RunMode As Integer
    Public Property RunMode() As Integer
        Get
            Return _RunMode
        End Get
        Set(ByVal value As Integer)
            _RunMode = value
        End Set
    End Property

    Private _HelpFileName As String
    Private Property HelpFileName() As String
        Get
            Return _HelpFileName
        End Get
        Set(ByVal value As String)
            _HelpFileName = value
        End Set
    End Property

    Private _StrDeliminator As String = ChrW(34)
    Public Property StrDeliminator() As String
        Get
            Return _StrDeliminator
        End Get
        Set(ByVal value As String)
            _StrDeliminator = value
        End Set
    End Property

#End Region

#Region "Parse Arguments"

    Public Sub GetArgs(ByVal Command As String)

        Dim h As helpFile = helpFile.none
        Dim exStr As String = Nothing
        Dim state As String = ""
        Dim Valid As Boolean

        If Command.Length = 0 Then Command = "/?"
        For Each arg As String In MakeArgs(Command)
            arg = Trim(arg)
            Select Case Left(arg, 1)
                Case "/", "-"
                    Select Case LCase(Right(arg, arg.Length - 1))
                        Case "?", "help"
                            state = "?"
                            h = helpFile.syntax
                        Case "w", "wait"
                            state = Nothing
                            Wait = True
                        Case "debug"
                            Console.WriteLine("Waiting for debug process to attach...")
                            Console.WriteLine("Press any key to continue.")
                            Dim strInput As String = Console.ReadKey(False).ToString
                            While (strInput = "")
                                Thread.Sleep(100)
                            End While
                        Case Else
                            Valid = True
                            RaiseEvent Switch(LCase(Right(arg, arg.Length - 1)), state, Valid)
                            If Not Valid Then
                                exStr = String.Format("Unknown argument: {0}. Please seek /help.", arg)
                            End If
                    End Select
                Case Else
                    Select Case state
                        Case "?"
                            If Not File.Exists( _
                                String.Format( _
                                    "{0}help\{1}.txt", _
                                    BasePath, arg _
                                    ) _
                                ) _
                            Then
                                exStr = String.Format("Invalid help file: help\{1}.", arg)
                            Else
                                h = helpFile.file
                                HelpFileName = arg
                            End If
                        Case Else
                            Valid = True
                            RaiseEvent SwitchVar(state, arg, Valid)
                            If Not Valid Then
                                exStr = "Invalid syntax. Please seek /help."
                            End If
                    End Select
            End Select
        Next

        If Not h = helpFile.none Then
            doHelp(h)
            Quit = True
        End If

        If Not IsNothing(exStr) Then
            Throw New Exception(exStr)
        End If

    End Sub

    Private Function MakeArgs(ByVal Value As String) As List(Of String)
        Dim ret As New List(Of String)
        While Value.StartsWith(Chr(34)) Or Value.StartsWith(Chr(32))
            Value = Right(Value, Value.Length - 1)
        End While
        While Value.EndsWith(Chr(34)) Or Value.EndsWith(Chr(32))
            Value = Left(Value, Value.Length - 1)
        End While
        Dim i As Integer = 0
        For Each quot As String In Value.Split(Chr(34))
            Select Case i Mod 2
                Case 0
                    For Each sp As String In quot.Split(Chr(32))
                        If Trim(sp).Length > 0 Then
                            ret.Add(Trim(sp))
                        End If
                    Next
                Case Else
                    ret.Add(quot)
            End Select
            i += 1
        Next

        'Dim ret() As String = Nothing
        'Dim sp() As String = Split(Value, StrDeliminator)
        'For i As Integer = 0 To UBound(sp)
        '    If EvenNumber(i + 1) Then
        '        NewArg(ret, sp(i))
        '    Else
        '        Dim tmp() As String = Split(sp(i), ChrW(32))
        '        For Each str As String In tmp
        '            NewArg(ret, str)
        '        Next
        '    End If
        'Next
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

    Public Sub doWelcome(ByVal Assemb As System.Reflection.Assembly)
        With Assemb.GetName()
            _AppName = .Name
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
                fn = "syntax.txt"
            Case helpFile.file
                fn = _HelpFileName & ".txt"
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


End Class
