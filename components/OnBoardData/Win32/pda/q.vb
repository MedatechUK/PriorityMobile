Public Class q
    Inherits PDAOnBoardData.PDAData

    Public Sub New(Optional ByRef App As PDAOnBoardData.BaseForm = Nothing)

        _App = App

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
