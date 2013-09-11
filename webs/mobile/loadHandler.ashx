<%@ WebHandler Language="VB" Class="loadHandler" %>

Imports System
Imports System.Web
Imports System.Xml
Imports System.IO

Public Class loadHandler : Implements IHttpHandler
        
    Private thisRequest As New XmlDocument
    
    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        
        Dim savedir As String = context.Server.MapPath("\") & "queue\"
        Dim fn As String = System.Guid.NewGuid.ToString & ".xml"
        
        Dim StatusCode As Integer = 200
        Dim StatusMessage As String = "Ok"
                               
        With context
            
            Dim reader As New StreamReader(.Request.InputStream)
            Try
                With thisRequest
                    .LoadXml(reader.ReadToEnd)
                    .Save(savedir & fn)
                    
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