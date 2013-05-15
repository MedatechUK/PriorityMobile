Imports System.Xml
Imports System.Text.RegularExpressions

Public Class cColumn
    Inherits cNode

    Friend _strDepends As New List(Of String)

    Public Overloads ReadOnly Property Parent() As cContainer
        Get
            Return _Parent
        End Get
    End Property

    Public Overrides ReadOnly Property NodeType() As String
        Get
            Return "column"
        End Get
    End Property

    Private ReadOnly Property DependsTriggers() As String()
        Get
            Dim ret As String() = {"CHOOSE-FIELD", "CHECK-FIELD"}
            Return ret
        End Get
    End Property

    Public Sub New(ByRef Parent As cContainer, ByRef Node As XmlNode)
        Try
            LoadNode(Node)
            _Triggers = New cTriggers(Me, thisNode)
            _Parent = Parent

            For Each dt As String In DependsTriggers
                If _Triggers.Keys.Contains(dt) Then
                    For Each colRef As String In _Triggers(dt).ColumnRefs
                        If Not _strDepends.Contains(colRef) Then
                            _strDepends.Add(colRef)
                        End If
                    Next
                End If
            Next

        Catch ex As Exception
            Throw New cfmtException("Failed to load {0}. {1}", NodeType, ex.Message)
        End Try
    End Sub

    Public Sub SetControl(ByRef uiCol As uiColumn)
        _uiCol = uiCol
    End Sub

    Private _uiCol As uiColumn
    Public ReadOnly Property uiCol() As uiColumn
        Get
            Return _uiCol
        End Get
    End Property

    Private _Depends As New List(Of cColumn)
    Public Property Depends() As List(Of cColumn)
        Get
            Return _Depends
        End Get
        Set(ByVal value As List(Of cColumn))
            _Depends = value
        End Set
    End Property

    Friend _Triggers As cTriggers
    Public ReadOnly Property Triggers() As Dictionary(Of String, cTrigger)
        Get
            Return _Triggers
        End Get
    End Property

#Region "Column Value"

    Public Function Validate(ByRef Value As String, ByRef ex As Exception) As Boolean

        _Value = Value
        Return True

    End Function

    Private _Value As String = ""
    Public Property Value() As String
        Get
            Return _Value
        End Get
        Set(ByVal value As String)
            _Value = value
        End Set
    End Property

#End Region

#Region "Column Properties"

    Public ReadOnly Property Name() As String
        Get
            Return thisNode.Attributes("name").Value
        End Get
    End Property

    Public ReadOnly Property Title() As String
        Get
            Return thisNode.Attributes("title").Value
        End Get
    End Property

    Public ReadOnly Property ColumnType() As String
        Get
            Return thisNode.Attributes("type").Value
        End Get
    End Property

    Public ReadOnly Property Visible() As Boolean
        Get
            Return IsNothing(thisNode.Attributes("hidden"))
        End Get
    End Property

    Public ReadOnly Property Mandatory() As Boolean
        Get
            Return Not IsNothing(thisNode.Attributes("mandatory"))
        End Get
    End Property

    Public ReadOnly Property isReadOnly() As Boolean
        Get
            Return Not IsNothing(thisNode.Attributes("readonly"))
        End Get
    End Property

#End Region

#Region "Calculated Column Properties"

    Public ReadOnly Property SQLValue() As String
        Get
            Select Case ColumnType.ToUpper
                Case "INT", "REAL", "DATE", "TIME"
                    Return String.Format("{0}", Value)
                Case Else
                    Return String.Format("'{0}'", Value)
            End Select
        End Get
    End Property

#End Region

End Class
