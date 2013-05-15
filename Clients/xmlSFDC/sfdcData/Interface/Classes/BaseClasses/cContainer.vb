Imports System.Xml

Public MustInherit Class cContainer
    Inherits cNode

    Public MustOverride ReadOnly Property ContainerType() As String

    Public Overrides ReadOnly Property NodeType() As String
        Get
            Return ContainerType
        End Get
    End Property

    Public Overloads ReadOnly Property Parent() As cInterface
        Get
            Return _Parent
        End Get
    End Property

    Friend _Columns As cColumns
    Public ReadOnly Property Columns() As Dictionary(Of String, cColumn)
        Get
            Return _Columns
        End Get
    End Property

    Friend _Triggers As cTriggers
    Public ReadOnly Property Triggers() As Dictionary(Of String, cTrigger)
        Get
            Return _Triggers
        End Get
    End Property

    Public Sub LoadDependency()

        For Each col As cColumn In _Columns.Values
            For Each dep As String In col._strDepends
                col.Depends.Add(GetColumn(Me, dep))
            Next
        Next

    End Sub

End Class
