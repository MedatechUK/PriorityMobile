Imports Bind
Public Class ColumnItem
    Inherits DatasetObjectBase
#Region "Private Variables"
    Private _TABLE As String
    Private _COLNAME As String
    Private _COLTYPE As String
    Private _ORD As Integer
    Private _KEYCOL As String
    Private _UPDATECOL As String
#End Region
#Region "Initialisation"
    Public Sub New()
        MyBase.New()
    End Sub
    Public Sub New(ByVal TABLE As String, ByVal COLNAME As String, ByVal COLTYPE As String, ByVal ORD As Integer, ByVal KEYCOL As String, ByVal UPDATECOL As String)
        MyBase.New()
        _TABLE = TABLE
        _COLNAME = COLNAME
        _COLTYPE = COLTYPE
        _ORD = ORD
        _KEYCOL = KEYCOL
        _UPDATECOL = UPDATECOL
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
        "TABLE", "COLNAME", "COLTYPE", "ORD", "KEYCOL", "UPDATECOL" _
        }
        Return myCols
    End Function
    Public Overrides Function KeyColumns() As String()
        ' List of colums that combine to create a unique key
        Dim myCols() As String = { _
        "TABLE", "COLNAME" _
        }
        Return myCols
    End Function
    Public Overrides Function UpdateColumns() As String()
        ' List of columns updated by a SOAP Syncronisation.
        Dim myCols() As String = { _
        "TABLE", "COLNAME", "COLTYPE", "ORD", "KEYCOL", "UPDATECOL" _
        }
        Return myCols
    End Function
#End Region
#Region "Public Properties"
    Public Property TABLE() As String
        Get
            Return _TABLE
        End Get
        Set(ByVal value As String)
            If _TABLE <> value Then
                OnPropertyChanging("TABLE", value)
                If Not GetCancelEdit() Then
                    _TABLE = value
                    OnPropertyChanged("TABLE")
                End If
            End If
        End Set
    End Property
    Public Property COLNAME() As String
        Get
            Return _COLNAME
        End Get
        Set(ByVal value As String)
            If _COLNAME <> value Then
                OnPropertyChanging("COLNAME", value)
                If Not GetCancelEdit() Then
                    _COLNAME = value
                    OnPropertyChanged("COLNAME")
                End If
            End If
        End Set
    End Property
    Public Property COLTYPE() As String
        Get
            Return _COLTYPE
        End Get
        Set(ByVal value As String)
            If _COLTYPE <> value Then
                OnPropertyChanging("COLTYPE", value)
                If Not GetCancelEdit() Then
                    _COLTYPE = value
                    OnPropertyChanged("COLTYPE")
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
    Public Property KEYCOL() As String
        Get
            Return _KEYCOL
        End Get
        Set(ByVal value As String)
            If _KEYCOL <> value Then
                OnPropertyChanging("KEYCOL", value)
                If Not GetCancelEdit() Then
                    _KEYCOL = value
                    OnPropertyChanged("KEYCOL")
                End If
            End If
        End Set
    End Property
    Public Property UPDATECOL() As String
        Get
            Return _UPDATECOL
        End Get
        Set(ByVal value As String)
            If _UPDATECOL <> value Then
                OnPropertyChanging("UPDATECOL", value)
                If Not GetCancelEdit() Then
                    _UPDATECOL = value
                    OnPropertyChanged("UPDATECOL")
                End If
            End If
        End Set
    End Property
#End Region
End Class