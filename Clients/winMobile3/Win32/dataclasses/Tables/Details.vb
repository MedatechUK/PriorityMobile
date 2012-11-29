Imports Bind
Public Class Details
    Inherits DatasetObjectBase
#Region "Private Variables"
    Private _DOCNO As String
    Private _TEXT As String
#End Region
#Region "Initialisation"
    Public Sub New()
        MyBase.New()
    End Sub
    Public Sub New(ByVal DOCNO As String, ByVal TEXT As String)
        MyBase.New()
        _DOCNO = DOCNO
        _TEXT = TEXT
    End Sub
#End Region
#Region "Overrides"
    Public Overrides Function ConQuery() As String
        ' The SQL statement to populate the dataset from.
        ' Return Nothing if the dataset is not updated from the SOAP service.
        Return _
        "select  " & _
        "[DOCNO] as DOCNO, " & _
        "[TXT] as TEXT  " & _
        "from V_SVCCALL_DETAILS  " & _
        "where [USERNAME] = '%username%' "
    End Function
    Public Overrides Function Columns() As String()
        ' List of columns in the table
        Dim myCols() As String = { _
        "DOCNO", "TEXT" _
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
        "DOCNO", "TEXT" _
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
    Public Property TEXT() As String
        Get
            Return _TEXT
        End Get
        Set(ByVal value As String)
            If _TEXT <> value Then
                OnPropertyChanging("TEXT", value)
                If Not GetCancelEdit() Then
                    _TEXT = value
                    OnPropertyChanged("TEXT")
                End If
            End If
        End Set
    End Property
#End Region
End Class
