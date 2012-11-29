Public Class _Details

    Inherits PDAOnBoardData.PDAData
    Dim ws As New priwebsvc.Service

#Region "Initialisation"

    Public Sub New(Optional ByRef App As PDAOnBoardData.BaseForm = Nothing)

        _App = App

        With Me
            .Name = "Details"
            .ConQuery = "SELECT * FROM V_SVCCALL_DETAILS WHERE USERLOGIN = '" & My.Settings.Username & "' "
            .Column(0) = "DOCNO"
            .Column(1) = "TEXT"
        End With

    End Sub

#End Region

#Region "Must Override Subs"

    Public Overrides Sub ChangeSettings()
        Me.ConQuery = "SELECT * FROM V_SVCCALL_DETAILS WHERE USERLOGIN = '" & My.Settings.Username & "' "
    End Sub

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

        Dim ch As Boolean = False
        Dim subName As String = New StackTrace().GetFrame(0).GetMethod.ToString()
        Try
            ' New data arrives here
            If Not IsNothing(tempArray) Then
                For i As Integer = 0 To UBound(tempArray, 2)
                    If ar.InArray(thisArray, 0, tempArray(0, i)) = -1 Then
                        ch = True
                        Dim n As Integer = _App.rss(o.Details).NewRecord
                        For x As Integer = 0 To UBound(thisArray, 1)
                            thisArray(x, n) = tempArray(x, i)
                        Next
                    End If
                Next
            End If

            ' Caller must save the data if changed
            If ch Then
                If Not Save() Then MsgBox("File I/O", , " Error saving " & Name & ".")
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
            Return v

        Catch e As Exception
            doWarning(subName, e.Message)
        End Try

    End Function

#End Region

End Class
