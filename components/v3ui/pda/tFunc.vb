Imports System.Data.SqlServerCe

Public Class tFunc

    Public Sub New(ByVal Path As String)
        Con = New SqlCeConnection(String.Format("Data Source={0};", Path))
        cmd = Con.CreateCommand
        Con.Open()
    End Sub

    Public Overloads Sub Finalize()
        Con.Close()
    End Sub

    Private _con As New SqlCeConnection
    Public Property Con() As SqlCeConnection
        Get
            Return _con
        End Get
        Set(ByVal value As SqlCeConnection)
            _con = value
        End Set
    End Property

    Private _cmd As New SqlCeCommand
    Public Property cmd() As SqlCeCommand
        Get
            Return _cmd
        End Get
        Set(ByVal value As SqlCeCommand)
            _cmd = value
        End Set
    End Property

    Public Function TableExists(ByVal Table As String) As Boolean
        cmd.CommandText = String.Format( _
            "select count(*) from INFORMATION_SCHEMA.TABLES where TABLE_NAME = '{0}'", _
            Table _
        )
        Dim ret As Integer = cmd.ExecuteScalar
        Return ret > 0
    End Function

    Public Sub MakeTable(ByRef table As xmlTable)
        Dim sql As String = ""
        With table
            sql += "CREATE TABLE " & .Name & " ("
            For i As Integer = 1 To table.Columns.Count
                With .Columns(i)
                    sql += .Name
                    Select Case .VarType
                        Case xmlColumnType.tText
                            sql += " nvarchar(" & CStr(.Width) & ")"
                        Case xmlColumnType.tReal
                            sql += " numeric(2)"
                        Case xmlColumnType.tInt
                            sql += " integer"
                        Case xmlColumnType.tDate
                            sql += " datetime"
                    End Select
                End With
                If i < table.Columns.Count Then sql += ", "
            Next
            sql += ")"
        End With
        With cmd
            .CommandText = sql
            .ExecuteNonQuery()
        End With
    End Sub

    Public Function GetReader(ByVal sql As String) As SqlCeDataReader
        Try
            cmd.CommandText = sql
            Return cmd.ExecuteReader()
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

End Class
