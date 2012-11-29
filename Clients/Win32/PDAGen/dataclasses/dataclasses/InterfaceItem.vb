Imports Bind
Public Class InterfaceItem
    Inherits DatasetObjectBase
#Region "Private Variables"
    Private _FullPath As String
    Private _Name As String
    Private _table As String
    Private _Column As String    
    Private _FieldStyle As String
    Private _IsReadOnly As String
    Private _Hidden As String
    Private _Mandatory As String
    Private _ListSource As String
    Private _ListTextCol As String
    Private _ListValueCol As String
    Private _ListFilter As String
    Private _REGEX As String
    Private _ORD As Integer
#End Region
#Region "Initialisation"
    Public Sub New()
        MyBase.New()
    End Sub
    Public Sub New(ByVal FullPath As String, ByVal Name As String, ByVal table As String, ByVal Column As String, ByVal FieldStyle As String, ByVal IsReadOnly As String, ByVal Hidden As String, ByVal Mandatory As String, ByVal ListSource As String, ByVal ListTextCol As String, ByVal ListValueCol As String, ByVal ListFilter As String, ByVal REGEX As String, ByVal ORD As Integer)
        MyBase.New()
        _FullPath = FullPath
        _Name = Name
        _table = table
        _Column = Column        
        _FieldStyle = FieldStyle
        _IsReadOnly = IsReadOnly
        _Hidden = Hidden
        _Mandatory = Mandatory
        _ListSource = ListSource
        _ListTextCol = ListTextCol
        _ListValueCol = ListValueCol
        _ListFilter = ListFilter
        _REGEX = REGEX
        _ORD = ORD
    End Sub
#End Region
#Region "Overrides"
    Public Overrides Function ConQuery() As String
        ' The SQL statement to populate the dataset from.
        ' Return Nothing if the dataset is not updated from the SOAP service.
        Return Nothing
    End Function
    Public Overrides Function Columns() As String()
        ' List of columns in the table
        Dim myCols() As String = { _
        "FullPath", "Name", "table", "Column", "FieldStyle", "IsReadOnly", "Hidden", "Mandatory", "ListSource", "ListTextCol", "ListValueCol", "ListFilter", "REGEX", "ORD" _
        }
        Return myCols
    End Function
    Public Overrides Function KeyColumns() As String()
        ' List of colums that combine to create a unique key
        Dim myCols() As String = { _
        "FullPath", "Name" _
        }
        Return myCols
    End Function
    Public Overrides Function UpdateColumns() As String()
        ' List of columns updated by a SOAP Syncronisation.
        Dim myCols() As String = { _
        "FullPath", "Name", "table", "Column", "FieldStyle", "IsReadOnly", "Hidden", "Mandatory", "ListSource", "ListTextCol", "ListValueCol", "ListFilter", "REGEX", "ORD" _
        }
        Return myCols
    End Function
