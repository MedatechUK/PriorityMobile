<%@ WebHandler Language="VB" Class="xmldata" %>

Imports System
Imports System.Web
Imports System.Xml
Imports System.Data.SqlClient
Imports System.Collections.Generic
Imports System.IO

Public Class xmldata : Implements IHttpHandler
    
#Region "Database connections"
    
    Public ReadOnly Property ConnectionString() As String
        Get
            Try
                Return System.Configuration.ConfigurationManager.AppSettings("DSN")
            Catch
                Return Nothing
            End Try
        End Get
    End Property
    
    Public ReadOnly Property Environment() As String
        Get
            Return System.Configuration.ConfigurationManager.AppSettings.Get("Environment")
        End Get
    End Property
    
#End Region
    
#Region "Impliments iHTTPHandler"
    
    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property
    
    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest

        With context.Response
            .Clear()
            .ContentType = "text/xml"
            .ContentEncoding = Encoding.UTF8
        End With
        
        Dim objX As New XmlTextWriter(context.Response.OutputStream, Nothing)
        With objX        
            Try
                Dim test As New ConfigValidation                                                          
                Dim xmlData As New XmlDocument
                
                '.WriteStartDocument()
                Using Connection As New SqlConnection(ConnectionString)
                    Connection.Open()
                    Dim ret As SqlCommand = Connection.CreateCommand
                    Using sr As New StreamReader(context.Server.MapPath("\") & "WEBPARTS.sql")
                        ret.CommandText = String.Format("use [{1}]{0}{2}", vbCrLf, Environment, sr.ReadToEnd)
                    End Using
                    Dim reader As XmlReader = ret.ExecuteXmlReader
                    xmlData.Load(reader)
                    xmlData.WriteTo(objX)
                End Using
                
            Catch ex As Exception
                .WriteStartDocument()
                .WriteStartElement("ERROR")
                .WriteStartAttribute("MESSAGE")
                .WriteValue(ex.Message)
                .WriteEndAttribute()
                .WriteEndElement()
                .WriteEndDocument()
            Finally
                .Flush()
                .Close()
                context.Response.End()
            End Try
        End With
        
    End Sub
 
#End Region
    
#Region "Private functions"
 
#End Region
    
End Class