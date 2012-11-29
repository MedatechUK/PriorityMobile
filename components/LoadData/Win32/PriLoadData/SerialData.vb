Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient

Public Class SerialData

    Public Function SerialiseDataReader(ByVal dataReader As SqlDataReader)

        Dim ret As String = ""

        Do While dataReader.Read()
            If Len(ret) > 0 Then
                ret = ret & "\n"
            End If
            For i As Integer = 0 To dataReader.FieldCount - 1
                ret = ret & dataReader(i)
                If i < dataReader.FieldCount - 1 Then
                    ret = ret & "\t"
                End If
            Next
        Loop

        Return ret

    End Function

    Public Function SerialiseDataArray(ByVal data(,) As String)

        Dim ret As String = ""
        If IsNothing(data) Then Return ret

        For y As Integer = 0 To UBound(data, 2)
            If Len(ret) > 0 Then
                ret = ret & "\n"
            End If
            For x As Integer = 0 To UBound(data, 1)
                ret = ret & data(x, y)
                If x < UBound(data, 1) Then
                    ret = ret & "\t"
                End If
            Next
        Next

        Return ret

    End Function

    Public Function DeSerialiseData(ByVal data As String)

        Dim ret As String(,) = Nothing
        If Len(Trim(data)) = 0 Then Return ret

        Dim l As String() = Split(data, "\n")
        Dim v As String() = Split(l(0), "\t")

        Dim x As Integer = UBound(v)
        Dim y As Integer = UBound(l)

        ReDim ret(x, y)

        For yy As Integer = 0 To y
            v = Split(l(yy), "\t")
            For xx As Integer = 0 To x
                ret(xx, yy) = v(xx)
            Next
        Next

        Return ret

    End Function

End Class
