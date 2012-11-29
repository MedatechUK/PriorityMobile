' CUST,CUSTNAME,CUSTDES,CUSTTYPE,CREATEDDATE
Public Class CustomerItem
    Inherits DatasetObjectBase
#Region "Private Variables"
    Private _CUST As String
    Private _CUSTNAME As String
    Private _CUSTDES As String
    Private _CUSTTYPE As String
    Private _CREATEDDATE As String
#End Region
#Region "Initialisation"
    Public Sub New()
        MyBase.New()
    End Sub
    Public Sub New(ByVal CUST As String, ByVal CUSTNAME As String, ByVal CUSTDES As String, ByVal CUSTTYPE As String, ByVal CREATEDDATE As String)
        MyBase.New()
        _CUST = CUST
        _CUSTNAME = CUSTNAME
        _CUSTDES = CUSTDES
        _CUSTTYPE = CUSTTYPE
        _CREATEDDATE = CREATEDDATE
    End Sub
#End Region
#Region "Public Properties"
    Public Property CUST() As String
        Get
            Return _CUST
        End Get
        Set(ByVal value As String)
            If _CUST <> value Then
                _CUST = value
                OnPropertyChanged("CUST")
            End If
        End Set
    End Property
    Public Property CUSTNAME() As String
        Get
            Return _CUSTNAME
        End Get
        Set(ByVal value As String)
            If _CUSTNAME <> value Then
                _CUSTNAME = value
                OnPropertyChanged("CUSTNAME")
            End If
        End Set
    End Property
    Public Property CUSTDES() As String
        Get
            Return _CUSTDES
        End Get
        Set(ByVal value As String)
            If _CUSTDES <> value Then
                _CUSTDES = value
                OnPropertyChanged("CUSTDES")
            End If
        End Set
    End Property
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
    Public Property CREATEDDATE() As String
        Get
            Return _CREATEDDATE
        End Get
        Set(ByVal value As String)
            If _CREATEDDATE <> value Then
                _CREATEDDATE = value
                OnPropertyChanged("CREATEDDATE")
            End If
        End Set
    End Property
#End Region
End Class
