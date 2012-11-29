Imports System
Imports System.Management
Imports Microsoft.SqlServer.Management
Imports Microsoft.SqlServer.Management.Smo
Imports System.Data.SqlClient
Imports System.IO

Friend Class ServerInstance
    Public Sub New(ByVal name As String, ByVal server As String, ByVal instance As String, ByVal clustered As Boolean, ByVal local As Boolean)
        m_name = name
        m_server = server
        m_instance = instance
        m_clustered = clustered
        m_local = local
    End Sub
    Private m_name As String = ""
    Public Property Name() As String
        Get
            Return m_name
        End Get
        Set(ByVal value As String)
            m_name = value
        End Set
    End Property
    Private m_server As String = ""
    Public Property Server() As String
        Get
            Return m_server
        End Get
        Set(ByVal value As String)
            m_server = value
        End Set
    End Property
    Private m_instance As String = ""
    Public Property Instance() As String
        Get
            Return m_instance
        End Get
        Set(ByVal value As String)
            m_instance = value
        End Set
    End Property
    Private m_clustered As Boolean
    Public Property IsClustered() As Boolean
        Get
            Return m_clustered
        End Get
        Set(ByVal value As Boolean)
            m_clustered = value
        End Set
    End Property
    Private m_local As Boolean
    Public Property IsLocal() As Boolean
        Get
            Return m_local
        End Get
        Set(ByVal value As Boolean)
            m_local = value
        End Set
    End Property
    Public ReadOnly Property ServerInstance()
        Get
            With Me
                Select Case .Instance.Length
                    Case 0
                        Return .Server
                    Case Else
                        Return String.Format("{0}\{1}", .Server, .Instance)
                End Select
            End With
        End Get
    End Property
    Private _LoginException As Exception
    Public Property LoginException() As Exception
        Get
            Return _LoginException
        End Get
        Set(ByVal value As Exception)
            _LoginException = value
        End Set
    End Property
    Private _PriorityShare As String = Nothing
    Public Property PriorityShare() As String
        Get
            Return _PriorityShare
        End Get
        Set(ByVal value As String)
            _PriorityShare = value
        End Set
    End Property
    Public ReadOnly Property PriorityUNC() As String
        Get
            With Me
                Return String.Format("\\{0}\{1}", .Server, .PriorityShare)
            End With
        End Get
    End Property
End Class

Friend Class ServerShare

    Public Sub New(ByRef LogBuilder As Builder, Optional ByVal ServerName As String = ".")
        Try
            Dim Dir As DirectoryInfo

            LogBuilder.AppendFormat("Scanning server [{0}] for shared folders...", ServerName).AppendLine()
            ' Check for \\{servername}\Priority Share 1st
            LogBuilder.AppendFormat("Looking for default 'Priority' share...", ServerName).AppendLine()
            Dir = New DirectoryInfo(String.Format("\\{0}\priority", ServerName))
            If Dir.Exists Then
                LogBuilder.AppendFormat("[{0}] has a shared folder called 'Priority'...", ServerName).AppendLine()
                With SubFolders(Dir)
                    LogBuilder.AppendFormat("Checking \\{0}\{1} for \Bin.95 and \System folders...", ServerName, "Priority").AppendLine()
                    If .Contains("BIN.95") And .Contains("SYSTEM") Then
                        If Not File.Exists(String.Format("{0}\nofollow.soap", Dir.FullName)) Then
                            _SharesPriority = True
                            _PriorityShare = "priority"
                            Exit Sub
                        Else
                            LogBuilder.AppendFormat("Found nofollow.soap file in \\{0}\{1}. Ignoring share.", ServerName, "Priority").AppendLine()
                        End If
                    End If
                End With
            Else
                LogBuilder.AppendFormat("[{0}] does not have a 'Priority' share.", ServerName).AppendLine()
                LogBuilder.Append("Looking for Priority installation in other shares...").AppendLine()
            End If

            ' Else Iterate through shares
            Dim Path As New ManagementPath
            With Path
                Path.Server = ServerName ' use . for local server, else server name
                Path.NamespacePath = "root\CIMV2"
                Path.RelativePath = "Win32_Share"
            End With

            Dim Shares As New ManagementClass(New ManagementScope(Path), Path, New ObjectGetOptions(Nothing, New TimeSpan(0, 0, 0, 5), True))
            For Each MO As ManagementObject In Shares.GetInstances()
                Try
                    If MO.Item("Type") = 0 Then
                        LogBuilder.AppendFormat("Looking for Priority installation in \\{0}\{1}", ServerName, MO.Item("Name")).AppendLine()
                        Dir = New DirectoryInfo(String.Format("\\{0}\{1}", ServerName, MO.Item("Name")))
                        With SubFolders(Dir)
                            LogBuilder.AppendFormat("Checking {0} for \Bin.95 and \System folders...", Dir).AppendLine()
                            If .Contains("BIN.95") And .Contains("SYSTEM") Then
                                If Not File.Exists(String.Format("{0}\nofollow.soap", Dir.FullName)) Then
                                    _SharesPriority = True
                                    _PriorityShare = MO.Item("Name")
                                    LogBuilder.AppendFormat("Found Priority Installation at {0}.", Dir).AppendLine()
                                    Exit For
                                Else
                                    LogBuilder.AppendFormat("Found nofollow.soap file in {0}. Ignoring folder.", Dir).AppendLine()
                                End If
                            End If
                        End With
                    End If
                Catch
                End Try
            Next
        Catch e As Exception
        End Try
    End Sub

    Private _SharesPriority As Boolean = False
    Public ReadOnly Property SharesPriority() As Boolean
        Get
            Return _SharesPriority
        End Get
    End Property

    Private _PriorityShare As String = Nothing
    Public ReadOnly Property PriorityShare() As String
        Get
            Return _PriorityShare
        End Get
    End Property

    Private Function SubFolders(ByVal dir As DirectoryInfo) As List(Of String)
        Dim ret As New List(Of String)
        For Each sDir As DirectoryInfo In dir.GetDirectories
            ret.Add(sDir.Name.ToUpper)
        Next
        Return ret
    End Function

