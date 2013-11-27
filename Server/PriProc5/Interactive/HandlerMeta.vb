Imports System.Threading
Imports PriPROC

Public Module HandlerMeta

#Region "Public Properties"

    Private _ServiceThread As Thread
    Public Property ServiceThread() As Thread
        Get
            Return _ServiceThread
        End Get
        Set(ByVal value As Thread)
            _ServiceThread = value
        End Set
    End Property

#End Region

#Region "Service Threaded Methods"

    Sub Main(ByVal args() As String)

        ServiceThread = New Thread(AddressOf svcMain)
        With ServiceThread
            .Name = String.Format("{0}_Interactive", svcType)
            .IsBackground = True
            .Start(New StartArgs(svcType, Reflection.Assembly.GetExecutingAssembly, args))
            While Not .ThreadState = ThreadState.Stopped
                Thread.Sleep(100)
            End While
        End With

    End Sub

    Public Sub svcMain(ByVal sArg As StartArgs)

        Dim thisService As ServiceMustInherit
        Try
            Using upd As New Update(sArg)
                thisService = upd.LoadHandler(sArg)
                While Not thisService.InteractiveShutdown
                    Thread.Sleep(10)
                End While
            End Using
            ServiceThread.Abort()

        Catch ex As System.Threading.ThreadAbortException
            ' Quit

        Catch ex As SystemFail
            With sArg
                Select Case .RunMode
                    Case eRunMode.Interactive
                        Console.WriteLine(.StartLog.LogData.ToString)
                        Console.Read()
                    Case Else
                        ' TODO: log to windows evtlog
                        End
                End Select
            End With

        Catch ex As Exception
            Beep()

        Finally
            If Not IsNothing(thisService) Then
                thisService.Shutdown()
                thisService.Dispose()
            End If

        End Try

    End Sub

#End Region

End Module
