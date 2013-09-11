<%@ WebHandler Language="VB" Class="postxml" %>

Imports System
Imports System.Web
Imports System.Xml
Imports System.io
Imports Priority.Loading

Public Class postxml : Implements IHttpHandler
    
    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest

        Dim StatusCode As Integer = 200
        Dim StatusMessage As String = "Ok"
        Dim thisRequest As New XmlDocument
        Dim ws As New PriWebSVC.Service        
        Dim result As String = String.Empty
        
        With context
            
            Dim reader As New StreamReader(.Request.InputStream)
            Try
                With thisRequest
                    .LoadXml(reader.ReadToEnd)
                    Dim node As Xml.XmlNode = .SelectSingleNode("//signature/vectors")                    
                    result = ws.SaveSignature(Replace(Replace(node.InnerText(), ",", "\t"), ".", "\n"))
                        
                End With
                
            Catch ex As Exception
                StatusMessage = ex.Message
                StatusCode = 400
            Finally
                With reader
                    .Close()
                    .Dispose()
                End With
            End Try
            
            With .Response
                .Clear()
                .ContentType = "text/xml"
                .ContentEncoding = Encoding.UTF8
                Dim objX As New XmlTextWriter(context.Response.OutputStream, Nothing)
                With objX
                    .WriteStartDocument()
                    .WriteStartElement("response")
                    .WriteAttributeString("status", CStr(StatusCode))
                    .WriteAttributeString("message", StatusMessage)
                    .WriteAttributeString("result", result)
                    .WriteEndElement() 'End Settings 
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