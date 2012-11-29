' PHONE,CUST,TITLEDES,NAME,PHONENUM,EMAIL
Public Class PhoneBookItem
    Inherits DatasetObjectBase
#Region "Private Variables"
    Private _PHONE As String = ""
    Private _CUST As String = ""
    Private _TITLEDES As String = ""
    Private _NAME As String = ""
    Private _PHONENUM As String = ""
    Private _EMAIL As String = ""
#End Region
#Region "Initialisation"
    Public Sub New()
        MyBase.New()
    End Sub
    Public Sub New(ByVal PHONE As String, ByVal CUST As String, ByVal TITLEDES As String, ByVal NAME As String, ByVal PHONENUM As String, ByVal EMAIL As String)
        MyBase.New()
        _PHONE = PHONE
        _CUST = CUST
        _TITLEDES = TITLEDES
        _NAME = NAME
        _PHONENUM = PHONENUM
        _EMAIL = EMAIL
    End Sub
#End Region
#Region "Public Properties"
    Public Property PHONE() As String
        Get
            Return _PHONE
        End Get
        Set(ByVal value As String)
            If _PHONE <> value Then
                _PHONE = value
                OnPropertyChanged("PHONE")
            End If
        End Set
    End Property
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
    Public Property TITLEDES() As String
        Get
            Return _TITLEDES
        End Get
        Set(ByVal value As String)
            If _TITLEDES <> value Then
                _TITLEDES = value
                OnPropertyChanged("TITLEDES")
            End If
        End Set
    End Property
    Public Property NAME() As String
        Get
            Return _NAME
        End Get
        Set(ByVal value As String)
            If _NAME <> value Then
                _NAME = value
                OnPropertyChanged("NAME")
            End If
        End Set
    End Property
    Public Property PHONENUM() As String
        Get
            Return _PHONENUM
        End Get
        Set(ByVal value As String)
            If _PHONENUM <> value Then
                _PHONENUM = value
                OnPropertyChanged("PHONENUM")
            End If
        End Set
    End Property
    Public Property EMAIL() As String
        Get            
            Return _EMAIL
        End Get
        Set(ByVal value As String)
            If _EMAIL <> value Then
                _EMAIL = value
                OnPropertyChanged("EMAIL")
            End If
        End Set
    End Property
#End Region
End Class
