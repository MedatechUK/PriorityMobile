Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient
Imports System.io

Public Class SerialData

#Region "public properties"

    Public ReadOnly Property RowCount() As Integer
        Get
            If Not IsNothing(Data) Then
                Return UBound(Data, 2) + 1
            Else
                Return 0
            End If
        End Get
    End Property

    Public Function GetDataError() As Exception
        With Me
            If Not IsNothing(.Data) Then
                If String.Compare(Strings.Left(.Data(0, 0), 1), "!") = 0 Then
                    Return New Exception(Right(.Data(0, 0), .Data(0, 0).Length - 1))
                Else
                    Return Nothing
                End If
            Else
                Return Nothing
            End If
        End With
    End Function

    Private _Data As String(,)
    Public ReadOnly Property Data() As String(,)
        Get
            Return _Data
        End Get
    End Property

#End Region

#Region "To"

    Public Sub ToFile(ByVal Filename As String)
        Dim y, n As Integer
        Dim er As Boolean = True

        If Not IsNothing(_Data) Then
            ' Data in array, so write it            
            Using sw As StreamWriter = New StreamWriter(Filename)
                ' Add some text to the file.
                For y = 0 To _Data.GetUpperBound(1)
                    For n = 0 To _Data.GetUpperBound(0)
                        sw.Write(_Data(n, y))
                        If n < _Data.GetUpperBound(0) Then sw.Write(Chr(9))
                    Next
                    If y < _Data.GetUpperBound(1) Then sw.Write(vbCrLf)
                Next
                sw.Close()
            End Using
        End If

    End Sub

    Public Function ToSerial() As String

        Dim ret As String = ""
        If IsNothing(Data) Then Return ""

        For y As Integer = 0 To UBound(Data, 2)
            If Len(ret) > 0 Then
                ret += "\n"
            End If
            For x As Integer = 0 To UBound(Data, 1)
                ret += Data(x, y)
                If x < UBound(Data, 1) Then
                    ret += "\t"
                End If
            Next
        Next

        Return ret

    End Function

#End Region

#Region "From"

    Public Function FromFile(ByVal Filename As String) As Boolean

        Dim ret As Boolean = True
        Dim it(-1) As String
        Dim x As Integer
        Dim Arr(-1, -1) As String
        Dim er As Boolean = True

        _Data = Nothing
        ' Create an instance of StreamReader to read from a file.
        Try
            If Not File.Exists(Filename) Then Throw New Exception(String.Format("File [{0}] not found.", Filename))
            Using sr As StreamReader = New StreamReader(CStr(Filename))
                Dim line As String
                ' Read and display the lines from the file until the end 
                ' of the file is reached.
                Do
                    line = sr.ReadLine()
                    If Len(Trim(line)) > 0 Then
                        it = Split(line, Chr(9))
                        ReDim Preserve Arr(UBound(it), Arr.GetUpperBound(1) + 1)
                        For x = 0 To UBound(it)
                            Arr(x, Arr.GetUpperBound(1)) = it(x)
                        Next
                    End If
                Loop Until line Is Nothing
                sr.Close()
                _Data = Arr
            End Using
        Catch ex As Exception
            ret = False
        End Try
        Return ret

    End Function

    Public Sub FromStr(ByVal data As String)

        _Data = Nothing
        If Len(Trim(data)) = 0 Then Exit Sub

        data = Replace(Trim(data), vbCrLf, "")
        If Right(data, 2) = "\n" Then
            data = Left(data, data.Length - 2)
        End If

        Dim l As String() = Split(data, "\n")
        Dim v As String() = Split(l(0), "\t")

        Dim x As Integer = UBound(v)
        Dim y As Integer = UBound(l)

        ReDim _Data(x, y)

        For yy As Integer = 0 To y
            v = Split(l(yy), "\t")
            For xx As Integer = 0 To x
                _Data(xx, yy) = v(xx)
            Next
        Next

    End Sub

    Public Sub FromSQLDataReader(ByVal dataReader As SqlDataReader)

        Dim first As Boolean = True
        _Data = Nothing

        Do While dataReader.Read()
            If first Then
                first = False
                ReDim _Data(dataReader.FieldCount - 1, 0)
            Else
                ReDim Preserve _Data(UBound(Data, 1), UBound(Data, 2) + 1)
            End If
            For i As Integer = 0 To dataReader.FieldCount - 1
                _Data(i, UBound(Data, 2)) = dataReader(i)
            Next
        Loop

    End Sub

#End Region

End Class
