Imports System.IO
Imports System.Data
Imports System.Data.SqlClient
Imports System.threading
Imports ntEvtlog

Public Structure Loading

#Region "Shared Variables"

    Shared LoadingConstants As New LoadConst
    Shared _ev As ntEvtlog.evt = Nothing
    Shared qLock As New Queue

#End Region

#Region "Private Variables"

    Private m_DebugFlag As Boolean
    Private m_Table As String
    Private m_Environment As String
    Private m_Procedure As String
    Private m_rec1 As String()
    Private m_rec2 As String()
    Private m_typ As String()

    Private _Records As String(,)

#End Region

#Region "Public Properties"

    Public WriteOnly Property DebugFlag() As Boolean
        Set(ByVal value As Boolean)
            m_DebugFlag = value
        End Set
    End Property

    Public Property Table() As String
        Set(ByVal value As String)
            m_Table = value
        End Set
        Get
            Return m_Table
        End Get
    End Property

    Public Property Procedure() As String
        Set(ByVal value As String)
            m_Procedure = value
        End Set
        Get
            Return m_Procedure
        End Get
    End Property

    Public Property Environment() As String
        Set(ByVal value As String)
            m_Environment = value
        End Set
        Get
            If IsNothing(m_Environment) Then
                Return ""
            ElseIf m_Environment.Length = 0 Then
                Return ""
            Else
                Return m_Environment
            End If
        End Get
    End Property

    Public Property RecordType1() As String
        Get
            Return ar2str(m_rec1)
        End Get
        Set(ByVal value As String)
            m_rec1 = str2ar(value)
        End Set
    End Property

    Public Property RecordType2() As String
        Get
            Return ar2str(m_rec2)
        End Get
        Set(ByVal value As String)
            m_rec2 = str2ar(value)
        End Set
    End Property

    Public WriteOnly Property RecordTypes() As String
        Set(ByVal value As String)
            m_typ = str2ar(value)
        End Set
    End Property

#End Region

