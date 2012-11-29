Imports Bind
Public Class Signature
Inherits DatasetObjectBase
#Region "Private Variables"
Private _DOCNO As String
Private _SIGDATA As String
Private _ASTEXT As String
#End Region
#Region "Initialisation"
Public Sub New()
MyBase.New()
End Sub
Public Sub New(ByVal DOCNO As String, ByVal SIGDATA As String, ByVal ASTEXT As String)
MyBase.New()
_DOCNO = DOCNO
_SIGDATA = SIGDATA
_ASTEXT = ASTEXT
end sub
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
"DOCNO", "SIGDATA", "ASTEXT" _
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
"DOCNO", "SIGDATA", "ASTEXT" _
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
Public Property SIGDATA() As String
Get
Return _SIGDATA
End Get
Set(ByVal value As String)
If _SIGDATA <> value Then
OnPropertyChanging("SIGDATA", value)
If Not GetCancelEdit() Then
_SIGDATA = value
OnPropertyChanged("SIGDATA")
End If
End If
End Set
End Property
Public Property ASTEXT() As String
Get
Return _ASTEXT
End Get
Set(ByVal value As String)
If _ASTEXT <> value Then
OnPropertyChanging("ASTEXT", value)
If Not GetCancelEdit() Then
_ASTEXT = value
OnPropertyChanged("ASTEXT")
End If
End If
End Set
End Property
#End Region
End Class