<%@ WebHandler Language="VB" Class="xmldata" %>

Imports System
Imports System.Web
Imports System.Xml
Imports System.Data.SqlClient
Imports System.Collections.generic

#Region "Class for Join Dictioanry"
Public Class cJoin
#Region "Initialisation"
    Public Sub New()
    End Sub
    Public Sub New(ByVal ColumnType As Type, ByVal OuterValue As String)
        _ColumnType = ColumnType
        _OuterValue = OuterValue
    End Sub
#End Region
#Region "Public properties"
    Private _ColumnType As Type = Nothing
    Public Property ColumnType() As Type
        Get
            Return _ColumnType
        End Get
        Set(ByVal value As Type)
            _ColumnType = value
        End Set
    End Property
    Private _OuterValue As String = Nothing
    Public Property OuterValue() As String
        Get
            Return _OuterValue
        End Get
        Set(ByVal value As String)
            _OuterValue = value
        End Set
    End Property
#End Region
End Class
#End Region

Public Class xmldata : Implements IHttpHandler
    
#Region "Private Variable declaration"
    private ob As String = Nothing
    private ob2 As String = Nothing
    Private join As New Dictionary(Of String, cJoin)
    private wh As New List(Of String)
    private wh2 As New List(Of String)
    private so As New List(Of String)
    private sod As New List(Of String)
    private so2 As New List(Of String)
    private sod2 As New List(Of String)
    private top As String = ""
    private top2 As String = ""
#End Region
    
#Region "Get URL parameters"        
    Private Sub GetParams(ByVal Context As System.Web.HttpContext)
        For Each p As String In Context.Request.QueryString.Keys()
            Select Case p.ToLower
                Case "ob" ' Specifies the UPPER level SQL view to display {1}
                    ob = Context.Request(p)
                Case "sort" ' Specifies the columns to sort ASC in the UPPER level view {0-8}
                    so.Add(Context.Request(p))
                Case "sortd" ' Specifies the columns to sort DESC in the UPPER level view {0-8}
                    sod.Add(Context.Request(p))
                Case "top" ' Limits the number of record returned by the UPPER level view (0-1)
                    top = " TOP " & Context.Request(p)
                Case "ob2" ' Specifies the LOWER level SQL view to display {0-1}
                    ob2 = Context.Request(p)
                Case "sort2" ' Specifies the columns to sort ASC in the LOWER level view {0-8}
                    so2.Add(Context.Request(p))
                Case "sortd2" ' Specifies the columns to sort DESC in the LOWER level view {0-8}
                    sod2.Add(Context.Request(p))
                Case "top2" ' Limits the number of record returned by the LOWER level view (0-1)
                    top2 = " TOP " & Context.Request(p)
                Case "join" ' defines the where clause for LOWER level view  {1-8, WHERE ob2 is specified ELSE 0}
                    join.Add(Context.Request(p), New cJoin)
                Case Else ' defines the where clause for the UPPER level {0-8}
                    wh.Add(" AND " & p & "=" & Context.Request(p) & "")
            End Select
        Next
    End Sub
#End Region
    
#Region "Database connections"
    
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
    
#Region "Impliments iHTTPHandler"
    
    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property
    
    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
                       
        GetParams(context)
        
        Dim objX As New XmlTextWriter(context.Response.OutputStream, Nothing)
        With context.Response
            .Clear()
            .ContentType = "text/xml"
            .ContentEncoding = Encoding.UTF8
        End With
        
        With objX
            Try
                If IsNothing(ob) Then Throw New Exception("Object not specified.")
                .WriteStartDocument()
                .WriteStartElement(ob)
                
                Using Connection As New SqlConnection(System.Configuration.ConfigurationManager.AppSettings.Get("DSN"))
                    Dim dataReader As SqlDataReader = myReader(Connection, ob, wh, so, sod, top)
                    
                    Do While dataReader.Read()
                        Try
                            .WriteStartElement("record")
                            
                            Dim joinVal As String = ""
                            Dim jointype As Type = Nothing
                            
                            For i As Integer = 0 To dataReader.FieldCount - 1
                                If Not IsDBNull(dataReader(i)) Then
                                    .WriteAttributeString(Replace(Replace(dataReader.GetName(i), "$", ""), " ", "_"), dataReader(i))
                                    If Not IsNothing(join) And Not IsNothing(ob2) Then
                                        If join.ContainsKey(dataReader.GetName(i)) Then
                                            With join(dataReader.GetName(i))
                                                .OuterValue = dataReader(i)
                                                .ColumnType = dataReader(i).GetType
                                            End With
                                        End If
                                    End If
                                End If
                            Next
                                                        
                            If Not IsNothing(ob2) And Not IsNothing(join) Then
                                Using Connection2 As New SqlConnection(System.Configuration.ConfigurationManager.AppSettings.Get("DSN"))

                                    Dim dataReader2 As SqlDataReader = myReader(Connection2, ob2, JoinWhere(), so2, sod2, top2)
                                    
                                    Do While dataReader2.Read()
                                        .WriteStartElement("subitem")
                                        Try
                                            For i As Integer = 0 To dataReader2.FieldCount - 1
                                                If Not IsDBNull(dataReader2(i)) Then
                                                    .WriteAttributeString(Replace(dataReader2.GetName(i), "$", ""), dataReader2(i))
                                                End If
                                            Next
                                        Catch EX As Exception
                                        Finally
                                            .WriteEndElement()
                                        End Try
                                    Loop
                                    
                                End Using
                            End If
                                                      
                        Catch EX As Exception
                        Finally
                            .WriteEndElement()
                        End Try
                    Loop
                End Using
            
            Catch ex As Exception
            Finally
                .WriteEndElement()
                .WriteEndDocument()
                .Flush()
                .Close()
                context.Response.End()
            End Try
        End With
        
    End Sub
 
#End Region
    
#Region "Private functions"
 
    Private Function myReader(ByVal CN As SqlConnection, _
        ByVal ob As String, _
        ByVal wh As List(Of String), _
        ByVal so As List(Of String), _
        ByVal sod As List(Of String), _
        ByVal top As String _
    ) As SqlDataReader
        
        Dim ret As SqlCommand
        
        CN.Open()
        ret = CN.CreateCommand()
        ret.CommandText = "use [tru];select" & top & " * from " & ob & " WHERE 0=0"
        For i As Integer = 0 To wh.Count - 1
            ret.CommandText += wh.Item(i)
        Next
        If so.Count > 0 Or sod.Count > 0 Then
            ret.CommandText += " ORDER BY "
        End If
        If so.Count > 0 Then
            For i As Integer = 0 To so.Count - 1
                ret.CommandText += so.Item(i) & " ASC"
                If i < so.Count - 1 Then
                    ret.CommandText += ", "
                End If
            Next
        End If
        If sod.Count > 0 Then
            If so.Count > 0 Then ret.CommandText += ", "
            For i As Integer = 0 To sod.Count - 1
                ret.CommandText += sod.Item(i) & " DESC"
                If i < sod.Count - 1 Then
                    ret.CommandText += ", "
                End If
            Next
        End If
        Return ret.ExecuteReader
        
    End Function
    
    Private Function JoinWhere() As List(Of String)
        Dim ret As New List(Of String)
        With ret
            .Clear()
            For Each k As String In join.Keys
                Select Case join(k).ColumnType.Name
                    Case "String"
                        .Add(" AND " & k & "='" & join(k).OuterValue & "'")
                    Case Else
                        .Add(" AND " & k & "=" & join(k).OuterValue)
                End Select
            Next
        End With
        Return ret
    End Function
    
#End Region
    
End Class