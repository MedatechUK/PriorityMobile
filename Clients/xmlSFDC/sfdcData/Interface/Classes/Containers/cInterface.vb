Imports System.Xml

Public Class cInterface
    Inherits cNode

    Public Sub New(ByRef Node As XmlNode, ByRef UE As UserEnv)
        Try
            _GUID = System.Guid.NewGuid.ToString
            LoadNode(Node)
            _ue = UE
            _Form = New cForm(Me, thisNode.SelectSingleNode("form"))
            _Table = New cTable(Me, thisNode.SelectSingleNode("table"))

            If Not IsNothing(thisNode.Attributes("ldTable")) Then
                _ldTable = thisNode.Attributes("ldTable").Value
            End If
            If Not IsNothing(thisNode.Attributes("ldProcedure")) Then
                _ldProcedure = thisNode.Attributes("ldProcedure").Value
            End If
            If Not IsNothing(thisNode.Attributes("ldEnv")) Then
                _ldEnv = thisNode.Attributes("ldEnv").Value
            End If
            If Not IsNothing(thisNode.Attributes("handler")) Then
                _strHandler = thisNode.Attributes("handler").Value
            End If

            _Form.LoadDependency()
            _Table.LoadDependency()

        Catch ex As Exception
            Throw New cfmtException("Failed to load {0}. {1}", NodeType, ex.Message)
        End Try
    End Sub

    Private _GUID As String
    Public ReadOnly Property GUID() As String
        Get
            Return _GUID
        End Get        
    End Property

    Private _iForm As iForm
    Public ReadOnly Property iForm() As iForm
        Get
            Return _iForm
        End Get
    End Property

    Public Sub SetiForm(ByRef thisForm As iForm)
        _iForm = thisForm
    End Sub

    Private _ue As UserEnv = Nothing
    Public Property ue() As UserEnv
        Get
            Return _ue
        End Get
        Set(ByVal value As UserEnv)
            _ue = value
        End Set
    End Property

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

    Public Property Arguments() As Dictionary(Of String, String)
        Get
            Return ue.Arguments
        End Get
        Set(ByVal value As Dictionary(Of String, String))
            ue.Arguments = value
        End Set
    End Property

    Private _ldTable As String = Nothing
    Public ReadOnly Property ldTable() As String
        Get
            Return _ldTable
        End Get
    End Property

    Private _ldProcedure As String = Nothing
    Public ReadOnly Property ldProcedure() As String
        Get
            Return _ldProcedure
        End Get
    End Property

    Private _ldEnv As String = Nothing
    Public ReadOnly Property ldEnv() As String
        Get
            Return _ldEnv
        End Get
    End Property

    Private _strHandler As String = Nothing
    Public ReadOnly Property strHandler() As String
        Get
            Return _strHandler
        End Get
    End Property

End Class
