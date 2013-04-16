Public MustInherit Class BaseRedirect

    Private _PostValues As New Dictionary(Of String, String)
    Public Property PostValues() As Dictionary(Of String, String)
        Get
            Return _PostValues
        End Get
        Set(ByVal value As Dictionary(Of String, String))
            _PostValues = value
        End Set
    End Property

    Public MustOverride ReadOnly Property PostURL() As String

End Class
