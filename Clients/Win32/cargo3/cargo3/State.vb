Public Class State

    Private _Ord As Integer = -1
    Public ReadOnly Property NextOrd() As Integer
        Get
            _Ord += 1
            Return _Ord
        End Get
    End Property

    Public Sub New(ByVal Name As String, Optional ByVal DefaultState As Boolean = False)
        _Name = Name
        _DefaultState = DefaultState
    End Sub

    Private _Name As String = ""
    Public ReadOnly Property Name() As String
        Get
            Return _Name
        End Get
    End Property

    Private _DefaultState As Boolean = False
    Public Property DefaultState() As Boolean
        Get
            Return _DefaultState
        End Get
        Set(ByVal value As Boolean)
            _DefaultState = value
        End Set
    End Property

    Private _Actions As New Dictionary(Of String, Action)
    Public Property Actions() As Dictionary(Of String, Action)
        Get
            Return _Actions
        End Get
        Set(ByVal value As Dictionary(Of String, Action))
            _Actions = value
        End Set
    End Property

End Class
