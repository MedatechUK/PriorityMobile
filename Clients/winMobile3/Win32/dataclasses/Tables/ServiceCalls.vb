Imports Bind
Public Class ServiceCalls
    Inherits DatasetObjectBase
#Region "Private Variables"
    Private _DOCNO As String
    Private _DATEOPENED As String
    Private _PDATE As Integer
    Private _USERLOGIN As String
    Private _STATUS As String
    Private _CUSTOMERNAME As String
    Private _ADDRESS2 As String
    Private _ADDRESS21 As String
    Private _ADDRESS3 As String
    Private _POSTCODE As String
    Private _CITYCOUNTY As String
    Private _NAME As String
    Private _PHONENUMBER As String
    Private _SERVTDES As String
    Private _PTIME As Integer
    Private _MALFCODE As String
    Private _RESCODE As String
#End Region
#Region "Initialisation"
    Public Sub New()
        MyBase.New()
    End Sub
    Public Sub New(ByVal DOCNO As String, ByVal DATEOPENED As String, ByVal PDATE As Integer, ByVal USERLOGIN As String, ByVal STATUS As String, ByVal CUSTOMERNAME As String, ByVal ADDRESS2 As String, ByVal ADDRESS21 As String, ByVal ADDRESS3 As String, ByVal POSTCODE As String, ByVal CITYCOUNTY As String, ByVal NAME As String, ByVal PHONENUMBER As String, ByVal SERVTDES As String, ByVal PTIME As Integer, ByVal MALFCODE As String, ByVal RESCODE As String)
        MyBase.New()
        _DOCNO = DOCNO
        _DATEOPENED = DATEOPENED
        _PDATE = PDATE
        _USERLOGIN = USERLOGIN
        _STATUS = STATUS
        _CUSTOMERNAME = CUSTOMERNAME
        _ADDRESS2 = ADDRESS2
        _ADDRESS21 = ADDRESS21
        _ADDRESS3 = ADDRESS3
        _POSTCODE = POSTCODE
        _CITYCOUNTY = CITYCOUNTY
        _NAME = NAME
        _PHONENUMBER = PHONENUMBER
        _SERVTDES = SERVTDES
        _PTIME = PTIME
        _MALFCODE = MALFCODE
        _RESCODE = RESCODE
    End Sub
#End Region
#Region "Overrides"
    Public Overrides Function ConQuery() As String
        ' The SQL statement to populate the dataset from.
        ' Return Nothing if the dataset is not updated from the SOAP service.
        Return _
        "SELECT [DOCNO] AS DOCNO, " & _
        "[Date Opened] AS DATEOPENED, " & _
        "[PDATE] AS PDATE, " & _
        "[USERLOGIN] AS USERLOGIN, " & _
        "[Status] AS STATUS, " & _
        "[Customer Name] AS CUSTOMERNAME, " & _
        "[Address 2] AS ADDRESS2, " & _
        "[Address 2.1] AS ADDRESS21, " & _
        "[Address 3] AS ADDRESS3, " & _
        "[Post Code] AS POSTCODE, " & _
        "[City/County] AS CITYCOUNTY, " & _
        "[NAME] AS NAME, " & _
        "[Phone Number] AS PHONENUMBER, " & _
        "[SERVTDES] AS SERVTDES, " & _
        "[PTIME] AS PTIME , " & _
        "MALFCODE, " & _
        "RESCODE " & _
        "FROM V_SERVCALL  " & _
        "WHERE USERLOGIN = '%username%' "
    End Function
    Public Overrides Function Columns() As String()
        ' List of columns in the table
        Dim myCols() As String = { _
        "DOCNO", "DATEOPENED", "PDATE", "USERLOGIN", "STATUS", "CUSTOMERNAME", "ADDRESS2", "ADDRESS21", "ADDRESS3", "POSTCODE", "CITYCOUNTY", "NAME", "PHONENUMBER", "SERVTDES", "PTIME", "MALFCODE", "RESCODE" _
        }
        Return myCols
    End Function
    Public Overrides Function KeyColumns() As String()
        ' List of colums that combine to create a unique key
        Dim myCols() As String = { _
        "DOCNO" _
        }
        Return myCols
    End Function
    Public Overrides Function UpdateColumns() As String()
        ' List of columns updated by a SOAP Syncronisation.
        Dim myCols() As String = { _
        "DOCNO", "DATEOPENED", "PDATE", "USERLOGIN", "CUSTOMERNAME", "ADDRESS2", "ADDRESS21", "ADDRESS3", "POSTCODE", "CITYCOUNTY", "NAME", "PHONENUMBER", "SERVTDES", "PTIME" _
        }
        Return myCols
    End Function
