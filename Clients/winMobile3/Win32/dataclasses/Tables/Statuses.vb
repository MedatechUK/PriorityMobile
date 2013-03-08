Imports Bind
Public Class Statuses
Inherits DatasetObjectBase
#Region "Private Variables"
Private _CODE As String
#End Region
#Region "Initialisation"
Public Sub New()
MyBase.New()
End Sub
Public Sub New(ByVal CODE As String)
MyBase.New()
_CODE = CODE
end sub
#End Region
#Region "Overrides"
Public Overrides Function ConQuery() As String
' The SQL statement to populate the dataset from.
' Return Nothing if the dataset is not updated from the SOAP service.
Return _
"select  " & _
"[CODE] as CODE  " & _
"FROM V_SVCCALL_STATUS "
End Function
Public Overrides Function Columns() As String()
' List of columns in the table
Dim myCols() As String = { _
"CODE" _
}
Return myCols
End Function
Public Overrides Function KeyColumns() As String()
' List of colums that combine to create a unique key
Dim myCols() As String = { _
"CODE" _
}
Return myCols
End Function
Public Overrides Function UpdateColumns() As String()
' List of columns updated by a SOAP Syncronisation.
Dim myCols() As String = { _
"CODE" _
}
Return myCols
End Function
#End Region
#Region "Public Properties"
Public Property CODE() As String
Get
Return _CODE
End Get
Set(ByVal value As String)
If _CODE <> value Then
OnPropertyChanging("CODE", value)
If Not GetCancelEdit() Then
_CODE = value
OnPropertyChanged("CODE")
End If
End If
End Set
End Property
#End Region
End Class