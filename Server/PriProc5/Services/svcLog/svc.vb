Imports System.Reflection

Public Class ppService

    Protected Overrides Sub OnStart(ByVal args() As String)
        ServiceThread = New System.Threading.Thread(AddressOf svcMain)
        With ServiceThread
            .Name = String.Format("{0}_Service", svcType)
            .IsBackground = True
            .Start(New StartArgs(svcType, Reflection.Assembly.GetExecutingAssembly, args))
        End With
    End Sub

    Protected Overrides Sub OnContinue()

    End Sub

    Protected Overrides Sub OnStop()
        ServiceThread.Abort()
    End Sub

End Class
