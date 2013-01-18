Public Class fctrlBase

    Public Event onKey()
    Public Overridable ReadOnly Property CtrlType() As Integer
        Get
            Throw New Exception("Control Type was not overriden.")
        End Get
    End Property

#Region "Initialisation and Finalisation"

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Public Sub New(ByVal ColumnName As String, ByVal ColumnTitle As String)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        _ColumnName = ColumnName
        _ColumnTitle = ColumnTitle

    End Sub

#End Region

#Region "Public Properties"

    Private _Value As String = ""
    Public Property Value() As String
        Get
            Return _Value
        End Get
        Set(ByVal value As String)
            _Value = value
        End Set
    End Property

    Private _ColumnName As String
    Public Property ColumnName() As String
        Get
            Return _ColumnName
        End Get
        Set(ByVal value As String)
            _ColumnName = value
        End Set
    End Property

    Private _ColumnTitle As String
    Public Property ColumnTitle() As String
        Get
            Return _ColumnTitle
        End Get
        Set(ByVal value As String)
            _ColumnTitle = value
            Me.ColumnLabel.Text = value
        End Set
    End Property

    Private _ValidRegex As String
    Public Property ColumnRegex() As String
        Get
            Return _ValidRegex
        End Get
        Set(ByVal value As String)
            _ValidRegex = value
        End Set
    End Property

    Private _ColumnReadOnly As Boolean
    Public Property ColumnReadOnly() As Boolean
        Get
            Return _ColumnReadOnly
        End Get
        Set(ByVal value As Boolean)
            _ColumnReadOnly = value
        End Set
    End Property

    Private _ColumnEnabled As Boolean
    Public Property ColumnEnabled() As Boolean
        Get
            Return _ColumnEnabled
        End Get
        Set(ByVal value As Boolean)
            _ColumnEnabled = value
            Me.ColumnLabel.Enabled = value
        End Set
    End Property

    Private _ColumnVisible As Boolean
    Public Property ColumnVisible() As Boolean
        Get
            Return _ColumnVisible
        End Get
        Set(ByVal value As Boolean)
            _ColumnVisible = value
        End Set
    End Property

    Private _ColumnWidth As Integer
    Public Property ColumnWidth() As Integer
        Get
            Return _ColumnWidth
        End Get
        Set(ByVal value As Integer)
            _ColumnWidth = value
        End Set
    End Property

    Private _ColumnAlignment As Windows.Forms.HorizontalAlignment
    Public Property ColumnAlignment() As Windows.Forms.HorizontalAlignment
        Get
            Return _ColumnAlignment
        End Get
        Set(ByVal value As Windows.Forms.HorizontalAlignment)
            _ColumnAlignment = value
        End Set
    End Property

#End Region

    Public Sub evtKeyPress(ByVal e As System.Windows.Forms.KeyPressEventArgs)
        RaiseEvent onKey()
    End Sub

End Class
