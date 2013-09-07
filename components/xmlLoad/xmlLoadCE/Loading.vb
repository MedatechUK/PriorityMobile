Imports System.Xml
Imports System.IO

Public Enum eProviderType
    MSSQL = 1
    ORACLE = 2
End Enum

Public Enum tColumnType
    typeCHAR = 1
    typeDATE = 2
    typeREAL = 3
    typeINT = 4
    typeBOOL = 5
    typeTIME = 6
End Enum

Public Class LoadColumn

    Public Sub New(ByVal ColumnName As String, ByVal ColumnType As tColumnType, ByVal Length As Integer)
        _ColumnName = ColumnName
        _ColumnType = ColumnType
        _Length = Length
    End Sub

    Public Sub New(ByVal ColumnName As String, ByVal ColumnType As tColumnType)
        _ColumnName = ColumnName
        _ColumnType = ColumnType
        _Length = -1
    End Sub

    Private _ColumnName As String
    Public Property ColumnName() As String
        Get
            Return _ColumnName
        End Get
        Set(ByVal value As String)
            _ColumnName = value
        End Set
    End Property

    Private _ColumnType As tColumnType
    Public Property ColumnType() As tColumnType
        Get
            Return _ColumnType
        End Get
        Set(ByVal value As tColumnType)
            _ColumnType = value
        End Set
    End Property

    Private _Length As Integer
    Public Property Length() As Integer
        Get
            Return _Length
        End Get
        Set(ByVal value As Integer)
            _Length = value
        End Set
    End Property

End Class

Public Class LoadRow

    Private _Data() As String = Nothing

    Sub New(ByVal ParamArray args As String())
        For Each Str As String In args
            Try
                ReDim Preserve _Data(UBound(_Data) + 1)
            Catch
                ReDim _Data(0)
            Finally
                _Data(UBound(_Data)) = Str
            End Try
        Next
    End Sub

    Public Property Data() As String()
        Get
            Return _Data
        End Get
        Set(ByVal value As String())
            _Data = value
        End Set
    End Property

    Public ReadOnly Property Length()
        Get
            Return UBound(_Data)
        End Get
    End Property

    Public ReadOnly Property Count() As Integer
        Get
            Return UBound(_Data) + 1
        End Get
    End Property

    Private _RecordType As Integer = 1
    Public Property RecordType() As Integer
        Get
            Return _RecordType
        End Get
        Set(ByVal value As Integer)
            _RecordType = value
        End Set
    End Property

End Class

Public Class Loading
    Implements IDisposable

#Region "Initialisation and Finalisation"

    Public Sub New()
        _DefaultEnv = Nothing
    End Sub

    Public Sub New(ByVal DefaultEnv As String, _
        Optional ByVal Provider As eProviderType = eProviderType.MSSQL)
        _Provider = Provider
        _DefaultEnv = DefaultEnv
    End Sub

    Public Sub Dispose() Implements System.IDisposable.Dispose
        LoadingColumns = Nothing
        LoadingRows = Nothing
        _XMLDoc = Nothing
    End Sub

#End Region

#Region "Private Variables"

    Private _DebugFlag As Boolean = False
    Private _Table As String = Nothing
    Private _Environment As String = Nothing
    Private _Procedure As String = Nothing
    Private _XMLDoc As New XmlDocument
    Private _BubbleID As String
    Private _DefaultEnv As String = Nothing
    Private _BubbleSQL As String = Nothing
    Private _Provider As eProviderType = eProviderType.MSSQL
    Private BeginData As Boolean = False
    Private LoadingColumns As New Dictionary(Of Integer, List(Of LoadColumn))
    Private LoadingRows As New List(Of LoadRow)

#End Region

#Region "Public Properties"


    Public ReadOnly Property HasSQL() As Boolean
        Get
            Return Not (IsNothing(Me.Table))
        End Get
    End Property

    Public Property DebugFlag() As Boolean
        Get
            Return _DebugFlag
        End Get
        Set(ByVal value As Boolean)
            _DebugFlag = value
        End Set
    End Property

    Public Property Table() As String
        Set(ByVal value As String)
            _Table = value
        End Set
        Get
            Return _Table
        End Get
    End Property

    Public Property Procedure() As String
        Set(ByVal value As String)
            _Procedure = value
        End Set
        Get
            Return _Procedure
        End Get
    End Property

    Public Property Environment() As String
        Set(ByVal value As String)
            _Environment = value
        End Set
        Get
            If IsNothing(_Environment) Then
                Return ""
            ElseIf _Environment.Length = 0 Then
                Return ""
            Else
                Return _Environment
            End If
        End Get
    End Property

    Public Property BubbleSQL() As String
        Get
            Return _BubbleSQL
        End Get
        Set(ByVal value As String)
            _BubbleSQL = value
        End Set
    End Property

    Public ReadOnly Property BubbleID() As String
        Get
            Return _BubbleID
        End Get
    End Property

