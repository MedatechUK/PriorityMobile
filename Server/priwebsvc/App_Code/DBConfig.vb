Imports Microsoft.VisualBasic

Public Class DBConfig

    Public Function ConnectionString(ByVal Path As String, ByVal Connection As String) As String

        Dim rootWebConfig As System.Configuration.Configuration
        rootWebConfig = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration(Path)
        Dim connString As New System.Configuration.ConnectionStringSettings
        If (0 < rootWebConfig.ConnectionStrings.ConnectionStrings.Count) Then
            connString = rootWebConfig.ConnectionStrings.ConnectionStrings(Connection)
            If (Nothing = connString.ConnectionString) Then
                Return Nothing
            End If
        Else
            Return Nothing
        End If
        Return connString.ConnectionString

    End Function

End Class