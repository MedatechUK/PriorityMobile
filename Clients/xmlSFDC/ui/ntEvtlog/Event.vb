#Region "Enumerations"

Public Enum LogEntryType As Integer
    Err = 1
    Information = 4
    FailureAudit = 16
    SuccessAudit = 8
    Warning = 2
End Enum
Public Enum EvtLogMode
    EventLog
    File
End Enum

Public Enum EvtLogVerbosity
    Normal = 1
    Verbose = 10
    VeryVerbose = 50
    Arcane = 99
End Enum

#End Region

Public Class evt

    Public Event LoggedEvent(ByVal Eventtype As ntEvtlog.LogEntryType, ByVal Data As String)

    Public Sub New()

    End Sub

    Public Sub New(ByVal LogMode As EvtLogMode, _
    ByVal LogVerbosity As EvtLogVerbosity, _
    ByVal AppName As String)
        With Me
            .LogMode = LogMode
            .LogVerbosity = LogVerbosity
            .AppName = AppName
        End With
    End Sub

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

    Private _BasePath As String = Nothing
    Public ReadOnly Property BasePath() As String
        Get
            If IsNothing(_BasePath) Then
                Dim fullPath As String = System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase
                If InStr(fullPath, "file:///", CompareMethod.Text) > 0 Then
                    fullPath = Replace(fullPath, "file:///", "")
                End If
                If InStr(fullPath, "/", CompareMethod.Text) > 0 Then
                    fullPath = Replace(fullPath, "/", "\")
                End If
                _BasePath = fullPath.Substring(0, fullPath.LastIndexOf("\"))
                If Strings.Right(_BasePath, 1) <> "\" Then _BasePath += "\"

            End If
            Return _BasePath
        End Get
    End Property

    Private Sub mkNEDir(ByVal dir As String)
        Dim f As New IO.DirectoryInfo(BasePath & dir & "\")
        If Not f.Exists Then
            IO.Directory.CreateDirectory(BasePath & dir)
        End If
    End Sub

    Public ReadOnly Property OutputFile() As String
        Get
            mkNEDir("LOGS")
            With Now
                Return String.Format("{0}LOGS\log_{1}-{2}-{3}.txt", _
                    BasePath, _
                    .Day.ToString, _
                    .Month.ToString, _
                    .Year.ToString)
            End With
        End Get
    End Property

    Private _LogMode As EvtLogMode = EvtLogMode.EventLog
    Public Property LogMode() As EvtLogMode
        Get
            Return _LogMode
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
        Optional ByVal EventType As LogEntryType = LogEntryType.Information, _
        Optional ByVal Verbosity As EvtLogVerbosity = EvtLogVerbosity.Normal _
    )

        'Console.WriteLine(Entry)
        If Verbosity <= LogVerbosity Then            
            Select Case LogMode
                Case EvtLogMode.EventLog
                    If Not IsNothing(AppName) Then
#If Not WindowsCE Then
                        Dim SpEnt As New System.Text.StringBuilder
                        For Each Line As String In Entry.Split(vbCrLf)
                            SpEnt.Append(Line)
                            If SpEnt.Length > 30000 Then
                                SpEnt.AppendLine.Append("...")
                                RaiseEvent LoggedEvent(EventType, SpEnt.ToString)
                                If Not (WriteToEventLog(SpEnt.ToString, EventType)) Then
                                    Throw New Exception("Event Log [" & LogName & "." & AppName & "] not defined.")
                                End If
                                SpEnt = New System.Text.StringBuilder("...").AppendLine
                            End If
                        Next
                        RaiseEvent LoggedEvent(EventType, SpEnt.ToString)
                        If Not (WriteToEventLog(SpEnt.ToString, EventType)) Then
                            Throw New Exception("Event Log [" & LogName & "." & AppName & "] not defined.")
                        End If
#End If
                    Else
                        Throw New Exception("Missing Application Name.")
                    End If

                Case Else
                    If Not IsNothing(OutputFile) Then
                        If Not (WriteToFile(Entry)) Then
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
#If Not WindowsCE Then
            'Register the App as an Event Source
            If Not Diagnostics.EventLog.SourceExists(AppName) Then
                Diagnostics.EventLog.CreateEventSource(AppName, LogName)
                Log( _
                    "An event log has been created for the application [" & _AppName & "] " & _
                    "in the [" & _LogName & "] log file.", _
                    LogEntryType.SuccessAudit, _
                     EvtLogVerbosity.Normal _
                    )
            Else
                Log( _
                    "An event log already exists for application [" & _AppName & "]" & _
                    "in the [" & _LogName & "] log file.", _
                    LogEntryType.SuccessAudit, _
                     EvtLogVerbosity.VeryVerbose _
                    )
            End If
#End If
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

#If Not WindowsCE Then
    Private Function WriteToEventLog( _
        ByVal Entry As String, _
        Optional ByVal EventType As LogEntryType = LogEntryType.Information) _
        As Boolean

        Dim objEventLog As New EventLog()
        Try
            objEventLog.Source = _AppName
            Select Case EventType
                Case LogEntryType.Err
                    objEventLog.WriteEntry(Entry, EventLogEntryType.Error)
                Case LogEntryType.FailureAudit
                    objEventLog.WriteEntry(Entry, EventLogEntryType.FailureAudit)
                Case LogEntryType.Information
                    objEventLog.WriteEntry(Entry, EventLogEntryType.Information)
                Case LogEntryType.SuccessAudit
                    objEventLog.WriteEntry(Entry, EventLogEntryType.SuccessAudit)
                Case LogEntryType.Warning
                    objEventLog.WriteEntry(Entry, EventLogEntryType.Warning)
            End Select
            Return True
        Catch Ex As Exception
            Return False
        End Try
    End Function
#End If

    Private Function WriteToFile( _
        ByVal Entry As String) _
        As Boolean

        Try
            Using sw As New System.IO.StreamWriter(OutputFile, True)
                Dim ent() As String = Split(Entry, vbCrLf)
                For i As Integer = 0 To UBound(ent)
                    sw.WriteLine( _
                        String.Format("{0}:{1}{4}{2}{4}{3}", _
                            Right("00" & Now.Hour.ToString, 2), Right("00" & Now.Minute.ToString, 2), _
                            AppName, _
                            ent(i), _
                            Chr(9) _
                        ) _
                    )
                Next
            End Using
            Return True
        Catch Ex As Exception
            Return False
        End Try
    End Function

#End Region

End Class
