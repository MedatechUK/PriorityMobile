Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System
Imports System.Collections
Imports System.Text
Imports System.Configuration
Imports System.Xml
Imports System.Diagnostics
Imports Priority
Imports System.Net
Imports System.Net.Sockets
Imports System.Threading

<WebService(Namespace:="http://priwebsvc.ntsa.org.uk/")> _
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Public Class Service
    Inherits System.Web.Services.WebService

    Dim myConnect As System.Net.Sockets.Socket

    Dim _AppName As String = "Priority Web Service"
    Dim _LogName As String = "Application"

    Dim cols1() As Integer
    Dim cols2() As Integer

    Dim WebPath As String = System.AppDomain.CurrentDomain.BaseDirectory.ToString()
    Dim Badmail As String = WebPath & "badmail\"
    Dim SignaturePath As String = WebPath & "signatures\"

    <WebMethod()> _
    Public Function SaveSignature(ByVal SerialData As String) As String

        Dim sd As New Priority.SerialData
        Dim ar As New MyCls.MyArray

        Dim data(,) As String = sd.DeSerialiseData(SerialData)
        Dim filename As String = RandomName(SignaturePath, ".sig")

        If ar.ArrayToFile(filename, data) Then
            Return Replace(filename, SignaturePath, "", , , CompareMethod.Text)
        Else
            Return ""
        End If

    End Function

    <WebMethod()> _
    Public Function LoadData(ByVal SerialData As String) As Boolean

        Dim sd As New Priority.SerialData
        Dim dbcon As New DBConfig
        Dim ar As New MyCls.MyArray

        Dim data(,) As String = sd.DeSerialiseData(SerialData)

        Dim y As Integer
        Dim yB As Integer = UBound(data, 2)
        Dim xB As Integer = UBound(data, 1)

        Dim tsql As String = ""
        Dim ln As Integer = 0
        Dim t1, t2

        cols1 = Nothing
        cols2 = Nothing

        t1 = rt(data, 1, xB)
        t2 = rt(data, 2, xB)

        Dim constr As String = dbcon.ConnectionString("/", "system")
        Using Connection As New SqlConnection(constr)

            Try
                Connection.Open()

            Catch ex As Exception
                Dim bad As String = RandomName(Badmail, ".bad")
                ar.ArrayToFile(bad, data)
                WriteToEventLog("Could not open connection " & _
                                    "[" & constr & "]" & vbCrLf & _
                                    ex.Message & vbCrLf & _
                                    "Saved data in [" & bad & "]" _
                                    , EventLogEntryType.FailureAudit)
                Return True
            End Try

            Dim command As SqlCommand = Connection.CreateCommand()

            Try ' to get the next line number
                command.CommandText = "select MAX(LINE)+1 from " & data(0, 0)
                ln = CInt(command.ExecuteScalar())

            Catch ex As Exception ' errors
                Dim bad As String = RandomName(Badmail, ".bad")
                ar.ArrayToFile(bad, data)
                WriteToEventLog("Could not get next LINE for load table " & _
                                    "[" & data(0, 0) & "]" & vbCrLf & _
                                    ex.Message & vbCrLf & _
                                    "Query was: " & vbCrLf & _
                                    command.CommandText & vbCrLf & _
                                    "Saved data in [" & bad & "]" _
                                    , EventLogEntryType.FailureAudit)
                Return True
            End Try

            For y = 4 To yB

                If Len(data(0, y)) > 0 Then
                    Select Case CInt(data(0, y))
                        Case 1
                            tsql = t1 & "'1', " & ln & ", " & ColData(data, y, cols1)
                        Case 2
                            tsql = t2 & "'2', " & ln & ", " & ColData(data, y, cols2)
                    End Select

                    Try
                        command.CommandText = tsql
                        command.ExecuteNonQuery()
                        ln = ln + 1
                    Catch EX As Exception
                        Dim bad As String = RandomName(Badmail, ".bad")
                        ar.ArrayToFile(bad, data)
                        WriteToEventLog("Could not insert data into table " & _
                                            "[" & data(0, 0) & "]" & vbCrLf & _
                                            "Query was: " & vbCrLf & _
                                            tsql & vbCrLf & _
                                            "Saved data in [" & bad & "]" _
                                            , EventLogEntryType.FailureAudit)
                        Return True
                    End Try
                End If

            Next

        End Using

        Dim bteSend() As Byte
        bteSend = Encoding.ASCII.GetBytes(data(1, 0) & ":" & vbCrLf)
        Dim myConnect As System.Net.Sockets.Socket
        Dim bidEndPoint As IPEndPoint
        bidEndPoint = New IPEndPoint(IPAddress.Parse("127.0.0.1"), 8022)
        myConnect = New Socket _
                     (AddressFamily.InterNetwork, _
                     SocketType.Stream, _
                     ProtocolType.Tcp)
        Try

            myConnect.Connect(bidEndPoint)
            While Not myConnect.Connected
                Thread.Sleep(10)
            End While

            myConnect.Send _
              (bteSend, 0, bteSend.Length, _
              SocketFlags.DontRoute)

            myConnect.Close()

        Catch ex As SocketException
            Dim bad As String = RandomName(Badmail, ".bad")
            ar.ArrayToFile(bad, data)
            WriteToEventLog("Socket Fails connecting to ProcService." & _
                                ex.Message & vbCrLf & _
                                "Saved data in [" & bad & "]" _
                                , EventLogEntryType.FailureAudit)
            Return True

        End Try

        Return True

    End Function

    <WebMethod()> _
    Public Function GetData(ByVal SQL As String) As String

        Dim sd As New Priority.SerialData
        Dim dbcon As New DBConfig

        Dim ret As String = ""

        Try
            Using Connection As New SqlConnection(dbcon.ConnectionString("/", "system"))

                Connection.Open()
                Dim command As SqlCommand = Connection.CreateCommand()

                command.CommandText = SQL
                Dim dataReader As SqlDataReader = _
                             command.ExecuteReader()

                ret = sd.SerialiseDataReader(dataReader)

            End Using
        Catch ex As Exception
            ret = "!" & ex.Message
        End Try

        Return ret

    End Function

    Private Function rt(ByRef data, ByVal y, ByVal xB)

        Dim x
        Dim cols() As Integer

        Dim thisrt As String = "INSERT INTO " & data(0, 0) & " (RECORDTYPE, LINE, "
        For x = 1 To xB
            If Len(data(x, y)) > 0 Then
                Try
                    ReDim Preserve cols(UBound(cols) + 1)
                Catch
                    ReDim cols(0)
                End Try
                cols(UBound(cols)) = x
                thisrt = thisrt & data(x, y) & ", "
            End If
        Next
        thisrt = Left(thisrt, Len(thisrt) - 2) & ") VALUES ("

        Select Case CInt(data(0, y))
            Case 1
                cols1 = cols
            Case 2
                cols2 = cols
        End Select

        Return thisrt

    End Function

    Private Function ColData(ByRef data, ByVal y, ByVal cols)

        Dim x
        Dim ret As String = ""

        For x = 0 To UBound(cols)
            Select Case LCase(data(cols(x), 3))
                Case "text"
                    ret = ret & "'" & Replace(data(cols(x), y), "'", "' + char(39) + '") & "'"
                Case "int"
                    ret = ret & "dbo.INTQUANT(" & data(cols(x), y) & ")"
                Case "real"
                    ret = ret & "dbo.REALQUANT(" & data(cols(x), y) & ")"
                Case "time"
                    If data(cols(x), y) = "" Then
                        ret = ret & CStr(DateDiff(DateInterval.Minute, CDate("00:00"), CDate("00:00")))
                    Else
                        ret = ret & CStr(DateDiff(DateInterval.Minute, CDate("00:00"), CDate(data(cols(x), y))))
                    End If
                Case Else
                    ret = ret & data(cols(x), y)
            End Select
            ret = ret & ", "
        Next
        ret = Left(ret, Len(ret) - 2) & ")"

        Return ret

    End Function

    Private Function RandomName(ByVal Path As String, ByVal Extension As String) As String

        Dim ret As String
        Dim randObj As New Random()

        If Not (Right(Path, 1)) = "\" Then Path = Path & "\"
        If Not (Left(Extension, 1)) = "." Then Extension = "." & Extension

        Path = UCase(Path)
        Extension = UCase(Extension)

        Do
            ret = ""
            For i As Integer = 0 To 7
                ret = ret & Chr(randObj.Next(65, 90))
            Next
        Loop While File.Exists(Path & ret & Extension)

        Return Path & ret & Extension

    End Function

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
