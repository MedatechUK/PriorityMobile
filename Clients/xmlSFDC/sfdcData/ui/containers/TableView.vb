Imports System.Windows.Forms
Public Class TableView

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

    Private _Parent As iForm
    Public Overloads ReadOnly Property Parent() As iForm
        Get
            Return _Parent
        End Get
    End Property

    Public Sub Load(ByRef Parent As iForm, ByRef thisTable As cTable)
        _parent = Parent
        ViewForm = New ColumnPanel(Me, thisTable.Columns)
        ViewTable = New TablePanel(Me, thisTable.Columns)
        With Me.Controls
            .Add(ViewForm)
            .Add(ViewTable)
            SetView()
        End With
    End Sub

End Class
