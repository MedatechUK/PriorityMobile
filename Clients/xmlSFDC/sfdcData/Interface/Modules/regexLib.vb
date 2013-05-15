Imports System.Text.RegularExpressions

Module regexLib

    Public rxColumn As Regex = New Regex("\:\$+\.[0-9A-Za-z]+", RegexOptions.IgnoreCase)
    Public rxArgs As Regex = New Regex("%[a-zA-Z]+%", RegexOptions.IgnoreCase)
    Public rxInsertInto As Regex = New Regex("INSERT.*INTO", RegexOptions.IgnoreCase + RegexOptions.Multiline)
    Public rxIntoFrom As Regex = New Regex("INTO.*FROM", RegexOptions.IgnoreCase + RegexOptions.Multiline)
    Public rxWhere As Regex = New Regex("WHERE.*;", RegexOptions.IgnoreCase + RegexOptions.Multiline)

    Public Function rxMatch(ByVal Pattern As Regex, ByRef SearchString As String) As List(Of String)
        Dim ret As New List(Of String)
        Dim M As Match = Pattern.Match(SearchString)
        Do While M.Success
            If Not ret.Contains(M.Value) Then
                ret.Add(M.Value)
            End If
            M = M.NextMatch
        Loop
        Return ret
    End Function

End Module
