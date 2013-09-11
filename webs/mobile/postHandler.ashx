<%@ WebHandler Language="VB" Class="posthandler" %>

Imports System
Imports System.Web
Imports system.xml
Imports System.IO
Imports priority
Imports System.Collections.Generic

Public Class posthandler : Implements IHttpHandler            
           
    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property
    
    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        
        Dim StatusCode As Integer = 200
        Dim StatusMessage As String = "Ok"
        
        With context
            Try
                Dim pdaload As New PDALoadings(context, "deliverydata")
                
            Catch ex As Exception
                StatusMessage = ex.Message
                StatusCode = 400
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
    
End Class