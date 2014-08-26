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
                                Case "DB"
                                    Dim g As New frmSettings
                                    g.ShowDialog()
                                    .Quit = True
                                Case "SCAN"
                                    Dim g As New frmScanSetts
                                    g.ShowDialog()
                                    .Quit = True
                            End Select

                            .Quit = True
                        Case myRunMode.file
                            .doWelcome(Assembly.GetExecutingAssembly())
                    End Select
                Catch ex As Exception
                    Console.WriteLine(Command)
                    Console.WriteLine(ex.Message)
                    Console.ReadLine()
                    .Quit = True
                End Try

                If Not .Quit Then
                    
                    Dim LineOfData() As String = FILENAME.Split(" ")
                    'gets the values from the command line
                    Dim sd As ScanControl.ScanDocument
                    'ScanControl.ChecktConnectionString(LineOfData(0))
                    ''checks the connection string
                    Dim sdate, edate As DateTime
                    sdate = Now
                    edate = FormatDateTime("1/1/1988", DateFormat.GeneralDate)
                    sd = New ScanControl.ScanDocument(LineOfData(2), DateDiff(DateInterval.Minute, edate, sdate), "N", LineOfData(1))

                    'Select Case LineOfData.Length
                    '    Case 5

                    '    Case Else
                    '        extra = LineOfData(5)
                    '        sd.doc_dir = sd.doc_dir.Remove(0, 6)
                    '        sd.doc_dir = extra + sd.doc_dir
                    'End Select


                    'Dim tw As New DTI.ImageMan.Twain.TwainControl
                    'Dim capabilityValue As Object
                    'Dim dataType As DTI.ImageMan.Twain.DataType
                    'Dim d As Boolean
                    'Dim retVal As Boolean = tw.GetCapability(DTI.ImageMan.Twain.Capabilities.PaperDetectable, capabilityValue, dataType)
                    'Dim t As Integer = 0
                    't = CInt(capabilityValue)
                    ''If retVal AndAlso CInt(capabilityValue) <> 0 Then
                    'd = tw.GetCapability(DTI.ImageMan.Twain.Capabilities.PaperDetectable, capabilityValue, dataType)
                    'If d AndAlso CInt(capabilityValue) <> 0 Then
                    '    MessageBox.Show("Feeder is loaded with paper")
                    'End If
                    'End If


                    Dim y As Boolean
                    'y = ScanControl.scannall_fake(sd)
                    y = ScanControl.scannall(sd)
                    If y = True Then
                        .Quit = True
                    End If





                End If
            End With
            Console.WriteLine(Command)
            'console.readline()


            'cApp.Quit = True
            'Application.Exit()
        Catch ex As Exception
            Console.WriteLine(ex.ToString)
        End Try
        'console.readline()

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

