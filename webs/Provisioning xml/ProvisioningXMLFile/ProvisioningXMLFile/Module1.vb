Imports System.IO
Imports System.Xml
Module Module1
#Region "Variables and Properties"

#Region "XML file / Stream locations"
    'This is the location of the provisioning feed
    Private ReadOnly Property ProvFeed() As String
        Get
            Return "http://mobile.emerge-it.co.uk:8080/priority_Provsion.ashx"
        End Get
    End Property

    'This is the location of any extra users that need to be added
    Private ReadOnly Property StaticFileLoc() As String
        Get
            Return "http://soti.emerge-it.co.uk/client/static_provision.xml"
        End Get
    End Property

#End Region

#Region "File Properties - used to hold the filenames / and file path"
    Private FilePath As String = String.Format("{0}{1}", Directory.GetCurrentDirectory, "/")
    Private Property path() As String
        Get
            Return FilePath
        End Get
        Set(ByVal value As String)
            FilePath = value
        End Set
    End Property

    Private ReadOnly Property fname() As String
        Get
            Return "provision2.xml"
        End Get
    End Property

    Private ReadOnly Property full_filename() As String
        Get
            Return String.Format("{0}{1}", path, fname)
        End Get
    End Property

    Private ReadOnly Property Full_Backup() As String
        Get
            Return String.Format("{0}{1}", path, backup_file)
        End Get
    End Property

    Public ReadOnly Property backup_file() As String
        Get
            Return fname.Replace("xml", "bak")
        End Get
    End Property

#End Region

#Region "XML Properties - holds static xml variables that are used in the main loop"

    Private u As XmlElement
    Private ur As XmlElement
    Private pstr As XmlAttribute
    Private un As XmlElement

    Public ReadOnly Property setting() As XmlWriterSettings
        Get
            Dim s As XmlWriterSettings
            s = New XmlWriterSettings
            s.Indent = True
            Return s
        End Get
    End Property

    Public Property user() As XmlElement
        Get
            Return u
        End Get
        Set(ByVal value As XmlElement)
            u = value
        End Set
    End Property

    Public Property ProvString() As XmlAttribute
        Get
            Return pstr
        End Get
        Set(ByVal value As XmlAttribute)
            pstr = value
        End Set
    End Property

    Public Property username() As XmlElement
        Get
            Return un
        End Get
        Set(ByVal value As XmlElement)
            un = value
        End Set
    End Property

    Public Property url() As XmlElement
        Get
            Return ur
        End Get
        Set(ByVal value As XmlElement)
            ur = value
        End Set
    End Property

#End Region

#End Region

    Sub Main(ByVal Args As String())

        Dim ArgsCheck As Boolean = GetFile(Args)
        If ArgsCheck = True Then
            Dim Prixmldata As New XmlDocument
            Dim LocalXMLData As New XmlDocument

            Try
                Dim EX As Exception = Backup_Provision()
                If Not IsNothing(EX) Then Throw EX

                Prixmldata.Load(ProvFeed)
                LocalXMLData.Load(StaticFileLoc)

                For Each Localnode As XmlElement In LocalXMLData.SelectNodes("/devices")
                    For Each usernode As XmlNode In Localnode

                        user = Prixmldata.CreateElement("user")

                        ProvString = Prixmldata.CreateAttribute("ProvisionString")
                        ProvString.InnerText = usernode.Attributes(0).Value

                        url = Prixmldata.CreateElement("url")
                        url.InnerText = usernode.SelectSingleNode("url").InnerText

                        username = Prixmldata.CreateElement("username")
                        username.InnerText = usernode.SelectSingleNode("username").InnerText

                        With user
                            .AppendChild(username)
                            .AppendChild(url)
                            .Attributes.Append(ProvString)
                        End With

                        Prixmldata.DocumentElement.AppendChild(user)

                    Next
                Next

                ' Save to file
                Prixmldata.Save(full_filename)

            Catch exep As Exception
                Console.Write("Error : {0}", exep.Message)
            End Try
        Else
            Console.WriteLine("There was an error with the arguments supplied")
        End If


    End Sub

    Private Function GetFile(ByVal args As String()) As Boolean

        If args.Length <> 2 Then
            Return False
        End If

        If args(0).ToUpper <> "/FILE" Then
            Return False
        End If

        If Directory.Exists(args(1).ToString) = False Then
            Console.WriteLine("Specified Directory does not exist")
            Return False
        End If

        Dim CheckLast As Char
        CheckLast = args(1).Substring(args(1).Length - 1)

        If CheckLast <> "/" Or CheckLast <> "\" Then
            args(1) = String.Format("{0}{1}", args(1), "\")
        End If

        path = args(1)
        Return True

    End Function


    Private Function Backup_Provision() As Exception

        Try
            If File.Exists(Full_Backup) = True Then
                File.Delete(Full_Backup)
            End If

            If File.Exists(full_filename) = True Then
                File.Move(full_filename, backup_file)
            End If

            Return Nothing

        Catch excep As Exception
            Return excep
        End Try

    End Function
End Module
