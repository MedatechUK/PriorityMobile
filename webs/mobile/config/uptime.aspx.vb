
Partial Class config_uptime
    Inherits iSettings
    Private StartTime As DateTime = ApplicationSetting("start")

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim uptime As DateTime = DateAdd(DateInterval.Minute, DateDiff(DateInterval.Minute, CDate(StartTime), Now), New DateTime(0))
        Dim gt_hr As String = ""
        If DateDiff(DateInterval.Second, New DateTime(0), uptime) < 60 Then
            Me.txterror.Text = String.Format("+uptime: {1} has been running for {0}.", "< 1 minute", AppName)
        Else
            If DateDiff(DateInterval.Hour, New DateTime(0), uptime) > 0 Then gt_hr = " and "
            Me.txterror.Text = String.Format("{0}{1}{2}{3}{4}.", _
                UpTimePart(DatePart(DateInterval.Year, uptime) - 1, "Year", , ","), _
                UpTimePart(DatePart(DateInterval.Month, uptime) - 1, "Month", , ","), _
                UpTimePart(DatePart(DateInterval.Day, uptime) - 1, "Day", , ","), _
                UpTimePart(DatePart(DateInterval.Hour, uptime), "Hour", , ""), _
                UpTimePart(DatePart(DateInterval.Minute, uptime), "Minute", gt_hr, ""), _
                AppName _
            )
        End If
    End Sub

    Function UpTimePart(ByVal Value As Integer, ByVal Description As String, Optional ByVal Prefix As String = "", Optional ByVal Suffix As String = "") As String
        If Value > 0 Then
            Dim plural As String = ""
            Dim count As Integer = 0
            If Value > 1 Then
                plural = "s"
            End If
            Return String.Format("{3}{0} {1}{2}{4}", Value.ToString, Description, plural, Prefix, Suffix)
        Else
            Return ""
        End If
    End Function

End Class
