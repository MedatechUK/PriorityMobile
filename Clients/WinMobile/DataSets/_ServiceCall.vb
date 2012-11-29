Public Class cfServiceCall
    Inherits cfOnBoardData.PDAData
    Dim ws As New priwebsvc.Service

#Region "Initialisation"

    Public Sub New(Optional ByRef App As cfOnBoardData.BaseForm = Nothing)

        CallerApp = App
        With Me
            .Name = "ServiceCall"
            .ConQuery = "SELECT * FROM V_SERVCALL WHERE USERLOGIN = '" & CallerApp.mySettings.Username & "' "
            .Column(0) = "DOCNO"
            .Column(1) = "DATEOPENED"
            .Column(2) = "PDATE"
            .Column(3) = "USERLOGIN"
            .Column(4) = "STATUS"
            .Column(5) = "CUSTNAME"
            .Column(6) = "ADDRESS1"
            .Column(7) = "ADDRESS2"
            .Column(8) = "ADDRESS3"
            .Column(9) = "POSTCODE"
            .Column(10) = "COUNTY"
            .Column(11) = "CONTACT"
            .Column(12) = "PHONENUM"
            .Column(13) = "SERVDES"
            .Column(14) = "STARTTIME"
        End With

    End Sub

#End Region

#Region "Must Override Subs"

    Public Overrides Sub ChangeSettings()
        Me.ConQuery = "SELECT * FROM V_SERVCALL WHERE USERLOGIN = '" & CallerApp.mySettings.Username & "' "
    End Sub

    Public Overrides Function ConWebService(ByRef data) As Boolean

        Dim subName As String = "ConWebService" 'New StackTrace().GetFrame(0).GetMethod.ToString()
        Try
            ' Connect to the web service and get data
            data = ws.GetData(ConQuery)
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
            ' New data arrives here
            If IsNothing(tempArray) Then Exit Sub

            'Iterate through new calls
            For i As Integer = 0 To UBound(tempArray, 2)
                ' If call exists
                Dim id As Integer = ar.InArray(thisArray, 0, tempArray(0, i))
                If id > -1 Then
                    'Call has already been downloaded
                    For x As Integer = 1 To UBound(thisArray) - 1
                        If Not x = 4 Then ' preserve the locally held status
                            thisArray(x, id) = tempArray(x, i)
                        End If
                    Next
                    'Is this the displayed call
                    If thisArray(0, id) = currentIndex Then
                        RedrawForm = True
                    End If
                Else
                    ' Call does not currently exist
                    ' Has it been closed?                
                    If ar.InArray(deletedArray, 0, tempArray(0, i)) = -1 Then
                        ' No, it's new, so add it to the temp array
                        HasNewData = True

                        Try
                            ReDim Preserve thisArray(UBound(tempArray, 1), UBound(thisArray, 2) + 1)
                        Catch
                            ReDim Preserve thisArray(UBound(tempArray, 1), 0)
                        End Try

                        For n As Integer = 0 To UBound(thisArray, 1)
                            thisArray(n, UBound(thisArray, 2)) = tempArray(n, i)
                        Next
                        'If currentIndex = "" Then 
                        RedrawForm = True

                    Else
                        'It is in deletedArray - has it been re-issued?
                        If ReIssued(tempArray(4, i)) Then
                            HasNewData = True

                            ' It has been re-issued, so remove the delete reference
                            UnDelete(tempArray(0, i))

                            ' And add it to the temp array
                            Try
                                ReDim Preserve thisArray(UBound(tempArray, 1), UBound(thisArray, 2) + 1)
                            Catch
                                ReDim Preserve thisArray(UBound(tempArray, 1), 0)
                            End Try

                            For n As Integer = 0 To UBound(thisArray, 1)
                                thisArray(n, UBound(thisArray, 2)) = tempArray(n, i)
                            Next
                            'If currentIndex = "" Then 
                            RedrawForm = True

                        End If
                    End If
                End If
            Next

            'Has the array changed?
            If Not Array.Equals(thisArray, tempArray) Then
                ' Caller must save the data if changed
                If Not Save() Then
                    MsgBox("File I/O", , " Error saving " & Name & ".")
                End If
            End If

        Catch e As Exception
            doWarning(subName, e.Message)
        End Try

    End Sub

    Public Overrides Sub LoadData(ByVal Ordinal As Integer)

        Dim subName As String = "LoadData" 'New StackTrace().GetFrame(0).GetMethod.ToString()
        Try
            With p
                ''.DebugFlag = True
                .Procedure = "ZPDA_LOAD_STATUS"
                .Table = "ZPDA_STATUS_LOAD"
                .RecordType1 = "DOCNO,STATDES"
                .RecordType2 = ""
                .RecordTypes = "TEXT,TEXT"
            End With

            ' Type 1 records
            Dim t1() As String = { _
                                GetField("DOCNO"), _
                                GetField("STATUS") _
                                }
            p.AddRecord(1) = t1

        Catch e As Exception
            doWarning(subName, e.Message)
        End Try

    End Sub

    Public Overrides Function Validate() As Boolean

        Dim subName As String = "Validate" 'New StackTrace().GetFrame(0).GetMethod.ToString()
        Try
            ' Check to see if data contained here is valid
            Dim v As Boolean = currentOrdinal > -1
            If Not v Then doWarning(subName, "No service call to load.")
            Return v
        Catch e As Exception
            doWarning(subName, e.Message)
        End Try

    End Function

#End Region

#Region "private functions"

    Private Function ReIssued(ByVal Status As String) As Boolean
        With CallerApp.rss(o.Actions)
            For i As Integer = 0 To UBound(.thisArray, 2)
                If LCase(Trim(.thisArray(1, i))) = LCase(Trim(txt_IsReIssue)) Then
                    If LCase(Trim(Status)) = LCase(Trim(.thisArray(2, i))) Then
                        Return True
                        Exit Function
                    End If
                End If
            Next
        End With
        Return False
    End Function

#End Region

End Class