#End Region
#Region "Public Properties"
    Public Property DOCNO() As String
        Get
            Return _DOCNO
        End Get
        Set(ByVal value As String)
            If _DOCNO <> value Then
                OnPropertyChanging("DOCNO", value)
                If Not GetCancelEdit() Then
                    _DOCNO = value
                    OnPropertyChanged("DOCNO")
                End If
            End If
        End Set
    End Property
    Public Property DATEOPENED() As String
        Get
            Return _DATEOPENED
        End Get
        Set(ByVal value As String)
            If _DATEOPENED <> value Then
                OnPropertyChanging("DATEOPENED", value)
                If Not GetCancelEdit() Then
                    _DATEOPENED = value
                    OnPropertyChanged("DATEOPENED")
                End If
            End If
        End Set
    End Property
    Public Property PDATE() As Integer
        Get
            Return _PDATE
        End Get
        Set(ByVal value As Integer)
            If _PDATE <> value Then
                OnPropertyChanging("PDATE", value)
                If Not GetCancelEdit() Then
                    _PDATE = value
                    OnPropertyChanged("PDATE")
                End If
            End If
        End Set
    End Property
    Public Property USERLOGIN() As String
        Get
            Return _USERLOGIN
        End Get
        Set(ByVal value As String)
            If _USERLOGIN <> value Then
                OnPropertyChanging("USERLOGIN", value)
                If Not GetCancelEdit() Then
                    _USERLOGIN = value
                    OnPropertyChanged("USERLOGIN")
                End If
            End If
        End Set
    End Property
    Public Property STATUS() As String
        Get
            Return _STATUS
        End Get
        Set(ByVal value As String)
            If _STATUS <> value Then
                OnPropertyChanging("STATUS", value)
                If Not GetCancelEdit() Then
                    _STATUS = value
                    OnPropertyChanged("STATUS")
                End If
            End If
        End Set
    End Property
    Public Property CUSTOMERNAME() As String
        Get
            Return _CUSTOMERNAME
        End Get
        Set(ByVal value As String)
            If _CUSTOMERNAME <> value Then
                OnPropertyChanging("CUSTOMERNAME", value)
                If Not GetCancelEdit() Then
                    _CUSTOMERNAME = value
                    OnPropertyChanged("CUSTOMERNAME")
                End If
            End If
        End Set
    End Property
    Public Property ADDRESS2() As String
        Get
            Return _ADDRESS2
        End Get
        Set(ByVal value As String)
            If _ADDRESS2 <> value Then
                OnPropertyChanging("ADDRESS2", value)
                If Not GetCancelEdit() Then
                    _ADDRESS2 = value
                    OnPropertyChanged("ADDRESS2")
                End If
            End If
        End Set
    End Property
    Public Property ADDRESS21() As String
        Get
            Return _ADDRESS21
        End Get
        Set(ByVal value As String)
            If _ADDRESS21 <> value Then
                OnPropertyChanging("ADDRESS21", value)
                If Not GetCancelEdit() Then
                    _ADDRESS21 = value
                    OnPropertyChanged("ADDRESS21")
                End If
            End If
        End Set
    End Property
    Public Property ADDRESS3() As String
        Get
            Return _ADDRESS3
        End Get
        Set(ByVal value As String)
            If _ADDRESS3 <> value Then
                OnPropertyChanging("ADDRESS3", value)
                If Not GetCancelEdit() Then
                    _ADDRESS3 = value
                    OnPropertyChanged("ADDRESS3")
                End If
            End If
        End Set
    End Property
    Public Property POSTCODE() As String
        Get
            Return _POSTCODE
        End Get
        Set(ByVal value As String)
            If _POSTCODE <> value Then
                OnPropertyChanging("POSTCODE", value)
                If Not GetCancelEdit() Then
                    _POSTCODE = value
                    OnPropertyChanged("POSTCODE")
                End If
            End If
        End Set
    End Property
    Public Property CITYCOUNTY() As String
        Get
            Return _CITYCOUNTY
        End Get
        Set(ByVal value As String)
            If _CITYCOUNTY <> value Then
                OnPropertyChanging("CITYCOUNTY", value)
                If Not GetCancelEdit() Then
                    _CITYCOUNTY = value
                    OnPropertyChanged("CITYCOUNTY")
                End If
            End If
        End Set
    End Property
    Public Property NAME() As String
        Get
            Return _NAME
        End Get
        Set(ByVal value As String)
            If _NAME <> value Then
                OnPropertyChanging("NAME", value)
                If Not GetCancelEdit() Then
                    _NAME = value
                    OnPropertyChanged("NAME")
                End If
            End If
        End Set
    End Property
    Public Property PHONENUMBER() As String
        Get
            Return _PHONENUMBER
        End Get
        Set(ByVal value As String)
            If _PHONENUMBER <> value Then
                OnPropertyChanging("PHONENUMBER", value)
                If Not GetCancelEdit() Then
                    _PHONENUMBER = value
                    OnPropertyChanged("PHONENUMBER")
                End If
            End If
        End Set
    End Property
    Public Property SERVTDES() As String
        Get
            Return _SERVTDES
        End Get
        Set(ByVal value As String)
            If _SERVTDES <> value Then
                OnPropertyChanging("SERVTDES", value)
                If Not GetCancelEdit() Then
                    _SERVTDES = value
                    OnPropertyChanged("SERVTDES")
                End If
            End If
        End Set
    End Property
    Public Property PTIME() As Integer
        Get
            Return _PTIME
        End Get
        Set(ByVal value As Integer)
            If _PTIME <> value Then
                OnPropertyChanging("PTIME", value)
                If Not GetCancelEdit() Then
                    _PTIME = value
                    OnPropertyChanged("PTIME")
                End If
            End If
        End Set
    End Property
    Public Property MALFCODE() As String
        Get
            Return _MALFCODE
        End Get
        Set(ByVal value As String)
            If _MALFCODE <> value Then
                OnPropertyChanging("MALFCODE", value)
                If Not GetCancelEdit() Then
                    _MALFCODE = value
                    OnPropertyChanged("MALFCODE")
                End If
            End If
        End Set
    End Property
    Public Property RESCODE() As String
        Get
            Return _RESCODE
        End Get
        Set(ByVal value As String)
            If _RESCODE <> value Then
                OnPropertyChanging("RESCODE", value)
                If Not GetCancelEdit() Then
                    _RESCODE = value
                    OnPropertyChanged("RESCODE")
                End If
            End If
        End Set
    End Property
#End Region
End Class