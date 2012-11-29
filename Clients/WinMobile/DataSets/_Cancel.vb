Public Class cfCancel
    Inherits cfOnBoardData.PDAData
    Dim ws As New priwebsvc.Service

#Region "Initialisation"

    Public Sub New(Optional ByRef App As cfOnBoardData.BaseForm = Nothing)

        CallerApp = App

        With Me
            .Name = "Cancel"
            .ConQuery = "Select * from ZPDA_CALLCANCEL WHERE USERNAME = '" & CallerApp.mySettings.Username & "' "
            .Column(0) = "DOCNO"
        End With

    End Sub

#End Region

#Region "Must Override Subs"

    Public Overrides Sub ChangeSettings()
        Me.ConQuery = "Select * from ZPDA_CALLCANCEL WHERE USERNAME = '" & CallerApp.mySettings.Username & "' "
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
                Dim id As Integer = ar.InArray(CallerApp.rss(o.ServiceCall).thisArray, 0, tempArray(0, i))
                If id > -1 Then
                    'Call has been cancelled
                    Dim _ServiceCall As String = tempArray(0, i)

                    With CallerApp.rss(o.Repair)
                        .currentIndex = _ServiceCall
                        If .Validate Then
                            .DeleteIndex()
                        End If
                    End With

                    With CallerApp.rss(o.Time)
                        .currentIndex = _ServiceCall
                        If .Validate Then
                            .DeleteIndex()
                        End If
                    End With

                    CancelParts(_ServiceCall)
                    With CallerApp.rss(o.Parts)
                        .currentIndex = _ServiceCall
                        If .Validate Then
                            .DeleteIndex()
                        End If
                    End With

                    With CallerApp.rss(o.Flags)
                        .currentIndex = _ServiceCall
                        If .Validate Then
                            .DeleteIndex()
                        End If
                    End With

                    With CallerApp.rss(o.Answers)
                        .currentIndex = _ServiceCall
                        If .Validate Then
                            .DeleteIndex()
                        End If
                    End With

                    With CallerApp.rss(o.ServiceCall)
                        .currentIndex = _ServiceCall
                        If .Validate Then
                            .DeleteIndex()
                        End If
                    End With

                    RedrawForm = True
                End If
            Next

        Catch e As Exception
            doWarning(subName, e.Message)
        End Try

    End Sub

    Public Overrides Sub LoadData(ByVal Ordinal As Integer)

        Dim subName As String = "LoadData" 'New StackTrace().GetFrame(0).GetMethod.ToString()
        Try
            ' Do nothing

        Catch e As Exception
            doWarning(subName, e.Message)
        End Try

    End Sub

    Public Overrides Function Validate() As Boolean

        Dim subName As String = "Validate" 'New StackTrace().GetFrame(0).GetMethod.ToString()
        Try

            Return True

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

    Sub CancelParts(ByVal SC As String)

        With CallerApp.rss(o.Parts)
            If Not IsNothing(.thisArray) Then
                For i As Integer = 0 To UBound(.thisArray, 2)
                    If LCase(Trim(.thisArray(i, 1))) = LCase(Trim(SC)) Then
                        Dim p As String = .thisArray(i, 2)
                        Dim q As Integer = CInt(.thisArray(i, 3))
                        Dim f As Boolean = False
                        With CallerApp.rss(o.Warehouse)
                            If Not IsNothing(.thisArray) Then
                                For w As Integer = 0 To UBound(.thisArray, 2)
                                    If LCase(Trim(.thisArray(0, w))) = LCase(Trim(p)) Then
                                        f = True
                                        .thisArray(2, w) = CStr(CInt(.thisArray(2, w)) + q)
                                        Exit For
                                    End If
                                Next
                                If Not f Then
                                    'NewWHItem(p, q)
                                End If
                            Else
                                'NewWHItem(p, q)
                            End If
                        End With
                    End If
                Next
            End If
        End With

    End Sub

#End Region

End Class
