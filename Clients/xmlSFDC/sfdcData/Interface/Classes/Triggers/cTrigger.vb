Imports System.Xml
Imports Priority

Public Class cTrigger
    Inherits cNode

#Region "Public Properties"

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

    Private _TriggerName As String
    Public ReadOnly Property TriggerName() As String
        Get
            Return _TriggerName
        End Get
    End Property

    Private _sql As String
    Public Property SQL() As String
        Get
            Return _sql
        End Get
        Set(ByVal value As String)
            _sql = value

            With _ColumnRef
                .Clear()
                For Each SelectIntoClause As String In rxMatch(rxSelectInto, value)
                    For Each c As String In rxMatch(rxColumn, SelectIntoClause)
                        If Not .Contains(c) Then
                            .Add(c)
                        End If
                    Next
                Next
                For Each WhereClause As String In rxMatch(rxWhere, value)
                    For Each c As String In rxMatch(rxColumn, WhereClause)
                        If Not .Contains(c) Then
                            .Add(c)
                        End If
                    Next
                Next
            End With

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

#End Region

#Region "Initailisation and Finalisation"

    Public Sub New(ByRef Parent As cNode, ByRef Node As XmlNode)
        Try
            LoadNode(Node)
            _Parent = Parent
            _TriggerName = Node.Attributes("trigger").Value
            SQL = Node.Attributes("sql").Value

        Catch ex As Exception
            Throw New cfmtException("Failed to load {0}. {1}", NodeType, ex.Message)
        End Try
    End Sub

#End Region

#Region "Public Methods"

    Public Function Execute() As Data.DataTable

        Dim thisSQL As String = SQL
        Dim ThisColumn As String = String.Empty
        Dim thisContainer As cContainer = Nothing
        Dim arg As Dictionary(Of String, String) = Nothing
        Dim par As New Dictionary(Of Integer, String)
        Dim int As cInterface = Nothing

        If Not IsNothing(TryCast(Parent, cContainer)) Then ' Form Triggers
            thisContainer = Parent
            int = thisContainer.Parent
        ElseIf Not IsNothing(TryCast(Parent, cColumn)) Then ' Column Triggers
            thisContainer = Parent.Parent
            int = thisContainer.Parent
            thisSQL = thisSQL.Replace(":$.@", String.Format(":$.{0}", TryCast(Parent, cColumn).Name))
        End If
        arg = int.Arguments

        Try

            ParseFormValues(thisSQL, thisContainer)

            Dim bsql As New Text.StringBuilder

            For Each stmt As String In thisSQL.Split(";")
                If stmt.Trim.Length > 0 Then

                    ParseArgValues(stmt, int)

                    Dim IntoFrom As List(Of String) = rxMatch(rxIntoFrom, stmt)
                    If IntoFrom.Count > 0 Then
                        Dim data As Data.DataTable = _
                            int.iForm.DataService.ExecuteReader( _
                                stmt.Replace( _
                                    IntoFrom(0), _
                                    " FROM").Replace( _
                                        "FROM DUMMY", "" _
                                    ) _
                                )

                        If thisContainer.NodeType = "table" And TriggerName = "PRE-FORM" Then

                            Dim piDic As New Dictionary(Of String, String)
                            If thisContainer.Triggers.Keys.Contains("PRE-INSERT") Then

                                Dim pi As String = thisContainer.Triggers("PRE-INSERT").SQL
                                ParseFormValues(pi, thisContainer)
                                ParseArgValues(pi, int)
                                Dim piIntoFrom As List(Of String) = rxMatch(rxIntoFrom, pi)
                                Dim pidata As Data.DataTable = _
                                    int.iForm.DataService.ExecuteReader( _
                                        pi.Replace( _
                                            piIntoFrom(0), _
                                            " FROM").Replace( _
                                                "FROM DUMMY", "" _
                                            ) _
                                        )

                                Dim picols As List(Of String) = rxMatch(rxColPar, piIntoFrom(0))
                                For i As Integer = 0 To picols.Count - 1
                                    piDic.Add(picols(i), pidata.Rows(0).Item(i))
                                Next

                            End If

                            Dim cols As List(Of String) = rxMatch(rxColumn, IntoFrom(0))

                            For r As Integer = 0 To data.Rows.Count - 1
                                Dim ti As New cTableItem(int.iForm.ViewMain.TableView.ViewTable.TableItem)

                                For Each pi As String In piDic.Keys
                                    ti.Column(pi) = piDic(pi)
                                Next

                                For i As Integer = 0 To cols.Count - 1
                                    ti.Column(cols(i)) = data.Rows(r).Item(i)
                                Next

                                int.iForm.ViewMain.TableView.ViewTable.thisTable.Items.Add(ti.thisItem)
                            Next

                        Else

                            Dim cols As List(Of String) = rxMatch(rxColPar, IntoFrom(0))
                            For i As Integer = 0 To cols.Count - 1
                                If rxIsPattern(rxColumn, cols(i)) Then
                                    Select Case data.Rows(0).Item(i).ToString
                                        Case "++"
                                            GetColumn(Me.Parent, cols(i)).isReadOnly = False
                                        Case "--"
                                            GetColumn(Me.Parent, cols(i)).isReadOnly = True
                                        Case Else
                                            GetColumn(Me.Parent, cols(i)).Value = data.Rows(0).Item(i)
                                    End Select

                                Else
                                    If rxIsPattern(rxPar, cols(i)) Then
                                        If par.Keys.Contains(CInt(cols(i).Replace(":PAR", ""))) Then
                                            par(CInt(cols(i).Replace(":PAR", ""))) = data.Rows(0).Item(i)
                                        Else
                                            par.Add(CInt(cols(i).Replace(":PAR", "")), data.Rows(0).Item(i))
                                        End If
                                    Else
                                        If arg.Keys.Contains(cols(i).Replace(":", "")) Then
                                            arg(cols(i).Replace(":", "")) = data.Rows(0).Item(i)
                                        Else
                                            arg.Add(cols(i).Replace(":", ""), data.Rows(0).Item(i))
                                        End If
                                    End If
                                End If
                            Next
                        End If

                    ElseIf TriggerName = "CHECK-FIELD" Then
                        Dim tbl As Data.DataTable
                        For Each p As Integer In par.Keys
                            stmt = stmt.Replace(String.Format("<P{0}>", p.ToString), par(p))
                        Next
                        tbl = int.iForm.DataService.ExecuteReader(stmt)
                        If tbl.Rows.Count > 0 Then
                            Return tbl
                            Exit Function
                        End If

                    Else
                        bsql.AppendFormat("{0};", stmt)
                    End If

                End If

            Next

            Select Case TriggerName
                Case "CHOOSE-FIELD"
                    Return int.iForm.DataService.ExecuteReader(bsql.ToString)
                Case Else
                    Return Nothing
            End Select

        Catch ex As Exception
            MsgBox(String.Format("Trigger execution failed: {0}", ex.Message), , TriggerName)
            Return Nothing
        End Try

    End Function

