Imports System.Threading

Public Class GlobalAX
    Inherits iSettings

    Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)

        ' Start a thread to generate connection strings 
        ' for all existing Priority Environments

        Dim lt As New Priority.configTest
        With lt
            .AppName = AppName
            .ConnectionString = ConnectionString
            .Environment = Environment
            .LogVerbosity = LogVerbosity
            .RemoteIP = RemoteIP
            .RunMode = Priority.configTest.eRunMode.asynchronous
            .ThisCommand = "inetpub"
            .ThisCommandDescription = "get Priority environments"
            .ThisArguments = Me.WebPath & Me.ConfigFile

            Dim myThread As Thread
            myThread = New Thread(New ThreadStart(AddressOf .ServerOutput))
            myThread.Start()
        End With

    End Sub

    Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs on application shutdown
    End Sub

    Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs when an unhandled error occurs
    End Sub

    Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs when a new session is started
    End Sub

    Sub Session_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs when a session ends. 
        ' Note: The Session_End event is raised only when the sessionstate mode
        ' is set to InProc in the Web.config file. If session mode is set to StateServer 
        ' or SQLServer, the event is not raised.
    End Sub

End Class