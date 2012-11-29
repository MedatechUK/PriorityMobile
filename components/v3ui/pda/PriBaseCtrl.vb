Imports bind
Imports System.Xml

Public Class PriBaseCtrl

    Private g As System.Drawing.Graphics = Me.CreateGraphics
    Private labelwidth As Integer = 0
    Public myCT As ctform

#Region "Overridable Properties"

    Overridable Property DerivedControl() As Control
        Get
            Return Nothing
        End Get
        Set(ByVal value As Control)

        End Set
    End Property

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

#End Region

#Region "Public Properties"

    Private _xmlMe As XmlNode
    Public Property xmlMe() As XmlNode
        Get
            Return _xmlMe
        End Get
        Set(ByVal value As XmlNode)
            _xmlMe = value
        End Set
    End Property

    Public Property IsReadOnly() As Boolean
        Get
            If Not IsNothing(DerivedControl) Then
                Return Not (DerivedControl.Enabled)
            Else
                Return False
            End If
        End Get
        Set(ByVal value As Boolean)
            If Not IsNothing(DerivedControl) Then
                DerivedControl.Enabled = Not (value)
            End If
        End Set
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
    Public Overrides Property Font() As Font
        Get
            Return _Font
        End Get
        Set(ByVal value As Font)
            _Font = value
            With Me
                .Label.Font = _Font
                .DerivedControl.Font = _Font
                labelwidth = GetLabelWidth()
                .Label.Width = labelwidth
            End With
        End Set
    End Property

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

#End Region

#Region "Resize code"

    Private Function GetLabelWidth() As Integer
        Return g.MeasureString(Label.Text, Font).Width + 5
    End Function

    Private Sub PriBaseCtrl_ParentChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.ParentChanged
        If Not IsNothing(Parent.Parent.Parent) Then
            Me.Font = Parent.Parent.Parent.Font
            PriBaseCtrl_Resize(Me, New System.EventArgs)
        End If
    End Sub

    Private Sub PriBaseCtrl_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
        Dim parent As PriBaseCtrl = sender
        With Label
            .Width = labelwidth
            .Left = 2
            .Top = 2
            .Height = parent.Height
        End With
        If Not IsNothing(DerivedControl) And Not IsNothing(parent.Parent) Then
            With DerivedControl
                .Top = 2
                .Left = Label.Left + Label.Width + 5
                .Width = parent.Width - (Label.Left + Label.Width)
                .Height = 23
            End With
        End If
    End Sub

#End Region

#Region "data"

    Public ReadOnly Property DG() As DataGrid
        Get
            Return myCT.DataGrid '.Controls("DataGrid")
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

#End Region

#Region "Handlers"
    Public Sub hLostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        If Me.regex.Length > 0 And CStr(Value()).Length > 0 Then
            If Not System.Text.RegularExpressions.Regex.IsMatch(Value, Me.regex) Then
                'MsgBox(String.Format("Invalid entry for {0}: '{1}'", LabelText, Value))
                Beep()
                DerivedControl.Focus()
            Else
                PriBaseCtrl_Resize(Me, New System.EventArgs)
            End If
        Else
            PriBaseCtrl_Resize(Me, New System.EventArgs)
        End If
    End Sub

    Public Sub hGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        With DerivedControl
            .Left = 2
            .Width = .Parent.Width '- 20
        End With
    End Sub
#End Region

#Region "public Subs"

    Public Sub SetProps(ByRef f As XmlNode)
        xmlMe = f
        With xmlMe
            'FieldStyle = .FieldStyle
            Visible = Not (CBool(f.Attributes("hidden").Value))
            LabelText = f.Attributes("name").Value
            Mandatory = CBool(f.Attributes("mandatory").Value)
            IsReadOnly = CBool(f.Attributes("readonly").Value)
            regex = f.Attributes("regex").Value
            Column = .InnerText
        End With
    End Sub

    Public Sub AddHandlers()
        AddHandler DerivedControl.LostFocus, AddressOf hLostFocus
        AddHandler DerivedControl.GotFocus, AddressOf hGotFocus
    End Sub

#End Region

End Class
