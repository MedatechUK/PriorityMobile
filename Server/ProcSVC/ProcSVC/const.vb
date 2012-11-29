Imports System
Imports System.IO
Imports System.Net
Imports System.Net.Sockets
Imports System.Text
Imports Microsoft.VisualBasic
Imports System.Threading

Module Module1

    Public _Domain As String
    Public _UserName As String
    Public _Password As String
    Public _Bin95 As String
    Public _PriorityUser As String
    Public _PriorityPwd As String
    Public _PriorityCompany As String
    Public _prepUNC As String
    Public _prepMapDrive As String
    Public _Proc As String

    Dim _AppName As String = "Priority Web Service"
    Dim _LogName As String = "Application"

    Function loadconfig() As Boolean

        Try
            With PriorityConfig.Current
                _Bin95 = .Bin95
                _PriorityUser = .PriorityUser
                _PriorityPwd = .PriorityPwd
                _PriorityCompany = .PriorityCompany
                _Domain = .RunAs.Domain
                _UserName = .RunAs.Username
                _Password = .RunAs.Password
                _prepUNC = .Prepare.UNC
                _prepMapDrive = .Prepare.MapDrive
            End With
        Catch
            Return False
        End Try
        Return True

    End Function

    Public Sub WriteToEventLog(ByVal Entry As String, _
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

End Module