End Class

Module locatePriority

    Public Function PriorityInstances(ByRef FindInstanceException As Exception, ByRef LogBuilder As Builder) As ServerInstance

        Dim SharesPriority As New List(Of ServerInstance)
        Dim HasPriTempdb As New List(Of ServerInstance)
        Dim LoginErrors As New List(Of Exception)
        Dim serverShares As New Dictionary(Of String, ServerShare)

        LogBuilder.Append("Scanning network for SQL server installations...").AppendLine()
        For Each s As ServerInstance In EnumerateServers()
            LogBuilder.AppendFormat("Found SQL instance on network at [{0}]", s.ServerInstance).AppendLine()
            If Not serverShares.Keys.Contains(s.Server) Then
                LogBuilder.AppendFormat("Scanning shared UNC folders on [\\{0}]", s.Name).AppendLine()
                serverShares.Add(s.Server, New ServerShare(LogBuilder, s.Server))
            End If
            If serverShares(s.Server).SharesPriority Then
                s.PriorityShare = serverShares(s.Server).PriorityShare
                SharesPriority.Add(s)
                If Not serverShares.Keys.Contains(s.Server) Then
                    LogBuilder.AppendFormat("Priority Share found at [{0}] on [{1}]", s.PriorityShare, s.Name).AppendLine()
                End If
            End If
        Next

        If SharesPriority.Count > 0 Then
            For Each s As ServerInstance In SharesPriority
                Try
                    With My.Settings
                        .PRIUNC = s.PriorityUNC
                        .Save()

                        Using Connection As GenericConnection = New GenericConnection( _
                                .PROVIDER, _
                                s.ServerInstance, _
                                .PRIORITYUSER, _
                                .PRIORITYPWD, _
                                "pritempdb" _
                            )
                            LogBuilder.AppendFormat("Attempting to open Priority Datasource (PriTemp DB) on [{0}]...", s.ServerInstance).AppendLine()
                            Connection.Open()
                            HasPriTempdb.Add(s)
                            LogBuilder.AppendFormat("Connect to Priority Datasource (PriTemp DB) on [{0}] OK.", s.ServerInstance).AppendLine()
                        End Using
                    End With
                Catch ex As Exception
                    s.LoginException = ex
                    LogBuilder.AppendFormat("Connect to Priority Datasource (PriTemp DB) on [{0}] FAILED.", s.ServerInstance).AppendLine.AppendFormat("{0}", s.LoginException.Message).AppendLine()
                End Try
            Next
        End If

        Select Case HasPriTempdb.Count
            Case 1
                FindInstanceException = Nothing
                Return HasPriTempdb(0)
            Case 0
                Select Case SharesPriority.Count
                    Case 0
                        FindInstanceException = New Exception("No Priority instances were found on the network.")
                    Case Else
                        Dim msg As New Text.StringBuilder
                        msg.Append("Failed to connect to any Priority instance.").AppendLine.Append("I tried the following servers: ").AppendLine()
                        For Each s As ServerInstance In SharesPriority
                            msg.AppendFormat("[{0}] said '{1}'. ", s.ServerInstance, s.LoginException.Message).AppendLine()
                        Next
                        msg.AppendLine.Append("This error condition probably means that your password is incorrect.").AppendLine()
                        FindInstanceException = New Exception(msg.ToString)
                End Select
                Return Nothing
            Case Else
                FindInstanceException = New Exception("Multiple Priority instances found. You can exclude Priority servers from this search by saving a file named 'nofollow.soap' in their \\[server]\Priority directory.")
                Return Nothing
        End Select

    End Function

    Private Function EnumerateServers(Optional ByVal computerName As String = "") As List(Of ServerInstance)
        Try
            Dim tableServers As DataTable = Nothing
            Dim slist As New List(Of ServerInstance)
            If computerName.Length = 0 Then
                tableServers = SmoApplication.EnumAvailableSqlServers
            Else
                tableServers = SmoApplication.EnumAvailableSqlServers(computerName)
            End If
            ' Build the list of servers.
            For Each row As System.Data.DataRow In tableServers.Rows
                slist.Add( _
                    New ServerInstance( _
                        row("Name").ToString(), _
                        row("Server").ToString(), _
                        row("Instance").ToString(), _
                        row("IsClustered").ToString(), _
                        row("IsLocal").ToString() _
                    ) _
                )
            Next
            Return slist
        Catch ex As Exception
            Throw New Exception(String.Format("Unable to enumerate SQL services. {0}", ex.Message))
            Return Nothing
        End Try
    End Function

End Module
