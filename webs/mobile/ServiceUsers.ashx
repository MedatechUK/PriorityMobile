<%@ WebHandler Language="VB" Class="xmldata" %>

Imports System
Imports System.Web
Imports System.Xml
Imports System.Data.SqlClient
Imports System.Collections.generic

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
        
        Dim test As New ConfigValidation
        Dim objX As New XmlTextWriter(context.Response.OutputStream, Nothing)
        With context.Response
            .Clear()
            .ContentType = "text/xml"
            .ContentEncoding = Encoding.UTF8
        End With
        
        With objX
            Try
                Dim xmlData As New XmlDocument
                '.WriteStartDocument()
                Using Connection As New SqlConnection(ConnectionString)
                    Connection.Open()
                    Dim ret As SqlCommand = Connection.CreateCommand
                    ret.CommandText = String.Format( _
                        "SELECT " & _
                        "	[CUSTNAME] as {0}@CUSTNAME{0}," & _
                        "	[CUSTDES] as {0}@CUSTDES{0}, (" & _
                        "			select [PHONE] as {0}@PHONE{0}," & _
                        "			[NAME] as {0}@NAME{0}," & _
                        "			[EMAIL] as {0}@EMAIL{0}" & _
                        "			FROM [dbo].[v_SvcCallCust]" & _
                        "			where cust.[CUSTNAME] = [CUSTNAME]" & _
                        "			for XML PATH('User'),type " & _
                        "			)" & _
                        "  FROM [dbo].[v_SvcCallCust] AS cust" & _
                        "  GROUP BY [CUSTNAME],[CUSTDES]" & _
                        "  for XML PATH('Customer'), ROOT('ServiceUsers')" _
                        , Chr(34) _
                    )
                    Dim reader As XmlReader = ret.ExecuteXmlReader
                    
                    xmlData.Load(reader)
                    xmlData.WriteTo(objX)
                    
                End Using
                                
            Catch ex As Exception
            Finally
                '.WriteEndElement()
                '.WriteEndDocument()
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