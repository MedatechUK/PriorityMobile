Imports System.Windows.Forms

Public Class TableView
    Inherits iFormChild

#Region "Table View"

    Public Enum eTableView
        vTable = 0
        vForm = 1
    End Enum

    Private _TableView As eTableView = eTableView.vTable
    Public Property TableView() As eTableView
        Get
            Return _TableView
        End Get
        Set(ByVal value As eTableView)
            _TableView = value
            SetView()
        End Set
    End Property

    Private Sub SetView()
        With Me

            If IsNothing(.ViewForm) Then Exit Sub
            If IsNothing(.ViewTable) Then Exit Sub

            Select Case .TableView
                Case eTableView.vForm
                    With .ViewTable
                        .Dock = DockStyle.None
                        .Visible = False
                    End With
                    With .ViewForm
                        .Visible = True
                        .Dock = DockStyle.Fill
                        '.Focus()
                        '.NextControl(True)
                    End With

                Case eTableView.vTable
                    With .ViewForm
                        .Clear()
                        .Dock = DockStyle.None
                        .Visible = False
                    End With
                    With .ViewTable
                        .Visible = True
                        .Dock = DockStyle.Fill
                    End With

            End Select

            ParentForm.ViewMain.FormButtons.RefreshButtons()

        End With
    End Sub

    Private _ThisTable As cTable
    Private Property ThisTable() As cTable
        Get
            Return _ThisTable
        End Get
        Set(ByVal value As cTable)
            _ThisTable = value
        End Set
    End Property

#End Region

#Region "Container Panels"

    Private _ViewForm As ColumnPanel = Nothing
    Public Property ViewForm() As ColumnPanel
        Get
            Return _ViewForm
        End Get
        Set(ByVal value As ColumnPanel)
            _ViewForm = value
        End Set
    End Property

    Private _ViewTable As TablePanel = Nothing
    Public Property ViewTable() As TablePanel
        Get
            Return _ViewTable
        End Get
        Set(ByVal value As TablePanel)
            _ViewTable = value
        End Set
    End Property

#End Region

#Region "Inheritance"

    Public Overrides ReadOnly Property ParentForm() As iForm
        Get
            Return _Parent.ParentForm
        End Get
    End Property

    Private _Parent As FormPanel
    Public Overloads ReadOnly Property Parent() As FormPanel
        Get
            Return _Parent
        End Get
    End Property

#End Region

#Region "Initialisation and finalisation"

    Public Sub Load(ByRef Parent As FormPanel, ByRef thisTable As cTable)

        _Parent = Parent
        _Container = thisTable
        _ThisTable = thisTable
        _ScannedRecord = New cTableItem(thisTable.Columns, thisTable.Unique)

        ViewForm = New ColumnPanel(Me, thisTable.Columns)
        ViewTable = New TablePanel(Me, thisTable.Columns)

        With Me.Controls
            .Add(ViewForm)
            .Add(ViewTable)
            SetView()
        End With

    End Sub

#End Region

#Region "Table View Event Handlers"

    Private Sub TableView_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.GotFocus
        Try
            Select Case TableView
                Case eTableView.vForm
                    ViewForm.Focus()
                Case eTableView.vTable
                    ViewTable.Focus()
            End Select
        Catch
        End Try
    End Sub

#End Region

#Region "Editing item"

    Private _EditItem As cTableItem
    Public Property EditItem() As cTableItem
        Get
            For Each col As cColumn In Me.ViewTable.Columns.Values
                _EditItem(String.Format(":$.{0}", col.Name)) = col.Value
            Next
            Return _EditItem
        End Get
        Set(ByVal value As cTableItem)
            _EditItem = value
            For Each k As String In value.Keys
                With Me.ViewForm.Columns(k.Replace(":$.", ""))
                    .NoPostField = True
                    .Value = value(k)
                    If Not IsNothing(.uiCol) Then
                        .uiCol.lbl_Value.Text = value(k)
                    End If
                    .NoPostField = False
                End With
            Next
        End Set
    End Property

    Public ReadOnly Property Unique() As Boolean
        Get
            For Each row As cTableItem In Me.ViewTable.Items
                If Not row.EditItem.Equals(EditItem.EditItem) Then
                    For Each keys As List(Of String) In _ThisTable.Unique
                        Dim un As Boolean = True
                        For Each key As String In keys
                            If Not String.Compare(row(key), EditItem(key)) = 0 Then
                                un = False
                                Exit For
                            End If
                        Next
                        If un Then
                            Return False
                        End If
                    Next
                End If
            Next
            Return True
        End Get        
    End Property

#End Region

#Region "Build Table Scanned record"

    Private _ScannedRecord As cTableItem
    Public Property ScannedRecord() As cTableItem
        Get
            Return _ScannedRecord
        End Get
        Set(ByVal value As cTableItem)
            _ScannedRecord = value
        End Set
    End Property

    Public Sub ProcessScanned(ByVal ValidColumns As Dictionary(Of String, String))

        For Each k As String In ValidColumns.Keys
            _ScannedRecord(String.Format(":$.{0}", k)) = ValidColumns(k)
        Next

        Dim hk As List(Of String) = _ScannedRecord.HasKeys
        If hk.Count > 0 Then

            Dim changed As New List(Of String)
            Dim mch As Boolean = False

            For Each row As cTableItem In Me.ViewTable.Items
                If row.Equals(_ScannedRecord, hk) Then
                    mch = True
                    EditItem = row
                    For Each key As String In _Container.Columns.Keys
                        If _ScannedRecord(String.Format(":$.{0}", key)).Length > 0 Then
                            If Not String.Compare( _
                                EditItem(String.Format(":$.{0}", key)), _
                                _ScannedRecord(String.Format(":$.{0}", key)) _
                            ) = 0 Then
                                EditItem(String.Format(":$.{0}", key)) = _ScannedRecord(String.Format(":$.", key))
                                changed.Add(key)
                            End If
                        End If
                    Next
                    Exit For
                End If
            Next

            If mch Or (Not (mch) And ParentForm.ViewMain.FormButtons.Buttons(eFormButtons.btn_Add).Enabled) Then
                If Not mch Then
                    EditItem = _ScannedRecord
                    EditItem.EditItem = Nothing

                    For Each key As String In _Container.Columns.Keys
                        If EditItem(String.Format(":$.{0}", key)).Length > 0 Then
                            changed.Add(key)
                        End If
                    Next

                    With Me.Container
                        If .Triggers.Keys.Contains("PRE-INSERT") Then
                            .Triggers("PRE-INSERT").Execute()
                        End If
                    End With

                End If

                For Each ch As String In changed
                    With ViewForm.Columns(ch).Triggers
                        If .Keys.Contains("POST-FIELD") Then _
                            .Item("POST-FIELD").Execute()
                    End With
                Next

                TableView = eTableView.vForm
                ViewForm.FirstControl()

            End If

        End If

        _ScannedRecord = New cTableItem(ThisTable.Columns, ThisTable.Unique)

    End Sub

#End Region

End Class
