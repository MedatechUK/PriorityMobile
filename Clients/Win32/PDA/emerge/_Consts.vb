Imports System.Text.RegularExpressions

Module _Consts

    Public _Warnings(,) As String = Nothing
    Public _dispWarn As Boolean = False

    Public regex_WebService As Regex = New Regex("http://.*\..*\.asmx")
    Public regex_String As Regex = New Regex("^[a-zA-Z0-9]+$")
    Public regex_Time As Regex = New Regex("^[0-9][0-9]\:[0-9][0-9]$")

    Public Const num_Actions As Integer = 5
    Public Const txt_SetTimeEnRoute As String = "Set Time En-Route"
    Public Const txt_SetTimeOnSite As String = "Set Time On-Site"
    Public Const txt_SetTimeFinished As String = "Set Time Finished"
    Public Const txt_PostData As String = "Post Data"
    Public Const txt_IsReIssue As String = "Re-Issued"


    Public Enum o
        ServiceCall = 0
        Warehouse = 1
        Statuses = 2
        Details = 3
        Malfunction = 4
        Resolution = 5
        Survey = 6
        Repair = 7
        Time = 8
        Parts = 9
        Signature = 10
        Answers = 11
        Flags = 12
        Actions = 13
        Cancel = 14
        DayEnd = 15
    End Enum

    Public Function DatetoMin() As String
        Return CStr(DateDiff(DateInterval.Minute, CDate("01/01/1988"), Now()))
    End Function

End Module
