<%@ WebHandler Language="VB" Class="site" %>

Imports System
Imports System.Web
Imports System.Xml
Imports System.IO

Public Class site : Implements IHttpHandler
    
    Private doc As New XmlDocument
    Private cat As New XmlDocument
    Private ShowHidden As Boolean = False
    
    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        With context
            If .Request("show") = "all" Then ShowHidden = True

            Using sr As New StreamReader(.Server.MapPath("cat.xml"))
                cat.LoadXml(sr.ReadToEnd)
            End Using
        
            Using sr As New StreamReader(.Server.MapPath("pages.xml"))
                doc.LoadXml(sr.ReadToEnd)
            End Using

            Dim objX As New XmlTextWriter(context.Response.OutputStream, Nothing)
            With .Response
                .Clear()
                .ContentType = "text/xml"
                .ContentEncoding = Encoding.UTF8
                   
                With objX
                    .WriteStartDocument()
                
                    Dim c As XmlNode = cat.SelectSingleNode("/*[position()=1]")
                    
                    .WriteStartElement("page")
                    
                    WriteCat(objX, c)
                    WritePage(objX, c)
                    
                    For Each n As XmlNode In c.ChildNodes
                        subpages(objX, n)
                    Next
                    
                    .WriteEndElement()
                    
                    .WriteEndDocument()
                    .Flush()
                    .Close()
                End With
                
                .End()
            End With
        End With
        
    End Sub
    
    Private Sub subpages(ByRef objX As XmlTextWriter, ByVal node As XmlNode)
        With objX
            If node.Attributes("showonmenu").Value Or ShowHidden Then
                .WriteStartElement("page")
                WriteCat(objX, node)
                WritePage(objX, node)
            
                For Each n As XmlNode In node.ChildNodes
                    subpages(objX, n)
                Next
            
                .WriteEndElement()
            End If
        End With
    End Sub
     
    Private Sub WritePage(ByRef objX As XmlTextWriter, ByVal c As XmlNode)
        '<page id="4321567890" masterpage="emerge.master" title="Home" description="" keywords="">
        Dim p As XmlNode = doc.SelectSingleNode(String.Format("//page[@id={0}{1}{0}]", Chr(34), c.Attributes("id").Value))
        With objX
            If Not IsNothing(p) Then            
                .WriteAttributeString("loc", p.Attributes("id").Value)
                .WriteAttributeString("pagetitle", p.Attributes("title").Value)
                .WriteAttributeString("description", p.Attributes("description").Value)
                
                '.WriteStartElement("section")
                'If Not IsNothing(p.ChildNodes) Then
                '    For Each ch As XmlNode In p.ChildNodes
                '        .WriteAttributeString("html", ch.Attributes("html").Value)
                '    Next
                'End If
                '.WriteEndElement()
                
            Else
                .WriteAttributeString("loc", "")
                .WriteAttributeString("pagetitle", "")
                .WriteAttributeString("description", "")
            End If        
        End With
    End Sub
    
    Private Sub WriteCat(ByRef objX As XmlTextWriter, ByVal node As XmlNode)
        With objX
            .WriteAttributeString("menutitle", node.Attributes("name").Value)
        End With
    End Sub
    
    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class