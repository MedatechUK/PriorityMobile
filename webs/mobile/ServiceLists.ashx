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
                        "	[LIST] as {0}@LIST{0}, " & _
                        "    [DEFAULTVALUE] as {0}@DEFAULT{0}, " & _
                        "      (" & _
                        "      sELECT " & _
                        "      [VALUE] AS {0}@VALUE{0}, " & _
                        "      [DESCRIPTION] AS {0}@DESCRIPTION{0} " & _
                        "	  FROM [dbo].[v_SvcCallLists] " & _
                        "	  where lists.[LIST] = [LIST] " & _
                        "     ORDER BY [DESCRIPTION] " & _
                        "	  for XML PATH('LISTITEM'),type " & _
                        "  )" & _
                        "FROM [dbo].[v_SvcCallLists] AS lists " & _
                        "group by [LIST], [DEFAULTVALUE] " & _
                        "for XML PATH('LIST'), ROOT('LOOKUP') " _
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