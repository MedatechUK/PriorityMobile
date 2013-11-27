Public MustInherit Class socketListener : Inherits Protocol

    Private _msgHandler As System.EventHandler(Of byteMsg)
    Public Property msgHandler() As System.EventHandler(Of byteMsg)
        Get
            Return _msgHandler
        End Get
        Set(ByVal value As System.EventHandler(Of byteMsg))
            _msgHandler = value
        End Set
    End Property

End Class
