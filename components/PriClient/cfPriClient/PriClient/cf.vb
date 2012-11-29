'cf stubs

Namespace Priority

    Public Class LoadCallBack

        Sub New(Optional ByRef ev As ntEvtLog.evt = Nothing)

        End Sub

        Private _Result As String
        Public ReadOnly Property Result() As Boolean
            Get
                Return _Result = "Y"
            End Get
        End Property

        Private _Data As String = ""
        Public ReadOnly Property Data() As String
            Get
                Return _Data
            End Get
        End Property

        Private _Connection As SqlClient.SqlConnection = Nothing
        Public Property Connection() As SqlClient.SqlConnection
            Get
                Return _Connection
            End Get
            Set(ByVal value As SqlClient.SqlConnection)
                _Connection = value
            End Set
        End Property

        Private _Table As String = Nothing
        Public Property Table() As String
            Get
                Return _Table
            End Get
            Set(ByVal value As String)
                _Table = value
            End Set
        End Property

        Private ReadOnly Property OpenConnection() As SqlClient.SqlCommand
            Get
                Try
                    If IsNothing(Connection) Then Throw New Exception("The connection has not been initialised.")
                    If IsNothing(Table) Then Throw New Exception("No table has been specified.")
                    If Not (Connection.State = ConnectionState.Open) Then
                        Connection.Open()
                    End If
                    Return Connection.CreateCommand()
                Catch ex As Exception
                    Throw New Exception(String.Format("Unable to open connection [{0}]. The error was: {1}", Connection.ConnectionString, ex.Message))
                End Try
            End Get
        End Property

        Private ReadOnly Property TriggerName() As String
            Get
                Return String.Format("{0}_SFDCLdTrig", Table)
            End Get
        End Property

        Public ReadOnly Property IsBubbleTable() As Boolean
            Get
                Dim cmd As SqlClient.SqlCommand = OpenConnection
                cmd.CommandText = String.Format( _
                    "SELECT COUNT(*) FROM sys.all_columns where object_id = " & _
                    "(select object_id from sys.tables where name = '{0}') " & _
                    "and name = 'BUBBLEID'" _
                    , Table _
                )
                Try
                    Return cmd.ExecuteScalar > 0
                Catch ex As Exception
                    Throw New Exception(String.Format("Unable to open verify if the table [{0}] supports Bubble events. The error was: {1}", Table, ex.Message))
                End Try
            End Get
        End Property

        Public ReadOnly Property TriggerExists() As Boolean
            Get
                Dim cmd As SqlClient.SqlCommand = OpenConnection
                cmd.CommandText = String.Format("select count(*) from sys.triggers where name = '{0}'", TriggerName)
                Try
                    Return cmd.ExecuteScalar > 0
                Catch ex As Exception
                    Throw New Exception(String.Format("Unable to open verify trigger [{0}] exists. The error was: {1}", TriggerName, ex.Message))
                End Try
            End Get
        End Property

        Public Function MakeTrigger() As Boolean

        End Function

        Public Function RegisterBubble(ByVal BubbleID As String) As Boolean

        End Function

        Public Function UnRegisterBubble(ByVal BubbleID As String) As Boolean

        End Function

        Public Function GetBubbleStatus(ByVal BubbleID As String) As Boolean

        End Function

    End Class

End Namespace
