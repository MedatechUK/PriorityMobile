Imports Bind
Public Class sync1 : Inherits SyncBase
    Public Overrides ReadOnly Property SyncDes() As String
        Get
            Return "Sync Description"
        End Get
    End Property
    Public Overrides ReadOnly Property FormName() As String
        Get
            Return "Service"
        End Get
    End Property
    Public Overrides ReadOnly Property FormInherit() As Boolean
        Get
            Return True
        End Get
    End Property
    Public Overrides Sub sync()
        For Each o As oBind In Tables.Values
            With o
                .Sync()
                .Save()
            End With
        Next
    End Sub
End Class
