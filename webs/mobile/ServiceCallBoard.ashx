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
                        "		SELECT [CustomerNum] as {0}@CustomerNum{0} " & _
                        "             ,[CustomerName] as {0}@CustomerName{0} " & _
                        "             ,[CallNumber] AS {0}@CallNumber{0} " & _
                        "			  ,[Opened] as {0}@Opened{0} " & _
                        "			  ,[Subject] as {0}@Subject{0} " & _
                        "			  ,[CALLTYPE] as {0}@CALLTYPE{0} " & _
                        "			  ,[CALLTYPEDES] as {0}@CALLTYPEDES{0} " & _
                        "			  ,[SEV] as {0}@SEV{0} " & _
                        "			  ,[SEVDES] as {0}@SEVDES{0} " & _
                        "			  ,[USER] as {0}@USER{0} " & _
                        "			  ,[AssignedTo] as {0}@AssignedTo{0} " & _
                        "			  ,[MALF] as {0}@MALF{0} " & _
                        "			  ,[MALFDES] as {0}@MALFDES{0} " & _
                        "			  ,[PHONE] as {0}@PHONE{0} " & _
                        "			  ,[NAME] as {0}@NAME{0} " & _
                        "			  ,[CALLSTATUS] as {0}@CALLSTATUS{0} " & _
                        "			  ,[Status] as {0}@Status{0} " & _
                        "			   FROM [dbo].[v_SvcCall] order by CURDATE DESC" & _
                        "  for XML PATH('Call'), ROOT('ServiceCalls')" _
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