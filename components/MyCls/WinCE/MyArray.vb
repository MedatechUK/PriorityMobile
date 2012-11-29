Imports System
Imports System.IO

Public Class MyArray

    Public Function ArrayFromFile(ByRef ThisArray(,) As String, ByVal Filename As String) As Boolean

        Dim it(-1) As String
        Dim x As Integer
        Dim Arr(-1, -1) As String
        Dim er As Boolean = True

        Try
            ' Create an instance of StreamReader to read from a file.
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
                ThisArray = Arr
            End Using
        Catch E As Exception
            er = False
        End Try

        Return er

    End Function

    Public Function TextToArray(ByVal Text As String, ByRef ThisArray(,) As String) As Boolean

        Dim er As Boolean = True
        Dim it(-1), line(-1) As String
        Dim n, x As Integer
        Dim arr(-1, -1) As String

        Try
            line = Split(Text, "\n")
            For n = 0 To UBound(line)
                If Len(Trim(line(n))) > 0 Then
                    it = Split(line(n), "\t")
                    ReDim Preserve arr(UBound(it), arr.GetUpperBound(1) + 1)
                    For x = 0 To UBound(it)
                        arr(x, arr.GetUpperBound(1)) = it(x)
                    Next
                End If
            Next
            ThisArray = arr
        Catch E As Exception
            er = False
        End Try

        Return er

    End Function

    Public Function ArrayToFile(ByVal filename As String, ByRef ThisArray(,) As String) As Boolean

        Dim y, n As Integer
        Dim er As Boolean = True

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

    Public Function InArray(ByRef ThisArray(,) As String, ByVal Ordinal As Integer, ByVal Search As String) As Integer

        Dim y As Integer
        Dim i As Integer = -1

        Try
            For y = 0 To UBound(ThisArray, 2)
                If LCase(ThisArray(Ordinal, y)) = LCase(Search) Then
                    i = y
                    Exit For
                End If
            Next
        Catch
            i = -1
        End Try

        Return i

    End Function

    Public Function InList(ByRef ThisArray() As String, ByVal Search As String) As Integer

        Dim y As Integer
        Dim i As Integer = -1

        Try
            For y = 0 To UBound(ThisArray)
                If LCase(ThisArray(y)) = LCase(Search) Then
                    i = y
                    Exit For
                End If
            Next
        Catch
            i = -1
        End Try

        Return i

    End Function

    Public Sub DeleteByIndex(ByRef ThisArray(,) As String, ByVal Ordinal() As String)

        Dim tArray(-1, -1) As String
        Dim y, i As Integer

        For y = 0 To UBound(ThisArray, 2)
            If InList(Ordinal, CStr(y)) > -1 Then
                ' Do not add
            Else
                ReDim Preserve tArray(UBound(ThisArray, 1), UBound(tArray, 2) + 1)
                For i = 0 To UBound(ThisArray, 1)
                    tArray(i, UBound(tArray, 2)) = ThisArray(i, y)
                Next
            End If
        Next

        ThisArray = tArray

    End Sub

    Public Sub DeleteByCriteria(ByRef ThisArray(,) As String, ByVal Ordinal As Integer, ByVal Search As String)

        Dim tArray(-1, -1) As String
        Dim y, i As Integer
        Dim hasdata As Boolean = False

        For y = 0 To UBound(ThisArray, 2)
            If LCase(ThisArray(Ordinal, y)) = LCase(Search) Then
                ' Do not add
            Else
                hasdata = True
                ReDim Preserve tArray(UBound(ThisArray, 1), UBound(tArray, 2) + 1)
                For i = 0 To UBound(ThisArray, 1)
                    tArray(i, UBound(tArray, 2)) = ThisArray(i, y)
                Next
            End If
        Next

        If hasdata Then
            ThisArray = tArray
        Else
            ThisArray = Nothing
        End If

    End Sub

    Public Function SubSet(ByRef ThisArray(,) As String, ByVal Ordinal As Integer, ByVal Search As String) As String(,)

        Dim tArray(,) As String = Nothing
        Dim y, i As Integer

        For y = 0 To UBound(ThisArray, 2)
            If LCase(ThisArray(Ordinal, y)) = LCase(Search) Then
                Try
                    ReDim Preserve tArray(UBound(ThisArray, 1), UBound(tArray, 2) + 1)
                Catch
                    ReDim Preserve tArray(UBound(ThisArray, 1), 0)
                End Try
                For i = 0 To UBound(ThisArray, 1)
                    tArray(i, UBound(tArray, 2)) = ThisArray(i, y)
                Next
            Else
                ' Do not add
            End If
        Next

        Return tArray

    End Function

    Public Function Matching(ByVal ThisArray(,) As String, ByVal Conditions(,) As String) As Integer()

        Dim y, i As Integer
        Dim m As Boolean
        Dim tArray(-1) As Integer
        Try
            For y = 0 To UBound(ThisArray, 2)
                m = True
                For i = 0 To UBound(Conditions, 2)
                    If ThisArray(CInt(Conditions(0, i)), y) <> Conditions(1, i) Then
                        m = False
                        Exit For
                    End If
                Next
                If m Then
                    ReDim Preserve tArray(UBound(tArray) + 1)
                    tArray(UBound(tArray)) = y
                End If
            Next
        Catch
        End Try

        Return tArray

    End Function

    Public Function IntToStr(ByVal array(,) As Integer)
        Dim tarray(UBound(array, 1), UBound(array, 2)) As String
        For y As Integer = 0 To UBound(array, 2)
            For x As Integer = 0 To UBound(array, 1)
                tarray(x, y) = CStr(array(x, y))
            Next
        Next
        Return tarray
    End Function

    Public Function StrToInt(ByVal array(,) As String)
        Dim tarray(UBound(array, 1), UBound(array, 2)) As Integer
        For y As Integer = 0 To UBound(array, 2)
            For x As Integer = 0 To UBound(array, 1)
                tarray(x, y) = CInt(array(x, y))
            Next
        Next
        Return tarray
    End Function

    Public Function IntToStr1d(ByVal array() As Integer)
        Dim tarray(UBound(array)) As String
        For x As Integer = 0 To UBound(array, 1)
            tarray(x) = CStr(array(x))
        Next
        Return tarray
    End Function

    Public Function StrToInt1d(ByVal array() As String)
        Dim tarray(UBound(array)) As Integer
        For x As Integer = 0 To UBound(array, 1)
            tarray(x) = CInt(array(x))
        Next
        Return tarray
    End Function

    Public Function Group(ByVal ThisArray(,) As String, ByVal Ordinal As Integer) As String()

        Dim ret(-1) As String
        Dim i As Integer

        If Not IsNothing(ThisArray) Then
            Try
                For i = 0 To UBound(ThisArray, 2)
                    If InList(ret, ThisArray(Ordinal, i)) = -1 Then
                        ReDim Preserve ret(UBound(ret) + 1)
                        ret(UBound(ret)) = ThisArray(Ordinal, i)
                    End If
                Next
            Catch
                ret = Nothing
            End Try
        End If
        Return ret

    End Function

    Public Function Merge(ByVal FirstArray(,) As String, ByVal SecondArray(,) As String) As String(,)

        Dim x, y As Integer
        Dim tArray(,) As String

        tArray = FirstArray

        For y = 0 To UBound(SecondArray, 2)
            ReDim Preserve tArray(UBound(SecondArray, 1), UBound(tArray, 2) + 1)
            For x = 0 To UBound(tArray, 1)
                Try
                    tArray(x, UBound(tArray, 2)) = SecondArray(x, y)
                Catch
                    Beep()
                End Try
            Next
        Next

        Return tArray

    End Function

    Public Function SameList(ByVal ListA() As String, ByVal ListB() As String) As Boolean

        Dim y As Integer

        If UBound(ListA) <> UBound(ListB) Then
            Return False
            Exit Function
        End If

        For y = 0 To UBound(ListA)
            If Not ListA(y) = ListB(y) Then
                Return False
                Exit Function
            End If
        Next

        Return True

    End Function
End Class
