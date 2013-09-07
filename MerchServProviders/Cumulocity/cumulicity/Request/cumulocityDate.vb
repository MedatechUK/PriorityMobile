Public Class cumulocityDate

    Public Shared Function PriorityToUTC(ByVal SqlDate As Integer) As String
        Return DateAdd(DateInterval.Second, SqlDate, New Date(1988, 1, 1)).ToUniversalTime.ToString
    End Function

    Public Shared Function UTCToPriority(ByVal UTCDate As String) As Integer
        Return DateDiff(DateInterval.Minute, New Date(1988, 1, 1), CDate(UTCDate))
    End Function

End Class
