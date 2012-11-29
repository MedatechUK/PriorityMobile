Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data.OracleClient

Public Enum eProviderType
    MSSQL = 1
    ORACLE = 2
End Enum

Public MustInherit Class myProvider
    Private _Provider As eProviderType = eProviderType.MSSQL
    Public Property Provider() As eProviderType
        Get
            Return _Provider
        End Get
        Set(ByVal value As eProviderType)
            _Provider = value
        End Set
    End Property
End Class

Public Class GenericConnection
    Inherits myProvider
    Implements IDisposable

#Region "Private Variables"

    Private _DataSource As Object
    Private disposedValue As Boolean = False        ' To detect redundant calls
    Private _FullConstr As String

#End Region

#Region "Initialisation and Finalisation"

    Public Sub New(ByVal Provider As eProviderType)
        Me.Provider = Provider
        Select Case Me.Provider
            Case eProviderType.MSSQL
                _DataSource = New System.Data.SqlClient.SqlConnection                
            Case eProviderType.ORACLE
                _DataSource = New System.Data.OracleClient.OracleConnection
        End Select
    End Sub

    Public Sub New(ByVal Provider As eProviderType, ByVal DBInstance As String, ByVal Username As String, ByVal Password As String, Optional ByVal InitialCatalog As String = "master")
        Me.Provider = Provider
        Select Case Me.Provider
            Case eProviderType.MSSQL
                _DataSource = New System.Data.SqlClient.SqlConnection
                _FullConstr = NewConnectionString( _
                        Provider, _
                        DBInstance, _
                        Username, _
                        Password, _
                        InitialCatalog _
                    )
                _DataSource.ConnectionString = _FullConstr
            Case eProviderType.ORACLE
                _DataSource = New System.Data.OracleClient.OracleConnection
                _FullConstr = NewConnectionString( _
                        Provider, _
                        DBInstance, _
                        Username, _
                        Password, _
                        InitialCatalog _
                    )
                _DataSource.ConnectionString = _FullConstr
        End Select
    End Sub

    ' IDisposable
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                Select Case Me.Provider
                    Case eProviderType.MSSQL
                        Dim myDatasource As System.Data.SqlClient.SqlConnection = _DataSource
                        myDatasource.Dispose()
                    Case eProviderType.ORACLE
                        Dim myDataSource As System.Data.OracleClient.OracleConnection = _DataSource
                        myDataSource.Dispose()
                End Select
                _DataSource = Nothing
            End If

            ' TODO: free your own state (unmanaged objects).
            ' TODO: set large fields to null.
        End If
        Me.disposedValue = True
    End Sub

    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub

#End Region

#Region "Public Properties"

    Public Property ConnectionString() As String
        Get
            Select Case Me.Provider
                Case eProviderType.MSSQL
                    Dim myDatasource As System.Data.SqlClient.SqlConnection = _DataSource
                    Return _FullConstr
                Case eProviderType.ORACLE
                    Dim myDataSource As System.Data.OracleClient.OracleConnection = _DataSource
                    Return _FullConstr
                Case Else
                    Return Nothing
            End Select
        End Get
        Set(ByVal value As String)
            _FullConstr = value
            Select Case Me.Provider
                Case eProviderType.MSSQL
                    Dim myDatasource As System.Data.SqlClient.SqlConnection = _DataSource
                    myDatasource.ConnectionString = value
                Case eProviderType.ORACLE
                    Dim myDataSource As System.Data.OracleClient.OracleConnection = _DataSource
                    myDataSource.ConnectionString = value
            End Select
        End Set
    End Property

    Public ReadOnly Property Connection() As Object
        Get
            Select Case Me.Provider
                Case eProviderType.MSSQL
                    Dim myDatasource As System.Data.SqlClient.SqlConnection = _DataSource
                    Return myDatasource
                Case eProviderType.ORACLE
                    Dim myDataSource As System.Data.OracleClient.OracleConnection = _DataSource
                    Return myDataSource
                Case Else
                    Return Nothing
            End Select
        End Get
    End Property

    Public ReadOnly Property State() As System.Data.ConnectionState
        Get
            Select Case Me.Provider
                Case eProviderType.MSSQL
                    Dim myDatasource As System.Data.SqlClient.SqlConnection = _DataSource
                    Return myDatasource.State
                Case eProviderType.ORACLE
                    Dim myDataSource As System.Data.OracleClient.OracleConnection = _DataSource
                    Return myDataSource.State
                Case Else
                    Return Nothing
            End Select
        End Get
    End Property

    Public ReadOnly Property StarredPassword() As String
        Get
            Dim dict As New Dictionary(Of String, String)
            Dim bStr As New System.Text.StringBuilder
            For Each pair As String In _FullConstr.Split(";")
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

