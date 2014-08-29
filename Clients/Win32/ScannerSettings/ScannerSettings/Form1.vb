Imports System.Data.Sql
Imports System.Data.SqlClient
Imports System.IO
Imports System.Text

Public Class Form1
#Region "Properties"
    Private ptype As Integer
    Private mpAG As Integer
    Private ui As Integer
    Private resol As Integer
    Private ConnString As String
    Private Server As String
    Private DataBase As String
    Private UName As String
    Private PassWord As String
    Private _CopyToLoc As String

    Public Property PagType() As Integer
        Get
            Return ptype
        End Get
        Set(ByVal value As Integer)
            ptype = value
        End Set
    End Property
    Public Property mpg() As Integer
        Get
            Return mpAG
        End Get
        Set(ByVal value As Integer)
            mpAG = value
        End Set
    End Property
    Public Property uid() As Integer
        Get
            Return ui
        End Get
        Set(ByVal value As Integer)
            ui = value
        End Set
    End Property
    Public Property resolu() As Integer
        Get
            Return resol
        End Get
        Set(ByVal value As Integer)
            resol = value
        End Set
    End Property
    Public Property DBServ() As String
        Get
            Return Server
        End Get
        Set(ByVal value As String)
            Server = value
        End Set
    End Property
    Public Property DBName() As String
        Get
            Return DataBase
        End Get
        Set(ByVal value As String)
            DataBase = value
        End Set
    End Property
    Public Property DBUname() As String
        Get
            Return UName
        End Get
        Set(ByVal value As String)
            UName = value
        End Set
    End Property
    Public Property DBPassword() As String
        Get
            Return PassWord
        End Get
        Set(ByVal value As String)
            PassWord = value
        End Set
    End Property
    Public ReadOnly Property ConStr() As String
        Get
            Return String.Format("Data Source={0} ;Uid={1};Pwd={2};Initial Catalog={3}", DBServ, DBUname, DBPassword, DBName)
        End Get
    End Property
    Public ReadOnly Property dir() As String
        Get
            Return My.Application.Info.DirectoryPath
        End Get
    End Property
    Public ReadOnly Property SettingsFile()
        Get
            Return String.Format("{0}\{1}", dir, "settings.xml")
        End Get
    End Property
    Public ReadOnly Property FilesDir()
        Get
            Return String.Format("{0}\{1}", dir, "source\")
        End Get
    End Property
    Public Property CopyToLoc()
        Get
            Return _CopyToLoc
        End Get
        Set(ByVal value)
            _CopyToLoc = value
        End Set
    End Property
#End Region
#Region "Functions"
    Private Declare Auto Function GetPrivateProfileString Lib "kernel32" (ByVal lpAppName As String, _
               ByVal lpKeyName As String, _
               ByVal lpDefault As String, _
               ByVal lpReturnedString As StringBuilder, _
               ByVal nSize As Integer, _
               ByVal lpFileName As String) As Integer
#End Region


    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        If FolderBrowserDialog1.ShowDialog() = DialogResult.OK Then
            TextBox3.Text = FolderBrowserDialog1.SelectedPath & "/"
            TextBox3.Enabled = False
        End If
    End Sub
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

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
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
        If TextBox3.Text = "" Then Exit Sub


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
        CopyToLoc = TextBox3.Text


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

        Dim el9 As Xml.XmlElement = xmldoc.CreateElement("CopyToLoc")
        el9.InnerText = TextBox3.Text
        headelement.AppendChild(el9)

        xmldoc.Save(SettingsFile)
        xmldoc.Save(CopyToLoc & "\Settings.xml")
        Dim direc As New IO.DirectoryInfo(FilesDir)
        Dim allFiles As IO.FileInfo() = direc.GetFiles("*.*")
        Dim singleFile As IO.FileInfo
        Try
            For Each singleFile In allFiles
                Console.WriteLine(singleFile.FullName)

                If File.Exists(singleFile.FullName) Then
                    'singleFile.Delete()
                    singleFile.CopyTo(CopyToLoc & singleFile.Name, True)
                End If
            Next
            MessageBox.Show("Files Deployed", "The files have been deployed to the folder specified")
        Catch ex As Exception
            MsgBox("Error copying files")
        End Try

       

    End Sub

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim pt, int As String
        pt = ""
        int = ""

        If File.Exists(SettingsFile) = False Then
            PagType = 0
            pt = "Black and White"
            mpg = -1
            uid = 0
            int = "Show Interface"
            Dim res As Integer
            Dim sb As StringBuilder

            sb = New StringBuilder(500)
            If File.Exists("c:\Windows\tabula.ini") = True Then
                res = GetPrivateProfileString("Environment", "Tabula Host", "", sb, sb.Capacity, "c:\Windows\tabula.ini")
                DBServ = sb.ToString()
                res = GetPrivateProfileString("Environment", "Tabula Path", "", sb, sb.Capacity, "c:\Windows\tabula.ini")
                TextBox3.Text = sb.ToString()
            Else
                DBServ = ""
                TextBox3.Text = ""

            End If
            resolu = 72

            DBUname = "tabula"
            DBPassword = "Tabula!"
            DBName = "Demo"
        Else
            Dim doc As New XDocument
            doc = XDocument.Load(SettingsFile)
            PagType = doc.<Settings>.<PixType>.Value
            mpg = doc.<Settings>.<MaxPage>.Value
            uid = doc.<Settings>.<UsrIntFc>.Value
            resol = doc.<Settings>.<Res>.Value
            DBServ = doc.<Settings>.<DBServ>.Value
            DBUname = doc.<Settings>.<DBUname>.Value
            DBPassword = doc.<Settings>.<DBPass>.Value
            DBName = doc.<Settings>.<DBName>.Value
            CopyToLoc = doc.<Settings>.<CopyToLoc>.Value
            Select Case PagType
                
                Case 0
                    pt = "Black and White"
                Case 1
                    pt = "Greyscale"
                Case 2
                    pt = "RGB Colour"
                Case 3
                    pt = "Palette Colour"
                Case Else
                    Exit Sub
            End Select



            Select Case uid
                
                Case 1
                    int = "Show Interface"
                Case 0
                    int = "Hide Interface"
                Case 2
                    int = "Modal Interface"
                Case Else
                    Exit Sub
            End Select


        End If
        TextBox1.Text = DBUname
        TextBox2.Text = DBPassword
        TextBox3.Text = CopyToLoc
        ComboBox1.Text = DBServ
        ComboBox2.Text = DBName
        ComboBox4.Text = pt
        ComboBox3.Text = int
        NumericUpDown1.Value = mpg
        NumericUpDown2.Value = resolu
    End Sub
End Class
