Module func

    Public Function DateToMin() As Integer
        Return DateDiff(DateInterval.Minute, #1/1/1988#, Now)
    End Function

    Public Function DateToMin(ByVal Thisdate As DateTime) As Integer
        Return DateDiff(DateInterval.Minute, #1/1/1988#, Thisdate)
    End Function

    Public Function DateFromInt(ByVal IntDate As Integer) As Date
        Return DateAdd(DateInterval.Minute, IntDate, New Date(1988, 1, 1))
    End Function

    Public Function FullPath()        
        Dim fp As String = System.Reflection.Assembly.GetExecutingAssembly().Location
        While Not fp.EndsWith("\")
            fp = fp.Remove(fp.Length - 1, 1)
        End While
        Return fp
    End Function

End Module
