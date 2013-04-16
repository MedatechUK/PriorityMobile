Imports System.Reflection

Module Logging

    Public WithEvents ev As ntEvtlog.evt
    Public LogBuilder As Builder 'System.Text.StringBuilder

    Public ReadOnly Property MachineName() As String
        Get
            Return Environment.MachineName
        End Get
    End Property

    Public ReadOnly Property OSName() As String
        Get
            Return String.Format("{0} ({1}.{2}.{3}", _
                        Environment.OSVersion.Platform.ToString, _
                        Environment.OSVersion.Version.Major, _
                        Environment.OSVersion.Version.Minor, _
                        Environment.OSVersion.Version.Build _
            )
        End Get
    End Property

    Private _AppName As String = Nothing
    Public ReadOnly Property AppName() As String
        Get
            If IsNothing(_AppName) Then
                With Assembly.GetExecutingAssembly().GetName()
                    _AppName = .Name.ToUpper
                End With
            End If
            Return _AppName
        End Get
    End Property

    Private _AppVersion As String = Nothing
    Public ReadOnly Property AppVersion() As String
        Get
            If IsNothing(_AppVersion) Then
                With Assembly.GetExecutingAssembly().GetName()
                    _AppVersion = String.Format("{0}.{1}.{3}", _
                        .Version.Major, _
                        .Version.Minor, _
                        .Version.Build, _
                        .Version.Revision _
                    )
                End With
            End If
            Return _AppVersion
        End Get
    End Property

    Private _sysinfo As String = Nothing
    Public ReadOnly Property Sysinfo()
        Get
            If IsNothing(_sysinfo) Then
                With Assembly.GetExecutingAssembly().GetName()
                    _sysinfo = String.Format("+{0} @{2}:{4} Build: {1} Running: {3})", _
                        AppName, _
                        AppVersion, _
                        MachineName, _
                        OSName, _
                        svr.Port _
                    )
                End With
            End If
            Return _sysinfo
        End Get
    End Property

    Private Sub hLogEvent(ByVal EntryType As ntEvtlog.LogEntryType, ByVal Data As String) Handles ev.LoggedEvent
        AdminConsoleLog(EntryType, Data)
    End Sub

End Module
