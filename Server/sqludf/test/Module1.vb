Module Module1
    Private Numbers As String = "1234567890ABCDEFGHIJKLMNOPQRTSUVWXYZ"

    Private Class ch

        Private _loc As Integer = 0

        Public ReadOnly Property Value() As String
            Get
                Return Numbers.Substring(_loc, 1)
            End Get
        End Property

        Public Function Increment() As Boolean
            If _loc < Numbers.Length - 1 Then
                _loc += 1
                Return True
            Else
                Return False
            End If
        End Function

    End Class

    Sub Main()

        Dim ModuleName As String = "eMerge Module"
        Dim PriorityLicence As String = "xD2SQ7e/EoA260775076d617a10174c422c4871150f49730f662a4439061d"

        Dim c As Integer = CheckVal(PriorityLicence, ModuleName)

        Dim k As String

        Do
            k = MakeKey(c)
            Console.WriteLine(k)
        Loop Until Not Validate(k, c)

    End Sub

    Private Function Validate(ByVal key As String, ByVal CheckVal As Integer) As Boolean
        Dim r As Integer = 0
        For i As Integer = 0 To key.Length - 1
            r += InStr(Numbers, key.Substring(i, 1)) - 1
        Next
        Return CBool(CheckVal = (r - 1))
    End Function

    Private Function MakeKey(ByVal CheckVal As Integer) As String

        Dim objRandom As New System.Random(CType(System.DateTime.Now.Ticks Mod System.Int32.MaxValue, Integer))

        Dim t As New List(Of ch)
        For i As Integer = 0 To 9
            t.Add(New ch)
        Next

        For i As Integer = 0 To CheckVal
            Dim r As Integer
            Do
                r = objRandom.Next(t.Count)
            Loop Until (t(r).Increment())
        Next

        Dim str As New Text.StringBuilder
        For Each Val As ch In t
            str.Append(Val.Value)
        Next
        Return str.ToString

    End Function

    Private Function CheckVal(ByVal PriorityLicence As String, ByVal ModuleName As String) As Integer

        Dim c As Integer = 0
        For i As Integer = 0 To PriorityLicence.Length - 1
            If IsNumeric(PriorityLicence.Substring(i, 1)) Then
                c += CInt(PriorityLicence.Substring(i, 1))
            End If
        Next
        For i As Integer = 0 To ModuleName.Length - 1
            c += CInt(Right(Asc(ModuleName.Substring(i, 1)).ToString, 1))
        Next

        Return c

    End Function

End Module
