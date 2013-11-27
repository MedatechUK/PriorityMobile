Imports System.IO
Imports System.Xml

Module Module1

    '-x c:\eve.xml -u http://dev.emerge-it.co.uk:8080/posthandler.ashx

    Private arg As clArg
    Private doc As XmlDocument

    Sub Main(ByVal args As String())

        doc = New XmlDocument
        arg = New clArg(args)

        Try
            With arg.Keys
                If .Contains("?") Or .Count = 0 Then
                    Help()
                Else
                    If Not .Contains("u") Then _
                        Throw New Exception("Missing URL.")

                    If (.Contains("x") And .Contains("f")) Or (Not .Contains("x") And Not .Contains("f")) Then _
                        Throw New Exception("Please specify either -(x)ml file OR -(f)older.")
                    If .Contains("f") Then
                        If Not Directory.Exists(arg("f")) Then _
                            Throw New Exception(String.Format("Invalid folder specified [{0}].", arg("f")))
                    End If
                    If .Contains("x") Then
                        If Not File.Exists(arg("x")) Then _
                            Throw New Exception(String.Format("Invalid XML File specified [{0}].", arg("f")))
                    End If

                    If .Contains("m") And .Contains("d") Then _
                        Throw New Exception("Please specify either -(m)ove OR -(d)elete.")
                    If .Contains("m") Then
                        If Not Directory.Exists(arg("m")) Then _
                            Throw New Exception("Invalid move folder.")
                    End If

                    If Not .Contains("t") Then
                        arg.Add("t", "*.xml")
                    End If

                    If .Contains("x") Then
                        If LoadDoc(arg("x"), True) Then
                            Console.WriteLine("Sending [{0}] to [{1}]...", arg("x"), arg("u"))
                            If Post(arg("u"), doc.OuterXml, True) Then
                                If .Contains("m") Then
                                    Move(arg("x"), arg("m"))
                                ElseIf .Contains("d") Then
                                    Delete(arg("x"))
                                End If
                            End If
                        End If

                    ElseIf .Contains("f") Then
                        For Each F As String In Directory.GetFiles(arg("f"), arg("t"))
                            If LoadDoc(F) Then
                                Console.WriteLine("Sending [{0}] to [{1}]...", F, arg("u"))
                                If Post(arg("u"), doc.OuterXml) Then
                                    If .Contains("m") Then
                                        Move(F, arg("m"))
                                    ElseIf .Contains("d") Then
                                        Delete(F)
                                    End If
                                End If
                            End If
                        Next

                    End If

                End If

            End With

        Catch EX As Exception
            Console.WriteLine(EX.Message)            
        End Try

    End Sub

    Private Sub Help()
        Dim syntax As New Dictionary(Of String, String)
        With syntax
            .Add("-u [Url]", "Url of the xml handler")
            .Add("(-x [file]|-f [dir])", "The file or folder to transmit")
            .Add("{(-m [dir]|-d)}", "Optional: Move (to dir) or delete the files after transmission")
            .Add("{-t}", "Optional: File pattern to search -f folder (default .xml)")
        End With
        Console.WriteLine()
        Console.WriteLine("Syntax:")
        Console.WriteLine()
        Console.Write("  {0}.exe ", System.Reflection.Assembly.GetExecutingAssembly.GetName.Name)
        For Each k As String In syntax.Keys
            Console.Write("{0} ", k)
        Next
        Console.WriteLine()
        Console.WriteLine()
        For Each k As String In syntax.Keys
            Console.WriteLine("  {0}", k)
            Console.WriteLine("     {0}", syntax(k))
            Console.WriteLine()
        Next
        Console.WriteLine()
        Console.ReadKey()
    End Sub

    Private Function LoadDoc(ByVal Spec As String, Optional ByVal throwex As Boolean = False) As Boolean
        Dim ret As Boolean = True
        Try
            doc.Load(Spec)
        Catch ldExcep As Exception
            ret = False
            Select Case throwex
                Case True
                    Throw New Exception( _
                        String.Format( _
                            "Error opening [{0}]: {1}", _
                            arg("x"), _
                            ldExcep.Message _
                        ) _
                    )
                Case Else
                    Console.WriteLine( _
                        "Error opening [{0}]: {1}", _
                        arg("x"), _
                        ldExcep.Message _
                    )
            End Select

        End Try
        Return ret
    End Function

    Private Sub Move(ByVal Source As String, ByVal Dest As String)
        If File.Exists( _
            String.Format("{0}\{1}", _
                Dest, Source.Split("\").Last _
            ) _
        ) Then
            Delete(String.Format("{0}\{1}", _
                Dest, Source.Split("\").Last _
            ) _
        )
        End If
        File.Move( _
            Source, String.Format( _
                "{0}\{1}", _
                Dest, _
                Source.Split("\").Last _
            ) _
        )
    End Sub

    Private Sub Delete(ByVal Filename As String)
        While File.Exists(Filename)
            Try
                File.Delete(Filename)
            Catch
                System.Threading.Thread.Sleep(100)
            End Try
        End While
    End Sub

    Private Function Post(ByVal Server As String, ByVal xmldata As String, Optional ByVal throwErr As Boolean = False) As Boolean

        Dim ret As Boolean = True
        Dim requestStream As Stream = Nothing
        Dim uploadResponse As Net.HttpWebResponse = Nothing
        Dim myEncoder As New System.Text.ASCIIEncoding
        Dim bytes As Byte() = myEncoder.GetBytes(xmldata)
        Dim ms As MemoryStream = New MemoryStream(bytes)

        Try

            Dim uploadRequest As Net.HttpWebRequest = CType(Net.HttpWebRequest.Create(Server), Net.HttpWebRequest)
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
            Dim reader As New StreamReader(uploadResponse.GetResponseStream)
            With thisRequest
                .LoadXml(reader.ReadToEnd)
                Dim n As XmlNode = .SelectSingleNode("response")
                For Each attrib As XmlAttribute In n.Attributes
                    Console.WriteLine(attrib.Name & ": " & attrib.Value)
                Next
            End With

        Catch uex As UriFormatException
            ret = False
            If throwErr Then
                Throw New Exception( _
                    String.Format( _
                        "Malformed URL [{0}]. {1}", _
                        Server, _
                        uex.Message _
                    ) _
                )
            Else
                Console.WriteLine( _
                        "Malformed URL [{0}]. {1}", _
                        Server, _
                        uex.Message _
                    )
            End If

        Catch uex As Net.WebException
            ret = False
            If throwErr Then
                Throw New Exception( _
                    String.Format( _
                        "Web Error connecting to [{0}]. {1}", _
                        Server, _
                        uex.Message _
                    ) _
                )
            Else
                Console.WriteLine( _
                        "Web Error connecting to [{0}]. {1}", _
                        Server, _
                        uex.Message _
                    )
            End If

        Finally
            If uploadResponse IsNot Nothing Then
                uploadResponse.Close()
            End If
            If requestStream IsNot Nothing Then
                requestStream.Close()
            End If
        End Try

        Return ret

    End Function

End Module
