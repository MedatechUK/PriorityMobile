<%@ WebHandler Language="VB" Class="GetHandler" %>

Imports System
Imports System.Web
Imports ntEvtlog
Imports ntEvtlog.evt
Imports System.Collections.Generic
Imports System.IO
Imports System.Xml

Public Class GetHandler : Implements IHttpHandler

    Private thisRequest As New XmlDocument
    Private StatusCode As Integer = 200
    Private StatusMessage As String = "Ok"
    
    Private Enum eRequestType
        Tabular = 1
        Scalar = 2
        NonQuery = 3
    End Enum
    
#Region "Private Properties"

    Private ReadOnly Property StarredPassword(ByVal ConnectionString As String) As String
        Get
            Dim dict As New Dictionary(Of String, String)
            Dim bStr As New System.Text.StringBuilder
            For Each pair As String In ConnectionString.Split(";")
                If pair.Length > 0 Then dict.Add(pair.Split("=")(0), pair.Split("=")(1))
            Next
            For Each key As String In dict.Keys
                If String.Compare(key, "password", True) = 0 Then
                    bStr.AppendFormat("{0}=******;", key)
                ElseIf String.Compare(key, "pwd", True) = 0 Then
                    bStr.AppendFormat("{0}=******;", key)
                Else
                    bStr.AppendFormat("{0}={1};", key, dict(key))
                End If
            Next
            Return bStr.ToString
        End Get
    End Property

    Private ReadOnly Property AppName() As String
        Get
            Return System.Configuration.ConfigurationManager.AppSettings.Get("appName")
        End Get
    End Property

    Private ReadOnly Property ConnectionString(Optional ByVal UseEnvironment As String = Nothing) As String
        Get
            Return System.Configuration.ConfigurationManager.AppSettings.Get("DSN")
        End Get
    End Property

    Private ReadOnly Property Provider() As Integer
        Get
            Return System.Configuration.ConfigurationManager.AppSettings.Get("PROVIDER")
        End Get
    End Property

    Private ReadOnly Property Environment() As String
        Get
            Return System.Configuration.ConfigurationManager.AppSettings.Get("Environment")
        End Get
    End Property

    Private ReadOnly Property LogVerbosity() As Integer
        Get
            Try
                Return CInt(System.Configuration.ConfigurationManager.AppSettings.Get("LogVerbosity"))
            Catch
                Return 99
            End Try
        End Get
    End Property

    Private ReadOnly Property RemoteIP() As String
        Get
            Return HttpContext.Current.Request.UserHostAddress.ToString
        End Get
    End Property

