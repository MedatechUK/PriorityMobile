Public Class q
    Inherits cfOnBoardData.PDAData

    Public Sub New(Optional ByRef App As cfOnBoardData.BaseForm = Nothing)

        CallerApp = App

        With Me
            .Name = "q"
            .ConQuery = ""
            .Column(0) = "filename"
        End With

    End Sub

    Public Overrides Sub ConFail(ByRef Cancel As Boolean)

    End Sub

    Public Overrides Function ConWebService(ByRef data As Object) As Boolean

    End Function

    Public Overrides Sub LoadData(ByVal Ordinal As Integer)

    End Sub

    Public Overrides Sub SyncNewData()

    End Sub

    Public Overrides Function Validate() As Boolean

    End Function

End Class
