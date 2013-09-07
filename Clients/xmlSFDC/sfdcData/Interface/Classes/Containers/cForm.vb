Imports System.Xml

Public Class cForm
    Inherits cContainer

    Public Overrides ReadOnly Property ContainerType() As String
        Get
            Return "form"
        End Get
    End Property

    Public Sub New(ByRef Parent As cInterface, ByRef Node As XmlNode)
        Try
            LoadNode(Node)
            _Triggers = New cTriggers(Me, thisNode)
            _Columns = New cColumns(Me, thisNode)
            _iMsg = New iMessages(thisNode)
            _Parent = Parent

        Catch ex As Exception
            Throw New cfmtException("Failed to load {0}. {1}", NodeType, ex.Message)
        End Try
    End Sub

End Class