#End Region

#Region "Private Methods"

    Private Sub ParseFormValues(ByRef ThisSQL As String, ByRef thisContainer As cContainer)

        For Each func As PriSQLFunc In SQLFuncs
            func.Parse(ThisSQL)
            'ThisSQL = ThisSQL.Replace(func, SQLFuncs(func))
        Next

        For Each InsertIntoClause As String In rxMatch(rxSelectInto, ThisSQL)
            Dim ii As String = InsertIntoClause
            For Each strColRef As String In rxMatch(rxColumn, ii)
                ii = ii.Replace(strColRef, GetColumn(Me.Parent, strColRef).SQLValue)
            Next
            ThisSQL = ThisSQL.Replace(InsertIntoClause, ii)
        Next

        For Each WhereClause As String In rxMatch(rxWhere, ThisSQL)
            Dim ii As String = WhereClause
            For Each strColRef As String In rxMatch(rxColumn, ii)
                ii = ii.Replace(strColRef, GetColumn(Me.Parent, strColRef).SQLValue)
            Next
            ThisSQL = ThisSQL.Replace(WhereClause, ii)
        Next

        For Each ErrM As String In rxMatch(rxErrMsg, ThisSQL)
            ThisSQL = ThisSQL.Replace(ErrM, String.Format("SELECT '{0}'", thisContainer.iMsg(CInt(ErrM.Split(" ")(1)))))
        Next

    End Sub

    Private Sub ParseArgValues(ByRef stmt As String, ByRef int As cInterface)

        For Each InsertIntoClause As String In rxMatch(rxSelectInto, stmt)
            Dim ii As String = InsertIntoClause
            For Each strArgRef As String In rxMatch(rxArg, InsertIntoClause)
                If rxIsPattern(rxArg, strArgRef) And Not rxIsPattern(rxPar, strArgRef) Then
                    If int.Arguments.Keys.Contains(strArgRef.Substring(1)) Then
                        ii = ii.Replace(strArgRef, String.Format("'{0}'", int.Arguments(strArgRef.Substring(1))))
                    Else
                        Throw New cfmtException("Argument {0} not found.", strArgRef)
                    End If
                End If
            Next
            stmt = stmt.Replace(InsertIntoClause, ii)
        Next

        For Each WhereClause As String In rxMatch(rxWhere, stmt)
            Dim ii As String = WhereClause
            For Each strArgRef As String In rxMatch(rxArg, WhereClause)
                If rxIsPattern(rxArg, strArgRef) And Not rxIsPattern(rxPar, strArgRef) Then
                    If int.Arguments.Keys.Contains(strArgRef.Substring(1)) Then
                        ii = ii.Replace(strArgRef, String.Format("'{0}'", int.Arguments(strArgRef.Substring(1))))
                    Else
                        Throw New cfmtException("Argument {0} not found.", strArgRef)
                    End If
                End If
            Next
            stmt = stmt.Replace(WhereClause, ii)
        Next

    End Sub

#End Region

End Class
