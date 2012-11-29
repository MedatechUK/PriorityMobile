Imports Priority.MyArray

Public Class _Repair

    Inherits PDAOnBoardData.PDAData
    Dim ws As New priwebsvc.Service

#Region "Initialisation"

    Public Sub New(Optional ByRef App As PDAOnBoardData.BaseForm = Nothing)

        _App = App

        With Me
            .Name = "Repair"
            .ConQuery = Nothing
            .Column(0) = "DOCNO"
            .Column(1) = "REPAIRTEXT"
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

        Dim Build As String = ""
        Dim subName As String = New StackTrace().GetFrame(0).GetMethod.ToString()
        Try
            With p
                '.DebugFlag = True
                .Procedure = "ZPDA_LOAD_REPAIR"
                .Table = "ZPDA_REPAIR_LOAD"
                .RecordType1 = "DOCNO"
                .RecordType2 = "REPAIRTEXT"
                .RecordTypes = "TEXT,TEXT"
            End With

            ' Type 1 records
            Dim t1() As String = { _
                                GetField("DOCNO") _
                                }
            p.AddRecord(1) = t1

            Dim l1() As String = {"<style> p,div,li "}
            p.AddRecord(2) = l1
            Dim l2() As String = {"{margin:0cm;font-size:10.0pt;font-family:'Arial';}</style>"}
            p.AddRecord(2) = l2

            Dim rText As String = GetField("REPAIRTEXT")
            Dim ln() As String = Split(Replace(rText, Chr(10), ""), Chr(13) & Chr(13))
            rText = ""
            For Each l As String In ln
                rText = rText & " <p> " & Replace(l, Chr(13), " <br> ") & " </p> "
            Next

            Dim words = Split(rText, " ")
            For i As Integer = 0 To UBound(words)
                If Len(Build & " " & words(i)) > 68 Then
                    Dim temp1() As String = {Build}
                    p.AddRecord(2) = temp1
                    Build = "" & words(i) & " "
                Else
                    Build = Build & words(i) & " "
                End If
            Next

            Dim temp() As String = {Build}
            p.AddRecord(2) = temp
            Build = ""


        Catch e As Exception
            doWarning(subName, e.Message)
        End Try

    End Sub

    Public Overrides Function Validate() As Boolean

        Dim subName As String = New StackTrace().GetFrame(0).GetMethod.ToString()
        Try

            ' Does the selected part exist in the recordset
            Dim v As Boolean = currentOrdinal > -1
            Return v

        Catch e As Exception
            doWarning(subName, e.Message)
        End Try

    End Function

    Public Overrides Function Selection() As Integer()

        Dim cond(1, 0) As String
        cond(0, 0) = "0"
        cond(1, 0) = currentIndex
        Return ar.Matching(thisArray, cond)

    End Function

#End Region

End Class
