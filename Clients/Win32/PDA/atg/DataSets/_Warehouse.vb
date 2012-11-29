Public Class _Warehouse

    Inherits PDAOnBoardData.PDAData
    Dim ws As New priwebsvc.Service

#Region "Initialisation"

    Public Sub New(Optional ByRef App As PDAOnBoardData.BaseForm = Nothing)

        _App = App

        With Me
            .Name = "Warehouse"
            .ConQuery = "SELECT * FROM V_SVCCALL_WARHS WHERE WARHSNAME = '" & My.Settings.Warehouse & "' "
            .Column(0) = "PARTNAME"
            .Column(1) = "PARTDES"
            .Column(2) = "QTY"
            .Column(3) = "PRICE"
            .Column(4) = "WARHSNAME"
        End With

    End Sub

#End Region

#Region "Must Override Subs"

    Public Overrides Sub ChangeSettings()
        Me.ConQuery = "SELECT * FROM V_SVCCALL_WARHS WHERE WARHSNAME = '" & My.Settings.Warehouse & "' "
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
                    If Not IsNothing(_App.rss(o.Parts).thisArray) Then
                        Dim used(,) As String = ar.SubSet(_App.rss(o.Parts).thisArray, 2, tempArray(0, i))
                        Dim c As Integer = 0
                        If Not IsNothing(used) Then
                            For u As Integer = 0 To UBound(used, 2)
                                c = c + used(3, u)
                            Next
                        End If
                        tempArray(2, i) = tempArray(2, i) - c
                    End If
                Next
                thisArray = tempArray
            End If

            ' Caller must save the data if changed
            If Not Save() Then
                MsgBox("File I/O", , " Error saving " & Name & ".")
            Else
                HasNewData = True
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

End Class
