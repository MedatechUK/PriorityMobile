Imports Bind
Public Class Malfunction
Inherits DatasetObjectBase
#Region "Private Variables"
Private _MALFCODE As String
Private _MALFDES As String
#End Region
#Region "Initialisation"
Public Sub New()
MyBase.New()
End Sub
Public Sub New(ByVal MALFCODE As String, ByVal MALFDES As String)
MyBase.New()
_MALFCODE = MALFCODE
_MALFDES = MALFDES
end sub
#End Region
#Region "Overrides"
Public Overrides Function ConQuery() As String
' The SQL statement to populate the dataset from.
' Return Nothing if the dataset is not updated from the SOAP service.
Return _
"select " & _
"[MALFCODE] as MALFCODE,  " & _
"[MALFDES] as MALFDES " & _
"from V_SVCCALL_MALFUNCTION "
End Function
Public Overrides Function Columns() As String()
' List of columns in the table
Dim myCols() As String = { _
"MALFCODE", "MALFDES" _
}
Return myCols
End Function
Public Overrides Function KeyColumns() As String()
' List of colums that combine to create a unique key
Dim myCols() As String = { _
"MALFCODE" _
}
Return myCols
End Function
Public Overrides Function UpdateColumns() As String()
' List of columns updated by a SOAP Syncronisation.
Dim myCols() As String = { _
"MALFCODE", "MALFDES" _
}
Return myCols
End Function
#End Region
#Region "Public Properties"
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
Public Property MALFDES() As String
Get
Return _MALFDES
End Get
Set(ByVal value As String)
If _MALFDES <> value Then
OnPropertyChanging("MALFDES", value)
If Not GetCancelEdit() Then
_MALFDES = value
OnPropertyChanged("MALFDES")
End If
End If
End Set
End Property
#End Region
End Class