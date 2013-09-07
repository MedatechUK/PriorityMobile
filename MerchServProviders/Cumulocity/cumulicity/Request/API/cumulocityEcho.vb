Public Class cumulocityEcho
    Inherits cumulocityRequest

    Sub New(ByRef Tennant As cumulocityCredentials, ByVal Message As String)
        MyBase.New(Tennant, "test")
        Parameters.Add("echo", Message)

    End Sub

    Public Overrides Function Result(ByRef excep As Exception) As CumulocityResponse

        excep = Nothing
        Dim ex As New Exception
        Dim rd As System.IO.StreamReader = Response(ex)
        If IsNothing(ex) Then
            Try
                Return New responseEcho(rd.ReadToEnd)

            Catch parseException As Exception
                excep = parseException
                Return Nothing
            End Try
        Else
            excep = ex
            Return Nothing
        End If

    End Function

End Class
