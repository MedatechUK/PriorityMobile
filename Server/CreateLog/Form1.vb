Imports System.Web
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System
Imports System.Collections
Imports System.Text
Imports System.Configuration
Imports System.Xml
Imports System.Diagnostics
Imports System.Net
Imports System.Net.Sockets
Imports System.Threading

Public Class Form1

    Dim _AppName As String = "Priority Web Service"
    Dim _LogName As String = "Application"

    Private Sub Form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        WriteToEventLog("Test of application log." _
                    , EventLogEntryType.SuccessAudit)
    End Sub

    Private Sub WriteToEventLog(ByVal Entry As String, _
  Optional ByVal EventType As _
    EventLogEntryType = EventLogEntryType.Information)

        Dim objEventLog As New EventLog()

        Try
            'Register the App as an Event Source
            If Not objEventLog.SourceExists(_AppName) Then
                objEventLog.CreateEventSource(_AppName, _LogName)
            End If

            objEventLog.Source = _AppName

            'WriteEntry is overloaded; this is one
            'of 10 ways to call it
            objEventLog.WriteEntry(Entry, EventType)

        Catch Ex As Exception

        End Try

    End Sub

End Class
