Public Class _Parts
    Inherits PDAOnBoardData.PDAData
    Dim ws As New priwebsvc.Service

#Region "Initialisation"

    Public Sub New(Optional ByRef App As PDAOnBoardData.BaseForm = Nothing)

        _App = App

        With Me
            .Name = "Parts"
            .ConQuery = Nothing
            .Column(0) = "INDEX"
            .Column(1) = "DOCNO"
            .Column(2) = "PARTNAME"
            .Column(3) = "QTY"
        End With

    End Sub

#End Region

#Region "Must Override Subs"

    Public Overrides Function ConWebService(ByRef data) As Boolean

        Dim subName As String = New StackTrace().GetFrame(0).GetMethod.ToString()
        Try
            ' Does not connect to the web service
            data = ""
            Return True
        Catch e As Exception
            Return False
        End Try

    End Function

    Public Overrides Sub ConFail(ByRef Cancel As Boolean)

        Dim subName As String = New StackTrace().GetFrame(0).GetMethod.ToString()
        Try
            ' Did not connect
            ' Cancel = True terminates the running thread
            Cancel = False

        Catch e As Exception
            doWarning(subName, e.Message)
        End Try

    End Sub

    Public Overrides Sub SyncNewData()

        Dim subName As String = New StackTrace().GetFrame(0).GetMethod.ToString()
        Try
            ' Does not sync

        Catch e As Exception
            doWarning(subName, e.Message)
        End Try

    End Sub

    Public Overrides Sub LoadData(ByVal Ordinal As Integer)

        Dim subName As String = New StackTrace().GetFrame(0).GetMethod.ToString()
        Try
            With p
                '.DebugFlag = True
                .Procedure = "ZPDA_LOAD_PART"
                .Table = "ZPDA_PART_LOAD"
                .RecordType1 = "DOCNO"
                .RecordType2 = "CURDATE,WARHS,PARTNAME,QTY"
                .RecordTypes = "TEXT,,TEXT,TEXT,INT"
            End With

            Dim cond(1, 0) As String
            cond(0, 0) = "1"
            cond(1, 0) = currentIndex
            Dim parts() As Integer = ar.Matching(thisArray, cond)

            ' Type 1 records
            Dim t1() As String = { _
                                thisArray(1, parts(0)) _
                                }
            p.AddRecord(1) = t1

            For i As Integer = 0 To UBound(parts)
                Dim temp() As String = {DatetoMin(), _
                                        My.Settings.Warehouse, _
                                        thisArray(2, parts(i)), _
                                        thisArray(3, parts(i))}
                p.AddRecord(2) = temp
            Next

        Catch e As Exception
            doWarning(subName, e.Message)
        End Try

    End Sub

    Public Overrides Function Validate() As Boolean

        Dim subName As String = New StackTrace().GetFrame(0).GetMethod.ToString()
        Try

            ' Does the selected part exist in the recordset
            If InStr(currentIndex, "/") > 0 Then
                Return currentOrdinal > -1
            Else
                Return UBound(Selection) > -1
            End If

        Catch e As Exception
            doWarning(subName, e.Message)
        End Try

    End Function

    Public Overrides Function Selection() As Integer()

        Dim cond(1, 0) As String
        cond(0, 0) = "1"
        cond(1, 0) = currentIndex
        Return ar.Matching(thisArray, cond)

    End Function

#End Region

End Class
