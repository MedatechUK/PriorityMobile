Imports Bind
Public Class Parts
    Inherits DatasetObjectBase
#Region "Private Variables"
    Private _DOCNO As String
    Private _PARTNAME As String
    Private _PARTDES As String
    Private _QTY As Integer
#End Region
#Region "Initialisation"
    Public Sub New()
        MyBase.New()
    End Sub
    Public Sub New(ByVal DOCNO As String, ByVal PARTNAME As String, ByVal PARTDES As String, ByVal QTY As Integer)
        MyBase.New()
        _DOCNO = DOCNO
        _PARTNAME = PARTNAME
        _PARTDES = PARTDES
        _QTY = QTY
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
        "DOCNO", "PARTNAME", "PARTDES", "QTY" _
        }
        Return myCols
    End Function
    Public Overrides Function KeyColumns() As String()
        ' List of colums that combine to create a unique key
        Dim myCols() As String = { _
        "DOCNO", "PARTNAME" _
        }
        Return myCols
    End Function
    Public Overrides Function UpdateColumns() As String()
        ' List of columns updated by a SOAP Syncronisation.
        Dim myCols() As String = { _
        "DOCNO", "PARTNAME", "PARTDES", "QTY" _
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
    Public Property PARTNAME() As String
        Get
            Return _PARTNAME
        End Get
        Set(ByVal value As String)
            If _PARTNAME <> value Then
                OnPropertyChanging("PARTNAME", value)
                If Not GetCancelEdit() Then
                    _PARTNAME = value
                    OnPropertyChanged("PARTNAME")
                End If
            End If
        End Set
    End Property
    Public Property PARTDES() As String
        Get
            Return _PARTDES
        End Get
        Set(ByVal value As String)
            If _PARTDES <> value Then
                OnPropertyChanging("PARTDES", value)
                If Not GetCancelEdit() Then
                    _PARTDES = value
                    OnPropertyChanged("PARTDES")
                End If
            End If
        End Set
    End Property
    Public Property QTY() As Integer
        Get
            Return _QTY
        End Get
        Set(ByVal value As Integer)
            If _QTY <> value Then
                OnPropertyChanging("QTY", value)
                If Not GetCancelEdit() Then
                    _QTY = value
                    OnPropertyChanged("QTY")
                End If
            End If
        End Set
    End Property
#End Region
End Class