Public Class _Survey

    Inherits PDAOnBoardData.PDAData
    Dim ws As New priwebsvc.Service

#Region "Initialisation"

    Public Sub New(Optional ByRef App As PDAOnBoardData.BaseForm = Nothing)

        _App = App

        With Me
            .Name = "Survey"
            .ConQuery = "SELECT * FROM V_SVCCALL_SURVEY"
            .Column(0) = "QUESTF"
            .Column(1) = "QUESTFDES"
            .Column(2) = "QUESTNUM"
            .Column(3) = "QUESTDES"
            .Column(4) = "ANSNUM"
            .Column(5) = "ANSDES"
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
            Cancel = True
            doWarning(subName, "Could not download data." & vbCrLf & "Please check your connection and try again.")
        Catch e As Exception
            doWarning(subName, e.Message)
        End Try

    End Sub

    Public Overrides Sub SyncNewData()

        Dim subName As String = New StackTrace().GetFrame(0).GetMethod.ToString()
        Try
            ' New data arrives here

            If Not IsNothing(tempArray) Then
                For i As Integer = 0 To UBound(tempArray, 2)
                    tempArray(3, i) = nohtml(tempArray(3, i))
                Next

                thisArray = tempArray

                ' Caller must save the data if changed
                If Not Save() Then
                    MsgBox("File I/O", , " Error saving " & Name & ".")
                Else
                    HasNewData = True
                End If
            End If

        Catch e As Exception
            doWarning(subName, e.Message)
        End Try

    End Sub

    Public Overrides Sub LoadData(ByVal Ordinal As Integer)

        Dim subName As String = New StackTrace().GetFrame(0).GetMethod.ToString()
        Try

            ' This dataset is not loaded back to Priority

        Catch e As Exception
            doWarning(subName, e.Message)
        End Try

    End Sub

    Public Overrides Function Validate() As Boolean

        Dim subName As String = New StackTrace().GetFrame(0).GetMethod.ToString()
        Try
            ' Does the selected part exist in the recordset
            Dim v As Boolean = currentOrdinal > -1
            If Not v Then doWarning(subName, currentIndex & " is not a valid part.")
            Return v
        Catch e As Exception
            doWarning(subName, e.Message)
        End Try

    End Function

#End Region

#Region "Private Functions"

    Private Function NoHTML(ByVal str As String) As String
        Dim ret As String = ""
        Dim li() As String = Split(str, "<")
        For Each l As String In li
            If InStr(l, ">") > 0 Then
                ret = ret & Split(l, ">")(1)
            Else
                ret = ret & l
            End If
        Next
        Return ret
    End Function

#End Region

End Class