#End Region

#Region "public Methods"

    Public Sub Open()
        Select Case Me.Provider
            Case eProviderType.MSSQL
                Dim myDatasource As System.Data.SqlClient.SqlConnection = _DataSource
                myDatasource.Open()
            Case eProviderType.ORACLE
                Dim myDataSource As System.Data.OracleClient.OracleConnection = _DataSource
                myDataSource.Open()
        End Select
    End Sub

    Public Sub Close()
        Select Case Me.Provider
            Case eProviderType.MSSQL
                Dim myDatasource As System.Data.SqlClient.SqlConnection = _DataSource
                myDatasource.Close()
            Case eProviderType.ORACLE
                Dim myDataSource As System.Data.OracleClient.OracleConnection = _DataSource
                myDataSource.Close()
        End Select
    End Sub

    Public Function CreateCommand() As GenericCommand
        Select Case Me.Provider
            Case eProviderType.MSSQL
                Dim myDatasource As System.Data.SqlClient.SqlConnection = _DataSource
                Return New GenericCommand(Me)
            Case eProviderType.ORACLE
                Dim myDataSource As System.Data.OracleClient.OracleConnection = _DataSource
                Return New GenericCommand(Me)
            Case Else
                Return Nothing
        End Select
    End Function

#End Region

#Region "Private Methods"

    Private Function NewConnectionString(ByVal Provider As eProviderType, ByVal DBInstance As String, ByVal Username As String, ByVal Password As String, Optional ByVal InitialCatalog As String = "master") As String
        Select Case Provider
            Case eProviderType.MSSQL
                Return String.Format("Data Source={0};Initial Catalog={1};User ID={2};Password={3}", _
                    DBInstance, _
                    InitialCatalog, _
                    Username, _
                    Password _
            )
            Case eProviderType.ORACLE
                Return String.Format("Data Source={0};User Id={2};Password={3}", _
                    DBInstance, _
                    InitialCatalog, _
                    Username, _
                    Password _
            )
            Case Else
                Return Nothing
        End Select
    End Function

#End Region

End Class

Public Class GenericCommand
    Inherits myProvider
    Implements IDisposable

#Region "Private Variables"

    Private _Command As Object
    Private disposedValue As Boolean = False        ' To detect redundant calls

#End Region

#Region "Initialisation and finalisation"

    Public Sub New(ByVal gConnection As GenericConnection)
        Me.Provider = gConnection.Provider
        Select Case Me.Provider
            Case eProviderType.MSSQL
                _Command = New System.Data.SqlClient.SqlCommand()
                Dim thisCommand As System.Data.SqlClient.SqlCommand = _Command
                thisCommand.Connection = gConnection.Connection
            Case eProviderType.ORACLE
                _Command = New System.Data.OracleClient.OracleCommand()
                Dim thisCommand As System.Data.OracleClient.OracleCommand = _Command
                thisCommand.Connection = gConnection.Connection
        End Select
    End Sub

    Public Sub New(ByVal CmdText As String, ByVal gConnection As GenericConnection)
        Me.Provider = gConnection.Provider
        Select Case Me.Provider
            Case eProviderType.MSSQL
                _Command = New System.Data.SqlClient.SqlCommand(CmdText, gConnection.Connection)
            Case eProviderType.ORACLE
                _Command = New System.Data.OracleClient.OracleCommand(CmdText, gConnection.Connection)
        End Select
    End Sub

    ' IDisposable
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                Select Case Me.Provider
                    Case eProviderType.MSSQL
                        Dim thisCommand As System.Data.SqlClient.SqlCommand = _Command
                        thisCommand.Dispose()
                    Case eProviderType.ORACLE
                        Dim thisCommand As System.Data.OracleClient.OracleCommand = _Command
                        thisCommand.Dispose()
                End Select
                _Command = Nothing
            End If

            ' TODO: free your own state (unmanaged objects).
            ' TODO: set large fields to null.
        End If
        Me.disposedValue = True
    End Sub

    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub

#End Region

