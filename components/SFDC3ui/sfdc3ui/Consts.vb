Imports System.Text.RegularExpressions

Module Consts

    Private _ValidationExp As New Dictionary(Of String, String)
    Public Property ValidationExp() As Dictionary(Of String, String)
        Get
            Return _ValidationExp
        End Get
        Set(ByVal value As Dictionary(Of String, String))
            _ValidationExp = value
        End Set
    End Property

End Module
