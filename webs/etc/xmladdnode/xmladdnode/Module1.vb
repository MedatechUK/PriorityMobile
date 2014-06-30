Imports System.Xml
Imports System.Web
Imports System.Net

Module Module1

    'this code generates a new ID from string input and checks for extant nodes with the same ID.
    'things you must do to make this work: 

    '1) update all extant cat & pages files through this code.
    '2) implement this at node creation time 
    '3) implement update to id at cat & page level 


    Dim cat As New XmlDocument
    Dim pages As New XmlDocument

    Sub Main()
        'produce ID 
        cat.Load("..\..\cat.xml")


        'load cat and pages 
        'iterate over each cat node taking id and searching pages by the value thereof. 
        'update pages then update cat. 
        'does name in pages equate to des in cat??? 




        pages.Load("..\..\pages.xml")
        Dim i As Integer = 0
        For Each c As XmlNode In cat.SelectNodes("//cat")
            Dim x As String = c.Attributes("id").Value
            Dim p As XmlNode = pages.SelectSingleNode(String.Format("//page[@id={0}{1}{0}]", Chr(34), x))

            Dim tmpID As String = NewID(c.Attributes("name").Value)
            c.Attributes("id").Value = tmpID

            If p IsNot Nothing Then
                p.Attributes("id").Value = tmpID
            End If

            i += 1



        Next
        Console.WriteLine(i & " nodes to be updated")
        Console.ReadLine()
        cat.Save("..\..\cat.xml")
        pages.Save("..\..\pages.xml")



        Console.ReadLine()
    End Sub

    Function NewID(ByVal title As String) As String
        Dim id As String = URLify(title)
        Dim suffix As Integer = 1
        Dim tmpID As String = id
        While True
            If cat.SelectSingleNode(String.Format("//cat[@id={0}{1}{0}]", Chr(34), tmpID)) Is Nothing Then
                Return tmpID
            Else
                tmpID = String.Format("{0}_{1}", id, suffix)
                suffix += 1
            End If
        End While
    End Function

    Function URLify(ByVal title As String) As String
        Dim ret As String = ""
        ret = RTrim(title)
        ret = LTrim(ret)
        ret = ret.ToLower
        ret = ret.Replace(" ", "-")
        ret = WebUtility.UrlEncode(ret)
        Return ret
    End Function

    Sub UpdateXML()
        Dim i As Integer = 0
        For Each n As XmlNode In cat.SelectNodes("//page")
            Dim a As XmlAttribute = cat.CreateAttribute("meta_title")
            a.Value = n.Attributes("title").Value
            n.Attributes.Append(a)
            i += 1
        Next
        cat.Save("..\..\this.xml")
        Console.WriteLine(i & " nodes updated.")
        Console.ReadLine()
    End Sub

End Module