#Region "Public Properties"

    Public Property CommandText() As String
        Get
            Select Case Me.Provider
                Case eProviderType.MSSQL
                    Dim myCommand As System.Data.SqlClient.SqlCommand = _Command
                    Return myCommand.CommandText
                Case eProviderType.ORACLE
                    Dim myCommand As System.Data.OracleClient.OracleCommand = _Command
                    Return myCommand.CommandText
                Case Else
                    Return Nothing
            End Select
        End Get
        Set(ByVal value As String)
            Select Case Me.Provider
                Case eProviderType.MSSQL
                    Dim myCommand As System.Data.SqlClient.SqlCommand = _Command
                    myCommand.CommandText = value
                Case eProviderType.ORACLE
                    Dim myCommand As System.Data.OracleClient.OracleCommand = _Command
                    myCommand.CommandText = value
            End Select
        End Set
    End Property

#End Region

#Region "Public Methods"

    Public Function ExecuteScalar() As Object
        Select Case Me.Provider
            Case eProviderType.MSSQL
                Dim thisCommand As System.Data.SqlClient.SqlCommand = _Command
                Return thisCommand.ExecuteScalar
            Case eProviderType.ORACLE
                Dim thisCommand As System.Data.OracleClient.OracleCommand = _Command
                Return thisCommand.ExecuteScalar
            Case Else
                Return Nothing
        End Select
    End Function

    Public Function ExecuteNonQuery() As Integer
        Select Case Me.Provider
            Case eProviderType.MSSQL
                Dim thisCommand As System.Data.SqlClient.SqlCommand = _Command
                Return thisCommand.ExecuteNonQuery()
            Case eProviderType.ORACLE
                Dim thisCommand As System.Data.OracleClient.OracleCommand = _Command
                Return thisCommand.ExecuteNonQuery()
            Case Else
                Return Nothing
        End Select
    End Function

    Public Function ExecuteReader() As GenericDataReader
        Select Case Me.Provider
            Case eProviderType.MSSQL
                Dim thisCommand As System.Data.SqlClient.SqlCommand = _Command
                Return New GenericDataReader(Me.Provider, thisCommand.ExecuteReader())
            Case eProviderType.ORACLE
                Dim thisCommand As System.Data.OracleClient.OracleCommand = _Command
                Return New GenericDataReader(Me.Provider, thisCommand.ExecuteReader())
            Case Else
                Return Nothing
        End Select
    End Function

#End Region

End Class

Public Class GenericDataReader
    Inherits myProvider
    Implements IDisposable

#Region "Private Variables"

    Private _reader As Object
    Private disposedValue As Boolean = False        ' To detect redundant calls

#End Region

#Region "Initialisation and Finalisation"

    Public Sub New(ByVal Provider As eProviderType, ByVal Reader As Object)
        _reader = Reader
        Me.Provider = Provider
        Select Case Me.Provider
            Case eProviderType.MSSQL
                Dim _reader As System.Data.SqlClient.SqlDataReader = Reader
            Case eProviderType.ORACLE
                Dim _reader As System.Data.OracleClient.OracleDataReader = Reader
        End Select
    End Sub

    ' IDisposable
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                Me.Provider = Provider
                Select Case Me.Provider
                    Case eProviderType.MSSQL
                        Dim thisReader As System.Data.SqlClient.SqlDataReader = _reader
                        thisReader.Close()
                    Case eProviderType.ORACLE
                        Dim thisreader As System.Data.OracleClient.OracleDataReader = _reader
                        thisreader.Close()
                End Select
                _reader = Nothing
            End If

            ' TODO: free your own state (unmanaged objects).
            ' TODO: set large fields to null.
        End If
        Me.disposedValue = True
    End Sub

    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub

#End Region

