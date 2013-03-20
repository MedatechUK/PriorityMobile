Imports System.IO
Imports System.Management
Imports System.Threading
Imports System.Reflection
Imports System.Net
Imports System.Text
Imports System.Windows.Forms


Module Module1

    Private ReadOnly Property WorkingDirectory() As String
        Get
            Return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)
        End Get
    End Property

    Private Property HostName() As String
        Get
            Dim hn As String = ""
            If System.IO.File.Exists(WorkingDirectory + "\hostname.txt") Then
                Using sr As New StreamReader(WorkingDirectory + "\hostname.txt")
                    hn = sr.ReadToEnd
                End Using
                Return hn
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal value As String)
            While System.IO.File.Exists(WorkingDirectory + "\hostname.txt")
                System.IO.File.Delete(WorkingDirectory + "\hostname.txt")
            End While
            Using sw As New StreamWriter(WorkingDirectory + "\hostname.txt")
                sw.Write(value)
            End Using
        End Set
    End Property

    Enum myRunMode As Integer
        file = 0
        url = 1
        Config = 2
    End Enum

    Dim FILE As String = Nothing
    Dim URL As String = Nothing
    Dim PRN As String = Nothing
    Dim isConsole As Boolean = True

    Dim WithEvents cApp As New ConsoleApp.CA

    Public Sub Main()

        'MsgBox(Environment.CommandLine)

        With cApp
            .RunMode = myRunMode.file
            .doWelcome(Assembly.GetExecutingAssembly())
            Try
                .GetArgs(Command)
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try

            If Not .Quit Then
                If IsNothing(FILE) And IsNothing(URL) Then
                    Display("Neither a file nor a URL was specified. Please seek /help.")
                    .Quit = True
                ElseIf Not (IsNothing(FILE)) And Not (IsNothing(URL)) Then
                    Display("Both a file AND a URL was specified. Please seek /help.")
                    .Quit = True
                End If

                If Not .Quit Then
                    Select Case .RunMode
                        Case myRunMode.file
                            If GetPrinter() Then
                                Print()
                            End If
                        Case myRunMode.url
                            If InStr(URL, "/") = 0 Then
                                Dim hn As String = HostName
                                If Not IsNothing(hn) Then
                                    URL = hn & "/" & URL
                                Else
                                    Display("Missing Hostname. Please seek /help.")
                                    .Quit = True
                                End If
                            End If
                            If Not .Quit Then
                                If download() Then
                                    If GetPrinter() Then
                                        Print()
                                    End If
                                End If
                            End If
                        Case myRunMode.Config
                            HostName = URL
                            Display(String.Format("The default hostname has been set to [{0}]{1}You can now ommit the hostname from the URL parameter e.g. /u file.pdf", URL, vbCrLf))
                    End Select
                End If
            End If

            cApp.Finalize()

        End With
    End Sub

    Private Sub cApp_Switch(ByVal StrVal As String, ByRef State As String, ByRef Valid As Boolean) Handles cApp.Switch
        Try
            With cApp
                Select Case StrVal
                    Case "runmode"
                        State = "rm"
                    Case "config"
                        .RunMode = myRunMode.Config
                        State = Nothing
                    Case "p", "prn", "printer"
                        State = "p"
                    Case "f", "file"
                        State = "f"
                    Case "u", "url"
                        State = "u"
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
                    Case "rm"
                        If StrVal.ToLower = "form" Then
                            isConsole = False
                        Else
                            isConsole = True
                        End If
                    Case "p"
                        PRN = StrVal
                    Case "f"
                        .RunMode = myRunMode.file
                        FILE = StrVal
                    Case "u"
                        If Not .RunMode = myRunMode.Config Then .RunMode = myRunMode.url
                        URL = StrVal
                    Case Else
                        Valid = False
                End Select
            End With
        Catch ex As Exception
            Console.Write(ex.Message)
        End Try
    End Sub

    Function GetPrinter() As Boolean

        Dim ret As Boolean = False
        Dim printerQuery As ManagementObjectSearcher
        Dim queryResults As Management.ManagementObjectCollection
        Dim onePrinter As Management.ManagementObject

        printerQuery = New Management.ManagementObjectSearcher("SELECT * FROM Win32_Printer")
        queryResults = printerQuery.Get()

        For Each onePrinter In queryResults
            If onePrinter!default And IsNothing(PRN) Then
                PRN = onePrinter!name
                ret = True
                Exit For
            ElseIf (Not IsNothing(PRN)) And (String.Compare(PRN, onePrinter!name, True) = 0) Then
                PRN = onePrinter!name
                ret = True
                Exit For
            End If
        Next

        If Not ret Then
            If IsNothing(PRN) Then
                Display("There is no default printer specified on this machine.")
            Else
                Display(String.Format("Invalid printer specification: {0}", PRN))
            End If
        Else
            Console.WriteLine(String.Format("Using Printer: {0}", PRN))
        End If

        Return ret

    End Function

    Function download() As Boolean
        Try
            Dim wc As New System.Net.WebClient
            Select Case URL.Split(".").Last.ToUpper
                Case "PDF"
                    FILE = String.Format( _
                        "{0}\{1}.pdf", _
                        Environment.GetFolderPath(Environment.SpecialFolder.InternetCache), _
                        Guid.NewGuid.ToString _
                    )
                Case "HTML", "HTM"
                    FILE = String.Format( _
                        "{0}\{1}.html", _
                        Environment.GetFolderPath(Environment.SpecialFolder.InternetCache), _
                        Guid.NewGuid.ToString _
                    )
            End Select

            Console.WriteLine(String.Format("Saving file from [{0}] to [{1}].", URL, FILE))
            wc.DownloadFile(URL, FILE)
            Return True

        Catch ex As Exception
            Display(String.Format("Could not download file at URL: {0}{2}{1}", URL, ex.Message, vbCrLf))
            Return False
        End Try
    End Function

    Sub Print()

        If Right(FILE, 3).ToUpper = "HTM" Or Right(FILE, 4).ToUpper = "HTML" Then
            Dim WB As New WebBrowser
            WB.Navigate(FILE)
            For i As Integer = 0 To 500
                Application.DoEvents()
                Threading.Thread.Sleep(10)
            Next
            WB.Print()
            For i As Integer = 0 To 1500
                Application.DoEvents()
                Threading.Thread.Sleep(10)
            Next
        Else

            Dim ps() As Process = Process.GetProcesses()
            Dim p As Process
            Dim acrorun As Boolean = False
            Dim resp As MsgBoxResult = Nothing
            Do
                acrorun = False
                For Each p In ps
                    If p.ProcessName.ToLower = "acrord32" Then
                        If Not p.HasExited Then
                            acrorun = True
                            Exit For
                        End If
                    End If
                Next
                If acrorun And isConsole Then
                    resp = MsgBox("Another instance of Acrobat Reader is already running. Please close any other open readers and press retry.", MsgBoxStyle.Exclamation + MsgBoxStyle.RetryCancel, "Warning!")
                Else
                    Thread.Sleep(5000)
                End If
            Loop Until acrorun = False Or resp = MsgBoxResult.Cancel

        If resp = MsgBoxResult.Cancel Then
            Console.WriteLine("Acrobat already running. User cancelled.")
            Exit Sub
        End If

        Using myProcess As System.Diagnostics.Process = New System.Diagnostics.Process()
            With myProcess
                With .StartInfo
                    .FileName = FILE 'CREATE THIS FILE WITH FILESHAREMODE.NONE 
                    .WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden
                    .CreateNoWindow = True
                    .Verb = "print"
                    .Arguments = PRN
                    .UseShellExecute = True
                End With
                Dim Print_Check_Counter As Integer = 0
                Try
                    .Start() 'GIVE THE PROCESS 2 SECOND TO START 
                    .WaitForExit(2000)
CLOSE:
                    Print_Check_Counter = Print_Check_Counter + 1
                    If Print_Check_Counter > 30 Then
                        'MsgBox.Show("Problem Printing this document: " & paginatedPDF, "Process Error")
                        Exit Sub
                    End If
                    'FILE WAS CREATED WITH FILESHAREMODE.NONE SO IF IT IS STILL BEING USED 'BY ACROBAT OR READER, THEN CATCH AN IO EXCEPTION 
                    Dim check As New FileStream(FILE, FileMode.Open)
                    .CloseMainWindow()
                    check.Close()
                Catch io As IOException 'IF IOEXCEPTION WAS CAUGHT THEN SLEEP AND GO BACK TO CLOSE 
                    Thread.Sleep(2000)
                    GoTo close
                Catch ex As Exception

                End Try
            End With
        End Using
        End If
    End Sub

    Private Sub Display(ByVal DisplayStr As String)
        Select Case isConsole
            Case True
                Console.WriteLine(DisplayStr)
            Case Else
                MsgBox(DisplayStr, , "PDF Print")
        End Select
    End Sub

End Module
