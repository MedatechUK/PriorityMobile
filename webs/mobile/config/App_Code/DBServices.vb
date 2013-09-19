Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Public Class DBServices
    Private ConnString As String

    Public Shared ReadOnly Property SQLConn() As SqlConnection
        Get
            Return New SqlConnection("Data Source=BALDRICK\PRI;Initial Catalog=demo;Persist Security Info=True;User ID=tabula;Password=Tabula!")
        End Get
    End Property
End Class
