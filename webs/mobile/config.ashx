<%@ WebHandler Language="VB" Class="config" %>

Imports System
Imports System.Web
Imports System.xml

Public Class config : Implements IHttpHandler
    
    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        With context            
            With .Response
                .Clear()
                .ContentType = "text/xml"
                .ContentEncoding = Encoding.UTF8
                Dim objX As New XmlTextWriter(context.Response.OutputStream, Nothing)
                With objX
                    .WriteStartDocument()
                    .WriteStartElement("response")
                    .WriteAttributeString("MachineName", Environment.MachineName)
                    .WriteAttributeString("SoapPath", context.Server.MapPath("/"))                   
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