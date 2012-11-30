Imports System.IO
Imports System.Reflection
Module Module1
    Dim WithEvents cApp As New ConsoleApp.CA
    Enum myRunMode As Integer
        file = 0
        url = 1
        Config = 2
    End Enum
    Dim FILENAME As String = Nothing

    Public Sub Main()

        With cApp
            .RunMode = myRunMode.file
            .doWelcome(Assembly.GetExecutingAssembly())
            Try
                .GetArgs(Command)
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try
            If Not .Quit Then
                If filename = "" Then
                    Console.WriteLine("No file name provided!")
                    .Quit = True
                End If
                If File.Exists(filename) = False Then
                    Console.WriteLine("Cant find the specified file please check the name provided")
                End If
                XMLHandler.createloading(FILENAME)
            End If
        End With
        Console.WriteLine(Command)


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
                    Case "f"
                        .RunMode = myRunMode.file
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
