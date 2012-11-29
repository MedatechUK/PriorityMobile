Public Class cfAnswers

    Inherits cfOnBoardData.PDAData
    Dim ws As New priwebsvc.Service

#Region "Initialisation"

    Public Sub New(Optional ByRef App As cfOnBoardData.BaseForm = Nothing)

        CallerApp = App

        With Me
            .Name = "Answers"
            .ConQuery = ""
            .Column(0) = "INDEX"
            .Column(1) = "DOCNUM"
            .Column(2) = "QUESTF"
            .Column(3) = "QUESTNUM"
            .Column(4) = "ANSNUM"
            .Column(5) = "ANSTEXT"
        End With

    End Sub

#End Region

#Region "Must Override Subs"

    Public Overrides Function ConWebService(ByRef data) As Boolean

        Dim subName As String = "ConWebService" 'New StackTrace().GetFrame(0).GetMethod.ToString()
        Try
            ' Connect to the web service and get data
            ' Does not connect

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
            Cancel = True
            doWarning(subName, "Could not download data." & vbCrLf & "Please check your connection and try again.")
        Catch e As Exception
            doWarning(subName, e.Message)
        End Try

    End Sub

    Public Overrides Sub SyncNewData()

        Dim subName As String = "SyncNewData" 'New StackTrace().GetFrame(0).GetMethod.ToString()
        Try
            ' Do nothing

        Catch e As Exception
            doWarning(subName, e.Message)
        End Try

    End Sub

    Public Overrides Sub LoadData(ByVal Ordinal As Integer)

        Dim Build As String = ""
        Dim subName As String = "LoadData" 'New StackTrace().GetFrame(0).GetMethod.ToString()
        Try
            With p
                '.DebugFlag = True
                .Procedure = "ZPDA_LOAD_ANSWERS"
                .Table = "ZPDA_ANSWERS_LOAD"
                .RecordType1 = "CURDATE,DOCNO,QUESTF"
                .RecordType2 = "QUESTNUM,ANSNUM,ANSTEXT"
                .RecordTypes = ",TEXT,TEXT,,,TEXT"
            End With

            Dim ans() As Integer = Selection()

            ' Type 1 records
            Dim t1() As String = { _
                                DatetoMin(), _
                                thisArray(1, ans(0)), _
                                thisArray(2, ans(0)) _
                                }
            p.AddRecord(1) = t1

            For i As Integer = 0 To UBound(ans)
                Dim temp() As String = { _
                                        thisArray(3, ans(i)), _
                                        qnum( _
                                            thisArray(2, ans(0)), _
                                            thisArray(3, ans(i)), _
                                            thisArray(4, ans(i)) _
                                        ), _
                                        "" _
                                        }
                p.AddRecord(2) = temp


                Dim rText As String = thisArray(5, ans(i))
                If Len(rText) > 0 Then
                    Dim l1() As String = { _
                            "-1", _
                            "-1", _
                            "<style> p,div,li "}
                    p.AddRecord(2) = l1

                    Dim l2() As String = { _
                            "-1", _
                            "-1", _
                            "{margin:0cm;font-size:10.0pt;font-family:'Arial';}</style>"}
                    p.AddRecord(2) = l2

                    Dim ln() As String = Split(Replace(rText, Chr(10), ""), Chr(13) & Chr(13))
                    rText = ""
                    For Each l As String In ln
                        rText = rText & " <p> " & Replace(l, Chr(13), " <br> ") & " </p> "
                    Next

                    Dim words() = Split(rText, " ")
                    For a As Integer = 0 To UBound(words)
                        If Len(Build & " " & words(a)) > 68 Then

                            Dim temp1() As String = { _
                            "-1", _
                            "-1", _
                            Build}
                            p.AddRecord(2) = temp1

                            Build = "" & words(a) & " "
                        Else
                            Build = Build & words(a) & " "
                        End If
                    Next

                    Dim temp2() As String = { _
                            "-1", _
                            "-1", _
                            Build}
                    p.AddRecord(2) = temp2
                    Build = ""

                End If


            Next

        Catch e As Exception
            doWarning(subName, e.Message)
        End Try

    End Sub

    Public Overrides Function Validate() As Boolean

        Dim subName As String = "Validate" 'New StackTrace().GetFrame(0).GetMethod.ToString()
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

#Region "Private Functions"

    Private Function qnum(ByVal QUESTF As String, ByVal QUESTNUM As String, ByVal ANSDES As String)
        Dim cond(1, 2) As String
        cond(0, 0) = "0"
        cond(1, 0) = QUESTF
        cond(0, 1) = "2"
        cond(1, 1) = QUESTNUM
        cond(0, 2) = "5"
        cond(1, 2) = ANSDES
        With CallerApp.rss(o.Survey)
            Return .thisArray(4, ar.Matching(.thisArray, cond)(0))
        End With
    End Function

#End Region
End Class
