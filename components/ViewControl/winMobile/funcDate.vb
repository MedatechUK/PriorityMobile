Public Class funcDate
    Public Shared Function DateFromInt(ByVal IntDate As Integer) As Date
        Return DateAdd(DateInterval.Minute, IntDate, New Date(1988, 1, 1))
    End Function

    Public Shared Function DateToMin()
        Return DateDiff(DateInterval.Minute, #1/1/1988#, Now)
    End Function

    Public Shared Function TimeToMin()
        Return DateDiff(DateInterval.Minute, New Date(Now.Year, Now.Month, Now.Day), Now)
    End Function
End Class
