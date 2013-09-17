Imports System.Text.RegularExpressions

Module regexLib

    Public rxColumn As Regex = New Regex("\:\$+\.[0-9A-Za-z\-_]+", RegexOptions.IgnoreCase)
    Public rxUpperColumn As Regex = New Regex("\:\$\$\.[0-9A-Za-z\-_]+", RegexOptions.IgnoreCase)
    Public rxArg As Regex = New Regex("\:[0-9A-Za-z\-_]+", RegexOptions.IgnoreCase)
    Public rxPar As Regex = New Regex("\:PAR[0-9]+", RegexOptions.IgnoreCase)    
    Public rxColPar As Regex = New Regex("\:\$+\.[0-9A-Za-z\-_]+|\:[0-9A-Za-z\-_]+", RegexOptions.IgnoreCase)
    Public rxArgs As Regex = New Regex("%[a-zA-Z]+%", RegexOptions.IgnoreCase)
    Public rxSelectInto As Regex = New Regex("SELECT.*INTO", RegexOptions.IgnoreCase + RegexOptions.Multiline)
    Public rxIntoFrom As Regex = New Regex("INTO.*FROM", RegexOptions.IgnoreCase + RegexOptions.Multiline)
    Public rxWhere As Regex = New Regex("WHERE.*;", RegexOptions.IgnoreCase + RegexOptions.Multiline)
    Public rxErrMsg As Regex = New Regex("ERRMSG [0-9]+", RegexOptions.IgnoreCase + RegexOptions.Multiline)
    Public rxMAC As Regex = New Regex("^([0-9A-F]{2}){5}([0-9A-F]{2})$", RegexOptions.IgnoreCase)

    Public rxINT As Regex = New Regex("^[\-]?[0-9]+$", RegexOptions.IgnoreCase)
    Public rxREAL As Regex = New Regex("^[\-]?[0-9]+[\.]?[0-9]*$", RegexOptions.IgnoreCase)

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

    Public Function rxIsPattern(ByVal Pattern As Regex, ByRef SearchString As String) As Boolean
        Return Pattern.IsMatch(SearchString)
    End Function

End Module
