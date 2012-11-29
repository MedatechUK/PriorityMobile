' BUBBLEID,CALLER,SOAPPROC,RESULT,DATA
Public Class eventItem
    Inherits DatasetObjectBase
#Region "Private Variables"
    Private _BUBBLEID As String
    Private _CALLER As String
    Private _SOAPPROC As String
    Private _RESULT As String
    Private _DATA As String
#End Region
#Region "Initialisation"
    Public Sub New()
        MyBase.New()
    End Sub
    Public Sub New(ByVal BUBBLEID As String, ByVal CALLER As String, ByVal SOAPPROC As String, ByVal RESULT As String, ByVal DATA As String)
        MyBase.New()
        _BUBBLEID = BUBBLEID
        _CALLER = CALLER
        _SOAPPROC = SOAPPROC
        _RESULT = RESULT
        _DATA = DATA
    End Sub
#End Region
#Region "Public Properties"
    Public Property BUBBLEID() As String
        Get
            Return _BUBBLEID
        End Get
        Set(ByVal value As String)
            If _BUBBLEID <> value Then
                _BUBBLEID = value
                OnPropertyChanged("BUBBLEID")
            End If
        End Set
    End Property
    Public Property CALLER() As String
        Get
            Return _CALLER
        End Get
        Set(ByVal value As String)
            If _CALLER <> value Then
                _CALLER = value
                OnPropertyChanged("CALLER")
            End If
        End Set
    End Property
    Public Property SOAPPROC() As String
        Get
            Return _SOAPPROC
        End Get
        Set(ByVal value As String)
            If _SOAPPROC <> value Then
                _SOAPPROC = value
                OnPropertyChanged("SOAPPROC")
            End If
        End Set
    End Property
    Public Property RESULT() As String
        Get
            Return _RESULT
        End Get
        Set(ByVal value As String)
            If _RESULT <> value Then
                _RESULT = value
                OnPropertyChanged("RESULT")
            End If
        End Set
    End Property
    Public Property DATA() As String
        Get
            Return _DATA
        End Get
        Set(ByVal value As String)
            If _DATA <> value Then
                _DATA = value
                OnPropertyChanged("DATA")
            End If
        End Set
    End Property
#End Region
End Class
