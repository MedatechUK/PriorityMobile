Imports System.Threading
Imports System.Diagnostics
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Reflection
Imports ConsoleApp
Imports System.io

Module conSign

    Dim fn As String = ""
    Dim WithEvents cApp As New ConsoleApp.CA

    Enum myRunMode As Integer
        Normal = 0
        Config = 1
        help = 2
    End Enum

    Sub Main()
        With cApp

            .RunMode = myRunMode.Normal
            .doWelcome(Assembly.GetExecutingAssembly())

            'Console.WriteLine("")
            'Console.WriteLine("Press any key to continue.")
            'Dim strInput As String = Console.ReadKey(False).ToString
            'While (strInput = "")
            '    Thread.Sleep(100)
            'End While

            Try
                .GetArgs(Command)
            Catch ex As Exception
                Console.WriteLine(ex.Message)
                .Quit = True
            End Try

            If Not .Quit Then
                If Not (File.Exists(fn)) Then
                    .Quit = True
                    MsgBox(String.Format("The file [{0}] was not found.", fn))
                End If

                Select Case .RunMode
                    Case myRunMode.Normal
                        payload()
                End Select

            End If

            cApp.Finalize()

        End With
    End Sub

#Region "Switches"

    Private Sub cApp_Switch(ByVal StrVal As String, ByRef State As String, ByRef Valid As Boolean) Handles cApp.Switch
        Try
            With cApp
                Select Case StrVal
                    Case "fn"
                        State = "fn"
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
                    Case "fn"
                        fn = StrVal                    
                    Case Else
                        Valid = False
                End Select
            End With
        Catch ex As Exception
            Console.Write(ex.Message)
        End Try
    End Sub

#End Region

    Sub payload()

        Dim objGraphics As Graphics
        Dim X1 As Integer = 0
        Dim X2 As Integer = 0
        Dim Y1 As Integer = 0
        Dim Y2 As Integer = 0
        Dim maxX As Integer = 1
        Dim maxY As Integer = 1

        Dim arcls As New MyCls.MyArray
        Dim data(,) As String = Nothing
        arcls.ArrayFromFile(data, fn)

        For i As Integer = 0 To UBound(data, 2)
            If data(0, i) > maxX Then
                maxX = data(0, i)
            End If
            If data(1, i) > maxY Then
                maxY = data(1, i)
            End If
        Next

        Dim b As Bitmap = New System.Drawing.Bitmap(maxX, maxY, PixelFormat.Format24bppRgb)
        objGraphics = Graphics.FromImage(b)
        objGraphics.Clear(Color.White)

        For i As Integer = 1 To UBound(data, 2)
            Try
                X1 = CInt(data(0, i - 1))
                Y1 = CInt(data(1, i - 1))
                X2 = CInt(data(0, i))
                Y2 = CInt(data(1, i))

                If Not (X1 = 0 And Y1 = 0) And Not (X2 = 0 And Y2 = 0) Then
                    objGraphics.DrawLine(Pens.Black, X1, Y1, X2, Y2)
                End If
            Catch

            End Try
        Next

        Console.Write(String.Format("Saving JPG as {0}.", UCase(Replace(fn, ".sig", ".jpg"))))
        b.Save(UCase(Replace(fn, ".sig", ".jpg")), ImageFormat.Jpeg)
        b.Dispose()
        objGraphics.Dispose()

    End Sub

End Module
