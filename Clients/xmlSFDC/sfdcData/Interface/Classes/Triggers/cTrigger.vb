Imports System.Xml

Public Class cTrigger
    Inherits cNode

    Public Overrides ReadOnly Property NodeType() As String
        Get
            Return "triggers"
        End Get
    End Property

    Public Overloads ReadOnly Property Parent() As cNode
        Get
            Return _Parent
        End Get
    End Property

    Private _sql As String
    Public Property SQL() As String
        Get
            Return _sql
        End Get
        Set(ByVal value As String)
            _sql = value
            _ColumnRef.Clear()
            _ColumnRef = rxMatch(rxColumn, _sql)
            _ArgsRef.Clear()
            _ArgsRef = rxMatch(rxArgs, _sql)
        End Set
    End Property

    Private _ColumnRef As New List(Of String)
    Public ReadOnly Property ColumnRefs() As List(Of String)
        Get
            Return _ColumnRef
        End Get
    End Property

    Private _ArgsRef As New List(Of String)
    Public ReadOnly Property ArgsRefs() As List(Of String)
        Get
            Return _ArgsRef
        End Get
    End Property

    Public Sub New(ByRef Parent As cNode, ByRef Node As XmlNode)
        Try
            LoadNode(Node)
            _Parent = Parent
            _sql = Node.Attributes("sql").Value

        Catch ex As Exception
            Throw New cfmtException("Failed to load {0}. {1}", NodeType, ex.Message)
        End Try
    End Sub

    Public Sub Execute()

        Dim thisSQL As String = SQL

        Dim int As cInterface = Nothing
        If Not IsNothing(TryCast(Parent, cContainer)) Then
            int = Parent
        ElseIf Not IsNothing(TryCast(Parent, cColumn)) Then
            int = Parent.Parent
        End If

        Try
            For Each InsertIntoClause As String In rxMatch(rxInsertInto, thisSQL)
                Dim ii As String = InsertIntoClause
                For Each strColRef As String In rxMatch(rxColumn, ii)
                    ii = ii.Replace(strColRef, GetColumn(Me.Parent, strColRef).SQLValue)
                Next
                For Each strArgRef As String In rxMatch(rxColumn, ii)
                    If int.Arguments.Keys.Contains(strArgRef.Replace("%", "")) Then
                        ii = ii.Replace(strArgRef, int.Arguments(strArgRef.Replace("%", "")))
                    Else
                        Throw New cfmtException("Argument {0} not found.", strArgRef.Replace("%", ""))
                    End If
                Next
                thisSQL = thisSQL.Replace(InsertIntoClause, ii)
            Next

            For Each WhereClause As String In rxMatch(rxWhere, thisSQL)
                Dim ii As String = WhereClause
                For Each strColRef As String In rxMatch(rxColumn, ii)
                    ii = ii.Replace(strColRef, GetColumn(Me.Parent, strColRef).SQLValue)
                Next
                For Each strArgRef As String In rxMatch(rxColumn, ii)
                    If int.Arguments.Keys.Contains(strArgRef.Replace("%", "")) Then
                        ii = ii.Replace(strArgRef, int.Arguments(strArgRef.Replace("%", "")))
                    Else
                        Throw New cfmtException("Argument {0} not found.", strArgRef.Replace("%", ""))
                    End If
                Next
                thisSQL = thisSQL.Replace(WhereClause, ii)
            Next

        Catch ex As Exception
            MsgBox(String.Format("Trigger execution failed: {0}", ex.Message))
        End Try
    End Sub

End Class
