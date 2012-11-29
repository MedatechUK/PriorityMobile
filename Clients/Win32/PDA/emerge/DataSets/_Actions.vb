Public Class _Actions
    Inherits PDAOnBoardData.PDAData
    Dim ws As New priwebsvc.Service

#Region "Initialisation"

    Public Sub New(Optional ByRef App As PDAOnBoardData.BaseForm = Nothing)

        _App = App

        With Me
            .Name = "Actions"
            .ConQuery = ""
            .Column(0) = "INDEX"
            .Column(1) = "ACTION"
            .Column(2) = "STATUS"
        End With

    End Sub

#End Region

#Region "Must Override Subs"

    Public Overrides Function ConWebService(ByRef data) As Boolean

        Dim subName As String = New StackTrace().GetFrame(0).GetMethod.ToString()
        Try
            ' Connect to the web service and get data
            ' Does not connect

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
            ' Do nothing

        Catch e As Exception
            doWarning(subName, e.Message)
        End Try

    End Sub

    Public Overrides Sub LoadData(ByVal Ordinal As Integer)

        Dim subName As String = New StackTrace().GetFrame(0).GetMethod.ToString()
        Try
            ' Do nothing

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
