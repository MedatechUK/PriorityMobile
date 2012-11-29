Imports System.Data.SqlClient

Module ConnStr

    Private _Connection As SqlConnection
    Public Property Connection() As SqlConnection
        Get
            Return _Connection
        End Get
        Set(ByVal value As SqlConnection)
            _Connection = value
        End Set
    End Property

    Public Function NewConnection(ByVal DBInstance As String, ByVal Username As String, ByVal Password As String, Optional ByVal InitialCatalog As String = "master") As SqlConnection
        Return New SqlConnection( _
            NewConnectionString( _
                DBInstance, _
                Username, _
                Password, _
                InitialCatalog _
            ) _
        )
    End Function

    Public Function NewConnectionString(ByVal DBInstance As String, ByVal Username As String, ByVal Password As String, Optional ByVal InitialCatalog As String = "master") As String
        Return String.Format("Data Source={0};Initial Catalog={1};User ID={2};Password={3}", _
                DBInstance, _
                InitialCatalog, _
                Username, _
                Password _
        )
    End Function

End Module
