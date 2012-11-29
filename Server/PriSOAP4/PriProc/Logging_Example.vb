Module Logging_Example

#Region "Declared in ntEvtLog.DLL"

    Public Enum EvtLogVerbosity
        Normal = 1
        Verbose = 10
        VeryVerbose = 50
        Arcane = 99
    End Enum

    Public Enum LogEntryType As Integer
        Err = 1
        Information = 4
        FailureAudit = 16
        SuccessAudit = 8
        Warning = 2
    End Enum

#End Region

    ' This is declared outside the method
    ' which allows us to pass it betwen many different methds
    Private logBuilder As Builder

    Sub main()

        ' Set the default Entry Type for this log. Usually a sucsessaudit
        Dim et As ntEvtlog.LogEntryType = ntEvtlog.LogEntryType.SuccessAudit

        ' Set the verbosity of the event log. Only messages with a verbosity
        ' greater than or equal to that set on the server will be recorded
        ' Set to normal if you always want a message logged        
        Dim verb As ntEvtlog.EvtLogVerbosity = ntEvtlog.EvtLogVerbosity.VeryVerbose
        ' VeryVerbose messages are only logged if the server verbosity is set to 
        ' VeryVerbose of Arcane.

        ' Start a new log message
        ' Note the builder is derived from the system.text.stringbuilder class
        logBuilder = New Builder

        Try
            ' Add some data to the log entry
            logBuilder.AppendFormat("The start of my log at {0}", Now.ToString).AppendLine()

            '... do some stuff ...

            ' Call another procedure, but send a reference to this log
            If AnotherMethod(logBuilder, et, verb) Then

                ' ... do some more stuff ...

            End If

        Catch JustAWarning As ExceptionWarning
            ' Catch the warning and append it to our log
            logBuilder.AppendFormat("A warning was issued. {0}", JustAWarning.Message)

            ' It's a warning so we change the Entry Type to warning
            et = ntEvtlog.LogEntryType.Warning

            ' And change the verbosity to verbose
            ' if the server verbosity log is verbose OR GREATER the message will be logged
            verb = ntEvtlog.EvtLogVerbosity.Verbose

        Catch FatalError As Exception
            ' Catch the error and append it to our log
            logBuilder.AppendFormat("Some Error condition occurred. {0}", FatalError.Message)

            ' It's an error so we change the Entry Type to a failureaudit
            et = ntEvtlog.LogEntryType.FailureAudit

            ' And change the verbosity to normal
            ' normal verbosity will ALWYAS add the message to the log
            verb = ntEvtlog.EvtLogVerbosity.Normal

        Finally
            ' Always write to the log by using finally block
            ev.Log(logBuilder.ToString, et, verb)
        End Try

    End Sub

    Private Function AnotherMethod(ByRef LogBuilder As Builder, ByVal et As ntEvtlog.LogEntryType, ByRef verb As ntEvtlog.EvtLogVerbosity) As Boolean

        Try
            ' Some logic
            If True Then
                Return True
            Else
                Return False
            End If

        Catch FatalError As Exception
            ' Catch the error and append it to our log
            LogBuilder.AppendFormat("Some Error condition occurred. {0}", FatalError.Message)

            ' It's an error so we change the Entry Type to a failureaudit
            et = ntEvtlog.LogEntryType.FailureAudit

            ' And change the verbosity to normal
            ' normal verbosity will ALWYAS add the message to the log
            verb = ntEvtlog.EvtLogVerbosity.Normal

        End Try

    End Function

End Module
