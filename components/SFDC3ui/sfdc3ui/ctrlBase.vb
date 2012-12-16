Imports System.Xml

Public Class ctrlBase

    Public thisForm As iForm

    Public Sub New()
        InitializeComponent()
    End Sub

    Public Sub Load(ByRef Sender As iForm, ByVal ParentNode As XmlNode)
        InitializeComponent()
        thisForm = Sender
        For Each ColumnNode As XmlNode In ParentNode.SelectNodes("//column")
            Me.Columns.Add(New CtrlColumn(Sender, ColumnNode))
        Next
    End Sub

    Private _Columns As List(Of CtrlColumn)
    Public Property Columns() As List(Of CtrlColumn)
        Get
            Return _Columns
        End Get
        Set(ByVal value As List(Of CtrlColumn))
            _Columns = value
        End Set
    End Property

End Class