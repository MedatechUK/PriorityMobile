Imports System.Text.RegularExpressions

Public Class Replacement

    Public Sub New(ByVal Find As String, ByVal Replace As String)
        _Find = Find
        _Replace = Replace
    End Sub

    Private _Find As String
    Public Property Find() As String
        Get
            Return _Find
        End Get
        Set(ByVal value As String)
            _Find = value
        End Set
    End Property

    Private _Replace As String
    Public Property Replace() As String
        Get
            Return _Replace
        End Get
        Set(ByVal value As String)
            _Replace = value
        End Set
    End Property

End Class

Public Class PriSQLFunc

    Private xParam As New Dictionary(Of String, Regex)

    Public Sub New(ByVal PrioritySyntax As String, ByVal MSSyntax As String)
        ' "SQL.DATE8(%1)" "dbo.DATETOMIN8(%1)"

        Dim pattern As String = PrioritySyntax.Replace("(", "\(").Replace(")", "\)").Replace(".", "\.").Replace(Chr(32), "\s?")
        Dim tmp As String = pattern
        For Each m As String In rxMatch(New Regex("%[0-9]+"), PrioritySyntax)
            xParam.Add(m, New Regex(String.Format("(?<={0})[a-zA-Z0-9]+", tmp.Split(m).First)))
            tmp = tmp.Split(m).Last
            pattern = pattern.Replace(m, "[a-zA-Z0-9]+")
        Next

        _Expression = New Regex(pattern, RegexOptions.IgnoreCase)
        _PrioritySyntax = PrioritySyntax
        _MSSyntax = MSSyntax
    End Sub

    Private _Expression As Regex
    Public ReadOnly Property Expression() As Regex
        Get
            Return _Expression
        End Get
    End Property

    Private _Matches As List(Of Replacement)
    Public ReadOnly Property Matches() As List(Of Replacement)
        Get
            Return _Matches
        End Get
    End Property

    Private _PrioritySyntax As String
    Public Property PrioritySyntax() As String
        Get
            Return _PrioritySyntax
        End Get
        Set(ByVal value As String)
            _PrioritySyntax = value
        End Set
    End Property

    Private _MSSyntax As String
    Public Property MSSyntax() As String
        Get
            Return _MSSyntax
        End Get
        Set(ByVal value As String)
            _MSSyntax = value
        End Set
    End Property

    Public Sub Parse(ByRef SQL As String)
        For Each str As String In rxMatch(_Expression, SQL)
            Dim repl As String = MSSyntax
            For Each k As String In xParam.Keys
                For Each m As String In rxMatch(xParam(k), str)
                    repl = repl.Replace(k, m)
                Next
            Next
            SQL = SQL.Replace(str, repl)            
        Next
    End Sub

End Class
