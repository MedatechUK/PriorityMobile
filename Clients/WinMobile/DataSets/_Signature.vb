Public Class cfSignature

    Inherits cfOnBoardData.PDAData
    Dim ws As New priwebsvc.Service

#Region "Initialisation"

    Public Sub New(Optional ByRef App As cfOnBoardData.BaseForm = Nothing)

        CallerApp = App

        With Me
            .Name = "Signature"
            .ConQuery = Nothing
            .Column(0) = "DOCNO"
            .Column(1) = "SIGDATA"
            .Column(2) = "LOAD"
            .Column(3) = "FILENAME"
        End With

    End Sub

#End Region

#Region "Must Override Subs"

    Public Overrides Function SaveSig(ByVal Data As String, ByRef Response As String) As Boolean

        Dim subName As String = "SaveSig" 'New StackTrace().GetFrame(0).GetMethod.ToString()
        Try
            ' Connect to the web service and save data
            Response = ws.SaveSignature(Data)
            Return True
        Catch e As Exception
            Return False
        End Try

    End Function

    Public Overrides Function ConWebService(ByRef data) As Boolean

        Dim subName As String = "ConWebService" 'New StackTrace().GetFrame(0).GetMethod.ToString()
        Try
            ' Does not connect to the web service
            data = ""
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
            ' Does not sync

        Catch e As Exception
            doWarning(subName, e.Message)
        End Try

    End Sub

    Public Overrides Sub LoadData(ByVal Ordinal As Integer)

        Dim subName As String = "LoadData" 'New StackTrace().GetFrame(0).GetMethod.ToString()
        Try

            With p
                '.DebugFlag = True
                .Procedure = "ZPDA_LOAD_SIG"
                .Table = "ZPDA_SIG_LOAD"
                .RecordType1 = "DOCNO,SIGNAME"
                .RecordType2 = ""
                .RecordTypes = "TEXT,TEXT"
            End With

            ' Type 1 records
            Dim t1() As String = { _
                                GetField("DOCNO"), _
                                GetField("FILENAME") _
                                }
            p.AddRecord(1) = t1

        Catch e As Exception
            doWarning(subName, e.Message)
        End Try

    End Sub

    Public Overrides Function Validate() As Boolean

        Dim subName As String = "Validate" 'New StackTrace().GetFrame(0).GetMethod.ToString()
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
