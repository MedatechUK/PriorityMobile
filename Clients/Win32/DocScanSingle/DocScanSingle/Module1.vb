Imports System.IO
Imports System.Reflection
Imports System.Windows.Forms
Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Sql
Imports System.Configuration
Imports System.Configuration.ConfigurationSettings

Module Module1
    Dim WithEvents cApp As New ConsoleApp.CA
    Enum myRunMode As Integer
        file = 0
        url = 1
        Config = 2
        enviro = 3
    End Enum
    Dim FILENAME As String = Nothing
    Sub Main()
        'ScanControl.write_error("its dead jim testy testy test", 1)
        '/e "010123 \\hannibal\emerge\test testtest ttyyttyy"
        Try
            With cApp

                '.RunMode = myRunMode.file

                Try
                    .GetArgs(Command)
                    Select Case .RunMode
                        Case myRunMode.Config
                            Select Case FILENAME
                                Case "Settings"
                                    Dim g As New frmSettings
                                    g.ShowDialog()
                            End Select

                            .Quit = True
                        Case myRunMode.file
                            .doWelcome(Assembly.GetExecutingAssembly())
                    End Select
                Catch ex As Exception
                    Console.WriteLine(ex.Message)
                    .Quit = True
                End Try

                If Not .Quit Then
                    Dim yyyy, mm, dd As Integer
                    yyyy = DatePart(DateInterval.Year, Today)
                    mm = DatePart(DateInterval.Month, Today)
                    dd = DatePart(DateInterval.Day, Today)
                    Dim LineOfData() As String = FILENAME.Split(" ")
                    Dim sd As ScanControl.ScanDocument
                    'ScanControl.ChecktConnectionString(LineOfData(0))
                    Dim sdate, edate As DateTime
                    sdate = Now
                    edate = FormatDateTime("1/1/1988", DateFormat.GeneralDate)
                    Try
                        sd = New ScanControl.ScanDocument(LineOfData(3), LineOfData(4), DateDiff(DateInterval.Minute, edate, sdate), LineOfData(2), LineOfData(1))

                    Catch ex As Exception
                        MsgBox(ex.ToString)
                    End Try
                    Dim fname As String
                    Dim extra As String = ""
                    Select Case LineOfData.Length
                        Case 5

                        Case Else
                            extra = LineOfData(5)
                            sd.doc_dir = sd.doc_dir.Remove(0, 6)
                            sd.doc_dir = extra + sd.doc_dir

                    End Select



                    FILENAME = sd.doc_dir & "\" & sd.file_name & yyyy & mm & dd & ".pdf"

                    fname = sd.file_name & yyyy & mm & dd & ".pdf"
                    If LineOfData(1) = "" Then
                        Console.WriteLine("No file name provided!")
                        .Quit = True
                    End If

                    If File.Exists(FILENAME) = True Then
                        'Console.WriteLine("This filename already exists please check the name provided")
                        FILENAME = sd.doc_dir & "\" & sd.file_name & yyyy & mm & dd & "_" & 1 & ".pdf"
                        fname = sd.file_name & yyyy & mm & dd & "_" & 1 & ".pdf"
                        Dim x As Integer = 2
                        Do While File.Exists(FILENAME) = True
                            'This will keep adding to the ending number until it gets to one that it can use
                            FILENAME = sd.doc_dir & "\" & sd.file_name & yyyy & mm & dd & "_" & x & ".pdf"
                            fname = sd.file_name & yyyy & mm & dd & "_" & x & ".pdf"
                            x += 1
                        Loop
                        'ScanControl.write_error("File exists - Renaming to " & FILENAME, 1)
                        sd.full_file = FILENAME
                        sd.file_name = fname
                        'ScanControl.scannall(sd)
                        '.Quit = True

                    Else
                        sd.full_file = FILENAME
                        fname = sd.file_name & yyyy & mm & dd & ".pdf"
                        sd.file_name = fname
                    End If


                    Dim y As Boolean
                    'y = ScanControl.scannall_fake(sd)
                    y = ScanControl.scannall(sd)
                    If y = True Then

                    End If


                    .Quit = True


                End If
            End With
            Console.WriteLine(Command)


            cApp.Quit = True
            Application.Exit()
        Catch ex As Exception
            Console.WriteLine(ex.ToString)
        End Try



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
                    Case "e"
                        State = "e"
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
                    Case "f"
                        .RunMode = myRunMode.file
                        FILENAME = StrVal
                    Case "e"
                        .RunMode = myRunMode.Config
                        FILENAME = StrVal
                    Case Else
                        Valid = False
                End Select
            End With
        Catch ex As Exception
            Console.Write(ex.Message)
        End Try
    End Sub
End Module
