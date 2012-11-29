Imports Bind
Public Class Warehouse
Inherits DatasetObjectBase
#Region "Private Variables"
Private _PARTNAME As String
Private _PARTDES As String
Private _QTY As Integer
Private _PRICE As String
Private _WARHSNAME As String
#End Region
#Region "Initialisation"
Public Sub New()
MyBase.New()
End Sub
Public Sub New(ByVal PARTNAME As String, ByVal PARTDES As String, ByVal QTY As Integer, ByVal PRICE As String, ByVal WARHSNAME As String)
MyBase.New()
_PARTNAME = PARTNAME
_PARTDES = PARTDES
_QTY = QTY
_PRICE = PRICE
_WARHSNAME = WARHSNAME
end sub
#End Region
#Region "Overrides"
Public Overrides Function ConQuery() As String
' The SQL statement to populate the dataset from.
' Return Nothing if the dataset is not updated from the SOAP service.
Return _
"select  " & _
"[PARTNAME] as PARTNAME, " & _
"[PARTDES] as PARTDES, " & _
"[QTY] as QTY, " & _
"[PRICE] as PRICE, " & _
"[WARHSNAME] as WARHSNAME  " & _
"from V_SVCCALL_WARHS " & _
"where WARHSNAME = '%van%' "
End Function
Public Overrides Function Columns() As String()
' List of columns in the table
Dim myCols() As String = { _
"PARTNAME", "PARTDES", "QTY", "PRICE", "WARHSNAME" _
}
Return myCols
End Function
Public Overrides Function KeyColumns() As String()
' List of colums that combine to create a unique key
Dim myCols() As String = { _
"PARTNAME" _
}
Return myCols
End Function
Public Overrides Function UpdateColumns() As String()
' List of columns updated by a SOAP Syncronisation.
Dim myCols() As String = { _
"PARTNAME", "PARTDES", "QTY", "PRICE", "WARHSNAME" _
}
Return myCols
End Function
#End Region
#Region "Public Properties"
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
Public Property PRICE() As String
Get
Return _PRICE
End Get
Set(ByVal value As String)
If _PRICE <> value Then
OnPropertyChanging("PRICE", value)
If Not GetCancelEdit() Then
_PRICE = value
OnPropertyChanged("PRICE")
End If
End If
End Set
End Property
Public Property WARHSNAME() As String
Get
Return _WARHSNAME
End Get
Set(ByVal value As String)
If _WARHSNAME <> value Then
OnPropertyChanging("WARHSNAME", value)
If Not GetCancelEdit() Then
_WARHSNAME = value
OnPropertyChanged("WARHSNAME")
End If
End If
End Set
End Property
#End Region
End Class