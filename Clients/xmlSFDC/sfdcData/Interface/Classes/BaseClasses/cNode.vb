Imports System.Xml

Public MustInherit Class cNode

    Public MustOverride ReadOnly Property NodeType() As String

    Friend _Parent As Object
    Public Property Parent() As Object
        Get
            Return _Parent
        End Get
        Set(ByVal value As Object)
            _Parent = value
        End Set
    End Property

    Public Sub LoadNode(ByRef thisnode As XmlNode)
        If Not (String.Compare(thisnode.Name, NodeType) = 0) Then _
            Throw New Exception("Invalid node.")
        _thisnode = thisnode
    End Sub

    Private _thisnode As XmlNode
    Public ReadOnly Property thisNode() As XmlNode
        Get
            Return _thisnode
        End Get
    End Property

End Class
