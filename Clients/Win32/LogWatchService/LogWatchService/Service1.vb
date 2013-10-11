Imports System.Diagnostics
Imports System.Data
Imports System.Net.Mail
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.Configuration.ConfigurationSettings
Imports System.Data.Sql
Imports System.Globalization
Imports System.Text.RegularExpressions
Imports System.IO
Imports System.Xml.Linq
Imports System.Timers

Public Class PriProcLogWatcher

#Region "Variables and Properties"
    '********** Variables **********
    Private ss As New ServSettings("", 0, "", "", "", "", "", False, "", "", "", False, False, False, "", "", 0, False)
    Private Property connstring() As String
        Get
            Return conn_string
        End Get
        Set(ByVal value As String)
            conn_string = value
        End Set
    End Property
    Private conn_string As String
    Private last_item_date As Date
    Private plog As Integer
    Private FCount As Integer
    Private FDateCheck As Date
#Region "Queued Items Variables"

    Private Property NumberOfFiles() As Integer
        Get
            Return FCount
        End Get
        Set(ByVal value As Integer)
            FCount = value
        End Set
    End Property

    Private Property lastcount() As Integer
        Get
            Return My.Settings.LastCoun
        End Get
        Set(ByVal value As Integer)
            My.Settings.LastCoun = value
        End Set
    End Property

    Private Property LastFileDate() As Date
        Get
            Return My.Settings.FileDate
        End Get
        Set(ByVal value As Date)
            My.Settings.FileDate = value
        End Set
    End Property

    Private Property dtchk() As Date
        Get
            Return FDateCheck
        End Get
        Set(ByVal value As Date)
            FDateCheck = value
        End Set
    End Property

    Private ReadOnly Property Path() As String
        Get
            Return "C:\inetpub\Mobile\queue"
        End Get
    End Property

#End Region
#Region "Event Log Variables"

    Private ReadOnly Property WMIQuery() As String
        Get
            Return String.Format( _
                "Select * from Win32_NTLogEvent " & _
                "Where Logfile = '{0}' " & _
                "and SourceName = '{1}'  " & _
                "and TimeWritten > {2} " & _
                "{3}", _
                LogFile, _
                SourceName, _
                LastItemDate, _
                EventLevel)
        End Get
    End Property

    Private ReadOnly Property FormatDate(ByVal dt As Date) As String
        Get
            Return String.Format("'{0}.000000-000'", dt.ToString("yyyyMMddHHmmss"))
        End Get
    End Property

    Private ReadOnly Property prilog() As DataTable
        Get
            Dim dterr As New DataTable
            Try
                With dterr
                    .Rows.Clear()
                    .Clear()

                    With .Columns
                        .Add("Index", GetType(Integer))
                        .Add("EntryType", GetType(String))
                        .Add("Img", GetType(String))
                        .Add("TimeGenerated", GetType(String))
                        .Add("Message", GetType(String))
                    End With

                    For Each objEvent As Object In ObjWMIEvt.ExecQuery(WMIQuery)

                        Dim dtErrRow As DataRow = dterr.NewRow
                        dtErrRow("Index") = objEvent.EventIdentifier
                        Select Case objEvent.EventType.ToString
                            Case "1"
                                dtErrRow("EntryType") = "Error"
                                dtErrRow("Img") = "images/error.png"
                            Case "2"
                                dtErrRow("EntryType") = "Warning"
                                dtErrRow("Img") = "images/warning.png"
                            Case "3"
                                dtErrRow("EntryType") = "Information"
                                dtErrRow("Img") = "images/info.png"
                            Case "4"
                                dtErrRow("EntryType") = "SucessAudit"
                                dtErrRow("Img") = "images/info.png"
                            Case "5"
                                dtErrRow("EntryType") = "FailureAudit"
                                dtErrRow("Img") = "images/error.png"
                        End Select
                        dtErrRow("TimeGenerated") = DateTime.ParseExact( _
                            objEvent.TimeGenerated.ToString.Substring(0, 14), _
                            "yyyyMMddHHmmss", _
                            CultureInfo.InvariantCulture _
                        ).ToString("dd/MM/yy HH:mm")
                        dtErrRow("Message") = objEvent.Message
                        dterr.Rows.Add(dtErrRow)

                    Next

                End With

            Catch ex As Exception

            End Try

            Return dterr
        End Get
    End Property

    Private ReadOnly Property ObjWMIEvt(Optional ByVal strComputer As String = ".") As Object
        Get
            Return GetObject("winmgmts:" & "{impersonationLevel=impersonate}!\\" & strComputer & "\root\cimv2")
        End Get
    End Property

    Private ReadOnly Property LogFile() As String
        Get
            Return "Application"
        End Get
    End Property

    Private ReadOnly Property SourceName() As String
        Get
            Return "PRIPROC4"
        End Get
    End Property

    Private Property LastItemDate() As String
        Get
            Return My.Settings.LastDate
        End Get
        Set(ByVal value As String)
            My.Settings.LastDate = value
        End Set
    End Property

    Private ReadOnly Property EventLevel() As String
        Get
            Return "and (EventType = 1 or Eventtype = 2 or EventType = 5) "
        End Get
    End Property


