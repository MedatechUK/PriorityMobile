Imports System.Threading
Imports ntDictionaryLib
Imports System.io

Public Class EVq
    Inherits dCls

    Private _ev As ntEvtlog.evt

    Sub New(Optional ByRef ev As ntEvtlog.evt = Nothing)

        MyBase.New("ntDictionaryLib.eventItem")

        ' Initialise the object
        With Me
            .Name = "eventq"
            .Undelete = False
        End With

        If Not IsNothing(ev) Then
            _ev = ev
        End If

        Load(tSource.File)

    End Sub

    Public Overrides Function Index(ByVal RowData() As String) As String
        Return RowData(0)
    End Function

    Public Overrides Function Columns() As String()
        ' Set the columns
        Dim myCols() As String = { _
            "BUBBLEID", "CALLER", "SOAPPROC", "RESULT", "DATA" _
        }
        Return myCols
    End Function

    Public Overrides Sub OnInsertComplete(ByVal key As Object, ByVal value As Object)
        Try
            MyBase.OnInsertComplete(key, value)
            If Not Me.Seeking Then
                Me.Save(tSource.File)
            End If
        Catch ex As Exception
            Me.SafeLog( _
                String.Format( _
                    "An error occured whilst inserting an item into the queue:{0}{1}", _
                    vbCrLf, _
                    ex.Message _
                ), _
                 EventLogEntryType.Information, _
                 ntEvtlog.EvtLogVerbosity.Normal _
            )
        End Try
    End Sub

    Public Overrides Sub OnRemoveComplete(ByVal key As Object, ByVal value As Object)
        MyBase.OnRemoveComplete(key, value)
        Me.SafeLog( _
            String.Format( _
                "The file was removed from the queue:{0}{1}.", _
                vbCrLf, _
                key _
            ), _
             EventLogEntryType.SuccessAudit, _
             ntEvtlog.EvtLogVerbosity.Verbose _
        )
        Me.Save(tSource.File)
    End Sub

    Private Sub SafeLog(ByVal Entry As String, ByVal EventType As EventLogEntryType, ByVal Verbosity As ntEvtlog.EvtLogVerbosity)
        Try
            If IsNothing(_ev) Then Throw New Exception("No event object specified.")
            _ev.Log( _
                Entry, _
                EventType, _
                Verbosity _
              )
        Catch exep As Exception
            Console.WriteLine( _
                "Failed to write to the [{0} {1}] log.{4}" & _
                "The error reported was: {4}{2}{4}" & _
                "The event could not be written to the log because: {4}{3}{4}", _
                _ev.LogName, _ev.AppName, Entry, exep.Message, vbCrLf _
            )
        End Try
    End Sub

End Class