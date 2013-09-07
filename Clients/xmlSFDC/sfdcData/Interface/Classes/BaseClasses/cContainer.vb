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

    Friend _iMsg As iMessages
    Public ReadOnly Property iMsg() As iMessages
        Get
            Return _iMsg
        End Get
    End Property

    Public Sub LoadDependency()

        For Each col As cColumn In _Columns.Values
            For Each dep As String In col._strDepends
                col.Depends.Add(GetColumn(Me, dep))
                AddHandler GetColumn(Me, dep).SetData, AddressOf col.CheckDependancy
            Next
        Next

    End Sub

    Public Function ScanColumn(ByVal StrName As String) As cColumn
        For Each k As String In _Columns.Keys
            If String.Compare(k, StrName) = 0 Then
                Return _Columns(k)
            ElseIf String.Compare(_Columns(k).BarcodeField, StrName) = 0 Then
                Return _Columns(k)
            End If
        Next
        Return Nothing
    End Function

End Class