#End Region

#End Region

#Region "Start Up and Close Down of service"

    Protected Overrides Sub OnStart(ByVal args() As String)
        'first up check to see if the settings file exists, if it doesnt service cant run
        If File.Exists("c:\emerge-it\PriProcWatch\Settings.xml") = False Then
            Diagnostics.EventLog.WriteEntry("Application", "No settings file, process unable to start")
            Me.Stop()
        End If

        GetSettings()
        'let the users know that the settings have loaded
        Diagnostics.EventLog.WriteEntry("Application", "Settings Loaded, starting watcher")
        Timer2.Enabled = True
        Timer2.Start()
        Diagnostics.EventLog.WriteEntry("Application", "Timer Started")
    End Sub

    Protected Overrides Sub OnStop()
        ' Add code here to perform any tear-down necessary to stop your service.
        Diagnostics.EventLog.WriteEntry("Application", "Service is stopping")
        Timer2.Stop()
    End Sub

#End Region

#Region "Main loop - timer tick"

    Private Sub Timer2_Elapsed(ByVal sender As System.Object, ByVal e As System.Timers.ElapsedEventArgs) Handles Timer2.Elapsed
        Try
            CheckQueue()

            For Each entry As DataRow In prilog.Rows
                SendMail(entry.Item("EntryType"), entry.Item("Message"))
            Next

        Catch ex As Exception
            Diagnostics.EventLog.WriteEntry("Application", "Service Crashed - " & ex.ToString)
        End Try
    End Sub

#End Region

