#Region "Enumerations"

Public Enum EvtLogMode
    EventLog
    File
    FileANDEventLog
End Enum

Public Enum EvtLogVerbosity
    Normal = 1
    Verbose = 10
    VeryVerbose = 50
    Arcane = 99
End Enum

#End Region

Public Class evt

#Region "Public Properties"

    Dim _AppName As String = Nothing
    Public Property AppName() As String
        Get
            Return _AppName
        End Get
        Set(ByVal value As String)
            _AppName = value
        End Set
    End Property

    Dim _LogName As String = "Application"
    Public Property LogName() As String
        Get
            Return _LogName
        End Get
        Set(ByVal value As String)
            _LogName = value
        End Set
    End Property

    Dim _OutputFile As String = Nothing
    Public Property OutputFile() As String
        Get
            Return _OutputFile
        End Get
        Set(ByVal value As String)
            _OutputFile = value
        End Set
    End Property

    Private _LogMode As EvtLogMode = EvtLogMode.EventLog
    Public Property LogMode() As EvtLogMode
        Get
            Return LogMode
        End Get
        Set(ByVal value As EvtLogMode)
            _LogMode = value
        End Set
    End Property

    Private _LogVerbosity As EvtLogVerbosity = EvtLogVerbosity.Normal
    Public Property LogVerbosity() As EvtLogVerbosity
        Get
            Return _LogVerbosity
        End Get
        Set(ByVal value As EvtLogVerbosity)
            _LogVerbosity = value
        End Set
    End Property

#End Region

#Region "public Subs"

#Region "Register Log"

    Public Overloads Sub RegisterLog()
        If Not RegisterForEvents() Then Throw New Exception("Event Log [" & LogName & "." & AppName & "] was not created.")
    End Sub

    Public Overloads Sub RegisterLog( _
     ByVal NewAppName As String)
        AppName = NewAppName
        If Not RegisterForEvents() Then Throw New Exception("Event Log [" & LogName & "." & AppName & "] was not created.")
    End Sub

    Public Overloads Sub RegisterLog( _
         ByVal NewLogName As String, _
         ByVal NewAppName As String)
        AppName = NewAppName
        LogName = NewLogName
        If Not RegisterForEvents() Then Throw New Exception("Event Log [" & LogName & "." & AppName & "] was not created.")
    End Sub

#End Region

    Public Sub Log( _
        ByVal Entry As String, _
        Optional ByVal EventType As Diagnostics.EventLogEntryType = Diagnostics.EventLogEntryType.Information, _
        Optional ByVal Verbosity As EvtLogVerbosity = EvtLogVerbosity.Normal _
    )

        If Verbosity <= LogVerbosity Then
            Console.WriteLine(Entry)
            Select Case LogMode
                Case EvtLogMode.EventLog
                    If Not IsNothing(AppName) Then
                        If Not (WriteToEventLog(Entry, EventType)) Then
                            Throw New Exception("Event Log [" & LogName & "." & AppName & "] not defined.")
                        End If
                    Else
                        Throw New Exception("Missing Application Name.")
                    End If

                Case EvtLogMode.FileANDEventLog
                    If Not IsNothing(AppName) Then
                        If Not (WriteToEventLog(Entry, EventType)) Then
                            Throw New Exception("Event Log [" & LogName & "." & AppName & "] not defined.")
                        End If
                    Else
                        Throw New Exception("Missing Application Name.")
                    End If
                    If Not IsNothing(OutputFile) Then
                        If Not (WriteToFile(Entry, EventType)) Then
                            Throw New Exception("Failed to write event to [" & OutputFile & "].")
                        End If
                    Else
                        Throw New Exception("Missing output file.")
                    End If

                Case Else
                    If Not IsNothing(OutputFile) Then
                        If Not (WriteToFile(Entry, EventType)) Then
                            Throw New Exception("Failed to write event to [" & OutputFile & "].")
                        End If
                    End If
            End Select
        End If

    End Sub

    Public Function DescribeVerbosity(ByVal verbosity As Integer) As String
        Select Case verbosity
            Case EvtLogVerbosity.Normal
                Return "Errors Only"
            Case EvtLogVerbosity.Verbose
                Return "Verbose"
            Case EvtLogVerbosity.VeryVerbose
                Return "Very Verbose"
            Case EvtLogVerbosity.Arcane
                Return "Arcane"
            Case Else
                Return String.Format("Verbosity level [{0}].", verbosity.ToString)
        End Select
    End Function

#End Region

#Region "Private Subs"

    Private Function RegisterForEvents() As Boolean
        Try
            'Register the App as an Event Source
            If Not Diagnostics.EventLog.SourceExists(AppName) Then
                Diagnostics.EventLog.CreateEventSource(AppName, LogName)
                Log( _
                    "An event log has been created for the application [" & _AppName & "]" & vbCrLf & _
                    "in the [" & _LogName & "] log file.", _
                    Diagnostics.EventLogEntryType.SuccessAudit, _
                     EvtLogVerbosity.Normal _
                    )
            Else
                Log( _
                    "An event log Already exists for application [" & _AppName & "]" & vbCrLf & _
                    "in the [" & _LogName & "] log file.", _
                    Diagnostics.EventLogEntryType.SuccessAudit, _
                     EvtLogVerbosity.VeryVerbose _
                    )
            End If
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Private Function WriteToEventLog( _
        ByVal Entry As String, _
        Optional ByVal EventType As Diagnostics.EventLogEntryType = Diagnostics.EventLogEntryType.Information) _
        As Boolean

        Dim objEventLog As New Diagnostics.EventLog
        Try
            objEventLog.Source = _AppName
            objEventLog.WriteEntry(Entry, EventType)
            Return True
        Catch Ex As Exception
            Return False
        End Try

    End Function

    Private Function WriteToFile( _
        ByVal Entry As String, _
        Optional ByVal EventType As Diagnostics.EventLogEntryType = Diagnostics.EventLogEntryType.Information) _
        As Boolean

        Try
            Using sw As New System.IO.StreamWriter(OutputFile, True)
                sw.WriteLine( _
                    Now.ToString, _
                    AppName, _
                    EventType.ToString, _
                    Entry _
                )
            End Using
            Return True
        Catch Ex As Exception
            Return False
        End Try
    End Function

#End Region

End Class
