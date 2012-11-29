Public Class _Flags
    Inherits PDAOnBoardData.PDAData
    Dim ws As New priwebsvc.Service

#Region "Initialisation"

    Public Sub New(Optional ByRef App As PDAOnBoardData.BaseForm = Nothing)

        _App = App

        With Me
            .Name = "Flags"
            .ConQuery = ""
            .Column(0) = "DOCNO"
            .Column(1) = "MALFCODE"
            .Column(2) = "RESCODE"
        End With

    End Sub

#End Region

#Region "Must Override Subs"

    Public Overrides Function ConWebService(ByRef data) As Boolean

        Dim subName As String = New StackTrace().GetFrame(0).GetMethod.ToString()
        Try
            ' Connect to the web service and get data
            data = ws.GetData(ConQuery)
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
            ' New data arrives here
            If IsNothing(tempArray) Then Exit Sub

            'Iterate through new calls
            For i As Integer = 0 To UBound(tempArray, 2)
                ' If call exists
                Dim id As Integer = ar.InArray(thisArray, 0, tempArray(0, i))
                If id > -1 Then
                    'Call has already been downloaded
                    For x As Integer = 1 To UBound(thisArray) - 1
                        thisArray(x, id) = tempArray(x, i)
                    Next
                    'Is this the displayed call
                    If thisArray(0, id) = currentIndex Then
                        RedrawForm = True
                    End If
                Else
                    HasNewData = True
                    ' Call does not currently exist
                    ' Has it been closed?                
                    If ar.InArray(deletedArray, 0, tempArray(0, i)) = -1 Then
                        ' No, it's new, so add it to the temp array
                        Try
                            ReDim Preserve thisArray(UBound(tempArray, 1), UBound(thisArray, 2) + 1)
                        Catch
                            ReDim Preserve thisArray(UBound(tempArray, 1), 0)
                        End Try

                        For n As Integer = 0 To UBound(thisArray, 1)
                            thisArray(n, UBound(thisArray, 2)) = tempArray(n, i)
                        Next
                        If currentIndex = "" Then RedrawForm = True

                    Else
                        'It is in deletedArray - has it been re-issued?
                        If Left(tempArray(UBound(tempArray, 1), i), Len("Issued")) = "Issued" Then

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
                            If currentIndex = "" Then RedrawForm = True

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

        Dim subName As String = New StackTrace().GetFrame(0).GetMethod.ToString()
        Try
            With p
                ''.DebugFlag = True
                .Procedure = "ZPDA_LOAD_FLAGS"
                .Table = "ZPDA_FLAGS_LOAD"
                .RecordType1 = "DOCNO,MALFCODE,RESCODE"
                .RecordType2 = ""
                .RecordTypes = "TEXT,TEXT,TEXT"
            End With

            ' Type 1 records
            Dim t1() As String = { _
                                GetField("DOCNO"), _
                                MalfCode(GetField("MALFCODE")), _
                                ResCode(GetField("RESCODE")) _
                                }
            p.AddRecord(1) = t1

        Catch e As Exception
            doWarning(subName, e.Message)
        End Try

    End Sub

    Public Overrides Function Validate() As Boolean

        Dim subName As String = New StackTrace().GetFrame(0).GetMethod.ToString()
        Try
            ' Check to see if data contained here is valid
            Dim v As Boolean = currentOrdinal > -1
            'If Not v Then doWarning(subName, "No service call to load.", False)
            Return v
        Catch e As Exception
            doWarning(subName, e.Message)
        End Try

    End Function

#End Region

#Region "Private functions"

    Function MalfCode(ByVal des As String) As String
        Dim ret As String = ""
        With _App.rss(o.Malfunction)
            If Not IsNothing(.thisArray) Then
                For i As Integer = 0 To UBound(.thisArray, 2)
                    If LCase(Trim(.thisArray(1, i))) = LCase(Trim(des)) Then
                        Return .thisArray(0, i)
                    End If
                Next
            End If
        End With
        Return ret
    End Function

    Function ResCode(ByVal des As String) As String
        Dim ret As String = ""
        With _App.rss(o.Resolution)
            If Not IsNothing(.thisArray) Then
                For i As Integer = 0 To UBound(.thisArray, 2)
                    If LCase(Trim(.thisArray(1, i))) = LCase(Trim(des)) Then
                        Return .thisArray(0, i)
                    End If
                Next
            End If
        End With
        Return ret
    End Function

#End Region

End Class
