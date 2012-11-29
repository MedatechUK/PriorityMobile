Public Class _DayEnd
    Inherits PDAOnBoardData.PDAData

    Dim ws As New priwebsvc.Service

    Public Sub New(Optional ByRef App As PDAOnBoardData.BaseForm = Nothing)

        _App = App

        With Me
            .Name = "DayEnd"
            .ConQuery = ""
        End With

    End Sub

    Public Overrides Sub ConFail(ByRef Cancel As Boolean)

    End Sub

    Public Overrides Function ConWebService(ByRef data As Object) As Boolean

    End Function

    Public Overrides Sub LoadData(ByVal Ordinal As Integer)
        Dim subName As String = New StackTrace().GetFrame(0).GetMethod.ToString()
        Try
            With p
                '.DebugFlag = True
                .Procedure = "ZPDA_LOAD_DAYEND"
                .Table = "ZPDA_DAYEND_LOAD"
                .RecordType1 = "USERNAME"
                .RecordType2 = "CURDATE,DAYEND"
                .RecordTypes = "TEXT,,TIME"
            End With

            ' Type 1 records
            Dim t1() As String = { _
                                My.Settings.Username _
                                }
            p.AddRecord(1) = t1

            Dim temp() As String = {DatetoMin(), _
                                    Right("00" & CStr(DatePart(DateInterval.Hour, Now())), 2) & _
                                    ":" & _
                                    Right("00" & CStr(DatePart(DateInterval.Minute, Now())), 2) _
                                    }
            p.AddRecord(2) = temp

        Catch e As Exception
            doWarning(subName, e.Message)
        End Try
    End Sub

    Public Overrides Sub SyncNewData()

    End Sub

    Public Overrides Function Validate() As Boolean
        Return True
    End Function

End Class
