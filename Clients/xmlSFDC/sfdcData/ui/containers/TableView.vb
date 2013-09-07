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
                        .NextControl(True)
                    End With

                Case eTableView.vTable
                    With .ViewForm
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

#End Region

End Class