#End Region
#Region "Public Properties"
    Public Property FullPath() As String
        Get
            Return _FullPath
        End Get
        Set(ByVal value As String)
            If _FullPath <> value Then
                OnPropertyChanging("FullPath", value)
                If Not GetCancelEdit() Then
                    _FullPath = value
                    OnPropertyChanged("FullPath")
                End If
            End If
        End Set
    End Property
    Public Property Name() As String
        Get
            Return _Name
        End Get
        Set(ByVal value As String)
            If _Name <> value Then
                OnPropertyChanging("Name", value)
                If Not GetCancelEdit() Then
                    _Name = value
                    OnPropertyChanged("Name")
                End If
            End If
        End Set
    End Property
    Public Property table() As String
        Get
            Return _table
        End Get
        Set(ByVal value As String)
            If _table <> value Then
                OnPropertyChanging("table", value)
                If Not GetCancelEdit() Then
                    _table = value
                    OnPropertyChanged("table")
                End If
            End If
        End Set
    End Property
    Public Property Column() As String
        Get
            Return _Column
        End Get
        Set(ByVal value As String)
            If _Column <> value Then
                OnPropertyChanging("Column", value)
                If Not GetCancelEdit() Then
                    _Column = value
                    OnPropertyChanged("Column")
                End If
            End If
        End Set
    End Property
    Public Property FieldStyle() As String
        Get
            Return _FieldStyle
        End Get
        Set(ByVal value As String)
            If _FieldStyle <> value Then
                OnPropertyChanging("FieldStyle", value)
                If Not GetCancelEdit() Then
                    _FieldStyle = value
                    OnPropertyChanged("FieldStyle")
                End If
            End If
        End Set
    End Property
    Public Property IsReadOnly() As String
        Get
            Return _IsReadOnly
        End Get
        Set(ByVal value As String)
            If _IsReadOnly <> value Then
                OnPropertyChanging("IsReadOnly", value)
                If Not GetCancelEdit() Then
                    _IsReadOnly = value
                    OnPropertyChanged("IsReadOnly")
                End If
            End If
        End Set
    End Property
    Public Property Hidden() As String
        Get
            Return _Hidden
        End Get
        Set(ByVal value As String)
            If _Hidden <> value Then
                OnPropertyChanging("Hidden", value)
                If Not GetCancelEdit() Then
                    _Hidden = value
                    OnPropertyChanged("Hidden")
                End If
            End If
        End Set
    End Property
    Public Property Mandatory() As String
        Get
            Return _Mandatory
        End Get
        Set(ByVal value As String)
            If _Mandatory <> value Then
                OnPropertyChanging("Mandatory", value)
                If Not GetCancelEdit() Then
                    _Mandatory = value
                    OnPropertyChanged("Mandatory")
                End If
            End If
        End Set
    End Property
    Public Property ListSource() As String
        Get
            Return _ListSource
        End Get
        Set(ByVal value As String)
            If _ListSource <> value Then
                OnPropertyChanging("ListSource", value)
                If Not GetCancelEdit() Then
                    _ListSource = value
                    OnPropertyChanged("ListSource")
                End If
            End If
        End Set
    End Property
    Public Property ListTextCol() As String
        Get
            Return _ListTextCol
        End Get
        Set(ByVal value As String)
            If _ListTextCol <> value Then
                OnPropertyChanging("ListTextCol", value)
                If Not GetCancelEdit() Then
                    _ListTextCol = value
                    OnPropertyChanged("ListTextCol")
                End If
            End If
        End Set
    End Property
    Public Property ListValueCol() As String
        Get
            Return _ListValueCol
        End Get
        Set(ByVal value As String)
            If _ListValueCol <> value Then
                OnPropertyChanging("ListValueCol", value)
                If Not GetCancelEdit() Then
                    _ListValueCol = value
                    OnPropertyChanged("ListValueCol")
                End If
            End If
        End Set
    End Property
    Public Property ListFilter() As String
        Get
            Return _ListFilter
        End Get
        Set(ByVal value As String)
            If _ListFilter <> value Then
                OnPropertyChanging("ListFilter", value)
                If Not GetCancelEdit() Then
                    _ListFilter = value
                    OnPropertyChanged("ListFilter")
                End If
            End If
        End Set
    End Property
    Public Property REGEX() As String
        Get
            Return _REGEX
        End Get
        Set(ByVal value As String)
            If _REGEX <> value Then
                OnPropertyChanging("REGEX", value)
                If Not GetCancelEdit() Then
                    _REGEX = value
                    OnPropertyChanged("REGEX")
                End If
            End If
        End Set
    End Property
    Public Property ORD() As Integer
        Get
            Return _ORD
        End Get
        Set(ByVal value As Integer)
            If _ORD <> value Then
                OnPropertyChanging("ORD", value)
                If Not GetCancelEdit() Then
                    _ORD = value
                    OnPropertyChanged("ORD")
                End If
            End If
        End Set
    End Property
#End Region
End Class