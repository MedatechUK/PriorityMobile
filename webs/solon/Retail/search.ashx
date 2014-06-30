<%@ WebHandler Language="VB" Class="SiteSearch" %>

Imports System
Imports System.Web
Imports System.Xml
Imports System.IO

Public Class SiteSearch : Implements IHttpHandler
    private xmlparts As new xmldocument
    Private doc As New XmlDocument
    Private cat As New XmlDocument
    Private ShowHidden As Boolean = False
    
    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        With context
    
            Dim sections As XmlNodeList
            Dim Found As New List(Of String)
            doc.Load(ConfigurationManager.AppSettings.Get("URL") & "/pages.xml")
            
            
            sections = doc.SelectNodes(String.Format("//section[contains(translate(@html, 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'), '{1}')]", Chr(34), context.Request("s").ToLower))
            For Each n As XmlNode In sections
                If Not Found.Contains(n.ParentNode.Attributes("id").Value) Then
                    Found.Add(n.ParentNode.Attributes("id").Value)
                End If
            Next
            sections = doc.SelectNodes(String.Format("//page[contains(translate(@title, 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'), '{1}')]", Chr(34), context.Request("s").ToLower))
            For Each n As XmlNode In sections
                If Not Found.Contains(n.Attributes("id").Value) Then
                    Found.Add(n.Attributes("id").Value)
                End If
            Next
            sections = doc.SelectNodes(String.Format("//page[contains(translate(@description, 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'), '{1}')]", Chr(34), context.Request("s").ToLower))
            For Each n As XmlNode In sections
                If Not Found.Contains(n.Attributes("id").Value) Then
                    Found.Add(n.Attributes("id").Value)
                End If
            Next
            sections = doc.SelectNodes(String.Format("//page[contains(translate(@keywords, 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'), '{1}')]", Chr(34), context.Request("s").ToLower))
            For Each n As XmlNode In sections
                If Not Found.Contains(n.Attributes("id").Value) Then
                    Found.Add(n.Attributes("id").Value)
                End If
            Next
            
            Dim PartsFound As New List(Of String)
            Using reader As XmlTextReader = New XmlTextReader(ConfigurationManager.AppSettings.Get("PartFeedURL"))
                xmlparts.Load(reader)
            End Using
            sections = xmlparts.SelectNodes(String.Format("//PART[contains(translate(PARTNAME, 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'), '{1}')]", Chr(34), context.Request("s").ToLower))
            For Each n As XmlNode In sections
                If Not PartsFound.Contains(n.SelectSingleNode("PARTNAME").InnerText) Then
                    PartsFound.Add(n.SelectSingleNode("PARTNAME").InnerText)
                End If
            Next
            sections = xmlparts.SelectNodes(String.Format("//PART[contains(translate(PARTDES, 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'), '{1}')]", Chr(34), context.Request("s").ToLower))
            For Each n As XmlNode In sections
                If Not PartsFound.Contains(n.SelectSingleNode("PARTNAME").InnerText) Then
                    PartsFound.Add(n.SelectSingleNode("PARTNAME").InnerText)
                End If
            Next
            sections = xmlparts.SelectNodes(String.Format("//PART[contains(translate(PARTREMARK, 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'), '{1}')]", Chr(34), context.Request("s").ToLower))
            For Each n As XmlNode In sections
                If Not PartsFound.Contains(n.SelectSingleNode("PARTNAME").InnerText) Then
                    PartsFound.Add(n.SelectSingleNode("PARTNAME").InnerText)
                End If
            Next
            sections = xmlparts.SelectNodes(String.Format("//PART[contains(translate(BARCODE, 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'), '{1}')]", Chr(34), context.Request("s").ToLower))
            For Each n As XmlNode In sections
                If Not PartsFound.Contains(n.SelectSingleNode("PARTNAME").InnerText) Then
                    PartsFound.Add(n.SelectSingleNode("PARTNAME").InnerText)
                End If
            Next
            sections = xmlparts.SelectNodes(String.Format("//PART/SPECS/SPEC[contains(translate(@VALUE, 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'), '{1}')]", Chr(34), context.Request("s").ToLower))
            For Each n As XmlNode In sections
                If Not PartsFound.Contains(n.SelectSingleNode("PARTNAME").InnerText) Then
                    PartsFound.Add(n.SelectSingleNode("PARTNAME").InnerText)
                End If
            Next
            
            For Each k As String In PartsFound
                sections = doc.SelectNodes(String.Format("//page[translate(@part, 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz') = {0}{1}{0}]", Chr(34), k.ToLower))
                For Each n As XmlNode In sections
                    If Not Found.Contains(n.Attributes("id").Value) Then
                        Found.Add(n.Attributes("id").Value)
                    End If
                Next
            Next
                                                
            Dim objX As New XmlTextWriter(context.Response.OutputStream, Nothing)
            With .Response
                .Clear()
                .ContentType = "text/xml"
                .ContentEncoding = Encoding.UTF8
                   
                With objX
                    .WriteStartDocument()
                    .WriteStartElement("results")
                    
                    For Each n As XmlNode In doc.SelectNodes(String.Format("//page"))
                        If Found.Contains(n.Attributes("id").Value) Then
                        
                            Dim id As String = n.Attributes("id").Value
                            Dim title As String = n.Attributes("title").Value
                            Dim description As String = n.Attributes("description").Value
                            
                            .WriteStartElement("result")
                            .WriteAttributeString("pagetitle", title)
                            .WriteAttributeString("loc", id)
                            .WriteAttributeString("description", description)
                            .WriteEndElement()
                        End If
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
    
    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class