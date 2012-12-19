Imports System.Xml

Public Class ctrlTable

    Private _thisform As iForm

    Public Sub New(ByRef thisform As iForm, ByVal ParentNode As XmlNode)
        InitializeComponent()
        _thisform = thisform
        Load(Me, ParentNode)
    End Sub

    Public Overrides Property Value(ByVal ColumnName As String) As String
        Get
            Return Columns(ColumnName).Value
        End Get
        Set(ByVal value As String)

        End Set
    End Property

End Class
