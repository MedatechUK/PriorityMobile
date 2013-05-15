Imports HtmlAgilityPack

Module htmlIndent

    Sub Main()
        HtmlNode.ElementsFlags.Remove("form")
        Dim inFile As String
        Dim args() As String = System.Environment.GetCommandLineArgs

        Try
            inFile = args(1)
        Catch
            Console.WriteLine("This utility takes as input a malformed html document (for example, a Priority report), and automatically indents it to make it more human readable.")
            inFile = Console.ReadLine()
        End Try

        Dim doc As New HtmlDocument

        Try
            doc.Load(inFile)
        Catch ex As Exception
            Console.WriteLine(ex.Message)
            Console.ReadLine()
        End Try

        doc.OptionOutputAsXml = True

        Dim xmlDoc As New Xml.XmlDocument

        xmlDoc.LoadXml(doc.DocumentNode.OuterHtml)

        Using ms As New System.IO.MemoryStream

            Using xtw As New Xml.XmlTextWriter(ms, System.Text.Encoding.Unicode)

                xtw.Indentation = 4
                xtw.Formatting = Xml.Formatting.Indented
                xmlDoc.WriteContentTo(xtw)
                xtw.Flush()

                ms.Seek(0, System.IO.SeekOrigin.Begin)

                Using sr As New System.IO.StreamReader(ms)

                    Using sw As New System.IO.StreamWriter(inFile)
                        'handles problems with converting html to xml. Probably a better way to do this, but it seems to work for common problems.
                        Dim corrected As String = sr.ReadToEnd.Replace("&gt;", ">").Replace("&lt;", "<").Replace("&amp;nbsp;", "&nbsp;").Replace("&quot;", """").Replace("//]]>//", "-->").Replace("//<![CDATA[", "<!--")

                        sw.Write(corrected)
                    End Using
                End Using
            End Using
        End Using
    End Sub

End Module