#Region "Dataset functions"

    Public Sub SetEventLog(ByRef Ev As ntEvtlog.evt)
        _ev = Ev
    End Sub

    Public ReadOnly Property Data() As String(,)

        Get

            ' ************************************
            ' Declares the contents of the header
            ' Do not edit

            Dim xu = UBound(m_typ) + 1
            Dim yu = 4
            yu = yu + UBound(Me._Records, 2)
            'Try
            '    yu = yu + (UBound(Me.m_t2Records) + 1)
            'Catch
            '    yu = yu + 0
            'End Try

            Dim ar(UBound(_Records, 1), UBound(_Records, 2) + 4) As String

            ar(0, 0) = Me.m_Table
            ar(1, 0) = Me.m_Procedure
            ar(2, 0) = Me.Environment

            For x As Integer = 0 To UBound(m_rec1)
                ar(x + 1, 1) = m_rec1(x)
            Next

            Try
                For x As Integer = 0 To UBound(m_rec2)
                    ar(x + 1 + (UBound(m_rec1) + 1), 2) = m_rec2(x)
                Next
            Catch
            End Try

            For x As Integer = 0 To UBound(m_typ)
                ar(x + 1, 3) = m_typ(x)
            Next
            ar(0, 1) = "1"
            ar(0, 2) = "2"
            ' *********************************************

            ' *******************************************************************
            ' *** Build the load data to be sent

            ' Type 1 records
            Dim yord As Integer = 4

            For y As Integer = 0 To UBound(_Records, 2)
                For x As Integer = 0 To UBound(_Records, 1)
                    ar(x, yord) = _Records(x, y)
                Next
                yord = yord + 1
            Next
            Return ar

        End Get

    End Property

    Public WriteOnly Property AddRecord(ByVal type As Integer) As String()
        Set(ByVal value As String())

            Select Case type
                Case 1
                    If Not UBound(value) = UBound(m_rec1) Then
                        Throw New Exception("Record Type 1 data does not match definition." & vbCrLf & _
                            String.Format("Definition: [{0}]", ar2str(m_rec1)) & vbCrLf & _
                            String.Format("Data: [{0}]", ar2str(value)) _
                        )
                    End If
                Case 2
                    If Not UBound(value) = UBound(m_rec2) Then
                        Throw New Exception("Record Type 2 data does not match definition." & vbCrLf & _
                            String.Format("Definition: [{0}]", ar2str(m_rec2)) & vbCrLf & _
                            String.Format("Data: [{0}]", ar2str(value)) _
                        )
                    End If

                Case Else
                    Throw New Exception("Only Record Types 1 and 2 are supported.")
            End Select

            Dim newline(UBound(m_rec1) + 1 + UBound(m_rec2) + 1) As String
            newline(0) = CStr(type)

            Dim f As Integer
            Select Case type
                Case 1
                    f = 1
                Case 2
                    f = UBound(m_rec1) + 2
                Case Else
                    Throw New Exception("Only Record Types 1 and 2 are supported.")
            End Select

            For i As Integer = 0 To UBound(value)
                newline(f) = value(i)
                f = f + 1
            Next

            Try
                ReDim Preserve _Records(UBound(_Records, 1), UBound(_Records, 2) + 1)
            Catch ex As Exception
                ReDim _Records(UBound(m_rec1) + 1 + UBound(m_rec2) + 1, 0)
            Finally
                For i As Integer = 0 To UBound(newline)
                    _Records(i, UBound(_Records, 2)) = LoadingConstants.Parse(newline(i))
                Next
            End Try
        End Set

    End Property

    Public Function Validate() As Boolean

        If Me.m_Table = "" Then Return False
        If Me.m_Procedure = "" Then Return False
        Return True

    End Function

    Public Sub Clear()

        m_DebugFlag = False
        m_Table = ""
        m_Procedure = ""
        m_rec1 = Nothing
        m_rec2 = Nothing
        m_typ = Nothing

        _Records = Nothing

    End Sub

    Public Function Load(ByVal ConnectionStr As String, ByVal BadMailPath As String, Optional ByVal BubbleID As String = Nothing) As Boolean

        'Monitor.Enter(qLock)

        Dim ret As Boolean = True
        Dim Connection As SqlConnection = Nothing
        Dim BadMail As String = Nothing
        Dim command As SqlCommand = Nothing
        Dim ln As Integer = 0
        Dim lastLn As Integer = Nothing
        Dim rollback As Boolean = False
        Dim myData As String(,) = Data

        Me.SafeLog( _
            String.Format("Inserting data into [{3}]{0}Using connection String: [{1}]{0}Bad mail path set to [{2}]", _
                vbCrLf, _
                ConnectionStr, _
                BadMailPath, _
                Table _
            ), _
             EventLogEntryType.Information, _
             ntEvtlog.EvtLogVerbosity.Verbose _
        )

        Try
            If Directory.Exists(BadMailPath) Then
                BadMail = Replace(BadMailPath & "\" & System.Guid.NewGuid.ToString & ".bad", "\\", "\")
            Else
                Me.SafeLog( _
                    String.Format( _
                        "The Bad mail folder specified is invalid:{0}[{1}]{0}Continuing, but failed bubbles will not be saved.", _
                        vbCrLf, _
                        BadMail _
                    ), _
                     EventLogEntryType.FailureAudit, _
                     ntEvtlog.EvtLogVerbosity.Normal _
                )
            End If

            Try
                Connection = New SqlConnection(ConnectionStr)
                Connection.Open()
                command = Connection.CreateCommand()

            Catch ex As Exception
                Throw New Exception( _
                    String.Format( _
                        "Failed to open connection string {1}{0}The database error was:{0}{2}", _
                        vbCrLf, _
                        ConnectionStr, _
                        ex.Message _
                    ) _
                )
            End Try

            Try ' to get the next line number
                command.CommandText = "select MAX(LINE)+1 from " & Table
                ln = CInt(command.ExecuteScalar())
                lastLn = ln - 1
            Catch ex As Exception ' errors
                Throw New Exception(String.Format( _
                    "Could not get next LINE for load table [{1}]{0}" & _
                    "The database sql query was [{2}]{0}" & _
                    "The database connection string was [{3}]{0}" & _
                    "The database returned the error: [{4}]", _
                    vbCrLf, Me.Table, command.CommandText, Connection.ConnectionString, ex.Message))
            End Try

            Dim AddBubbleID As Boolean = False
            If IsNothing(BubbleID) Then
                Dim be As New Priority.LoadCallBack(_ev)
                Try
                    With be
                        .Table = Me.Table
                        .Connection = Connection
                        'is there a bubbleid in the table
                        AddBubbleID = .IsBubbleTable
                    End With
                Catch ex As Exception
                    AddBubbleID = False
                Finally
                    be = Nothing
                End Try
            End If

            For y As Integer = 4 To UBound(myData, 2)
                If Len(myData(0, y)) > 0 Then
                    Try
                        If AddBubbleID Then
                            If myData(0, y) = "1" Then
                                BubbleID = System.Guid.NewGuid.ToString
                            End If
                        End If
                        command.CommandText = rt(myData, y, ln, BubbleID) & ColData(myData, y)
                        Me.SafeLog( _
                            String.Format("Running sql: [{1}]", _
                                vbCrLf, _
                                command.CommandText _
                            ), _
                             EventLogEntryType.Information, _
                             ntEvtlog.EvtLogVerbosity.Verbose _
                        )
                        command.ExecuteNonQuery()
                        ln += 1
                        rollback = True

                    Catch ex As Exception
                        Throw New Exception(String.Format( _
                            "Could not insert into load table [{1}]{0}" & _
                            "The database sql query was [{2}]{0}" & _
                            "The database connection string was [{3}]{0}" & _
                            "The database returned the error: [{4}]", _
                            vbCrLf, Me.Table, command.CommandText, Connection.ConnectionString, ex.Message))
                    End Try
                End If
            Next

        Catch ex As Exception
            ret = False
            Try
                If Not IsNothing(BadMail) Then
                    If Me.ToFile(BadMail) Then
                        Throw New Exception(String.Format( _
                            "{1}{0}Bad mail file written to [{2}].", _
                            vbCrLf, ex.Message, BadMail))
                    Else
                        Throw New Exception(String.Format( _
                            "{1}{0}Bad mail file [{2}] could not be written.", _
                            vbCrLf, ex.Message, BadMail))
                    End If
                Else
                    Throw New Exception(String.Format( _
                        "{1}{0}Bad mail folder [{2}] does not exist.", _
                        vbCrLf, ex.Message, BadMailPath))
                End If

            Catch innerex As Exception
                Me.SafeLog( _
                    String.Format("{1}", _
                        vbCrLf, _
                        innerex.Message _
                    ), _
                     EventLogEntryType.Error, _
                     ntEvtlog.EvtLogVerbosity.Normal _
                )
            Finally
                If rollback Then
                    command.CommandText = _
                        String.Format("delete from {0} where LINE > {1}", _
                            Table, lastLn)
                    Me.SafeLog( _
                        String.Format("Rolling back transactions:{0}[{1}]", _
                            vbCrLf, _
                            command.CommandText _
                        ), _
                         EventLogEntryType.Information, _
                         ntEvtlog.EvtLogVerbosity.Verbose _
                    )
                    command.ExecuteNonQuery()
                End If
                If Not IsNothing(Connection) Then Connection.Close()
                'Monitor.Exit(qLock)
            End Try
        End Try

        Return ret

    End Function

#End Region

#Region "Data format conversion routines"

#Region "To / From File"

    Public Function ToFile(ByVal filename As String) As Boolean

        Dim y, n As Integer
        Dim er As Boolean = True
        Dim ThisArray(,) = Data

        Try
            ' Is there data in the array?
            y = UBound(ThisArray, 2)
            If y = -1 Then Err.Raise(1)
        Catch
            ' No, so delete the file
            If File.Exists(filename) Then
                File.Delete(filename)
            End If
            ' And quit
            Return True
            Exit Function
        End Try

        ' Data in array, so write it
        Try
            Using sw As StreamWriter = New StreamWriter(filename)
                ' Add some text to the file.
                For y = 0 To ThisArray.GetUpperBound(1)
                    For n = 0 To ThisArray.GetUpperBound(0)
                        sw.Write(ThisArray(n, y))
                        If n < ThisArray.GetUpperBound(0) Then sw.Write(Chr(9))
                    Next
                    If y < ThisArray.GetUpperBound(1) Then sw.Write(vbCrLf)
                Next
                sw.Close()
            End Using
        Catch E As Exception
            er = False
        End Try

        Return er

    End Function

    Public Function FromFile(ByVal filename As String) As Boolean

        Dim l As Integer = 0
        If Not File.Exists(filename) Then
            Return False
        End If

        Clear()
        Try
            Using sr As StreamReader = New StreamReader(CStr(filename))
                While Not sr.EndOfStream
                    Dim line() As String = Split(sr.ReadLine(), Chr(9))
                    With Me
                        Select Case l
                            Case 0
                                .Table = line(0)
                                .Procedure = line(1)
                                .Environment = line(2)

                            Case 1, 2, 3
                                For li As Integer = 1 To UBound(line)
                                    Select Case l
                                        Case 1
                                            If line(li).Length > 0 Then redimVar(.m_rec1, line(li))
                                        Case 2
                                            If line(li).Length > 0 Then redimVar(.m_rec2, line(li))
                                        Case 3
                                            redimVar(.m_typ, line(li))
                                    End Select
                                Next

                            Case Else


                                Try
                                    ReDim Preserve _Records(UBound(line), UBound(_Records, 2) + 1)
                                Catch ex As Exception
                                    ReDim _Records(UBound(line), 0)
                                Finally
                                    For i As Integer = 0 To UBound(line)
                                        _Records(i, UBound(_Records, 2)) = line(i)
                                    Next
                                End Try

                        End Select

                    End With
                    l += 1
                End While
            End Using
            Return True

        Catch ex As Exception
            Return False
        End Try

    End Function

#End Region

#Region "To / From Serial"

    Public Function ToSerial() As String

        Dim ret As String = ""
        If IsNothing(Data) Then Return ret

        For y As Integer = 0 To UBound(Data, 2)
            If Len(ret) > 0 Then
                ret = ret & "\n"
            End If
            For x As Integer = 0 To UBound(Data, 1)
                ret = ret & Data(x, y)
                If x < UBound(Data, 1) Then
                    ret = ret & "\t"
                End If
            Next
        Next

        Return ret

    End Function

    Public Function FromSerial(ByVal Data As String) As Boolean

        Dim thisLine As Integer = 0
        If (Trim(Data).Length) = 0 Then Return False

        Clear()
        Try
            For Each l As String In Split(Data, "\n")
                Dim line() As String = Split(l, "\t")
                With Me
                    Select Case thisLine
                        Case 0
                            .Table = line(0)
                            .Procedure = line(1)
                            .Environment = line(2)

                        Case 1, 2, 3
                            For li As Integer = 1 To UBound(line)
                                Select Case thisLine
                                    Case 1
                                        If line(li).Length > 0 Then redimVar(.m_rec1, line(li))
                                    Case 2
                                        If line(li).Length > 0 Then redimVar(.m_rec2, line(li))
                                    Case 3
                                        redimVar(.m_typ, line(li))
                                End Select
                            Next

                        Case Else

                            Try
                                ReDim Preserve _Records(UBound(line), UBound(_Records, 2) + 1)
                            Catch ex As Exception
                                ReDim _Records(UBound(line), 0)
                            Finally
                                For i As Integer = 0 To UBound(line)
                                    _Records(i, UBound(_Records, 2)) = line(i)
                                Next
                            End Try

                    End Select

                End With
                thisLine += 1
            Next
            Return True

        Catch ex As Exception
            Return False
        End Try

    End Function

#End Region

#End Region

#Region "Private functions"

    Private Sub SafeLog(ByVal Entry As String, ByVal EventType As LogEntryType, ByVal Verbosity As ntEvtlog.EvtLogVerbosity)
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

    Private Function str2ar(ByVal str As String) As String()
        Dim tmp As String() = Split(str, ",", , CompareMethod.Text)
        For i As Integer = 0 To UBound(tmp)
            tmp(i) = Trim(tmp(i))
        Next
        Return tmp
    End Function

    Private Function ar2str(ByVal str() As String) As String
        Dim tmp As String = ""
        For i As Integer = 0 To UBound(str)
            tmp += Trim(str(i))
            If i < UBound(str) Then
                tmp += ","
            End If
        Next
        Return tmp
    End Function

    Private Sub redimVar(ByRef Var() As String, ByVal newVal As String)
        Try
            ReDim Preserve Var(UBound(Var) + 1)
        Catch
            ReDim Var(0)
        Finally
            Var(UBound(Var)) = newVal
        End Try
    End Sub

    Private Function rt(ByRef myData(,) As String, ByVal y As Integer, ByVal Ln As String, ByVal BubbleID As String) As String
        Dim thisrt As String = "INSERT INTO " & Table & " (RECORDTYPE, LINE, "
        If Not IsNothing(BubbleID) Then
            thisrt += "BUBBLEID, "
        End If
        Select Case CInt(myData(0, y))
            Case 1
                For x As Integer = 0 To UBound(Me.m_rec1)
                    thisrt += m_rec1(x)
                    If x < UBound(Me.m_rec1) Then
                        thisrt += ", "
                    End If
                Next
            Case 2
                For x As Integer = 0 To UBound(Me.m_rec2)
                    thisrt += m_rec2(x)
                    If x < UBound(Me.m_rec2) Then
                        thisrt += ", "
                    End If
                Next
        End Select
        thisrt += ") VALUES (" & "'" & CStr(myData(0, y)) & "', " & Ln & ", "
        If Not IsNothing(BubbleID) Then
            thisrt += String.Format("'{0}', ", BubbleID)
        End If
        Return thisrt
    End Function

    Private Function ColData(ByRef myData(,) As String, ByVal y As Integer) As String
        Dim ret As String = ""
        Dim os As Integer
        Select Case CInt(myData(0, y))
            Case 1
                os = 1
                For x As Integer = 0 To UBound(m_rec1)
                    ret += FormatStr(myData(x + os, y), myData(x + os, 3))
                    If x < UBound(m_rec1) Then ret += ", "
                Next
            Case 2
                os = 1 + UBound(m_rec1) + 1
                For x As Integer = 0 To UBound(m_rec2)
                    ret += FormatStr(myData(x + os, y), myData(x + os, 3))
                    If x < UBound(m_rec2) Then ret += ", "
                Next
        End Select
        Return ret & ")"
    End Function

    Private Function FormatStr(ByVal Str As String, ByVal StrFormat As String) As String
        Select Case LCase(StrFormat)
            Case "text", "char"
                Return "'" & Replace(Str, "'", "' + char(39) + '") & "'"
            Case "int"
                Return "dbo.INTQUANT(" & Str & ")"
            Case "real"
                Return "dbo.REALQUANT(" & Str & ")"
            Case "time"
                If Str = "" Then
                    Return CStr(DateDiff(DateInterval.Minute, CDate("00:00"), CDate("00:00")))
                Else
                    Return CStr(DateDiff(DateInterval.Minute, CDate("00:00"), CDate(Str)))
                End If
            Case Else
                Return Str
        End Select
    End Function

#End Region

End Structure
