Imports System.IO
Imports System.Reflection
Module Module1
    Dim WithEvents cApp As New ConsoleApp.CA
    Enum myRunMode As Integer
        file = 0
        url = 1
        Config = 2
        enviro = 3
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
                    Console.WriteLine(FILENAME)
                    .Quit = True
                End If
                Dim valid As Boolean = validatecsv(FILENAME)
                If valid = False Then
                    .Quit = True
                Else
                    XMLHandler.createloading(FILENAME)
                End If

            End If
        End With
        Console.WriteLine(Command)


    End Sub
    Private Function validatecsv(ByVal fi As String)
        Dim filereader As StreamReader
        Dim line_no As Integer = 1

        filereader = New StreamReader(fi)
        Using filereader
            Dim li As String
            li = filereader.ReadLine 'read the first line of data in
            'keep reading until the end of the file
            Dim col0, col1, col2, col3, col4, col5, col6, col7, col8, col9, col10, col11 As Integer
            col0 = 0
            col1 = 0
            col2 = 0
            col3 = 0
            col4 = 0
            col5 = 0
            col6 = 0
            col7 = 0
            col8 = 0
            col9 = 0
            col10 = 0
            col11 = 0
            Dim LineOfData() As String = li.Split(",") ' seperate each value of the line
            If UBound(LineOfData) <> 11 Then
                Console.WriteLine("The file is not in the required format as it does not have the required amount of columns")
                

                Return False
            Else
                Dim cou As Integer = 0
                Do While cou <> UBound(LineOfData)
                    Select Case LineOfData(cou)
                        Case ""
                            If cou <> 0 Or cou <> 7 Then
                                Select Case cou
                                    Case 1
                                        col1 = 1
                                    Case 2
                                        col2 = 1
                                    Case 3
                                        col3 = 1
                                    Case 4
                                        col4 = 1
                                    Case 5
                                        col5 = 1
                                    Case 6
                                        col6 = 1
                                    Case 7
                                        col7 = 1
                                    Case 8
                                        col8 = 1
                                    Case 9
                                        col9 = 1
                                    Case 10
                                        col10 = 1
                                    Case 11
                                        col11 = 1
                                End Select
                            End If
                        Case "Account Number"
                            If cou <> 1 Then
                                Console.WriteLine("Account Number column is out of place, it should be the second column")
                                col1 = 1
                                Return False
                            End If


                        Case "Order placed"
                            If cou <> 2 Then
                                Console.WriteLine("Order Placed date column is out of place, it should be the third column")
                                col2 = 1
                                Return False
                            End If

                        Case "Customer's Reference"
                            If cou <> 3 Then
                                Console.WriteLine("Customers Reference column is out of place, it should be the fourth column")
                                col3 = 1
                                Return False
                            End If

                        Case "Ad-hoc Instructions - 64 Characters"
                            If cou <> 4 Then
                                Console.WriteLine("Ad Hoc Instructions column is out of place, it should be the fifth column")
                                col4 = 1
                                Return False
                            End If

                        Case "Location"
                            If cou <> 5 Then
                                Console.WriteLine("Location column is out of place, it should be the sixth column")
                                col5 = 1
                                Return False
                            End If

                        Case "Requested Delivery Date"
                            If cou <> 6 Then
                                Console.WriteLine("Delivery date column is out of place, it should be the seventh column")
                                col6 = 1
                                Return False
                            End If

                        Case "Product"
                            If cou <> 8 Then
                                Console.WriteLine("Product Code column is out of place, it should be the eighth column")
                                col8 = 1
                                Return False
                            End If

                        Case "Quantity"
                            If cou <> 9 Then
                                Console.WriteLine("Quantity column is out of place, it should be the ninth column")
                                col9 = 1
                                Return False
                            End If

                        Case "Price"
                            If cou <> 10 Then
                                Console.WriteLine("Price column is out of place, it should be the tenth column")
                                col10 = 1
                                Return False
                            End If

                        Case "End of Line"
                            If cou <> 10 Then
                                Console.WriteLine("End Marker is out of place, it should be the eleventh column")
                                col10 = 1
                                Return False
                            End If

                        Case Else
                            Console.WriteLine("Unexpected Column Name (" & LineOfData(cou) & ") found in file please check the data file and correct it.")
                            Return False

                    End Select
                    cou += 1
                Loop


            End If
        End Using
        Return True

    End Function
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

                    Case Else
                        Valid = False
                End Select
            End With
        Catch ex As Exception
            Console.Write(ex.Message)
        End Try
    End Sub
End Module
