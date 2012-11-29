Imports Bind
Public Class Time
Inherits DatasetObjectBase
#Region "Private Variables"
Private _DOCNO As String
Private _ONSITE As Integer
Private _ONROUTE As Integer
Private _ENDTIME As Integer
#End Region
#Region "Initialisation"
Public Sub New()
MyBase.New()
End Sub
Public Sub New(ByVal DOCNO As String, ByVal ONSITE As Integer, ByVal ONROUTE As Integer, ByVal ENDTIME As Integer)
MyBase.New()
_DOCNO = DOCNO
_ONSITE = ONSITE
_ONROUTE = ONROUTE
_ENDTIME = ENDTIME
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
"DOCNO", "ONSITE", "ONROUTE", "ENDTIME" _
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
"DOCNO", "ONSITE", "ONROUTE", "ENDTIME" _
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
Public Property ONSITE() As Integer
Get
Return _ONSITE
End Get
Set(ByVal value As Integer)
If _ONSITE <> value Then
OnPropertyChanging("ONSITE", value)
If Not GetCancelEdit() Then
_ONSITE = value
OnPropertyChanged("ONSITE")
End If
End If
End Set
End Property
Public Property ONROUTE() As Integer
Get
Return _ONROUTE
End Get
Set(ByVal value As Integer)
If _ONROUTE <> value Then
OnPropertyChanging("ONROUTE", value)
If Not GetCancelEdit() Then
_ONROUTE = value
OnPropertyChanged("ONROUTE")
End If
End If
End Set
End Property
Public Property ENDTIME() As Integer
Get
Return _ENDTIME
End Get
Set(ByVal value As Integer)
If _ENDTIME <> value Then
OnPropertyChanging("ENDTIME", value)
If Not GetCancelEdit() Then
_ENDTIME = value
OnPropertyChanged("ENDTIME")
End If
End If
End Set
End Property
#End Region
End Class