#Region "Public Properties"

    Public ReadOnly Property GetValue(ByVal i As Integer) As Object
        Get
            Select Case Me.Provider
                Case eProviderType.MSSQL
                    Dim thisReader As System.Data.SqlClient.SqlDataReader = _reader
                    Return thisReader.GetValue(i)
                Case eProviderType.ORACLE
                    Dim thisReader As System.Data.OracleClient.OracleDataReader = _reader
                    Return thisReader.GetValue(i)
                Case Else
                    Return Nothing
            End Select
        End Get
    End Property

    Public ReadOnly Property GetValues(ByVal Values() As Object) As Integer
        Get
            Select Case Me.Provider
                Case eProviderType.MSSQL
                    Dim thisReader As System.Data.SqlClient.SqlDataReader = _reader
                    Return thisReader.GetValues(Values)
                Case eProviderType.ORACLE
                    Dim thisReader As System.Data.OracleClient.OracleDataReader = _reader
                    Return thisReader.GetValues(Values)
                Case Else
                    Return Nothing
            End Select
        End Get
    End Property

    Default Public ReadOnly Property Item(ByVal i As Integer) As Object
        Get
            Select Case Me.Provider
                Case eProviderType.MSSQL
                    Dim thisReader As System.Data.SqlClient.SqlDataReader = _reader
                    Return thisReader.Item(i)
                Case eProviderType.ORACLE
                    Dim thisReader As System.Data.OracleClient.OracleDataReader = _reader
                    Return thisReader.Item(i)
                Case Else
                    Return Nothing
            End Select
        End Get
    End Property

    Public ReadOnly Property IsDBNull(ByVal i As Integer) As Boolean
        Get
            Select Case Me.Provider
                Case eProviderType.MSSQL
                    Dim thisReader As System.Data.SqlClient.SqlDataReader = _reader
                    Return thisReader.IsDBNull(i)
                Case eProviderType.ORACLE
                    Dim thisReader As System.Data.OracleClient.OracleDataReader = _reader
                    Return thisReader.IsDBNull(i)
                Case Else
                    Return Nothing
            End Select
        End Get
    End Property

    Public ReadOnly Property HasRows() As Boolean
        Get
            Select Case Me.Provider
                Case eProviderType.MSSQL
                    Dim thisReader As System.Data.SqlClient.SqlDataReader = _reader
                    Return thisReader.HasRows
                Case eProviderType.ORACLE
                    Dim thisReader As System.Data.OracleClient.OracleDataReader = _reader
                    Return thisReader.HasRows
                Case Else
                    Return Nothing
            End Select
        End Get
    End Property

    Public ReadOnly Property IsClosed() As Boolean
        Get
            Select Case Me.Provider
                Case eProviderType.MSSQL
                    Dim thisReader As System.Data.SqlClient.SqlDataReader = _reader
                    Return thisReader.IsClosed
                Case eProviderType.ORACLE
                    Dim thisReader As System.Data.OracleClient.OracleDataReader = _reader
                    Return thisReader.IsClosed
                Case Else
                    Return Nothing
            End Select
        End Get
    End Property

    Public ReadOnly Property FieldCount() As Integer
        Get
            Select Case Me.Provider
                Case eProviderType.MSSQL
                    Dim thisReader As System.Data.SqlClient.SqlDataReader = _reader
                    Return thisReader.FieldCount
                Case eProviderType.ORACLE
                    Dim thisReader As System.Data.OracleClient.OracleDataReader = _reader
                    Return thisReader.FieldCount
                Case Else
                    Return Nothing
            End Select
        End Get
    End Property

    Public ReadOnly Property RecordsAffected() As Integer
        Get
            Select Case Me.Provider
                Case eProviderType.MSSQL
                    Dim thisReader As System.Data.SqlClient.SqlDataReader = _reader
                    Return thisReader.RecordsAffected
                Case eProviderType.ORACLE
                    Dim thisReader As System.Data.OracleClient.OracleDataReader = _reader
                    Return thisReader.RecordsAffected
                Case Else
                    Return Nothing
            End Select
        End Get
    End Property

#End Region

#Region "Public Methods"

    Public Function GetSchemaTable() As DataTable
        Select Case Me.Provider
            Case eProviderType.MSSQL
                Dim thisReader As System.Data.SqlClient.SqlDataReader = _reader
                Return thisReader.GetSchemaTable
            Case eProviderType.ORACLE
                Dim thisReader As System.Data.OracleClient.OracleDataReader = _reader
                Return thisReader.GetSchemaTable
            Case Else
                Return Nothing
        End Select
    End Function

    Public Function Read() As Boolean
        Select Case Me.Provider
            Case eProviderType.MSSQL
                Dim thisReader As System.Data.SqlClient.SqlDataReader = _reader
                Return thisReader.Read
            Case eProviderType.ORACLE
                Dim thisReader As System.Data.OracleClient.OracleDataReader = _reader
                Return thisReader.Read
            Case Else
                Return Nothing
        End Select
    End Function

    Public Function NextResult() As Boolean
        Select Case Me.Provider
            Case eProviderType.MSSQL
                Dim thisReader As System.Data.SqlClient.SqlDataReader = _reader
                Return thisReader.NextResult
            Case eProviderType.ORACLE
                Dim thisReader As System.Data.OracleClient.OracleDataReader = _reader
                Return thisReader.NextResult
            Case Else
                Return Nothing
        End Select
    End Function

    Public Sub Close()
        Select Case Me.Provider
            Case eProviderType.MSSQL
                Dim thisReader As System.Data.SqlClient.SqlDataReader = _reader
                thisReader.Close()
            Case eProviderType.ORACLE
                Dim thisReader As System.Data.OracleClient.OracleDataReader = _reader
                thisReader.Close()
        End Select
    End Sub

#End Region

End Class
