Imports System.IO
Imports System
Imports System.Management
Imports Microsoft.SqlServer.Management
Imports Microsoft.SqlServer.Management.Smo
Imports System.Data.SqlClient

Public Class frmStartArgs

    Declare Function WNetGetConnection Lib "mpr.dll" Alias "WNetGetConnectionA" (ByVal lpszLocalName As String, _
         ByVal lpszRemoteName As String, ByRef cbRemoteName As Integer) As Integer

    Private Sub OK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK.Click
        With My.Settings
            .saved_lstProvider = lstProvider.Text
            .saved_DATASOURCE = DATASOURCE.Text
            .saved_UsernameTextBox = UsernameTextBox.Text
            .saved_PasswordTextBox = PasswordTextBox.Text
            .saved_PRIORITYDIR = PRIORITYDIR.Text
            .saved_PRIUNC = PRIUNC.Text
        End With

        Me.Close()
    End Sub

    Private Sub Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel.Click
        Me.Close()
    End Sub

    Private Sub frmStartArgs_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        For Each d As DriveInfo In DriveInfo.GetDrives
            If d.DriveType = DriveType.Network Then
                PRIORITYDIR.Items.Add(d.Name.Substring(0, 2))
            End If
        Next
    End Sub

    Private Sub BrowseFolder_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BrowseFolder.Click
        If Me.FolderBrowserDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            Me.PRIUNC.Text = Me.FolderBrowserDialog1.SelectedPath
        End If
    End Sub

    Private Sub PRIORITYDIR_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PRIORITYDIR.SelectedIndexChanged
        Dim f As Boolean = False
        For Each d As DriveInfo In DriveInfo.GetDrives
            If d.Name.Substring(0, 2) = PRIORITYDIR.SelectedItem Then
                If d.DriveType = DriveType.Network Then
                    Dim UNCName As String = Space(160)
                    If WNetGetConnection(d.Name.Substring(0, 2), UNCName, UNCName.Length) = 0 Then
                        Me.PRIUNC.Text = UNCName.Trim
                        f = True
                        Exit For
                    End If
                End If
            End If
        Next
        If Not f Then Me.PRIUNC.Text = ""
    End Sub

    Private Sub CheckBox1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox1.CheckedChanged
        With Me

            .UsernameTextBox.Enabled = CheckBox1.Checked
            .PasswordTextBox.Enabled = CheckBox1.Checked

            .PRIORITYDIR.Enabled = CheckBox1.Checked
            .PRIUNC.Enabled = CheckBox1.Checked
            .DATASOURCE.Enabled = CheckBox1.Enabled
            .SERVICEPORT.Enabled = CheckBox1.Enabled

            .BrowseFolder.Enabled = CheckBox1.Enabled
            .ListServers.Enabled = CheckBox1.Enabled
            .lstProvider.Enabled = CheckBox1.Enabled

            Select Case CheckBox1.Checked
                Case True
                    .UsernameTextBox.BackColor = Color.White
                    .PasswordTextBox.BackColor = Color.White

                    .PRIORITYDIR.BackColor = Color.White
                    .PRIUNC.BackColor = Color.White
                    .DATASOURCE.BackColor = Color.White
                    .SERVICEPORT.BackColor = Color.White
                    .lstProvider.BackColor = Color.White

                    With My.Settings
                        lstProvider.Text = .saved_lstProvider
                        DATASOURCE.Text = .saved_DATASOURCE
                        UsernameTextBox.Text = .saved_UsernameTextBox
                        PasswordTextBox.Text = .saved_PasswordTextBox
                        PRIORITYDIR.Text = .saved_PRIORITYDIR
                        PRIUNC.Text = .saved_PRIUNC
                    End With

                Case False
                    .UsernameTextBox.BackColor = Color.WhiteSmoke
                    .PasswordTextBox.BackColor = Color.WhiteSmoke

                    .PRIORITYDIR.BackColor = Color.WhiteSmoke
                    .PRIUNC.BackColor = Color.WhiteSmoke
                    .DATASOURCE.BackColor = Color.WhiteSmoke
                    .SERVICEPORT.BackColor = Color.WhiteSmoke
                    .lstProvider.BackColor = Color.WhiteSmoke

                    .lstProvider.Text = ""
                    .UsernameTextBox.Text = ""
                    .PasswordTextBox.Text = ""
                    .PRIORITYDIR.Text = ""
                    .PRIUNC.Text = ""
                    .DATASOURCE.Text = ""
                    .SERVICEPORT.Text = "8021"
            End Select

        End With
    End Sub

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

    Private Sub hListServers_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListServers.Click
        With Me.DATASOURCE.Items
            .Clear()
            For Each s As ServerInstance In EnumerateServers()
                .Add(s.ServerInstance)
            Next
        End With
    End Sub

End Class

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