<%@ WebHandler Language="VB" Class="ProvXML" %>

Imports System
Imports System.Web
Imports System.Xml
Imports System.Data.SqlClient
Imports System.Collections.Generic
Imports System.IO

Public Class ProvXML : Implements IHttpHandler
#Region "Database connections"
    Public ReadOnly Property SQLFile() As String
        Get
            Return "prov.sql"
        End Get
    End Property
    Public ReadOnly Property ConnectionString(Optional ByVal UseEnvironment As String = Nothing) As String
        Get
            Try
                Dim rootWebConfig As System.Configuration.Configuration
                rootWebConfig = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("\")
                Dim connString As New System.Configuration.ConnectionStringSettings
                If (0 < rootWebConfig.ConnectionStrings.ConnectionStrings.Count) Then
                    If IsNothing(UseEnvironment) Then
                        connString = rootWebConfig.ConnectionStrings.ConnectionStrings(Environment)
                    Else
                        connString = rootWebConfig.ConnectionStrings.ConnectionStrings(UseEnvironment)
                    End If

                    If (Nothing = connString.ConnectionString) Then
                        Return Nothing
                    End If
                Else
                    Return Nothing
                End If
                Return connString.ConnectionString
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
    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        context.Response.ContentType = "text/xml"
        context.Response.ContentEncoding = Encoding.UTF8
        'first up we need an xml writer to write the xml. No need to re-invent the wheel as Si has written one already (and very good it is too!)
        Dim objX As New XmlTextWriter(context.Response.OutputStream, Nothing)
        With objX
        
            Try
            

                                           
                Dim xmlData As New XmlDocument
                '.WriteStartDocument()
                Using Connection As New SqlConnection(ConnectionString)
                    Connection.Open()
                    Dim sqlString As String
                    Dim ret As SqlCommand = Connection.CreateCommand
                    Using sr As New StreamReader(context.Server.MapPath("\") & SQLFile)
                        sqlString = sr.ReadToEnd
                    End Using
                                        
                    ' --- 20/04/2013 - si
                    ' --- Impliment SQL parameters from the request string.
                    
                    ' Check the .sql for mandatory fields
                    Dim Mandatory As Regex = New Regex( _
                        "declare.*@.*--.*mandatory", _
                        RegexOptions.IgnoreCase _
                    )
                    ' Iterate through the mandatory fields
                    For Each m As Match In Mandatory.Matches(sqlString)
                        Dim expectedVar As String = _
                            Trim(m.Value.Substring(m.Value.IndexOf("@") + 1).Split(" ")(0))
                        
                        ' If mandatory parameter is missing then throw an error
                        If IsNothing(HttpContext.Current.Request.QueryString(expectedVar)) Then
                            Throw New Exception( _
                                String.Format( _
                                    "The {0} parameter is mandatory.", _
                                    expectedVar _
                                ) _
                            )
                        ElseIf HttpContext.Current.Request.QueryString(expectedVar).Length = 0 Then
                            Throw New Exception( _
                                String.Format( _
                                    "The {0} parameter is mandatory.", _
                                    expectedVar _
                                ) _
                            )
                        End If
                    Next
                    
                    ' Iterate through the parameters in the request string
                    For Each k As String In HttpContext.Current.Request.QueryString.Keys
                        Dim declaration As Regex = New Regex( _
                            String.Format( _
                                "declare.*@{0}", _
                                k _
                            ), _
                            RegexOptions.IgnoreCase _
                        )
                        ' Check the .SQL for matching DECLARE statements
                        If declaration.IsMatch(sqlString) Then
                            
                            ' Get the parameter name from the DECLARE statement
                            Dim MatchVal As String = declaration.Match(sqlString).Value
                            Dim VarName As String = Trim( _
                                MatchVal.Substring( _
                                    MatchVal.IndexOf("@") _
                                ) _
                            )
                            
                            ' Find the SET statement
                            Dim SetStatement As Regex = New Regex( _
                                String.Format( _
                                    "set.*@{0}.*=.*'.*'", _
                                    k _
                                ), _
                                RegexOptions.IgnoreCase _
                            )
                            If SetStatement.Match(sqlString).Success Then
                                ' Update the SET statement for this parameter
                                sqlString = SetStatement.Replace( _
                                    sqlString, _
                                    String.Format( _
                                        "set {0} = '{1}'", _
                                        VarName, _
                                        HttpContext.Current.Request.QueryString(k) _
                                    ) _
                                )
                            Else
                                ' Set statement not found
                                Throw New Exception( _
                                    String.Format( _
                                        "SET statement for parameter {0} not found in SQL file (1).", _
                                        k, _
                                        SQLFile _
                                    ) _
                                )
                            End If
                        End If
                    Next
                    
                    ' Execute the reader
                    ret.CommandText = String.Format("use [{1}]{0}{2}", vbCrLf, Environment, sqlString)
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