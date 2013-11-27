Public Class byteMsg : Inherits System.EventArgs

    Sub New(ByVal ProtocolType As eProtocolType, ByVal Message As Byte())
        _msgID = Guid.NewGuid.ToString
        _ProtocolType = ProtocolType
        _msg = Message
    End Sub

    Private _msgID As String
    Public ReadOnly Property msgID() As String
        Get
            Return _msgID
        End Get
    End Property

    Private _ProtocolType As eProtocolType
    Public Property ProtocolType() As eProtocolType
        Get
            Return _ProtocolType
        End Get
        Set(ByVal value As eProtocolType)
            _ProtocolType = value
        End Set
    End Property

    Private _msg As Byte() = Nothing
    Public Property Message() As Byte()
        Get
            Return _msg
        End Get
        Set(ByVal value As Byte())
            _msg = value
        End Set
    End Property

End Class

