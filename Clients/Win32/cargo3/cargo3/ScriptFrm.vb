Public MustInherit Class ScriptFrm
    Inherits System.Windows.Forms.Form

    Private _ResultScript As String = String.Empty
    Public Property ResultScript() As String
        Get
            Return _ResultScript
        End Get
        Set(ByVal value As String)
            _ResultScript = value
        End Set
    End Property

    Private _ScriptName As String = Nothing
    Public Property ScriptName() As String
        Get
            Return _ScriptName
        End Get
        Set(ByVal value As String)
            _ScriptName = value
        End Set
    End Property

    Private _CloseOnEscape As Boolean = False
    Public Property CloseOnEscape() As Boolean
        Get
            Return _CloseOnEscape
        End Get
        Set(ByVal value As Boolean)
            _CloseOnEscape = value
        End Set
    End Property

End Class
