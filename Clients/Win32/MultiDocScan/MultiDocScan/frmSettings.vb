Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.Configuration.ConfigurationSettings
Imports System.Data.Sql
Public Class frmSettings
    Dim filling As Boolean = False
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim instance As SqlDataSourceEnumerator = _
            SqlDataSourceEnumerator.Instance
        Dim table As System.Data.DataTable = instance.GetDataSources()
        Dim listOfServers As New List(Of String)()
        For Each RowOfData As DataRow In table.Rows
            'get the server name 
            Dim serverName As String = RowOfData("ServerName").ToString()
            'get the instance name 
            Dim instanceName As String = RowOfData("InstanceName").ToString()

            'check if the instance name is empty 
            If Not instanceName.Equals(String.Empty) Then
                'append the instance name to the server name 
                serverName += String.Format("\{0}", instanceName)
            End If
            'add the server to our list 

            listOfServers.Add(serverName)


        Next
        filling = True
        ComboBox1.DataSource = listOfServers
        filling = False


    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox1.SelectedIndexChanged
        If filling = True Then Exit Sub
        Dim listDataBases As List(Of String) = New List(Of String)
        Dim connectString As String
        Dim selectSQL As String
        Dim server As String
        ' Check if user was selected a server to connect
        ComboBox2.DataSource = Nothing
        If ComboBox1.Text = "" Then
            MsgBox("Must select a server")
            Exit Sub
        Else
            server = ComboBox1.Text
        End If
        If RadioButton1.Checked = True Then
            connectString = "Data Source=" & server & " ;Integrated Security=True;Initial Catalog=master"
        Else
            If TextBox1.Text = "" Or TextBox2.Text = "" Then
                MsgBox("Must Provide username and password")
                Exit Sub
                'Uid=myUsername;Pwd=myPassword;

            End If
            connectString = "Data Source=" & server & " ;Uid=" & TextBox1.Text & ";Pwd=" & TextBox2.Text & ";Initial Catalog=master"
        End If



        Using con As New SqlConnection(connectString)
            ' Open connection
            Try
                con.Open()
                'Get databases names in server in a datareader
                selectSQL = "select name from sys.databases;"
                Dim com As SqlCommand = New SqlCommand(selectSQL, con)
                Dim dr As SqlDataReader = com.ExecuteReader()
                While (dr.Read())
                    listDataBases.Add(dr(0).ToString())
                End While
                'Set databases list as combobox’s datasource 
                ComboBox2.DataSource = listDataBases
            Catch ex As Exception
                MsgBox("These settings generated an error, please check and try again")
            End Try

        End Using


    End Sub

    Private Sub RadioButton2_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton2.CheckedChanged
        If RadioButton2.Checked = True Then
            ComboBox2.DataSource = Nothing
        End If
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        If ComboBox1.Text = "" Then
            MsgBox("No server chosen please check and try again")
            Exit Sub
        End If
        If RadioButton2.Checked = True Then
            If TextBox1.Text = "" Then
                MsgBox("No username selected")
                Exit Sub
            End If
            If TextBox2.Text = "" Then
                MsgBox("No password selected")
                Exit Sub
            End If

        End If
        If ComboBox2.Text = "" Then
            MsgBox("No database selected please check and try again")
        End If

        Dim connectstring As String
        If RadioButton1.Checked = True Then
            connectstring = "Data Source=" & ComboBox1.Text & " ;Integrated Security=True;Initial Catalog=" & ComboBox2.Text
        Else
            connectstring = "Data Source=" & ComboBox1.Text & " ;Uid=" & TextBox1.Text & ";Pwd=" & TextBox2.Text & ";Initial Catalog=" & ComboBox2.Text
        End If
        Dim selectSQL As String
        Using con As New SqlConnection(connectstring)
            ' Open connection
            Try
                con.Open()
                If con.State = ConnectionState.Open Then
                    MsgBox("Test Succesful")
                End If
            Catch ex As Exception
                MsgBox("These settings generated an error, please check and try again")
            End Try

        End Using



    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        Dim consect As New ConnectionStringsSection
        Dim ConExists As Boolean = False
        Dim config As System.Configuration.Configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None)
        Dim ConStr As ConnectionStringSettings
        'first up we make sure that the settings file contains a ConnectionStrings area
        If config.Sections("connectionStrings") Is Nothing Then
            config.Sections.Add("connectionStrings", consect)
        Else
            'next we see if the name (PriorityDB) exists as a connection

            For Each ConStr In config.ConnectionStrings.ConnectionStrings
                If ConStr.Name = "PriorityDB" Then
                    ConExists = True
                End If
            Next
        End If
        If ConExists = True Then
            Dim constring As String
            If RadioButton1.Checked = True Then
                constring = "Data Source=" & ComboBox1.Text & " ;Integrated Security=True;Initial Catalog=" & ComboBox2.Text
            Else
                constring = "Data Source=" & ComboBox1.Text & " ;Uid=" & TextBox1.Text & ";Pwd=" & TextBox2.Text & ";Initial Catalog=" & ComboBox2.Text
            End If
            config.ConnectionStrings.ConnectionStrings("PriorityDB").ConnectionString = constring
            config.Save()
        Else
            Dim conStringname As String = "PriorityDB"
            Dim conString As String
            If RadioButton1.Checked = True Then
                conString = "Data Source=" & ComboBox1.Text & " ;Integrated Security=True;Initial Catalog=" & ComboBox2.Text
            Else
                conString = "Data Source=" & ComboBox1.Text & " ;Uid=" & TextBox1.Text & ";Pwd=" & TextBox2.Text & ";Initial Catalog=" & ComboBox2.Text
            End If
            Dim providerName As String = "System.Data.SqlClient"

            Dim connStrSettings As New ConnectionStringSettings()
            connStrSettings.Name = conStringname
            connStrSettings.ConnectionString = conString
            connStrSettings.ProviderName = providerName

            config.ConnectionStrings.ConnectionStrings.Add(connStrSettings)
            consect.SectionInformation.ForceSave = True
            config.Save(ConfigurationSaveMode.Full)

        End If
        Me.DialogResult = Windows.Forms.DialogResult.OK
    End Sub

    Private Sub ComboBox2_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox2.SelectedIndexChanged

    End Sub

    Private Sub frmSettings_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub
End Class