#End Region
    
    Private ReadOnly Property RequestSQL() As String
        Get
            Try
                Return thisRequest.SelectSingleNode("//GetRequest/SQL").InnerText
            Catch ex As Exception
                Return ""
            End Try
        End Get
    End Property
    
    Private ReadOnly Property RequestEnvironment() As String
        Get
            Try
                Return thisRequest.SelectSingleNode("//GetRequest/Environment").InnerText
            Catch ex As Exception
                Return ""
            End Try
        End Get
    End Property

    Private ReadOnly Property RequestType() As eRequestType
        Get
            Try
                Select Case thisRequest.SelectSingleNode("//GetRequest/RequestType").InnerText.ToLower
                    Case "tabular"
                        Return eRequestType.Tabular
                    Case "scalar"
                        Return eRequestType.Scalar
                    Case "nonquery"
                        Return eRequestType.NonQuery
                    Case Else
                        Throw New Exception("Unknown RequestType")
                End Select
            Catch ex As Exception
                Return eRequestType.Tabular
            End Try
        End Get
    End Property
    
    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest              
                               
        With context
            
            Using reader As New StreamReader(.Request.InputStream)
                Try
                    With thisRequest
                        .LoadXml(reader.ReadToEnd)
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
            End Using
            
            With .Response
                .Clear()
                .ContentType = "text/xml"
                .ContentEncoding = Encoding.UTF8
                Dim objX As New XmlTextWriter(context.Response.OutputStream, Nothing)
                With objX
                    .WriteStartDocument()
                    .WriteStartElement("response")
                    If StatusCode = 200 then
                        Dim ResponseException As New Exception
                        ResponseException = GetResponse(RequestEnvironment, RequestSQL, RequestType, objX)
                        If Not IsNothing(ResponseException) Then
                            StatusCode = 500
                            StatusMessage = ResponseException.Message
                        End If
                    end if
                    .WriteStartElement("status")
                    .WriteAttributeString("code", CStr(StatusCode))
                    .WriteAttributeString("message", StatusMessage)
                    .WriteEndElement() 'End status 
                    .WriteEndElement() 'End response 
                    .WriteEndDocument()
                    .Flush()
                    .Close()
                End With
                .End()
            End With
                
        End With
        
    End Sub
    
    Private Function GetResponse( _
        ByVal env As String, _
        ByVal sql As String, _
        ByVal RequestType As eRequestType, _
        ByRef response As XmlTextWriter _
    ) As Exception
            
        Dim responseException As New Exception        
        responseException = Nothing
        
        Dim command As GenericCommand = Nothing
        Dim ev As New ntEvtlog.evt(ntEvtlog.EvtLogMode.EventLog, LogVerbosity, AppName)
        
        Try

            Using Connection As New GenericConnection(Provider)
                Connection.ConnectionString = ConnectionString

                If env.Length = 0 Then env = Environment

                Connection.Open()
                command = Connection.CreateCommand()

                Select Case Provider
                    Case 1
                        command.CommandText = String.Format("use [{0}]; {1}", env, sql)
                    Case 2
                        command.CommandText = String.Format("{0}", sql.Replace("{0}", env))
                End Select
                
                With response
                    Select Case RequestType
                        Case eRequestType.Tabular
                            Dim dataReader As GenericDataReader = _
                                command.ExecuteReader()
                        
                            .WriteStartElement("columns")
                            For Each col As Data.datarow In dataReader.GetSchemaTable.Rows
                                .WriteStartElement("column")
                                .WriteAttributeString("name", col.Item("ColumnName"))
                                .WriteAttributeString("type", col.Item("DataType").tostring)                                
                                .WriteEndElement() 'End column 
                            Next
                            .WriteEndElement() 'End columns 
                                                
                            .WriteStartElement("rows")
                            If dataReader.HasRows Then
                                dataReader.Read()
                                Do
                                    .WriteStartElement("row")
                                    For i As Integer = 0 To dataReader.FieldCount - 1
                                        .WriteAttributeString("_" & i.ToString, dataReader.Item(i).ToString)
                                    Next
                                    .WriteEndElement() 'End row 
                                Loop While dataReader.Read()

                            End If
                            .WriteEndElement() 'End rows 
                            
                        Case eRequestType.Scalar                                                         
                                .WriteElementString("result",command.ExecuteScalar())                                
                                
                        Case eRequestType.NonQuery
                                Dim dataReader As GenericDataReader = _
                                command.ExecuteReader()
                    
                    End Select
                End With

                ev.Log( _
                    String.Format( _
                        "Ran SQL Statement for client [{4}].{0}" & _
                        "The sql query was [{2}].", _
                        vbCrLf, _
                        "", _
                        command.CommandText, _
                        ConnectionString, _
                        RemoteIP _
                    ), _
                    ntEvtlog.LogEntryType.SuccessAudit, _
                    ntEvtlog.EvtLogVerbosity.VeryVerbose _
                )
            End Using

        Catch ex As Exception

            If IsNothing(command) Then
                responseException = New Exception( _
                    String.Format( _
                        "An error occured while processing an SQL Statement for client [{3}].{0}" & _
                        "The connection string used was: [{2}].{0}" & _
                        "The error message was [{1}].", _
                        vbCrLf, _
                        ex.Message, _
                        StarredPassword(Me.ConnectionString), _
                        RemoteIP _
                    ) _
                )
                ev.Log( _
                    responseException.Message, _
                    ntEvtlog.LogEntryType.FailureAudit, _
                    ntEvtlog.EvtLogVerbosity.Normal _
                )
            Else
                responseException = New Exception( _
                    String.Format( _
                        "An error occured while processing an SQL Statement for client [{4}].{0}" & _
                        "The sql query was [{2}].{0}The connection string used was: [{3}].{0}" & _
                        "The error message was [{1}].", _
                        vbCrLf, _
                        ex.Message, _
                        command.CommandText, _
                        StarredPassword(Me.ConnectionString), _
                        RemoteIP _
                    ) _
                )
                ev.Log( _
                    responseException.Message, _
                    ntEvtlog.LogEntryType.FailureAudit, _
                    ntEvtlog.EvtLogVerbosity.Normal _
                )
            End If

        End Try
        
        Return responseException

    End Function
 
    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class