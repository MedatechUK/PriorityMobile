Imports Bind
Public Class FormItem
Inherits DatasetObjectBase
#Region "Private Variables"
Private _FullPath As String
Private _Name As String
Private _Filter As String
Private _DefaultView As String
Private _SQLFrom As String
Private _IsReadOnly As String
#End Region
#Region "Initialisation"
Public Sub New()
MyBase.New()
End Sub
Public Sub New(ByVal FullPath As String, ByVal Name As String, ByVal Filter As String, ByVal DefaultView As String, ByVal SQLFrom As String, ByVal IsReadOnly As String)
MyBase.New()
_FullPath = FullPath
_Name = Name
_Filter = Filter
_DefaultView = DefaultView
_SQLFrom = SQLFrom
_IsReadOnly = IsReadOnly
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
"FullPath", "Name", "Filter", "DefaultView", "SQLFrom", "IsReadOnly" _
}
Return myCols
End Function
Public Overrides Function KeyColumns() As String()
' List of colums that combine to create a unique key
Dim myCols() As String = { _
"FullPath" _
}
Return myCols
End Function
Public Overrides Function UpdateColumns() As String()
' List of columns updated by a SOAP Syncronisation.
Dim myCols() As String = { _
"FullPath", "Name", "Filter", "DefaultView", "SQLFrom", "IsReadOnly" _
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
Public Property Filter() As String
Get
Return _Filter
End Get
Set(ByVal value As String)
If _Filter <> value Then
OnPropertyChanging("Filter", value)
If Not GetCancelEdit() Then
_Filter = value
OnPropertyChanged("Filter")
End If
End If
End Set
End Property
Public Property DefaultView() As String
Get
Return _DefaultView
End Get
Set(ByVal value As String)
If _DefaultView <> value Then
OnPropertyChanging("DefaultView", value)
If Not GetCancelEdit() Then
_DefaultView = value
OnPropertyChanged("DefaultView")
End If
End If
End Set
End Property
Public Property SQLFrom() As String
Get
Return _SQLFrom
End Get
Set(ByVal value As String)
If _SQLFrom <> value Then
OnPropertyChanging("SQLFrom", value)
If Not GetCancelEdit() Then
_SQLFrom = value
OnPropertyChanged("SQLFrom")
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
#End Region
End Class