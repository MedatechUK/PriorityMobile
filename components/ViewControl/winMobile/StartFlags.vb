Public Class pdaStartFlags

    Public Sub New()

    End Sub

    Public Sub New(ByVal ClearCache As Boolean, ByVal WipeData As Boolean, ByVal NoProvision As Boolean)
        _ClearCache = ClearCache
        _WipeData = WipeData
        _NoProvision = NoProvision
    End Sub

    Private _ClearCache As Boolean = False
    Public Property ClearCache() As Boolean
        Get
            Return _ClearCache
        End Get
        Set(ByVal value As Boolean)
            _ClearCache = value
        End Set
    End Property

    Private _WipeData As Boolean = False
    Public Property WipeData() As Boolean
        Get
            Return _WipeData
        End Get
        Set(ByVal value As Boolean)
            _WipeData = value
        End Set
    End Property

    Private _NoProvision As Boolean = False
    Public Property NoProvision() As Boolean
        Get
            Return _NoProvision
        End Get
        Set(ByVal value As Boolean)
            _NoProvision = value
        End Set
    End Property
End Class
