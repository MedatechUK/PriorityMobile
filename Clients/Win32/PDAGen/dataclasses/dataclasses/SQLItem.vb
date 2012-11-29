Imports Bind
Public Class SQLItem
Inherits DatasetObjectBase
#Region "Private Variables"
Private _TABLE As String
Private _SQL As String
#End Region
#Region "Initialisation"
Public Sub New()
MyBase.New()
End Sub
Public Sub New(ByVal TABLE As String, ByVal SQL As String)
MyBase.New()
_TABLE = TABLE
_SQL = SQL
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
"TABLE", "SQL" _
}
Return myCols
End Function
Public Overrides Function KeyColumns() As String()
' List of colums that combine to create a unique key
Dim myCols() As String = { _
"TABLE" _
}
Return myCols
End Function
Public Overrides Function UpdateColumns() As String()
' List of columns updated by a SOAP Syncronisation.
Dim myCols() As String = { _
"TABLE", "SQL" _
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
Public Property SQL() As String
Get
Return _SQL
End Get
Set(ByVal value As String)
If _SQL <> value Then
OnPropertyChanging("SQL", value)
If Not GetCancelEdit() Then
_SQL = value
OnPropertyChanged("SQL")
End If
End If
End Set
End Property
#End Region
End Class