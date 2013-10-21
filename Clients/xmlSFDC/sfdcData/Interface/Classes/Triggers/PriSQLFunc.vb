Imports System.Text.RegularExpressions

Public Class PriSQLFunc

    Private xParam As New Dictionary(Of String, Regex)

#Region "Private Properties"

    Private _PrioritySyntax As String
    Private Property PrioritySyntax() As String
        Get
            Return _PrioritySyntax
        End Get
        Set(ByVal value As String)
            _PrioritySyntax = value
        End Set
    End Property

    Private _MSSyntax As String
    Private Property MSSyntax() As String
        Get
            Return _MSSyntax
        End Get
        Set(ByVal value As String)
            _MSSyntax = value
        End Set
    End Property

    Private _Expression As Regex
    Private ReadOnly Property Expression() As Regex
        Get
            Return _Expression
        End Get        
    End Property

#End Region

#Region "Initialisation and Finalisation"

    Public Sub New(ByVal PrioritySyntax As String, ByVal MSSyntax As String)
        ' "SQL.DATE8(%1)" "dbo.DATETOMIN8(%1)"

        Dim pattern As String = PrioritySyntax.Replace("(", "\(").Replace(")", "\)").Replace(".", "\.").Replace(Chr(32), "\s*").Replace("=", "\=").Replace("?", "\?")
        _PrioritySyntax = PrioritySyntax
        _MSSyntax = MSSyntax
        _Expression = New Regex(rxReplace(rxSyntaxParam, pattern, ".*"))

        For Each m As String In rxMatch(rxSyntaxParam, PrioritySyntax)
            Dim l As String = rxReplace(rxSyntaxParam, Split(pattern, m)(0), ".*")
            Dim r As String = rxReplace(rxSyntaxParam, Split(pattern, m)(1), ".*")
            xParam.Add(m, New Regex(String.Format("(?<={0}).*(?={1})", l, r)))
        Next

    End Sub

#End Region

#Region "Public Methods"

    Public Sub Parse(ByRef SQL As String)

        For Each m As String In rxMatch(Me.Expression, SQL)
            Dim repl As String = MSSyntax            
            For Each k As String In xParam.Keys
                For Each p As String In rxMatch(xParam(k), m)
                    repl = repl.Replace(k, p)
                Next
            Next
            SQL = SQL.Replace(m, repl)
        Next

    End Sub

#End Region

End Class
