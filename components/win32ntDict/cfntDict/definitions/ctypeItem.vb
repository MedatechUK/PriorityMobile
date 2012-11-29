' CUSTTYPE,CTYPECODE,CTYPENAME
Public Class cTypeItem
    Inherits DatasetObjectBase
#Region "Private Variables"
    Private _CUSTTYPE As String
    Private _CTYPECODE As String
    Private _CTYPENAME As String
#End Region
#Region "Initialisation"
    Public Sub New()
        MyBase.New()
    End Sub
    Public Sub New(ByVal CUSTTYPE As String, ByVal CTYPECODE As String, ByVal CTYPENAME As String)
        MyBase.New()
        _CUSTTYPE = CUSTTYPE
        _CTYPECODE = CTYPECODE
        _CTYPENAME = CTYPENAME
    End Sub
#End Region
#Region "Public Properties"
    Public Property CUSTTYPE() As String
        Get
            Return _CUSTTYPE
        End Get
        Set(ByVal value As String)
            If _CUSTTYPE <> value Then
                _CUSTTYPE = value
                OnPropertyChanged("CUSTTYPE")
            End If
        End Set
    End Property
    Public Property CTYPECODE() As String
        Get
            Return _CTYPECODE
        End Get
        Set(ByVal value As String)
            If _CTYPECODE <> value Then
                _CTYPECODE = value
                OnPropertyChanged("CTYPECODE")
            End If
        End Set
    End Property
    Public Property CTYPENAME() As String
        Get
            Return _CTYPENAME
        End Get
        Set(ByVal value As String)
            If _CTYPENAME <> value Then
                _CTYPENAME = value
                OnPropertyChanged("CTYPENAME")
            End If
        End Set
    End Property
#End Region
End Class

