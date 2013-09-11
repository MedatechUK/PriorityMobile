Imports System.Xml
Imports System.io

Partial Class testpost
    Inherits System.Web.UI.Page

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click

        Dim strPostURL = "http://127.0.0.1:8080/postxml.ashx"
        Dim xmldata As String = Me.xmldata.Text

        Dim requestStream As Stream = Nothing        
        Dim uploadResponse As Net.HttpWebResponse = Nothing
        Dim myEncoder As New System.Text.ASCIIEncoding
        Dim bytes As Byte() = myEncoder.GetBytes(xmldata)
        Dim ms As MemoryStream = New MemoryStream(bytes)

        Try

            Dim uploadRequest As Net.HttpWebRequest = CType(Net.HttpWebRequest.Create(strPostURL), Net.HttpWebRequest)
            uploadRequest.Method = Net.WebRequestMethods.Http.Post
            uploadRequest.Proxy = Nothing
            requestStream = uploadRequest.GetRequestStream()

            ' Upload the XML
            Dim buffer(1024) As Byte
            Dim bytesRead As Integer
            While True
                bytesRead = ms.Read(buffer, 0, buffer.Length)
                If bytesRead = 0 Then
                    Exit While
                End If
                requestStream.Write(buffer, 0, bytesRead)
            End While

            ' The request stream must be closed before getting the response.
            requestStream.Close()

            uploadResponse = uploadRequest.GetResponse()

            Dim thisRequest As New XmlDocument
            With Context
                Dim reader As New StreamReader(uploadResponse.GetResponseStream)
                With thisRequest
                    .LoadXml(reader.ReadToEnd)
                    Dim n As XmlNode = .SelectSingleNode("response")
                    For Each attrib As XmlAttribute In n.Attributes
                        Response.Write(attrib.Name & ": " & attrib.Value & "<br>")
                    Next
                End With
            End With

        Catch ex As UriFormatException
            Response.Write(ex.Message)
        Catch ex As Net.WebException
            Response.Write(ex.Message)
        Finally
            If uploadResponse IsNot Nothing Then
                uploadResponse.Close()
            End If
            If requestStream IsNot Nothing Then
                requestStream.Close()
            End If
        End Try

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim fText As String = ""
        If Not IsNothing(Request("Order")) Then
            Dim filename As String = String.Format("{0}orders/{1}.xml", Server.MapPath("/"), Request("Order"))
            If File.Exists(filename) Then
                Using sr As New StreamReader(filename)
                    fText = sr.ReadToEnd
                End Using
            End If
            Me.xmldata.Text = fText
        End If
    End Sub

End Class
