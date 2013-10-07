Imports System.Xml

Public Class cTable
    Inherits cContainer

    Private PreFormLoaded As Boolean = False

    Friend _strDepends As New List(Of String)

    Public Overrides ReadOnly Property ContainerType() As String
        Get
            Return "table"
        End Get
    End Property

    Private _CheckDependancy As Boolean = False
    Public ReadOnly Property CheckDependancy() As Boolean
        Get
            Return _CheckDependancy
        End Get
    End Property

    Private _Unique As New List(Of List(Of String))
    Public ReadOnly Property Unique() As List(Of List(Of String))
        Get
            Return _Unique
        End Get
    End Property

    Public Sub New(ByRef Parent As cInterface, ByRef Node As XmlNode)
        Try
            LoadNode(Node)
            _Columns = New cColumns(Me, thisNode)
            _Triggers = New cTriggers(Me, thisNode)
            _iMsg = New iMessages(thisNode)
            _Parent = Parent

            For Each formTrigger As cTrigger In _Triggers.Values
                If String.Compare(formTrigger.TriggerName, "UNIQ") = 0 Then
                    For Each unStr As String In formTrigger.SQL.Split(";")
                        Dim un As New List(Of String)
                        For Each uCol As String In rxMatch(rxColumn, unStr)
                            un.Add(uCol)
                        Next
                        If un.Count > 0 Then _Unique.Add(un)
                    Next
                Else
                    For Each strTrig As String In rxMatch(rxUpperColumn, formTrigger.SQL)
                        If Not _strDepends.Contains(strTrig) Then
                            _strDepends.Add(strTrig)
                        End If
                    Next
                End If
            Next

            For Each thiscolumn As cColumn In _Columns.Values
                For Each columnTigger As cTrigger In thiscolumn.Triggers.Values
                    For Each strTrig As String In rxMatch(rxUpperColumn, columnTigger.SQL)
                        If Not _strDepends.Contains(strTrig) Then
                            _strDepends.Add(strTrig)
                        End If
                    Next
                Next
            Next

            If _strDepends.Count = 0 Then
                _CheckDependancy = True
            Else
                _CheckDependancy = False
                For Each strCol As String In _strDepends
                    AddHandler Parent.Form.Columns(strCol.Replace(":$$.", "")).SetData, AddressOf hTableDepends
                Next
            End If

        Catch ex As Exception
            Throw New cfmtException("Failed to load {0}. {1}", NodeType, ex.Message)
        End Try

    End Sub

    Private Sub hTableDepends()

        For Each strCol As String In _strDepends
            If Parent.Form.Columns(strCol.Replace(":$$.", "")).Value.Length = 0 Then
                _CheckDependancy = False
                Parent.iForm.ViewMain.FormButtons.RefreshButtons()
                Exit Sub
            End If
        Next

        _CheckDependancy = True
        Parent.iForm.ViewMain.FormButtons.RefreshButtons()

        If _Triggers.Keys.Contains("PRE-FORM") Then
            If Not PreFormLoaded Then
                _Triggers("PRE-FORM").Execute()
                PreFormLoaded = True
            End If
        End If

    End Sub

End Class
