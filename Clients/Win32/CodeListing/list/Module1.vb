Imports System.IO
Imports System.Text.RegularExpressions

Module Module1

    Private Solutions As New Dictionary(Of String, Solution)

    Sub Main()

        Solutions.Add("Shared", New Solution())

        For i As Integer = 1 To UBound(Environment.GetCommandLineArgs)
            Solutions.Add( _
                Environment.GetCommandLineArgs(i).Substring(Environment.GetCommandLineArgs(i).LastIndexOf("\")), _
                New Solution(Solutions, Environment.GetCommandLineArgs(i)) _
            )
        Next

        Using sw As New StreamWriter("M:\Clients\Win32\CodeListing\list\codetest.htm")

            sw.WriteLine("<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.1//EN' 'http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd'>")
            sw.WriteLine("<html xmlns='http://www.w3.org/1999/xhtml' >")
            sw.WriteLine("<head>")
            sw.WriteLine("<title>Untitled Page</title>")
            sw.WriteLine("<link href='style.css' type='text/css' rel='stylesheet'/>")
            sw.WriteLine("</head>")
            sw.WriteLine("<body>")

            For Each s As String In Solutions.Keys
                If Not s = "Shared" Then
                    sw.WriteLine("<div class=""folder""><h1 class=""tab{0}"">Solution: {1}</h1></div>", 1, Solutions(s).Name)
                    For Each p As Project In Solutions(s).Projects.Values
                        sw.WriteLine("<div class=""folder""><h2 class=""tab{0}"">Project: {1}</h2></div>", 2, p.Title)
                        For Each fld As Folder In p.Values
                            SubFolders(fld, sw, p)
                        Next
                    Next
                End If
            Next
            sw.WriteLine("<div class=""folder""><h1 class=""tab{0}"">{1}</h1></div>", 1, Solutions("Shared").Name)
            For Each p As Project In Solutions("Shared").Projects.Values
                sw.WriteLine("<div class=""folder""><h2 class=""tab{0}"">Project: {1}</h2></div>", 2, p.Title)
                For Each fld As Folder In p.Values
                    SubFolders(fld, sw, p)
                Next
            Next

            sw.WriteLine("</body>")
            sw.WriteLine("</html>")

        End Using
        Console.WriteLine("Finished!")
        Console.ReadKey()

    End Sub

    Public Sub SubFolders(ByVal fld As Folder, ByRef sw As StreamWriter, ByRef p As Project)

        If fld.Name.Length > 0 Then
            sw.WriteLine("<div class=""folder""><h3 class=""tab{0}"">\{1}</h3></div>", fld.Depth + 1, fld.Name)
        End If
        For Each sf As Folder In fld.Values
            SubFolders(sf, sw, p)
        Next

        For Each f As CodeFile In fld.Files.Values
            sw.WriteLine("<div class=""folder""><h3 class=""tab{0}"">+{1}</h3></div>", fld.Depth + 2, f.Name)
            sw.WriteLine("<div class=""folder""><h3 class=""tab{0}"">{1}</h3></div>", fld.Depth + 2, f.CodeData)
        Next

    End Sub

End Module
