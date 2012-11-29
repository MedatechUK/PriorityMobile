Imports Microsoft.SqlServer.Management
Imports Microsoft.SqlServer.Management.Smo

Public Class ServerInstance
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
End Class

Public Class FindPriority

    Public Function EnumerateServers(Optional ByVal computerName As String = "") As List(Of ServerInstance)
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
    End Function
End Class

