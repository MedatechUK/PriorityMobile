Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.Configuration.ConfigurationSettings
Imports System.Data.Sql
Imports System.IO

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
                    CheckBox1.Checked = True
                End If
            Catch ex As Exception
                MsgBox("These settings generated an error, please check and try again")
            End Try

        End Using



    End Sub
    Public ReadOnly Property dir() As String
        Get
            Dim t As String
            t = My.Application.Info.DirectoryPath.ToString
            Return t
        End Get
    End Property
    Public ReadOnly Property SettingsFile()
        Get
            Dim v As String = String.Format("{0}/{1}", dir, "settings.xml")
            Return v
        End Get
    End Property
    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        If CheckBox1.Checked = False Then
            MsgBox("Database settings havent been verified")
            Exit Sub
        End If
        If ComboBox1.Text = "" Then Exit Sub
        If ComboBox2.Text = "" Then Exit Sub
        If ComboBox3.Text = "" Then Exit Sub
        If ComboBox4.Text = "" Then Exit Sub
        If TextBox1.Text = "" Then Exit Sub
        If TextBox2.Text = "" Then Exit Sub

        Dim pt As Integer = 0
        Dim mp As Integer = 0
        Dim int As Integer = 0
        Dim res As Integer = 0
        Select Case ComboBox4.Text
            Case ""
                Exit Sub
            Case "Black and White"
                pt = 0
            Case "Greyscale"
                pt = 1
            Case "RGB Colour"
                pt = 2
            Case "Palette Colour"
                pt = 3
            Case Else
                Exit Sub
        End Select
        mp = NumericUpDown1.Value
        Select Case ComboBox3.Text
            Case ""
                Exit Sub
            Case "Show Interface"
                int = 1
            Case "Hide Interface"
                int = 0
            Case "Modal Interface"
                int = 2
            Case Else
                Exit Sub
        End Select
        res = NumericUpDown2.Value



        Dim d As String
        d = SettingsFile


        If File.Exists(SettingsFile) = True Then
            File.Delete(SettingsFile)
        End If
        Dim xmldoc As New Xml.XmlDocument
        Dim headelement As Xml.XmlElement = xmldoc.CreateElement("Settings")

        xmldoc.AppendChild(headelement)


        Dim e1 As Xml.XmlElement = xmldoc.CreateElement("PixType")
        e1.InnerText = pt
        headelement.AppendChild(e1)

        Dim el2 As Xml.XmlElement = xmldoc.CreateElement("MaxPage")
        el2.InnerText = mp
        headelement.AppendChild(el2)

        Dim el3 As Xml.XmlElement = xmldoc.CreateElement("UsrIntFc")
        el3.InnerText = int
        headelement.AppendChild(el3)

        Dim el4 As Xml.XmlElement = xmldoc.CreateElement("Res")
        el4.InnerText = res
        headelement.AppendChild(el4)

        Dim el5 As Xml.XmlElement = xmldoc.CreateElement("DBServ")
        el5.InnerText = ComboBox1.Text
        headelement.AppendChild(el5)

        Dim el6 As Xml.XmlElement = xmldoc.CreateElement("DBUname")
        el6.InnerText = TextBox1.Text
        headelement.AppendChild(el6)

        Dim el7 As Xml.XmlElement = xmldoc.CreateElement("DBPass")
        el7.InnerText = TextBox2.Text
        headelement.AppendChild(el7)

        Dim el8 As Xml.XmlElement = xmldoc.CreateElement("DBName")
        el8.InnerText = ComboBox2.Text
        headelement.AppendChild(el8)

        xmldoc.Save(SettingsFile)

        Me.DialogResult = Windows.Forms.DialogResult.OK
    End Sub

    Private Sub ComboBox2_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox2.SelectedIndexChanged

    End Sub

    Private Sub frmSettings_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub
End Class