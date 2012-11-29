Public Class PriBaseCtrl

    Overridable Sub NewFont(ByVal FormFont As Font)

    End Sub
    Overridable Sub SetReadOnly()

    End Sub
    Overridable Property Value() As String
        Get
            Return Nothing
        End Get
        Set(ByVal value As String)

        End Set
    End Property
    Overridable Property ControlText() As String
        Get
            Return Nothing
        End Get
        Set(ByVal value As String)

        End Set
    End Property
    Overridable Sub hResize(ByVal sender As Object, ByVal e As System.EventArgs)

    End Sub

    Public ReadOnly Property DG() As DataGrid
        Get
            Return Parent.Parent.Parent.Parent.Controls("DataGrid")
        End Get
    End Property

    Public ReadOnly Property ColNo() As Integer
        Get
            With DG
                For i As Integer = 0 To .TableStyles(0).GridColumnStyles.Count - 1
                    If String.Compare(.TableStyles.Item(0).GridColumnStyles(i).MappingName, Me.Column) = 0 Then
                        Return i
                    End If
                Next
            End With
        End Get
    End Property

    Private _Active As Boolean = False
    Public Property Active() As Boolean
        Get
            Return _Active
        End Get
        Set(ByVal value As Boolean)
            _Active = value
        End Set
    End Property
    Private _DisplayOrder As Integer
    Public Property DisplayOrder() As Integer
        Get
            Return _DisplayOrder
        End Get
        Set(ByVal value As Integer)
            _DisplayOrder = value
        End Set
    End Property

    Private _Font As New Font("Tahoma", 10, FontStyle.Regular)
    Public Property FormFont() As Font
        Get
            Return _Font
        End Get
        Set(ByVal value As Font)
            _Font = value
            With Me
                .Label.Font = _Font
                NewFont(value)
            End With
        End Set
    End Property

    Private _LabelWidth As Integer
    Public ReadOnly Property LabelWidth() As Integer
        Get
            Return _LabelWidth + 5
        End Get
    End Property

    Private _LabelText As String = ""
    Public Property LabelText() As String
        Get
            Return Label.Text
        End Get
        Set(ByVal value As String)
            Label.Text = value
        End Set
    End Property

    Private _FieldStyle As xfFieldStyle = xfFieldStyle.xfText
    Public Property FieldStyle() As xfFieldStyle
        Get
            Return _FieldStyle
        End Get
        Set(ByVal value As xfFieldStyle)
            _FieldStyle = value
        End Set
    End Property

    Private _Mandatory As Boolean = False
    Public Property Mandatory() As Boolean
        Get
            Return _Mandatory
        End Get
        Set(ByVal value As Boolean)
            _Mandatory = value
            If _Mandatory Then
                Label.ForeColor = Color.Red
            Else
                Label.ForeColor = Color.Black
            End If
        End Set
    End Property

    Private _ReadOnly As Boolean = False
    Public Property IsReadOnly() As Boolean
        Get
            Return _ReadOnly
        End Get
        Set(ByVal value As Boolean)
            _ReadOnly = value
            SetReadOnly()
        End Set
    End Property

    Private _Hidden As Boolean = False
    Public Property Hidden() As Boolean
        Get
            Return _Hidden
        End Get
        Set(ByVal value As Boolean)
            _Hidden = value
        End Set
    End Property

    Private _ListSource As String = ""
    Public Property ListSource() As String
        Get
            Return _ListSource
        End Get
        Set(ByVal value As String)
            _ListSource = value
        End Set
    End Property

    Private _ListValueCol As String = ""
    Public Property ListValueCol() As String
        Get
            Return _ListValueCol
        End Get
        Set(ByVal value As String)
            _ListValueCol = value
        End Set
    End Property

    Private _ListTextCol As String
    Public Property ListTextCol() As String
        Get
            Return _ListTextCol
        End Get
        Set(ByVal value As String)
            _ListTextCol = value
        End Set
    End Property

    Private _ListFilter As String
    Public Property ListFilter() As String
        Get
            Return _ListFilter
        End Get
        Set(ByVal value As String)
            _ListFilter = value
        End Set
    End Property

    Private _Column As String = ""
    Public Property Column() As String
        Get
            Return _Column
        End Get
        Set(ByVal value As String)
            _Column = value
        End Set
    End Property

    Private _regex As String = ""
    Public Property regex() As String
        Get
            Return _regex
        End Get
        Set(ByVal value As String)
            _regex = value
        End Set
    End Property

    Private Sub PriBaseCtrl_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Me.Paint
        Dim StringSize As New SizeF
        StringSize = e.Graphics.MeasureString(LabelText, FormFont)
        If StringSize.Width > Me.Width / 3 Then
            _LabelWidth = Me.Width / 3
        Else
            _LabelWidth = StringSize.Width
        End If
        Label.Width = LabelWidth

    End Sub

    Private Sub PriBaseCtrl_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
        With Label
            .Width = LabelWidth
            .Left = 1
            .Top = 1
            .Height = Me.Height - 2
        End With

    End Sub

End Class
