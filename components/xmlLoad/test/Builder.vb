Public Class Builder
    Private Build As System.Text.StringBuilder
    Private LastLen As Integer = 0
    Private _ConnectionID As String = Nothing

    Sub New()
        Build = New System.Text.StringBuilder
    End Sub

    Sub New(ByVal ConnectionID As String)
        _ConnectionID = ConnectionID
        Build = New System.Text.StringBuilder
    End Sub    

    Public Property Length() As Integer
        Get
            Return Build.Length
        End Get
        Set(ByVal value As Integer)
            Build.Length = value
        End Set
    End Property

    Public Property Capacity() As Integer
        Get
            Return Build.Capacity
        End Get
        Set(ByVal value As Integer)
            Build.Capacity = value
        End Set
    End Property

    Public ReadOnly Property MaxCapacity() As Integer
        Get
            Return Build.MaxCapacity
        End Get
    End Property

    Public Property Chars(ByVal i As Integer)
        Get
            Return Build.Chars(i)
        End Get
        Set(ByVal value)
            Build.Chars(i) = value
        End Set
    End Property

    Public Function Append(ByVal Str As String) As Builder
        Build.Append(Str)
        Return Me
    End Function

    Public Function AppendFormat(ByVal Str As String, ByVal ParamArray Args() As Object) As Builder
        Build.AppendFormat(Str, Args)
        Return Me
    End Function

    Public Function AppendLine() As Builder
        Build.AppendLine()
        If IsNothing(_ConnectionID) Then
            Console.Write(Build.ToString.Substring(LastLen, Build.Length - LastLen))
        Else
            svr.Send(_ConnectionID, Build.ToString.Substring(LastLen, Build.Length - LastLen).Replace(vbCrLf, ""))
        End If
        LastLen = Build.Length
        Return Me
    End Function

    Public Overloads Sub Replace(ByVal OldValue As String, ByVal NewValue As String)
        Build.Replace(OldValue, NewValue)
    End Sub

    Public Overloads Sub Replace(ByVal OldValue As String, ByVal NewValue As String, ByVal StartIndex As Integer, ByVal Count As Integer)
        Build.Replace(OldValue, NewValue, StartIndex, Count)
    End Sub

    Public Overloads Sub Replace(ByVal OldChar As Char, ByVal NewChar As Char)
        Build.Replace(OldChar, NewChar)
    End Sub

    Public Overloads Sub Replace(ByVal OldChar As Char, ByVal NewChar As Char, ByVal StartIndex As Integer, ByVal Count As Integer)
        Build.Replace(OldChar, NewChar, StartIndex, Count)
    End Sub

    Public Sub Clear()
        Build = New System.Text.StringBuilder
    End Sub

    Public Overrides Function ToString() As String
        Return Build.ToString
    End Function

End Class
