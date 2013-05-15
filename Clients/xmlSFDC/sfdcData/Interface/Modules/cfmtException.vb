Public Class cfmtException
    Inherits Exception

    Public Sub New(ByVal Pattern As String, ByVal ParamArray Args() As String)
        MyBase.New(String.Format(Pattern, Args))
    End Sub

End Class
