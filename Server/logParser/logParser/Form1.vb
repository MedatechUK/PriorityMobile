Imports System.IO
Imports System.Text.RegularExpressions

Public Class Form1

    Private logEvents As New Dictionary(Of Date, LogHour)

    Private Sub Form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim di As New DirectoryInfo("c:\weblog")
        For Each f As FileInfo In di.GetFiles
            Using sw As New StreamReader(f.FullName)
                Dim line As String
                While Not sw.EndOfStream
                    line = sw.ReadLine
                    If Not Microsoft.VisualBasic.Strings.Left(line, 1) = "#" Then
                        Dim ev As New LogEvent
                        With ev
                            .edate = Split(line, Chr(32))(0)
                            .time = Split(line, Chr(32))(1)
                            .sip = Split(line, Chr(32))(2)
                            .csmethod = Split(line, Chr(32))(3)
                            .csuristem = Split(line, Chr(32))(4)
                            .csuriquery = Split(line, Chr(32))(5)
                            .sport = Split(line, Chr(32))(6)
                            .csusername = Split(line, Chr(32))(7)
                            .cip = Split(line, Chr(32))(8)
                            .csUserAgent = Split(line, Chr(32))(9)
                            .scstatus = Split(line, Chr(32))(10)
                            .scsubstatus = Split(line, Chr(32))(11)
                            .scwin32status = Split(line, Chr(32))(12)
                            .timetaken = Split(line, Chr(32))(13)
                        End With
                        If Not (logEvents.Keys.Contains(ev.edate)) Then
                            logEvents.Add(ev.edate, New LogHour)
                        End If
                        If Not logEvents(ev.edate).Events.Keys.Contains(Hour(ev.time)) Then
                            logEvents(ev.edate).Events.Add(Hour(ev.time), New List(Of LogEvent))
                        End If
                        logEvents(ev.edate).Events(Hour(ev.time)).Add(ev)
                    End If
                End While
            End Using
        Next

        Dim firstdate As Date = logEvents.Keys.Min
        Dim lastdate As Date = logEvents.Keys.Max
        Dim thisdate As Date = firstdate

        Using sw As New StreamWriter("c:\weblog\Visitors.csv")

            sw.Write(Chr(9))
            For h As Integer = 0 To 23
                sw.Write( _
                    String.Format( _
                        "{0}:00{1}", _
                          Microsoft.VisualBasic.Strings.Right("00" & CStr(h), 2), _
                          Chr(9) _
                    ) _
                )
            Next
            sw.Write(vbCrLf)


            While thisdate <= lastdate

                sw.Write( _
                    String.Format( _
                        "{0} {1}/{2}/{3}", _
                        thisdate.DayOfWeek, _
                        thisdate.Day, _
                        thisdate.Month, _
                        thisdate.Year, _
                        Chr(9) _
                    ) _
                )

                For h As Integer = 0 To 23
                    sw.Write( _
                        String.Format( _
                            "{0}{1}", _
                            Chr(9), _
                            Visitors(thisdate, h) _
                        ) _
                    )
                Next
                sw.Write(vbCrLf)

                thisdate = DateAdd(DateInterval.Day, 1, thisdate)

            End While
        End Using

    End Sub

    Private Function Hits(ByVal thisDate As Date, ByVal thisHour As Integer) As Integer
        If logEvents.Keys.Contains(thisDate) Then
            If logEvents(thisDate).Events.Keys.Contains(thisHour) Then
                Return logEvents(thisDate).Events(thisHour).Count
            Else
                Return 0
            End If
        Else
            Return 0
        End If
    End Function

    Private Function Visitors(ByVal thisDate As Date, ByVal thisHour As Integer) As Integer
        Dim ip As New List(Of String)
        If logEvents.Keys.Contains(thisDate) Then
            If logEvents(thisDate).Events.Keys.Contains(thisHour) Then
                For Each e As LogEvent In logEvents(thisDate).Events(thisHour)
                    If Not ip.Contains(e.cip) Then
                        ip.Add(e.cip)
                    End If
                Next
                Return ip.Count
            Else
                Return 0
            End If
        Else
            Return 0
        End If
    End Function

    Private Function PageVisits(ByVal thisDate As Date, ByVal thisHour As Integer) As Integer
        Dim c As Integer = 0
        Dim guidRegEx As New Regex("^(\{{0,1}([0-9a-fA-F]){8}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){12}\}{0,1})$")        
        If logEvents.Keys.Contains(thisDate) Then
            If logEvents(thisDate).Events.Keys.Contains(thisHour) Then
                For Each e As LogEvent In logEvents(thisDate).Events(thisHour)
                    If e.csuristem = "/" Or guidRegEx.IsMatch(e.csuristem) Then
                        c += 1
                    End If
                Next
                Return c
            Else
                Return 0
            End If
        Else
            Return 0
        End If
    End Function

    Private Function PageCount(ByVal thisDate As Date) As Dictionary(Of String, Integer)
        Dim PDFCount As New Dictionary(Of String, Integer)
        If logEvents.Keys.Contains(thisDate) Then
            For Each h As Integer In logEvents(thisDate).Events.Keys
                For Each e As LogEvent In logEvents(thisDate).Events(h)
                    If Microsoft.VisualBasic.Strings.Right(e.csuristem, 4).ToUpper = ".PDF" Then
                        If Not PDFCount.Keys.Contains(e.csuristem) Then
                            PDFCount.Add(e.csuristem, 1)
                        Else
                            PDFCount(e.csuristem) += 1
                        End If
                    End If
                Next
            Next
        End If        
        Return PDFCount
        'Dim pages As Dictionary(Of String, Integer) = PageCount(thisdate)
        'For Each p As String In pages.Keys
        '    Debug.Print( _
        '        String.Format( _
        '            "{0} {1}", _
        '            p, _
        '            pages(p) _
        '        ) _
        '    )
        'Next
    End Function
End Class
