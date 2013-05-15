Imports System.Xml

Public Class cInterface
    Inherits cNode

    Public Sub New(ByRef Node As XmlNode, Optional ByVal Arguments As Dictionary(Of String, String) = Nothing)
        Try
            LoadNode(Node)
            _Form = New cForm(Me, thisNode.SelectSingleNode("form"))
            _Table = New cTable(Me, thisNode.SelectSingleNode("table"))
            _Arguments = Arguments

            _Form.LoadDependency()
            _Table.LoadDependency()

        Catch ex As Exception
            Throw New cfmtException("Failed to load {0}. {1}", NodeType, ex.Message)
        End Try
    End Sub

    Public Overrides ReadOnly Property NodeType() As String
        Get
            Return "interface"
        End Get
    End Property

    Private _Form As cForm
    Public ReadOnly Property Form() As cForm
        Get
            Return _Form
        End Get
    End Property

    Private _Table As cTable
    Public ReadOnly Property Table() As cTable
        Get
            Return _Table
        End Get
    End Property

    Private _Arguments As New Dictionary(Of String, String)
    Public Property Arguments() As Dictionary(Of String, String)
        Get
            Return _Arguments
        End Get
        Set(ByVal value As Dictionary(Of String, String))
            _Arguments = value
        End Set
    End Property

End Class
