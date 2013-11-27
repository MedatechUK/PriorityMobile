Module PriorityFunctions

    Public ReadOnly Property SQLFuncs() As List(Of PriSQLFunc) 'Dictionary(Of String, String)
        Get
            Static ret As New List(Of PriSQLFunc) 'Dictionary(Of String, String)
            If ret.Count = 0 Then
                With ret
                    .Add(New PriSQLFunc("SQL.DATE8", "dbo.DATETOMIN(getdate())"))
                    .Add(New PriSQLFunc("ITOA(%1)", "ltrim(rtrim(str(%1)))"))
                    .Add(New PriSQLFunc("(%1 = %2 ? %3 : %4)", "CASE %1 WHEN %2 THEN %3 ELSE %4 END"))
                    .Add(New PriSQLFunc("SQL.USER", "SELECT loginame from master..sysprocesses where spid = @@SPID"))
                    '.Add("SQL.DATE8", "dbo.DATETOMIN(GETDATE())")
                    ''.Add("SQL.DATE", "dbo.DATETOMIN(GETDATE())")
                    '.Add("ITOA(", "STR(")
                End With
            End If
            Return ret
        End Get
    End Property

End Module
