Imports System.IO
Imports System.Xml
Imports System.Web
Public Class Form1
    Private path As String = Directory.GetCurrentDirectory & "\"
    Private fname As String = "provision2.xml"
    Private full_filename As String = path & fname

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim responseData As String


        If File.Exists(path & "provision2.bak") = True Then
            File.Delete(path & "provision2.bak")
        End If
        If File.Exists(full_filename) = True Then
            'File.Replace("c:\provision2.xml", "c:\provision2.bak", "c:\provisbak.bak")
            My.Computer.FileSystem.RenameFile(full_filename, "provision2.bak")
        End If
        Dim settings As XmlWriterSettings = New XmlWriterSettings()
        settings.Indent = True

        Dim objX As XmlWriter
        objX = XmlWriter.Create(full_filename, settings)
        With objX



            Try
                Dim xmlData As New XmlDocument
                Dim hwrequest As Net.HttpWebRequest = Net.WebRequest.Create("http://mobile.emerge-it.co.uk:8080/provxml.ashx")
                hwrequest.AllowAutoRedirect = True
                hwrequest.UserAgent = "http_requester/0.1"
                hwrequest.Timeout = 60000
                hwrequest.Method = "GET"

                Dim hwresponse As Net.HttpWebResponse = hwrequest.GetResponse()
                If hwresponse.StatusCode = Net.HttpStatusCode.OK Then
                    Dim responseStream As IO.StreamReader = _
                      New IO.StreamReader(hwresponse.GetResponseStream())
                    responseData = responseStream.ReadToEnd()
                End If
                hwresponse.Close()
                xmlData.LoadXml(responseData)
                xmlData.WriteTo(objX)



            Catch ex As Exception
                responseData = "An error occurred: " & ex.Message
            Finally
                '.WriteEndElement()
                '.WriteEndDocument()
                .Flush()
                .Close()

            End Try
        End With
        Dim d As Boolean
        d = getSettings()
        Me.Close()

    End Sub
    Public Function getSettings() As Boolean
        File.Delete(path & "testdata.xml")
        Try
            My.Computer.Network.DownloadFile("http://soti.emerge-it.co.uk/client/test.xml", path & "testdata.xml")
        Catch ex As Exception
            MsgBox("couldnt download")
        End Try

        Try
            If File.Exists(path & "testdata.xml") = False Then

                Return False
            Else

            End If
            Dim doc As New XmlDocument
            doc.Load(full_filename)
            Dim newxml As New XmlDocument
            newxml.Load(path & "testdata.xml")

            Dim nodelist As XmlNodeList = newxml.SelectNodes("/devices")
            For Each node As XmlElement In nodelist
                Dim j As String
                j = node.Name
                For Each usernode As XmlNode In node
                    Dim pstring, urlstr, uname As String
                    pstring = usernode.Attributes.Item(0).InnerText
                    For Each k As XmlNode In usernode
                        Select Case k.Name
                            Case "url"
                                urlstr = k.InnerText
                            Case "username"
                                uname = k.InnerText
                        End Select

                    Next
                    Dim hh As String
                    hh = usernode.Name
                    Dim user As XmlElement = doc.CreateElement("user")
                    Dim ProvString As XmlAttribute = doc.CreateAttribute("ProvisionString")
                    ProvString.InnerText = pstring
                    Dim username As XmlElement = doc.CreateElement("username")
                    username.InnerText = uname
                    Dim url As XmlElement = doc.CreateElement("url")
                    url.InnerText = urlstr
                    user.AppendChild(username)
                    user.AppendChild(url)
                    user.Attributes.Append(ProvString)
                    doc.DocumentElement.AppendChild(user)
                Next

            Next

            doc.Save(full_filename)


        Catch ex As Exception
            Return False
        End Try
        Return True
    End Function
End Class
