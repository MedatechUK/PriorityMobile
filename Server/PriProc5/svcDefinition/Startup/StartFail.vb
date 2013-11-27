Public Enum eSysFailCode
    FAIL_NO_HANDLER
    FAIL_ASSBY_LOAD
    FAIL_ASSBY_CTOR
    FAIL_TCP_LISTENER
    FAIL_UDP_LISTENER
End Enum

Public Class SystemFail : Inherits Exception

    Private _FailCode As eSysFailCode
    Public Property FailCode() As eSysFailCode
        Get
            Return _FailCode
        End Get
        Set(ByVal value As eSysFailCode)
            _FailCode = value
        End Set
    End Property

    Public Sub New(ByVal FailCode As eSysFailCode)
        _FailCode = FailCode
    End Sub 'New

    Public Sub New(ByVal FailCode As eSysFailCode, ByVal message As String)        
        MyBase.New(message)
        _FailCode = FailCode
    End Sub 'New

    Public Sub New(ByVal FailCode As eSysFailCode, ByVal message As String, ByVal inner As Exception)
        MyBase.New(message, inner)
        _FailCode = FailCode
    End Sub 'New

End Class
