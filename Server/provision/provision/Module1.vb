Imports System.Xml

Module Module1

    Private ProvisionFile As String = "provision.xml"
    Private ProvisionXML As XmlDocument

    Private ReadOnly Property ProvisionString(ByVal pnode As XmlNode) As String
        Get            
            Return pnode.Attributes("ProvisionString").Value
        End Get
    End Property

    Private ReadOnly Property Username(ByVal pnode As XmlNode) As String
        Get
            For Each p As XmlNode In pnode.ChildNodes
                If p.Name = "username" Then
                    Return p.InnerText
                End If
            Next
            Return Nothing
        End Get
    End Property

    Private ReadOnly Property Url(ByVal pnode As XmlNode) As String
        Get
            For Each p As XmlNode In pnode.ChildNodes
                If p.Name = "url" Then
                    Return p.InnerText
                End If
            Next
            Return Nothing
        End Get
    End Property

    Private ReadOnly Property NewProvisionString() As String
        Get
            Dim ret As String
            Dim pnode As XmlNode
            Do
                ret = System.Guid.NewGuid.ToString.Split("-").First.ToUpper
                pnode = ProvisionXML.SelectSingleNode(String.Format("devices/user[@ProvisionString={0}{1}{0}]", Chr(34), ret))
            Loop While Not (IsNothing(pnode))
            Return ret
        End Get
    End Property

    Private Function UserNode(ByVal UserName As String, ByVal Url As String) As XmlNode

        Dim pnode As XmlNode = Nothing
        Try
            pnode = ProvisionXML.SelectSingleNode( _
                String.Format( _
                    "devices/user[username={0}{1}{0} and url={0}{2}{0}]", _
                        Chr(34), _
                        UserName, _
                        Url _
                    ) _
                )
        Catch
        End Try

        If Not (IsNothing(pnode)) Then
            Return pnode
        Else

            pnode = ProvisionXML.CreateNode(XmlNodeType.Element, "user", "")
            With pnode
                .Attributes.Append(ProvisionXML.CreateAttribute("ProvisionString"))
                .Attributes("ProvisionString").Value = NewProvisionString
                .AppendChild(ProvisionXML.CreateElement("username"))
                .LastChild.InnerText = UserName
                .AppendChild(ProvisionXML.CreateElement("url"))
                .LastChild.InnerText = Url
            End With

            ProvisionXML.SelectSingleNode("devices").AppendChild(pnode)

            Return ProvisionXML.SelectSingleNode( _
                String.Format( _
                    "devices/user[username={0}{1}{0} and url={0}{2}{0}]", _
                        Chr(34), _
                        UserName, _
                        Url _
                    ) _
                )
        End If

    End Function

    Sub Main()

        ProvisionXML = New XmlDocument
        Try
            ProvisionXML.Load(ProvisionFile)
        Catch ex As Exception
            ProvisionXML.ParentNode.AppendChild(ProvisionXML.CreateElement("devices"))
        End Try

        Using sr As New System.IO.StreamReader("c:\users.txt")
            Do Until sr.EndOfStream
                Dim pnode As XmlNode = UserNode(sr.ReadLine, "http://owa.virginstrausswater.com:8080/")
                Console.WriteLine("{0} {1} {2}", Username(pnode), Url(pnode), ProvisionString(pnode))
            Loop
            ProvisionXML.Save(ProvisionFile)
        End Using


    End Sub

End Module
