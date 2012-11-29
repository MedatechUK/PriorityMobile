Imports System.IO
Imports System.Data
Imports System.threading

Public Structure DataLoad

#Region "Shared Variables"

    Shared qLock As New Queue
    Shared m_consts As New Dictionary(Of String, String)
    Shared m_ev As ntEvtLog.evt = Nothing

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

    Private Function ParseConstName(ByVal Name As String)
        Dim r As String = Name
        If Not (Left(r, 1) = "%") Then r = "%" & r
        If Not (Right(r, 1) = "%") Then r = r & "%"
        Return Replace(UCase(r), Chr(32), "")
    End Function

    Public Property Constants(ByVal Parameter As String) As String
        Get
            Dim p As String = ParseConstName(Parameter)
            Select Case p
                Case "%DATE%"
                    Return DateDiff(DateInterval.Minute, #1/1/1988#, Now).ToString()
                Case "%DATE8%"
                    Return (DateDiff(DateInterval.Minute, #1/1/1988#, Now) - DateDiff(DateInterval.Minute, New Date(Year(Now), Month(Now), Day(Now)), Now)).ToString
                Case "%TIME%"
                    Return DateDiff(DateInterval.Minute, New Date(Year(Now), Month(Now), Day(Now)), Now).ToString
                Case Else
                    If m_consts.ContainsKey(p) Then
                        Return m_consts(p)
                    Else
                        Return Nothing
                    End If
            End Select
        End Get
        Set(ByVal value As String)
            Dim p As String = ParseConstName(Parameter)
            If m_consts.ContainsKey(p) Then
                m_consts(p) = value
            Else
                m_consts.Add(p, value)
            End If
        End Set
    End Property


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

    Public ReadOnly Property Data() As String(,)

        Get

            ' ************************************
            ' Declares the contents of the header
            ' Do not edit

            If IsNothing(Me._Records) Then Return Nothing

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
            End Try

            For i As Integer = 0 To UBound(newline)
                Dim part() As String = Split(newline(i), "%")
                Dim c As Integer = UBound(part) + 1
                If c Mod 2 = 1 And c > 2 Then
                    Dim bstr As String = ""
                    For y As Integer = 0 To c - 1
                        Select Case y Mod 2
                            Case 1
                                bstr += Constants(ParseConstName(part(y)))
                            Case Else
                                bstr += Trim(part(y))
                        End Select
                    Next
                    _Records(i, UBound(_Records, 2)) = bstr
                Else
                    _Records(i, UBound(_Records, 2)) = Trim(newline(i))
                End If
                ' LoadingConstants.Parse(newline(i))
            Next

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

    Public Sub SetEventLog(ByRef Ev As ntEvtLog.evt)
        m_ev = Ev
    End Sub

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

    Private Sub SafeLog(ByVal Entry As String, ByVal EventType As ntEvtLog.LogEntryType, ByVal Verbosity As ntEvtLog.EvtLogVerbosity)
        Try
            If Not (IsNothing(m_ev)) Then
                m_ev.Log( _
                    Entry, _
                    EventType, _
                    Verbosity _
                  )
            End If
        Catch exep As Exception
            Console.WriteLine( _
                "Failed to write to the [{0} {1}] log.{4}" & _
                "The error reported was: {4}{2}{4}" & _
                "The event could not be written to the log because: {4}{3}{4}", _
                m_ev.LogName, m_ev.AppName, Entry, exep.Message, vbCrLf _
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

#End Region

End Structure
