Module ConnStr

    Private _Connection As GenericConnection
    Public Property Connection() As GenericConnection
        Get
            Return _Connection
        End Get
        Set(ByVal value As GenericConnection)
            _Connection = value
        End Set
    End Property

    'Public ReadOnly Property StarredPassword(ByVal ConnectionString As String) As String
    '    Get
    '        Dim dict As New Dictionary(Of String, String)
    '        Dim bStr As New System.Text.StringBuilder
    '        For Each pair As String In ConnectionString.Split(";")
    '            If pair.Length > 0 Then dict.Add(pair.Split("=")(0), pair.Split("=")(1))
    '        Next
    '        For Each key As String In dict.Keys
    '            If String.Compare(key, "password", True) = 0 Then
    '                bStr.AppendFormat("{0}=******;", key)
    '            ElseIf String.Compare(key, "pwd", True) = 0 Then
    '                bStr.AppendFormat("{0}=******;", key)
    '            Else
    '                bStr.AppendFormat("{0}={1};", key, dict(key))
    '            End If
    '        Next
    '        Return bStr.ToString
    '    End Get
    'End Property

    'Public Function NewConnection(ByVal Provider As eProviderType, ByVal DBInstance As String, ByVal Username As String, ByVal Password As String, Optional ByVal InitialCatalog As String = "master") As GenericConnection
    '    Dim cn As New GenericConnection(Provider)
    '    cn.ConnectionString = NewConnectionString( _
    '                    Provider, _
    '                    DBInstance, _
    '                    Username, _
    '                    Password, _
    '                    InitialCatalog _
    '                )
    '    Return cn
    'End Function

    'Public Function NewConnectionString(ByVal Provider As eProviderType, ByVal DBInstance As String, ByVal Username As String, ByVal Password As String, Optional ByVal InitialCatalog As String = "master") As String
    '    Select Case Provider
    '        Case eProviderType.MSSQL
    '            Return String.Format("Data Source={0};Initial Catalog={1};User ID={2};Password={3}", _
    '                DBInstance, _
    '                InitialCatalog, _
    '                Username, _
    '                Password _
    '        )
    '        Case eProviderType.ORACLE
    '            Return String.Format("Data Source={0};User Id={2};Password={3}", _
    '                DBInstance, _
    '                InitialCatalog, _
    '                Username, _
    '                Password _
    '        )
    '        Case Else
    '            Return Nothing
    '    End Select
    'End Function

End Module
