<%@ WebHandler Language="VB" Class="ProvXML" %>

Imports System
Imports System.Web
Imports System.Xml
Imports System.Data.SqlClient
Imports System.Collections.Generic
Imports System.IO

Public Class ProvXML : Implements IHttpHandler
#Region "Database connections"
    
    Public ReadOnly Property ConnectionString(Optional ByVal UseEnvironment As String = Nothing) As String
        Get
            Return System.Configuration.ConfigurationManager.AppSettings.Get("DSN")
        End Get
    End Property
    
    Public ReadOnly Property Environment() As String
        Get
            Return System.Configuration.ConfigurationManager.AppSettings.Get("Environment")
        End Get
    End Property
    
#End Region
    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        context.Response.ContentType = "text/xml"
        context.Response.ContentEncoding = Encoding.UTF8
        'first up we need an xml writer to write the xml. No need to re-invent the wheel as Si has written one already (and very good it is too!)
        Dim objX As New XmlTextWriter(context.Response.OutputStream, Nothing)
        With objX
        
            Try
            
                Dim sqlInput As String = context.Server.MapPath(".") & "\prov.sql"
                If Not File.Exists(sqlInput) Then Throw New Exception("SQL file does not exist.")
                                           
                Dim xmlData As New XmlDocument
                '.WriteStartDocument()
                Using Connection As New SqlConnection(ConnectionString)
                    Connection.Open()
                    Dim ret As SqlCommand = Connection.CreateCommand
                    ret.CommandText = String.Format("use {0}; ", Environment)
                    Using sr As New StreamReader(sqlInput)
                        ret.CommandText += sr.ReadToEnd
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
                '.WriteEndElement()
                '.WriteEndDocument()
                .Flush()
                .Close()
                context.Response.End()
            End Try
        End With
    End Sub
 
    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class