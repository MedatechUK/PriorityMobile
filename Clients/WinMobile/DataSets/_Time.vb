Public Class cfTime
    Inherits cfOnBoardData.PDAData
    Dim ws As New priwebsvc.Service

#Region "Initialisation"

    Public Sub New(Optional ByRef App As cfOnBoardData.BaseForm = Nothing)

        CallerApp = App

        With Me
            .Name = "Time"
            .ConQuery = Nothing
            .Column(0) = "DOCNO"
            .Column(1) = "ONROUTE"
            .Column(2) = "ONSITE"
            .Column(3) = "END"
        End With

    End Sub

#End Region

#Region "Must Override Subs"

    Public Overrides Function ConWebService(ByRef data) As Boolean

        Dim subName As String = "ConWebService" 'New StackTrace().GetFrame(0).GetMethod.ToString()
        Try
            ' Does not connect to the web service
            data = ""
            Return True
        Catch e As Exception
            Return False
        End Try

    End Function

    Public Overrides Sub ConFail(ByRef Cancel As Boolean)

        Dim subName As String = "ConFail" 'New StackTrace().GetFrame(0).GetMethod.ToString()
        Try
            ' Did not connect
            ' Cancel = True terminates the running thread
            Cancel = False

        Catch e As Exception
            doWarning(subName, e.Message)
        End Try

    End Sub

    Public Overrides Sub SyncNewData()

        Dim subName As String = "SyncNewData" 'New StackTrace().GetFrame(0).GetMethod.ToString()
        Try
            ' Does not sync

        Catch e As Exception
            doWarning(subName, e.Message)
        End Try

    End Sub

    Public Overrides Sub LoadData(ByVal Ordinal As Integer)

        Dim subName As String = "LoadData" 'New StackTrace().GetFrame(0).GetMethod.ToString()
        Try
            With p
                '.DebugFlag = True
                .Procedure = "ZPDA_LOAD_TIME"
                .Table = "ZPDA_TIME_LOAD"
                .RecordType1 = "DOCNO"
                .RecordType2 = "USERNAME,CURDATE,ONROUTE,ONSITE,ENDCALL"
                .RecordTypes = "TEXT,TEXT,,TIME,TIME,TIME"
            End With

            ' Type 1 records
            Dim t1() As String = { _
                                GetField("DOCNO") _
                                }
            p.AddRecord(1) = t1

            Dim temp() As String = {CallerApp.mySettings.Username, _
                                    DatetoMin(), _
                                    TravelTime(fixTime(GetField("ONROUTE")), fixTime(GetField("ONSITE"))), _
                                    fixTime(GetField("ONSITE")), _
                                    fixTime(GetField("END")) _
                                    }
            p.AddRecord(2) = temp

        Catch e As Exception
            doWarning(subName, e.Message)
        End Try

    End Sub

    Public Overrides Function Validate() As Boolean

        Dim subName As String = "Validate" 'New StackTrace().GetFrame(0).GetMethod.ToString()
        Try

            ' Does the selected part exist in the recordset
            Dim v As Boolean = currentOrdinal > -1
            Return v

        Catch e As Exception
            doWarning(subName, e.Message)
        End Try

    End Function

#End Region

#Region "Private Functions"

    Private Function TravelTime(ByVal er As String, ByVal os As String) As String

        Try
            Dim min As Integer = DateDiff(DateInterval.Minute, CDate(er), CDate(os))
            Dim hr As String = Right("00" & Split(CStr(Int(min / 60)), ".")(0), 2)
            Dim mi As String = Right("00" & CStr(min - (60 * Int(min / 60))), 2)
            Return hr & ":" & mi
        Catch
            Return "00:00"
        End Try

    End Function

    Private Function fixTime(ByVal t As String)
        If Not regex_Time.IsMatch(t) Then
            Return "00:00"
        Else
            Return t
        End If
    End Function

#End Region

End Class