#Region "Subordinate Functions"

    Private Sub SendMail(ByVal ErrType As String, ByVal msg As String)
        Dim cansend As Boolean = False


        Dim MailTo As String = ""


        ' Create the Mail Message
        Dim Mail As New MailMessage
        ' Set the address information
        Mail.From = New MailAddress("priproc@emerge-it.co.uk")
        For Each it As String In ss.xmlMailList.Split(";")
            If it <> "" Then
                Mail.To.Add(it)
            End If

        Next
        If ss.xmlError = True Then
            If ErrType = "Error" Then
                cansend = True
            End If
        End If

        If ss.xmlFail = True Then
            If ErrType = "Failure" Then
                cansend = True
            End If
        End If

        If ss.xmlWarn = True Then
            If ErrType = "Warning" Then
                cansend = True
            End If
        End If
        If ErrType = "Queue" Then
            cansend = True
            ErrType = "Queue Blockage"
        End If
        For Each ig As String In ss.xmlIgnoreList.Split(";")
            If ig <> "" Then
                If msg.Contains(ig) Then
                    cansend = False
                End If
            End If
        Next
        ' Set the content of the email
        Mail.Subject = "PRIPROC has thrown an " & ErrType
        Mail.Body = "The PriProc logging service has thrown a(n) " & ErrType & vbCrLf & "The associated Message text is " & vbCrLf & msg
        ' Send the message
        Dim SMTP As New SmtpClient(ss.xmlSMTPServer)
        SMTP.EnableSsl = CBool(ss.xmlSSL)
        SMTP.Credentials = New System.Net.NetworkCredential(ss.xmlSMTPUser, ss.xmlSMTPassword)
        SMTP.Port = 25
        Try
            If cansend = True Then
                SMTP.Send(Mail)
            End If
            Mail.Dispose()
        Catch ex As Exception
            Diagnostics.EventLog.WriteEntry("Application", "Mail didnt send - " & ex.ToString)

            Exit Sub
        End Try
    End Sub

    Public Sub GetSettings()
        Try

            If File.Exists("c:\emerge-it\PriProcWatch\Settings.xml") = False Then
                Diagnostics.EventLog.WriteEntry("Application", "failed to read xml - ")
                Exit Sub
            Else

            End If
            Dim doc = XDocument.Load("c:\emerge-it\PriProcWatch\Settings.xml")


            Diagnostics.EventLog.WriteEntry("Application", "XML Document Loaded")
            Dim i As Integer = 1
            ss.xmlLogItem = doc.<ServSettings>.<LogItem>.Value
            'Diagnostics.EventLog.WriteEntry("Application", "Read Log Item")
            ss.xmlLogRefresh = doc.<ServSettings>.<LogRefresh>.Value
            'Diagnostics.EventLog.WriteEntry("Application", "set Log refresh")
            ss.xmlSMTPServer = doc.<ServSettings>.<SMTPServer>.Value
            'Diagnostics.EventLog.WriteEntry("Application", "Read smtp server")
            ss.xmlSSL = doc.<ServSettings>.<SSL>.Value
            'Diagnostics.EventLog.WriteEntry("Application", "Read smtp ssl")
            ss.xmlSMTPUser = doc.<ServSettings>.<SMTPUname>.Value
            'Diagnostics.EventLog.WriteEntry("Application", "Read smtp username")
            ss.xmlSMTPassword = doc.<ServSettings>.<SMTPPassword>.Value
            'Diagnostics.EventLog.WriteEntry("Application", "Read smtp password")
            ss.xmlSMTPPort = doc.<ServSettings>.<SMTPPort>.Value
            'Diagnostics.EventLog.WriteEntry("Application", "Read smtp port")
            ss.xmlError = doc.<ServSettings>.<MAILErr>.Value
            'Diagnostics.EventLog.WriteEntry("Application", "Read checkbox Error")
            ss.xmlFail = doc.<ServSettings>.<MAILFail>.Value
            'Diagnostics.EventLog.WriteEntry("Application", "Read checkbox Fail")
            ss.xmlWarn = doc.<ServSettings>.<MAILWarn>.Value
            'Diagnostics.EventLog.WriteEntry("Application", "Read checkbox warning")
            ss.xmlMailList = doc.<ServSettings>.<EmailList>.Value
            'Diagnostics.EventLog.WriteEntry("Application", "Read email list")
            ss.xmlIgnoreList = doc.<ServSettings>.<IgnoreList>.Value
            'Diagnostics.EventLog.WriteEntry("Application", "Read ignorelist")
            Timer2.Interval = CInt(ss.xmlLogRefresh) * 60000
            Diagnostics.EventLog.WriteEntry("Application", "Set Timer interval" & CInt(ss.xmlLogRefresh))
        Catch ex As Exception


            Diagnostics.EventLog.WriteEntry("Application", "Error reading xml settings file - " & ex.ToString)
            Me.Stop()
        End Try

    End Sub

    Private Sub CheckQueue()
        'write current counts
        Diagnostics.EventLog.WriteEntry("Application", "Queue count old = " & lastcount)
        NumberOfFiles = System.IO.Directory.GetFiles(Path).Length
        dtchk = Now
        Diagnostics.EventLog.WriteEntry("Application", "Queue count current = " & NumberOfFiles)

        'check the current count against the last count

        If NumberOfFiles >= lastcount And NumberOfFiles <> 0 Then

            Dim folder As DirectoryInfo
            folder = New DirectoryInfo(Path)

            'next we get the oldest file in the folder and compare it to the oldest file we found on the last iteration

            For Each checkfile As FileInfo In folder.GetFiles
                If checkfile.CreationTime > dtchk Then

                Else
                    dtchk = checkfile.CreationTime
                End If
            Next

            If dtchk = LastFileDate Then
                'so if the file count is equal to (or greater than) and the file both have the same creation date/time then the queue may have stopped so we email the user
                SendMail("Queue", "The queue doesnt seem to be moving, please check the priproc service")
            End If
        End If

        'now we reset the variables
        lastcount = NumberOfFiles
        LastFileDate = dtchk
    End Sub


#End Region

End Class