#End Region

#Region "Write Only Properties"

    Public WriteOnly Property AddColumn(ByVal RecordType As Integer) As LoadColumn
        Set(ByVal value As LoadColumn)
            If Not BeginData Then
                If Not LoadingColumns.Keys.Contains(RecordType) Then
                    LoadingColumns.Add(RecordType, New List(Of LoadColumn))
                End If
                LoadingColumns(RecordType).Add(value)
            Else
                Throw New Exception("Cannot add more columns after AddRecordType(x) directive.")
            End If
        End Set
    End Property

    Public WriteOnly Property AddRecordType(ByVal RecordType As Integer) As LoadRow
        Set(ByVal Row As LoadRow)
            BeginData = True

            If Not LoadingColumns.Keys.Contains(RecordType) Then
                Throw New Exception(String.Format("Invalid record type [{0}]. Record type [{0}] was not defined.", RecordType))
            Else
                Row.RecordType = RecordType
            End If

            If Not LoadingColumns(RecordType).Count = Row.Count Then
                Throw New Exception( _
                    String.Format( _
                        "Column mismatch. {0} record type {1} columns were defined, but current row contains {2} columns.", _
                        LoadingColumns(RecordType).Count, RecordType, Row.Count _
                        ) _
                    )
            End If

            For i As Integer = 0 To Row.Length
                If LoadingColumns(RecordType)(i).Length > -1 Then
                    If Row.Data(i).Length > LoadingColumns(RecordType)(i).Length Then
                        Throw New Exception( _
                            String.Format( _
                                "Column {0} of record type {1} must be less than {2} characters in length.", _
                                LoadingColumns(RecordType)(i).ColumnName, _
                                RecordType, _
                                LoadingColumns(RecordType)(i).Length _
                            ) _
                        )
                    End If
                End If

                Select Case LoadingColumns(RecordType)(i).ColumnType
                    Case tColumnType.typeCHAR
                        Row.Data(i) = String.Format("'{0}'", Row.Data(i).Replace("'", "' + char(39) + '"))

                    Case tColumnType.typeINT
                        If Row.Data(i).Trim.Length = 0 Then Row.Data(i) = "0"
                        If Not IsNumeric(Row.Data(i)) Then _
                            Throw New Exception( _
                                String.Format( _
                                    "Invalid Data [{0}]. Record Type {1}, column [{2}] is declared as INT.", _
                                    Row.Data(i), _
                                    RecordType, _
                                    LoadingColumns(RecordType)(i).ColumnName _
                                 ) _
                            )
                        Row.Data(i) = String.Format("dbo.INTQUANT({0})", Row.Data(i))

                    Case tColumnType.typeREAL
                        If Row.Data(i).Trim.Length = 0 Then Row.Data(i) = "0"
                        If Not IsNumeric(Row.Data(i)) Then _
                            Throw New Exception( _
                                String.Format( _
                                    "Invalid Data [{0}]. Record Type {1}, column [{2}] is declared as REAL.", _
                                    Row.Data(i), _
                                    RecordType, _
                                    LoadingColumns(RecordType)(i).ColumnName _
                                 ) _
                            )
                        Row.Data(i) = String.Format("dbo.REALQUANT({0})", Row.Data(i))

                    Case tColumnType.typeDATE
                        If Row.Data(i).Trim.Length = 0 Then Row.Data(i) = "0"
                        If IsDate(Row.Data(i)) Then
                            Row.Data(i) = String.Format("{0}", DateDiff(DateInterval.Minute, #1/1/1988#, CDate(Row.Data(i))))
                        ElseIf String.Compare(Row.Data(i), "%NOW%", True) = 0 Then
                            Row.Data(i) = String.Format("{0}", DateDiff(DateInterval.Minute, #1/1/1988#, Now()))
                        ElseIf String.Compare(Row.Data(i), "%DATE%", True) = 0 Then
                            Row.Data(i) = String.Format("{0}", DateDiff(DateInterval.Minute, #1/1/1988#, Now()))
                        ElseIf IsNumeric(Row.Data(i)) Then
                            Row.Data(i) = String.Format("{0}", Row.Data(i))
                        Else
                            Throw New Exception( _
                                String.Format( _
                                    "Invalid Data [{0}]. Record Type {1}, column [{2}] is declared as DATE.", _
                                    Row.Data(i), _
                                    RecordType, _
                                    LoadingColumns(RecordType)(i).ColumnName _
                                 ) _
                            )
                        End If

                    Case tColumnType.typeBOOL
                        Select Case Row.Data(i).ToUpper
                            Case "Y", "N", ""
                                Row.Data(i) = String.Format("'{0}'", Row.Data(i).ToUpper)
                            Case Else
                                Throw New Exception( _
                                    String.Format( _
                                        "Invalid Data [{0}]. Record Type {1}, column [{2}] is declared as BOOLEAN.", _
                                        Row.Data(i), _
                                        RecordType, _
                                        LoadingColumns(RecordType)(i).ColumnName _
                                     ) _
                                )
                        End Select

                End Select
            Next
            LoadingRows.Add(Row)
        End Set
    End Property

#End Region

#Region "Private Properties"

    Private ReadOnly Property toByte() As Byte()
        Get
            Dim myEncoder As New System.Text.ASCIIEncoding
            Dim str As New System.Text.StringBuilder
            Dim xw As XmlWriter = XmlWriter.Create(str)
            With xw

                .WriteStartDocument()
                .WriteStartElement("PriorityLoading")

                .WriteAttributeString("ENVIRONMENT", Me.Environment)
                .WriteAttributeString("TABLE", Me.Table)
                .WriteAttributeString("PROCEDURE", Me.Procedure)

                For Each row As LoadRow In LoadingRows
                    .WriteStartElement("ROW")
                    .WriteAttributeString("RECORDTYPE", row.RecordType.ToString)
                    For Each rt As Integer In LoadingColumns.Keys
                        If rt = row.RecordType Then
                            For i As Integer = 0 To UBound(row.Data)
                                .WriteAttributeString(LoadingColumns(rt)(i).ColumnName, row.Data(i))
                            Next
                        Else
                            For Each col As LoadColumn In LoadingColumns(rt)
                                Select Case col.ColumnType
                                    Case tColumnType.typeBOOL, tColumnType.typeCHAR
                                        .WriteAttributeString(col.ColumnName, "''")
                                    Case tColumnType.typeDATE, tColumnType.typeINT, tColumnType.typeREAL
                                        .WriteAttributeString(col.ColumnName, "0")
                                End Select
                            Next
                        End If
                    Next
                    .WriteEndElement()
                Next
                .WriteEndDocument()
                .Flush()

            End With

            Return myEncoder.GetBytes(str.ToString)
        End Get
    End Property

    Private ReadOnly Property XMLtoSQL() As String
        Get
            Dim sql As New System.Text.StringBuilder
            Dim LN As Integer = 0
            Dim ins As Boolean = False
            Dim PriorityLoading As XmlNode = XMLDoc.SelectSingleNode("//PriorityLoading")

            With Me
                .Clear()
                .Procedure = PriorityLoading.Attributes("PROCEDURE").Value

                If IsNothing(PriorityLoading.Attributes("ENVIRONMENT")) Then
                    .Environment = DefaultEnv
                Else
                    If PriorityLoading.Attributes("ENVIRONMENT").Value.Length > 0 Then
                        .Environment = PriorityLoading.Attributes("ENVIRONMENT").Value
                    Else
                        .Environment = DefaultEnv
                    End If
                End If

                If Not IsNothing(PriorityLoading.Attributes("TABLE")) Then
                    .Table = PriorityLoading.Attributes("TABLE").Value
                Else
                    .Table = Nothing
                End If

                sql.AppendFormat("use [{0}]; ", .Environment).AppendLine()
            End With

            If IsNothing(Me.Table) Then
                Return ""
            End If

            sql.AppendFormat("DELETE FROM {0} WHERE LINE > 0; ", Table).AppendLine()
            sql.Append("delete from ERRMSGS ").AppendLine()
            sql.Append("where T$USER = ").AppendLine()
            sql.Append("(SELECT T$USER FROM system.dbo.USERS where USERLOGIN='%USERNAME%') ").AppendLine()
            sql.Append("and TYPE = 'i';").AppendLine()
            sql.Append("DECLARE @LN INT; ").AppendLine()
            sql.AppendFormat("SET @LN = (SELECT MAX(LINE) FROM {0}); ", Table).AppendLine()
            For Each Row As XmlNode In PriorityLoading.SelectNodes("//ROW")
                LN += 1
                If Not ins Then
                    sql.AppendFormat("insert into {0} (LINE, BUBBLEID, ", Table)
                    For i As Integer = 0 To Row.Attributes.Count - 1
                        sql.Append(Row.Attributes(i).Name)
                        If i < Row.Attributes.Count - 1 Then
                            sql.Append(", ")
                        Else
                            sql.Append(") ").AppendLine()
                        End If
                    Next
                    ins = True

                Else
                    sql.Append("UNION ALL ").AppendLine()
                End If

                sql.AppendFormat("select @LN + {0}, '{1}', ", LN, BubbleID)
                For i As Integer = 0 To Row.Attributes.Count - 1
                    sql.Append(Row.Attributes(i).Value)
                    If i < Row.Attributes.Count - 1 Then
                        sql.Append(", ")
                    Else
                        sql.AppendLine()
                    End If
                Next
            Next
            Return sql.ToString
        End Get
    End Property

    Public ReadOnly Property SOAPToSQL(ByVal filename As String) As String
        Get
            Dim l As Integer = 0
            Dim sql As New System.Text.StringBuilder
            Dim DefaultRow As New List(Of String)
            Dim LN As Integer = 0
            Dim sr As StreamReader

            Try
                ' The file exists but may still be being written to.
                ' Wait for it to become available.
                Dim exep As Exception
                Do
                    Try
                        exep = New Exception
                        exep = Nothing
                        sr = New StreamReader(CStr(filename))
                    Catch EX As Exception
                        exep = EX
                        Threading.Thread.Sleep(1)
                    End Try
                Loop Until IsNothing(exep)

                While Not sr.EndOfStream
                    With Me
                        Dim line() As String = Split(sr.ReadLine(), Chr(9))
                        If l = 4 Then
                            sql.Append(") ").AppendLine()
                        End If
                        If l > 4 Then
                            sql.Append("UNION ALL ").AppendLine()
                        End If
                        If l > 3 Then
                            LN += 1
                            sql.AppendFormat("select @LN + {0}, '{1}', '{2}', ", LN, .BubbleID, line(0))
                        End If

                        Select Case l
                            Case 0
                                .Clear()
                                .Table = line(0)
                                .Procedure = line(1)
                                If line(2).Length > 0 Then
                                    .Environment = line(2)
                                Else
                                    .Environment = DefaultEnv
                                End If
                                sql.AppendFormat("use [{0}]; ", .Environment).AppendLine()
                                sql.AppendFormat("DELETE FROM {0} WHERE LINE > 0; ", .Table).AppendLine()
                                sql.Append("delete from ERRMSGS ").AppendLine()
                                sql.Append("where T$USER = ").AppendLine()
                                sql.Append("(SELECT T$USER FROM system.dbo.USERS where USERLOGIN='%USERNAME%') ").AppendLine()
                                sql.Append("and TYPE = 'i';").AppendLine()
                                sql.Append("DECLARE @LN INT; ").AppendLine()
                                sql.AppendFormat("SET @LN = (SELECT MAX(LINE) FROM {0}); ", .Table).AppendLine()
                                sql.AppendFormat("insert into {0} (LINE, BUBBLEID, RECORDTYPE, ", Table)

                            Case 1, 2, 3
                                For li As Integer = 1 To UBound(line)
                                    Select Case l
                                        Case 1, 2
                                            If ((l = 1 And li > 1) Or (l = 2)) And line(li).Length > 0 Then
                                                sql.Append(", ")
                                            End If
                                            If line(li).Length > 0 Then
                                                sql.AppendFormat("{0}", line(li))
                                            End If
                                        Case 3
                                            DefaultRow.Add(line(li).ToUpper)
                                    End Select
                                Next

                            Case Else

                                For i As Integer = 0 To DefaultRow.Count - 1

                                    Select Case DefaultRow(i).ToLower

                                        Case "text", "char"
                                            sql.AppendFormat("'{0}'", line(i + 1).Replace("'", "' + char(39) + '"))

                                        Case "int"
                                            If line(i + 1).Length > 0 Then
                                                sql.AppendFormat("dbo.INTQUANT({0})", line(i + 1))
                                            Else
                                                sql.Append("0")
                                            End If

                                        Case "real"
                                            If line(i + 1).Length > 0 Then
                                                sql.AppendFormat("dbo.REALQUANT({0})", line(i + 1))
                                            Else
                                                sql.Append("0")
                                            End If

                                        Case "time"
                                            If line(i + 1).Length > 0 Then
                                                Try
                                                    sql.AppendFormat("{0}", _
                                                        DateDiff( _
                                                            DateInterval.Minute, _
                                                            CDate("00:00"), _
                                                            CDate(line(i + 1))).ToString _
                                                        )
                                                Catch
                                                    sql.Append("0")
                                                End Try
                                            Else
                                                sql.Append("0")
                                            End If

                                        Case "date"
                                            If line(i + 1).Length > 0 Then

                                                If IsNumeric(line(i + 1)) Then
                                                    sql.AppendFormat("{0}", line(i + 1).ToString)

                                                ElseIf IsDate(line(i + 1)) Then
                                                    Try
                                                        sql.AppendFormat("{0}", _
                                                            DateDiff( _
                                                                DateInterval.Minute, _
                                                                New Date(1988, 1, 1), _
                                                                CDate(line(i + 1))).ToString _
                                                            )
                                                    Catch
                                                        sql.Append("0")
                                                    End Try

                                                ElseIf String.Compare(line(i + 1), "%DATE%", True) Then
                                                    Try
                                                        sql.AppendFormat("{0}", _
                                                            DateDiff( _
                                                                DateInterval.Minute, _
                                                                New Date(1988, 1, 1), _
                                                                Now()).ToString _
                                                            )
                                                    Catch
                                                        sql.Append("0")
                                                    End Try

                                                Else
                                                    sql.Append("0")
                                                End If

                                            Else
                                                sql.Append("0")
                                            End If

                                        Case Else
                                            If line(i + 1).Length > 0 Then
                                                sql.AppendFormat("{0}", line(i + 1))
                                            Else
                                                sql.Append("0")
                                            End If

                                    End Select

                                    If i < DefaultRow.Count - 1 Then
                                        sql.Append(", ")
                                    Else
                                        sql.Append(" ")
                                    End If
                                Next

                        End Select

                    End With
                    l += 1

                End While

                ' End Using
                With sr
                    .Close()
                    .Dispose()
                End With

                Return sql.ToString

            Catch ex As Exception
                Throw New Exception(String.Format("Invalid SOAP Bubble syntax in {0}. Error thrown was {1}", filename, ex.Message))
            End Try
        End Get

    End Property

    Private ReadOnly Property XMLtoPLSQL() As String
        Get
            Dim sql As New System.Text.StringBuilder
            Dim LN As Integer = 0
            Dim ins As Boolean = False
            Dim PriorityLoading As XmlNode = XMLDoc.SelectSingleNode("//PriorityLoading")

            With Me
                .Clear()
                .Procedure = PriorityLoading.Attributes("PROCEDURE").Value

                If IsNothing(PriorityLoading.Attributes("ENVIRONMENT")) Then
                    .Environment = DefaultEnv
                Else
                    If PriorityLoading.Attributes("ENVIRONMENT").Value.Length > 0 Then
                        .Environment = PriorityLoading.Attributes("ENVIRONMENT").Value
                    Else
                        .Environment = DefaultEnv
                    End If
                End If

                If Not IsNothing(PriorityLoading.Attributes("TABLE")) Then
                    .Table = PriorityLoading.Attributes("TABLE").Value
                Else
                    .Table = Nothing
                End If
            End With

            If IsNothing(Me.Table) Then
                Return ""
            End If

            sql.AppendFormat("DECLARE LN INTEGER ;", "").AppendLine()
            sql.AppendFormat("BEGIN", "").AppendLine()
            sql.AppendFormat("DELETE FROM {0}${1} WHERE LINE > 0; ", Environment, Table).AppendLine()
            sql.AppendFormat("select MAX(LINE) into LN from {0}${1};", Environment, Table).AppendLine()
            sql.AppendFormat("delete from {0}$ERRMSGS ", Environment).AppendLine()
            sql.Append("where T$USER = ").AppendLine()
            sql.Append("(select T$USER from USERS where reverse(USERLOGIN) = '%USERNAME%' or USERLOGIN = '%USERNAME%')").AppendLine()
            sql.Append("and TYPE = 'i';").AppendLine()

            For Each Row As XmlNode In PriorityLoading.SelectNodes("//ROW")
                LN += 1
                If Not ins Then
                    sql.AppendFormat("insert into {0}${1} (LINE, BUBBLEID, ", Environment, Table)
                    For i As Integer = 0 To Row.Attributes.Count - 1
                        sql.Append(Row.Attributes(i).Name)
                        If i < Row.Attributes.Count - 1 Then
                            sql.Append(", ")
                        Else
                            sql.Append(") ").AppendLine()
                        End If
                    Next
                    ins = True

                Else
                    sql.Append("UNION ALL ").AppendLine()
                End If

                sql.AppendFormat("select LN + {0}, '{1}', ", LN, BubbleID)
                For i As Integer = 0 To Row.Attributes.Count - 1
                    Dim val As String
                    If Row.Attributes(i).Value.Contains(".") Then
                        If String.Compare("DBO.", Row.Attributes(i).Value.Substring(0, 4).ToUpper) = 0 Then
                            val = Row.Attributes(i).Value.Substring(4)
                        Else
                            val = Row.Attributes(i).Value
                        End If
                    Else
                        val = Row.Attributes(i).Value
                    End If
                    val = val.Replace("' + char(39) + '", "' || chr(39) || '")
                    If val = "''" Then val = "' '"
                    sql.Append(val)

                    If i < Row.Attributes.Count - 1 Then
                        sql.Append(", ")
                    Else
                        sql.Append(" FROM DUAL ").AppendLine()
                    End If
                Next
            Next
            sql.Append(";").AppendLine()
            sql.AppendFormat("END;", "").AppendLine()
            Return sql.ToString
        End Get
    End Property

    Public ReadOnly Property SOAPToPLSQL(ByVal filename As String) As String
        Get
            Dim l As Integer = 0
            Dim sql As New System.Text.StringBuilder
            Dim DefaultRow As New List(Of String)
            Dim LN As Integer = 0
            Dim sr As StreamReader

            Try
                ' The file exists but may still be being written to.
                ' Wait for it to become available.
                Dim exep As Exception
                Do
                    Try
                        exep = New Exception
                        exep = Nothing
                        sr = New StreamReader(CStr(filename))
                    Catch EX As Exception
                        exep = EX
                        Threading.Thread.Sleep(1)
                    End Try
                Loop Until IsNothing(exep)

                While Not sr.EndOfStream
                    With Me
                        Dim line() As String = Split(sr.ReadLine(), Chr(9))
                        If l = 4 Then
                            sql.Append(") ").AppendLine()
                        End If
                        If l > 4 Then
                            sql.Append("UNION ALL ").AppendLine()
                        End If
                        If l > 3 Then
                            LN += 1
                            sql.AppendFormat("select LN + {0}, '{1}', '{2}', ", LN, .BubbleID, line(0))
                        End If

                        Select Case l
                            Case 0
                                .Clear()
                                .Table = line(0)
                                .Procedure = line(1)
                                If line(2).Length > 0 Then
                                    .Environment = line(2)
                                Else
                                    .Environment = DefaultEnv
                                End If

                                sql.AppendFormat("DECLARE LN INTEGER ;", "").AppendLine()
                                sql.AppendFormat("BEGIN", "").AppendLine()
                                sql.AppendFormat("DELETE FROM {0}${1} WHERE LINE > 0; ", Environment, .Table).AppendLine()
                                sql.AppendFormat("delete from {0}$ERRMSGS ", Environment).AppendLine()
                                sql.Append("where T$USER = ").AppendLine()
                                sql.Append("(select T$USER from USERS where reverse(USERLOGIN) = '%USERNAME%' or USERLOGIN = '%USERNAME%')").AppendLine()
                                sql.Append("and TYPE = 'i';").AppendLine()
                                sql.AppendFormat("select MAX(LINE) into LN from {0}${1};", Environment, Table).AppendLine()
                                sql.AppendFormat("insert into {0}${1} (LINE, BUBBLEID, RECORDTYPE, ", Environment, Table)

                            Case 1, 2, 3
                                For li As Integer = 1 To UBound(line)
                                    Select Case l
                                        Case 1, 2
                                            If ((l = 1 And li > 1) Or (l = 2)) And line(li).Length > 0 Then
                                                sql.Append(", ")
                                            End If
                                            If line(li).Length > 0 Then
                                                sql.AppendFormat("{0}", line(li))
                                            End If
                                        Case 3
                                            DefaultRow.Add(line(li).ToUpper)
                                    End Select
                                Next

                            Case Else

                                For i As Integer = 0 To DefaultRow.Count - 1

                                    Select Case DefaultRow(i).ToLower

                                        Case "text", "char"
                                            Dim val As String
                                            val = String.Format("'{0}'", line(i + 1).Replace("'", "' || chr(39) || '"))
                                            If val = "''" Then val = "' '"
                                            sql.Append(val)

                                        Case "int"
                                            sql.AppendFormat("INTQUANT({0})", line(i + 1))

                                        Case "real"
                                            sql.AppendFormat("REALQUANT({0})", line(i + 1))

                                        Case "time"
                                            If line(i + 1).Length > 0 Then
                                                Try
                                                    sql.AppendFormat("{0}", _
                                                        DateDiff( _
                                                            DateInterval.Minute, _
                                                            CDate("00:00"), _
                                                            CDate(line(i + 1))).ToString _
                                                        )
                                                Catch
                                                    sql.Append("0")
                                                End Try
                                            Else
                                                sql.Append("0")
                                            End If

                                        Case "date"
                                            If line(i + 1).Length > 0 Then

                                                If IsNumeric(line(i + 1)) Then
                                                    sql.AppendFormat("{0}", line(i + 1).ToString)

                                                ElseIf IsDate(line(i + 1)) Then
                                                    Try
                                                        sql.AppendFormat("{0}", _
                                                            DateDiff( _
                                                                DateInterval.Minute, _
                                                                New Date(1988, 1, 1), _
                                                                CDate(line(i + 1))).ToString _
                                                            )
                                                    Catch
                                                        sql.Append("0")
                                                    End Try

                                                ElseIf String.Compare(line(i + 1), "%DATE%", True) Then
                                                    Try
                                                        sql.AppendFormat("{0}", _
                                                            DateDiff( _
                                                                DateInterval.Minute, _
                                                                New Date(1988, 1, 1), _
                                                                Now()).ToString _
                                                            )
                                                    Catch
                                                        sql.Append("0")
                                                    End Try

                                                Else
                                                    sql.Append("0")
                                                End If

                                            Else
                                                sql.Append("0")
                                            End If

                                        Case Else
                                            If line(i + 1).Length > 0 Then
                                                sql.AppendFormat("{0}", line(i + 1))
                                            Else
                                                sql.Append("0")
                                            End If

                                    End Select

                                    If i < DefaultRow.Count - 1 Then
                                        sql.Append(", ")
                                    Else
                                        sql.Append(" FROM DUAL ").AppendLine()
                                    End If
                                Next

                        End Select

                    End With
                    l += 1

                End While

                ' End Using
                With sr
                    .Close()
                    .Dispose()
                End With

                sql.Append(";").AppendLine()
                sql.AppendFormat("END;", "").AppendLine()

                Return sql.ToString

            Catch ex As Exception
                Throw New Exception(String.Format("Invalid SOAP Bubble syntax in {0}. Error thrown was {1}", filename, ex.Message))
            End Try
        End Get

    End Property

    Private ReadOnly Property DefaultEnv() As String
        Get
            Return _DefaultEnv
        End Get
    End Property

    Private Property XMLDoc() As XmlDocument
        Get
            Return _XMLDoc
        End Get
        Set(ByVal value As XmlDocument)
            _XMLDoc = value
        End Set
    End Property

#End Region

#Region "Public Methods"

    Public Sub Clear()
        LoadingColumns = New Dictionary(Of Integer, List(Of LoadColumn))
        LoadingRows = New List(Of LoadRow)
        With Me
            .Environment = Nothing
            .Table = Nothing
            .Procedure = Nothing
            .XMLDoc = Nothing
        End With
    End Sub

    Public Function ToFile(ByVal Filename As String, ByRef ex As Exception, Optional ByVal Overwrite As Boolean = False) As Boolean

        Dim Saved As Boolean = False
        ex = Nothing

        Try
            ' Dete existing file or throw error if not(overwrite)
            If File.Exists(Filename) Then
                If Overwrite Then
                    Do
                        File.Delete(Filename)
                        Threading.Thread.Sleep(1)
                    Loop Until Not File.Exists(Filename)
                Else
                    Throw New Exception( _
                        String.Format( _
                            "File [{0}] already exists.", _
                            Filename _
                         ) _
                    )
                End If
            End If

            ' Validate Procedure and Table are present
            If IsNothing(Me.Procedure) Or IsNothing(Me.Table) Then
                Throw New Exception( _
                    String.Format( _
                        "Procedure or loading table not specified." _
                     ) _
                )
            End If

            ' Validate the loading contains some rows
            If Not LoadingRows.Count > 0 Then
                Throw New Exception( _
                    String.Format( _
                        "Loading contains no data." _
                     ) _
                )
            End If

            Using sw As New StreamWriter(Filename)
                sw.Write(toByte)
            End Using

            Saved = True

        Catch SaveException As Exception
            Throw New Exception( _
                String.Format( _
                    "Bubble.ToFile failed: {0}", _
                    SaveException.Message _
                 ) _
            )
        End Try

        Return Saved

    End Function

    Public Function Post(ByVal url As String, ByRef Ex As Exception) As Boolean

        Dim uploadResponse As Net.HttpWebResponse
        Dim requestStream As Stream
        Dim posted As Boolean = False
        Ex = Nothing

        Try

            ' Add script handler name if not specified by the request
            ' Defaults to loadhandler.ashx if ommited
            Try
                If Not String.Compare(url.Split("/").Last.Split(".").Last, "ashx", True) = 0 Then
                    If Not String.Compare(url.Last, "/") = 0 Then
                        url += "/"
                    End If
                    url += "loadHandler.ashx"
                End If
            Catch
                Throw New Exception("Invalid URL specified.")
            End Try

            ' Validate Procedure and Table are present
            If IsNothing(Me.Procedure) Or IsNothing(Me.Table) Then
                Throw New Exception( _
                    String.Format( _
                        "Procedure or loading table not specified." _
                     ) _
                )
            End If

            ' Validate the loading contains some rows
            If Not LoadingRows.Count > 0 Then
                Throw New Exception( _
                    String.Format( _
                        "Loading contains no data." _
                     ) _
                )
            End If

            requestStream = Nothing
            uploadResponse = Nothing

            Dim ms As MemoryStream = New MemoryStream(toByte)
            Dim uploadRequest As Net.HttpWebRequest = CType(Net.HttpWebRequest.Create(url), Net.HttpWebRequest)

            uploadRequest.Method = "POST"
            uploadRequest.Proxy = Nothing
            uploadRequest.SendChunked = True
            requestStream = uploadRequest.GetRequestStream()

            ' Upload the XML
            Dim buffer(1024) As Byte
            Dim bytesRead As Integer
            While True
                bytesRead = ms.Read(buffer, 0, buffer.Length)
                If bytesRead = 0 Then
                    Exit While
                End If
                requestStream.Write(buffer, 0, bytesRead)
            End While

            ' The request stream must be closed before getting the response.
            requestStream.Close()
            uploadResponse = uploadRequest.GetResponse()

            Dim thisRequest As New XmlDocument
            Dim reader As New StreamReader(uploadResponse.GetResponseStream)
            With thisRequest
                .LoadXml(reader.ReadToEnd)
                Dim n As XmlNode = .SelectSingleNode("response")
                Dim er As Boolean = False
                For Each attrib As XmlAttribute In n.Attributes
                    If attrib.Name = "status" Then
                        If Not attrib.Value = "200" Then er = True
                    End If
                    If attrib.Name = "message" Then
                        If er Then
                            Throw New Exception(attrib.Value)
                        End If
                    End If
                Next
            End With

            posted = True

        Catch exep As UriFormatException
            Ex = New Exception(String.Format("Invalid URL: {0}", exep.Message))
        Catch exep As Net.WebException
            Ex = New Exception(String.Format("Connection Error: {0}", exep.Message))
        Catch exep As Exception
            Ex = New Exception(String.Format("Posting failed: {0}", exep.Message))
        Finally
            ' Clean up the streams
            If Not IsNothing(uploadResponse) Then
                uploadResponse.Close()
            End If
            If Not IsNothing(requestStream) Then
                requestStream.Close()
            End If
        End Try

        Return posted

    End Function

    Public Sub FromFile(ByVal FileName As String)

        ' Must initialise with a defualt environment if here
        If IsNothing(DefaultEnv) Then
            Throw New Exception( _
                String.Format( _
                    "Default Priority loading environment was not specified.", _
                    BubbleID _
                 ) _
            )
        End If

        If Not File.Exists(FileName) Then
            Throw New Exception( _
                String.Format( _
                    "Missing Bubble File [{0}].", FileName _
                    ) _
                )
        Else
            _BubbleID = FileName.Split("\").Last.Split(".").First
        End If

        Select Case FileName.Split("\").Last.Split(".").Last.ToUpper
            Case "XML"
                Dim retry As Integer = 0
                Dim exep As Exception
                Do
                    Try
                        exep = New Exception
                        exep = Nothing
                        XMLDoc.Load(FileName)
                    Catch EX As Exception
                        retry += 1
                        exep = EX
                        Threading.Thread.Sleep(20)
                    End Try
                Loop Until IsNothing(exep) Or retry > 50

                If retry > 100 Then
                    Throw New Exception( _
                        String.Format( _
                            "Invalid XML in [{0}].", _
                            FileName.Split("\").Last.Split(".").Last.ToUpper _
                         ) _
                    )
                End If

                If IsNothing(XMLDoc.SelectSingleNode("//PriorityLoading")) Then
                    Throw New Exception( _
                        String.Format( _
                            "Invalid XML bubble [{0}].", _
                            BubbleID _
                         ) _
                    )
                End If

                Select Case _Provider
                    Case eProviderType.MSSQL
                        Me.BubbleSQL = XMLtoSQL
                    Case eProviderType.ORACLE
                        Me.BubbleSQL = XMLtoPLSQL
                End Select


            Case "TXT"
                Try
                    Select Case _Provider
                        Case eProviderType.MSSQL
                            Me.BubbleSQL = SOAPToSQL(FileName)
                        Case eProviderType.ORACLE
                            Me.BubbleSQL = SOAPToPLSQL(FileName)
                    End Select

                Catch ex As Exception
                    Throw New Exception( _
                        String.Format( _
                            "Invalid SOAP bubble [{0}]: {1}", _
                            BubbleID, _
                            ex.Message _
                        ) _
                    )
                End Try

            Case Else
                Throw New Exception( _
                    String.Format( _
                        "Invalid Bubble File Type [{0}]. Expecting XML or TXT", _
                        FileName.Split("\").Last.Split(".").Last.ToUpper _
                     ) _
                )
        End Select

    End Sub

    Public Function NamedType(ByVal TypeStr As String) As tColumnType
        Select Case TypeStr.ToLower
            Case "int"
                Return tColumnType.typeINT
            Case "real"
                Return tColumnType.typeREAL
            Case "char"
                Return tColumnType.typeCHAR
            Case "date"
                Return tColumnType.typeDATE
            Case "time"
                Return tColumnType.typeTIME
            Case Else
                Return tColumnType.typeCHAR
        End Select
    End Function

#End Region

End Class
